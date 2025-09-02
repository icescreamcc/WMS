using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSArgsInput
    {
        /// <summary>
        /// 请求唯一码 *必填
        /// </summary>
        public string reqCode { get; set; }

        public string reqTime { get; set; }

        public string clientCode { get; set; }

        public string tokenCode { get; set; }
          
        public string podTyp { get; set; }

        public string podCode { get; set; }

        public string wbCode { get; set; }
    }
}
