using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Helpers
{
    public static class DBHelpers
    {
        public static string SafeSqLiteString(this string value)
        {
            return value.Replace("'", "''");
        }
    }
}
