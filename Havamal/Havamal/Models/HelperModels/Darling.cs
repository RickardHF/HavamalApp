using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace Havamal.Models.HelperModels
{
    public struct Darling<TModel>
    {
        private static readonly List<TModel> _data = new();

        public U MayI<U>(Func<TModel, U> yes, Func<U> no)
        {
            if (_data.Any()) return yes(_data.SingleOrDefault());
            return no();
        }

        public void MayI(Action<TModel> yes, Action no)
        {
            if (_data.Any()) {
                yes(_data.SingleOrDefault());
                return; 
            }
            no();
        }

        public void SetValue(TModel value)
        {
            _data.Clear();
            if (!CheckValueValid(value)) return;
            _data.Add(value);
        }

        public TModel HopeForYes()
        {
            return _data.SingleOrDefault();
        }

        public  static bool CheckValueValid(TModel value)
        {
            return !(value == null || value.Equals(default(TModel)));
        }
    }
}
