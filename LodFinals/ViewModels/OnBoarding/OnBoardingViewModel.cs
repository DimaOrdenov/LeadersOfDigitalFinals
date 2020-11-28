using System;
using System.Windows.Input;
using LodFinals.BusinessLayer;
using LodFinals.Views;
using NoTryCatch.BL.Core;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Definitions.Enums;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Forms;

namespace LodFinals.ViewModels.OnBoarding
{
    public class OnBoardingViewModel : PageViewModel
    {
        private readonly ExtendedUserContext _userContext;

        private int _step = 0;
        private string _entryText;

        public ICommand NextStepCommand { get; }

        public OnBoardingViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            ExtendedUserContext userContext)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            _userContext = userContext;

            NextStepCommand = BuildPageVmCommand(
                async () =>
                {
                    if (_step == 5)
                    {
                        State = PageStateType.MinorLoading;

                        await userContext.SetFirstLaunchCompleted();

                        NavigationService.SetRootTabbedPage<MainTabbedPage>(0);

                        State = PageStateType.Default;

                        return;
                    }

                    if (_step == 1)
                    {
                        await userContext.SetName(_entryText);
                    }

                    _step++;

                    EntryText = null;

                    OnPropertyChanged(nameof(NextButtonText));
                    OnPropertyChanged(nameof(NextButtonBackgroundColor));
                    OnPropertyChanged(nameof(Header));
                    OnPropertyChanged(nameof(Description));
                    OnPropertyChanged(nameof(EntryKeyboard));
                    OnPropertyChanged(nameof(EntryIsPassword));
                    OnPropertyChanged(nameof(IsEntryVisible));
                });
        }

        public string EntryText
        {
            get => _entryText;
            set => SetProperty(ref _entryText, value);
        }

        public string NextButtonText =>
            _step == 0 ?
            "Начать" :
                (_step > 0 && _step < 6) ?
                "Продолжить" :
            "Поехали";

        public Color NextButtonBackgroundColor =>
            _step == 5 ? AppColors.Green : AppColors.Yellow;

        public string Header
        {
            get
            {
                switch (_step)
                {
                    case 0:
                        return "Добро пожаловать";
                    case 1:
                        return "Как тебя зовут?";
                    case 2:
                        return "Возраст";
                    case 3:
                        return "Почта";
                    case 4:
                        return "Пароль";
                    case 5:
                        return "Почти всё!";
                    default:
                        return null;
                }
            }
        }

        public string Description
        {
            get
            {
                switch (_step)
                {
                    case 0:
                        return "Это разговорник, здесь мы научим тебя говорить не только правильно, но и с нужным тебе акцентом.";
                    case 1:
                        return "Чтобы наши уведомления не были безликими - представься.";
                    case 2:
                        return $"{_userContext.Name}, укажи пожалуйста свой возраст";
                    case 3:
                        return "Оставь свою почту, чтобы мы узнали тебя, даже если откроешь приложение с другого устройства.";
                    case 4:
                        return "Осталось совсем немного, введи свой пароль, чтобы не потерять учетку. Не волнуйся, все пароли надежно защищены.";
                    case 5:
                        return "Дело за малым. Пройди пару заданий или начни общаться с ботом, чтобы мы понимали уровень владения языком, это дело пяти минут";
                    default:
                        return null;
                }
            }
        }

        public Keyboard EntryKeyboard
        {
            get
            {
                switch (_step)
                {
                    case 1:
                        return Keyboard.Text;
                    case 2:
                        return Keyboard.Numeric;
                    case 3:
                        return Keyboard.Email;
                    case 4:
                        return Keyboard.Text;
                    default:
                        return Keyboard.Default;
                }
            }
        }

        public bool EntryIsPassword => _step == 4;

        public bool IsEntryVisible => _step != 0 && _step != 5;
    }
}
