using System;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Xamarin.Forms;
using System.Diagnostics;
using Microsoft.Data.Edm;

namespace BlobCogBob.Core
{
    public class UploadProgress : IProgress<StorageProgress>
    {
        public event EventHandler<double> Updated;

        public double TotalImageBytes { get; set; }

        void IProgress<StorageProgress>.Report(StorageProgress value)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (TotalImageBytes == 0)
                    return;

                double updatePercentage = (double)value.BytesTransferred / (double)TotalImageBytes;
                Debug.WriteLine($"bytes; {value.BytesTransferred} out of {TotalImageBytes} for {updatePercentage}");

                Updated?.Invoke(this, updatePercentage);
            });
        }
    }
}
