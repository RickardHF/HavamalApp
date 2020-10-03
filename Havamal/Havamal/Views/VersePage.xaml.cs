using Havamal.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VersePage : ContentPage 
    {
        private Action<object> _onFavClick;
        public VersePage()
        {
            InitializeComponent();
            var bindingContext = Startup.ServiceProvider.GetService<VersePageModel>();
            BindingContext = bindingContext;
            _onFavClick = new Action<object>(bindingContext.FavoriteClicked);
        }

        public void FavoriteClicked(object sender, EventArgs ards)
        {
            _onFavClick(sender);
        }

        public void OnSwipe(object sender, SwipedEventArgs e)
        {
            Debug.WriteLine("Swipe regisertest");
        }
    }
}