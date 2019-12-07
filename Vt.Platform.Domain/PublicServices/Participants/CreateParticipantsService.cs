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
using System.Net.Mail;

namespace Vt.Platform.Domain.PublicServices.Participants
{
    public class CreateParticipantsService : BaseService<CreateParticipantsService.Request, CreateParticipantsService.Response>
    {
        private IDataRepository _dataRepository;
        private IRandomGenerator _randomGenerator;
        private IEmailService _emailService;

        public CreateParticipantsService(
          IDataRepository dataRepository,
          IRandomGenerator randomGenerator,
          IEmailService emailService,
          ILogger logger) : base(logger)
        {
            // CONSTRUCTOR
            _dataRepository = dataRepository;
            _randomGenerator = randomGenerator;
            _emailService = emailService;
        }

        protected override async Task<Response> Implementation(Request request)
        {

            string partcipantCode = await _randomGenerator.GetParticipantCode(); 
            string confirmationCode = await _randomGenerator.GetSigninToken();
            // THIS IS WHERE OUR SERVICE LOGIC WILL EXIST
            //CREATE PARTICIPANT DTO
            var pdto = new ParticipantDto
            {
                EventCode = request.EventCode,
                ParticipantCode = partcipantCode,
                ConfirmationCode = confirmationCode,
                ParticipantName = request.ParticipantName,
                ParticipantEmail = request.ParticipantEmail,
                ParticipantStatus = request.ParticipantStatus,
                ParticipantValidated = false
            };

            await _dataRepository.SaveOrUpdateParticipantAsync(pdto);

            MailAddress mailAddress = new MailAddress(request.ParticipantEmail);
            MailAddress[] mailAddresses = new MailAddress[] { mailAddress };
            string emailBody = "This is an auto generated email. Please click on the below link to conform your participation to the event " + request.EventCode + "<br>";
            emailBody = emailBody + "http://localhost:57834/participants/ConfirmParticipant?" + request.EventCode + "/" + partcipantCode;
            string emailSubject = "Confirmation email for the event participation";
            await _emailService.SendEmail(mailAddresses, emailSubject, emailBody);

            // CREATE RESPONSE OBJECT
            var response = new Response();
            // ASSIGN VALUES TO THE RESPONSE OBJECT
            response.PartcipantCode = partcipantCode;  

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
            public string ParticipantName { get; set; }
            public string ParticipantEmail { get; set; }
            public string ParticipantStatus { get; set; }
        }

        public class Response : BaseResponse
        {
            // RESPONSE DATA MODEL GOES HERE
            public string PartcipantCode { get; set; }
        }
    }
}