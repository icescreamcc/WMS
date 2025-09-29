import request from '@/utils/system/request'

// 查询公司组织架构数据并组建成树形结构模型
export function getMenusData() {
return request.get(`/Menu/GetMenusData`)
}

//根据组织架构类型查询用户
export function getMenuInfo( menuId:string) {
return request.get(`/Menu/GetMenuInfo?menuId=${menuId}`)
}

//添加部门
export function addMenu(data:any){
return request.post("/Menu/AddMenu",data);
}

//修改部门
export function updateMenu(data:any){
return request.post("/Menu/UpdateMenu",data);
}

//删除部门
export function delMenu(menuId:string,parentId:string,menuType:string){
return request.get(`/Menu/DelMenu?menuId=${menuId}&parentId=${parentId}&menuType=${menuType}`);
}
 
