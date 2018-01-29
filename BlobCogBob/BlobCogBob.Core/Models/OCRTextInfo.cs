using System;
namespace BlobCogBob.Core
{
    public class OCRTextInfo
    {
        public string LineText { get; set; }
        public ImageCoordinate LeftTop { get; set; }
        public ImageCoordinate RightTop { get; set; }
        public ImageCoordinate RightBottom { get; set; }
        public ImageCoordinate LeftBottom { get; set; }
        public double Rating { get; set; }
    }

    public class ImageCoordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
