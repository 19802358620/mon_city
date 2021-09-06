using System;
using EKP.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using EKP.Base.Identity;

namespace EKP.Web.Start
{
    /// <summary>
    /// 名    称：Startup
    /// 作    者：胡政
    /// 创建时间：2015-08-28
    /// 联系方式：13436053642
    /// 描    述：程序启动时自动执行
    /// </summary>
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            //Identity
            app.CreatePerOwinContext(() => new EKP_JSEntities());
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                CookieName = ".ASPNET.ApplicationCookie.EKP_Jsgcwx",
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/User/Login"),

                CookieSecure = CookieSecureOption.Never,
            });
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}