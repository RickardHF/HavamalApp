using Havamal.Resources.TextResources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Xamarin.Essentials;

namespace Havamal.Helpers
{
    public static class HavamalPreferences
    {
        public static int CurrentVerse { get
            {
                return Preferences.Get("CurrentVerse", 1);
            } set {
                Preferences.Set("CurrentVerse", value);
            }
        }

        public static int SelectedLanguage
        {
            get
            {
                return Preferences.Get("SelectedLanguage", 1);
            }
            set
            {
                Preferences.Set("SelectedLanguage", value);
            }
        }

        public static int Theme
        {
            get
            {
                return Preferences.Get("Theme", 0);
            }
            set
            {
                Preferences.Set("Theme", value);
            }
        }

        public static string AppLanguage
        {
            get
            {
                return Preferences.Get("AppLanguage", "en");
            }
            set
            {
                UpdateLang(value);
                Preferences.Set("AppLanguage", value.ToLower());
            }
        }

        private static void UpdateLang(string code)
        {
            try
            {
                CultureInfo language = new CultureInfo(code);

                Thread.CurrentThread.CurrentUICulture = language;
                AppResources.Culture = language;
            } catch
            {
                // Do something smart
            }
        }

        public static string SetupDbName
        {
            get
            {
                return "TempVerses";
            }
        }

        public static string DbPassword
        {
            get
            {
                return "HavmalIsBest";
            }
        }
    }
}
