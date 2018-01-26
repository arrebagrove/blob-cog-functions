using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;

// Overall implementation thanks to Brandon Minnick
// https://github.com/brminnick/AzureBlobStorageSampleApp/blob/master/AzureBlobStorageSampleApp/Services/Base/BaseHttpClientService.cs
using System.Diagnostics;

namespace BlobCogBob.Core.Services
{
    public abstract class HttpService
    {
        static readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };

        public static async Task<HttpResponseMessage> Post<T>(string url, T objectToPost)
        {
            var payload = JsonConvert.SerializeObject(objectToPost);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            try
            {
                return await client.PostAsync(url, content).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"**** ERROR: {ex.Message}");
                return null;
            }
        }
    }
}
