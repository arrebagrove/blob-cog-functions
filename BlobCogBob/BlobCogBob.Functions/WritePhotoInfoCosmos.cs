using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using BlobCogBob.Shared;

namespace BlobCogBob.Functions
{
    public static class WritePhotoInfoCosmos
    {
        [FunctionName("WritePhotoInfoCosmos")]
        public static void Run([QueueTrigger("menu-photos")]string myQueueItem, 
            [DocumentDB("blobcogbob-db", "menu-photos", ConnectionStringSetting ="BlobCogBob_Cosmos", Id ="/BlobUrl")]out dynamic document,
            TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");

            try
            {
                var jsonified = JsonConvert.DeserializeObject<PhotoInfo>(myQueueItem);

                if (jsonified != null)
                    document = jsonified;
                else
                    document = null;
            }
            catch (JsonReaderException ex)
            {
                log.Error($"*** Error: {ex.Message}");
                document = null;
            }
        }
    }
}
