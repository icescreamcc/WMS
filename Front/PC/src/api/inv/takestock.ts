import request from '@/utils/system/request'

//查询消耗趋势
export function getExpendTrend(classifyGroup:string){
    return request.get(`/TakeStock/GetExpendTrend?classifyGroup=${classifyGroup}`)
}

//获取当前盘点被锁定的物品
export function getTakeStockLockGoods(pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string, typeId:number, searchKey:string) {
return request.get(`/TakeStock/GetTakeStockLockGoods?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&typeId=${typeId}&searchKey=${searchKey}`)
}

// 查询历史盘点记录
export function getTakeStockHis(pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,goodsGroup:string, goodsClassifyId:number, month:number, searchKey:string) {
return request.get(`/TakeStock/GetTakeStockHis?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&goodsGroup=${goodsGroup}&goodsClassifyId=${goodsClassifyId}&month=${month}&searchKey=${searchKey}`)
}

//导出盘点记录
export function exportTakeStockHis(orderField:string,  orderType:string,goodsGroup:string, goodsClassifyId:number, month:number, searchKey:string) {
return request.get(`/TakeStock/ExportTakeStockHis?orderField=${orderField}&orderType=${orderType}&goodsGroup=${goodsGroup}&goodsClassifyId=${goodsClassifyId}&month=${month}&searchKey=${searchKey}`)
}

//获取盘点相关选项参数
export function getTaskStockDetil(flowId:number) {
return request.get(`/TakeStock/GetTaskStockDetil?flowId=${flowId}`);
}

export function getOptions() {
return request.get(`/TakeStock/GetOptions`);
}

//修改盘点盈亏原因分析
export function updateReason(data:any) {
return request.post(`/TakeStock/UpdateReason`,data);
}

//查询所有物品列表
export function getGoodsByKey( pgSize:Number,  pgIndex:Number, orderFiled:string,  orderType:string, goodsGroup:string,  goodsClassifyId:number, warehouseId:string, keyword:string,isTakeStockCurDate:boolean) {
return request.get(`/TakeStock/GetGoodsByKey?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&goodsGroup=${goodsGroup}&goodsClassifyId=${goodsClassifyId}&warehouseId=${warehouseId}&keyword=${keyword}&isTakeStockCurDate=${isTakeStockCurDate}`)
}
   
//查询当前物品是否在盘点中
export function getGoodsTakeStockStatus(goodsId:string) {
return request.get(`/TakeStock/GetGoodsTakeStockStatus?goodsId=${goodsId}`);
}

//盘点锁定物品
export function setTakeStockLockByGoods(goodsId:Array<string>) {
return request.post(`/TakeStock/SetTakeStockLockByGoods`,goodsId);
}

//查询指定物品的存储明细
export function getGoodsInventoryDetail(goodsId:string) {
return request.get(`/TakeStock/GetGoodsInventoryDetail?goodsId=${goodsId}`)
}

//提交盘点任务（按物品盘点）
export function addTaskStockOrderByGoods(data:any) {
return request.post(`/TakeStock/AddTaskStockOrderByGoods`,data);
}
  
// 获取当前盘点被锁定的库位
export function getTakeStockLockInvBin(pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string, searchKey:string) {
return request.get(`/TakeStock/GetTakeStockLockInvBin?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}`)
}

//分页查询货位信息
export function getBins(pgSize:Number,  pgIndex:Number, warehouseId:string, shelfId:string, workbinSpecId:number,goodsGroup:string, orderFiled:string,  orderType:string, keyword:string,isTakeStockCurDate:boolean) {
return request.get(`/TakeStock/GetBins?pgSize=${pgSize}&pgIndex=${pgIndex}&warehouseId=${warehouseId}&shelfId=${shelfId}&workbinSpecId=${workbinSpecId}&goodsGroup=${goodsGroup}&orderFiled=${orderFiled}&orderType=${orderType}&keyword=${keyword}&isTakeStockCurDate=${isTakeStockCurDate}`)
}
 
//查询当前货位是否在盘点中
export function getBinTakeStockStatus(binId:number) {
return request.get(`/TakeStock/GetBinTakeStockStatus?binId=${binId}`);
}

//盘点锁定库位
export function setTakeStockLockByBin(data:any){
return request.post(`/TakeStock/SetTakeStockLockByBin`,data);
}

//查询指定库位存储明细
export function getBinInventoryDetail(binId:number) {
return request.get(`/TakeStock/GetBinInventoryDetail?binId=${binId}`)
}

//提交盘点任务（按货位盘点）
export function addTaskStockOrderByBin(data:any) {
return request.post(`/TakeStock/AddTaskStockOrderByBin`,data);
}