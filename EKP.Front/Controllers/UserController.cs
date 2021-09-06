using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Role;
using EKP.Service.User;
using EKP.Web.Areas.Base.Application;
using EKP.Front;
using Ge.Infrastructure.Email;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Mvc.Extensions;
using Ge.Infrastructure.Utilities;
using Microsoft.AspNet.Identity;
using EKP.Base;
using EKP.Base.Controllers;
using Ge.Infrastructure.Sms;
using GeetestSDK;
using EKP.Base.Identity;
using Ge.Infrastructure.Extensions;
using Microsoft.AspNet.Identity.Owin;
using EKP.Front.Authority;

namespace EKP.Front.Controllers
{
    /// <summary>
    /// 用户登录、注册、密码找回
    /// </summary>
    public class UserController : BaseController
    {
        private readonly IUserService userService = Ioc.GetService<IUserService>();
        private readonly IRoleService roleService = Ioc.GetService<IRoleService>();

        #region View

        /// <summary>
        /// 登录
        /// </summary>
        [FrontFilter.Disabled]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        [FrontFilter.Disabled]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        [FrontFilter.Disabled]
        public ActionResult ForgotPwd()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        [FrontFilter.Disabled]
        public ActionResult SettingPwd()
        {
            return View();
        }

        #endregion

        #region 基础业务
        /// <summary>
        /// 个人中心用户资料编辑
        /// </summary>
        [HttpPost]
        public ActionResult Edit(UserEditModel model)
        {
            try
            {
                userService.Update(model, "Type", "RealName", "Education", "Photo", "QQ");
                return Json(DialogFactory.Create(DialogType.Success, string.Empty, "保存成功"));
            }
            catch (Exception ex)
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "保存失败[" + ex.Message + "]"));
            }
        }
        #endregion

        #region 密码相关业务

        /// <summary>
        /// 找回密码Post
        /// </summary>
        [HttpPost]
        public ActionResult ForgotValidCode(string code)
        {
            if (!CheckForgotPwdValidCode(code))
                return Json(DialogFactory.Create(DialogType.Error, "验证码有误或已超时"));
            else
                return Json(DialogFactory.Create(DialogType.Success, string.Empty, "验证码验证成功"));
        }

        /// <summary>
        /// 修改密码Post
        /// </summary>
        [HttpPost]
        public ActionResult SettingPwd(ResetPasswordModel model, string token)
        {
            if (!CheckForgotPwdValidCode(token))
                return Json(DialogFactory.Create(DialogType.Error, "修改密码操作已过期，请在5分钟内修改密码！"));

            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }

            var user = userService.GetEntiy(string.Format("Account = '{0}'", model.Account));
            if (user == null)
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Format("账户'{0}'不存在", model.Account)));
            }
            user.PassWord = Md5Encrypt.Md5EncryptPassword(model.Password);
            userService.Update(user);
            SessionManager.Remove(WebSiteSessionType.FindPasswordToken);
            SessionManager.Remove(WebSiteSessionType.FindPasswordTime);
            return Json(DialogFactory.Create(DialogType.Success, "密码修改成功"));
        }

        /// <summary>
        /// 修改密码Post
        /// </summary>
        [HttpPost]
        public ActionResult ChangePwd(ChangePwdModel model)
        {
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }

            var user = userService.GetEntiy(string.Format("Account = '{0}'", model.Account));
            if (user == null)
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Format("账户'{0}'不存在", model.Account)));
            }
            if (!user.PassWord.Equals(Md5Encrypt.Md5EncryptPassword(model.OldPassword)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Format("旧密码输入有误")));
            }
            user.PassWord = Md5Encrypt.Md5EncryptPassword(model.Password);
            userService.Update(user, "PassWord");
            return Json(DialogFactory.Create(DialogType.Success, "密码修改成功"));
        }
        #endregion

        #region 登录业务

        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }

            if (!CheckGeeTestResult())
                return Json(DialogFactory.Create(DialogType.Error, "请先完成验证"));

            var result = new JavaScriptResult();
            var password = Md5Encrypt.Md5EncryptPassword(model.Password).ToString().Trim();
            var user = userService.GetEntiy(string.Format("(Account = '{0}' or Phone='{0}' or Email='{0}') and Password='{1}' and IsDeleted = '{2}'",
                model.Name, password, IsDelete.undeleted));
            if (user == null)
            {
                return Json(DialogFactory.Create(DialogType.Error, "账号或密码错误"));
            }

            var identityUser = new IdentityUser(user);
            identityUser.LoginMethod = (LoginMethod)model.LoginMethod;
            identityUser.LoginRoleId = user.RoleId.ToString();
            var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            signInManager.SignIn(identityUser, false);

            return Json(DialogFactory.Create(DialogType.Success, "登录成功"));
        }

        private bool CheckGeeTestResult()
        {
            var geetest = new GeetestLib(AppSettingHelper.Get("GeekTestId"), AppSettingHelper.Get("GeekTestKey"));
            var gtServerStatusCode = (byte)Session[GeetestLib.gtServerStatusSessionKey];
            var userId = (string)Session["userID"];

            var challenge = Request.Form.Get(GeetestLib.fnGeetestChallenge);
            var validate = Request.Form.Get(GeetestLib.fnGeetestValidate);
            var seccode = Request.Form.Get(GeetestLib.fnGeetestSeccode);
            var result = gtServerStatusCode == 1 ? geetest.enhencedValidateRequest(challenge, validate, seccode, userId) : geetest.failbackValidateRequest(challenge, validate, seccode);
            return result == 1;
        }

        #endregion

        #region 公共业务
        private bool CheckRegisterValidCode(string code)
        {
            var session_token = SessionManager.Get(WebSiteSessionType.ValidCodeTime) == null ? null : SessionManager.Get(WebSiteSessionType.ValidCode).ToString();
            var session_time = SessionManager.Get(WebSiteSessionType.ValidCodeTime) == null
                ? (DateTime?)null
                : Convert.ToDateTime(SessionManager.Get(WebSiteSessionType.ValidCodeTime));

            if (string.IsNullOrEmpty(session_token) || session_time == null ||
                session_token != code || Convert.ToDateTime(session_time).AddMinutes(5) < DateTime.Now)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool CheckForgotPwdValidCode(string code)
        {
            var session_token = SessionManager.Get(WebSiteSessionType.FindPasswordToken) == null ? null : SessionManager.Get(WebSiteSessionType.FindPasswordToken).ToString();
            var session_time = SessionManager.Get(WebSiteSessionType.FindPasswordTime) == null
                ? (DateTime?)null
                : Convert.ToDateTime(SessionManager.Get(WebSiteSessionType.FindPasswordTime));

            if (string.IsNullOrEmpty(session_token) || session_time == null ||
                session_token != code || Convert.ToDateTime(session_time).AddMinutes(5) < DateTime.Now)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckBindValidCode(string code)
        {
            var session_token = SessionManager.Get(WebSiteSessionType.BindValidCodeToken) == null ? null : SessionManager.Get(WebSiteSessionType.BindValidCodeToken).ToString();
            var session_time = SessionManager.Get(WebSiteSessionType.BindValidCodeTime) == null
                ? (DateTime?)null
                : Convert.ToDateTime(SessionManager.Get(WebSiteSessionType.BindValidCodeTime));

            if (string.IsNullOrEmpty(session_token) || session_time == null ||
                session_token != code || Convert.ToDateTime(session_time).AddMinutes(5) < DateTime.Now)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}