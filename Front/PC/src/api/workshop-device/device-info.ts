import request from '@/utils/system/request'
 
export function getDeviceInfo( pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string, warehouseId:string, searchKey:string) {
return request.get(`/WorkShopDeviceInfo/GetDeviceInfo?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&warehouseId=${warehouseId}&searchKey=${searchKey}`)
} 

export function getDeviceInfoDetail(deviceId:number){
return request.get(`/WorkShopDeviceInfo/GetDeviceInfoDetail?deviceId=${deviceId}`);
}

export function getDeviceInfoOptions(deviceType:string){
return request.get(`/WorkShopDeviceInfo/GetDeviceInfoOptions?deviceType=${deviceType}`);
}
 
export function addDeviceInfo(data:any){
return request.post(`/WorkShopDeviceInfo/AddDeviceInfo`,data);
}
 
export function updateDeviceInfo(data:Array<any>){
return request.post(`/WorkShopDeviceInfo/UpdateDeviceInfo`,data);
}

export function delDeviceInfo(data:Array<number>){
return request.post(`/WorkShopDeviceInfo/DelDeviceInfo`,data);
}
  
export function getDeviceBin( pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string, deviceType:string, searchKey:string) {
return request.get(`/WorkShopDeviceInfo/GetDeviceBin?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&deviceType=${deviceType}&searchKey=${searchKey}`)
} 
 
export function getArgs(){
return request.get(`/WorkShopDeviceInfo/GetArgs`);
}
 
export function getDeviceBinDetail(binId:number){
return request.get(`/WorkShopDeviceInfo/GetDeviceBinDetail?binId=${binId}`);
}
     
export function addDeviceBin(data:any){
return request.post(`/WorkShopDeviceInfo/AddDeviceBin`,data);
}
 
export function updateDeviceBin(data:Array<any>){
return request.post(`/WorkShopDeviceInfo/UpdateDeviceBin`,data);
}
 
export function delDeviceBin(binsId:Array<number>){
return request.post(`/WorkShopDeviceInfo/DelDeviceBin`,binsId);
}
 
