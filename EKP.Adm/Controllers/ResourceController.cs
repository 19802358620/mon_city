using EKP.Base.Controllers;
using EKP.Base.Identity;
using EKP.Entity;
using EKP.Repository.Ef;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Class;
using EKP.Service.LearningResource;
using EKP.Service.LearningResourceClass;
using EKP.Service.HomeworkSubmit;
using EKP.Service.User;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Sql;
using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EKP.Adm.Controllers
{
    public class ResourceController : EntityController<T_LearningResource>
    {
        private readonly IResourceService resourceService = Ioc.GetService<IResourceService>();
        private readonly IResourceClassService resourceclassService = Ioc.GetService<IResourceClassService>();
        private readonly IClassService classService = Ioc.GetService<IClassService>();
        private readonly IUserService userService = Ioc.GetService<IUserService>();

        public ResourceController()
        : base(Ioc.GetService<IResourceService>())
        {
        }
#region
        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        public ActionResult Pager()
        {
            return View();
        }
        /// <summary>
        /// 分页 post
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Pager(ResourcePagerParam param)
        {
            var loginUser = ApplicationSignInManager.GetLoginUser();
            var user = loginUser.LoginUser;
            var _user = userService.GetEntiy(user.Id);
            var datail = ObjectMapper.Mapper<T_User, UserPagerParam>(_user);

            param.UserId = user.Id;
            param.RoleId =(int ) datail.RoleId;
            param.ClassIds = datail.ClassIds;
            param.SortBy = "DateTime";
            param.SortOrder = "desc";

            JqgridResult<ResourcePagerModel> modelList = resourceService.GetPager<ResourcePagerModel>(param, "T_User");
            modelList.Rows.ForEach(item =>
            {               
                if (item.ClassId !=null && item.ClassId.Length > 0)
                {
                    item.ClassId = item.ClassId.Substring(0, item.ClassId.Length - 1);
                    item.ClassName = item.ClassName.Substring(0, item.ClassName.Length - 1);
                }                
            });
            return Json(modelList);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return PartialView();
        }
        /// <summary>
        /// 创建post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ResourceCreateModel model)
        {
            if(model.UserId==0)
            {
                var loginInUser = ApplicationSignInManager.GetLoginUser();
                var user = loginInUser.LoginUser;

                model.UserId = user.Id;
            }


            model.DateTime = DateTime.Now;
            for(int i=1;i<2;i++)
            {
                var filedata = Request.Files["file"];
                if(filedata !=null&&filedata.ContentLength !=0)
                {
                    var filename = System.IO.Path.GetFileName(filedata.FileName);
                    filename = GetFileName() + "-" + filename;
                    var virtualPath = string.Format("~Area/Adm/Content/attached/file/{0}" + filename);
                    var path = this.Server.MapPath(virtualPath);
                    filedata.SaveAs(path);
                    var path1= "~/Areas/Adm/Content/attached/file/" + filename;
                }
            }
            model.DownLoadCount = 0;
            string sql = "";
            List<string> sqlDetails = new List<string>();
            List<string> sqls = new List<string>();
           
            string sqlMain = "insert into T_LearningResource" +
                           " (Name,Type,Description,Attachment,AttachmentName,DateTime,UserId,MyURL,MyURLName,DownLoadCount,Remark)" +
                           " values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')";
            sqlMain = string.Format(sqlMain, model.Name, model.Type, model.Description, model.Attachment, model.AttachmentName, model.DateTime, model.UserId, model.MyURL, model.MyURLName, model.DownLoadCount,  model.Remark);

            //分享给资源_班级 model.ClassId,model.ResourceId
            List<string> classIdList = new List<string>();
            if (!string.IsNullOrEmpty(model.ClassIds))
                classIdList = model.ClassIds.Split(',').ToList<string>();
            foreach(string classId in classIdList)
            {
                sql = "insert into T_LearningResourceClass(ResourceId,ClassId) values({0}," + classId + ")";
                sqlDetails.Add(sql);
            }

            //分享给老师 model.UserIds
            List<string> UserIdList = new List<String>();
            if (!string.IsNullOrEmpty(model.UserIds))
                UserIdList = model.UserIds.Split(',').ToList<string>();
            foreach(string teacherId in UserIdList)
            {
                string sharedResourceId = "{0}";
                sql = "insert into T_LearningResource" +
                      "(Name,Type,Description,Attachment,AttachmentName,DateTime,UserId,MyURL,MyURLName,DownLoadCount,SharedUserId,SharedResourceId,Remark) " +
                      "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')";
                sql = string.Format(sql, model.Name, model.Type, model.Description, model.Attachment, model.AttachmentName, model.DateTime, teacherId, model.MyURL, model.MyURLName, model.DownLoadCount, model.UserId, sharedResourceId, model.Remark);
                sqlDetails.Add(sql);
            }

            SqlDBHelper db = new SqlDBHelper();
            db.ExecTansaction(sqlMain, sqlDetails, sqls);
          

            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！"));
        }
        ///
        //public ActionResult count()
        //{
        //    var entiy=resourceService.GetEntiy(mod)
        //}

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            return PartialView();
        }
        /// <summary>
        /// 编辑post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ResourceEditModel model)
        {

            var entiy = resourceService.GetEntiy(model.Id);
            entiy.Name = model.Name;
            entiy.Type = model.Type;
            entiy.Remark = model.Remark;
            entiy.Attachment = model.Attachment;
            entiy.AttachmentName = model.AttachmentName;
            entiy.MyURLName = model.MyURLName;
            entiy.MyURL = model.MyURL;
            entiy.Description = model.Description;

            entiy.DateTime = DateTime.Now;

            return Json(Edit(string.Format("Id={0}", entiy.Id), entiy));
        }
        /// <summary>
        /// 删除post
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            return Json(base.Delete(ids.ToArray(),false));
        }
        /// <summary>
        /// 发布班级post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Publish(int Id, string ClassIds)
        {
            //var entiy = resourceService.GetEntiy(Id);

            List<string> sqls = new List<string>();
            string sql = "";
            //分享给资源_班级 model.ClassId,model.ResourceId
            List<string> classIdList = new List<string>();
            if (!string.IsNullOrEmpty(ClassIds))
                classIdList = ClassIds.Split(',').ToList<string>();
            foreach (string classId in classIdList)
            {
                sql = "insert into T_LearningResourceClass(ResourceId,ClassId) values({0},{1})";
                sql = string.Format(sql, Id, classId);
                sqls.Add(sql);
            }

            SqlDBHelper db = new SqlDBHelper();
            db.ExecTansaction(sqls);

            return Json(DialogFactory.Create(DialogType.Success, "发布成功"));
        }

        /// <summary>
        /// 下载次数增加1
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddDownLoadCount(int id)
        {
            var entiy = resourceService.GetEntiy(id);           
            entiy.DownLoadCount = entiy.DownLoadCount+1;
            
            return Json(Edit(string.Format("Id={0}", entiy.Id), entiy));
        }
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var _resource = resourceService.GetEntiy(id);
            var datail = ObjectMapper.Mapper<T_LearningResource, ResourcePagerModel>(_resource);
            return Json(datail);
        }

        #endregion




        #region 生成文档名字
        private string GetFileName()
        {
            Random rd = new Random();
            StringBuilder serial = new StringBuilder();
            serial.Append(DateTime.Now.ToString("yyyyMMddHHmmssff"));
            serial.Append(rd.Next(0, 999999).ToString());
            return serial.ToString();
        }
        #endregion

    }
}
