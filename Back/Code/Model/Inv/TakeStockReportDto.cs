using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class TakeStockReportDto
    {
        public int FlowId { get; set; }

        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string Group { get; set; }

        public string TypeName { get; set; }

        public string GoodsTypeName { get; set; }

        public string GoodsModel { get; set; }

        public string Supplier { get; set; }

        public string FlowType { get; set; }

        public string FlowTypeDesc { get; set; }

        public float Quantity { get; set; }
          
        public string UnitName { get; set; }
          
        public string WarehouseName { get; set; }
          
        public string ShelfName { get; set; } 

        public string BinName { get; set; }
          
        public string WorkbinNo { get; set; }
         
        public string CellNo { get; set; }

        public string OperateDate { get; set; } 

        public string OperatorName { get; set; }

        public int DateMonth { get; set; }

        public double UnitPrice { get; set; }

        public double TotalPrice { get; set; }

        public string PriceUnit { get; set; }

        public string RemarkType { get; set; }

        public string Remark { get; set; }
    }
}
