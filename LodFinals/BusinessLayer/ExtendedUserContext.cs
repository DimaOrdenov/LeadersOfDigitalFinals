using System;
using System.Threading.Tasks;
using NoTryCatch.BL.Core;

namespace LodFinals.BusinessLayer
{
    public class ExtendedUserContext : UserContext
    {
        public const string CHOSEN_ACCENT = "ChosenAccent";

        private string _settingAccent;

        public ExtendedUserContext()
        {
            SettingAccent = "en-US";
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

        public async Task TryRestore()
        {
            if (App.Current.Properties.ContainsKey(CHOSEN_ACCENT))
            {
                SettingAccent = App.Current.Properties[CHOSEN_ACCENT] as string;
            }
            else
            {
                App.Current.Properties.Add(CHOSEN_ACCENT, _settingAccent);
            }

            await App.Current.SavePropertiesAsync();
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
    }
}
