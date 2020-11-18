using Havamal.Helpers;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
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
    public partial class SearchPage : ContentPage
    {
        private readonly SearchPageModel _pageModel;

        public SearchPage()
        {
            InitializeComponent();

            var bindingContext = Startup.ServiceProvider.GetService<SearchPageModel>();
            _pageModel = bindingContext;
            BindingContext = bindingContext;
        }

        private void Search_Clicked(object sender, EventArgs e)
        {
            var searchText = SearchText.Text;
            var onlyFavs = OnlyFavs.IsChecked;
            var numericOrder = NumericOrder.IsChecked;
            var allLang = AllLAng.IsChecked;

            var searchParam = new SearchParameter
            {
                SearchText = searchText,
                OnlyFavorites = onlyFavs,
                NumericOrder = numericOrder,
                AllLanguages = allLang
             };

            _pageModel.LoadData(searchParam);
        }

        private void Checkbox_Changed(object sender, CheckedChangedEventArgs e)
        {
            Search_Clicked(sender, e);
        }

        private void Advanced_Clicked(object sender, EventArgs e)
        {
            var btn = (ImageButton)sender;
            if (AdvancedSettings.IsVisible)
            {
                AdvancedSettings.IsVisible = false;
                OnlyFavs.IsChecked = false;
                NumericOrder.IsChecked = false;
                AllLAng.IsChecked = false;
                btn.Style = (Style)Application.Current.Resources["ShowBtn"];
            } else
            {
                AdvancedSettings.IsVisible = true;
                btn.Style = (Style)Application.Current.Resources["HideBtn"];
            }
        }

        private void VerseTapped(object sender, EventArgs e)
        {
            var tappedStanza = (Verse)((TappedEventArgs)e).Parameter;
            HavamalPreferences.CurrentVerse = tappedStanza.VerseId;

            var page = (Page)Activator.CreateInstance(typeof(StanzaPage));

            NavigationHelpers.GoToPage(page, AppResources.Stanzas);
        }
    }
}