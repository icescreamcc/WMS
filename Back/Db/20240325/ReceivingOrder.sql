/*
 Navicat Premium Data Transfer

 Source Server         : localhost_3306
 Source Server Type    : MySQL
 Source Server Version : 80032 (8.0.32)
 Source Host           : localhost:3306
 Source Schema         : ims_db

 Target Server Type    : MySQL
 Target Server Version : 80032 (8.0.32)
 File Encoding         : 65001

 Date: 25/03/2024 15:51:57
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for receivingorder
-- ----------------------------
DROP TABLE IF EXISTS `receivingorder`;
CREATE TABLE `receivingorder`  (
  `OrderNo` varchar(50) CHARACTER SET utf8mb4  NOT NULL COMMENT '收货单号',
  `ExternalOrderNo` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '外部订单号',
  `SupplierId` varchar(50) CHARACTER SET utf8mb4  NOT NULL COMMENT '供应商ID',
  `SupplierName` varchar(50) CHARACTER SET utf8mb4  NOT NULL COMMENT '供应商名称',
  `TotalPrice` float NOT NULL COMMENT '收货总价',
  `PriceUnitId` int NOT NULL COMMENT '计价单位ID',
  `ReceivingLevel` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '重要级别',
  `ExpectDate` datetime NOT NULL COMMENT '预期到货日期',
  `IsASN` tinyint(1) NOT NULL COMMENT '是否ASN收货',
  `ASNCheckStatus` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT 'ASN Check状态',
  `Status` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '单据状态',
  `IsMakeInvoice` tinyint(1) NOT NULL COMMENT '是否开票',
  `InvoiceNumber` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '发票编号',
  `IsAccountPaid` tinyint(1) NOT NULL COMMENT '是否已付款',
  `PaymentAmount` float NOT NULL COMMENT '已付款金额',
  `YeareAndMonth` int NOT NULL COMMENT '收货年月',
  `Remark` varchar(500) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '备注',
  `ReceivingResponsableUserId` varchar(20) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '收货责任人ID',
  `ReceivingResponsableUserName` varchar(20) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '收货人责任人名称',
  `ReceivingResponsableUserEmail` varchar(100) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '收货责任人邮箱',
  `IsAutoEmailToReceiving` tinyint(1) NOT NULL COMMENT '是否自动邮件通知收货责任人',
  `ReceivingAddress` varchar(200) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '收货地址',
  `IsEmailNotification` tinyint(1) NOT NULL COMMENT '邮件通知状态',
  `GoodsClassify` varchar(20) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '物品分类',
  `ApprovalDate` datetime NOT NULL COMMENT '审批时间',
  `ApproverId` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '审批人ID',
  `ApproverName` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '审批人姓名',
  `ApproverRole` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '审批人角色',
  `ApprovalStatus` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '审批状态',
  `UpdateUserId` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '上次修改人ID',
  `UpdateUserName` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '上次修改人姓名',
  `UpdateDate` datetime NOT NULL COMMENT '上次修改时间',
  `CreateUserId` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '创建人ID',
  `CreateUserName` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '创建人姓名',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  `PriceUnitName` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '计价单位名称',
  PRIMARY KEY (`OrderNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4  COMMENT = '收货计划主表' ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
