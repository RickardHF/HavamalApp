using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Parameters;
using Havamal.SearchHelpers.Filtering;
using Havamal.SearchHelpers.Indexing;
using Havamal.SearchHelpers.Indexing.Helper;
using Java.Security;
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


        public ObservableCollection<Verse> SearchResult { get; private set; }
        public string ResultText { get; private set; }

        public SearchPageModel(IFavoriteRepository favoriteRepository, IVerseRepository verseRepository)
        {
            _favoriteRepository = favoriteRepository;
            _verseRepository = verseRepository;

            ResultText = "Search for verses";

            SearchResult = new ObservableCollection<Verse>();
        }

        public async void LoadData(SearchParameter param)
        {
            IsBusy = true;
            var verseParam = new VerseParameter();

            var indexingParam = param.SearchText.CreateIndexingParameter();

            if (string.IsNullOrEmpty(param.SearchText))
            {
                ResultText = "Search text cannot be empty";
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

            // TODO : Make possible get no lang or other than current
            verseParam.Language = param.LanguageId.HopeForYes();

            var verses = await _verseRepository.Get(verseParam, CancellationToken.None).ConfigureAwait(false);

            verses.CanI(success => {
                var indexedVerses = success.IndexObjects(indexingParam, x => x.Content).FilterOnThresh(0.1);

                if (param.NumericOrder) indexedVerses = indexedVerses.OrderBy(x => x.DataObject.VerseId).ToList();

                SearchResult.Clear();

                if (!indexedVerses.Any()) ResultText = "No verses found";
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
