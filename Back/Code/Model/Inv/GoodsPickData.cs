using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class GoodsPickData
    {
        public string OrderNo { get; set; }

        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string TypeName { get; set; }

        public string GoodsModel { get; set; }

        public string Line { get; set; }

        public float Quantity { get; set; } 

        public int UnitId { get; set; }

        public string UnitName { get; set; }

        public string CreateUserName { get; set; }
         
        public DateTime CreateDate { get; set; }

        public string Status { get; set; }

        public string StatusDesc { get; set; }
    }
}
