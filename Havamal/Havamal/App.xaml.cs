using Android.Content.Res;
using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Repositories.MockRepositories;
using Havamal.Resources.TextResources;
using Havamal.Resources.Themes;
using Havamal.Views;
using System;
using System.Globalization;
using System.Threading;
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


            CultureInfo language = new CultureInfo(HavamalPreferences.AppLanguage);

            Thread.CurrentThread.CurrentUICulture = language;
            AppResources.Culture = language;

            var currentTheme = (HavamalTheme)HavamalPreferences.Theme;
            Resources.MergedDictionaries.Add(currentTheme.GetTheme());
        }

        private void SetUpFinished(object sender, EventArgs e)
        {
            MainPage = new MasterPage();
        }

        public void Reset(NavigationPage navigationPage)
        {
            MainPage = new MasterPage(navigationPage);
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
