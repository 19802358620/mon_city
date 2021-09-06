using EKP.Service.Project;
using EKP.Service.Subject;
using EKP.Wechat.Controllers.Base;
using Ge.Infrastructure.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EKP.Wechat.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubjectService subjectService = Ioc.GetService<ISubjectService>();

        [WechatActionFilter]
        public ActionResult Index(string projectId)
        {
            
            return View();
        }
        
        
	}
}