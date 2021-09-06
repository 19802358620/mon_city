namespace Ge.Infrastructure.WebApi.ApiAction
{
    /// <summary>
    /// 作    者：胡政
    /// 创建时间：2015-09-01
    /// 联系方式：13436053642
    ///  描    述：ApiController返回数据具体类
    /// </summary>
    public class ApiActionResult : BaseApiActionResult
    {
        public ApiActionResult(bool? isSucceed, object data)
        {
            base.IsSucceed = isSucceed;
            base.Result = data;
        }

        public ApiActionResult(bool? isSucceed)
        {
            base.IsSucceed = isSucceed;
            base.Result = null;
        }
    }
}
