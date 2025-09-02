<template>
    <div class="full">
      <div class="left">
        <PackingClassifyTree />
      </div>
      <div class="content">
        <PackingTable/>
      </div>
    </div>
  </template>
  
  <script lang="ts" setup>
  defineOptions({
    name: "packing-material"
  })
  import { ref, provide } from 'vue'
  import PackingClassifyTree from './packingClassifyTree.vue'
  import PackingTable from './packingTable.vue'
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