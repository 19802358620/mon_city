using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.Base.EkpBaseModel
{
    /// <summary>
    /// 分组结果模型
    /// </summary>
    public class GroupByModel<T> 
    {
        public T Field { get; set; }//聚类字段
        public int? Count { get; set; }//数量
    }
}
