using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using Havamal.Resources.TextResources;
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
    public class ComparePageModel : BaseSwipeAction
    {

        private List<Verse> _fromLanguage;
        private List<Verse> _toLanguage;

        private int _verseId { get { return HavamalPreferences.CurrentVerse; } set { HavamalPreferences.CurrentVerse = value; } }

        public ObservableCollection<Language> FromLanguages { get; private set; }
        public ObservableCollection<Language> ToLanguages { get; private set; }

        private Darling<Language> _from;
        private Darling<Language> _to;
        public Language CurrentFromLanguage { get
            {
                return _from.MayI<Language>(yes => yes, () => new Language(0, AppResources.SelLang, "XX", ""));
            }
            set {
                _from.SetValue(value);
                LoadFrom();
                OnPropertyChanged(nameof(CurrentFromLanguage));
            }
        }
        public Language CurrentToLanguage {
            get
            {
                return _to.MayI<Language>(yes => yes, () => new Language(0, AppResources.SelLang, "XX", ""));
            }
            set
            {
                _to.SetValue(value);
                LoadTo();
                OnPropertyChanged(nameof(CurrentToLanguage));
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

            OnVerseIdChange = i => { SetFromContent(); SetToContent(); UpdateFields(); };

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
                        FromLanguages.Add(new Language(l.Id, l.Name, l.LanguageCode, l.Authors));
                    }
                    foreach (var l in yes)
                    {
                        ToLanguages.Add(new Language(l.Id, l.Name, l.LanguageCode, l.Authors));
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
                    _fromLanguage = (List<Verse>) yes;
                    SetFromContent();
                }, no =>
                {
                    FromError = no.Message;
                });
            }, () =>
            {
                FromError = AppResources.CouldNotVersForLang;
            });

            UpdateFrom();
        }

        private void UpdateFields()
        {
            UpdateFrom();
            UpdateTo();
            OnPropertyChanged(nameof(VerseId));
        }

        private void UpdateFrom()
        {
            OnPropertyChanged(nameof(FromError));
            OnPropertyChanged(nameof(FromVerseContent));
        }

        private void UpdateTo()
        {
            OnPropertyChanged(nameof(ToError));
            OnPropertyChanged(nameof(ToVerseContent));
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
                    _toLanguage = (List<Verse>) yes;
                    SetToContent();
                }, no =>
                {
                    ToError = no.Message;
                });
            }, () =>
            {
                ToError = AppResources.CouldNotVersForLang;
            });

            UpdateTo();
        }

        private void SetFromContent()
        {
            var fromverse = DarlingExtensions.Allow(_fromLanguage.FirstOrDefault(x => x.VerseId == _verseId));
            fromverse.MayI(yes => {
                FromVerseContent = yes.Content;
            }, () =>
            {
                FromError = AppResources.CouldNotVersForId;
            });
        }

        private void SetToContent()
        {
            var toverse = DarlingExtensions.Allow(_toLanguage.FirstOrDefault(x => x.VerseId == _verseId));
            toverse.MayI(yes => {
                ToVerseContent = yes.Content;
            }, () =>
            {
                ToError = AppResources.CouldNotVersForId;
            });
        }
    }
}
