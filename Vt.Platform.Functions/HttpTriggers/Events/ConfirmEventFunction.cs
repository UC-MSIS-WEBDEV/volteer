using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Tokenize.Client;
using Vt.Platform.Domain.PublicServices.Events;
using Vt.Platform.Domain.PublicServices.System;
using Vt.Platform.Domain.Repositories;
using Vt.Platform.Utils;

namespace Vt.Platform.Functions.HttpTriggers.Events
{
    public class ConfirmEventFunction
    {

        private readonly ILogger<GetEventFunction> _logger;
        private readonly IObjectTokenizer _objectTokenizer;
        private readonly IDataRepository _dataRepository;

        public ConfirmEventFunction(
            IDataRepository dataRepository,
            ILogger<GetEventFunction> logger,
            IObjectTokenizer objectTokenizer)
        {
            _dataRepository = dataRepository;
            _logger = logger;
            _objectTokenizer = objectTokenizer;
        }
        [FunctionName("ConfirmEvent")]
        public async Task<HttpResponseMessage> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req)
        {
            var response = await req.CallService(
                () => new ConfirmEventService(_dataRepository, _logger),
                _objectTokenizer,
                HttpMethod.Get,
                HttpMethod.Post);
            return response;
        }
    }
}
