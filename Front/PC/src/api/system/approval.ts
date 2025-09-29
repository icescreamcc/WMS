import request from '@/utils/system/request'

// 获取审批项目列表
export function getApprovalSubject() {
return request.get(`/Approval/GetApprovalSubject`)
}

// 获取所有角色信息
export function getRoles() {
return request.get(`/Approval/GetRoles`)
}

//获取审批模式
export function getApprovalModels() {
return request.get(`/Approval/GetApprovalModels`)
}

// 根据审批数据类型获取审批流程
export function getProcess(approvalDataType:string) {
return request.get(`/Approval/GetProcess?approvalDataType=${approvalDataType}`)
}
   
//保存审批设置
export function updateProcess(data:Array<any>){
return request.post(`/Approval/UpdateProcess`,data);
}


 
