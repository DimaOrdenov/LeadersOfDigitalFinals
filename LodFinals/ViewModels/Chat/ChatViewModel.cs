using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LodFinals.BusinessLayer;
using LodFinals.DependencyServices;
using LodFinals.Services;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Definitions;
using NoTryCatch.Xamarin.Portable.Definitions.Enums;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Essentials;

namespace LodFinals.ViewModels.Chat
{
    public class ChatViewModel : PageViewModel
    {
        private readonly IPlatformSpeechToTextService _platformSpeechToTextService;
        private readonly IPlatformFileManagerService _platformFileManagerService;
        private readonly IPlatformAudioPlayerService _platformAudioPlayerService;
        private readonly ILexService _lexService;
        private readonly ISpeechLogic _speechLogic;
        private readonly ExtendedUserContext _userContext;

        private bool _isRecording;
        private ObservableCollection<ChatItemViewModel> _messagesCollection;
        private string _todayText;

        public ICommand RecordCommand { get; }

        public ChatViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            IPlatformSpeechToTextService platformSpeechToTextService,
            IPlatformFileManagerService platformFileManagerService,
            IPlatformAudioPlayerService platformAudioPlayerService,
            ILexService lexService,
            ISpeechLogic speechLogic,
            ExtendedUserContext userContext)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            _platformSpeechToTextService = platformSpeechToTextService;
            _platformFileManagerService = platformFileManagerService;
            _platformAudioPlayerService = platformAudioPlayerService;
            _lexService = lexService;
            _speechLogic = speechLogic;
            _userContext = userContext;

            RecordCommand = BuildPageVmCommand(
                async () =>
                {
                    if (IsRecording)
                    {
                        await ExceptionHandler.PerformCatchableTask(
                            new ViewModelPerformableAction(
                                 () =>
                                 {
                                     _platformSpeechToTextService.StopSpeechToText();

                                     IsRecording = false;

                                     return Task.FromResult(true);
                                 }));
                    }
                    else
                    {
                        if (!(await CrossPermissionsExtension.CheckAndRequestPermissionIfNeeded(new Permissions.BasePermission[]
                            {
                                new Permissions.Microphone(),
                                new Permissions.Speech(),
                            })).All(x => x.Value == PermissionStatus.Granted))
                        {
                            DialogService.ShowPlatformShortAlert("Нужны разрешения на использование микрофона и записи речи");

                            return;
                        }

                        await ExceptionHandler.PerformCatchableTask(
                            new ViewModelPerformableAction(
                                 () =>
                                 {
                                     _platformSpeechToTextService.StartSpeechToText();

                                     IsRecording = true;

                                     return Task.FromResult(true);
                                 }));
                    }
                });

            _messagesCollection = new ObservableCollection<ChatItemViewModel>();
            _messagesCollection.CollectionChanged += MessagesCollectionChanged;
        }

        private void MessagesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _messagesCollection.CollectionChanged -= MessagesCollectionChanged;

            TodayText = $"Сегодня, {DateTime.Now:HH:mm}";
        }

        public bool IsRecording
        {
            get => _isRecording;
            set => SetProperty(ref _isRecording, value);
        }

        public ObservableCollection<ChatItemViewModel> MessagesCollection
        {
            get => _messagesCollection;
            set => SetProperty(ref _messagesCollection, value);
        }

        public string TodayText
        {
            get => _todayText;
            set => SetProperty(ref _todayText, value);
        }

        public override async Task OnAppearing()
        {
            _platformSpeechToTextService.Finished += ServiceSpeechRecognitionFinished;
            _platformSpeechToTextService.ErrorOccured += SpeechRecognitionErrorOccured;

            if (!(await CrossPermissionsExtension.CheckAndRequestPermissionIfNeeded(new Permissions.BasePermission[]
                {
                    new Permissions.StorageRead(),
                    new Permissions.StorageRead(),
                    new Permissions.Microphone(),
                    new Permissions.Speech(),
                })).All(x => x.Value == PermissionStatus.Granted))
            {
                DialogService.ShowPlatformShortAlert("Нужны доступы к файловой системе, для использования микрофона и для записи речи");

                await NavigationService.NavigateBackAsync();
            }

            await base.OnAppearing();
        }

        public override Task OnDisappearing()
        {
            _platformSpeechToTextService.Finished -= ServiceSpeechRecognitionFinished;
            _platformSpeechToTextService.ErrorOccured -= SpeechRecognitionErrorOccured;

            return base.OnDisappearing();
        }

        private async void ServiceSpeechRecognitionFinished(object sender, string e)
        {
            MessagesCollection.Add(new ChatItemViewModel(e, true));

            State = PageStateType.MinorLoading;

            await ExceptionHandler.PerformCatchableTask(
                new ViewModelPerformableAction(
                    async () =>
                    {
                        string response = await _lexService.Conversation(e);

                        try
                        {
                            await File.WriteAllBytesAsync(
                                Path.Combine(_platformFileManagerService.DownloadDirectory, "sample.mp3"),
                                Convert.FromBase64String(await _speechLogic.ConvertTextToSpeechAsync(response, _userContext.SettingAccent, CancellationToken)));

                            await _platformAudioPlayerService.Play(Path.Combine(_platformFileManagerService.DownloadDirectory, "sample.mp3"));
                        }
                        finally
                        {
                            MessagesCollection.Add(new ChatItemViewModel(response, false));
                        }
                    }));

            IsRecording = false;

            State = PageStateType.Default;
        }

        private void SpeechRecognitionErrorOccured(object sender, string e)
        {
            IsRecording = false;
        }
    }
}
