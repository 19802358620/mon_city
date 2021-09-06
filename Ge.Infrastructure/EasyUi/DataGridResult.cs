using System.Collections.Generic;
using Ge.Infrastructure.EasyUi.Base;

namespace Ge.Infrastructure.EasyUi
{
    /// <summary>
    /// easyui datagrid查询结果
    /// </summary>
    public class DataGridResult<TModel> : IEasyUiResult
    {
        public string PluginName { get { return "DataGrid"; } }

        /// <summary>
        /// 总条数
        /// </summary>
        public long total { get; set; }

        /// <summary>
        /// 列表
        /// </summary>
        public List<TModel> rows { get; set; }
    }
}
