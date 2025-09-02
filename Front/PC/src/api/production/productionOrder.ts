import request from '@/utils/system/request'

// 生产订单分页查询
export function getOrders( pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string) {
return request.get(`/ProductionOrder/GetOrders?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}`)
} 
 
//生产订单明细查询
export function getOrderDetail( deliverNo:string) {
return request.get(`/ProductionOrder/GetOrderDetail?deliverNo=${deliverNo}`)
} 

//获取生产订单编辑所需的选项数据
export function getOptions() {
return request.get(`/ProductionOrder/GetOptions`)
} 
  
//添加生产订单
export function addOrder(data:any){
return request.post(`/ProductionOrder/AddOrder`,data);
}

//修改生产订单
export function updateOrder(data:any){
return request.post(`/ProductionOrder/UpdateOrder`,data);
}

//删除生产订单
export function delOrder(data:Array<any>){
return request.post(`/ProductionOrder/DelOrder`,data);
}

//创建小车唯一码
export function createCarCode(orderId:number){
return request.get(`/ProductionOrder/CreateCarCode?orderId=${orderId}`);
}

//修改生产订单打印日期
export function updatePrintDate(orderId:number){
return request.get(`/ProductionOrder/UpdatePrintDate?orderId=${orderId}`);
}

//关闭生产订单
export function updateOrderClosed(deliverNo:string,userName:string){
return request.get(`/ProductionOrder/UpdateOrderClosed?deliverNo=${deliverNo}&userName=${userName}`);
}

 
