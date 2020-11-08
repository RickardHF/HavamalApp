using Havamal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StanzaCarouselPage : ContentPage
    {
        private readonly StanzaCarouselPageModel _context;
        public StanzaCarouselPage()
        {
            InitializeComponent();
            var context = Startup.ServiceProvider.GetService<StanzaCarouselPageModel>();

            BindingContext = context;
            _context = context;

            Carousel.ItemsSource = context.Stanzas;
            Carousel.CurrentItemChanged += context.StanzaChanged;

            SetStart();
        }

        private void SetStart()
        {
            var startPos = _context.CurrentStanzaIndex;
            if (startPos >= 0) Carousel.Position = startPos;
        }

        public async void FavoriteClicked(object sender, EventArgs ards)
        {
            await _context.FavoriteClicked();
            var btn = (ImageButton)sender;
            btn.Style = _context.CurrentStanza.Favorite;
        }

        public void FavoriteTapped(object sender, EventArgs ards)
        {
            var btn = (StackLayout)sender;
            var realBtn = btn.Children.FirstOrDefault(x => x.GetType() == typeof(ImageButton));
            FavoriteClicked(realBtn, ards);
        }
    }
}