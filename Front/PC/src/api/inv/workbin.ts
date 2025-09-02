import request from '@/utils/system/request'

// 获取所有料箱列表
export function getWorkbins( pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string) {
return request.get(`/Workbin/GetWorkbins?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}`)
} 

// 获取料箱单元格信息
export function getWorkbinCells(workbinId:any){
return request.get(`/Workbin/GetWorkbinCells?workbinId=${workbinId}`);
}

// 获取料箱单元格信息
export function getWorkbinCellsByCellNo(cellNo:any){
return request.get(`/Workbin/GetWorkbinCellsByCellNo?cellNo=${cellNo}`);
}

//获取料箱规格列表
export function getWorkbinSpec(){
return request.get(`/Workbin/GetWorkbinSpec`);
}

//获取料箱编辑相关参数
export function getArgs(){
return request.get(`/Workbin/GetArgs`);
}
  
//修改料箱信息
export function updateWorkbin(data:any){
return request.post(`/Workbin/UpdateWorkbin`,data);
}

//添加料箱规格
export function addWorkbinSpeci(data:any){
return request.post(`/Workbin/AddWorkbinSpeci`,data);
}

//修改料箱规格
export function updateWorkbinSpeci(data:Array<any>){
return request.post(`/Workbin/UpdateWorkbinSpeci`,data);
}

//删除料箱规格
export function delWorkbinSpeci(specId:number){
return request.get(`/Workbin/DelWorkbinSpeci?specId=${specId}`);
}
 
