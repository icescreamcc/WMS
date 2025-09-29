using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class Shelf
    {
        public string ShelfId { get; set; }
         
        public string WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public string ShelfNo { get; set; }

        public string ShelfName { get; set; }

        public string Size { get; set; }
         
        public string Property { get; set; }
         
        public string Remark { get; set; }

        public bool IsAbandon { get; set; }

        public bool HasWorkbin { get; set; }

        public int Rank { get; set; }
    }
}
