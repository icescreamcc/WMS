<template>
  <div class="full">
    <div class="left">
      <organizationTree />
    </div>
    <div class="content">
     <menus/>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, provide } from 'vue'
import organizationTree from './approvalDataTypeTree.vue'
import menus from './process.vue'
export default defineComponent({
  name: 'approval',
  components: {
    organizationTree,
    menus,
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