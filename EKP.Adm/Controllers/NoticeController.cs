using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Role;
using EKP.Base.Controllers;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Utilities;
using EKP.Service.Notice;
using EKP.Base.Identity;
using System.Text;
using System.Linq;
using Ge.Infrastructure.Sql;

namespace EKP.Adm.Controllers
{
   public   class NoticeController : EntityController<T_Notice>
    {
        private readonly INoticeService noticeService = Ioc.GetService<INoticeService>();
        public NoticeController()
            : base(Ioc.GetService<INoticeService>())
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
        public ActionResult Pager(NoticePagerParam param)
        {

          return Json(noticeService.GetPager<NoticePagerModel>(param, "T_Site"));
        }


        public ActionResult NoticePage()
        {
            return View();
        }

        /// <summary>
        /// 学生查看自己班的通知信息（传值）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NoticeStudentPager(NoticePagerParam param)
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            var user = loginInUser.LoginUser;
            param.UserId = user.Id;
            param.ClassIds = user.ClassIds;
            param.SortBy = "DateTime";
            param.SortOrder = "desc";
            
            JqgridResult<NoticePagerModel> modelList = noticeService.SudentGetNoticePager<NoticePagerModel>(param, "T_User");
            modelList.Rows.ForEach(item =>
            {
                item.NoticeState = item.NoticeState == null ? "未发布" : item.NoticeState;
            });
            return Json(modelList);
        }
        /// <summary>
        /// 老师发布通知（传值）
        /// </summary>
        /// <returns></returns>
        public ActionResult NoticeTecherPager(NoticePagerParam param)
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            var user = loginInUser.LoginUser;
            param.UserId = user.Id;

            param.SortBy = "DateTime";
            param.SortOrder = "desc";

            JqgridResult<NoticePagerModel> modelList = noticeService.GetPager<NoticePagerModel>(param, "T_User");

            modelList.Rows.ForEach(item =>
            {
                item.ClassIds = item.ClassIds.Substring(0, item.ClassIds.Length - 1);
                item.ClassNames = item.ClassNames.Substring(0, item.ClassNames.Length - 1);
            });

            return Json(modelList);
        }


        public ActionResult Detail()
        {
            return PartialView();
        }

        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var _notice = noticeService.GetEntiy(id);  
            var detail = ObjectMapper.Mapper<T_Notice, NoticePagerModel>(_notice);
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
        [ValidateInput(false)]
        public ActionResult Create(NoticeCreateModel model)
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            var user = loginInUser.LoginUser;

            model.UserId = user.Id;
            model.DateTime = DateTime.Now;

            for (int i = 1; i < 2; i++)
            {
                var filedata = Request.Files["file"];
                if (filedata != null && filedata.ContentLength != 0)
                {
                    var filename = System.IO.Path.GetFileName(filedata.FileName);
                    filename = GetFileName() + "-" + filename;
                    var virtualPath = string.Format("~/Areas/Adm/Content/attached/noticefile/{0}", filename);
                    var path = this.Server.MapPath(virtualPath);            // 文件系统不能使用虚拟路径
                    filedata.SaveAs(path);
                    var path1 = "~/Areas/Adm/Content/attached/noticefile/" + filename;
                }
            }

            string sql = "";
            List<string> sqlDetails = new List<string>();
            List<string> sqls = new List<string>();

            string sqlMain = " insert into T_Notice (Title,Content,InvalidDateTime,DateTime,UserId,Link,LinkName,Accessory,AccessoryName,Remark)" +
                             "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";//主要的sql语句，向表中插入数据
            sqlMain = string.Format(sqlMain, model.Title, model.Content, model.InvalidDateTime, model.DateTime, model.UserId, model.Link, model.LinkName, model.Accessory, model.AccessoryName, model.Remark);
            List<string> classIdList = new List<string>();
            if (!string.IsNullOrEmpty(model.ClassIds))
                classIdList = model.ClassIds.Split(',').ToList<string>();
            foreach (string classId in classIdList)
            {
                sql = "insert into T_NoticeClass(NoticeId, ClassId) values({0}," + classId + ")";
                sqlDetails.Add(sql);
            }
            SqlDBHelper db = new SqlDBHelper();
            db.ExecTansaction(sqlMain, sqlDetails, sqls);
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
        [ValidateInput(false)]
        public ActionResult Edit(NoticekEditModel model)
        {
            for (int i = 1; i < 2; i++)
            {
                var filedata = Request.Files["file"];
                if (filedata != null && filedata.ContentLength != 0)
                {
                    var filename = System.IO.Path.GetFileName(filedata.FileName);
                    filename = GetFileName() + "-" + filename;
                    var virtualPath = string.Format("~/Areas/Adm/Content/attached/noticefile/{0}", filename);
                    var path = this.Server.MapPath(virtualPath);            // 文件系统不能使用虚拟路径
                    filedata.SaveAs(path);
                    var path1 = "~/Areas/Adm/Content/attached/noticefile/" + filename;
                }
            }
            return Json(Edit(string.Format("id = {0}", model.Id), model, new string[] { "Title", "Content", "InvalidDateTime", "Link", "LinkName", "Accessory", "AccessoryName" }));
        }

        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
          
            return Json(base.Delete(ids.ToArray(), false));
        }

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
