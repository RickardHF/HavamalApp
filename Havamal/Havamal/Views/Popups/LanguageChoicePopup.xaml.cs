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
                //LanguageSelected();
            }
        }

        public LanguageChoicePopup(List<Language> langs, Action<Language> selectionAction, Language selectedLanguage = null, bool showSelectedLanguage = true)
        {
            Languages = new ObservableCollection<Language>();

            _selectionAction = selectionAction;
            _selectedLanguage = selectedLanguage;

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
            Close();
        }

        private void PopulateList(List<Language> langs)
        {
            foreach(var lang in langs)
            {
                Languages.Add(lang);
            }
        }

        private void Close()
        {
            Navigation.RemovePopupPageAsync(this);
        }

        private void CloseClicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}