import request from '@/utils/system/request'

//获取商品类型所有数据
export function getFinishedProductClassifyData() {
return request.get(`/FinishedProduct/GetFinishedProductClassifyData`);
}

//添加商品类型
export function addFinishedProductClassify(data:any){
return request.post(`/FinishedProduct/AddFinishedProductClassify`,data);
}

//修改商品类型
export function updateFinishedProductClassify(data:any){
return request.post(`/FinishedProduct/UpdateFinishedProductClassify`,data);
}

//删除商品类型
export function delFinishedProductClassify(typeId:string){
return request.get(`/FinishedProduct/DelFinishedProductClassify?typeId=${typeId}`);
}
 
// 获取所有成品列表
export function getFinishedProductList(userId:string, pgSize:number,  pgIndex:number,  orderFiled:string,  orderType:string, typeId:number, searchKey:string) {
return request.get(`/FinishedProduct/GetFinishedProductList?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&typeId=${typeId}&searchKey=${searchKey}`)
}

//获取成品表启用的闲置字段信息
export function getEnableSpareFields() {
return request.get(`/FinishedProduct/GetEnableSpareFields`);
}

//获取成品详细信息
export function getFinishedProductDetail(sampleId:string) {
return request.get(`/FinishedProduct/GetFinishedProductDetail?sampleId=${sampleId}`);
}

//获取成品相关选项的数据
export function getOptions() {
return request.get(`/FinishedProduct/GetOptions`);
}
   
//添加成品
export function addFinishedProduct(data:any){
return request.post(`/FinishedProduct/AddFinishedProduct`,data);
}

//修改成品
export function updateFinishedProduct(data:any){
return request.post(`/FinishedProduct/UpdateFinishedProduct`,data);
}

//删除成品
export function delFinishedProduct(data:Array<any>){
return request.post(`/FinishedProduct/DelFinishedProduct`,data);
}

//查询BOM
export function getBOMList(goodsId:string){
return request.get(`/FinishedProduct/GetBOMList?goodsId=${goodsId}`);
}

//修改BOM
export function updateBOM(data:any){
return request.post(`/FinishedProduct/UpdateBOM`,data);
}
 
//导出成品
export function exportFinishedProduct( searchKey:string, orderField:string,  orderType:string,goodsClassifyId:number, fields:Array<any>){
return request.put(`/FinishedProduct/ExportFinishedProduct?searchKey=${searchKey}&orderField=${orderField}&orderType=${orderType}&goodsClassifyId=${goodsClassifyId}`,fields);
}

//获取当前用户导出用户列表被允许的字段
export function getAllowField() {
return request.get(`/FinishedProduct/GetAllowField`);
}
