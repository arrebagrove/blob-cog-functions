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

            bool uploadResult = false;

            var options = new StoreCameraMediaOptions();

            using (var photo = await CrossMedia.Current.TakePhotoAsync(options).ConfigureAwait(false))
            {
                using (var mediaStream = photo.GetStream())
                {
                    progressUpdater.TotalImageBytes = mediaStream.Length;
                    uploadResult = await StorageService.UploadBlob(mediaStream, progressUpdater).ConfigureAwait(false);
                }
            }

            progressUpdater.Updated -= UpdateImageUploadProgress;

            if (uploadResult)
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await NavigationService.Instance.PopModalAsync().ConfigureAwait(false);
                });
            else
                await Application.Current.MainPage.DisplayAlert("bad bad", "c'mon", "fine");
        }

        void UpdateImageUploadProgress(object sender, double e)
        {
            UploadProgress = e;
        }
    }
}
