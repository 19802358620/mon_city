using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EKP.Service.OAuthRefreshToken;
using Ge.Infrastructure.Ioc;
using EKP.Entity;

namespace EKP.Api.OAuth
{
    public class SimpleRefreshTokenProvider : AuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, string> _refreshTokens = new ConcurrentDictionary<string, string>();
        private readonly IOAuthRefreshTokenService oAuthRefreshTokenService = Ioc.GetService<IOAuthRefreshTokenService>();
        /// <summary>
        /// 生成 refresh_token
        /// </summary>
        public override void Create(AuthenticationTokenCreateContext context)
        {
            context.Ticket.Properties.IssuedUtc = DateTime.UtcNow;
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddDays(30);
            Guid num = Guid.NewGuid();
            context.SetToken(num.ToString());
            //_refreshTokens[context.Token] = context.SerializeTicket();

            //保存到数据库
            OAuthRefreshTokenCreateModel model = new OAuthRefreshTokenCreateModel
            {
                Num = num,
                Token = context.SerializeTicket(),
                CreateTime = DateTime.Now
            };
            oAuthRefreshTokenService.Add(model);

        }

        /// <summary>
        /// 由 refresh_token 解析成 access_token
        /// </summary>
        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            //通过数据库获取值
            try
            {
                T_OAuthRefreshToken oAuthRefreshToken = oAuthRefreshTokenService.GetEntiy(string.Format(" Num = '{0}'", Guid.Parse(context.Token)));
                if (oAuthRefreshToken != null)
                {
                    context.DeserializeTicket(oAuthRefreshToken.Token);
                    oAuthRefreshTokenService.Delete(oAuthRefreshToken);
                    ////记录登录日志
                    //bool IsAuthenticated = false;
                    //long userId = 0;
                    //ILoginLogService loginLogService = AutofacExt.GetFromFac<ILoginLogService>();
                    //var currentUser = (ClaimsPrincipal)System.Web.HttpContext.Current.User;
                    //if (currentUser != null)
                    //{
                    //    IsAuthenticated = currentUser.Identity.IsAuthenticated;
                    //    if (IsAuthenticated)
                    //    {
                    //        foreach (Claim c in currentUser.Claims)
                    //        {
                    //            if (c.Type == ClaimTypes.PrimarySid)
                    //                userId = long.Parse(c.Value);
                    //        }
                    //    }
                    //}
                    //LoginLog model = new LoginLog
                    //{
                    //    UserId = userId,
                    //    LoginInTime = DateTime.Now,
                    //    CreateTime = DateTime.Now
                    //};
                    //loginLogService.Insert(model);
                }
            }
            catch (Exception ex) { }
        }
    }
}
