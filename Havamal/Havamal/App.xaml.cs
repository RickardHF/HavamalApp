using Android.Content.Res;
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
        public App()
        {
            InitializeComponent();

            var initPage = new InitPage();
            MainPage = initPage;
            initPage.SetUpFinished += SetUpFinished;

        }

        private void SetUpFinished(object sender, EventArgs e)
        {
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
