using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Havamal.ViewModels
{
    public class ComparePageModel : BasePageModel
    {

        public ObservableCollection<Verse> FromLanguage;
        public ObservableCollection<Verse> ToLanguage;

        public ObservableCollection<Language> Languages;

        private Darling<Language> _from;
        private Darling<Language> _to;
        public Language CurrentFromLanguage { get
            {
                return _from.HopeForYes();
            }
            set {
                _from.SetValue(value);
                LoadFrom();
            }
        }
        public Language CurrentToLanguage {
            get
            {
                return _to.HopeForYes();
            }
            set
            {
                _to.SetValue(value);
                LoadTo();
            }
        }

        public string FromVerseId { get; set; }
        public string FromVerseContent { get; set; }
        public string FromError { get; set; }

        public string ToVerseId { get; set; }
        public string ToVerseContent{ get; set; }
        public string ToError { get; set; }


        private readonly IVerseRepository _verseRepository;
        private readonly ILanguageRepository _languageRepository;

        public ComparePageModel(IVerseRepository verseRepository, ILanguageRepository languageRepository)
        {
            _verseRepository = verseRepository;
            _languageRepository = languageRepository;

            FromLanguage = new ObservableCollection<Verse>();
            ToLanguage = new ObservableCollection<Verse>();

            LoadLanguages();

        }

        private async void LoadLanguages()
        {
            Languages.Clear();
            var langs = await _languageRepository.Get(new LanguageParameter(), CancellationToken.None).ConfigureAwait(false);
            langs.CanI(yes =>
            {
                foreach(var l in yes)
                {
                    Languages.Add(l);
                }
            }, no =>
            {
                // TODO : Error handling
            });
        }


        private void LoadFrom()
        {

            FromLanguage.Clear();
            FromError = "";
            FromVerseContent = "";
            _from.MayI(async yes =>
            {
                var froms = await _verseRepository.Get(new VerseParameter { Language = yes.Id }, CancellationToken.None).ConfigureAwait(false);
                froms.CanI(yes =>
                {
                    foreach(var fo in yes)
                    {
                        FromLanguage.Add(fo);
                    }
                }, no =>
                {
                    FromError = no.Message;
                });
            }, () =>
            {
                FromError = "Could not gather verses from selected language";
            });

            UpdateFields();
        }

        private void UpdateFields()
        {

        }

        private void LoadTo()
        {

        }
    }
}
