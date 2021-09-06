using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Class;
using EKP.Service.Detection;
using EKP.Service.Question;
using EKP.Service.Subject;
using EKP.Service.DetectionReply;
using EKP.Service.User;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Pager;
using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EKP.Service.DetectionHand
{
    /// <summary>
    /// 试卷管理
    /// </summary>
    public interface IDetectionHandService : IEkpEntityService<T_DetectionHand>
    {
        /// <summary>
        /// 获取交卷详情
        /// </summary>
        T GetDetail<T>(int id, params string[] includePath) where T : DetectionHandPagerModel, new();

        /// <summary>
        /// 获取用户交卷详情
        /// </summary>
        T GetUserDetail<T>(int detectiontId, int userId, params string[] includePath) where T : DetectionHandPagerModel, new();

        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(DetectionHandPagerParam param, params string[] includePath) where T : DetectionHandPagerModel, new();

        /// <summary>
        /// 答题学生分页
        /// </summary>
        JqgridResult<T> GetAnswerUserPager<T>(AnswerUserPagerParam param, params string[] includePath) where T : AnswerUserPagerModel, new();

        /// <summary>
        /// 个人成绩分页
        /// </summary>
        JqgridResult<T> GetStuedntScorePager<T>(StuedntScorePagerParam param, params string[] includePath) where T : StuedntScorePagerModel, new();

        /// <summary>
        /// 班级成绩分页
        /// </summary>
        JqgridResult<T> GetClassScorePager<T>(ClassScorePagerParam param, params string[] includePath) where T : ClassScorePagerModel, new();

        /// <summary>
        /// 完成情况
        /// </summary>
        JqgridResult<T> GetCompareClassScore<T>(int userId, int projectId, int? classId) where T : CompareClassScoreModel, new();
    }

    /// <summary>
    /// 试卷管理
    /// </summary>
    public class DetectionHandService : EkpEntityService<T_DetectionHand>, IDetectionHandService
    {
        /// <summary>
        /// 获取交卷详情
        /// </summary>
        public T GetDetail<T>(int id, params string[] includePath) where T : DetectionHandPagerModel, new()
        {
            var detectionReplyService = Ioc.GetService<IDetectionReplyService>();
            var detectionService = Ioc.GetService<IDetectionService>();

            var detectionHand = this.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_DetectionHand, T>(detectionHand);

            //填充练习信息
            detail.Detection = detectionService.GetEntiy(Convert.ToInt32(detail.DetectiontId)).ToModel<T_Detection, DetectionPagerModel>();

            //填充答题信息
            var detectionReplys = detectionReplyService.GetPager<DetectionReplyPagerModel>(new DetectionReplyPagerParam {
                   DetectionHandId = id,
                   PageSize = Int32.MaxValue
                }).Rows;
            detail.DetectionReplys = detectionReplys;

            //设置得分
            var detectionHandScore = detectionReplyService.GetTestPaperScores(new List<int>() { detail.Id }).FirstOrDefault();
            if (detectionHandScore != null)
            {
                detail.Score = detectionHandScore.Score;
            }

            return detail;
        }

        /// <summary>
        /// 获取用户交卷详情
        /// </summary>
        public T GetUserDetail<T>(int detectiontId, int userId, params string[] includePath) where T : DetectionHandPagerModel, new()
        {
            var detectionHand = this.GetEntiy("DetectiontId='{0}' and CreateBy='{1}' and IsDeleted='{2}'".Format2(detectiontId, userId, IsDelete.undeleted));
            if(detectionHand == null) return null;

            return GetDetail<T>(Convert.ToInt32(detectionHand.Id));
        }

        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(DetectionHandPagerParam param, params string[] includePath) where T : DetectionHandPagerModel, new()
        {
            var detectionReplyService = Ioc.GetService<IDetectionReplyService>();
            var detectionService = Ioc.GetService<IDetectionService>();

            var sql = "select top 99.99999999 percent T_DetectionHand.* {0} from T_DetectionHand {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_DetectionHand.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_User"))
            {
                sqlSelect += " ,(T_User.Account)CreateByAccount ,(T_User.RealName)CreateByRealName ";
                sqlJoin += " left join T_User on T_User.Id = T_DetectionHand.CreateBy ";
            }
            if (includePath.Contains("T_Detection"))
            {
                sqlSelect += "  ";
                sqlJoin += " left join T_Detection on T_Detection.Id = T_DetectionHand.DetectiontId ";
            }
            if (includePath.Contains("T_Site"))
            {
                sqlSelect += " ,(T_Site.Name)SiteName ";
                sqlJoin += " left join T_Site on T_Site.Id = T_User.SiteId ";
            }

            //查询
            if (param.KeyWord != null)
            {
                sqlWhere += string.Format(" and (T_User.Account like '%{0}%') ", param.KeyWord);
            }
            if (param.ProjectId != null)
            {
                sqlWhere += string.Format(" and (T_Detection.ProjectId = '{0}') ", param.ProjectId);
            }
            if (param.Status != null)
            {
                sqlWhere += string.Format(" and (T_Detection.Status = '{0}') ", param.Status);
            }
            if (param.DetectiontId != null)
            {
                sqlWhere += string.Format(" and (T_DetectionHand.DetectiontId = '{0}') ", param.DetectiontId);
            }
            if (param.CreateBy != null)
            {
                sqlWhere += string.Format(" and (T_DetectionHand.CreateBy = '{0}') ", param.CreateBy);
            }

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by {0}{1} {2} ",
                    param.SortBy.Contains(".") ? string.Empty : "T_DetectionHand.", param.SortBy, param.SortOrder);

            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);

            //获取分页
            var pager = EkpDbService.GetPager<T>(sql, param);

            //设置总分
            if (pager.Rows != null && pager.Rows.Count > 0)
            {
                var detectionHandScoreList = detectionReplyService.GetTestPaperScores(pager.Rows.Select(u => u.Id).ToList());
                foreach (var item in pager.Rows)
                {
                    var score = detectionHandScoreList.Where(u => u.DetectionHandId == item.Id).Select(u => u.Score).FirstOrDefault();
                    item.Score = score;
                }
            }

            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        }

        /// <summary>
        /// 答题学生分页
        /// </summary>
        public JqgridResult<T> GetAnswerUserPager<T>(AnswerUserPagerParam param, params string[] includePath) where T : AnswerUserPagerModel, new()
        {
            var classService = Ioc.GetService<IClassService>();

            var sql = "select top 99.99999999 percent T_User.* {0} from T_User {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_User.IsDeleted = '{0}' ", IsDelete.undeleted),
                sqlOrderBy = string.Empty;

            //连接查询
            
            //查询
            if (param.KeyWord != null)
                sqlWhere += string.Format(" and (T_User.Account like '%{0}%' or T_User.RealName like '%{0}%' or T_User.Email like '%{0}%') ", param.KeyWord);
            if (!string.IsNullOrEmpty(param.Gender))
                sqlWhere += string.Format(" and T_User.Gender like '%{0}%' ", param.Gender);
            if (param.RoleId != null)
                sqlWhere += string.Format(" and T_User.RoleId = {0} ", param.RoleId);
            if (param.SiteId != null)
                sqlWhere += string.Format(" and T_User.SiteId = '{0}' ", param.SiteId);
            if (!string.IsNullOrEmpty(param.Status))
                sqlWhere += string.Format(" and T_User.Status = '{0}' ", param.Status);
            if (!string.IsNullOrEmpty(param.Grade))
                sqlWhere += string.Format(" and T_Role.Grade = '{0}' ", param.Grade);
            if (param.ClassId != null)
            {
                sqlWhere += string.Format(" and ',' + T_User.ClassIds + ',' like '%,{0},%' ", param.ClassId);
            }
            if (!string.IsNullOrEmpty(param.ClassIds))
            {
                sqlWhere += string.Format(" and (',{0},' like  '%,' + T_User.ClassIds + ',%') ", param.ClassIds);
            }
            if (param.ProjectId != null)
            {
                sqlWhere += " and T_User.Id in( " +
                    "select T_DetectionHand.CreateBy from T_DetectionHand  " +
                    "left join T_Detection on T_Detection.Id = T_DetectionHand.DetectiontId " +
                    "where T_DetectionHand.IsDeleted='undeleted' and T_Detection.ProjectId = '{0}') ".Format2(param.ProjectId);
            }

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by {0}{1} {2} ",
                    param.SortBy.Contains(".") ? string.Empty : "T_User.", param.SortBy, param.SortOrder);

            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);
            var pager = EkpDbService.GetPager<T>(sql, param);

            //填充班级
            var classIds = new List<int>();
            var classes = new List<T_Class>();
            pager.Rows.FindAll(row => !string.IsNullOrEmpty(row.ClassIds)).ForEach(row => {
                row.ClassIds.Split(',').ToList().ForEach(idstr => { classIds.Add(Convert.ToInt32(idstr)); });
            });
            classes = classService.GetList(classIds.ToArray());
            pager.Rows.FindAll(row => !string.IsNullOrEmpty(row.ClassIds)).ForEach(row => {
                row.ClassIds.Split(',').ToList().ForEach(idstr => {
                    var class_ = classes.FirstOrDefault(c => c.Id == Convert.ToInt32(idstr));
                    if (class_ == null) return;
                    row.ShowClasses += "," + class_.Name;
                });
                row.ShowClasses = row.ShowClasses.RelpaceFirst(",", string.Empty);
            });

            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        }

        /// <summary>
        /// 个人成绩分页
        /// </summary>
        public JqgridResult<T> GetStuedntScorePager<T>(StuedntScorePagerParam param, params string[] includePath) where T : StuedntScorePagerModel, new()
        {
            var detectionReplyService = Ioc.GetService<IDetectionReplyService>();
            var classService = Ioc.GetService<IClassService>();

            var sql = "select top 99.99999999 percent {0} from T_DetectionHand {1} {2} group by T_User.Id, T_User.Account, T_User.RealName, T_User.ClassIds, T_Detection.ProjectId {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_DetectionHand.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_User"))
            {
                sqlSelect += " (T_User.Id)CreateBy, (T_User.Account)CreateByAccount, (T_User.RealName)CreateByRealName, (T_User.ClassIds)CreateByClassIds ";
                sqlJoin += " left join T_User on T_User.Id = T_DetectionHand.CreateBy ";
            }
            if (includePath.Contains("T_Detection"))
            {
                sqlSelect += "  ";
                sqlJoin += " left join T_Detection on T_Detection.Id = T_DetectionHand.DetectiontId ";
            }
            if (includePath.Contains("T_Project"))
            {
                sqlSelect += " ,(T_Detection.ProjectId)ProjectId ";
                sqlJoin += " left join T_Project on T_Project.Id = T_Detection.ProjectId ";
            }
            if (includePath.Contains("T_Site"))
            {
                sqlSelect += " ,(T_Site.Name)SiteName ";
                sqlJoin += " left join T_Site on T_Site.Id = T_User.SiteId ";
            }

            //查询
            if (param.KeyWord != null)
            {
                sqlWhere += string.Format(" and (T_User.Account like '%{0}%' or  or T_User.RealName like '%{0}%') ", param.KeyWord);
            }
            if (param.ProjectId != null)
            {
                sqlWhere += string.Format(" and (T_Detection.ProjectId = '{0}') ", param.ProjectId);
            }
            if (param.Status != null)
            {
                sqlWhere += string.Format(" and (T_DetectionHand.Status = '{0}') ", param.Status);
            }
            if (param.DetectiontId != null)
            {
                sqlWhere += string.Format(" and (T_DetectionHand.DetectiontId = '{0}') ", param.DetectiontId);
            }
            if (param.CreateBy != null)
            {
                sqlWhere += string.Format(" and (T_DetectionHand.CreateBy = '{0}') ", param.CreateBy);
            }
            if (param.CreateByClassIds != null)
            {
                sqlWhere += string.Format(" and (',{0},' like  '%,' + T_User.ClassIds + ',%') ", param.CreateByClassIds);
            }
            if (param.ClassId != null)
            {
                sqlWhere += string.Format(" and ',' + T_User.ClassIds + ',' like '%,{0},%' ", param.ClassId);
            }

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by {0}{1} {2} ",
                    param.SortBy.Contains(".") ? string.Empty : "T_DetectionHand.", param.SortBy, param.SortOrder);

            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);

            //获取分页
            var pager = EkpDbService.GetPager<T>(sql, param);

            //设置班级
            var classes = classService.GetList(string.Empty);
            pager.Rows.ForEach(row =>
            {
                if (!string.IsNullOrEmpty(row.CreateByClassIds))
                {
                    var classIds = row.CreateByClassIds.Split(',').Select(c => Convert.ToInt32(c)).ToList();
                    row.CreateByClasses = classes.FindAll(c => classIds.Contains(c.Id)).ToModel<List<T_Class>, List<ClassPagerModel>>();
                    row.ShowCreateByClasses = row.CreateByClasses.Select(c => c.Name).ToList().ToStringBySplit(",");
                }
            });

            //设置总分
            if (pager.Rows != null && pager.Rows.Count > 0)
            {
                var detectionReplys = detectionReplyService.GetPager<DetectionReplyPagerModel>(new DetectionReplyPagerParam
                {
                    PageSize = 99999
                }, new string[] { "T_DetectionHand", "T_Detection" }).Rows;
                foreach (var item in pager.Rows)
                {
                    var score = detectionReplys.Where(u => u.ProjectId == item.ProjectId && u.CreateBy == item.CreateBy).Sum(u => Convert.ToDouble(u.Score));
                    item.Score = score;
                }
            }

            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        }

        /// <summary>
        /// 班级成绩分页
        /// </summary>
        public JqgridResult<T> GetClassScorePager<T>(ClassScorePagerParam param, params string[] includePath) where T : ClassScorePagerModel, new()
        {
            var detectionReplyService = Ioc.GetService<IDetectionReplyService>();
            var classService = Ioc.GetService<IClassService>();
            var userService = Ioc.GetService<IUserService>();
            var subjectService = Ioc.GetService<ISubjectService>();

            var sql = "select top 99.99999999 percent {0} from T_DetectionHand {1} {2} group by T_Class.Id, T_Class.Name, T_Detection.ProjectId {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_DetectionHand.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_User"))
            {
                sqlSelect += "  ";
                sqlJoin += " left join T_User on T_User.Id = T_DetectionHand.CreateBy  ";
            }
            if (includePath.Contains("T_Class"))
            {
                sqlSelect += " (T_Class.Id)ClassId, (T_Class.Name)ClassName ";
                sqlJoin += " left join T_Class on (',{0},' like  '%,' + convert(varchar(11), T_Class.Id) + ',%')".Format2(param.ClassIds);
            }
            if (includePath.Contains("T_Detection"))
            {
                sqlSelect += "  ";
                sqlJoin += " left join T_Detection on T_Detection.Id = T_DetectionHand.DetectiontId ";
            }
            if (includePath.Contains("T_Project"))
            {
                sqlSelect += " ,(T_Detection.ProjectId)ProjectId ";
                sqlJoin += " left join T_Project on T_Project.Id = T_Detection.ProjectId ";
            }
            if (includePath.Contains("T_Site"))
            {
                sqlSelect += " ,(T_Site.Name)SiteName ";
                sqlJoin += " left join T_Site on T_Site.Id = T_Class.SiteId ";
            }

            //查询
            if (param.KeyWord != null)
            {
                sqlWhere += string.Format(" and (T_Class.Name like '%{0}%') ", param.KeyWord);
            }
            if (param.ProjectId != null)
            {
                sqlWhere += string.Format(" and (T_Detection.ProjectId = '{0}') ", param.ProjectId);
            }
            if (param.Status != null)
            {
                sqlWhere += string.Format(" and (T_DetectionHand.Status = '{0}') ", param.Status);
            }
            if (param.DetectiontId != null)
            {
                sqlWhere += string.Format(" and (T_DetectionHand.DetectiontId = '{0}') ", param.DetectiontId);
            }

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
            {
                sqlOrderBy = string.Format(" order by {0}{1} {2} ",
                    param.SortBy.Contains(".") ? string.Empty : "T_DetectionHand.", param.SortBy, param.SortOrder);
            }

            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);

            //获取分页
            var pager = EkpDbService.GetPager<T>(sql, param);

            //获取所有题目
            var subjects = subjectService.GetPager<SubjectPagerModel>(new SubjectPagerParam
            {
                PageSize = Int32.MaxValue,
                ProjectId = param.ProjectId
            }).Rows;

            //每个学生分数情况
            var allStudentScores = this.GetStuedntScorePager<StuedntScorePagerModel>(new StuedntScorePagerParam
            {
                Status = DetectionHandStatus.Examed.ToString(),
                ProjectId = param.ProjectId,
                SortBy = "T_User.Account",
                PageSize = Int32.MaxValue
            }, "T_DictValue", "T_User", "T_Detection", "T_Project").Rows;

            //完成率
            pager.Rows.ForEach(row =>
            {
                //班级学生总数
                var students = userService.GetList("ClassIds='{0}' and IsDeleted='{1}' and RoleId='{2}'".Format2(row.ClassId, IsDelete.undeleted, 138));

                //班级每个学生分数情况
                var studentScores = allStudentScores.FindAll(ass => students.Exists(s => s.Id == ass.CreateBy));

                //平均分
                row.AverageScore = studentScores.Average(ss => ss.Score);

                //最高分
                row.MaxScore = studentScores.Max(ss => ss.Score);

                //最低分
                row.MinScore = studentScores.Min(ss => ss.Score);

                //提交比例
                row.SubmitRate = "{0}/{1}".Format2(studentScores.Count(), students.Count());

                //满分
                row.TotalScore = subjects.Sum(s => s.Score);

                //得分率
                if(studentScores.Count > 0)
                {
                    //高分率
                    row.HighScoreRate = Math.Round((double)studentScores.Count(ss => ss.Score >= row.TotalScore * 0.8) / (double)studentScores.Count, 2);

                    //低分率
                    row.LowScoreRate = Math.Round((double)studentScores.Count(ss => ss.Score <= (double)row.TotalScore * 0.7) / studentScores.Count, 2);

                    //优秀率
                    row.ExcellenceRate = Math.Round((double)studentScores.Count(ss => ss.Score >= (double)row.TotalScore * 0.9) / studentScores.Count, 2);

                    //及格率
                    row.PassRate = Math.Round((double)studentScores.Count(ss => ss.Score >= row.TotalScore * 0.6) / (double)studentScores.Count, 2);
                }

            });

            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        }

        /// <summary>
        /// 完成情况
        /// </summary>
        public JqgridResult<T> GetCompareClassScore<T>(int userId, int projectId, int? classId) where T : CompareClassScoreModel, new()
        {
            var userService = Ioc.GetService<IUserService>();
            var subjectService = Ioc.GetService<ISubjectService>();
            var detectionService = Ioc.GetService<IDetectionService>();
            var user = userService.GetEntiy(userId);
            var detection = detectionService.GetEntiy("ProjectId='{0}' and IsDeleted='{1}'".Format2(projectId, IsDelete.undeleted));

            //所有题目
            var subjects = subjectService.GetPager<SubjectPagerModel>(new SubjectPagerParam
            {
                Page = 1,
                PageSize = Int32.MaxValue,
                DetectionId = detection.Id,
                SortBy = "CreateTime",
                SortOrder = "asc"
            });

            //总学生数
            var totalStudentCount = userService.GetPager<UserPagerModel>(new UserPagerParam
            {
                Page=1,
                PageSize =1,
                ClassIds = classId == null ? user.ClassIds : classId.ToString(),
                RoleId = 138
            }).TotalRecords;

            //作答信息
            var answerInfosSql = @"select T_DetectionReply.SubjectId, T_DetectionReply.Value, T_DetectionReply.Score, (T_Subject.Score)CorrectScore  from T_DetectionReply
                        left join T_DetectionHand on T_DetectionHand.Id = T_DetectionReply.DetectionHandId
                        left join T_Detection on T_Detection.Id = T_DetectionHand.DetectiontId
                        left join T_Project on T_Project.Id = T_Detection.ProjectId
                        left join T_Subject on T_Subject.Id = T_DetectionReply.SubjectId
                        left join T_User on T_User.Id = T_DetectionHand.CreateBy
                        where T_Project.Id = '{0}'  and T_DetectionHand.IsDeleted = 'undeleted' and T_Subject.IsDeleted = 'undeleted' and T_DetectionReply.IsDeleted = 'undeleted' {1}";
            var answerInfosSqlWhere = string.Empty;
            if(classId != null)
            {
                answerInfosSqlWhere += string.Format(" and ',' + T_User.ClassIds + ',' like '%,{0},%' ", classId);
            }
            else
            {
                answerInfosSqlWhere += string.Format(" and (',{0},' like  '%,' + T_User.ClassIds + ',%') ", user.ClassIds);
            }
            answerInfosSql = answerInfosSql.Format2(projectId, answerInfosSqlWhere);
            var answerInfos = EkpDbService.GetDt(answerInfosSql);

            //计算统计信息
            var compareClassScoreRows = new List<T>();
            subjects.Rows.ForEach(subject => {
                var compareClassScore = new T { SubjectId = subject.Id, SubjectName = subject.Name };
                var answerCount = answerInfos.ToList().Count(row => row["SubjectId"].ToString() == subject.Id.ToString());
                var answerValueCount = answerInfos.ToList().Count(row => row["SubjectId"].ToString() == subject.Id.ToString() && row["Value"].ToString() != string.Empty);
                var correctCount = answerInfos.ToList().Count(row => row["SubjectId"].ToString() == subject.Id.ToString() && row["Score"].ToString() == row["CorrectScore"].ToString());
                if (totalStudentCount > 0)
                {
                    compareClassScore.CompletyRate = Math.Round((double)answerValueCount / (double)totalStudentCount, 2);
                }
                if(answerCount > 0)
                {
                    compareClassScore.CorrectRate = Math.Round((double)correctCount / (double)answerCount, 2);
                }
                compareClassScoreRows.Add(compareClassScore);
            });

            return new JqgridResult<T>()
            {
                PageSize = Int32.MaxValue,
                Page = 1,
                Rows = compareClassScoreRows,
                TotalRecords = compareClassScoreRows.Count
            };
        }
    }
}
