import request from '@/utils/system/request'
 
export function getAGVInfo( pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string, agvType:string, searchKey:string) {
return request.get(`/AGVSetting/GetAGVInfo?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&agvType=${agvType}&searchKey=${searchKey}`)
} 

export function getAGVInfoDetail(agvId:number){
return request.get(`/AGVSetting/GetAGVInfoDetail?agvId=${agvId}`);
}
  
export function addAGVInfo(data:any){
return request.post(`/AGVSetting/AddAGVInfo`,data);
}
 
export function updateAGVInfo(data:Array<any>){
return request.post(`/AGVSetting/UpdateAGVInfo`,data);
}

export function delAGVInfo(agvId:number){
return request.get(`/AGVSetting/DelAGVInfo?agvId=${agvId}`);
}
    
export function getArgs(){
return request.get(`/AGVSetting/GetArgs`);
}