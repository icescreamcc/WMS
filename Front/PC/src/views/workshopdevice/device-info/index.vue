<template> 
    <div class="layout-container">
      <div class="layout-container-form flex space-between">
        <div class="layout-container-form-handle">
          <el-button v-if="permission.isPermisstion('WORKSHOPDEVICEINFOADD')&&tabsName=='info'" type="primary" icon="el-icon-circle-plus-outline" @click="handleDeviceinfoAdd">新增</el-button> 
          <el-button v-if="permission.isPermisstion('WORKSHOPDEVICEBINADD')&&tabsName=='type'" type="primary" icon="el-icon-circle-plus-outline" @click="handleDeviceBinAdd">新增</el-button> 
        </div>
        <div class="layout-container-form-search"> 
          <el-select v-model="queryKeywords.warehouseId" :disabled="disableClassifyGroupSelect" v-if="tabsName=='info'"  size="small"  class="m-2" style="width:100%;margin-right:10px;" @change="getdeviceinfoTableData(true)" placeholder="选择仓库">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">所属仓库</div></template>        
            <el-option v-for="item in warehouseOptions" :key="item.warehouseId" :label="item.warehouseName" :value="item.warehouseId"> </el-option>
          </el-select>
          <el-select v-model="queryKeywords.deviceType"  v-if="tabsName=='type'"  size="small"  class="m-2" style="width:100%;margin-right:10px;" @change="getdeviceBinTableData(true)" placeholder="选择设备类型">
            <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">设备类型</div></template>        
            <el-option v-for="item in deviceTypeOptions" :key="item.key" :label="item.value" :value="item.key"> </el-option>
          </el-select>
          <el-input v-model="queryKeywords.input"  placeholder="请输入关键词进行检索" size="small" clearable  @clear="queryTableData"></el-input>
          <el-button type="primary" icon="el-icon-search" v-if="tabsName=='info'" class="search-btn" @click="getdeviceinfoTableData(true)">搜索</el-button>  
          <el-button type="primary" icon="el-icon-search" v-if="tabsName=='type'" class="search-btn" @click="getdeviceBinTableData(true)">搜索</el-button>  
        </div>
      </div>
      <div class="layout-container-table">
        <el-tabs v-model="tabsName" @tab-click="onTabChange">
            <el-tab-pane label="设备信息列表" name="info" :style="{height:tbHeight+'px'}"> 
                  <TableServer  ref="table"   v-model:page="deviceinfoPageData" v-loading="deviceinfoTableLoading"  :data="deviceinfoTableData" 
                    @getTableData="getdeviceinfoTableData"  
                    @orderChanged="handleDeviceinfoSortChange"> 
                    <el-table-column prop="deviceNo" label="设备编码"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>
                    <el-table-column prop="deviceName" label="设备名称"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                    <el-table-column prop="deviceType" label="设备类型"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
                      <template #default="scope">
                        <span>{{ scope.row.deviceTypeDesc }}</span>
                      </template>
                    </el-table-column>
                    <el-table-column prop="connectAddress" label="连接地址"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                    <el-table-column prop="warehouseName" label="所属仓库"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                    <el-table-column prop="isActive" label="状态"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
                      <template #default="scope">
                        <span v-if="scope.row.isActive" class="text-success">启用</span>
                        <span v-else class="text-danger">未启用</span>
                      </template>
                    </el-table-column> 
                    <el-table-column  v-if="permission.isPermisstion('WORKSHOPDEVICEINFOUPDATE','WORKSHOPDEVICEINFODEL')"  :label="$t('message.common.handle')" align="center"  min-width="120">
                      <template #default="scope"> 
                        <el-button  v-if="permission.isPermisstion('WORKSHOPDEVICEINFOUPDATE')" @click="handleDeviceinfoEdit(scope.row)">{{$t("message.common.update")}}</el-button> 
                        <el-popconfirm v-if="permission.isPermisstion('WORKSHOPDEVICEINFODEL')"  :title="$t('message.common.delTip')" @confirm="onDeviceinfoDel([scope.row])">
                          <template #reference>
                            <el-button type="danger">{{ $t("message.common.del") }}</el-button>
                          </template>
                        </el-popconfirm> 
                      </template>
                    </el-table-column>
                  </TableServer>
                  <EditDeviceInfoModal :layer="editDeviceinfoLayer"  @dataSubmit="onDeviceinfoEditSubmit" v-if="editDeviceinfoLayer.show" />  
            </el-tab-pane>
            <el-tab-pane label="设备仓位列表" name="type" :style="{height:tbHeight+'px'}"> 
              <TableServer  ref="table"   v-model:page="deviceBinPageData" v-loading="deviceBinLoading"  :data="deviceBinTableData" 
                    @getTableData="getdeviceBinTableData"  
                    @orderChanged="handleDeviceBinSortChange">  
                    <el-table-column prop="binNo" label="仓位编码"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                    <el-table-column prop="agvBinCode_Delivery" label="AGV送货仓位码"  align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true"/> 
                    <el-table-column prop="agvBinCode_Receive" label="AGV取货仓位码"  align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true"/> 
                    <el-table-column prop="sort" label="仓位排序"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                    <el-table-column prop="binStatus" label="仓位状态"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
                      <template #default="scope">
                        <span>{{ scope.row.binStatusDesc }}</span>
                      </template>
                    </el-table-column>
                    <el-table-column prop="column" label="列"   align="center" sortable="custom" min-width="80" :show-overflow-tooltip="true"/> 
                    <el-table-column prop="row" label="行"   align="center" sortable="custom" min-width="80" :show-overflow-tooltip="true"/> 
                    <el-table-column prop="tier" label="层"   align="center" sortable="custom" min-width="80" :show-overflow-tooltip="true"/> 
                    <el-table-column prop="deviceNo" label="所属设备"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                    <el-table-column prop="deviceType" label="设备类型"   align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
                      <template #default="scope">
                        <span>{{ scope.row.deviceTypeDesc }}</span>
                      </template>
                    </el-table-column>
                    <el-table-column  v-if="permission.isPermisstion('WORKSHOPDEVICEBINUPDATE','WORKSHOPDEVICEBINDEL')" min-width="150" :label="$t('message.common.handle')" align="center">
                      <template #default="scope"> 
                        <el-button  v-if="permission.isPermisstion('WORKSHOPDEVICEBINUPDATE')" @click="handleDeviceBinEdit(scope.row)">{{$t("message.common.update")}}</el-button> 
                        <el-popconfirm v-if="permission.isPermisstion('WORKSHOPDEVICEBINDEL')"  :title="$t('message.common.delTip')" @confirm="onDeviceBinDel(scope.row)">
                          <template #reference>
                            <el-button type="danger">{{ $t("message.common.del") }}</el-button>
                          </template>
                        </el-popconfirm> 
                      </template>
                    </el-table-column>
                  </TableServer>
                <EditDeviceBinModal :layer="editDeviceBinLayer"  @dataSubmit="onDeviceBinSubmit" v-if="editDeviceBinLayer.show"/>
            </el-tab-pane> 
        </el-tabs> 
      </div> 
    </div>
  </template>
  
  <script lang="ts" setup>
  defineOptions({
    name: "device-info"
  })
  import { ref, reactive,onMounted,onBeforeMount} from "vue";  
  import {getDeviceInfo,getDeviceInfoDetail,getDeviceInfoOptions,addDeviceInfo,updateDeviceInfo,delDeviceInfo,getDeviceBin,getArgs,getDeviceBinDetail,addDeviceBin,updateDeviceBin,delDeviceBin} from "@/api/workshop-device/device-info";
  import { LayerInterface } from "@/components/layer/index.vue"; 
  import TableServer from "@/components/table/tableServer.vue"; 
  import EditDeviceInfoModal from "./deviceEditLayer.vue";  
  import EditDeviceBinModal from './deviceBinEditLayer.vue';
  import { Page } from "@/components/table/type";
  import permission from '@/utils/system/permission' 
  import commonHelper from "@/utils/system/common-helper";
  import msg from '@/utils/system/message';
  import { deftClassifyGroup ,disableClassifyGroupSelect} from '@/config'; 
  
  const queryKeywords = reactive({
        input: "",
        warehouseId:'',
        deviceType:''
   }); 
  const tabsName=ref('info') 
  const tbHeight=ref(0)
  const warehouseOptions=ref(new Array<any>());
  const deviceTypeOptions=ref(new Array<any>());
  onBeforeMount(()=>{
    tbHeight.value=window.innerHeight-260;
  })
   
  onMounted(()=>{
    getArgsOption();
    queryTableData();
  })

  const getArgsOption=()=>{
     getArgs().then(res=>{ 
      if(deftClassifyGroup){
        warehouseOptions.value=res.data.warehouseOptions.filter((f:any)=>f.warehouseType==deftClassifyGroup); 
        queryKeywords.warehouseId=warehouseOptions.value[0].warehouseId;
      }
       else{
        warehouseOptions.value=res.data.warehouseOptions; 
        warehouseOptions.value.unshift({warehouseId:'',warehouseName:'所有'});
       }
      deviceTypeOptions.value=res.data.deviceTypeOptions;
      deviceTypeOptions.value.unshift({key:'',value:'所有'});
     });
  }

  const queryTableData=()=>{ 
    if(tabsName.value=='info'){
      getdeviceinfoTableData(true);
    }
    else{
      getdeviceBinTableData(true);
    } 
  }

  const onTabChange=(tab:any)=>{ 
     if(tab.paneName=='info'){
      getdeviceinfoTableData(true);
     }
     else{
      getdeviceBinTableData(true);
     }
  }

  //设备信息
  const deviceinfoPageData: Page = reactive({
        index: 1,
        size: 20,
        total: 0,
        orderField:'',
        orderType:''
      }); 
   const deviceinfoTableLoading = ref(false);
   const deviceinfoTableData = ref([]);   
   const editDeviceinfoLayer: LayerInterface = reactive({
        show: false,
        title: "",
        showButton: true,
        btnLoading:false,
        width:"30%",
        data:null, 
        type:'', 
        otherButton:{ }
    });  
       
    const handleDeviceinfoSortChange=(orderRow:any)=>{   
      getdeviceinfoTableData(true);
    }
 
 
    const getdeviceinfoTableData = (init: Boolean) => { 
      if (init) {
        deviceinfoPageData.index = 1
      }   
      deviceinfoTableLoading.value = true; 
      getDeviceInfo( deviceinfoPageData.size,deviceinfoPageData.index,deviceinfoPageData.orderField,deviceinfoPageData.orderType,queryKeywords.warehouseId,queryKeywords.input)
        .then((res) => { 
          let data = res.data.rows
          data.forEach((d: any) => {
            d.loading = false;
          })
          deviceinfoTableData.value = data
          deviceinfoPageData.total = Number(res.data.total);
        })
        .catch((error) => {
          deviceinfoTableData.value = [];
          deviceinfoPageData.index = 1;
          deviceinfoPageData.total = 0;
        })
        .finally(() => {
          deviceinfoTableLoading.value = false;
        });
    } 
  
  const handleDeviceinfoAdd = () => {
    editDeviceinfoLayer.title = "新增设备信息";
    editDeviceinfoLayer.show = true; 
    editDeviceinfoLayer.type='add';   
    delete editDeviceinfoLayer.data;
  }
 
  const handleDeviceinfoEdit = (row: any) => {  
    getDeviceInfoDetail(row.deviceId).then(res=>{
      editDeviceinfoLayer.title = "编辑设备信息"; 
      editDeviceinfoLayer.show = true;
      editDeviceinfoLayer.type='update'; 
      editDeviceinfoLayer.data = res.data; 
    }) 
  }

  const onDeviceinfoDel=(rows:Array<any>)=>{
    let ids= rows.map(m=>{
      return m.deviceId
    })
    delDeviceInfo(ids).then(res=>{
      getdeviceinfoTableData(false);
    })
  }
       
  const onDeviceinfoEditSubmit=(data:any,actionType:string)=>{   
    editDeviceinfoLayer.btnLoading=true; 
    if(actionType=='add'){
      addDeviceInfo(data).then(res=>{
            editDeviceinfoLayer.show = false;
          getdeviceinfoTableData(false);
      }).finally(()=> editDeviceinfoLayer.btnLoading=false);
    }
    else{
      updateDeviceInfo(data).then(res=>{
            editDeviceinfoLayer.show = false;
          getdeviceinfoTableData(false);
      }).finally(()=> editDeviceinfoLayer.btnLoading=false);
    } 
  }
 
  //设备类型
  const deviceBinPageData: Page = reactive({
        index: 1,
        size: 20,
        total: 0,
        orderField:'',
        orderType:''
      }); 
  const deviceBinLoading=ref(false);
  const deviceBinTableData=ref([]);
  const editDeviceBinLayer: LayerInterface = reactive({
        show: false,
        title: "",
        showButton: true,
        btnLoading:false,
        width:"35%",
        data:null, 
        type:'', 
        otherButton:{ }
    });

  const handleDeviceBinSortChange=(orderRow:any)=>{   
    getdeviceBinTableData(true);
  }

  const getdeviceBinTableData=(init: Boolean)=>{
    if (init) {
      deviceBinPageData.index = 1
      }   
      deviceBinLoading.value = true
      getDeviceBin( deviceBinPageData.size,deviceBinPageData.index,deviceBinPageData.orderField,deviceBinPageData.orderType,queryKeywords.deviceType,queryKeywords.input)
        .then((res) => { 
          let data = res.data.rows
          data.forEach((d: any) => {
            d.loading = false
          })
          deviceBinTableData.value = data
          deviceBinPageData.total = Number(res.data.total);
        })
        .catch((error) => {
          deviceBinTableData.value = [];
          deviceBinPageData.index = 1;
          deviceBinPageData.total = 0;
        })
        .finally(() => {
          deviceBinLoading.value = false;
        });
  }
 
  const handleDeviceBinAdd = () => {
    editDeviceBinLayer.title = "新增设备仓位";
    editDeviceBinLayer.show = true; 
    editDeviceBinLayer.type='add';   
    delete editDeviceBinLayer.data;
  }

  const handleDeviceBinEdit=(row:any)=>{
    getDeviceBinDetail(row.binId).then(res=>{
      editDeviceBinLayer.title = "修改设备仓位";
      editDeviceBinLayer.show = true; 
      editDeviceBinLayer.type='update'; 
      editDeviceBinLayer.data=res.data; 
    }) 
  }
 
  const onDeviceBinDel = (row: any) => {    
       delDeviceBin([row.binId]).then((res) => {  
        getdeviceBinTableData(false);
      });
  }
 
  const onDeviceBinSubmit=(data:any,actionType:string)=>{
    editDeviceBinLayer.btnLoading=true;
    if(actionType=='add'){ 
      addDeviceBin(data).then(()=>{
        editDeviceBinLayer.show=false;
        getArgsOption();
        getdeviceBinTableData(true);
      }).finally(()=>editDeviceBinLayer.btnLoading=false)
    }
    else{
      updateDeviceBin(data).then(()=>{
        editDeviceBinLayer.show=false;
        getArgsOption();
        getdeviceBinTableData(false);
      }).finally(()=>editDeviceBinLayer.btnLoading=false)
    }
  }
 
  </script>
  
  <style lang="scss" scoped>  
  </style>
  