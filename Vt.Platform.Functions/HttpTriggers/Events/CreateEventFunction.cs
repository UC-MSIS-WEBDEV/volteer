
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
        private readonly IEmailService _emailService;
        private readonly IObjectTokenizer _objectTokenizer;

        public CreateEventFunction(
            ILogger<GetEventFunction> logger,
            IDataRepository dataRepository,
            IRandomGenerator randomGenerator,
            IEmailService emailService,
            IObjectTokenizer objectTokenizer)
        {
            _logger = logger;
            _randomGenerator = randomGenerator;
            _dataRepository = dataRepository;
            _emailService = emailService;
            _objectTokenizer = objectTokenizer;
        }

        [FunctionName("CreateEvent")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req)
        {
            var response = await req.CallService(
                () => new CreateEventService(_dataRepository, _randomGenerator, _emailService, _logger),
                _objectTokenizer,
                HttpMethod.Get,
                HttpMethod.Post);
            return response;
        }
    }
}