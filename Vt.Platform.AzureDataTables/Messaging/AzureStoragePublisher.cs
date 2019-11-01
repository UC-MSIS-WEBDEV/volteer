using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Vt.Platform.Domain.Enums;
using Vt.Platform.Domain.Messaging;
using Vt.Platform.Utils;

namespace Vt.Platform.AzureDataTables.Messaging
{
    public class AzureStoragePublisher : IMessagePublisher, IQueueManager
    {
        private readonly ILogger<AzureStoragePublisher> _logger;

        static AzureStoragePublisher()
        {
            TableClient = CreateClient();
        }

        public AzureStoragePublisher(ILogger<AzureStoragePublisher> logger)
        {
            _logger = logger;
        }

        public async Task Publish(MessageTopic topic, ITraceable message, Guid correlationId)
        {
            message.CorrelationId = correlationId;
            var queueName = topic.ToString().ToLowerInvariant();
            var queue = TableClient.GetQueueReference(queueName);
            bool retry;
            int attempts = 0;

            do
            {
                retry = false;
                attempts++;

                try
                {
                    var msg = new CloudQueueMessage(JsonConvert.SerializeObject(message));
                    _logger.LogInformation($"Published message {queueName}:{message.CorrelationId}");
                    await queue.AddMessageAsync(msg);
                }
                catch (StorageException ex)
                {
                    _logger.LogError(ex, $"Error publishing message {queueName}:{message.CorrelationId}");
                    var ri = ex.RequestInformation;
                    if (ri.HttpStatusCode == 404) // if error response is 404 then queue does not exist
                    {
                        _logger.LogInformation($"Creating queue {queueName}");
                        await queue.CreateIfNotExistsAsync();
                        _logger.LogInformation($"Created queue {queueName} successfully");
                        retry = true;
                    }
                    else
                    {
                        throw;
                    }
                }
            } while (retry && attempts < 4);
        }

        private static readonly CloudQueueClient TableClient;

        private static CloudQueueClient CreateClient()
        {
            var connectionString = Environment.GetEnvironmentVariable("TableStorage");
            var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            return cloudStorageAccount.CreateCloudQueueClient();
        }

        public async Task BuildQueues()
        {

            var allTopics = Enum.GetNames(typeof(MessageTopic));
            foreach (var topic in allTopics)
            {
                var queue = TableClient.GetQueueReference(topic.ToLowerInvariant());
                await queue.CreateIfNotExistsAsync();
            }
        }
    }
}
