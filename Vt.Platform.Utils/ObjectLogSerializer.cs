using System;
using System.Collections.Generic;
using System.Text;
using Vt.Platform.Utils.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Vt.Platform.Utils
{
    public static class ObjectLogSerializer
    {
        private static readonly JsonSerializerSettings Settings;

        static ObjectLogSerializer()
        {
            Settings = new JsonSerializerSettings();
            Settings.Converters.Add(new EnumConverter());
            Settings.Formatting = Formatting.Indented;
            Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public static string Serialize<T>(T obj)
        {
            var str = JsonConvert.SerializeObject(obj, Settings);
            return str;
        }
    }
}
