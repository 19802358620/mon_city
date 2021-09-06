using EKP.Base.Identity;
using EKP.Service.AdmSetting;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Base
{
    /// <summary>
    /// 当前的http请求后台模板相关信息
    /// </summary>
    public class AppAdm
    {
        private static readonly IAdmSettingService admSettingService = Ioc.GetService<IAdmSettingService>();

        public static string Layout
        {
            get
            {
                var layout = "_Layout";

                var loginInUser = ApplicationSignInManager.GetLoginUser();
                if(loginInUser != null)
                {
                    var admSetting = admSettingService.GetEntiy("UserId = '{0}' and IsWork = '{1}' and Isdeleted = '{2}'".Format2(
                        loginInUser.Id, IsYes.yes, IsDelete.undeleted));
                    if(admSetting != null)
                    {
                        layout = admSetting.Layout;
                    }
                }

                return layout;
            }
        }
    }
}
