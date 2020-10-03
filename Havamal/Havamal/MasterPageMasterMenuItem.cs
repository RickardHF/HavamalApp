using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havamal
{

    public class MasterPageMasterMenuItem
    {
        public MasterPageMasterMenuItem()
        {
            TargetType = typeof(MasterPageMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}