using Ge.Infrastructure.EasyUi.Base;

namespace Ge.Infrastructure.EasyUi
{
    /// <summary>
    /// 名    称：Dialog
    /// 作    者：胡政
    /// 创建时间：2015-08-29
    /// 联系方式：13436053642
    /// 描    述：对话框
    /// </summary>
    public abstract class Dialog : IEasyUiResult
    {
        public string PluginName { get { return "Dialog"; } }//结果类型

        public virtual string Type { get; set; }//弹窗类型

        public virtual string Title { get; set; } //标题

        public virtual string Content { get; set; }//内容
    }

    /// <summary>
    /// 成功消息对话框
    /// </summary>
    public class SuccessDialog : Dialog
    {
        private string type = "info";
        private string title = "成功";
        private string content = "操作成功";

        public override string Type
        {
            get { return type; }
            set { type = value; }
        }

        public override string Title
        {
            get { return title; }
            set { title = value; }
        }

        public override string Content
        {
            get { return content; }
            set { content = value; }
        }
    }

    /// <summary>
    /// 错误消息对话框
    /// </summary>
    public class ErrorDialog : Dialog
    {
        private string type = "error";
        private string title = "错误";
        private string content = "操作失败";

        public override string Type
        {
            get { return type; }
            set { type = value; }
        }

        public override string Title
        {
            get { return title; }
            set { title = value; }
        }

        public override string Content
        {
            get { return content; }
            set { content = value; }
        }
    }

    /// <summary>
    /// 登陆框
    /// </summary>
    public class LoginDialog : Dialog
    {
        private string type = "error";
        private string title = "登录";
        private string content = "请登录";

        public override string Type
        {
            get { return type; }
            set { type = value; }
        }

        public override string Title
        {
            get { return title; }
            set { title = value; }
        }

        public override string Content
        {
            get { return content; }
            set { content = value; }
        }

        public string DialogMark
        {
            get { return "LoginIn"; }
        }
    }
}
