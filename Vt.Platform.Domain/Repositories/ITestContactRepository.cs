using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vt.Platform.Domain.Models.Persistence;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.Repositories
{
    public interface ITestContactRepository
    {
        Task<PersistResponse> PersistContact(TestContactDto testContact);
        Task<TestContactDto> GetContact(string code);
    }
}
