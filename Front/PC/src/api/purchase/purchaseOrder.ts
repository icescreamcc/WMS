import request from '@/utils/system/request'

// 获取所有采购订单列表
export function getOrders(userId:string, pgSize:Number,  pgIndex:Number,  orderField:string,  orderType:string,  searchKey:string,dateStart:string,dateEnd:string, purchaseTypeId:number,  status:string,  goodsClassifyGroup:string,  goodsClassifyId:number) {
return request.get(`/PurchaseOrder/GetOrders?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderField}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}&purchaseTypeId=${purchaseTypeId}&status=${status}&goodsClassifyGroup=${goodsClassifyGroup}&goodsClassifyId=${goodsClassifyId}`)
}
 
//查询采购订单相关参数
export function getArgs() {
return request.get(`/PurchaseOrder/GetArgs`);
}

//获取采购订单相关选项及参数数据
export function getOptions() {
return request.get(`/PurchaseOrder/GetOptions`);
}

//查询采购订单相关参数
export function isLastApproval(userId:string) {
return request.get(`/PurchaseOrder/IsLastApproval?userId=${userId}`);
}
 
//获取采购订单详细信息
export function getOrderDetail(detailId:number,userId:string) {
return request.get(`/PurchaseOrder/GetOrderDetail?detailId=${detailId}&userId=${userId}`);
} 
  
//添加采购订单
export function addPurchaseOrder(data:any){
return request.post(`/PurchaseOrder/AddPurchaseOrder`,data);
}

//修改采购订单
export function updatePurchaseOrder(data:any){
return request.post(`/PurchaseOrder/UpdatePurchaseOrder`,data);
}

//审批时修改采购订单
export function updateApprovalPurchaseOrder(data:any){
return request.post(`/PurchaseOrder/UpdateApprovalPurchaseOrder`,data);
}

//删除采购订单
export function delPurchaseOrder(data:Array<number>){
return request.post(`/PurchaseOrder/DelPurchaseOrder`,data);
}

//发送邮件通知收货
export function adviceReceiving(orderNo:string,data:any){
return request.put(`/PurchaseOrder/AdviceReceiving?orderNo=${orderNo}`,data);
}

//确认收货
export function submitReceivedWH(data:Array<number>){
return request.post(`/PurchaseOrder/SubmitReceivedWH`,data);
}

export function submitReceivedWK(data:Array<number>){
return request.post(`/PurchaseOrder/SubmitReceivedWK`,data);
}

export function updateFlowStatus(detailsId:number,  status:string) {
return request.get(`/PurchaseOrder/UpdateFlowStatus?detailsId=${detailsId}&status=${status}`);
}

export function getApprovalHis(orderNo:string) {
return request.get(`/PurchaseOrder/GetApprovalHis?orderNo=${orderNo}`);
}
 
//审批收货单
export function approvalPurchaseOrder(data:any){
return request.post(`/PurchaseOrder/ApprovalPurchaseOrder`,data);
}
  
export function exportPurchaseData(orderFiled:string,  orderType:string,  searchKey:string,  dateStart:string,  dateEnd:string,  purchaseTypeId:number,  flowStatus:string,  goodsClassifyGroup:string,  goodsClassifyId:number, data:any) {
return request.put(`/PurchaseOrder/ExportPurchaseData?orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}&purchaseTypeId=${purchaseTypeId}&flowStatus=${flowStatus}&goodsClassifyGroup=${goodsClassifyGroup}&goodsClassifyId=${goodsClassifyId}`,data);
}

export function getExportFields() {
return request.get(`/PurchaseOrder/GetExportFields`);
}
 
  