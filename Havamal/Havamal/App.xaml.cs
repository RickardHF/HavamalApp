using Android.Content.Res;
using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Repositories.MockRepositories;
using Havamal.Resources.TextResources;
using Havamal.Resources.Themes;
using Havamal.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal
{
    public partial class App : Application
    {
        private readonly Stack<Page> _pageStack;
        public App()
        {
            InitializeComponent();

            _pageStack = new Stack<Page>();

            CultureInfo language = new CultureInfo(HavamalPreferences.AppLanguage);

            Thread.CurrentThread.CurrentUICulture = language;
            AppResources.Culture = language;

            var currentTheme = (HavamalTheme)HavamalPreferences.Theme;
            Resources.MergedDictionaries.Add(currentTheme.GetTheme());
        }

        private void SetUpFinished(object sender, EventArgs e)
        {
            MainPage = new MasterPage
            {
                Title = AppResources.Stanzas
            };
        }

        public void Reset(NavigationPage navigationPage)
        {
            MainPage = new MasterPage(navigationPage)
            {
                Title = navigationPage.Title
            };
        }

        protected override void OnStart()
        {
            var initPage = new InitPage(SetUpFinished);
            MainPage = initPage;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            
        }
    }

}
