import request from '@/utils/system/request'

// 获取所有供应商列表
export function getSuppliers(userId:string, pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string) {
return request.get(`/Supplier/GetSuppliers?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}`)
}

//获取供应商表启用的闲置字段信息
export function getEnableSpareFields() {
return request.get(`/Supplier/GetEnableSpareFields`);
}

//获取供应商详细信息
export function getSupplierDetail(supplierId:string) {
return request.get(`/Supplier/GetSupplierDetail?supplierId=${supplierId}`);
}

//获取供应商相关选项的数据
export function getOptions() {
return request.get(`/Supplier/GetOptions`);
}
   
//添加供应商
export function addSupplier(data:any){
return request.post(`/Supplier/AddSupplier`,data);
}

//修改供应商
export function updateSupplier(data:any){
return request.post(`/Supplier/UpdateSupplier`,data);
}

//删除供应商
export function delSupplier(data:Array<any>){
return request.post(`/Supplier/DelSupplier`,data);
}

//导出供应商
export function exportSuppliers( searchKey:string, orderField:string,  orderType:string, fields:Array<object>){
return request.put(`/Supplier/ExportSuppliers?orderField=${orderField}&orderType=${orderType}&searchKey=${searchKey}`,fields);
}

//获取当前用户导出供应商列表被允许的字段
export function getAllowField(userId:string="") {
return request.get(`/Supplier/GetAllowField?userId=${userId}`);
}
