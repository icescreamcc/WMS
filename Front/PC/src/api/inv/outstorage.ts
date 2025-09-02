import request from '@/utils/system/request'

// 获取所有出库单列表
export function getOrders(userId:string, pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string,dateStart:string,dateEnd:string,goodsGroup:string) {
return request.get(`/OutStorage/GetOrders?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}&goodsGroup=${goodsGroup}`)
}
    
//查询出库单相关参数
export function getArgs() {
return request.get(`/OutStorage/GetArgs`);
}

//获取出库单相关选项及参数数据
export function getOptions() {
return request.get(`/OutStorage/GetOptions`);
}

//获取出库单详细信息
export function getOrderDetail(userId:string,orderNo:string) {
return request.get(`/OutStorage/GetOrderDetail?userId=${userId}&orderNo=${orderNo}`);
}
  
//添加出库单
export function addOutStorage(data:any){
return request.post(`/OutStorage/AddOutStorage`,data);
}

//修改出库单
export function updateOutStorage(data:any){
return request.post(`/OutStorage/UpdateOutStorage`,data);
}

//删除出库单
export function delOutStorage(data:Array<any>){
return request.post(`/OutStorage/DelOutStorage`,data);
}

//修改实际出库数量
export function updateActualQuantity(data:any){
return request.post(`/OutStorage/UpdateActualQuantity`,data);
}

//出库确认
export function confirmOutStorage(orderNo:string){
return request.get(`/OutStorage/ConfirmOutStorage?orderNo=${orderNo}`);
}
 
//导出出库单
export function exportOutStorage( searchKey:string, orderField:string,  orderType:string,dateStart:string,dateEnd:string, goodsGroup:string){
return request.get(`/OutStorage/ExportOutStorage?orderField=${orderField}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}&goodsGroup=${goodsGroup}`);
}

//获取当前用户导出出库单数据被允许的字段
export function getAllowField(userId:string="") {
return request.get(`/OutStorage/GetAllowField?userId=${userId}`);
}

//审批出库单
export function approvalOutStorage(data:Array<any>,isApprove:boolean,opinion:string, userId:string,userName:string){
return request.put(`/OutStorage/ApprovalOutStorage?isApprove=${isApprove}&opinion=${opinion}&userId=${userId}&userName=${userName}`,data);
}
 