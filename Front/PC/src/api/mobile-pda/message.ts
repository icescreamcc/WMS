import request from '@/utils/system/request'

//根据小车唯一码或交接单码查询交接单信息
export function getMessages(pgSize:number,pgIndex:number,userId:string) {
return request.get(`/PDAMessage/GetMessages?pgSize=${pgSize}&pgIndex=${pgIndex}&userId=${userId}`);
}

//获取未读消息的数量
export function getNotReadMessageCount(userId:string) {
return request.get(`/PDAMessage/GetNotReadMessageCount?userId=${userId}`);
}
 
//将未读消息修改成已读
export function setRead(messageId:Array<any>,userId:string) {
return request.post(`/PDAMessage/SetRead?userId=${userId}`,messageId);
}

//获取订阅消息的类型
export function getSubscribeMessageType(userId:string) {
return request.get(`/PDAMessage/GetSubscribeMessageType?userId=${userId}`);
}

//设置订阅消息类型
export function submitSubscribeMessageType(messageTypes:Array<any>,userId:string) {
return request.post(`/PDAMessage/SubmitSubscribeMessageType?userId=${userId}`,messageTypes);
}