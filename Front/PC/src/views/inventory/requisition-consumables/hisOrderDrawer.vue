<template>
    <div>
       <el-drawer v-model="props.options.show" ref="orderDrawer" :close-on-click-modal="false" :with-header="false" :show-close="true" @close="onClosed"  direction="btt" size="500px">
       <div class="content">
         <div class="content-header">
          <h5>
            <div class="header-title">
              <img src="/public/icon-img/q065chakandingdan.png">
               <span>历史领用记录</span>
            </div>
            <div class="header-closed">
              <img @click="onCloseDrawer" src="/public/icon-img/guanbi_1.png">
            </div>
          </h5> 
         </div> 
          <div class="content-body" :style="{height:drawerHeight+'px'}">
            <el-row :gutter="20">
              <el-col :span="16">
                <div class="body-order">
                  <el-row v-if="orderData.length>0" class="order-item" :style="item.status=='Receiving'?pendingStyle:processedStyle"  v-for="item in orderData">
                    <el-col :span="3" class="item-header">
                      <div class="item-header-title">{{ item.orderNo }}</div>
                      <div class="item-header-date">{{ commonHelper.formatToDateTime(item.createDate) }}</div>
                    </el-col>
                    <el-col :span="5" class="item-img">
                      <el-image style="width: 70px;height: 70px; border-radius: 4px;" hide-on-click-modal :src="item.url"  :zoom-rate="1.2" :preview-src-list="[item.url]" :initial-index="0"  fit="cover"/> 
                    </el-col>
                    <el-col :span="7" style="text-align: left;">
                      <div class="item-desc">
                        <div class="desc-title">名称：{{ item.goodsName }}</div>
                        <div class="desc-info" v-if="item.goodsModel">型号：{{ item.goodsModel }}</div>
                        <div class="desc-info">状态：<span v-if="item.status=='Receiving'" style="color:#e6a23c">{{ item.statusDesc }}</span><span v-else style="color:#337d80">{{ item.statusDesc }}</span></div> 
                      </div>
                    </el-col> 
                    <el-col :span="ruleForm.goodsClassifyGroup=='SparePart'?7:8" style="text-align: left;">
                      <div class="item-desc">
                        <div class="desc-info"> 计划领用：{{ item.quantity+item.unitName }} </div>
                        <div class="desc-info" >实际发货：{{item.actualQuantity+item.actualUnitName }}</div>
                        <div class="desc-info">备&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;注：{{ item.purpose }}</div>
                      </div>
                    </el-col>
                    <el-col :span="1" v-if="ruleForm.goodsClassifyGroup=='SparePart'">   
                        <el-popconfirm v-if="item.status=='Receiving'" :title="`是否确认启用AGV送货？`" @confirm="onDeliver(item)">
                          <template #reference>
                            <div  class="item-btn-deliver" title="启用AGV自动送货"><div>送货</div></div>
                          </template>
                        </el-popconfirm>   
                    </el-col>
                    <el-col :span="1">   
                        <el-popconfirm v-if="item.status=='Receiving'" :title="`删除'${item.goodsName}'后仓库将不在对其发货，是否确认？`" @confirm="onRemove(item)">
                          <template #reference>
                            <div  class="item-btn-remove"><div>删除</div></div>
                          </template>
                        </el-popconfirm>  
                      <div v-else class="item-btn-done" ><img src="/public/icon-img/xuanze_2.png"></div>
                    </el-col>
                 </el-row>
                 <el-row v-else>
                  <el-col><el-empty description="没有任何数据" /></el-col>
                 </el-row>
                </div>
              </el-col>
              <el-col :span="8">
                <div class="body-form">
                  <el-form :model="ruleForm"  ref="formRef" label-width="auto" label-position="left">  
                    <el-row>
                      <el-col :span="18" >
                      <el-form-item label="刷卡认证">
                        <el-popover  :visible="authTipsVisible"  placement="left"  title="提示" :width="200"  content="确保客户端已安装读卡器，进行刷卡认证后允许查询">
                          <template #reference>
                            <el-input v-model="ruleForm.authInfo" :disabled="authDone" ref="authInput" @click="onShowSubmitTips"  @keyup.enter="userAuth" placeholder="点击这里光标闪烁时开始刷卡认证"/>
                          </template>
                        </el-popover> 
                      </el-form-item>
                      </el-col>
                    </el-row> 
                    <br>
                    <el-row>
                      <el-col :span="18">
                        <el-form-item label="选择日期">
                          <el-date-picker
                            ref="datePickerRef"
                            :disabled="!authDone"
                            v-model="ruleForm.queryDate"
                            type="date"
                            size="small"
                            placeholder="请选择查询日期"  
                            :shortcuts="shortcuts"
                            @change="onSelectedDate"
                            style="width:100%"
                          />
                          </el-form-item>  
                      </el-col> 
                    </el-row>   
                  </el-form> 
                  <br>
                  <div class="body-tips">
                    <el-icon class="tips-icon"><InfoFilled /></el-icon>
                    <span>提示：查询历史领用记录需要刷卡认证，认证通过后默认查询您当天提交的领用记录，选择日期可以查询其他天的记录，注意领用单状态，允许删除未发货的物品</span>
                  </div>
                </div> 
              </el-col>
            </el-row> 
          </div> 
       </div>
     </el-drawer>
     <AutoOperaionLayer :layer="autoOperationLayer" v-if="autoOperationLayer.show" @operationFinished="onOperationFinished"/> 
    </div>
</template>

<script lang="ts" setup>
import {ref ,defineEmits,defineProps,onMounted,onBeforeMount,onBeforeUnmount} from 'vue';   
import {swipingCardAuth, getOrderList,delReqisitionOrder,confirmReceived } from "@/api/inv/requisition";
import { ElForm } from 'element-plus'; 
import commonHelper from "@/utils/system/common-helper";  
import { ElLoading } from 'element-plus';
import { InfoFilled } from '@element-plus/icons-vue';
import AutoOperaionLayer from "@/components/auto-operation/autoOperaion.vue";
import { isAllowAutoTransport,getHisTask } from "@/api/auto-operation/auto-transport";
import msg from "@/utils/system/message";

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
var loadingOption={
 target:'', 
 text:"The data submission is being initiated to the warehouse.Waiting...", 
 background: 'rgba(6, 255, 255, 0.178)' 
}
const pendingStyle=ref({
  border:'1px solid #e6a23c',
  borderLeft:'5px solid #e6a23c'
})
const processedStyle=ref({
  border:'1px solid #337d80',
  borderLeft:'5px solid #337d80'
})
const orderData=ref(new Array<any>());   
const authInput=ref<null | HTMLElement>(null);
const authTipsVisible=ref(false);
const authDone=ref(false); 
const datePickerRef=ref<null | HTMLElement>(null);
const shortcuts = ref([
  {
    text: 'Today',
    value: new Date(),
  },
  {
    text: 'Yesterday',
    value: () => {
      const date = new Date()
      date.setTime(date.getTime() - 3600 * 1000 * 24)
      return date
    },
  },
  {
    text: 'A week ago',
    value: () => {
      const date = new Date()
      date.setTime(date.getTime() - 3600 * 1000 * 24 * 7)
      return date
    },
  },
])

onMounted(()=>{  
  setTimeout(() => { 
    authInput.value?.focus(); 
    onShowSubmitTips();
  }, 500);  
});

const onRemove=(item:any)=>{
  loadingOption.target=orderDrawer.value.$refs.drawerRef;
      let loadingInstance= ElLoading.service(loadingOption);  
      setTimeout(() => { 
        delReqisitionOrder(item.orderNo,item.detailId,ruleForm.value.userCard)
        .then(()=>{
          onSelectedDate(); 
        }) 
        .finally(()=>loadingInstance.close())
      }, 800);
  
}

const onDeliver=(item:any)=>{ 
  if(ruleForm.value.goodsClassifyGroup=="SparePart"){
    getHisTask(item.orderNo).then(hisRes=>{
          autoOperationLayer.value.data={
            title:'领用出库',
            orderNo:hisRes.data[0].orderNo,
            goodsClassifyGroup:ruleForm.value.goodsClassifyGroup,
            operatorId:ruleForm.value.userId,
            operator:ruleForm.value.userName,
            orderType:hisRes.data[0].orderType,
            line:hisRes.data[0].line,
            remark:hisRes.data[0].remark,
            details:hisRes.data
          };
         autoOperationLayer.value.show=true; 
        });  
  } 
}

const onShowSubmitTips=()=>{
  authTipsVisible.value=true;
    setTimeout(() => {
      authTipsVisible.value=false;
    }, 4000);
}

const userAuth=()=>{
  swipingCardAuth(ruleForm.value.authInfo).then((res:any)=>{
    ruleForm.value.userId=res.data.userId;
    ruleForm.value.userName=res.data.userName;
    ruleForm.value.userCard=res.data.cardId;
    ruleForm.value.authInfo=res.data.userName+' '+res.data.cardId;
    authDone.value=true;
    authTipsVisible.value=false;
    onSelectedDate(); 
  })
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