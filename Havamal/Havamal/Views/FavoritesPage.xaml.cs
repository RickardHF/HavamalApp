using Havamal.Helpers;
using Havamal.Models;
using Havamal.Resources.TextResources;
using Havamal.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesPage : ContentPage
    {
        private readonly Action<Verse> OnRemove;
        public FavoritesPage()
        {
            InitializeComponent();
            var bindingContext = Startup.ServiceProvider.GetService<FavoritesPageModel>();
            BindingContext = bindingContext;
            OnRemove = bindingContext.RemoveFavorite;
        }

        private void FavoriteTapped(object sender, EventArgs e)
        {
            var tappedStanza = (Verse) ((TappedEventArgs)e).Parameter;
            HavamalPreferences.CurrentVerse = tappedStanza.VerseId;

            //var page = (Page) Activator.CreateInstance(typeof(StanzaCarouselPage));
            var page = new StanzaCarouselPage() { Title = AppResources.Stanzas };
            NavigationHelpers.GoToPage(page, AppResources.Stanzas);
        }



        private void Remove_Clicked(object sender, EventArgs e)
        {
            var btn = (Button) sender;
            var verse = (Verse) btn.CommandParameter;

            OnRemove(verse);
        }
    }
}
