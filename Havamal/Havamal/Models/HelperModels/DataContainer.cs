using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace Havamal.Models.HelperModels
{
    public struct DataContainer<TModel>
    {
        private static List<TModel> _data = new List<TModel>();

        public static DataContainer<TModel> Empty()
        {
            return new DataContainer<TModel>();
        }

        public U ValueOrEmpty<U>(Func<TModel, U> value, Func<U> empty)
        {
            if (_data.Any()) return value(_data.SingleOrDefault());
            return empty();
        }

        public void ValueOrEmpty(Action<TModel> value, Action empty)
        {
            if (_data.Any()) {
                value(_data.SingleOrDefault());
                return; 
            }
            empty();
        }

        public void SetValue(TModel value)
        {
            _data.Clear();
            if (!CheckValueValid(value)) return;
            _data.Add(value);
        }

        public TModel GetValueOrDefault()
        {
            return _data.SingleOrDefault();
        }

        private static bool CheckValueValid(TModel value)
        {
            return !(value == null || value.Equals(default(TModel)));
        }

        public static DataContainer<TModel> WithValue(TModel value)
        {
            if (! CheckValueValid(value)) return DataContainer<TModel>.Empty();
            var container = new DataContainer<TModel>();
            container.SetValue(value);
            return container;
        }
    }
}
