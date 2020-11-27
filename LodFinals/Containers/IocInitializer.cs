using System;
using System.Reflection;
using Autofac;
using LodFinals.DependencyServices;
using LodFinals.Helpers;
using LodFinals.Services;
using LodFinals.ViewModels;
using LodFinals.Views;
using NoTryCatch.BL.Core;
using NoTryCatch.BL.Core.Exceptions;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.Services.PlatformServices;
using NoTryCatch.Xamarin.Portable.ViewModels;
using RestSharp;

namespace LodFinals.Containers
{
    public static class IocInitializer
    {
        public static IContainer Container { get; private set; }

        public static void Init(
            IPlatformAlertMessageService platformAlertMessageServiceImplementation,
            IPlatformSpeechToTextService platformSpeechToTextService)
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Services
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<PageFactory>().As<IPageFactory>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<DebuggerService>().As<IDebuggerService>().SingleInstance();
            builder.RegisterType<ExceptionHandler>().As<IExceptionHandler>().SingleInstance();
            builder.RegisterType<UserContext>().AsSelf().SingleInstance();
            builder.RegisterType<SpeechToTextService>().As<ISpeechToTextService>().SingleInstance();

            builder.RegisterInstance(platformAlertMessageServiceImplementation).As<IPlatformAlertMessageService>().SingleInstance();
            builder.RegisterInstance(platformSpeechToTextService).As<IPlatformSpeechToTextService>().SingleInstance();

            // BL
            builder.RegisterInstance<IRestClient>(new RestClient(Secrets.ApiUrl));

            // ViewModels
            //builder.RegisterType<MainPViewModel>().AsSelf();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.FullName.Contains(nameof(ViewModels)))
                .OnActivated(e =>
                {
                    if (!(e.Instance is PageViewModel pageVm))
                    {
                        return;
                    }

                    pageVm.ExceptionHandler.AddExceptionTypeHandler<BusinessLogicException>((ex, action) => Container.Resolve<DialogService>().DisplayAlert("Ошибка", ex.Message, "Ок"));
                    pageVm.ExceptionHandler.AddExceptionTypeHandler<Exception>((ex, action) => Container.Resolve<DialogService>().DisplayAlert("Ошибка", ex.Message, "Ок"));
                })
                .AsSelf();

            Container = builder.Build();

            IPageFactory pageFactory = Container.Resolve<IPageFactory>();

            // Pages
            pageFactory.Configure<MainPage, MainPViewModel>(() => Container.Resolve<MainPViewModel>());
        }
    }
}
