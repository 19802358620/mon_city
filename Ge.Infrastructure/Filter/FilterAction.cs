using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Filter
{
    /// <summary>
    /// 某个权限的一个操作
    /// </summary>
    public class FilterAction
    {   
        //权限名
        public string FilterEntiyName { get; set; }
        //操作名
        public string EntiyName { get; set; }
        //操作显示名
        public string DisplayName { get; set; }
    }
}
