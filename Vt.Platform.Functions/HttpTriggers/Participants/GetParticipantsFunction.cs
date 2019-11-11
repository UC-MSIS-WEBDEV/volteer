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
using Vt.Platform.Domain.PublicServices.Participants;
using Vt.Platform.Functions.HttpTriggers.Events;
using Vt.Platform.Utils;

namespace Vt.Platform.Functions.HttpTriggers.Participants
{
    public class GetParticipantsFunction
    {
        private readonly ILogger<GetEventFunction> _logger;
        private readonly IObjectTokenizer _objectTokenizer;

        public GetParticipantsFunction(
            ILogger<GetEventFunction> logger,
            IObjectTokenizer objectTokenizer)
        {
            _logger = logger;
            _objectTokenizer = objectTokenizer;
        }

        [FunctionName("GetParticipants")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req)
        {
            var response = await req.CallService(
                () => new GetParticipantsService(_logger),
                _objectTokenizer,
                HttpMethod.Get,
                HttpMethod.Post);
            return response;
        }
    }
}
