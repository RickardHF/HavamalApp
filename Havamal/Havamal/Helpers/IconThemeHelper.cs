using Havamal.Resources.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Helpers
{
    public static class IconThemeHelper
    {
        public static string GetBookSource(this HavamalTheme theme)
        {
            return theme switch
            {
                HavamalTheme.Dark => "bookWH.png",
                HavamalTheme.Light => "bookB.png",
                HavamalTheme.Water => "bookTW.png",
                _ => "bookDG.png"
            };
        }
        public static string GetSearchSource(this HavamalTheme theme)
        {
            return theme switch
            {
                HavamalTheme.Dark => "searchWH.png",
                HavamalTheme.Light => "searchB.png",
                HavamalTheme.Water => "searchTW.png",
                _ => "searchDG.png"
            };
        }
        public static string GetRandomSource(this HavamalTheme theme)
        {
            return theme switch
            {
                HavamalTheme.Dark => "randomWH.png",
                HavamalTheme.Light => "randomB.png",
                HavamalTheme.Water => "randomTW.png",
                _ => "randomDG.png"
            };
        }
        public static string GetFavoriteSource(this HavamalTheme theme)
        {
            return theme switch
            {
                HavamalTheme.Dark => "favoritewheat.png",
                HavamalTheme.Light => "favoriteblack.png",
                HavamalTheme.Water => "favoritetimberwolf.png",
                _ => "favoritespanishgray.png"
            };
        }
        public static string GetCompareSource(this HavamalTheme theme)
        {
            return theme switch
            {
                HavamalTheme.Dark => "compareWH.png",
                HavamalTheme.Light => "compareB.png",
                HavamalTheme.Water => "compareTW.png",
                _ => "compareDG.png"
            };
        }
        public static string GetSpokesSource(this HavamalTheme theme)
        {
            return theme switch
            {
                HavamalTheme.Dark => "spokesWH.png",
                HavamalTheme.Light => "spokesB.png",
                HavamalTheme.Water => "spokesTW.png",
                _ => "spokesDG.png"
            };
        }

        public static string GetListSource(this HavamalTheme theme)
        {
            return theme switch
            {
                HavamalTheme.Dark => "listWH.png",
                HavamalTheme.Light => "listB.png",
                HavamalTheme.Water => "listTW.png",
                _ => "listDG.png"
            };
        }
    }
}
