import request from '@/utils/system/request'

//获取商品类型所有数据
export function getSeparatorClassifyData() {
return request.get(`/Separator/GetSeparatorClassifyData`);
}

//添加商品类型
export function addSeparatorClassify(data:any){
return request.post(`/Separator/AddSeparatorClassify`,data);
}

//修改商品类型
export function updateSeparatorClassify(data:any){
return request.post(`/Separator/UpdateSeparatorClassify`,data);
}

//删除商品类型
export function delSeparatorClassify(typeId:string){
return request.get(`/Separator/DelSeparatorClassify?typeId=${typeId}`);
}
 
// 获取所有辅材列表
export function getSeparatorList(userId:string, pgSize:number,  pgIndex:number,  orderFiled:string,  orderType:string, typeId:number, searchKey:string) {
return request.get(`/Separator/GetSeparatorList?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&typeId=${typeId}&searchKey=${searchKey}`)
}

//获取辅材表启用的闲置字段信息
export function getEnableSpareFields() {
return request.get(`/Separator/GetEnableSpareFields`);
}

//获取辅材详细信息
export function getSeparatorDetail(sampleId:string) {
return request.get(`/Separator/GetSeparatorDetail?sampleId=${sampleId}`);
}

//获取辅材相关选项的数据
export function getOptions() {
return request.get(`/Separator/GetOptions`);
}
   
//添加辅材
export function addSeparator(data:any){
return request.post(`/Separator/AddSeparator`,data);
}

//修改辅材
export function updateSeparator(data:any){
return request.post(`/Separator/UpdateSeparator`,data);
}

//删除辅材
export function delSeparator(data:Array<any>){
return request.post(`/Separator/DelSeparator`,data);
}

 
//导出辅材
export function exportSeparator( searchKey:string, orderField:string,  orderType:string,goodsClassifyId:number, fields:Array<any>){
return request.put(`/Separator/ExportSeparator?searchKey=${searchKey}&orderField=${orderField}&orderType=${orderType}&goodsClassifyId=${goodsClassifyId}`,fields);
}

//获取当前用户导出用户列表被允许的字段
export function getAllowField() {
return request.get(`/Separator/GetAllowField`);
}
