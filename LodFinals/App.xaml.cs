﻿using System;
using Autofac;
using LodFinals.Containers;
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

            navigationService.SetRootPage<MainPage>();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
