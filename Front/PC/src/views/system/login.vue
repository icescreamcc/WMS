<template>
  <div class="container">
    <div class="box" v-if="showLoginFace">
      <h1>登录</h1>
      <p class="text-welcome">
      <span></span>
     </p>
      <el-form class="form">
        <el-input
          size="large"
          ref="inputUserId"
          v-model="form.name"
          :placeholder="$t('message.system.userName')"
          type="text"
          maxlength="50"
          clearable 
          autofocus
        >
          <template #prepend>
            <i class="sfont system-xingmingyonghumingnicheng"></i>
          </template>
        </el-input>
        <el-input
          size="large"
          ref="password" 
          v-model="form.password"
          :type="passwordType"
          :placeholder="$t('message.system.password')"
          name="password"
          maxlength="50"
          clearable 
          
        >
          <template #prepend>
            <i class="sfont system-mima"></i>
          </template>
          <template #append>
            <i class="sfont password-icon" :class="passwordType ? 'system-yanjing-guan': 'system-yanjing'" @click="passwordTypeChange"></i>
          </template>
        </el-input>
        <el-button type="primary" :loading="form.loading" @click="submit" style="width: 100%;" size="medium">{{ $t('message.system.login') }}</el-button>
      </el-form>
      <div class="fixed-top-right" >
        <select-lang />
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup> 
import { ref, reactive,onMounted,onBeforeUnmount} from 'vue'
import { useStore } from 'vuex'
import { useRouter, useRoute } from 'vue-router'
import type { RouteLocationRaw  } from 'vue-router'
import { addRoutes } from '@/router'
import msg from "@/utils/system/message"
import selectLang from '@/layout/Header/functionList/word.vue'  
import { ElLoading } from 'element-plus'
import {getTicketUrl} from '@/api/system/user'
import jsonp from '@/utils/system/jsonp'
import { useI18n } from 'vue-i18n'
import commonHelper from '@/utils/system/common-helper';
import sysconst from '@/utils/system/sysConst'  

const store = useStore()
const router = useRouter()
const route = useRoute() 
const storageLoginStatusKey=sysconst+'loginStatus';
const form = reactive({
  name: '',
  password: '',
  loading: false
}) 
const { t } = useI18n()
const passwordType = ref('password')
const inputUserId= ref<null | HTMLElement>(null);
const inputPassword=ref<null | HTMLElement>(null);
const passwordTypeChange = () => {
  passwordType.value === '' ? passwordType.value = 'password' : passwordType.value = ''
} 
const isMobileFlag=commonHelper.isMobile(); 
const showLoginFace=ref(false);
const loginStatus=commonHelper.getLocalStorage(storageLoginStatusKey);

const autoAuth = () => {   
  getTicketUrl().then((ticket:any)=>{
    if(ticket.data){
      msg.successAuto(t('message.system.authTips_windows'))
      const loading = ElLoading.service({
          lock: true,
          text: '正在获取Windows账户信息',
          background: 'rgba(0, 0, 0, 0.7)',
        });
        setTimeout(() => {
          let ticketUrl=ticket.data+encodeURIComponent(document.location.href)
          jsonp(ticketUrl).then((res:any)=>{   
          store.dispatch('user/windowsAutoLogin', res.ticket)
          .then(async (res) => {   
            loading.close();
            commonHelper.setLocalStorage(storageLoginStatusKey,'0');
            //获取并加载用户权限菜单  
            addRoutes(res.userId,false).then(()=>{
              router.push(route.query.redirect as RouteLocationRaw || '/')
            });   
          })
          .catch(err=>{showLoginFace.value=true;})
          .finally(() => {
            loading.close();
            })
        }) 
        }, 1500);
    }
    else{
      msg.successAuto(t('message.system.authTips_up'))
      showLoginFace.value=true;
    } 
  })   
};
 

onMounted(()=>{  
  if(!isMobileFlag&&(!loginStatus||loginStatus=='0')){
    autoAuth();
    }
    else{
      showLoginFace.value=true;
    }
    document.addEventListener('keydown', handleKeyDown); 
})
onBeforeUnmount(()=>{
  document.removeEventListener('keydown', handleKeyDown); 
})  

const checkForm = () => {
  return new Promise((resolve, reject) => { 
    if (form.name === '') {
        msg.warningAuto("用户名不能为空");
      inputUserId.value?.focus();
      return;
    }
    if (form.password === '') { 
        msg.warningAuto("密码不能为空"); 
        inputPassword.value?.focus();
      return;
    }
    resolve(true)
  })
}

const submit = () => { 
  checkForm()
  .then(() => {
    form.loading = true
    let params = {
        userId: form.name,
      password: form.password,
      device:isMobileFlag?'mobile':'pc'
    } 
    store.dispatch('user/login', params)
    .then(async (res) => {    
      //获取并加载用户权限菜单  
        addRoutes(res.userId,isMobileFlag).then(()=>{ 
          commonHelper.setLocalStorage(storageLoginStatusKey,'0');
          if(isMobileFlag){
            router.push({name:'mobilepda'});   
          }
          else{
            router.push(route.query.redirect as RouteLocationRaw || '/')
          }  
        });   
      })
      .catch(err=>{})
      .finally(() => {
      form.loading = false
      })
  })
} 
const handleKeyDown=(event:any)=> {
  if (event.keyCode === 13) {
    event.preventDefault(); 
    submit(); 
  }
} 
</script>

<style lang="scss" scoped>
.container {
  position: relative;
  width: 100vw;
  height: 100vh;
  background-color: #eef0f3; 
  .login-logo{
    height: 130px;background-color: #ffa500;
    img{
      position: relative;
      bottom: -40%;
    }
  }
  .box {
    width: 540px;
    position: absolute;
    left: 50%;
    top: 50%;
    // background: rgba(4, 40, 51, 0.263);
    background-color: #fff;
    border-radius: 2px;
    transform: translate(-50%, -50%);
    height: 300px;
    overflow: hidden;
    box-shadow: 0 6px 20px 5px #cdcdcd,
      0 16px 24px 2px #ebb563;
    h1 {
      margin-top: 30px;
      text-align: center; 
      color: #696969;
    }
    .text-welcome{ 
      text-align: left; 
      padding: 0 25px 0 30px;
      color: #696969;
    }
    .form {
      width: 90%;
      margin: 20px auto 15px;
      .el-input {
        margin-bottom: 20px;
      }
      .password-icon {
        cursor: pointer;
        color: #ebb563;
      }
    }
    .fixed-top-right {
      position: absolute;
      top: 10px;
      right: 10px;
    }
  }
}
@media screen and (max-width: 750px) {
  .container-logo{
    z-index: 1;
  }
  .container .box {
    width: 100vw;
    height: 100vh;
    box-shadow: none;
    left: 0;
    top: 0;
    transform: none;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    h1 {
      margin-top: 0;
    }
    .form {
    }
  }
}
</style>
