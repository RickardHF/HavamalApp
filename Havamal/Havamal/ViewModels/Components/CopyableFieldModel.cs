using Havamal.Helpers;
using Havamal.Resources.Themes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Havamal.ViewModels.Components
{
    public class CopyableFieldModel : BasePageModel
    {
        private Timer timer;
        public bool JustCopied { get; set; }
        public string ImageSource => JustCopied ? $"ok{ThemePart}.xml" : $"copy{ThemePart}.xml";

        public ICommand CopyCommand { get; set; }

        private string ThemePart => (HavamalTheme)HavamalPreferences.Theme switch
        {
            HavamalTheme.Dark => "DARK",
            HavamalTheme.Light => "LIGHT",
            HavamalTheme.Water => "WATER",
            HavamalTheme.Earth => "EARTH",
            _ => ""
        };


        public CopyableFieldModel()
        {
            CopyCommand = new Command<string>(OnCopy);
        }
        public void OnTimerReached(object stateInfo) {
            JustCopied = false;
            OnPropertyChanged(nameof(ImageSource));
            timer.Dispose();
        }

        private async void OnCopy(string text)
        {
            await Clipboard.SetTextAsync(text);
            JustCopied = true;
            OnPropertyChanged(nameof(ImageSource));
            var autoEvent = new AutoResetEvent(false);

            timer = new Timer(OnTimerReached, autoEvent, 1000, 0);
        }
    }
}
