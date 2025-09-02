import request from '@/utils/system/request'
import { int } from '@zxing/library/esm/customTypings';
 
export function getPlanOrders(userId:string, pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string,year:number,week:number,goodsGroup:string,status:string) {
return request.get(`/MaterialRequirementPlan/GetPlanOrders?pgSize=${pgSize}&pgIndex=${pgIndex}&orderField=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&year=${year}&week=${week}&goodsGroup=${goodsGroup}&status=${status}`)
}
 
export function getOptions() {
return request.get(`/MaterialRequirementPlan/GetOptions`);
}
 
export function getPlanOrderDetails(orderNo:string) {
return request.get(`/MaterialRequirementPlan/GetPlanOrderDetails?orderNo=${orderNo}`);
}
 
export function getOrderMailInfo(orderNo:string) {
return request.get(`/MaterialRequirementPlan/GetOrderMailInfo?orderNo=${orderNo}`);
}

export function getProductionInfoByReqPlanOrder(year:number, week:number, version:number) {
return request.get(`/MaterialRequirementPlan/GetProductionInfoByReqPlanOrder?year=${year}&week=${week}&version=${version}`);
}
   
export function addPlanOrder(data:any){
return request.post(`/MaterialRequirementPlan/AddPlanOrder`,data);
}
 
export function updatePlanOrder(data:any){
return request.post(`/MaterialRequirementPlan/UpdatePlanOrder`,data);
}
 
export function delPlanOrder(data:Array<number>){
return request.post(`/MaterialRequirementPlan/DelPlanOrder`,data);
}
  
export function getExportFields() {
return request.get(`/MaterialRequirementPlan/GetExportFields`);
}

export function exportPlanOrder( orderField:string, orderType:string, searchKey:string, year:number,week:number,  goodsGroup:string,status:string,  fields:Array<any>) {
return request.put(`/MaterialRequirementPlan/ExportPlanOrder?orderField=${orderField}&orderType=${orderType}&searchKey=${searchKey}&year=${year}&week=${week}&goodsGroup=${goodsGroup}&status=${status}`,fields);
}

export function createAttachment(orderNo:string){
return request.get(`/MaterialRequirementPlan/CreateAttachment?orderNo=${orderNo}`);
}


export function sendMailToSupplier(data:any){
return request.post(`/MaterialRequirementPlan/SendMailToSupplier`,data);
}
 
  