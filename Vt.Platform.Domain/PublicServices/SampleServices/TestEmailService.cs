using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vt.Platform.Domain.Enums;
using Vt.Platform.Domain.Messaging;
using Vt.Platform.Domain.Models.Messaging;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.PublicServices.SampleServices
{
    public class TestEmailService : BaseService<TestEmailService.Request, TestEmailService.Response>
    {
        private readonly IMessagePublisher _messagePublisher;
        public TestEmailService(ILogger logger, IMessagePublisher messagePublisher) : base(logger)
        {
            _messagePublisher = messagePublisher;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            var msg = new TestMessage
            {
                Message = request.Message
            };

            await _messagePublisher.Publish(MessageTopic.TestMessage, msg, request.CorrelationId);
            return new Response();
        }

        public override IDictionary<int, string> GetErrorCodes()
        {
            return new Dictionary<int, string>();
        }

        public class Request : BaseRequest
        {
            public string Message { get; set; }
        }

        public class Response : BaseResponse
        {

        }
    }
}
