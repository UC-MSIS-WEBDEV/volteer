
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
    public class CreateEventFunction
    {
        private readonly ILogger<GetEventFunction> _logger;
        private readonly IRandomGenerator _randomGenerator;
        private readonly IDataRepository _dataRepository;
        private readonly IObjectTokenizer _objectTokenizer;
        private readonly IEmailService _emailService;

        public CreateEventFunction(
            ILogger<GetEventFunction> logger,
            IDataRepository dataRepository,
            IRandomGenerator randomGenerator,
            IObjectTokenizer objectTokenizer,
            IEmailService emailService)
        {
            _logger = logger;
            _randomGenerator = randomGenerator;
            _dataRepository = dataRepository;
            _objectTokenizer = objectTokenizer;
            _emailService = emailService;
        }

        [FunctionName("CreateEvent")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req)
        {
            var response = await req.CallService(
                () => new CreateEventService(_dataRepository, _randomGenerator, _logger, _emailService),
                _objectTokenizer,
                HttpMethod.Get,
                HttpMethod.Post);
            return response;
        }
    }
}