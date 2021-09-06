using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Metronicv.Dialog
{
    /// <summary>
    /// 名    称：IDialogFactory
    /// 作    者：胡政
    /// 创建时间：2015-08-29
    /// 联系方式：13436053642
    /// 描    述：弹出框生成工厂接口
    /// </summary>
    public interface IDialogFactory
    {

    }

    /// <summary>
    /// 消息弹出框工厂
    /// </summary>
    public class DialogFactory : IDialogFactory
    {
        /// <summary>
        /// 用类型创建弹出框
        /// </summary>
        public static Dialog Create(DialogType dialogType)
        {
            var dialog = new Dialog { DialogType = dialogType };
            if (dialogType == DialogType.Success)
            {
                dialog.Title = string.Empty;
                dialog.Content = "操作成功！";
            }
            else if (dialogType == DialogType.Error)
            {
                dialog.Title = string.Empty;
                dialog.Content = "操作失败！";
            }
            return Create(dialog.DialogType, dialog.Title, dialog.Content, dialog.OtherInfo);
        }

        /// <summary>
        /// 用类型、内容创建弹出框
        /// </summary>
        public static Dialog Create(DialogType dialogType, string content)
        {
            var dialog = Create(dialogType);
            dialog.Content = content;
            return Create(dialog.DialogType, dialog.Title, dialog.Content, dialog.OtherInfo);
        }

        /// <summary>
        /// 用类型、标题、内容创建弹出框
        /// </summary>
        public static Dialog Create(DialogType dialogType, string title, string content)
        {
            var dialog = Create(dialogType, title);
            dialog.Title = title;
            dialog.Content = content;
            return Create(dialog.DialogType, dialog.Title, dialog.Content, dialog.OtherInfo);
        }

        /// <summary>
        /// 用类型、标题、内容、附加信息创建弹出框
        /// </summary>
        public static Dialog Create(DialogType dialogType, string title, string content, string otherInfo)
        {
            var dialog = new Dialog()
            {
                DialogType = dialogType,
                Content = content,
                Title = title,
                OtherInfo = otherInfo
            };
            return dialog;
        }

        /// <summary>
        /// 创建弹出框
        /// </summary>
        /// <param name="dialog"></param>
        public static Dialog Create(Dialog dialog)
        {
            return dialog;
        }
    }

}
