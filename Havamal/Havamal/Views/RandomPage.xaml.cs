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
    }
}