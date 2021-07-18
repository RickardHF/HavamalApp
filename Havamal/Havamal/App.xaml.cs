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

[assembly: ExportFont("Raleway-Regular.ttf", Alias = "Raleway")]
[assembly: ExportFont("Roboto-Regular.ttf", Alias = "Roboto")]
namespace Havamal
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            CultureInfo language = new CultureInfo(HavamalPreferences.AppLanguage);

            Thread.CurrentThread.CurrentUICulture = language;
            AppResources.Culture = language;

            var currentTheme = (HavamalTheme)HavamalPreferences.Theme;
            Resources.MergedDictionaries.Add(currentTheme.GetTheme());
        }

        public void SetUpFinished(object sender, EventArgs e)
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
