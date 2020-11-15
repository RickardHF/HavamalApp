using Havamal.Resources.TextResources;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerseChoicePopup : PopupPage
    {
        private Action<int> _verseChosen;
        private readonly int _upperLimit;
        public VerseChoicePopup(Action<int> chosenAction, int upperLimit)
        {
            InitializeComponent();
            _verseChosen = chosenAction;
            _upperLimit = upperLimit;
        }

        private void VerseChosen(object sender, EventArgs e)
        {
            ErrorTextField.Text = "";
            if (int.TryParse(IdField.Text, out int i))
            {
                if(i <= 0)
                {
                    ErrorTextField.Text = AppResources.VerseChoiceTooLow;
                }
                else if(i > _upperLimit)
                {
                    ErrorTextField.Text = AppResources.VerseChoiceTooHigh;
                }
                else
                {
                    _verseChosen(i);
                    Close();
                }
            }else
            {
                ErrorTextField.Text = AppResources.VerseChoiceError;
            }
        }

        private void Close()
        {
            Navigation.RemovePopupPageAsync(this);
        }
    }
}