﻿using Havamal.Views;
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
            ListView.ItemSelected += PageSelected;
        }

        private void PageSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageMasterMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            
        }

        class MasterPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterPageMasterMenuItem> MenuItems { get; set; }

            public MasterPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MasterPageMasterMenuItem>(new[]
                {
                    new MasterPageMasterMenuItem { Id = 0, Title = "Verses", TargetType = typeof(VersePage) },
                    new MasterPageMasterMenuItem { Id = 1, Title = "Search", TargetType = typeof(SearchPage) },
                    new MasterPageMasterMenuItem { Id = 2, Title = "Favorites", TargetType = typeof(FavoritesPage) },
                    new MasterPageMasterMenuItem { Id = 3, Title = "Settings", TargetType = typeof(SettingsPage) }
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