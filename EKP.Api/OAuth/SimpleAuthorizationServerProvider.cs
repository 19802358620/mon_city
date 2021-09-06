using EKP.Service.ApiUser;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Utilities;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace EKP.Api.OAuth
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// 验证 client 信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Factory.StartNew(() => context.Validated());
        }
        /// <summary>
        /// 生成 access_token（resource owner password credentials 授权方式）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            IApiUserService apiUserService = Ioc.GetService<IApiUserService>();
            try
            {
                if (string.IsNullOrEmpty(context.UserName))
                {
                    context.SetError("error", "账号不能为空");
                    return;
                }
                if (string.IsNullOrEmpty(context.Password))
                {
                    context.SetError("error", "密码不能为空");
                    return;
                }

                var model = apiUserService.GetEntiy(string.Format(" Account = '{0}'", context.UserName));
                if (model.Status.ToLower().Equals("stop"))
                {
                    context.SetError("error", "该账号已被停用");
                    return;
                }
                if (Md5Encrypt.Md5EncryptPassword(context.Password.Trim()) != model.Password)
                {
                    context.SetError("error", "登录密码错误，请重新输入");
                    return;
                }
                //记录登录日志
                //ILoginLogService loginLogService = AutofacExt.GetFromFac<ILoginLogService>();
                //LoginLog model = new LoginLog
                //{
                //    UserId = user.Id,
                //    LoginInTime = DateTime.Now,
                //    CreateTime = DateTime.Now
                //};
                //loginLogService.Insert(model);

                var OAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                OAuthIdentity.AddClaim(new Claim(ClaimTypes.PrimarySid, model.Id.ToString()));
                OAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, model.Account));
                //OAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "user_name", context.UserName
                    },
                    {
                        "is_authenticated","true"
                    }
                });
                var ticket = new AuthenticationTicket(OAuthIdentity, props);
                context.Validated(ticket);
            }
            catch (Exception ex)
            {
                var e = ex;
            }

        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
        /// <summary>
        /// 验证 access_token 的请求
        /// </summary>
        //public override async Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
        //{
        //    if (context.TokenRequest.IsAuthorizationCodeGrantType || context.TokenRequest.IsRefreshTokenGrantType)
        //    {
        //        context.Validated();
        //    }
        //    else
        //    {
        //        context.Rejected();
        //    }
        //}
    }
}