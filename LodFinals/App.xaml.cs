using System;
using Autofac;
using LodFinals.BusinessLayer;
using LodFinals.Containers;
using LodFinals.Services;
using LodFinals.Views;
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

            IocInitializer.Container.Resolve<ILexService>()
                .SetUser(Guid.NewGuid().ToString());

            IocInitializer.Container.Resolve<ExtendedUserContext>().TryRestore()
                .ContinueWith(t => navigationService.SetRootTabbedPage<MainTabbedPage>(0));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
