using Android.Print;
using Havamal.ViewModels;
using System;
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
                ItemSpacing = 0
            };

            SetStart();
        }

        private void SetStart()
        {
            var startPos = _context.CurrentStanzaIndex;
            if (startPos >= 0) Carousel.Position = startPos;
        }

        public async void FavoriteClicked(object sender, EventArgs ards)
        {
            // TODO : Check id on item
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
