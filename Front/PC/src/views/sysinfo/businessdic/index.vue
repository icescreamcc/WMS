<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
        <el-button v-if="permission.isPermisstion('BUSINESSDICADD')" type="primary" icon="el-icon-circle-plus-outline" @click="handleAdd">新增</el-button> 
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
          @click="getTableData()"
          >搜索</el-button>  
      </div>
    </div>
    <div class="layout-container-table">
      <Table
        ref="table" 
        v-loading="loading" 
        :data="tableData"   
      >
         <el-table-column prop="argsId" label="选项" min-width="130" type="expand" align="center" sortable :show-overflow-tooltip="true"> 
             <template #default="props">
                   <span style="font-weight:600;">{{props.row.argsKeyName}}选项</span>
              <p v-for="opt in props.row.argsOptions" :key="opt.optionId">● {{ opt.optionName}}</p> 
            </template> 
          </el-table-column> 
        <el-table-column prop="argsKey" label="字典编码"  align="center" sortable :show-overflow-tooltip="true"/>
        <el-table-column prop="argsKeyName" label="字典名称" align="center" sortable min-width="80" :show-overflow-tooltip="true"/>  
        <el-table-column prop="remark" label="备注" align="center" sortable min-width="80" :show-overflow-tooltip="true"/>      
       <el-table-column prop="rank" label="排序" align="center" sortable min-width="80" :show-overflow-tooltip="true"/>      
        <el-table-column
          v-if="permission.isPermisstion('BUSINESSDICUPDATE','BUSINESSDICDEL')"
          :label="$t('message.common.handle')"
          align="center" 
          width="200"
        >
          <template #default="scope">
            <el-button  v-if="permission.isPermisstion('BUSINESSDICUPDATE')" @click="handleEdit(scope.row)">{{
              $t("message.common.update")
            }}</el-button>
            <!-- <el-popconfirm
             v-if="permission.isPermisstion('BUSINESSDICDEL')"
              :title="$t('message.common.delTip')"
              @confirm="handleDel(scope.row)"
            >
              <template #reference>
                <el-button type="danger">{{ $t("message.common.del") }}</el-button>
              </template>
            </el-popconfirm> -->
          </template>
        </el-table-column>
      </Table>
      <EditModal :layer="editLayer"  @dataSubmit="dataSave" v-if="editLayer.show" /> 
    </div>
  </div>
</template>

<script lang="ts"> 
import { defineComponent, ref, reactive } from "vue"; 
import { getArgs,addArgs,updateArgs,delArgs} from "@/api/system/businessDic";
import { LayerInterface } from "@/components/layer/index.vue"; 
import Table from "@/components/table/tableClient.vue";
import EditModal from "./editLayer.vue";  
import permission from '@/utils/system/permission'
export default defineComponent({
  name: "businessdic",
  components: {
    Table,
    EditModal
  },
  setup() {  
    // 存储搜索用的数据
    let query = reactive({
      input: "",
    });

    //公告编辑弹窗控制器
    let editLayer: LayerInterface = reactive({
      show: false,
      title: "",
      showButton: true,
      btnLoading:false,
      width:"45%",
      data:null,
      type:'',
      otherButton:{ }
    }); 
 
    let loading = ref(true);
    let tableData = ref([]);  
  
    // 获取表格数据 
    let getTableData = () => {
      loading.value = true
      getArgs(query.input).then((res:any)=>{
        tableData.value=res.data
      }).finally(()=>loading.value = false) 
     }

     // 删除功能
    let handleDel = (data: any) => {   
      delArgs(data.argsKey).then((res) => { 
        getTableData();
      });
    }

    // 新增弹窗功能
    let handleAdd = () => {
      editLayer.title = "添加字典";
      editLayer.show = true; 
      editLayer.type='add'
      delete editLayer.data;
    }

    // 编辑弹窗功能
    let handleEdit = (row: any) => { 
        editLayer.title = "编辑字典"; 
        editLayer.show = true;
        editLayer.type='update'
        editLayer.data=row;
    }
   

    //新增或编辑数据提交
    let dataSave=(data:any,actionType:string)=>{   
      editLayer.btnLoading=true;
        if(actionType=='add'){ 
          addArgs(data).then(res=>{
            editLayer.show = false;
            getTableData();
          }).finally(()=> editLayer.btnLoading=false);
        }
        else{
           updateArgs(data).then(res=>{
            editLayer.show = false;
            getTableData();
          }).finally(()=> editLayer.btnLoading=false);
        }
    }
      
    getTableData()
    return { 
      permission,
      query, 
      tableData, 
      loading, 
      editLayer,  
      getTableData,
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
