CREATE TABLE `ProdStation`(
`StationNo` varchar(50) NOT NULL  ,
`PlantNo` varchar(50) NOT NULL  ,
`AreaNo` varchar(50) NOT NULL  ,
`LineNo` varchar(50) NOT NULL  ,
`StationName` varchar(50) NOT NULL   , Primary key(`StationNo`));

ALTER TABLE `ProdStation` COMMENT='工位信息';