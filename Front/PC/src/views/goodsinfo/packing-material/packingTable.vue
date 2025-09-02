<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
           <h2>包材列表</h2>
      </div> 
        <div class="layout-container-form-search" style="text-align:right"> 
        <el-input 
          v-model="query.input"
          placeholder="请输入关键词进行检索(名称/编码/型号/属性)"
          size="small"
        ></el-input>
        <el-button
          type="primary"
          icon="el-icon-search"
          class="search-btn"
          @click="getTableData(true)"
          >搜索</el-button>   
       <el-button type="primary" v-if="permission.isPermisstion('PACKINGMATERIALADD')"  icon="el-icon-circle-plus-outline" :disabled="addDisabled" @click="handleAdd" >新增</el-button> 
        <el-popconfirm title='确定删除选中的数据吗' v-if="permission.isPermisstion('PACKINGMATERIALDEL')"   @confirm="handleDel(chooseData)">
          <template #reference>
            <el-button type="danger" icon="el-icon-delete" :disabled="chooseData.length === 0">批量删除</el-button>
          </template>
        </el-popconfirm>
           <el-button icon="el-icon-download" v-if="permission.isPermisstion('PACKINGMATERIALEXPORT')"  style="margin-left:20px"  @click="getExportAllowField">导出</el-button>
      </div>
    </div>
    <div class="layout-container-table" style="margin-top:-10px">
      <Table
        ref="table"
        v-model:page="page"
        v-loading="loading"
        :isShowSum="true" 
        :data="tableData" 
        :showSelection="true" 
        @getTableData="getTableData"
        @selection-change="handleSelectionChange"
        @orderChanged="handleSortChange" 
        >
         <el-table-column prop="goodsNo" label="SAP编码" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>  
            <el-table-column prop="goodsClassifyName" label="包材分类" align="center" sortable="custom"  min-width="100"  :show-overflow-tooltip="true"/>      
  
        <el-table-column prop="goodsName" label="包材名称" min-width="100" sortable="custom"  :show-overflow-tooltip="true"/> 
         <el-table-column prop="goodsProperty" label="包材属性" min-width="100" align="center" sortable="custom" :show-overflow-tooltip="true"/> 
        <el-table-column prop="packageCount" label="每包装数量" align="center"  min-width="120" sortable="custom"  :show-overflow-tooltip="true">
               <template #default="scope"> 
                   <span v-if="scope.row.minPackageUnitName==scope.row.packageUnitName&&scope.row.packageUnitName==scope.row.maxPackageUnitName">{{scope.row.packageCount+scope.row.minPackageUnitName}}</span>
                   <span v-else-if="scope.row.minPackageUnitName!=scope.row.packageUnitName&&scope.row.packageUnitName==scope.row.maxPackageUnitName">{{scope.row.packageCount+scope.row.minPackageUnitName+'/'+scope.row.packageUnitName}}</span>
                   <span v-else-if="scope.row.minPackageUnitName==scope.row.packageUnitName&&scope.row.packageUnitName!=scope.row.maxPackageUnitName">{{scope.row.maxPackageCount+scope.row.packageUnitName+'/'+scope.row.maxPackageUnitName}}</span>
                   <span v-else>{{scope.row.packageCount+scope.row.minPackageUnitName+'/'+scope.row.packageUnitName+','+scope.row.maxPackageCount+scope.row.packageUnitName+'/'+scope.row.maxPackageUnitName}}</span>
                </template>
         </el-table-column>   
            <el-table-column prop="safetyInventory" label="安全库存"   align="center"  sortable="custom" min-width="120" :show-overflow-tooltip="true">
                  <template #default="scope"> 
                   <span v-if="scope.row.safetyInventory>0">{{scope.row.safetyInventory+scope.row.safetyInventoryUnitName}}</span>
                    <span v-else><span class="text-deft">~</span>{{scope.row.safetyInventoryUnitName}}</span>
                </template>
           </el-table-column> 
           <el-table-column prop="supplier" label="供应商" min-width="100" sortable="custom"  :show-overflow-tooltip="true"/>   
        <el-table-column prop="isInSAP" label="是否在SAP" align="center" min-width="120"  sortable="custom" :show-overflow-tooltip="true">
          <template #default="scope"> 
                   <span>{{scope.row.isInSAP?'是':'否'}}</span>
                </template>
        </el-table-column>    
        <el-table-column prop="remark" label="备注"  sortable="custom"  :show-overflow-tooltip="true"/> 
        <el-table-column prop="createDate" label="创建日期"  sortable="custom"  min-width="100" :show-overflow-tooltip="true">
          <template #default="scope"> 
                   <span>{{commonHelper.formatToDateTime(scope.row.createDate) }}</span>
                </template>
        </el-table-column>   
        <el-table-column v-if="getSpareFields('GoodsField1')" prop="goodsField1" :label="getSpareFields('GoodsField1').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('GoodsField2')" prop="goodsField2" :label="getSpareFields('GoodsField2').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('GoodsField3')" prop="goodsField3" :label="getSpareFields('GoodsField3').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('GoodsField4')" prop="goodsField4" :label="getSpareFields('GoodsField4').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('GoodsField5')" prop="goodsField5" :label="getSpareFields('GoodsField5').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="permission.isPermisstion('PACKINGMATERIALUPDATE','PACKINGMATERIALDEL')" :label="$t('message.common.handle')" align="center"  width="160"> 
          <template #default="scope">
            <el-button v-if="permission.isPermisstion('PACKINGMATERIALUPDATE')" @click="handleEdit(scope.row)">{{
              $t("message.common.update")
            }}</el-button>
            <el-popconfirm v-if="permission.isPermisstion('PACKINGMATERIALDEL')" 
              :title="$t('message.common.delTip')"
              @confirm="handleDel([scope.row])"
            >
              <template #reference>
                <el-button type="danger">{{ $t("message.common.del") }}</el-button>
              </template>
            </el-popconfirm> 
          </template>
        </el-table-column>
      </Table> 
        <EditModal :layer="editLayer" :spareClassify="currentType" @dataSubmit="dataSave" v-if="editLayer.show" /> 
        <ExportModal :layer="exportLayer"  @dataSubmit="exportData" v-if="exportLayer.show" />
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, reactive, inject, watch } from 'vue'
import Table from "@/components/table/tableServer.vue";
import { Page } from '@/components/table/type' 
import type { LayerInterface } from '@/components/layer/index.vue' 
import EditModal from './packingEditLayer.vue'
import {getPackingMaterialList,getEnableSpareFields,getPackingMaterialDetail,addPackingMaterial,updatePackingMaterial,delPackingMaterial,exportPackingMaterial,getAllowField} from '@/api/baseinfo/packingMaterial'
import ExportModal from "@/components/layer/exportLayer.vue" 
import permission from '@/utils/system/permission'
import commonHelper from '@/utils/system/common-helper';

const query = reactive({
      input: "",
    });
  //编辑弹窗控制器
const editLayer: LayerInterface = reactive({
  show: false,
  title: "",
  showButton: true,
  btnLoading:false,
  width:"50%",
  data:null ,
  otherButton:{ }
});

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

// 分页参数, 供table使用
const page: Page = reactive({
  index: 1,
  size: 20,
  total: 0,
    orderField:'',
  orderType:''
})
const currentType=ref({
    typeId:'',
    typeName:''
  })
const addDisabled=ref(true)
const activeCategory: any = inject('active')
const loading = ref(true)
const tableData = ref([])
const fieldsData=ref([]) 
const chooseData = ref([])
const handleSelectionChange = (val: []) => {
  chooseData.value = val
  
}

//排序事件
const handleSortChange=(orderRow:any)=>{ 
  getTableData(true);
}

  // 获取表格数据
// params <init> Boolean ，默认为false，用于判断是否需要初始化分页
const getTableData = (init: Boolean) => {
  loading.value = true
  if (init) {
    page.index = 1
  }   
    loading.value = false
    let typeid=0;
    if(activeCategory.value.id&&activeCategory.value.id!='ROOT'){
    typeid=Number(activeCategory.value.id) 
    } 

  getPackingMaterialList(permission.getOperator().userId, page.size as number,page.index as number,page.orderField,page.orderType,typeid,query.input)
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

getEnableSpareFields().then((res:any)=>{
fieldsData.value=res.data
})

const getSpareFields:any=(fieldsName:string)=>{ 
  let obj= fieldsData.value?.filter((x:any)=>x.fieldName==fieldsName) 
  return obj[0]
}

  // 编辑弹窗功能
const handleEdit = (row: any) => {  
  getPackingMaterialDetail(row.goodsId).then((res:any)=>{
    editLayer.title = "";
    editLayer.row = res.data; 
    editLayer.show = true; 
    editLayer.type='update'
    editLayer.data=fieldsData.value;
  })
}

  // 新增弹窗功能
const handleAdd = () => {
  editLayer.title = "";
  editLayer.show = true; 
  editLayer.type='add'
  editLayer.data=fieldsData.value;
  delete editLayer.row;
}

// 删除功能
const handleDel = (data: object[]) => {
    let ids=Array<any>();
  data.forEach((d:any)=>{
    ids.push(d.goodsId);
  }) 
  delPackingMaterial(ids).then((res) => { 
    getTableData(tableData.value.length === 1 ? true : false);
  }); 
}  

  //新增或编辑数据提交
const dataSave=(data:any,actionType:string)=>{  
  editLayer.btnLoading=true;
      data.createUser=permission.getOperator().userName;
    if(actionType=='add'){
      addPackingMaterial(data).then(res=>{
        editLayer.show = false;
        getTableData(true);
      }).finally(()=> editLayer.btnLoading=false);
    }
    else{
        updatePackingMaterial(data).then(res=>{
        editLayer.show = false;
        getTableData(false);
      }).finally(()=> editLayer.btnLoading=false);
    }
}

//获取被允许导出的字段列表
const getExportAllowField=()=>{
  getAllowField().then(res=>{
    exportLayer.row = permission.getOperator().userId; 
    exportLayer.show = true;
    exportLayer.data=res.data; 
  }) 
}

  //导出客户 
const exportData=(selField:Array<any>)=>{ 
  let classifyId=0;
    if(activeCategory.value.id&&activeCategory.value.id!='ROOT'){
      classifyId=Number(activeCategory.value.id) 
    } 
    console.log("activeCategory.value.id",activeCategory.value.id)
  exportLayer.btnLoading=true;
  exportPackingMaterial(query.input,page.orderField,page.orderType,classifyId,selField).then(res=>{ 
    let link = document.createElement('a') 
      link.style.display = 'none'
      link.href =res.data
      document.body.appendChild(link)
      link.click() 
      document.body.removeChild(link)
  }).finally(()=>exportLayer.btnLoading=false)
}

watch(activeCategory, (newVal) => {
  if(activeCategory.value.id=="ROOT"){
    addDisabled.value=true
  }
  else{
    addDisabled.value=false
    currentType.value={
    typeId:newVal.id,
    typeName:newVal.label
  }
  }
  getTableData(true)
}) 
</script>

<style lang="scss" scoped>
  .layout-container {
    height: 100%;
    margin: 0 0 0 10px;
    width: calc(100% - 10px);
    h2 {
      padding: 0;
      margin: 0;
      margin-right: 20px;
      font-size: 14px;
      display: -webkit-box;
      -webkit-line-clamp: 1;
      -webkit-box-orient: vertical;
      overflow: hidden;
      height: 30px;
      line-height: 30px;
    }
  }
</style>