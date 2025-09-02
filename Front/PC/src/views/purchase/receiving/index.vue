<template>
    <div class="layout-container">
      <div class="layout-container-form flex space-between" style=" display: block; /* 确保包裹容器是块级元素 */">
        <div class="layout-container-form-handle">
          <el-button v-if="permission.isPermisstion('RECEIVINGADD')" type="primary" icon="el-icon-circle-plus-outline" @click="handleAdd">新增</el-button>
          <el-popconfirm title='确定删除选中的数据吗？' v-if="permission.isPermisstion('RECEIVINGDEL')" @confirm="handleDel(chooseData)">
            <template #reference>
              <el-button type="danger"  icon="el-icon-delete" :disabled="chooseData.length === 0">批量删除</el-button>
            </template>
          </el-popconfirm>  
          <el-button type="success" v-if="permission.isPermisstion('RECEIVINGAFFIRM')" @click="onShowReceivingQtyEdit(chooseData,true)" :disabled="chooseData.length==0||!isBatchReceiving"><el-icon style="position: relative;top: 2px;right: 2px;"><ShoppingCartFull /></el-icon>批量收货</el-button> 
          <el-popconfirm title='是否确定审批所选中的数据？' v-if="permission.isPermisstion('RECEIVINGAPPROVAL')&&hasApproval" @confirm="handleApproval(chooseData)">
          <template #reference>
            <el-button type="warning" :disabled="chooseData.length==0||!isBatchApproval"><el-icon style="position: relative;top: 2px;right: 2px;"><Checked /></el-icon>批量审批</el-button>
          </template></el-popconfirm> 
          <el-button icon="el-icon-download"  style="margin-left:10px"  @click="getImportTemplate">模板下载</el-button>
          <el-button icon="el-icon-upload"  style="margin-left:10px"  @click="showImportModal">导入计划</el-button> 

          <el-form :inline="true" class="demo-form-inline">
            <el-form-item 
              label="" 
              prop="sumWorkpieceTray" 
              style="width: 50%"
              :label-style="{ color: 'red', fontWeight: 'bold' }">
            <span style="color: red; font-weight: bold;">总料盘数：{{query.sumWorkpieceTray}}</span>
          </el-form-item>
  
            <el-form-item 
              label="" 
              prop="sumPallet" 
              style="width: 50%"
              :label-style="{ color: 'red', fontWeight: 'bold' }">
             <span style="color: red; font-weight: bold;">总托盘数：{{query.sumPallet}}</span>
            </el-form-item>
          </el-form>
       
        </div>

        <div>&nbsp;&nbsp;</div>
        <div class="layout-container-form-search">
          <el-select v-model="selectedGoodsGroup" ref="refSelectClassifyGroup" size="small" :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;" @change="getTableData(true)" placeholder="选择物品大类">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              分类</div></template>        
            <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select>

          <el-select v-model="query.receivingLevel" ref="refSelectClassifyGroup" size="small" :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;" @change="getTableData(true)" filterable clearable placeholder="选择优先级">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              优先级</div></template>        
            <el-option v-for="item in receivingLevelData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select>

          <el-select v-model="query.createUserNames" ref="refSelectClassifyGroup" size="small" :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;" @change="getTableData(true)" filterable clearable placeholder="选择计划员">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              计划员</div></template>        
            <el-option v-for="item in createUserNameData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select>

          <el-select v-model="query.detailStatus" ref="refSelectClassifyGroup" size="small" :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;" @change="getTableData(true)" filterable clearable placeholder="选择单据状态">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              单据状态</div></template>        
            <el-option v-for="item in detailStatusData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select>

          <el-date-picker v-model="dateRange" type="daterange" range-separator="-" start-placeholder="最早到货日期" end-placeholder="最晚到货日期" size="small" value-format="YYYY-MM-DD" style="margin-left: 10px;;width:100%" @change="getTableData(true)"></el-date-picker>
          <el-input  v-model="query.input" placeholder="请输入关键词进行检索" size="small" style="width: 80%;margin-left: 10px;" clearable @clear="getTableData(true)"></el-input>
          <el-button type="primary" icon="el-icon-search" class="search-btn" @click="getTableData(true)" >搜索</el-button> 
          <el-button icon="el-icon-download" type="info"  v-if="permission.isPermisstion('RECEIVINGSAPEXPORT')" style="margin-left:10px"  @click="getExportAllowField">导出计划</el-button>  
        </div>

      </div>
      <div class="layout-container-table">
        <Table
          ref="table"
          v-model:page="page"
          v-loading="loading"
          :showSelection="true"
          :data="tableData" 
          @getTableData="getTableData"
          @selection-change="handleSelectionChange"
          @orderChanged="handleSortChange"  
          @expandChange="handleExpandChange"
        > 
          <el-table-column prop="goodsNo" label="物料号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="externalOrderNo" label="SAP订单号" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
          <el-table-column prop="receivingLevel" label="优先级" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
             <template #default="props">
                <span v-if="props.row.receivingLevel=='Urgent_Especial'" class="text-danger">{{props.row.receivingLevelDesc}}</span>
                <span v-else-if="props.row.receivingLevel=='Urgent_General'" class="text-warning" >{{props.row.receivingLevelDesc}}</span>
                 <span v-else >{{props.row.receivingLevelDesc}}</span>
              </template>
           </el-table-column> 
          <el-table-column prop="createUserName" label="计划员" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="isASN" label="ASN收货" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"> 
              <template #default="props">
                <span v-if="props.row.isASN">是</span>
              </template>
          </el-table-column>   
          <!-- <el-table-column prop="asnCheckStatus" label="ASN Check" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>  -->
          <el-table-column prop="asnCheckStatus" label="ASN Check" align="center" sortable="custom" min-width="110" :show-overflow-tooltip="true">
               <template #default="props"> 
                <span v-if="props.row.asnCheckStatus=='Y'" class="text-success">是</span>
                <!-- <span v-else-if="props.row.asnCheckStatus=='N'" class="text-warning">否</span>  -->
              </template>
          </el-table-column> 


          <el-table-column prop="downTime" label="预计停线时间" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="props">
                <span>{{commonHelper.formatToDateTime(props.row.downTime)}}</span>
              </template>
          </el-table-column> 
          <el-table-column prop="waybillNo" label="运单号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>   
          <el-table-column prop="goodsClassifyName" label="物料类别" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
              <template #default="props">
                <span>{{props.row.goodsClassifyName}}</span>
              </template>
          </el-table-column>     
          <el-table-column prop="supplierName" label="供应商" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
           <el-table-column prop="quantity" label="数量" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
             <template #default="props">
                <span>{{props.row.quantity+props.row.quantityUnitName}}</span>
              </template>
           </el-table-column> 
           <el-table-column prop="quantityUrgency" label="紧急数量" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
             <template #default="props">
              <span >{{props.row.quantityUrgency+props.row.quantityUnitName}}</span> 
              <span  v-if="props.row.receivingLevel=='Urgent_Especial'" @click="onShowUrgencyQtyEdit(props.row)" style="cursor: pointer;margin-left: 10px;position: relative;bottom: -2px;" title="点击编辑紧急数量"><el-icon><EditPen /></el-icon></span>
            </template>
           </el-table-column> 
           <el-table-column prop="workpieceTray" label="料盘数" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
           <el-table-column prop="pallet" label="托数/桶数" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
           <el-table-column prop="abnormalDeliveryPallet" label="异常到货托数" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
           <el-table-column prop="expectDate" label="预计到货日期" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
             <template #default="props">
                <span>{{commonHelper.formatToDate(props.row.expectDate)}}</span>
              </template>
           </el-table-column>  
          <el-table-column prop="detailStatus" label="状态" align="center" sortable="custom" min-width="110" :show-overflow-tooltip="true">
               <template #default="props"> 
                <span v-if="props.row.detailStatus=='WaitReceiving'" class="text-primary">{{ props.row.detailStatusDesc }}</span>
                <span v-else-if="props.row.detailStatus=='ReceivingAbnormal'||props.row.detailStatus=='Reject'" class="text-danger">
                  <span v-if="props.row.detailStatus=='Reject'" class="csm-link"  @click="onShowApprovalInfo(props.row.orderNo)">{{props.row.detailStatusDesc}}</span> 
                  <span v-else >{{props.row.detailStatusDesc}}</span>
                </span>
                <span v-else-if="props.row.detailStatus=='Pending'||props.row.detailStatus=='Approvaling'" class="text-warning">
                  <span v-if="props.row.detailStatus=='Approvaling'" class="csm-link" @click="onShowApprovalInfo(props.row.orderNo)">{{props.row.detailStatusDesc}} </span>
                  <span v-else>{{props.row.detailStatusDesc}}</span> 
                </span> 
                <span v-else class="text-success">{{ props.row.detailStatusDesc }}</span>
              </template>
          </el-table-column> 
          <el-table-column prop="detailStatus" label="通知收货" v-if="permission.isPermisstion('RECEIVINGNOTIFICATION')" align="center" min-width="100"  :show-overflow-tooltip="true">
               <template #default="props">
                <el-button v-if="props.row.detailStatus=='WaitReceiving'&&props.row.isEmailNotification"  style="height: 30px;" title="已发送邮件通知" @click="onReceiving(props.row)" circle>
                  <img src="../../../../public/icon-img/yidu1.png" height="14"> 
                </el-button>
                <el-button v-if="props.row.detailStatus=='WaitReceiving'&&!props.row.isEmailNotification"  style="height: 30px;" title="未发送邮件通知" @click="onReceiving(props.row)" circle>
                  <img src="../../../../public/icon-img/xiaoxi4.png" height="14"> 
                </el-button> 
              </template>
          </el-table-column> 
          <!-- <el-table-column prop="receivingAbnormalType" :v-show="false" label="异常到货" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
             <template #default="props">
                <el-button v-if="!props.row.receivingAbnormalType" style="height: 30px;"  title="点击编辑异常到货信息" @click="onShowReceivingAbnormal(props.row)" circle>
                  <img src="../../../../public/icon-img/yichangchuli_1.png" height="12">
                </el-button>
                <span v-else class="csm-link text-danger"  title="点击编辑异常到货信息" @click="onShowReceivingAbnormal(props.row)">{{props.row.receivingAbnormalType}}</span>
              </template>
           </el-table-column>  -->
          <el-table-column  label="确认收货" v-if="permission.isPermisstion('RECEIVINGAFFIRM')" align="center" min-width="100" :show-overflow-tooltip="true">
               <template #default="props">
                <el-button v-if="props.row.detailStatus=='WaitReceiving'" circle type="success" style="height: 30px;"  @click="onShowReceivingQtyEdit([props.row],true)" title="点击确认收货"><el-icon><ShoppingCartFull /></el-icon></el-button> 
                <!-- <span v-else>{{ props.row.quantityActual+props.row.quantityUnitName}}</span> -->
                <span v-else-if="props.row.detailStatus=='ReceivingAbnormal'" class="csm-link text-danger"  title="点击查看异常到货信息" @click="onShowReceivingQtyEdit([props.row],false)">{{props.row.receivingAbnormalTypeName}}</span>
                <span v-else>{{ props.row.quantityActual+props.row.quantityUnitName}}</span>
              </template>
          </el-table-column>  
          
          <el-table-column prop="remark" label="备注" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
          <el-table-column prop="createDate" label="创建时间" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
          <el-table-column  label="审批" v-if="hasApproval&&permission.isPermisstion('RECEIVINGAPPROVAL')" align="center" min-width="100" :show-overflow-tooltip="true">
               <template #default="scope">
                <el-button @click="handleApproval([scope.row])" circle type="warning" style="height: 30px;" title="点击打开审批编辑界面" v-if="scope.row.isApproval"><el-icon><Checked /></el-icon></el-button>
              </template>
          </el-table-column> 

         <el-table-column :label="$t('message.common.handle')" align="left"  min-width="220" v-if="permission.isPermisstion('RECEIVINGUPDATE','RECEIVINGDEL','RECEIVINGAPPROVAL')">
            <template #default="scope">  
              <el-button @click="handleRead(scope.row)">{{$t("message.common.read")}}</el-button>
              <el-button @click="handleEdit(scope.row)"   v-if="permission.isPermisstion('RECEIVINGUPDATE')&&(scope.row.detailStatus=='WaitReceiving'||scope.row.detailStatus=='Pending')">{{$t("message.common.update")}}</el-button>
              <el-popconfirm v-if="permission.isPermisstion('RECEIVINGDEL')&&(scope.row.detailStatus=='WaitReceiving'||scope.row.detailStatus=='Pending')" :title="$t('message.common.delTip')" @confirm="handleDel([scope.row])">
                <template #reference>
                  <el-button type="danger">{{ $t("message.common.del") }}</el-button>
                </template>
              </el-popconfirm> 
              <el-button v-if="scope.row.isInBaseFiles === true" @click="onShowReceivingQtyEdit([scope.row], false)">图片</el-button>
            </template>
          </el-table-column>
        </Table>
        <OrderEditModal :layer="orderLayer"  @dataSubmit="dataSave" v-if="orderLayer.show" />   
        <MailEditLayer  :layer="mailEditLayer"  @dataSubmit="onMailSubmit" v-if="mailEditLayer.show"/>
        <ApprovalModal :layer="approvalLayer"  @dataSubmit="approvalSubmit" v-if="approvalLayer.show" />
        <ApprovalQuery :layer="approvalQueryLayer" v-if="approvalQueryLayer.show" />
        <OrderAbnormalLayer :layer="orderAbnormalLayer" @dataSubmit="onSubmitReceivingAbnormal" v-if="orderAbnormalLayer.show"/>
        <ExportModal :layer="exportLayer"  @dataSubmit="exportData" v-if="exportLayer.show" />
        <ImportModal :layer="importLayer" @dataSubmit="uploadSuccess" v-if="importLayer.show" />
        <UrgencyEditModal :layer="urgencyEditLayer" @dataSubmit="onSubmitModifyUrgencyQty" v-if="urgencyEditLayer.show" />
        <ReceivingModal :layer="receivingEditLayer" @dataSubmit="onSubmitModifyReceivingQty" v-if="receivingEditLayer.show" />
      </div>
    </div>
  </template>
  
  <script lang="ts" setup>
   defineOptions({
    name: "receiving"
  })
  import { onMounted, ref, reactive } from "vue";
  import { Page } from "@/components/table/type";
  import { getArgs,getOrders, getOrderDetail,getReceivingLevelGroup,getCreateUserNameGroup,getDetailStatusGroup,addReceivingOrder, updateReceivingOrder,delReceivingOrder,adviceReceiving,submitReceived,approvalReceivingOrder,getApprovalHis,updateReceivingAbnormal,updateReceivingUrgency,createImportTemplate,exportReceivingData,getExportFields } from "@/api/purchase/receivingorder";
  import { LayerInterface } from "@/components/layer/index.vue"; 
  import Table from "@/components/table/tableServer.vue";
  import OrderEditModal from "./orderEditLayer.vue";   
  import permission from '@/utils/system/permission';
  import {getGoodsGroup} from '@/api/common'; 
  import MailEditLayer from "@/components/layer/mailLayer.vue";
  import {Message,ShoppingCartFull,Checked,Promotion,View,EditPen ,Upload}  from '@element-plus/icons-vue';
  import msg from "@/utils/system/message";
  import ApprovalModal from '@/components/layer/approvalLayer.vue'; 
  import ApprovalQuery from '@/components/layer/approvalQueryLayer.vue';
  import OrderAbnormalLayer from'./orderAbnormalLayer.vue';
  import { deftClassifyGroup ,disableClassifyGroupSelect} from '@/config';
  import commonHelper from "@/utils/system/common-helper";
  import ExportModal from "@/components/layer/exportLayer.vue"; 
  import ImportModal from "@/components/layer/importLayer.vue";
  import UrgencyEditModal from "./urgencyEditLayer.vue";
  import ReceivingModal from "./orderReceivingLayer.vue";
 
      const query = reactive({
        input: "",
        receivingLevel:"",
        createUserNames:"",
        detailStatus:"",
        sumWorkpieceTray:"",
        sumPallet:""
      });
      const dateRange=ref()  
      const page: Page = reactive({
        index: 1,
        size: 20,
        total: 0,
        orderField:'',
        orderType:''
      });
      const orderLayer: LayerInterface = reactive({
        show: false,
        title: "",
        showButton: true,
        btnLoading:false,
        width:"70%",
        data:null , 
        options:null,
        otherButton:{}
      });
      const mailEditLayer:LayerInterface=reactive({
        show: false,
        title: "收货邮件通知",
        showButton: true,
        btnLoading:false,
        width:"65%",
        data:null ,
        otherButton:{}
      })
      const approvalLayer:LayerInterface=reactive({
        show: false,
        title: "收货计划审批",
        showButton: true,
        btnLoading:false,
        width:"50%",
        data:null ,
        otherButton:{}
    });
    const approvalQueryLayer:LayerInterface=reactive({
        show: false,
        title: "审批详情",
        showButton: false,
        btnLoading:false,
        width:"50%",
        data:null ,
        otherButton:{}
    });
    const orderAbnormalLayer:LayerInterface=reactive({
        show: false,
        title: "选择异常到货类型",
        showButton: true,
        btnLoading:false,
        width:"45%",
        data:null ,
        otherButton:{}
    });
    const urgencyEditLayer:LayerInterface=reactive({
        show: false,
        title: "紧急数量",
        showButton: true,
        btnLoading:false,
        width:"45%",
        data:null ,
        otherButton:{}
    });
    const receivingEditLayer:LayerInterface=reactive({
        show: false,
        title: "确认收货",
        showButton: true,
        btnLoading:false,
        width:"65%",
        data:null ,
        otherButton:{}
    });
    const exportLayer: LayerInterface = reactive({
        show: false,
        title: "选择导出字段",
        showButton: true,
        btnLoading:false,
        width:"45%",
        data:null,
        otherButton:{ }
      });
      const importLayer: LayerInterface = reactive({
        show: false,
        title: "导入文件",
        showButton: false,
        btnLoading:false,
        width:"45%",
        apiurl:"/ReceivingOrder/ImportReceivingData",
        data:null,
        otherButton:{ }
      });
  
      const loading = ref(true);
      const tableData = ref([]);
      const chooseData = ref([]); 
      const goodsGroupData=ref(new Array<any>());
      const receivingLevelData=ref(new Array<any>());
      const createUserNameData=ref(new Array<any>());
      const detailStatusData=ref(new Array<any>());
      const selectedGoodsGroup=ref(deftClassifyGroup||"RawMaterial");
      const hasApproval=ref(false); 
      const receivingAbnormalTypeData=ref(new Array<any>());
      const isBatchReceiving=ref(false);
      const isBatchApproval=ref(false);
      const refSelectClassifyGroup=ref<null | HTMLElement>(null);

      onMounted(()=>{ 
        getCreateUserNameGroupData();
        getArgsData();
        getGoodsGroupData();
        getReceivingLevelGroupData();
        getDetailStatusGroupData();
        getTableData(true);
      })
      
      const getArgsData=()=>{
        getArgs().then(res=>{ 
          hasApproval.value=res.data.hasApproval
          receivingAbnormalTypeData.value=res.data.receivingAbnormalTypeOptions.argsOptions
        })
      }
      //加载下拉框
      const getGoodsGroupData=()=>{
        getGoodsGroup().then(res=>{ 
          goodsGroupData.value=res.data;  
        })
      }
      
      const getReceivingLevelGroupData=()=>{
        getReceivingLevelGroup().then(res=>{ 
          receivingLevelData.value=res.data;  
        })
      }

      const getCreateUserNameGroupData=()=>{
        getCreateUserNameGroup().then(res=>{ 
          createUserNameData.value=res.data; 
         var tt=query.createUserNames;
        })
      }

      const getDetailStatusGroupData=()=>{
        getDetailStatusGroup().then(res=>{ 
          detailStatusData.value=res.data;  
        })
      }

      const getTableData = (init: Boolean) => {
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
         loading.value = false
        getOrders(permission.getOperator().userId,page.size,page.index,page.orderField,page.orderType,query.input,dateStart,dateEnd,selectedGoodsGroup.value,query.receivingLevel,query.createUserNames,query.detailStatus)
          .then((res) => {
            let data = res.data.rows
            data.forEach((d: any) => {
              d.loading = false
            })
            tableData.value = data
            page.total = Number(res.data.total);
            query.sumWorkpieceTray=res.data.sum;
            query.sumPallet=res.data.count;
          })
          .catch((error) => {
            tableData.value = [];
            page.index = 1;
            page.total = 0;
          })
          .finally(() => {
            loading.value = false;
          })
      } 
   
      const detailList=ref(new Array<any>()) 
      const handleExpandChange=(row:any)=>{  
         getOrderDetail(permission.getOperator().userId, row.orderNo).then(res=>{
              detailList.value=detailList.value.filter((b:any)=>b.orderNo!=row.orderNo)  
              res.data?.forEach((b:any) => {
                  detailList.value.push(b)
                });   
         }) 
      }
   
      const handleSortChange=(orderRow:any)=>{ 
        getTableData(true);
      }
   
       const handleSelectionChange = (val: []) => {
        chooseData.value = val;
        if(chooseData.value.length>0){
          let isApprovalOrder=val.filter((x:any)=>x.isApproval);
          if(isApprovalOrder.length!=val.length){
          isBatchApproval.value=false;
          }
          else{
            isBatchApproval.value=true;
          }
          let isReceivingOrder=val.filter((x:any)=>x.detailStatus=='WaitReceiving');
          if(isReceivingOrder.length!=val.length){
            isBatchReceiving.value=false;
          }
          else{
            isBatchReceiving.value=true;
          } 
        } 
      };
  
      const handleDel = (data: any[]) => {  
        let ids=Array<any>();
        data.forEach(d=>{
          ids.push(d.detialId);
        }) 
        delReceivingOrder(ids).then((res) => { 
          getTableData(tableData.value.length === 1 ? true : false);
        });
      } 
     
      const onReceiving=(row:any)=>{  
        getOrderDetail(row.orderNo,permission.getOperator().userId).then((res:any)=>{
          let t=goodsGroupData.value.find(f=>f.key==selectedGoodsGroup.value).value;  
          let bodyTitle=`<div style="text-align: left;"> 
                          <div style="font-weight: 600;font-size:15px;">${res.data.createUserName}创建了一份${t}收货计划，预计收货日期为${commonHelper.formatToDate(res.data.expectDate)}</div>
                          </div>`;
          let bodyContent='<div style="padding:3px 10px;border: 1px solid #ededed;">';
           res.data.details.forEach((d:any) => {
            let info=`<div style="padding:5px;">
                          <div style="width:100%;padding-top:3px;">物料：${d.goodsNo}</span>
                          <div style="width:100%;padding-top:3px;">数量：${d.quantity}${d.quantityUnitName}</span>
                          <div style="width:100%;padding-top:3px;">类型:${d.goodsClassifyName}</span>
                          <div style="width:100%;padding-top:3px;">供应商:${d.supplierName}</span>
                          <div style="width:100%;padding-top:3px;">优先级:${d.receivingLevelDesc}</span>
                        </div>`;
              bodyContent+=info; 
           });
           bodyContent+=`</div>`;
           let content=`<div>${bodyTitle}${bodyContent}</div>`
          mailEditLayer.data={
            subject:t+'收货通知',
            body:content,
            bodyType:'html',
            toReciver:[row.receivingResponsableUserEmail],
            toCC:[],
            orderNo:row.orderNo, 
          }
          mailEditLayer.show=true;
        });  
      }

      const onMailSubmit=(data:any)=>{
        mailEditLayer.btnLoading=true;
        adviceReceiving(data.orderNo,data)
        .then(()=>{
          msg.successAuto("已发送通知收货邮件到收货责任人")
          mailEditLayer.show=false;
          getTableData(false);
        })
        .finally(()=>mailEditLayer.btnLoading=false);
      }
     
  
      const handleAdd = () => {
        if(!selectedGoodsGroup.value){
          msg.deftAuto("请先选择物料分类");
          refSelectClassifyGroup.value?.focus(); 
          return;
        }
        orderLayer.title = "新增收货计划";
        orderLayer.show = true;  
        orderLayer.showButton=true; 
        orderLayer.options={
          goodsGroup:selectedGoodsGroup.value,
          isDisabledGroup:disableClassifyGroupSelect
        }
        delete orderLayer.data;
      }
   
      const handleEdit = (row: any) => {  
        getOrderDetail(row.orderNo,permission.getOperator().userId).then((res:any)=>{
           orderLayer.title = "编辑收货计划"; 
           orderLayer.show = true; 
           orderLayer.showButton=true;
           orderLayer.options={
            goodsGroup:selectedGoodsGroup.value,
            isDisabledGroup:true
          }
          orderLayer.data=res.data 
        }) 
      }

      const handleRead = (row: any) => {  
        getOrderDetail(row.orderNo,permission.getOperator().userId).then((res:any)=>{
           orderLayer.title = "查看收货计划"; 
           orderLayer.show = true; 
           orderLayer.showButton=false;
           orderLayer.options={
            goodsGroup:selectedGoodsGroup.value,
            isDisabledGroup:true
          }
          orderLayer.data=res.data 
        }) 
      }
       
      const dataSave=(data:any,actionType:string)=>{ 
        orderLayer.btnLoading=true;
          if(actionType=='add'){
            data.createUserId=permission.getOperator().userId,
            data.createUserName=permission.getOperator().userName, 
            addReceivingOrder(data).then(res=>{
              orderLayer.show = false;
              getTableData(true);
            }).finally(()=> orderLayer.btnLoading=false);
          }
          else{
            data.updateUserName = permission.getOperator().userName;
            data.updateUserId = permission.getOperator().userId;
             updateReceivingOrder(data).then(res=>{
              orderLayer.show = false;
              getTableData(false);
            }).finally(()=> orderLayer.btnLoading=false);
          }
      } 
 
   const handleApproval=(data: any[])=>{ 
     let isApprovalOrderNo=data.filter(x=>x.isApproval).map(x=>{return x.orderNo}); 
     if(isApprovalOrderNo.length==data.length){
       approvalLayer.show = true;  
       approvalLayer.data=data;
     } 
     else{
      msg.deftAuto("所选数据中存在不符合审批条件的数据");
     }
   }
 
  const approvalSubmit=(orders:any[],data:any)=>{  
     let orderNo=orders.filter(x=>x.isApproval).map(x=>{return x.orderNo});
     let uniqueOrderNo = Array.from(new Set(orderNo));  
     approvalLayer.btnLoading=true
     approvalReceivingOrder(uniqueOrderNo,data.isApprove,data.opinion,permission.getOperator().userId,permission.getOperator().userName)
     .then(res=>{
       approvalLayer.show=false
       getTableData(false);
     })
     .finally(()=>approvalLayer.btnLoading=false)
   }
   
   const onShowApprovalInfo=(orderNo:string)=>{
    getApprovalHis(orderNo).then(res=>{
      approvalQueryLayer.data=res.data;
      approvalQueryLayer.show=true;
    }); 
   }

   const onShowReceivingAbnormal=(row:any)=>{
    getOrderDetail(row.orderNo,permission.getOperator().userId).then(res=>{
      orderAbnormalLayer.data=receivingAbnormalTypeData.value;
      orderAbnormalLayer.show=true;
      orderAbnormalLayer.selected={
        detailId:row.detialId,
        abnormalType:res.data.details.find((f:any)=>f.detialId==row.detialId)?.receivingAbnormalType,
        abnormalDesc:res.data.details.find((f:any)=>f.detialId==row.detialId)?.receivingAbnormalDesc
      };
    }) 
   }

   const onSubmitReceivingAbnormal=(selected:any)=>{
      orderAbnormalLayer.btnLoading=true;
      updateReceivingAbnormal(selected.detailId,selected.abnormalType,selected.abnormalDesc).then(res=>{
        orderAbnormalLayer.show=false; 
        getTableData(false);
      }).finally(()=>{orderAbnormalLayer.btnLoading=false;})
   }

   const onShowUrgencyQtyEdit=(item:any)=>{
    urgencyEditLayer.data=item;
    urgencyEditLayer.show=true;
   }

   const onSubmitModifyUrgencyQty=(item:any)=>{ 
    var goods:any=tableData.value.find((f:any)=>f.goodsId==item.goodsId);  
    urgencyEditLayer.btnLoading=true;
      updateReceivingUrgency(goods.detialId,item.quantityUrgency).then(res=>{
        goods.quantityUrgency=item.quantityUrgency;
      }).finally(()=>urgencyEditLayer.btnLoading=false);
   }

   const onShowReceivingQtyEdit=(items:Array<any>,isShowButton:boolean)=>{
    receivingEditLayer.data=items;
    receivingEditLayer.show=true;
    receivingEditLayer.showButton=isShowButton;
   }

   //确认收货
   const onSubmitModifyReceivingQty=(data:Array<any>)=>{ 
    receivingEditLayer.btnLoading=true;
    submitReceived(data).then(res=>{
      receivingEditLayer.show=false;
          getTableData(true);
        }).finally(()=>receivingEditLayer.btnLoading=false);
   }
 
  const getImportTemplate=()=>{
    createImportTemplate().then(res=>{
      let link = document.createElement('a') 
      link.style.display = 'none'
      link.href =res.data
      document.body.appendChild(link)
      link.click() 
      document.body.removeChild(link)
    })
  }

  const uploadSuccess=()=>{
    importLayer.show=false;
    getTableData(true);
  }

  const showImportModal=()=>{
    importLayer.show=true;
  } 

   const getExportAllowField=()=>{
    getExportFields().then(res=>{
      exportLayer.row = permission.getOperator().userId; 
      exportLayer.show = true;
      exportLayer.data=res.data; 
    }) 
  }

const exportData=(selField:Array<any>)=>{  
  exportLayer.btnLoading=true;
  let dateStart='';
  let dateEnd='';
  if(dateRange.value?.length>0){
    dateStart=dateRange.value[0]
  }
    if(dateRange.value?.length>1){
    dateEnd=dateRange.value[1]
  }
  
  exportReceivingData(page.orderField,page.orderType,query.input,dateStart,dateEnd,selectedGoodsGroup.value,selField).then(res=>{ 
    let link = document.createElement('a') 
      link.style.display = 'none'
      link.href =res.data
      document.body.appendChild(link)
      link.click() 
      document.body.removeChild(link)
  }).finally(()=>exportLayer.btnLoading=false)
}
  </script>
  
  <style lang="scss" scoped>
    .csm-link{
      cursor: pointer;
      text-decoration: underline;
    }
  </style> 