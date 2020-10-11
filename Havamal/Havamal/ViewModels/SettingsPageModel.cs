using Android.Preferences;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Parameters;
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

        private Language _currentVerseLanguage;
        public Language CurrentVerseLanguage { get {
                return _currentVerseLanguage;
            }
            set {
                _currentVerseLanguage = value;
                OnPropertyChanged(nameof(CurrentVerseLanguage));
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
                var languages = await _languages.Get(null, CancellationToken.None).ConfigureAwait(false);
                languages.CanI(success =>
                {
                    foreach (var lang in success)
                    {
                        Languages.Add(lang);
                    }

                    CurrentVerseLanguage = Languages.FirstOrDefault(x => x.Id == Preferences.Get("SelectedLanguage", 1));
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
