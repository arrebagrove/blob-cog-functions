using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BlobCogBob.Shared;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Queue;

namespace BlobCogBob.Functions
{
    [StorageAccount("AzureWebJobsStorage")]
    public static class WritePhotoInfoQueue
    {
        [FunctionName("WritePhotoInfoQueue")]
        [return: Queue("menu-photos")]
        public static async Task<PhotoInfo> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage input, TraceWriter log)
        {
            // Will eventually use this when we require the user to be authorized to use the app
            var isAuth = input.GetRequestContext().Principal.Identity.IsAuthenticated;

            log.Info($"Is Auth: {isAuth}", "request context");

            try
            {
                PhotoInfo data = await input.Content.ReadAsAsync<PhotoInfo>();

                return data;
            }
            catch (Exception ex)
            {
                log.Error($"*** Error: {ex.Message}");
                return null;
            }
        }
    }
}
