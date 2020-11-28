using System;
using System.Collections.Generic;
using System.Reflection;
using Amazon;
using Amazon.Comprehend;
using Amazon.Lex;
using Autofac;
using FFImageLoading.Svg.Forms;
using Google.Apis.Services;
using Google.Cloud.Translation.V2;
using LodFinals.BusinessLayer;
using LodFinals.DependencyServices;
using LodFinals.Helpers;
using LodFinals.Services;
using LodFinals.ViewModels;
using LodFinals.ViewModels.Exercises;
using LodFinals.ViewModels.Profile;
using LodFinals.Views;
using LodFinals.Views.Exercises;
using LodFinals.Views.Profile;
using NoTryCatch.BL.Core;
using NoTryCatch.BL.Core.Exceptions;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.Services.PlatformServices;
using NoTryCatch.Xamarin.Portable.ViewModels;
using RestSharp;
using Xamarin.Forms;

namespace LodFinals.Containers
{
    public static class IocInitializer
    {
        public static IContainer Container { get; private set; }

        public static void Init(
            IPlatformAlertMessageService platformAlertMessageServiceImplementation,
            IPlatformSpeechToTextService platformSpeechToTextService,
            IPlatformAudioPlayerService platformAudioPlayerService,
            IPlatformFileManagerService platformFileManagerService)
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Services
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<PageFactory>().As<IPageFactory>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<DebuggerService>().As<IDebuggerService>().SingleInstance();
            builder.RegisterType<ExceptionHandler>().As<IExceptionHandler>().SingleInstance();
            builder.RegisterType<ExtendedUserContext>().As<UserContext>().AsSelf().SingleInstance();
            builder.RegisterType<LexService>().As<ILexService>().SingleInstance();

            var lexClient = new AmazonLexClient(Secrets.LexKeyId, Secrets.LexAccessKey, region: RegionEndpoint.EUCentral1);
            builder.RegisterInstance(lexClient).SingleInstance();

            var lexComprehendClient = new AmazonComprehendClient(Secrets.LexKeyId, Secrets.LexAccessKey, region: RegionEndpoint.EUCentral1);
            builder.RegisterInstance(lexComprehendClient).SingleInstance();

            builder.RegisterInstance(platformAlertMessageServiceImplementation).As<IPlatformAlertMessageService>().SingleInstance();
            builder.RegisterInstance(platformSpeechToTextService).As<IPlatformSpeechToTextService>().SingleInstance();
            builder.RegisterInstance(platformAudioPlayerService).As<IPlatformAudioPlayerService>().SingleInstance();
            builder.RegisterInstance(platformFileManagerService).As<IPlatformFileManagerService>().SingleInstance();

            // BL
            builder.RegisterInstance<IRestClient>(new RestClient(Secrets.ApiUrl));
            builder.RegisterInstance(TranslationClient.CreateFromApiKey(Secrets.GoogleApiCloudKey));

            IRestClient googleTextToSpeechClient = new RestClient("https://texttospeech.googleapis.com/v1/text:synthesize");
            googleTextToSpeechClient.AddDefaultHeader("Authorization", $"Bearer {Secrets.GoogleApiAccessToken}");

            builder.RegisterType<GoogleCloudTranslationLogic>().As<IGoogleCloudTranslationLogic>().SingleInstance();
            builder
                .Register(context => new GoogleCloudTextToSpeechLogic(
                    googleTextToSpeechClient,
                    context.Resolve<ExtendedUserContext>(),
                    context.Resolve<IDebuggerService>()))
                .As<IGoogleCloudTextToSpeechLogic>()
                .SingleInstance();

            // ViewModels
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
            pageFactory.Configure<MainTabbedPage, MainTabbedPViewModel>(
                () => Container.Resolve<MainTabbedPViewModel>(),
                new List<Type>
                {
                    typeof(ExercisesPage),
                    typeof(ProfilePage),
                },
                new List<string>
                {
                    "Задания",
                    "Профиль",
                },
                new List<FileImageSource>
                {
                    AppImages.IcBookPng as FileImageSource,
                    AppImages.IcUserPng as FileImageSource,
                });

            pageFactory.Configure<MainPage, MainPViewModel>(() => Container.Resolve<MainPViewModel>());
            pageFactory.Configure<ExercisesPage, ExercisesViewModel>(() => Container.Resolve<ExercisesViewModel>());
            pageFactory.Configure<ProfilePage, ProfileViewModel>(() => Container.Resolve<ProfileViewModel>());
            pageFactory.Configure<AccentSettingPage, AccentSettingViewModel>(() => Container.Resolve<AccentSettingViewModel>());
        }
    }
}
