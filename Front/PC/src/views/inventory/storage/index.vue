<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle"> 
         <el-button v-if="permission.isPermisstion('STORAGEALLOCATION')" type="primary" icon="el-icon-circle-plus-outline" @click="showAllcationModal">库存调拨</el-button>
      </div>
      <div class="layout-container-form-search"> 
        <el-select v-model="selectedGoodsGroup"  size="small"  class="m-2" style="width:100%;margin-right:10px;" @change="onGroupSelected" placeholder="选择物品大类">
          <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">分类</div></template>        
          <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
          </el-select>
        <el-select v-model="selectedWarehouseId"  class="m-2" style="width:100%;margin-right:10px;" @change="onWarehouseSelected" size="small">
              <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">仓库</div></template>
              <el-option v-for="item in warehouseData" :key="item.warehouseId" :label="item.warehouseName" :value="item.warehouseId">
          </el-option>
        </el-select> 
        <el-input 
          v-model="query.input"
          placeholder="请输入关键词进行检索"
          size="small"
        ></el-input>
        <el-button
          type="primary"
          icon="el-icon-search"
          class="search-btn"
          @click="onSearch"
          >搜索</el-button>  
           <el-button type="primary" class="el-icon-tickets" @click="calcStorageStatistics">汇总计算</el-button>
           <el-button v-if="permission.isPermisstion('STORAGEEXPORT')" icon="el-icon-download" style="margin-left:20px"   @click="exportData">导出</el-button>
      </div>
    </div>
    <div class="layout-container-table">
      <el-tabs v-model="tabsName" @tab-click="onTabsChanged">
        <el-tab-pane label="库存汇总信息" name="storage" :style="{height:tbHeight+'px'}">
          <Table ref="table"  v-model:page="page" v-loading="loading"  :data="tableData"  @getTableData="getTableData"   @orderChanged="handleSortChange">  
            <el-table-column prop="goodsClassifyName" label="分类" align="center" sortable="custom" :show-overflow-tooltip="true"/>   
              <el-table-column prop="goodsName" label="名称" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>  
              <el-table-column prop="goodsModel" label="型号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>
              <el-table-column prop="goodsNo" label="SAP编码" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/> 
              <el-table-column prop="supplierName" label="供应商" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
              <el-table-column prop="supplierNo" label="供应商编码" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true"/> 
              <el-table-column  label="包装" align="center"  :show-overflow-tooltip="true">
                <template #default="scope">  
                    <span v-if="scope.row.minPackageUnitName==scope.row.packageUnitName&&scope.row.packageUnitName==scope.row.maxPackageUnitName">{{scope.row.packageCount+scope.row.minPackageUnitName}}</span>
                    <span v-else-if="scope.row.minPackageUnitName!=scope.row.packageUnitName&&scope.row.packageUnitName==scope.row.maxPackageUnitName">{{scope.row.packageCount+scope.row.minPackageUnitName+'/'+scope.row.packageUnitName}}</span>
                    <span v-else-if="scope.row.minPackageUnitName==scope.row.packageUnitName&&scope.row.packageUnitName!=scope.row.maxPackageUnitName">{{scope.row.maxPackageCount+scope.row.packageUnitName+'/'+scope.row.maxPackageUnitName}}</span>
                    <span v-else>{{scope.row.packageCount+scope.row.minPackageUnitName+'/'+scope.row.packageUnitName+','+scope.row.maxPackageCount+scope.row.packageUnitName+'/'+scope.row.maxPackageUnitName}}</span>
                  </template>
              </el-table-column> 
              <el-table-column label="库存量" align="center"  min-width="140" :show-overflow-tooltip="true">
                <template #default="scope">
                  <span v-if="scope.row.minPackageUnitName==scope.row.packageUnitName&&scope.row.packageUnitName==scope.row.maxPackageUnitName">
                      <span>{{' '+scope.row.standardPackageStock+scope.row.packageUnitName}}</span>  
                  </span>
                    <span v-else-if="scope.row.minPackageUnitName!=scope.row.packageUnitName&&scope.row.packageUnitName==scope.row.maxPackageUnitName">
                      <span v-if="scope.row.standardPackageStock!=0">{{' '+scope.row.standardPackageStock+scope.row.packageUnitName}}</span> 
                          <span v-if="scope.row.minPackageStock!=0">{{scope.row.minPackageStock+scope.row.minPackageUnitName}}</span>  
                    </span>
                    <span v-else-if="scope.row.minPackageUnitName==scope.row.packageUnitName&&scope.row.packageUnitName!=scope.row.maxPackageUnitName">
                        <span v-if="scope.row.maxPackageStock!=0">{{' '+scope.row.maxPackageStock+scope.row.maxPackageUnitName}}</span>  
                      <span v-if="scope.row.standardPackageStock!=0">{{' '+scope.row.standardPackageStock+scope.row.packageUnitName}}</span>  
                    </span>
                    <span v-else>
                        <span v-if="scope.row.maxPackageStock!=0">{{' '+scope.row.maxPackageStock+scope.row.maxPackageUnitName}}</span>  
                      <span v-if="scope.row.standardPackageStock!=0">{{' '+scope.row.standardPackageStock+scope.row.packageUnitName}}</span>   
                        <span v-if="scope.row.minPackageStock!=0">{{scope.row.minPackageStock+scope.row.minPackageUnitName}}</span>  
                    </span> 
                </template>
              </el-table-column> 
              <el-table-column prop="safetyInventory"  label="安全库存" align="center" min-width="130" sortable="custom" :show-overflow-tooltip="true">
                <template #default="scope">
                  <span v-if="scope.row.safetyInventory!=0">{{scope.row.safetyInventory+scope.row.safetyInventoryUnitName}}</span>
                  <span v-if="scope.row.safetyInventory==0" class="text-deft">--</span>
                </template>
              </el-table-column>      
              <el-table-column  :label="$t('message.common.handle')" align="center"  min-width="200">
                <template #default="scope"> 
                  <el-button @click="showStorageDetails(scope.row)" type="plain">仓储明细</el-button>
                  <el-button @click="showStorageFlow(scope.row)" type="plain"  v-if="permission.isPermisstion('STORAGEFLOWREAD')">流水账目</el-button>
                </template>
              </el-table-column>
          </Table>
        </el-tab-pane>
        <el-tab-pane label="库存明细信息" name="storageDetail" :style="{height:tbHeight+'px'}">
          <Table ref="table"  v-model:page="detailPage" v-loading="detailLoading"  :data="detailTableData"  @getTableData="getDetailTableData"   @orderChanged="handleDetailSortChange">  
            <el-table-column prop="goodsClassifyName" label="分类" align="center" sortable="custom" :show-overflow-tooltip="true"/>   
              <el-table-column prop="goodsName" label="名称" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>  
              <el-table-column prop="goodsModel" label="型号" align="center" sortable="custom" :show-overflow-tooltip="true"/> 
              <el-table-column prop="goodsNo" label="SAP编码" align="center" sortable="custom" :show-overflow-tooltip="true"/> 
              <el-table-column prop="supplierName" label="供应商" align="center" sortable="custom" :show-overflow-tooltip="true"/> 
              <el-table-column prop="supplierNo" label="供应商编码" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true"/>
              <el-table-column prop="warehouseName" label="仓库" align="center" sortable="custom" :show-overflow-tooltip="true"/>
              <el-table-column prop="shelfName" label="货架" align="center" sortable="custom" :show-overflow-tooltip="true"/>
              <el-table-column prop="binName" label="货位" align="center" sortable="custom" :show-overflow-tooltip="true"/>
              <el-table-column prop="cellNo" label="料箱" align="center" sortable="custom" :show-overflow-tooltip="true"/> 
              <el-table-column prop="stock" label="库存量" align="center"  min-width="140" :show-overflow-tooltip="true">
                <template #default="scope">
                  <span>{{scope.row.stock+' '+scope.row.unitName}}</span> 
                </template>
              </el-table-column> 
               
          </Table>
        </el-tab-pane>
      </el-tabs>
   
      <DetailModal :layer="detailLayer"  v-if="detailLayer.show" />    
      <FlowModal :layer="flowLayer"   v-if="flowLayer.show" /> 
      <AllocationModal :layer="allcationLayer"  @allocationDataSubmit="allocationSubmit" v-if="allcationLayer.show" />    
    </div>
  </div>
</template>

<script lang="ts" setup>
  defineOptions({
    name: "storage"
  })
import { ref, reactive,onMounted,onBeforeMount } from "vue";
import { Page } from "@/components/table/type";
import { getStorageList,getStorageDetailList,getStorageDetails,storageStatistics,addAllocationStorage,exportStorage,exportStorageDetail } from "@/api/inv/storage";
import { LayerInterface } from "@/components/layer/index.vue"; 
import Table from "@/components/table/tableServer.vue";
import DetailModal from "./detailLayer.vue";  
import FlowModal from "./flowLayer.vue";   
import AllocationModal from './allocationLayer.vue';
import permission from '@/utils/system/permission';
import msg from "@/utils/system/message";
import {getWarehouses,getGoodsGroup} from '@/api/common';  
import { deftClassifyGroup} from '@/config';
 
 const tbHeight=ref(0)
 const tabsName=ref('storage')
 const query = reactive({
      input: "",
    });   
  var warehouseDataBuffer=new Array<any>();
  const warehouseData=ref(new Array<any>());
  const selectedWarehouseId=ref('');
  const goodsGroupData=ref(new Array<any>()); 
  const selectedGoodsGroup=ref(deftClassifyGroup); 
  const page: Page = reactive({
    index: 1,
    size: 20,
    total: 0,
    orderField:'',
    orderType:''
  });
  const loading = ref(true);
  const tableData = ref([]);  
  const detailPage: Page = reactive({
    index: 1,
    size: 20,
    total: 0,
    orderField:'',
    orderType:''
  });
  const detailLoading = ref(true);
  const detailTableData = ref([]);
  const detailLayer: LayerInterface = reactive({
    show: false,
    title: "仓储明细",
    showButton: false,
    btnLoading:false,
    width:"48%",
    data:null ,
    otherButton:{
            show:false,
            otherBtnLoading:false,
            text:"",
            type:""
          }
  }); 
  const flowLayer: LayerInterface = reactive({
    show: false,
    title: "流水账目",
    showButton: false,
    btnLoading:false,
    width:"60%",
    data:null ,
    otherButton:{
            show:false,
            otherBtnLoading:false,
            text:"",
            type:""
          }
  });
  const allcationLayer: LayerInterface = reactive({
    show: false,
    title: "库存调拨",
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
 
  onBeforeMount(()=>{
    tbHeight.value=window.innerHeight-260;
  })

  onMounted(() => {
    getGoodsGroupData();
    getWarehousesData().then(()=>{
      onGroupSelected();
    });
  })

  const getWarehousesData=()=>{
      return getWarehouses().then(res=>{
          if(res.data.length>0){ 
              warehouseDataBuffer=res.data.map((m:any)=>{
                return{
                  warehouseId:m.warehouseId,
                  warehouseName:m.warehouseName,
                  warehouseType:m.warehouseType
                }
              }); 
          } 
      })
  }
 
  const getGoodsGroupData=()=>{
      getGoodsGroup().then(res=>{
        if(deftClassifyGroup=='SparePart'){
          goodsGroupData.value=res.data.filter((f:any)=>f.key=='SparePart'||f.key=='Consumables');
        }
        else{
          goodsGroupData.value=res.data;
        } 
        getTableData(true);
      })
    } 

  const onGroupSelected=()=>{ 
    warehouseData.value.length=0;
    selectedWarehouseId.value='';
    let warehouseBygroup=warehouseDataBuffer.filter(f=>f.warehouseType==selectedGoodsGroup.value);  
    if(warehouseBygroup?.length>0){   
      warehouseData.value=[{warehouseId:'',warehouseName:'All'},...warehouseBygroup] 
      if(tabsName.value=='storage'){
        getTableData(true);
      }
      else{
        getDetailTableData(true);
      }
    }  
  }

  const onWarehouseSelected=()=>{
     if(tabsName.value=='storage'){
        getTableData(true);
      }
      else{
        getDetailTableData(true);
      }
  }

  const onSearch=()=>{
    if(tabsName.value=='storage'){
      getTableData(true);
    }
    else{
      getDetailTableData(true);
    }
  }

  const onTabsChanged=()=>{
    onSearch();
  }

  const getTableData = (init: Boolean) => {
    loading.value = true
    if (init) {
      page.index = 1
    }    
      loading.value = false
    getStorageList(page.size,page.index,page.orderField,page.orderType,query.input,selectedWarehouseId.value,selectedGoodsGroup.value)
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
    
  const showStorageDetails = (row: any) => {  
    getStorageDetails(row.goodsId).then((res:any)=>{ 
        detailLayer.show = true; 
        res.data.forEach((x:any)=>{
          x.goodsId=row.goodsId;
          x.goodsName=row.goodsName;
          x.goodsModel=row.goodsModel;
        })
        detailLayer.data=res.data;
    }) 
  }
  
  const showStorageFlow = (row: any) => {   
        flowLayer.show = true; 
        flowLayer.data=row;
  }
  
  const showAllcationModal = () => {   
        allcationLayer.show = true;  
  }
     
  const calcStorageStatistics=()=>{  
    storageStatistics(permission.getOperator().userId,permission.getOperator().userName).then(res=>{
      msg.successAuto("汇总计算完成");
      getTableData(true)
    })
  }
   
  const allocationSubmit=(data:any)=>{
    allcationLayer.btnLoading=true
    addAllocationStorage(data).then(res=>{
      allcationLayer.show=false
    }).finally(()=>{allcationLayer.btnLoading=false})
  }
 
  const exportData=()=>{
    if(tabsName.value=='storage'){
      exportStorageData();
    }
    else{
      exportStorageDetailData();
    }
  }

  const exportStorageData=()=>{   
      exportStorage(page.orderField,page.orderType,query.input,selectedWarehouseId.value,selectedGoodsGroup.value).then(res=>{ 
            let link = document.createElement('a') 
            link.style.display = 'none'
            link.href =res.data
            document.body.appendChild(link)
            link.click() 
            document.body.removeChild(link)
        })
  }

  const getDetailTableData = (init: Boolean) => {
    detailLoading.value = true
    if (init) {
      detailPage.index = 1
    }    
    detailLoading.value = false
    getStorageDetailList(detailPage.size,detailPage.index,detailPage.orderField,detailPage.orderType,query.input,selectedWarehouseId.value,selectedGoodsGroup.value)
      .then((res) => {
        let data = res.data.rows
        data.forEach((d: any) => {
          d.loading = false
        })
        detailTableData.value = data
        detailPage.total = Number(res.data.total);
      })
      .catch((error) => {
        detailTableData.value = [];
        detailPage.index = 1;
        detailPage.total = 0;
      })
      .finally(() => {
        detailLoading.value = false;
      })
  }
 
  const handleDetailSortChange=(orderRow:any)=>{ 
    getDetailTableData(true);
  }

  const exportStorageDetailData=()=>{   
      exportStorageDetail(page.orderField,page.orderType,query.input,selectedWarehouseId.value,selectedGoodsGroup.value).then(res=>{ 
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
.statusName {
  margin-right: 10px;
}
</style>
