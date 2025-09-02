using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.AutomationDevice.PLCConfig
{
    public class TransportDBConst
    {
        /// <summary>
        /// 心跳信号，或表示设备通讯正常
        /// </summary>
        public const string Ticks_W = "Ticks_W";

        /// <summary>
        /// 设备状态,1-允许放置物品，0-有物品，不允许放置物品
        /// </summary>
        public const string State_R = "State_R";

        /// <summary>
        /// 启动传送带向外滚动
        /// </summary>
        public const string RollOutside_W = "RollOutside_W";

        /// <summary>
        /// 向外滚动到位信号
        /// </summary>
        public const string RollOutsideFinished_R = "RollOutsideFinished_R";

        /// <summary>
        /// 启动传送带向内滚动
        /// </summary>
        public const string RollInside_W = "RollInside_W";

        /// <summary>
        /// 向内滚动到位信号
        /// </summary>
        public const string RollInsideFinished_R = "RollInsideFinished_R";

        /// <summary>
        /// 任务完成复位信号
        /// </summary>
        public const string Reset_W = "Reset_W";
    }
}
