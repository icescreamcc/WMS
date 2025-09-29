using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class TakeStockGoodsDto
    {
        public string GoodsId { get; set; }

        public string GoodsName { get; set; }

        public string GoodsModel { get; set; }

        public string GoodsClassifyGroup { get; set; } 

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public string Remark { get; set; }

        public double TotalPrice { get; set; }
         
        public double UnitPrice { get; set; }

        public string RemarkType { get; set; }

        public string PriceUnit { get; set; }

        public List<InvstoryDetail> InventoryDetails { get; set; }

    }
}
