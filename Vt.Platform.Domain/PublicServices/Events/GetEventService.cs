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
    public class GetEventService : BaseService<GetEventService.Request, GetEventService.Response>
    {

        private IDataRepository _dataRepository;

        public GetEventService(
            IDataRepository dataRepository, 
            ILogger logger) : base(logger)
        {
            _dataRepository = dataRepository;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            var dto = new EventDto();
            dto = await _dataRepository.GetEventAsync(request.EventCode);



            //await Task.CompletedTask;

            return new Response
            {
                EventCode = request.EventCode,
                OrganizerName = dto.OrganizerName,
                Summary = dto.EventSummary,
                Details = dto.EventDetails, 
                NumberOfParticipantsRequested = dto.NumberOfParticipants,
                EventDate = dto.EventDate,
                Latitude = 39.1337922,
                Longitude = -84.5145203,
                Address1 = "2906 Woodside Drive",
                Address2 = "",
                City = "Cincinnati",
                PostalCode = "45221",
                State = "OH"
            };
        }

        public override IDictionary<int, string> GetErrorCodes()
        {
            return new Dictionary<int, string>();
        }


        public class Request : BaseRequest
        {
            [Required]
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
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
        }

    }
}
