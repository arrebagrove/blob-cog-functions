using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using BlobCogBob.Core.Shared;
using BlobCogBob.Shared;

namespace BlobCogBob.Core.Services
{
    public class FunctionService : HttpService
    {
        public async static Task<string> GetContainerReadSASToken()
        {
            var resp = await Post(BackendConstants.SASRetrievalUrl, new StoragePermissionRequest());

            return await resp?.Content.ReadAsStringAsync();
        }

        public async static Task<string> GetContainerWriteSasToken()
        {
            var resp = await Post(BackendConstants.SASRetrievalUrl, new StoragePermissionRequest { Permission = "Write" });

            return await resp?.Content.ReadAsStringAsync();
        }
    }
}
