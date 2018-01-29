using System;
using System.Collections.Generic;

namespace BlobCogBob.Core
{
    public class PhotoInfo
    {
        public string UserId { get; set; }
        public string BlobUrl { get; set; }
        public List<OCRTextInfo> IdentifiedText { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
