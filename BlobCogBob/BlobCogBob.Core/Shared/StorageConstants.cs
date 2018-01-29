using System;

namespace BlobCogBob.Core
{
    public static class StorageConstants
    {
        public static readonly string AccountName = "blobcogbob";
        public static readonly string AccountUrlSuffix = "core.windows.net";
        public static readonly string PhotosContainerName = "menu-photos";
        public static readonly Uri QueueUri = new Uri("https://blobcogbob.queue.core.windows.net/");
    }
}
