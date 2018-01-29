using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;
using CodeMill.VMFirstNav;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BlobCogBob.Core
{
    public class AddPhotoViewModel : NavigableViewModel
    {
        readonly string found_results_info = "Here's what we found:";
        readonly string no_results_info = "We couldn't find any words in that photo!";

        public AddPhotoViewModel()
        {
            Title = "Add Photo";
            SearchComplete = false;
            SearchInProgress = false;
        }

        #region Properties

        double uploadProgress;
        public double UploadProgress
        {
            get => uploadProgress;
            set
            {
                SetProperty(ref uploadProgress, value);
            }
        }

        string foundWords;
        public string FoundWords
        {
            get => foundWords;
            set
            {
                SetProperty(ref foundWords, value, onChanged: () => ((Command)Save).ChangeCanExecute());
            }
        }

        string searchResultInfo;
        public string SearchResultInfo
        {
            get => searchResultInfo;
            set
            {
                SetProperty(ref searchResultInfo, value);
            }
        }

        bool searchComplete;
        public bool SearchComplete
        {
            get => searchComplete;
            set
            {
                SetProperty(ref searchComplete, value);
            }
        }

        bool searchInProgress;
        public bool SearchInProgress
        {
            get => searchInProgress;
            set
            {
                SetProperty(ref searchInProgress, value);
            }
        }

        ImageSource theImage;
        public ImageSource TheImage
        {
            get => theImage;
            set
            {
                SetProperty(ref theImage, value);
            }
        }

        #endregion

        #region Command Properties

        ICommand _cancelCommand;
        public ICommand Cancel => _cancelCommand ??
            (_cancelCommand = new Command(async () => await ExecuteCancelCommand()));

        async Task ExecuteCancelCommand()
        {
            await NavigationService.Instance.PopModalAsync();
        }

        ICommand _saveCommand;
        public ICommand Save => _saveCommand ??
        (_saveCommand = new Command(async () => await ExecuteSaveCommand(), () => !string.IsNullOrWhiteSpace(FoundWords)));

        async Task ExecuteSaveCommand()
        {
            await NavigationService.Instance.PopModalAsync();
        }

        ICommand _takePhotoCommand;
        public ICommand TakePhoto => _takePhotoCommand ??
        (_takePhotoCommand = new Command(async () => await ExecuteTakePhotoCommand()));

        public async Task ExecuteTakePhotoCommand()
        {
            if (SearchInProgress)
                return;

            try
            {
                SearchInProgress = true;
                SearchComplete = false;
                SearchResultInfo = "";
                FoundWords = "";
                TheImage = null;
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
                using (var mediaStream = MediaService.GetPhotoStream(thePhoto, false))
                {
                    TheImage = ImageSource.FromStream(() => MediaService.GetPhotoStream(thePhoto, true));
                    progressUpdater.TotalImageBytes = mediaStream.Length;
                    blobAddress = await StorageService.UploadBlob(mediaStream, progressUpdater).ConfigureAwait(false);
                }

                progressUpdater.Updated -= UpdateImageUploadProgress;

                if (blobAddress != null)
                {
                    var ocrResult = await OCRService.ReadHandwrittenText(blobAddress.ToString()).ConfigureAwait(false);

                    if (ocrResult != null)
                    {
                        var tempWords = new StringBuilder();
                        ocrResult.ForEach(ocr => tempWords.AppendLine(ocr.LineText));

                        FoundWords = tempWords.ToString();
                        SearchResultInfo = found_results_info;
                    }
                    else
                    {
                        FoundWords = "";
                        SearchResultInfo = no_results_info;
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Upload Error", "Couldn't upload the photo to BLOB storage", "OK");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"*** {ex.Message}");
            }
            finally
            {
                SearchComplete = true;
                SearchInProgress = false;
            }
        }

        #endregion

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
