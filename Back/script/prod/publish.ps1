echo Please use administrator privileges to run.
#请使用管理员权限执行

############################!!!!此处根据实际目录需修改！！！###############################
#站点名称
$name="auto-IWMS-api"
#网站源文件物理路径 根据不同地址进行相应修改
$physicalPath="D:\web-sites\IWMS\api"
#绑定域名和端口号
$domain="*:8030"
############################end###############################


# 项目根目录
$appRoot="src"
# 备份文件夹
$backupDirectory="backup"

#按小时备份之前的内容
#创建备份目录
$newBackup="{0:yyyy-MM-dd-HH}" -f (get-date)   

echo  $physicalPath\$backupDirectory\$newBackup

if (Test-Path  $physicalPath\$backupDirectory\$newBackup) {
    echo "Delete Folder :  $physicalPath\$backupDirectory\$newBackup"
    Remove-Item -Recurse -Force  $physicalPath\$backupDirectory\$newBackup
}
mkdir  $physicalPath\$backupDirectory\$newBackup



# 停止服务
C:/Windows/system32/Inetsrv/APPCMD.exe stop site /site.name:$name
#删除同名网站
C:/Windows/system32/Inetsrv/APPCMD.exe delete site /site.name:$name


if (Test-Path $physicalPath\$appRoot) {
    echo "backup app Folder : $newBackup"
    Copy-Item $physicalPath\$appRoot\*  $physicalPath\$backupDirectory\$newBackup  -Recurse -Force
}else {
    mkdir  $physicalPath\$appRoot
}


if ( Test-Path $physicalPath\$appRoot\Files) {
    echo "Exist Files Folder :  $physicalPath\$appRoot\Files"
}else{
    New-Item  $physicalPath\$appRoot\Files  -ItemType "directory"
}

# 复制编译文件到发布目录
Copy-Item  .\Code\publish\* -Destination  $physicalPath\$appRoot -Recurse -Force


#设置权限
echo Y|cacls $physicalPath\$appRoot /T /G Everyone:F

#删除同名程序池
C:/Windows/system32/Inetsrv/APPCMD.exe delete AppPool /AppPool.name:$name
#添加程序池
C:/Windows/system32/Inetsrv/APPCMD.exe add AppPool /name:$name
#添加网站
C:/Windows/system32/Inetsrv/APPCMD.exe add site /name:$name /physicalPath:$physicalPath\$appRoot /bindings:"http/${domain}:"
#设置程序池
C:/Windows/system32/Inetsrv/APPCMD.exe set app "$name/"  /applicationPool:$name

