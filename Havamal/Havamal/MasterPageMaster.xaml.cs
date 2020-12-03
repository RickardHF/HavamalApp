using Havamal.Helpers;
using Havamal.Resources.TextResources;
using Havamal.Resources.Themes;
using Havamal.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPageMaster : ContentPage
    {
        public ListView ListView;

        public MasterPageMaster()
        {
            InitializeComponent();

            BindingContext = new MasterPageMasterViewModel();
            ListView = MenuItemsListView;
            //ListView.ItemSelected -= PageSelected;
            //ListView.ItemSelected += PageSelected;
        }

        private void PageSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageMasterMenuItem;
            if (item == null)
                return;


            var page = (Page)Startup.ServiceProvider.GetService(item.TargetType);
            page.Title = item.Title;

        }

        public class MasterPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterPageMasterMenuItem> MenuItems { get; set; }

            public MasterPageMasterViewModel()
            {

                var currentTheme = (HavamalTheme)HavamalPreferences.Theme;

                MenuItems = new ObservableCollection<MasterPageMasterMenuItem>(new[]
                {
                    new MasterPageMasterMenuItem { Id = 0, Title = AppResources.Stanzas, TargetType = typeof(StanzaCarouselPage), Icon = currentTheme.GetBookSource() },
                    new MasterPageMasterMenuItem { Id = 1, Title = AppResources.Search, TargetType = typeof(SearchPage), Icon = currentTheme.GetSearchSource()  },
                    new MasterPageMasterMenuItem { Id = 2, Title = AppResources.Random, TargetType = typeof(RandomPage), Icon = currentTheme.GetRandomSource() },
                    new MasterPageMasterMenuItem { Id = 3, Title = AppResources.Compare, TargetType = typeof(CompareCarouselPage), Icon = currentTheme.GetCompareSource() },
                    new MasterPageMasterMenuItem { Id = 4, Title = AppResources.Favorites, TargetType = typeof(FavoritesPage), Icon = currentTheme.GetFavoriteSource() },
                    new MasterPageMasterMenuItem { Id = 5, Title = AppResources.Settings, TargetType = typeof(SettingsPage), Icon = currentTheme.GetSpokesSource()  }
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}