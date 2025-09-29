import request from '@/utils/system/request';

//查询所有配对小车信息
export function getMatchingInfo( pgSize:number,  pgIndex:number) {
return request.get(`/BufferWarehouseDashboard/GetMatchingInfo?pgSize=${pgSize}&pgIndex=${pgIndex}`);
} 

//查询缓存仓库位信息
export function getBufferWarehouseElement() {
return request.get(`/BufferWarehouseDashboard/GetBufferWarehouseElement`)
} 


//根据库位查询存放信息
export function getStockInfoByBin(binId:number) {
return request.get(`/BufferWarehouseDashboard/GetStockInfoByBin?binId=${binId}`);
}