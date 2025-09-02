import request from '@/utils/system/request'

// 查询公司组织架构数据并组建成树形结构模型
export function getOrganizationData() {
return request.get(`/Organization/GetOrganizationData`)
}

//根据组织架构类型查询用户
export function getUsersByOrganizationType( searchId:string,  organizationType:string) {
return request.get(`/Organization/GetUsersByOrganizationType?searchId=${searchId}&organizationType=${organizationType}`)
}

//添加部门
export function addDept(data:any){
return request.post("/Organization/AddDept",data);
}

//修改部门
export function updateDept(data:any){
return request.post("/Organization/UpdateDept",data);
}

//删除部门
export function delDept(deptId:string){
return request.get(`/Organization/DelDept?deptId=${deptId}`);
}

//添加角色
export function addRole(data:any){
return request.post("/Organization/AddRole",data);
}

//修改角色
export function updateRole(data:any){
return request.post("/Organization/UpdateRole",data);
}

//删除角色
export function delRole(roleId:string){
return request.get(`/Organization/DelRole?roleId=${roleId}`);
}

//获取企业信息
export function getCompanyInfo() {
return request.get(`/Organization/GetCompanyInfo`)
}

//修改企业信息
export function updateCompany(data:any){
return request.post("/Organization/UpdateCompany",data);
}

//上传企业LOGO
export function uploadCompanyLogo(){
return request.post("/Organization/UploadCompanyLogo");
}

