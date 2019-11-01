using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Tokenize.Client;
using Vt.Platform.Domain.PublicServices.System;
using Vt.Platform.Domain.Services;
using Vt.Platform.Utils;

namespace Vt.Platform.Functions.HttpTriggers.System
{
    public class UpdateStaticSiteFunction
    {
        private readonly ILogger<UpdateStaticSiteFunction> _logger;
        private readonly IObjectTokenizer _objectTokenizer;
        private readonly IStaticSiteStorageService _staticSiteStorageService;

        public UpdateStaticSiteFunction(
            ILogger<UpdateStaticSiteFunction> logger,
            IObjectTokenizer objectTokenizer,
            IStaticSiteStorageService staticSiteStorageService)
        {
            _logger = logger;
            _objectTokenizer = objectTokenizer;
            _staticSiteStorageService = staticSiteStorageService;
        }

        [FunctionName("UpdateStaticSite")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req)
        {
            var response = await req.CallService(
                () => new UpdateStaticSiteService(_logger, _staticSiteStorageService), 
                _objectTokenizer,
                HttpMethod.Get,
                HttpMethod.Post);
            return response;
        }
    }
}