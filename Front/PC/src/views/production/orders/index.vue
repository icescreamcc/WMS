<template>
    <div class="layout-container">
      <div class="layout-container-form flex space-between">
        <div class="layout-container-form-handle">
          <el-button v-if="permission.isPermisstion('PRODORDERADD')" type="primary" icon="el-icon-circle-plus-outline" @click="handleAdd">新增</el-button> 
           <el-popconfirm title='确定删除选中的数据吗' v-if="permission.isPermisstion('PRODORDERDEL')"  @confirm="handleDel(chooseData)">
            <template #reference>
              <el-button type="danger"  icon="el-icon-delete" :disabled="chooseData.length === 0">批量删除</el-button>
            </template>
          </el-popconfirm> 
        </div>
        <div class="layout-container-form-search"> 
          <el-input v-model="query.input" placeholder="请输入关键词进行检索" size="small"></el-input>
          <el-button type="primary" icon="el-icon-search" class="search-btn" @click="getTableData(true)">搜索</el-button>  
        </div>
      </div>
      <div class="layout-container-table">
        <Table  ref="table"   v-model:page="page" v-loading="loading" :showSelection="true" :data="tableData" 
          @getTableData="getTableData"
          @selection-change="handleSelectionChange" 
          @expandChange="handleExpandChange"
          @orderChanged="handleSortChange">
           <el-table-column prop="OrderId" label=""  type="expand" min-width="120" sortable="custom" align="center" :show-overflow-tooltip="true"> 
               <template #default="props">
                    <div v-if="orderDetails.filter(x=>x.deliverNo==props.row.deliverNo)?.length==0">
                      <span class="text-warning">未分分配小车唯一码</span>
                    </div>
                    <div v-else>
                       <div style="margin-bottom:10px" >
                       <span  style="font-weight:600;">小车唯一码列表</span> 
                     </div>
                     <el-row>
                       <el-col :span="6" v-for="opt in orderDetails.filter(x=>x.deliverNo==props.row.deliverNo)" style="margin-bottom:10px">
                         <span>● {{ `${opt.carSoleCode} 数量：${opt.planTotalByCar+props.row.unitName} 车次：${opt.carRank}`}}</span> 
                       </el-col>
                     </el-row> 
                    </div> 
              </template> 
            </el-table-column>  
          <el-table-column prop="deliverNo" label="生产订单号"   align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
          <el-table-column prop="consignNum" label="发货型号"   align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
          <el-table-column prop="prodctionTypeNo" label="产品型号"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>  
          <el-table-column prop="total" label="生产数量"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="scope"> 
              <span>{{ scope.row.total+scope.row.unitName }}</span> 
            </template>
          </el-table-column>  
          <el-table-column prop="total" label="已下架数量"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="scope"> 
              <span>{{ scope.row.totalPutout+scope.row.unitName }}</span> 
            </template>
          </el-table-column>  
          <el-table-column prop="totalByCar" label="装车数量"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="scope"> 
              <span>{{ scope.row.totalByCar+scope.row.unitName }}</span> 
            </template>
          </el-table-column>   
          <el-table-column prop="countByCar" label="需求车次"  align="center"  sortable="custom" min-width="120" :show-overflow-tooltip="true"/>  
          <el-table-column prop="matchingCount" label="小车配对数"  align="center"  sortable="custom" min-width="120" :show-overflow-tooltip="true"/>  
          <el-table-column prop="line" label="产线"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>   
          <el-table-column prop="status" label="状态"  align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true">
            <template #default="scope"> 
              <span v-if="scope.row.status=='InStorage'||scope.row.status=='PackageFinished'" class="text-success">{{ scope.row.statusDesc}}</span> 
              <span v-else-if="scope.row.status=='Create'">{{ scope.row.statusDesc}}</span> 
              <span v-else-if="scope.row.status=='Closed'" class="text-primary">{{ scope.row.statusDesc}}</span> 
              <span v-else class="text-warning">{{ scope.row.statusDesc}}</span> 
            </template>
          </el-table-column> 
          <!-- <el-table-column prop="printDate" label="打印日期"  align="center" sortable="custom" min-width="110" :show-overflow-tooltip="true">
            <template #default="scope"> 
              <span>{{commonHelper.formatToDateTime(scope.row.printDate) }}</span> 
            </template>
          </el-table-column> -->
          <el-table-column  label="创建小车码"  align="center" min-width="130" :show-overflow-tooltip="true">
            <template #default="scope"> 
              <el-button v-if="scope.row.status=='Create'" @click="onCreateCarCode(scope.row.orderId)" type="primary" icon="el-icon-menu" circle></el-button>
              <span v-else>已创建</span> 
            </template>
          </el-table-column>   
          <el-table-column prop="printDate" label="打印"  align="center" sortable="custom" min-width="80" :show-overflow-tooltip="true">
            <template #default="scope"> 
              <el-button :disabled="scope.row.status=='Create'"  v-if="permission.isPermisstion('PRODORDERPRINT')" type="primary" circle icon="el-icon-printer" @click="handlePrint(scope.row)"></el-button>
            </template>
          </el-table-column>  
          <el-table-column  v-if="permission.isPermisstion('PRODORDERUPDATE','PRODORDERDEL','PRODORDERCLOSED')"  :label="$t('message.common.handle')" align="left" width="250">
            <template #default="scope"> 
              <el-button  v-if="permission.isPermisstion('PRODORDERUPDATE')" @click="handleEdit(scope.row)">{{$t("message.common.update")}}</el-button>
              <el-popconfirm v-if="permission.isPermisstion('PRODORDERDEL')" :title="$t('message.common.delTip')" @confirm="handleDel([scope.row])">
                <template #reference>
                  <el-button type="danger">{{ $t("message.common.del") }}</el-button>
                </template>
              </el-popconfirm>
              <el-popconfirm v-if="permission.isPermisstion('PRODORDERCLOSED')&&scope.row.status!='Create'&&scope.row.status!='Allot'&&scope.row.status!='PackageFinished'&&scope.row.status!='Closed'" title="是否确认已完成该订单？" @confirm="handleClosed(scope.row)">
                <template #reference>
                  <el-button type="success" :loading="btnLoading">关闭订单</el-button>
                </template>
              </el-popconfirm>
            </template>
          </el-table-column>
        </Table>
        <EditModal :layer="editLayer"  @dataSubmit="dataSave" v-if="editLayer.show" /> 
      </div>
    </div>
  </template>
  
  <script lang="ts" setup>
  import { ref, reactive } from "vue";  
  import { getOrders,getOrderDetail,addOrder,updateOrder,delOrder,createCarCode,updateOrderClosed} from "@/api/production/productionOrder";
  import { LayerInterface } from "@/components/layer/index.vue"; 
  import Table from "@/components/table/tableServer.vue";
  import EditModal from "./editLayer.vue";  
  import { Page } from "@/components/table/type";
  import permission from '@/utils/system/permission' 
  import commonHelper from "@/utils/system/common-helper";
  import msg from '@/utils/system/message'
  import { useRoute, useRouter} from "vue-router";
 
  const router = useRouter();
  const query = reactive({
        input: "",
      }); 
  const page: Page = reactive({
        index: 1,
        size: 20,
        total: 0,
        orderField:'',
        orderType:''
      });
   const btnLoading=ref(false);
   const loading = ref(true);
   const tableData = ref([]); 
   const chooseData = ref([]); 
   const orderDetails=ref(new Array<any>()); 
   const editLayer: LayerInterface = reactive({
        show: false,
        title: "",
        showButton: true,
        btnLoading:false,
        width:"30%",
        data:null,
        type:'', 
        otherButton:{ }
    });  
     
  
     //排序事件
     const handleSortChange=(orderRow:any)=>{   
        getTableData(true);
      }
  
     //多选
     const handleSelectionChange = (val: []) => {
        chooseData.value = val;
      };

      //创建小车唯一码
      const onCreateCarCode=(orderId:number)=>{ 
        createCarCode(orderId).then(res=>{  
          msg.successAuto("已生成并分配小车唯一码");
          getTableData(false);
        });
      }
  
      //展开与收缩 
      const handleExpandChange=(row:any)=>{  
         getOrderDetail(row.deliverNo).then(res=>{
          orderDetails.value=orderDetails.value.filter((b:any)=>b.deliverNo!=row.deliverNo);  
              res.data.details?.forEach((b:any) => {
                orderDetails.value.push(b)
                });   
         }); 
      }
  
      // 获取表格数据 
      const getTableData = (init: Boolean) => {
        loading.value = true
        if (init) {
          page.index = 1
        }   
         loading.value = false
        getOrders( page.size,page.index,page.orderField,page.orderType,query.input)
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
     
    
    // 删除功能
    const handleDel = (data: any) => {   
       let id=data.map((x:any)=>{return x.deliverNo} ) 
         delOrder(id).then((res) => { 
          getTableData(tableData.value.length === 1 ? true : false);
        });
    }

    //关闭订单
    const handleClosed=(data:any)=>{
      btnLoading.value=true;
      updateOrderClosed(data.deliverNo,permission.getOperator().userName).then(()=>{
        getTableData(tableData.value.length === 1 ? true : false);
      }).finally(()=>btnLoading.value=false);
    }
  
      // 新增弹窗功能
    const handleAdd = () => {
        editLayer.title = "添加生产订单";
        editLayer.show = true; 
        editLayer.type='add'  
        delete editLayer.data;
      }
  
    // 编辑弹窗功能
    const handleEdit = (row: any) => {  
      getOrderDetail(row.deliverNo).then(res=>{
        editLayer.title = "编辑生产订单"; 
        editLayer.show = true;
        editLayer.type='update' 
        editLayer.data = res.data; 
      }); 
    }
     
    const handlePrint=(row:any)=>{ 
      router.push({name:"printOrder",query:{deliverNo:row.deliverNo}}); 
    }
   
    //新增或编辑数据提交
    const dataSave=(data:any,actionType:string)=>{   
      editLayer.btnLoading=true;
        if(actionType=='add'){ 
          data.createUser=permission.getOperator().userName;
        addOrder(data).then(res=>{
            editLayer.show = false;
            getTableData(true);
        }).finally(()=> editLayer.btnLoading=false);
        }
        else{
          data.modifyUser=permission.getOperator().userName;
            updateOrder(data).then(res=>{
            editLayer.show = false;
            getTableData(false);
        }).finally(()=> editLayer.btnLoading=false);
        }
    }
  </script>
  
  <style lang="scss" scoped>
  .statusName {
    margin-right: 10px;
  }
  </style>
  