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

namespace Vt.Platform.Domain.PublicServices.Participants
{
    public class EditParticipantService : BaseService<EditParticipantService.Request, EditParticipantService.Response>
    {
        private IDataRepository _dataRepository;

        public EditParticipantService(
          IDataRepository dataRepository,
          ILogger logger) : base(logger)
        {
            // CONSTRUCTOR
            _dataRepository = dataRepository;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            ParticipantDto participant = await _dataRepository.GetParticipantAsync(request.EventCode, request.ParticipantCode);
            if (participant == null) {
                throw new Exception("Participant Not Found");
            }

            participant.ParticipantStatus = request.ParticipantStatus;

            // UPDATE IN REPOSITORY
            if (request.ConfirmationCode.Equals(participant.ConfirmationCode))
            {
                await _dataRepository.UpdateParticipantStatusAsync(participant);
            }
            else {
                throw new Exception("Confirmation code Invalid");
            }

            var response = new Response 
            { 
                EventCode = request.EventCode,
                ParticipantCode = request.ParticipantCode,
                ParticipantStatus = request.ParticipantStatus
            };
            return response;
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
            // REQUEST DATA MODEL GOES HERE
            [Required]
            public string EventCode { get; set; }
            [Required]
            public string ParticipantCode { get; set; }
            [Required]
            public string ConfirmationCode { get; set; }
            [Required]
            public string ParticipantStatus { get; set; }
        }

        public class Response : BaseResponse
        {
            // RESPONSE DATA MODEL GOES HERE
            [Required]
            public string EventCode { get; set; }
            public string ParticipantCode { get; set; }
            public string ParticipantName { get; set; }
            public string ParticipantEmail { get; set; }
            public string ParticipantStatus { get; set; }
        }
    }
}
