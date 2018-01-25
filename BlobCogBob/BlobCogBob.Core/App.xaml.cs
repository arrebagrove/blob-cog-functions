using BlobCogBob.Core.ViewModels;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BlobCogBob.Core
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MonkeyCache.FileStore.Barrel.ApplicationId = "blobcogbob";

            MainPage = new NavigationPage(new MenusList { ViewModel = new MenusListViewModel() });
        }
    }
}
