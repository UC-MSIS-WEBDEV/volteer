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
    public class ConfirmEventService : BaseService<ConfirmEventService.Request, ConfirmEventService.Response>
    {
        private IDataRepository _dataRepository;
        public ConfirmEventService(
            IDataRepository dataRepository,
            ILogger logger) : base(logger)
        {
            _dataRepository = dataRepository;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            // THIS IS WHERE OUR SERVICE LOGIC WILL EXIST
            await Task.CompletedTask;
            string eventCode = request.EventCode;
            string confirmationCode = request.ConfirmationCode;

            var dto = new EventDto
            {
              EventCode = eventCode,
              ConfirmationCode = confirmationCode
            };

            // SAVE IN REPOSITORY
            await _dataRepository.SaveOrUpdateEvent(dto);

            return new Response
            {
                statusCode = "ok"
            };

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
            public string EventCode { get; set; }
            [Required]
            public string ConfirmationCode { get; set; }
        }

        public class Response : BaseResponse
        {
            // RESPONSE DATA MODEL GOES HERE
            public string statusCode { get; set; }
        }
    }
}
