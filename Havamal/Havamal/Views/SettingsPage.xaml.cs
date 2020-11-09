using Havamal.Helpers;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Resources;
using Havamal.Resources.TextResources;
using Havamal.ViewModels;
using Havamal.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage, INotifyPropertyChanged
    {
        private readonly SettingsPageModel _pagemodel;
        public SettingsPage()
        {
            InitializeComponent();
            var bindingContext = Startup.ServiceProvider.GetService<SettingsPageModel>();
            _pagemodel = bindingContext;
            BindingContext = bindingContext;

            versePicker.OnClick = () => OnVersionClicked(new object(), null);
            langPicker.OnClick = () => OnAppLangClicked(new object(), null);
            themePicker.OnClick = () => OnThemeClicked(new object(), null);
        }

        private void OnThemeClicked(object sender, EventArgs args)
        {
            Navigation.PushPopupAsync(new ThemeChoicePopup(_pagemodel.Themes.ToList(), SetTheme));
        }


        public void OnVersionClicked(object sender, EventArgs args)
        {
            var page = new LanguageChoicePopup(_pagemodel.Languages.ToList(), SetLanguage, _pagemodel.CurrentVerseLanguage);
            Navigation.PushPopupAsync(page);
        }
        public void OnAppLangClicked(object sender, EventArgs args)
        {
            var page = new LanguageChoicePopup(_pagemodel.AppLanguages.ToList(), SetAppLanguage, _pagemodel.CurrentAppLanguage);
            Navigation.PushPopupAsync(page);
        }

        public void SetLanguage(Language language)
        {
            HavamalPreferences.SelectedLanguage = language.Id;
            _pagemodel.CurrentVerseLanguage = language;
        }
        public void SetAppLanguage(Language language)
        {
            HavamalPreferences.AppLanguage = language.LanguageCode;
            _pagemodel.CurrentAppLanguage = language;
            var app = (App) Application.Current;
            app.Reset(new NavigationPage(new SettingsPage() { Title = AppResources.Settings } ));
        }

        public void SetTheme(ThemeListItem theme)
        {
            var dics = Application.Current.Resources.MergedDictionaries;
            dics.Clear();

            HavamalPreferences.Theme = theme.ThemeId;

            dics.Add(theme.Theme);
            _pagemodel.CurrentTheme = theme;
        }

    }
}