using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Mvc.Extensions;
using Ge.Infrastructure.Pager;
using Ge.Infrastructure.Utilities;
using Microsoft.Practices.ObjectBuilder2;
using Newtonsoft.Json2;


namespace EKP.Base.Controllers
{
    /// <summary>
    /// 作    者：胡政
    /// 联系方式：13436053642
    ///  描    述：抽象实体操作控制器，抽象出实体的创建、编辑、删除、分页方法
    /// </summary>
    public abstract class EntityController<TEntity> : BaseController where TEntity : class, new()
    {
        protected IEkpEntityService<TEntity> entityService = null;//实体数据服务

        protected EntityController(IEkpEntityService<TEntity> entityService)
        {
            this.entityService = entityService;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        protected virtual Dialog Create<TCreateModel>(TCreateModel model)
            where TCreateModel : class, new()
        {
            //模型数据安全验证
            if (!this.ModelValidate(model).IsValid)
            {
                return DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage);
            }
            var entity = ObjectMapper.Mapper<TCreateModel, TEntity>(model);
            entityService.Add(entity);

            var property = entity.GetType().GetProperty("Id");
            var id = string.Empty;
            if (property != null)
                id = property.GetValue(entity).ToString();
            return DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！", id);
        }

        /// <summary>
        /// 创建（更新）实体，有则更新，没有则创建
        /// </summary>
        protected virtual Dialog CreateSingle<TCreateModel>(TCreateModel model, string where, params string[] fileds)
            where TCreateModel : class, new()
        {
            //模型数据安全验证
            if (!this.ModelValidate(model).IsValid)
            {
                return DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage);
            }

            var entity = entityService.GetEntiy(where);

            if (entity == null)
            {
                entity = ObjectMapper.Mapper<TCreateModel, TEntity>(model);
                entityService.Add(entity);
            }
            else
            {
                entity = ObjectMapper.Mapper(model, entity, null, new string[] { "Id" });
                entityService.Update(entity, fileds);
            }

            var property = entity.GetType().GetProperty("Id");
            var id = string.Empty;
            if (property != null)
                id = property.GetValue(entity).ToString();
            return DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！", id);
        }

        /// <summary>
        /// 编辑实体
        /// </summary>
        /// <param name="fileds">标注哪些字段需要被修改，如果为null则所有字段都会被修改</param>
        protected virtual Dialog Edit<TEditModel>(string where, TEditModel model, params string[] fileds)
            where TEditModel : class, new()
        {
            //模型数据安全验证
            if (!this.ModelValidate(model).IsValid)
            {
                return DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage);
            }

            var entity = entityService.GetEntiy(where);
            if (entity == null)
            {
                return DialogFactory.Create(DialogType.Error, "对象不存在或者已被删除");
            }
            entity = ObjectMapper.Mapper(model, entity);
            entityService.Update(entity, fileds);
            return DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！");
        }

        /// <summary>
        /// 编辑实体，并指定忽略哪些列
        /// </summary>
        /// <param name="ignoreFileds">标注哪些字段需要被忽略，如果为null则所有字段都会被修改</param>
        protected virtual Dialog EditIgnore<TEditModel>(string where, TEditModel model, params string[] ignoreFileds)
            where TEditModel : class, new()
        {
            //模型数据安全验证
            if (!this.ModelValidate(model).IsValid)
            {
                return DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage);
            }

            var entity = entityService.GetEntiy(where);
            if (entity == null)
            {
                return DialogFactory.Create(DialogType.Error, "对象不存在或者已被删除");
            }
            if (ignoreFileds.Length == 0)
            {
                var tempFileds = new List<string>();
                var propertys = model.GetType().GetProperties();
                propertys.ForEach(p =>
                {
                    if (p.Name.ToLower() == "createip" && p.GetSetMethod() != null)
                        tempFileds.Add("CreateIp");
                    else if (p.Name.ToLower() == "createby" && p.GetSetMethod() != null)
                        tempFileds.Add("CreateBy");
                    else if (p.Name.ToLower() == "createtime" && p.GetSetMethod() != null)
                        tempFileds.Add("CreateTime");
                    else if (p.Name.ToLower() == "isdeleted" && p.GetSetMethod() != null)
                        tempFileds.Add("IsDeleted");
                    else if (p.Name.ToLower() == "siteid" && p.GetSetMethod() != null)
                        tempFileds.Add("SiteId");
                });
                ignoreFileds = tempFileds.ToArray();
            }
            entity = ObjectMapper.Mapper(model, entity);
            entityService.UpdateIgnore(entity, ignoreFileds);
            return DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！");
        }

        /// <summary>
        /// 根据ids删除实体
        /// </summary>
        [HttpPost]
        protected virtual Dialog Delete(int[] ids, bool isFlag = true)
        {
            var entitys = entityService.GetList(ids);
            if (isFlag)
            {
                PropertyInfo property = null;
                entitys.ForEach(e =>
                {
                    if (property == null)
                        property = e.GetType().GetProperty("IsDeleted");
                    property.SetValue(e, IsDelete.deleted.ToString());
                });
                entityService.Update(entitys.ToArray(), "IsDeleted");
            }
            else
            {
                try
                {
                    entityService.Delete(entitys.ToArray());

                }
                catch
                {
                    return DialogFactory.Create(DialogType.Error, "操作失败");
                }
            }

            return DialogFactory.Create(DialogType.Success, string.Empty, "操作成功");
        }


        /// <summary>
        /// 根据where条件删除实体
        /// </summary>
        [HttpPost]
        protected virtual Dialog Delete(string where, bool isFlag = true)
        {
            var entitys = entityService.GetList(where);
            if (isFlag)
            {
                PropertyInfo property = null;
                entitys.ForEach(e =>
                {
                    if (property == null)
                        property = e.GetType().GetProperty("IsDeleted");
                    property.SetValue(e, IsDelete.deleted.ToString());
                });
                entityService.Update(entitys.ToArray(), "IsDeleted");
            }
            else
            {
                try
                {
                    entityService.Delete(entitys.ToArray());

                }
                catch
                {
                    return DialogFactory.Create(DialogType.Error, "操作失败");
                }
            }

            return DialogFactory.Create(DialogType.Success, string.Empty, "操作成功");
        }

        /// <summary>
        /// 分页
        /// </summary>
        [HttpPost]
        protected virtual PagerResult<TPagerModel> Pager<TPagerModel>(PagerParameter param, string where)
            where TPagerModel : class, new()
        {
            var result = entityService.GetPager<TPagerModel>(param, where);
            return result;
        }

        /// <summary>
        /// 按条件获取对象列表
        /// </summary>
        protected virtual List<TEntity> GetList(string where, params string[] path)
        {
            var result = entityService.GetList(where);
            return result;
        }

        /// <summary>
        /// 按Sql获取对象列表
        /// </summary>
        protected virtual List<TEntity> GetList(string entsql, ObjectParameter[] parameters, params string[] includePath)
        {
            var result = entityService.GetList(entsql);
            return result;
        }

        /// <summary>
        /// 将对象序列化为返回json结果的ContentResult
        /// </summary>
        /// <param name="data">待序列化数据类型</param>
        /// <param name="referenceLoopHandling">对于循环调用的处理行为</param>
        protected JavaScriptResult Json(object data, ReferenceLoopHandling referenceLoopHandling = ReferenceLoopHandling.Ignore)
        {
            var jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = referenceLoopHandling;
            var result = new JavaScriptResult();
            result.Script = JsonConvert.SerializeObject(data, jsSettings);
            return result;
        }
    }
}