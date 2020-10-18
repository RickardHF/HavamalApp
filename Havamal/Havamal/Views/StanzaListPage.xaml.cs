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
            Carousel.Position = _context.CurrentStanzaIndex;
        }
    }
}
