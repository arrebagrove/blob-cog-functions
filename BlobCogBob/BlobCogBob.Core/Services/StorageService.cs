using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlobCogBob.Core.Services;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net.Http;
using MonkeyCache.FileStore;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace BlobCogBob.Core
{
    enum StoragePermissionType
    {
        List,
        Read,
        Write
    }

    public class StorageService
    {
        public async static Task<List<MenuBlob>> ListAllBlobs()
        {
            var listCredentials = await ObtainStorageCredentials(StoragePermissionType.List);
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
                                                                       continuationToken, null, null).ConfigureAwait(false);

                continuationToken = allBlobs.ContinuationToken;

                foreach (var blob in allBlobs.Results)
                {
                    if ((blob is CloudBlockBlob cloudBlob))
                        theBlobs.Add(new MenuBlob { BlobName = cloudBlob.Name, BlobUri = cloudBlob.Uri });
                }

            } while (continuationToken != null);


            return theBlobs;
        }

        public async static Task<Uri> UploadBlob(Stream blobContent, UploadProgress progressUpdater)
        {
            Uri blobAddress = null;
            try
            {
                var writeCredentials = await ObtainStorageCredentials(StoragePermissionType.Write);

                var csa = new CloudStorageAccount(writeCredentials, StorageConstants.AccountName, StorageConstants.AccountUrlSuffix, true);

                var blobClient = csa.CreateCloudBlobClient();

                var container = blobClient.GetContainerReference(StorageConstants.PhotosContainerName);

                var blockBlob = container.GetBlockBlobReference($"{Guid.NewGuid()}.png");

                await blockBlob.UploadFromStreamAsync(blobContent, null, null, null, progressUpdater, new CancellationToken());

                blobAddress = blockBlob.Uri;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"*** Error {ex.Message}");

                return null;
            }

            return blobAddress;
        }

        #region Helpers

        static async Task<StorageCredentials> ObtainStorageCredentials(StoragePermissionType permissionType)
        {
            var cacheKey = permissionType.ToString();

            if (Barrel.Current.Exists(cacheKey) && !Barrel.Current.IsExpired(cacheKey))
                return new StorageCredentials(Barrel.Current.Get<string>(cacheKey));

            string storageToken;
            switch (permissionType)
            {
                case StoragePermissionType.List:
                    storageToken = await FunctionService.GetContainerListSasToken();
                    break;
                case StoragePermissionType.Read:
                    storageToken = await FunctionService.GetContainerReadSASToken();
                    break;
                case StoragePermissionType.Write:
                    storageToken = await FunctionService.GetContainerWriteSasToken();
                    break;
                default:
                    storageToken = null;
                    break;
            }

            return storageToken == null ? null : StuffCredentialsInBarrel(storageToken, cacheKey);
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

            Barrel.Current.Add<string>(cacheKey, storageToken, expireIn);

            return credentials;
        }

        #endregion

    }



}
