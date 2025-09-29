<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
           <h2>人员信息</h2>
      </div> 
    </div>
    <div class="layout-container-table" style="margin-top:-10px">
      <Table
        ref="table" 
        v-loading="loading"  
        :data="tableData"  >
         <el-table-column prop="userId" label="用户ID" align="center" :sortable="true" min-width="100" :show-overflow-tooltip="true"/> 
        <el-table-column prop="userName" label="用户名" align="center" :sortable="true" min-width="100" :show-overflow-tooltip="true"/>
        <el-table-column prop="nickName" label="昵称" align="center" :sortable="true" :show-overflow-tooltip="true"/> 
        <el-table-column prop="email" label="邮箱" align="center" :sortable="true" min-width="100" :show-overflow-tooltip="true"/>
        <el-table-column prop="wechat" label="微信" align="center" :sortable="true" min-width="100" :show-overflow-tooltip="true"/> 
        <el-table-column prop="mobilePhone" label="手机" align="center" :sortable="true" min-width="100" :show-overflow-tooltip="true"/>
        <el-table-column prop="phone" label="电话" align="center" :sortable="true" min-width="100" :show-overflow-tooltip="true"/>  
      </Table> 
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, reactive, inject, watch } from 'vue'
import Table from '@/components/table/tableClient.vue' 
import{getUsersByOrganizationType} from '@/api/system/organization'
export default defineComponent({
  components: {
    Table
  },
  props:{
      params:{
        default: () => {
        return { 
          id:'',
          type:''
        }
      }
      }
  },
  setup(props,ctx) {  
    
    const activeCategory: any = inject('active')
    const loading = ref(true)
    const tableData = ref([]) 

    // 获取表格数据 
    const getTableData = () => {
      loading.value = true
      getUsersByOrganizationType(props.params.id,props.params.type).then((res:any)=>{
        tableData.value=res.data
      }).finally(()=>loading.value = false)
    }
 
    watch(activeCategory, (newVal) => { 
      getTableData()
    }) 
    
    return {  
      tableData, 
      loading,  
      getTableData
    }
  }
})
</script>

<style lang="scss" scoped>
  .layout-container {
    height: 100%;
    margin: 0 0 0 10px;
    width: calc(100% - 10px);
    h2 {
      padding: 0;
      margin: 0;
      margin-right: 20px;
      font-size: 14px;
      display: -webkit-box;
      -webkit-line-clamp: 1;
      -webkit-box-orient: vertical;
      overflow: hidden;
      height: 30px;
      line-height: 30px;
    }
  }
</style>