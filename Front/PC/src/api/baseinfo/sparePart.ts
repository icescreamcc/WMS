import request from '@/utils/system/request'

//获取备件类型所有数据
export function getSparePartClassifyData() {
return request.get(`/SparePart/GetSparePartClassifyData`);
}
 
//添加备件类型
export function addSparePartType(data:any){
return request.post(`/SparePart/AddSparePartType`,data);
}

//修改备件类型
export function updateSparePartType(data:any){
return request.post(`/SparePart/UpdateSparePartType`,data);
}

//删除备件类型
export function delSparePartType(typeId:string){
return request.get(`/SparePart/DelSparePartType?typeId=${typeId}`);
}

// 获取所有备件列表
export function getSparePartList(userId:string, pgSize:number,  pgIndex:number,  orderFiled:string,  orderType:string, goodsClassifyId:number, searchKey:string) {
return request.get(`/SparePart/GetSparePartList?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&goodsClassifyId=${goodsClassifyId}&searchKey=${searchKey}`)
}
 
//获取备件表启用的闲置字段信息
export function getEnableSpareFields() {
return request.get(`/SparePart/GetEnableSpareFields`);
}

//获取备件详细信息
export function getSparePartDetail(spareId:string) {
return request.get(`/SparePart/GetSparePartDetail?spareId=${spareId}`);
}

//获取备件相关选项的数据
export function getOptions() {
return request.get(`/SparePart/GetOptions`);
}
  
//添加备件
export function addSparePart(data:any){
return request.post(`/SparePart/AddSparePart`,data);
}

//修改备件
export function updateSparePart(data:any){
return request.post(`/SparePart/UpdateSparePart`,data);
}
 
//删除备件
export function delSparePart(data:Array<any>){
return request.post(`/SparePart/DelSparePart`,data);
}
 
//导出备件
export function exportSparePart( searchKey:string, orderField:string,  orderType:string,goodsClassifyId:number, fields:Array<any>){
return request.put(`/SparePart/ExportSparePart?searchKey=${searchKey}&orderField=${orderField}&orderType=${orderType}&goodsClassifyId=${goodsClassifyId}`,fields);
}

//获取当前用户导出用户列表被允许的字段
export function getAllowField() {
return request.get(`/SparePart/GetAllowField`);
}
