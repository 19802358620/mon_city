using EKP.Base.Controllers;
using EKP.Base.Identity;
using EKP.Entity;
using EKP.Repository.Ef;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Class;
using EKP.Service.Homework;
using EKP.Service.HomeworkClass;
using EKP.Service.HomeworkSubmit;
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
    public class HomeworkController : EntityController<T_Homework>
    {
        private readonly IHomeworkService homeworkService = Ioc.GetService<IHomeworkService>();
        private readonly IHomeworkClassService homeworkClassService = Ioc.GetService<IHomeworkClassService>();
        private readonly IClassService classService = Ioc.GetService<IClassService>();
        private readonly IHomeworkSubmitService homeworksubmitservice = Ioc.GetService<IHomeworkSubmitService>();

        public HomeworkController()
            : base(Ioc.GetService<IHomeworkService>())
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
        public ActionResult Pager(HomeworkPagerParm param)
        {
            //JqgridResult<HomeworkPagerModel> modelList = HomeworkService.GetPager<HomeworkPagerModel>(param, "T_User");
            ////List<int> homeworkIdList = new List<int>();
            ////foreach (HomeworkPagerModel model in modelList.Rows)
            ////{
            ////    if (!homeworkIdList.Contains(model.Id))
            ////        homeworkIdList.Add(model.Id);
            ////}
            ////List<HomeworkPagerModel> modelListResult = modelList.Rows.FindAll(h => homeworkIdList.Contains(h.Id));
            ////modelList.Rows = modelListResult;
            //return Json(modelList);

            var loginInUser = ApplicationSignInManager.GetLoginUser();
            var user = loginInUser.LoginUser;
            param.UserId = user.Id;
            if (user.RoleId == 2)
                param.UserId = 0;

            param.SortBy = "DateTime";
            param.SortOrder = "desc";
            JqgridResult<HomeworkPagerModel> modelList = homeworkService.GetPager<HomeworkPagerModel>(param, "T_User");
            modelList.Rows.ForEach(item =>
            {
                item.ClassId = item.ClassId.Substring(0, item.ClassId.Length - 1);
                item.ClassNames = item.ClassNames.Substring(0, item.ClassNames.Length - 1);
            });
            return Json(modelList);
        }


        /// <summary>
        /// 学生查看自己班的离线作业信息（页面）
        /// </summary>
        /// <returns></returns>
        public ActionResult Studenthomeworkpager()
        {
            return View();
        }

        /// <summary>
        /// 学生查看自己班的离线作业信息（传值）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Studenthomeworkpager(HomeworkPagerParm param)
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            var user = loginInUser.LoginUser;
            param.UserId = user.Id;
            param.ClassId = user.ClassIds;

            param.SortBy = "DateTime";
            param.SortOrder = "desc";
            JqgridResult<HomeworkPagerModel> modelList = homeworkService.SudentGetHomeworkPager<HomeworkPagerModel>(param, "T_User");
            modelList.Rows.ForEach(item =>
            {
                item.SubmitSatatus = item.SubmitSatatus == null ? "未提交" : item.SubmitSatatus;
            });
            //modelList.Rows.ForEach(item => {
            //    item.ClassId = item.ClassId.Substring(0, item.ClassId.Length - 1);
            //    item.ClassNames = item.ClassNames.Substring(0, item.ClassNames.Length - 1);
            //});
            return Json(modelList);
        }


        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var _homework = homeworkService.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_Homework, HomeworkPagerModel>(_homework);
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
        public ActionResult Create(HomeworkCreateModel model)
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            var user = loginInUser.LoginUser;

            if (string.IsNullOrEmpty(model.ClassId))
                return Json(DialogFactory.Create(DialogType.Warn, string.Empty, "请选择班级！"));

            model.UserId = user.Id;
            model.DateTime = DateTime.Now;
            for (int i = 1; i < 2; i++)
            {
                var filedata = Request.Files["file"];
                if (filedata != null && filedata.ContentLength != 0)
                {
                    var filename = System.IO.Path.GetFileName(filedata.FileName);
                    filename = GetFileName() + "-" + filename;
                    var virtualPath = string.Format("~/Areas/Adm/Content/attached/file/{0}", filename);
                    var path = this.Server.MapPath(virtualPath);            // 文件系统不能使用虚拟路径
                    filedata.SaveAs(path);
                    var path1 = "~/Areas/Adm/Content/attached/file/" + filename;
                }
            }

            string sql = "";
            List<string> sqlDetails = new List<string>();
            List<string> sqls = new List<string>();

            string sqlMain = "insert into T_Homework(Name,Request,Link,LinkName,Attachment,AttachmentName,UserId,DateTime,StartDateTime,EndDateTime,Status,ScoreDegree,Remark,IsDeleted)" +
                             "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')";
            sqlMain = string.Format(sqlMain, model.Name, model.Request, model.Link, model.LinkName, model.Attachment, model.AttachmentName, model.UserId, model.DateTime, model.StartDateTime, model.EndDateTime, model.Status, model.ScoreDegree, model.Remark, IsDelete.undeleted);

            //分享给班级 model.AnswerAttachment, model.AnswerAttachmentName, 

            List<string> classIdList = new List<string>();
            if (!string.IsNullOrEmpty(model.ClassId))
                classIdList = model.ClassId.Split(',').ToList<string>();
            foreach (string classId in classIdList)
            {
                sql = "insert into T_HomeWorkClass(HomeworkId, ClassId) values({0}," + classId + ")";
                sqlDetails.Add(sql);
            }

            SqlDBHelper db = new SqlDBHelper();
            db.ExecTansaction(sqlMain, sqlDetails, sqls);


            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！"));

            //return DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！");
            // return Json(base.Create(model));
            //return Json("成功");
            //return Content("<script> alert('添加成功');window.location.href = '/Views/Homework/Pager'; </script>");

            //return Content("<script> alert('添加成功');window.location.href = '/WebSite/HomeWork/ManageHomeWork'; </script>");



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
        public ActionResult Edit(HomeworkEditModel model)
        {
            model.IsDeleted = "undeleted";
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            var user = loginInUser.LoginUser;
            model.UserId = user.Id;
            return Json(Edit(string.Format("Id = {0}", model.Id),model, "Name", "Request", "Link", "LinkName", "Attachment", "AttachmentName", "UserId", "StartDateTime", "EndDateTime", "Status", "ScoreDegree", "Remark"));
        }

        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            return Json(base.Delete(ids.ToArray()));
        }

        [HttpPost]
        public ActionResult PublisHomeWork(int Id)
        {
            var homework = homeworkService.GetEntiy(Id);
            homework.Status = "已发布";
            return Json(Edit(string.Format("Id = {0}", homework.Id), homework));
        }

        [HttpPost]
        public ActionResult UndoPublisHomeWork(int Id)
        {
            var homework = homeworkService.GetEntiy(Id);
            homework.Status = "未发布";
            return Json(Edit(string.Format("Id = {0}", homework.Id), homework));
        }

        [HttpPost]
        public ActionResult SubmitHomeworkPager(HomeworkSubmitPagerParm param)
        {

            JqgridResult<HomeworkSubmitPagerModel> modelList = homeworksubmitservice.GetPager<HomeworkSubmitPagerModel>(param, "T_User");
            return Json(modelList);
        }

        /// <summary>
        /// 显示当前哪条学生的作业的详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HomeworkSubmitDetail(int Id)
        {
            var _homeworksubmit = homeworksubmitservice.getHomeworkSubmitModel(Id);
            return Json(_homeworksubmit);
        }


        [HttpPost]
        public ActionResult SubmitAnswer(HomeworkEditModel model)
        {
            var entity = homeworkService.GetEntiy(model.Id);
            entity.AnswerAttachment = model.AnswerAttachment;
            entity.AnswerAttachmentName = model.AnswerAttachmentName;

            Dialog dialog = Edit(string.Format("Id={0}", model.Id), entity, new string[] { "AnswerAttachment", "AnswerAttachmentName" });
            return Json(dialog);
        }


        /// <summary>
        /// 查看答案
        /// </summary>
        [HttpPost]
        public ActionResult AnswerDetail(int id)
        {
            var _homework = homeworkService.GetEntiy(id);
            if (_homework.AnswerAttachment == null)
            {
                var detail = ObjectMapper.Mapper<T_Homework, HomeworkPagerModel>(_homework);
                detail.AnswerAttachment = "老师暂时没有提供答案";
                detail.AnswerAttachmentName = "老师暂时没有提供答案";
                return Json(detail);
            }
            else
            {
                var detail = ObjectMapper.Mapper<T_Homework, HomeworkPagerModel>(_homework);
                return Json(detail);
            }
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
