import request from '@/utils/system/request'

//获取商品类型所有数据
export function getRawMaterialClassifyData() {
return request.get(`/RawMaterial/GetRawMaterialClassifyData`);
}

//添加商品类型
export function addRawMaterialClassify(data:any){
return request.post(`/RawMaterial/AddRawMaterialClassify`,data);
}

//修改商品类型
export function updateRawMaterialClassify(data:any){
return request.post(`/RawMaterial/UpdateRawMaterialClassify`,data);
}

//删除商品类型
export function delRawMaterialClassify(typeId:string){
return request.get(`/RawMaterial/DelRawMaterialClassify?typeId=${typeId}`);
}
 
// 获取所有包材列表
export function getRawMaterialList(userId:string, pgSize:number,  pgIndex:number,  orderFiled:string,  orderType:string, typeId:number, searchKey:string) {
return request.get(`/RawMaterial/GetRawMaterialList?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&typeId=${typeId}&searchKey=${searchKey}`)
}

//获取包材表启用的闲置字段信息
export function getEnableSpareFields() {
return request.get(`/RawMaterial/GetEnableSpareFields`);
}

//获取包材详细信息
export function getRawMaterialDetail(sampleId:string) {
return request.get(`/RawMaterial/GetRawMaterialDetail?sampleId=${sampleId}`);
}

//获取包材相关选项的数据
export function getOptions() {
return request.get(`/RawMaterial/GetOptions`);
}
   
//添加包材
export function addRawMaterial(data:any){
return request.post(`/RawMaterial/AddRawMaterial`,data);
}

//修改包材
export function updateRawMaterial(data:any){
return request.post(`/RawMaterial/UpdateRawMaterial`,data);
}

//删除包材
export function delRawMaterial(data:Array<any>){
return request.post(`/RawMaterial/DelRawMaterial`,data);
}

 
//导出包材
export function exportRawMaterial( searchKey:string, orderField:string,  orderType:string,goodsClassifyId:number, fields:Array<any>){
return request.put(`/RawMaterial/ExportRawMaterial?searchKey=${searchKey}&orderField=${orderField}&orderType=${orderType}&goodsClassifyId=${goodsClassifyId}`,fields);
}

//获取当前用户导出用户列表被允许的字段
export function getAllowField() {
return request.get(`/RawMaterial/GetAllowField`);
}
