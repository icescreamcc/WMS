using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class RequisitionOrderDetailDto
    {
        public int DetailId { get; set; }
         
        public string OrderNo { get; set; }

        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string GoodsClassifyName { get; set; }

        public string GoodsModel { get; set; }

        public float Quantity { get; set; }
         
        public float ActualQuantity { get; set; }

        public int UnitId { get; set; }

        public string UnitName { get; set; }

        public int ActualUnitId { get; set; }
         
        public string ActualUnitName { get; set; }
    }
}
