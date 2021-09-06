using System;
using System.Web.Mvc;
using EKP.Base;
using EKP.Base.Controllers;
using EKP.Front.Authority;
using Ge.Infrastructure.Utilities;
using GeetestSDK;

namespace EKP.Front.Controllers
{
    /// <summary>
    /// 用户登录、注册、密码找回
    /// </summary>
    public class GeekTestController : BaseController
    {

        /// <summary>
        /// 极验验证码生成
        /// </summary>
        [FrontFilter.Disabled]
        public ActionResult GeekTest()
        {
            return Content(GetCaptcha(), "application/json");
        }
        
        private string GetCaptcha()
        {
            var geetest = new GeetestLib(AppSettingHelper.Get("GeekTestId"), AppSettingHelper.Get("GeekTestKey"));
            var gtServerStatus = geetest.preProcess();
            Session[GeetestLib.gtServerStatusSessionKey] = gtServerStatus;
            return geetest.getResponseStr();
        }
    }
}