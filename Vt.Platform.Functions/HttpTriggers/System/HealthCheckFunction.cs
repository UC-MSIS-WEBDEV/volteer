using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Tokenize.Client;
using Vt.Platform.Domain.PublicServices;
using Vt.Platform.Domain.PublicServices.System;
using Vt.Platform.Domain.Services;
using Vt.Platform.Utils;

namespace Vt.Platform.Functions.HttpTriggers
{
    public class HealthCheckFunction
    {
        private readonly ILogger<HealthCheckFunction> _logger;
        private readonly IObjectTokenizer _objectTokenizer;

        public HealthCheckFunction(
            ILogger<HealthCheckFunction> logger, 
            IObjectTokenizer objectTokenizer)
        {
            _logger = logger;
            _objectTokenizer = objectTokenizer;
        }

        [FunctionName("HealthCheck")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req)
        {
            var response = await req.CallService(
                () => new HealthCheckService(_logger), 
                _objectTokenizer, 
                HttpMethod.Get,
                HttpMethod.Post);
            return response;
        }
    }
}
