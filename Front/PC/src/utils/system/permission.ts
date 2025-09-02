import store from '@/store'  

//当前登录操作人
const getOperator=()=>{ 
  return {
    userId:store.getters['user/userId'],
    userName:store.getters['user/userName'],
    dept:store.getters['user/userInfo']?.deptName,
  } 
}
  
//判断是否有操作权限
  const isPermisstion=(menuStr1:string,menuStr2='',menuStr3='')=>{
    let permissionMenus=store.getters['user/menus']; 
    let key= permissionMenus.filter((m:any)=>{ 
      return  m==menuStr1||m==menuStr2||m==menuStr3
     }) 
     return key?.length>0?true:false
  }

  //将用户当前跳转路由加入路由历史
  const addRouteHis=(routeName:string)=>{
    store.commit('user/routeHisAdd',routeName)
  }

  //将用户当前跳转路由从路由历史中移除
  const removeRouteHis=(routeName:string)=>{
    store.commit('user/routeHisRemove',routeName)
  }

  //获取用户路由历史记录
  const getRouteHis=()=>{
    return store.getters['user/routeHis'];
  }

  //清空用户路由历史记录
  const clearRouteHis=()=>{
    store.commit('user/routeHisClear')
  }

  export default {
    getOperator, 
    isPermisstion,
    addRouteHis,
    getRouteHis,
    clearRouteHis,
    removeRouteHis
}