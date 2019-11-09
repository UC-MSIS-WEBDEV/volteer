using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vt.Platform.Domain.PublicServices.System;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.PublicServices.Events
{
    public class GetEventService : BaseService<GetEventService.Request, GetEventService.Response>
    {
        public GetEventService(ILogger logger) : base(logger)
        {
        }

        protected override async Task<Response> Implementation(Request request)
        {
            await Task.CompletedTask;

            return new Response
            {
                EventCode = request.EventCode,
                OrganizerName = "John Smith",
                Summary = "Bearcats Homecoming Parade",
                Details = "Come join the bear cats parade",
                NumberOfParticipantsRequested = 100,
                EventDate = DateTime.Now
            };
        }

        public override IDictionary<int, string> GetErrorCodes()
        {
            return new Dictionary<int, string>();
        }


        public class Request : BaseRequest
        {
            public string EventCode { get; set; }
        }

        public class Response : BaseResponse
        {
            public string EventCode { get; set; }
            public string OrganizerName { get; set; }
            public DateTime EventDate { get; set; }
            public string Summary { get; set; }
            public string Details { get; set; }
            public int NumberOfParticipantsRequested { get; set; }
        }

    }
}
