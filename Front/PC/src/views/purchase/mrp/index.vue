<template>
    <div class="layout-container">
      <div class="layout-container-form flex space-between">
        <div class="layout-container-form-handle">
          <!-- <el-button v-if="permission.isPermisstion('MATERIALREQUIREMENTPLANADD')" type="primary" icon="el-icon-circle-plus-outline" @click="handleAdd">新增</el-button> -->
          <el-button icon="el-icon-download" v-if="permission.isPermisstion('MATERIALREQUIREMENTPLANEXPORT')"  style="margin-left:20px"  @click="getExportAllowField">导出</el-button>
        </div>
        <div class="layout-container-form-search">
          <el-select v-model="selectedGoodsGroup" ref="refSelectClassifyGroup" size="small" disabled class="m-2" style="width:100%;margin-right:10px;" @change="getTableData(true)" placeholder="选择物品大类">
          <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">分类</div></template>        
          <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select> 
          <el-select v-model="selectedStatus"  size="small" class="m-2" style="width:100%;margin-right:10px;" @change="getTableData(true)" placeholder="选择计划订单状态">
          <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">状态</div></template>        
          <el-option v-for="item in statusData" :key="item.key" :label="item.value" :value="item.key"></el-option>
          </el-select> 
          <el-date-picker v-model="query.week" type="week" size="small" :picker-options="{'firstDayOfWeek':1}"  value-format="YYYY-MM-DD-ww" placeholder="请选择计划周" style="margin-right:10px;width:120%" @change="getTableData(true)"></el-date-picker>
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
          :showSelection="false"
          :data="tableData" 
          @getTableData="getTableData" 
          @orderChanged="handleSortChange"   
        >  
          <el-table-column prop="week" label="计划周期" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
             <template #default="props"> 
                 <span>{{ props.row.year }}CW{{props.row.week}}</span>
              </template>
           </el-table-column> 
          <el-table-column prop="version" label="版本号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="goodsClassifyGroup" label="物料需求类别" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"> 
              <template #default="props">
                <span>{{ goodsGroupData.find(f=>f.key==props.row.goodsClassifyGroup).value }}</span>
              </template>
          </el-table-column>   
          <el-table-column prop="supplierName" label="供应商" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>   
           <el-table-column prop="createDate" label="创建时间" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
             <template #default="props">
                <span>{{commonHelper.formatToDateTime(props.row.createDate)}}</span>
              </template>
           </el-table-column>  
           <el-table-column prop="status" label="状态" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
              <template #default="props">
                <span v-if="props.row.status=='Exceed'" class="text-warning">{{ props.row.statusDesc }}</span>
                <span v-else-if="props.row.status=='Obsolete'" class="text-danger">{{ props.row.statusDesc }}</span>
                <span v-else >{{ props.row.statusDesc }}</span>
              </template>
          </el-table-column>  
          <el-table-column  label="主动推送供应商" v-if="permission.isPermisstion('MATERIALREQUIREMENTPLANMAIL')" align="center" min-width="150"  :show-overflow-tooltip="true">
               <template #default="props">
                <el-button v-if="props.row.isSendMail"  style="height: 30px;" title="已发送邮件通知" @click="onSendMailEdit(props.row)" circle>
                  <img src="../../../../public/icon-img/yidu1.png" height="14"> 
                </el-button>
                <el-button v-else  style="height: 30px;" title="未发送邮件通知" @click="onSendMailEdit(props.row)" circle>
                  <img src="../../../../public/icon-img/xiaoxi4.png" height="14"> 
                </el-button> 
              </template>
          </el-table-column> 
          <el-table-column  label="查看成品计划" align="center" min-width="120" :show-overflow-tooltip="true">
             <template #default="props">
                <el-button style="height: 30px;"  title="点击查看对应的成品生产计划" circle  @click="onShowProductionPlan(props.row)">
                  <img src="../../../../public/icon-img/jihuadingdanchaxun.png" height="14">
                </el-button> 
              </template>
           </el-table-column> 
          <el-table-column prop="receivingAbnormalTypeId" label="邮件历史" align="center" min-width="120" :show-overflow-tooltip="true">
             <template #default="props">
                <el-button style="height: 30px;"  title="点击查看邮件发送历史记录" circle @click="onShowMailHis(props.row)">
                  <img src="../../../../public/icon-img/piaojulishixinxichaxun.png" height="14">
                </el-button> 
              </template>
           </el-table-column> 
         <el-table-column :label="$t('message.common.handle')" align="center"  min-width="150">
            <template #default="scope">  
              <el-button @click="handleRead(scope.row)">{{$t("message.common.read")}}</el-button> 
            </template>
          </el-table-column>
        </Table>
        <OrderEditModal :layer="orderLayer"  @dataSubmit="dataSave" v-if="orderLayer.show" />   
        <MailEditLayer  :layer="mailEditLayer"  @dataSubmit="onMailSubmit" v-if="mailEditLayer.show"/>
        <ExportModal :layer="exportLayer"  @dataSubmit="exportData" v-if="exportLayer.show" />
        <ProductionPlanModal :layer="productionPlanLayer" v-if="productionPlanLayer.show" />
        <MailHisModal :layer="mailHisLayer" v-if="mailHisLayer.show" />
      </div>
    </div>
  </template>
  
  <script lang="ts" setup>
   defineOptions({
    name: "mrp"
  })
  import { onMounted, ref, reactive } from "vue";
  import { Page } from "@/components/table/type";
  import {getPlanOrders,getPlanOrderDetails,getOrderMailInfo,getOptions,getProductionInfoByReqPlanOrder,addPlanOrder,updatePlanOrder,delPlanOrder,getExportFields,exportPlanOrder,createAttachment,sendMailToSupplier } from "@/api/purchase/materialRequirementPlan";
  import { LayerInterface } from "@/components/layer/index.vue"; 
  import Table from "@/components/table/tableServer.vue"; 
  import permission from '@/utils/system/permission';  
  import msg from "@/utils/system/message";
  import OrderEditModal from "./orderEditLayer.vue"; 
  import MailEditLayer from "@/components/layer/mailLayer.vue";
  import ExportModal from "@/components/layer/exportLayer.vue"; 
  import ProductionPlanModal from "./productionPlanLayer.vue";
  import MailHisModal from "./mailHisLayer.vue";
  import { deftClassifyGroup ,disableClassifyGroupSelect} from '@/config';
  import commonHelper from "@/utils/system/common-helper"; 
      
      const query = reactive({
        input: "",
        year:0,
        week:''
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
        showButton: true,
        btnLoading:false,
        width:"70%",
        data:null , 
        options:null,
        otherButton:{}
      });
      const mailEditLayer:LayerInterface=reactive({
        show: false,
        title: "供应商邮件通知",
        showButton: true,
        btnLoading:false,
        width:"60%",
        data:null ,
        otherButton:{}
      })  
      const exportLayer: LayerInterface = reactive({
        show: false,
        title: "选择导出字段",
        showButton: true,
        btnLoading:false,
        width:"45%",
        data:null,
        otherButton:{ }
      });
      const productionPlanLayer: LayerInterface = reactive({
        show: false,
        title: "成品生产计划",
        showButton: false,
        btnLoading:false,
        width:"70%",
        data:null,
        otherButton:{ }
      });
      const mailHisLayer: LayerInterface = reactive({
        show: false,
        title: "邮件历史记录",
        showButton: false,
        btnLoading:false,
        width:"70%",
        data:null,
        otherButton:{ }
      });

      const loading = ref(true);
      const tableData = ref([]); 
      const goodsGroupData=ref(new Array<any>());
      const selectedGoodsGroup=ref(deftClassifyGroup||"PackingMaterial");  
      const selectedStatus=ref("");
      const statusData=ref(new Array<any>());
      const refSelectClassifyGroup=ref<null | HTMLElement>(null);

      onMounted(()=>{   
        getOptionData();
        getTableData(true)
      })
      
      const getOptionData=()=>{ 
        getOptions().then(res=>{
          statusData.value=res.data.statusData;
          statusData.value.unshift({key:'',value:'所有'});
          goodsGroupData.value=res.data.goodsClassifyData;
        })
      }
 
      const getTableData = (init: Boolean) => {
        loading.value = true
        if (init) {
          page.index = 1
        }   
        let year=0;
        let week=0;
        if(query.week){
          let weekArr=query.week.split('-');
          year=Number(weekArr[0]);
          week=Number(weekArr[3]);
        } 
         loading.value = false
        getPlanOrders(permission.getOperator().userId,page.size,page.index,page.orderField,page.orderType,query.input,year,week,selectedGoodsGroup.value,selectedStatus.value)
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

      const handleDel = (row:any) => {   
        delPlanOrder(row.orderNo).then((res) => { 
          getTableData(tableData.value.length === 1 ? true : false);
        });
      } 
     
      const onSendMailEdit=(row:any)=>{  
        getPlanOrderDetails(row.orderNo).then((res:any)=>{
          if(res.data.status!='Created'){
            msg.deftAuto("当前需求计划已过期或已废弃，不再允许推送给供应商");
            //return;
          }
          let t=goodsGroupData.value.find(f=>f.key==res.data.goodsClassifyGroup).value;    
          mailEditLayer.options={
            reciverType:"manual"
          }
          mailEditLayer.data={
            subject:t+'需求计划'+row.year+"CW"+row.week+"-v"+row.version,
            body:"",
            bodyType:'html',
            toReciver:res.data.supplierContactList?.map((m:any)=>{return m.email}),
            toReciverOptions:res.data.supplierContactList,
            toCC:[], 
            orderNo:row.orderNo
          }
          createAttachment(row.orderNo).then(res=>{
            mailEditLayer.data.attachments=[res.data]; 
            mailEditLayer.show=true;
          });
         
        });  
      }

      const onMailSubmit=(data:any)=>{
        var dataConvert=JSON.parse(JSON.stringify(data)); 
        mailEditLayer.btnLoading=true;
        sendMailToSupplier(dataConvert).then(()=>{
          msg.successAuto("已发送需求计划到供应商")
          mailEditLayer.show=false;
          getTableData(false);
        }).finally(()=>mailEditLayer.btnLoading=false);
      }

      const onShowProductionPlan=(row:any)=>{
        getProductionInfoByReqPlanOrder(row.year,row.week,row.version).then(res=>{ 
          productionPlanLayer.title=`${res.data[0].planName} v${res.data[0].version}`;
          productionPlanLayer.data=res.data;
          productionPlanLayer.show=true;
        }) 
      }

      const onShowMailHis=(row:any)=>{
        if(!row.isSendMail){
            msg.deftAuto("当前需求计划还未发送过邮件给供应商");
            return;
        }
        getOrderMailInfo(row.orderNo).then(res=>{
          mailHisLayer.data=res.data;
          mailHisLayer.show=true;
        })
      }
      
      const handleAdd = () => {
        if(!selectedGoodsGroup.value){
          msg.deftAuto("请先选择物料分类");
          refSelectClassifyGroup.value?.focus(); 
          return;
        }
        orderLayer.title = "新增物料需求计划";
        orderLayer.show = true;  
        orderLayer.showButton=true; 
        orderLayer.options={
          goodsGroup:selectedGoodsGroup.value,
          isDisabledGroup:true
        }
        delete orderLayer.data;
      }
   
      const handleEdit = (row: any) => {  
        getPlanOrderDetails(row.orderNo).then((res:any)=>{
           orderLayer.title = "编辑物料需求计划"; 
           orderLayer.show = true; 
           orderLayer.showButton=false;
           orderLayer.options={
            goodsGroup:selectedGoodsGroup.value,
            isDisabledGroup:true
          }
          orderLayer.data=res.data 
        }) 
      }

      const handleRead = (row: any) => {  
        getPlanOrderDetails(row.orderNo).then((res:any)=>{
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
            addPlanOrder(data).then(res=>{
              orderLayer.show = false;
              getTableData(true);
            }).finally(()=> orderLayer.btnLoading=false);
          }
          else{
             updatePlanOrder(data).then(res=>{
              orderLayer.show = false;
              getTableData(false);
            }).finally(()=> orderLayer.btnLoading=false);
          }
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
  let year=0;
  let week=0;
  if(query.week){
    let weekArr=query.week.split('-');
    year=Number(weekArr[0]);
    week=Number(weekArr[3]);
  }
  exportPlanOrder(page.orderField,page.orderType,query.input,year,week,selectedGoodsGroup.value,selectedStatus.value,selField).then(res=>{ 
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