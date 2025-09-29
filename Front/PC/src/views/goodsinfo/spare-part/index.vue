<template>
    <div class="full">
      <div class="left">
        <SpareClassifyTree />
      </div>
      <div class="content">
        <SpareTable/>
      </div>
    </div>
  </template>
  
  <script lang="ts" setup>
   defineOptions({
    name: "spare-part"
  })
  import { ref, provide } from 'vue'
  import SpareClassifyTree from './spareClassifyTree.vue'
  import SpareTable from './spareTable.vue'
  const active: any = ref({})
  provide('active', active)
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