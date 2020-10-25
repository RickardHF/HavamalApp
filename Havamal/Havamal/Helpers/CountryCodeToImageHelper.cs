using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Helpers
{
    public static class CountryCodeToImageHelper
    {
        private static IDictionary<string, string> _langs;

        static CountryCodeToImageHelper()
        {
            _langs = new Dictionary<string, string>
            {
                {"NO", "btv.png" }
            };
        }

        public static string Get(string key)
        {
            _langs.TryGetValue(key, out string val);

            return val;
        }
    }
}
