using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using EKP.Service.Base;
using EKP.Service.Base.Tree;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Xml.XmlHelper;
using Microsoft.Practices.Unity;
using Ge.Infrastructure.Utilities;

namespace EKP.Web.Start
{
    /// <summary>
    /// 名    称：InitializeIoc
    /// 作    者：胡政
    /// 创建时间：2015-08-28
    /// 联系方式：13436053642
    /// 描    述：Ioc容器注册（依赖注入）
    /// </summary>
    public class InitializeIoc
    {
        public static void Register()
        {
            var unityContainer = new UnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(unityContainer));

            //通用注入
            unityContainer.RegisterInterfaceTypes(typeof(EKP.Service.Base.IIoc));

            //EKP_JS数据库
            unityContainer.RegisterType(typeof(IEkpEntityService<>), typeof(EkpEntityService<>));
            unityContainer.RegisterInheritedTypes(typeof(EkpEntityService<>));

            //EKP_JS数据库树形结构
            unityContainer.RegisterType(typeof(IBaseTreeService<>), typeof(BaseTreeService<>));
            unityContainer.RegisterInheritedTypes(typeof(BaseTreeService<>));
        }
    }
}