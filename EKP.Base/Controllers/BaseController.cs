using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ge.Infrastructure.Metronicv;

namespace EKP.Base.Controllers
{
    /// <summary>
    /// 作    者：胡政
    /// 联系方式：13436053642
    /// 描    述：抽象控制器（其它所有控制器都应该继承或间接继承与这个控制器）
    /// </summary>
    public class BaseController : Controller
    {
        protected BaseController()
        {

        }

        /// <summary>
        /// 单条记录分页post
        /// 根据分页符号分页
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DetailPager(JqgridParam param, string content)
        {
            var rows = content.Split(new string[] { "<hr style=\"page-break-after:always;\" class=\"ke-pagebreak\" />" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var pager = new JqgridResult<string>(param)
            {
                Rows = new List<string>() { rows[param.Page - 1] },
                TotalRecords = rows.Count,
            };

            return Json(pager);
        }
    }
}