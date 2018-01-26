using System;
using System.Collections.Generic;
using System.Text;

namespace BlobCogBob.Core.Shared
{
    public static class BackendConstants
    {
        public static readonly string FunctionAppUrl = "https://blobcogbob.azurewebsites.net";
        public static readonly string SASRetrievalUrl = $"{FunctionAppUrl}/api/SASRetrieval";
    }
}
