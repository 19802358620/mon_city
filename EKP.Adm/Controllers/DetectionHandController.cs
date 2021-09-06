using EKP.Base.Controllers;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.DetectionHand;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EKP.Adm.Controllers
{
    public class DetectionHandController : EntityController<T_DetectionHand>
    {
        private readonly IDetectionHandService detectionHandService = Ioc.GetService<IDetectionHandService>();

        public DetectionHandController()
            : base(Ioc.GetService<IDetectionHandService>())
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
        public ActionResult Pager(DetectionHandPagerParam param)
        {
            return Json(detectionHandService.GetPager<DetectionHandPagerModel>(param, "T_DictValue", "T_User", "T_Detection"));
        }

        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var model = detectionHandService.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_DetectionHand, DetectionHandPagerModel>(model);
            return Json(detail);
        }

        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            return Json(base.Delete(ids.ToArray()));
        }

        /// <summary>
        /// 答题学生分页
        /// </summary>
        public ActionResult AnswerUserPager()
        {
            return View();
        }

        /// <summary>
        /// 答题学生分页post
        /// </summary>
        [HttpPost]
        public ActionResult AnswerUserPager(AnswerUserPagerParam param)
        {
            return Json(detectionHandService.GetAnswerUserPager<AnswerUserPagerModel>(param));
        }

        /// <summary>
        /// 删除学生作答post
        /// </summary>
        [HttpPost]
        public ActionResult DeleleUserAnswer(int userId, int projectId)
        {
            var detectionHands = detectionHandService.GetPager<DetectionHandPagerModel>(new DetectionHandPagerParam
            {
                ProjectId = projectId,
                CreateBy = userId
            }, new string[] { "T_DictValue", "T_User", "T_Detection" }).Rows.ToModel<List<DetectionHandPagerModel>, List<T_DetectionHand>>();

            detectionHands.ForEach(d => d.IsDeleted = IsDelete.deleted.ToString());

            using (var trans = EkpDbService.CreateEntityTrans())
            {
                detectionHands.ForEach(trans.Update);
                trans.SaveChange();
            }

            return Json(DialogFactory.Create(DialogType.Success));
        }

        /// <summary>
        /// 个人成绩分页post
        /// </summary>
        [HttpPost]
        public ActionResult StuedntScorePager(StuedntScorePagerParam param)
        {
            return Json(detectionHandService.GetStuedntScorePager<StuedntScorePagerModel>(param, "T_DictValue", "T_User", "T_Detection", "T_Project"));
        }

        /// <summary>
        /// 班级成绩分页post
        /// </summary>
        [HttpPost]
        public ActionResult ClassScorePager(ClassScorePagerParam param)
        {
            return Json(detectionHandService.GetClassScorePager<ClassScorePagerModel>(param, "T_DictValue", "T_User", "T_Class", "T_Detection", "T_Project"));
        }

        /// <summary>
        /// 完成情况post
        /// </summary>
        /// <returns></returns>
        public ActionResult CompareClassScore(int userId, int projectId, int? classId)
        {
            return Json(detectionHandService.GetCompareClassScore<CompareClassScoreModel>(userId, projectId, classId));
        }
    }
}