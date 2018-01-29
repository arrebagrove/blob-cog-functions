using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;
using CodeMill.VMFirstNav;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace BlobCogBob.Core
{
    public class AddPhotoViewModel : NavigableViewModel
    {
        public AddPhotoViewModel()
        {
            Title = "Add Photo";
        }

        double uploadProgress;
        public double UploadProgress
        {
            get => uploadProgress;
            set
            {
                SetProperty(ref uploadProgress, value);
            }
        }

        ICommand _cancelCommand;
        public ICommand Cancel => _cancelCommand ??
            (_cancelCommand = new Command(async () => await ExecuteCancelCommand()));

        async Task ExecuteCancelCommand()
        {
            await NavigationService.Instance.PopModalAsync();
        }

        ICommand _takePhotoCommand;
        public ICommand TakePhoto => _takePhotoCommand ??
        (_takePhotoCommand = new Command(async () => await ExecuteTakePhotoCommand()));

        public async Task ExecuteTakePhotoCommand()
        {
            UploadProgress = 0;
            UploadProgress progressUpdater = new UploadProgress();

            progressUpdater.Updated += UpdateImageUploadProgress;

            var shouldTakeNewPhoto = await ShouldTakeNewPhoto();

            MediaFile thePhoto = null;
            if (shouldTakeNewPhoto)
                thePhoto = await MediaService.GetMediaFileFromCamera().ConfigureAwait(false);
            else
                thePhoto = await MediaService.GetMediaFileFromLibrary().ConfigureAwait(false);

            if (thePhoto == null)
                return;

            Uri blobAddress;
            using (var mediaStream = MediaService.GetPhotoStream(thePhoto, true))
            {
                progressUpdater.TotalImageBytes = mediaStream.Length;
                blobAddress = await StorageService.UploadBlob(mediaStream, progressUpdater).ConfigureAwait(false);
            }

            progressUpdater.Updated -= UpdateImageUploadProgress;


            if (blobAddress != null)
            {
                await OCRService.ReadHandwrittenText(blobAddress.ToString()).ConfigureAwait(false);

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await NavigationService.Instance.PopModalAsync().ConfigureAwait(false);
                });
            }
            else
                await Application.Current.MainPage.DisplayAlert("bad bad", "c'mon", "fine");
        }

        void UpdateImageUploadProgress(object sender, double e)
        {
            UploadProgress = e;
        }

        async Task<bool> ShouldTakeNewPhoto()
        {
            const string take_photo = "Take Photo";
            string takePhotoResult = default(string);

            takePhotoResult = await Application.Current.MainPage.DisplayActionSheet(
                "Take or Pick Photo",
                "Cancel",
               null,
                new string[] { "Take Photo", "Pick Photo" }
            );

            return takePhotoResult == take_photo;
        }
    }
}
