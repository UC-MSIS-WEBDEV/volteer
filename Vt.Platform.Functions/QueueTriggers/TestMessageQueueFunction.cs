using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Vt.Platform.Domain.Models.Messaging;
using Vt.Platform.Domain.Services;

namespace Vt.Platform.Functions.QueueTriggers
{
    public class TestMessageQueueFunction
    {
        private readonly ILogger<TestMessageQueueFunction> _logger;
        private readonly IEmailService _emailService;

        public TestMessageQueueFunction(
            ILogger<TestMessageQueueFunction> logger,
            IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        [FunctionName("TestMessageQueueFunction")]
        public async Task Run(
            [QueueTrigger("testmessage", Connection = "TableStorage")]
            string msg,
            int dequeueCount,
            ExecutionContext context)
        {
            var obj = JsonConvert.DeserializeObject<TestMessage>(msg);
            _logger.LogInformation($"Processing {obj.CorrelationId}. Dequeue Count: {dequeueCount}");
            var testAddress = Environment.GetEnvironmentVariable("Smtp.TestEmail");

            if (string.IsNullOrWhiteSpace(testAddress))
            {
                _logger.LogError("Cannot send test email as no test email address found");
                return;
            }

            _logger.LogInformation($"Using {testAddress} as the email address");
            await _emailService.SendEmail(
                new[] { new MailAddress(testAddress) },
                "Volteer Test Email", 
                $"Test Email at: {DateTime.UtcNow}. CorrelationId={obj.CorrelationId}<br /><br />{obj.Message}");

        }
    }
}
