using EKP.Entity;
using EKP.Service.SubjectQuestion;
using Ge.Infrastructure.Metronicv;
using System.ComponentModel.DataAnnotations;

namespace EKP.Service.Question
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class QuestionPagerParam : JqgridSqlParam<T_Question>
    {
        public int? SubjectId { get; set; }

        public int? ChapterId { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class QuestionPagerModel : T_Question
    {

        public string ShowAnswer
        {
            get
            {
                if (this.Type == QuestionType.bit.ToString())
                {
                    if (Answer == "1")
                        return "正确";
                    else if (Answer == "0")
                        return "错误";

                    return this.Answer;
                }
                return this.Answer;
            }
        }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class QuestionCreateModel : T_Question
    {
        [Required(ErrorMessage = "题目Id不能为空")]
        public new int? SubjectId { get; set; }

    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class QuestionEditModel : T_Question
    {
        [Required(ErrorMessage = "题目Id不能为空")]
        public new int? SubjectId { get; set; }
    }
}
