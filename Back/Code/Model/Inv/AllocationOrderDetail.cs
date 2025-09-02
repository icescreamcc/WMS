using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
   public class AllocationOrderDetail
    {
        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string GoodsClassifyName { get; set; }

        public string GoodsModel { get; set; }

        public float Quantity { get; set; }

        public int UnitId { get; set; }

        public string InShelfId { get; set; }

        public int InBinId { get; set; }

        public int InWorkbinId { get; set; }

        public int InWorkbinCellId { get; set; } 

        public string OutShelfId { get; set; }

        public int OutBinId { get; set; }

        public int OutWorkbinId { get; set; }

        public int OutWorkbinCellId { get; set; }
    }
}
