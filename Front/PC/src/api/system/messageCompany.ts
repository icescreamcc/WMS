import request from '@/utils/system/request'

// 获取所有公告列表
export function getMessages(pgSize:Number,pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string, dateStart:string,  dateEnd:string) {
return request.get(`/MessageCompany/GetMessages?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}`)
}

//查询最新发布的公告明细
export function getNewMessageDetail() {
return request.get(`/MessageCompany/GetNewMessageDetail`)
}
   
//添加(发布企业公告)
export function addMessage(data:any){
return request.post(`/MessageCompany/AddMessage`,data);
}
 
//修改企业公告
export function updateMessage(data:any){
return request.post(`/MessageCompany/UpdateMessage`,data);
}

//删除企业公告
export function delMessage(data:Array<any>){
return request.post(`/MessageCompany/DelMessage`,data);
}
