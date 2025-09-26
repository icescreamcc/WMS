import request from '@/utils/system/request'

// 获取所有发货计划列表
export function getSending(userId:string, pgSize:Number,  pgIndex:Number,  orderField:string,  orderType:string,  searchKey:string,dateStart:string,dateEnd:string,goodsGroup:string,isUrgentShipment:string,sendingAddress:string,detailStatus:string) {
return request.get(`/SendingOrder/GetSending?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderField}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}&goodsGroup=${goodsGroup}&isUrgentShipment=${isUrgentShipment}&sendingAddress=${sendingAddress}&detailStatus=${detailStatus}`);
}

//获取发货计划相关选项及参数数据
export function getOptions() {
return request.get(`/SendingOrder/GetOptions`);
}

//获取收货计划详细信息
export function getOrderDetail(userId:string,orderNo:string) {
return request.get(`/SendingOrder/GetOrderDetail?userId=${userId}&orderNo=${orderNo}`);
}

//获取是否紧急发货/是否有足够库存
export function getYesOrNoGroup() {
return request.get(`/SendingOrder/GetYesOrNoGroup`)
}

//获取计划员
export function getCreateUserNameGroup() {
return request.get(`/SendingOrder/GetCreateUserNameGroup`)
}
//获取到货地址
export function getSendingAddressGroup () {
return request.get(`/SendingOrder/GetSendingAddressGroup`)
}


//获取单据状态
export function getDetailStatusGroup() {
return request.get(`/SendingOrder/GetDetailStatusGroup`)
}
  
//添加发货计划 点击确认后
export function addSending(data:any){
return request.post(`/SendingOrder/AddSending`,data);
}

//修改发货计划
export function updateSending(data:any){
return request.post(`/SendingOrder/UpdateSending`,data);
}

//删除发货计划
export function delSending(data:Array<number>){
return request.post(`/SendingOrder/DelSending`,data);
}

export function createImportTemplate() {
return request.get(`/SendingOrder/CreateImportTemplate`);
}
   
export function exportSendingData(orderFiled:string,  orderType:string,  searchKey:string,  dateStart:string,  dateEnd:string,  goodsGroup:string, data:any) {
return request.put(`/SendingOrder/ExportSendingData?orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}&goodsGroup=${goodsGroup}`,data);
}

export function getExportFields() {
return request.get(`/SendingOrder/GetExportFields`);
}

//获取文件存储信息
export function getBaseFiles(userId:string, pgSize:Number,  pgIndex:Number,  orderField:string,  orderType:string, searchKey:string,orderNo:string,fileInfoType:string) {
    return request.get(`/SendingOrder/GetBaseFiles?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderField}&orderType=${orderType}&searchKey=${searchKey}&orderNo=${orderNo}&fileInfoType=${fileInfoType}`)
}

//发送邮件通知
export function adviceSending(orderNo:string,data:any){
return request.put(`/SendingOrder/AdviceSending?orderNo=${orderNo}`,data);
}

//扫描二维码 如计划发货和实际不同 修改发货数量  
export function ConfirmSendingAndOutStorage(data:any){
return request.post(`/SendingOrder/ConfirmSendingAndOutStorage`,data);
}