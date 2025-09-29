import request from '@/utils/system/request'

// 获取所有收货计划列表
export function getOrders(userId:string, pgSize:Number,  pgIndex:Number,  orderField:string,  orderType:string,  searchKey:string,dateStart:string,dateEnd:string,goodsGroup:string,receivingLevel:string,createUserName:string,detailStatus:string) {
return request.get(`/ReceivingOrder/GetOrders?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderField}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}&goodsGroup=${goodsGroup}&receivingLevel=${receivingLevel}&createUserName=${createUserName}&detailStatus=${detailStatus}`)
}
 
//查询收货计划相关参数
export function getArgs() {
return request.get(`/ReceivingOrder/GetArgs`);
}

//获取收货计划相关选项及参数数据
export function getOptions() {
return request.get(`/ReceivingOrder/GetOptions`);
}

//获取收货计划详细信息
export function getOrderDetail(orderNo:string,userId:string) {
return request.get(`/ReceivingOrder/GetOrderDetail?orderNo=${orderNo}&userId=${userId}`);
}

//获取优先级
export function getReceivingLevelGroup() {
return request.get(`/ReceivingOrder/GetReceivingLevelGroup`)
}

//获取计划员
export function getCreateUserNameGroup() {
return request.get(`/ReceivingOrder/GetCreateUserNameGroup`)
}

//获取单据状态
export function getDetailStatusGroup() {
return request.get(`/ReceivingOrder/GetDetailStatusGroup`)
}

//获取指定商品的历史采购价目
export function getGoodsPurchasePriceHis(goodsId:string,limit:number) {
return request.get(`/ReceivingOrder/GetGoodsPurchasePriceHis?goodsId=${goodsId}&limit=${limit}`);
}
  
//添加收货计划
export function addReceivingOrder(data:any){
return request.post(`/ReceivingOrder/AddReceivingOrder`,data);
}

//修改收货计划
export function updateReceivingOrder(data:any){
return request.post(`/ReceivingOrder/UpdateReceivingOrder`,data);
}

//删除收货计划
export function delReceivingOrder(data:Array<number>){
return request.post(`/ReceivingOrder/DelReceivingOrder`,data);
}

//发送邮件通知收货
export function adviceReceiving(orderNo:string,data:any){
return request.put(`/ReceivingOrder/AdviceReceiving?orderNo=${orderNo}`,data);
}

//确认收货
export function submitReceived(data:Array<any>){
return request.post(`/ReceivingOrder/SubmitReceived`,data);
}

export function getApprovalHis(orderNo:string) {
return request.get(`/ReceivingOrder/GetApprovalHis?orderNo=${orderNo}`);
}

export function updateReceivingAbnormal(detailId:number,receivingAbnormalType:string,receivingAbnormalDesc:string) {
return request.get(`/ReceivingOrder/UpdateReceivingAbnormal?detailId=${detailId}&receivingAbnormalType=${receivingAbnormalType}&receivingAbnormalDesc=${receivingAbnormalDesc}`);
}

export function updateReceivingUrgency(detailId:number,qty:number) {
return request.get(`/ReceivingOrder/UpdateReceivingUrgency?detailId=${detailId}&qty=${qty}`);
}

//审批收货单
export function approvalReceivingOrder(data:Array<any>,isApprove:boolean,opinion:string, userId:string,userName:string){
return request.put(`/ReceivingOrder/ApprovalReceivingOrder?isApprove=${isApprove}&opinion=${opinion}&userId=${userId}&userName=${userName}`,data);
}

export function createImportTemplate() {
return request.get(`/ReceivingOrder/CreateImportTemplate`);
}
   
export function exportReceivingData(orderFiled:string,  orderType:string,  searchKey:string,  dateStart:string,  dateEnd:string,  goodsGroup:string, data:any) {
return request.put(`/ReceivingOrder/ExportReceivingData?orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}&goodsGroup=${goodsGroup}`,data);
}

export function getExportFields() {
return request.get(`/ReceivingOrder/GetExportFields`);
}
 
//获取文件存储信息
export function getBaseFiles(userId:string, pgSize:Number,  pgIndex:Number,  orderField:string,  orderType:string, searchKey:string,orderNo:string,fileInfoType:string) {
    return request.get(`/ReceivingOrder/GetBaseFiles?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderField}&orderType=${orderType}&searchKey=${searchKey}&orderNo=${orderNo}&fileInfoType=${fileInfoType}`)
}
  