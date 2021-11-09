using Havamal.Helpers;
using Havamal.Interfaces.Helpers;
using Havamal.Models.HelperModels;
using Havamal.Resources;
using Havamal.Resources.TextResources;
using Havamal.Resources.Themes;
using Havamal.Views.Popups;
using System;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Extensions;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.ViewModels
{
    public class BaseThemePageModel
    {
        private readonly IThemeChanger _themeChanger;
        private readonly NavigationPage _resetPage;
        public ObservableCollection<ThemeListItem> Themes { get; private set; }
        public ThemeListItem CurrentTheme { get; private set; }
        public ICommand ChangeThemeCommand { get; private set; }

        public BaseThemePageModel(IThemeChanger themeChanger, NavigationPage resetPage)
        {
            _themeChanger = themeChanger;
            _resetPage = resetPage;

            Themes = new ObservableCollection<ThemeListItem> {
                    new ThemeListItem { ThemeId = (int) HavamalTheme.Earth, ThemeName = AppResources.ThemeEarth, Theme = new EarthTheme(), ThemeImage = "odelEARTH.png"}
                    , new ThemeListItem {ThemeId = (int) HavamalTheme.Light, ThemeName = AppResources.ThemeLight, Theme = new LightTheme(), ThemeImage = "odelLIGHT.png"}
                    , new ThemeListItem { ThemeId = (int) HavamalTheme.Water, ThemeName = AppResources.ThemeWater, Theme = new WaterTheme(), ThemeImage = "odelWATER.png"}
                    , new ThemeListItem { ThemeId = (int) HavamalTheme.Dark, ThemeName = AppResources.ThemeDark, Theme = new DarkTheme(), ThemeImage = "odelDARK.png"}
                };

            CurrentTheme = Themes.FirstOrDefault(x => x.ThemeId == HavamalPreferences.Theme);

            ChangeThemeCommand = new Command<ThemeListItem>(SetTheme);
        }

        private void OnThemeClicked(object sender, EventArgs args)
        {
            _ = Application.Current.MainPage.Navigation.PushPopupAsync(new ThemeChoicePopup(Themes.ToList(), SetTheme));
        }

        private void SetTheme(ThemeListItem theme)
        {
            var dics = Application.Current.Resources.MergedDictionaries;
            dics.Clear();

            HavamalPreferences.Theme = theme.ThemeId;

            dics.Add(theme.Theme);
            CurrentTheme = theme;
            var app = (App)Application.Current;
            app.Reset(_resetPage);
            _themeChanger.ChangeTheme((HavamalTheme)theme.ThemeId);
        }
    }
}
