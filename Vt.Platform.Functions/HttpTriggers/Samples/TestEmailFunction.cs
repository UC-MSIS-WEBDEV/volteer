using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Tokenize.Client;
using Vt.Platform.Domain.Messaging;
using Vt.Platform.Domain.PublicServices.SampleServices;
using Vt.Platform.Utils;

namespace Vt.Platform.Functions.HttpTriggers.Samples
{
    public class TestEmailFunction
    {
        private readonly ILogger<TestEmailFunction> _logger;
        private readonly IObjectTokenizer _objectTokenizer;
        private readonly IMessagePublisher _messagePublisher;

        public TestEmailFunction(
            ILogger<TestEmailFunction> logger,
            IObjectTokenizer objectTokenizer,
            IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _objectTokenizer = objectTokenizer;
            _messagePublisher = messagePublisher;
        }

        [FunctionName("TestEmail")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req)
        {
            var response = await req.CallService(
                () => new TestEmailService(_logger, _messagePublisher), 
                _objectTokenizer,
                HttpMethod.Get,
                HttpMethod.Post);
            return response;
        }
    }
}