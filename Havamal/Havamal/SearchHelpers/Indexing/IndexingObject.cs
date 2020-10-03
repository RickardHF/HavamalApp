using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;

namespace Havamal.SearchHelpers.Indexing
{
    public class IndexingObject<TModel>
    {
        public TModel DataObject { get; }
        public double Weight { get; }

        public IndexingObject(TModel model, double weight)
        {
            DataObject = model;
            Weight = weight;
        }
    }
}
