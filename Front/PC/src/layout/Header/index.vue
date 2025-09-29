<template>
  <header>
    <div class="left-box">
      <!-- 收缩按钮 -->
      <div class="menu-icon" @click="opendStateChange">
        <i :class="isCollapse ? 'el-icon-s-unfold' : 'el-icon-s-fold'"></i>
      </div>
      <!-- <Breadcrumb /> -->
      <div class="app-name">
        {{ $t(systemTitle) }}
      </div>
    </div>
    <div class="right-box">
      <!-- 快捷功能按钮 -->
      <div class="function-list">
        <div class="function-list-item"><a href="http://10.24.78.64:8019" title="Go Protal"><el-icon  style="font-size: 19px;position: relative;top:2px"><HomeFilled /></el-icon></a></div> 
        <div class="function-list-item hidden-sm-and-down" style="margin-bottom:2px"><select-lang /></div>
        <div class="function-list-item hidden-sm-and-down"><Full-screen /></div> 
        <!-- <div class="function-list-item" ><SizeChange /></div> -->
        <!-- <div class="function-list-item hidden-sm-and-down" ><Theme /></div>  -->
      </div>
      <!-- 用户信息 -->
      <div class="user-info">
        <el-dropdown>
          <span class="el-dropdown-link">
            {{ permission.getOperator().userName}}
            <i class="el-icon-arrow-down el-icon--right"></i>
          </span>
          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item @click="showPasswordLayer">{{ $t('message.system.changePassword') }}</el-dropdown-item>
              <el-dropdown-item @click="loginOut">{{ $t('message.system.loginOut') }}</el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
      </div>
      <password-layer :layer="layer" v-if="layer.show" />
    </div>
  </header>
</template>

<script lang="ts">
import { defineComponent, computed, reactive,onMounted,watch } from 'vue'
import { useStore } from 'vuex'
import { useRouter, useRoute } from 'vue-router'
import FullScreen from './functionList/fullscreen.vue'
import selectLang from './functionList/word.vue'
import SizeChange from './functionList/sizeChange.vue' 
import Theme from './functionList/theme.vue'
import Breadcrumb from './Breadcrumb.vue'
import PasswordLayer from './passwordLayer.vue'
import permission from '@/utils/system/permission'
import commonHelper from '@/utils/system/common-helper'
import sysconst from '@/utils/system/sysConst'
import { HomeFilled} from '@element-plus/icons-vue'; 

export default defineComponent({
  components: {
    FullScreen,
    Breadcrumb,
    selectLang,
    SizeChange, 
    Theme,
    PasswordLayer, 
    HomeFilled
  },
  setup() {
    const store = useStore()
    const router = useRouter()
    const route = useRoute()
    const layer = reactive({
      show: false,
      showButton: true,
      otherButton:{
                show:false,
                otherBtnLoading:false,
                text:"",
                type:""
              }
    }) 
    const storageLoginStatusKey=sysconst+'loginStatus';
    const isCollapse = computed(() => store.state.app.isCollapse)
    // isCollapse change to hide/show the sidebar
    const opendStateChange = () => {
      store.commit('app/isCollapseChange', !isCollapse.value)
    }

    // login out the system
    const loginOut = () => {
      store.dispatch('user/loginOut').then(()=>{
        commonHelper.setLocalStorage(storageLoginStatusKey,'1');
      })
    }
    
    const showPasswordLayer = () => {
      layer.show = true
    }

    //获取系统参数信息
    const systemInfo = computed(() => store.getters['user/systemInfo']);
    const systemColor = computed(() => {
      return systemInfo.value.find((item: any) => item.argsKey === "SystemColor")?.argsValue || '#ffa500';
    });
    const systemTitle = computed(() => {
      return systemInfo.value.find((item: any) => item.argsKey === "SystemName")?.argsValue;
    });

    watch(systemColor, (newColor) => {
      if (newColor) {
        document.documentElement.style.setProperty('--system-header-background', newColor);
        document.documentElement.style.setProperty('--system-logo-background', newColor);
      }
    }, { immediate: true });

    return {
      isCollapse,
      layer,
      permission,
      opendStateChange,
      loginOut,
      showPasswordLayer,
      systemTitle
    }
  }
})
</script>

<style lang="scss" scoped>
  .logo-container {
    background-color: var(--system-logo-background);
  }

  header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    height: 60px;
    background-color: var(--system-header-background);
    padding-right: 22px;
  }
  .left-box {
    height: 100%;
    display: flex;
    align-items: center;
    .menu-icon {
      width: 60px;
      height: 100%;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 25px;
      font-weight: 100;
      cursor: pointer;
      margin-right: 10px;
      &:hover {
        background-color: var(--system-header-item-hover-color);
      }
      i {
        color: var(--system-header-text-color);
      }
    }
    .app-name{
      color: #fff;
      font-weight: 600;
      font-size: x-large;
    }
  }
  .right-box {
    display: flex;
    justify-content: center;
    align-items: center;
    .function-list{
      display: flex;
      .function-list-item {
        width: 30px;
        display: flex;
        justify-content: center;
        align-items: center;
        :deep(i) {
          color: var(--system-header-text-color);
        }
      }
    }
    .user-info {
      margin-left: 20px;
      .el-dropdown-link {
        color: var(--system-header-breadcrumb-text-color);
      }
    }
  }
</style>