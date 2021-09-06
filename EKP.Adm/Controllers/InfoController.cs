using EKP.Base.Controllers;
using EKP.Entity;
using EKP.Service.Info;
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
    public class InfoController : EntityController<T_Info>
    {
        private readonly IInfoService InfoService = Ioc.GetService<IInfoService>();

        public InfoController()
            : base(Ioc.GetService<IInfoService>())
        {

        }

        #region View
        /// <summary>
        /// 分页
        /// </summary>
        public ActionResult Pager()
        {
            return View();
        }

        /// <summary>
        /// 创建
        /// </summary>
        public ActionResult Create()
        {
            return PartialView();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public ActionResult Edit()
        {
            return PartialView();
        }

        #endregion

        #region 基础业务

        /// <summary>
        /// 分页post
        /// </summary>
        [HttpPost]
        public ActionResult Pager(InfoPagerParam param)
        {
            return Json(InfoService.GetPager<InfoPagerModel>(param, "T_DictValue"));
        }

        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var model = InfoService.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_Info, InfoPagerModel>(model);
            return Json(detail);
        }


        /// <summary>
        /// 创建post
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(InfoCreateModel model)
        {
            if(model.Type == "0")
            {
                return Json(DialogFactory.Create(DialogType.Error, "一级类型不能为空"));
            }
            if (model.ContentType == "0")
            {
                return Json(DialogFactory.Create(DialogType.Error, "二级类型不能为空"));
            }
            return Json(base.Create(model));
        }


        /// <summary>
        /// 编辑post
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(InfoEditModel model)
        {
            return Json(EditIgnore(string.Format("id = {0}", model.Id), model, "CreateTime", "CreateBy", "IsDeleted", "Writer", "SiteId", "Type", "ContentType"));
        }

        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            return Json(base.Delete(ids.ToArray()));
        }

        #endregion
    }
}