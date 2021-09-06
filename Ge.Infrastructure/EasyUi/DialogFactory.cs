namespace Ge.Infrastructure.EasyUi
{
    /// <summary>
    /// 名    称：IDialogFactory
    /// 作    者：胡政
    /// 创建时间：2015-08-29
    /// 联系方式：13436053642
    /// 描    述：对话框生成工厂接口
    /// </summary>
    public interface IDialogFactory
    {

    }

    /// <summary>
    /// 消息对话框工厂
    /// </summary>
    public class DialogFactory : IDialogFactory
    {
        /// <summary>
        /// 成功消息对话框
        /// </summary>
        public static SuccessDialog SuccessDialog(string content = "操作成功", string title = "成功")
        {
            var successDialog = new SuccessDialog()
            {
                Title = title,
                Content = content
            };
            return successDialog;
        }

        /// <summary>
        /// 错误消息对话框
        /// </summary>
        public static ErrorDialog ErroeDialog(string content = "操作失败", string title = "错误")
        {
            var errorDialog = new ErrorDialog()
            {
                Title = title,
                Content = content
            };
            return errorDialog;
        }

        /// <summary>
        /// 登录对话框
        /// </summary>
        public static LoginDialog LoginDialog(string content = "请登录", string title = "登录")
        {
            var loginDialog = new LoginDialog()
            {
                Title = title,
                Content = content
            };
            return loginDialog;
        }
    }

}
