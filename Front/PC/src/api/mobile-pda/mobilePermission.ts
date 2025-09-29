import request from '@/utils/system/request'

export function getUserMenus(userId:string) {
return request.get(`/PDAPermission/GetUserMenus?userId=${userId}`);
}