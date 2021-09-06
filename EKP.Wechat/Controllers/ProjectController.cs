using EKP.Service.Project;
using EKP.Wechat.Controllers.Base;
using Ge.Infrastructure.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EKP.Wechat.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService projectService = Ioc.GetService<IProjectService>();

        //
        // GET: /Home/
        /// <summary>
        /// 项目列表（点击显示项目导学）
        /// </summary>
        /// <returns></returns>
        [WechatActionFilter]
        public ActionResult ProjectList1()
        {
            var modelList = projectService.GetList("Type='project' and IsDeleted='undeleted'");
            
            return View(modelList);
        }
        /// <summary>
        /// 项目列表（点击显示项目任务列表）
        /// </summary>
        /// <returns></returns>
        [WechatActionFilter]
        public ActionResult ProjectList2()
        {
            var modelList = projectService.GetList("Type='project' and IsDeleted='undeleted'");

            return View(modelList);
        }
        /// <summary>
        /// 任务列表（点击显示任务自我检测）
        /// </summary>
        /// <returns></returns>
        [WechatActionFilter]
        public ActionResult TaskList(string projectId)
        {
            var modelList = projectService.GetList("Type='task' and IsDeleted='undeleted' and ParentId='" + projectId + "'");
            
            return View(modelList);
        }
        /// <summary>
        /// 项目导学
        /// </summary>
        /// <returns></returns>
        [WechatActionFilter]
        public ActionResult ProjectGuidance(string projectId)
        {
            var model = projectService.GetEntiy("IsDeleted='undeleted' and Id='" + projectId + "'");

            return View(model);
        }
        
	}
}