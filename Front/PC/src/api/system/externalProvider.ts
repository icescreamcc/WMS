import request from '@/utils/system/request'

// 分页查询账号
export function getProviders(pgSize:Number,pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string) {
return request.get(`/ExternalProvider/GetProviders?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}`)
}
  
//添加账号
export function addProvider(data:any){
return request.post("/ExternalProvider/AddProvider",data);
}

//修改账号
export function updateProvider(data:any){
return request.post("/ExternalProvider/UpdateProvider",data);
}

//删除账号
export function delProvider(providerName:Array<any>){
return request.post(`/ExternalProvider/DelProvider`,providerName);
}
 
