using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Question;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Utilities;
using System.Collections.Generic;
using System.Linq;
using System;
using Ge.Infrastructure.Excel;
using System.Data;
using Newtonsoft.Json;
using EKP.Service.SubjectQuestion;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace EKP.Service.Subject
{
    /// <summary>
    /// 题目管理
    /// </summary>
    public interface ISubjectService : IEkpEntityService<T_Subject>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(SubjectPagerParam param, params string[] includePath) where T : SubjectPagerModel, new();

        /// <summary>
        /// 检测答题是否做对，并返回检测结果
        /// </summary>
        /// <returns></returns>
        List<CheckAnswerModel> GetCheckAnswers(List<CheckAnswerParam> param);

        /// <summary>
        /// 判断答题是否做对
        /// </summary>
        /// <param name="answer"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        bool? IsAnswerRight(string answer, SubjectPagerModel subject);

    }

    /// <summary>
    /// 题目管理
    /// </summary>
    public class SubjectService : EkpEntityService<T_Subject>, ISubjectService
    {
        private readonly IQuestionService questionService = Ioc.GetService<IQuestionService>();

        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(SubjectPagerParam param, params string[] includePath) where T : SubjectPagerModel, new()
        {
            var sql = "select top 99.99999999 percent T_Subject.* {0} from T_Subject {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_Subject.IsDeleted = '{0}'  ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询
            sqlJoin += " left join T_Detection on  T_Detection.Id = T_Subject.DetectionId ";
            sqlWhere += " and T_Detection.IsDeleted = '{0}' ".Format2(IsDelete.undeleted);

            //查询
            if (!string.IsNullOrEmpty(param.KeyWord))
            {
                sqlWhere += string.Format(" and (T_Subject.Name like '%{0}%') ", param.KeyWord);
            }
            if (!string.IsNullOrEmpty(param.Type))
            {
                sqlWhere += string.Format(" and (T_Subject.Type = '{0}') ", param.Type);
            }
            if (param.DetectionId != null)
            {
                sqlWhere += string.Format(" and (T_Subject.DetectionId = '{0}') ", param.DetectionId);
            }
            if (param.CreateBy != null)
            {
                sqlWhere += string.Format(" and (T_Subject.CreateBy = '{0}') ", param.CreateBy);
            }
            if (param.Ids != null && param.Ids.Count > 0)
            {
                sqlWhere += string.Format(" and T_Subject.Id in ({0}) ", param.Ids.ToStringBySplit(",", "'"));
            }
            if (param.ProjectId != null)
            {
                sqlWhere += string.Format(" and (T_Detection.ProjectId = '{0}') ", param.ProjectId);
            }

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by {0}{1} {2} ",
                    param.SortBy.Contains(".") ? string.Empty : "T_Subject.", param.SortBy, param.SortOrder);

            //题目
            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);
            var pager = EkpDbService.GetPager<T>(sql, param);

            //问题
            var subjectIds = new List<int>();
            var questions = new List<T_Question>();
            subjectIds.AddRange(pager.Rows.Select(row => row.Id));
            if (subjectIds.Count > 0)
            {
                questions = questionService.GetList("SubjectId in ({0})".Format2(subjectIds.ToStringBySplit(",", "'")));
            }
            pager.Rows.ForEach(row =>
            {
                var questionModels = questions.FindAll(q => q.SubjectId == row.Id);
                row.Questions = new List<QuestionPagerModel>();
                row.Questions.AddRange(ObjectMapper.Mapper<List<T_Question>, List<QuestionPagerModel>>(questionModels));
            });

            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        }

        /// <summary>
        /// 检测答题是否做对，并返回检测结果
        /// </summary>
        /// <returns></returns>
        public List<CheckAnswerModel> GetCheckAnswers(List<CheckAnswerParam> param)
        {
            var checkAnswerModels = new List<CheckAnswerModel>();

            var subjectIds = param.Select(row => Convert.ToInt32(row.SubjectId)).ToList();
            var subjects = this.GetPager<SubjectPagerModel>(new SubjectPagerParam
            {
                Page = 1,
                PageSize = Int32.MaxValue,
                Ids = subjectIds
            }).Rows;
            subjects.ForEach(subject =>
            {
                var checkAnswerParam = param.First(p => p.SubjectId == subject.Id);
                var checkAnswerModel = new CheckAnswerModel()
                {
                    Answer = checkAnswerParam.Answer,
                    Subject = subject,
                    IsRight = IsAnswerRight(checkAnswerParam.Answer, subject)
                };
                checkAnswerModels.Add(checkAnswerModel);
            });


            return checkAnswerModels;
        }

        /// <summary>
        /// 判断题目是做对
        /// </summary>
        /// <param name="answer"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public bool? IsAnswerRight(string answer, SubjectPagerModel subject)
        {
            var isRight = (bool?)null;

            foreach (var q in subject.Questions)
            {
                if (q.Answer != answer)
                {
                    isRight = false;
                    break;
                }
                else
                {
                    isRight = true;
                }
            }

            return isRight;
        }

        /// <summary>
        /// 获取选择题所有可能的选项值
        /// </summary>
        /// <returns></returns>
        public List<string> GetChoices()
        {
            var arr = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "Z" };
            var arr2 = new string[] { "(A)", "(B)", "(C)", "(D)", "(E)", "(F)", "(G)", "(H)", "(I)", "(J)", "(K)", "(L)", "(M)", "(N)", "(O)", "(P)", "(Q)", "(R)", "(S)", "(T)", "(U)", "(V)", "(X)", "(Y)", "(Z)" };

            var list = new List<string>();
            list.AddRange(arr.ToArray());
            list.AddRange(arr2.ToArray());

            return list;
        }
    }
}
