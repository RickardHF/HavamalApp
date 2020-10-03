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
        public Language CurrentVerseLanguage { get { 
                return Languages.FirstOrDefault(x => x.Id == Preferences.Get("SelectedLanguage", 1));
            } 
            set {
                Preferences.Set("SelectedLanguage", value.Id);
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
                languages.SuccessOrFail(success =>
                {
                    foreach (var lang in success)
                    {
                        Languages.Add(lang);
                    }
                    //Languages = success.ToList();

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
