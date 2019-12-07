using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vt.Platform.Domain.Enums;
using Vt.Platform.Domain.Models.Api;
using Vt.Platform.Domain.Models.Persistence;
using Vt.Platform.Domain.Repositories;
using Vt.Platform.Domain.Services;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.PublicServices.Participants
{
    public class GetParticipantsService : BaseService<GetParticipantsService.Request, GetParticipantsService.Response>
    {
        private IDataRepository _dataRepository;
        private IRandomGenerator _randomGenerator;
        private IEmailService _emailService;
        public GetParticipantsService(
          IDataRepository dataRepository,
          IRandomGenerator randomGenerator,
          IEmailService emailService,
          ILogger logger) : base(logger)
        {
            // INSTANTIATE BASE VALUES
            _dataRepository = dataRepository;
            _randomGenerator = randomGenerator;
            _emailService = emailService;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            await Task.CompletedTask;
            ParticipantDto[] dto = await _dataRepository.GetParticipantsAsync(request.EventCode);

            var response = new Response();
            response.EventCode = request.EventCode;
            response.UpdatedAt = DateTime.Now;
            response.Participants = dto;
           

    

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
            public string EventCode { get; set; }

        }

        public class Response : BaseResponse
        {
            // RESPONSE DATA MODEL GOES HERE
            public string EventCode { get; set; }
            public ParticipantDto[] Participants { get; set; }
            public DateTime UpdatedAt { get; set; }

        }
    }
}
