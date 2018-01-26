using System;
using System.Collections.Generic;

using Xamarin.Forms;
using CodeMill.VMFirstNav;

namespace BlobCogBob.Core
{
    public partial class AddPhotoPage : ContentPage, IViewFor<AddPhotoViewModel>
    {
        public AddPhotoPage()
        {
            InitializeComponent();
        }

        AddPhotoViewModel vm;
        public AddPhotoViewModel ViewModel { get => vm; set { vm = value; BindingContext = vm; } }
    }
}
