<template>
  <Layer :layer="layer" >
  <div class="full" style="height: 500px;">
 

    <div class="layout-container">
      <div class="layout-container-form flex space-between">
        <div class="layout-container-form-handle">
        </div>

        <div class="layout-container-form-search"> 
        </div>
      </div>
      <div class="layout-container-table">
        <Table
              ref="table"
              v-model:page="page"
              v-loading="loading"
              :showSelection="true"
              :data="tableData" 
              :csmHeight="'400px'"
              @getTableData="getTableData"
              @selection-change="handleSelectionChange"
              @orderChanged="handleSortChange"
        >
          <el-table-column prop="taskId" label="ID" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true" v-if="false"/> 
          <el-table-column prop="taskCode" label="任务编码" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true" v-if="false"/> 
          <el-table-column prop="aGVReqCode" label="AGV任务请求吗" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true" v-if="false"/> 
          <el-table-column prop="lineNo" label="需求产线" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="orderNo" label="业务单号(出库单号或入库单号)" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true"/> 
                <el-table-column prop="goodsClassifyGroup" label="物料分类" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true"/> 
                <el-table-column prop="businessType" label="业务类型" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                <el-table-column prop="goodsInfo" label="运送物料信息(Json字符串)" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="taskType" label="任务类型(出库或入库)" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                <el-table-column prop="actionType" label="AGV动作类型(取料箱/还料箱)" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                <el-table-column prop="isExecuting" label="是否处于执行中" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                <el-table-column prop="taskModel" label="任务模式(单次任务/多次任务)" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                <el-table-column prop="taskStatus" label="送料箱时任务状态" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                <el-table-column prop="isReturn" label="是否已还回料箱" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                <el-table-column prop="returnTaskStatus" label="还料箱时任务状态" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                <el-table-column prop="startingDeviceNo" label="起始地设备编号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="startingDeviceType" label="起始地设备类型" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="startingAGVPositionNo" label="起始地AGV仓位标识点" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="startingBinNo" label="起始地仓位编码" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="startingBinRank" label="起始地仓位序号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="destinationDeviceNo" label="目的地设备编号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="destinationDeviceType" label="目的地设备类型" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="destinationAGVPositionNo" label="目的地AGV仓位标识点" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="destinationBinNo" label="目的地仓位编码" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="destinationBinRank" label="目的地仓位序号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="aGVNo" label="AGV编号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="taskCreateTime" label="任务创建时间" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                <el-table-column prop="taskStartTime" label="任务开始时间" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                <el-table-column prop="taskEndTime" label="任务结束时间" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                <el-table-column prop="operatorId" label="操作人ID" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
                <el-table-column prop="operatorName" label="操作人姓名" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
        
                <el-table-column
          :label="$t('message.common.handle')"
          align="center" 
          width="120"
        >
          <template #default="scope">
            <!-- <el-button  type="primary"  @click="handleEdit(scope.row)">{{
              $t("message.common.update")
            }}</el-button> -->
          
            <el-button @click="btnStop(scope.row)" type="primary" >停止AGV任务</el-button> 
          </template>
        </el-table-column>

              </Table>

      </div>
    </div>

  </div>
  
  </Layer>
</template>

<script lang="ts">
import { defineComponent, ref, reactive } from "vue";
import { Page } from "@/components/table/type";
import store from '@/store'
import Layer from '@/components/layer/index.vue'  
import Table from "@/components/table/tableServer.vue";
import { LayerInterface } from "@/components/layer/index.vue";
import { ElForm } from 'element-plus';
import msg from '@/utils/system/message'
import { UploadFile } from 'element-plus/lib/el-upload/src/upload.type'
import permission from '@/utils/system/permission'
// import {getTextBookFileData,uploadFile,downloadFile} from '@/api/skill/textbook'
// import { getOrders } from "@/api/purchase/purchaseOrder";
import { getAGVList,updateAGVisExecuting } from "@/api/inv/requisition";
export default defineComponent({
  components: {
    Table,
    Layer,
  },
  props: {
    layer: {
      type: Object,
      default: () => {
        return {
          show: false,
          title: '',
          showButton: false,
          btnLoading:false,
          type:'',
          data:null
        }
      }
    },
    uploadParams: {
      type: Object,
      default: () => {
        return {
          uploadApi: '',
          limit: 1,
          imgUrlList: [],
          validFileType: '',
          validFileSize: 1024,
          isEdit: true,
          titile: '',
          width: '',
          height: ''
        }
      }
    }
  }, 
  setup(props, context) {  
    // let ruleForm = ref({
    //   textFileId:props.layer.row?.textFileId,
    //   textFileName: props.layer.row?.textFileName,
    //   textFileVer:props.layer.row?.textFileVer,
    //   publisLink: props.layer.row?.publisLink,
    // }) 


   let fullscreenLoading=ref(false);
   const headersObj = { authorization: store.getters['user/token'] };
   const ids=props.layer.row?.textFileId;
   const user=permission.getOperator().userName;


 

    
const query = reactive({
     types: "",
     input:""
});
 // 分页参数, 供table使用
 const tbHeght=ref('500')
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
    //排序事件
  
    // 获取表格数据
    // params <init> Boolean ，默认为false，用于判断是否需要初始化分页
    let getTableData = (init: Boolean) => {
      loading.value = true
      if (init) {
        page.index = 1
      }   
       loading.value = false
      //  getOrders(permission.getOperator().userId,page.size,page.index,page.orderField,page.orderType,"","","",0,"","SparePart",0)
      getAGVList(permission.getOperator().userId,page.size,page.index,page.orderField,page.orderType,"")
        .then((res) => {
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
    //停止AGV任务
    const btnStop=(row: any) =>{
      if(row.isExecuting)
      {
        updateAGVisExecuting(row).then(res=>{
            // unitLayer.show = false;
            getTableData(false);
          })
      }else
      {
        msg.warningAuto("此AGV任务未处于执行中!")
      }
      // historyLayer.title = "版本更新";
      // historyLayer.row = row; 
      // historyLayer.type='ver'
      // historyLayer.show = true;  
      // historyLayer.otherButton.show=false;
}


   

    getTableData(true);

    return {
      //ruleForm,
      headersObj,

      query,
      permission,
      tbHeght,
      loading,
      page, 
      tableData,
      chooseData,
      fullscreenLoading,
      btnStop,
      handleSelectionChange,
      handleSortChange,
      getTableData,
    }
  } 
})
</script>

<style lang="scss" scoped>
 * {
  text-align: left;
}

.el-select--mini {
  width: 100%;
}

.muilt-uploader {
  .el-upload-list--picture-card .el-upload-list__item {
    height: 90px !important;
    width: 90px !important;
    float: left !important;
  }

  .el-upload--picture-card {
    height: 90px !important;
    width: 90px !important;
    float: left !important;
    line-height: 90px !important;

    .el-icon {
      position: relative !important;
      top: 10px !important;
    }
  }
}
</style>