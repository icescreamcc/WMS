<template>
  <div class="layout-container">
    <div class="layout-container-form-handle">
    </div>
    <div class="layout-container-form flex space-between"> 
      <div class="layout-container-form-search">
       <el-date-picker 
        v-model="dateRange"
        type="daterange"
        range-separator="-"
        start-placeholder="最早日期"
        end-placeholder="最晚日期"
         size="small"
         value-format="YYYY-MM-DD"
         style="margin-right:10px;width:120%"
      >
      </el-date-picker>
        <el-input 
          v-model="query.input"
          placeholder="请输入关键词进行检索"
          size="small"
        ></el-input>
        <el-button
          type="primary"
          icon="el-icon-search"
          class="search-btn"
          @click="getTableData(true)"
          >搜索</el-button>  
      </div>
    </div>
    <div class="layout-container-table">
      <Table
        ref="table"
        v-model:page="page"
        v-loading="loading" 
        :data="tableData"  
        @getTableData="getTableData"
        @selection-change="handleSelectionChange"
        @orderChanged="handleSortChange"
      >
         <el-table-column prop="dateTime" label="时间" align="center" sortable="custom" :show-overflow-tooltip="true"/> 
           <el-table-column prop="userName" label="操作人" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>  
             <el-table-column prop="logType" label="操作类型" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
                <template #default="scope">
              <span>{{scope.row.title}}</span> 
            </template>
             </el-table-column>   
        <el-table-column prop="message" label="操作明细" align="center" sortable="custom" min-width="150" :show-overflow-tooltip="true"/>    
      </Table> 
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, reactive } from "vue";
import { Page } from "@/components/table/type";
import { getLogs} from "@/api/system/logInfo"; 
import Table from "@/components/table/tableServer.vue";  
export default defineComponent({
  name: "loginfo",
  components: {
    Table
  },
  setup() { 
    let dateRange=ref()
    // 存储搜索用的数据
    let query = reactive({
      input: "",
    });
 

    // 分页参数, 供table使用
    const page: Page = reactive({
      index: 1,
      size: 20,
      total: 0,
      orderField:'',
      orderType:''
    });
    let loading = ref(true);
    let tableData = ref([]);
    let chooseData = ref([]);
    let handleSelectionChange = (val: []) => {
      chooseData.value = val;
    };

    //排序事件
    let handleSortChange=(orderRow:any)=>{ 
      getTableData(true);
    }

    // 获取表格数据
    // params <init> Boolean ，默认为false，用于判断是否需要初始化分页
    let getTableData = (init: Boolean) => {
      loading.value = true
      if (init) {
        page.index = 1
      }  
      let dateStart='';
      let dateEnd='';
      if(dateRange.value?.length>0){
        dateStart=dateRange.value[0]
      }
       if(dateRange.value?.length>1){
        dateEnd=dateRange.value[1]
      }
      getLogs(page.size,page.index,page.orderField,page.orderType,query.input,dateStart,dateEnd)
        .then((res) => {
          console.log("getMessages2",res.data)
          let data = res.data.rows
          data.forEach((d: any) => {
            d.loading = false
          })
          tableData.value = data
          page.total = Number(res.data.total);
        })
        .catch((error) => {
          tableData.value = [];
          page.index = 1;
          page.total = 0;
        })
        .finally(() => {
          loading.value = false;
        });
    }
   
      
    getTableData(true)
    return { 
      dateRange,
      query, 
      tableData,
      chooseData,
      loading,
      page,  
      handleSelectionChange,
      handleSortChange,
      getTableData
    };
  }
});
</script>

<style lang="scss" scoped>
.statusName {
  margin-right: 10px;
}
</style>
