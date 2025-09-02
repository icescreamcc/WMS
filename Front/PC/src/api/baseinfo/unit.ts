import request from '@/utils/system/request'

// 获取所有参数列表
export function getUnits( pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string) {
return request.get(`/Unit/GetUnits?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}`)
}
   
//获取单位相关选项数据
export function getAboutUnitOptions(){
return request.get(`/Unit/GetAboutUnitOptions`);
}

//添加单位
export function addUnit(data:any){
return request.post(`/Unit/AddUnit`,data);
}

//修改单位
export function updateUnit(data:any){
return request.post(`/Unit/UpdateUnit`,data);
}

//删除单位
export function delUnit(data:Array<any>){
return request.post(`/Unit/DelUnit`,data);
}
 
