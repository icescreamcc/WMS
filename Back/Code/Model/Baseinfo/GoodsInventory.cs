using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
    public class GoodsInventory
    {
        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string GoodsClassifyGroup { get; set; }

        public string GoodsClassifyName { get; set; }

        public string GoodsPicture { get; set; }

        public string GoodsTypeName { get; set; }

        public string GoodsModel { get; set; }

        public int WorkbinId { get; set; }

        public string WorkbinNo { get; set; }

        public int CellId { get; set; }

        public string CellNo { get; set; }

        public float Stock { get; set; }

        public float StockActual { get; set; }

        public int UnitId { get; set; }

        public string UnitName { get; set; }

        public string Remark { get; set; }

        public string RemarkType { get; set; }

        public double TotalPrice { get; set; }

        public double UnitPrice { get; set; }

        public string PriceUnit { get; set; }
    }
}
