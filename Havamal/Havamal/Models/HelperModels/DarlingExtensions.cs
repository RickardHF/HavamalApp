using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Models.HelperModels
{
    public static class DarlingExtensions
    {
        public static Darling<TModel> Allow<TModel>(TModel value)
        {
            if (!Darling<TModel>.CheckValueValid(value)) return No<TModel>();
            var container = new Darling<TModel>();
            container.SetValue(value);
            return container;
        }

        public static Darling<TModel> No<TModel>()
        {
            return new Darling<TModel>();
        }
    }
}
