<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
        <el-button type="primary"  v-if="permission.isPermisstion('COMPANYANNOUNCEADD')" icon="el-icon-circle-plus-outline" @click="handleAdd">新增</el-button>
        <el-popconfirm title='确定删除选中的数据吗'  v-if="permission.isPermisstion('COMPANYANNOUNCEDEL')"  @confirm="handleDel(chooseData)">
          <template #reference>
            <el-button type="danger"  icon="el-icon-delete" :disabled="chooseData.length === 0">批量删除</el-button>
          </template>
        </el-popconfirm> 
      </div>
      <div class="layout-container-form-search">
       <el-date-picker 
        v-model="dateRange"
        type="daterange"
        range-separator="-"
        start-placeholder="最早日期"
        end-placeholder="最晚日期"
         size="small"
         value-format="YYYY-MM-DD"
         style="margin-right:10px;width:120%"
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
         <el-table-column prop="dateTime" label="发布时间" align="center" sortable="custom" :show-overflow-tooltip="true"/> 
           <el-table-column prop="remark" label="标题" align="center" sortable="custom" min-width="80" :show-overflow-tooltip="true"/>  
        <el-table-column prop="content" label="发布内容" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>   
        <el-table-column prop="isPublishCurrent" label="是否当前发布" align="center" sortable="custom" :show-overflow-tooltip="true">
            <template #default="scope">
            <span class="text-success" v-if="scope.row.isPublishCurrent">是</span>
            <span  v-else>否</span> 
          </template>
        </el-table-column> 
        <el-table-column 
         v-if="permission.isPermisstion('COMPANYANNOUNCEUPDATE','COMPANYANNOUNCEDEL')"
          :label="$t('message.common.handle')"
          align="center" 
          width="200"
        >
          <template #default="scope">
            <el-button   v-if="permission.isPermisstion('COMPANYANNOUNCEUPDATE')" @click="handleEdit(scope.row)">{{
              $t("message.common.update")
            }}</el-button>
            <el-popconfirm
            v-if="permission.isPermisstion('COMPANYANNOUNCEDEL')"
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
      <EditModal :layer="editAnnounceLayer"  @dataSubmit="dataSave" v-if="editAnnounceLayer.show" /> 
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, reactive } from "vue";
import { Page } from "@/components/table/type";
import { getMessages,addMessage,updateMessage,delMessage} from "@/api/system/messageCompany";
import { LayerInterface } from "@/components/layer/index.vue"; 
import Table from "@/components/table/tableServer.vue";
import EditModal from "./editLayer.vue"; 
import permission from '@/utils/system/permission'
export default defineComponent({
  components: {
    Table,
    EditModal
  },
  setup() { 
    let dateRange=ref()
    // 存储搜索用的数据
    let query = reactive({
      input: "",
    });

    //公告编辑弹窗控制器
    let editAnnounceLayer: LayerInterface = reactive({
      show: false,
      title: "",
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
      let dateStart='';
      let dateEnd='';
      if(dateRange.value?.length>0){
        dateStart=dateRange.value[0]
      }
       if(dateRange.value?.length>1){
        dateEnd=dateRange.value[1]
      }
      getMessages(page.size,page.index,page.orderField,page.orderType,query.input,dateStart,dateEnd)
        .then((res) => {
          console.log("getMessages2",res.data)
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
        ids.push(d.messageId);
      }) 
      delMessage(ids).then((res) => { 
        getTableData(tableData.value.length === 1 ? true : false);
      });
    }

    // 新增弹窗功能
    let handleAdd = () => {
      editAnnounceLayer.title = "发布企业公告";
      editAnnounceLayer.show = true; 
      delete editAnnounceLayer.data;
    }

    // 编辑弹窗功能
    let handleEdit = (row: any) => { 
        editAnnounceLayer.title = "编辑企业公告"; 
        editAnnounceLayer.show = true;
        editAnnounceLayer.data=row;
    }
   

    //新增或编辑数据提交
    let dataSave=(data:any,actionType:string)=>{   
      editAnnounceLayer.btnLoading=true;
        if(actionType=='add'){ 
          addMessage(data).then(res=>{
            editAnnounceLayer.show = false;
            getTableData(true);
          }).finally(()=> editAnnounceLayer.btnLoading=false);
        }
        else{
           updateMessage(data).then(res=>{
            editAnnounceLayer.show = false;
            getTableData(false);
          }).finally(()=> editAnnounceLayer.btnLoading=false);
        }
    }
      
    getTableData(true)
    return {
      permission,
      dateRange,
      query, 
      tableData,
      chooseData,
      loading,
      page, 
      editAnnounceLayer, 
      handleSelectionChange,
      handleSortChange,
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
