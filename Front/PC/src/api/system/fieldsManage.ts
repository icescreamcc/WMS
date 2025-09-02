import request from '@/utils/system/request'

// 获取所有表及备用字段信息
export function getTableFieldList() {
return request.get(`/FieldsManage/GetTableFieldList`)
}

//获取指定表字段详细信息
export function getSpareField( fieldsManageId:string) {
return request.get(`/FieldsManage/GetSpareField?fieldsManageId=${fieldsManageId}`)
}
   
//修改表字段信息
export function setSpareField(data:any){
return request.post(`/FieldsManage/SetSpareField`,data);
}
 
