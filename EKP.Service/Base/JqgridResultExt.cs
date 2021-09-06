using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Pager;
using Newtonsoft.Json2.Linq;

namespace EKP.Service.Base
{
    public class JqgridResultExt<JToken> : PagerResult<JToken>
    {
        public JqgridResultExt()
            : base()
        {

        }

        public JqgridResultExt(PagerParameter param)
            : base(param)
        {
            
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get { return TotalRecords % PageSize == 0 ? TotalRecords / PageSize : TotalRecords / PageSize + 1; }
        }

        /// <summary>
        /// 需要在后台处理拼接的html字符串
        /// </summary>
        public string ListHtml { get; set; }
    }
}
