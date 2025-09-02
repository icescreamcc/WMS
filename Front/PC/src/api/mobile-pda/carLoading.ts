import request from '@/utils/system/request'

//根据小车唯一码查询已分配小车的交接单信息
export function getProductionOrderInfo(carSoleCode:string) {
return request.get(`/PDACarLoad/GetProductionOrderInfo?carSoleCode=${carSoleCode}`);
}

//根据小车唯一码查询推荐存放的缓存库位
export function getFreeBin(carSoleCode:string) {
return request.get(`/PDACarLoad/GetFreeBin?carSoleCode=${carSoleCode}`);
}

//锁定库位
export function lockBin(data:any) {
return request.post(`/PDACarLoad/LockBin`,data);
}

//确认装车
export function submitCarLoad(data:any) {
return request.post(`/PDACarLoad/SubmitCarLoad`,data);
}