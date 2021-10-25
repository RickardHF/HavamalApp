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
    public partial class FirstTimePage : ContentPage
    {
        public FirstTimePage()
        {
            InitializeComponent();

            var context = Startup.ServiceProvider.GetService<FirstTimePageModel>();

            BindingContext = context;
        }
    }
}