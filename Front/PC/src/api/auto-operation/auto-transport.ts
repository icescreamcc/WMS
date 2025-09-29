import request from '@/utils/system/request'
 
export function connectAllPLC(data:any){
return request.post(`/AutoTransport/ConnectAllPLC`,data);
}

export function closeAllPLC(data:any){
return request.post(`/AutoTransport/CloseAllPLC`,data);
}

export function getDeviceState(deviceNo:string){
return request.get(`/AutoTransport/GetDeviceState?deviceNo=${deviceNo}`);
}

export function getHisTask(orderNo:string){
return request.get(`/AutoTransport/GetHisTask?orderNo=${orderNo}`);
}


export function isAllowAutoTransport(){
return request.get(`/AutoTransport/IsAllowAutoTransport`);
}

export function sendDeviceRollOut(deviceNo:string){
return request.get(`/AutoTransport/SendDeviceRollOut?deviceNo=${deviceNo}`);
}

export function sendDeviceRollIn(deviceNo:string){
return request.get(`/AutoTransport/SendDeviceRollIn?deviceNo=${deviceNo}`);
}
  
export function getAutoTransportInfrastructureInfo(goodsClassifyGroup:string){
return request.get(`/AutoTransport/GetAutoTransportInfrastructureInfo?goodsClassifyGroup=${goodsClassifyGroup}`);
}
 
export function executeTransTask(taskId:number){
return request.get(`/AutoTransport/ExecuteTransTask?taskId=${taskId}`);
}

export function getPendingExecTransTask(goodsClassifyGroup:string,userId:string){
return request.get(`/AutoTransport/GetPendingExecTransTask?goodsClassifyGroup=${goodsClassifyGroup}&userId=${userId}`);
}
 
 