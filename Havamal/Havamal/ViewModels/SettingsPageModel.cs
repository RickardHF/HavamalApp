using Havamal.Helpers;
using Havamal.Interfaces.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using Havamal.Resources;
using Havamal.Resources.TextResources;
using Havamal.Resources.Themes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Havamal.ViewModels
{
    public class SettingsPageModel : BasePageModel
    {
        private readonly ILanguageRepository _languages;

        public readonly IThemeChanger ThemeChanger;

        public ICommand ThemeClicked { get; set; }

        public ObservableCollection<Language> Languages { get; private set; }
        public ObservableCollection<ThemeListItem> Themes { get; private set; }

        public ObservableCollection<Language> AppLanguages { get; private set; }

        private Language _currentVerseLanguage;
        public Language CurrentVerseLanguage { get {
                return _currentVerseLanguage;
            }
            set {
                _currentVerseLanguage = value;
                OnPropertyChanged(nameof(CurrentVerseLanguage));
            }
        }

        private Language _currentAppLanguage;
        public Language CurrentAppLanguage
        {
            get
            {
                return _currentAppLanguage;
            }
            set
            {
                _currentAppLanguage = value;
                OnPropertyChanged(nameof(CurrentAppLanguage));
            }
        }

        private ThemeListItem _currentTheme;

        public ThemeListItem CurrentTheme
        {
            get
            {
                return _currentTheme;
            }
            set
            {
                _currentTheme = value;
                OnPropertyChanged(nameof(CurrentTheme));
            }
        }

        public SettingsPageModel (ILanguageRepository languages, IThemeChanger themeChanger)
        {
            _languages = languages;
            Languages = new ObservableCollection<Language>();
            ThemeChanger = themeChanger;

            ThemeClicked = new Command(() => { });

            InitSettings();
        }

        private async void InitSettings()
        {
            IsBusy = true;

            try
            {
                Themes = new ObservableCollection<ThemeListItem> {
                    new ThemeListItem { ThemeId = (int) HavamalTheme.Earth, ThemeName = AppResources.ThemeEarth, Theme = new EarthTheme(), ThemeImage = "odelEARTH.png"}
                    , new ThemeListItem {ThemeId = (int) HavamalTheme.Light, ThemeName = AppResources.ThemeLight, Theme = new LightTheme(), ThemeImage = "odelLIGHT.png"}
                    , new ThemeListItem { ThemeId = (int) HavamalTheme.Water, ThemeName = AppResources.ThemeWater, Theme = new WaterTheme(), ThemeImage = "odelWATER.png"}
                    , new ThemeListItem { ThemeId = (int) HavamalTheme.Dark, ThemeName = AppResources.ThemeDark, Theme = new DarkTheme(), ThemeImage = "odelDARK.png"}
                };

                CurrentTheme = Themes.FirstOrDefault(x => x.ThemeId == HavamalPreferences.Theme);


                AppLanguages = new ObservableCollection<Language>
                {
                    new Language(0, "Norsk", "no", "")
                    , new Language(1, "English", "en", "")
                    , new Language(2, "Český", "cs", "")
                    , new Language(3, "Русский", "ru", "Эрмек Акелов")
                    , new Language(4, "Nederlans", "nl", "Tjark Roozendaal")
                    , new Language(5, "Македонски", "mk", "Филип Џамбазоски")
                    , new Language(6, "Española", "es", "Josep Rebés")
                    , new Language(7, "Català", "ca", "Josep Rebés")
                    //, new Language(6, "Deutsh - placeholder", "de", "")
                    //, new Language(7, "Spanish - placeholder", "es", "")
                    //, new Language(8, "Kyrgiz - placeholder", "kg", "")
                };

                CurrentAppLanguage = AppLanguages.FirstOrDefault(x => x.LanguageCode.Equals(HavamalPreferences.AppLanguage));


                var languages = await _languages.Get(null, CancellationToken.None).ConfigureAwait(false);
                languages.CanI(success =>
                {
                    foreach (var lang in success)
                    {
                        Languages.Add(lang);
                    }

                    CurrentVerseLanguage = Languages.FirstOrDefault(x => x.Id == HavamalPreferences.SelectedLanguage);
                }, empty =>
                {
                    throw empty;
                });

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
