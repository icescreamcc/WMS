import request from '@/utils/system/request'

// 获取所有订单计划列表
export function getOrders(userId:string, pgSize:Number,  pgIndex:Number,  orderField:string,  orderType:string,  searchKey:string) {
return request.get(`/OrderPlan/GetOrders?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderField}&orderType=${orderType}&searchKey=${searchKey}`)
} 
//获取订单计划相关选项及参数数据
export function getOptions() {
return request.get(`/OrderPlan/GetOptions`);
}


  
//添加订单计划
export function addOrderPlan(data:any){
return request.post(`/OrderPlan/AddOrderPlan`,data);
}

//修改订单计划
export function updateOrderPlan(data:any){
return request.post(`/OrderPlan/UpdateOrderPlan`,data);
}

//删除订单计划
export function delOrderPlan(data:Array<string>){
return request.post(`/OrderPlan/DelOrderPlan`,data);
}


export function getApprovalHis(orderNo:string) {
return request.get(`/OrderPlan/GetApprovalHis?orderNo=${orderNo}`);
}

export function updateReceivingAbnormal(detailId:number,receivingAbnormalType:string,receivingAbnormalDesc:string) {
return request.get(`/OrderPlan/UpdateReceivingAbnormal?detailId=${detailId}&receivingAbnormalType=${receivingAbnormalType}&receivingAbnormalDesc=${receivingAbnormalDesc}`);
}

export function updateReceivingUrgency(detailId:number,qty:number) {
return request.get(`/OrderPlan/UpdateReceivingUrgency?detailId=${detailId}&qty=${qty}`);
}

//审批收货单
export function approvalReceivingOrder(data:Array<any>,isApprove:boolean,opinion:string, userId:string,userName:string){
return request.put(`/OrderPlan/ApprovalReceivingOrder?isApprove=${isApprove}&opinion=${opinion}&userId=${userId}&userName=${userName}`,data);
}

export function createImportTemplate() {
return request.get(`/OrderPlan/CreateImportTemplate`);
}
   
export function exportReceivingData(orderFiled:string,  orderType:string,  searchKey:string,  dateStart:string,  dateEnd:string,  goodsGroup:string, data:any) {
return request.put(`/OrderPlan/ExportReceivingData?orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}&goodsGroup=${goodsGroup}`,data);
}

export function getExportFields() {
return request.get(`/OrderPlan/GetExportFields`);
}
 
//获取文件存储信息
export function getBaseFiles(userId:string, pgSize:Number,  pgIndex:Number,  orderField:string,  orderType:string, searchKey:string,orderNo:string,fileInfoType:string) {
    return request.get(`/OrderPlan/GetBaseFiles?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderField}&orderType=${orderType}&searchKey=${searchKey}&orderNo=${orderNo}&fileInfoType=${fileInfoType}`)
}
  