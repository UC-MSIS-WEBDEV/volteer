﻿using Microsoft.Extensions.Logging;
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
    public class CancelEventService : BaseService<CancelEventService.Request, CancelEventService.Response>
    {
        private IDataRepository _dataRepository;

        public CancelEventService(
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
            var dto = new EventDto
            {
                EventCode = request.EventCode,
                ConfirmationCode = request.ConfirmationCode
               
            };

            // UPDATE IN REPOSITORY
            await _dataRepository.CancelEventAsync(dto);

            var response = new Response
            {
                EventCode = request.EventCode,
                 EventSummary = dto.EventSummary,
                EventDetails = dto.EventDetails,

                EventDate = dto.EventDate,
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
            public string ConfirmationCode { get; set; }
        }

        public class Response : BaseResponse
        {
            // RESPONSE DATA MODEL GOES HERE
            [Required]
            public string EventCode { get; set; }
            public string OrganizerName { get; set; }
            public string EventDetails { get; set; }
            public string EventSummary { get; set; }
            public DateTime EventDate { get; set; }
            public bool OrganizerValidated { get; set; }
        }
    }
}

