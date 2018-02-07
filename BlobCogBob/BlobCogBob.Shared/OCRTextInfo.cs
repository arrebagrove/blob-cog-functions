using System;
using System.Collections.Generic;
using System.Text;

namespace BlobCogBob.Shared
{
    public class OCRTextInfo
    {
        public string LineText { get; set; }
        public ImageCoordinate LeftTop { get; set; }
        public ImageCoordinate RightTop { get; set; }
        public ImageCoordinate RightBottom { get; set; }
        public ImageCoordinate LeftBottom { get; set; }
        public double Rating { get; set; }
        public BeerInfo BeerInfo { get; set; }
    }
}
