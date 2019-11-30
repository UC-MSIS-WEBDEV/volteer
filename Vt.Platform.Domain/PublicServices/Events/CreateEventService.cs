
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

namespace Vt.Platform.Domain.PublicServices.Events
{
    public class CreateEventService : BaseService<CreateEventService.Request, CreateEventService.Response>
    {
        private IDataRepository _dataRepository;
        private IRandomGenerator _randomGenerator;
        private IEmailService _emailservice;
        public CreateEventService(
          IDataRepository dataRepository,
          IRandomGenerator randomGenerator,
          ILogger logger,
          IEmailService emailservice) : base(logger)
        {
            // CONSTRUCTOR
            _dataRepository = dataRepository;
            _randomGenerator = randomGenerator;
            _emailservice = emailservice;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            // THIS IS WHERE OUR SERVICE LOGIC WILL EXIST

            string eventCode = await _randomGenerator.GetEventCode();
            string organizerCode = await _randomGenerator.GetSigninToken();
            string confirmationCode = await _randomGenerator.GetSigninToken();


            var dto = new EventDto
            {
                EventCode = eventCode,
                OrganizerCode = organizerCode,
                ConfirmationCode = confirmationCode,
                EventDate = request.EventDate,
                EventDetails = request.EventDetails,
                EventLocation = request.EventLocation,
                EventSummary = request.EventSummary,
                NumberOfParticipants = request.NumberOfParticipants,
                OrganizerEmail = request.OrganizerEmail,
                OrganizerName = request.OrganizerName,
                OrganizerValidated = false
            };

            // SAVE IN REPOSITORY
            await _dataRepository.SaveOrUpdateEvent(dto);
            // CREATE RESPONSE OBJECT
            var response = new Response();
            // ASSIGN VALUES TO THE RESPONSE OBJECT
            response.EventCode = eventCode;
            MailAddress mailAddress = new MailAddress(request.OrganizerEmail);
            MailAddress[] mailAddresses = new MailAddress[] { mailAddress };
            string emailBodyfl = "Your event has been created. Please do not delete this email as it contains important links you will need to confirm and to administer your event.<br/> <br/> Next Steps: <br/> <br/>";
            string emailBodysl = "1. Confirm your Event <br/> Please click the following URL to confirm your event <br/> https://volteer.us/confirmEvent?event="+response.EventCode+"&confirm="+ confirmationCode + "<br/> <br/>";
            string emailBodytl = "2. Share your Event <br/> Use this link to share your event with your contacts <br/> https://volteer.us/events/"+response.EventCode + "<br/> <br/>";
            string emailBodyfol = "3. Administer your Event <br/> *Do Not Share This Link*. The link below is just for you and allows you to administer your event. <br/> Administration Link: https://volteer.us/editEvent?event="+response.EventCode+"&adminCode="+ organizerCode + "<br/> <br/>";
            string emailBodyfil = "4. Delete your Event <br/> *Do Not Share This Link*. The link below is just for you and allows you to delete your event. <br/> Delete Event Link: https://volteer.us/cancelEvent?event="+response.EventCode+"&adminCode="+ organizerCode;
            string emailBody = emailBodyfl + emailBodysl + emailBodytl+ emailBodyfol+ emailBodyfil;
            string emailSubject = "Volteer Event Created - Confirmation Required ";
            //Currently hardcoding email receipient, as it only accomodates verified email.
            await _emailservice.SendEmail(mailAddresses, emailSubject, emailBody);

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
            // RESPONSE DATA MODEL GOES HERE
            public string EventCode { get; set; }
        }
    }
}