using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ge.Infrastructure.Pager;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.Base.WebServiceInterface
{
    /// <summary>
    /// web数据接口分页请求参数
    /// </summary>
    public class JqgridWebServiceInterfaceParam : PagerParameter
    {
        private int page = 1;
        private int pageSize = 10;

        ///<summary>
        /// 当前页位置
        /// </summary>
        public virtual int Page
        {
            get { return page; }
            set { page = value; }
        }

        /// <summary>
        /// 每页显示的条目数
        /// </summary>
        public virtual int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        /// <summary>
        /// 条目起始位置
        /// </summary>
        public override int Skip
        {
            get { return PageSize * (Page - 1); }
        }

        /// <summary>
        /// 条目数
        /// </summary>
        public override int Take
        {
            get { return PageSize; }
        }
    }
}
