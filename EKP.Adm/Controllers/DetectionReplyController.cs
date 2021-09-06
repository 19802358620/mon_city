using EKP.Base.Controllers;
using EKP.Base.Identity;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.DetectionHand;
using EKP.Service.Subject;
using EKP.Service.DetectionReply;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Mvc.Extensions;
using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 练习题回答管理
    /// </summary>
    public class DetectionReplyController : EntityController<T_DetectionReply>
    {
        private readonly ISubjectService subjectService = Ioc.GetService<ISubjectService>();
        private readonly IDetectionHandService detectionHandService = Ioc.GetService<IDetectionHandService>();
        private readonly IDetectionReplyService detectionReplyService = Ioc.GetService<IDetectionReplyService>();

        public DetectionReplyController()
            : base(Ioc.GetService<IDetectionReplyService>())
        {

        }

        /// <summary>
        /// 提交练习题回答post
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitAll(List<DetectionReplyCreateModel> models, int? detectionId, string status)
        {
            //参数验证
            if (!(detectionId > 0))
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数错误！"));
            }

            //模型验证
            foreach (var model in models)
            {
                if (!this.ModelValidate(model).IsValid)
                {
                    return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
                }
            }

            //登录验证
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            if (loginInUser == null)
            {
                return Json(DialogFactory.Create(DialogType.Error, "请登录后再操作！"));
            }

            //练习题状态检测
            var detectionHandModel = detectionHandService.GetUserDetail<DetectionHandPagerModel>(Convert.ToInt32(detectionId), Convert.ToInt32(loginInUser.Id));
            if (detectionHandModel != null)
            {
                return Json(DialogFactory.Create(DialogType.Error, "请勿重复提交练习！"));
            }

            //更新回答内容
            var detectionReplys = detectionReplyService.GetPager<DetectionReplyPagerModel>(new DetectionReplyPagerParam
            {
                Page = 1,
                PageSize = Int32.MaxValue,
                CreateBy = Convert.ToInt32(loginInUser.Id),
            }, new string[] { "T_DetectionHand" }).Rows;
            var eidtDetectionReplyModels = new List<DetectionReplyPagerModel>();
            models.ForEach(m =>
            {
                var detectionReplyModel = detectionReplys.FirstOrDefault(tpr => tpr.QuestionId == m.QuestionId);
                if (detectionReplyModel == null)
                {
                    eidtDetectionReplyModels.Add(new DetectionReplyPagerModel
                    {
                        SubjectId = m.SubjectId,
                        QuestionId = m.QuestionId,
                        Value = m.Value,
                        CreateBy = Convert.ToInt32(loginInUser.Id),
                        CreateTime = DateTime.Now,
                        IsDeleted = IsDelete.undeleted.ToString()
                    });
                }
                else
                {
                    detectionReplyModel.Value = m.Value;
                    eidtDetectionReplyModels.Add(detectionReplyModel);
                }
            });

            //填充试卷题目
            var tpSubjectIds = new List<int>();
            tpSubjectIds.AddRange(eidtDetectionReplyModels.Select(row => Convert.ToInt32(row.SubjectId)));
            var subjects = new List<T_Subject>();
            if (tpSubjectIds.Count > 0)
            {
                subjects = subjectService.GetList("Id in ({0})".Format2(tpSubjectIds.ToStringBySplit(",", "'")));
            }
            eidtDetectionReplyModels.ForEach(tpr =>
            {
                var subject = subjects.FirstOrDefault(tps => tps.Id == tpr.SubjectId);
                tpr.Subject = ObjectMapper.Mapper<T_Subject, SubjectPagerModel>(subject);
            });

            //更新评分
            detectionReplyService.SynsScore(eidtDetectionReplyModels);

            //创建提交信息
            var detectionHand = new T_DetectionHand
            {
                DetectiontId = detectionId,
                LastSubmitTime = DateTime.Now,
                Status = DetectionHandStatus.Examed.ToString(),
                CreateBy = Convert.ToInt32(loginInUser.Id),
                CreateTime = DateTime.Now,
                IsDeleted = IsDelete.undeleted.ToString()
            };
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                detectionHandService.Add(detectionHand);
                eidtDetectionReplyModels.ForEach(tprModel =>
                {
                    tprModel.DetectionHandId = detectionHand.Id;
                    var tpr = ObjectMapper.Mapper<DetectionReplyPagerModel, T_DetectionReply>(tprModel);
                    if (tpr.Id > 0)
                    {
                        trans.Update(tpr);
                    }
                    else
                    {
                        trans.Add(tpr);
                    }
                });
                detectionHandService.Update(detectionHand);
                trans.SaveChange();
            }

            return Json(DialogFactory.Create(DialogType.Success));
        }

        /// <summary>
        /// 保存评分post
        /// </summary>
        [HttpPost]
        public ActionResult UpdateScore(List<UpdateScoreModel> models, int? detectionHandId)
        {
            //模型验证
            foreach (var model in models)
            {
                if (!this.ModelValidate(model).IsValid)
                {
                    return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
                }
            }
            if (!(detectionHandId > 0))
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数错误！"));
            }

            //验证分数
            var detectionReplyIds = models.Select(m => Convert.ToInt32(m.Id)).ToList();
            var detectionReplys = detectionReplyService.GetList(detectionReplyIds.ToArray());
            var subjectIds = detectionReplys.Select(tpr => Convert.ToInt32(tpr.SubjectId)).ToList();
            var subjects = subjectService.GetList(subjectIds.ToArray());
            foreach (var model in models)
            {
                var detectionReply = detectionReplys.First(tpr => tpr.Id == model.Id);
                var subject = subjects.First(tps => tps.Id == detectionReply.SubjectId);
                if (model.Score < 0)
                {
                    return Json(DialogFactory.Create(DialogType.Error, "分数不得小于0！"));
                }
                if (model.Score > subject.Score)
                {
                    return Json(DialogFactory.Create(DialogType.Error, "分数不得超过该题最大分值！"));
                }
            }

            //更新交卷状态
            var detectionHand = detectionHandService.GetEntiy(Convert.ToInt32(detectionHandId));
            detectionHand.Status = DetectionHandStatus.MakeScore.ToString();

            //更新数据
            detectionReplys.ForEach(dr =>
            {
                var model = models.First(m => m.Id == dr.Id);
                ObjectMapper.Mapper(model, dr);
            });
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                detectionReplys.ForEach(trans.Update);
                trans.SaveChange();
            }

            return Json(DialogFactory.Create(DialogType.Success));
        }
    }
}