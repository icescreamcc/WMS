import request from '@/utils/system/request'

//根据小车唯一码查询已配对的生产交接单信息
export function getProductionOrderInfo(carSoleCode:string) {
return request.get(`/PDAPutout/GetProductionOrderInfo?carSoleCode=${carSoleCode}`);
}

//扫码下架,校验小车唯一码和库位码，校验配对码
export function checkPutout(carSoleCode:string,binNo:string) {
return request.get(`/PDAPutout/CheckPutout?carSoleCode=${carSoleCode}&binNo=${binNo}`);
}
 

//提交下架
export function submitPutout(data:any) {
return request.post(`/PDAPutout/SubmitPutout`,data);
} 