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
    /// 描    述：弹窗类型
    /// </summary>
    public enum DialogType
    {
        Success,//成功
        Error,//失败
        Login,//登录
        Lock,//锁屏
        Warn,//警告
        Loading,//加载
        Confirm,//确认弹窗
        Alert//普通弹出框
    }
}
