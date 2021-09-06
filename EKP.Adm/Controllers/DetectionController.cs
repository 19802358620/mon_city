using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKP.Base.Controllers;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Base.Tree;
using EKP.Service.Detection;
using EKP.Service.Project;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Mvc.Extensions;
using Ge.Infrastructure.Utilities;
using Newtonsoft.Json2;
using Newtonsoft.Json2.Linq;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 自我检测管理
    /// </summary>
    public class DetectionController : EntityController<T_Detection>
    {
        private readonly IDetectionService detectionService = Ioc.GetService<IDetectionService>();
        private readonly IProjectService projectService = Ioc.GetService<IProjectService>();

        public DetectionController()
            : base(Ioc.GetService<IDetectionService>())
        {

        }

        /// <summary>
        /// 分页post
        /// </summary>
        [HttpPost]
        public ActionResult Pager(DetectionPagerParam param)
        {
            return Json(detectionService.GetPager<DetectionPagerModel>(param));
        }

        /// <summary>
        /// 单条记录post
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var detection = detectionService.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_Detection, DetectionPagerModel>(detection);
            return Json(detail);
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        [HttpPost]
        public ActionResult Create(DetectionCreateModel model)
        {
            model.Name = "未命名";
            model.SortIndex = 10;

            //模型验证
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }

            //模块级数验证
            if (model.ParentId > 0)
            {
                return Json(DialogFactory.Create(DialogType.Error, "错误", "系统只允许添加1级栏目！"));
            }


            var detection = ObjectMapper.Mapper<DetectionCreateModel, T_Detection>(model);
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                trans.Add(detection);
                trans.SaveChange();
            }
            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！"));
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
        public ActionResult Edit(DetectionEditModel model)
        {
            return Json(Edit(string.Format("id = '{0}'", model.Id), model, new string[] { "Name", "SortIndex" }));
        }

        /// <summary>
        /// 编辑某个字段post
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditField(DetectionEditFieldModel model, string[] fields)
        {
            return Json(Edit(string.Format("id = '{0}'", model.Id), model, fields));
        }

        /// <summary>
        /// 树
        /// </summary>
        public ActionResult Tree()
        {

            return PartialView();
        }

        /// <summary>
        /// 树post
        /// </summary>
        [HttpPost]
        public ActionResult Tree(int projectId)
        {
            var detectionIds = detectionService.GetPager<DetectionPagerModel>(new DetectionPagerParam
            {
                Page = 1,
                PageSize = Int32.MaxValue,
                ProjectId = projectId
            }).Rows.Select(mc => mc.Id).ToList();

            var listDto = new List<JsTreeNode>();
            var ds = detectionService.GetList(detectionIds.ToArray());
            var list = detectionService.GetTree<DetectionTree>(ds);

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
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<List<DetectionTree>, List<JsTreeNode>>(mapConfig);
            listDto = mapper.Map(list);

            return Json(listDto);
        }

        /// <summary>
        /// 初始化树
        /// </summary>
        [HttpPost]
        public ActionResult InitTree(int projectId)
        {
            if (!(projectId > 0))
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数有误！"));
            }

            var project = projectService.GetEntiy(projectId);
            var detection = new T_Detection
            {
                Name = "未命名",
                SortIndex = 10,
                ProjectId = projectId,
                SiteId = Convert.ToInt32(project.SiteId),
                CreateTime = DateTime.Now,
                IsDeleted = IsDelete.undeleted.ToString()
            };

            using (var trans = EkpDbService.CreateEntityTrans())
            {
                trans.Add(detection);
                trans.SaveChange();
            }
            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！"));
        }

        /// <summary>
        /// 重命名节点
        /// </summary>
        [HttpPost]
        public ActionResult Rename(int id, string name)
        {
            var detection = detectionService.GetEntiy(id);
            detection.Name = name;
            detectionService.Update(detection, "Name");
            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！"));
        }

        /// <summary>
        /// 删除节点（包含子节点）
        /// </summary>
        public ActionResult Delete(int id)
        {
            //获取需要删除的节点
            var ds = detectionService.GetList(string.Empty);
            var deleteNodes = detectionService.GetSomeChildren(ds, id);
            var deleteNodeIds = deleteNodes.Select(n => n.Id).ToList();
            deleteNodeIds.Add(id);
            var deleteDetections = detectionService.GetList(deleteNodeIds.ToArray());
            deleteDetections.ForEach(p => p.IsDeleted = IsDelete.deleted.ToString());

            //删除
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                deleteDetections.ForEach(trans.Update);
                trans.SaveChange();
            }
            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！"));
        }
    }
}