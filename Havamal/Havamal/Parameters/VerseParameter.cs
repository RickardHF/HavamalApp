using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Parameters
{
    public class VerseParameter
    {
        public int Language { get; set; }
        public bool OnIds { get; set; }
        public IReadOnlyCollection<int> Ids {get; set;}
    }
}
