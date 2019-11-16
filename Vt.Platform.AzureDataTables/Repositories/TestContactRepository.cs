using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Vt.Platform.AzureDataTables.Models;
using Vt.Platform.Domain.Models.Persistence;
using Vt.Platform.Domain.Repositories;
using Vt.Platform.Utils;

namespace Vt.Platform.AzureDataTables.Repositories
{
    public class TestContactRepository : RepositoryBase, ITestContactRepository
    {
        public async Task<PersistResponse> PersistContact(TestContactDto testContact)
        {
            var data = new TestContactTable
            {
                PartitionKey = "Contact",
                RowKey = testContact.Code,
                Name = testContact.Name,
                EmailHash = testContact.EmailMask,
                EmailToken = testContact.EmailToken
            };
            data.UpdateAuditable(testContact);

            var table = await GetTable("TestContacts");
            var insertOperation = TableOperation.Insert(data);
            await table.ExecuteAsync(insertOperation);

            return PersistResponse.Created();
        }

        public Task<TestContactDto> GetContact(string code)
        {
            throw new NotImplementedException();
        }
    }
}
