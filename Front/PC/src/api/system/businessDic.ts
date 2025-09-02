import request from '@/utils/system/request'

// 获取所有字典列表
export function getArgs(searchKey:string) {
return request.get(`/BusinessDic/GetArgs?searchKey=${searchKey}`)
}
   
//添加字典
export function addArgs(data:any){
return request.post(`/BusinessDic/AddArgs`,data);
}
 
//修改字典
export function updateArgs(data:any){
return request.post(`/BusinessDic/UpdateArgs`,data);
}

//删除字典
export function delArgs(argsKey:string){
return request.get(`/BusinessDic/DelArgs?argsKey=${argsKey}`);
}
