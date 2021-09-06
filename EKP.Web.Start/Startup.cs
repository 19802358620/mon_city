using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(EKP.Web.Start.Startup))]
namespace EKP.Web.Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);//注册Identity
            app.MapSignalR();//注册signalR

            //api
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            HttpConfiguration config = new HttpConfiguration();

            //WebApiConfig.Register(config);
            config.EnableCors();
            app.UseWebApi(config);
            app.UseCors(CorsOptions.AllowAll);
            //app.MapSignalR();


            ConfigureOAuth(app);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),//获取 access_token 授权服务请求地址
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60), //access_token 过期时间
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}

