
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vt.Platform.Domain.Repositories;
using Vt.Platform.Domain.Services;
using System.ComponentModel.DataAnnotations;
using Vt.Platform.Utils;
using Vt.Platform.Domain.Models.Persistence;

namespace Vt.Platform.Domain.PublicServices.Events
{
    public class CreateEventService : BaseService<CreateEventService.Request, CreateEventService.Response>
    {
        private IDataRepository _dataRepository;
        private IRandomGenerator _randomGenerator;
        public CreateEventService(
          IDataRepository dataRepository,
          IRandomGenerator randomGenerator,
          ILogger logger) : base(logger)
        {
            // CONSTRUCTOR
            _dataRepository = dataRepository;
            _randomGenerator = randomGenerator;
        }

        protected override async Task<Response> Implementation(Request request)
        {

            string eventCode = await _randomGenerator.GetEventCode();
            string organizerCode = await _randomGenerator.GetSigninToken();
            string confirmationCode = await _randomGenerator.GetSigninToken();


            // CREATE DTO
            var dto = new EventDto
            {
                EventCode = eventCode,
                OrganizerCode = organizerCode,
                ConfirmationCode = confirmationCode,
                EventDate = request.EventDate,
                EventDetails = request.EventDetails,
                EventLocation = request.EventLocation,
                EventSummary = request.EventSummary,
                NumberOfParticipants = request.NumberOfParticipants.Value,
                OrganizerEmail = request.OrganizerEmail,
                OrganizerName = request.OrganizerName,
                OrganizerValidated = false
            };

            // SAVE IN REPOSITORY
            await _dataRepository.SaveOrUpdateEvent(dto);


            var response = new Response();
            // ASSIGN VALUES TO THE RESPONSE OBJECT
            response.EventCode = eventCode;

            return response;
            // THIS IS WHERE OUR SERVICE LOGIC WILL EXIST
            //throw new NotImplementedException();
        }

        public override IDictionary<int, string> GetErrorCodes()
        {
            // WE CAN DEFINE ANY CUSTOM ERROR CODES HERE
            return new Dictionary<int, string>
            {

            };
        }


        public class Request : BaseRequest
        {
            [Required]
            public string OrganizerName { get; set; }
            [Required]
            [EmailAddress]
            public string OrganizerEmail { get; set; }
            public DateTime EventDate { get; set; }
            [Required]
            public string EventSummary { get; set; }
            public string EventDetails { get; set; }
            public string EventLocation { get; set; }
            [Required]
            public int? NumberOfParticipants { get; set; }
        }

        public class Response : BaseResponse
        {
            public string EventCode { get; set; }
        }
    }
}
