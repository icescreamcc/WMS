using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum AGVTaskStatus
    {
        /// <summary>
        /// 待执行
        /// </summary>
        [Description("待执行")]
        Await,

        /// <summary>
        /// AGV已启动
        /// </summary>
        [Description("AGV已启动")]
        Start,

        /// <summary>
        /// AGV走出储位（也当作到达仓储位用）
        /// </summary>
        [Description("AGV已到达储位")]
        Arrive,

        /// <summary>
        /// AGV等待放下料箱
        /// </summary>
        [Description("AGV等待放下料箱")]
        WaitPutdown,

        /// <summary>
        /// AGV已放下料箱
        /// </summary>
        [Description("AGV已放下料箱")]
        Putdown,

        /// <summary>
        /// AGV等待取走料箱
        /// </summary>
        [Description("AGV等待取走料箱")]
        WaitePickup,

        /// <summary>
        /// AGV已取走料箱
        /// </summary>
        [Description("AGV已取走料箱")]
        Pickup,

        /// <summary>
        /// AGV任务结束
        /// </summary>
        [Description("AGV任务已结束")]
        End,

        /// <summary>
        /// AGV任务取消
        /// </summary>
        [Description("AGV任务已取消")]
        Cancel
    }
}
