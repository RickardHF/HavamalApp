using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Havamal.Models.HelperModels
{
    public class ThemeListItem
    {
        public int ThemeId { get; set; }
        public string ThemeName { get; set; }
        public string ThemeImage { get; set; }
        public ResourceDictionary Theme {get; set;}
    }
}
