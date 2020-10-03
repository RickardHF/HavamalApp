using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.SearchHelpers.Indexing
{
    public class IndexingParameter
    {
        public IReadOnlyCollection<string> SearchWords { get; internal set; }
        public IReadOnlyCollection<int> SearchNumbers { get; internal set; }
    }
}
