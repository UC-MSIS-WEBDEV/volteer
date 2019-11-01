using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vt.Platform.Domain.Messaging;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.PublicServices.System
{
    public class ConfigQueuesService : BaseService<ConfigQueuesService.Request, ConfigQueuesService.Response>
    {
        private readonly IQueueManager _queueManager;

        public ConfigQueuesService(ILogger logger, IQueueManager queueManager) : base(logger)
        {
            _queueManager = queueManager;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            await _queueManager.BuildQueues();
            return new Response();
        }

        public override IDictionary<int, string> GetErrorCodes()
        {
            return new Dictionary<int, string>();
        }

        public class Request : BaseRequest
        {

        }

        public class Response : BaseResponse
        {

        }
    }
}
