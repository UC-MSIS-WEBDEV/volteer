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
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.PublicServices.Participants
{
    public class GetParticipantsService : BaseService<GetParticipantsService.Request, GetParticipantsService.Response>
    {
        private IDataRepository _dataRepository;
        public GetParticipantsService(IDataRepository dataRepository, ILogger logger) : base(logger)
        {
            _dataRepository = dataRepository;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            await Task.CompletedTask;

            /*string eventCode = request.EventCode;

            ParticipantDto[] participants = await _dataRepository.GetParticipantsAsync(eventCode);
            var response = new Response();
            response.EventCode = request.EventCode;
            response.Participants = participants;
            return response;*/
            return new Response
            {
                EventCode = request.EventCode,
                UpdatedAt = DateTime.UtcNow,
                Participants = new[]
                {
                    new ParticipantDisplayInfoModel { Name = "Benny Davidson", Status = ParticipantEventStatus.Confirmed },
                    new ParticipantDisplayInfoModel { Name = "Matt Gilbert", Status = ParticipantEventStatus.Tentative },
                    new ParticipantDisplayInfoModel { Name = "Roman Boone", Status = ParticipantEventStatus.Confirmed },
                    new ParticipantDisplayInfoModel { Name = "Lynette Blake", Status = ParticipantEventStatus.Confirmed },
                    new ParticipantDisplayInfoModel { Name = "Marianne Baker", Status = ParticipantEventStatus.Confirmed },
                    new ParticipantDisplayInfoModel { Name = "Geoffrey Hamilton", Status = ParticipantEventStatus.Tentative },
                    new ParticipantDisplayInfoModel { Name = "Janice Bailey", Status = ParticipantEventStatus.Confirmed },
                    new ParticipantDisplayInfoModel { Name = "Brenda Cook", Status = ParticipantEventStatus.Confirmed },
                    new ParticipantDisplayInfoModel { Name = "Betsy Roberts", Status = ParticipantEventStatus.Tentative },
                    new ParticipantDisplayInfoModel { Name = "Lionel Ross", Status = ParticipantEventStatus.Confirmed }
                }
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
            public ParticipantDisplayInfoModel[] Participants { get; set; }
            public DateTime UpdatedAt { get; set; }
        }
    }
}
