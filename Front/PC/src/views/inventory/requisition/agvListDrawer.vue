<template>
    <div>
       <el-drawer v-model="props.options.show" ref="orderDrawer" :close-on-click-modal="false" :with-header="false" :show-close="true" @close="onClosed"  direction="btt" size="500px">
       <div class="content">
         <div class="content-header">
          <h5>
            <div class="header-title">
              <img src="/public/icon-img/q065chakandingdan.png">
               <span>AGV任务列表</span>
            </div>
            <div class="header-closed">
              <img @click="onCloseDrawer" src="/public/icon-img/guanbi_1.png">
            </div>
          </h5> 
         </div> 
          <!-- <div class="content-body" :style="{height:drawerHeight+'px'}">
            
          </div>  -->
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
              <el-table-column prop="goodsNameZH" label="物料名称(中文)" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true"/> 
              <el-table-column prop="goodsNameEN" label="物料名称(英文)" align="center" sortable="custom" min-width="130" :show-overflow-tooltip="true"/> 
              <el-table-column prop="goodsModel" label="物料型号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
              <el-table-column prop="goodsNo" label="MNA编码" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
              <el-table-column prop="createUserName" label="创建人" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
              <el-table-column prop="consignee" label="需求者" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
            </Table>
            </div>
       </div>
     </el-drawer>
     <AutoOperaionLayer :layer="autoOperationLayer" v-if="autoOperationLayer.show" @operationFinished="onOperationFinished"/> 
    </div>
</template>

<script lang="ts" setup>
import {ref ,reactive,defineEmits,defineProps,onMounted,onBeforeMount,onBeforeUnmount} from 'vue';   
import {swipingCardAuth, getOrderList,delReqisitionOrder,confirmReceived } from "@/api/inv/requisition";
import { ElForm } from 'element-plus'; 
import commonHelper from "@/utils/system/common-helper";  
import { ElLoading } from 'element-plus';
import { InfoFilled } from '@element-plus/icons-vue';
import AutoOperaionLayer from "@/components/auto-operation/autoOperaion.vue";
import { isAllowAutoTransport,getHisTask } from "@/api/auto-operation/auto-transport";
import msg from "@/utils/system/message";
import permission from '@/utils/system/permission';
import { Page } from "@/components/table/type";
import { getOrders } from "@/api/purchase/purchaseOrder";
const props=defineProps({
 options: {
     type: Object,
     default: () => {
       return {
         show: false,
         title: '', 
         type:'',  
         data:null 
       }
     }
   }
});  
const page: Page = reactive({
        index: 1,
        size: 20,
        total: 0,
        orderField:'',
        orderType:''
});

const loading = ref(true);
const tableData = ref([]); 
const chooseData = ref([]); 

const orderDrawer=ref(); 
const drawerHeight=ref(400);
const emit = defineEmits(['submitRemove','submited']);
const ruleForm=ref({
        goodsClassifyGroup:props.options.type,
        queryDate:commonHelper.formatToDate(new Date()),  
        userCard:'',
        userId:'',
        userName:'',
        authInfo:'' 
}); 
const autoOperationLayer:any = ref({
      show: false,
      title: "",
      showButton: true,
      btnLoading:false, 
      width:"78%",
      data:null,  
      otherButton:{}
}); 

const orderData=ref(new Array<any>());   
const authInput=ref<null | HTMLElement>(null);
const authTipsVisible=ref(false);
const datePickerRef=ref<null | HTMLElement>(null);


const getTableData = (init: Boolean) => {
        loading.value = true
        if (init) {
          page.index = 1
        }   
        let dateStart='';
        let dateEnd='';
         loading.value = false
        getOrders(permission.getOperator().userId,page.size,page.index,page.orderField,page.orderType,"","","",0,"","",0)
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
   
const handleSelectionChange = (val: []) => {
        chooseData.value = val;
      };

getTableData(true)
onMounted(()=>{  
  setTimeout(() => { 
    authInput.value?.focus(); 
    onShowSubmitTips();
  }, 500);  
});



const onShowSubmitTips=()=>{
  authTipsVisible.value=true;
    setTimeout(() => {
      authTipsVisible.value=false;
    }, 4000);
}

const onSelectedDate=()=>{
  if(ruleForm.value.queryDate&&ruleForm.value.userId){
    let date=commonHelper.formatToDate(ruleForm.value.queryDate); 
    getOrderList(ruleForm.value.userId,ruleForm.value.goodsClassifyGroup,date).then(res=>{
      orderData.value=res.data;
    })
  }
  else{
    if(!ruleForm.value.queryDate){
      setTimeout(() => {
        datePickerRef.value?.focus(); 
      }, 300);
    }
    else{
      setTimeout(() => {
        authInput.value?.focus(); 
      }, 300);
    }
  } 
}
 
const onCloseDrawer=()=>{
  orderDrawer.value.handleClose();
}

const onClosed=()=>{ 
  authTipsVisible.value=false;
}

const onOperationFinished=()=>{
  onSelectedDate();
}

</script>
<style lang="scss" scoped>  
.content{  
   height: 450px;
   position: relative;
   .content-header{
     height: 60px; 
     text-align: left;  
     h5{
       background-color:rgba(32, 87, 87, 0.174);
       padding: 7px 15px;
       color: #6c6c6c;
       .header-title{
         img{
          position: relative;
          bottom: -2px;
          margin-right: 5px;
          height: 17px;
         }
       }
       .header-closed{
        position: relative;
        float: right; 
        top:-25px;
        cursor: pointer; 
        img{
          height: 30px;
        }
     }
     } 
   } 
   .content-body{ 
     height: 390px;
     padding: 10px; 
     margin-top: -15px;
     background-color:rgba(32, 87, 87, 0.174);
     .body-order{
      height: 400px;
      overflow-y: scroll;    
      background-color:rgb(244, 253, 253); 
      border-radius: 5px;
      padding: 0 5px; 
        .order-item{ 
          margin-top: 8px; 
          border-radius: 5px;
          border:1px solid #e6a23c;
          border-left: 5px solid #e6a23c; 
          .item-header{
            color: #888;
            .item-header-title{
              font-size: 15px;
              font-weight: 600;  
              position: relative;
              top: 30%;
            }
            .item-header-date{
              font-size: 13px;
              font-weight: 600; 
              position: relative;
              top: 33%; 
            }
          }
          .item-img{
            padding: 5px 0;
          }
          .item-desc{
            font-size: 12px;
            text-align: left;
            padding: 10px 0;
            .desc-title{
              font-weight: 600;
            }
            .desc-info{
              margin-top: 5px;
            } 
          } 
          .item-btn-deliver{
            background-color: #00bc42; 
            border-top: 1px solid #e6a23c;
            border-bottom: 1px solid #e6a23c;
              padding:0;
              color: #fff;
              width: 100%;
              height: 100%;  
              position: relative; 
              cursor: pointer; 
              right: 1px;
              div{ 
                position: absolute;
                top:50%;
                left: 50%;
                transform: translate(-50%,-50%);
                font-size: 14px; 
              }
          }
          .item-btn-remove{
              background-color: #e6a23c; 
              padding:0;
              color: #fff;
              width: 100%;
              height: 100%; 
              border-radius: 0 4px 4px 0;
              position: relative;
              cursor: pointer; 
              div{
                position: absolute;
                top:50%;
                left: 50%;
                transform: translate(-50%,-50%);
                font-size: 14px;
              }
            }
            .item-btn-done{ 
              padding:0;
              width: 100%;
              height: 100%;
              border-radius: 0 4px 4px 0;
              position: relative;
              img{
                height: 60px;
                position: absolute;
                top:0;
                right: 0;
              }
            }
        } 
     }
     .body-form{
        height: 380px;
        background-color:rgb(244, 253, 253);
        padding: 20px 10px 0 20%;
        border-radius: 5px; 
        width: 100%;
        .body-tips{
          color: #9b9b9b;
          .tips-icon{
            position: relative;
            top:2px;
          }
          padding-right: 20%;
          font-size: 13px;
          text-align: left;
        }
     } 
   } 
}
</style>