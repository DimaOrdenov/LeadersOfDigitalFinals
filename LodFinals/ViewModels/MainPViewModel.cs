using System;
using System.Collections.ObjectModel;
using System.IO;
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
        private readonly IGoogleCloudTextToSpeechLogic _googleCloudTextToSpeechLogic;
        private readonly IPlatformSpeechToTextService _platformSpeechToTextService;

        private string _translatedText;
        private bool _isRecording;

        public ICommand TranslateTextCommand { get; }

        public ICommand RecordCommand { get; }

        public ICommand ReadCommand { get; }

        public MainPViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            IGoogleCloudTranslationLogic googleCloudTranslationLogic,
            IGoogleCloudTextToSpeechLogic googleCloudTextToSpeechLogic,
            IPlatformSpeechToTextService platformSpeechToTextService,
            IPlatformAudioPlayerService platformAudioPlayerService,
            IPlatformFileManagerService platformFileManagerService)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            _googleCloudTranslationLogic = googleCloudTranslationLogic;
            _googleCloudTextToSpeechLogic = googleCloudTextToSpeechLogic;
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
                    if (_isRecording)
                    {
                        _platformSpeechToTextService.StopSpeechToText();

                        _isRecording = false;
                    }
                    else
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

                        _isRecording = true;
                    }
                });

            ReadCommand = BuildPageVmCommand<string>(
                async textToRead =>
                {
                    if (!(await CrossPermissionsExtension.CheckAndRequestPermissionIfNeeded(new Permissions.BasePermission[]
                        {
                            new Permissions.StorageRead(),
                            new Permissions.StorageRead(),
                        })).All(x => x.Value == PermissionStatus.Granted))
                    {
                        DialogService.ShowPlatformShortAlert("Нужен доступ к файловой системе");
                        return;
                    }

                    State = PageStateType.MinorLoading;

                    using FileStream stream = await _googleCloudTextToSpeechLogic.TextToSpeechAsync(textToRead, CancellationToken);

                    await platformAudioPlayerService.Play(Path.Combine(platformFileManagerService.DownloadDirectory, "sample.mp3"));

                    State = PageStateType.Default;
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
