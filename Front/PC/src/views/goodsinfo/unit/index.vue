<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
        <el-button type="primary" v-if="permission.isPermisstion('UNITADD')" icon="el-icon-circle-plus-outline" @click="handleAdd">新增</el-button>
        <el-popconfirm title='确定删除选中的数据吗' v-if="permission.isPermisstion('UNITDEL')" @confirm="handleDel(chooseData)">
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
        @orderChanged="handleSortChange"
      >
        <el-table-column prop="unitNo" label="单位编号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
        <el-table-column prop="unitName" label="单位名称" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>
         <el-table-column prop="unitType" label="单位类型" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
            <template #default="unit"> 
            <span>{{unit.row.unitTypeDesc}}</span> 
          </template>
         </el-table-column>
        <el-table-column prop="remark" label="备注" align="center" sortable="custom" :show-overflow-tooltip="true"/>   
        <el-table-column
          v-if="permission.isPermisstion('UNITUPDATE','UNITDEL')"
          :label="$t('message.common.handle')"
          align="center" 
          width="200"
        >
          <template #default="scope">
            <el-button  v-if="permission.isPermisstion('UNITUPDATE')" @click="handleEdit(scope.row)">{{
              $t("message.common.update")
            }}</el-button>
            <el-popconfirm
             v-if="permission.isPermisstion('UNITDEL')"
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
      <UnitEditModal :layer="unitLayer"  @dataSubmit="userDataSave" v-if="unitLayer.show" /> 
    </div>
  </div>
</template>

<script lang="ts" > 
import { defineComponent, ref, reactive } from "vue";
import { Page } from "@/components/table/type";
import { getUnits, addUnit,updateUnit, delUnit } from "@/api/baseinfo/unit";
import { LayerInterface } from "@/components/layer/index.vue"; 
import Table from "@/components/table/tableServer.vue";
import UnitEditModal from "./unitEditLayer.vue";   
import permission from '@/utils/system/permission'
export default defineComponent({
  name: 'unit',
  components: {
    Table,
    UnitEditModal,  
  },
  setup() { 
    // 存储搜索用的数据
    let query = reactive({
      input: "",
    });

    //编辑弹窗控制器
    let unitLayer: LayerInterface = reactive({
      show: false,
      title: "",
      showButton: true,
      btnLoading:false,
      width:"30%",
      data:null ,
      type:'add',
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
      getUnits(page.size,page.index,page.orderField,page.orderType,query.input)
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

     // 删除功能
    let handleDel = (data: any[]) => {  
      let ids=Array<any>();
      data.forEach(d=>{
        ids.push(d.unitId);
      })  
      delUnit(ids).then((res) => { 
        getTableData(tableData.value.length === 1 ? true : false);
      });
    }

    // 新增弹窗功能
    let handleAdd = () => {
      unitLayer.title = "新增单位";
      unitLayer.show = true; 
      unitLayer.type='add'
      delete unitLayer.row;
    }

    // 编辑弹窗功能
    let handleEdit = (row: any) => {   
         unitLayer.title = "编辑单位";
         unitLayer.row = row; 
         unitLayer.type='update'
         unitLayer.show = true;  
    }
   

    //新增或编辑数据提交
    let userDataSave=(data:any,actionType:string)=>{ 
      unitLayer.btnLoading=true;
        if(actionType=='add'){
          addUnit(data).then(res=>{
            unitLayer.show = false;
            getTableData(true);
          }).finally(()=> unitLayer.btnLoading=false);
        }
        else{
           updateUnit(data).then(res=>{
            unitLayer.show = false;
            getTableData(false);
          }).finally(()=> unitLayer.btnLoading=false);
        }
    }
      
    getTableData(true)
    return {
      query, 
      tableData,
      chooseData,
      loading,
      page, 
      unitLayer, 
      permission,
      handleSelectionChange,
      handleSortChange,
      getTableData,
      handleDel,
      handleAdd,
      handleEdit,  
      userDataSave
    };
  }
});
</script>

<style lang="scss" scoped>
.statusName {
  margin-right: 10px;
}
</style>
