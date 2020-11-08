using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Havamal.Models.HelperModels
{
    public class VerseListItem
    {
        public int VerseId { get; set; }
        public string Content { get; set; }
        public Style Favorite { get; set; }
    }
}
