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

 Date: 25/03/2024 15:53:22
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for receivingorderdetail
-- ----------------------------
DROP TABLE IF EXISTS `receivingorderdetail`;
CREATE TABLE `receivingorderdetail`  (
  `DetialId` int NOT NULL AUTO_INCREMENT COMMENT '明细ID',
  `OrderNo` varchar(50) CHARACTER SET utf8mb4  NOT NULL COMMENT '收货单号',
  `ReceivingAbnormalTypeId` int NOT NULL COMMENT '异常到货类别ID',
  `GoodsId` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '物品ID',
  `GoodsNo` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '物品编码',
  `GoodsClassifyName` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '物品类型',
  `GoodsModel` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '物品型号',
  `GoodsName` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '物品名称',
  `Quantity` float NOT NULL COMMENT '收货数量',
  `QuantityUnitId` int NOT NULL COMMENT '数量单位ID',
  `QuantityUnitName` varchar(10) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '数量单位',
  `Price` float NOT NULL COMMENT '收货单价',
  `WorkpieceTray` int NOT NULL COMMENT '料盘数',
  `Pallet` int NOT NULL COMMENT '托盘数',
  `DetailTotalPrice` float NOT NULL COMMENT '收货总价',
  `DetailStatus` varchar(20) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '状态',
  `ReceivingDate` datetime NOT NULL COMMENT '收货日期',
  `ReceivingOperatorId` varchar(20) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '收货人ID',
  `ReceivingOperatorName` varchar(20) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '收货人名称',
  PRIMARY KEY (`DetialId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 103 CHARACTER SET = utf8mb4  COMMENT = '收货计划明细表' ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
