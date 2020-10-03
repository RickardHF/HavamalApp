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
    public partial class FavoritesPage : ContentPage
    {

        public FavoritesPage()
        {
            InitializeComponent();
            var bindingContext = Startup.ServiceProvider.GetService<FavoritesPageModel>();
            BindingContext = bindingContext;
        }

    }
}
