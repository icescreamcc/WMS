import request from '@/utils/system/request'

// 获取所有菜单,并标识当前角色的权限菜单
export function getPermissionMenus(roleId:string) {
return request.get(`/UserPermission/GetPermissionMenus?roleId=${roleId}`)
}

export function getOrganizationData() {
return request.get(`/UserPermission/GetOrganizationData`)
}
 
//修改角色权限
export function updatePermissions(roleId:string,data:Array<any>){
return request.put(`/UserPermission/UpdatePermissions?roleId=${roleId}`,data);
}
  
