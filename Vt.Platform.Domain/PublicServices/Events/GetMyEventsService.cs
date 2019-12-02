using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vt.Platform.Domain.Models.Persistence;
using Vt.Platform.Domain.PublicServices.System;
using Vt.Platform.Domain.Repositories;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.PublicServices.Events
{
    public class GetMyEventsService : BaseService<GetMyEventsService.Request, GetMyEventsService.Response>
    {

        private IDataRepository _dataRepository;

        public GetMyEventsService(
            IDataRepository dataRepository,
            ILogger logger) : base(logger)
        {
            _dataRepository = dataRepository;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            string emailbody = await _dataRepository.GetMyEventsAsync(request.Mail);
            var response = new Response();
            response.emailBody = emailbody;
            return response;
        }

        public override IDictionary<int, string> GetErrorCodes()
        {
            return new Dictionary<int, string>();
        }


        public class Request : BaseRequest
        {
            [Required]
            public string Mail { get; set; }
        }

        public class Response : BaseResponse
        {
            public string emailBody { get; set; }
        }

    }
}
