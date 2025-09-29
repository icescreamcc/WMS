 
namespace Logic.LogicBase
{
   public class BusinessConst
    {
        /// <summary>
        /// 系统默认管理员用户
        /// </summary>
        public const string UserAdmin = "admin";

        /// <summary>
        /// 系统默认管理员角色
        /// </summary>
        public const string RoleAdmin = "ADMIN"; 

        /// <summary>
        /// 系统版本
        /// </summary>
        public const string SysVersion = "SysVersion";

        /// <summary>
        /// 用户初始密码
        /// </summary>
        public const string InitialPassword = "InitialPassword";

        /// <summary>
        /// 超级管理员账户
        /// </summary>
        public const string DeveloperAccount = "AdminAccount";

        /// <summary>
        /// 产品图片上传限制
        /// </summary>
        public const string GoodsPhotoLimit = "GoodsPhotoLimit";
          
        /// <summary>
        /// 是否启用入库审批
        /// </summary>
        public const string IsInStorageApproval = "IsInStorageApproval";

        /// <summary>
        /// 是否启用出库审批
        /// </summary>
        public const string IsOutStorageApproval = "IsOutStorageApproval";

        /// <summary>
        /// 是否启用收货计划审批
        /// </summary>
        public const string IsReceivingApproval = "IsReceivingApproval";

        /// <summary>
        /// 是否启用采购订单审批
        /// </summary>
        public const string IsPurchaseOrderApproval = "IsPurchaseOrderApproval";

        /// <summary>
        /// 启用安全库存审批
        /// </summary>
        public const string IsSafetyInventoryApproval = "IsSafetyInventoryApproval";

        /// <summary>
        /// 入库隔离（间隔天数）
        /// </summary>
        public const string InStorageIsolateDays = "InStorageIsolateDays";

        /// <summary>
        /// 小车码前缀
        /// </summary>
        public const string CarCodePrefix = "CarCodePrefix";

        /// <summary>
        /// 指定成品下线缓存仓类型
        /// </summary>
        public const string ProductBufferWarehouse = "ProductBufferWarehouse";

        /// <summary>
        /// 指定拆垛机器仓库类型
        /// </summary>
        public const string UnstackWarehouse = "UnstackWarehouse";

        /// <summary>
        /// 是否启用先进先出
        /// </summary>
        public const string IsFIFO = "IsFIFO";
          
        /// <summary>
        /// 产品等级
        /// </summary>
        public const string ProductLevel = "ProductLevel";

        /// <summary>
        /// 产品属性
        /// </summary>
        public const string ProductProperty = "ProductProperty";

        /// <summary>
        /// 备件类型
        /// </summary>
        public const string SparePartType = "SparePartType";

        /// <summary>
        /// 备件类型
        /// </summary>
        public const string ConsumablesType = "ConsumablesType";

        /// <summary>
        /// 样件类型
        /// </summary>
        public const string SamplePieceType = "SamplePieceType";

        /// <summary>
        /// 辅材类型
        /// </summary>
        public const string SeparatorType = "SeparatorType";

        /// <summary>
        /// 包材类型
        /// </summary>
        public const string PackingMaterialType = "PackingMaterialType";

        /// <summary>
        /// 成品类型
        /// </summary>
        public const string FinishedProductType = "FinishedProductType";

        /// <summary>
        /// 原材料类型
        /// </summary>
        public const string RawMaterialType = "RawMaterialType";

        /// <summary>
        /// 异常收货分类
        /// </summary>
        public const string AbnormalReceiptClassification = "AbnormalReceiptClassification";

        /// <summary>
        /// 工厂编码
        /// </summary>
        public const string PlantNo = "CS";
          
        /// <summary>
        /// 胶箱编号等同货位编号
        /// </summary>
        public const string IsWorkbinNoSameBinNo = "IsWorkbinNoSameBinNo";

        /// <summary>
        /// 供应商类型
        /// </summary>
        public const string SupplierType = "SupplierType";

        /// <summary>
        /// 供应商性质
        /// </summary>
        public const string SupplierProperty = "SupplierProperty";

        /// <summary>
        /// 付款方式
        /// </summary>
        public const string CreditType = "CreditType";

        /// <summary>
        /// 采购渠道
        /// </summary>
        public const string PurchaseChanne = "PurchaseChanne";

        /// <summary>
        /// 采购类型
        /// </summary>
        public const string PurchaseType = "PurchaseType";

        /// <summary>
        /// 采购CPMG
        /// </summary>
        public const string Purchase_CPMG = "Purchase-CPMG";

        /// <summary>
        /// 采购科目
        /// </summary>
        public const string Purchase_Account = "Purchase-Account";

        /// <summary>
        /// 采购类别
        /// </summary>
        public const string Purchase_Classes = "Purchase-Classes";

        /// <summary>
        /// 备件领用类型
        /// </summary>
        public const string SparePartRequisitionType = "SparePartRequisitionType";

        /// <summary>
        /// 通用领用类型 
        /// </summary>
        public const string GeneralRequisitionType = "GeneralRequisitionType";

        /// <summary>
        /// 收货计划-收货异常类型
        /// </summary>
        public const string ReceivingAbnormalType = "ReceivingAbnormalType"; 

        /// <summary>
        /// 收发货地址
        /// </summary>
        public const string ReceivingAddress = "ReceivingAddress"; 

        /// <summary>
        /// 自动邮件通知（物料需求计划）
        /// </summary>
        public const string IsAutoSendMailByMaterialRequirement = "IsAutoSendMailByMaterialRequirement";

        /// <summary>
        /// 自动邮件通知（安全库存预警）
        /// </summary>
        public const string IsAutoSendMailBySavetyInvenstory = "IsAutoSendMailBySavetyInvenstory";

        /// <summary>
        /// 盘点盈亏原因分类
        /// </summary>
        public const string TakeStockReasonType = "TakeStockReasonType";

        /// <summary>
        /// 产线
        /// </summary>
        public const string Line = "Line";
    }
}
