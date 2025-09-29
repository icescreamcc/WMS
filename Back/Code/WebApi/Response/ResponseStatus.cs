using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Response
{
    public enum ResponseStatus
    {
        /// <summary>
        ///  成功
        /// </summary>
        Success,

        /// <summary>
        /// 失败
        /// </summary>
        Failed,

        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 请求被拒绝
        /// </summary>
        Denied, 

        /// <summary>
        /// 请求无权限
        /// </summary>
        NoPermission,

        /// <summary>
        /// 验证票据被占用(在其他客户端重复登录)
        /// </summary>
        Occupied

    }
}
