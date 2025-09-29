<template> 
    <div class="layout-container">
      <div class="layout-container-form flex space-between">
        <div class="layout-container-form-handle">
          <el-button v-if="permission.isPermisstion('AGVINFOADD')" type="primary" icon="el-icon-circle-plus-outline" @click="handleAGVInfoAdd">新增</el-button> 
        </div>
        <div class="layout-container-form-search">  
          <el-select v-model="queryKeywords.agvType"  size="small"  class="m-2" style="width:100%;margin-right:10px;" @change="queryTableData(true)" placeholder="AGV类型">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">AGV类型</div></template>        
            <el-option v-for="item in agvTypeOptions" :key="item.key" :label="item.value" :value="item.key"> </el-option>
          </el-select>
          <el-input v-model="queryKeywords.input"  placeholder="请输入关键词进行检索" size="small" clearable  @clear="queryTableData"></el-input>
          <el-button type="primary" icon="el-icon-search"  class="search-btn" @click="queryTableData(true)">搜索</el-button>   
        </div>
      </div>
      <div class="layout-container-table">
        <TableServer  ref="table"   v-model:page="tabelPageData" v-loading="tableLoading"  :data="tableData" 
            @getTableData="queryTableData"  
            @orderChanged="handleTabelSortChange"> 
            <el-table-column prop="agvNo" label="AGV编码"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
            <el-table-column prop="agvType" label="AGV类型"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
                <template #default="scope">
                <span>{{ scope.row.agvTypeDesc }}</span>
                </template>
            </el-table-column>
            <el-table-column prop="taskType" label="执行任务模板"   align="center" sortable="custom" min-width="110" :show-overflow-tooltip="true"/> 
            <el-table-column prop="ctnrType" label="搬运容器类型"   align="center" sortable="custom" min-width="110" :show-overflow-tooltip="true"/> 
            <el-table-column prop="positionCodeType" label="路径位置类型"   align="center" sortable="custom" min-width="110" :show-overflow-tooltip="true"/> 
            <el-table-column prop="isDeft" label="是否默认配置"   align="center" sortable="custom" min-width="110" :show-overflow-tooltip="true">
                <template #default="scope">
                <span v-if="scope.row.isDeft" class="text-success">是</span> 
                </template>
            </el-table-column> 
            <el-table-column prop="status" label="当前状态"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
                <template #default="scope">
                <span v-if="scope.row.status=='Free'" class="text-success">{{ scope.row.statusDesc }}</span> 
                <span v-else-if="scope.row.status=='Abandon'" class="text-danger">{{ scope.row.statusDesc }}</span> 
                <span v-else class="text-warning">{{ scope.row.statusDesc }}</span> 
                </template>
            </el-table-column> 
            <el-table-column prop="updateUserName" label="上次修改人"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/> 
            <el-table-column prop="updateDate" label="修改时间"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
                <template #default="scope">
                <span>{{commonHelper.formatToDateTime(scope.row.updateDate) }}</span>
                </template>
            </el-table-column> 
            <el-table-column  v-if="permission.isPermisstion('AGVINFOUPDATE','AGVINFODEL')"  :label="$t('message.common.handle')" align="center"  min-width="120">
                <template #default="scope"> 
                <el-button  v-if="permission.isPermisstion('AGVINFOUPDATE')" @click="handleAGVInfoEdit(scope.row)">{{$t("message.common.update")}}</el-button> 
                <el-popconfirm v-if="permission.isPermisstion('AGVINFODEL')"  :title="$t('message.common.delTip')" @confirm="onAGVInfoDel(scope.row)">
                    <template #reference>
                    <el-button type="danger">{{ $t("message.common.del") }}</el-button>
                    </template>
                </el-popconfirm> 
                </template>
            </el-table-column>
            </TableServer>
            <EditAGVInfoModal :layer="editAGVInfoLayer"  @dataSubmit="onAGVInfoEditSubmit" v-if="editAGVInfoLayer.show" />  
      </div> 
    </div>
  </template>
  
  <script lang="ts" setup>
  defineOptions({
    name: "device-info"
  })
  import { ref, reactive,onMounted,onBeforeMount} from "vue";  
  import {getAGVInfo,getAGVInfoDetail,addAGVInfo,updateAGVInfo,delAGVInfo,getArgs} from "@/api/workshop-device/agv-setting";
  import { LayerInterface } from "@/components/layer/index.vue"; 
  import TableServer from "@/components/table/tableServer.vue"; 
  import EditAGVInfoModal from "./agvInfoEditLayer.vue";   
  import { Page } from "@/components/table/type";
  import permission from '@/utils/system/permission' 
  import commonHelper from "@/utils/system/common-helper";
  import msg from '@/utils/system/message' 
  
  const queryKeywords = reactive({
        input: "", 
        agvType:''
   });  
  const tbHeight=ref(0) 
  const agvTypeOptions=ref(new Array<any>());
  onBeforeMount(()=>{
    tbHeight.value=window.innerHeight-260;
  })
   
  onMounted(()=>{
    getArgsOption();
    queryTableData(true);
  })

  const getArgsOption=()=>{
     getArgs().then(res=>{ 
        agvTypeOptions.value=res.data.agvTypeOptions;
        agvTypeOptions.value.unshift({key:'',value:'所有'});
     });
  }
  

  //设备信息
  const tabelPageData: Page = reactive({
        index: 1,
        size: 20,
        total: 0,
        orderField:'',
        orderType:''
      }); 
   const tableLoading = ref(false);
   const tableData = ref([]);   
   const editAGVInfoLayer: LayerInterface = reactive({
        show: false,
        title: "",
        showButton: true,
        btnLoading:false,
        width:"30%",
        data:null, 
        type:'', 
        otherButton:{ }
    });  
       
    const handleTabelSortChange=(orderRow:any)=>{   
      queryTableData(true);
    }
 
 
    const queryTableData = (init: Boolean) => { 
      if (init) {
        tabelPageData.index = 1
      }   
      tableLoading.value = true; 
      getAGVInfo( tabelPageData.size,tabelPageData.index,tabelPageData.orderField,tabelPageData.orderType,queryKeywords.agvType,queryKeywords.input)
        .then((res) => { 
          let data = res.data.rows
          data.forEach((d: any) => {
            d.loading = false;
          })
          tableData.value = data
          tabelPageData.total = Number(res.data.total);
        })
        .catch((error) => {
          tableData.value = [];
          tabelPageData.index = 1;
          tabelPageData.total = 0;
        })
        .finally(() => {
          tableLoading.value = false;
        });
    } 
  
  const handleAGVInfoAdd = () => {
    editAGVInfoLayer.title = "新增AGV配置信息";
    editAGVInfoLayer.show = true; 
    editAGVInfoLayer.type='add';   
    delete editAGVInfoLayer.data;
  }
 
  const handleAGVInfoEdit = (row: any) => {  
    getAGVInfoDetail(row.agvId).then(res=>{
      editAGVInfoLayer.title = "编辑AGV配置信息"; 
      editAGVInfoLayer.show = true;
      editAGVInfoLayer.type='update'; 
      editAGVInfoLayer.data = res.data; 
    }) 
  }

  const onAGVInfoDel=(row:any)=>{ 
    delAGVInfo(row.agvId).then(res=>{
      queryTableData(false);
    })
  }
       
  const onAGVInfoEditSubmit=(data:any,actionType:string)=>{   
    editAGVInfoLayer.btnLoading=true; 
    if(actionType=='add'){
      addAGVInfo(data).then(res=>{
            editAGVInfoLayer.show = false;
          queryTableData(false);
      }).finally(()=> editAGVInfoLayer.btnLoading=false);
    }
    else{
      updateAGVInfo(data).then(res=>{
            editAGVInfoLayer.show = false;
          queryTableData(false);
      }).finally(()=> editAGVInfoLayer.btnLoading=false);
    } 
  } 
 
  </script>
  
  <style lang="scss" scoped>  
  </style>
  