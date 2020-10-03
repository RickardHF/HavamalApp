using Havamal.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Parameters
{
    public class SearchParameter
    {
        public DataContainer<int> LanguageId { get; set; }
        public bool OnlyFavorites { get; set; }
        public bool NumericOrder { get; set; }
        public string SearchText { get; set; }
    }
}
