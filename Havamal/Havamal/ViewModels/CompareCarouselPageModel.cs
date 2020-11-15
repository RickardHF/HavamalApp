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

namespace Havamal.ViewModels
{
    public class CompareCarouselPageModel : BasePageModel
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IVerseRepository _verseRepository;

        private Language _from;

        private Language _to;

        public ObservableCollection<Language> FromLanguages;
        public ObservableCollection<Language> ToLanguages;

        public ObservableCollection<CompareVerseListItem> Comparisons;

        public Language CurrentFromLanguage
        {
            get
            {
                //return _from.MayI<Language>(
                //    yes => yes
                //    , () => new Language(0, AppResources.SelLang, "XX", "")
                //    );
                return _from ?? new Language(0, AppResources.SelLang, "XX", "");
            }
            set
            {
                //_from.SetValue(value);
                _from = value;
                _ = LoadComparisons();
                OnPropertyChanged(nameof(CurrentFromLanguage));
            }
        }
        public Language CurrentToLanguage
        {
            get
            {
                //return _to.MayI<Language>(yes => yes, () => new Language(0, AppResources.SelLang, "XX", ""));
                return _to ?? new Language(0, AppResources.SelLang, "XX", "");
            }
            set
            {
                //_to = DarlingExtensions.Allow(value);
                _to = value;
                _ = LoadComparisons();
                OnPropertyChanged(nameof(CurrentToLanguage));
            }
        }

        public CompareCarouselPageModel(IVerseRepository verseRepository, ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
            _verseRepository = verseRepository;

            FromLanguages = new ObservableCollection<Language>();
            ToLanguages = new ObservableCollection<Language>();

            Comparisons = new ObservableCollection<CompareVerseListItem>();

            LoadLanguages();
        }

        public int GetVersePosision(int verseId)
        {
            var comp = Comparisons.FirstOrDefault(x => x.VerseId == verseId);
            return Comparisons.IndexOf(comp);
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
            try
            {

                var fromLangId = CurrentFromLanguage.Id;
                var toLangId = CurrentToLanguage.Id;
                if(fromLangId <= 0 || toLangId <= 0)
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
                    }, no =>
                    {

                    });
                    OnPropertyChanged(nameof(Comparisons));
                }
            } catch (Exception e)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
