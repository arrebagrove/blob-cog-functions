using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using BlobCogBob.Core;

using CodeMill.VMFirstNav;
using Lottie.Forms.iOS.Renderers;

namespace BlobCogBob.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            AnimationViewRenderer.Init();

            LoadApplication(new App());

            NavigationService.Instance.RegisterViewModels(typeof(App).Assembly);

            return base.FinishedLaunching(app, options);
        }
    }
}
