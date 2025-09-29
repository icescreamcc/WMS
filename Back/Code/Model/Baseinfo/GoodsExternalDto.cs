using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
    public class GoodsExternalDto
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
          
        public string Supplier { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
