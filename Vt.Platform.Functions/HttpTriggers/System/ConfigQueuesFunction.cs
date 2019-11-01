using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Tokenize.Client;
using Vt.Platform.Domain.Messaging;
using Vt.Platform.Domain.PublicServices.System;
using Vt.Platform.Utils;

namespace Vt.Platform.Functions.HttpTriggers.System
{
    public class ConfigQueuesFunction
    {
        private readonly ILogger<HealthCheckFunction> _logger;
        private readonly IObjectTokenizer _objectTokenizer;
        private readonly IQueueManager _queueManager;

        public ConfigQueuesFunction(
            ILogger<HealthCheckFunction> logger,
            IObjectTokenizer objectTokenizer,
            IQueueManager queueManager)
        {
            _logger = logger;
            _objectTokenizer = objectTokenizer;
            _queueManager = queueManager;
        }

        [FunctionName("ConfigQueues")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req)
        {
            var response = await req.CallService(
                () => new ConfigQueuesService(_logger, _queueManager), 
                _objectTokenizer);
            return response;
        }
    }
}