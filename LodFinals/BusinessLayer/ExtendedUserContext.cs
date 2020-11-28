using System;
using System.Threading.Tasks;
using NoTryCatch.BL.Core;

namespace LodFinals.BusinessLayer
{
    public class ExtendedUserContext : UserContext
    {
        public const string CHOSEN_ACCENT = "ChosenAccent";
        public const string FIRST_LAUNCH = "FirstLaunch";
        public const string USER_NAME = "UserName";

        private string _settingAccent;
        private bool _isFirstLaunch;
        private string _name;

        public ExtendedUserContext()
        {
            SettingAccent = "en-US";
            _isFirstLaunch = true;
        }

        public event EventHandler UserContextChanged;

        public string SettingAccent
        {
            get => _settingAccent;
            private set
            {
                _settingAccent = value;

                UserContextChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool IsFirstLaunch => _isFirstLaunch;

        public string Name => _name;

        public void TryRestore()
        {
            if (App.Current.Properties.ContainsKey(CHOSEN_ACCENT))
            {
                SettingAccent = App.Current.Properties[CHOSEN_ACCENT] as string;
            }

            if (App.Current.Properties.ContainsKey(FIRST_LAUNCH))
            {
                _isFirstLaunch = App.Current.Properties[FIRST_LAUNCH] as bool? == true;
            }

            if (App.Current.Properties.ContainsKey(USER_NAME))
            {
                _name = App.Current.Properties[USER_NAME] as string;
            }
        }

        public async Task SetAccent(string accent)
        {
            if (App.Current.Properties.ContainsKey(CHOSEN_ACCENT))
            {
                App.Current.Properties[CHOSEN_ACCENT] = accent;
            }
            else
            {
                App.Current.Properties.Add(CHOSEN_ACCENT, accent);
            }

            await App.Current.SavePropertiesAsync();

            SettingAccent = accent;
        }

        public async Task SetFirstLaunchCompleted()
        {
            if (App.Current.Properties.ContainsKey(FIRST_LAUNCH))
            {
                App.Current.Properties[FIRST_LAUNCH] = false;
            }
            else
            {
                App.Current.Properties.Add(FIRST_LAUNCH, false);
            }

            await App.Current.SavePropertiesAsync();

            _isFirstLaunch = false;
        }

        public async Task SetName(string name)
        {
            if (App.Current.Properties.ContainsKey(USER_NAME))
            {
                App.Current.Properties[USER_NAME] = name;
            }
            else
            {
                App.Current.Properties.Add(USER_NAME, name);
            }

            await App.Current.SavePropertiesAsync();

            _name = name;
        }
    }
}
