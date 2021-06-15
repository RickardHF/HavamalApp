using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Parameters;
using Havamal.Resources.TextResources;
using Havamal.SearchHelpers.Filtering;
using Havamal.SearchHelpers.Indexing;
using Havamal.SearchHelpers.Indexing.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Havamal.ViewModels
{
    public class SearchPageModel : BasePageModel
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IVerseRepository _verseRepository;
        private readonly ILanguageRepository _languageRepository;


        public ObservableCollection<Verse> SearchResult { get; private set; }
        public string ResultText { get; private set; }

        public SearchPageModel(IFavoriteRepository favoriteRepository
            , IVerseRepository verseRepository
            , ILanguageRepository languageRepository
            )
        {
            _favoriteRepository = favoriteRepository;
            _verseRepository = verseRepository;
            _languageRepository = languageRepository;


            SearchResult = new ObservableCollection<Verse>();
        }

        public async void LoadData(SearchParameter param)
        {
            IsBusy = true;
            var verseParam = new VerseParameter();

            var indexingParam = param.SearchText.CreateIndexingParameter();

            if (string.IsNullOrEmpty(param.SearchText))
            {
                ResultText = AppResources.SearchEmpty;
                SearchResult.Clear();
                OnPropertyChanged(nameof(ResultText));
                return;
            }

            ResultText = "";

            if (param.OnlyFavorites)
            {
                var favs = await _favoriteRepository.Get(new FavoriteParameter(), CancellationToken.None).ConfigureAwait(false);
                favs.CanI(success =>
                {
                    verseParam.Ids = success.Select(x => x.VerseId).Distinct().ToList();
                    verseParam.OnIds = true;
                }, fail =>
                {
                    ResultText = fail.Message;
                    SearchResult = new ObservableCollection<Verse>();
                    OnPropertyChanged(nameof(ResultText));
                    return;
                });
            }

            var langs = new List<int> { HavamalPreferences.SelectedLanguage };
            if (param.AllLanguages)
            {
                var allLangs = await _languageRepository.Get(new LanguageParameter()).ConfigureAwait(false);
                allLangs.CanI(yes => {
                    langs = yes.Select(x => x.Id).ToList();
                }, no =>
                {
                    ResultText = no.Message;
                    SearchResult = new ObservableCollection<Verse>();
                    OnPropertyChanged(nameof(ResultText));
                    return;
                });
            }
            verseParam.Language = langs;

            var verses = await _verseRepository.Get(verseParam, CancellationToken.None).ConfigureAwait(false);

            verses.CanI(success => {
                var indexedVerses = success.IndexObjects(indexingParam, x => x.Content).FilterOnThresh(0.1);

                if (param.NumericOrder) indexedVerses = indexedVerses.OrderBy(x => x.DataObject.VerseId).ToList();

                SearchResult.Clear();

                if (!indexedVerses.Any()) ResultText = AppResources.SearchNotFound;
                foreach(var verse in indexedVerses)
                {
                    SearchResult.Add(verse.DataObject);
                }
            }, fail =>
            {
                ResultText = fail.Message;
                SearchResult.Clear();
                OnPropertyChanged(nameof(ResultText));
                return;
            });

            OnPropertyChanged(nameof(ResultText));

            IsBusy = false;
        }

    }
}
