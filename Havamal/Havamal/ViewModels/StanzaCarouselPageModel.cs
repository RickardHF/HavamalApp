using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Havamal.ViewModels
{
    public class StanzaCarouselPageModel : BasePageModel
    {
        private readonly IVerseRepository _verseRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        public ObservableCollection<VerseListItem> Stanzas;
        private List<Favorite> _favorites;

        public EventHandler ItemsLoaded;

        private VerseListItem _currentStanza { get; set; }
        public VerseListItem CurrentStanza { 
            get { 
                return _currentStanza; 
            } set {
                if (value == null) return;
                _currentStanza = value;
                if (UpdateCurrent) HavamalPreferences.CurrentVerse = value.VerseId;
                InformChange();
            } 
        }
        public  bool UpdateCurrent { get; internal set; }

        public string FavoriteImage { get; set; }

        public Style FavoriteImageStyle { get; set; }

        public int PositionOfVerse(int verseId)
        {
            return Stanzas.IndexOf(x => x.VerseId == verseId);
        }

        private void InformChange()
        {
            OnPropertyChanged(nameof(CurrentStanza));
            OnPropertyChanged(nameof(CurrentStanza.Favorite));
            OnPropertyChanged(nameof(CurrentStanza.Chapter));
            OnPropertyChanged(nameof(CurrentStanza.Content));
            OnPropertyChanged(nameof(CurrentStanza.VerseId));
            OnPropertyChanged(nameof(Chapter));
        }

        public StanzaCarouselPageModel(IVerseRepository verseRepository
            , IFavoriteRepository favoriteRepository)
        {
            _verseRepository = verseRepository;
            _favoriteRepository = favoriteRepository;

            OnPropertyChanged(nameof(FavoriteImage));
            Initialize();
        }
        public string Chapter
        {
            get { return CurrentStanza.VerseId.GetSection().GetSectionString(); }
        }

        public void ChangeSelectedStanza(int verseId)
        {
            var curr = Stanzas.FirstOrDefault(x => x.VerseId == verseId);
            if (curr != null) CurrentStanza = curr;
        }

        public async void Initialize()
        {
            IsBusy = false;
            Stanzas = new ObservableCollection<VerseListItem>();
            _favorites = new List<Favorite>();
            UpdateCurrent = false;
            try
            {
                await FetchFavorites();
                await FetchStanzas();
                SetFavoriteImages();
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
                .Get(new VerseParameter {Language = new List<int> { HavamalPreferences.SelectedLanguage } }, CancellationToken.None)
                .ConfigureAwait(false);

            stanzas.CanI(yes =>
            {
                foreach(var nod in yes)
                {
                    Stanzas.Add(new VerseListItem
                    {
                        VerseId = nod.VerseId
                        , Content = nod.Content
                        , Favorite = _favorites.Any(x => x.VerseId == nod.VerseId)
                                ? (Style)Application.Current.Resources["FavSelected"]
                                : (Style)Application.Current.Resources["FavUnselected"]
                    }); ;
                }
                CurrentStanza = Stanzas.FirstOrDefault(x => x.VerseId == HavamalPreferences.CurrentVerse);
                UpdateCurrent = true;
                OnPropertyChanged(nameof(Stanzas));
            }, no =>
            {

            });
        }

        public async Task FavoriteClicked()
        {
            await FavoriteClicked(CurrentStanza.VerseId);

            SetFavoriteImages();
        }

        private async Task FavoriteClicked(int verseId)
        {
            if(_favorites.Any(x => x.VerseId == verseId))
            {
                var toDelete = _favorites.Where(x => x.VerseId == verseId).ToList();
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
                var toAdd = new List<Favorite> { new Favorite(verseId) };
                var addResult = await _favoriteRepository.Create(toAdd, new FavoriteParameter()).ConfigureAwait(false);
                addResult.CanI(success =>
                {
                    _favorites.AddRange(success);
                }, empty => { });
            }
        }

        public async Task FavoriteClicked(IReadOnlyCollection<int> verseIds)
        {
            foreach(var verse in verseIds)
            {
                await FavoriteClicked(verse);
            }
            SetFavoriteImages();
        }

        private void SetFavoriteImages()
        {
            foreach(var vers in Stanzas)
            {
                vers.Favorite = _favorites.Any(x => x.VerseId == vers.VerseId) 
                    ? (Style)Application.Current.Resources["FavSelected"] 
                    : (Style)Application.Current.Resources["FavUnselected"];
            }

            //CurrentStanza = Stanzas.FirstOrDefault(x => x.VerseId == HavamalPreferences.CurrentVerse);
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
