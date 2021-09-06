using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.WebApi.ApiAction
{
    /// <summary>
    /// 作    者：胡政
    /// 创建时间：2015-09-01
    /// 联系方式：13436053642
    ///  描    述：ApiController返回数据基类
    /// </summary>
    public abstract class BaseApiActionResult
    {
        private bool? isSucceed = true;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool? IsSucceed
        {
            get { return isSucceed; }
            set { isSucceed = value; }
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 返回数据的类型（前台可以根据类型决定做何种操作）
        /// </summary>
        public string DataKind
        {
            get
            {
                if (Result == null)
                    return null;
                return Result.GetType().Name;
            }
        }

    }

}
