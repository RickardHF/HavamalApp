using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Havamal.ViewModels
{
    public class BaseSwipeAction : BasePageModel
    {
        public Action<int> OnVerseIdChange;
        public Command SwipeLeft => new Command(NextVerse);
        public Command SwipeRight => new Command(PreviousVerse);

        private void NextVerse(object sender)
        {
            try
            {
                var some = Preferences.Get("CurrentVerse", 1);
                if (some < 164 ) // TODO : get dynamic
                {
                    var newCurrentVerse = some + 1;
                    Preferences.Set("CurrentVerse", newCurrentVerse);
                    OnVerseIdChange(newCurrentVerse);
                }
            }
            catch (Exception)
            {

            }

        }

        private void PreviousVerse(object sender)
        {
            try {
                var some = Preferences.Get("CurrentVerse", 1);
                if (some > 1)
                {
                    var newCurrentVerse = some - 1;
                    Preferences.Set("CurrentVerse", newCurrentVerse);
                    OnVerseIdChange(newCurrentVerse);
                }
            } catch ( Exception )
            {

            }
        }
    }
}
