using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class WarehouseElement
    {
        public object Id { get; set; }

        public string  No { get; set; }

        public string Name { get; set; }

        public object ParentId { get; set; }

        public string ParentNo { get; set; }

        public string ParentName { get; set; }

        public string Type { get; set; }

        public int Rank { get; set; }

        public string Props { get; set; }

        public int SpecificationId { get; set; }

        public string SpecificationName { get; set; }

        public string Remark { get; set; }

        public string Status { get; set; }

        public bool HasWorkbin { get; set; }

        public string Product { get; set; }

        public string Code { get; set; }

        public double Total { get; set; }

        public bool IsAbandon { get; set; }

        public bool IsTakeStockLock { get; set; } 
    }
}
