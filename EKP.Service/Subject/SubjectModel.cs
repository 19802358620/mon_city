using EKP.Entity;
using EKP.Service.Question;
using Ge.Infrastructure.Metronicv;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.Subject
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class SubjectPagerParam : JqgridSqlParam<T_Subject>
    {
        public string KeyWord { get; set; }

        public string Type { get; set; }

        public int? DetectionId { get; set; }

        public int? CreateBy { get; set; }

        public List<int> Ids { get; set; }

        public int? ProjectId { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class SubjectPagerModel : T_Subject
    {
       public string ShowType
       {
            get
            {
                if (!string.IsNullOrEmpty(this.Type))
                {
                    var subjectType = EnumHelper.GetEnum<SubjectType>(this.Type);
                    return EnumHelper.GetEnumDescription(subjectType);
                }

                return string.Empty;
            }
       }

       public List<QuestionPagerModel> Questions { get; set; }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class SubjectCreateModel : T_Subject
    {
        [Required(ErrorMessage = "题目不能为空")]
        public new string Name { get; set; }

        [Required(ErrorMessage = "题目类型不能为空")]
        public new string Type { get; set; }

        public string Answer { get; set; }

        //选项
        public string Options { get; set; }

        public int? OptionsColumns { get; set; }
    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class SubjectEditModel : T_Subject
    {
        [Required(ErrorMessage = "题目不能为空")]
        public new string Name { get; set; }

        [Required(ErrorMessage = "题目类型不能为空")]
        public new string Type { get; set; }

        //选项
        public string Answer { get; set; }
        public string Options { get; set; }

        public int? OptionsColumns { get; set; }
    }

    /// <summary>
    /// 检测题目是否做对分页参数
    /// </summary>
    public class CheckAnswerParam
    {
        /// <summary>
        /// 回答内容
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 题目Id
        /// </summary>
        public int? SubjectId { get; set; }
    }

    /// <summary>
    /// 检测题目是否做对分页模型
    /// </summary>
    public class CheckAnswerModel
    {
        /// <summary>
        /// 回答内容
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 题目
        /// </summary>
        public SubjectPagerModel Subject { get; set; }

        /// <summary>
        /// 是否做对
        /// </summary>
        public bool? IsRight { get; set; }
    }
}
