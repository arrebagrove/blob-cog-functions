using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using BlobCogBob.Core.Shared;
using BlobCogBob.Shared;
using System.IO;
using System.Net.Http;

namespace BlobCogBob.Core.Services
{
    public class FunctionService : HttpService
    {
        public async static Task<string> GetContainerReadSASToken()
        {
            var resp = await Post(BackendConstants.SASRetrievalUrl, new StoragePermissionRequest());

            return await ReadStringValueFromResponse(resp).ConfigureAwait(false);
        }

        public async static Task<string> GetContainerWriteSasToken()
        {
            var resp = await Post(BackendConstants.SASRetrievalUrl, new StoragePermissionRequest { Permission = "Write" });

            return await ReadStringValueFromResponse(resp).ConfigureAwait(false);
        }

        public async static Task<string> GetContainerListSasToken()
        {
            var resp = await Post(BackendConstants.SASRetrievalUrl, new StoragePermissionRequest
            {
                Permission = "List"
            });

            return await ReadStringValueFromResponse(resp).ConfigureAwait(false);
        }

        async static Task<string> ReadStringValueFromResponse(HttpResponseMessage response)
        {
            if (response?.Content == null)
                return null;

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}
