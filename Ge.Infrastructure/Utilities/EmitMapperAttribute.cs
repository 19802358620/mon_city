using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Utilities
{
    /// <summary>
    /// 名    称：MapperIgnoreAttribute
    /// 作    者：胡政
    /// 参    考：http://www.mamicode.com/info-detail-95629.html
    /// 创建时间：2015-09-02
    /// 联系方式：13436053642
    /// 描    述：自定义标签：通过在属性上面添加此标签来忽视掉此字段的映射
    /// </summary>
    public class IgnoreMapperAttribute : System.Attribute
    {
    }    
    /// <summary>
    /// 将DateTime类型映射为string类型(仅限于datatable和datarow有效)
    /// </summary>
    public class DateTimeToStrAttribute : System.Attribute
    {
        /// <summary>
        /// 时间格式
        /// </summary>
        public string Formatter { get; set; }
    }
}
