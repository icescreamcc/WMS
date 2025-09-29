using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
    public class BOMDto
    { 
        public string MaterialId { get; set; }
         
        public string MaterialName { get; set; }

        public string MaterialModel { get; set; }

        public string MaterialClassifyGroup { get; set; }

        public string MaterialClassifyName { get; set; }

        public int MinPackageUnitId { get; set; }

        public string MinPackageUnitName { get; set; }

        public int PackageUnitId { get; set; }

        public string PackageUnitName { get; set; }

        public int MaxPackageUnitId { get; set; }

        public string MaxPackageUnitName { get; set; } 

        public string ParentId { get; set; }
         
        public float Quantity { get; set; }
         
        public string Unit { get; set; } 
    }
}
