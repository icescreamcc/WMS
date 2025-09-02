<template>
    <div class="full">
      <div class="left">
        <ProductClassifyTree />
      </div>
      <div class="content">
        <ProductTable/>
      </div>
    </div>
  </template>
  
  <script lang="ts" setup>
    defineOptions({
    name: "finshed-product"
  })
  import { ref, provide } from 'vue'
  import ProductClassifyTree from './productClassifyTree.vue'
  import ProductTable from './productTable.vue'
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