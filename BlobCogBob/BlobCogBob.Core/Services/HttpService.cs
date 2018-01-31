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
using Microsoft.Rest.Azure;
using System.IO;

namespace BlobCogBob.Core.Services
{
    public abstract class HttpService
    {
        static readonly JsonSerializer _serializer = new JsonSerializer();
        static readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };

        public static async Task<HttpResponseMessage> Post<T>(string url, T objectToPost, string authToken = null)
        {
            var payload = JsonConvert.SerializeObject(objectToPost);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    if (!string.IsNullOrWhiteSpace(authToken))
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                    request.Content = content;

                    return await client.SendAsync(request).ConfigureAwait(false);
                }

                //return await client.PostAsync(url, content).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"**** ERROR: {ex.Message}");
                return null;
            }
        }

        protected static async Task<TDataObject> GetDataObjectFromAPI<TDataObject, TPayloadData>(string apiUrl, TPayloadData data = default(TPayloadData), string authToken = null)
        {
            var stringPayload = string.Empty;

            if (data != null)
                stringPayload = await Task.Run(() => JsonConvert.SerializeObject(data)).ConfigureAwait(false);

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, apiUrl))
                {
                    if (!string.IsNullOrWhiteSpace(authToken))
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                    using (var response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                        using (var reader = new StreamReader(stream))
                        using (var json = new JsonTextReader(reader))
                        {
                            if (json == null)
                                return default(TDataObject);

                            return await Task.Run(() => _serializer.Deserialize<TDataObject>(json)).ConfigureAwait(false);

                        }
                    }
                }
            }
            catch (Exception)
            {
                return default(TDataObject);
            }
        }

    }
}
