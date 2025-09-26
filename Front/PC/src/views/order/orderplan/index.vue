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
        </div>

        <div>&nbsp;&nbsp;</div>
        <div class="layout-container-form-search">
          <el-select v-model="query.customerName" ref="refSelectClassifyGroup" size="small" :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;" @change="getTableData(true)" placeholder="选择客户">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              客户</div></template>        
            <el-option v-for="item in supplierUserData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select>

          <el-select v-model="query.goodsName" ref="refSelectClassifyGroup" size="small" :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;" @change="getTableData(true)" filterable clearable placeholder="选择物品">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              物品</div></template>        
            <el-option v-for="item in goodsData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select>

          <!-- <el-select v-model="query.createUserNames" ref="refSelectClassifyGroup" size="small" :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;" @change="getTableData(true)" filterable clearable placeholder="选择计划员">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              计划员</div></template>        
            <el-option v-for="item in createUserNameData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select>

          <el-select v-model="query.detailStatus" ref="refSelectClassifyGroup" size="small" :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;" @change="getTableData(true)" filterable clearable placeholder="选择单据状态">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              单据状态</div></template>        
            <el-option v-for="item in detailStatusData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select> -->

          <el-date-picker v-model="dateRange" type="daterange" range-separator="-" start-placeholder="最早到货日期" end-placeholder="最晚到货日期" size="small" value-format="YYYY-MM-DD" style="margin-left: 10px;;width:100%" @change="getTableData(true)"></el-date-picker>
          <el-input  v-model="query.input" placeholder="请输入关键词进行检索" size="small" style="width: 80%;margin-left: 10px;" clearable @clear="getTableData(true)"></el-input>
          <el-button type="primary" icon="el-icon-search" class="search-btn" @click="getTableData(true)" >搜索</el-button> 
        
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
          <el-table-column prop="orderNo" label="订单号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="contractNo" label="合同号" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
          <el-table-column prop="signingDate" label="签约日期" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="props">
                <span>{{commonHelper.formatToDateTime(props.row.signingDate)}}</span>
              </template>
          </el-table-column> 
          <el-table-column prop="customerName" label="客户名称" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
           <el-table-column prop="customerNames" label="客户名称" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="goodsName" label="物品名称" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" v-if="false"/> 
          <el-table-column prop="goodsNames" label="物品名称" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true" /> 
          <el-table-column prop="orderNum" label="订单量" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="unit" label="单位" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"  v-if="false" />   
          <el-table-column prop="unitName" label="单位" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>   
          <el-table-column prop="orderAmount" label="订单金额" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="deliveryDate" label="交货截至日期" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="props">
                <span>{{commonHelper.formatToDateTime(props.row.deliveryDate)}}</span>
              </template>
          </el-table-column> 
          <el-table-column prop="shippedNum" label="已发重量" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="belial" label="完成比列" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="invoice" label="开票信息" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="paymentState" label="回款状态" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="orderState" label="订单状态" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="remark" label="备注" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
          <el-table-column prop="createDate" label="创建时间" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
          <el-table-column prop="orderUrl" label="签字附件地址" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true" v-if="false" />
          <el-table-column :label="$t('message.common.handle')" align="center" width="200">
            <template #default="scope">
               <el-button @click="handleRead(scope.row)">{{ $t("message.common.read") }}</el-button>
              <el-button  type="primary"  @click="handleEdit(scope.row)" :disabled="scope.row.orderState != '新建'" >
                {{
                $t("message.common.update")
              }}</el-button>
            </template>
        </el-table-column>
        </Table>
          <OrderEditModal :layer="orderLayer"  @dataSubmit="dataSave" v-if="orderLayer.show" />   
      </div>
    </div>
  </template>
  
  <script lang="ts" setup>
   defineOptions({
    name: "orderplan"
  })
  import { onMounted, ref, reactive } from "vue";
  import { Page } from "@/components/table/type";
  import { getOrders,addOrderPlan, updateOrderPlan,delOrderPlan,approvalReceivingOrder,getApprovalHis,exportReceivingData,getExportFields } from "@/api/order/orderplan";
  import { LayerInterface } from "@/components/layer/index.vue"; 
  import Table from "@/components/table/tableServer.vue"; 
  import OrderEditModal from "./orderEditLayer.vue";  
  import permission from '@/utils/system/permission';
  import msg from "@/utils/system/message";
  import { deftClassifyGroup ,disableClassifyGroupSelect} from '@/config';
  import commonHelper from "@/utils/system/common-helper";
  import{getOptions} from'@/api/order/orderplan';

      const query = reactive({
        input: "",
        receivingLevel:"",
        createUserNames:"",
        detailStatus:"",
        sumWorkpieceTray:"",
        sumPallet:"",
        customerName:"",
        goodsName:"",
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
  
      const loading = ref(true);
      const tableData = ref([]);
      const chooseData = ref([]); 

      const supplierUserData=ref(new Array<any>());
      const goodsData=ref(new Array<any>()); 

  
      const hasApproval=ref(false); 
      const receivingAbnormalTypeData=ref(new Array<any>());

      const refSelectClassifyGroup=ref<null | HTMLElement>(null);

      onMounted(()=>{ 
       
        getDetailStatusGroupData();
        getTableData(true);
      })
 
      //加载下拉框

      const getDetailStatusGroupData=()=>{
          getOptions().then((res:any)=>{ 
            supplierUserData.value=res.data.userOptions;
            goodsData.value=res.data.goodsNameOptions; 
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
        getOrders(permission.getOperator().userId,page.size,page.index,page.orderField,page.orderType,query.input)
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
        //  getOrderDetail(permission.getOperator().userId, row.orderNo).then(res=>{
        //       detailList.value=detailList.value.filter((b:any)=>b.orderNo!=row.orderNo)  
        //       res.data?.forEach((b:any) => {
        //           detailList.value.push(b)
        //         });   
        //  }) 
      }
   
      const handleSortChange=(orderRow:any)=>{ 
        getTableData(true);
      }
   
       const handleSelectionChange = (val: []) => {
        chooseData.value = val;
      };
  
      const handleDel = (data: any[]) => {  
        let ids=Array<any>();
        data.forEach(d=>{
          ids.push(d.orderNo);
        }) 
        delOrderPlan(ids).then((res) => { 
          getTableData(tableData.value.length === 1 ? true : false);
        });
      } 
     

  
      const handleAdd = () => {
        orderLayer.title = "新增订单计划";
        orderLayer.type='add'
        orderLayer.show = true;  
        orderLayer.showButton=true; 
        orderLayer.options={
          
          isDisabledGroup:disableClassifyGroupSelect
        }
        delete orderLayer.row;
      }
     // 编辑弹窗功能
    let handleEdit = (row: any) => {   
         orderLayer.title = "编辑订单计划";
         orderLayer.row = row; 
         orderLayer.type='update'
         orderLayer.show = true;  
    }
    const handleRead = (row: any) => {
      orderLayer.title = "查看订单计划";
      orderLayer.show = true;
      orderLayer.type='select'
      orderLayer.showButton = false;
      orderLayer.data = row
}
      const dataSave=(data:any,actionType:string)=>{ 
        orderLayer.btnLoading=true;
          if(actionType=='add'){
            // data.createUserId=permission.getOperator().userId,
            // data.createUserName=permission.getOperator().userName, 
            addOrderPlan(data).then(res=>{
              orderLayer.show = false;
              getTableData(true);
            }).finally(()=> orderLayer.btnLoading=false);
          }
          else{
            // data.updateUserName = permission.getOperator().userName;
            // data.updateUserId = permission.getOperator().userId;
             updateOrderPlan(data).then(res=>{
              orderLayer.show = false;
              getTableData(false);
            }).finally(()=> orderLayer.btnLoading=false);
          }
      } 
  </script>
  
  <style lang="scss" scoped>
    .csm-link{
      cursor: pointer;
      text-decoration: underline;
    }
  </style> 