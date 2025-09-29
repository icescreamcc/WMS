using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
   public class SamplePieceDto
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

        public string GoodsLevel { get; set; }

        public bool IsConstraintSpec { get; set; }

        public int GoodsSpecificationId { get; set; }

        public string GoodsSpecificationName { get; set; }

        public float MaxStock { get; set; }

        public string ForArea { get; set; }

        public string Supplier { get; set; }

        public string SupplierName { get; set; }

        public int MinPackageUnitId { get; set; }

        public string MinPackageUnitName { get; set; }

        public int PackageUnitId { get; set; }

        public string PackageUnitName { get; set; }

        public int MaxPackageUnitId { get; set; }

        public string MaxPackageUnitName { get; set; }

        public float PackageCount { get; set; }

        public float MaxPackageCount { get; set; }

        public float SafetyInventory { get; set; }

        public int SafetyInventoryUnitId { get; set; }

        public string SafetyInventoryUnitName { get; set; }

        public float Long { get; set; }

        public float Wide { get; set; }

        public float Height { get; set; }

        public string SizeUnitName { get; set; }

        public float Weight { get; set; }

        public string WeightUnitName { get; set; }

        public string Color { get; set; }

        public string Source { get; set; }

        public float CostPrice { get; set; }

        public string CostPriceUnitName { get; set; }

        public float RefPurchPrice { get; set; }

        public string RefPurchPriceUnitName { get; set; }

        public string PriceUnitName { get; set; }

        public string Direction { get; set; }

        public DateTime ProductDate { get; set; }

        public int ExpirationDate { get; set; }

        public bool IsInSAP { get; set; }

        public float PurchaseCycle { get; set; }

        public float PurchaseMinimum { get; set; }

        public string PurchaseMinimumUnitName { get; set; }

        public bool IsOldForNew { get; set; }

        public string Liveness { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }

        public string Remark { get; set; }

        public bool IsValid { get; set; }

        public string GoodsField1 { get; set; }

        public string GoodsField2 { get; set; }

        public string GoodsField3 { get; set; }

        public string GoodsField4 { get; set; }

        public string GoodsField5 { get; set; }

        public List<FileInfoDto> Photos { get; set; }
    }
}
