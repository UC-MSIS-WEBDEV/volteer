using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vt.Platform.Domain.Models.Persistence;
using Vt.Platform.Domain.PublicServices.System;
using Vt.Platform.Domain.Repositories;
using Vt.Platform.Domain.Services;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.PublicServices.Events
{
    public class GetMyEventsService : BaseService<GetMyEventsService.Request, GetMyEventsService.Response>
    {

        private IDataRepository _dataRepository;
        private IEmailService _emailService;

        public GetMyEventsService(
            IDataRepository dataRepository,
            IEmailService emailService,
            ILogger logger) : base(logger)
        {
            _dataRepository = dataRepository;
            _emailService = emailService;
        }

        protected override async Task<Response> Implementation(Request request)
        {
            string emailbody = await _dataRepository.GetMyEventsAsync(request.Mail, request.Name);

            MailAddress mailAddress = new MailAddress(request.Mail);
            MailAddress[] mailAddresses = new MailAddress[] { mailAddress };
            string emailBody = emailbody;
            string emailSubject = "Volteer.us has sent your events";
            await _emailService.SendEmail(mailAddresses, emailSubject, emailBody);

            var response = new Response();
            response.emailBody = emailbody;
            return response;
        }

        public override IDictionary<int, string> GetErrorCodes()
        {
            return new Dictionary<int, string>();
        }


        public class Request : BaseRequest
        {
            [Required]
            public string Mail { get; set; }
            [Required]
            public string Name { get; set; }
        }

        public class Response : BaseResponse
        {
            public string emailBody { get; set; }
        }

    }
}
