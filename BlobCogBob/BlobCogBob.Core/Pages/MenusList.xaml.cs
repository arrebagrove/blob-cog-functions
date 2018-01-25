using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using CodeMill.VMFirstNav;
using BlobCogBob.Core.ViewModels;

namespace BlobCogBob.Core
{
    public partial class MenusList : ContentPage, IViewFor<MenusListViewModel>
    {
        public MenusList()
        {
            InitializeComponent();
        }

        MenusListViewModel vm;
        public MenusListViewModel ViewModel { get => vm; set { vm = value; BindingContext = vm; } }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.BeginInvokeOnMainThread(blobList.BeginRefresh);
        }
    }
}
