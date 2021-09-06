using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using EKP.Service.Base.EkpBaseModel;
using Microsoft.Practices.ObjectBuilder2;
using EKP.Base.Identity;
using Ge.Infrastructure.Utilities;
using System.Collections.Generic;
using System.Collections;

namespace EKP.Base.FilterAttribute
{
    /// <summary>
    /// 名    称：模型特殊填充器
    /// 描    述：自动给模型填充CreateTime（创建时间）、IsDeleted（是否删除）、IsLock（是否锁定）个字段
    /// 备    注：只有在用户处于登陆状态且模型字段名和以上字段名相符时有效
    /// </summary>
    public class ModelFillAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            
            var parameters = filterContext.ActionParameters;
            if (parameters == null || parameters.Count == 0)
                return;

            //检查是否登录
            var user = ApplicationSignInManager.GetLoginUser();

            parameters.ForEach(parameter =>
            {
                var model = parameter.Value;
                if (model == null) return;

                var list = new ArrayList();

                if (typeof(ICollection).IsAssignableFrom(model.GetType()))
                {
                    list.AddRange(model as ICollection);
                }
                else
                {
                    list.Add(model);
                }

                list.ToArray().ForEach(item =>
                {
                    var propertys = item.GetType().GetProperties();
                    //模型处于创建状态
                    bool isCreate = propertys.Any(p => p.Name.ToLower() == "id" &&
                           (p.GetValue(item) == null || string.IsNullOrEmpty(p.GetValue(item).ToString()) || p.GetValue(item).ToString() == "0"));
                    if (isCreate)
                    {
                        var ip = HttpHelper.GetIP();
                        var pcc = IpsHelper.IpToArea(ip);
                        propertys.ForEach(p =>
                        {
                            //公共信息填充
                            if (p.Name.ToLower() == "createip" && p.GetSetMethod() != null && !IsContainKey(p.Name))
                                p.SetValue(item, ip);
                            else if (p.Name.ToLower() == "createby" && p.GetSetMethod() != null && !IsContainKey(p.Name) && user != null)
                                p.SetValue(item, Convert.ToInt32(user.Id));
                            else if (p.Name.ToLower() == "createtime" && p.GetSetMethod() != null && !IsContainKey(p.Name))
                                p.SetValue(item, DateTime.Now);
                            else if (p.Name.ToLower() == "isdeleted" && p.GetSetMethod() != null && !IsContainKey(p.Name))
                                p.SetValue(item, IsDelete.undeleted.ToString());
                            else if (p.Name.ToLower() == "islock" && p.GetSetMethod() != null && !IsContainKey(p.Name))
                                p.SetValue(item, IsLock.unlock.ToString());
                            else if (p.Name.ToLower() == "siteid" && p.GetSetMethod() != null && !IsContainKey(p.Name))
                                p.SetValue(item, App.Site.Id);

                            //日志相关信息填充
                            else if (p.Name.ToLower() == "requestcountry" && p.GetSetMethod() != null)
                                p.SetValue(item, pcc.Country);
                            else if (p.Name.ToLower() == "requestprovince" && p.GetSetMethod() != null)
                                p.SetValue(item, pcc.Province);
                            else if (p.Name.ToLower() == "requestcity" && p.GetSetMethod() != null)
                                p.SetValue(item, pcc.City);
                            else if (p.Name.ToLower() == "requestisp" && p.GetSetMethod() != null)
                                p.SetValue(item, pcc.ISP);
                            else if (p.Name.ToLower() == "requestuseragent" && p.GetSetMethod() != null && p.GetValue(item) == null)
                                p.SetValue(item, HttpContext.Current.Request.UserAgent);
                            else if (p.Name.ToLower() == "requestreferurl" && p.GetSetMethod() != null && p.GetValue(item) == null)
                                if (HttpContext.Current.Request.UrlReferrer != null)
                                    p.SetValue(item, HttpContext.Current.Request.UrlReferrer.AbsoluteUri);
                        });
                    }

                    //模型处于编辑状态
                    bool isUpdate = propertys.Any(p => p.Name.ToLower() == "id" &&
                            (p.GetValue(item) != null && !string.IsNullOrEmpty(p.GetValue(item).ToString()) && p.GetValue(item).ToString() != "0"));
                    if (isUpdate)
                    {
                        propertys.ForEach(p =>
                        {
                            //
                        });
                    }

                    //既不是创建也不是编辑状态
                    if (!isCreate && !isUpdate)
                    {
                        propertys.ForEach(p =>
                        {
                            if (p.Name.ToLower() == "siteid" && p.GetSetMethod() != null && !IsContainKey(p.Name))
                                p.SetValue(item, App.Site.Id);
                        });
                    }
                });
            });
        }

        /// <summary>
        /// 前台请求是否包含某个Key值
        /// </summary>
        /// <returns></returns>
        private bool IsContainKey(string key)
        {
           return HttpContext.Current.Request.Form.AllKeys.ToList().Exists(ak => ak.ToLower() == key.ToLower());
        }
    }
}