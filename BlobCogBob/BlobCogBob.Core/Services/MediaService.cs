using System;
using System.IO;
using System.Threading.Tasks;

using Plugin.Media.Abstractions;
using Plugin.Media;
using Xamarin.Forms;

namespace BlobCogBob.Core
{
    public static class MediaService
    {
        #region Events 

        public static event EventHandler NoCameraDetected;
        public static event EventHandler CannotPickPhoto;

        #endregion

        #region Photo Methods

        public static Stream GetPhotoStream(MediaFile mediaFile, bool disposeMediaFile)
        {
            var stream = mediaFile.GetStream();

            if (disposeMediaFile)
                mediaFile.Dispose();

            return stream;
        }

        public static async Task<MediaFile> GetMediaFileFromCamera()
        {
            await CrossMedia.Current.Initialize().ConfigureAwait(false);

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                OnNoCameraDetected();
                return null;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Large,
                DefaultCamera = CameraDevice.Rear
            }).ConfigureAwait(false);

            return file;
        }

        static void OnNoCameraDetected() => NoCameraDetected?.Invoke(null, EventArgs.Empty);

        #endregion

        #region Pick Methods

        public static async Task<MediaFile> GetMediaFileFromLibrary()
        {
            await CrossMedia.Current.Initialize().ConfigureAwait(false);

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                OnCannotPickPhoto();
                return null;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { PhotoSize = PhotoSize.Large }).ConfigureAwait(false);

            return file;
        }

        static void OnCannotPickPhoto() => CannotPickPhoto?.Invoke(null, EventArgs.Empty);

        #endregion
    }
}
