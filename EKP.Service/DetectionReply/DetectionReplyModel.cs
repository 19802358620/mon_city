using System.Runtime.Serialization;
using DataAnnotationsExtensions;
using EKP.Entity;
using EKP.Service.Question;
using EKP.Service.Subject;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.DetectionReply
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class DetectionReplyPagerParam : JqgridSqlParam<T_DetectionReply>
    {
        public int? DetectionHandId { get; set; }

        public int? CreateBy { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class DetectionReplyPagerModel : T_DetectionReply
    {
        public SubjectPagerModel Subject { get; set; }

        public QuestionPagerModel Question { get; set; }

        public int? ProjectId { get; set; }

        public bool? IsRight {
            get
            {
                if (this.Question == null) return null;

                if (this.Score == 0) return false;
                else if (this.Score == 1) return true;
                else
                {
                    var questionService = Ioc.GetService<IQuestionService>();
                    var question = ObjectMapper.Mapper<QuestionPagerModel, T_Question>(this.Question);
                    var isRight = questionService.IsRight(question, this.Value);
                    return questionService.IsRight(question, this.Value);
                }
                
            }
        }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class DetectionReplyCreateModel : T_DetectionReply
    {
        [Required(ErrorMessage = "试卷题目Id不能为空")]
        public new int? SubjectId { get; set; }

        [Required(ErrorMessage = "问题Id不能为空")]
        public new int? QuestionId { get; set; }
    }

    /// <summary>
    /// 更新分数模型
    /// </summary>
    public class UpdateScoreModel
    {
        public int? Id { get; set; }

        [Min(0, ErrorMessage = "分数不得小于0")]
        public double? Score { get; set; }
    }

    public class TestPaperHandScore
    {
        public double? Score { get; set; }

        public int DetectionHandId { get; set; }
    }
}
