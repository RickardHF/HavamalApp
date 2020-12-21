using Havamal.Helpers;
using Havamal.Models;
using Havamal.Resources.TextResources;
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
    public partial class RandomPage : ContentPage
    {
        private readonly RandomPageModel _randomPageModel;
        public RandomPage()
        {
            InitializeComponent();

            _randomPageModel = Startup.ServiceProvider.GetService<RandomPageModel>();

            BindingContext = _randomPageModel;
        }

        private async void GetRandom(object sender, EventArgs e)
        {
            await _randomPageModel.GetRandomVerse();
        }

        private void VerseTapped(object sender, EventArgs e)
        {
            var tappedStanza = (RandomPageModel)((TappedEventArgs)e).Parameter;
            HavamalPreferences.CurrentVerse = tappedStanza.VerseId;

            //var page = (Page)Activator.CreateInstance(typeof(StanzaCarouselPage));
            var page = new StanzaCarouselPage() { Title = AppResources.Stanzas };

            NavigationHelpers.GoToPage(page, AppResources.Stanzas);
        }
    }
}