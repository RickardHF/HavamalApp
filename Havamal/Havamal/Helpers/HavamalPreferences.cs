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
        public static bool HasSeenInstructions
        {
            get => Preferences.Get("HasSeenInstructions", false);
            set => Preferences.Set("HasSeenInstructions", value);
        }

        public static int SelectedFromLanguage {
            get => Preferences.Get("FromLanguage", -1);
            set => Preferences.Set("FromLanguage", value);
        }

        public static int SelectedToLanguage
        {
            get => Preferences.Get("ToLanguage", -1);
            set => Preferences.Set("ToLanguage", value);
        }

        public static int CurrentVerse
        {
            get => Preferences.Get("CurrentVerse", 1); set => Preferences.Set("CurrentVerse", value);
        }

        public static int SelectedLanguage
        {
            get => Preferences.Get("SelectedLanguage", 1);
            set => Preferences.Set("SelectedLanguage", value);
        }

        public static int Theme
        {
            get => Preferences.Get("Theme", 0);
            set => Preferences.Set("Theme", value);
        }

        public static string AppLanguage
        {
            get => Preferences.Get("AppLanguage", "en");
            set
            {
                UpdateLang(value);
                Preferences.Set("AppLanguage", value.ToLower());
            }
        }

        public static DateTime LastUpdated
        {
            get => Preferences.Get("LastUpdated", DateTime.MinValue);
            set => Preferences.Set("LastUpdated", value);
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

        public static string SetupDbName => "TempVerses";

        public static string DbPassword => "HavmalIsBest";

    }
}
