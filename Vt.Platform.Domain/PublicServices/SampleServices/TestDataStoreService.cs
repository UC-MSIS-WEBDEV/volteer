using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tokenize.Client;
using Vt.Platform.Domain.Models.Persistence;
using Vt.Platform.Domain.Repositories;
using Vt.Platform.Domain.Services;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.PublicServices.SampleServices
{
    public class TestDataStoreService : BaseService<TestDataStoreService.Request, TestDataStoreService.Response>
    {
        private readonly ITestContactRepository _testContactRepository;
        private readonly IRandomGenerator _randomGenerator;

        public TestDataStoreService(ILogger logger, 
            ITestContactRepository testContactRepository,
            IRandomGenerator randomGenerator) : base(logger)
        {
            _testContactRepository = testContactRepository;
            _randomGenerator = randomGenerator;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            var contactCode = await _randomGenerator.GetEventCode();

            var dto = new TestContactDto
            {
                Name = request.Name,
                EmailToken = request.Email.Token,
                EmailMask = request.Email.MaskedValue,
                Code = contactCode
            };

            await _testContactRepository.PersistContact(dto);
            return new Response
            {
                Code = contactCode
            };
        }

        public override IDictionary<int, string> GetErrorCodes()
        {
            return new Dictionary<int, string>();
        }

        public class Request : BaseRequest
        {
            public string Name { get; set; }
            public TokenString Email { get; set; }
        }

        public class Response : BaseResponse
        {
            public string Code { get; set; }
        }
    }
}
