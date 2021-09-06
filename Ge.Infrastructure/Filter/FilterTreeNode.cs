using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Filter
{
    public class FilterTreeNode
    {
        public int Id { get; set; }
        public string Mark { get; set; }//标记
        public bool Expanded { get; set; }
        public int Grade { get; set; }
        public string EntiyName { get; set; }
        public string ActionEntiyName { get; set; }
        public string DisplayName { get; set; }
        public string OrderRecord { get; set; }
        public List<FilterTreeNode> ChildsNodes { get; set; }
    }
}
