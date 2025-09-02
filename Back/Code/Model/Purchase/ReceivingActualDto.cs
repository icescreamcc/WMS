using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Purchase
{
    public class ReceivingActualDto
    {
        public string OrderNo { get; set; }

        public int DetialId { get; set; }

        /// <summary>
        /// 异常到货类别
        /// </summary>
        public string ReceivingAbnormalType { get; set; }

        /// <summary>
        /// 异常到货描述
        /// </summary>
        public string ReceivingAbnormalDesc { get; set; }

        /// <summary>
        /// 实际收货数量
        /// </summary>
        public float QuantityActual { get; set; }

        public string ReceivingOperatorId { get; set; }

        public string ReceivingOperatorName { get; set; }
    }
}
