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
using EKP.Service.Project;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using Ge.Infrastructure.Extensions;
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
    /// 项目管理
    /// </summary>
    public class ProjectController : EntityController<T_Project>
    {
        private readonly IProjectService projectService = Ioc.GetService<IProjectService>();

        public ProjectController()
            : base(Ioc.GetService<IProjectService>())
        {

        }

        /// <summary>
        /// 分页post
        /// </summary>
        [HttpPost]
        public ActionResult Pager(ProjectPagerParam param)
        {
            return Json(projectService.GetPager<ProjectPagerModel>(param));
        }

        /// <summary>
        /// 单条记录post
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var project = projectService.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_Project, ProjectPagerModel>(project);
            return Json(detail);
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        [HttpPost]
        public ActionResult Create(ProjectCreateModel model)
        {
            model.Type = model.ParentId > 0 ? ProjectType.task.ToString() : ProjectType.project.ToString();
            model.Name = model.Type == ProjectType.project.ToString() ? "未命名项目" : "未命名任务";
            model.SortIndex = 10;

            //模型验证
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }

            var project = ObjectMapper.Mapper<ProjectCreateModel, T_Project>(model);
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                trans.Add(project);
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
        public ActionResult Edit(ProjectEditModel model)
        {
            return Json(Edit(string.Format("id = '{0}'", model.Id), model, new string[] { "Name", "Type", "SortIndex" }));
        }

        /// <summary>
        /// 编辑某个字段post
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditField(ProjectEditFieldModel model, string[] fields)
        {
            return Json(Edit(string.Format("id = '{0}'", model.Id), model, fields));
        }

        /// <summary>
        /// 树
        /// </summary>
        public ActionResult Tree(){

            return View();
        }

        /// <summary>
        /// 树post
        /// </summary>
        [HttpPost]
        public ActionResult Tree(int siteId)
        {
            var projectIds = projectService.GetPager<ProjectPagerModel>(new ProjectPagerParam
            {
                Page = 1,
                PageSize = Int32.MaxValue,
                SiteId = siteId
            }).Rows.Select(mc => mc.Id).ToList();

            var listDto = new List<JsTreeNode>();
            var ds = projectService.GetList(projectIds.ToArray());
            var list = projectService.GetTree<ProjectTree>(ds);

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
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<List<ProjectTree>, List<JsTreeNode>>(mapConfig);
            listDto = mapper.Map(list);

            return Json(listDto);
        }

        /// <summary>
        /// 课程学习树post
        /// </summary>
        [HttpPost]
        public ActionResult StudyTree(int siteId)
        {
            var projects = projectService.GetPager<ProjectPagerModel>(new ProjectPagerParam
            {
                Page = 1,
                PageSize = Int32.MaxValue,
                SiteId = siteId,
                Fields = "T_Project.[Id],[ParentId],[Name],[Type],[SortIndex],[CreateIp],[CreateBy],[CreateTime],[IsDeleted],[SiteId]"
            }).Rows;

            //获取树
            var listDto = new List<ProjectJsTTree>();
            var ds = projects.ToModel<List<ProjectPagerModel>, List<T_Project>>();
            var list = projectService.GetTree<ProjectTree>(ds);

            //填充学习相关节点
            list.DeepForeach("Children", project =>
            {
                if (project.Type == ProjectType.task.ToString())
                {
                    project.Children = project.Children ?? new List<ProjectTree>();
                    project.Children.Add(new ProjectTree()
                    {
                        Id = -1,
                        ParentId = project.Id,
                        Type = "LearningTarget",
                        Name = "相关知识"
                    });
                    project.Children.Add(new ProjectTree()
                    {
                        Id = -1,
                        ParentId = project.Id,
                        Type = "ExtendedLearning",
                        Name = "知识拓展"
                    });
                    //project.Children.Add(new ProjectTree()
                    //{
                    //    Id = -1,
                    //    ParentId = project.Id,
                    //    Type = "LearningImport",
                    //    Name = "学习导入"
                    //});
                    //project.Children.Add(new ProjectTree()
                    //{
                    //    Id = -1,
                    //    ParentId = project.Id,
                    //    Type = "TaskDescription",
                    //    Name = "任务描述"
                    //});
                    //project.Children.Add(new ProjectTree()
                    //{
                    //    Id = -1,
                    //    ParentId = project.Id,
                    //    Type = "TaskPrepare",
                    //    Name = "任务准备"
                    //});
                    //project.Children.Add(new ProjectTree()
                    //{
                    //    Id = -1,
                    //    ParentId = project.Id,
                    //    Type = "TaskImple",
                    //    Name = "任务实施"
                    //});
                    project.Children.Add(new ProjectTree()
                    {
                        Id = -1,
                        ParentId = project.Id,
                        Type = "ProjectInfo",
                        Name = "思考与练习"
                    });
                }
            });

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
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<List<ProjectTree>, List<ProjectJsTTree>>(mapConfig);
            listDto = mapper.Map(list);

            return Json(listDto);
        }

        /// <summary>
        /// 初始化树
        /// </summary>
        [HttpPost]
        public ActionResult InitTree(int siteId)
        {
            if (!(siteId > 0))
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数有误！"));
            }

            var project = new T_Project
            {
                Name = "未命名项目",
                Type = ProjectType.project.ToString(),
                SortIndex = 10,
                SiteId = siteId,
                CreateTime = DateTime.Now,
                IsDeleted = IsDelete.undeleted.ToString()
            };

            using (var trans = EkpDbService.CreateEntityTrans())
            {
                trans.Add(project);
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
            var project = projectService.GetEntiy(id);
            project.Name = name;
            projectService.Update(project, "Name");
            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！"));
        }

        /// <summary>
        /// 删除节点（包含子节点）
        /// </summary>
        public ActionResult Delete(int id)
        {
            //获取需要删除的节点
            var ds = projectService.GetList(string.Empty);
            var deleteNodes = projectService.GetSomeChildren(ds, id);
            var deleteNodeIds = deleteNodes.Select(n => n.Id).ToList();
            deleteNodeIds.Add(id);
            var deleteProjects = projectService.GetList(deleteNodeIds.ToArray());
            deleteProjects.ForEach(p => p.IsDeleted = IsDelete.deleted.ToString());

            //删除
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                deleteProjects.ForEach(trans.Update);
                trans.SaveChange();
            }
            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！"));
        }
    }
}