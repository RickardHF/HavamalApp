using Havamal.SearchHelpers.Indexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Havamal.SearchHelpers.Filtering
{
    public static class FilterExtensions
    {
        public static IReadOnlyCollection<TModel> Filter<TModel>(this IReadOnlyCollection<TModel> data, IndexingParameter param, Func<TModel, string> getContent)
        {
            return data.Where(x => param.SearchWords.Any(y => getContent(x).ToLower().Contains(y))).ToList();
        }

        public static IReadOnlyCollection<IndexingObject<TModel>> FilterOnThresh<TModel>(this IReadOnlyCollection<IndexingObject<TModel>> data, double thresh)
        {
            return data.Where(x => x.Weight > thresh).OrderByDescending(x => x.Weight).ToList();
        }
    }
}
