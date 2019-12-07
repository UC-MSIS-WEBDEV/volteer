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
using Vt.Platform.Domain.Services;
using Vt.Platform.Utils;

namespace Vt.Platform.Functions.HttpTriggers.Events
{
    public class GetMyEventsFunction
    {
        private readonly ILogger<GetMyEventsFunction> _logger;
        private readonly IDataRepository _dataRepository;
        private readonly IEmailService _emailService;
        private readonly IObjectTokenizer _objectTokenizer;

        public GetMyEventsFunction(
            ILogger<GetMyEventsFunction> logger,
            IDataRepository dataRepository,
            IEmailService emailService,
            IObjectTokenizer objectTokenizer)
        {
            _logger = logger;
            _dataRepository = dataRepository;
            _emailService = emailService;
            _objectTokenizer = objectTokenizer;
        }

        [FunctionName("GetMyEvents")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req)
        {
            var response = await req.CallService(
                () => new GetMyEventsService(_dataRepository, _emailService, _logger),
                _objectTokenizer,
                HttpMethod.Get,
                HttpMethod.Post);
            return response;
        }
    }
}
