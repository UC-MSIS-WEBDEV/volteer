using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Tokenize.Client;
using Vt.Platform.Domain.PublicServices.SampleServices;
using Vt.Platform.Domain.Repositories;
using Vt.Platform.Domain.Services;
using Vt.Platform.Utils;

namespace Vt.Platform.Functions.HttpTriggers.Samples
{
    public class TestDataStoreFunction
    {
        private readonly ILogger<TestDataStoreFunction> _logger;
        private readonly IObjectTokenizer _objectTokenizer;
        private readonly ITestContactRepository _testContactRepository;
        private readonly IRandomGenerator _randomGenerator;

        public TestDataStoreFunction(
            ILogger<TestDataStoreFunction> logger,
            IObjectTokenizer objectTokenizer,
            ITestContactRepository testContactRepository,
            IRandomGenerator randomGenerator)
        {
            _logger = logger;
            _objectTokenizer = objectTokenizer;
            _testContactRepository = testContactRepository;
            _randomGenerator = randomGenerator;
        }

        [FunctionName("TestDataStore")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req)
        {
            var response = await req.CallService(
                () => new TestDataStoreService(_logger, _testContactRepository, _randomGenerator), 
                _objectTokenizer,
                HttpMethod.Get,
                HttpMethod.Post);
            return response;
        }
    }
}