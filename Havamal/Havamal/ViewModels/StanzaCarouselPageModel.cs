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
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace Havamal.ViewModels
{
    public class StanzaCarouselPageModel : BasePageModel
    {
        private readonly IVerseRepository _verseRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        public ObservableCollection<Verse> Stanzas;
        private List<Favorite> _favorites;

        public Verse CurrentStanza { get; private set; }

        public string FavoriteImage { get; set; }

        public StanzaCarouselPageModel(IVerseRepository verseRepository
            , IFavoriteRepository favoriteRepository)
        {
            _verseRepository = verseRepository;
            _favoriteRepository = favoriteRepository;



            OnPropertyChanged(nameof(FavoriteImage));
            Initialize();
        }
        internal int CurrentStanzaIndex
        {
            get {
                return Stanzas.IndexOf(CurrentStanza); 
            }
        }

        internal void StanzaChanged(object sender, Xamarin.Forms.CurrentItemChangedEventArgs e)
        {
            var newStanza = (Verse) e.CurrentItem;
            if (newStanza == null) return;
            CurrentStanza = newStanza;
            HavamalPreferences.CurrentVerse = newStanza.VerseId;

            SetFavoriteImage();
        }

        public async void Initialize()
        {
            IsBusy = false;
            Stanzas = new ObservableCollection<Verse>();
            _favorites = new List<Favorite>();
            try
            {
                await FetchStanzas();
                await FetchFavorites();
                SetFavoriteImage();
            }
            catch
            {

            }
            finally
            {
                IsBusy = true;
            }
        }

        private async Task FetchStanzas()
        {
            Stanzas.Clear();
            var stanzas = await _verseRepository
                .Get(new VerseParameter {Language = HavamalPreferences.SelectedLanguage }, CancellationToken.None)
                .ConfigureAwait(false);
            stanzas.CanI(yes =>
            {
                foreach(var nod in yes)
                {
                    Stanzas.Add(nod);
                }
                CurrentStanza = Stanzas.FirstOrDefault(x => x.VerseId == HavamalPreferences.CurrentVerse);
            }, no =>
            {

            });
        }

        public async Task FavoriteClicked()
        {
            if(_favorites.Any(x => x.VerseId == CurrentStanza.VerseId))
            {
                var toDelete = _favorites.Where(x => x.VerseId == CurrentStanza.VerseId).ToList();
                var didDelete = await _favoriteRepository.Delete(toDelete, new FavoriteParameter()).ConfigureAwait(false);
                didDelete.CanI(success =>
                {
                    if (success)
                    {
                        foreach (var toDel in toDelete)
                        {
                            _favorites.Remove(toDel);
                        }
                    }
                }, empty => { });
            } else
            {
                var toAdd = new List<Favorite> { new Favorite(CurrentStanza.VerseId) };
                var addResult = await _favoriteRepository.Create(toAdd, new FavoriteParameter()).ConfigureAwait(false);
                addResult.CanI(success =>
                {
                    _favorites.AddRange(success);
                }, empty => { });
            }

            SetFavoriteImage();
        }

        private void SetFavoriteImage()
        {

            if (_favorites.Any(x => x.VerseId == CurrentStanza.VerseId)) FavoriteImage = "favorite.png";
            else FavoriteImage = "NotFavorite.png";

            OnPropertyChanged(nameof(FavoriteImage));
        }

        private async Task FetchFavorites()
        {
            _favorites.Clear();

            var favs = await _favoriteRepository
                .Get(new FavoriteParameter { }, CancellationToken.None)
                .ConfigureAwait(false);

            favs.CanI(yes => {
                _favorites.AddRange(yes);
            }, no => { 

            });
        }

    }
}
