using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKP.Base.Controllers;
using EKP.Entity;
using EKP.Service.Question;
using EKP.Service.SubjectQuestion;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Utilities;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 问题管理
    /// </summary>
    public class QuestionController : EntityController<T_Question>
    {
        private readonly IQuestionService questionService = Ioc.GetService<IQuestionService>();

        public QuestionController()
            : base(Ioc.GetService<IQuestionService>())
        {

        }

        /// <summary>
        /// 分页
        /// </summary>
        public ActionResult Pager()
        {
            return View();
        }

        /// <summary>
        /// 分页post
        /// </summary>
        [HttpPost]
        public ActionResult Pager(QuestionPagerParam param)
        {
            return Json(questionService.GetPager<QuestionPagerModel>(param, "T_Subject"));
        }

        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var question = questionService.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_Question, QuestionPagerModel>(question);
            return Json(detail);
        }

        /// <summary>
        /// 创建
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
        public ActionResult Create(QuestionCreateModel model)
        {
            return Json(base.Create(model));
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
        public ActionResult Edit(QuestionEditModel model)
        {
            return Json(Edit(string.Format("id = {0}", model.Id), model,
                "Name", "Options", "Answer"));
        }

        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            return Json(base.Delete(ids.ToArray()));
        }

    }
}