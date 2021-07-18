using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Havamal.Helpers
{
    public static class NavigationHelpers
    {

        public static void GoToPage(Page page, string title = "")
        {
            var main = (MasterPage)App.Current.MainPage;
            var master = (MasterPageMaster)main.Flyout;


            main.Detail = new NavigationPage(page) { Title = title };
            master.ListView.SelectedItem = null;
        }
    }
}
