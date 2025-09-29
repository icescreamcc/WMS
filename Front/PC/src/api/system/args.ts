import request from '@/utils/system/request'

// 获取所有参数列表
export function getArgs() {
return request.get(`/SysArgs/GetArgs`)
}
   
//修改参数
export function updateArgs(data:Array<any>){
return request.post(`/SysArgs/UpdateArgs`,data);
}
 
