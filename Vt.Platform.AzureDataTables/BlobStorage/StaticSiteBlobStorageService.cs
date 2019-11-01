using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Vt.Platform.Domain.Services;

namespace Vt.Platform.AzureDataTables.BlobStorage
{
    public class StaticSiteBlobStorageService : IStaticSiteStorageService
    {
        static StaticSiteBlobStorageService()
        {
            BlobClient = CreateClient();
        }

        public async Task StoreContent(string path, string contentType, byte[] content)
        {
            path = path.Substring(1, path.Length - 1);
            var container = Environment.GetEnvironmentVariable("Blob.WebContainer");
            CloudBlobContainer cloudBlobContainer = BlobClient.GetContainerReference(container);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(path.ToLowerInvariant());
            cloudBlockBlob.Properties.ContentType = contentType;
            await cloudBlockBlob.UploadFromByteArrayAsync(content, 0, content.Length);
        }

        private static readonly CloudBlobClient BlobClient;

        private static CloudBlobClient CreateClient()
        {
            var connectionString = Environment.GetEnvironmentVariable("TableStorage");
            var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            return cloudStorageAccount.CreateCloudBlobClient();
        }
    }
}
