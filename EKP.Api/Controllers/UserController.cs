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
using EKP.Api;
using Ge.Infrastructure.Email;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Mvc.Extensions;
using Ge.Infrastructure.Utilities;
using Microsoft.AspNet.Identity;
using EKP.Base;
using EKP.Base.Controllers;


namespace EKP.Api.Controllers
{
    /// <summary>
    /// 用户登录、注册、密码找回
    /// </summary>
    public class UserController : BaseController
    {
        private readonly IUserService userService = Ioc.GetService<IUserService>();
        private readonly IRoleService roleService = Ioc.GetService<IRoleService>();

        /// <summary>
        /// 登录
        /// </summary>
        public ActionResult Login()
        {
            return View();
        }
        
        /// <summary>
        /// 注册
        /// </summary>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 注册Post
        /// </summary>
        [HttpPost]
        public ActionResult Register(UserRegisterModel model)
        {
            //模型数据安全验证
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }

            //邮箱验证
            if (userService.IsExist(string.Format("Email='{0}'", model.Email)))
            {
                return Json(DialogFactory.Create(DialogType.Error, "错误", "邮箱重复，请更换！"));
            }

            var user = ObjectMapper.Mapper<UserRegisterModel, T_User>(model);
            var role = new T_Role();
            user.Status = UserStatus.Normal.ToString();
            user.PassWord = Md5Encrypt.Md5EncryptPassword(model.PassWord).ToString().Trim();
            user.RoleId = role.Id;
            user.CreateTime = DateTime.Now;
            user.IsDeleted = IsDelete.undeleted.ToString();
            
            userService.Add(user);

            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "注册成功！"));
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        public ActionResult ForgotPwd()
        {
            return View();
        }

        /// <summary>
        /// 找回密码Post
        /// </summary>
        [HttpPost]
        public ActionResult ForgotPwd(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(DialogFactory.Create(DialogType.Error, "请输入邮箱."));
            }

            var user = userService.GetEntiy(string.Format("Email='{0}'", email));
            if (user == null)
            {
                return Json(DialogFactory.Create(DialogType.Error, "邮箱用户不存在."));
            }
            
            var mailContent = new StringBuilder();
            var token = Md5Encrypt.Md5EncryptPassword(new Random().Next(0, 10000).ToString());
            var url = string.Format("http://{0}/WebSite/User/SettingPwd?n={1}&token={2}", Request.Url.Authority, user.Account, token);
            mailContent.Append("尊敬的用户：<br/>");
            mailContent.Append("        您好！你于");
            mailContent.Append(DateTime.Now.ToLongTimeString());
            mailContent.Append(string.Format("通过<a href=\"http://{0}/Index.aspx\" target='_blank'>企业知识服务平台平台</a>申请找回密码。", Request.Url.Authority));
            mailContent.Append("<br/>        为了安全起见，请点击以下链接重设个人密码（5分钟内有效！）：");
            mailContent.Append("<br/>        <a href=\"" + url + "\" target='_blank'>" + url + "</a>");
            var smtp = new SmtpClient("smtp.qq.com")
            {
                Port = 25, //设置邮箱端口，pop3端口:110, smtp端口是:25
                Credentials = new NetworkCredential("734214002", "13436053642")
            };
            var sendEmailService = new EmailService(smtp, "734214002@qq.com");
            var message = new IdentityMessage()
            {
                Destination = user.Email,
                Subject = "企业知识服务平台平台-重置密码",
                Body = mailContent.ToString(),
            };
            try
            {
                sendEmailService.Send(message);
            }
            catch
            {
                return Json(DialogFactory.Create(DialogType.Error, "邮件发送失败，请稍后重试."));
            }
            SessionManager.Set(WebSiteSessionType.FindPasswordToken, token);
            SessionManager.Set(WebSiteSessionType.FindPasswordTime, DateTime.Now);
            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "邮件已发送，请及时修改密码."));
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public ActionResult SettingPwd()
        {
            return View();
        }

        /// <summary>
        /// 修改密码Post
        /// </summary>
        [HttpPost]
        public ActionResult SettingPwd(ResetPasswordModel model, string token)
        {
            var session_token = (string)SessionManager.Get(WebSiteSessionType.FindPasswordToken);
            var session_time = SessionManager.Get(WebSiteSessionType.FindPasswordTime) == null
                ? (DateTime?)null
                : Convert.ToDateTime(SessionManager.Get(WebSiteSessionType.FindPasswordTime));

            if (string.IsNullOrEmpty(session_token) || session_time == null ||
                session_token != token || Convert.ToDateTime(session_time).AddMinutes(5) < DateTime.Now)
            {
                return Json(DialogFactory.Create(DialogType.Error, "修改密码操作已过期，请在5分钟内修改密码！"));
            }

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
    }
}