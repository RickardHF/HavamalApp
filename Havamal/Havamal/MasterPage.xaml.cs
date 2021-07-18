using Havamal.Resources.TextResources;
using Havamal.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : FlyoutPage
    {
        public MasterPage() : this(new NavigationPage(new StanzaCarouselPage() { Title = AppResources.Stanzas }) { Title = AppResources.Stanzas })
        {
        }

        public MasterPage(NavigationPage navPage)
        {
            InitializeComponent();
            masterPageMaster.ListView.ItemSelected -= ListView_ItemSelected;
            masterPageMaster.ListView.ItemSelected += ListView_ItemSelected;

            Detail = navPage;
        }

        public void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is MasterPageMasterMenuItem item))
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;
        }
    }
}