<template>
    <div class="layout-container">
      <div class="layout-container-form flex space-between">
        <div class="layout-container-form-handle">  
        <el-button type="warning"  v-if="permission.isPermisstion('SAFETYINVENTORYAPPROVAL')&&hasApproval" @click="handleApproval(chooseData)" :disabled="chooseData.length==0||!isBatchApproval"><el-icon style="position: relative;top: 2px;right: 2px;"><Checked /></el-icon>批量审批</el-button>
          <el-button icon="el-icon-download" v-if="permission.isPermisstion('SAFETYINVENTORYEXPORT')"  style="margin-left:20px"  @click="getExportAllowField">导出</el-button>
        </div>
        <div class="layout-container-form-search">
          <el-select v-model="query.goodsGroup" ref="refSelectClassifyGroup" size="small"  class="m-2" style="width:100%;margin-right:10px;" @change="getGoodsClassifyData" placeholder="选择物品大类">
          <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">分类</div></template>        
          <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select> 
          <el-select v-model="query.goodsClassifyType"  size="small" class="m-2" style="width:100%;margin-right:10px;" @change="getTableData(true)" placeholder="选择物品类型">
          <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">类型</div></template>        
          <el-option v-for="item in goodsTypeData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select> 
          <el-select v-model="query.status"  size="small" class="m-2" style="width:100%;margin-right:10px;" @change="getTableData(true)" placeholder="选择预警状态">
          <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">状态</div></template>        
          <el-option v-for="item in statusData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select> 
       
          <el-input 
            v-model="query.input"
            placeholder="请输入关键词进行检索"
            size="small"
            clearable
            @clear="getTableData(true)"
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
          :showSelection="true"
          :data="tableData" 
          @selection-change="handleSelectionChange"
          @getTableData="getTableData" 
          @orderChanged="handleSortChange">   
          <el-table-column prop="month" label="年月" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="goodsName" label="名称" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="goodsNo" label="SAP编码" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>    
          <el-table-column prop="goodsModel" label="型号" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/> 
          <el-table-column prop="supplier" label="供应商" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>   
          <el-table-column prop="areaName" label="区域" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>    
          <el-table-column prop="curInvenstory" label="现存量" align="center"  min-width="120" :show-overflow-tooltip="true">
            <template #default="props"> 
                <span v-if="props.row.curInvenstory<props.row.safetyInventory" class="text-danger">{{ props.row.curInvenstory+ props.row.safetyInventoryUnitName }}</span> 
                <span v-else >{{ props.row.curInvenstory+ props.row.safetyInventoryUnitName }}</span> 
              </template>
          </el-table-column> 
          <el-table-column prop="safetyInventory" label="安全库存量" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="props">
                <span>{{ props.row.safetyInventory+ props.row.safetyInventoryUnitName }}</span> 
              </template>
          </el-table-column>    
          <el-table-column prop="purchaseMinimum" label="最小采购量" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="props">
                <span>{{ props.row.purchaseMinimum+ props.row.purchaseMinimumUnitName }}</span> 
              </template>
          </el-table-column>    
          <el-table-column prop="costPrice" label="成本单价" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="props">
                <span>{{ props.row.costPrice+ (props.row.priceUnitName||'元') }}/{{ props.row.costPriceUnitName }}</span> 
              </template>
          </el-table-column>    
          <el-table-column prop="status" label="状态" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
              <template #default="props">
                <span v-if="props.row.status=='Warning'" class="text-danger">{{ props.row.statusDesc }}</span>
                <span v-else-if="props.row.status=='Purchasing'" class="text-warning">{{ props.row.statusDesc }}</span>
                <span v-else-if="props.row.status=='NoPurchase'" class="text-primary">{{ props.row.statusDesc }}</span>
                <span v-else class="text-success">{{ props.row.statusDesc }}</span>
              </template>
          </el-table-column> 
          <el-table-column prop="needPurchase" label="是否需要采购" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/> 
          <el-table-column prop="requirementQuantity" label="采购数量" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>   
          <el-table-column prop="purchaseOrderNo" label="采购单号" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>    
          <el-table-column  label="审批" v-if="hasApproval" align="center" min-width="100" :show-overflow-tooltip="true">
               <template #default="scope"> 
                <el-button v-if="scope.row.isApproval&&permission.isPermisstion('SAFETYINVENTORYAPPROVAL')" @click="handleApproval([scope.row])" circle type="warning" style="height: 28px;" title="点击打开审批编辑界面" ><el-icon><Checked /></el-icon></el-button>
                <div v-else>
                  <span v-if="scope.row.approvalStatus=='Reject'" class="text-danger">{{ scope.row.approvalStatusDesc }}</span>
                  <span v-else-if="scope.row.approvalStatus=='Approve'" class="text-success">{{ scope.row.approvalStatusDesc }}</span>
                  <span v-else-if="scope.row.approvalStatus=='Approvaling'||scope.row.approvalStatus=='Pending'" class="text-warning">{{ scope.row.approvalStatusDesc }}</span>
                  <span v-else>{{ scope.row.approvalStatusDesc }}</span>
                </div>
              </template>
          </el-table-column>  
          <el-table-column prop="createDate" label="创建时间" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
             <template #default="props">
                <span>{{commonHelper.formatToDateTime(props.row.createDate)}}</span>
              </template>
           </el-table-column>  
           <el-table-column prop="remark" label="备注" align="center" sortable="custom" min-width="150" :show-overflow-tooltip="true"/>     
         <el-table-column :label="$t('message.common.handle')" align="center"  min-width="150">
            <template #default="scope"> 
              <el-button @click="handleRead(scope.row)">{{$t("message.common.read")}}</el-button> 
              <el-button  v-if="permission.isPermisstion('SAFETYINVENTORYUPDATE')&&scope.row.status=='Warning'" @click="handleEdit(scope.row)">{{ $t("message.common.update")}}</el-button>
            </template>
          </el-table-column>
        </Table>
        <OrderEditModal :layer="orderLayer"  @dataSubmit="dataSave" v-if="orderLayer.show" />  
        <ApprovalModal :layer="approvalLayer"  @dataSubmit="approvalSubmit" v-if="approvalLayer.show" />
        <ApprovalQuery :layer="approvalQueryLayer" v-if="approvalQueryLayer.show" />  
        <ExportModal :layer="exportLayer"  @dataSubmit="exportData" v-if="exportLayer.show" /> 
      </div>
    </div>
  </template>
  
  <script lang="ts" setup>
   defineOptions({
    name: "safety-inventory"
  })
  import { onMounted, ref, reactive } from "vue";
  import { Page } from "@/components/table/type";
  import {getWarningInfo,getOptions,getSafetyInfoDetail,updateWarningInfo,getExportFields, exportWarningInfo,getArgs,getWarningApprovalHis,updateReceived,approvalSafetyInventory} from "@/api/inv/safety-warning-inventory";
  import{getGoodsClassify} from "@/api/common"
  import { LayerInterface } from "@/components/layer/index.vue"; 
  import Table from "@/components/table/tableServer.vue"; 
  import permission from '@/utils/system/permission';  
  import msg from "@/utils/system/message";
  import OrderEditModal from "./orderEditLayer.vue";  
  import ExportModal from "@/components/layer/exportLayer.vue";  
  import { deftClassifyGroup } from '@/config';
  import commonHelper from "@/utils/system/common-helper"; 
  import {Checked}  from '@element-plus/icons-vue';
  import ApprovalModal from '@/components/layer/approvalLayer.vue'; 
  import ApprovalQuery from '@/components/layer/approvalQueryLayer.vue';
      
      const query = reactive({
        input: "",
        goodsGroup:deftClassifyGroup||'',
        goodsClassifyType:'',
        status:''
      }); 
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
        type:'',
        showButton: true,
        btnLoading:false,
        width:"70%",
        data:null , 
        options:null,
        otherButton:{}
      }); 
      const approvalLayer:LayerInterface=reactive({
        show: false,
        title: "审批",
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
      const goodsGroupData=ref(new Array<any>());  
      const goodsTypeData=ref(new Array<any>());  
      const statusData=ref(new Array<any>()); 
      const hasApproval=ref(false);
      const chooseData = ref([]); 
      const isBatchApproval=ref(false);

      onMounted(()=>{   
        getArgs().then(res=>{ 
          hasApproval.value=res.data.hasApproval 
        })
        getOptionData();
        getGoodsClassifyData(); 
      })
      
      const getOptionData=()=>{ 
        getOptions().then(res=>{
          statusData.value=res.data.statusData;
          statusData.value.unshift({key:'',value:'所有'}); 
          if(deftClassifyGroup=='SparePart'){
            goodsGroupData.value=res.data.goodsClassifyData.filter((f:any)=>f.key=='SparePart'||f.key=='Consumables');
          }
          else{
            goodsGroupData.value=res.data.goodsClassifyData;
          }  
        })
      }

      const getGoodsClassifyData=()=>{
        getGoodsClassify(query.goodsGroup).then(res=>{
          goodsTypeData.value=res.data;
          goodsTypeData.value.unshift({key:'',value:'所有'});
        });
        getTableData(true);
      }
 
      const getTableData = (init: Boolean) => {
        loading.value = true
        if (init) {
          page.index = 1
        }    
         loading.value = false
         let type=query.goodsClassifyType?Number(query.goodsClassifyType):0; 
         var curUser=permission.getOperator().userId;
         getWarningInfo(curUser,page.size,page.index,page.orderField,page.orderType,query.input,query.goodsGroup,type,query.status)
          .then((res) => {
            let data = res.data.rows;
            data.forEach((d: any) => {
              d.loading = false
            })
            tableData.value = data;
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
        } 
      };
   
      const handleSortChange=(orderRow:any)=>{ 
        getTableData(true);
      }  

      const handleRead= (row: any) => {  
        getSafetyInfoDetail(row.detailId).then((res:any)=>{ 
           orderLayer.show = true;  
           orderLayer.type="Edit";
          orderLayer.data=res.data;
          orderLayer.showButton=false; 
        }) 
      }  

      const handleEdit = (row: any) => {  
        getSafetyInfoDetail(row.detailId).then((res:any)=>{ 
           orderLayer.show = true;  
           orderLayer.type="Edit";
           orderLayer.data=res.data; 
        }) 
      } 
       
      const dataSave=(data:any,actionType:string)=>{ 
        orderLayer.btnLoading=true;
         updateWarningInfo(data).then(res=>{
              orderLayer.show = false;
              getTableData(false);
            }).finally(()=> orderLayer.btnLoading=false);
      } 
  
    const handleApproval=(rows:Array<any>)=>{ 
      let isApprovalFlowId=rows.filter(x=>x.isApproval).map(x=>{return x.flowId}); 
      if(isApprovalFlowId.length==rows.length){
        approvalLayer.show = true;  
        approvalLayer.data=rows;
      } 
      else{
        msg.deftAuto("所选数据中存在不符合审批条件的数据");
      }
    }

    const approvalSubmit=(rows:Array<any>,data:any)=>{ 
      let flowId=rows.filter(x=>x.isApproval).map(x=>{return x.flowId});
      let uniqueFlowId = Array.from(new Set(flowId));  
      approvalLayer.btnLoading=true
      approvalSafetyInventory(uniqueFlowId,data.isApprove,data.opinion,query.goodsGroup,permission.getOperator().userId,permission.getOperator().userName)
      .then(res=>{
        approvalLayer.show=false
        getTableData(false);
      })
      .finally(()=>approvalLayer.btnLoading=false)
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
  let type=query.goodsClassifyType?Number(query.goodsClassifyType):0;
  exportWarningInfo(page.orderField,page.orderType,query.input,query.goodsGroup,type,query.status,selField).then(res=>{ 
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