using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
   public class GoodsSimple
    {
        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string GoodsClassifyGroup { get; set; }

        public string GoodsClassifyName { get; set; }
        
        public string GoodsTypeName { get; set; }

        public string GoodsModel { get; set; }

        public string GoodsLevel { get; set; }

        public string GoodsProperty { get; set; }

        public DateTime ProductDate { get; set; }
         
        public int ExpirationDate { get; set; }
         
        public bool IsInSAP { get; set; }
         
        public float PurchaseCycle { get; set; }
         
        public float PurchaseMinimum { get; set; }

        public string PurchaseMinimumUnitName { get; set; }

        public int GoodsSpecificationId { get; set; }

        public string GoodsSpecificationName { get; set; }

        public int WarrantyPeriodDate { get; set; }

        public bool IsConstraintSpec { get; set; }

        public float PackageCount { get; set; }

        public float MaxPackageCount { get; set; }

        public int PackageUnitId { get; set; }

        public string PackageUnitName { get; set; }

        public int MinPackageUnitId { get; set; }

        public string MinPackageUnitName { get; set; }

        public int MaxPackageUnitId { get; set; }

        public string MaxPackageUnitName { get; set; }

        public float CostPrice { get; set; }

        public string PriceUnitName { get; set; }

        public string CostPriceUnitName { get; set; }

        public float Stock { get; set; }

        public bool IsTakeStockLock { get; set; }

        public string LastInventoryDate { get; set; }
         
        public string LastInventoryOperator { get; set; }

        public float SafetyInventory { get; set; }

        public string SafetyInventoryUnitName { get; set; }

        public float StandardPackageStock { get; set; }

        public float MinPackageStock { get; set; }

        public float MaxPackageStock { get; set; }

        public string Supplier { get; set; }

        public string GoodsPicture { get; set; }

        public bool IsUnSubmitLabels { get; set; }

        public string DeftStockBin { get; set; }

        public string DeftStockBinCell { get; set; }

        public string GoodsField1 { get; set; }

        public string CustomerGoodsNo { get; set; }

        public string CustomerIdentificationCode { get; set; }
    }
}
