import request from '@/utils/system/request'

// 获取所有仓库列表
export function getWarehouses(userId:string, pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string) {
return request.get(`/WarehouseBin/GetWarehouses?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}`)
} 

//获取仓库表启用的闲置字段信息
export function getEnableSpareFields() {
return request.get(`/WarehouseBin/GetEnableSpareFields`);
}

//获取仓库相关选项的数据
export function getOptions() {
return request.get(`/WarehouseBin/GetOptions`);
}

//查询仓库相关参数
export function getArgs() {
return request.get(`/WarehouseBin/GetArgs`);
}

//获取仓库详细信息
export function getWarehouseBins(warehouseId:string) {
return request.get(`/WarehouseBin/GetWarehouseBins?warehouseId=${warehouseId}`);
}
  
//添加仓库
export function addWarehouseBin(data:any){
return request.post(`/WarehouseBin/AddWarehouseBin`,data);
}

//修改仓库
export function updateWarehouseBin(data:any){
return request.post(`/WarehouseBin/UpdateWarehouseBin`,data);
}

//删除仓库
export function delWarehouseBin(data:Array<any>){
return request.post(`/WarehouseBin/DelWarehouseBin`,data);
}


 
