<template>
  <div class="full">
    <div class="left">
      <MenuTree/>
    </div>
    <div class="content">
      <MenuInfo  />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, provide, reactive } from 'vue'
import MenuTree from './fieldTree.vue'
import MenuInfo from './fieldInfo.vue'
export default defineComponent({
  name: 'treeTable',
  components: {
    MenuTree,
    MenuInfo,
  },
  
  setup() {
    let active: any = ref({})
    provide('active', active)  
 
  }
})
</script>

<style lang="scss" scoped>
  .full {
    width: 100%;
    height: 100%;
    box-sizing: border-box;
    padding: 15px;
    display: flex;
    .left {
      width: 350px;
    }
    .content {
      flex: 1;
      width: calc(100% - 330px);
      height: 100%;
    }
  }
</style>