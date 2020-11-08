using Havamal.Models.HelperModels;
using Havamal.Resources.TextResources;
using Havamal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var item = (VerseListItem)((ImageButton)sender).CommandParameter;
            await Share.RequestAsync(new ShareTextRequest
            {
                Title = Title,
                Text = $"{item.VerseId}\n{item.Content}\n  - {AppResources.Havamal}",
                PresentationSourceBounds = DeviceInfo.Platform == DevicePlatform.iOS && DeviceInfo.Idiom == DeviceIdiom.Tablet
                            ? new System.Drawing.Rectangle(0, 20, 0, 0)
                            : System.Drawing.Rectangle.Empty
            });;
        }
    }
}