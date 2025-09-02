import request from '@/utils/system/request'

//根据小车唯一码查询已下架的生产交接单信息
export function getProductionOrderInfo(carSoleCode:string) {
return request.get(`/PDAUnstack/GetProductionOrderInfo?carSoleCode=${carSoleCode}`);
}

//上架拆垛机检查
export function checkOnUnstack(carSoleCode:string,binNo:string) {
return request.get(`/PDAUnstack/CheckOnUnstack?carSoleCode=${carSoleCode}&binNo=${binNo}`);
}
 

//上架拆垛机提交
export function submitOnUnStack(data:any) {
return request.post(`/PDAUnstack/SubmitOnUnStack`,data);
} 