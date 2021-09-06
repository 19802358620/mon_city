using EKP.Entity;
using EKP.Service.Class;
using EKP.Service.Detection;
using EKP.Service.DetectionReply;
using EKP.Service.User;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EKP.Service.DetectionHand
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class DetectionHandPagerParam : JqgridSqlParam<T_DetectionHand>
    {
        public string KeyWord { get; set; }

        public int? DetectiontId { get; set; }

        public string Status { get; set; }

        public int? ProjectId { get; set; }

        public int? CreateBy { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class DetectionHandPagerModel : T_DetectionHand
    {
        public string ShowStatus
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Status))
                {
                    var status = EnumHelper.GetEnum<DetectionHandStatus>(this.Status);
                    return EnumHelper.GetEnumDescription(status);
                }

                return string.Empty;
            }
        }

        public string CreateByAccount { get; set; }

        public string CreateByRealName { get; set; }

        public string CompanyName { get; set; }

        public DetectionPagerModel Detection { get; set; }

        public List<DetectionReplyPagerModel> DetectionReplys { get; set; }

        /// <summary>
        /// 设置考试还剩余的时间
        /// </summary>
        public double? RemainSecond{ get; set; }

        /// <summary>
        /// 考试所得分数
        /// </summary>
        public double? Score { get; set; }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class DetectionHandCreateModel : T_DetectionHand
    {
        [Required(ErrorMessage = "练习Id不能为空")]
        public new int? DetectiontId { get; set; }
    }

    /// <summary>
    /// 答题学生分页参数
    /// </summary>
    public class AnswerUserPagerParam : JqgridSqlParam<T_User>
    {
        public string KeyWord { get; set; }
        public string Gender { get; set; }
        public int? RoleId { get; set; }
        public int? SiteId { get; set; }
        public string Status { get; set; }
        public string Grade { get; set; }
        public int? ClassId { get; set; }
        public string ClassIds { get; set; }
        public int? ProjectId { get; set; }
    }

    /// <summary>
    /// 答题学生分页模型
    /// </summary>
    public class AnswerUserPagerModel : T_User
    {
        public string RoleName { get; set; }
        public string RoleKey { get; set; }
        public string SiteName { get; set; }
        public string ShowStatus
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Status))
                {
                    var userStatus = EnumHelper.GetEnum<UserStatus>(this.Status);
                    return EnumHelper.GetEnumDescription(userStatus);
                }

                return string.Empty;
            }
        }

        public string ShowClasses { get; set; }
    }

    /// <summary>
    /// 个人成绩分页参数
    /// </summary>
    public class StuedntScorePagerParam : JqgridSqlParam<T_DetectionHand>
    {
        public string KeyWord { get; set; }

        public int? DetectiontId { get; set; }

        public string Status { get; set; }

        public int? ProjectId { get; set; }

        public int? CreateBy { get; set; }

        public string CreateByClassIds { get; set; }

        public string ClassId { get; set; }
    }

    /// <summary>
    /// 个人成绩分页模型
    /// </summary>
    public class StuedntScorePagerModel 
    {
        public int? CreateBy { get; set; }

        public string CreateByAccount { get; set; }

        public string CreateByRealName { get; set; }

        public string CreateByClassIds { get; set; }

        public string ShowCreateByClasses { get; set; }

        public int? ProjectId { get; set; }

        public List<ClassPagerModel> CreateByClasses { get; set; }

        /// <summary>
        /// 考试所得分数
        /// </summary>
        public double? Score { get; set; }
    }

    /// <summary>
    /// 班级成绩分页参数
    /// </summary>
    public class ClassScorePagerParam : JqgridSqlParam<T_DetectionHand>
    {
        public string KeyWord { get; set; }

        public int? DetectiontId { get; set; }

        public string Status { get; set; }

        public int? ProjectId { get; set; }

        public string ClassIds { get; set; }
    }

    /// <summary>
    /// 班级成绩分页模型
    /// </summary>
    public class ClassScorePagerModel
    {
        //班级Id
        public int? ClassId { get; set; }

        //班级名称
        public string ClassName { get; set; }

        //完成率
        public double? CompletionRate { get; set; }

        //平均分
        public double? AverageScore { get; set; }

        //最高分
        public double? MaxScore { get; set; }

        //最低分
        public double? MinScore { get; set; }

        //高分率
        public double? HighScoreRate { get; set; }

        //低分率
        public double? LowScoreRate { get; set; }

        //优秀率
        public double? ExcellenceRate { get; set; }

        //及格率
        public double? PassRate { get; set; }

        //总分
        public double? TotalScore { get; set; }

        //提交比例
        public string SubmitRate { get; set; }
    }

    /// <summary>
    /// 完成情况
    /// </summary>
    public class CompareClassScoreModel
    {
        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        /// <summary>
        /// 完成率
        /// </summary>
        public double CompletyRate { get; set; }

        /// <summary>
        /// 正确率
        /// </summary>
        public double CorrectRate { get; set; }
    }
}
