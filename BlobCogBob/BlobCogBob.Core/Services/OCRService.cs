using System;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Rest;
using System.Text;
using System.Diagnostics;

using System.Net.Http.Formatting;
using System.Net.Http;

namespace BlobCogBob.Core
{
    public static class OCRService
    {

        public static async Task<string> ReadHandwrittenText(string urlOfImage)
        {
            try
            {
                var scc = new ApiKeyServiceClientCredentials(APIKeys.ComputerVisionAPIKey);
                var vision = new ComputerVisionAPI(scc);
                vision.AzureRegion = AzureRegions.Southcentralus;

                var headers = await vision.RecognizeTextAsync(urlOfImage, false).ConfigureAwait(false);

                var allSplits = headers.OperationLocation.Split(new string[] { @"/" }, StringSplitOptions.None);
                var operationId = allSplits[allSplits.GetUpperBound(0)];

                var opResult = await vision.GetTextOperationResultAsync(operationId).ConfigureAwait(false);

                while (opResult.Status != TextOperationStatusCodes.Succeeded && opResult.Status != TextOperationStatusCodes.Failed)
                {
                    opResult = await vision.GetTextOperationResultAsync(operationId).ConfigureAwait(false);
                }

                if (opResult.Status == TextOperationStatusCodes.Failed)
                    return null;


                StringBuilder allWords = new StringBuilder();
                foreach (var line in opResult.RecognitionResult.Lines)
                {
                    allWords.AppendLine(line.Text);
                }

                return allWords.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"*** ERROR: {ex.Message}");
            }

            return null;
        }
    }
}
