<template>
  <Layer :layer="layer" @confirm="queryData" >
     <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle"> 
      </div>
      <div class="layout-container-form-search"> 
          <el-date-picker
        v-model="dateRange"
        type="daterange"
        range-separator="-"
        start-placeholder="最早日期"
        end-placeholder="最晚日期" 
         value-format="YYYY-MM-DD"
         style="margin-right:10px;width:120%"
      >
      </el-date-picker>
      <el-select v-model="flowType" class="m-2" style="width:50%" placeholder="类别"> 
        <el-option  label="所有" value=""/>
        <el-option  label="入库" value="In"/>
        <el-option  label="出库" value="Out"/> 
      </el-select>
        <el-input 
        style="margin-left:20px"
          v-model="query.input"
          placeholder="请输入关键词进行检索" 
        ></el-input>
        <el-button 
          type="primary"
          icon="el-icon-search"
          class="search-btn"
          @click="getFlowTableData(true)"
          >搜索</el-button>
          <el-button v-if="permission.isPermisstion('STORAGEFLOWEXPORT')" icon="el-icon-download" style="margin-left:20px"   @click="exportData">导出</el-button>   
      </div>
    </div>
    <div class="layout-container-table" style="height:400px">
      <Table 
        v-model:page="page"
        v-loading="flowLoading" 
        :data="flowTableData" 
        @getFlowTableData="getFlowTableData" 
        @orderChanged="handleSortChange"   
      > 
        <el-table-column prop="goodsName" label="物品名称" align="center"  min-width="150" :show-overflow-tooltip="true">
          <template #default="scope"> 
            <span>{{scope.row.goodsName}}</span>&nbsp;
            <span>{{scope.row.goodsModel}}</span>
           </template>
        </el-table-column>
        <el-table-column prop="flowType" label="类别" align="center" sortable="custom"  :show-overflow-tooltip="true">
            <template #default="scope"> 
            <span  class="text-deft">{{scope.row.flowTypeDesc}}</span>
           </template>
        </el-table-column>
           <el-table-column prop="sourceStorageSubType" label="类型" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
            <template #default="scope"> 
            <span  class="text-deft">{{scope.row.sourceStorageSubTypeDesc}}</span>
           </template>
        </el-table-column>
        <el-table-column prop="quantity" label="数量" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
          <template #default="scope"> 
            <span>{{scope.row.quantity+scope.row.unitName}}</span>
           </template>
        </el-table-column> 
        <el-table-column prop="operateDate" label="操作时间" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
          <template #default="scope"> 
            <span>{{commonHelper.formatToDateTime(scope.row.operateDate)}}</span>
           </template>
        </el-table-column>  
        <el-table-column prop="operatorName" label="操作人" align="center" sortable="custom" :show-overflow-tooltip="true"/> 
        <el-table-column prop="sourceStorageType" label="来源单据" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
          <template #default="scope"> 
            <span  class="text-deft">{{scope.row.sourceStorageTypeDesc}}</span>
           </template>
        </el-table-column>   
         <el-table-column prop="sourceOrderNo" label="来源单号" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>  
         <el-table-column prop="isStatistics" label="已入账" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
          <template #default="scope"> 
            <span  class="text-success" v-if="scope.row.isStatistics">是</span>
           </template>
        </el-table-column>   
      </Table> 
    </div>
  </Layer> 
</template>

<script lang="ts" setup>
import {  reactive, ref,defineEmits,defineProps,onMounted  } from 'vue' 
import Layer from '@/components/layer/index.vue' 
import Table from "@/components/table/tableServer.vue";  
import { Page } from '@/components/table/type';
import { getStorageFlowList ,exportStorageFlow} from "@/api/inv/storage";
import commonHelper from '@/utils/system/common-helper';
import permission from '@/utils/system/permission';

const props=defineProps({
  layer: {
      type: Object,
      default: () => {
        return {
          show: false,
          title: '',
          showButton: true,
          btnLoading:false,
          type:'',
          data:null, 
        }
      }
    }
})
 
 // 存储搜索用的数据
 const query = reactive({
    input: "",
  });
  const dateRange=ref() 
  const flowType=ref('')
  // 分页参数, 供table使用
  const page: Page = reactive({
    index: 1,
    size: 20,
    total: 0,
    orderField:'',
    orderType:''
  });
  const flowLoading = ref(true);
  const flowTableData = ref(new Array<any>());

  onMounted(() => {
    getFlowTableData(true);
  })

  const getFlowTableData = (init: Boolean) => {
    flowLoading.value = true
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
      flowLoading.value = false
      page.orderType=page.orderType?page.orderType:'' 
    getStorageFlowList(props.layer.data.goodsId,page.size,page.index,page.orderField,page.orderType,query.input,flowType.value,dateStart,dateEnd)
      .then((res) => {
        let data = res.data.rows
        data.forEach((d: any) => {
          d.loading = false
        })
        flowTableData.value = data
        page.total = Number(res.data.total);
      })
      .catch((error) => {
        flowTableData.value = [];
        page.index = 1;
        page.total = 0;
      })
      .finally(() => {
        flowLoading.value = false;
      })
  }
  
  const handleSortChange=(orderRow:any)=>{  
      page.orderField=orderRow.prop;
      page.orderType=orderRow.order;  
      getFlowTableData(true);
  }
   
  const  queryData=()=> {    
      getFlowTableData(true) 
  }

  const exportData=()=>{
    let dateStart='';
    let dateEnd='';
    if(dateRange.value?.length>0){
      dateStart=dateRange.value[0]
    }
    if(dateRange.value?.length>1){
      dateEnd=dateRange.value[1]
      }
      page.orderType=page.orderType?page.orderType:''
    exportStorageFlow(props.layer.data.goodsId,page.orderField,page.orderType,query.input,flowType.value,dateStart,dateEnd).then(res=>{ 
            let link = document.createElement('a') 
            link.style.display = 'none'
            link.href =res.data
            document.body.appendChild(link)
            link.click() 
            document.body.removeChild(link)
        })
  }
</script>

<style lang="scss" scoped>
  * {
    text-align: left;
  }
  .box-card{
    margin-top: 10px;
  }
  .option-content{
    border:1px solid rgb(230, 230, 230);
    border-radius: 3px;
    padding: 5px;
    .head{
      border-bottom: 1px solid rgb(230, 230, 230);
      margin-bottom: 5px;
      .title{
        margin: 5px 0 0 0;
      }
    }
    .item{
      margin:3px 0;
    }
  }
</style>