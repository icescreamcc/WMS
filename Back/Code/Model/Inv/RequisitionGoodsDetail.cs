using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class RequisitionGoodsDetail
    {
        public string OrderNo { get; set; }

        public string OrderType { get; set; }

        public string GoodsClassifyGroup { get; set; }

        public string Line { get; set; }

        public string Purpose { get; set; }

        public string Remark { get; set; }

        public string Status { get; set; }

        public string StatusDesc { get; set; }

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime CreateDate { get; set; }

        public string UpdateUserId { get; set; }

        public string UpdateUserName { get; set; }

        public DateTime UpdateDate { get; set; }

        public int DetailId { get; set; }

        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string GoodsClassifyName { get; set; }

        public string GoodsModel { get; set; }

        public string Url { get; set; }

        public float Quantity { get; set; }

        public float ActualQuantity { get; set; }

        public string UnitName { get; set; }
          
        public string ActualUnitName { get; set; }
    }
}
