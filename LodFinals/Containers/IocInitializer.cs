using System;
using System.IO;
using System.Reflection;
using Autofac;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using Google.Cloud.Translation.V2;
using LodFinals.BusinessLayer;
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
            builder.RegisterType<UserContext>().AsSelf().SingleInstance();
            builder.RegisterType<SpeechToTextService>().As<ISpeechToTextService>().SingleInstance();

            builder.RegisterInstance(platformAlertMessageServiceImplementation).As<IPlatformAlertMessageService>().SingleInstance();
            builder.RegisterInstance(platformSpeechToTextService).As<IPlatformSpeechToTextService>().SingleInstance();
            builder.RegisterInstance(platformAudioPlayerService).As<IPlatformAudioPlayerService>().SingleInstance();
            builder.RegisterInstance(platformFileManagerService).As<IPlatformFileManagerService>().SingleInstance();

            // BL
            builder.RegisterInstance<IRestClient>(new RestClient(Secrets.ApiUrl));
            builder.RegisterInstance(TranslationClient.CreateFromApiKey(Secrets.GoogleApiCloudKey));

            builder.RegisterInstance(
                new TextToSpeechClientBuilder
                {
                    JsonCredentials = "{\"type\":\"service_account\",\"project_id\":\"nth-bucksaw-136423\",\"private_key_id\":\"9142a5ff141ee9510aa1f1a196467fc6212164fc\",\"private_key\":\"-----BEGIN PRIVATE KEY-----\\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCgy/QobE66ykvz\\nuGCk5WIUKW7LRo92bfVciJYoQ0tZjqt0bmXRep0C+ifzwSlaJgtAJmz8D1i5gQnu\\nb2JPinGPVUjjhA/suew0OP5e2FcAJ/OxokNgbnuaXtus3LhjwvmYTU1n/wN4tWHJ\\nUvWeIG3/JEOtON6m5x+xHzdHQfOT9Nr1nJL6UtcHM+jtfRF+oypCJW2NhpKgH6cT\\nAvy9yWJ8lNUEe/ni8bn0mlmDlzoVD+vdZakcOhJb18ih/TrH2666k2hL/fbV/5CH\\nlV5exTe+g12VStvM8dYth/SnI/AINr3f2GBonYhACJ8WrkfrXrUpCjF1cZjrHsDx\\nz3txi3ZBAgMBAAECggEACGn5DLnIz4gREV/EhDG8lXGAQALL3wFB+uWVdeXk5iV2\\nXfv+N/KX5LVSVzRJsFNCo3NfSzvtA0f397y8WDfWWWRiZrAHuqC/9hDwE+ZzfFPw\\nMeCxGzVXd8HJnPIFHrNFJu257zHY7KgRGzJ2x8of918zwLdebz1eMIEsU1OQJghv\\nasyi8tJN2oUU1KKWro+Wrx4ZYPqICTBQ8C+R/Kd+rldGZsaNSY/QN4VQwt4D+nlo\\n/quh9WXwvK/IhuH8GZCjL3oFIQqsE7w5PiBtsUgaLw/oEvke9W/OWwbf31sjrzAq\\nr2SPij8f3GjSreV2Cg3clm7pi14huhehY8dGUw5tQQKBgQDUyuPaUPth3vnCqdSl\\nzYeDK6BNJMtlUn5ZnrX0r16OsZa9SnM5U0UGgpuf6ZNnegco6wwWoFQtg4H/idl7\\nxgLFEhoVkE/cMm9Bk/nTM/C2gn1VQxiCLIfBfEnHnQ8soqCkTx9I32u5Nb2W0stZ\\nJS0c9UQLPGwDv7DjlGFzezLTqwKBgQDBckg3tOS+pp1VUKP+HoZb7/Euz45PqcWS\\nsS1qrVxx0IUaqL5lYU80sHy2mhszHdEpdclmnMBo1FCeZP2zZTnm3NaKcT5xjQv0\\nmU44OlKnUZDYumsjlo2hOJ217i+cAcPxyOqKSZfF0YIK8tsTCmGryc7o8GL2fyKO\\neqSeFXGxwwKBgDQjinThxtaSRexWpI4DsCKJu/tq+pNXBTJ8WuUbWzXneaHj7hur\\ntYwE0JD8MGv4UGzMOugIXNfSEzZD1pAnUmJrLwz9kTPI/823rkz7uw1dJ8yOZiDv\\nPExCYemRKDitmGLqKCY46DI3Yr7j44NyQn2H8yY1pdm9TGBW6RHcq/WFAoGBAIoW\\nSBLr3/Vv/hNukwHNTF/IccPWiWG4cL58F011Hu5nyPv0xKSdx+p2qS+35f6hJDOS\\nTwgekQLDvCC1Opyaf7Cap9L/b3GjG4uo+7sLdwDVKq5qtBwdVlBll77MndOhNvwb\\nTcEVM3AUU9346gU3fe0hC8AgCKSosERAq83kxNn9AoGAKlSUHxROOAZ3fAtKvPMc\\nWvHaHQBn8yzF+FXbB5ThsXoLWbZ22MTJ5/z10F21YUknDkgBkDgkYGlxQ1W8313K\\nMy5oVVI3LjaPisANQJ9Hqf1KRgKQQb80HRPB3BvP1mxTerdr3YfN2C9NKeUBFLsz\\nQ0bMAk6j/JYRjkVGJEt1OeU=\\n-----END PRIVATE KEY-----\\n\",\"client_email\":\"mainserviceaccount@nth-bucksaw-136423.iam.gserviceaccount.com\",\"client_id\":\"102074317172850031391\",\"auth_uri\":\"https://accounts.google.com/o/oauth2/auth\",\"token_uri\":\"https://oauth2.googleapis.com/token\",\"auth_provider_x509_cert_url\":\"https://www.googleapis.com/oauth2/v1/certs\",\"client_x509_cert_url\":\"https://www.googleapis.com/robot/v1/metadata/x509/mainserviceaccount%40nth-bucksaw-136423.iam.gserviceaccount.com\"}",
                }.Build());

            builder.RegisterType<GoogleCloudTranslationLogic>().As<IGoogleCloudTranslationLogic>().SingleInstance();
            builder.RegisterType<GoogleCloudTextToSpeechLogic>().As<IGoogleCloudTextToSpeechLogic>().SingleInstance();

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
            pageFactory.Configure<MainPage, MainPViewModel>(() => Container.Resolve<MainPViewModel>());
        }
    }
}
