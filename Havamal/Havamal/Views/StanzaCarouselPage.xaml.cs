using Havamal.Helpers;
using Havamal.Models.HelperModels;
using Havamal.Resources.TextResources;
using Havamal.ViewModels;
using Havamal.Views.Popups;
using Rg.Plugins.Popup.Extensions;
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

            Carousel.IsScrollAnimated = false;
            Carousel.Loop = false;

            var context = Startup.ServiceProvider.GetService<StanzaCarouselPageModel>();
            
            //Carousel.ItemsSource = context.Stanzas;
            
            context.ChangeCurrent = GoToStanza;

            BindingContext = context;
            _context = context;

            //GoToStanza(HavamalPreferences.CurrentVerse);
            //_context.InformChange();
        }


        public async void FavoriteClicked(object sender, EventArgs ards)
        {
            await _context.FavoriteClicked();
            var btn = (ImageButton)sender;
            btn.Style = _context.CurrentStanza.Favorite;
        }

        public void FavoriteTapped(object sender, EventArgs ards)
        {
            var btn = (AbsoluteLayout)sender;
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

        private void GoToStanza(int i) {
            var pos = _context.PositionOfVerse(i);
            if (pos < 0) return;
            
            _context.UpdateCurrent = false;
            Carousel.Position = pos;
            _context.UpdateCurrent = true;
        }

        private void TapVerseId(object sender, EventArgs e)
        {            
            var page = new VerseChoicePopup(i => {
                GoToStanza(i);
            }, _context.Stanzas.Select(x => x.VerseId).Max());
            Navigation.PushPopupAsync(page);
        }
    }
}