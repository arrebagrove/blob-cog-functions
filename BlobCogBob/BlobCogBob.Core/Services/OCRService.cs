using System;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Rest;
using System.Text;
using System.Diagnostics;

using System.Net.Http.Formatting;
using System.Net.Http;
using System.Collections.Generic;
using BlobCogBob.Shared;

namespace BlobCogBob.Core
{
    public static class OCRService
    {

        public static async Task<List<OCRTextInfo>> ReadHandwrittenText(string urlOfImage)
        {
            try
            {
                var scc = new ApiKeyServiceClientCredentials(APIKeys.ComputerVisionAPIKey);
                var vision = new ComputerVisionAPI(scc);

                // Set this to whatever region you created your service in the portal
                vision.AzureRegion = AzureRegions.Southcentralus;

                var headers = await vision.RecognizeTextAsync(urlOfImage, true).ConfigureAwait(false);

                var allSplits = headers.OperationLocation.Split(new string[] { @"/" }, StringSplitOptions.None);
                var operationId = allSplits[allSplits.GetUpperBound(0)];

                var opResult = await vision.GetTextOperationResultAsync(operationId).ConfigureAwait(false);

                while (opResult.Status != TextOperationStatusCodes.Succeeded && opResult.Status != TextOperationStatusCodes.Failed)
                {
                    opResult = await vision.GetTextOperationResultAsync(operationId).ConfigureAwait(false);
                }

                if (opResult.Status == TextOperationStatusCodes.Failed)
                    return null;

                var ocrInfo = new List<OCRTextInfo>();
                foreach (var line in opResult.RecognitionResult.Lines)
                {
                    ocrInfo.Add(new OCRTextInfo
                    {
                        LineText = line.Text,
                        LeftTop = new ImageCoordinate { X = line.BoundingBox[0], Y = line.BoundingBox[1] },
                        RightTop = new ImageCoordinate { X = line.BoundingBox[2], Y = line.BoundingBox[3] },
                        RightBottom = new ImageCoordinate { X = line.BoundingBox[4], Y = line.BoundingBox[5] },
                        LeftBottom = new ImageCoordinate { X = line.BoundingBox[6], Y = line.BoundingBox[7] }
                    });
                }

                return ocrInfo;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"*** ERROR: {ex.Message}");
                return null;
            }
        }
    }
}
