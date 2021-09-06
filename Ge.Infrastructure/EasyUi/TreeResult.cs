using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Ge.Infrastructure.EasyUi.Base;

namespace Ge.Infrastructure.EasyUi
{
    /// <summary>
    /// easyui tree数据源
    /// </summary>
    public class TreeResult : IEasyUiResult
    {
        public string PluginName { get { return "Tree"; } }

        public string id { get; set; }

        public string entityName { get; set; }

        public string name { get; set; }

        public string text { get; set; }

        public string iconcls { get; set; }

        public string data_link { get; set; }

        public string iframe { get; set; }

        public List<TreeResult> children { get; set; }
    }
}
