﻿using Foundation;
using LodFinals.Containers;
using LodFinals.iOS.DependencyServices;
using NoTryCatch.Core.Services;
using UIKit;

namespace LodFinals.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();

            global::Xamarin.Forms.Forms.Init();

            IocInitializer.Init(
                new PlatformAlertMessageService(),
                new PlatformSpeechToTextService(new DebuggerService()),
                new PlatformAudioPlayerService(),
                new PlatformFileManagerService());

            // Init nugets
            XamEffects.iOS.Effects.Init();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            Hackiftekhar.IQKeyboardManager.Xamarin.IQKeyboardManager.SharedManager().Enable = true;

            PanCardView.iOS.CardsViewRenderer.Preserve();

            MediaManager.CrossMediaManager.Current.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (Xamarin.Essentials.Platform.OpenUrl(app, url, options))
            {
                return true;
            }

            return base.OpenUrl(app, url, options);
        }
    }
}
