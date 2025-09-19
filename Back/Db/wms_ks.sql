/*
 Navicat Premium Data Transfer

 Source Server         : 8.153.165.125
 Source Server Type    : MySQL
 Source Server Version : 50740 (5.7.40-log)
 Source Host           : 8.153.165.125:3306
 Source Schema         : wms_ks

 Target Server Type    : MySQL
 Target Server Version : 50740 (5.7.40-log)
 File Encoding         : 65001

 Date: 18/09/2025 09:08:12
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for approvalhis
-- ----------------------------
DROP TABLE IF EXISTS `approvalhis`;
CREATE TABLE `approvalhis`  (
  `ApprovalId` int(11) NOT NULL AUTO_INCREMENT COMMENT '审批记录ID',
  `PrimaryId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '审批数据ID',
  `DataType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '审批数据类型',
  `ApprovalDate` datetime NOT NULL COMMENT '审批时间',
  `ApproverId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '审批人员ID',
  `ApproverName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '审批人员姓名',
  `ApproverRoleId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人员角色ID',
  `ApproverRoleName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人员角色名称',
  `ApprovalModel` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '审批模式',
  `ApprovalRank` int(11) NOT NULL COMMENT '审批顺序',
  `ApprovalStatus` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '审批状态',
  `Opinion` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批意见',
  PRIMARY KEY (`ApprovalId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '审批记录表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of approvalhis
-- ----------------------------

-- ----------------------------
-- Table structure for approvalprocess
-- ----------------------------
DROP TABLE IF EXISTS `approvalprocess`;
CREATE TABLE `approvalprocess`  (
  `ProcessId` int(11) NOT NULL AUTO_INCREMENT COMMENT '审批流程ID',
  `DataType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '审批数据类型',
  `ApproverRole` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '审批人员角色',
  `Rank` int(11) NOT NULL COMMENT '审批顺序',
  `IsLastApproval` tinyint(1) NOT NULL COMMENT '是否终审',
  `IsJump` tinyint(1) NOT NULL COMMENT '是否允许跳审',
  `ApprovalModel` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '审批模式',
  PRIMARY KEY (`ProcessId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '审批流程表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of approvalprocess
-- ----------------------------

-- ----------------------------
-- Table structure for autoproddevice
-- ----------------------------
DROP TABLE IF EXISTS `autoproddevice`;
CREATE TABLE `autoproddevice`  (
  `DeviceId` int(11) NOT NULL AUTO_INCREMENT COMMENT '设备ID',
  `DeviceNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '设备编码',
  `DeviceName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '设备名称',
  `DeviceType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '设备类型',
  `ConnectAddress` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '连接地址',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所属仓库',
  `IsActive` tinyint(1) NOT NULL COMMENT '是否启用',
  `IsUsing` tinyint(1) NOT NULL COMMENT '是否在使用中',
  `AGVTaskId` int(11) NOT NULL COMMENT '当前正在执行的AGV任务ID',
  PRIMARY KEY (`DeviceId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '车间设备' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of autoproddevice
-- ----------------------------

-- ----------------------------
-- Table structure for autoproddeviceagv
-- ----------------------------
DROP TABLE IF EXISTS `autoproddeviceagv`;
CREATE TABLE `autoproddeviceagv`  (
  `AGVId` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `AGVNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'AGV编码',
  `AGVType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'AGV类型',
  `TaskType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '执行任务模板',
  `CtnrType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '搬运容器类型',
  `PositionCodeType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '路径位置类型',
  `IsDeft` tinyint(1) NOT NULL COMMENT '是否默认配置',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '小车状态',
  `UpdateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人ID',
  `UpdateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人姓名',
  `UpdateDate` datetime NOT NULL COMMENT '上次修改时间',
  PRIMARY KEY (`AGVId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '车间设备-AGV' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of autoproddeviceagv
-- ----------------------------

-- ----------------------------
-- Table structure for autoproddeviceplcconfig
-- ----------------------------
DROP TABLE IF EXISTS `autoproddeviceplcconfig`;
CREATE TABLE `autoproddeviceplcconfig`  (
  `DeviceNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '设备编码',
  `SignalName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '信号名称',
  `DeviceType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '设备类型',
  `BinRank` int(11) NOT NULL COMMENT '库位号',
  `SignalType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '信号类型（R/W）',
  `DbRange` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '内存区域',
  `DbOffset` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '偏移量',
  `ValueType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '值类型',
  `DeftVal` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '默认值',
  `Remark` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `TaskType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '获取到该信号后需要执行的任务类型',
  `ActionType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '获取到该信号后需要执行的动作类型',
  `Message` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '提示消息',
  PRIMARY KEY (`DeviceNo`, `SignalName`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of autoproddeviceplcconfig
-- ----------------------------

-- ----------------------------
-- Table structure for autoproddevicewarehouse
-- ----------------------------
DROP TABLE IF EXISTS `autoproddevicewarehouse`;
CREATE TABLE `autoproddevicewarehouse`  (
  `BinId` int(11) NOT NULL AUTO_INCREMENT COMMENT '仓位ID',
  `DeviceId` int(50) NOT NULL COMMENT '设备ID',
  `BinNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '仓位编码',
  `AGVBinCode_Delivery` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'AGV仓位编码-送料点',
  `AGVBinCode_Receive` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'AGV仓位编码-取料点',
  `Sort` int(11) NOT NULL COMMENT '排列',
  `Tier` int(11) NOT NULL COMMENT '层',
  `Column` int(11) NOT NULL COMMENT '列',
  `Row` int(11) NOT NULL COMMENT '排',
  `BinStatus` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '仓位状态',
  PRIMARY KEY (`BinId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '车间设备仓位' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of autoproddevicewarehouse
-- ----------------------------

-- ----------------------------
-- Table structure for autoprodtasktracking
-- ----------------------------
DROP TABLE IF EXISTS `autoprodtasktracking`;
CREATE TABLE `autoprodtasktracking`  (
  `TaskId` int(50) NOT NULL AUTO_INCREMENT,
  `TaskCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '任务编码',
  `AGVReqCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'AGV任务请求码',
  `LineNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '需求产线',
  `OrderNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '业务单号(出库单号或入库单号)',
  `GoodsClassifyGroup` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物料分类',
  `BusinessType` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '业务类型',
  `GoodsInfo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '运送物料信息(Json字符串)',
  `TaskType` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '任务类型(出库或入库)',
  `ActionType` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'AGV动作类型(取料箱/还料箱)',
  `IsExecuting` tinyint(1) NOT NULL COMMENT '是否处于执行中',
  `TaskModel` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '任务模式(单次任务/多次任务)',
  `TaskStatus` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '送料箱时任务状态',
  `IsReturn` tinyint(1) NOT NULL COMMENT '是否已还回料箱',
  `ReturnTaskStatus` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '还料箱时任务状态',
  `StartingDeviceNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '起始地设备编号',
  `StartingDeviceType` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '起始地设备类型',
  `StartingAGVPositionNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '起始地AGV仓位标识点',
  `StartingBinNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '起始地仓位编码',
  `StartingBinRank` int(11) NOT NULL COMMENT '起始地仓位序号',
  `DestinationDeviceNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '目的地设备编号',
  `DestinationDeviceType` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '目的地设备类型',
  `DestinationAGVPositionNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '目的地AGV仓位标识点',
  `DestinationBinNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '目的地仓位编码',
  `DestinationBinRank` int(11) NOT NULL COMMENT '目的地仓位序号',
  `AGVNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'AGV编号',
  `TaskCreateTime` datetime NOT NULL COMMENT '任务创建时间',
  `TaskStartTime` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '任务开始时间',
  `TaskEndTime` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '任务结束时间',
  `OperatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '操作人ID',
  `OperatorName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '操作人姓名',
  PRIMARY KEY (`TaskId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = 'AGV自动化任务追踪表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of autoprodtasktracking
-- ----------------------------

-- ----------------------------
-- Table structure for basebom
-- ----------------------------
DROP TABLE IF EXISTS `basebom`;
CREATE TABLE `basebom`  (
  `MaterialId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品ID',
  `ParentId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '父级ID',
  `MaterialName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品名称',
  `MaterialClassifyGroup` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品分类',
  `Quantity` float NOT NULL COMMENT '所需数量',
  `Unit` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单位',
  PRIMARY KEY (`MaterialId`, `ParentId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = 'BOM信息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of basebom
-- ----------------------------

-- ----------------------------
-- Table structure for baseclients
-- ----------------------------
DROP TABLE IF EXISTS `baseclients`;
CREATE TABLE `baseclients`  (
  `ClientId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '客户ID',
  `ClientNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '客户编码',
  `ClientName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '客户名称',
  `ClientTypeId` int(11) NOT NULL COMMENT '客户类型ID',
  `ClientTypeName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '客户类型名称',
  `ClientProperty` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '客户属性',
  `ClientLevel` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '客户等级',
  `Province` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所在省份',
  `City` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所在城市',
  `Address` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '详细地址',
  `Telephone` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '电话',
  `Mobilephone` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '手机',
  `Email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '邮箱',
  `Wechat` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '微信',
  `Wangwang` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '旺旺',
  `Alipay` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '支付宝',
  `CommonContact` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '常用联系方式',
  `IsImportant` tinyint(1) NULL DEFAULT NULL COMMENT '是否重要客户',
  `Remark` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `CreateUser` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人',
  `CreateDate` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建时间',
  `Consignee` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货人',
  `ConsigneeTel` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货人电话',
  `ConsigneeAddress` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货人地址',
  `IsValid` tinyint(1) NOT NULL COMMENT '是否有效',
  `SpareField1` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `SpareField2` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `SpareField3` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `SpareField4` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `SpareField5` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  PRIMARY KEY (`ClientId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '客户表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of baseclients
-- ----------------------------

-- ----------------------------
-- Table structure for basefiles
-- ----------------------------
DROP TABLE IF EXISTS `basefiles`;
CREATE TABLE `basefiles`  (
  `FileId` int(11) NOT NULL AUTO_INCREMENT COMMENT '文件ID',
  `FileName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '文件名称',
  `FileInfoType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '文件类型',
  `PrimaryId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '对应主表ID',
  `Url` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '文件地址',
  `Path` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '存储路径',
  `IsDeft` tinyint(1) NOT NULL COMMENT '是否默认文件',
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`FileId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '文件存储信息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of basefiles
-- ----------------------------
INSERT INTO `basefiles` VALUES (2, 'webwxgetmsgimg (2).jpg', 'GoodsPhoto', 'S10000001', 'http://8.153.165.125:9000/wms/Image/SparePartPhoto638923245634660786.jpg', NULL, 1, NULL);
INSERT INTO `basefiles` VALUES (3, '17580949496917986690353296920639.jpg', 'InStoragePhoto', 'I10000009', 'http://8.153.165.125:9000/wmsks/Image/Instorage638937205633005942.jpg', NULL, 1, NULL);
INSERT INTO `basefiles` VALUES (4, 'image.jpg', 'InStoragePhoto', 'I10000010', 'http://8.153.165.125:9000/wmsks/Image/Instorage638937248256984144.jpg', NULL, 1, NULL);

-- ----------------------------
-- Table structure for basegoods
-- ----------------------------
DROP TABLE IF EXISTS `basegoods`;
CREATE TABLE `basegoods`  (
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品ID',
  `GoodsNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品编码',
  `GoodsName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品名称',
  `GoodsClassifyId` int(11) NOT NULL COMMENT '物品分类',
  `GoodsTypeId` int(11) NULL DEFAULT NULL COMMENT '物品类型',
  `GoodsModel` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品型号',
  `GoodsProperty` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品属性',
  `GoodsLevel` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品等级',
  `GoodsSpecificationId` int(11) NOT NULL COMMENT '物品存放规格',
  `MaxStock` float NOT NULL COMMENT '最大堆积数量',
  `IsConstraintSpec` tinyint(1) NOT NULL COMMENT '是否按此规格约束',
  `ForArea` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所属区域',
  `Supplier` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所属供应商',
  `MinPackageUnitId` int(11) NOT NULL COMMENT '最小包装单位ID',
  `MinPackageUnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '最小包装单位名称',
  `PackageUnitId` int(11) NOT NULL COMMENT '标准包装单位ID',
  `PackageUnitName` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '标准包装单位名称',
  `MaxPackageUnitId` int(11) NOT NULL COMMENT '最大包装单位ID',
  `MaxPackageUnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '最大包装单位名称',
  `PackageCount` float NOT NULL COMMENT '每标准包装数量',
  `MaxPackageCount` float NOT NULL COMMENT '每最大包装数量',
  `SafetyInventory` float NULL DEFAULT NULL COMMENT '安全库存',
  `SafetyInventoryUnitId` int(11) NOT NULL COMMENT '库存单位Id',
  `SafetyInventoryUnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '库存单位名称',
  `Long` float NULL DEFAULT NULL COMMENT '长度',
  `Wide` float NULL DEFAULT NULL COMMENT '宽度',
  `Height` float NULL DEFAULT NULL COMMENT '高度',
  `SizeUnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '尺寸单位名称',
  `Weight` float NULL DEFAULT NULL COMMENT '重量',
  `WeightUnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '重量单位名称',
  `Color` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '颜色',
  `Source` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '来源产地',
  `CostPrice` float NULL DEFAULT NULL COMMENT '成本价',
  `CostPriceUnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '成本价计数单位',
  `RefPurchPrice` float NULL DEFAULT NULL COMMENT '采购参考价',
  `RefPurchPriceUnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '采购参考价计数单位',
  `PriceUnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '价格单位',
  `Direction` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '用途',
  `ProductDate` datetime NULL DEFAULT NULL COMMENT '生产日期',
  `ExpirationDate` int(11) NULL DEFAULT NULL COMMENT '保质期（月）',
  `WarrantyPeriodDate` int(11) NULL DEFAULT NULL COMMENT '保修期（天）',
  `IsInSAP` tinyint(1) NOT NULL COMMENT '是否在SAP',
  `PurchaseCycle` float NULL DEFAULT NULL COMMENT '采购周期（天）',
  `PurchaseMinimum` float NULL DEFAULT NULL COMMENT '最小采购量',
  `PurchaseMinimumUnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '最小采购量单位',
  `IsOldForNew` tinyint(1) NOT NULL COMMENT '是否支持以旧换新',
  `Liveness` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '活跃度',
  `CreateUser` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人',
  `CreateDate` datetime NULL DEFAULT NULL COMMENT '创建时间',
  `ModifyUser` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '修改人',
  `ModifyDate` datetime NULL DEFAULT NULL COMMENT '修改时间',
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `IsValid` tinyint(1) NOT NULL COMMENT '是否生效',
  `IsTakeStockLock` tinyint(1) NOT NULL COMMENT '盘点锁定',
  `LastInventoryDate` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次盘点日期',
  `LastInventoryOperator` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次盘点人员',
  `GoodsField1` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `GoodsField2` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `GoodsField3` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `GoodsField4` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `GoodsField5` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `IsDeleted` tinyint(1) NOT NULL COMMENT '是否已删除',
  `IsNotPurchase` tinyint(1) NULL DEFAULT NULL COMMENT '是否不需要采购',
  `CustomerGoodsNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '客户料号',
  `CustomerIdentificationCode` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '客户识别码',
  PRIMARY KEY (`GoodsId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '物品信息' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of basegoods
-- ----------------------------
INSERT INTO `basegoods` VALUES ('S10000001', '9850002052600', 'PET 625*0.3', 25, 0, NULL, NULL, NULL, 0, 0, 0, NULL, NULL, 43, '支', 27, '托', 27, '托', 9, 1, 5, 27, '托', 0, 0, 0, NULL, 0, NULL, NULL, NULL, 100, '托', 0, '托', '元', NULL, '1900-01-01 00:00:00', 0, 0, 1, 0, 0, NULL, 0, NULL, '管理员', '2025-09-01 11:52:28', '管理员', '2025-09-01 16:21:57', NULL, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 0, NULL, NULL);
INSERT INTO `basegoods` VALUES ('S10000002', '1', '2', 25, 60, NULL, '3', NULL, 0, 0, 0, NULL, NULL, 31, '袋', 31, '袋', 31, '袋', 1, 1, 0, 31, '袋', 0, 0, 0, NULL, 0, NULL, NULL, NULL, 0, '袋', 0, '袋', '元', NULL, '1900-01-01 00:00:00', 0, 0, 1, 0, 0, NULL, 0, NULL, '管理员', '2025-09-02 14:39:04', NULL, '1900-01-01 00:00:00', NULL, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 0, NULL, NULL);
INSERT INTO `basegoods` VALUES ('S10000003', 'SYK100', '石英矿-100目', 41, 0, NULL, NULL, NULL, 0, 0, 0, NULL, NULL, 8, '吨', 8, '吨', 8, '吨', 1, 1, 0, 8, '吨', 0, 0, 0, NULL, 0, NULL, NULL, NULL, 0, '吨', 0, '吨', '元', NULL, '1900-01-01 00:00:00', 0, 0, 1, 0, 0, NULL, 0, NULL, '管理员', '2025-09-02 14:45:07', '管理员', '2025-09-10 14:34:57', NULL, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 0, '22', NULL);
INSERT INTO `basegoods` VALUES ('S10000004', 'SYYK', '石英原矿', 28, 0, NULL, NULL, NULL, 0, 0, 0, NULL, NULL, 8, '吨', 8, '吨', 8, '吨', 1, 1, 0, 8, '吨', 0, 0, 0, NULL, 0, NULL, NULL, NULL, 0, '吨', 0, '吨', '元', NULL, '1900-01-01 00:00:00', 0, 0, 1, 0, 0, NULL, 0, NULL, '管理员', '2025-09-02 15:30:48', '管理员', '2025-09-10 14:35:14', NULL, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 0, NULL, NULL);
INSERT INTO `basegoods` VALUES ('S10000005', 'SYK200M', '石英矿成品1', 41, 0, NULL, NULL, NULL, 0, 0, 0, NULL, NULL, 8, '吨', 8, '吨', 8, '吨', 1, 1, 0, 8, '吨', 0, 0, 0, NULL, 0, NULL, NULL, NULL, 0, '吨', 0, '吨', '元', NULL, '1900-01-01 00:00:00', 0, 0, 1, 0, 0, NULL, 0, NULL, '管理员', '2025-09-10 14:50:32', '管理员', '2025-09-16 14:55:14', NULL, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL);
INSERT INTO `basegoods` VALUES ('S10000006', 'SYYK', '石英石原矿', 28, 0, NULL, NULL, NULL, 0, 0, 0, NULL, NULL, 8, '吨', 8, '吨', 8, '吨', 1, 1, 0, 8, '吨', 0, 0, 0, NULL, 0, NULL, NULL, NULL, 0, '吨', 0, '吨', '元', NULL, '1900-01-01 00:00:00', 0, 0, 1, 0, 0, NULL, 0, NULL, '管理员', '2025-09-10 15:04:43', NULL, '1900-01-01 00:00:00', NULL, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL);

-- ----------------------------
-- Table structure for basesupplieraccountcredited
-- ----------------------------
DROP TABLE IF EXISTS `basesupplieraccountcredited`;
CREATE TABLE `basesupplieraccountcredited`  (
  `AccountId` int(11) NOT NULL AUTO_INCREMENT COMMENT '账户ID',
  `SupplierId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '供应商ID',
  `AccountTypeId` int(11) NOT NULL COMMENT '账户类型ID',
  `AccountName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '账户名',
  `AccountNumber` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '账户号',
  `IsCommonAccount` tinyint(1) NOT NULL COMMENT '是否常用账户',
  `OpeningBank` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '开户行',
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '账户备注',
  PRIMARY KEY (`AccountId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '供应商收款账户表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of basesupplieraccountcredited
-- ----------------------------

-- ----------------------------
-- Table structure for basesuppliercontact
-- ----------------------------
DROP TABLE IF EXISTS `basesuppliercontact`;
CREATE TABLE `basesuppliercontact`  (
  `ContacttId` int(11) NOT NULL AUTO_INCREMENT COMMENT '账户ID',
  `SupplierId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '供应商ID',
  `ContactPerson` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '联系人',
  `Telephone` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '电话',
  `Mobilephone` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '手机',
  `Email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '邮箱',
  `Wechat` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '微信',
  `Wangwang` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '旺旺',
  `Alipay` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '支付宝',
  `IsDeft` tinyint(1) NOT NULL COMMENT '常用联系方式',
  PRIMARY KEY (`ContacttId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '供应商联系方式' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of basesuppliercontact
-- ----------------------------

-- ----------------------------
-- Table structure for basesuppliers
-- ----------------------------
DROP TABLE IF EXISTS `basesuppliers`;
CREATE TABLE `basesuppliers`  (
  `SupplierId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '供应商ID',
  `SupplierNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '供应商编码',
  `SupplierName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '供应商名称',
  `SupplierTypeId` int(11) NOT NULL COMMENT '供应商类型ID',
  `SupplierTypeName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '供应商类型名称',
  `SupplierPropertyId` int(11) NULL DEFAULT NULL COMMENT '供应商性质ID',
  `SupplierPropertyName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '供应商性质名称',
  `Province` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所在省份',
  `City` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所在城市',
  `Address` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '详细地址',
  `IsImportant` tinyint(1) NULL DEFAULT NULL COMMENT '是否重要供应商',
  `Remark` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `CreateUser` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人',
  `CreateDate` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建时间',
  `Consignee` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货人',
  `ConsigneeTel` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货人电话',
  `ConsigneeAddress` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货人地址',
  `IsValid` tinyint(1) NOT NULL COMMENT '是否有效',
  `SpareField1` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `SpareField2` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `SpareField3` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `SpareField4` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `SpareField5` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `IsDeleted` tinyint(1) NOT NULL COMMENT '是否已删除',
  PRIMARY KEY (`SupplierId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '供应商表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of basesuppliers
-- ----------------------------
INSERT INTO `basesuppliers` VALUES ('S10000001', 'CMDU', '达飞轮船', 121, '运输供应商', 0, NULL, NULL, NULL, NULL, 0, NULL, 'admin', '2025-09-10 14:24:27', NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, 0);

-- ----------------------------
-- Table structure for basetype
-- ----------------------------
DROP TABLE IF EXISTS `basetype`;
CREATE TABLE `basetype`  (
  `TypeId` int(11) NOT NULL AUTO_INCREMENT COMMENT '类型ID',
  `TypeNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '类型编码',
  `TypeName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '类型名称',
  `Remark` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `ParentId` int(11) NOT NULL COMMENT '父级ID',
  `Group` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '类型分组',
  `Rank` int(11) NOT NULL COMMENT '界面展示排序',
  PRIMARY KEY (`TypeId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 42 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '物品类型表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of basetype
-- ----------------------------
INSERT INTO `basetype` VALUES (2, 'Sample_warehouse', '样件管理', NULL, 0, 'SamplePiece', 1);
INSERT INTO `basetype` VALUES (4, 'LD', '螺刀类', NULL, 0, 'SparePart', 1);
INSERT INTO `basetype` VALUES (5, 'MF', '密封类', NULL, 0, 'SparePart', 2);
INSERT INTO `basetype` VALUES (6, 'WJ', '微机类', NULL, 0, 'SparePart', 3);
INSERT INTO `basetype` VALUES (7, 'DZ', '电子类', NULL, 0, 'SparePart', 4);
INSERT INTO `basetype` VALUES (8, 'HJ', '焊接ESD类', NULL, 0, 'SparePart', 5);
INSERT INTO `basetype` VALUES (9, 'JX', '机械类', NULL, 0, 'SparePart', 6);
INSERT INTO `basetype` VALUES (10, 'QD', '气动类', NULL, 0, 'SparePart', 7);
INSERT INTO `basetype` VALUES (11, 'SJ', '视觉类', NULL, 0, 'SparePart', 8);
INSERT INTO `basetype` VALUES (12, 'TJ', '涂胶类', NULL, 0, 'SparePart', 9);
INSERT INTO `basetype` VALUES (13, 'CR', 'C&Robot', NULL, 0, 'SparePart', 10);
INSERT INTO `basetype` VALUES (14, 'MO', 'Montrac', NULL, 0, 'SparePart', 11);
INSERT INTO `basetype` VALUES (15, 'PL', 'Plasma', NULL, 0, 'SparePart', 12);
INSERT INTO `basetype` VALUES (18, 'ZX', '纸箱类', NULL, 0, 'PackingMaterial', 1);
INSERT INTO `basetype` VALUES (19, 'SJL', '塑胶类', NULL, 0, 'PackingMaterial', 1);
INSERT INTO `basetype` VALUES (22, 'TP', '托盘类', NULL, 0, 'PackingMaterial', 1);
INSERT INTO `basetype` VALUES (28, 'Chemical part', '原矿', NULL, 0, 'RawMaterial', 1);
INSERT INTO `basetype` VALUES (32, '清洁/5S用品', '清洁/5S用品', NULL, 0, 'Separator', 1);
INSERT INTO `basetype` VALUES (34, '打包/标签用品', '打包/标签用品', NULL, 0, 'Separator', 1);
INSERT INTO `basetype` VALUES (35, '劳保用品', '劳保用品', NULL, 0, 'Separator', 1);
INSERT INTO `basetype` VALUES (41, 'finish good', '成品', NULL, 0, 'FinishedProduct', 1);

-- ----------------------------
-- Table structure for baseunits
-- ----------------------------
DROP TABLE IF EXISTS `baseunits`;
CREATE TABLE `baseunits`  (
  `UnitId` int(11) NOT NULL AUTO_INCREMENT COMMENT '单位ID',
  `UnitNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单位编码',
  `UnitName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单位名称',
  `UnitType` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单位类型',
  `Remark` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `IsValid` tinyint(1) NOT NULL COMMENT '数据是否生效',
  PRIMARY KEY (`UnitId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 44 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '统计单位表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of baseunits
-- ----------------------------
INSERT INTO `baseunits` VALUES (1, 'CNY', '元', 'Currency', NULL, 1);
INSERT INTO `baseunits` VALUES (2, 'HKD', '港元', 'Currency', NULL, 1);
INSERT INTO `baseunits` VALUES (3, 'TWD', '台币', 'Currency', NULL, 1);
INSERT INTO `baseunits` VALUES (4, 'USD', '美元', 'Currency', NULL, 1);
INSERT INTO `baseunits` VALUES (5, 'EUR', '欧元', 'Currency', NULL, 1);
INSERT INTO `baseunits` VALUES (6, 'GBP', '英镑', 'Currency', NULL, 1);
INSERT INTO `baseunits` VALUES (7, 'RU', '卢布', 'Currency', NULL, 1);
INSERT INTO `baseunits` VALUES (8, 'T', '吨', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (9, 'KG', '千克', 'Weight', NULL, 0);
INSERT INTO `baseunits` VALUES (11, 'KG', '千克', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (12, 'K', '克', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (13, 'CHL', '两', 'Weight', NULL, 1);
INSERT INTO `baseunits` VALUES (14, 'M', '米', 'Size', NULL, 1);
INSERT INTO `baseunits` VALUES (15, 'DM', '分米', 'Size', NULL, 1);
INSERT INTO `baseunits` VALUES (16, 'CM', '厘米', 'Size', NULL, 1);
INSERT INTO `baseunits` VALUES (17, 'MM', '毫米', 'Size', NULL, 1);
INSERT INTO `baseunits` VALUES (18, 'CHZ', '丈', 'Size', NULL, 1);
INSERT INTO `baseunits` VALUES (19, 'CHC', '尺', 'Size', NULL, 1);
INSERT INTO `baseunits` VALUES (20, 'CHGF', '公分', 'Size', NULL, 1);
INSERT INTO `baseunits` VALUES (21, 'M2', '平方米', 'Acreage', NULL, 1);
INSERT INTO `baseunits` VALUES (22, 'FM2', '平方分米', 'Acreage', NULL, 1);
INSERT INTO `baseunits` VALUES (23, 'CM2', '平方厘米', 'Acreage', NULL, 1);
INSERT INTO `baseunits` VALUES (24, 'M3', '立方米', 'Volume', NULL, 1);
INSERT INTO `baseunits` VALUES (25, 'FM3', '立方分米', 'Volume', NULL, 1);
INSERT INTO `baseunits` VALUES (26, 'CM3', '立方厘米', 'Volume', NULL, 1);
INSERT INTO `baseunits` VALUES (27, 'ST', '托', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (28, 'CH', '箱', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (29, 'BOX', '盒', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (30, 'BCK', '桶', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (31, 'BAG', '袋', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (32, 'SING', '个', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (33, 'PIE', '件', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (34, 'BAG2', '包', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (35, 'BOT', '瓶', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (36, 'TI', '听', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (37, 'PI', '片', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (38, 'HU', '壶', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (39, 'BEN', '本', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (40, 'roll', '卷', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (41, 'L', '升', 'Volume', NULL, 1);
INSERT INTO `baseunits` VALUES (42, 'Pan', '盘', 'Pack', NULL, 1);
INSERT INTO `baseunits` VALUES (43, 'Tube', '支', 'Pack', NULL, 1);

-- ----------------------------
-- Table structure for invbin
-- ----------------------------
DROP TABLE IF EXISTS `invbin`;
CREATE TABLE `invbin`  (
  `BinId` int(11) NOT NULL AUTO_INCREMENT COMMENT '货位ID',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '仓库ID',
  `ShelfId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货架ID',
  `BinNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '货位编号',
  `BinName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货位名称',
  `AGVNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'AGV地址编码',
  `Long` double NULL DEFAULT NULL COMMENT '货位长度',
  `Width` double NULL DEFAULT NULL COMMENT '货位宽度',
  `Property` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货位布局',
  `Specification` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货位规格',
  `Remark` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `IsAbandon` tinyint(1) NOT NULL COMMENT '是否已弃用',
  `Rank` int(11) NULL DEFAULT NULL COMMENT '排序',
  `IsTakeStockLock` tinyint(1) NOT NULL COMMENT '盘点锁定',
  `IsVarietyStock` tinyint(1) NOT NULL COMMENT '允许多样化存储',
  `LastInventoryDate` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次盘点日期',
  `LastInventoryOperator` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次盘点人员',
  `Status` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '状态',
  PRIMARY KEY (`BinId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 41 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '仓库货位信息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invbin
-- ----------------------------
INSERT INTO `invbin` VALUES (38, 'W10000001', 'S10000004', 'MaterialBinNo1', '原材料货位1', '', 0, 0, '', '', '', 0, 0, 0, 1, NULL, NULL, 'Free');
INSERT INTO `invbin` VALUES (40, 'W10000003', 'S10000006', 'FinishBinNo1', '成品仓货位1', '', 0, 0, '', '', '', 0, 0, 0, 1, NULL, NULL, 'Free');

-- ----------------------------
-- Table structure for invinstorage
-- ----------------------------
DROP TABLE IF EXISTS `invinstorage`;
CREATE TABLE `invinstorage`  (
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '入库单号',
  `InStorageType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '入库类型',
  `GoodsClassify` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品类型',
  `SourceOrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '数据来源单号',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '入库仓库',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单据状态',
  `CreateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人ID',
  `CreateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  `UpdateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人ID',
  `UpdateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人姓名',
  `UpdateDate` datetime NULL DEFAULT NULL COMMENT '上次修改时间',
  `InstorageDate` datetime NULL DEFAULT NULL COMMENT '入库时间',
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `ResponsibleId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '责任人ID',
  `Responsible` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '责任人',
  `ApprovalDate` datetime NULL DEFAULT NULL COMMENT '审批时间',
  `ApproverId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人ID',
  `ApproverName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人姓名',
  `ApproverRole` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人角色',
  `ApprovalStatus` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批状态',
  `TransportOrderNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '运输单号',
  `LicensePlateNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '车牌单号',
  PRIMARY KEY (`OrderNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '入库单主表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invinstorage
-- ----------------------------
INSERT INTO `invinstorage` VALUES ('I10000001', 'PurchaseIn', 'RawMaterial', NULL, 'W10000001', 'InStorage', 'admin', '管理员', '2025-09-01 11:57:10', NULL, NULL, '1900-01-01 00:00:00', '2025-09-01 11:57:43', NULL, NULL, NULL, '1900-01-01 00:00:00', NULL, NULL, NULL, 'NoApproval', NULL, NULL);
INSERT INTO `invinstorage` VALUES ('I10000003', 'ProductIn', 'FinishedProduct', NULL, 'W10000003', 'InStorage', 'admin', '管理员', '2025-09-04 14:17:46', NULL, NULL, '1900-01-01 00:00:00', '2025-09-04 14:19:49', NULL, NULL, NULL, '1900-01-01 00:00:00', NULL, NULL, NULL, 'NoApproval', NULL, NULL);
INSERT INTO `invinstorage` VALUES ('I10000004', 'PurchaseIn', 'RawMaterial', NULL, 'W10000001', 'InStorage', 'admin', '管理员', '2025-09-04 14:21:02', NULL, NULL, '1900-01-01 00:00:00', '2025-09-04 14:21:07', NULL, NULL, NULL, '1900-01-01 00:00:00', NULL, NULL, NULL, 'NoApproval', NULL, NULL);
INSERT INTO `invinstorage` VALUES ('I10000005', 'ProductIn', 'FinishedProduct', NULL, 'W10000003', 'InStorage', 'admin', '管理员', '2025-09-11 14:11:25', NULL, NULL, '1900-01-01 00:00:00', '2025-09-11 14:11:33', NULL, NULL, NULL, '1900-01-01 00:00:00', NULL, NULL, NULL, 'NoApproval', NULL, NULL);
INSERT INTO `invinstorage` VALUES ('I10000006', 'ProductIn', 'RawMaterial', NULL, 'W10000001', 'InStorage', 'admin', '管理员', '2025-09-11 14:11:40', NULL, NULL, '1900-01-01 00:00:00', '2025-09-11 14:12:44', NULL, NULL, NULL, '1900-01-01 00:00:00', NULL, NULL, NULL, 'NoApproval', NULL, NULL);
INSERT INTO `invinstorage` VALUES ('I10000007', 'ProductIn', 'FinishedProduct', NULL, 'W10000003', 'WaitInStorage', 'admin', '管理员', '2025-09-16 14:56:14', NULL, NULL, '1900-01-01 00:00:00', '1900-01-01 00:00:00', NULL, NULL, NULL, '1900-01-01 00:00:00', NULL, NULL, NULL, 'NoApproval', NULL, NULL);
INSERT INTO `invinstorage` VALUES ('I10000008', 'ProductIn', 'RawMaterial', '', 'W10000001', 'InStorage', 'admin', '管理员', '2025-09-17 00:00:00', NULL, NULL, '1900-01-01 00:00:00', '2025-09-17 15:34:35', '', '', '', '1900-01-01 00:00:00', NULL, NULL, NULL, 'NoApproval', '京东001', '湘m2342');
INSERT INTO `invinstorage` VALUES ('I10000009', 'ProductIn', 'RawMaterial', '', 'W10000001', 'InStorage', 'admin', '管理员', '2025-09-17 00:00:00', NULL, NULL, '1900-01-01 00:00:00', '2025-09-17 15:43:02', '', '', '', '1900-01-01 00:00:00', NULL, NULL, NULL, 'NoApproval', '', '');
INSERT INTO `invinstorage` VALUES ('I10000010', 'ProductIn', 'RawMaterial', '', 'W10000001', 'InStorage', 'admin', '管理员', '2025-09-17 00:00:00', NULL, NULL, '1900-01-01 00:00:00', '2025-09-17 16:54:25', '', '', '', '1900-01-01 00:00:00', NULL, NULL, NULL, 'NoApproval', '', '');

-- ----------------------------
-- Table structure for invinstoragedetail
-- ----------------------------
DROP TABLE IF EXISTS `invinstoragedetail`;
CREATE TABLE `invinstoragedetail`  (
  `DetailId` int(11) NOT NULL AUTO_INCREMENT COMMENT '明细ID',
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '入库单号',
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品ID',
  `GoodsName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品名称',
  `Quantity` float NOT NULL COMMENT '计划入库数量',
  `ActualQuantity` float NOT NULL COMMENT '实际入库数量',
  `UnitId` int(11) NOT NULL COMMENT '入库单位ID',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '入库仓库',
  `ShelfId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '入库货架',
  `BinId` int(11) NOT NULL COMMENT '入库库位',
  `WorkbinId` int(11) NOT NULL COMMENT '胶箱',
  `WorkbinCellId` int(11) NOT NULL COMMENT '胶箱单元格',
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '入库明细备注',
  `TotalPrice` double NOT NULL DEFAULT 0 COMMENT '入库总价',
  `UnitPrice` double NOT NULL DEFAULT 0 COMMENT '入库单价',
  `PriceUnit` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '价格单位',
  `RemarkType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`DetailId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 13 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '入库单明细表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invinstoragedetail
-- ----------------------------
INSERT INTO `invinstoragedetail` VALUES (1, 'I10000001', 'S10000001', 'PET', 12, 12, 27, 'W10000001', 'S10000002', 25, 0, 0, NULL, 1200, 100, '元', NULL);
INSERT INTO `invinstoragedetail` VALUES (3, 'I10000003', 'S10000003', '石英矿-100目', 12, 12, 8, 'W10000003', 'S10000006', 40, 0, 0, NULL, 0, 0, '元', NULL);
INSERT INTO `invinstoragedetail` VALUES (4, 'I10000004', 'S10000004', '石英原矿', 12, 12, 8, 'W10000001', 'S10000004', 38, 0, 0, NULL, 0, 0, '元', NULL);
INSERT INTO `invinstoragedetail` VALUES (7, 'I10000005', 'S10000005', '石英矿-200目', 1, 1, 8, 'W10000003', NULL, 40, 0, 0, NULL, 0, 0, NULL, NULL);
INSERT INTO `invinstoragedetail` VALUES (8, 'I10000006', 'S10000006', '石英石原矿', 1, 1, 8, 'W10000001', NULL, 38, 0, 0, NULL, 0, 0, NULL, NULL);
INSERT INTO `invinstoragedetail` VALUES (9, 'I10000007', 'S10000005', '石英矿成品1', 2, 0, 8, 'W10000003', NULL, 40, 0, 0, NULL, 0, 0, NULL, NULL);
INSERT INTO `invinstoragedetail` VALUES (10, 'I10000008', 'S10000006', '石英石原矿', 1, 1, 8, 'W10000001', 'S10000004', 38, 0, 0, NULL, 0, 0, NULL, NULL);
INSERT INTO `invinstoragedetail` VALUES (11, 'I10000009', 'S10000006', '石英石原矿', 12, 12, 8, 'W10000001', 'S10000004', 38, 0, 0, NULL, 0, 0, NULL, NULL);
INSERT INTO `invinstoragedetail` VALUES (12, 'I10000010', 'S10000006', '石英石原矿', 18, 18, 8, 'W10000001', 'S10000004', 38, 0, 0, NULL, 0, 0, NULL, NULL);

-- ----------------------------
-- Table structure for invinstoragelabels
-- ----------------------------
DROP TABLE IF EXISTS `invinstoragelabels`;
CREATE TABLE `invinstoragelabels`  (
  `CodeString` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '标签码字符串',
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品ID',
  `GoodsClassifyGroup` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品类型分组',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '仓库ID',
  `ShelfId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货架ID',
  `BinId` int(11) NOT NULL COMMENT '货位ID',
  `WorkbinId` int(11) NOT NULL COMMENT '料箱ID',
  `WorkbinCellId` int(11) NOT NULL COMMENT '料箱单元格ID',
  `UnitId` int(11) NOT NULL COMMENT '单位ID',
  `UnitName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单位',
  `Quantity` float NOT NULL COMMENT '每个标签对应数量',
  `PatchNum` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '批次号',
  `Status` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '状态',
  `UpdateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人ID',
  `UpdateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人姓名',
  `UpdateDate` datetime NOT NULL COMMENT '上次修改时间',
  `CreateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人ID',
  `CreateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`CodeString`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invinstoragelabels
-- ----------------------------

-- ----------------------------
-- Table structure for invoutstorage
-- ----------------------------
DROP TABLE IF EXISTS `invoutstorage`;
CREATE TABLE `invoutstorage`  (
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '出库单号',
  `OutStorageType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '出库类型',
  `GoodsClassify` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品类型',
  `LineNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '产线',
  `SourceOrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '数据来源单号',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '出库仓库',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单据状态',
  `CreateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人ID',
  `CreateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  `UpdateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人ID',
  `UpdateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人姓名',
  `UpdateDate` datetime NULL DEFAULT NULL COMMENT '上次修改时间',
  `OutStorageDate` datetime NULL DEFAULT NULL COMMENT '出库时间',
  `Remark` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `ApprovalDate` datetime NULL DEFAULT NULL COMMENT '审批时间',
  `ApproverId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人ID',
  `ApproverName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人姓名',
  `ApproverRole` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人角色',
  `ApprovalStatus` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批状态',
  PRIMARY KEY (`OrderNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '出库单主表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invoutstorage
-- ----------------------------
INSERT INTO `invoutstorage` VALUES ('O10000001', 'SalesOut', 'FinishedProduct', NULL, NULL, 'W10000003', 'OutStorage', 'admin', '管理员', '2025-09-08 15:33:01', NULL, NULL, '1900-01-01 00:00:00', '2025-09-10 14:43:10', NULL, '1900-01-01 00:00:00', NULL, NULL, NULL, 'NoApproval');
INSERT INTO `invoutstorage` VALUES ('O10000002', 'ReceiveOut', 'RawMaterial', NULL, NULL, 'W10000001', 'WaitOutStorage', 'admin', '管理员', '2025-09-11 14:13:07', NULL, NULL, '1900-01-01 00:00:00', '1900-01-01 00:00:00', NULL, '1900-01-01 00:00:00', NULL, NULL, NULL, 'NoApproval');

-- ----------------------------
-- Table structure for invoutstoragedetail
-- ----------------------------
DROP TABLE IF EXISTS `invoutstoragedetail`;
CREATE TABLE `invoutstoragedetail`  (
  `DetailId` int(11) NOT NULL AUTO_INCREMENT COMMENT '明细ID',
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '出库单号',
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品ID',
  `GoodsName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品名称',
  `Quantity` float NOT NULL COMMENT '出库数量',
  `ActualQuantity` float NOT NULL COMMENT '实际出库数量',
  `UnitId` int(11) NOT NULL COMMENT '出库单位ID',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '出库仓库',
  `ShelfId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '出库货架',
  `BinId` int(11) NOT NULL COMMENT '出库库位',
  `WorkbinId` int(11) NOT NULL COMMENT '胶箱',
  `WorkbinCellId` int(11) NOT NULL COMMENT '胶箱单元格',
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '出库明细备注',
  `TotalPrice` double NOT NULL DEFAULT 0 COMMENT '出库总价',
  `UnitPrice` double NOT NULL DEFAULT 0 COMMENT '出库单价',
  `PriceUnit` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '价格单位',
  `RemarkType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`DetailId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '出库单明细表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invoutstoragedetail
-- ----------------------------
INSERT INTO `invoutstoragedetail` VALUES (1, 'O10000001', 'S10000003', '石英矿-100目', 1, 1, 31, 'W10000003', 'S10000006', 40, 0, 0, NULL, 0, 0, '元', NULL);
INSERT INTO `invoutstoragedetail` VALUES (2, 'O10000002', 'S10000006', '石英石原矿', 1, 0, 8, 'W10000001', NULL, 38, 0, 0, NULL, 0, 0, '元', NULL);

-- ----------------------------
-- Table structure for invrequisitionorder
-- ----------------------------
DROP TABLE IF EXISTS `invrequisitionorder`;
CREATE TABLE `invrequisitionorder`  (
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单据编号',
  `OrderType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单据类型',
  `GoodsClassifyGroup` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品类型',
  `Line` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '产线',
  `Purpose` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '领用目的',
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '状态',
  `UpdateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人ID',
  `UpdateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人姓名',
  `UpdateDate` datetime NOT NULL COMMENT '上次修改时间',
  `CreateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人ID',
  `CreateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`OrderNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '物品领用单' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invrequisitionorder
-- ----------------------------

-- ----------------------------
-- Table structure for invrequisitionorderdetail
-- ----------------------------
DROP TABLE IF EXISTS `invrequisitionorderdetail`;
CREATE TABLE `invrequisitionorderdetail`  (
  `DetailId` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单据编号',
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品ID',
  `GoodsName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品名称',
  `Quantity` float NOT NULL COMMENT '计划领用数量',
  `ActualQuantity` float NOT NULL COMMENT '实际领用数量',
  `UnitId` int(11) NOT NULL COMMENT '单位ID(计划)',
  `UnitName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单位(计划)',
  `ActualUnitId` int(11) NOT NULL COMMENT '单位ID(实际)',
  `ActualUnitName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单位(实际)',
  PRIMARY KEY (`DetailId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '物品领用单明细' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invrequisitionorderdetail
-- ----------------------------

-- ----------------------------
-- Table structure for invsafetywarningrecord
-- ----------------------------
DROP TABLE IF EXISTS `invsafetywarningrecord`;
CREATE TABLE `invsafetywarningrecord`  (
  `DetailId` int(11) NOT NULL AUTO_INCREMENT,
  `FlowId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '流程ID',
  `Year` int(11) NOT NULL COMMENT '年',
  `Month` int(11) NOT NULL COMMENT '月',
  `Week` int(11) NOT NULL COMMENT '周',
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品ID',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '状态',
  `PurchaseOrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '采购单号',
  `RequirementQuantity` double NULL DEFAULT NULL COMMENT '需求数量',
  `NeedPurchase` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '是否需要采购',
  `IsNeedPurchase` tinyint(1) NOT NULL COMMENT '是否需要采购',
  `IsSendMail` tinyint(1) NOT NULL COMMENT '是否已发送邮件',
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `ApprovalDate` datetime NULL DEFAULT NULL COMMENT '审批时间',
  `ApproverId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人ID',
  `ApproverName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人姓名',
  `ApproverRole` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人角色',
  `ApprovalStatus` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批状态',
  `UpdateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人ID',
  `UpdateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人姓名',
  `UpdateDate` datetime NOT NULL COMMENT '上次修改时间',
  `CreateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人ID',
  `CreateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`DetailId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '安全库存报警记录表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invsafetywarningrecord
-- ----------------------------

-- ----------------------------
-- Table structure for invshelf
-- ----------------------------
DROP TABLE IF EXISTS `invshelf`;
CREATE TABLE `invshelf`  (
  `ShelfId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '货架ID',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '仓库ID',
  `ShelfNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '货架编号',
  `ShelfName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货架编号',
  `Size` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货架尺寸',
  `Property` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货架布局',
  `Remark` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `IsAbandon` tinyint(1) NOT NULL COMMENT '是否已弃用',
  `HasWorkbin` tinyint(1) NOT NULL COMMENT '是否启用料箱',
  `Rank` int(11) NULL DEFAULT NULL COMMENT '排序',
  PRIMARY KEY (`ShelfId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '仓库货架信息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invshelf
-- ----------------------------
INSERT INTO `invshelf` VALUES ('S10000004', 'W10000001', 'MShelfNo1', '原材料货架1', '', '', '', 0, 0, 0);
INSERT INTO `invshelf` VALUES ('S10000006', 'W10000003', 'GShelfNo1', '成品仓货架1', '', '', '', 0, 0, 0);

-- ----------------------------
-- Table structure for invstorage
-- ----------------------------
DROP TABLE IF EXISTS `invstorage`;
CREATE TABLE `invstorage`  (
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '商品ID',
  `Stock` float NOT NULL COMMENT '库存总量',
  `LastStatisticsDate` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '上次汇总日期',
  `LastOperatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次汇总操作人ID',
  `LastOperatorName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '上次汇总操作人姓名',
  PRIMARY KEY (`GoodsId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '库存表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invstorage
-- ----------------------------
INSERT INTO `invstorage` VALUES ('S10000001', 12, '2025-09-01 11:57:42', 'admin', '管理员');
INSERT INTO `invstorage` VALUES ('S10000003', 0, '2025-09-10 14:43:10', 'admin', '管理员');
INSERT INTO `invstorage` VALUES ('S10000004', 12, '2025-09-04 14:21:07', 'admin', '管理员');
INSERT INTO `invstorage` VALUES ('S10000005', 1, '2025-09-11 14:11:32', 'admin', '管理员');
INSERT INTO `invstorage` VALUES ('S10000006', 32, '2025-09-17 16:54:25', 'admin', '管理员');

-- ----------------------------
-- Table structure for invstorageflowdetail
-- ----------------------------
DROP TABLE IF EXISTS `invstorageflowdetail`;
CREATE TABLE `invstorageflowdetail`  (
  `FlowId` int(11) NOT NULL AUTO_INCREMENT COMMENT '记录ID',
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品ID',
  `FlowType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '流水类型',
  `SourceOrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '数据来源单号',
  `SourceStorageType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '数据来源类型',
  `SourceStorageSubType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '数据来源子类型',
  `Quantity` float NOT NULL COMMENT '出入数量',
  `UnitId` int(11) NOT NULL COMMENT '单位ID',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '仓库',
  `ShelfId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货架',
  `BinId` int(11) NOT NULL COMMENT '库位',
  `WorkbinId` int(11) NULL DEFAULT NULL COMMENT '料箱',
  `WorkbinCellId` int(11) NOT NULL COMMENT '料箱单元格',
  `OperateDate` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '出入时间',
  `OperatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '操作人ID',
  `OperatorName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '操作人姓名',
  `IsStatistics` tinyint(1) NOT NULL COMMENT '是否已计入库存',
  `DateYear` int(11) NOT NULL COMMENT '年份，格式：2023',
  `DateMonth` int(11) NOT NULL COMMENT '月份，格式：202309',
  `DateDay` int(11) NOT NULL COMMENT '月份，格式：20230901',
  `BatchNumber` bigint(20) NOT NULL COMMENT '批次号',
  `ProductionDate` datetime NULL DEFAULT NULL COMMENT '生产日期',
  `TotalPrice` double NOT NULL DEFAULT 0 COMMENT '总价',
  `UnitPrice` double NOT NULL DEFAULT 0 COMMENT '单价',
  `PriceUnit` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '价格单位',
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `RemarkType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`FlowId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 10 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '库存流水明细表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invstorageflowdetail
-- ----------------------------
INSERT INTO `invstorageflowdetail` VALUES (1, 'S10000001', 'In', 'I10000001', 'InStorage', 'PurchaseIn', 12, 27, 'W10000001', 'S10000002', 25, 0, 0, '2025-09-01 11:57:42', 'admin', '管理员', 1, 2025, 202509, 20250901, 20250901115742, '1900-01-01 00:00:00', 1200, 100, '元', NULL, NULL);
INSERT INTO `invstorageflowdetail` VALUES (2, 'S10000003', 'In', 'I10000003', 'InStorage', 'ProductIn', 12, 31, 'W10000003', 'S10000006', 40, 0, 0, '2025-09-04 14:19:48', 'admin', '管理员', 1, 2025, 202509, 20250904, 20250904141948, '1900-01-01 00:00:00', 0, 0, '元', NULL, NULL);
INSERT INTO `invstorageflowdetail` VALUES (3, 'S10000004', 'In', 'I10000004', 'InStorage', 'PurchaseIn', 12, 28, 'W10000001', 'S10000004', 38, 0, 0, '2025-09-04 14:21:07', 'admin', '管理员', 1, 2025, 202509, 20250904, 20250904142107, '1900-01-01 00:00:00', 0, 0, '元', NULL, NULL);
INSERT INTO `invstorageflowdetail` VALUES (4, 'S10000003', 'Out', 'O10000001', 'OutStorage', 'SalesOut', -1, 31, 'W10000003', 'S10000006', 40, 0, 0, '2025-09-10 14:43:10', 'admin', '管理员', 1, 2025, 202509, 20250910, 20250910144310, '1900-01-01 00:00:00', 0, 0, '元', NULL, NULL);
INSERT INTO `invstorageflowdetail` VALUES (5, 'S10000005', 'In', 'I10000005', 'InStorage', 'ProductIn', 1, 8, 'W10000003', NULL, 40, 0, 0, '2025-09-11 14:11:32', 'admin', '管理员', 1, 2025, 202509, 20250911, 20250911141132, '1900-01-01 00:00:00', 0, 0, NULL, NULL, NULL);
INSERT INTO `invstorageflowdetail` VALUES (6, 'S10000006', 'In', 'I10000006', 'InStorage', 'ProductIn', 1, 8, 'W10000001', NULL, 38, 0, 0, '2025-09-11 14:12:43', 'admin', '管理员', 1, 2025, 202509, 20250911, 20250911141243, '1900-01-01 00:00:00', 0, 0, NULL, NULL, NULL);
INSERT INTO `invstorageflowdetail` VALUES (7, 'S10000006', 'In', 'I10000008', 'InStorage', 'ProductIn', 1, 8, 'W10000001', 'S10000004', 38, 0, 0, '2025-09-17 15:34:34', 'admin', '管理员', 1, 2025, 202509, 20250917, 20250917153434, '1900-01-01 00:00:00', 0, 0, NULL, NULL, NULL);
INSERT INTO `invstorageflowdetail` VALUES (8, 'S10000006', 'In', 'I10000009', 'InStorage', 'ProductIn', 12, 8, 'W10000001', 'S10000004', 38, 0, 0, '2025-09-17 15:43:01', 'admin', '管理员', 1, 2025, 202509, 20250917, 20250917154301, '1900-01-01 00:00:00', 0, 0, NULL, NULL, NULL);
INSERT INTO `invstorageflowdetail` VALUES (9, 'S10000006', 'In', 'I10000010', 'InStorage', 'ProductIn', 18, 8, 'W10000001', 'S10000004', 38, 0, 0, '2025-09-17 16:54:25', 'admin', '管理员', 1, 2025, 202509, 20250917, 20250917165425, '1900-01-01 00:00:00', 0, 0, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for invstoragewarehousedetail
-- ----------------------------
DROP TABLE IF EXISTS `invstoragewarehousedetail`;
CREATE TABLE `invstoragewarehousedetail`  (
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '商品ID',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '仓库ID',
  `BinId` int(11) NOT NULL COMMENT '货位ID',
  `WorkbinCellId` int(11) NOT NULL COMMENT '料箱单元格ID',
  `UnitId` int(11) NOT NULL COMMENT '库存单位Id',
  `ShelfId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货架ID',
  `WorkbinId` int(11) NULL DEFAULT NULL COMMENT '料箱ID',
  `Stock` float NOT NULL COMMENT '库存量',
  PRIMARY KEY (`GoodsId`, `WarehouseId`, `BinId`, `WorkbinCellId`, `UnitId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '库存明细表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invstoragewarehousedetail
-- ----------------------------
INSERT INTO `invstoragewarehousedetail` VALUES ('S10000001', 'W10000001', 25, 0, 27, 'S10000002', 0, 12);
INSERT INTO `invstoragewarehousedetail` VALUES ('S10000003', 'W10000003', 40, 0, 31, 'S10000006', 0, 11);
INSERT INTO `invstoragewarehousedetail` VALUES ('S10000004', 'W10000001', 38, 0, 28, 'S10000004', 0, 12);
INSERT INTO `invstoragewarehousedetail` VALUES ('S10000005', 'W10000003', 40, 0, 8, NULL, 0, 1);
INSERT INTO `invstoragewarehousedetail` VALUES ('S10000006', 'W10000001', 38, 0, 8, NULL, 0, 32);

-- ----------------------------
-- Table structure for invwarehouse
-- ----------------------------
DROP TABLE IF EXISTS `invwarehouse`;
CREATE TABLE `invwarehouse`  (
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '仓库ID',
  `WarehouseNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '仓库编码',
  `WarehouseName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '仓库名称',
  `WarehouseType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '仓库类型',
  `ChargePerson` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '负责人',
  `ChargePersonPhone` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '负责人电话',
  `Province` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所在省份',
  `City` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所在城市',
  `Address` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所在地址',
  `IsAbandon` tinyint(1) NOT NULL COMMENT '是否已弃用',
  `InventoryDateOfLast` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次盘点日期',
  `InventoryOperatorIdOfLast` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次盘点人员ID',
  `InventoryOperatorNameOfLast` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次盘点人员姓名',
  `Remark` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `SpareField1` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `SpareField2` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `SpareField3` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `SpareField4` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  `SpareField5` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '预留字段',
  PRIMARY KEY (`WarehouseId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '仓库信息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invwarehouse
-- ----------------------------
INSERT INTO `invwarehouse` VALUES ('W10000001', 'W01', '原材料仓库', 'RawMaterial', NULL, '', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `invwarehouse` VALUES ('W10000003', 'W03', '成品仓', 'FinishedProduct', NULL, '', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for invworkbin
-- ----------------------------
DROP TABLE IF EXISTS `invworkbin`;
CREATE TABLE `invworkbin`  (
  `WorkbinId` int(11) NOT NULL AUTO_INCREMENT COMMENT '料箱ID',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '仓库ID',
  `ShelfId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货架ID',
  `BinId` int(11) NOT NULL COMMENT '库位',
  `WorkbinNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '料箱编号',
  `SpecId` int(11) NOT NULL COMMENT '料箱规格',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单元格状态',
  `Remark` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`WorkbinId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '货架料箱信息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invworkbin
-- ----------------------------

-- ----------------------------
-- Table structure for invworkbincell
-- ----------------------------
DROP TABLE IF EXISTS `invworkbincell`;
CREATE TABLE `invworkbincell`  (
  `CellId` int(11) NOT NULL AUTO_INCREMENT COMMENT '单元格ID',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '仓库ID',
  `ShelfId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货架ID',
  `BinId` int(11) NOT NULL COMMENT '库位',
  `WorkbinId` int(11) NOT NULL COMMENT '胶箱',
  `CellNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单元格编号',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单元格状态',
  PRIMARY KEY (`CellId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '料箱单元格信息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invworkbincell
-- ----------------------------

-- ----------------------------
-- Table structure for invworkbinspecification
-- ----------------------------
DROP TABLE IF EXISTS `invworkbinspecification`;
CREATE TABLE `invworkbinspecification`  (
  `SpecId` int(11) NOT NULL AUTO_INCREMENT,
  `SpecName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Size` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `LoadWeight` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CellCount` int(11) NOT NULL DEFAULT 1,
  `Rank` int(11) NOT NULL,
  PRIMARY KEY (`SpecId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '料箱规格信息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of invworkbinspecification
-- ----------------------------
INSERT INTO `invworkbinspecification` VALUES (1, 'A', '200*100*30', '50', 1, 1);
INSERT INTO `invworkbinspecification` VALUES (2, 'B', '200*100*30', '50', 2, 2);

-- ----------------------------
-- Table structure for planfinishedproductorder
-- ----------------------------
DROP TABLE IF EXISTS `planfinishedproductorder`;
CREATE TABLE `planfinishedproductorder`  (
  `OrderId` int(11) NOT NULL AUTO_INCREMENT,
  `PlanId` int(11) NOT NULL COMMENT '计划订单ID',
  `PlanName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '计划订单名称',
  `Year` int(11) NOT NULL COMMENT '年',
  `Week` int(11) NOT NULL COMMENT '周',
  `Version` float NOT NULL COMMENT '发布计划版本',
  `ConsignNum` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '发货型号',
  `ProdctionTypeNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '产品型号',
  `QuantityTotal` float NOT NULL COMMENT '生产数量',
  `UnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单位',
  `DateOfMon` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '周一日期',
  `QuantityDayOfMon` float NOT NULL COMMENT '周一白班生产数量',
  `QuantityNightOfMon` float NOT NULL COMMENT '周一晚班生产数量',
  `DateOfTues` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '周二日期',
  `QuantityDayOfTues` float NOT NULL COMMENT '周二白班生产数量',
  `QuantityNightOfTues` float NOT NULL COMMENT '周二晚班生产数量',
  `DateOfWed` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '周三日期',
  `QuantityDayOfWed` float NOT NULL COMMENT '周三白班生产数量',
  `QuantityNightOfWed` float NOT NULL COMMENT '周三晚班生产数量',
  `DateOfThur` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '周四日期',
  `QuantityDayOfThur` float NOT NULL COMMENT '周四白班生产数量',
  `QuantityNightOfThur` float NOT NULL COMMENT '周四晚班生产数量',
  `DateOfFri` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '周五日期',
  `QuantityDayOfFri` float NOT NULL COMMENT '周五白班生产数量',
  `QuantityNightOfFri` float NOT NULL COMMENT '周五晚班生产数量',
  `DateOfSat` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '周六日期',
  `QuantityDayOfSat` float NOT NULL COMMENT '周六白班生产数量',
  `QuantityNightOfSat` float NOT NULL COMMENT '周六晚班生产数量',
  `DateOfSun` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '周日日期',
  `QuantityDayOfSun` float NOT NULL COMMENT '周日白班生产数量',
  `QuantityNightOfSun` float NOT NULL COMMENT '周日晚班生产数量',
  `Remark` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `CreateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人ID',
  `CreateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`OrderId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '成品生产计划' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of planfinishedproductorder
-- ----------------------------

-- ----------------------------
-- Table structure for planmaterialrequirementdetail
-- ----------------------------
DROP TABLE IF EXISTS `planmaterialrequirementdetail`;
CREATE TABLE `planmaterialrequirementdetail`  (
  `DetailId` int(11) NOT NULL AUTO_INCREMENT,
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单号',
  `GoodsClassifyGroup` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物料分类',
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物料ID',
  `GoodsNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物料编码',
  `GoodsName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物料名称',
  `Quantity` float NOT NULL COMMENT '需求数量',
  `UnitName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单位',
  `RequirementLevel` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '需求等级',
  `Shift` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '需求班次',
  `ReqDate` datetime NOT NULL COMMENT '需求日期',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '状态',
  PRIMARY KEY (`DetailId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '物料需求计划-明细' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of planmaterialrequirementdetail
-- ----------------------------

-- ----------------------------
-- Table structure for planmaterialrequirementorder
-- ----------------------------
DROP TABLE IF EXISTS `planmaterialrequirementorder`;
CREATE TABLE `planmaterialrequirementorder`  (
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单号',
  `PlanId` int(11) NOT NULL COMMENT '计划订单ID',
  `Year` int(11) NOT NULL COMMENT '年',
  `Week` int(11) NOT NULL COMMENT '周',
  `Version` float NOT NULL COMMENT '发布计划版本',
  `GoodsClassifyGroup` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物料分类',
  `SupplierId` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '供应商',
  `IsSendMail` tinyint(1) NOT NULL COMMENT '是否已发送邮件',
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '状态',
  `UpdateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人ID',
  `UpdateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人姓名',
  `UpdateDate` datetime NOT NULL COMMENT '上次修改时间',
  `CreateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人ID',
  `CreateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`OrderNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '物料需求计划' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of planmaterialrequirementorder
-- ----------------------------

-- ----------------------------
-- Table structure for prodarea
-- ----------------------------
DROP TABLE IF EXISTS `prodarea`;
CREATE TABLE `prodarea`  (
  `AreaNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `PlantNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `AreaName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`AreaNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '区域信息' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of prodarea
-- ----------------------------

-- ----------------------------
-- Table structure for prodcachewarehouseflow
-- ----------------------------
DROP TABLE IF EXISTS `prodcachewarehouseflow`;
CREATE TABLE `prodcachewarehouseflow`  (
  `FlowId` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `FlowType` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '类型',
  `ActualTotal` double NOT NULL COMMENT '数量',
  `SourceOrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '来源订单号',
  `SourceOrderType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '来源订单类型',
  `WarehouseId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '仓库ID',
  `ShelfId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货架ID',
  `BinId` int(11) NULL DEFAULT NULL COMMENT '库位ID',
  `Product` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '产品',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  `CreateUser` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人',
  `IsStatistical` tinyint(1) NOT NULL COMMENT '是否已流转In->Out',
  PRIMARY KEY (`FlowId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '成品缓存仓进出流水表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of prodcachewarehouseflow
-- ----------------------------

-- ----------------------------
-- Table structure for prodline
-- ----------------------------
DROP TABLE IF EXISTS `prodline`;
CREATE TABLE `prodline`  (
  `LineNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `PlantNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `AreaNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `LineName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`LineNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '产线信息' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of prodline
-- ----------------------------

-- ----------------------------
-- Table structure for prodmatchingcar
-- ----------------------------
DROP TABLE IF EXISTS `prodmatchingcar`;
CREATE TABLE `prodmatchingcar`  (
  `MatchingId` int(11) NOT NULL AUTO_INCREMENT,
  `DeliverNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '生产订单号',
  `ConsignNum` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '发货单号',
  `Prodct` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '产品',
  `CountByCar` int(11) NOT NULL COMMENT '需求车次',
  `MatchingCount` int(11) NOT NULL COMMENT '小车配对数',
  `Total` double NOT NULL COMMENT '生产订单数量',
  `CarSoleCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '小车唯一码',
  `MatchingNum` int(11) NOT NULL COMMENT '小车配对序列号',
  `MatchingCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '小车配对码',
  `PlanTotalByCar` double NOT NULL COMMENT '计划装车数量',
  `ActualTotalByCar` double NOT NULL COMMENT '实际装车数量',
  `BinId` int(11) NOT NULL COMMENT '缓存仓库位ID',
  `BinNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '缓存仓库位编号',
  `IsMatch` tinyint(1) NOT NULL COMMENT '是否完成配对',
  `IsPackage` tinyint(1) NOT NULL COMMENT '是否已包装完成',
  `MatchDate` datetime NULL DEFAULT NULL COMMENT '配对时间',
  `PackageDate` datetime NULL DEFAULT NULL COMMENT '包装完成时间',
  PRIMARY KEY (`MatchingId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '小车配对信息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of prodmatchingcar
-- ----------------------------

-- ----------------------------
-- Table structure for prodorderdetails
-- ----------------------------
DROP TABLE IF EXISTS `prodorderdetails`;
CREATE TABLE `prodorderdetails`  (
  `DetailId` int(11) NOT NULL AUTO_INCREMENT COMMENT '明细ID',
  `DeliverNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '生产订单号',
  `DetailNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '交接单号',
  `CarSoleCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '小车唯一码',
  `CarRank` int(11) NULL DEFAULT NULL COMMENT '配送车次',
  `PlanTotalByCar` double NOT NULL COMMENT '计划装车数量',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '状态',
  PRIMARY KEY (`DetailId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '生产订单明细' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of prodorderdetails
-- ----------------------------

-- ----------------------------
-- Table structure for prodorders
-- ----------------------------
DROP TABLE IF EXISTS `prodorders`;
CREATE TABLE `prodorders`  (
  `OrderId` int(11) NOT NULL AUTO_INCREMENT,
  `DeliverNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '生产订单号',
  `ConsignNum` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '发货型号',
  `ProdctionTypeNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '产品型号',
  `ProductName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '产品名称',
  `MatchingCount` int(11) NOT NULL COMMENT '小车配对数',
  `Total` double NULL DEFAULT NULL COMMENT '生产订单数量',
  `TotalPutout` double NULL DEFAULT NULL COMMENT '已上架数量',
  `TotalByCar` double NOT NULL COMMENT '装车数量',
  `CountByCar` int(11) NOT NULL COMMENT '需求车次',
  `UnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单位',
  `Line` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '产线',
  `CreateDate` datetime NULL DEFAULT NULL COMMENT '创建日期',
  `ModifyDate` datetime NULL DEFAULT NULL COMMENT '修改日期',
  `CreateUser` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人',
  `ModifyUser` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '修改人',
  `PrintDate` datetime NULL DEFAULT NULL COMMENT '打印日期',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '状态',
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`OrderId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '生产订单' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of prodorders
-- ----------------------------

-- ----------------------------
-- Table structure for prodorderstatusflow
-- ----------------------------
DROP TABLE IF EXISTS `prodorderstatusflow`;
CREATE TABLE `prodorderstatusflow`  (
  `FlowId` int(11) NOT NULL AUTO_INCREMENT,
  `CarSoleCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '小车唯一码',
  `DeliverNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '生产订单号',
  `ConsignNum` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '发货单号',
  `DetailNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '交接单号',
  `ProdctionTypeNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '产品型号',
  `Line` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '产线',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '状态',
  `CreateDate` datetime NULL DEFAULT NULL COMMENT '创建日期',
  `CreateUser` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人',
  PRIMARY KEY (`FlowId`) USING BTREE,
  INDEX `ProdOrderStatusFlow_Index`(`CarSoleCode`, `Status`, `CreateDate`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '生产订单状态变化流水记录表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of prodorderstatusflow
-- ----------------------------

-- ----------------------------
-- Table structure for prodplant
-- ----------------------------
DROP TABLE IF EXISTS `prodplant`;
CREATE TABLE `prodplant`  (
  `PlantNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `PlantName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`PlantNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '工厂信息' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of prodplant
-- ----------------------------

-- ----------------------------
-- Table structure for prodstation
-- ----------------------------
DROP TABLE IF EXISTS `prodstation`;
CREATE TABLE `prodstation`  (
  `StationNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `PlantNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `AreaNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `LineNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `StationName` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`StationNo`, `LineNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '工位信息' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of prodstation
-- ----------------------------

-- ----------------------------
-- Table structure for produnstackbinqueue
-- ----------------------------
DROP TABLE IF EXISTS `produnstackbinqueue`;
CREATE TABLE `produnstackbinqueue`  (
  `CarSoleCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '小车唯一码',
  `MatchingCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '小车配对码',
  `UnstackBinId` int(11) NOT NULL COMMENT '拆垛机库位ID',
  `UnstackBinNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '拆垛机库位编号',
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '订单号',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '状态',
  `CreateDate` datetime NOT NULL,
  PRIMARY KEY (`CarSoleCode`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '拆垛机库位队列信息' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of produnstackbinqueue
-- ----------------------------

-- ----------------------------
-- Table structure for purchaseorder
-- ----------------------------
DROP TABLE IF EXISTS `purchaseorder`;
CREATE TABLE `purchaseorder`  (
  `DetialId` int(11) NOT NULL AUTO_INCREMENT COMMENT '明细ID',
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '采购单号',
  `PurchaseTypeId` int(11) NOT NULL COMMENT '采购方式',
  `IsApplyMaterialNumber` tinyint(1) NOT NULL COMMENT '是否申请料号',
  `IsPurchaseBuy` tinyint(1) NOT NULL COMMENT '是否采买',
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品ID',
  `GoodsNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品编码(SAP编码)',
  `GoodsClassifyGroup` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品分类',
  `NPMBuyer` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'NPM采购',
  `NPMBuyerEMail` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'NPM采购-邮箱',
  `GoodsNameZH` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品名称(中文)',
  `GoodsNameEN` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品名称(英文)',
  `ExternalOrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '订货号',
  `GoodsModel` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品型号',
  `FlowStatus` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '流程状态',
  `Status` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '数据状态',
  `MNANo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'MNA编号',
  `PONumber` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'PO号',
  `PODate` datetime NOT NULL COMMENT 'PO日期',
  `PRNumber` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'PR号',
  `PRDate` datetime NOT NULL COMMENT 'PR日期',
  `QuantityActual` float NOT NULL COMMENT '下单实际数量',
  `Price` float NOT NULL COMMENT '单价',
  `DetailTotalPrice` float NOT NULL COMMENT '总价',
  `PriceUnitName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '计价单位',
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'PO描述',
  `ArrivalStatus` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '实物到货状态',
  `ArrivalDate` datetime NOT NULL COMMENT '实物到货日期',
  `QuantityArrival` float NOT NULL COMMENT '实际到货数量',
  `ArrivalAbnormalReason` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '实物到货异常原因',
  `ReceivingStatus` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'WK收货状态',
  `ReceivingDate` datetime NOT NULL COMMENT 'WK收货日期',
  `ReceivingAbnormalReason` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'WK收货异常原因',
  `SupplierId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '供应商ID',
  `SupplierNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '供应商代码',
  `SupplierName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '供应商名称',
  `Manufactor` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '生产厂家',
  `Quantity` float NOT NULL COMMENT '下单数量',
  `QuantityUnitId` int(11) NOT NULL COMMENT '数量单位ID',
  `QuantityUnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '数量单位',
  `QuoteLink` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '报价链接',
  `PMType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'PM类型',
  `ApplyReason` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '申请原因',
  `CPMG` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'CPMG',
  `AccountNumber` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '科目号',
  `Consignee` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货人(需求人)',
  `ConsigneeEmail` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货人邮箱',
  `Line` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '适用产线',
  `ClassesNumber` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '类别编号',
  `ClassesNumberDesc` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '类别描述',
  `ClassesKey` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '类别关键词',
  `GoodsClassifyId` int(11) NOT NULL COMMENT '物品类型ID',
  `GoodsClassifyName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品类型',
  `SafetyInventory` float NOT NULL COMMENT '安全库存',
  `MinLotSize` float NOT NULL COMMENT '最小订货量',
  `ReturnCycle` float NOT NULL COMMENT '回货周期',
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `ConsigneeOperator` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货操作人',
  `IsEmailNotification` tinyint(1) NOT NULL COMMENT '邮件通知状态',
  `ApprovalDate` datetime NOT NULL COMMENT '审批时间',
  `ApproverId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人ID',
  `ApproverName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人姓名',
  `ApproverRole` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人角色',
  `ApprovalStatus` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批状态',
  `UpdateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人ID',
  `UpdateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人姓名',
  `UpdateDate` datetime NOT NULL COMMENT '上次修改时间',
  `CreateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人ID',
  `CreateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  `IsOnceBuy` tinyint(1) NULL DEFAULT NULL COMMENT '是否一次性采买',
  PRIMARY KEY (`DetialId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of purchaseorder
-- ----------------------------

-- ----------------------------
-- Table structure for receivingorder
-- ----------------------------
DROP TABLE IF EXISTS `receivingorder`;
CREATE TABLE `receivingorder`  (
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '收货单号',
  `TotalPrice` float NOT NULL COMMENT '收货总价',
  `PriceUnitId` int(11) NOT NULL COMMENT '计价单位ID',
  `PriceUnitName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '计价单位名称',
  `ExpectDate` datetime NOT NULL COMMENT '预期到货日期',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单据状态',
  `IsAccountPaid` tinyint(1) NOT NULL COMMENT '是否已付款',
  `PaymentAmount` float NOT NULL COMMENT '已付款金额',
  `YeareAndMonth` int(11) NOT NULL COMMENT '收货年月',
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `ReceivingResponsableUserId` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货责任人ID',
  `ReceivingResponsableUserName` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货人责任人名称',
  `ReceivingResponsableUserEmail` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货责任人邮箱',
  `IsAutoEmailToReceiving` tinyint(1) NOT NULL COMMENT '是否自动邮件通知收货责任人',
  `ReceivingAddress` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货地址',
  `IsEmailNotification` tinyint(1) NOT NULL COMMENT '邮件通知状态',
  `GoodsClassify` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品分类',
  `ApprovalDate` datetime NOT NULL COMMENT '审批时间',
  `ApproverId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人ID',
  `ApproverName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人姓名',
  `ApproverRole` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人角色',
  `ApprovalStatus` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批状态',
  `UpdateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人ID',
  `UpdateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人姓名',
  `UpdateDate` datetime NOT NULL COMMENT '上次修改时间',
  `CreateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人ID',
  `CreateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`OrderNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '收货计划主表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of receivingorder
-- ----------------------------

-- ----------------------------
-- Table structure for receivingorderdetail
-- ----------------------------
DROP TABLE IF EXISTS `receivingorderdetail`;
CREATE TABLE `receivingorderdetail`  (
  `DetialId` int(11) NOT NULL AUTO_INCREMENT COMMENT '明细ID',
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '收货单号',
  `ReceivingAbnormalType` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '异常到货类别',
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品ID',
  `GoodsNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品编码',
  `GoodsClassifyName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品类型',
  `GoodsModel` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品型号',
  `GoodsName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品名称',
  `SupplierId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '供应商ID',
  `SupplierName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '供应商名称',
  `IsASN` tinyint(1) NOT NULL COMMENT '是否ASN收货',
  `ASNCheckStatus` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'ASN Check状态',
  `ReceivingLevel` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '重要级别',
  `IsMakeInvoice` tinyint(1) NOT NULL COMMENT '是否开票',
  `InvoiceNumber` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '发票编号',
  `ExternalOrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '外部订单号',
  `WaybillNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '运单号',
  `Quantity` float NOT NULL COMMENT '收货数量',
  `QuantityUrgency` float NOT NULL COMMENT '紧急收货数量',
  `QuantityUnitId` int(11) NOT NULL COMMENT '数量单位ID',
  `QuantityUnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '数量单位',
  `Price` float NOT NULL COMMENT '收货单价',
  `WorkpieceTray` int(11) NOT NULL COMMENT '料盘数',
  `Pallet` int(11) NOT NULL COMMENT '托盘数',
  `DetailTotalPrice` float NOT NULL COMMENT '收货总价',
  `DetailStatus` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '状态',
  `ReceivingDate` datetime NOT NULL COMMENT '收货日期',
  `ReceivingOperatorId` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货人ID',
  `ReceivingOperatorName` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收货人名称',
  `DownTime` datetime NULL DEFAULT NULL COMMENT '预计停线时间',
  `QuantityActual` float NULL DEFAULT NULL COMMENT '实际收货数量',
  `ReceivingAbnormalDesc` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '异常收货描述',
  `AbnormalDeliveryPallet` int(11) NULL DEFAULT NULL COMMENT '异常到货托数',
  PRIMARY KEY (`DetialId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '收货计划明细表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of receivingorderdetail
-- ----------------------------

-- ----------------------------
-- Table structure for sendingorder
-- ----------------------------
DROP TABLE IF EXISTS `sendingorder`;
CREATE TABLE `sendingorder`  (
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '发货单号',
  `SendingDate` datetime NULL DEFAULT NULL COMMENT '计划发货日期',
  `RequestDate` datetime NULL DEFAULT NULL COMMENT '要求到货日期',
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单据状态',
  `IsUrgentShipment` varchar(1) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '是否紧急发货',
  `IsSufficientStock` varchar(1) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '是否有足够库存',
  `YearAndMonth` int(11) NOT NULL COMMENT '发货年月',
  `SpecialRequest` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '特殊要求',
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `SendingResponsableUserId` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '发货责任人ID-废弃',
  `SendingResponsableUserName` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '发货人责任人名称-废弃',
  `SendingResponsableUserEmail` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '发货责任人邮箱-废弃',
  `ReceivingResponsableUserInfo` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收件人信息',
  `SendingAddress` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '到货地址',
  `GoodsClassify` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物料分类',
  `UpdateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人ID',
  `UpdateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次修改人姓名',
  `UpdateDate` datetime NOT NULL COMMENT '上次修改时间',
  `CreateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人ID',
  `CreateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  `SupplierId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '运输供应商ID',
  `IsEmailNotification` tinyint(1) NULL DEFAULT NULL COMMENT '邮件通知状态(0:未发送  1:已发送)',
  PRIMARY KEY (`OrderNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '发货计划主表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sendingorder
-- ----------------------------
INSERT INTO `sendingorder` VALUES ('S10000001', '2025-09-16 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, 'sfdfda', 'sfsfdaf', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-16 16:48:25', 'S10000001', 0);
INSERT INTO `sendingorder` VALUES ('S10000002', '2025-09-17 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, '1212', '1212', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 10:54:10', 'S10000001', 0);
INSERT INTO `sendingorder` VALUES ('S10000003', '2025-09-17 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, '111', '222', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 10:55:34', 'S10000001', 0);
INSERT INTO `sendingorder` VALUES ('S10000004', '2025-09-17 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, 'sdf', 'fsa', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 11:37:45', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000005', '2025-09-18 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, 'fssdfd', '121221', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 11:38:44', 'S10000001', 0);
INSERT INTO `sendingorder` VALUES ('S10000006', '2025-09-18 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, 'fdsaf', 'sfdafsd', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 11:39:31', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000007', '2025-09-18 00:00:00', '2025-09-19 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, 'fsad', 'fsafdsfd', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 11:39:45', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000008', '2025-09-17 00:00:00', '2025-09-18 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, 'sfdd', 'sfafd', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 12:52:57', 'S10000001', 0);
INSERT INTO `sendingorder` VALUES ('S10000009', '2025-09-17 00:00:00', '2025-09-18 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, 'sfdsaf', NULL, NULL, NULL, 'sfdsafd', 'sfa', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 13:00:09', 'S10000001', 0);
INSERT INTO `sendingorder` VALUES ('S10000010', '2025-09-18 00:00:00', '2025-09-19 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, 'dsf', 'sfsda', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 13:19:46', 'S10000001', 0);
INSERT INTO `sendingorder` VALUES ('S10000011', '2025-09-17 00:00:00', '2025-09-19 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, 'qq', '1111', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 13:41:14', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000012', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 13:42:32', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000013', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 13:49:51', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000014', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 14:03:06', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000015', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 14:06:08', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000016', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, '111111111111111111111111111', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 14:06:19', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000017', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 14:06:59', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000018', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, '2222222222222', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 14:10:25', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000019', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, '3333333333333', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 14:12:23', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000020', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, '6666666666', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 14:17:04', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000021', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 14:25:55', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000022', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 14:39:56', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000023', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 14:55:46', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000024', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 15:02:15', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000025', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, '3333333333345555', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 15:03:30', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000026', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 15:07:37', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000027', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 15:08:09', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000028', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, '0000000000000', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 15:08:34', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000029', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, 'wwwwwwwww', 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 15:09:21', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000030', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 15:09:58', NULL, 0);
INSERT INTO `sendingorder` VALUES ('S10000031', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 'WaitingShipment', 'N', 'N', 202509, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'FinishedProduct', NULL, NULL, '1900-01-01 00:00:00', 'admin', '管理员', '2025-09-17 15:14:16', NULL, 0);

-- ----------------------------
-- Table structure for sendingorderdetail
-- ----------------------------
DROP TABLE IF EXISTS `sendingorderdetail`;
CREATE TABLE `sendingorderdetail`  (
  `DetialId` int(11) NOT NULL AUTO_INCREMENT COMMENT '明细ID',
  `OrderNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '发货单号',
  `GoodsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物品ID',
  `GoodsNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '物品编码',
  `CustomerGoodsNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '客户料号',
  `CustomerIdentificationCode` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '客户配送中心',
  `Quantity` float NOT NULL COMMENT '计划发货数量',
  `PalletsQuantity` float NULL DEFAULT NULL COMMENT '发货托数',
  `QuantityUnitId` int(11) NULL DEFAULT NULL COMMENT '数量单位ID',
  `QuantityUnitName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '数量单位',
  `DetailStatus` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '状态',
  PRIMARY KEY (`DetialId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 32 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '发货计划明细表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sendingorderdetail
-- ----------------------------
INSERT INTO `sendingorderdetail` VALUES (1, 'S10000001', 'S10000005', 'SYK200M', NULL, NULL, 12, 12, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (2, 'S10000002', 'S10000005', 'SYK200M', NULL, NULL, 12, 12, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (3, 'S10000003', 'S10000005', 'SYK200M', NULL, NULL, 122, 122, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (4, 'S10000004', 'S10000005', 'SYK200M', NULL, NULL, 12, 12, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (5, 'S10000005', 'S10000005', 'SYK200M', NULL, NULL, 12, 12, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (6, 'S10000006', 'S10000005', 'SYK200M', NULL, NULL, 12122, 12122, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (7, 'S10000007', 'S10000005', 'SYK200M', NULL, NULL, 122, 122, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (8, 'S10000008', 'S10000005', 'SYK200M', NULL, NULL, 1, 1, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (9, 'S10000009', 'S10000005', 'SYK200M', NULL, NULL, 11, 11, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (10, 'S10000010', 'S10000005', 'SYK200M', NULL, NULL, 11, 11, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (11, 'S10000011', 'S10000005', 'SYK200M', NULL, NULL, 111, 111, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (12, 'S10000012', 'S10000005', 'SYK200M', NULL, NULL, 11, 11, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (13, 'S10000013', 'S10000005', 'SYK200M', NULL, NULL, 11, 11, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (14, 'S10000014', 'S10000005', 'SYK200M', NULL, NULL, 1, 1, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (15, 'S10000015', 'S10000005', 'SYK200M', NULL, NULL, 0, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (16, 'S10000016', 'S10000005', 'SYK200M', NULL, NULL, 0, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (17, 'S10000017', 'S10000005', 'SYK200M', NULL, NULL, 111, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (18, 'S10000018', 'S10000005', 'SYK200M', NULL, NULL, 0, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (19, 'S10000019', 'S10000005', 'SYK200M', NULL, NULL, 333333, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (20, 'S10000020', 'S10000005', 'SYK200M', NULL, NULL, 5555, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (21, 'S10000021', 'S10000005', 'SYK200M', NULL, NULL, 5555, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (22, 'S10000022', 'S10000005', 'SYK200M', NULL, NULL, 5555, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (23, 'S10000023', 'S10000005', 'SYK200M', NULL, NULL, 5555, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (24, 'S10000024', 'S10000005', 'SYK200M', NULL, NULL, 5555, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (25, 'S10000025', 'S10000005', 'SYK200M', NULL, NULL, 5555, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (26, 'S10000026', 'S10000005', 'SYK200M', NULL, NULL, 0, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (27, 'S10000027', 'S10000005', 'SYK200M', NULL, NULL, 0, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (28, 'S10000028', 'S10000005', 'SYK200M', NULL, NULL, 0, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (29, 'S10000029', 'S10000005', 'SYK200M', NULL, NULL, 44, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (30, 'S10000030', 'S10000005', 'SYK200M', NULL, NULL, 43, 0, 0, NULL, 'WaitingShipment');
INSERT INTO `sendingorderdetail` VALUES (31, 'S10000031', 'S10000005', 'SYK200M', NULL, NULL, 22, 0, 0, NULL, 'WaitingShipment');

-- ----------------------------
-- Table structure for sysarea
-- ----------------------------
DROP TABLE IF EXISTS `sysarea`;
CREATE TABLE `sysarea`  (
  `AreaId` int(11) NOT NULL COMMENT '区县ID',
  `CityId` int(11) NOT NULL COMMENT '城市ID',
  `AreaCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '区县编号',
  `AreaName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '区县名称',
  `Remark` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `FirstSpell` varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '拼音首字母',
  `Rank` int(11) NOT NULL COMMENT '界面展示排序',
  PRIMARY KEY (`AreaId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '区县表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysarea
-- ----------------------------

-- ----------------------------
-- Table structure for sysargs
-- ----------------------------
DROP TABLE IF EXISTS `sysargs`;
CREATE TABLE `sysargs`  (
  `ArgsId` int(11) NOT NULL AUTO_INCREMENT COMMENT '参数ID',
  `ArgsKey` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '参数编码',
  `ArgsKeyName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '参数编码名称',
  `ArgsKeyNameEn` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '参数编码英文名',
  `ArgsValue` varchar(5000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '参数值',
  `ArgsGroup` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '参数分类',
  `ArgsSubGroup` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '参数子类',
  `Remark` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `ArgsType` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '参数类型',
  `Rank` int(11) NOT NULL COMMENT '界面展示排序',
  `IsVisible` tinyint(1) NOT NULL COMMENT '是否可见',
  PRIMARY KEY (`ArgsId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 48 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '系统参数表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysargs
-- ----------------------------
INSERT INTO `sysargs` VALUES (1, 'AdminAccount', '开发者', NULL, 'developer', 'Args', 'Sys', '11C9C475F21FE001', 'Text', 1, 0);
INSERT INTO `sysargs` VALUES (2, 'InitialPassword', '用户初始密码', NULL, '123456', 'Args', NULL, NULL, 'Text', 2, 1);
INSERT INTO `sysargs` VALUES (3, 'GoodsPhotoLimit', '物品图片上传上限', NULL, '3', 'Args', NULL, NULL, 'Number', 3, 1);
INSERT INTO `sysargs` VALUES (4, 'IsInStorageApproval', '启用入库审批', NULL, 'false', 'Args', 'Approval', NULL, 'Check', 7, 0);
INSERT INTO `sysargs` VALUES (5, 'IsOutStorageApproval', '启用出库审批', NULL, 'false', 'Args', 'Approval', NULL, 'Check', 8, 0);
INSERT INTO `sysargs` VALUES (6, 'IsReceivingApproval', '启用收货计划审批', NULL, 'true', 'Args', NULL, NULL, 'Check', 9, 1);
INSERT INTO `sysargs` VALUES (7, 'CarCodePrefix', '小车唯一码前缀', NULL, 'CAR', 'Args', 'Production', NULL, 'Text', 9, 0);
INSERT INTO `sysargs` VALUES (8, 'IsFIFO', '启用先进先出', NULL, 'true', 'Args', 'Production', NULL, 'Check', 10, 0);
INSERT INTO `sysargs` VALUES (9, 'ProductBufferWarehouse', '指定成品下线缓存仓类型', NULL, 'ProductBuffer', 'Args', 'Production', 'WarehouseType', 'SingleSelect', 11, 0);
INSERT INTO `sysargs` VALUES (10, 'UnstackWarehouse', '指定拆垛机器仓库类型', NULL, 'Unstack', 'Args', 'Production', 'WarehouseType', 'SingleSelect', 12, 0);
INSERT INTO `sysargs` VALUES (11, 'IsWorkbinNoSameBinNo', '料箱编号等同货位编号', NULL, 'true', 'Args', 'Inventory', NULL, 'Check', 12, 0);
INSERT INTO `sysargs` VALUES (12, 'ReceivingAddress', '物料收发货地址', NULL, '湖南省长沙市长沙县', 'Args', NULL, NULL, 'Text', 13, 1);
INSERT INTO `sysargs` VALUES (13, 'IsAutoSendMailByMaterialRequirement', '自动邮件通知（物料需求计划）', NULL, 'true', 'Args', NULL, NULL, 'Check', 14, 1);
INSERT INTO `sysargs` VALUES (14, 'IsAutoSendMailBySavetyInvenstory', '自动邮件通知（安全库存预警）', NULL, 'true', 'Args', NULL, NULL, 'Check', 15, 1);
INSERT INTO `sysargs` VALUES (15, 'ClientType', '客户类型', NULL, '', 'Dic', NULL, NULL, 'SingleSelect', 1, 1);
INSERT INTO `sysargs` VALUES (16, 'ClientLevel', '客户等级', NULL, '', 'Dic', NULL, NULL, 'SingleSelect', 2, 1);
INSERT INTO `sysargs` VALUES (17, 'SupplierType', '供应商类型', NULL, NULL, 'Dic', NULL, '', 'SingleSelect', 3, 1);
INSERT INTO `sysargs` VALUES (18, 'SupplierProperty', '供应商性质', NULL, '', 'Dic', NULL, NULL, 'SingleSelect', 4, 1);
INSERT INTO `sysargs` VALUES (19, 'CreditType', '收款方式', NULL, '', 'Dic', NULL, NULL, 'SingleSelect', 5, 1);
INSERT INTO `sysargs` VALUES (20, 'ProductLevel', '产品等级', NULL, '', 'Dic', NULL, NULL, 'SingleSelect', 6, 1);
INSERT INTO `sysargs` VALUES (21, 'ProductProperty', '产品属性', NULL, '', 'Dic', NULL, NULL, 'SingleSelect', 7, 1);
INSERT INTO `sysargs` VALUES (22, 'WarehouseType', '仓库类型', NULL, NULL, 'Dic', NULL, '', 'SingleSelect', 9, 1);
INSERT INTO `sysargs` VALUES (23, 'SparePartType', '备件类型', NULL, '', 'Dic', NULL, NULL, 'SingleSelect', 11, 1);
INSERT INTO `sysargs` VALUES (24, 'SamplePieceType', '样件类型', NULL, '', 'Dic', NULL, NULL, 'SingleSelect', 12, 1);
INSERT INTO `sysargs` VALUES (25, 'SeparatorType', '辅材类型', NULL, '', 'Dic', NULL, NULL, 'SingleSelect', 12, 1);
INSERT INTO `sysargs` VALUES (26, 'PackingMaterialType', '包材类型', NULL, '', 'Dic', NULL, NULL, 'SingleSelect', 12, 1);
INSERT INTO `sysargs` VALUES (27, 'RawMaterialType', '原材料类型', NULL, NULL, 'Dic', NULL, '', 'SingleSelect', 12, 1);
INSERT INTO `sysargs` VALUES (28, 'PurchaseChanne', '采购渠道', NULL, '', 'Dic', NULL, NULL, 'SingleSelect', 8, 1);
INSERT INTO `sysargs` VALUES (29, 'PurchaseType', '采购类型', NULL, '', 'Dic', NULL, NULL, 'SingleSelect', 9, 1);
INSERT INTO `sysargs` VALUES (30, 'SparePartRequisitionType', '备件领用类型', NULL, '', 'Dic', 'Requisition', NULL, 'SingleSelect', 9, 1);
INSERT INTO `sysargs` VALUES (31, 'GeneralRequisitionType', '通用领用类型', NULL, '', 'Dic', 'Requisition', NULL, 'SingleSelect', 10, 1);
INSERT INTO `sysargs` VALUES (32, 'ReceivingAbnormalType', '发货异常类型', NULL, '', 'Dic', 'Requisition', NULL, 'SingleSelect', 11, 1);
INSERT INTO `sysargs` VALUES (41, 'IsSafetyInventoryApproval', '启用安全库存审批', NULL, 'false', 'Args', NULL, NULL, 'Check', 9, 1);
INSERT INTO `sysargs` VALUES (42, 'InStorageIsolateDays', '入库隔离（间隔天数）', NULL, '7', 'Args', NULL, NULL, 'Number', 16, 1);
INSERT INTO `sysargs` VALUES (43, 'AbnormalReceiptClassification', '异常收货分类', NULL, NULL, 'Dic', NULL, '', 'SingleSelect', 1, 1);
INSERT INTO `sysargs` VALUES (44, 'SystemVersion', '当前系统版本', NULL, 'V1.0.0 Copyright © 2023-2026 ContiCS', 'Args', NULL, NULL, 'Text', 2, 1);
INSERT INTO `sysargs` VALUES (45, 'SystemName', '系统名称', NULL, 'WMS', 'Args', NULL, NULL, 'Text', 1, 1);
INSERT INTO `sysargs` VALUES (46, 'SystemHomePageLogo', '系统主页Logo', NULL, 'http://8.153.165.125:9000/wmsks/Image/SystemHomePageLogo638936373503485243.png', 'Args', NULL, NULL, 'Upload', 7, 1);
INSERT INTO `sysargs` VALUES (47, 'SystemColor', '系统主题', NULL, 'rgba(0, 113, 188, 1)', 'Args', NULL, NULL, 'ColorPicker', 8, 1);

-- ----------------------------
-- Table structure for sysargsoptions
-- ----------------------------
DROP TABLE IF EXISTS `sysargsoptions`;
CREATE TABLE `sysargsoptions`  (
  `OptionId` int(11) NOT NULL AUTO_INCREMENT COMMENT '参数选项ID',
  `ArgsKey` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '参数ID',
  `OptionKey` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '参数选项编码',
  `OptionName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '参数选项编码名称',
  `OptionNameEn` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '参数选项编码英文名',
  `Remark` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `Rank` int(11) NOT NULL COMMENT '界面展示排序',
  PRIMARY KEY (`OptionId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 146 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '系统参数选项表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysargsoptions
-- ----------------------------
INSERT INTO `sysargsoptions` VALUES (1, 'ClientType', 'Line', '线上客户', NULL, NULL, 1);
INSERT INTO `sysargsoptions` VALUES (2, 'ClientType', 'Offline', '线下客户', NULL, NULL, 2);
INSERT INTO `sysargsoptions` VALUES (3, 'ClientLevel', 'A', 'A', NULL, NULL, 1);
INSERT INTO `sysargsoptions` VALUES (4, 'ClientLevel', 'B', 'B', NULL, NULL, 2);
INSERT INTO `sysargsoptions` VALUES (5, 'ClientLevel', 'C', 'C', NULL, NULL, 3);
INSERT INTO `sysargsoptions` VALUES (6, 'ClientLevel', 'D', 'D', NULL, NULL, 4);
INSERT INTO `sysargsoptions` VALUES (11, 'SupplierProperty', 'Internal', '国内供应商', NULL, NULL, 1);
INSERT INTO `sysargsoptions` VALUES (12, 'SupplierProperty', 'Foreign', '国外供应商', NULL, NULL, 2);
INSERT INTO `sysargsoptions` VALUES (13, 'CreditType', 'Alipay', '支付宝', NULL, NULL, 1);
INSERT INTO `sysargsoptions` VALUES (14, 'CreditType', 'BankAccount', '银行账户', NULL, NULL, 2);
INSERT INTO `sysargsoptions` VALUES (15, 'CreditType', 'CashCheque', '现金支票', NULL, NULL, 3);
INSERT INTO `sysargsoptions` VALUES (16, 'ProductLevel', 'A', 'A级', NULL, NULL, 1);
INSERT INTO `sysargsoptions` VALUES (17, 'ProductLevel', 'B', 'B级', NULL, NULL, 2);
INSERT INTO `sysargsoptions` VALUES (18, 'ProductLevel', 'C', 'C级', NULL, NULL, 3);
INSERT INTO `sysargsoptions` VALUES (19, 'ProductProperty', 'Standard', '标准', NULL, NULL, 1);
INSERT INTO `sysargsoptions` VALUES (20, 'ProductProperty', 'Nonstandard', '非标', NULL, NULL, 2);
INSERT INTO `sysargsoptions` VALUES (27, 'SparePartType', 'General', '一般备件', NULL, NULL, 13);
INSERT INTO `sysargsoptions` VALUES (28, 'SparePartType', 'Specific', '特殊备件', NULL, NULL, 14);
INSERT INTO `sysargsoptions` VALUES (29, 'SamplePieceType', 'General', '一般样件', NULL, NULL, 13);
INSERT INTO `sysargsoptions` VALUES (30, 'SamplePieceType', 'Specific', '特殊样件', NULL, NULL, 14);
INSERT INTO `sysargsoptions` VALUES (31, 'SeparatorType', 'General', '一般辅材', NULL, NULL, 13);
INSERT INTO `sysargsoptions` VALUES (32, 'SeparatorType', 'Specific', '特殊辅材', NULL, NULL, 14);
INSERT INTO `sysargsoptions` VALUES (33, 'PackingMaterialType', 'General', '一般包材', NULL, NULL, 13);
INSERT INTO `sysargsoptions` VALUES (34, 'PackingMaterialType', 'Specific', '特殊包材', NULL, NULL, 14);
INSERT INTO `sysargsoptions` VALUES (35, 'PurchaseChanne', 'Taobao', '淘宝', NULL, NULL, 1);
INSERT INTO `sysargsoptions` VALUES (36, 'PurchaseChanne', 'Jindong', '京东', NULL, NULL, 2);
INSERT INTO `sysargsoptions` VALUES (37, 'PurchaseChanne', 'Dunhuang', '亚马逊', NULL, NULL, 3);
INSERT INTO `sysargsoptions` VALUES (38, 'PurchaseChanne', 'Exhibition', '供应商', NULL, NULL, 4);
INSERT INTO `sysargsoptions` VALUES (39, 'PurchaseChanne', 'Exhibition', '其他', NULL, NULL, 5);
INSERT INTO `sysargsoptions` VALUES (40, 'PurchaseType', 'Platform', '线上采购', NULL, NULL, 1);
INSERT INTO `sysargsoptions` VALUES (41, 'PurchaseType', 'Offline', '线下采购', NULL, NULL, 2);
INSERT INTO `sysargsoptions` VALUES (42, 'SparePartRequisitionType', 'PM01', '紧急维修', NULL, NULL, 1);
INSERT INTO `sysargsoptions` VALUES (43, 'SparePartRequisitionType', 'PM02', '维修保养', NULL, NULL, 2);
INSERT INTO `sysargsoptions` VALUES (44, 'SparePartRequisitionType', 'PM03', '计划维修', NULL, NULL, 3);
INSERT INTO `sysargsoptions` VALUES (45, 'SparePartRequisitionType', 'PM04', '改造', NULL, NULL, 4);
INSERT INTO `sysargsoptions` VALUES (46, 'GeneralRequisitionType', 'ProductionRequisition', '生产领用', NULL, NULL, 1);
INSERT INTO `sysargsoptions` VALUES (47, 'GeneralRequisitionType', 'BorrowRequisition', '借用出库', NULL, NULL, 2);
INSERT INTO `sysargsoptions` VALUES (48, 'ReceivingAbnormalType', 'LabelAbnormal', '标签异常', NULL, NULL, 1);
INSERT INTO `sysargsoptions` VALUES (49, 'ReceivingAbnormalType', 'MaterialAbnormal', '物料异常', NULL, NULL, 2);
INSERT INTO `sysargsoptions` VALUES (50, 'ReceivingAbnormalType', 'PackageAbnormal', '包装异常', NULL, NULL, 3);
INSERT INTO `sysargsoptions` VALUES (51, 'ReceivingAbnormalType', 'DeliveryAbnormal', '送货单异常', NULL, NULL, 4);
INSERT INTO `sysargsoptions` VALUES (52, 'ReceivingAbnormalType', 'FreeMaterial', '免费样件', NULL, NULL, 5);
INSERT INTO `sysargsoptions` VALUES (53, 'ReceivingAbnormalType', 'DataAbnormal', '主数据异常', NULL, NULL, 6);
INSERT INTO `sysargsoptions` VALUES (60, 'RawMaterialType', 'General', '普通原材料', NULL, NULL, 0);
INSERT INTO `sysargsoptions` VALUES (61, 'RawMaterialType', 'Specific', '特殊原材料', NULL, '', 2);
INSERT INTO `sysargsoptions` VALUES (91, 'AbnormalReceiptClassification', '1', '包装破损', NULL, '', 1);
INSERT INTO `sysargsoptions` VALUES (92, 'AbnormalReceiptClassification', '2', '密封包装问题', NULL, '', 2);
INSERT INTO `sysargsoptions` VALUES (93, 'AbnormalReceiptClassification', '3', '包装未遵循包装说明', NULL, '', 3);
INSERT INTO `sysargsoptions` VALUES (94, 'AbnormalReceiptClassification', '4', '包装混料或混托', NULL, '', 4);
INSERT INTO `sysargsoptions` VALUES (95, 'AbnormalReceiptClassification', '5', '无标签', NULL, '', 5);
INSERT INTO `sysargsoptions` VALUES (96, 'AbnormalReceiptClassification', '6', '标签无法识别', NULL, '', 6);
INSERT INTO `sysargsoptions` VALUES (97, 'AbnormalReceiptClassification', '7', '标签未遵循标签规则', NULL, '', 7);
INSERT INTO `sysargsoptions` VALUES (98, 'AbnormalReceiptClassification', '8', '无ASN', NULL, '', 8);
INSERT INTO `sysargsoptions` VALUES (99, 'AbnormalReceiptClassification', '9', 'ASN未遵循规则', NULL, '', 9);
INSERT INTO `sysargsoptions` VALUES (100, 'AbnormalReceiptClassification', '10', '料号错误', NULL, '', 10);
INSERT INTO `sysargsoptions` VALUES (101, 'AbnormalReceiptClassification', '11', '数量错误', NULL, '', 11);
INSERT INTO `sysargsoptions` VALUES (102, 'AbnormalReceiptClassification', '12', '标签位置错误', NULL, '', 12);
INSERT INTO `sysargsoptions` VALUES (103, 'AbnormalReceiptClassification', '13', '无送货单', NULL, '', 13);
INSERT INTO `sysargsoptions` VALUES (104, 'AbnormalReceiptClassification', '14', '送货单不符合邀请', NULL, '', 14);
INSERT INTO `sysargsoptions` VALUES (105, 'AbnormalReceiptClassification', '15', '到货不遵循到货要求', NULL, '', 15);
INSERT INTO `sysargsoptions` VALUES (106, 'AbnormalReceiptClassification', '16', '订单号错误', NULL, '', 16);
INSERT INTO `sysargsoptions` VALUES (107, 'AbnormalReceiptClassification', '17', '送货日期错误', NULL, '', 17);
INSERT INTO `sysargsoptions` VALUES (108, 'AbnormalReceiptClassification', '18', '超过质保期规定', NULL, '', 18);
INSERT INTO `sysargsoptions` VALUES (109, 'AbnormalReceiptClassification', '19', '物料短缺', NULL, '', 19);
INSERT INTO `sysargsoptions` VALUES (116, 'SupplierType', 'SparePart', '备件供应商', NULL, NULL, 1);
INSERT INTO `sysargsoptions` VALUES (117, 'SupplierType', 'SamplePiece', '样件供应商', NULL, NULL, 2);
INSERT INTO `sysargsoptions` VALUES (118, 'SupplierType', 'Separator', '辅材供应商', NULL, NULL, 3);
INSERT INTO `sysargsoptions` VALUES (119, 'SupplierType', 'PackingMaterial', '包材供应商', NULL, NULL, 4);
INSERT INTO `sysargsoptions` VALUES (120, 'SupplierType', 'RawMaterial', '原材料供应商', NULL, '', 5);
INSERT INTO `sysargsoptions` VALUES (121, 'SupplierType', 'Transportation', '运输供应商', NULL, '', 6);
INSERT INTO `sysargsoptions` VALUES (137, 'WarehouseType', 'Non-production Part', '辅材库', NULL, '', 7);
INSERT INTO `sysargsoptions` VALUES (138, 'WarehouseType', 'Material', '原材料仓', NULL, '', 7);
INSERT INTO `sysargsoptions` VALUES (139, 'WarehouseType', 'Mold', '模具仓库', NULL, '', 8);
INSERT INTO `sysargsoptions` VALUES (140, 'WarehouseType', 'ProductBuffer', '成品仓', NULL, NULL, 11);
INSERT INTO `sysargsoptions` VALUES (141, 'WarehouseType', 'SparePart', '备件仓', NULL, NULL, 12);
INSERT INTO `sysargsoptions` VALUES (142, 'WarehouseType', 'SamplePiece', '样件仓', NULL, NULL, 13);
INSERT INTO `sysargsoptions` VALUES (143, 'WarehouseType', 'Unstack', '拆垛仓', NULL, NULL, 14);
INSERT INTO `sysargsoptions` VALUES (144, 'WarehouseType', 'PackingMaterial', '包材仓', NULL, NULL, 14);
INSERT INTO `sysargsoptions` VALUES (145, 'WarehouseType', 'Defective_products', '不良品仓', NULL, '', 9);

-- ----------------------------
-- Table structure for syscity
-- ----------------------------
DROP TABLE IF EXISTS `syscity`;
CREATE TABLE `syscity`  (
  `CityId` int(11) NOT NULL COMMENT '城市ID',
  `ProvinceId` int(11) NOT NULL COMMENT '省份ID',
  `CityCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '城市编号',
  `CityName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '城市名称',
  `Remark` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `FirstSpell` varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '拼音首字母',
  `Rank` int(11) NOT NULL COMMENT '界面展示排序',
  PRIMARY KEY (`CityId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '城市表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of syscity
-- ----------------------------

-- ----------------------------
-- Table structure for syscompany
-- ----------------------------
DROP TABLE IF EXISTS `syscompany`;
CREATE TABLE `syscompany`  (
  `CompanyId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '企业ID',
  `CompanyNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '企业编号',
  `CompanyName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '企业名称',
  `Email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '企业邮箱',
  `Phone` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '企业电话',
  `Url` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '企业网址',
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `Logo` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '企业Logo路径',
  PRIMARY KEY (`CompanyId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '企业信息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of syscompany
-- ----------------------------
INSERT INTO `syscompany` VALUES ('1000000001', 'XiangSheng', '英德市湘昇矿产贸易有限公司', NULL, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for sysdepartments
-- ----------------------------
DROP TABLE IF EXISTS `sysdepartments`;
CREATE TABLE `sysdepartments`  (
  `DeptId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '部门ID',
  `DeptNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '部门编号',
  `DeptName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '部门名称',
  `Rank` int(11) NOT NULL COMMENT '界面展示排序',
  `CompanyId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '企业ID',
  `IsVaild` tinyint(1) NOT NULL COMMENT '是否有效',
  PRIMARY KEY (`DeptId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '部门表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysdepartments
-- ----------------------------

-- ----------------------------
-- Table structure for sysexternalprovider
-- ----------------------------
DROP TABLE IF EXISTS `sysexternalprovider`;
CREATE TABLE `sysexternalprovider`  (
  `ProviderName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProviderSecretKey` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProviderSecret` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProviderHost` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `IsValid` tinyint(1) NOT NULL,
  PRIMARY KEY (`ProviderName`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '第三方身份信息' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysexternalprovider
-- ----------------------------

-- ----------------------------
-- Table structure for sysfieldsmanage
-- ----------------------------
DROP TABLE IF EXISTS `sysfieldsmanage`;
CREATE TABLE `sysfieldsmanage`  (
  `FieldsManageId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '主键ID',
  `TableName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '表名',
  `TableDesc` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '表描述',
  `FieldName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '字段名',
  `FieldDesc` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '字段描述',
  `FieldType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '字段类型',
  `FieldLength` int(11) NULL DEFAULT NULL COMMENT '允许保存长度',
  `FieldArgsKey` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '对应字典选项',
  `IsEnable` tinyint(1) NOT NULL COMMENT '是否启用当前字段',
  `Remark` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '备注',
  `Rank` int(11) NOT NULL COMMENT '排序',
  `IsExport` tinyint(1) NOT NULL COMMENT '是否支持导出',
  PRIMARY KEY (`FieldsManageId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '字段管理表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysfieldsmanage
-- ----------------------------

-- ----------------------------
-- Table structure for sysfieldspermissionsrole
-- ----------------------------
DROP TABLE IF EXISTS `sysfieldspermissionsrole`;
CREATE TABLE `sysfieldspermissionsrole`  (
  `FieldsManageId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '数据权限ID',
  `DeniedRoleId` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '拒绝访问的角色',
  PRIMARY KEY (`FieldsManageId`, `DeniedRoleId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '字段权限表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysfieldspermissionsrole
-- ----------------------------

-- ----------------------------
-- Table structure for syslogs
-- ----------------------------
DROP TABLE IF EXISTS `syslogs`;
CREATE TABLE `syslogs`  (
  `LogId` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '操作人ID',
  `Title` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '标题',
  `Message` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '操作信息',
  `Remark` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL COMMENT '备注',
  `LogType` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '日志类型',
  `DateTime` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '记录时间',
  `ModuleName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '模块名称',
  PRIMARY KEY (`LogId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 122021 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '系统日志表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of syslogs
-- ----------------------------

-- ----------------------------
-- Table structure for sysmail
-- ----------------------------
DROP TABLE IF EXISTS `sysmail`;
CREATE TABLE `sysmail`  (
  `MailId` int(11) NOT NULL AUTO_INCREMENT,
  `PrimaryId` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '主数据ID/附件ID',
  `AttachmentId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '附件ID',
  `BusinessType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '业务类型',
  `Sender` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '发件人',
  `ToReceiver` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '收件人',
  `ToCC` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '抄送人',
  `Subject` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '主题',
  `Body` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL COMMENT '内容',
  `CreateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人ID',
  `CreateUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`MailId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '邮件记录表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysmail
-- ----------------------------

-- ----------------------------
-- Table structure for sysmenus
-- ----------------------------
DROP TABLE IF EXISTS `sysmenus`;
CREATE TABLE `sysmenus`  (
  `MenuId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '菜单ID',
  `ParentId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '父级菜单ID',
  `MenuName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '菜单名称',
  `Rank` int(11) NOT NULL COMMENT '界面展示排序',
  `CtrlName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '控制器名',
  `ActionName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'Action方法名',
  `Icon` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '菜单图标',
  `MenuType` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '菜单类型',
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `MenuNameEn` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '菜单英文名',
  `RoutePath` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '前端路由',
  `ComponentPath` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '前端组件',
  `HideMenu` tinyint(1) NOT NULL COMMENT '是否隐藏菜单',
  `MenuLayout` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '是否为移动端主菜单',
  `MenuDisplay` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '展示端',
  `IsValid` tinyint(1) NOT NULL COMMENT '是否有效',
  `IsVisible` tinyint(1) NOT NULL COMMENT '是否可见',
  `Url` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '外部链接',
  PRIMARY KEY (`MenuId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '系统菜单表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysmenus
-- ----------------------------
INSERT INTO `sysmenus` VALUES ('AGVINFOADD', 'AGVINFOMGR', '添加AGV配置', 2, 'AGVSetting', 'AddAGVInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('AGVINFODEL', 'AGVINFOMGR', '删除AGV配置', 4, 'AGVSetting', 'DelAGVInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('AGVINFOMGR', 'WORKSHOPDEVICE', 'AGV配置', 2, '', '', NULL, 'Menu', NULL, NULL, 'agv-info', 'workshopdevice/agv-info/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('AGVINFOREAD', 'AGVINFOMGR', '查看AGV配置', 1, 'AGVSetting', 'GetAGVInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('AGVINFOUPDATE', 'AGVINFOMGR', '修改AGV配置', 3, 'AGVSetting', 'UpdateAGVInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('APPROVALREAD', 'APPROVALSETUP', '查看审批设置', 1, 'Approval', 'GetApprovalSubject', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('APPROVALSETUP', 'SYSSETUP', '审批设置', 5, '', '', '', 'Menu', NULL, NULL, 'approval', 'syssetup/approval/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('APPROVALUPDATE', 'APPROVALSETUP', '编辑审批设置', 2, 'Approval', 'UpdateProcess', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('BINADD', 'BINMGR', '添加货位', 5, 'ShelfBin', 'AddBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('BINDEL', 'BINMGR', '删除货位', 7, 'ShelfBin', 'DelBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('BINMGR', 'INVENTORYMGR', '货位管理', 2, '', '', NULL, 'Menu', NULL, NULL, 'bin', 'inventory/bin/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('BINREAD', 'BINMGR', '查看货位', 1, 'ShelfBin', 'GetWarehouseTree', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('BINUPDATE', 'BINMGR', '修改货位', 6, 'ShelfBin', 'UpdateBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('BUFFERWAREHOUSEDASHBOARD', 'ROOT', '缓存仓看板', 1, '', '', 'iconfont ionfont-md icon-baobiao', 'Menu', NULL, NULL, 'bufferWarehouseDashboard', 'production/bufferWarehouseDashboard/index', 0, NULL, 'DASHBOARD', 0, 0, NULL);
INSERT INTO `sysmenus` VALUES ('BUSINESSDICADD', 'BUSINESSDICMGR', '添加字典', 2, 'BusinessDic', 'AddArgs', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('BUSINESSDICDEL', 'BUSINESSDICMGR', '删除字典', 4, 'BusinessDic', 'DelArgs', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('BUSINESSDICMGR', 'SYSINFO', '企业字典', 6, '', '', '', 'Menu', NULL, NULL, 'businessdic', 'sysinfo/businessdic/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('BUSINESSDICREAD', 'BUSINESSDICMGR', '查看字典', 1, 'BusinessDic', 'GetArgs', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('BUSINESSDICUPDATE', 'BUSINESSDICMGR', '修改字典', 3, 'BusinessDic', 'UpdateArgs', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('CLIENTADD', 'CLIENTMGR', '添加客户信息', 2, 'Client', 'AddClient', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('CLIENTDEL', 'CLIENTMGR', '删除客户信息', 4, 'Client', 'DelClient', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('CLIENTEXPORT', 'CLIENTMGR', '导出客户信息', 5, 'Client', 'ExportClients', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('CLIENTMGR', 'CLIENTSUPPLIERMGR', '客户管理', 1, '', '', NULL, 'Menu', NULL, NULL, 'client', 'clientsupplier/client/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('CLIENTREAD', 'CLIENTMGR', '查看客户信息', 1, 'Client', 'GetClients', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('CLIENTSUPPLIERMGR', 'ROOT', '客户与供应商', 6, '', '', 'iconfont ionfont-md icon-hezuoguanxi', 'Menu', NULL, NULL, '/clientsupplier', NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('CLIENTUPDATE', 'CLIENTMGR', '修改客户信息', 3, 'Client', 'UpdateClient', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('COMPANYANNOUNCEADD', 'COMPANYANNOUNCEMGR', '发布公告', 2, 'MessageCompany', 'AddMessage', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('COMPANYANNOUNCEDEL', 'COMPANYANNOUNCEMGR', '删除公告', 4, 'MessageCompany', 'DelMessage', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('COMPANYANNOUNCEMGR', 'SYSINFO', '企业公告', 5, '', '', '', 'Menu', NULL, NULL, 'companyannounce', 'sysinfo/companyannounce/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('COMPANYANNOUNCEREAD', 'COMPANYANNOUNCEMGR', '查看公告', 1, 'MessageCompany', 'GetMessages', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('COMPANYANNOUNCEUPDATE', 'COMPANYANNOUNCEMGR', '修改公告', 3, 'MessageCompany', 'UpdateMessage', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('COMPANYMGR', 'SYSINFO', '企业信息', 4, '', '', '', 'Menu', NULL, NULL, 'company', 'sysinfo/company/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('COMPANYREAD', 'COMPANYMGR', '查看企业信息', 1, 'Organization', 'GetCompanyInfo', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('COMPANYUPDATE', 'COMPANYMGR', '编辑企业信息', 2, 'Organization', 'UpdateCompany', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('DELSENDINGFILES', 'SENDINGMGR', '删除发货计划附件', 9, 'SendingOrder', 'DelSendingFiles', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('DEPTADD', 'ORGANIZATIONMGR', '添加部门', 5, 'Organization', 'AddDept', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('DEPTDEL', 'ORGANIZATIONMGR', '删除部门', 7, 'Organization', 'DelDept', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('DEPTUPDATE', 'ORGANIZATIONMGR', '编辑部门', 6, 'Organization', 'UpdateDept', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FIELDSMGR', 'SYSSETUP', '表字段管理', 6, '', '', '', 'Menu', NULL, NULL, 'fieldsmanage', 'syssetup/fieldsmanage/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FIELDSPERMISSIONREAD', 'FIELDSPERMISSIONSETUP', '查看字段权限', 1, 'FieldsPermission', 'GetPermissionTbFields', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FIELDSPERMISSIONSETUP', 'SYSSETUP', '字段权限管理', 7, '', '', '', 'Menu', NULL, NULL, 'fieldspermission', 'syssetup/fieldspermission/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FIELDSPERMISSIONUPDATE', 'FIELDSPERMISSIONSETUP', '编辑字段权限', 2, 'FieldsPermission', 'SetPermissioFieldRole', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FIELDSREAD', 'FIELDSMGR', '查看表字段', 1, 'FieldsManage', 'GetTableFieldList', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FIELDSUPDATE', 'FIELDSMGR', '编辑表字段', 2, 'FieldsManage', 'SetSpareField', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FINISHEDPRODUCTADD', 'FINISHEDPRODUCTMGR', '添加成品信息', 2, 'FinishedProduct', 'AddFinishedProduct', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FINISHEDPRODUCTBOM', 'FINISHEDPRODUCTMGR', 'BOM维护', 5, 'FinishedProduct', 'UpdateBOM', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FINISHEDPRODUCTDEL', 'FINISHEDPRODUCTMGR', '删除成品信息', 4, 'FinishedProduct', 'DelFinishedProduct', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FINISHEDPRODUCTEXPORT', 'FINISHEDPRODUCTMGR', '导出成品信息', 5, 'FinishedProduct', 'ExportFinishedProduct', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FINISHEDPRODUCTMGR', 'GOODSINFOMGR', '成品管理', 5, '', '', NULL, 'Menu', NULL, NULL, 'finished-product', 'goodsinfo/finished-product/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FINISHEDPRODUCTREAD', 'FINISHEDPRODUCTMGR', '查看成品信息', 1, 'FinishedProduct', 'GetFinishedProductList', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FINISHEDPRODUCTTYPEADD', 'FINISHEDPRODUCTMGR', '添加成品类型', 6, 'FinishedProduct', 'AddFinishedProductClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FINISHEDPRODUCTTYPEDEL', 'FINISHEDPRODUCTMGR', '删除成品类型', 8, 'FinishedProduct', 'DelFinishedProductClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FINISHEDPRODUCTTYPEUPDATE', 'FINISHEDPRODUCTMGR', '编辑成品类型', 7, 'FinishedProduct', 'UpdateFinishedProductClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('FINISHEDPRODUCTUPDATE', 'FINISHEDPRODUCTMGR', '修改成品信息', 3, 'FinishedProduct', 'UpdateFinishedProduct', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('GOODSINFOMGR', 'ROOT', '货品信息', 6, '', '', 'iconfont ionfont-md icon-tijikongjian', 'Menu', NULL, NULL, '/goodsinfo', NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('GOODSMOBILEDETAIL', 'PDAMOBILEMGR', '货品信息-货品明细', 14, '', '', NULL, 'Menu', NULL, NULL, 'goodsdetail-mobile', 'mobilepda/goods/detail-mobile', 0, NULL, 'MOBILE', 1, 0, NULL);
INSERT INTO `sysmenus` VALUES ('GOODSMOBILEDETAILREAD', 'GOODSMOBILEDETAIL', '查看货品明细', 1, 'Goods', 'GetGoodsDetail', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('GOODSMOBILEDETAILUPDATE', 'GOODSMOBILEDETAIL', '修改货品明细', 1, 'Goods', 'UpdateGoodsPhoto', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('GOODSMOBILEMGR', 'PDAMOBILEMGR', '货品信息', 13, '', '', 'beijianguanli.png', 'Menu', NULL, NULL, 'goodslist-mobile', 'mobilepda/goods/index-mobile', 0, NULL, NULL, 1, 0, NULL);
INSERT INTO `sysmenus` VALUES ('GOODSMOBILEREAD', 'GOODSMOBILEMGR', '查看货品列表', 1, 'Goods', 'GetGoodsList', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, NULL, 1, 0, NULL);
INSERT INTO `sysmenus` VALUES ('GOODSMREAD', 'INSTORGEMOBILE', '查看明细', 1, 'Goods', 'GetGoodsList', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, NULL, 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('IMPORTMGR', 'SYSSETUP', '数据导入设置', 4, '', '', '', 'Menu', NULL, NULL, 'dataimport', 'syssetup/dataimport/index', 0, NULL, 'PC', 0, 1, NULL);
INSERT INTO `sysmenus` VALUES ('IMPORTREAD', 'IMPORTMGR', '查看数据导入设置', 1, 'DataImport', 'Index', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('IMPORTUPDATE', 'IMPORTMGR', '编辑数据导入设置', 2, 'DataImport', 'Update', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('INSTORAGEDIT', 'INSTORGEMOBILE', '入库管理-编辑入库', 15, '', '', NULL, 'Menu', NULL, NULL, 'orderedit-mobile', 'mobilepda/instorage/orderEditLayer', 1, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('INSTORAGEMGR', 'STOCKMGR', '入库管理', 4, '', '', NULL, 'Menu', NULL, NULL, 'instorage', 'inventory/instorage/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('INSTORAGEORDERADD', 'INSTORAGEMGR', '添加入库单', 2, 'InStorage', 'AddInStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('INSTORAGEORDERCONFIRM', 'INSTORAGEMGR', '入库确认', 7, 'InStorage', 'ConfirmInStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('INSTORAGEORDERDEL', 'INSTORAGEMGR', '删除入库单', 4, 'InStorage', 'DelInStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('INSTORAGEORDEREXPORT', 'INSTORAGEMGR', '导出入库单', 6, 'InStorage', 'ExportInStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('INSTORAGEORDERREAD', 'INSTORAGEMGR', '查看入库单', 1, 'InStorage', 'GetOrders', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('INSTORAGEORDERUPDATE', 'INSTORAGEMGR', '修改入库单', 3, 'InStorage', 'UpdateInStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('INSTORAGEORDMOBILERADD', 'INSTORGEMOBILE', '确认', 2, 'InStorage', 'AddInStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, NULL, 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('INSTORGEMOBILE', 'PDAMOBILEMGR', '原材料入库登记', 16, '', '', 'zhuangche2.png', 'Menu', NULL, NULL, 'instoragemobile', 'mobilepda/instoragemobile/index', 0, 'Primary', 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('INVENTORYMGR', 'ROOT', '仓储管理', 3, '', '', 'iconfont ionfont-md icon-cangkucangchu', 'Menu', NULL, NULL, '/inventory', NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LABELDESIGNADD', 'LABELDESIGNMGR', '添加标签', 2, 'MaterialLabelDesign', 'AddLabelDesign', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LABELDESIGNDEL', 'LABELDESIGNMGR', '删除标签', 5, 'MaterialLabelDesign', 'DelLabelDesign', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LABELDESIGNMGR', 'LABELMGR', '标签设计', 7, '', '', NULL, 'Menu', NULL, NULL, 'label-design', 'label/label-design/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LABELDESIGNREAD', 'LABELDESIGNMGR', '查看标签', 1, 'MaterialLabelDesign', 'GetLabelDesign', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LABELDESIGNUPDATE', 'LABELDESIGNMGR', '修改标签', 3, 'MaterialLabelDesign', 'UpdateLabelDesign', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LABELDESIGNUPDATEDEFT', 'LABELDESIGNMGR', '修改默认模板', 4, 'MaterialLabelDesign', 'UpdateLabelDeft', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LABELMGR', 'ROOT', '标签管理', 7, '', '', 'iconfont ionfont-md icon-label', 'Menu', NULL, NULL, '/label', NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LABELPRINT', 'LABELPRINTMGR', '打印标签', 1, 'LabelPrint', 'AddPrintRecord', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LABELPRINTMGR', 'LABELMGR', '标签打印', 8, '', '', NULL, 'Menu', NULL, NULL, 'label-print', 'label/label-print/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LAYOUTREAD', 'WAREHOUSELAYOUT', '查看布局', 1, 'WarehouseLayout', 'GetWarehouseTree', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LAYOUTUNLOCK', 'WAREHOUSELAYOUT', '货位解锁', 1, 'WarehouseLayout', 'UnlockBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LOGINFO', 'SYSSETUP', '操作日志', 8, '', '', '', 'Menu', NULL, NULL, 'loginfo', 'syssetup/loginfo/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('LOGREAD', 'LOGINFO', '查看操作日志', 1, 'LogInfo', 'GetLogs', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MATERIALREQUIREMENTPLAN', 'RECEIVINGDELIVERYPLAN', '物料需求计划', 3, '', '', NULL, 'Menu', NULL, NULL, 'mrp', 'purchase/mrp/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MATERIALREQUIREMENTPLANEXPORT', 'MATERIALREQUIREMENTPLAN', '导出物料需求计划', 5, 'MaterialRequirementPlan', 'ExportPlanOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MATERIALREQUIREMENTPLANMAIL', 'MATERIALREQUIREMENTPLAN', '发送物料需求计划邮件', 5, 'MaterialRequirementPlan', 'SendMailToSupplier', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MATERIALREQUIREMENTPLANREAD', 'MATERIALREQUIREMENTPLAN', '查看物料需求计划', 1, 'MaterialRequirementPlan', 'GetPlanOrders', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MENUSADD', 'MENUSMGR', '添加菜单', 1, 'Menu', 'AddMenu', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MENUSDEL', 'MENUSMGR', '删除菜单', 3, 'Menu', 'DelMenu', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MENUSMGR', 'SYSSETUP', '菜单管理', 1, '', '', '', 'Menu', NULL, NULL, 'menu', 'syssetup/menu/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MENUSREAD', 'MENUSMGR', '查看菜单', 1, 'Menu', 'GetMenusData', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MENUSUPDATE', 'MENUSMGR', '编辑菜单', 2, 'Menu', 'UpdateMenu', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MESSAGE', 'PDAMOBILEMGR', '消息', 6, '', '', '', 'Menu', NULL, NULL, 'pdamsg', 'mobilepda/message', 1, 'Bottom', 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MESSAGEREAD', 'MESSAGE', '查看消息', 1, 'PDAMessage', 'GetMessages', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MESSAGESUBSCRIBE', 'MYSETTING', '订阅消息', 2, 'PDAMessage', 'SubmitSubscribeMessageType', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MYSETTING', 'PDAMOBILEMGR', '我的', 7, '', '', '', 'Menu', NULL, NULL, 'pdausersetting', 'mobilepda/usersetting', 1, 'Bottom', 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('MYSETTINGREAD', 'MYSETTING', '我的设置', 1, 'PDAMessage', '', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('ORGANIZATIONMGR', 'SYSINFO', '组织架构管理', 2, '', '', '', 'Menu', NULL, NULL, 'deptroles', 'sysinfo/deptroles/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('ORGANIZATIONREAD', 'ORGANIZATIONMGR', '查看组织架构', 1, 'Organization', 'GetOrganizationData', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('OUTSTORAGEMGR', 'STOCKMGR', '出库管理', 5, '', '', NULL, 'Menu', NULL, NULL, 'outstorage', 'inventory/outstorage/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('OUTSTORAGEORDERADD', 'OUTSTORAGEMGR', '添加出库单', 2, 'OutStorage', 'AddOutStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('OUTSTORAGEORDERCONFIRM', 'OUTSTORAGEMGR', '出库确认', 7, 'OutStorage', 'ConfirmOutStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('OUTSTORAGEORDERDEL', 'OUTSTORAGEMGR', '删除出库单', 4, 'OutStorage', 'DelOutStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('OUTSTORAGEORDEREXPORT', 'OUTSTORAGEMGR', '导出出库单', 6, 'OutStorage', 'ExportOutStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('OUTSTORAGEORDERREAD', 'OUTSTORAGEMGR', '查看出库单', 1, 'OutStorage', 'GetOrders', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('OUTSTORAGEORDERUPDATE', 'OUTSTORAGEMGR', '修改出库单', 3, 'OutStorage', 'UpdateOutStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, NULL, 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PACKINGMATERIALADD', 'PACKINGMATERIALMGR', '添加包材信息', 2, 'PackingMaterial', 'AddPackingMaterial', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PACKINGMATERIALDEL', 'PACKINGMATERIALMGR', '删除包材信息', 4, 'PackingMaterial', 'DelPackingMaterial', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PACKINGMATERIALEXPORT', 'PACKINGMATERIALMGR', '导出包材信息', 5, 'PackingMaterial', 'ExportPackingMaterial', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PACKINGMATERIALMGR', 'GOODSINFOMGR', '包材管理', 4, '', '', NULL, 'Menu', NULL, NULL, 'packing-material', 'goodsinfo/packing-material/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PACKINGMATERIALREAD', 'PACKINGMATERIALMGR', '查看包材信息', 1, 'PackingMaterial', 'GetPackingMaterialList', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PACKINGMATERIALTYPEADD', 'PACKINGMATERIALMGR', '添加包材类型', 6, 'PackingMaterial', 'AddPackingMaterialClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PACKINGMATERIALTYPEDEL', 'PACKINGMATERIALMGR', '删除包材类型', 8, 'PackingMaterial', 'DelPackingMaterialClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PACKINGMATERIALTYPEUPDATE', 'PACKINGMATERIALMGR', '编辑包材类型', 7, 'PackingMaterial', 'UpdatePackingMaterialClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PACKINGMATERIALUPDATE', 'PACKINGMATERIALMGR', '修改包材信息', 3, 'PackingMaterial', 'UpdatePackingMaterial', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PDAMOBILEMGR', 'ROOT', '移动端操作', 11, '', '', '', 'Menu', NULL, NULL, 'mobilepda', 'mobilepda/index', 1, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PERMISSIONREAD', 'PERMISSIONSETUP', '查看系统权限', 1, 'UserPermission', 'GetOrganizationData', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PERMISSIONSETUP', 'SYSSETUP', '系统权限设置', 2, '', '', '', 'Menu', NULL, NULL, 'syspermission', 'syssetup/syspermission/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PERMISSIONUPDATE', 'PERMISSIONSETUP', '修改系统权限', 2, 'UserPermission', 'UpdatePermissions', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODORDERADD', 'PRODORDERMGR', '添加订单', 2, 'ProductionOrder', 'AddOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODORDERCARCODECREATE', 'PRODORDERMGR', '创建小车唯一码', 5, 'ProductionOrder', 'CreateCarCode', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODORDERCLOSED', 'PRODORDERMGR', '关闭订单', 7, 'ProductionOrder', 'UpdateOrderClosed', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODORDERDEL', 'PRODORDERMGR', '删除订单', 4, 'ProductionOrder', 'DelOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODORDERMGR', 'PRODUCTIONMGR', '生产订单管理', 1, '', '', NULL, 'Menu', NULL, NULL, 'orders', 'production/orders/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODORDERPRINT', 'PRODORDERMGR', '打印订单', 6, 'ProductionOrder', 'PrintOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODORDERPRINTPAGE', 'ROOT', '生产订单打印预览', 1, '', '', NULL, 'Menu', NULL, NULL, 'printOrder', 'production/orders/printOrder', 1, NULL, 'OTHER', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODORDERREAD', 'PRODORDERMGR', '查看订单', 1, 'ProductionOrder', 'GetOrders', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODORDERUPDATE', 'PRODORDERMGR', '修改订单', 3, 'ProductionOrder', 'UpdateOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODPACKAGECLEARMATCH', 'PRODPACKAGEMGR', '配对包装提交', 2, 'ProductPackage', 'SubmitMatchPackage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'SEPARATE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODPACKAGEMGR', 'ROOT', '成品包装管理', 1, '', '', 'iconfont ionfont-md icon-chaibaoguoqujian', 'Menu', NULL, NULL, 'package', 'production/package/index', 0, NULL, 'SEPARATE', 0, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODPACKAGEREAD', 'PRODPACKAGEMGR', '查看包装配对', 1, 'ProductPackage', 'GetMatchingInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'SEPARATE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PRODUCTIONMGR', 'ROOT', '生产管理', 2, '', '', 'iconfont ionfont-md icon-createtask', 'Menu', NULL, NULL, '/production', NULL, 0, NULL, 'PC', 1, 0, NULL);
INSERT INTO `sysmenus` VALUES ('PROVIDERADD', 'PROVIDERMGR', '添加第三方账号', 1, 'ExternalProvider', 'AddProvider', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PROVIDERDEL', 'PROVIDERMGR', '删除第三方账号', 3, 'ExternalProvider', 'DelProvider', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PROVIDERMGR', 'SYSINFO', '第三方账号管理', 1, '', '', '', 'Menu', NULL, NULL, 'externalProvider', 'sysinfo/externalProvider/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PROVIDERREAD', 'PROVIDERMGR', '查看第三方账号', 1, 'ExternalProvider', 'GetProviders', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PROVIDERUPDATE', 'PROVIDERMGR', '编辑第三方账号', 2, 'ExternalProvider', 'UpdateProvider', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PUTAWAY', 'PDAMOBILEMGR', '缓存仓上架', 2, '', '', 'shangjia.png', 'Menu', NULL, NULL, 'putaway', 'mobilepda/putaway/index', 1, 'Primary', 'MOBILE', 0, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PUTAWAYQUERY', 'PUTAWAY', '扫码查询', 1, 'PDAPutaway', 'GetProductionOrderInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PUTAWAYSUBMIT', 'PUTAWAY', '确认上架', 2, 'PDAPutaway', 'SubmitPutaway', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PUTOUT', 'PDAMOBILEMGR', '缓存仓下架', 3, '', '', 'xiajia.png', 'Menu', NULL, NULL, 'putout', 'mobilepda/putout/index', 1, 'Primary', 'MOBILE', 0, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PUTOUTQUERY', 'PUTOUT', '扫码查询', 1, 'PDAPutout', 'GetProductionOrderInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('PUTOUTSUBMIT', 'PUTOUT', '确认下架', 2, 'PDAPutout', 'SubmitPutout', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('QUERYORDER', 'PDAMOBILEMGR', '交接单查询', 5, '', '', 'danjuchaxun.png', 'Menu', NULL, NULL, 'queryorder', 'mobilepda/queryorder/index', 1, 'Primary', 'MOBILE', 0, 1, NULL);
INSERT INTO `sysmenus` VALUES ('QUERYORDERQUERY', 'QUERYORDER', '扫码查询', 1, '', '', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('QUERYORDERUNLOCK', 'QUERYORDER', '货位解锁', 2, 'PDAOrderQuery', 'UnlockBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RAWMATERIALADD', 'RAWMATERIALMGR', '添加原材料信息', 2, 'RawMaterial', 'AddRawMaterial', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RAWMATERIALDEL', 'RAWMATERIALMGR', '删除原材料信息', 4, 'RawMaterial', 'DelRawMaterial', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RAWMATERIALEXPORT', 'RAWMATERIALMGR', '导出原材料信息', 5, 'RawMaterial', 'ExportRawMaterial', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RAWMATERIALMGR', 'GOODSINFOMGR', '原材料管理', 5, '', '', NULL, 'Menu', NULL, NULL, 'raw-material', 'goodsinfo/raw-material/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RAWMATERIALREAD', 'RAWMATERIALMGR', '查看原材料信息', 1, 'RawMaterial', 'GetRawMaterialList', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RAWMATERIALTYPEADD', 'RAWMATERIALMGR', '添加原材料类型', 6, 'RawMaterial', 'AddRawMaterialClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RAWMATERIALTYPEDEL', 'RAWMATERIALMGR', '删除原材料类型', 8, 'RawMaterial', 'DelRawMaterialClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RAWMATERIALTYPEUPDATE', 'RAWMATERIALMGR', '编辑原材料类型', 7, 'RawMaterial', 'UpdateRawMaterialClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RAWMATERIALUPDATE', 'RAWMATERIALMGR', '修改原材料信息', 3, 'RawMaterial', 'UpdateRawMaterial', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGABNORMAL', 'RECEIVINGMGR', '修改异常到货', 8, 'ReceivingOrder', 'UpdateReceivingAbnormal', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGADD', 'RECEIVINGMGR', '添加收货计划', 2, 'ReceivingOrder', 'AddReceivingOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGAFFIRM', 'RECEIVINGMGR', '确认收货', 7, 'ReceivingOrder', 'SubmitReceived', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGAPPROVAL', 'RECEIVINGMGR', '审批收货计划', 5, 'ReceivingOrder', 'ApprovalReceivingOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGDEL', 'RECEIVINGMGR', '删除收货计划', 4, 'ReceivingOrder', 'DelReceivingOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGDELIVERYORDERADD', 'RECEIVINGMGR', '创建送货单', 12, 'ReceivingOrder', 'AddDeliveryOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGDELIVERYORDERPRINT', 'RECEIVINGMGR', '打印送货单', 13, 'ReceivingOrder', 'PrintDeliveryOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGDELIVERYPLAN', 'ROOT', '计划管理', 2, '', '', 'iconfont ionfont-md icon-caigou', 'Menu', NULL, NULL, '/purchase', NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGMATLABELPRINT', 'RECEIVINGMGR', '打印MAT-LABEL', 14, 'ReceivingOrder', 'PrintMatLabel', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGMGR', 'RECEIVINGDELIVERYPLAN', '收货计划', 1, '', '', NULL, 'Menu', NULL, NULL, 'receiving', 'purchase/receiving/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGNOTIFICATION', 'RECEIVINGMGR', '邮件通知收货', 6, 'ReceivingOrder', 'AdviceReceiving', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGREAD', 'RECEIVINGMGR', '查看收货计划', 1, 'ReceivingOrder', 'GetOrders', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGREADUPLOAD', 'RECEIVINGMGR', '查看收货计划异常图片', 16, 'ReceivingOrder', 'GetBaseFiles', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGSAPEXPORT', 'RECEIVINGMGR', '导出收货计划', 11, 'ReceivingOrder', 'ExportReceivingData', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGSAPIMPORT', 'RECEIVINGMGR', '导入收货计划', 10, 'ReceivingOrder', 'ImportReceivingData', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGUPDATE', 'RECEIVINGMGR', '修改收货计划', 3, 'ReceivingOrder', 'UpdateReceivingOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGUPLOAD', 'RECEIVINGMGR', '上传收货计划异常图片', 15, 'ReceivingOrder', 'UploadReceivingDocument', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('RECEIVINGURGENCY', 'RECEIVINGMGR', '修改紧急数量', 9, 'ReceivingOrder', 'UpdateReceivingUrgency', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('REPORTSAMPLEPIECE', 'REPORTSMGR', '样件报表', 1, '', '', NULL, 'Menu', NULL, NULL, 'samplepiece', 'reports/samplepiece/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('REPORTSAMPLEPIECEREAD', 'REPORTSAMPLEPIECE', '样件报表查看', 1, 'ReportSamplePiece', '', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('REPORTSMGR', 'ROOT', '报表管理', 2, '', '', 'iconfont ionfont-md icon-bingtu', 'Menu', NULL, NULL, '/reports', NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('REPORTSPAREPART', 'REPORTSMGR', '备件报表', 1, '', '', NULL, 'Menu', NULL, NULL, 'sparepart', 'reports/sparepart/index', 0, NULL, 'EXTERNAL', 1, 1, 'https://app.powerbi.com/groups/914ae554-ec29-40a1-bd4c-8e5e62970ce2/reports/fb6380fc-7c82-4e1b-a501-8a1be431c200/ReportSection?experience=power-bi');
INSERT INTO `sysmenus` VALUES ('REPORTSPAREPARTREAD', 'REPORTSPAREPART', '备件报表查看', 1, 'ReportSparepart', '', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'EXTERNAL', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('REQUISITIONADD', 'REQUISITIONMGR', '添加领用单', 2, 'Requisition', 'AddReqisitionOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'SEPARATE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('REQUISITIONDEL', 'REQUISITIONMGR', '删除领用单', 4, 'Requisition', 'DelReqisitionOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'SEPARATE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('REQUISITIONEREAD', 'REQUISITIONMGR', '查看领用单', 1, 'Requisition', 'GetOrderList', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'SEPARATE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('REQUISITIONMGR', 'ROOT', '物品领用', 1, '', '', 'iconfont ionfont-md icon-yewu-xianxing', 'Menu', NULL, NULL, 'requisition', 'inventory/requisition/index', 0, NULL, 'SEPARATE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('REQUISITIONRECEIVE', 'REQUISITIONMGR', '确认收货', 5, 'Requisition', 'ConfirmReceived', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'SEPARATE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('REQUISITIONUPDATE', 'REQUISITIONMGR', '修改领用单', 3, 'Requisition', 'UpdateReqisitionOrder', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'SEPARATE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('ROLEADD', 'ORGANIZATIONMGR', '添加角色', 2, 'Organization', 'AddRole', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('ROLEDEL', 'ORGANIZATIONMGR', '删除角色', 4, 'Organization', 'DelRole', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('ROLEUPDATE', 'ORGANIZATIONMGR', '编辑角色', 3, 'Organization', 'UpdateRole', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAFETYINVENTORYAPPROVAL', 'SAFETYINVENTORYMGR', '审批预警信息', 4, 'SafetyWarningRecord', 'ApprovalSafetyInventory', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAFETYINVENTORYEXPORT', 'SAFETYINVENTORYMGR', '导出预警信息', 1, 'SafetyWarningRecord', 'ExportWarningInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAFETYINVENTORYMGR', 'STOCKMGR', '安全库存', 8, '', '', NULL, 'Menu', NULL, NULL, 'safety-inventory', 'inventory/safety-inventory/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAFETYINVENTORYREAD', 'SAFETYINVENTORYMGR', '查看库存预警', 1, 'SafetyWarningRecord', 'GetWarningInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAFETYINVENTORYUPDATE', 'SAFETYINVENTORYMGR', '修改预警信息', 1, 'SafetyWarningRecord', 'UpdateWarningInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAMPLEPIECEADD', 'SAMPLEPIECEMGR', '添加样件信息', 2, 'SamplePiece', 'AddSamplePiece', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAMPLEPIECEDEL', 'SAMPLEPIECEMGR', '删除样件信息', 4, 'SamplePiece', 'DelSamplePiece', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAMPLEPIECEEXPORT', 'SAMPLEPIECEMGR', '导出样件信息', 5, 'SamplePiece', 'ExportSamplePiece', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAMPLEPIECEMGR', 'GOODSINFOMGR', '模具管理', 1, '', '', NULL, 'Menu', NULL, NULL, 'sample-piece', 'goodsinfo/sample-piece/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAMPLEPIECEREAD', 'SAMPLEPIECEMGR', '查看样件信息', 1, 'SamplePiece', 'GetSamplePieceList', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAMPLEPIECETYPEADD', 'SAMPLEPIECEMGR', '添加样件类型', 6, 'SamplePiece', 'AddSamplePieceClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAMPLEPIECETYPEDEL', 'SAMPLEPIECEMGR', '删除样件类型', 8, 'SamplePiece', 'DelSamplePieceClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAMPLEPIECETYPEUPDATE', 'SAMPLEPIECEMGR', '编辑样件类型', 7, 'SamplePiece', 'UpdateSamplePieceClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SAMPLEPIECEUPDATE', 'SAMPLEPIECEMGR', '修改样件信息', 3, 'SamplePiece', 'UpdateSamplePiece', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SCANINSTORAGE', 'PDAMOBILEMGR', '扫码入库', 5, '', '', 'rukudan.png', 'Menu', NULL, NULL, 'scan-instorage', 'mobilepda/scan-instorage/index', 1, 'Primary', 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SCANINSTORAGEGOODSQUERY', 'SCANINSTORAGE', '查看物品列表', 1, 'InStorageLabels', 'GetGoodsByKey', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SCANINSTORAGESELECTGOODS', 'PDAMOBILEMGR', '扫码入库-选择物品', 1, '', '', '', 'Menu', NULL, NULL, 'select-instorage-goods', 'mobilepda/scan-instorage/select-instorage-goods', 1, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SCANINSTORAGESUBMITCODE', 'SCANINSTORAGE', '提交入库', 2, 'InStorageLabels', 'SubmitCode', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SCANINSTORAGESUBMITSCAN', 'SCANINSTORAGE', '扫码登记', 1, 'InStorageLabels', 'SubmitScan', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SENDINGADD', 'SENDINGMGR', '添加发货计划', 2, 'SendingOrder', 'AddSending', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SENDINGDEL', 'SENDINGMGR', '删除发货计划', 4, 'SendingOrder', 'DelSending', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SENDINGEXPORT', 'SENDINGMGR', '导入发货计划', 6, 'SendingOrder', 'ImportSendingData', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SENDINGIMPORT', 'SENDINGMGR', '导出发货计划', 5, 'SendingOrder', 'ExportSendingData', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SENDINGMGR', 'RECEIVINGDELIVERYPLAN', '发货计划', 2, '', '', NULL, 'Menu', NULL, NULL, 'sending', 'purchase/sending/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SENDINGNOTIFICATION', 'SENDINGMGR', '发送邮件通知', 10, 'SendingOrder', 'AdviceSending', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SENDINGREAD', 'SENDINGMGR', '查看发货计划', 1, 'SendingOrder', 'GetSending', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SENDINGREADUPLOAD', 'SENDINGMGR', '查看发货计划附件', 8, 'SendingOrder', 'GetBaseFiles', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SENDINGUPDATE', 'SENDINGMGR', '修改发货计划', 3, 'SendingOrder', 'UpdateSending', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SENDINGUPLOAD', 'SENDINGMGR', '上传发货计划附件', 7, 'SendingOrder', 'UploadSendingDocument', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SEPARATORADD', 'SEPARATORMGR', '添加辅材信息', 2, 'Separator', 'AddSeparator', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SEPARATORDEL', 'SEPARATORMGR', '删除辅材信息', 4, 'Separator', 'DelSeparator', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SEPARATOREXPORT', 'SEPARATORMGR', '导出辅材信息', 5, 'Separator', 'ExportSeparator', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SEPARATORMGR', 'GOODSINFOMGR', '辅材管理', 3, '', '', NULL, 'Menu', NULL, NULL, 'separator', 'goodsinfo/separator/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SEPARATORREAD', 'SEPARATORMGR', '查看辅材信息', 1, 'Separator', 'GetSeparatorList', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SEPARATORTYPEADD', 'SEPARATORMGR', '添加辅材类型', 6, 'Separator', 'AddSeparatorClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SEPARATORTYPEDEL', 'SEPARATORMGR', '删除辅材类型', 8, 'Separator', 'DelSeparatorClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SEPARATORTYPEUPDATE', 'SEPARATORMGR', '编辑辅材类型', 7, 'Separator', 'UpdateSeparatorClassify', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SEPARATORUPDATE', 'SEPARATORMGR', '修改辅材信息', 3, 'Separator', 'UpdateSeparator', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SHELFADD', 'BINMGR', '添加货架', 2, 'ShelfBin', 'AddShelf', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SHELFDEL', 'BINMGR', '删除货架', 4, 'ShelfBin', 'DelShelf', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SHELFUPDATE', 'BINMGR', '修改货架', 3, 'ShelfBin', 'UpdateShelf', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SPAERPARTTYPEADD', 'SPAREPARTMGR', '添加备件类型', 6, 'SparePart', 'AddSparePartType', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SPAERPARTTYPEDEL', 'SPAREPARTMGR', '删除备件类型', 8, 'SparePart', 'DelSparePartType', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SPAERPARTTYPEUPDATE', 'SPAREPARTMGR', '编辑备件类型', 7, 'SparePart', 'UpdateSparePartType', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SPAREPARTADD', 'SPAREPARTMGR', '添加备件信息', 2, 'SparePart', 'AddSparePart', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SPAREPARTDEL', 'SPAREPARTMGR', '删除备件信息', 4, 'SparePart', 'DelSparePart', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SPAREPARTEXPORT', 'SPAREPARTMGR', '导出备件信息', 5, 'SparePart', 'ExportSparePart', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SPAREPARTMGR', 'GOODSINFOMGR', '备件管理', 2, '', '', NULL, 'Menu', NULL, NULL, 'spare-part', 'goodsinfo/spare-part/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SPAREPARTREAD', 'SPAREPARTMGR', '查看备件信息', 1, 'SparePart', 'GetSparePartList', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SPAREPARTUPDATE', 'SPAREPARTMGR', '修改备件信息', 3, 'SparePart', 'UpdateSparePart', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('STOCKMGR', 'ROOT', '库存管理', 4, '', '', 'iconfont ionfont-md icon-zhongzhuanzhan', 'Menu', NULL, NULL, '/stock', NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('STORAGEALLOCATION', 'STORAGEMGR', '添加库存调拨', 4, 'Storage', 'AddAllocationStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('STORAGEDETAILEXPORT', 'STORAGEMGR', '导出库存明细信息', 6, 'Storage', 'ExportStorageDetail', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('STORAGEDETAILREAD', 'STORAGEMGR', '查看库存明细', 2, 'Storage', 'GetStorageDetailList', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('STORAGEEXPORT', 'STORAGEMGR', '导出库存汇总信息', 5, 'Storage', 'ExportStorage', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('STORAGEFLOWEXPORT', 'STORAGEMGR', '导出库存流水记录', 7, 'Storage', 'ExportStorageFlow', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('STORAGEFLOWREAD', 'STORAGEMGR', '查看库存流水', 3, 'Storage', 'GetStorageFlowList', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('STORAGEMGR', 'STOCKMGR', '库存信息', 6, '', '', NULL, 'Menu', NULL, NULL, 'storage', 'inventory/storage/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('STORAGEREAD', 'STORAGEMGR', '查看库存汇总', 1, 'Storage', 'GetStorageList', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SUPPLIERADD', 'SUPPLIERMGR', '添加供应商信息', 2, 'Supplier', 'AddSupplier', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SUPPLIERDEL', 'SUPPLIERMGR', '删除供应商信息', 4, 'Supplier', 'DelSupplier', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SUPPLIEREXPORT', 'SUPPLIERMGR', '导出供应商信息', 5, 'Supplier', 'ExportSuppliers', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SUPPLIERMGR', 'CLIENTSUPPLIERMGR', '供应商管理', 2, '', '', NULL, 'Menu', NULL, NULL, 'supplier', 'clientsupplier/supplier/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SUPPLIERREAD', 'SUPPLIERMGR', '查看供应商信息', 1, 'Supplier', 'GetSuppliers', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SUPPLIERUPDATE', 'SUPPLIERMGR', '修改供应商信息', 3, 'Supplier', 'UpdateSupplier', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SYSARGSREAD', 'SYSARGSSETUP', '查看系统参数', 1, 'SysArgs', 'GetArgs', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SYSARGSSETUP', 'SYSSETUP', '系统参数设置', 3, '', '', '', 'Menu', NULL, NULL, 'args', 'syssetup/args/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SYSARGSUPDATE', 'SYSARGSSETUP', '编辑系统参数', 2, 'SysArgs', 'UpdateArgs', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SYSINFO', 'ROOT', '系统信息', 9, '', '', 'iconfont ionfont-md icon-jiaosequnti', 'Menu', NULL, NULL, '/sysinfo', NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('SYSSETUP', 'ROOT', '系统设置', 10, '', '', 'iconfont ionfont-md icon-shezhi', 'Menu', NULL, NULL, '/syssetup', NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKBYBINMOBILEADD', 'TAKESTOCKMOBILEINVENTORYBYBIN', '盘点提交', 8, 'TakeStock', 'AddTaskStockOrderByBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKBYGOODSMOBILEADD', 'TAKESTOCKMOBILEINVENTORYBYGOODS', '盘点提交', 7, 'TakeStock', 'AddTaskStockOrderByGoods', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKEXPORT', 'TAKESTOCKMGR', '导出盘点记录', 2, 'TakeStock', 'ExportTakeStockHis', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMGR', 'STOCKMGR', '库存盘点', 7, '', '', NULL, 'Menu', NULL, NULL, 'takestock', 'inventory/takestock/index', 0, NULL, NULL, 1, 0, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMOBILEBYBIN', 'PDAMOBILEMGR', '库存盘点-按货位盘点', 10, '', '', NULL, 'Menu', NULL, NULL, 'takestockbybin', 'mobilepda/takestock/takebin-mobile', 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMOBILEBYBINREAD', 'TAKESTOCKMOBILEBYBIN', '查看货位列表', 1, 'TakeStock', 'GetBins', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMOBILEBYGOODS', 'PDAMOBILEMGR', '库存盘点-按物品盘点', 9, '', '', NULL, 'Menu', NULL, NULL, 'takestockbygoods', 'mobilepda/takestock/takegoods-mobile', 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMOBILEBYGOODSREAD', 'TAKESTOCKMOBILEBYGOODS', '查看物品列表', 1, 'TakeStock', 'GetGoodsByKey', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMOBILEINVENTORYBYBIN', 'PDAMOBILEMGR', '库存盘点-按货位清点', 12, '', '', NULL, 'Menu', NULL, NULL, 'takebinsubmit', 'mobilepda/takestock/takebinsubmit-mobile', 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMOBILEINVENTORYBYBINREAD', 'TAKESTOCKMOBILEINVENTORYBYBIN', '查看仓储明细', 1, 'TakeStock', 'GetBinInventoryDetail', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMOBILEINVENTORYBYGOODS', 'PDAMOBILEMGR', '库存盘点-按物品清点', 11, '', '', NULL, 'Menu', NULL, NULL, 'takegoodssubmit', 'mobilepda/takestock/takegoodssubmit-mobile', 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMOBILEINVENTORYBYGOODSREAD', 'TAKESTOCKMOBILEINVENTORYBYGOODS', '查看仓储明细', 1, 'TakeStock', 'GetGoodsInventoryDetail', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMOBILELOCKBIN', 'TAKESTOCKMOBILEBYBIN', '锁定盘点货位', 6, 'TakeStock', 'SetTakeStockLockByBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMOBILELOCKGOODS', 'TAKESTOCKMOBILEBYGOODS', '锁定盘点物品', 5, 'TakeStock', 'SetTakeStockLockByGoods', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMOBILEMGR', 'PDAMOBILEMGR', '库存盘点', 8, '', '', 'kucundan.png', 'Menu', NULL, NULL, 'takestock-mobile', 'mobilepda/takestock/index-mobile', 0, 'Primary', 'MOBILE', 1, 0, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKMOBILEREAD', 'TAKESTOCKMOBILEMGR', '查看盘点信息', 1, 'TakeStock', 'GetExpendTrend', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TAKESTOCKREAD', 'TAKESTOCKMGR', '查看盘点记录', 1, 'TakeStock', 'GetTakeStockHis', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TRUCKLOADING', 'PDAMOBILEMGR', '扫码装车', 1, '', '', 'zhuangche.png', 'Menu', NULL, NULL, 'truckLoading', 'mobilepda/truckLoading/index', 1, 'Primary', 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TRUCKQUERY', 'TRUCKLOADING', '扫码查询', 1, 'PDACarLoad', 'GetProductionOrderInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('TRUCKSUBMIT', 'TRUCKLOADING', '确认装车', 3, 'PDACarLoad', 'SubmitCarLoad', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('UNITADD', 'UNITMGR', '添加单位信息', 2, 'Unit', 'AddUnit', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('UNITDEL', 'UNITMGR', '删除单位信息', 4, 'Unit', 'DelUnit', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('UNITMGR', 'GOODSINFOMGR', '计量单位', 6, '', '', NULL, 'Menu', NULL, NULL, 'unit', 'goodsinfo/unit/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('UNITREAD', 'UNITMGR', '查看单位信息', 1, 'Unit', 'GetUnits', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('UNITUPDATE', 'UNITMGR', '修改单位信息', 3, 'Unit', 'UpdateUnit', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('UNSTACK', 'PDAMOBILEMGR', '拆垛机上架', 4, '', '', 'chaiduo.png', 'Menu', NULL, NULL, 'unstack', 'mobilepda/unstack/index', 1, 'Primary', 'MOBILE', 0, 1, NULL);
INSERT INTO `sysmenus` VALUES ('UNSTACKQUERY', 'UNSTACK', '扫码查询', 1, 'PDAUnstack', 'GetProductionOrderInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('UNSTACKSUBMIT', 'UNSTACK', '确认上架', 2, 'PDAUnstack', 'SubmitOnUnStack', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'MOBILE', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('USERADD', 'USERMGR', '添加用户', 2, 'User', 'AddUser', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('USERDEL', 'USERMGR', '删除用户', 4, 'User', 'DelUser', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('USEREXPORT', 'USERMGR', '导出用户', 5, 'User', 'ExportUsers', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('USERINITPASSWORD', 'USERMGR', '初始化用户密码', 6, 'User', 'InitUserPassword', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('USERMGR', 'SYSINFO', '用户管理', 1, '', '', NULL, 'Menu', NULL, NULL, 'users', 'sysinfo/users/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('USERREAD', 'USERMGR', '查看用户', 1, 'User', 'GetUsers', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('USERUPDATE', 'USERMGR', '编辑用户', 3, 'User', 'UpdateUser', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('USERUPDATEROLE', 'USERMGR', '修改用户角色', 8, 'User', 'UpdateUserRole', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('USERUPDATESTATUS', 'USERMGR', '修改用户状态', 7, 'User', 'UpdateUserStatus', '', 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WAREHOUSEADD', 'WAREHOUSEMGR', '添加仓库', 2, 'WarehouseBin', 'AddWarehouseBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WAREHOUSEDEL', 'WAREHOUSEMGR', '删除仓库', 4, 'WarehouseBin', 'DelWarehouseBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WAREHOUSELAYOUT', 'INVENTORYMGR', '仓库布局', 3, '', '', NULL, 'Menu', NULL, NULL, 'layout', 'inventory/layout/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WAREHOUSEMGR', 'INVENTORYMGR', '仓库管理', 1, '', '', NULL, 'Menu', NULL, NULL, 'warehouse', 'inventory/warehouse/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WAREHOUSEPICKDASHBOARD', 'ROOT', '仓库拣货看板', 1, '', '', 'iconfont ionfont-md icon-jiankongshexiangtou', 'Menu', NULL, NULL, 'warehousePickDashboard', 'inventory/warehousePickDashboard/index', 1, NULL, 'DASHBOARD', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WAREHOUSEREAD', 'WAREHOUSEMGR', '查看仓库', 1, 'WarehouseBin', 'GetWarehouses', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WAREHOUSEUPDATE', 'WAREHOUSEMGR', '修改仓库', 3, 'WarehouseBin', 'UpdateWarehouseBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKBINMGR', 'INVENTORYMGR', '料箱管理', 3, '', '', NULL, 'Menu', NULL, NULL, 'workbin', 'inventory/workbin/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKBINREAD', 'WORKBINMGR', '查看料箱信息', 1, 'Workbin', 'GetWorkbins', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKBINSPECADD', 'WORKBINMGR', '添加料箱规格', 1, 'Workbin', 'AddWorkbinSpeci', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKBINSPECDEL', 'WORKBINMGR', '删除料箱规格', 1, 'Workbin', 'DelWorkbinSpeci', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKBINSPECUPDATE', 'WORKBINMGR', '修改料箱规格', 1, 'Workbin', 'UpdateWorkbinSpeci', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKBINUPDATE', 'WORKBINMGR', '编辑料箱信息', 1, 'Workbin', 'UpdateWorkbin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKSHOPDEVICE', 'ROOT', '车间设备管理', 5, '', '', 'iconfont ionfont-md icon-PDAshouchigongzuoshebei', 'Menu', NULL, NULL, '/workshopdevice', NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKSHOPDEVICEBINADD', 'WORKSHOPDEVICEINFO', '添加设备仓位', 5, 'WorkShopDeviceInfo', 'AddDeviceBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKSHOPDEVICEBINDEL', 'WORKSHOPDEVICEINFO', '删除设备仓位', 7, 'WorkShopDeviceInfo', 'DelDeviceBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKSHOPDEVICEBINUPDATE', 'WORKSHOPDEVICEINFO', '修改设备仓位', 6, 'WorkShopDeviceInfo', 'UpdateDeviceBin', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKSHOPDEVICEINFO', 'WORKSHOPDEVICE', '设备与仓位', 1, '', '', NULL, 'Menu', NULL, NULL, 'device-info', 'workshopdevice/device-info/index', 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKSHOPDEVICEINFOADD', 'WORKSHOPDEVICEINFO', '添加设备信息', 2, 'WorkShopDeviceInfo', 'AddDeviceInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKSHOPDEVICEINFODEL', 'WORKSHOPDEVICEINFO', '删除设备信息', 4, 'WorkShopDeviceInfo', 'DelDeviceInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKSHOPDEVICEINFOREAD', 'WORKSHOPDEVICEINFO', '查看设备信息', 1, 'WorkShopDeviceInfo', 'GetDeviceInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);
INSERT INTO `sysmenus` VALUES ('WORKSHOPDEVICEINFOUPDATE', 'WORKSHOPDEVICEINFO', '修改设备信息', 3, 'WorkShopDeviceInfo', 'UpdateDeviceInfo', NULL, 'Action', NULL, NULL, NULL, NULL, 0, NULL, 'PC', 1, 1, NULL);

-- ----------------------------
-- Table structure for sysmessage
-- ----------------------------
DROP TABLE IF EXISTS `sysmessage`;
CREATE TABLE `sysmessage`  (
  `MessageId` int(11) NOT NULL AUTO_INCREMENT COMMENT '消息ID',
  `MessageType` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '消息类型',
  `Sender` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '发送人',
  `Receiver` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '接收人',
  `Content` varchar(5000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '消息体',
  `Link` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '消息对应的前端链接',
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `CreateDate` datetime NOT NULL COMMENT '记录时间',
  PRIMARY KEY (`MessageId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 14 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '系统消息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysmessage
-- ----------------------------
INSERT INTO `sysmessage` VALUES (1, 'InStorage', '管理员', '', '管理员提交了一份待确认的(采购入库)入库单', '', 'PET', '2025-09-01 11:57:10');
INSERT INTO `sysmessage` VALUES (2, 'InStorage', '管理员', '', '管理员提交了一份待确认的(采购入库)入库单', '', 'PET 625*0.3', '2025-09-01 16:23:53');
INSERT INTO `sysmessage` VALUES (3, 'InStorage', '管理员', '', '管理员提交了一份待确认的(生产入库)入库单', '', '石英矿-100目', '2025-09-04 14:17:46');
INSERT INTO `sysmessage` VALUES (4, 'InStorage', '管理员', '', '管理员提交了一份待确认的(采购入库)入库单', '', '石英原矿', '2025-09-04 14:21:02');
INSERT INTO `sysmessage` VALUES (5, 'InStorage', '管理员', '', '管理员提交了一份待确认的(生产入库)入库单', '', '石英原矿,石英矿-100目', '2025-09-06 19:00:05');
INSERT INTO `sysmessage` VALUES (6, 'OutStorage', '管理员', '', '管理员提交了一份待确认的(销售出库)出库单', '', '石英矿-100目', '2025-09-08 15:33:01');
INSERT INTO `sysmessage` VALUES (7, 'InStorage', '管理员', '', '管理员提交了一份待确认的(生产入库)入库单', '', '石英矿-200目', '2025-09-11 14:11:25');
INSERT INTO `sysmessage` VALUES (8, 'InStorage', '管理员', '', '管理员提交了一份待确认的(生产入库)入库单', '', '石英石原矿', '2025-09-11 14:11:40');
INSERT INTO `sysmessage` VALUES (9, 'OutStorage', '管理员', '', '管理员提交了一份待确认的(领用出库)出库单', '', '石英石原矿', '2025-09-11 14:13:07');
INSERT INTO `sysmessage` VALUES (10, 'InStorage', '管理员', '', '管理员提交了一份待确认的(生产入库)入库单', '', '石英矿成品1', '2025-09-16 14:56:15');
INSERT INTO `sysmessage` VALUES (11, 'InStorage', '管理员', '', '管理员提交了一份待确认的(生产入库)入库单', '', '石英石原矿', '2025-09-17 15:34:34');
INSERT INTO `sysmessage` VALUES (12, 'InStorage', '管理员', '', '管理员提交了一份待确认的(生产入库)入库单', '', '石英石原矿', '2025-09-17 15:43:01');
INSERT INTO `sysmessage` VALUES (13, 'InStorage', '管理员', '', '管理员提交了一份待确认的(生产入库)入库单', '', '石英石原矿', '2025-09-17 16:54:25');

-- ----------------------------
-- Table structure for sysmessagecompany
-- ----------------------------
DROP TABLE IF EXISTS `sysmessagecompany`;
CREATE TABLE `sysmessagecompany`  (
  `MessageId` int(11) NOT NULL AUTO_INCREMENT COMMENT '消息ID',
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '消息体',
  `IsPublishCurrent` tinyint(1) NOT NULL COMMENT '是否为当前发布',
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `DateTime` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '记录时间',
  PRIMARY KEY (`MessageId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '企业公告表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysmessagecompany
-- ----------------------------

-- ----------------------------
-- Table structure for sysmessagemobile
-- ----------------------------
DROP TABLE IF EXISTS `sysmessagemobile`;
CREATE TABLE `sysmessagemobile`  (
  `MessageId` int(11) NOT NULL AUTO_INCREMENT COMMENT '消息ID',
  `Sender` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '发送方',
  `Receiver` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '接收人用户ID',
  `Content` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '消息体',
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `DateTime` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '记录时间',
  PRIMARY KEY (`MessageId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '手机验证码消息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysmessagemobile
-- ----------------------------

-- ----------------------------
-- Table structure for sysmessagereaduser
-- ----------------------------
DROP TABLE IF EXISTS `sysmessagereaduser`;
CREATE TABLE `sysmessagereaduser`  (
  `UserId` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '用户ID',
  `MessageId` int(11) NOT NULL COMMENT '消息ID',
  PRIMARY KEY (`UserId`, `MessageId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysmessagereaduser
-- ----------------------------

-- ----------------------------
-- Table structure for sysmessagesubscribe
-- ----------------------------
DROP TABLE IF EXISTS `sysmessagesubscribe`;
CREATE TABLE `sysmessagesubscribe`  (
  `UserId` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '用户ID',
  `MessageType` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '消息类型',
  PRIMARY KEY (`UserId`, `MessageType`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '系统消息订阅' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysmessagesubscribe
-- ----------------------------

-- ----------------------------
-- Table structure for sysmigrationreleasehis
-- ----------------------------
DROP TABLE IF EXISTS `sysmigrationreleasehis`;
CREATE TABLE `sysmigrationreleasehis`  (
  `ReleaseNo` int(11) NOT NULL COMMENT '版本号',
  `ReleaseDate` datetime NOT NULL COMMENT '发布时间',
  PRIMARY KEY (`ReleaseNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '脚本迁移记录' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysmigrationreleasehis
-- ----------------------------

-- ----------------------------
-- Table structure for sysprovince
-- ----------------------------
DROP TABLE IF EXISTS `sysprovince`;
CREATE TABLE `sysprovince`  (
  `ProvinceId` int(11) NOT NULL COMMENT '省份ID',
  `ProvinceCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '省份编号',
  `ProvinceName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '省份名称',
  `ProvinceShotName` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '省份简称',
  `Remark` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `FirstSpell` varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '拼音首字母',
  `Rank` int(11) NOT NULL COMMENT '界面展示排序',
  PRIMARY KEY (`ProvinceId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '省份表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysprovince
-- ----------------------------

-- ----------------------------
-- Table structure for syspublicrelease
-- ----------------------------
DROP TABLE IF EXISTS `syspublicrelease`;
CREATE TABLE `syspublicrelease`  (
  `VersionNo` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '版本号',
  `SQLMigrationNo` int(11) NULL DEFAULT NULL COMMENT 'SQL脚本迁移版本',
  `ReleaseDate` datetime NOT NULL COMMENT '发布时间',
  PRIMARY KEY (`VersionNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '发布版本管理' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of syspublicrelease
-- ----------------------------

-- ----------------------------
-- Table structure for sysroles
-- ----------------------------
DROP TABLE IF EXISTS `sysroles`;
CREATE TABLE `sysroles`  (
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '角色ID',
  `RoleNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '角色编码',
  `RoleName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '角色名称',
  `Rank` int(11) NOT NULL COMMENT '界面展示排序',
  `DeptId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所属部门ID',
  `ParentId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '上级岗位ID',
  `IsVaild` tinyint(1) NOT NULL COMMENT '是否有效',
  PRIMARY KEY (`RoleId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '角色表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysroles
-- ----------------------------
INSERT INTO `sysroles` VALUES ('2f21dc9e803c43f69ee0c7ca4aac50aa', 'Quality_Inspector', '质检员', 7, NULL, '1000000001', 1);
INSERT INTO `sysroles` VALUES ('6889ef79b92d4210b65f620b6ad1c31d', 'Processing_Admin', '加工管理员', 4, NULL, '1000000001', 1);
INSERT INTO `sysroles` VALUES ('6ef4e1ac313c4d91a2a4e994316a0c9f', 'Order_Admin', '订单管理员', 2, NULL, '1000000001', 1);
INSERT INTO `sysroles` VALUES ('84237547b3ef4a5e9fafe81a9ddb98fe', 'GateClerk', '门卫管理员', 6, NULL, '1000000001', 1);
INSERT INTO `sysroles` VALUES ('ADMIN', 'ADMIN', '管理员', 1, NULL, '1000000001', 1);
INSERT INTO `sysroles` VALUES ('bf471f560acb4f74a1e3eb7c151bdc9d', 'Shipping_Admin', '发货管理员', 3, NULL, '1000000001', 1);
INSERT INTO `sysroles` VALUES ('fa5b9b63e17245a080d6ccf6a030c0b5', 'Financial_Admin', '财务管理员', 5, NULL, '1000000001', 1);

-- ----------------------------
-- Table structure for sysuser
-- ----------------------------
DROP TABLE IF EXISTS `sysuser`;
CREATE TABLE `sysuser`  (
  `UserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '用户ID',
  `UserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '用户姓名',
  `UserCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '工号',
  `NickName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '昵称',
  `DomainName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '域用户名',
  `AuthAccount` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '登录账户',
  `Password` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '登录密码',
  `Email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '邮箱',
  `Wechat` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '微信号',
  `Phone` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '电话',
  `MobilePhone` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '手机号',
  `Address` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '通讯地址',
  `ProvinceName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所在省份',
  `CityName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所在城市',
  `DeptId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所属部门',
  `IsVaild` tinyint(1) NOT NULL COMMENT '是否有效',
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `CreateDate` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '创建时间',
  `DepartureDate` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '离职时间',
  `EditDate` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上次编辑时间',
  `AvatarImgId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '用户图像',
  `CardId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '员工卡号',
  PRIMARY KEY (`UserId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '用户表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysuser
-- ----------------------------
INSERT INTO `sysuser` VALUES ('13', 'Binyan', 'uif92187', 'Binyan', 'uif92187', 'uif92187', 'e10adc3949ba59abbe56e057f20f883e', 'yan.bin-ext@continental-corporation.com', NULL, NULL, NULL, NULL, NULL, NULL, '2', 1, NULL, '2023-06-01 01:49', NULL, '2025-01-03T13:22:30', NULL, NULL);
INSERT INTO `sysuser` VALUES ('159', '谢单', '28081547', 'Xie Dan', '28081547', 'uie89756', 'e10adc3949ba59abbe56e057f20f883e', 'DAN.XIE@CONTINENTAL-CORPORATION.COM', NULL, '15817630920', '15817630920', NULL, NULL, NULL, '17', 1, NULL, '2023-09-14 02:34', NULL, '2025-06-19T17:05:00', NULL, NULL);
INSERT INTO `sysuser` VALUES ('21', '郭人宾', '28088175', 'Guo Renbin', '28088175', 'uig17563', 'e10adc3949ba59abbe56e057f20f883e', 'RENBIN.GUO@CONTINENTAL-CORPORATION.COM', NULL, '18616527816', '18616527816', NULL, NULL, NULL, '2', 1, NULL, '2023-06-09 01:49', NULL, '2025-06-19T17:05:00', NULL, '28088175');
INSERT INTO `sysuser` VALUES ('admin', '管理员', NULL, NULL, NULL, 'admin', 'e10adc3949ba59abbe56e057f20f883e', NULL, NULL, NULL, '13818461927', NULL, NULL, NULL, NULL, 1, NULL, '2023-10-26 14:28:30', NULL, NULL, NULL, '666');

-- ----------------------------
-- Table structure for sysuserpermissions
-- ----------------------------
DROP TABLE IF EXISTS `sysuserpermissions`;
CREATE TABLE `sysuserpermissions`  (
  `PermissionId` int(11) NOT NULL AUTO_INCREMENT COMMENT '用户权限ID',
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '角色ID',
  `MenuId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '权限菜单',
  PRIMARY KEY (`PermissionId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 13730 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '用户权限表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysuserpermissions
-- ----------------------------
INSERT INTO `sysuserpermissions` VALUES (13606, 'ADMIN', 'SENDINGMGR');
INSERT INTO `sysuserpermissions` VALUES (13607, 'ADMIN', 'RECEIVINGDELIVERYPLAN');
INSERT INTO `sysuserpermissions` VALUES (13608, 'ADMIN', 'SENDINGREAD');
INSERT INTO `sysuserpermissions` VALUES (13609, 'ADMIN', 'SENDINGADD');
INSERT INTO `sysuserpermissions` VALUES (13610, 'ADMIN', 'SENDINGUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13611, 'ADMIN', 'SENDINGDEL');
INSERT INTO `sysuserpermissions` VALUES (13612, 'ADMIN', 'SENDINGIMPORT');
INSERT INTO `sysuserpermissions` VALUES (13613, 'ADMIN', 'SENDINGEXPORT');
INSERT INTO `sysuserpermissions` VALUES (13614, 'ADMIN', 'SENDINGUPLOAD');
INSERT INTO `sysuserpermissions` VALUES (13615, 'ADMIN', 'SENDINGREADUPLOAD');
INSERT INTO `sysuserpermissions` VALUES (13616, 'ADMIN', 'DELSENDINGFILES');
INSERT INTO `sysuserpermissions` VALUES (13617, 'ADMIN', 'SENDINGNOTIFICATION');
INSERT INTO `sysuserpermissions` VALUES (13618, 'ADMIN', 'INSTORAGEMGR');
INSERT INTO `sysuserpermissions` VALUES (13619, 'ADMIN', 'STOCKMGR');
INSERT INTO `sysuserpermissions` VALUES (13620, 'ADMIN', 'INSTORAGEORDERREAD');
INSERT INTO `sysuserpermissions` VALUES (13621, 'ADMIN', 'INSTORAGEORDERADD');
INSERT INTO `sysuserpermissions` VALUES (13622, 'ADMIN', 'INSTORAGEORDERUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13623, 'ADMIN', 'INSTORAGEORDERDEL');
INSERT INTO `sysuserpermissions` VALUES (13624, 'ADMIN', 'INSTORAGEORDEREXPORT');
INSERT INTO `sysuserpermissions` VALUES (13625, 'ADMIN', 'INSTORAGEORDERCONFIRM');
INSERT INTO `sysuserpermissions` VALUES (13626, 'ADMIN', 'OUTSTORAGEMGR');
INSERT INTO `sysuserpermissions` VALUES (13627, 'ADMIN', 'OUTSTORAGEORDERREAD');
INSERT INTO `sysuserpermissions` VALUES (13628, 'ADMIN', 'OUTSTORAGEORDERADD');
INSERT INTO `sysuserpermissions` VALUES (13629, 'ADMIN', 'OUTSTORAGEORDERUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13630, 'ADMIN', 'OUTSTORAGEORDERDEL');
INSERT INTO `sysuserpermissions` VALUES (13631, 'ADMIN', 'OUTSTORAGEORDEREXPORT');
INSERT INTO `sysuserpermissions` VALUES (13632, 'ADMIN', 'OUTSTORAGEORDERCONFIRM');
INSERT INTO `sysuserpermissions` VALUES (13633, 'ADMIN', 'STORAGEMGR');
INSERT INTO `sysuserpermissions` VALUES (13634, 'ADMIN', 'STORAGEREAD');
INSERT INTO `sysuserpermissions` VALUES (13635, 'ADMIN', 'STORAGEDETAILREAD');
INSERT INTO `sysuserpermissions` VALUES (13636, 'ADMIN', 'STORAGEFLOWREAD');
INSERT INTO `sysuserpermissions` VALUES (13637, 'ADMIN', 'STORAGEALLOCATION');
INSERT INTO `sysuserpermissions` VALUES (13638, 'ADMIN', 'STORAGEEXPORT');
INSERT INTO `sysuserpermissions` VALUES (13639, 'ADMIN', 'STORAGEDETAILEXPORT');
INSERT INTO `sysuserpermissions` VALUES (13640, 'ADMIN', 'STORAGEFLOWEXPORT');
INSERT INTO `sysuserpermissions` VALUES (13641, 'ADMIN', 'SUPPLIERMGR');
INSERT INTO `sysuserpermissions` VALUES (13642, 'ADMIN', 'CLIENTSUPPLIERMGR');
INSERT INTO `sysuserpermissions` VALUES (13643, 'ADMIN', 'SUPPLIERREAD');
INSERT INTO `sysuserpermissions` VALUES (13644, 'ADMIN', 'SUPPLIERADD');
INSERT INTO `sysuserpermissions` VALUES (13645, 'ADMIN', 'SUPPLIERUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13646, 'ADMIN', 'SUPPLIERDEL');
INSERT INTO `sysuserpermissions` VALUES (13647, 'ADMIN', 'SUPPLIEREXPORT');
INSERT INTO `sysuserpermissions` VALUES (13648, 'ADMIN', 'FINISHEDPRODUCTMGR');
INSERT INTO `sysuserpermissions` VALUES (13649, 'ADMIN', 'GOODSINFOMGR');
INSERT INTO `sysuserpermissions` VALUES (13650, 'ADMIN', 'FINISHEDPRODUCTREAD');
INSERT INTO `sysuserpermissions` VALUES (13651, 'ADMIN', 'FINISHEDPRODUCTADD');
INSERT INTO `sysuserpermissions` VALUES (13652, 'ADMIN', 'FINISHEDPRODUCTUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13653, 'ADMIN', 'FINISHEDPRODUCTDEL');
INSERT INTO `sysuserpermissions` VALUES (13654, 'ADMIN', 'FINISHEDPRODUCTBOM');
INSERT INTO `sysuserpermissions` VALUES (13655, 'ADMIN', 'FINISHEDPRODUCTEXPORT');
INSERT INTO `sysuserpermissions` VALUES (13656, 'ADMIN', 'FINISHEDPRODUCTTYPEADD');
INSERT INTO `sysuserpermissions` VALUES (13657, 'ADMIN', 'FINISHEDPRODUCTTYPEUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13658, 'ADMIN', 'FINISHEDPRODUCTTYPEDEL');
INSERT INTO `sysuserpermissions` VALUES (13659, 'ADMIN', 'RAWMATERIALMGR');
INSERT INTO `sysuserpermissions` VALUES (13660, 'ADMIN', 'RAWMATERIALREAD');
INSERT INTO `sysuserpermissions` VALUES (13661, 'ADMIN', 'RAWMATERIALADD');
INSERT INTO `sysuserpermissions` VALUES (13662, 'ADMIN', 'RAWMATERIALUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13663, 'ADMIN', 'RAWMATERIALDEL');
INSERT INTO `sysuserpermissions` VALUES (13664, 'ADMIN', 'RAWMATERIALEXPORT');
INSERT INTO `sysuserpermissions` VALUES (13665, 'ADMIN', 'RAWMATERIALTYPEADD');
INSERT INTO `sysuserpermissions` VALUES (13666, 'ADMIN', 'RAWMATERIALTYPEUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13667, 'ADMIN', 'RAWMATERIALTYPEDEL');
INSERT INTO `sysuserpermissions` VALUES (13668, 'ADMIN', 'UNITMGR');
INSERT INTO `sysuserpermissions` VALUES (13669, 'ADMIN', 'UNITREAD');
INSERT INTO `sysuserpermissions` VALUES (13670, 'ADMIN', 'UNITADD');
INSERT INTO `sysuserpermissions` VALUES (13671, 'ADMIN', 'UNITUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13672, 'ADMIN', 'UNITDEL');
INSERT INTO `sysuserpermissions` VALUES (13673, 'ADMIN', 'USERMGR');
INSERT INTO `sysuserpermissions` VALUES (13674, 'ADMIN', 'SYSINFO');
INSERT INTO `sysuserpermissions` VALUES (13675, 'ADMIN', 'USERREAD');
INSERT INTO `sysuserpermissions` VALUES (13676, 'ADMIN', 'USEREXPORT');
INSERT INTO `sysuserpermissions` VALUES (13677, 'ADMIN', 'USERINITPASSWORD');
INSERT INTO `sysuserpermissions` VALUES (13678, 'ADMIN', 'USERUPDATESTATUS');
INSERT INTO `sysuserpermissions` VALUES (13679, 'ADMIN', 'USERUPDATEROLE');
INSERT INTO `sysuserpermissions` VALUES (13680, 'ADMIN', 'ORGANIZATIONMGR');
INSERT INTO `sysuserpermissions` VALUES (13681, 'ADMIN', 'ORGANIZATIONREAD');
INSERT INTO `sysuserpermissions` VALUES (13682, 'ADMIN', 'ROLEADD');
INSERT INTO `sysuserpermissions` VALUES (13683, 'ADMIN', 'ROLEUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13684, 'ADMIN', 'ROLEDEL');
INSERT INTO `sysuserpermissions` VALUES (13685, 'ADMIN', 'DEPTADD');
INSERT INTO `sysuserpermissions` VALUES (13686, 'ADMIN', 'DEPTUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13687, 'ADMIN', 'DEPTDEL');
INSERT INTO `sysuserpermissions` VALUES (13688, 'ADMIN', 'BUSINESSDICMGR');
INSERT INTO `sysuserpermissions` VALUES (13689, 'ADMIN', 'BUSINESSDICREAD');
INSERT INTO `sysuserpermissions` VALUES (13690, 'ADMIN', 'BUSINESSDICADD');
INSERT INTO `sysuserpermissions` VALUES (13691, 'ADMIN', 'BUSINESSDICUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13692, 'ADMIN', 'BUSINESSDICDEL');
INSERT INTO `sysuserpermissions` VALUES (13693, 'ADMIN', 'MENUSMGR');
INSERT INTO `sysuserpermissions` VALUES (13694, 'ADMIN', 'SYSSETUP');
INSERT INTO `sysuserpermissions` VALUES (13695, 'ADMIN', 'MENUSADD');
INSERT INTO `sysuserpermissions` VALUES (13696, 'ADMIN', 'MENUSREAD');
INSERT INTO `sysuserpermissions` VALUES (13697, 'ADMIN', 'MENUSUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13698, 'ADMIN', 'MENUSDEL');
INSERT INTO `sysuserpermissions` VALUES (13699, 'ADMIN', 'PERMISSIONSETUP');
INSERT INTO `sysuserpermissions` VALUES (13700, 'ADMIN', 'PERMISSIONREAD');
INSERT INTO `sysuserpermissions` VALUES (13701, 'ADMIN', 'PERMISSIONUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13702, 'ADMIN', 'SYSARGSSETUP');
INSERT INTO `sysuserpermissions` VALUES (13703, 'ADMIN', 'SYSARGSREAD');
INSERT INTO `sysuserpermissions` VALUES (13704, 'ADMIN', 'SYSARGSUPDATE');
INSERT INTO `sysuserpermissions` VALUES (13705, 'ADMIN', 'LOGINFO');
INSERT INTO `sysuserpermissions` VALUES (13706, 'ADMIN', 'LOGREAD');
INSERT INTO `sysuserpermissions` VALUES (13707, 'ADMIN', 'MESSAGE');
INSERT INTO `sysuserpermissions` VALUES (13708, 'ADMIN', 'PDAMOBILEMGR');
INSERT INTO `sysuserpermissions` VALUES (13709, 'ADMIN', 'MESSAGEREAD');
INSERT INTO `sysuserpermissions` VALUES (13710, 'ADMIN', 'MYSETTING');
INSERT INTO `sysuserpermissions` VALUES (13711, 'ADMIN', 'MYSETTINGREAD');
INSERT INTO `sysuserpermissions` VALUES (13712, 'ADMIN', 'MESSAGESUBSCRIBE');
INSERT INTO `sysuserpermissions` VALUES (13713, 'ADMIN', 'TAKESTOCKMOBILEBYGOODS');
INSERT INTO `sysuserpermissions` VALUES (13714, 'ADMIN', 'TAKESTOCKMOBILEBYGOODSREAD');
INSERT INTO `sysuserpermissions` VALUES (13715, 'ADMIN', 'TAKESTOCKMOBILELOCKGOODS');
INSERT INTO `sysuserpermissions` VALUES (13716, 'ADMIN', 'TAKESTOCKMOBILEBYBIN');
INSERT INTO `sysuserpermissions` VALUES (13717, 'ADMIN', 'TAKESTOCKMOBILEBYBINREAD');
INSERT INTO `sysuserpermissions` VALUES (13718, 'ADMIN', 'TAKESTOCKMOBILELOCKBIN');
INSERT INTO `sysuserpermissions` VALUES (13719, 'ADMIN', 'TAKESTOCKMOBILEINVENTORYBYGOODS');
INSERT INTO `sysuserpermissions` VALUES (13720, 'ADMIN', 'TAKESTOCKMOBILEINVENTORYBYGOODSREAD');
INSERT INTO `sysuserpermissions` VALUES (13721, 'ADMIN', 'TAKESTOCKBYGOODSMOBILEADD');
INSERT INTO `sysuserpermissions` VALUES (13722, 'ADMIN', 'TAKESTOCKMOBILEINVENTORYBYBIN');
INSERT INTO `sysuserpermissions` VALUES (13723, 'ADMIN', 'TAKESTOCKMOBILEINVENTORYBYBINREAD');
INSERT INTO `sysuserpermissions` VALUES (13724, 'ADMIN', 'TAKESTOCKBYBINMOBILEADD');
INSERT INTO `sysuserpermissions` VALUES (13725, 'ADMIN', 'INSTORGEMOBILE');
INSERT INTO `sysuserpermissions` VALUES (13726, 'ADMIN', 'GOODSMREAD');
INSERT INTO `sysuserpermissions` VALUES (13727, 'ADMIN', 'INSTORAGEORDMOBILERADD');
INSERT INTO `sysuserpermissions` VALUES (13728, 'ADMIN', 'INSTORGEMOBILEBTN');
INSERT INTO `sysuserpermissions` VALUES (13729, 'ADMIN', 'INSTORAGEDIT');

-- ----------------------------
-- Table structure for sysuserroles
-- ----------------------------
DROP TABLE IF EXISTS `sysuserroles`;
CREATE TABLE `sysuserroles`  (
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '角色ID',
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '用户ID',
  PRIMARY KEY (`RoleId`, `UserId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '用户角色表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysuserroles
-- ----------------------------
INSERT INTO `sysuserroles` VALUES ('ADMIN', '13');
INSERT INTO `sysuserroles` VALUES ('ADMIN', 'admin');
INSERT INTO `sysuserroles` VALUES ('bf471f560acb4f74a1e3eb7c151bdc9d', '159');
INSERT INTO `sysuserroles` VALUES ('fa5b9b63e17245a080d6ccf6a030c0b5', '21');

-- ----------------------------
-- Table structure for sysvipuserpermissions
-- ----------------------------
DROP TABLE IF EXISTS `sysvipuserpermissions`;
CREATE TABLE `sysvipuserpermissions`  (
  `PermissionId` int(11) NOT NULL AUTO_INCREMENT COMMENT '用户权限ID',
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '用户ID',
  `MenuId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '权限菜单',
  PRIMARY KEY (`PermissionId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = 'Vip用户权限表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysvipuserpermissions
-- ----------------------------

SET FOREIGN_KEY_CHECKS = 1;
