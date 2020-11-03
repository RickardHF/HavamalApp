using Android.Preferences;
using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using Havamal.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Havamal.ViewModels
{
    public class SettingsPageModel : BasePageModel
    {
        private readonly ILanguageRepository _languages;

        public ObservableCollection<Language> Languages { get; private set; }
        public ObservableCollection<ThemeListItem> Themes { get; private set; }

        private Language _currentVerseLanguage;
        public Language CurrentVerseLanguage { get {
                return _currentVerseLanguage;
            }
            set {
                _currentVerseLanguage = value;
                OnPropertyChanged(nameof(CurrentVerseLanguage));
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

        public SettingsPageModel (ILanguageRepository languages)
        {
            _languages = languages;
            Languages = new ObservableCollection<Language>();

            InitSettings();
        }

        private async void InitSettings()
        {
            IsBusy = true;

            try
            {
                Themes = new ObservableCollection<ThemeListItem> {
                    new ThemeListItem { ThemeId = (int) HavamalTheme.Earth, ThemeName = "Earth", Theme = new EarthTheme()}
                    , new ThemeListItem {ThemeId = (int) HavamalTheme.Light, ThemeName = "Light", Theme = new LightTheme()}
                };

                CurrentTheme = Themes.FirstOrDefault(x => x.ThemeId == HavamalPreferences.Theme);


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
