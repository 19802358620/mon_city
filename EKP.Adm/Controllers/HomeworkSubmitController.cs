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
    public class HomeworkSubmitController : EntityController<T_HomeworkSubmit>
    {
        //private readonly IHomeworkService homeworkService = Ioc.GetService<IHomeworkService>();
        //private readonly IHomeworkClassService homeworkClassService = Ioc.GetService<IHomeworkClassService>();
        //private readonly IClassService classService = Ioc.GetService<IClassService>();
        private readonly IHomeworkSubmitService homeworksubmitservice = Ioc.GetService<IHomeworkSubmitService>();
        private readonly IHomeworkService homeworkService = Ioc.GetService<IHomeworkService>();

        public HomeworkSubmitController()
            : base(Ioc.GetService<IHomeworkSubmitService>())
        {

        }


        [HttpPost]
        public ActionResult SubmitHomeworkPager(HomeworkSubmitPagerParm param)
        {

            JqgridResult<HomeworkSubmitPagerModel> modelList = homeworksubmitservice.GetPager<HomeworkSubmitPagerModel>(param, "T_User");
            
            //if (modelList.Rows.Count >0)
            //    ViewBag.ScoreDegree = modelList.Rows[0].ScoreDegree;
            //if(modelList.Rows[0].DealDateTime==)
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

        /// <summary>
        /// 打分
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PlayScore(HomeworkSubmitEditModel model)
        {
            //model.Score = model.Score;
            //var loginInUser = ApplicationSignInManager.GetLoginUser();
            //var user = loginInUser.LoginUser;
            //model.UserId = user.Id;

            var entity = homeworksubmitservice.GetEntiy(model.Id);
            entity.Score = model.Score;
            entity.DealDateTime = DateTime.Now;

            Dialog dialog =Edit(string.Format("Id={0}",model.Id), entity, new string[] { "Score", "DealDateTime" });
            return Json(dialog);
        }

        /// <summary>
        /// 提交作业
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult HomeworkSubmit(HoemworkSubmitModel model)
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            var user = loginInUser.LoginUser;
            var hh= Int32.Parse((Request.Form["Id"].ToString())); 
            model.HomeworkId = Int32.Parse((Request.Form["Id"].ToString()));
            model.UserId = user.Id;
            model.SubmitDateTime = DateTime.Now;           
            model.Status = "已提交";

            for (int i = 1; i < 2; i++)
            {
                var filedata = Request.Files["file"];
                if (filedata != null && filedata.ContentLength != 0)
                {
                    var filename = System.IO.Path.GetFileName(filedata.FileName);
                    filename = GetFileName() + "-" + filename;
                    var virtualPath = string.Format("~/Areas/Adm/Content/attached/submitfile/{0}", filename);
                    var path = this.Server.MapPath(virtualPath);
                    filedata.SaveAs(path);
                    var path1 = "~/Areas/Adm/Content/attached/submitfile/" + filename;
                }
            }

            string sql = "";
            List<string> sqlDetails = new List<string>();
            List<string> sqls = new List<string>();

            string sqlMain = "  insert into T_HomeworkSubmit(Answer,Attachment,AttachmentName,HomeworkId,UserId,SubmitDateTime,Status,DealDateTime,Score,Remark)" + "values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";
            ;
            sqlMain = string.Format(sqlMain, model.Answer, model.Attachment, model.AttachmentName, model.HomeworkId, model.UserId, model.SubmitDateTime, model.Status, model.DealDateTime, model.Score, model.Remark);

            SqlDBHelper db = new SqlDBHelper();
            db.ExecTansaction(sqlMain, sqlDetails, sqls);


            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！"));
        }


        public ActionResult ScoreDetail(int Id)
        {
            var _homework = homeworksubmitservice.GetEntiy(Id);
            var detail = ObjectMapper.Mapper<T_HomeworkSubmit, HomeworkSubmitPagerModel>(_homework);
            return Json(detail);
        }

        /// <summary>
        /// 学生提交答案的时候弹出的那个框的页面detail
        /// </summary>
        [HttpPost]
        public ActionResult SubmitAnswerDetail(int id)//所用到的Id是某一个作业的id，
        {
            var _homeworksubmit = homeworkService.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_Homework, HomeworkPagerModel>(_homeworksubmit);               
                detail.Attachment = "点击上传答案";
                detail.AttachmentName = "    ";
                return Json(detail);
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
