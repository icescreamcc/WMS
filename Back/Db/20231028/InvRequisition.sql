CREATE TABLE `InvRequisitionOrder`(
`OrderNo` varchar(50) NOT NULL   COMMENT '单据编号' ,
`OrderType` varchar(50) DEFAULT NULL   COMMENT '单据类型' ,
`GoodsClassifyGroup` varchar(50) NOT NULL   COMMENT '物品类型' ,
`Line` varchar(100) DEFAULT NULL   COMMENT '产线' ,
`Purpose` varchar(50) DEFAULT NULL   COMMENT '领用目的' ,
`Remark` varchar(200) DEFAULT NULL   COMMENT '备注' ,
`Status` varchar(50) DEFAULT NULL   COMMENT '状态' ,
`UpdateUserId` varchar(50) DEFAULT NULL   COMMENT '上次修改人ID' ,
`UpdateUserName` varchar(50) DEFAULT NULL   COMMENT '上次修改人姓名' ,
`UpdateDate` datetime NOT NULL   COMMENT '上次修改时间' ,
`CreateUserId` varchar(50) DEFAULT NULL   COMMENT '创建人ID' ,
`CreateUserName` varchar(50) DEFAULT NULL   COMMENT '创建人姓名' ,
`CreateDate` datetime NOT NULL   COMMENT '创建时间'  , Primary key(`OrderNo`));
  

CREATE TABLE `InvRequisitionOrderDetail`  (
  `DetailId` int NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `OrderNo` varchar(50) CHARACTER SET utf8mb4  NOT NULL COMMENT '单据编号',
  `GoodsId` varchar(50) CHARACTER SET utf8mb4  NOT NULL COMMENT '物品ID',
  `GoodsName` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '物品名称',
  `Quantity` float NOT NULL COMMENT '计划领用数量',
  `ActualQuantity` float NOT NULL COMMENT '实际领用数量',
  `UnitId` int NOT NULL COMMENT '单位ID(计划)',
  `UnitName` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '单位(计划)',
  `ActualUnitId` int NOT NULL COMMENT '单位ID(实际)',
  `ActualUnitName` varchar(50) CHARACTER SET utf8mb4  NULL DEFAULT NULL COMMENT '单位(实际)',
  PRIMARY KEY (`DetailId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4  COMMENT = '物品领用单明细' ROW_FORMAT = Dynamic;
 
 