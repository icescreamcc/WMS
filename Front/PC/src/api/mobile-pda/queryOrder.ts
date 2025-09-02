import request from '@/utils/system/request'

//根据小车唯一码或交接单码查询交接单信息
export function getProductionOrderByCarCode(carSoleCode:string) {
return request.get(`/PDAOrderQuery/GetProductionOrderByCarCode?carSoleCode=${carSoleCode}`);
}
 
//解锁库位
export function unlockBin(binNo:string,userName:string) {
return request.get(`/PDAOrderQuery/UnlockBin?binNo=${binNo}&userName=${userName}`);
}