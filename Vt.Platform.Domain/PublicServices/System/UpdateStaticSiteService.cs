using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Vt.Platform.Domain.Models.Api;
using Vt.Platform.Domain.Services;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.PublicServices.System
{
    public class UpdateStaticSiteService : BaseService<UpdateStaticSiteService.Request, UpdateStaticSiteService.Response>
    {
        private readonly IStaticSiteStorageService _staticSiteStorageService;

        public UpdateStaticSiteService(ILogger logger, 
            IStaticSiteStorageService staticSiteStorageService) : base(logger)
        {
            _staticSiteStorageService = staticSiteStorageService;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            var siteUrl = Environment.GetEnvironmentVariable("Volteer.Website");
            var contentMap = await HttpClient.GetStringAsync(siteUrl + "/api/routes");
            var map = JsonConvert.DeserializeObject<StaticSiteApiModel>(contentMap);

            foreach (var content in map.Content)
            {
                var res = await HttpClient.GetAsync(siteUrl + content);
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsByteArrayAsync();
                    await _staticSiteStorageService.StoreContent(content, res.Content.Headers.ContentType.MediaType,
                        data);
                }
            }

            foreach (var page in map.Pages)
            {
                var res = await HttpClient.GetAsync(siteUrl + page);
                var data = await res.Content.ReadAsByteArrayAsync();
                await _staticSiteStorageService.StoreContent(page, res.Content.Headers.ContentType.MediaType, data);

            }

            return new Response();
        }

        public override IDictionary<int, string> GetErrorCodes()
        {
            return new Dictionary<int, string>();
        }

        public class Request : BaseRequest
        {

        }

        public class Response : BaseResponse
        {

        }
    }
}
