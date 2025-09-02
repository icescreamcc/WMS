import localSysEn from '@/locale/modules/en/system';
import localSysZh from '@/locale/modules/zh-cn/system';

const showLogo: Boolean = true; // 是否显示Logo顶部模块
const systemTitle = 'message.system.title' // 系统名称，用于显示在左上角模块，以及浏览器标题上使用,使用配置项

const appName='CHU DWES(Changsha Digital Warehouse Execution System)';
const deftClassifyGroup:string=""
const disableClassifyGroupSelect=false;

// const appName='CHU DSPM(Changsha Digital Spare Parts Management)';
// const deftClassifyGroup:string="SparePart"; 
// const disableClassifyGroupSelect=true;



localSysEn.system.title=appName;
localSysZh.system.title=appName;
export {
  systemTitle,
  deftClassifyGroup,
  disableClassifyGroupSelect
}