using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tokenize.Client;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.PublicServices.System
{
    public class HealthCheckService : BaseService<HealthCheckService.Request, HealthCheckService.Response>
    {
        public HealthCheckService(ILogger logger) : base(logger)
        {
        }

        protected override async Task<Response> Implementation(Request request)
        {
            await Task.CompletedTask;
            Logger.LogInformation("Heath check {request}", request);

            if (request.ErrorCode != null)
            {
                return ErrorCodeResponse(request.ErrorCode.Value);
            }

            return new Response
            {
                Message = request.Message,
                SensitiveMessage = request.SensitiveMessage
            };
        }

        public override IDictionary<int, string> GetErrorCodes()
        {
            return new Dictionary<int, string> {
                [100] = "Health check Error Code 100",
                [101] = "Health check Error Code 101",
                [102] = "Health check Error Code 102",
            };
        }

        public class Request : BaseRequest
        {
            public string Message { get; set; }
            public int? ErrorCode { get; set; }
            public TokenString SensitiveMessage { get; set; }
        }

        public class Response : BaseResponse
        {
            public string Message { get; set; }
            public TokenString SensitiveMessage { get; set; }

        }
    }
}
