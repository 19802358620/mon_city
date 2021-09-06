using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.Ado;
using EKP.Service.User;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Metronicv;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Metronicv.Dialog;
using Newtonsoft.Json.Linq;
using EKP.Base.Controllers;
using EKP.Base.Identity;

namespace EKP.Api.Controllers
{
    /// <summary>
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 主页
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
    }
}