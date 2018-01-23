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

            MainPage = new NavigationPage(new MenusList());
        }
    }
}
