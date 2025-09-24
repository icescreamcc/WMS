import request from '@/utils/system/request'

// 获取所有入库单列表
export function getOrders(userId:string, pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string,dateStart:string,dateEnd:string,goodsGroup:string) {
return request.get(`/InStorage/GetOrders?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}&goodsGroup=${goodsGroup}`)
}
    
//查询入库单相关参数
export function getArgs() {
return request.get(`/InStorage/GetArgs`);
}

//获取入库单相关选项及参数数据
export function getOptions() {
return request.get(`/InStorage/GetOptions`);

}
//获取入库单详细信息
export function getOrderDetail(userId:string,orderNo:string) {
return request.get(`/InStorage/GetOrderDetail?userId=${userId}&orderNo=${orderNo}`);
}
  
//根据物品查询推荐入库料箱
export function getWorkbinRecommend(goodsId:string) {
return request.get(`/InStorage/GetWorkbinRecommend?goodsId=${goodsId}`);
}

//查询指定仓库和规格的货架、货位
export function getBinRecommend(warehouseId:string,specId:number) {
return request.get(`/InStorage/GetBinRecommend?warehouseId=${warehouseId}&specId=${specId}`);
}

//添加入库单
export function addInStorage(data:any){
return request.post(`/InStorage/AddInStorage`,data);
}

//修改入库单
export function updateInStorage(data:any){
return request.post(`/InStorage/UpdateInStorage`,data);
}

//删除入库单
export function delInStorage(data:Array<any>){
return request.post(`/InStorage/DelInStorage`,data);
}

//入库确认
export function confirmInStorage(orderNo:string){
return request.get(`/InStorage/ConfirmInStorage?orderNo=${orderNo}`);
}

export function agvScheduling(orderNo:string){
return request.get(`/InStorage/AGVScheduling?orderNo=${orderNo}`);
}
 
//导出入库单
export function exportInStorage( searchKey:string, orderField:string,  orderType:string,dateStart:string,dateEnd:string, goodsGroup:string){
return request.get(`/InStorage/ExportInStorage?orderField=${orderField}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}&goodsGroup=${goodsGroup}`);
}

//获取当前用户导出入库单数据被允许的字段
export function getAllowField(userId:string="") {
return request.get(`/InStorage/GetAllowField?userId=${userId}`);
}

//审批入库单
export function approvalInStorage(data:Array<any>,isApprove:boolean,opinion:string, userId:string,userName:string){
return request.put(`/InStorage/ApprovalInStorage?isApprove=${isApprove}&opinion=${opinion}&userId=${userId}&userName=${userName}`,data);
}