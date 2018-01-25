using CodeMill.VMFirstNav;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;
using BlobCogBob.Core.Services;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Diagnostics;
using System.Linq;

namespace BlobCogBob.Core.ViewModels
{
    public class MenusListViewModel : NavigableViewModel
    {
        public MenusListViewModel()
        {
            Title = "Menus";
        }

        ICommand _refreshList;
        public ICommand RefreshList => _refreshList ??
            (_refreshList = new Command(async () => await ExecuteRefreshListCommand()));

        async Task ExecuteRefreshListCommand()
        {
            IsRefreshing = true;

            try
            {
                var blobListing = await StorageService.ListAllBlobs().ConfigureAwait(false);

                AllBlobs = new ObservableCollection<MenuBlob>();

                foreach (var blob in blobListing)
                {
                    AllBlobs.Add(new MenuBlob { BlobName = blob.BlobName, BlobUri = blob.BlobUri });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        ObservableCollection<MenuBlob> _allBlobs;
        public ObservableCollection<MenuBlob> AllBlobs { get => _allBlobs; set => SetProperty(ref _allBlobs, value); }
    }
}
