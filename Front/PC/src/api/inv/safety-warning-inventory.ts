import request from '@/utils/system/request' 
 
export function getWarningInfo(userId:string, pgSize:Number,  pgIndex:Number,  orderField:string,  orderType:string,  searchKey:string,  goodsGroup:string,  goodsClassifyType:number,  status:string) {
return request.get(`/SafetyWarningRecord/GetWarningInfo?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderField=${orderField}&orderType=${orderType}&searchKey=${searchKey}&goodsGroup=${goodsGroup}&goodsClassifyType=${goodsClassifyType}&status=${status}`)
}

export function getArgs() {
return request.get(`/SafetyWarningRecord/GetArgs`);
}
 
export function getOptions() {
return request.get(`/SafetyWarningRecord/GetOptions`);
}
 
export function getSafetyInfoDetail(detailId:number) {
return request.get(`/SafetyWarningRecord/GetSafetyInfoDetail?detailId=${detailId}`);
}

export function getWarningApprovalHis(detailId:number) {
return request.get(`/SafetyWarningRecord/GetWarningApprovalHis?detailId=${detailId}`);
}
 
export function updateWarningInfo(data:any) {
return request.post(`/SafetyWarningRecord/UpdateWarningInfo`,data);
}
 
export function updateReceived(detailId:Array<any>,userId:string,userName:string) {
return request.put(`/SafetyWarningRecord/UpdateReceived?userId=${userId}&userName=${userName}`,detailId);
}

export function approvalSafetyInventory(flowId:Array<any>,isApprove:boolean,opinion:string,goodsClassifyGroup:string, userId:string,userName:string) {
return request.put(`/SafetyWarningRecord/ApprovalSafetyInventory?isApprove=${isApprove}&opinion=${opinion}&goodsClassifyGroup=${goodsClassifyGroup}&userId=${userId}&userName=${userName}`,flowId);
}
  
export function getExportFields() {
return request.get(`/SafetyWarningRecord/GetExportFields`);
}

export function exportWarningInfo( orderField:string, orderType:string, searchKey:string,  goodsGroup:string,goodsClassifyType:number, status:string,  fields:Array<any>) {
return request.put(`/SafetyWarningRecord/ExportWarningInfo?orderField=${orderField}&orderType=${orderType}&searchKey=${searchKey}&goodsGroup=${goodsGroup}&goodsClassifyType=${goodsClassifyType}&status=${status}`,fields);
}
 
  