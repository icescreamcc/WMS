CREATE TABLE `SysMigrationReleaseHis`(
`ReleaseNo` int NOT NULL   COMMENT '版本号' ,
`ReleaseDate` datetime NOT NULL   COMMENT '发布时间'  , Primary key(`ReleaseNo`));

ALTER TABLE `SysMigrationReleaseHis` COMMENT='脚本迁移记录';