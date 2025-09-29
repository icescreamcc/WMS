CREATE TABLE `PurchaseOrder`(
`OrderNo` varchar(50) NOT NULL   COMMENT '采购单号' ,
`ExternalOrderNo` varchar(50) DEFAULT NULL   COMMENT '外部订单号' ,
`PurchaseTypeId` int NOT NULL   COMMENT '采购类型' ,
`PurchaseChannelId` int DEFAULT NULL   COMMENT '采购渠道' ,
`SupplierId` varchar(50) NOT NULL   COMMENT '供应商ID' ,
`SupplierName` varchar(50) NOT NULL   COMMENT '供应商名称' ,
`TotalPrice` float NOT NULL   COMMENT '采购总价' ,
`PriceUnitId` int NOT NULL   COMMENT '计价单位' ,
`PurchaseLevel` varchar(50) DEFAULT NULL   COMMENT '重要级别' ,
`ExpectDate` varchar(50) DEFAULT NULL   COMMENT '预期到货日期' ,
`Status` varchar(50) NOT NULL   COMMENT '单据状态' ,
`IsMakeInvoice` tinyint(1) NOT NULL   COMMENT '是否开票' ,
`InvoiceNumber` varchar(50) DEFAULT NULL   COMMENT '发票编号' ,
`IsAccountPaid` tinyint(1) NOT NULL   COMMENT '是否已付款' ,
`PaymentAmount` float NOT NULL   COMMENT '已付款金额' ,
`YeareAndMonth` int NOT NULL   COMMENT '采购年月' ,
`Remark` varchar(500) DEFAULT NULL   COMMENT '备注' ,
`ApprovalDate` varchar(50) DEFAULT NULL   COMMENT '审批时间' ,
`ApproverId` varchar(50) DEFAULT NULL   COMMENT '审批人ID' ,
`ApproverName` varchar(50) DEFAULT NULL   COMMENT '审批人姓名' ,
`ApproverRole` varchar(50) DEFAULT NULL   COMMENT '审批人角色' ,
`ApprovalStatus` varchar(50) DEFAULT NULL   COMMENT '审批状态' ,
`UpdateUserId` varchar(50) DEFAULT NULL   COMMENT '上次修改人ID' ,
`UpdateUserName` varchar(50) DEFAULT NULL   COMMENT '上次修改人姓名' ,
`UpdateDate` datetime NOT NULL   COMMENT '上次修改时间' ,
`CreateUserId` varchar(50) DEFAULT NULL   COMMENT '创建人ID' ,
`CreateUserName` varchar(50) DEFAULT NULL   COMMENT '创建人姓名' ,
`CreateDate` datetime NOT NULL   COMMENT '创建时间'  , Primary key(`OrderNo`));

CREATE TABLE `PurchaseOrderDetail`(
`DetialId` int NOT NULL  AUTO_INCREMENT COMMENT '明细ID' ,
`OrderNo` varchar(50) NOT NULL   COMMENT '采购单号' ,
`GoodsId` varchar(50) NOT NULL   COMMENT '物品ID' ,
`GoodsName` varchar(50) NOT NULL   COMMENT '物品名称' ,
`Quantity` float NOT NULL   COMMENT '采购数量' ,
`QuantityReturn` float NOT NULL   COMMENT '退货数量' ,
`QuantityUnitId` int NOT NULL   COMMENT '数量单位ID' ,
`Price` float NOT NULL   COMMENT '采购单价' ,
`DetailTotalPrice` float NOT NULL   COMMENT '采购总价' ,
`DetailStatus` varchar(255) DEFAULT NULL   COMMENT '商品状态' ,
`RefundStatus` varchar(255) DEFAULT NULL   COMMENT '退款状态'  , Primary key(`DetialId`));

ALTER TABLE `PurchaseOrder` COMMENT='采购单主表';

ALTER TABLE `PurchaseOrderDetail` COMMENT='采购单明细表';

ALTER TABLE `PurchaseOrder` ADD `PurchaseOperatorId` varchar(20) DEFAULT NULL   COMMENT '采购人ID';  

ALTER TABLE `PurchaseOrder` ADD `PurchaseOperator` varchar(20) DEFAULT NULL   COMMENT '采购人' ; 

ALTER TABLE `PurchaseOrder` ADD `PurchaseOperatorEmail` varchar(100) DEFAULT NULL   COMMENT '采购人邮箱';  

ALTER TABLE `PurchaseOrder` ADD `GoodsClassify` varchar(20) DEFAULT NULL   COMMENT '物品分类';

ALTER TABLE `PurchaseOrder` ADD `IsEmailNotification` tinyint(1) NOT NULL   COMMENT '邮件通知' ; 