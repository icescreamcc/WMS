using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSArgsAGVTaskInput: RCSArgsInput
    {
        /// <summary>
        /// 任务类型 *必填
        /// </summary>
        public string taskTyp { get; set; }

        public string ctnrTyp { get; set; }

        public string ctnrCode { get; set; }

        public string taskMode { get; set; }

        public string taskCode { get; set; }

        public string userCallCode { get; set; }

        /// <summary>
        /// 优先级1-127 值越大优先级越大
        /// </summary>
        public string priority { get; set; }

        /// <summary>
        /// 位置路径：起始地-目的地（type:00 表示传入位置编号） *必填 
        /// </summary>
        public RCS_AGV_Position[] positionCodePath { get; set; }

    }
}
