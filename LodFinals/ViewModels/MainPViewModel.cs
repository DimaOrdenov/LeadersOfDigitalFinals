using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LodFinals.BusinessLayer;
using LodFinals.DependencyServices;
using LodFinals.Helpers;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Definitions;
using NoTryCatch.Xamarin.Portable.Definitions.Enums;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Essentials;

namespace LodFinals.ViewModels
{
    public class MainPViewModel : PageViewModel
    {
        private readonly IGoogleCloudTranslationLogic _googleCloudTranslationLogic;
        private readonly IPlatformSpeechToTextService _platformSpeechToTextService;

        private string _translatedText;

        public ICommand TranslateTextCommand { get; }

        public ICommand RecordCommand { get; }

        public MainPViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            IGoogleCloudTranslationLogic googleCloudTranslationLogic,
            IPlatformSpeechToTextService platformSpeechToTextService)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            _googleCloudTranslationLogic = googleCloudTranslationLogic;
            _platformSpeechToTextService = platformSpeechToTextService;

            TranslateTextCommand = BuildPageVmCommand<string>(
                async textToTranslate =>
                {
                    State = PageStateType.MinorLoading;

                    await ExceptionHandler.PerformCatchableTask(
                        new ViewModelPerformableAction(
                            async () =>
                            {
                                TranslatedText = await _googleCloudTranslationLogic.TranslateTextAsync(textToTranslate, CancellationToken);
                            }));

                    State = PageStateType.Default;
                });

            RecordCommand = BuildPageVmCommand(
                async () =>
                {
                    if (!(await CrossPermissionsExtension.CheckAndRequestPermissionIfNeeded(new Permissions.BasePermission[]
                        {
                            new Permissions.Microphone(),
                            new Permissions.Speech(),
                        })).All(x => x.Value == PermissionStatus.Granted))
                    {
                        DialogService.ShowPlatformShortAlert("Не хватает разрешений на использование микрофона и записи речи");

                        return;
                    }

                    _platformSpeechToTextService.StartSpeechToText();
                });

            _platformSpeechToTextService.SpeechRecognitionFinished += (sender, result) => TranslatedText = result;
        }

        public string TranslatedText
        {
            get => _translatedText;
            private set => SetProperty(ref _translatedText, value);
        }

        public override async Task OnAppearing()
        {
            if (PageDidAppear)
            {
                return;
            }



            await base.OnAppearing();
        }
    }
}
