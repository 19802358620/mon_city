using System.Web.Mvc;

namespace EKP.Front.Controllers.Base
{
    /// <summary>
    /// 作    者：胡政
    /// 联系方式：13436053642
    ///  描    述：抽象控制器（其它所有控制器都应该继承或间接继承与这个控制器）
    /// </summary>
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {

        }
    }
}