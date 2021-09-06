using EKP.Entity;
using Ge.Infrastructure.Metronicv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.Homework
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class HomeworkPagerParm: JqgridSqlParam<T_Homework>
    {
        public string KeyWord { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public string TeacherName { get; set; }

        public string AnswerAttachmentName { get; set; }

        public string AttachmentName { get; set; }

        public string  ClassId { get; set; }

        public string Status { get; set; }
        public string StuScore { get; set; }


    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class HomeworkPagerModel : T_Homework
    {
        public string TeacherName { get; set; }

        public string  ClassId { get; set; }
        public string ClassNames { get; set; }
        public string StuScore { get; set; }

        public string SubmitSatatus { get; set; }

    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class HomeworkCreateModel : T_Homework
    {
        public int HomeworkId { get; set; }

        public string  ClassId { get; set; }
    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class HomeworkEditModel : T_Homework
    {
        //public string AnswerAttachmentName { get; set; }

        //public string AttachmentName { get; set; }
    }

    public class HoemworkSubmitModel : T_HomeworkSubmit
    {

    }

}
