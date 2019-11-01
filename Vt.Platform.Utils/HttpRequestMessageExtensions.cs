using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using Vt.Platform.Utils.Json;
using Newtonsoft.Json.Serialization;

namespace Vt.Platform
{
    public static class HttpRequestMessageExtensions
    {
        static readonly JsonSerializerSettings Settings;

        static HttpRequestMessageExtensions()
        {
            Settings = new JsonSerializerSettings();
            Settings.Converters.Add(new EnumConverter());
            Settings.Formatting = Formatting.Indented;
            Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpRequestMessage msg) where T : new()
        {
            var body = await msg.Content.ReadAsStringAsync();
            var headers = msg.Headers.Select(x => new
            {
                x.Key, 
                Value = x.Value.FirstOrDefault()
            }).ToDictionary(x => x.Key, y => y.Value);

            var joBody = string.IsNullOrWhiteSpace(body) ? new JObject() : JObject.Parse(body);
            var joQuery = ParseQueryString(msg.RequestUri);
            var joHeaders = JObject.FromObject(headers);

            joHeaders.Merge(joQuery, new JsonMergeSettings
            {
                MergeNullValueHandling = MergeNullValueHandling.Ignore,
                MergeArrayHandling = MergeArrayHandling.Union
            });

            joHeaders.Merge(joBody, new JsonMergeSettings
            {
                MergeNullValueHandling = MergeNullValueHandling.Ignore,
                MergeArrayHandling = MergeArrayHandling.Union
            });

            var json = joHeaders.ToString();

            if (string.IsNullOrWhiteSpace(json))
            {
                return new T();
            }

            var obj = JsonConvert.DeserializeObject<T>(json, Settings);
            return obj;
        }

        private static JObject ParseQueryString(Uri url)
        {
            if (string.IsNullOrWhiteSpace(url.Query))
            {
                return new JObject();
            }

            NameValueCollection query = HttpUtility.ParseQueryString(url.Query);
            var formDictionary = query.AllKeys
                .Where(p => query[p] != "null")
                .ToDictionary(p => p, p => query[p]);
            var json = JsonConvert.SerializeObject(formDictionary);
            return JObject.Parse(json);
        }
    }
}
