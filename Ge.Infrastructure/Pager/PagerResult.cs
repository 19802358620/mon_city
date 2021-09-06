using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ge.Infrastructure.Pager
{
    /// <summary>
    /// 名    称：PagerResult
    /// 作    者：胡政
    /// 创建时间：2015-08-22
    /// 联系方式：13436053642
    /// 描    述：分页数据源
    /// </summary>
    [Serializable]
    public class PagerResult<T>
    {
        public PagerResult()
        {
            
        }

        public PagerResult(PagerParameter param)
        {
            this.PageSize = param.Take;
            this.Page = (param.Take > 0 ? param.Skip / param.Take : 0) + 1;
        }

        /// <summary>
        /// 条目集合
        /// </summary>
        public List<T> Rows { get; set; }
        /// <summary>
        /// 总条目数
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// 每页条目数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int Page { get; set; }
    }
}
