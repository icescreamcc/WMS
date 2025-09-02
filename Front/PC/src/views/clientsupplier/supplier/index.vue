<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
        <el-button type="primary" v-if="permission.isPermisstion('SUPPLIERADD')" icon="el-icon-circle-plus-outline" @click="handleAdd">新增</el-button>
        <el-popconfirm title='确定删除选中的数据吗' v-if="permission.isPermisstion('SUPPLIERDEL')"  @confirm="handleDel(chooseData)">
          <template #reference>
            <el-button type="danger"  icon="el-icon-delete" :disabled="chooseData.length === 0">批量删除</el-button>
          </template>
        </el-popconfirm> 
      </div>
      <div class="layout-container-form-search"> 
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
          <el-button v-if="permission.isPermisstion('SUPPLIEREXPORT')" icon="el-icon-download" style="margin-left:20px" type="info" @click="getExportAllowField">导出</el-button>
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
        <el-table-column prop="supplierId" label="供应商ID"  sortable="custom" min-width="110" :show-overflow-tooltip="true"/>
        <el-table-column prop="supplierNo" label="供应商编码"  sortable="custom" min-width="110" :show-overflow-tooltip="true"/>
        <el-table-column prop="supplierName" label="供应商名称"  sortable="custom" min-width="110" :show-overflow-tooltip="true"/> 
        <el-table-column prop="supplierTypeName" label="供应商类型"  sortable="custom" min-width="110" :show-overflow-tooltip="true"/>
        <el-table-column prop="supplierPropertyName" label="供应商性质"  sortable="custom" min-width="110"  :show-overflow-tooltip="true"/>   
        <el-table-column prop="province" label="省份"  sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column prop="city" label="城市"  sortable="custom" :show-overflow-tooltip="true"/> 
        <el-table-column prop="isImportant" label="重要供应商" align="center" min-width="110"  sortable="custom" :show-overflow-tooltip="true">
         <template #default="scope"> 
            <i v-if="scope.row.isImportant" class="iconfont ionfont-sm icon-shoucang text-danger"></i>
          </template>
        </el-table-column> 
        <el-table-column prop="remark" label="备注"  sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('SpareField1')" prop="spareField1" :label="getSpareFields('SpareField1').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('SpareField2')" prop="spareField2" :label="getSpareFields('SpareField2').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('SpareField3')" prop="spareField3" :label="getSpareFields('SpareField3').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('SpareField4')" prop="spareField4" :label="getSpareFields('SpareField4').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('SpareField5')" prop="spareField5" :label="getSpareFields('SpareField5').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column
          :label="$t('message.common.handle')"
           align="center"  
          width="200"
          v-if="permission.isPermisstion('SUPPLIERUPDATE','SUPPLIERDEL')"
        >
          <template #default="scope">
            <el-button  v-if="permission.isPermisstion('SUPPLIERUPDATE')" @click="handleEdit(scope.row)">{{
              $t("message.common.update")
            }}</el-button>
            <el-popconfirm
               v-if="permission.isPermisstion('SUPPLIERDEL')"
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
      <EditModal :layer="editLayer"  @dataSubmit="dataSave" v-if="editLayer.show" /> 
      <ExportModal :layer="exportLayer"  @dataSubmit="exportData" v-if="exportLayer.show" />
    </div>
  </div>
</template>

<script lang="ts"> 
import { defineComponent, ref, reactive } from "vue";
import { Page } from "@/components/table/type";
import { getSuppliers,getSupplierDetail,getEnableSpareFields, addSupplier,updateSupplier,delSupplier,exportSuppliers ,getAllowField} from "@/api/baseinfo/supplier";
import { LayerInterface } from "@/components/layer/index.vue"; 
import Table from "@/components/table/tableServer.vue";
import EditModal from "./editLayer.vue";  
import ExportModal from "@/components/layer/exportLayer.vue"
import permission from '@/utils/system/permission'
export default defineComponent({
  name: "supplier",
  components: {
    Table,
    EditModal,  
    ExportModal
  },
  setup() { 
    // 存储搜索用的数据
    let query = reactive({
      input: "",
    });

    //编辑弹窗控制器
    let editLayer: LayerInterface = reactive({
      show: false,
      title: "",
      showButton: true,
      btnLoading:false,
      width:"55%",
      data:null ,
      otherButton:{ }
    });

    //数据导出弹窗控制器
    let exportLayer: LayerInterface = reactive({
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
    });
    let loading = ref(true);
    let tableData = ref([]);
    let fieldsData=ref([]) 
    let chooseData = ref([]);
    let handleSelectionChange = (val: []) => {
      chooseData.value = val;
    };
 

    //排序事件
    let handleSortChange=(orderRow:any)=>{  
      getTableData(true);
    }

    // 获取表格数据
    // params <init> Boolean ，默认为false，用于判断是否需要初始化分页
    let getTableData = (init: Boolean) => {
      loading.value = true
      if (init) {
        page.index = 1
      }   
       loading.value = false
      getSuppliers(permission.getOperator().userId, page.size,page.index,page.orderField,page.orderType,query.input)
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

  let getSpareFields:any=(fieldsName:string)=>{ 
   let obj= fieldsData.value?.filter((x:any)=>x.fieldName==fieldsName) 
    return obj[0]
  }
     // 删除功能
    let handleDel = (data: any[]) => {   
      let ids=Array<any>();
      data.forEach(d=>{
        ids.push(d.supplierId);
      }) 
      delSupplier(ids).then((res) => { 
        getTableData(tableData.value.length === 1 ? true : false);
      });
    }

    // 新增弹窗功能
    let handleAdd = () => {
      editLayer.title = "";
      editLayer.show = true; 
      editLayer.data=fieldsData.value; 
      delete editLayer.row;
    }

    // 编辑弹窗功能
    let handleEdit = (row: any) => {   
        getSupplierDetail(row.supplierId).then((res:any)=>{
         editLayer.title = "";
         editLayer.row = res.data; 
         editLayer.show = true;
         editLayer.data=fieldsData.value;
        }) 
    }
   

    //新增或编辑数据提交
    let dataSave=(data:any,actionType:string)=>{ 
      data.createUser=permission.getOperator().userId;
      editLayer.btnLoading=true;
        if(actionType=='add'){
          addSupplier(data).then(res=>{
            editLayer.show = false;
            getTableData(true);
          }).finally(()=> editLayer.btnLoading=false);
        }
        else{
           updateSupplier(data).then(res=>{ 
            editLayer.show = false;
            getTableData(false);
          }).finally(()=> editLayer.btnLoading=false);
        }
    }

    //获取被允许导出的字段列表
    let getExportAllowField=()=>{
      getAllowField(permission.getOperator().userId).then(res=>{
        exportLayer.row = permission.getOperator().userId; 
        exportLayer.show = true;
        exportLayer.data=res.data?.map((m:any)=>{
          return{
            key:m.fieldName,
            value:m.fieldDesc
          }
        }); 
      }) 
    }

     //导出供应商 
    let exportData=(selField:Array<any>)=>{ 
      exportLayer.btnLoading=true
        exportSuppliers(query.input,page.orderField,page.orderType,selField).then(res=>{ 
          let link = document.createElement('a') 
            link.style.display = 'none'
            link.href =res.data
            document.body.appendChild(link)
            link.click() 
            document.body.removeChild(link)
        }).finally(()=>exportLayer.btnLoading=false)
    }
      
    getTableData(true)
    return {
      query, 
      tableData,
      fieldsData,
      chooseData,
      loading,
      page, 
      editLayer,  
      exportLayer, 
      permission,
      getSpareFields,
      handleSelectionChange,
      handleSortChange,
      getTableData,
      handleDel,
      handleAdd,
      handleEdit,  
      dataSave,
      getExportAllowField,
      exportData
    };
  }
});
</script>

<style lang="scss" scoped>
.statusName {
  margin-right: 10px;
}
</style>
