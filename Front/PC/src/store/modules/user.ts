import { loginApi, getUserMenus, loginOut ,getWindowsUser} from '@/api/system/user'
import { ActionContext } from 'vuex'
import {getArgs} from '@/api/system/args' 

interface State {
  token: string,
  signature:string,
  info: any,
  menus:Array<any>,
  routeHis:Array<any>,
  systemInfo:Array<any>,
}
const state = (): State => ({
  token: '', // 登录token
  signature:'',//验证票据
  info: {},  // 用户信息
  menus:[], //用户权限菜单
  routeHis:[], //路由历史
  systemInfo:[],  //系统信息
})

// getters
const getters = {
  token(state: State) { 
    return state.token;
  }, 
  userInfo(state:State){
    return state.info;
  },
  userId(state:State){
    return state.info.userId;
  },
  userName(state:State){
    return state.info.userName;
  },
  menus(state:State){
    return state.menus;
  },
  routeHis(state:State){
    return state.routeHis;
  },
  systemInfo(state:State){
    return state.systemInfo;
  }, 
}

// mutations
const mutations = {
  tokenChange(state: State, token: string) { 
    state.token = token
  }, 
  infoChange(state: State, info: object) {
    state.info = info
  },
  menuChange(state:State,menus:Array<any>){
    state.menus=menus;
  },
  systemInfoChange(state: State, systemInfo:Array<any>) {
    state.systemInfo = systemInfo
  },
  routeHisAdd(state:State,routeName:string){ 
    (state.routeHis as Array<any>).push(routeName);
  },
  routeHisRemove(state:State,routeName:string){
    let index=(state.routeHis as Array<any>).indexOf(routeName); 
    if(index>-1){
      (state.routeHis as Array<any>).splice(index+1);
    }
  },
  routeHisClear(state:State){ 
    state.routeHis=[];
  }
}

// actions
const actions = {
  // login by login.vue
  login({ commit, dispatch }: ActionContext<State, State>, params: any) {
    return new Promise((resolve, reject) => {
      loginApi(params).then(res=>{ 
        if(res.status.toString()=="Success"){  
          commit('infoChange', res.data); 
          commit('systemInfoChange', res.data.systemInfo);
          resolve(res.data) 
        }
        else{
          reject(res)
        } 
        }).catch(err=>{ 
          reject(err)
        }) 
    })
  },
  windowsAutoLogin({ commit, dispatch }: ActionContext<State, State>, ticket: string) {
    return new Promise((resolve, reject) => {
      getWindowsUser(ticket).then(res=>{ 
        if(res.status.toString()=="Success"){  
          commit('infoChange', res.data); 
          commit('systemInfoChange', res.data.systemInfo);
          resolve(res.data) 
        }
        else{
          reject(res)
        } 
        }).catch(err=>{ 
          reject(err)
        }) 
    })
  },
  //重新获取系统参数信息
  loadSystemInfo({ commit }: ActionContext<State, State>) {
    return new Promise((resolve, reject) => {
      getArgs().then(res => {
        if (res.status.toString()=="Success"){  
          commit('systemInfoChange', res.data);
          resolve(res.data);
        } else {
          reject(res);
        }
      }).catch(reject);
    });
  },
  // get user menus after user logined
  getMenus({ commit }: ActionContext<State, State>, userId: string) {
    return new Promise((resolve, reject) => {
      getUserMenus(userId)
      .then(res => {  
        commit('menuChange', res.data)
        resolve(res.data); 
      })
    })
  },

  // login out the system after user click the loginOut button
  loginOut({ commit }: ActionContext<State, State>,userId:string) {
    loginOut(userId)
    .then(res => {

    })
    .catch(error => {

    })
    .finally(() => {
      localStorage.removeItem('tabs')
      localStorage.removeItem('vuex')
      location.reload()
    })
  }
}

export default {
  namespaced: true,
  state,
  actions,
  getters,
  mutations
}
