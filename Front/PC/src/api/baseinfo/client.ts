import request from '@/utils/system/request'

// 获取所有客户列表
export function getClients(userId:string, pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string) {
return request.get(`/Client/GetClients?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}`)
}

//获取客户表启用的闲置字段信息
export function getEnableSpareFields() {
return request.get(`/Client/GetEnableSpareFields`);
}

//获取客户详细信息
export function getClientDetail(clientId:string) {
return request.get(`/Client/GetClientDetail?clientId=${clientId}`);
}

//获取客户相关选项的数据
export function getOptions() {
return request.get(`/Client/GetOptions`);
}
   
//添加客户
export function addClient(data:any){
return request.post(`/Client/AddClient`,data);
}

//修改客户
export function updateClient(data:any){
return request.post(`/Client/UpdateClient`,data);
}

//删除客户
export function delClient(data:Array<any>){
return request.post(`/Client/DelClient`,data);
}

//导出客户
export function exportClients( searchKey:string, orderField:string,  orderType:string, fields:Array<object>){
return request.put(`/Client/ExportClients?orderField=${orderField}&orderType=${orderType}&searchKey=${searchKey}`,fields);
}

//获取当前用户导出客户列表被允许的字段
export function getAllowField(userId:string="") {
return request.get(`/Client/GetAllowField?userId=${userId}`);
}
