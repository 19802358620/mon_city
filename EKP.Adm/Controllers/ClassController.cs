using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Class;
using EKP.Base.Controllers;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Utilities;
using EKP.Base.Identity;
using EKP.Service.User;
using System.Linq;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 班级管理
    /// </summary>
    public class ClassController : EntityController<T_Class>
    {
        private readonly IClassService classService = Ioc.GetService<IClassService>();
        private readonly IUserService userService = Ioc.GetService<IUserService>();

        public ClassController()
            : base(Ioc.GetService<IClassService>())
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
        public ActionResult Pager(ClassPagerParam param)
        {
            try
            {
                var loginInUser = ApplicationSignInManager.GetLoginUser();
                var user = loginInUser.LoginUser;
                param.UserId = user.Id;
                if (user.RoleId == 2) //管理员
                    param.UserId = 0;

                if (user.RoleId == 2) //管理员
                {
                    return Json(classService.GetPager<ClassPagerModel>(param, "T_Site"));
                }
                else
                {
                    var userEntity = userService.GetEntiy(user.Id);
                    var classIds = ObjectMapper.Mapper<List<string>, List<int>>(userEntity.ClassIds.Split(',').ToList());
                    List<T_Class> classes = classService.GetList(classIds.ToArray());
                    List<ClassPagerModel> classesList  = ObjectMapper.Mapper<List<T_Class>, List<ClassPagerModel>>(classes);
                    var modelList =  new JqgridResult<ClassPagerModel>(param)
                                    {
                                        Rows = classesList,
                                        TotalRecords = classesList.Count,
                                    };
                    return Json(modelList);
                }
            }catch(Exception ex)
            {
                return Json(null);
            }
        }

        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var _class = classService.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_Class, ClassPagerModel>(_class);
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
        public ActionResult Create(ClassCreateModel model)
        {
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
        public ActionResult Edit(ClassEditModel model)
        {
            return Json(Edit(string.Format("id = {0}", model.Id), model,
                "Name", "Remark"));
        }

        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            return Json(base.Delete(ids.ToArray()));
        }

        /// <summary>
        /// 教师班级分页
        /// </summary>
        [HttpPost]
        public ActionResult TeacherClassPager(TeacherClassPagerParam param)
        {
            return Json(classService.GetTeacherClassPager<ClassPagerModel>(param, "T_Site"));
        }
    }
}