using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSArgsAGVContinueInput: RCSArgsInput
    {
        /// <summary>
        /// 下一个位置信息（用于继续执行任务接口）
        /// </summary>
        public RCS_AGV_Position nextPositionCode { get; set; }
    }
}
