using System;
using System.Collections.Generic;
using System.Linq;
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
                {"NO", "bvt.png" }
                , {"EN", "shn.png"}
                , {"CS", "cze.png"}
                , {"RU", "rus.png"}
                , {"XX", "nor.png"}
                , {"DK", "dnk.png"}
                , {"SE", "swe.png"}
                , {"MK", "mkd.png"}
                , {"NL", "bes.png"}
                , {"KG", "kgz.png"}
                , {"DE", "deu.png"}
                , {"ES", "esp.png"}
                , {"CA", "cat.png"}
            };
        }

        public static string Get(string key)
        {
            _langs.TryGetValue(key.ToUpper(), out string val);

            return val;
        }

        public static string Revert(string value)
        {
            return _langs.FirstOrDefault(pair => value.Equals(pair.Value)).Key;
        }
    }
}
