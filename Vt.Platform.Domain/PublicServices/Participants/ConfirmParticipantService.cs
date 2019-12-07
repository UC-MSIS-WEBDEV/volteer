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
    public class ConfirmParticipantService : BaseService<ConfirmParticipantService.Request, ConfirmParticipantService.Response>
    {
        private IDataRepository _dataRepository;

        public ConfirmParticipantService(
          IDataRepository dataRepository,
          ILogger logger) : base(logger)
        {
            // CONSTRUCTOR
            _dataRepository = dataRepository;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            // THIS IS WHERE OUR SERVICE LOGIC WILL EXIST
            //throw new NotImplementedException();
            var pdto = new ParticipantDto
            {
                EventCode = request.EventCode,
                ParticipantCode = request.ParticipantCode,
                ParticipantValidated = true
            };

            // UPDATE IN REPOSITORY
            await _dataRepository.SaveOrUpdateParticipantAsync(pdto);

            var response = new Response 
            { 
                EventCode = request.EventCode,
                ParticipantCode = request.ParticipantCode
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
