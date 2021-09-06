using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using EKP.Entity;
using EKP.Service.User;
using EKP.Web.Areas.Base.Application;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;


namespace EKP.Base.Identity
{
    /// <summary>
    /// 名    称：ApplicationSignInManager
    /// 作    者：胡政
    /// 参考：http://www.dotblogs.com.tw/ageoldmemories/archive/2013/12/19/135285.aspx
    /// 创建时间：2015-08-28
    /// 联系方式：13436053642
    /// 描    述：登录信息管理
    /// </summary>
    public class ApplicationSignInManager : SignInManager<IdentityUser, string>
    {
        private readonly IUserService userService = Ioc.GetService<IUserService>();


        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        /// <summary>
        /// 获取当前登录用户，未登录返回null
        /// </summary>
        public static IdentityUser GetLoginUser()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return null;

            var userId = HttpContext.Current.User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId)) return null;

            var userService = Ioc.GetService<IUserService>();
            var user = userService.GetEntiy(string.Format("Id = '{0}'", userId));
            var identityUser = new IdentityUser(user);
            var claimsIdentitys = (HttpContext.Current.User.Identity as ClaimsIdentity);
            if (claimsIdentitys == null) 
                return identityUser;

            var claims = claimsIdentitys.Claims.ToList();
            var loginMethod = claims.ToList().Find(c => c.Type == "LoginMethod").Value;
            var loginRoleId = claims.ToList().Find(c => c.Type == "LoginRoleId").Value;

            identityUser.Id = user.Id.ToString();
            identityUser.UserName = user.Account;
            identityUser.LoginMethod = (LoginMethod)Enum.Parse(typeof(LoginMethod), loginMethod);
            identityUser.LoginRoleId = loginRoleId;

            return identityUser;
        }

        /// <summary>
        /// 当前是否登录
        /// </summary>
        public static bool IsLogin()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            return identity != null && identity.IsAuthenticated;
        }

        /// <summary>
        /// 登录
        /// </summary>
        public void SignIn(IdentityUser user, bool isPersistent)
        {
            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaims(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("LoginMethod", user.LoginMethod.ToString()),
                new Claim("LoginRoleId", user.LoginRoleId),
            });

            AuthenticationProperties authenticationProperties = null;
            if (!isPersistent)
            {
                authenticationProperties = new AuthenticationProperties() { IsPersistent = true };
            }
            else
            {
                authenticationProperties = new AuthenticationProperties()
                {
                    IsPersistent = false,
                    IssuedUtc = DateTime.Now,
                    ExpiresUtc = DateTime.Now.AddMinutes(Convert.ToDouble(20))//20分钟后登录失效
                };
            }
            AuthenticationManager.SignIn(authenticationProperties, identity);
            CookieManager.Set(BaseCookieType.IsExitLogin, "0");
        }

        /// <summary>
        /// 切换当前登录用户的角色
        /// </summary>
        public void SwitchRole(string roleId)
        {
            var loginUser = GetLoginUser();
            if (loginUser == null) return;

            loginUser.LoginRoleId = roleId;
            SignIn(loginUser, false);
        }

        /// <summary>
        /// Ip自动登陆
        /// </summary>
        /// <returns>true：登陆；false：未登录</returns>
        public bool IpSignIn()
        {
            var ip = HttpHelper.GetIP();
            var user = userService.GetUserByIp(ip);
            if (user != null)
            {
                var identityUser = new IdentityUser(user);
                identityUser.LoginMethod = LoginMethod.Ip登录;
                SignIn(identityUser, true);
                CookieManager.Set(BaseCookieType.IsExitLogin, "0");
                return true;

            }
            else
            {
                var log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(string.Format("\r\n\tIp“{0}”登陆失败，不在登陆范围内。\r\n\r\n\r\n\r\n--------------------------------------------------------------------------------------------------", ip));
            }
            return false;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public void SignOut()
        {
            CookieManager.Set(BaseCookieType.IsExitLogin, "1");
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}