using EKP.Base.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EKP.Front.Controllers
{
    /// <summary>
    /// 站点管理
    /// </summary>
    public class SiteController : BaseController
    {
        /// <summary>
        /// 关于我们
        /// </summary>
        public ActionResult AboutUs()
        {
            return View();
        }

        /// <summary>
        /// 帮助中心
        /// </summary>
        public ActionResult Helper()
        {
            return View();
        }
    }
}
