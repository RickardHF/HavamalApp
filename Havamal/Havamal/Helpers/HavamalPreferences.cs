using System;
using System.Collections.Generic;
using System.Text;
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

        public static string SetupDbName
        {
            get
            {
                return "TempVerses";
            }
        }
    }
}
