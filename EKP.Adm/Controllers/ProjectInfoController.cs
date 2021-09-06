using EKP.Base.Controllers;
using EKP.Entity;
using EKP.Service.ProjectInfo;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EKP.Adm.Controllers
{
    public class ProjectInfoController : EntityController<T_ProjectInfo>
    {
        private readonly IProjectInfoService projectInfoService = Ioc.GetService<IProjectInfoService>();

        public ProjectInfoController()
            : base(Ioc.GetService<IProjectInfoService>())
        {

        }

        /// <summary>
        /// 分页post
        /// </summary>
        [HttpPost]
        public ActionResult Pager(ProjectInfoPagerParam param)
        {
            return Json(projectInfoService.GetPager<ProjectInfoPagerModel>(param));
        }

        /// <summary>
        /// 创建post
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ProjectInfoCreateModel model)
        {
            return Json(base.Create(model));
        }

        /// <summary>
        /// 编辑post
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ProjectInfoEditModel model)
        {
            //string picName = VideoHelper.GetPicFromVideo(model.Video, "240*180", "1");
            return Json(Edit(string.Format("id = {0}", model.Id), model, "Name", "Picture", "Video", "Content"));
        }        
       
        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var model = projectInfoService.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_ProjectInfo, ProjectInfoPagerModel>(model);
            return Json(detail);
        }


        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            return Json(base.Delete(ids.ToArray()));
        }

        #region 知识拓展

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        public ActionResult KnowledgePager()
        {
            return PartialView();
        }

        /// <summary>
        /// 创建
        /// </summary>
        public ActionResult KnowledgeCreate()
        {
            return PartialView();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public ActionResult KnowledgeEdit()
        {
            return PartialView();
        }

        #endregion

        #region 动画演示

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        public ActionResult VideoPager()
        {
            return PartialView();
        }

        /// <summary>
        /// 创建
        /// </summary>
        public ActionResult VideoCreate()
        {
            return PartialView();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public ActionResult VideoEdit()
        {
            return PartialView();
        }

        #endregion
    }
}