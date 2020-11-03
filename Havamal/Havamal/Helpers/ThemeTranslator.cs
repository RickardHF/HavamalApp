using Havamal.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Havamal.Helpers
{
    public static class ThemeTranslator
    {

        public static ResourceDictionary GetTheme(this HavamalTheme themeId)
        {
            return themeId switch
            {
                HavamalTheme.Light => new LightTheme(),
                _ => new EarthTheme(),
            };
        }
    }
}
