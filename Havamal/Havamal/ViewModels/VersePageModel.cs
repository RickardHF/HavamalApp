using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Havamal.ViewModels
{
    public class VersePageModel : BasePageModel
    {
        private Darling<int> _currentId;
        private List<Favorite> _favorites;
        
        private readonly IVerseRepository _repository;
        private readonly IFavoriteRepository _favoriteRepository;

        public ObservableCollection<Verse> Verses;
        public Command SwipeLeft => new Command(NextVerse);
        public Command SwipeRight => new Command(PreviousVerse);
        public ICommand ClickFavorite => new Command(FavoriteClicked);
        public async void FavoriteClicked(object obj)
        {
            if (_isFavorite)
            {
                var toDelete = _favorites.Where(x => x.VerseId == _verseId).ToList();
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
                var toAdd = new List<Favorite> { new Favorite(_verseId) };
                var addResult = await _favoriteRepository.Create(toAdd, new FavoriteParameter()).ConfigureAwait(false);
                addResult.CanI(success =>
                {
                    _favorites.AddRange(success);
                }, empty => { });
            }

            CheckIsFavorite();
        }

        private string _verseContent { get; set; }
        private int _verseId { get; set; }
        private string _favImgSource;
        private bool _isFavorite;

        public string VerseContent
        {
            get { return _verseContent; }
            private set
            {
                _verseContent = value;
                OnPropertyChanged(nameof(VerseContent));
            }
        }
        public int VerseId { get { return _verseId; } private set
            {
                _verseId = value;
                OnPropertyChanged(nameof(VerseId));
            }
        }

        public string FavoriteImage { 
            get { 
                return _favImgSource; 
            } private set 
            { 
                _favImgSource = value; 
                OnPropertyChanged(nameof(FavoriteImage)); 
            } 
        } 


        private void CheckIsFavorite()
        {
            FavoriteImage = _currentId.MayI(value =>
            {
                if (_favorites == null || !_favorites.Any(x => x.VerseId == value)) return "NotFavorite.png";
                return "favorite.png";
            }, () => "NotFavorite.png");
            _isFavorite = _currentId.MayI(value =>
            {
                if (_favorites == null || !_favorites.Any(x => x.VerseId == value)) return false;
                return true;
            }, () => false);
        }

        public void SetCurrentVerse()
        {
            var verse = _currentId.MayI(some =>
            {
                return Verses.FirstOrDefault(x => x.VerseId == some);
            }, () =>
            {
                return new Verse(0, 0, "Error : Verse not loaded");
            });

            VerseId = verse.VerseId;
            VerseContent = verse.Content;

            CheckIsFavorite();
        }

        public VersePageModel(IVerseRepository repository, IFavoriteRepository favoriteRepository)
        {
            _repository = repository;
            _favoriteRepository = favoriteRepository;
            _currentId = Darling<int>.No();
            Verses = new ObservableCollection<Verse>();
            _favorites = new List<Favorite>();

            InitView();
        }

        private async void InitView()
        {
            IsBusy = true;

            try
            {
                var verses = await _repository.Get(new VerseParameter { Language = Preferences.Get("SelectedLanguage", 1)}, CancellationToken.None).ConfigureAwait(false);

                verses.CanI(success =>
                {
                    Verses.Clear();
                    foreach (var verse in success)
                    {
                        Verses.Add(verse);
                    }
                    _currentId = Darling<int>.Allow(Preferences.Get("CurrentVerse", 1));

                    SetCurrentVerse();
                }, exception =>
                {
                    VerseContent = exception.Message;
                });

                InitFavorites();

            } catch (Exception e){
                VerseContent = e.Message;
            }
            finally {
                IsBusy = false; 
            }
        }

        private async void InitFavorites()
        {
            try
            {
                var favs = await _favoriteRepository.Get(null, CancellationToken.None).ConfigureAwait(false);

                favs.CanI(success =>
               {
                   _favorites = success.ToList();
               }, exception =>
               {
                   throw exception;
               });
            } catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private void NextVerse(object sender)
        {
            _currentId.MayI(some =>
            {
                if (some < Verses.Count())
                {
                    var newCurrentVerse = some + 1;
                    Preferences.Set("CurrentVerse", newCurrentVerse);
                    _currentId.SetValue(newCurrentVerse);
                    SetCurrentVerse();
                }
            }, () =>
            {
            });

        }

        private void PreviousVerse(object sender)
        {
            _currentId.MayI(some =>
            {
                if (some > 1)
                {
                    var newCurrentVerse = some - 1;
                    Preferences.Set("CurrentVerse", newCurrentVerse);
                    _currentId.SetValue(newCurrentVerse);
                    SetCurrentVerse();
                }
            }, () =>
            {
            });
        }
    }
}