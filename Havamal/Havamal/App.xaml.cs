using Android.Content.Res;
using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Repositories.MockRepositories;
using Havamal.Resources;
using Havamal.Views;
using System;
using Xamarin.Essentials;
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


            var currentTheme = (HavamalTheme)HavamalPreferences.Theme;
            Resources.MergedDictionaries.Add(currentTheme.GetTheme());
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
