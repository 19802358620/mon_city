using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Question;
using EKP.Service.Subject;
using EKP.Service.SubjectQuestion;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EKP.Service.DetectionReply
{
    /// <summary>
    /// 试卷答题管理
    /// </summary>
    public interface IDetectionReplyService : IEkpEntityService<T_DetectionReply>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(DetectionReplyPagerParam param, params string[] includePath) where T : DetectionReplyPagerModel, new();

        /// <summary>
        /// 更新评分信息
        /// </summary>
        void SynsScore<T>(List<T> detectionReplys) where T : DetectionReplyPagerModel, new();

        /// <summary>
        /// 根据handId获取成绩集合
        /// </summary>
        /// <param name="handIds"></param>
        /// <returns></returns>
        List<TestPaperHandScore> GetTestPaperScores(List<int> handIds);
    }

    /// <summary>
    /// 试卷答题管理
    /// </summary>
    public class DetectionReplyService : EkpEntityService<T_DetectionReply>, IDetectionReplyService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(DetectionReplyPagerParam param, params string[] includePath) where T : DetectionReplyPagerModel, new()
        {
            var subjectService = Ioc.GetService<ISubjectService>();
            var questionService = Ioc.GetService<IQuestionService>();

            var sql = "select top 99.99999999 percent T_DetectionReply.* {0} from T_DetectionReply {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_DetectionReply.IsDeleted = '{0}' ", IsDelete.undeleted),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_User"))
            {
                sqlSelect += " ,(T_User.CreateBy)CreateByAccount ";
                sqlJoin += " left join T_User on T_User.Id = T_DetectionReply.CreateBy ";
            }
            if (includePath.Contains("T_DetectionHand"))
            {
                sqlSelect += " ,(T_DetectionHand.Id)DetectionHandId ";
                sqlJoin += " left join T_DetectionHand on T_DetectionHand.Id = T_DetectionReply.DetectionHandId ";
                sqlWhere += " and T_DetectionHand.IsDeleted = '{0}' ".Format2(IsDelete.undeleted);
            }
            if (includePath.Contains("T_Detection"))
            {
                sqlSelect += " ,(T_Detection.ProjectId)ProjectId ";
                sqlJoin += " left join T_Detection on T_Detection.Id = T_DetectionHand.DetectiontId ";
            }

            //查询
            if (param.DetectionHandId != null)
                sqlWhere += string.Format(" and (T_DetectionReply.DetectionHandId = '{0}') ", param.DetectionHandId);
            if (param.CreateBy != null)
                sqlWhere += string.Format(" and (T_DetectionReply.CreateBy = '{0}') ", param.CreateBy);

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by {0}{1} {2} ",
                    param.SortBy.Contains(".") ? string.Empty : "T_DetectionReply.", param.SortBy, param.SortOrder);

            //获取分页
            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);
            var pager = EkpDbService.GetPager<T>(sql, param);

            //填充试卷题目
            var subjectIds = pager.Rows.Select(row => Convert.ToInt32(row.SubjectId)).ToList();
            if (subjectIds.Count > 0)
            {
                var subjects = subjectService
                    .GetList(" Id in ({0}) and IsDeleted='{1}' ".Format2(subjectIds.ToStringBySplit(",", "'"), IsDelete.undeleted));
                pager.Rows.ForEach(row => {
                    var subject = subjects.FirstOrDefault(tps => tps.Id == row.SubjectId);
                    row.Subject = ObjectMapper.Mapper<T_Subject, SubjectPagerModel>(subject);
                });
            }

            //填充试卷问题
            var questionIds = pager.Rows.Select(row => Convert.ToInt32(row.QuestionId)).ToList();
            if (questionIds.Count > 0)
            {
                var questions = questionService
                    .GetList(" Id in ({0}) and IsDeleted='{1}' ".Format2(questionIds.ToStringBySplit(",", "'"), IsDelete.undeleted));
                pager.Rows.ForEach(row =>
                {
                    var question = questions.FirstOrDefault(tps => tps.Id == row.QuestionId);
                    row.Question = ObjectMapper.Mapper<T_Question, QuestionPagerModel>(question);
                });
            }

            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        }

        /// <summary>
        /// 更新评分信息
        /// </summary>
        public void SynsScore<T>(List<T> detectionReplys) where T : DetectionReplyPagerModel, new()
        {
            if (detectionReplys.Count() == 0) return;

            var questionIds = detectionReplys.Select(tpr => tpr.QuestionId).ToList();
            var questionService = Ioc.GetService<IQuestionService>();
            var questions = questionService.GetList("Id in ({0})".Format2(questionIds.ToStringBySplit(",", "'")));

            detectionReplys.ForEach(tpr =>
            {
                var question = questions.FirstOrDefault(q => q.Id == tpr.QuestionId);
                if (question == null) return;

                var isRight = questionService.IsRight(question, tpr.Value);
                if(isRight == true)
                {
                    tpr.Score = tpr.Subject.Score;
                }
                else if (isRight == false)
                {
                    tpr.Score = 0;
                }
            });
        }

        /// <summary>
        /// 根据handId获取成绩集合
        /// </summary>
        /// <param name="handIds"></param>
        /// <returns></returns>
        public List<TestPaperHandScore> GetTestPaperScores(List<int> handIds)
        {
            var sql = "select sum(T_DetectionReply.Score)Score, DetectionHandId from T_DetectionReply where IsDeleted = 'undeleted'";
            var sqlWhere = " and DetectionHandId in ({0}) group by DetectionHandId ";
            var str = "";
            handIds.ForEach(u => {
                str += u + ",";
            });
            sqlWhere = string.Format(sqlWhere, str.Substring(0, str.Length - 1));
            sql += sqlWhere;
            var result = EkpDbService.GetList<TestPaperHandScore>(sql);

            return result;
        }
    }
}
