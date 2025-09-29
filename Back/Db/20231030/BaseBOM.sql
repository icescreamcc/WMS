CREATE TABLE `BaseBOM`(
`MaterialId` varchar(50) NOT NULL   COMMENT '物品ID' ,
`ParentId` varchar(50) NOT NULL   COMMENT '父级ID' ,
`MaterialName` varchar(100) DEFAULT NULL   COMMENT '物品名称' ,
`MaterialClassifyGroup` varchar(100) DEFAULT NULL   COMMENT '物品分类' ,
`Quantity` float NOT NULL   COMMENT '所需数量' ,
`Unit` varchar(255) NOT NULL   COMMENT '单位'  , Primary key(`MaterialId`,`ParentId`));

ALTER TABLE `BaseBOM` COMMENT='BOM信息表';