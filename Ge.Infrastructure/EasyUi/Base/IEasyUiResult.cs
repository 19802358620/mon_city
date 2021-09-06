namespace Ge.Infrastructure.EasyUi.Base
{
    /// <summary>
    /// EasyUi结果基础接口（所有EasyUi返回的json结果都应该实现这个接口）
    /// </summary>
    public interface IEasyUiResult
    {
        /// <summary>
        /// EasyUi插件名（Dialog、DataGrid等）
        /// </summary>
        string PluginName { get; }
    }
}
