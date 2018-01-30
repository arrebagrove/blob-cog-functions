using System;
using System.Collections.Generic;
using System.Text;

namespace BlobCogBob.Shared
{
    public class PhotoInfo
    {
        public string UserId { get; set; }
        public string BlobUrl { get; set; }
        public List<OCRTextInfo> IdentifiedText { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int QualityRating { get; set; }
    }
}
