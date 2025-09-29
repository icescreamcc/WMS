import request from '@/utils/system/request'

//根据小车唯一码查询已开始装车的生产交接单信息
export function getProductionOrderInfo(carSoleCode:string) {
return request.get(`/PDAPutaway/GetProductionOrderInfo?carSoleCode=${carSoleCode}`);
}

//扫码上架,校验小车唯一码和库位码
export function checkPutaway(carSoleCode:string,binNo:string) {
return request.get(`/PDAPutaway/CheckPutaway?carSoleCode=${carSoleCode}&binNo=${binNo}`);
}
 

//提交上架
export function submitPutaway(data:any) {
return request.post(`/PDAPutaway/SubmitPutaway`,data);
}
 