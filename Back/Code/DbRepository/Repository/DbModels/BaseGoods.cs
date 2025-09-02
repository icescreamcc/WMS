using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("BaseGoods", "物品信息")]
    public class BaseGoods: DeletedModel
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "物品ID")]
        public string GoodsId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品编码")]
        public string GoodsNo { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物品名称")]
        public string GoodsName { get; set; }

        [SugarColumn(ColumnDescription = "物品分类")]
        public int GoodsClassifyId { get; set; } 

        [SugarColumn( IsNullable = true, ColumnDescription = "物品类型")]
        public int GoodsTypeId { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "物品型号")]
        public string GoodsModel { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "物品属性")]
        public string GoodsProperty { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "物品等级")]
        public string GoodsLevel { get; set; }

        [SugarColumn( ColumnDescription = "物品存放规格")]
        public int GoodsSpecificationId { get; set; }

        [SugarColumn(ColumnDescription = "最大堆积数量")]
        public float MaxStock { get; set; }

        [SugarColumn(ColumnDescription = "是否按此规格约束")]
        public bool IsConstraintSpec { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "所属区域")]
        public string ForArea { get; set; }

        [SugarColumn( Length = 50, IsNullable = true, ColumnDescription = "所属供应商")]
        public string Supplier { get; set; }
         
        [SugarColumn(ColumnDescription = "最小包装单位ID")]
        public int MinPackageUnitId { get; set; }

        [SugarColumn(Length = 10, ColumnDescription = "最小包装单位名称")]
        public string MinPackageUnitName { get; set; }

        [SugarColumn(ColumnDescription = "标准包装单位ID")]
        public int PackageUnitId { get; set; }

        [SugarColumn(Length = 20, ColumnDescription = "标准包装单位名称")]
        public string PackageUnitName { get; set; }

        [SugarColumn(ColumnDescription = "最大包装单位ID")]
        public int MaxPackageUnitId { get; set; }

        [SugarColumn(Length = 10, ColumnDescription = "最大包装单位名称")]
        public string MaxPackageUnitName { get; set; }

        [SugarColumn(ColumnDescription = "每标准包装数量")]
        public float PackageCount { get; set; }

        [SugarColumn(ColumnDescription = "每最大包装数量")]
        public float MaxPackageCount { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "安全库存")]
        public float SafetyInventory { get; set; }

        [SugarColumn(ColumnDescription = "库存单位Id")]
        public int SafetyInventoryUnitId { get; set; }

        [SugarColumn(Length = 10, ColumnDescription = "库存单位名称")]
        public string SafetyInventoryUnitName { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "长度")]
        public float Long { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "宽度")]
        public float Wide { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "高度")]
        public float Height { get; set; }

        [SugarColumn(Length = 10, IsNullable = true, ColumnDescription = "尺寸单位名称")]
        public string SizeUnitName { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "重量")]
        public float Weight { get; set; }

        [SugarColumn(Length = 10, IsNullable = true, ColumnDescription = "重量单位名称")]
        public string WeightUnitName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "颜色")]
        public string Color { get; set; }
  
        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "来源产地")]
        public string Source { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "成本价")]
        public float CostPrice { get; set; }

        [SugarColumn(Length = 10, IsNullable = true, ColumnDescription = "成本价计数单位")]
        public string CostPriceUnitName { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "采购参考价")]
        public float RefPurchPrice { get; set; }

        [SugarColumn(Length = 10, IsNullable = true, ColumnDescription = "采购参考价计数单位")]
        public string RefPurchPriceUnitName { get; set; }

        [SugarColumn(Length = 10, IsNullable = true, ColumnDescription = "价格单位")]
        public string PriceUnitName { get; set; }
          
        [SugarColumn(Length = 1000, IsNullable = true, ColumnDescription = "用途")]
        public string Direction { get; set; }

        [SugarColumn( IsNullable = true, ColumnDescription = "生产日期")]
        public DateTime ProductDate { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "保质期（月）")]
        public int ExpirationDate { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "保修期（天）")]
        public int WarrantyPeriodDate { get; set; }

        [SugarColumn(ColumnDescription = "是否在SAP")]
        public bool IsInSAP { get; set; }

        [SugarColumn(IsNullable =true, ColumnDescription = "采购周期（天）")]
        public float PurchaseCycle { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "最小采购量")]
        public float PurchaseMinimum { get; set; }

        [SugarColumn(IsNullable = true,Length =10, ColumnDescription = "最小采购量单位")]
        public string PurchaseMinimumUnitName { get; set; }

        [SugarColumn(ColumnDescription = "是否支持以旧换新")]
        public bool IsOldForNew { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "活跃度")]
        public string Liveness { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "创建人")]
        public string CreateUser { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime CreateDate { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "修改人")]
        public string ModifyUser { get; set; }

        [SugarColumn(IsNullable =true, ColumnDescription = "修改时间")]
        public DateTime ModifyDate { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(ColumnDescription = "是否生效")]
        public bool IsValid { get; set; } 

        [SugarColumn(ColumnDescription = "盘点锁定")]
        public bool IsTakeStockLock { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "上次盘点日期")]
        public string LastInventoryDate { get; set; } 

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "上次盘点人员")]
        public string LastInventoryOperator { get; set; }

        [SugarColumn(ColumnDescription = "是否不需要采购")]
        public bool IsNotPurchase { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "客户料号")]
        public string CustomerGoodsNo { get; set; }

        [SugarColumn(Length = 200, ColumnDescription = "客户识别码")]
        public string CustomerIdentificationCode { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "预留字段")]
        public string GoodsField1 { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "预留字段")]
        public string GoodsField2 { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "预留字段")]
        public string GoodsField3 { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "预留字段")]
        public string GoodsField4 { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "预留字段")]
        public string GoodsField5 { get; set; }
    }
}
