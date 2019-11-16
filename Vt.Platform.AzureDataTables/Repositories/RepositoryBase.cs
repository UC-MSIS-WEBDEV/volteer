using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vt.Platform.AzureDataTables.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Vt.Platform.AzureDataTables.Repositories
{
    public abstract class RepositoryBase
    {
        protected RepositoryBase()
        {
            TableClient = CreateClient();
        }

        private static readonly HashSet<string> InitializedTables = new HashSet<string>();

        protected CloudTableClient TableClient { get; }

        private static CloudTableClient CreateClient()
        {
            var connectionString = Environment.GetEnvironmentVariable("TableStorage");
            var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            return cloudStorageAccount.CreateCloudTableClient();
        }

        protected async Task<CloudTable> GetTable(string tableName)
        {
            var table = TableClient.GetTableReference(tableName);
            if (!InitializedTables.Contains(tableName))
            {
                await table.CreateIfNotExistsAsync();
                InitializedTables.Add(tableName);
            }

            return table;
        }

        protected Task<TableResult> InsertDataAsync<T>(CloudTable table, T data) where T : BaseTable
        {
            TableOperation insertOperation = TableOperation.Insert(data);
            return table.ExecuteAsync(insertOperation);
        }
    }
}