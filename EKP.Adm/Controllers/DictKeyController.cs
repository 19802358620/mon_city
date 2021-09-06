using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.DictKey;
using EKP.Service.DictValue;
using EKP.Base.Controllers;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Utilities;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 字典键管理 
    /// </summary>
    public class DictKeyController : EntityController<T_DictKey>
    {
        private readonly IDictKeyService dictKeyService = Ioc.GetService<IDictKeyService>();

        public DictKeyController()
            : base(Ioc.GetService<IDictKeyService>())
        {
            
        }

        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int? id, string key)
        {
            var dictKey = (T_DictKey)null;

            if (id > 0)
            {
                dictKey = dictKeyService.GetEntiy(Convert.ToInt32(id));
            }
            else if (!string.IsNullOrEmpty(key))
            {
                dictKey = dictKeyService.GetEntiy(string.Format("[Key] = '{0}'", key));
            }

            var detail = ObjectMapper.Mapper<T_DictKey, DictKeyPagerModel>(dictKey);
            return Json(detail);
        }
    }
}