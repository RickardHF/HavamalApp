using Havamal.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LanguageChoicePopup : PopupPage
    {
        public ObservableCollection<Language> Languages { get; set; }

        private Language _selectedLanguage;
        private Action<Language> _selectionAction;

        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
                Close();
                //LanguageSelected();
            }
        }

        public LanguageChoicePopup(List<Language> langs, Action<Language> selectionAction, bool showSelectedLanguage = true)
        {
            Languages = new ObservableCollection<Language>();

            _selectionAction = selectionAction;

            PopulateList(langs);

            InitializeComponent();
            LangList.ItemsSource = Languages;
            if(showSelectedLanguage) LangList.SelectedItem = SelectedLanguage;
            LangList.ItemSelected += LangList_ItemSelected;
        }

        private void LangList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SelectedLanguage = (Language) e.SelectedItem;
            _selectionAction(SelectedLanguage);
        }

        private void PopulateList(List<Language> langs)
        {
            foreach(var lang in langs)
            {
                Languages.Add(lang);
            }
            _selectedLanguage = langs.FirstOrDefault(X => X.Id == Preferences.Get("SelectedLanguage", 1));
        }

        public void LanguageSelected()
        {
            Preferences.Set("SelectedLanguage", _selectedLanguage?.Id ?? 1);
            //Navigation.RemovePopupPageAsync(this);
        }

        private void Close()
        {
            Navigation.RemovePopupPageAsync(this);
        }

    }
}