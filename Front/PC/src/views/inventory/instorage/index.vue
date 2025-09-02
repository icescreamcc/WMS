<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
        <el-button v-if="permission.isPermisstion('INSTORAGEORDERADD')" type="primary" icon="el-icon-circle-plus-outline" @click="handleAdd">新增</el-button>
 
          <span style="margin-right: 10px;"></span>
        <el-popconfirm title='确定删除选中的数据吗' v-if="permission.isPermisstion('INSTORAGEORDERDEL')" @confirm="handleDel(chooseData)">
          <template #reference>
            <el-button type="danger"  icon="el-icon-delete" :disabled="chooseData.length === 0">批量删除</el-button>
          </template>
        </el-popconfirm>
        <span style="margin-right: 10px;"></span> 
         <el-popconfirm :title='apprvalMsg' v-if="permission.isPermisstion('INSTORAGEORDERAPPROVAL')&&isInStorageApproval" @confirm="handleApproval(chooseData)">
          <template #reference>
            <el-button type="warning"  icon="el-icon-delete" :disabled="chooseData.length === 0">批量审批</el-button>
          </template>
        </el-popconfirm> 
       
      </div>
      <div class="layout-container-form-search">
        <el-select v-model="selectedGoodsGroup" size="small"  class="m-2" style="width:100%;margin-right:10px;" @change="onClassifyChanged" placeholder="选择物品大类">
          <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">分类</div></template>        
          <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
          </el-select>
      <el-date-picker
        v-model="dateRange"
        type="daterange"
        range-separator="-"
        start-placeholder="最早日期"
        end-placeholder="最晚日期"
         size="small"
         value-format="YYYY-MM-DD"
         style="margin-right:10px;width:100%"
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
            <el-button v-if="permission.isPermisstion('INSTORAGEORDEREXPORT')" icon="el-icon-download" style="margin-left:20px"  type="info" @click="exportData">导出</el-button>
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
        <el-table-column prop="orderNo" label="明细" type="expand" align="center" sortable :show-overflow-tooltip="true"> 
             <template #default="props">
                    <div style="margin-bottom:10px" >
                     <span  style="font-weight:600;">入库明细</span> 
                   </div>
                   <div>
                      <el-table :data="detailList.filter(x=>x.orderNo==props.row.orderNo)"  style="width: 100%">
                      <el-table-column prop="goodsName" label="名称" width="180" />
                      <el-table-column prop="goodsModel" label="型号" width="180" />
                      <el-table-column prop="goodsClassifyName" label="类型" width="180" />
                      <el-table-column prop="quantity" label="计划入库数量" >
                          <template #default="detail"> 
                            <span>{{detail.row.quantity+detail.row.unitName}}</span> 
                          </template>
                      </el-table-column>
                        <el-table-column prop="actualQuantity" label="实际入库数量">
                          <template #default="detail"> 
                            <span>{{detail.row.actualQuantity+detail.row.unitName}}</span> 
                          </template>
                      </el-table-column>
                      <!-- <el-table-column prop="packageCount" label="包装" >
                          <template #default="scope">  
                            <span v-if="scope.row.minPackageUnitName==scope.row.packageUnitName&&scope.row.packageUnitName==scope.row.maxPackageUnitName">{{scope.row.packageCount+scope.row.minPackageUnitName}}</span>
                            <span v-else-if="scope.row.minPackageUnitName!=scope.row.packageUnitName&&scope.row.packageUnitName==scope.row.maxPackageUnitName">{{scope.row.packageCount+scope.row.minPackageUnitName+'/'+scope.row.packageUnitName}}</span>
                            <span v-else-if="scope.row.minPackageUnitName==scope.row.packageUnitName&&scope.row.packageUnitName!=scope.row.maxPackageUnitName">{{scope.row.maxPackageCount+scope.row.packageUnitName+'/'+scope.row.maxPackageUnitName}}</span>
                            <span v-else>{{scope.row.packageCount+scope.row.minPackageUnitName+'/'+scope.row.packageUnitName+','+scope.row.maxPackageCount+scope.row.packageUnitName+'/'+scope.row.maxPackageUnitName}}</span>
                          </template>
                      </el-table-column> -->
                      <el-table-column prop="warehouseName" label="入库仓库" />
                      <el-table-column prop="shelfName" label="入库货架"/>
                      <el-table-column prop="binName" label="入库货位"/>
                      <el-table-column prop="workbinCellNo" label="入库料箱"/>
                    </el-table> 
                   </div> 
            </template> 
          </el-table-column> 
        <el-table-column prop="orderNo" label="入库单号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>
        <el-table-column prop="warehouseName" label="入库仓库" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
        <el-table-column prop="inStorageType" label="入库类型" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
            <template #default="props">
              <span>{{props.row.inStorageTypeDesc}}</span>
            </template>
        </el-table-column>    
        <el-table-column prop="createUserName" label="创建人" align="center" sortable="custom" :show-overflow-tooltip="true"/>
        <el-table-column prop="createDate" label="创建时间" align="center" sortable="custom" :show-overflow-tooltip="true">
          <template #default="props">
              <span>{{commonHelper.formatToDateTime(props.row.createDate) }}</span>
            </template>
        </el-table-column>   
        <el-table-column prop="instorageDate" label="入库时间" align="center" sortable="custom" :show-overflow-tooltip="true">
          <template #default="props">
              <span>{{commonHelper.formatToDateTime(props.row.instorageDate) }}</span>
            </template>
        </el-table-column>     
        <el-table-column prop="remark" :label="remarkLabel" align="center" sortable="custom" :show-overflow-tooltip="true"/> 
        <el-table-column prop="status" label="状态" align="center" sortable="custom" :show-overflow-tooltip="true">
              <template #default="props">
              <div v-if="props.row.status=='QualityFailed'||props.row.status=='Reject'" class="text-danger">{{props.row.statusDesc}}</div>
              <div  v-else-if="props.row.status=='WaitInStorage'">
                   <el-popconfirm title="是否确认入库？" v-if="permission.isPermisstion('INSTORAGEORDERCONFIRM')" @confirm="submitInStorage(props.row)">
                    <template #reference>
                      <el-button  title="点击确认入库" type="warning" :loading="props.row.loading">{{props.row.statusDesc}}</el-button> 
                    </template>
                  </el-popconfirm> 
                  <span v-else>{{props.row.statusDesc}}</span>
              </div> 
              <span v-else>{{props.row.statusDesc}}</span>
            </template>
        </el-table-column>     
        <el-table-column  label="AGV调度"    v-if="permission.isPermisstion('INSTORAGEORDERAGVSCHED')" align="center" min-width="80"  :show-overflow-tooltip="true">
          <template #default="props">
            <img src="../../../../public/icon-img/icon_agv_online.png" v-if="props.row.status=='WaitInStorage'" title="点击调度AGV自动入库" style="height: 28px;cursor: pointer;position: relative;bottom: -4px;" @click="onAGVScheduling(props.row)" >
            </template>
        </el-table-column>      
        <el-table-column prop="approvalDate" v-if="isInStorageApproval" label="审批时间" align="center" min-width="130" sortable="custom" :show-overflow-tooltip="true"/>      
        <el-table-column :label="$t('message.common.handle')" min-width="150" align="left" fixed="right" v-if="permission.isPermisstion('INSTORAGEORDERUPDATE','INSTORAGEORDERDEL')" >
          <template #default="scope"> 
            <div v-if="scope.row.status=='WaitInStorage'">
              <el-button @click="handleEdit(scope.row)"   v-if="permission.isPermisstion('INSTORAGEORDERUPDATE')">{{$t("message.common.update")}}</el-button>
              <el-popconfirm v-if="permission.isPermisstion('INSTORAGEORDERDEL')" :title="$t('message.common.delTip')"  @confirm="handleDel([scope.row])">
                <template #reference>
                  <el-button type="danger">{{ $t("message.common.del") }}</el-button>
                </template>
              </el-popconfirm> 
            </div> 
            <div v-else>
              <el-button @click="handleRead(scope.row)" >{{$t("message.common.read")}}</el-button>
            </div>
          </template>
        </el-table-column>
      </Table>
      <OrderEditModal :layer="orderLayer"  @dataSubmit="dataSave" v-if="orderLayer.show" />  
      <ExportModal :layer="exportLayer"  @dataSubmit="exportData" v-if="exportLayer.show" />
       <ApprovalModal :layer="approvalLayer"  @dataSubmit="approvalSubmit" v-if="approvalLayer.show" />
       <AutoOperaionLayer :layer="autoOperationLayer" v-if="autoOperationLayer.show" @operationFinished="onAgvTaskFinished"/>
    </div>
  </div>
</template>

<script lang="ts" setup>
defineOptions({
    name: "instorage"
  })
import {  ref, reactive,onMounted } from "vue";
import { Page } from "@/components/table/type";
import { getOrders,getArgs, getOrderDetail,addInStorage, updateInStorage,delInStorage,exportInStorage,getAllowField,approvalInStorage,confirmInStorage,agvScheduling } from "@/api/inv/instorage";
import { LayerInterface } from "@/components/layer/index.vue"; 
import Table from "@/components/table/tableServer.vue";
import OrderEditModal from "./orderEditLayer.vue";  
import ApprovalModal from '@/components/layer/approvalLayer.vue' 
import ExportModal from "@/components/layer/exportLayer.vue"
import permission from '@/utils/system/permission'
import commonHelper from "@/utils/system/common-helper"; 
import {getGoodsGroup} from '@/api/common'; 
import { deftClassifyGroup} from '@/config';
import msg from '@/utils/system/message';
import store from '@/store'; 
import {DocumentAdd} from '@element-plus/icons-vue';
import AutoOperaionLayer from "@/components/auto-operation/autoOperaion.vue";

   // 存储搜索用的数据
   const query = reactive({
      input: "",
    });
    let dateRange=ref() 
    // 分页参数, 供table使用
    const page: Page = reactive({
      index: 1,
      size: 20,
      total: 0,
      orderField:'',
      orderType:''
    });
    const loading = ref(true);
    const tableData = ref([]);
    const chooseData = ref([]);
    const apprvalMsg=ref("");
    const goodsGroupData=ref(new Array<any>());
    const selectedGoodsGroup=ref(deftClassifyGroup); 
    const isInStorageApproval=ref(false);
    const remarkLabel=ref('备注'); 
    const orderLayer: LayerInterface = reactive({
        show: false,
        title: "",
        showButton: true,
        btnLoading:false,
        width:"75%",
        data:null ,
        otherButton:{
                show:false,
                otherBtnLoading:false,
                text:"",
                type:""
              }
      }); 
    const approvalLayer:LayerInterface=reactive({
      show: false,
      title: "入库审批",
      showButton: true,
      btnLoading:false,
      width:"40%",
      data:null ,
      otherButton:{
              show:false,
              otherBtnLoading:false,
              text:"",
              type:""
            }
    });
    const autoOperationLayer:any = ref({
      show: false,
      title: "",
      showButton: true,
      btnLoading:false, 
      width:"78%",
      data:null,  
      otherButton:{}
}); 
    const uploadExport:any = ref()
    const baseURL: any = import.meta.env.VITE_BASE_URL;
    const uploadUrl = baseURL+'/InStorage/SparePartExport';
    const headersObj = { authorization: store.getters['user/token'] };

    onMounted(()=>{ 
      getArgs().then(res=>{ 
      isInStorageApproval.value=res.data.isInStorageApproval
      })
      getGoodsGroupData();
      getTableData(true);
    }) 
 
    const getGoodsGroupData=()=>{
      getGoodsGroup().then(res=>{ 
        if(deftClassifyGroup=='SparePart'){
          goodsGroupData.value=res.data.filter((f:any)=>f.key=='SparePart'||f.key=='Consumables');
        }
        else{
          goodsGroupData.value=res.data;
        } 
      })
    } 

    const onClassifyChanged=()=>{
      remarkLabel.value=selectedGoodsGroup.value=='Separator'?'备注':'备注';
      getTableData(true);
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
      getOrders(permission.getOperator().userId,page.size,page.index,page.orderField,page.orderType,query.input,dateStart,dateEnd,selectedGoodsGroup.value)
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

    //展开与收缩
    const detailList=ref(new Array<any>()) 
      const handleExpandChange=(row:any)=>{  
       getOrderDetail(permission.getOperator().userId, row.orderNo).then(res=>{
            detailList.value=detailList.value.filter((b:any)=>b.orderNo!=row.orderNo)  
            res.data?.forEach((b:any) => {
                detailList.value.push(b)
              });   
       }) 
    }

    //排序事件
    const handleSortChange=(orderRow:any)=>{ 
      getTableData(true);
    }

     //批量选择
     const handleSelectionChange = (val: []) => {
      chooseData.value = val;
      let isApprovalOrderNo=val.filter((x:any)=>x.isApproval)
      if(isApprovalOrderNo.length!=val.length){
        apprvalMsg.value="当前选择存在不符合审批条件的数据"
      }
      else{
        apprvalMsg.value="确定审批选中的数据吗"
      }
    };

    const handleDel = (data: any[]) => {  
      let ids=Array<any>();
      data.forEach(d=>{
        ids.push(d.orderNo);
      }) 
      delInStorage(ids).then((res) => { 
        getTableData(tableData.value.length === 1 ? true : false);
      });
    }  

   //审批弹窗功能 
   const handleApproval=(data: any[])=>{ 
     let isApprovalOrderNo=data.filter(x=>x.isApproval).map(x=>{return x.orderNo}) 
     if(isApprovalOrderNo.length==data.length){
       approvalLayer.show = true;  
      approvalLayer.data=data;
     } 
   }

  //审批数据提交
  const approvalSubmit=(orders:any[],data:any)=>{  
     let orderNo=orders.filter(x=>x.isApproval).map(x=>{return x.orderNo}) 
     approvalLayer.btnLoading=true
     approvalInStorage(orderNo,data.isApprove,data.opinion,permission.getOperator().userId,permission.getOperator().userName)
     .then(res=>{
       approvalLayer.show=false
        getTableData(false);
     })
     .finally(()=>approvalLayer.btnLoading=false)
   }
 
    // 新增弹窗功能
   const handleAdd = () => {
      orderLayer.title = "新增入库单";
      orderLayer.show = true;
      orderLayer.showButton=true;  
      delete orderLayer.data;
    }

    // 编辑弹窗功能
    const handleEdit = (row: any) => {  
      getOrderDetail(permission.getOperator().userId, row.orderNo).then((res:any)=>{
         orderLayer.title = "编辑入库单"; 
         orderLayer.show = true;
         orderLayer.showButton=true;
         row.details=res.data
         orderLayer.data=row;
      }) 
    }

    const handleRead=(row: any)=>{
      getOrderDetail(permission.getOperator().userId, row.orderNo).then((res:any)=>{
         orderLayer.title = "入库单"; 
         orderLayer.showButton=false;
         orderLayer.show = true;
         row.details=res.data
         orderLayer.data=row;
      }) 
    }
     
    //新增或编辑数据提交
    const dataSave=(data:any,actionType:string)=>{ 
      orderLayer.btnLoading=true;
        if(actionType=='add'){
          addInStorage(data).then(res=>{
            orderLayer.show = false;
            getTableData(true);
          }).finally(()=> orderLayer.btnLoading=false);
        }
        else{
           updateInStorage(data).then(res=>{
            orderLayer.show = false;
            getTableData(false);
          }).finally(()=> orderLayer.btnLoading=false);
        }
    }

    //启用agv调度
    const onAGVScheduling=(row:any)=>{
      agvScheduling(row.orderNo).then(()=>{
        if(selectedGoodsGroup.value=="SparePart"){
          autoOperationLayer.value.data={
                        title:row.inStorageTypeDesc,
                        orderNo:row.orderNo,
                        goodsClassifyGroup:selectedGoodsGroup.value,
                        operatorId:row.updateUserId,
                        operator:row.updateUserName,
                        orderType:row.inStorageType,
                        line:'',
                        remark:row.remark,
                        details:null
                      };
                    autoOperationLayer.value.show=true; 
              }
      })
    }

    const onAgvTaskFinished=()=>{ 
      getTableData(false);
    }

    //确认入库
    const submitInStorage=(row:any)=>{
      row.loading=true
      confirmInStorage(row.orderNo).then(res=>{
         getTableData(false);
      }).finally(()=>row.loading=false)
    }

    //数据导出弹窗控制器
    const exportLayer: LayerInterface = reactive({
      show: false,
      title: "选择导出字段",
      showButton: true,
      btnLoading:false,
      width:"40%",
      data:null,
      otherButton:{ }
    });
 
     //导出入库单
     const exportData=()=>{ 
      let dateStart='';
      let dateEnd='';
      if(dateRange.value?.length>0){
        dateStart=dateRange.value[0]
      }
       if(dateRange.value?.length>1){
        dateEnd=dateRange.value[1]
      }
      exportLayer.btnLoading=true;
        exportInStorage(query.input,page.orderField,page.orderType,dateStart,dateEnd,selectedGoodsGroup.value).then(res=>{ 
          let link = document.createElement('a') 
            link.style.display = 'none'
            link.href =res.data
            document.body.appendChild(link)
            link.click() 
            document.body.removeChild(link)
        }).finally(()=>exportLayer.btnLoading=false)
    }

 
//导入上传
const exportErrorHandle=(res:any)=>{ 
  console.log("exportErrorHandle",JSON.parse(res.message))
  if(res.message){
    msg.errorAuto(JSON.parse(res.message).message,5000)
  }
  else{
    msg.errorAuto("上传失败，未知原因")
  }
}
 
const exportSuccessHandle=(res: any)=>{   
    if(res.status=="Success"){
      uploadExport.value!.clearFiles()
      msg.successAuto("导入成功");
      getTableData(true);
    }  
}
</script>

<style lang="scss" scoped>
.statusName {
  margin-right: 10px;
}
</style>
