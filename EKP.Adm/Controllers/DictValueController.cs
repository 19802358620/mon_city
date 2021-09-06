using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.DictValue;
using EKP.Base.Controllers;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Utilities;
using EKP.Service.DictKey;
using Ge.Infrastructure.Extensions;
using EKP.Service.Base.Tree;
using Ge.Infrastructure.Metronicv;
using EmitMapper.MappingConfiguration;
using EmitMapper;
using Ge.Infrastructure.Metronicv.Dialog;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 字典选项管理
    /// </summary>
    public class DictValueController : EntityController<T_DictValue>
    {
        private readonly IDictKeyService dictKeyService = Ioc.GetService<IDictKeyService>();
        private readonly IDictValueService dictValueService = Ioc.GetService<IDictValueService>();

        public DictValueController()
            : base(Ioc.GetService<IDictValueService>())
        {

        }

        /// <summary>
        /// 分页
        /// </summary>
        public ActionResult Pager()
        {
            return View();
        }

        /// <summary>
        /// 分页post
        /// </summary>
        [HttpPost]
        public ActionResult Pager(DictValuePagerParam param)
        {
            return Json(dictValueService.GetPager<DictValuePagerModel>(param, "T_DictKey"), "text/html");
        }

        /// <summary>
        /// 树形表格
        /// </summary>
        /// <returns></returns>
        public ActionResult Tree()
        {
            return View();
        }

        /// <summary>
        /// 树形表格post
        /// </summary>
        [HttpPost]
        public ActionResult Tree(int keyId)
        {
            var dicValueIds = dictValueService.GetPager<DictValuePagerModel>(new DictValuePagerParam
            {
                KeyId = keyId,
                Page = 1,
                PageSize = Int32.MaxValue
            }).Rows.Select(mc => mc.Id).ToList();

            var list = new List<DictValueTree>();
            var listDto = new List<JsTreeNode>();
            var ds = dictValueService.GetList(dicValueIds.ToArray());

            list = dictValueService.GetTree<DictValueTree>(ds);
            //自定义模型映射规则
            var mapConfig = new DefaultMapConfig().MatchMembers((x, y) =>
            {
                if (x == "Id" && y == "id")
                {
                    return true;
                }
                else if (x == "Name" && y == "text")
                {
                    return true;
                }
                else if (x == "Children" && y == "children")
                {
                    return true;
                }
                return x == y;
            });
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<List<DictValueTree>, List<JsTreeNode>>(mapConfig);
            listDto = mapper.Map(list);

            return Json(listDto);
        }

        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var dictValue = dictValueService.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_DictValue, DictValuePagerModel>(dictValue);
            return Json(detail);
        }

        /// <summary>
        /// 单条记录（根据字典键和字典值获取）
        /// </summary>
        [HttpPost]
        public ActionResult Detail2(string key, string value)
        {
            var dictKey = dictKeyService.GetEntiy("[Key]='{0}'".Format2(key));
            var dictValue = dictValueService.GetEntiy("KeyId='{0}' and Value='{1}'".Format2(dictKey.Id, value));
            var detail = ObjectMapper.Mapper<T_DictValue, DictValuePagerModel>(dictValue);
            return Json(detail);
        }

        /// <summary>
        /// 创建
        /// </summary>
        public ActionResult Create()
        {
            return PartialView();
        }

        /// <summary>
        /// 创建post
        /// </summary>
        [HttpPost]
        public ActionResult Create(DictValueCreateModel model)
        {
            var dictValue = dictValueService.GetEntiy("[KeyId]='{0}' and [Value]='{1}'".Format2(model.KeyId, model.ShowValue));
            if (dictValue != null)
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "数据名称重复，请更换！"));
            }

            return Json(base.Create(model));
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public ActionResult Edit()
        {
            return PartialView();
        }
        
        /// <summary>
        /// 编辑post
        /// </summary>
        [HttpPost]
        public ActionResult Edit(DictValueEditModel model)
        {
            var dictValue = dictValueService.GetEntiy("[KeyId]='{0}' and [Value]='{1}' and Id != '{2}'".Format2
                (model.KeyId, model.ShowValue, model.Id));
            if (dictValue != null)
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "数据名称重复，请更换！"));
            }

            return Json(Edit(string.Format("id = {0}", model.Id), model,
                "Value", "ShowValue", "IsWork", "Remark"));

        }

        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            return Json(base.Delete(ids.ToArray()));
        }
    }
}