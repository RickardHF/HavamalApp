using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.SearchHelpers.Indexing.Helper
{
    public static class IndexingParameterHelper
    {
        public static IndexingParameter CreateIndexingParameter(this string searchString)
        {

            if (string.IsNullOrEmpty(searchString)) return new IndexingParameter();

            char splitter = ' ';
            var searchWords = new List<string>();
            var searchNumbers = new List<int>();
            foreach (var part in searchString.Trim().Split(splitter))
            {
                if(int.TryParse(part, out int number)) {
                    searchNumbers.Add(number);
                } else
                {
                    if(part.Contains("+"))
                    {
                        searchWords.Add(part.Replace("+", " ").ToLower());
                    } else
                    {
                        searchWords.Add(part.ToLower());
                    }
                }
            }

            return new IndexingParameter
            {
                SearchWords = searchWords,
                SearchNumbers = searchNumbers
            };
        }
    }
}
