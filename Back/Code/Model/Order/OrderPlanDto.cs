using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Order 
{
    public class OrderPlanDto
    {
        public string OrderNo { get; set; }

        public string ContractNo { get; set; }

        public string SigningDate { get; set; }

        public string CustomerName { get; set; }

        public string CustomerNames { get; set; }

        public string GoodsName { get; set; }
        public string GoodsNames { get; set; }
        
        public string OrderNum { get; set; } 

        public string Unit { get; set; }

        public string UnitName { get; set; }

        public string OrderAmount { get; set; }

        public string DeliveryDate { get; set; }

        public string Remarks { get; set; }

        public string ShippedNum { get; set; }

        public string Belial { get; set; }

        public string Invoice { get; set; }

        public string PaymentState { get; set; }

        public string OrderState { get; set; }

        //public DateTime DownTime { get; set; }

        //public DateTime ExpectDate { get; set; }

        public string OrderUrl { get; set; }

        public string CreateDate { get; set; }

        public string ModifyDate { get; set; }

    }
}
