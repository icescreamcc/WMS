using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
    public class GoodsTakeStockDto
    {
        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public int GoodsClassifyId { get; set; }

        public string GoodsClassifyName { get; set; }

        public int GoodsTypeId { get; set; }

        public string GoodsTypeName { get; set; }

        public string GoodsModel { get; set; }

        public string GoodsProperty { get; set; }

        public string GoodsLevel { get; set; }

        public string ForArea { get; set; }

        public string Supplier { get; set; }
          
        public float SafetyInventory { get; set; } 

        public string SafetyInventoryUnitName { get; set; } 

        public float PurchaseCycle { get; set; }

        public float PurchaseMinimum { get; set; }

        public string PurchaseMinimumUnitName { get; set; } 

        public string Remark { get; set; } 

        public string Photo { get; set; }

        public string LastInventoryDate { get; set; }
         
        public string LastInventoryOperator { get; set; }
    }
}
