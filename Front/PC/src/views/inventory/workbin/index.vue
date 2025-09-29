<template> 
    <div class="layout-container">
      <div class="layout-container-form flex space-between">
        <div class="layout-container-form-handle">
          <el-button v-if="permission.isPermisstion('WORKBINSPECADD')&&tabsName=='workbincell'" type="primary" icon="el-icon-circle-plus-outline" @click="handleWorkbinSpecAdd">新增</el-button> 
       
        </div>
        <div class="layout-container-form-search"> 
          <el-input v-model="queryWorkbin.input" placeholder="请输入关键词进行检索" size="small"></el-input>
          <el-button type="primary" icon="el-icon-search" class="search-btn" @click="getWorkbinTableData(true)">搜索</el-button>  
        </div>
      </div>
      <div class="layout-container-table">
        <el-tabs v-model="tabsName" >
            <el-tab-pane label="料箱容器列表" name="workbin" :style="{height:tbHeight+'px'}"> 
                  <TableServer  ref="table"   v-model:page="pageWorkbin" v-loading="workbinTableLoading"  :data="workbinTableData" 
                    @getTableData="getWorkbinTableData" 
                    @expandChange="handleWorkbinExpandChange"
                    @orderChanged="handleWorkbinSortChange">
                    <el-table-column prop="workbinId" label=""  type="expand" min-width="120" sortable="custom" align="center" :show-overflow-tooltip="true"> 
                        <template #default="props">
                              <div v-if="workbinCells.filter(x=>x.workbinId==props.row.workbinId)?.length==0">
                                <span class="text-warning">未创建料箱容器单元格</span>
                              </div>
                              <div v-else>
                                <div style="margin-bottom:10px" >
                                <span  style="font-weight:600;">料箱单元格</span> 
                              </div>
                              <div style="width: 384px;height:104px ; border-radius: 4px;background-color: #1e3055;">
                                <el-row style="width: 380px;position: relative;top: 2px;left: 2px;">
                                  <el-col v-if="workbinCells.filter(x=>x.workbinId==props.row.workbinId).length==1" v-for="(opt,index) in workbinCells.filter(x=>x.workbinId==props.row.workbinId)" 
                                    class="workbin-cell" :span="24"
                                    :style="{height: '100px',lineHeight:'100px'}" :title="opt.cellNo">
                                  <span>{{ opt.cellNo }}</span>
                                  </el-col> 
                                  <el-col v-else-if="workbinCells.filter(x=>x.workbinId==props.row.workbinId).length==2" v-for="(opt,index) in workbinCells.filter(x=>x.workbinId==props.row.workbinId)" 
                                    class="workbin-cell" :span="12"
                                    :style="{height: '100px',lineHeight:'100px'}" :title="opt.cellNo">
                                  <span>{{ opt.cellNo }}</span>
                                  </el-col> 
                                  <el-col v-else v-for="(opt,index) in workbinCells.filter(x=>x.workbinId==props.row.workbinId)" 
                                    class="workbin-cell" :span="24/(workbinCells.filter(x=>x.workbinId==props.row.workbinId).length/2)"
                                    :style="{height: '50px',lineHeight:'50px'}" :title="opt.cellNo">
                                  <span>{{ opt.cellNo }}</span>
                                  </el-col> 
                                </el-row>  
                              </div>
                              </div> 
                        </template> 
                      </el-table-column>  
                    <el-table-column prop="warehouseName" label="仓库"   align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
                    <el-table-column prop="shelfNo" label="货架"   align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
                    <el-table-column prop="binNo" label="货位编号"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>  
                    <el-table-column prop="workbinNo" label="料箱编号"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/> 
                    <el-table-column prop="specName" label="料箱规格"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/> 
                    <el-table-column  v-if="permission.isPermisstion('WORKBINUPDATE')"  :label="$t('message.common.handle')" align="center" width="100">
                      <template #default="scope"> 
                        <el-button  v-if="permission.isPermisstion('WORKBINUPDATE')" @click="handleWorkbinEdit(scope.row)">{{$t("message.common.update")}}</el-button> 
                      </template>
                    </el-table-column>
                  </TableServer>
                  <EditWorkbinModal :layer="editWorkbinLayer"  @dataSubmit="dataWorkbinSave" v-if="editWorkbinLayer.show" />  
            </el-tab-pane>
            <el-tab-pane label="料箱规格列表" name="workbincell" :style="{height:tbHeight+'px'}"> 
                <TableClient  v-loading="workbinSpecLoading"  :data="workbinSpecTableData">
                  <el-table-column prop="specName" label="规格名称" align="center" :sortable="true" min-width="100" :show-overflow-tooltip="true"/> 
                  <el-table-column prop="size" label="尺寸" align="center" :sortable="true" min-width="100" :show-overflow-tooltip="true"/>
                  <el-table-column prop="loadWeight" label="承重" align="center" :sortable="true" :show-overflow-tooltip="true"/> 
                  <el-table-column prop="cellCount" label="分隔数" align="center" :sortable="true" min-width="100" :show-overflow-tooltip="true"/>
                  <el-table-column prop="rank" label="排序" align="center" :sortable="true" min-width="100" :show-overflow-tooltip="true"/> 
                  <el-table-column  v-if="permission.isPermisstion('WORKBINSPECUPDATE','WORKBINSPECDEL')"  :label="$t('message.common.handle')"  align="center"  width="200" >
                    <template #default="scope">
                      <el-button  v-if="permission.isPermisstion('WORKBINSPECUPDATE')" @click="handleWorkbinSpecEdit(scope.row)">{{ $t("message.common.update") }}</el-button>
                      <el-popconfirm  v-if="permission.isPermisstion('WORKBINSPECDEL')"  :title="$t('message.common.delTip')"   @confirm="handleWorkbinSpecDel(scope.row)" >
                        <template #reference>
                          <el-button type="danger">{{ $t("message.common.del") }}</el-button>
                        </template>
                      </el-popconfirm>
                    </template>
                  </el-table-column>
                </TableClient> 
                <EditWorkbinSpecModal :layer="editWorkbinSpecLayer"  @dataSubmit="dataWorkbinSpecSave" v-if="editWorkbinSpecLayer.show"/>
            </el-tab-pane> 
        </el-tabs> 
      </div> 
    </div>
  </template>
  
  <script lang="ts" setup>
    defineOptions({
    name: "workbin"
  })
  import { ref, reactive,onMounted,onBeforeMount} from "vue";  
  import { getWorkbins,getWorkbinCells,updateWorkbin,getWorkbinSpec,addWorkbinSpeci,updateWorkbinSpeci,delWorkbinSpeci} from "@/api/inv/workbin";
  import { LayerInterface } from "@/components/layer/index.vue"; 
  import TableServer from "@/components/table/tableServer.vue";
  import TableClient from "@/components/table/tableClient.vue";
  import EditWorkbinModal from "./editWorkbinLayer.vue";  
  import EditWorkbinSpecModal from './editWorkbinSpecLayer.vue';
  import { Page } from "@/components/table/type";
  import permission from '@/utils/system/permission' 
  import commonHelper from "@/utils/system/common-helper";
  import msg from '@/utils/system/message' 
  
  const queryWorkbin = reactive({
        input: "",
      }); 
  const tabsName=ref('workbin')

  const tbHeight=ref(0)

  onBeforeMount(()=>{
    tbHeight.value=window.innerHeight-260;
  })
   
  onMounted(()=>{
    getWorkbinTableData(true);
    getWorkbinSpecTableData();
  })

  //料箱
  const pageWorkbin: Page = reactive({
        index: 1,
        size: 20,
        total: 0,
        orderField:'',
        orderType:''
      }); 
   const workbinTableLoading = ref(false);
   const workbinTableData = ref([]);  
   const workbinCells=ref(new Array<any>()); 
   const editWorkbinLayer: LayerInterface = reactive({
        show: false,
        title: "",
        showButton: true,
        btnLoading:false,
        width:"30%",
        data:null,
        type:'', 
        otherButton:{ }
    });  
       
    const handleWorkbinSortChange=(orderRow:any)=>{   
      getWorkbinTableData(true);
    }

 
    const handleWorkbinExpandChange=(row:any)=>{  
      getWorkbinCells(row.workbinId).then(res=>{
        workbinCells.value=workbinCells.value.filter((b:any)=>b.workbinId!=row.workbinId);  
            res.data?.forEach((b:any) => {
              workbinCells.value.push(b)
              });   
        }); 
    }
 
    const getWorkbinTableData = (init: Boolean) => { 
      if (init) {
        pageWorkbin.index = 1
      }   
        workbinTableLoading.value = true
        getWorkbins( pageWorkbin.size,pageWorkbin.index,pageWorkbin.orderField,pageWorkbin.orderType,queryWorkbin.input)
        .then((res) => { 
          let data = res.data.rows
          data.forEach((d: any) => {
            d.workbinTableLoading = false
          })
          workbinTableData.value = data
          pageWorkbin.total = Number(res.data.total);
        })
        .catch((error) => {
          workbinTableData.value = [];
          pageWorkbin.index = 1;
          pageWorkbin.total = 0;
        })
        .finally(() => {
          workbinTableLoading.value = false;
        });
    } 
 
  const handleWorkbinEdit = (row: any) => {  
      editWorkbinLayer.title = "编辑料箱信息"; 
      editWorkbinLayer.show = true;
      editWorkbinLayer.type='update' 
      editWorkbinLayer.data = row; 
  }
       
  const dataWorkbinSave=(data:any,actionType:string)=>{   
    editWorkbinLayer.btnLoading=true;
    data.modifyUser=permission.getOperator().userName;
          updateWorkbin(data).then(res=>{
          editWorkbinLayer.show = false;
          getWorkbinTableData(false);
      }).finally(()=> editWorkbinLayer.btnLoading=false);
  }
 
  //料箱规格
  const workbinSpecLoading=ref(false);
  const workbinSpecTableData=ref([]);
  const editWorkbinSpecLayer: LayerInterface = reactive({
        show: false,
        title: "",
        showButton: true,
        btnLoading:false,
        width:"30%",
        data:null,
        type:'', 
        otherButton:{ }
    });

  const getWorkbinSpecTableData=()=>{
    workbinSpecLoading.value=true;
    getWorkbinSpec().then(res=>{
      workbinSpecTableData.value=res.data;
    }).finally(()=>workbinSpecLoading.value=false);
  }
 
  const handleWorkbinSpecAdd = () => {
    editWorkbinSpecLayer.title = "新增料箱规格";
    editWorkbinSpecLayer.show = true; 
    editWorkbinSpecLayer.type='add'  
    delete editWorkbinSpecLayer.data;
  }

  const handleWorkbinSpecEdit=(row:any)=>{
    editWorkbinSpecLayer.title = "修改料箱规格";
    editWorkbinSpecLayer.show = true; 
    editWorkbinSpecLayer.type='update';
    editWorkbinSpecLayer.data=row;  
  }
 
  const handleWorkbinSpecDel = (row: any) => {    
       delWorkbinSpeci(row.specId).then((res) => { 
        getWorkbinSpecTableData();
      });
  }
 
  const dataWorkbinSpecSave=(data:any,actionType:string)=>{
    editWorkbinSpecLayer.btnLoading=true;
    if(actionType=='add'){ 
      addWorkbinSpeci(data).then(()=>{
        editWorkbinSpecLayer.show=false;
        getWorkbinSpecTableData();
      }).finally(()=>editWorkbinSpecLayer.btnLoading=false)
    }
    else{
      updateWorkbinSpeci(data).then(()=>{
        editWorkbinSpecLayer.show=false;
        getWorkbinSpecTableData();
      }).finally(()=>editWorkbinSpecLayer.btnLoading=false)
    }
  }
 
  </script>
  
  <style lang="scss" scoped> 
  .workbin-cell{
    background-color: #717c91;
    border-right: 1px solid #fff;
    border-bottom: 1px solid #fff;
    color: #fff; 
    font-size: xx-small;
    text-align: center;
  }
  </style>
  