<template>
  <el-config-provider :locale="locale">
    <router-view></router-view>
  </el-config-provider>
</template>

<script lang="ts">
import { defineComponent, computed } from 'vue'
import { useI18n } from 'vue-i18n'
export default defineComponent({
  name: 'App',
  setup() {
    const i18n:any = useI18n()
    const locale = computed(() => {
      return {
        name: i18n.locale.value,
        el: i18n.messages.value[i18n.locale.value].el
      }
    })
    return {
      locale
    }
  }
})
</script>
<console class="log"></console>
<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  width: 100%;
  height: 100vh;
}

@media screen and (max-width: 450px){
  /* 设置滚动条的宽度、高度、背景色和边框样式 */
  ::-webkit-scrollbar { 
  width: 0px;
  height: 0px;  
  border-radius: 5px;
  }

  /* 设置滚动条轨道的背景色和圆角 */
  ::-webkit-scrollbar-track { 
    background-color: transparent; 
  }
    
  /* 设置滚动条滑块的背景色和圆角 */
  ::-webkit-scrollbar-thumb { 
    background-color: #ffffffc0;
    border-radius: 5px;
  }

  /* 设置滚动条滑块在悬停状态下的背景色和圆角 */
  ::-webkit-scrollbar-thumb:hover { 
  background-color: #ffffffc0;  
  border-radius: 5px;
  }
}

@media screen and (min-width: 450px){
  /* 设置滚动条的宽度、高度、背景色和边框样式 */
  ::-webkit-scrollbar { 
  width: 8px;
  height: 10px;  
  border-radius: 2px;
  }

  /* 设置滚动条轨道的背景色和圆角 */
  ::-webkit-scrollbar-track { 
    background-color: transparent; 
  }
    
  /* 设置滚动条滑块的背景色和圆角 */
  ::-webkit-scrollbar-thumb { 
    background-color: #dedddd9a;
    border-radius: 2px;
  }

  /* 设置滚动条滑块在悬停状态下的背景色和圆角 */
  ::-webkit-scrollbar-thumb:hover { 
  background-color: #c9c9c9db;  
  border-radius: 2px; 
  cursor: pointer;
  }
}




/* 隐藏滚动条 IE Edge */
:-ms-scrollbar {
  width: 10px;
  height: 10px;  
  border-radius: 5px;
}

:-ms-scrollbar-track {
  background-color: transparent;
}
  
:-ms-thumb {
  background-color: #f1f1f1c0;
  border-radius: 5px;
}

:-ms-thumb:hover {
  background-color: #d3d3d3c2;  
border-radius: 5px
}

/* 隐藏滚动条 Firefox */
/* .hide-scrollbar {
  scrollbar-width: thin; 
  scrollbar-color: transparent transparent; 
} */

</style>
