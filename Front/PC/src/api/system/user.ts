import request from '@/utils/system/request'

 // 登录 
export function loginApi(data:object){
  return request.post("/Auth/Login",data);
} 

//casSever获取ticket的url地址
export function getTicketUrl(){
  return request.get("/Auth/GetTicketUrl");
}
 
//从case中获取客户端windows登录账户
export function getWindowsUser(ticket: string) {
  return request.get(`/Auth/GetWindowsUser?ticket=${ticket}`)
}

// 退出登录
export function loginOut(userId:string) {
  return request.get(`/Auth/LoginOut?userId=${userId}`)
}

// 获取用户权限菜单
export function getUserMenus(userId: string) {
  return request.get(`/User/GetUserMenus?userId=${userId}`)
}

// 获取用户详细信息
export function getUserDetail(userId: string) {
  return request.get(`/User/GetUserDetail?userId=${userId}`)
}

// 获取用户角色
export function getUserRoles(userId: string) {
  return request.get(`/User/GetUserRoles?userId=${userId}`)
}
 
// 修改用户角色
export function updateUserRole(userId:string,roleId: Array<string>) {
  return request.put(`/User/UpdateUserRole?userId=${userId}`,roleId);
}

// 修改用户密码
export function updateUserPassword(data: object) {
  return request.post("/User/UpdateUserPassword",data);
}

// 初始化用户密码
export function initUserPassword(userId:string) {
  return request.get(`/User/InitUserPassword?userId=${userId}`);
}

// 修改用户状态
export function updateUserStatus(userId:string,isVaild:boolean) {
  return request.get(`/User/UpdateUserStatus?userId=${userId}&isVaild=${isVaild}`);
}

//用户信息分页查询
export function getUsers( pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string,userId:string) {
  return request.get(`/User/GetUsers?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&userId=${userId}`)
}

//获取用户相关选项的数据
export function getAboutUserOptions() {
  return request.get(`/User/GetAboutUserOptions`);
}

//根据省份获取城市
export function getCitysByProvince(provinceId:number) {
  return request.get(`/User/GetCitysByProvince?provinceId=${provinceId}`);
}

// 新增用户
export function addUser(data: object) {
  return request.post("/User/AddUser",data);
}

// 编辑用户
export function updateUser(data: object) {
  return request.post("/User/UpdateUser",data);
}
  
// 删除用户
export function delUser(userIds: Array<string>) {
  return request.post(`/User/DelUser`,userIds)
}

//导出用户信息
export function exportUsers(  searchKey:string, orderField:string,  orderType:string, fields:Array<object>) {
  return request.put(`/User/ExportUsers?orderField=${orderField}&orderType=${orderType}&searchKey=${searchKey}`,fields)
}

//获取当前用户导出用户列表被允许的字段
export function getAllowField(userId:string="") {
  return request.get(`/User/GetAllowField?userId=${userId}`);
}
