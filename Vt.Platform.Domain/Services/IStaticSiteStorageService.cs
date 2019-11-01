using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vt.Platform.Domain.Services
{
    public interface IStaticSiteStorageService
    {
        Task StoreContent(string path, string contentType, byte[] content);
    }
}
