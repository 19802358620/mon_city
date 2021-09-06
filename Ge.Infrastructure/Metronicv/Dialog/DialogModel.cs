using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Metronicv.Dialog
{
    /// <summary>
    /// 名    称：Dialog
    /// 作    者：胡政
    /// 创建时间：2015-08-29
    /// 联系方式：13436053642
    /// 描    述：对话框
    /// </summary>
    public class Dialog
    {
        public virtual DialogType DialogType { get; set; }//类型

        public virtual string Type { get { return DialogType.ToString(); } }//类型文本

        public virtual string Title { get; set; } //标题

        public virtual string Content { get; set; }//内容

        public virtual string OtherInfo { get; set; }//附加信息
    }
}
