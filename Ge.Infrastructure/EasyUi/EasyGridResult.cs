using System.Collections.Generic;
using Ge.Infrastructure.EasyUi.Base;
using Ge.Infrastructure.Pager;

namespace Ge.Infrastructure.EasyUi
{
    /// <summary>
    /// easyui datagrid查询结果
    /// </summary>
    public class EasyGridResult<TModel> : PagerResult<TModel>, IEasyUiResult
    {
        public string PluginName { get { return "DataGrid"; } }

        public EasyGridResult()
            : base()
        {

        }

        public EasyGridResult(PagerParameter param) 
            : base(param)
        {
            
        }

        /// <summary>
        /// 总条数
        /// </summary>
        public int total { get { return base.TotalRecords; }}

        /// <summary>
        /// 列表
        /// </summary>
        public List<TModel> rows { get { return base.Rows; } }
    }
}
