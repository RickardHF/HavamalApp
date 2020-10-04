using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Havamal.ViewModels
{
    public class ComparePageModel : BasePageModel
    {

        private List<Verse> _fromLanguage;
        private List<Verse> _toLanguage;

        private int _verseId { get { return Preferences.Get("CurrentVerse", 1); } set { Preferences.Set("CurrentVerse", value); } }

        public ObservableCollection<Language> FromLanguages { get; private set; }
        public ObservableCollection<Language> ToLanguages { get; private set; }

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

        public int VerseId
        {
            get { return _verseId; }
            private set
            {
                _verseId = value;
                OnPropertyChanged(nameof(VerseId));
            }
        }

        public string FromVerseContent { get; set; }
        public string FromError { get; set; }

        public string ToVerseContent{ get; set; }
        public string ToError { get; set; }


        private readonly IVerseRepository _verseRepository;
        private readonly ILanguageRepository _languageRepository;

        public ComparePageModel(IVerseRepository verseRepository, ILanguageRepository languageRepository)
        {
            _verseRepository = verseRepository;
            _languageRepository = languageRepository;

            FromLanguages = new ObservableCollection<Language>();
            ToLanguages = new ObservableCollection<Language>();

            _fromLanguage = new List<Verse>();
            _toLanguage = new List<Verse>();

            LoadLanguages();
        }


        private async void LoadLanguages()
        {
            IsBusy = true;
            try
            {
                var langs = await _languageRepository.Get(new LanguageParameter(), CancellationToken.None).ConfigureAwait(false);
                langs.CanI(yes =>
                {
                    foreach(var l in yes)
                    {
                        FromLanguages.Add(l);
                        ToLanguages.Add(l);
                    }
                }, no =>
                {
                    // TODO : Error handling
                });
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }


        private void LoadFrom()
        {

            _fromLanguage.Clear();
            FromError = "";
            FromVerseContent = "";
            _from.MayI(async yes =>
            {
                var froms = await _verseRepository.Get(new VerseParameter { Language = yes.Id }, CancellationToken.None).ConfigureAwait(false);
                froms.CanI(yes =>
                {
                    foreach(var fo in yes)
                    {
                        _fromLanguage.Add(fo);
                    }
                    SetFromContent();
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
            OnPropertyChanged(FromError);
            OnPropertyChanged(FromVerseContent);
            OnPropertyChanged(ToError);
            OnPropertyChanged(ToVerseContent);
        }

        private void LoadTo()
        {
            _toLanguage.Clear();
            ToError = "";
            ToVerseContent = "";
            _from.MayI(async yes =>
            {
                var froms = await _verseRepository.Get(new VerseParameter { Language = yes.Id }, CancellationToken.None).ConfigureAwait(false);
                froms.CanI(yes =>
                {
                    foreach (var fo in yes)
                    {
                        _toLanguage.Add(fo);
                    }
                    SetToContent();
                }, no =>
                {
                    ToError = no.Message;
                });
            }, () =>
            {
                ToError = "Could not gather verses from selected language";
            });

            UpdateFields();
        }

        private void SetFromContent()
        {
            var fromverse = Darling<Verse>.Allow(_fromLanguage.FirstOrDefault(x => x.VerseId == _verseId));
            fromverse.MayI(yes => {
                FromVerseContent = yes.Content;
            }, () =>
            {
                FromError = "Could not find verse for this language at this Id";
            });
        }

        private void SetToContent()
        {
            var toverse = Darling<Verse>.Allow(_toLanguage.FirstOrDefault(x => x.VerseId == _verseId));
            toverse.MayI(yes => {
                ToVerseContent = yes.Content;
            }, () =>
            {
                ToError = "Could not find verse for this language at this Id";
            });
        }
    }
}
