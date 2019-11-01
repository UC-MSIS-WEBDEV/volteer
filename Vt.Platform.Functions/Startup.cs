using System;
using Vt.Platform.Functions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tokenize.Client;
using Vt.Platform.AzureDataTables.Messaging;
using Vt.Platform.AzureDataTables.Repositories;
using Vt.Platform.Domain.Messaging;
using Vt.Platform.Domain.Repositories;
using Vt.Platform.Domain.Services;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Vt.Platform.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // DEPENDENCY INJECTION RULES GO HERE
            builder.Services.AddTransient(typeof(IObjectTokenizer), provider => new TokenizeClient(
                Environment.GetEnvironmentVariable("tokenize.hostname"),
                Environment.GetEnvironmentVariable("tokenize.client"),
                Environment.GetEnvironmentVariable("tokenize.authKey")
            ));

            builder.Services.AddTransient<IQueueManager, AzureStoragePublisher>();
            builder.Services.AddTransient<IMessagePublisher, AzureStoragePublisher>();

            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IRandomGenerator, RandomGenerator>();

            builder.Services.AddTransient<ITestContactRepository, TestContactRepository>();

        }
    }
}