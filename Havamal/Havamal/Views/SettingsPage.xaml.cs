using Havamal.Models;
using Havamal.ViewModels;
using Havamal.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsPageModel _pagemodel;
        public SettingsPage()
        {
            InitializeComponent();
            var bindingContext = Startup.ServiceProvider.GetService<SettingsPageModel>();
            _pagemodel = bindingContext;
            BindingContext = bindingContext;

            versePicker.OnClick = () => OnVersionClicked(new object(), null);
        }

        public void OnVersionClicked(object sender, EventArgs args)
        {
            Navigation.PushPopupAsync(new LanguageChoicePopup(_pagemodel.Languages.ToList(), SetLanguage));
        }

        public void SetLanguage(Language language)
        {

            Preferences.Set("SelectedLanguage", language.Id);
            _pagemodel.CurrentVerseLanguage = language;
        }

    }
}