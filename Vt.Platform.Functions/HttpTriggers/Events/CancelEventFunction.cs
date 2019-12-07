using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Tokenize.Client;
using Vt.Platform.Domain.PublicServices.Events;
using Vt.Platform.Domain.Repositories;
using Vt.Platform.Domain.Services;
using Vt.Platform.Utils;

namespace Vt.Platform.Functions.HttpTriggers.Events
{
    public class CancelEventFunction
    {
        private readonly ILogger<CancelEventFunction> _logger;
        private readonly IDataRepository _dataRepository;
        private readonly IObjectTokenizer _objectTokenizer;

        public CancelEventFunction(
            ILogger<CancelEventFunction> logger,
            IDataRepository dataRepository,
            IObjectTokenizer objectTokenizer)
        {
            _logger = logger;
            _dataRepository = dataRepository;
            _objectTokenizer = objectTokenizer;
        }

        [FunctionName("CancelEvent")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req)
        {
            var response = await req.CallService(
                () => new CancelEventService(_dataRepository, _logger),
                _objectTokenizer,
                HttpMethod.Get,
                HttpMethod.Post);
            return response;
        }
    }
}