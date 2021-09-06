using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.SubjectQuestion;
using Ge.Infrastructure.Metronicv;
using System.Collections.Generic;
using System.Linq;

namespace EKP.Service.Question
{
    /// <summary>
    /// 问题管理
    /// </summary>
    public interface IQuestionService : IEkpEntityService<T_Question>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(QuestionPagerParam param, params string[] includePath) where T : class, new();

        /// <summary>
        /// 判断问题是否做对
        /// </summary>
        bool? IsRight<T>(T question, string value) where T : T_Question, new();
    }

    /// <summary>
    /// 问题管理
    /// </summary>
    public class QuestionService : EkpEntityService<T_Question>, IQuestionService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(QuestionPagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_Question.* {0} from T_Question {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_Question.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_Subject"))
            {
                sqlSelect += " ,(T_Subject.Name)SubjectName ";
                sqlJoin += " left join T_Subject on T_Subject.Id = T_Question.SubjectId ";
            }

            //查询
            if (param.SubjectId != null)
            {
                sqlWhere += string.Format(" and (T_Question.SubjectId = '{0}') ", param.SubjectId);
            }
            if(param.ChapterId != null)
            {
                sqlWhere += string.Format(" and (T_Subject.ChapterId = '{0}') ", param.ChapterId);
            }

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by {0}{1} {2} ",
                    param.SortBy.Contains(".") ? string.Empty : "T_Question.", param.SortBy, param.SortOrder);

            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);

            var pager = EkpDbService.GetPager<T>(sql, param);
            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        }

        /// <summary>
        /// 判断问题是否做对
        /// </summary>
        public bool? IsRight<T>(T question, string value) where T : T_Question, new()
        {
            value = value == null ? string.Empty : value.Trim();
            question.Answer = question.Answer == null ? string.Empty : question.Answer.Trim();
            if (question.Type == QuestionType.single.ToString())//单选
            {
                return question.Answer == value;
            }
            else if (question.Type == QuestionType.multi.ToString())//多选
            {
                return question.Answer == value;
            }
            else if (question.Type == QuestionType.bit.ToString())//判断题
            {
                return question.Answer == value;
            }

            return null;
        }
    }
}
