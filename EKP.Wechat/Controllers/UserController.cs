using EKP.Base;
using EKP.Service.User;
using Ge.Infrastructure.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKP.Wechat.Controllers.Base;
using Ge.Infrastructure.Mvc.Extensions;
using Ge.Infrastructure.Utilities;
using Ge.Infrastructure.Metronicv.Dialog;
using EKP.Service.Base.EkpBaseModel;
using EKP.Base.Identity;
using Newtonsoft.Json2;
using Microsoft.AspNet.Identity.Owin;

namespace EKP.Wechat.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService = Ioc.GetService<IUserService>();
        //
        // GET: /Home/
        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult Login(string url)
        {
            ViewBag.Url = url;
            //Session.RemoveAll();
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLoginModel model, string ValidCode)
        {
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }

            if (Session["ValidCode"] == null ||
                ValidCode.ToLower() != Session["ValidCode"].ToString().ToLower())
            {
                return Json(DialogFactory.Create(DialogType.Error, "验证码有误"));
            }

            var result = new JavaScriptResult();
            var password = Md5Encrypt.Md5EncryptPassword(model.Password).ToString().Trim();
            var user = userService.GetEntiy(string.Format("(Account = '{0}' or Phone='{0}' or Email='{0}') and Password='{1}' and IsDeleted = '{2}'",
                model.Name, password, IsDelete.undeleted));
            if (user == null)
            {
                return Json(DialogFactory.Create(DialogType.Error, "账号或密码错误"));
            }

            var identityUser = new IdentityUser(user);
            identityUser.LoginMethod = LoginMethod.微信登录;
            identityUser.LoginRoleId = user.RoleId.ToString();
            var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            signInManager.SignIn(identityUser, false);

            Session.Remove("ValidCode");
            Session["LoginUserId"] = user.Id;
            Session["LoginUserName"] = user.Account;
            Session["LoginUserRealName"] = user.RealName;
            Session["LoginRoleId"] = user.RoleId;
            if (user.RoleId == 137)
                Session["role"] = "teacher";
            else if (user.RoleId == 138)
                Session["role"] = "student";

            return Json(DialogFactory.Create(DialogType.Success, "登录成功", "登录成功", user.RoleId.ToString()));
            //return Json(user.RoleId);//返回角色Id
        }
        /// <summary>
        /// 退出登录Post
        /// </summary>
        [HttpGet]
        [WechatActionFilter]
        public ActionResult LoginOut()
        {
            Session.RemoveAll();
            return RedirectToAction("Login", "User");
        }

        [WechatActionFilter]
        public ActionResult UserInfo()
        {
            return View();
        }
        [WechatActionFilter]
        [HttpPost]
        public ActionResult GetUserInfo()
        {
            int userid = Convert.ToInt32(Session["LoginUserId"]);
            var user = userService.GetEntiy(string.Format("Id = '{0}' and IsDeleted = '{1}'", userid, IsDelete.undeleted));
            
            var result = new JavaScriptResult();
            result.Script = JsonConvert.SerializeObject(user);
            return result;
        }
        /// <summary>
        /// 修改个人信息
        /// </summary>
        [WechatActionFilter]
        [HttpPost]
        public ActionResult EditUserInfo(string Account, string RealName, string Age, string Sex, string Email, string Telephone)
        {
            int userid = Convert.ToInt32(Session["LoginUserId"]);
            var entity = userService.GetEntiy(userid);
            entity.RealName = RealName;
            entity.Age = string.IsNullOrEmpty(Age) ? 0 : Convert.ToInt32(Age);
            entity.Sex = Sex;
            entity.Email = Email;
            entity.Telephone = Telephone;

            userService.Update(entity, new string[] { "RealName", "Age", "Sex", "Email", "Telephone" });
            return Json("true");
        }
        [WechatActionFilter]
        public ActionResult ModifyPassword()
        {
            return View();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        [WechatActionFilter]
        [HttpPost]
        public ActionResult EditPassword(string Password, string NewPassword, string ReNewPassword)
        {
            if (NewPassword != ReNewPassword)
                return Json("新密码与确认密码不一致");

            int userid = Convert.ToInt32(Session["LoginUserId"]);
            var entity = userService.GetEntiy(userid);
            string password = Md5Encrypt.Md5EncryptPassword(Password).ToString().Trim();
            if (entity.PassWord != password)
                return Json("旧密码不正确");

            string newPassword = Md5Encrypt.Md5EncryptPassword(NewPassword).ToString().Trim();
            entity.PassWord = newPassword;

            userService.Update(entity, new string[] { "PassWord" });
            return Json("true");
        }
        #region 登录验证码
        /// <summary>
        /// 登录验证码
        /// </summary>
        public ActionResult SignInValidCode()
        {
            var validCode = new ValidCodeImage(4);
            var img = validCode.CreateCheckCodeImage();
            var ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            Session["ValidCode"] = validCode.CheckCode;

            return new FileContentResult(ms.ToArray(), "image/Gif");
        }
        #endregion
	}
}