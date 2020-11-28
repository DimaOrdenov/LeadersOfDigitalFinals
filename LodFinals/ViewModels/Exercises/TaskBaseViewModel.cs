using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Schema;
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
using Xamarin.Forms;

namespace LodFinals.ViewModels.Exercises
{
    public class TaskBaseViewModel : PageViewModel
    {
        private readonly IPlatformSpeechToTextService _platformSpeechToTextService;
        private readonly IPlatformFileManagerService _platformFileManagerService;
        private readonly IPlatformAudioPlayerService _platformAudioPlayerService;
        private readonly ILexService _lexService;
        private readonly ISpeechLogic _speechLogic;
        private readonly ExtendedUserContext _userContext;

        private bool _isRecording;
        private string _answer;
        private int _taskNumber;
        private int _taskCount;
        private bool _isTaskDescriptionVisible;
        private bool _isTaskCompleted;
        private string _taskHeader;
        private string _taskDescription;
        private ImageSource _taskImage;
        private string _hoverImageText;
        private bool _isPlaying;

        public ICommand RecordCommand { get; }

        public ICommand PlayCommand { get; }

        public ICommand NextTaskCommand { get; }

        public TaskBaseViewModel(
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
                    IsTaskDescriptionVisible = false;

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

            PlayCommand = BuildPageVmCommand(
                async () =>
                {
                    State = PageStateType.MinorLoading;

                    if (_isPlaying)
                    {
                        await ExceptionHandler.PerformCatchableTask(
                            new ViewModelPerformableAction(
                                 () =>
                                 {
                                     _platformAudioPlayerService.Stop();

                                     IsPlaying = false;

                                     return Task.FromResult(true);
                                 }));
                    }
                    else
                    {
                        await ExceptionHandler.PerformCatchableTask(
                            new ViewModelPerformableAction(
                                 async () =>
                                 {
                                     string response = await _speechLogic.ConvertTextToSpeechAsync(
                                         "When are you leaving?",
                                         userContext.SettingAccent,
                                         CancellationToken);

                                     await File.WriteAllBytesAsync(
                                        Path.Combine(_platformFileManagerService.DownloadDirectory, "sample.mp3"),
                                        Convert.FromBase64String(response));

                                     await _platformAudioPlayerService.PlayAsync(Path.Combine(_platformFileManagerService.DownloadDirectory, "sample.mp3"));

                                     IsPlaying = true;
                                 }));
                    }

                    State = PageStateType.Default;
                });

            NextTaskCommand = BuildPageVmCommand(
                async () =>
                {
                    if (_taskNumber == 4)
                    {
                        await DialogService.DisplayAlert(string.Empty, "Это всё", "Ок");

                        await NavigationService.NavigateBackAsync();

                        return;
                    }

                    TaskNumber++;

                    IsRecording = false;
                    IsTaskDescriptionVisible = true;
                    IsTaskCompleted = false;
                    Answer = null;

                    switch (_taskNumber)
                    {
                        case 2:
                            TaskHeader = "Ответьте на задание";
                            TaskDescription = "Прочитайте внимательно задание. Нажмите на кнопку и расскажите, как вы поступите.";
                            TaskImage = AppImages.ImageTaskTwo;
                            HoverImageText = "Итак, вы в гостинице. Ваш багаж очень тяжелый, попросите консьержа поднять ваши вещи на этаж.";

                            break;
                        case 3:
                            TaskHeader = "Прослушайте вопрос";
                            TaskDescription = "Нажмите на кнопку и внимательно прослушайте вопрос. После ответьте на него.";
                            TaskImage = AppImages.ImageTaskThree;
                            HoverImageText = "Сейчас прозвучит вопрос на тему “Бронирование гостиницы по телефону”";

                            break;
                        case 4:
                            TaskHeader = "Произнеси правильно";
                            TaskDescription = "Прочитайте внимательно задание. Нажмите на кнопку, и произнесите слово в том акценте, который изучаете.";
                            TaskImage = AppImages.ImageTaskFour;
                            HoverImageText = "Ask";
                            Answer = "[æːsk]";

                            break;
                    }
                });

            NavigateBackCommand = BuildPageVmCommand(
                async () =>
                {
                    if (await DialogService.DisplayAlert(string.Empty, "Прекратить выполнение упражнения?", "Да", "Нет"))
                    {
                        await NavigationService.NavigateBackAsync();
                    }
                });

            _taskNumber = 1;
            _taskCount = 4;
            _isTaskDescriptionVisible = true;
            _taskHeader = "Расскажите что на картинке";
            _taskDescription = "Посмотрите внимательно на картинку. Нажмите на кнопку и расскажите на английском, что видите на ней.";
            _taskImage = AppImages.ImageTaskOne;
        }

        public double TaskProgress => _taskNumber / (double)_taskCount;

        public bool IsRecording
        {
            get => _isRecording;
            set
            {
                SetProperty(ref _isRecording, value);
                OnPropertyChanged(nameof(IsPlayButtonVisible));
            }
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                SetProperty(ref _isPlaying, value);
                OnPropertyChanged(nameof(IsRecordButtonVisible));
            }
        }

        public bool IsPlayButtonVisible =>
            _taskNumber == 3 &&
            !_isRecording &&
            !_isTaskCompleted;

        public bool IsRecordButtonVisible =>
            !_isPlaying &&
            !_isTaskCompleted;

        public string Answer
        {
            get => _answer;
            set => SetProperty(ref _answer, value);
        }

        public int TaskNumber
        {
            get => _taskNumber;
            private set
            {
                SetProperty(ref _taskNumber, value);
                OnPropertyChanged(nameof(TaskProgress));
            }
        }

        public int TaskCount
        {
            get => _taskCount;
            private set
            {
                SetProperty(ref _taskCount, value);
                OnPropertyChanged(nameof(TaskProgress));
            }
        }

        public bool IsTaskDescriptionVisible
        {
            get => _isTaskDescriptionVisible;
            set => SetProperty(ref _isTaskDescriptionVisible, value);
        }

        public bool IsTaskCompleted
        {
            get => _isTaskCompleted;
            set
            {
                SetProperty(ref _isTaskCompleted, value);
                OnPropertyChanged(nameof(IsPlayButtonVisible));
                OnPropertyChanged(nameof(IsRecordButtonVisible));
            }
        }

        public string TaskHeader
        {
            get => _taskHeader;
            set => SetProperty(ref _taskHeader, value);
        }

        public string TaskDescription
        {
            get => _taskDescription;
            set => SetProperty(ref _taskDescription, value);
        }

        public ImageSource TaskImage
        {
            get => _taskImage;
            set => SetProperty(ref _taskImage, value);
        }

        public string HoverImageText
        {
            get => _hoverImageText;
            set => SetProperty(ref _hoverImageText, value);
        }

        public override async Task OnAppearing()
        {
            _platformSpeechToTextService.PartialResultsReceived += ServiceSpeechRecognitionPartialResultsReceived;
            _platformSpeechToTextService.Finished += ServiceSpeechRecognitionFinished;
            _platformSpeechToTextService.ErrorOccured += SpeechRecognitionErrorOccured;

            _platformAudioPlayerService.Finished += AudioPlayingFinished;

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
            _platformSpeechToTextService.PartialResultsReceived -= ServiceSpeechRecognitionPartialResultsReceived;
            _platformAudioPlayerService.Finished -= AudioPlayingFinished;

            return base.OnDisappearing();
        }

        private void ServiceSpeechRecognitionFinished(object sender, string e)
        {
            if (_taskNumber != 4)
            {
                Answer = e;
            }

            IsRecording = false;

            IsTaskCompleted = true;
        }

        private void SpeechRecognitionErrorOccured(object sender, string e)
        {
            IsRecording = false;
        }

        private void ServiceSpeechRecognitionPartialResultsReceived(object sender, string e)
        {
            if (_taskNumber != 4)
            {
                Answer = e;
            }
        }

        private void AudioPlayingFinished(object sender, EventArgs e)
        {
            IsPlaying = false;
        }
    }
}
