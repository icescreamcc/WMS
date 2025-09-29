<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
        <el-button v-if="permission.isPermisstion('WAREHOUSEADD')" type="primary" icon="el-icon-circle-plus-outline" @click="handleAdd">新增</el-button> 
         <el-popconfirm title='确定删除选中的数据吗' v-if="permission.isPermisstion('WAREHOUSEDEL')"  @confirm="handleDel(chooseData)">
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
        @expandChange="handleExpandChange"
         @orderChanged="handleSortChange"  
      > 
        <el-table-column prop="warehouseNo" label="仓库编码"   sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
        <el-table-column prop="warehouseName" label="仓库名称"  sortable="custom" min-width="120" :show-overflow-tooltip="true"/>   
        <el-table-column prop="chargePersonPhone" label="负责人" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>   
        <el-table-column prop="remark" label="备注"  sortable="custom" min-width="80" :show-overflow-tooltip="true"/>      
        <el-table-column prop="isAbandon" label="是否弃用" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="scope"> 
            <span v-if="scope.row.isAbandon" class="text-danger">已弃用</span> 
          </template>
        </el-table-column>  
        <el-table-column v-if="getSpareFields('SpareField1')" prop="spareField1" :label="getSpareFields('SpareField1').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('SpareField2')" prop="spareField2" :label="getSpareFields('SpareField2').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('SpareField3')" prop="spareField3" :label="getSpareFields('SpareField3').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('SpareField4')" prop="spareField4" :label="getSpareFields('SpareField4').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>  
        <el-table-column v-if="getSpareFields('SpareField5')" prop="spareField5" :label="getSpareFields('SpareField5').fieldDesc"   min-width="120" sortable="custom" :show-overflow-tooltip="true"/>     
        <el-table-column
          v-if="permission.isPermisstion('WAREHOUSEUPDATE','WAREHOUSEDEL')"
          :label="$t('message.common.handle')"
          align="center" 
          width="200"
        >
          <template #default="scope">
            <el-button  v-if="permission.isPermisstion('WAREHOUSEUPDATE')" @click="handleEdit(scope.row)">{{
              $t("message.common.update")
            }}</el-button>
            <el-popconfirm
             v-if="permission.isPermisstion('WAREHOUSEDEL')"
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
    </div>
  </div>
</template>

<script lang="ts"> 
import { defineComponent, ref, reactive } from "vue"; 
import { getWarehouses,getWarehouseBins,getEnableSpareFields,addWarehouseBin,updateWarehouseBin,delWarehouseBin} from "@/api/inv/warehouse";
import { LayerInterface } from "@/components/layer/index.vue"; 
import Table from "@/components/table/tableServer.vue";
import EditModal from "./editLayer.vue";  
import { Page } from "@/components/table/type";
import permission from '@/utils/system/permission'
export default defineComponent({
  name: "warehouse",
  components: {
    Table,
    EditModal
  },
  setup() {  
    // 存储搜索用的数据
    let query = reactive({
      input: "",
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

    //编辑弹窗控制器
    let editLayer: LayerInterface = reactive({
      show: false,
      title: "",
      showButton: true,
      btnLoading:false,
      width:"45%",
      data:null,
      type:'',
      binArgs:false,
      otherButton:{ }
    });  
   

   //排序事件
    let handleSortChange=(orderRow:any)=>{   
      getTableData(true);
    }

   //多选
    let handleSelectionChange = (val: []) => {
      chooseData.value = val;
    };

    //展开与收缩
    let binList=ref(new Array<any>()) 
    let handleExpandChange=(row:any)=>{  
       getWarehouseBins(row.warehouseId).then(res=>{
            binList.value=binList.value.filter((b:any)=>b.warehouseId!=row.warehouseId)  
            res.data?.forEach((b:any) => {
                binList.value.push(b)
              });   
       }) 
    }

    // 获取表格数据 
   let getTableData = (init: Boolean) => {
      loading.value = true
      if (init) {
        page.index = 1
      }   
       loading.value = false
      getWarehouses(permission.getOperator().userId, page.size,page.index,page.orderField,page.orderType,query.input)
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

   getTableData(true)
 
//获取备用字段信息
  getEnableSpareFields().then((res:any)=>{
    fieldsData.value=res.data
  })

  const getSpareFields:any=(fieldsName:string)=>{ 
   let obj= fieldsData.value?.filter((x:any)=>x.fieldName==fieldsName) 
    return obj[0]
  }
  
     // 删除功能
    let handleDel = (data: any) => {   
      let id=data.map((x:any)=>{return x.warehouseId} ) 
      delWarehouseBin(id).then((res) => { 
        getTableData(tableData.value.length === 1 ? true : false);
      });
    }

    // 新增弹窗功能
    let handleAdd = () => {
      editLayer.title = "添加仓库";
      editLayer.show = true; 
      editLayer.type='add'
      editLayer.data=fieldsData.value;  
      delete editLayer.row;
    }

    // 编辑弹窗功能
    let handleEdit = (row: any) => {  
        getWarehouseBins(row.warehouseId).then((res:any)=>{
          editLayer.title = "编辑仓库"; 
          editLayer.show = true;
          editLayer.type='update'
          editLayer.data=fieldsData.value; 
          editLayer.row = row;  
          editLayer.row.warehouseBin=res.data
        }) 
    }
   

    //新增或编辑数据提交
    let dataSave=(data:any,actionType:string)=>{   
      editLayer.btnLoading=true;
        if(actionType=='add'){ 
          addWarehouseBin(data).then(res=>{
            editLayer.show = false;
            getTableData(true);
          }).finally(()=> editLayer.btnLoading=false);
        }
        else{
           updateWarehouseBin(data).then(res=>{
            editLayer.show = false;
            getTableData(false);
          }).finally(()=> editLayer.btnLoading=false);
        }
    }
       
    return { 
      permission,
      page,
      query, 
      tableData, 
      loading, 
      chooseData,
      editLayer,   
      binList,  
      getSpareFields,
      getTableData,
      handleSelectionChange,
      handleExpandChange,
      handleSortChange,
      handleDel,
      handleAdd,
      handleEdit, 
      dataSave
    };
  }
});
</script>

<style lang="scss" scoped>
.statusName {
  margin-right: 10px;
}
</style>
