/**
 * @description 所有人可使用的参数配置列表
 * @params hideMenu: 是否隐藏当前路由结点不在导航中展示
 * @params alwayShow: 只有一个子路由时是否总是展示菜单，默认false
 */
import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router'
import store from '@/store'
import i18n from '@/locale'
import NProgress from '@/utils/system/nprogress'
import { changeTitle } from '@/utils/system/title' 
import {  getUserMenus } from '@/api/system/user'  
// 动态路由相关引入数据
import Layout from '@/layout/index.vue'  

// 引入modules 
import System from './modules/system'
import Home from './modules/home'
import commonHelper from '@/utils/system/common-helper'
 
const { t } = i18n.global 
var modules: object[] = System; 
var routes: any = modules 
var router = createRouter({
  history: createWebHashHistory(),
  routes
}) 

var asyncRoutes: RouteRecordRaw[] = Home
 
asyncRoutes.forEach(item => {
  modules.push(item)
  router.addRoute(item)
})

// 动态路由的权限新增，供登录后调用
 
export  function addRoutes(userId:string,isMobileFlag:boolean){   
 return getUserMenus(userId).then(res=>{   
    store.commit('user/menuChange', res.data.map((m:any)=>{return m.menuId})) 
    let menuData:Array<any>=res.data.filter((m:any)=>m.menuType=='Menu'); 
    setAsyncRouters(menuData);   
    if(isMobileFlag)
     router.push({name:'mobilepda'}); 
     //router.push({name:'takestock-mobile'});  
     else
     router.push("/")
  }) 
}

let asyncModules = import.meta.glob('../views/**/**/*.vue')

let setAsyncRouters=(menuData:Array<any>)=>{  
  let data =new Array();  
  menuData.forEach(m=>{
    if(m.parentId=="ROOT"&&m.menuDisplay=='PC'){
      let parent={ 
        path: m.routePath,
        component:Layout, 
        meta: { title: m.menuName, icon: m.icon },
        hideMenu:m.hideMenu,
        children:new Array<any>()
      }
      menuData.forEach(c=>{
      if(c.parentId==m.menuId){ 
        let child={
          name:c.routePath,
          path: c.routePath.indexOf("/")>=0?c.routePath.replace("/",""):c.routePath, 
          component:asyncModules[`../views/${c.componentPath}.vue`], 
          meta: { title: c.menuName, icon: null,hideTabs:c.hideMenu },
          url: c.url,
          display:c.menuDisplay
        }
        parent.children.push(child)
      }
     });
     data.push(parent); 
    } 
    else if(m.parentId=="ROOT"&&m.menuDisplay=='MOBILE'){
      // const parentCompPath = `../views/${m.componentPath}.vue`;
      // const parentComponent = asyncModules[parentCompPath];
      // if (!parentComponent) {
      //     console.warn("父组件不存在，跳过MOBILE路由:", parentCompPath);
      // }
      let parent={ 
        name:m.routePath, 
        path: '/'+m.routePath,
        component: asyncModules[`../views/${m.componentPath}.vue`], 
        meta: { title: m.menuName, icon: m.icon },
        hideMenu:m.hideMenu,
        children:new Array<any>()
      } 
      menuData.forEach(c=>{
      // if (c.parentId === m.menuId) { 
      //       const childCompPath = `../views/${c.componentPath}.vue`;
      //       const childComponent = asyncModules[childCompPath];
      //       if (!childComponent) {
      //           console.warn("子组件不存在，跳过MOBILE子路由:", childCompPath);
      //           return; // 不存在就跳过
      //       }
      //     }

        if(c.parentId==m.menuId){ 
          let child={
            name:c.routePath,
            path: c.routePath.indexOf("/")>=0?c.routePath.replace("/",""):c.routePath,
            component:asyncModules[`../views/${c.componentPath}.vue`], 
            meta: { title: c.menuName, icon: null,hideTabs:c.hideMenu },
            children:new Array<any>()
          } 
          parent.children.push(child)
        }
       });
      data.push(parent)
    }
    else if(m.parentId=="ROOT"&&(m.menuDisplay=='DASHBOARD'||m.menuDisplay=='OTHER'||m.menuDisplay=='SEPARATE')){
      let parent={ 
        name:m.routePath, 
        path: '/'+m.routePath,
        component: asyncModules[`../views/${m.componentPath}.vue`], 
        meta: { title: m.menuName, icon: m.icon,hideTabs: true, },
        hideMenu:m.hideMenu, 
        children:new Array<any>()
      } 
      menuData.forEach(c=>{
        if(c.parentId==m.menuId){ 
          let child={
            name:c.routePath,
            path: c.routePath.indexOf("/")>=0?c.routePath.replace("/",""):c.routePath,
            component:asyncModules[`../views/${c.componentPath}.vue`], 
            meta: { title: c.menuName, icon: null, hideTabs:true }, 
          }
          parent.children.push(child)
        }
       });
      data.push(parent)
    } 
  });     
  data.forEach(item => {
    modules.push(item)
    router.addRoute(item)
  }) 
}

//刷新页面后重新加载菜单
if (store.getters['user/token']) { 
  let isMobileFlag=commonHelper.isMobile(); 
   addRoutes(store.getters['user/userId'], isMobileFlag);

  // addRoutes(store.getters['user/userId'], isMobileFlag).then(() => {
  //   const currentPath = router.currentRoute.value.fullPath;
  //   if (currentPath === "/" || currentPath === "/home") {
  //     router.replace(currentPath); // 确保在刷新时跳转到正确的地址
  //   }
  // });
}

const whiteList = ['/login']
let dynamicRoutesLoaded = false;
let initialPath: string | null = null; 
if (window.location.hash) {
  initialPath = window.location.hash.slice(1); // 获取 hash 模式中的路径
}

router.beforeEach( async(to, _from, next) => { 
  NProgress.start();  
 
  if (store.getters['user/token'] || whiteList.indexOf(to.path) !== -1) {
    to.meta.title ? (changeTitle(to.meta.title)) : ""; // 动态title
    next()
  } else { 
     next("/login"); // 全部重定向到登录页
     to.meta.title ? (changeTitle(to.meta.title)) : ""; // 动态title
  }
});

// router.beforeEach(async (to, _from, next) => { 
//   NProgress.start(); 
//   if (!dynamicRoutesLoaded && store.getters['user/token']) {
//     let isMobileFlag = commonHelper.isMobile();
//     await addRoutes(store.getters['user/userId'], isMobileFlag);
//     dynamicRoutesLoaded = true; 
//     var initRedirect=initialPath??to.fullPath; 
//     next(initRedirect); // 确保导航到目标路径
//     return;
//   } 
//   if (store.getters['user/token'] || whiteList.indexOf(to.path) !== -1) {
//     to.meta.title ? (changeTitle(to.meta.title)) : ""; // 动态title
//     next()
//   } else { 
//      next("/login"); // 全部重定向到登录页
//      to.meta.title ? (changeTitle(to.meta.title)) : ""; // 动态title
//   }
// });

router.afterEach((to, _from) => { 
  const keepAliveComponentsName = store.getters['keepAlive/keepAliveComponentsName'] || []; 
  const name =  to.name;//to.matched[to.matched.length - 1].components.default.name;  
  if (to.meta  && name && !keepAliveComponentsName.includes(name)) {
    store.commit('keepAlive/addKeepAliveComponentsName', name)
  }
  NProgress.done();
});

export {
  modules
}

export default router
