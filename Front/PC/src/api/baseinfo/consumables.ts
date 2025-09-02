import request from '@/utils/system/request'

//获取耗材类型所有数据
export function getConsumablesClassifyData() {
return request.get(`/Consumables/GetConsumablesClassifyData`);
}
 
//添加耗材类型
export function addConsumablesType(data:any){
return request.post(`/Consumables/AddConsumablesType`,data);
}

//修改耗材类型
export function updateConsumablesType(data:any){
return request.post(`/Consumables/UpdateConsumablesType`,data);
}

//删除耗材类型
export function delConsumablesType(typeId:string){
return request.get(`/Consumables/DelConsumablesType?typeId=${typeId}`);
}

// 获取所有耗材列表
export function getConsumablesList(userId:string, pgSize:number,  pgIndex:number,  orderFiled:string,  orderType:string, goodsClassifyId:number, searchKey:string) {
return request.get(`/Consumables/GetConsumablesList?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&goodsClassifyId=${goodsClassifyId}&searchKey=${searchKey}`)
}
 
//获取耗材表启用的闲置字段信息
export function getEnableSpareFields() {
return request.get(`/Consumables/GetEnableSpareFields`);
}

//获取耗材详细信息
export function getConsumablesDetail(spareId:string) {
return request.get(`/Consumables/GetConsumablesDetail?spareId=${spareId}`);
}

//获取耗材相关选项的数据
export function getOptions() {
return request.get(`/Consumables/GetOptions`);
}
  
//添加耗材
export function addConsumables(data:any){
return request.post(`/Consumables/AddConsumables`,data);
}

//修改耗材
export function updateConsumables(data:any){
return request.post(`/Consumables/UpdateConsumables`,data);
}
 
//删除耗材
export function delConsumables(data:Array<any>){
return request.post(`/Consumables/DelConsumables`,data);
}
 
//导出耗材
export function exportConsumables( searchKey:string, orderField:string,  orderType:string,goodsClassifyId:number, fields:Array<any>){
return request.put(`/Consumables/ExportConsumables?searchKey=${searchKey}&orderField=${orderField}&orderType=${orderType}&goodsClassifyId=${goodsClassifyId}`,fields);
}

//获取当前用户导出用户列表被允许的字段
export function getAllowField() {
return request.get(`/Consumables/GetAllowField`);
}
