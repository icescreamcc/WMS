import request from '@/utils/system/request'
 
export function swipingCardAuth(cardNo:string) {
return request.get(`/Requisition/SwipingCardAuth?cardNo=${cardNo}`)
}

export function getRequisitionTypes(goodsClassifyGroup:string){
    return request.get(`/Requisition/GetRequisitionTypes?goodsClassifyGroup=${goodsClassifyGroup}`)
}

//查询出库单相关参数
export function getOrderList(userId:any,  goodsClassifyGroup:string,  date:any) {
return request.get(`/Requisition/GetOrderList?userId=${userId}&goodsClassifyGroup=${goodsClassifyGroup}&date=${date}`);
}
// 获取AGV任务列表
export function getAGVList(userId:string, pgSize:Number,  pgIndex:Number,  orderField:string,  orderType:string,  searchKey:string) {
return request.get(`/Requisition/GetAGVList?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderField}&orderType=${orderType}&searchKey=${searchKey}`)
}
//关闭执行中的AVG任务
export function updateAGVisExecuting(data:any){
return request.post(`/Requisition/UpdateAGVisExecuting`,data);
}

//添加领用单
export function addReqisitionOrder(data:any){
return request.post(`/Requisition/AddReqisitionOrder`,data);
}

//修改领用单
export function updateReqisitionOrder(data:any){
return request.post(`/Requisition/UpdateReqisitionOrder`,data);
}

//删除领用单
export function delReqisitionOrder(orderNo:string, detailId:number,userCardNo:string){
return request.get(`/Requisition/DelReqisitionOrder?orderNo=${orderNo}&detailId=${detailId}&userCardNo=${userCardNo}`);
}

//出库收货
export function confirmReceived(data:any){
return request.post(`/Requisition/ConfirmReceived`,data);
}