import request from '@/utils/system/request'

// 获取所有表及字段信息
export function getPermissionTbFields() {
return request.get(`/FieldsPermission/GetPermissionTbFields`)
}

//获取指定表字段信息
export function getPermissioField( fieldsManageId:string) {
return request.get(`/FieldsPermission/GetPermissioField?fieldsManageId=${fieldsManageId}`)
}
   
//获取所有角色
export function getRoles() {
return request.get(`/FieldsPermission/GetRoles`)
}

//修改表字段权限
export function setPermissioFieldRole(data:any){
return request.post(`/FieldsPermission/SetPermissioFieldRole`,data);
}
 
