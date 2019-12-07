using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Tokenize.Client;
using Vt.Platform.Domain.PublicServices.Participants;
using Vt.Platform.Domain.Repositories;
using Vt.Platform.Domain.Services;
using Vt.Platform.Utils;

namespace Vt.Platform.Functions.HttpTriggers.Participants
{
    public class ConfirmParticipantFunction
    {
        private readonly ILogger<ConfirmParticipantFunction> _logger;
        private readonly IDataRepository _dataRepository;
        private readonly IObjectTokenizer _objectTokenizer;

        public ConfirmParticipantFunction(
            ILogger<ConfirmParticipantFunction> logger,
            IDataRepository dataRepository,
            IObjectTokenizer objectTokenizer)
        {
            _logger = logger;
            _dataRepository = dataRepository;
            _objectTokenizer = objectTokenizer;
        }

        [FunctionName("ConfirmParticipant")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req)
        {
            var response = await req.CallService(
                () => new ConfirmParticipantService(_dataRepository, _logger),
                _objectTokenizer,
                HttpMethod.Get,
                HttpMethod.Post);
            return response;
        }
    }
}