import request from '@/utils/system/request'

//获取商品类型所有数据
export function getPackingMaterialClassifyData() {
return request.get(`/PackingMaterial/GetPackingMaterialClassifyData`);
}

//添加商品类型
export function addPackingMaterialClassify(data:any){
return request.post(`/PackingMaterial/AddPackingMaterialClassify`,data);
}

//修改商品类型
export function updatePackingMaterialClassify(data:any){
return request.post(`/PackingMaterial/UpdatePackingMaterialClassify`,data);
}

//删除商品类型
export function delPackingMaterialClassify(typeId:string){
return request.get(`/PackingMaterial/DelPackingMaterialClassify?typeId=${typeId}`);
}
 
// 获取所有包材列表
export function getPackingMaterialList(userId:string, pgSize:number,  pgIndex:number,  orderFiled:string,  orderType:string, typeId:number, searchKey:string) {
return request.get(`/PackingMaterial/GetPackingMaterialList?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&typeId=${typeId}&searchKey=${searchKey}`)
}

//获取包材表启用的闲置字段信息
export function getEnableSpareFields() {
return request.get(`/PackingMaterial/GetEnableSpareFields`);
}

//获取包材详细信息
export function getPackingMaterialDetail(sampleId:string) {
return request.get(`/PackingMaterial/GetPackingMaterialDetail?sampleId=${sampleId}`);
}

//获取包材相关选项的数据
export function getOptions() {
return request.get(`/PackingMaterial/GetOptions`);
}
   
//添加包材
export function addPackingMaterial(data:any){
return request.post(`/PackingMaterial/AddPackingMaterial`,data);
}

//修改包材
export function updatePackingMaterial(data:any){
return request.post(`/PackingMaterial/UpdatePackingMaterial`,data);
}

//删除包材
export function delPackingMaterial(data:Array<any>){
return request.post(`/PackingMaterial/DelPackingMaterial`,data);
}

 
//导出包材
export function exportPackingMaterial( searchKey:string, orderField:string,  orderType:string,goodsClassifyId:number, fields:Array<any>){
return request.put(`/PackingMaterial/ExportPackingMaterial?searchKey=${searchKey}&orderField=${orderField}&orderType=${orderType}&goodsClassifyId=${goodsClassifyId}`,fields);
}

//获取当前用户导出用户列表被允许的字段
export function getAllowField() {
return request.get(`/PackingMaterial/GetAllowField`);
}
