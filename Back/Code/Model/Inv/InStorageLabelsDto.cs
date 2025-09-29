using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class InStorageLabelsDto
    {  
        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string GoodsModel { get; set; }

        public string GoodsClassifyName { get; set; }

        public string GoodsClassifyGroup { get; set; }
         
        public string CodeString { get; set; }

        public string ScanNo { get; set; }

        public string WarehouseId { get; set; }
         
        public string ShelfId { get; set; }
         
        public int BinId { get; set; }

        public string BinNo { get; set; }

        public int WorkbinId { get; set; } 

        public int WorkbinCellId { get; set; }

        public string WorkbinCellNo { get; set; }

        public int UnitId { get; set; }
         
        public string UnitName { get; set; }
         
        public float Quantity { get; set; }
         
        public string PatchNum { get; set; }
         
        public string Status { get; set; }

        public string StatusDesc { get; set; }

        public string CreateUserId { get; set; }
         
        public string CreateUserName { get; set; }
         
        public DateTime CreateDate { get; set; }

        public string UpdateUserId { get; set; }
         
        public string UpdateUserName { get; set; }
         
        public DateTime UpdateDate { get; set; }
    }
}
