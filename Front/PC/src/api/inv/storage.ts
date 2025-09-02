import request from '@/utils/system/request'

// 库存汇总分页查询
export function getStorageList( pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string,warehouseId:string,goodsGroup:string) {
return request.get(`/Storage/GetStorageList?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&warehouseId=${warehouseId}&goodsGroup=${goodsGroup}`)
}

//库存明细分页查询
export function getStorageDetailList( pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string,warehouseId:string,goodsGroup:string) {
return request.get(`/Storage/GetStorageDetailList?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&warehouseId=${warehouseId}&goodsGroup=${goodsGroup}`)
}
    
//库存流水分页查询
export function getStorageFlowList( goodsId:string, pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string,  flowType:string,  dateStart:string,  dateEnd:string) {
return request.get(`/Storage/GetStorageFlowList?goodsId=${goodsId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&flowType=${flowType}&dateStart=${dateStart}&dateEnd=${dateEnd}`)
}
   
//获取库存相关选项及参数数据
export function getOptions() {
return request.get(`/Storage/GetOptions`);
}

//获取库存详细信息
export function getStorageDetails(goodsId:string) {
return request.get(`/Storage/GetStorageDetails?goodsId=${goodsId}`);
}
  
//库存汇总计算
export function storageStatistics( userId:string,  userName:string){
return request.get(`/Storage/StorageStatistics?userId=${userId}&userName=${userName}`);
}
 
//添加库存调拨
export function addAllocationStorage(data:any){
return request.post(`/Storage/AddAllocationStorage`,data);
}

//库存汇总信息导出
export function exportStorage( orderFiled:string,  orderType:string,  searchKey:string,  warehouseId:string,goodsGroup:string){
return request.get(`/Storage/ExportStorage?orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&warehouseId=${warehouseId}&goodsGroup=${goodsGroup}`);
}

//库存明细信息导出
export function exportStorageDetail( orderFiled:string,  orderType:string,  searchKey:string,  warehouseId:string,goodsGroup:string){
return request.get(`/Storage/ExportStorageDetail?orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&warehouseId=${warehouseId}&goodsGroup=${goodsGroup}`);
}

//库存流水信息导出
export function exportStorageFlow( goodsId:string,  orderFieled:string,  orderType:string,  searchKey:string,  flowType:string,  dateStart:any,  dateEnd:any){
return request.get(`/Storage/ExportStorageFlow?goodsId=${goodsId}&orderFieled=${orderFieled}&orderType=${orderType}&searchKey=${searchKey}&flowType=${flowType}&dateStart=${dateStart}&dateEnd=${dateEnd}`);
}