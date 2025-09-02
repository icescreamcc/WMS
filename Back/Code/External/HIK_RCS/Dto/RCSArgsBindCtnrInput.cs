using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSArgsBindCtnrInput: RCSArgsInput
    {
        /// <summary>
        /// 容器类型
        /// </summary>
        public string ctnrTyp { get; set; }

        /// <summary>
        /// 容器编号
        /// </summary>
        public string ctnrCode { get; set; }

        /// <summary>
        /// 仓位编码
        /// </summary>
        public string stgBinCode { get; set; }

        /// <summary>
        /// 绑定：1/解绑：0
        /// </summary>
        public string indBind { get; set; }
    }
}
