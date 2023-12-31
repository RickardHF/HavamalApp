﻿using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
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
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Havamal.ViewModels
{
    public class FavoritesPageModel : BasePageModel
    {
        private readonly IVerseRepository _verseRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        private readonly SemaphoreSlim _semaphore;

        private string _errMsg;

        private List<Favorite> _favorited;

        public ObservableCollection<Verse> Favorites { get; private set; }

        public Verse SelectedStanza { get; set; }

        public Command LoadDataCommand { get; private set; }

        public Action<Verse> RemoveFavorite { get; private set; } 

        public FavoritesPageModel(IVerseRepository verseRepository, IFavoriteRepository favoriteRepository)
        {
            _verseRepository = verseRepository;
            _favoriteRepository = favoriteRepository;

            Favorites = new ObservableCollection<Verse>();
            _favorited = new List<Favorite>();

            _semaphore = new SemaphoreSlim(1);

            LoadDataCommand = new Command(async () => await LoadFavorites());
            RemoveFavorite = new Action<Verse>(async verse => await RemoveFav(verse));

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

        private async Task RemoveFav(Verse verse)
        {
            try
            {
                _errMsg = "";
                var del = await _favoriteRepository.Delete(new List<Favorite> { new Favorite(verse.VerseId) }, null);
                del.CanI(yes =>
                {
                    _favorited.RemoveAll(x => x.VerseId == verse.VerseId);
                    Favorites.Remove(verse);
                }, no => { });
            } catch (Exception e)
            {
                _errMsg = e.Message;
            }
        }

        private async Task<bool> LoadFavored()
        {
            try
            {
                var favs = await _favoriteRepository
                    .Get(new FavoriteParameter(), CancellationToken.None)
                    .ConfigureAwait(false)
                    ;
                return favs.CanI(success => {
                    _favorited = (List<Favorite>)success;
                    if(! _favorited.Any()) _errMsg = AppResources.NoFavorites;
                    return true;
                }, empty => {
                    _errMsg = AppResources.CouldNotLoadFavs;
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
            await _semaphore.WaitAsync();
            try
            {
                Favorites.Clear();


                var favoriteIds = _favorited.Select(x => x.VerseId).Distinct().ToList();
                var verses = await _verseRepository.Get(new VerseParameter { 
                    Language = new List<int> { HavamalPreferences.SelectedLanguage }
                    , OnIds = true
                    , Ids = favoriteIds
                }, CancellationToken.None).ConfigureAwait(false);

                
                verses.CanI(success =>
                {
                    foreach(var v in success)
                    {
                        Favorites.Add(v);
                    }
                }, fail =>
                {
                    _errMsg = AppResources.CouldNotLoadFavs;
                });
            } catch (Exception e)
            {
                _errMsg = e.Message;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
