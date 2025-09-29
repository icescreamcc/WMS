using Models.Model.Baseinfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Purchase
{
    public class PurchaseReceiveDto
    {
        public int DetialId { get; set; }

        public string OrderNo { get; set; }

        public string GRNo { get; set; }

        public string ArrivalStatus { get; set; }

        public DateTime ArrivalDate { get; set; }

        public float QuantityArrival { get; set; }
         
        public string ArrivalAbnormalReason { get; set; }

        public string ReceivingStatus { get; set; }

        public DateTime ReceivingDate { get; set; }

        public string ReceivingAbnormalReason { get; set; }

        public int GoodsSpecificationId { get; set; }
         
        public float MaxStock { get; set; }

        public string UpdateUserId { get; set; }

        public string UpdateUserName { get; set; }

        public List<FileInfoDto> GoodsPictureList { get; set; }
    }
}
