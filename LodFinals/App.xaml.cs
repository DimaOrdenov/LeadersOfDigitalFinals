using System;
using Autofac;
using LodFinals.BusinessLayer;
using LodFinals.Containers;
using LodFinals.Services;
using LodFinals.Views;
using LodFinals.Views.OnBoarding;
using NoTryCatch.Xamarin.Portable.Services;
using Xamarin.Forms;

namespace LodFinals
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ContentPage();
        }

        protected override void OnStart()
        {
            INavigationService navigationService = IocInitializer.Container.Resolve<INavigationService>();
            ExtendedUserContext userContext = IocInitializer.Container.Resolve<ExtendedUserContext>();

            IocInitializer.Container.Resolve<ILexService>()
                .SetUser(Guid.NewGuid().ToString());

            userContext.TryRestore();

            if (userContext.IsFirstLaunch)
            {
                navigationService.SetRootPage<OnBoardingPage>();
            }
            else
            {
                navigationService.SetRootTabbedPage<MainTabbedPage>(0);
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
