using System.ComponentModel.DataAnnotations;
using EKP.Entity;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.Notice
{

    /// <summary>
    /// 分页参数
    /// </summary>
    public class NoticePagerParam : JqgridSqlParam<T_Notice>
    {
        public string KeyWord { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public string TeacherName { get; set; }

        public string AnswerAttachmentName { get; set; }

        public string AttachmentName { get; set; }
        public string ClassNames { get; set; }
        public string ClassIds { get; set; }

    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class NoticePagerModel : T_Notice
    {
        public string TeacherName { get; set; }

        [Required(ErrorMessage ="班级不能为空")]
        public string ClassIds { get; set; }
        public string ClassNames { get; set; }
        public string NoticeState { get; set; }

    }
    /// <summary>
    /// 创建模型
    /// </summary>
    public class NoticeCreateModel : T_Notice
{
        
        public  int NoticeId { get; set; }
        public string ClassIds { get; set; }
    }
    /// <summary>
    /// 编辑模型
    /// </summary>
    public class NoticekEditModel : T_Notice
    {
        public string ClassIds { get; set; }

    }
}

