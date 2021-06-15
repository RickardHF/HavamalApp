using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;

namespace Havamal.SearchHelpers.Indexing
{
    public static class ResultIndexer
    {
        public static IReadOnlyCollection<IndexingObject<TModel>> IndexObjects<TModel>(this IReadOnlyCollection<TModel> model, IndexingParameter param, Func<TModel, string> getContent)
        {
            // Implemented TF-IDF

            var idfs = new Dictionary<string, double>();
            var indexedObjects = new List<IndexingObject<TModel>>();

            if (!model.Any() || !param.SearchWords.Any()) return indexedObjects;

            foreach(var term in param.SearchWords)
            {
                var df = model.Count(x => getContent(x).Contains(term));
                if (df != 0)
                {
                    var idf = Math.Log(model.Count / df);
                    idfs.Add(term, idf);
                }
            }

            foreach(var verse in model)
            {
                var content = getContent(verse);
                var weigth = idfs.Select(x => Regex.Matches(content, x.Key).Count * x.Value).Sum();
                indexedObjects.Add(new IndexingObject<TModel>(verse, weigth));
            }

            return indexedObjects.OrderByDescending(x => x.Weight).ToList();
        }
    }
}
