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
            Carousel.Position = _context.CurrentStanzaIndex;
        }

        public async void FavoriteClicked(object sender, EventArgs ards)
        {
            await _context.FavoriteClicked();
        }
    }
}