using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlobCogBob.Core.Services;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net.Http;
using MonkeyCache.FileStore;
using System.Runtime.InteropServices;
using Xamarin.Forms;

namespace BlobCogBob.Core
{
    public class StorageService
    {
        public async static Task<List<MenuBlob>> ListAllBlobs()
        {
            var listCredentials = await ObtainListCredentials();
            var csa = new CloudStorageAccount(listCredentials, StorageConstants.AccountName,
                                              StorageConstants.AccountUrlSuffix, true);

            var blobClient = csa.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(StorageConstants.PhotosContainerName);

            List<MenuBlob> theBlobs = new List<MenuBlob>();

            BlobContinuationToken continuationToken = null;
            do
            {
                var allBlobs = await container.ListBlobsSegmentedAsync("", true,
                                                                       BlobListingDetails.None, 100,
                                                                       continuationToken, null, null);

                continuationToken = allBlobs.ContinuationToken;

                foreach (var blob in allBlobs.Results)
                {
                    if ((blob is CloudBlockBlob cloudBlob))
                        theBlobs.Add(new MenuBlob { BlobName = cloudBlob.Name, BlobUri = cloudBlob.Uri });
                }

            } while (continuationToken != null);


            return theBlobs;
        }

        static async Task<StorageCredentials> ObtainListCredentials()
        {
            var cacheKey = "listCredentials";

            if (Barrel.Current.Exists(cacheKey) && !Barrel.Current.IsExpired(cacheKey))
                return Barrel.Current.Get<StorageCredentials>(cacheKey);

            var listToken = await FunctionService.GetContainerListSasToken().ConfigureAwait(false);

            return StuffCredentialsInBarrel(listToken, cacheKey);
        }

        static StorageCredentials ReadCredentials()
        {
            throw new NotImplementedException();
        }

        static StorageCredentials WriteCredentials()
        {
            throw new NotImplementedException();
        }

        static TimeSpan GetExpirationSpan(string tokenQueryString)
        {
            // We'll need to parse the token query string
            // easiest way is to make it ino a URI and parse it with URI.ParseQueryString

            var fakeTokenUri = new Uri($"http://localhost{tokenQueryString}");
            var queryStringParts = fakeTokenUri.ParseQueryString();

            var endDateString = queryStringParts["se"];

            // Expire one minute before we really need to
            var endTimeSpan = DateTimeOffset.Parse(endDateString) - DateTimeOffset.UtcNow - TimeSpan.FromMinutes(1);

            return endTimeSpan;
        }

        static StorageCredentials StuffCredentialsInBarrel(string storageToken, string cacheKey)
        {
            var credentials = new StorageCredentials(storageToken);
            var expireIn = GetExpirationSpan(storageToken);

            Barrel.Current.Add(cacheKey, credentials, expireIn);

            return credentials;
        }
    }
}
