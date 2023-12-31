﻿using Havamal.Models.HelperModels;
using Havamal.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StanzaListPage : ContentPage
    {
        private readonly StanzaCarouselPageModel _context;
        public StanzaListPage()
        {
            InitializeComponent();

            var context = Startup.ServiceProvider.GetService<StanzaCarouselPageModel>();
            BindingContext = context;
            _context = context;

            Carousel.PeekAreaInsets = 100;
            Carousel.ItemsSource = _context.Stanzas;
            

            Carousel.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
            {
                SnapPointsAlignment = SnapPointsAlignment.Center
            };

            
            Carousel.IsScrollAnimated = false;

        }
        

        public async void FavoriteClicked(object sender, EventArgs ards)
        {
            // TODO : Check id on item
            var btn = (ImageButton)sender;
            var selItem = (VerseListItem) btn.CommandParameter; 
            await _context.FavoriteClicked(new List<int> { selItem.VerseId });
            btn.Style = _context.CurrentStanza.Favorite;
        }

        public void FavoriteTapped(object sender, EventArgs ards)
        {
            var btn = (AbsoluteLayout)sender;
            var tappedArds = (TappedEventArgs)ards;
            var realBtn = (ImageButton) btn.Children.FirstOrDefault(x => x.GetType() == typeof(ImageButton));
            realBtn.CommandParameter = tappedArds.Parameter;
            FavoriteClicked(realBtn, ards);
        }
    }
}
