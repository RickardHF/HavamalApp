using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Repositories.MockRepositories;
using Havamal.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal
{
    public partial class App : Application
    {
        public App(VersePage mainPage, AppShell shell)
        {
            InitializeComponent();

            MainPage = new MasterPage();

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }

}
