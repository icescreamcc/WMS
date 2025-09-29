<template>
  <div class="logo-container">
    <img :src="systemHomePageLogo" alt="" style="height:48px">
  </div>
</template>

<script lang="ts">
import { defineComponent, computed } from 'vue'
import { useStore } from 'vuex'
export default defineComponent({
  setup() {
    const store = useStore()
    const isCollapse = computed(() => store.state.app.isCollapse)

    //获取系统参数信息
    const systemInfo = computed(() => store.getters['user/systemInfo']);
    const systemHomePageLogo = computed(() => {
      return systemInfo.value.find((item: any) => item.argsKey === "SystemHomePageLogo")?.argsValue;
    });

    return {
      isCollapse,
      systemHomePageLogo,
    }
  }
})
</script>

<style lang="scss" scoped>
  .logo-container {
    height: 64px;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    background-color: var(--system-logo-background);
    img{
      width: 100%;
    }
    h1 {
      font-size: 20px;
      white-space: nowrap;
      color: var(--system-logo-color);
    }
  }
</style>