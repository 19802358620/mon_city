using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EKP.Base.Controllers;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Subject;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Utilities;
using Ge.Infrastructure.Mvc.Extensions;
using EKP.Service.SubjectQuestion;
using EKP.Service.Question;
using Ge.Infrastructure.Extensions;
using EKP.Service.DictValue;
using System.Linq;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 题目管理
    /// </summary>
    public class SubjectController : EntityController<T_Subject>
    {
        private readonly ISubjectService subjectService = Ioc.GetService<ISubjectService>();
        private readonly IQuestionService questionService = Ioc.GetService<IQuestionService>();
        private readonly IDictValueService dictValueService = Ioc.GetService<IDictValueService>();

        public SubjectController()
            : base(Ioc.GetService<ISubjectService>())
        {
           
        }

        /// <summary>
        /// 分页post
        /// </summary>
        [HttpPost]
        public ActionResult Pager(SubjectPagerParam param)
        {
            return Json(subjectService.GetPager<SubjectPagerModel>(param));
        }

        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var subject = subjectService.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_Subject, SubjectPagerModel>(subject);

            var questions = questionService.GetList("SubjectId in ('{0}')".Format2(id));
            var questionModels = ObjectMapper.Mapper<List<T_Question>, List<QuestionPagerModel>>(questions);
            detail.Questions = questionModels;

            return Json(detail, "text/html");
        }

        /// <summary>
        /// 添加
        /// </summary>
        public ActionResult Create()
        {
            return PartialView();
        }

        /// <summary>
        /// 创建post
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SubjectCreateModel model)
        {
            //模型数据安全验证
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }
            model.Score = 1;
            var subject = ObjectMapper.Mapper<SubjectCreateModel, T_Subject>(model);
            var questions = new List<T_Question>();

            //根据不同题目类型创建问题
            if (model.Type == SubjectType.single.ToString()) //单选
            {
                questions.Add(new T_Question
                {
                    Type = QuestionType.single.ToString(),
                    Options = model.Options,
                    OptionsColumns = model.OptionsColumns
                });
                if (string.IsNullOrEmpty(model.Options))
                {
                    return Json(DialogFactory.Create(DialogType.Error, "选项不能为空！"));
                }
            }
            else if (model.Type == SubjectType.multi.ToString()) //多选
            {
                questions.Add(new T_Question
                {
                    Type = QuestionType.multi.ToString(),
                    Options = model.Options,
                    OptionsColumns = model.OptionsColumns
                });
                if (string.IsNullOrEmpty(model.Options))
                {
                    return Json(DialogFactory.Create(DialogType.Error, "选项不能为空！"));
                }
            }
            else if (model.Type == SubjectType.bit.ToString()) //判断
            {
                questions.Add(new T_Question
                {
                    Type = QuestionType.bit.ToString(),
                    Options = model.Options,
                    OptionsColumns = model.OptionsColumns
                });
            }
            else if (model.Type == SubjectType.fill.ToString()) //填空题
            {
                questions.Add(new T_Question
                {
                    Type = QuestionType.fill.ToString()
                });
            }
            else if (model.Type == SubjectType.shortAnswer.ToString()) //简答
            {
                questions.Add(new T_Question
                {
                    Type = QuestionType.shortAnswer.ToString(),
                });
            }
            else
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数错误！"));
            }
            questions.ForEach(q =>
            {
                q.Answer = model.Answer;
                q.CreateBy = model.CreateBy;
                q.CreateTime = model.CreateTime;
                q.IsDeleted = model.IsDeleted;
            });

            //插入题目到数据库
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                trans.Add(subject);
                questions.ForEach(question =>
                {
                    question.SubjectId = subject.Id;
                });
                questions.ForEach(trans.Add);
                trans.SaveChange();
            }

            return Json(DialogFactory.Create(DialogType.Success));
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public ActionResult Edit()
        {
            return PartialView();
        }

        /// <summary>
        /// 编辑post
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(SubjectEditModel model)
        {
            //模型数据安全验证
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }
            var subject = subjectService.GetEntiy(model.Id);
            var questions = questionService.GetList(string.Format("SubjectId='{0}' and IsDeleted='{1}'", model.Id, IsDelete.undeleted));

            //更新题目
            subject.Name = model.Name;
            subject.Analysis = model.Analysis;

            //更新问题
            if (model.Type == SubjectType.single.ToString()) //单选
            {
                questions.ForEach(question =>
                {
                    question.Answer = model.Answer;
                    question.Options = model.Options;
                    question.OptionsColumns = model.OptionsColumns;
                });
                if (string.IsNullOrEmpty(model.Options))
                {
                    return Json(DialogFactory.Create(DialogType.Error, "选项不能为空！"));
                }
            }
            else if (model.Type == SubjectType.multi.ToString()) //多选
            {
                questions.ForEach(question =>
                {
                    question.Answer = model.Answer;
                    question.Options = model.Options;
                    question.OptionsColumns = model.OptionsColumns;
                });
                if (string.IsNullOrEmpty(model.Options))
                {
                    return Json(DialogFactory.Create(DialogType.Error, "选项不能为空！"));
                }
            }
            else if (model.Type == SubjectType.bit.ToString()) //判断
            {
                questions.ForEach(question =>
                {
                    question.Answer = model.Answer;
                    question.Options = model.Options;
                    question.OptionsColumns = model.OptionsColumns;
                });
            }
            else if (model.Type == SubjectType.fill.ToString()) //填空题
            {
                questions.ForEach(question =>
                {
                    question.Answer = model.Answer;
                });
            }
            else if (model.Type == SubjectType.shortAnswer.ToString()) //简答
            {
                questions.ForEach(question =>
                {
                    question.Answer = model.Answer;
                });
            }
            else
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数错误！"));
            }

            //插入题目到数据库
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                trans.Update(subject);
                questions.ForEach(trans.Update);
                trans.SaveChange();
            }

            return Json(DialogFactory.Create(DialogType.Success));
        }

        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            var subjects = subjectService.GetList(ids.ToArray());
            if (subjects.Count == 0)
            {
                return Json(DialogFactory.Create(DialogType.Success));
            }

            var idStrs = ids.ToStringBySplit(",", "'");
            var questions = questionService.GetList("Id in ({0})".Format2(idStrs));

            using (var trans = EkpDbService.CreateEntityTrans())
            {
                subjects.ForEach(subject => subject.IsDeleted = IsDelete.deleted.ToString());
                questions.ForEach(question => question.IsDeleted = IsDelete.deleted.ToString());
                subjects.ForEach(trans.Update);
                questions.ForEach(trans.Update);
                trans.SaveChange();
            }

            return Json(DialogFactory.Create(DialogType.Success));
        }

        /// <summary>
        /// 获取题目类型列表post
        /// </summary>
        [HttpPost]
        public ActionResult SubjectTypes()
        {
            var subjectTypes = EnumHelper.GetList<SubjectType>().Select(subjectType => new
            {
                Value = subjectType.ToString(),
                ShowValue = EnumHelper.GetEnumDescription(subjectType)
            });
            return Json(subjectTypes);
        }

        /// <summary>
        /// 检测答题是否做对，并返回检测结果
        /// </summary>
        public ActionResult CheckAnswers(List<CheckAnswerParam> param)
        {
            return Json(subjectService.GetCheckAnswers(param));
        }
    }
}