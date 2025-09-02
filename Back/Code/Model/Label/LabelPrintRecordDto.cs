using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Label
{
    public class LabelPrintRecordDto
    {
        public string PackageId { get; set; }
         
        public string LabelId { get; set; }
         
        public string GoodsClassifyGroup { get; set; }
         
        public string GoodsId { get; set; }
         
        public string GoodsName { get; set; }
         
        public string GoodsNo { get; set; }
         
        public string GoodsModel { get; set; }
         
        public float PackageCount { get; set; }
         
        public string PackageUnitName { get; set; }
    }
}
