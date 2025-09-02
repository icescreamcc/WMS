<template>
    <div class="layout-container">
      <div class="layout-container-form flex space-between">
        <div class="layout-container-form-handle">
          <el-button v-if="permission.isPermisstion('PURCHASEADD')" type="primary" icon="el-icon-circle-plus-outline" @click="handleAdd">创建订单</el-button>
          <el-popconfirm title='确定删除选中的数据吗？' v-if="permission.isPermisstion('PURCHASEDEL')" @confirm="handleDel(chooseData)">
            <template #reference>
              <el-button type="danger"  icon="el-icon-delete" :disabled="chooseData.length === 0">批量删除</el-button>
            </template>
          </el-popconfirm>  
          <!-- <el-button type="success" v-if="permission.isPermisstion('PURCHASERECEIVEDWH')" :disabled="chooseData.length==0||!isBatchReceiving"><el-icon style="position: relative;top: 2px;right: 2px;"><ShoppingCartFull /></el-icon>批量实物收货</el-button>  -->
          <!-- <el-button type="success" v-if="permission.isPermisstion('PURCHASERECEIVEDWK')" :disabled="chooseData.length==0||!isBatchReceiving"><el-icon style="position: relative;top: 2px;right: 2px;"><ShoppingCartFull /></el-icon>批量WK收货</el-button>     -->
        <el-button icon="el-icon-download" type="info"  v-if="permission.isPermisstion('PURCHASEEXPORT')" style="margin-left:10px"  @click="getExportAllowField">导出</el-button> 
        </div>
        <div class="layout-container-form-search">
          <el-input  v-model="query.input" placeholder="请输入关键词进行检索" size="small" style="width: 80%;margin-left: 10px;" clearable @clear="getTableData(true)"></el-input>
          <el-button type="primary" icon="el-icon-search" class="search-btn" @click="getTableData(true)" >搜索</el-button>  
        </div>
      </div>
      <div class="layout-container-form flex space-between">
        <div class="layout-container-form-handle"></div>
        <div class="layout-container-form-search">
          <el-date-picker v-model="dateRange" type="daterange" range-separator="-" start-placeholder="最早创建日期" end-placeholder="最晚创建日期" size="small" value-format="YYYY-MM-DD" style="margin-left: 10px;;width:100%" @change="getTableData(true)"></el-date-picker>
          <el-select v-model="query.goodsGroup" ref="refSelectClassifyGroup" size="small"  class="m-2" style="width:90%;margin-left: 20px;" @change="onSelectGroup" placeholder="选择物料大类">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30.5px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">物料大类</div></template>        
            <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select>
          <el-select v-model="query.goodsClassifyId" ref="refSelectClassifyName" size="small"  class="m-2" style="width:90%;margin-left: 20px;" @change="getTableData(true)" placeholder="选择物料类型">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30.5px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">物料类型</div></template>        
            <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select>
          <el-select v-model="query.purchaseType" ref="refSelectPurchaseType" size="small"  class="m-2" style="width:90%;margin-left: 20px;" @change="getTableData(true)" placeholder="选择采购方式">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30.5px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">采购方式</div></template>        
            <el-option v-for="item in purchaseTypeData" :key="item.optionId" :label="item.optionName" :value="item.optionId"></el-option>
          </el-select>
          <el-select v-model="query.flowStatus" ref="refSelectStatus" size="small"  class="m-2" style="width:90%;margin-left: 20px;" @change="getTableData(true)" placeholder="选择订单状态">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30.5px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">订单状态</div></template>        
            <el-option v-for="item in orderFlowStatusData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select>  
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
        >   
        <el-table-column prop="orderNo" label="内部订单号" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true"/> 
          <el-table-column prop="purchaseTypeId" label="采购方式" align="center" sortable="custom" min-width="110" :show-overflow-tooltip="true">
            <template #default="props">
                <span>{{ props.row.purchaseTypeDesc }}</span>
              </template>
          </el-table-column>   
          <el-table-column prop="isApplyMaterialNumber" label="是否申请料号" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="props">
                <span v-if="props.row.isApplyMaterialNumber" class="text-primary">是</span>
                <span v-else>否</span>
              </template>
          </el-table-column> 
          <el-table-column prop="isPurchaseBuy" label="是否采买" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="props">
                <span v-if="props.row.isPurchaseBuy" class="text-primary">采买</span> 
                <span v-else>不采买</span>
              </template>
          </el-table-column>  
          <el-table-column prop="goodsNameZH" label="物料名称(中文)" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true"/> 
          <el-table-column prop="goodsNameEN" label="物料名称(英文)" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true"/> 
          <el-table-column prop="goodsModel" label="物料型号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="goodsNo" label="MNA编码" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="createDate" label="创建日期" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
            <template #default="props">
                <span>{{commonHelper.formatToDateTime(props.row.createDate)}}</span>
              </template>
           </el-table-column>  
          <el-table-column prop="quantity" label="采买数量" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"> 
              <template #default="props">
                <span>{{ props.row.quantity }}{{ props.row.quantityUnitName }}</span>
              </template>
          </el-table-column>    
          <el-table-column prop="flowStatus" label="状态" align="center" sortable="custom" min-width="190" :show-overflow-tooltip="true">
               <template #default="props">  
                <span v-if="props.row.flowStatus=='InStoraged'" class="text-success">{{ props.row.flowStatusDesc }}</span>
                <span  v-else-if="props.row.flowStatus.split('_')[0]=='Abnormal'" class="text-danger" style="margin-right: 10px;position: relative;bottom: -2px;" >{{ props.row.flowStatusDesc }}<el-icon v-if="permission.isPermisstion('PURCHASEUPDATEFLOWSTATUS')" @click="onEditFlowStatus(props.row)" title="点击编辑状态" style="cursor: pointer;margin-left: 5px;color: #1dcc09;position: relative;bottom: -2px"><EditPen /></el-icon></span>
                <span  v-else class="text-warning" style="margin-right: 10px;position: relative;bottom: -2px;" >{{ props.row.flowStatusDesc }}<el-icon v-if="permission.isPermisstion('PURCHASEUPDATEFLOWSTATUS')" @click="onEditFlowStatus(props.row)" title="点击编辑状态" style="cursor: pointer;margin-left: 5px;color: #1dcc09;position: relative;bottom: -2px"><EditPen /></el-icon></span>
              </template>
          </el-table-column>   
          <el-table-column  label="审批" v-if="hasApproval" align="center" min-width="100" :show-overflow-tooltip="true">
               <template #default="scope"> 
                <el-button v-if="scope.row.isApproval&&permission.isPermisstion('PURCHASEAPPROVAL')" @click="handleApproval(scope.row)" circle type="warning" style="height: 28px;" title="点击打开审批编辑界面" ><el-icon><Checked /></el-icon></el-button>
                <div v-else>
                  <span v-if="scope.row.approvalStatus=='Reject'" class="text-danger">{{ scope.row.approvalStatusDesc }}</span>
                  <span v-else-if="scope.row.approvalStatus=='Approve'" class="text-success">{{ scope.row.approvalStatusDesc }}</span>
                  <span v-else-if="scope.row.approvalStatus=='Approvaling'||scope.row.approvalStatus=='Pending'" class="text-warning">{{ scope.row.approvalStatusDesc }}</span>
                  <span v-else>{{ scope.row.approvalStatusDesc }}</span>
                </div>
              </template>
          </el-table-column>  
          <el-table-column prop="createUserName" label="创建人" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="consignee" label="需求者" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="arrivalStatus" label="实物到货状态" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="props">
                <span v-if="props.row.arrivalStatus=='Arrived'" class="text-success">{{ props.row.arrivalStatusDesc }}</span>
                <span v-else-if="props.row.arrivalStatus=='Abnormal'" class="text-danger" :title="props.row.arrivalAbnormalReason||''">{{ props.row.arrivalStatusDesc }}<img v-if="permission.isPermisstion('PURCHASERECEIVEDWH')&&props.row.approvalStatus=='Approve'" src="../../../../public/icon-img/shouhuo1.png" @click="onEditWarehouseReceiving(props.row)" height="16" style="position: relative;bottom: -3px;cursor: pointer;margin-left: 4px;" title="点击修改收货状态"></span>
                <span v-else>{{ props.row.arrivalStatusDesc }} <img v-if="permission.isPermisstion('PURCHASERECEIVEDWH')&&props.row.approvalStatus=='Approve'" src="../../../../public/icon-img/shouhuo1.png" @click="onEditWarehouseReceiving(props.row)" height="16" style="position: relative;bottom: -3px;cursor: pointer;margin-left: -2px;" title="点击确认实物收货"></span>
              </template>
           </el-table-column>   
           <el-table-column prop="receivingStatus" label="WK收货状态" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="props">
              <span v-if="props.row.receivingStatus=='Arrived'" class="text-success">{{ props.row.receivingStatusDesc }}</span>
                <span v-else-if="props.row.receivingStatus=='Abnormal'" class="text-danger" :title="'异常原因：'+props.row.receivingAbnormalReason||'未填写'">{{ props.row.receivingStatusDesc }}<img v-if="permission.isPermisstion('PURCHASERECEIVEDWK')&&props.row.approvalStatus=='Approve'" src="../../../../public/icon-img/wk2.png" @click="onEditWKReceiving(props.row)" height="16" style="position: relative;bottom: -3px;cursor: pointer;margin-left: 4px;" title="点击修改收货状态"></span>
                <span v-else>{{ props.row.receivingStatusDesc }} <img v-if="permission.isPermisstion('PURCHASERECEIVEDWK')&&props.row.approvalStatus=='Approve'" src="../../../../public/icon-img/wk2.png" @click="onEditWKReceiving(props.row)" height="15" style="position: relative;bottom: -3px;cursor: pointer;margin-left:3px;" title="点击确认WK收货"></span>
              </template>
           </el-table-column>  
          <el-table-column prop="receivingStatus" label="通知WK收货"  align="center" min-width="100"  :show-overflow-tooltip="true">
               <template #default="props">
                <el-button v-if="props.row.arrivalStatus=='Arrived'&&props.row.receivingStatus=='NoArrived'&&props.row.isEmailNotification"  style="height: 30px;" title="已发送邮件通知" @click="onReceivingMail(props.row)" circle>
                  <img src="../../../../public/icon-img/yidu1.png" height="14"> 
                </el-button>
                <el-button v-if="props.row.arrivalStatus=='Arrived'&&props.row.receivingStatus=='NoArrived'&&!props.row.isEmailNotification"  style="height: 30px;" title="未发送邮件通知" @click="onReceivingMail(props.row)" circle>
                  <img src="../../../../public/icon-img/xiaoxi4.png" height="14"> 
                </el-button> 
              </template>
          </el-table-column>     
         <el-table-column :label="$t('message.common.handle')" align="left"  min-width="300">
            <template #default="scope">  
              <el-button @click="handleRead(scope.row)">{{$t("message.common.read")}}</el-button>
              <el-button @click="handleCopy(scope.row)" type="info"  v-if="permission.isPermisstion('PURCHASEADD')">{{$t("message.common.copy")}}</el-button>
              <el-button @click="handleEdit(scope.row)" type="warning"   v-if="permission.getOperator().userId==scope.row.createUserId&&permission.isPermisstion('PURCHASEUPDATE')&&scope.row.status!='Approvaling'&&scope.row.status!='Received'&&scope.row.status!='WaitReceiving'">{{$t("message.common.update")}}</el-button>
              <el-popconfirm v-if="permission.getOperator().userId==scope.row.createUserId&&permission.isPermisstion('PURCHASEDEL')&&scope.row.status!='Approvaling'&&scope.row.status!='WaitReceiving'&&scope.row.status!='Received'" :title="$t('message.common.delTip')" @confirm="handleDel([scope.row])">
                <template #reference>
                  <el-button type="danger">{{ $t("message.common.del") }}</el-button>
                </template>
              </el-popconfirm>
            </template>
          </el-table-column>
        </Table>
        <OrderLaunchModal :layer="orderLaunchOption"  @dataSubmit="dataSave" v-if="orderLaunchOption.show" />   
        <OrderApprovingModal :layer="orderApprovingOption"  @dataSubmit="approvalSubmit" @dataSave="approvalSave" v-if="orderApprovingOption.show" />
        <FlowStatusModal :layer="flowStatusOption"  @dataSubmit="submitFlowStatus" v-if="flowStatusOption.show" />
        <WarehouseReceivingModal :layer="warehouseReceivingOption"  @dataSubmit="submitWarehouseReceived" v-if="warehouseReceivingOption.show"/>
        <WKReceivingModal :layer="wkReceivingOption"  @dataSubmit="submitWKReceived" v-if="wkReceivingOption.show"/>
        <OrderInfoModal :layer="orderReadOption" v-if="orderReadOption.show"/>
        <MailEditLayer  :layer="mailEditLayer"  @dataSubmit="onMailSubmit" v-if="mailEditLayer.show"/>  
        <ApprovalQuery :layer="approvalQueryLayer" v-if="approvalQueryLayer.show" /> 
        <ExportModal :layer="exportLayer"  @dataSubmit="exportData" v-if="exportLayer.show" /> 
      </div>
    </div>
  </template>
  
  <script lang="ts" setup>
   defineOptions({
    name: "purchase-order"
  })
  import { onMounted, ref, reactive } from "vue";
  import { Page } from "@/components/table/type";
  import { getArgs,getOrders,getOptions, getOrderDetail,addPurchaseOrder, updatePurchaseOrder,updateApprovalPurchaseOrder,delPurchaseOrder,adviceReceiving,submitReceivedWH,submitReceivedWK,updateFlowStatus,approvalPurchaseOrder,getApprovalHis,exportPurchaseData,getExportFields } from "@/api/purchase/purchaseOrder";
  import { LayerInterface } from "@/components/layer/index.vue"; 
  import Table from "@/components/table/tableServer.vue"; 
  import permission from '@/utils/system/permission';
  import {getGoodsGroup,getGoodsClassify} from '@/api/common';  
  import {Message,ShoppingCartFull,Checked,Promotion,View,EditPen ,Upload}  from '@element-plus/icons-vue';
  import msg from "@/utils/system/message"; 
  import { deftClassifyGroup} from '@/config';
  import commonHelper from "@/utils/system/common-helper"; 
  import OrderLaunchModal from "./orderLaunchLayer.vue"; 
  import OrderApprovingModal from "./orderApprovingLayer.vue"; 
  import FlowStatusModal from './flowStatusLayer.vue';
  import WarehouseReceivingModal from './warehouseReceivingLayer.vue';
  import WKReceivingModal from './wkReceivingLayer.vue';
  import OrderInfoModal from "./orderInfoLayer.vue";   
  import ApprovalQuery from '@/components/layer/approvalQueryLayer.vue'; 
  import ExportModal from "@/components/layer/exportLayer.vue";
  import MailEditLayer from "@/components/layer/mailLayer.vue";
 
      const query = reactive({
        input: "",
        goodsGroup:deftClassifyGroup||'',
        goodsClassifyId:'',
        purchaseType:'',
        flowStatus:''
      }); 
      const dateRange=ref()  
      const page: Page = reactive({
        index: 1,
        size: 20,
        total: 0,
        orderField:'',
        orderType:''
      });
      const orderLaunchOption: LayerInterface = reactive({
        show: false,
        title: "采购申请",
        showButton: true,
        btnLoading:false,
        width:"65%",
        data:null , 
        type:'',
        options:null,
        otherButton:{}
      });
      const orderApprovingOption: LayerInterface = reactive({
        show: false,
        title: "采购审批",
        showButton: true,
        btnLoading:false,
        width:"65%",
        data:null , 
        options:null,
        otherButton:{}
      });
      const flowStatusOption:LayerInterface=reactive({
        show: false,
        title: "流程状态编辑",
        showButton: true,
        btnLoading:false,
        width:"35%",
        data:null ,
        options:[],
        otherButton:{}
      });
      const warehouseReceivingOption: LayerInterface = reactive({
        show: false,
        title: "实物收货",
        showButton: true,
        btnLoading:false,
        width:"65%",
        data:null , 
        options:null,
        otherButton:{}
      });
      const wkReceivingOption: LayerInterface = reactive({
        show: false,
        title: "WK收货",
        showButton: true,
        btnLoading:false,
        width:"65%",
        data:null , 
        options:null,
        otherButton:{}
      });
      const orderReadOption: LayerInterface = reactive({
        show: false,
        title: "订单明细",
        showButton: true,
        btnLoading:false,
        width:"65%",
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
    const approvalQueryLayer:LayerInterface=reactive({
        show: false,
        title: "审批详情",
        showButton: false,
        btnLoading:false,
        width:"40%",
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
      const loading = ref(true);
      const tableData = ref([]); 
      const chooseData = ref([]); 
      const goodsGroupData=ref(new Array<any>());
      const goodsClassifyData=ref(new Array<any>());
      const orderStatusData=ref(new Array<any>());
      const orderFlowStatusData=ref(new Array<any>());
      const purchaseTypeData=ref(new Array<any>()); 
      const hasApproval=ref(false);  
      const isBatchReceiving=ref(false);
      const isBatchApproval=ref(false);
      const refSelectClassifyGroup=ref<null | HTMLElement>(null);

      onMounted(()=>{ 
        getArgsData();
        getOptionData(); 
        onSelectGroup();
        getTableData(true)
      })
      
      const getArgsData=()=>{
        getArgs().then(res=>{ 
          hasApproval.value=res.data.hasApproval 
        })
      }

      const getOptionData=()=>{
        getOptions().then((res)=>{  
          if(deftClassifyGroup=='SparePart'){
            goodsGroupData.value=res.data.goodsClassifyGroupOptions.filter((f:any)=>f.key=='SparePart'||f.key=='Consumables');
          }
          else{
            goodsGroupData.value=res.data.goodsClassifyGroupOptions;
          }  
          orderStatusData.value=res.data.orderStatusOptions;
          orderFlowStatusData.value=res.data.orderFlowStatusOptions; 
          purchaseTypeData.value=res.data.purchaseTypeOptions;
        })
      }

      const onSelectGroup=()=>{
          goodsClassifyData.value.length=0;
          query.goodsClassifyId=''; 
          getGoodsClassifyData();
          getTableData(true);
      }

      const getGoodsClassifyData=()=>{
          return getGoodsClassify(query.goodsGroup).then(res=>{
              goodsClassifyData.value=[{key:0,value:'All'},...res.data];
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
        getOrders(permission.getOperator().userId,page.size,page.index,page.orderField,page.orderType,query.input,dateStart,dateEnd,query.purchaseType?Number(query.purchaseType):0,query.flowStatus,query.goodsGroup,query.goodsClassifyId?Number(query.goodsClassifyId):0)
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
          let isReceivingOrder=val.filter((x:any)=>x.status=='WaitReceiving');
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
        delPurchaseOrder(ids).then((res) => { 
          getTableData(tableData.value.length === 1 ? true : false);
        });
      } 

      const onEditFlowStatus=(row:any)=>{
        getOrderDetail(row.detialId,permission.getOperator().userId).then(res=>{
          flowStatusOption.data=res.data;
          flowStatusOption.show=true;
          flowStatusOption.options=orderFlowStatusData.value;
        }) 
      }

      const submitFlowStatus=(data:any)=>{
        flowStatusOption.btnLoading=true;
         updateFlowStatus(data.detialId,data.flowStatus).then(()=>{
          flowStatusOption.show=false;
          getTableData(tableData.value.length === 1 ? true : false);
         }).finally(()=>{
          flowStatusOption.btnLoading=false;
         })
      }
     
      const onEditWarehouseReceiving=(row:any)=>{
        getOrderDetail(row.detialId,permission.getOperator().userId).then(res=>{
          warehouseReceivingOption.data=res.data;
          warehouseReceivingOption.show=true;
        }) 
      }

      const submitWarehouseReceived=(data:any)=>{
        warehouseReceivingOption.btnLoading=true;
        submitReceivedWH([data]).then(res=>{
          warehouseReceivingOption.show=false;
          getTableData(tableData.value.length === 1 ? true : false);
        }).finally(()=>warehouseReceivingOption.btnLoading=false);
      }

      const onEditWKReceiving=(row:any)=>{
        getOrderDetail(row.detialId,permission.getOperator().userId).then(res=>{
          wkReceivingOption.data=res.data;
          wkReceivingOption.show=true;
        })  
      }

      const submitWKReceived=(data:any)=>{
        wkReceivingOption.btnLoading=true;
          submitReceivedWK([data]).then(res=>{
            wkReceivingOption.show=false;
            getTableData(tableData.value.length === 1 ? true : false);
          }).finally(()=>wkReceivingOption.btnLoading=false);
      }

      const handleSubmitReceived=(data: any[])=>{
        let ids=Array<any>();
        data.forEach(d=>{
          ids.push(d.detialId);
        })  
      }

      const onReceivingMail=(row:any)=>{   
        getOrderDetail(row.detialId,permission.getOperator().userId).then((res:any)=>{
          let t=goodsGroupData.value.find(f=>f.key==query.goodsGroup).value;  
          let bodyTitle=`<div style="text-align: left;"> 
                          <div style="font-weight: 600;font-size:15px;">${t}采购订单收货提醒</div>
                          </div>`;
          let bodyContent='<div style="padding:3px 10px;border: 1px solid #ededed;">';
            let info=`<div style="padding:5px;">
                          <div style="width:100%;padding-top:3px;">物料：${res.data.goodsNameZH}(${res.data.goodsNameEN})</span>
                          <div style="width:100%;padding-top:3px;">型号：${res.data.goodsModel}</span>
                          <div style="width:100%;padding-top:3px;">数量：${res.data.quantity}${res.data.quantityUnitName}</span>
                          <div style="width:100%;padding-top:3px;">类型:${res.data.goodsClassifyName}</span>
                          <div style="width:100%;padding-top:3px;">供应商:${res.data.supplierName}</span> 
                        </div>`;
           bodyContent+=info;
           bodyContent+=`</div>`;
           let content=`<div>${bodyTitle}${bodyContent}</div>`
          mailEditLayer.data={
            subject:t+'收货通知',
            body:content,
            bodyType:'html',
            toReciver:[row.consigneeEmail],
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
        if(!query.goodsGroup){
          msg.deftAuto("请先选择物料大类");
          refSelectClassifyGroup.value?.focus(); 
          return;
        }
        orderLaunchOption.title = "创建采购订单";
        orderLaunchOption.show = true;  
        orderLaunchOption.type='add';
        orderLaunchOption.showButton=true; 
        orderLaunchOption.options={
          goodsGroup:query.goodsGroup 
        }
        delete orderLaunchOption.data;
      }
   
      const handleEdit = (row: any) => {  
        getOrderDetail(row.detialId,permission.getOperator().userId).then((res:any)=>{
           orderLaunchOption.title = "修改采购订单"; 
           orderLaunchOption.show = true; 
           orderLaunchOption.type='update';
           orderLaunchOption.showButton=true;
           orderLaunchOption.options={
            goodsGroup:query.goodsGroup,
            isDisabledGroup:true
          }
          orderLaunchOption.data=res.data 
        }) 
      }

      const handleCopy=(row:any)=>{
        getOrderDetail(row.detialId,permission.getOperator().userId).then((res:any)=>{
           orderLaunchOption.title = `创建采购订单（已复制订单:${row.orderNo}）`; 
           orderLaunchOption.show = true; 
           orderLaunchOption.showButton=true;
           orderLaunchOption.type='add';
           orderLaunchOption.options={
            goodsGroup:query.goodsGroup,
            isDisabledGroup:true
          }
          orderLaunchOption.data=res.data;
          orderLaunchOption.data.orderNo=''; 
        }) 
      }

      const handleRead = (row: any) => {  
        getOrderDetail(row.detialId,permission.getOperator().userId).then((res:any)=>{ 
          orderReadOption.show = true; 
          orderReadOption.showButton=false; 
          orderReadOption.data=res.data; 
        }) 
      }
       
      const dataSave=(data:any,actionType:string)=>{ 
        orderLaunchOption.btnLoading=true;
          if(actionType=='add'){
            addPurchaseOrder(data).then(res=>{
              orderLaunchOption.show = false;
              getTableData(true);
            }).finally(()=> orderLaunchOption.btnLoading=false);
          }
          else{
             updatePurchaseOrder(data).then(res=>{
              orderLaunchOption.show = false;
              getTableData(false);
            }).finally(()=> orderLaunchOption.btnLoading=false);
          }
      } 
 
   const handleApproval=(row:any)=>{  
     if(row.isApproval){
      getOrderDetail(row.detialId,permission.getOperator().userId).then((res:any)=>{
        orderApprovingOption.title = "审批采购订单"; 
        orderApprovingOption.show = true; 
        orderApprovingOption.showButton=true; 
        orderApprovingOption.data=res.data;
        if(res.data.isLastApproval){
          orderApprovingOption.otherButton={
            show:true,
            type:'warning',
            text:'保存',
            otherBtnLoading:false
          }
        } 
        })  
     } 
     else{
      msg.deftAuto("当前订单不属于待审批状态");
     }
   }
 
  const approvalSubmit=(data:any)=>{   
    orderApprovingOption.btnLoading=true
     approvalPurchaseOrder(data)
     .then(res=>{
      orderApprovingOption.show=false
       getTableData(false);
     })
     .finally(()=>orderApprovingOption.btnLoading=false)
   }

   const approvalSave=(data:any)=>{
    orderApprovingOption.otherButton.otherBtnLoading=true
    updateApprovalPurchaseOrder(data)
     .then(res=>{
      orderApprovingOption.show=false
       getTableData(false);
     })
     .finally(()=>orderApprovingOption.otherButton.otherBtnLoading=false)
   }
   
   const onShowApprovalInfo=(orderNo:string)=>{
    getApprovalHis(orderNo).then(res=>{
      approvalQueryLayer.data=res.data;
      approvalQueryLayer.show=true;
    }); 
   }
   
   const getExportAllowField=()=>{
    getExportFields().then(res=>{
      exportLayer.row = permission.getOperator().userId; 
      exportLayer.show = true;
      exportLayer.data=res.data; 
    }) 
  }

const exportData=(selField:Array<any>)=>{   
  let dateStart='';
  let dateEnd='';
  if(dateRange.value?.length>0){
    dateStart=dateRange.value[0]
  }
    if(dateRange.value?.length>1){
    dateEnd=dateRange.value[1]
  } 
  exportLayer.btnLoading=true;
  exportPurchaseData(page.orderField,page.orderType,query.input,dateStart,dateEnd,query.purchaseType?Number(query.purchaseType):0,query.flowStatus,query.goodsGroup,query.goodsClassifyId?Number(query.goodsClassifyId):0,selField).then(res=>{ 
    let link = document.createElement('a') 
      link.style.display = 'none'
      link.href =res.data
      document.body.appendChild(link)
      link.click() 
      document.body.removeChild(link)
      exportLayer.show=false;
  }).finally(()=>exportLayer.btnLoading=false)
}
  </script>
  
  <style lang="scss" scoped>
    .csm-link{
      cursor: pointer;
      text-decoration: underline;
    }
  </style> 