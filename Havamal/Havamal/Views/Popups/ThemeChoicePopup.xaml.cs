using Havamal.Helpers;
using Havamal.Models.HelperModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThemeChoicePopup : PopupPage
    {
        public ObservableCollection<ThemeListItem> Themes { get; set; }

        private ThemeListItem _selectedTheme;
        private Action<ThemeListItem> _selectionAction;

        public ThemeListItem SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                _selectedTheme = value;
                OnPropertyChanged(nameof(SelectedTheme));
                Close();
                //LanguageSelected();
            }
        }

        public ThemeChoicePopup(List<ThemeListItem> langs, Action<ThemeListItem> selectionAction, bool showSelectedLanguage = true)
        {
            Themes = new ObservableCollection<ThemeListItem>();

            _selectionAction = selectionAction;

            PopulateList(langs);

            InitializeComponent();
            LangList.ItemsSource = Themes;
            if (showSelectedLanguage) LangList.SelectedItem = SelectedTheme;
            LangList.ItemSelected += LangList_ItemSelected;
        }

        private void LangList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SelectedTheme = (ThemeListItem)e.SelectedItem;
            _selectionAction(SelectedTheme);
        }

        private void PopulateList(List<ThemeListItem> langs)
        {
            foreach (var lang in langs)
            {
                Themes.Add(lang);
            }
            _selectedTheme = langs.FirstOrDefault(X => X.ThemeId == HavamalPreferences.Theme);
        }

        public void LanguageSelected()
        {
            HavamalPreferences.Theme = _selectedTheme?.ThemeId ?? 0;
            //Navigation.RemovePopupPageAsync(this);
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