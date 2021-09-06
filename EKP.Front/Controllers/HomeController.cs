using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.Ado;
using EKP.Service.Detection;
using EKP.Service.DetectionHand;
using EKP.Service.Subject;
using EKP.Service.User;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Metronicv;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Metronicv.Dialog;
using Newtonsoft.Json.Linq;
using EKP.Base.Controllers;
using EKP.Base.Identity;
using EKP.Service.Site;
using EKP.Base.Cache;
using EKP.Base;
using EKP.Service.Info;
using EKP.Service.DetectionSetting;
using EKP.Service.HomeworkSubmit;

namespace EKP.Front.Controllers
{
    /// <summary>
    /// Home
    /// </summary>
    public class HomeController : BaseController
    {
        private readonly IInfoService infoService = Ioc.GetService<IInfoService>();
        private readonly ISiteService siteService = Ioc.GetService<ISiteService>();
        private readonly ISubjectService subjectService = Ioc.GetService<ISubjectService>();
        private readonly IDetectionService detectionService = Ioc.GetService<IDetectionService>();
        private readonly IDetectionHandService detectionHandService = Ioc.GetService<IDetectionHandService>();
        private readonly IDetectionSettingService detectionSettingService = Ioc.GetService<IDetectionSettingService>();
        private readonly IUserService userService = Ioc.GetService<IUserService>();

        /// <summary>
        /// 首页
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 课程介绍
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseIntroduce()
        {
            return View();
        }

        /// <summary>
        /// 课程学习
        /// </summary>
        /// <returns></returns>
        public ActionResult Project()
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            if (loginInUser == null)
            {
                return RedirectToAction("Login", "User");
            }
            ;
            return View();
        }

        /// <summary>
        /// 课程学习.自我检测
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectSubject()
        {
            return PartialView();
        }

        /// <summary>
        /// 课程学习.自我检测post
        /// </summary>
        [HttpPost]
        public ActionResult ProjectSubject(int? detectionId, int? userId)
        {
            //题目
            var subjects = subjectService.GetPager<SubjectPagerModel>(new SubjectPagerParam
            {
                Page = 1,
                PageSize = Int32.MaxValue,
                DetectionId = detectionId,
                SortBy = "CreateTime",
                SortOrder = "asc"
            });

            //答题信息
            var detectionHand = (DetectionHandPagerModel)null;
            if (userId != null)
            {
                detectionHand = detectionHandService.GetUserDetail<DetectionHandPagerModel>(
                    Convert.ToInt32(detectionId), Convert.ToInt32(userId));
            }

            //练习设置
            var user = userService.GetEntiy(Convert.ToInt32(userId));
            var teacher = userService.GetEntiy("(',' + classIds + ',') like '%,{0},%' and RoleId='137' and IsDeleted='{1}'".Format2(user.ClassIds, IsDelete.undeleted));
            var detectionSetting = detectionSettingService.GetEntiy("DetectionId='{0}' and UserId='{1}' and IsDeleted='{2}'".Format2(detectionId, teacher.Id, IsDelete.undeleted));

            return Json(new
            {
                subjects = subjects,
                detectionHand = detectionHand,
                detectionSetting = detectionSetting
            });
        }

        /// <summary>
        /// 动画演示
        /// </summary>
        /// <returns></returns>
        public ActionResult Video()
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            if (loginInUser == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        /// <summary>
        /// 思考练习
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectGuidance()
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            if (loginInUser == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        /// <summary>
        /// 思考练习设置
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectGuidanceSetting()
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            if (loginInUser == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        /// <summary>
        /// 成绩管理.学生成绩
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentScore()
        {
            return View();
        }

        /// <summary>
        /// 成绩管理.成绩统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ScoreStatistics()
        {
            return View();
        }

        /// <summary>
        /// 成绩统计.查看成绩
        /// </summary>
        /// <returns></returns>
        public ActionResult SeeScore()
        {
            return View();
        }

        /// <summary>
        /// 成绩统计.练习批改
        /// </summary>
        /// <returns></returns>
        public ActionResult MarkScore()
        {
            return View();
        }

        /// <summary>
        /// 动画演示
        /// </summary>
        /// <returns></returns>
        public ActionResult VideoDetail()
        {
            return View();
        }

        /// <summary>
        /// 学生管理.选择班级
        /// </summary>
        public ActionResult ClassChoose()
        {
            return View();
        }

        /// <summary>
        /// 学生管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Student()
        {
            return View();
        }

        /// <summary>
        /// 学生管理.增加
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentCreate()
        {
            return PartialView();
        }

        /// <summary>
        /// 学生管理.编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentEdit()
        {
            return PartialView();
        }

        /// <summary>
        /// 知识拓展
        /// </summary>
        /// <returns></returns>
        public ActionResult Knowledge()
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            if (loginInUser == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }
        /// <summary>
        /// 离线作业
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeworkPager()
        {
            return View();
        }
        /// <summary>
        /// 离线作业的增加
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeworkCreate()
        {
            return View();
        }
        /// <summary>
        /// 离线作业的增加
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeworkEdit()
        {
            return View();
        }

        /// <summary>
        /// 学生提交作业页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Offlinework()
        {
            return View();
        }
        /// <summary>
        /// 打开提交作业页面
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeworkSubmit()
        {
            return View();
        }
        /// <summary>
        /// 打开要批改的作业的页面
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeworkScore(int id)
        {
            //int Id = Int32.Parse(Request.Form["id"].ToString());
            //Session["HomeworkId"] = Id;
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 打开批改作业页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ScoreView()
        {
            return View();
        }

        /// <summary>
        /// 打开提交答案
        /// </summary>
        /// <returns></returns>
        public ActionResult Submitanswer()
        {
            return View();
        }

        /// <summary>
        /// 查看答案视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Seeanswer()
        {
            return View();
        }

        public ActionResult HomeworkDetail()
        {
            return View();
        }


        /// <summary>
        /// 信息发布
        /// </summary>
        /// <returns></returns>
        public ActionResult NoticeTeacherPager()
        {
            return View();
        }
        /// <summary>
        /// 信息发布
        /// </summary>
        /// <returns></returns>
        public ActionResult NoticeStudentPager()
        {
            return View();
        }
        
        /// <summary>
        /// 发布通知
        /// </summary>
        /// <returns></returns>
        public ActionResult NoticeCreate()
        {
            return View();
        }
        /// <summary>
        /// 修改通知
        /// </summary>
        /// <returns></returns>
        public ActionResult NoticeEdit()
        {
            return View();
        }
        public ActionResult NoticeDetail()
        {
            return View();
        }
        /// <summary>
        /// 备课中心
        /// </summary>
        /// <returns></returns>
        public ActionResult ResourceTeacherPager()
        {
            return View();
        }

        /// <summary>
        /// 上传资源
        /// </summary>
        public ActionResult ResourceCreate()
        {
            return View();
        }
        /// <summary>
        /// 编辑资源
        /// </summary>
        /// <returns></returns>
        public ActionResult ResourceEdit()
        {
            return View();
        }
        /// <summary>
        /// 资源下载
        /// </summary>
        /// <returns></returns>
        public ActionResult ResourceStudentPager()
        {
            return View();
        }
        /// <summary>
        /// 资源发布班级
        /// </summary>
        /// <returns></returns>
        public ActionResult ResourcePublish()
        {
            return View();
        }
        


    }
}