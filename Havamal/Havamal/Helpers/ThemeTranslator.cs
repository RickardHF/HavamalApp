﻿using Havamal.Resources;
using Havamal.Resources.Themes;
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
                HavamalTheme.Dark => new DarkTheme(),
                HavamalTheme.Water => new WaterTheme(),
                _ => new EarthTheme(),
            };
        }
    }
}
