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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Havamal.ViewModels
{
    public class CompareCarouselPageModel : BasePageModel
    {
        public Action<int> ChangePos { get; internal set; }

        private readonly ILanguageRepository _languageRepository;
        private readonly IVerseRepository _verseRepository;

        private CompareVerseListItem Current { get; set; }
        public CompareVerseListItem CurrentComparison { 
            get { return Current; } 
            set { 
                Current = value; 
                if (SaveChange && value != null) HavamalPreferences.CurrentVerse = value.VerseId;
            } 
        }


        private Language _from;

        private Language _to;

        public bool SaveChange { get; set; }

        public ObservableCollection<Language> FromLanguages;
        public ObservableCollection<Language> ToLanguages;

        public ObservableCollection<CompareVerseListItem> Comparisons;

        public Language CurrentFromLanguage
        {
            get
            {
                return _from ?? new Language(-1, AppResources.SelLang, "", "");
            }
            set
            {
                SaveChange = false;
                _from = value;
                _ = LoadComparisons();
                OnPropertyChanged(nameof(CurrentFromLanguage));
            }
        }
        public Language CurrentToLanguage
        {
            get
            {
                return _to ?? new Language(-1, AppResources.SelLang, "", "");
            }
            set
            {
                SaveChange = false;
                _to = value;
                _ = LoadComparisons();
                OnPropertyChanged(nameof(CurrentToLanguage));
            }
        }

        public CompareCarouselPageModel(IVerseRepository verseRepository, ILanguageRepository languageRepository)
        {
            SaveChange = false;

            _languageRepository = languageRepository;
            _verseRepository = verseRepository;

            FromLanguages = new ObservableCollection<Language>();
            ToLanguages = new ObservableCollection<Language>();

            Comparisons = new ObservableCollection<CompareVerseListItem>();
            LoadLanguages();
        }

        public void SetComparison(int verseId)
        {
            if (Comparisons.Any())
            {
                var selection = Comparisons.FirstOrDefault(x => x.VerseId == verseId);
                if(selection != null)
                {
                    CurrentComparison = selection;
                    OnPropertyChanged(nameof(CurrentComparison));
                }
            }
            //OnPropertyChanged(nameof(Comparisons));
        }

        public int VersePosition(int verseId)
        {
            return Comparisons.IndexOf(Comparisons.FirstOrDefault(x => x.VerseId == verseId));
        }

        private async void LoadLanguages()
        {
            IsBusy = true;
            FromLanguages.Clear();
            ToLanguages.Clear();
            try
            {
                var langs = await _languageRepository.Get(new LanguageParameter(), CancellationToken.None).ConfigureAwait(false);
                langs.CanI(yes =>
                {
                    foreach (var l in yes)
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadComparisons()
        {
            IsBusy = true;
            Comparisons.Clear();
            SaveChange = false;
            try
            {

                var fromLangId = CurrentFromLanguage.Id;
                var toLangId = CurrentToLanguage.Id;
                if(fromLangId < 0 || toLangId < 0)
                {

                }
                else
                {
                    var param = new VerseParameter { Language = new List<int> { fromLangId, toLangId } };
                    var verses = await _verseRepository.Get(param).ConfigureAwait(false);

                    verses.CanI(yes =>
                    {
                        var ids = yes.Select(x => x.VerseId).Distinct().OrderBy(x => x);
                        foreach(var id in ids)
                        {
                            var comparison = new CompareVerseListItem() { VerseId = id };
                            
                            var fromVerse = yes.FirstOrDefault(x => x.VerseId == id && x.LanguageId == fromLangId);
                            var toVerse = yes.FirstOrDefault(x => x.VerseId == id && x.LanguageId == toLangId);

                            comparison.FromContent = fromVerse?.Content ?? AppResources.CouldNotVersForLang;
                            comparison.ToContent = toVerse?.Content ?? AppResources.CouldNotVersForLang;

                            Comparisons.Add(comparison);
                        }
                        SetComparison(HavamalPreferences.CurrentVerse);
                        ChangePos(HavamalPreferences.CurrentVerse);
                        SaveChange = true;
                    }, no =>
                    {

                    });
                    OnPropertyChanged(nameof(Comparisons));
                }
            } catch (Exception)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
