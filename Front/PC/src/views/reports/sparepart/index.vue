<template>
    <div class="layout-container">
      <div class="layout-container-form flex space-between">
        
      </div>
      <div class="layout-container-table">
        <p>请访问PowerBI链接</p>
        <el-link type="primary" :href="powerBILink">{{ powerBILink }}</el-link>
      </div>
    </div>
  </template>
  
  <script lang="ts" setup>
   defineOptions({
    name: "mrp"
  })
  import { onMounted, ref, reactive } from "vue";  
  import msg from "@/utils/system/message"; 
  import commonHelper from "@/utils/system/common-helper"; 
 
  const powerBILink=ref("https://app.powerbi.com/groups/914ae554-ec29-40a1-bd4c-8e5e62970ce2/reports/fb6380fc-7c82-4e1b-a501-8a1be431c200/ReportSection?experience=power-bi");
  onMounted(()=>{
 
  })
  </script>
  
  <style lang="scss" scoped>
    .csm-link{
      cursor: pointer;
      text-decoration: underline;
    }
  </style> 