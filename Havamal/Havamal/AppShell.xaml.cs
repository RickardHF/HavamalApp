using Havamal.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell(
            VersePage versePage
            )
        {
            InitializeComponent();
            BindingContext = this;
            Routing.RegisterRoute(nameof(VersePage), typeof(VersePage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }
        public ICommand ToSettings => new Command(async () => await GoToAsync("//SettingsPage"));
    }

}