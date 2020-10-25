using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Havamal.ViewModels
{
    public class FavoritesPageModel : BasePageModel
    {
        private readonly IVerseRepository _verseRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        private string _errMsg;

        private List<Favorite> _favorited;

        public ObservableCollection<Verse> Favorites { get; private set; }

        public Verse SelectedStanza { get; set; }

        public Command LoadDataCommand { get; private set; }
        

        public FavoritesPageModel(IVerseRepository verseRepository, IFavoriteRepository favoriteRepository)
        {
            _verseRepository = verseRepository;
            _favoriteRepository = favoriteRepository;

            Favorites = new ObservableCollection<Verse>();
            _favorited = new List<Favorite>();

            LoadDataCommand = new Command(async () => await LoadFavorites());

            InitPage();
        }

        private async void InitPage()
        {
            await LoadFavorites();
        }

        private async Task LoadFavorites()
        {
            IsBusy = true;
            try
            {
                var didLoadFavs = await LoadFavored().ConfigureAwait(false);
                if (didLoadFavs) LoadFavoriteVerses();

            } catch (Exception e)
            {
                _errMsg = e.Message;
            } finally
            {
                IsBusy = false;
            }
        }

        private async Task<bool> LoadFavored()
        {
            try
            {
                var favs = await _favoriteRepository.Get(new FavoriteParameter(), CancellationToken.None).ConfigureAwait(false);
                return favs.CanI(success => {
                    _favorited = (List<Favorite>)success;
                    if(! _favorited.Any()) _errMsg = "You have no favorites";
                    return true;
                }, empty => {
                    _errMsg = "Could not load favorites";
                    return false;
                });
            } catch (Exception e)
            {
                _errMsg = e.Message;
                return false;
            } 
        }

        private async void LoadFavoriteVerses()
        {
            try
            {
                var favoriteIds = _favorited.Select(x => x.VerseId).Distinct().ToList();
                var verses = await _verseRepository.Get(new VerseParameter { 
                    Language = HavamalPreferences.SelectedLanguage
                    , OnIds = true
                    , Ids = favoriteIds
                }, CancellationToken.None).ConfigureAwait(false);

                Favorites.Clear();
                
                verses.CanI(success =>
                {
                    foreach(var v in success)
                    {
                        Favorites.Add(v);
                    }
                }, fail =>
                {
                    _errMsg = "Could not load favorite verses";
                });
            } catch (Exception e)
            {
                _errMsg = e.Message;
            }
        }
    }
}
