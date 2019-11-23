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
        public GetEventService(IDataRepository dataRepository, ILogger logger) : base(logger)
        {
            _dataRepository = dataRepository;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            await Task.CompletedTask;
            string EventCode = request.EventCode;
            //Retrieve from repository
            EventDto dto = await _dataRepository.GetEventAsync(EventCode);
            var response = new Response();
            response.EventCode = dto.EventCode;
            response.OrganizerName = dto.OrganizerName;
            response.EventDate = dto.EventDate;
            response.Summary = dto.EventSummary;
            response.Details = dto.EventDetails;
            response.NumberOfParticipantsRequested = dto.NumberOfParticipants.Value;
            response.Address1 = dto.EventLocation;
            response.Address2 = dto.EventLocation;
            response.City = dto.EventLocation;
            response.State = dto.EventLocation;
            response.PostalCode = dto.EventLocation;


            return response;
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
