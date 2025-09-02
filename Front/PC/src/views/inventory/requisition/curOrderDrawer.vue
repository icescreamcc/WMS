<template>
    <div>
       <el-drawer v-model="props.options.show" ref="orderDrawer" :close-on-click-modal="false" :with-header="false" :show-close="true" @close="onClosed"  direction="btt" size="500px">
       <div class="content">
         <div class="content-header">
          <h5>
            <div class="header-title">
              <img src="/public/icon-img/bianji4.png">
               <span>领用单提交</span>
            </div>
            <div class="header-closed">
              <img @click="onCloseDrawer" src="/public/icon-img/guanbi_1.png">
            </div>
          </h5> 
         </div> 
          <div class="content-body" :style="{height:drawerHeight+'px'}">
            <el-row :gutter="20">
              <el-col :span="8">
                <div class="body-order">
                  <el-row class="order-item" :style="submitDone?processedStyle:pendingStyle" v-for="item in ruleForm.details">
                  <el-col :span="6" class="item-img">
                      <el-image style="width: 105px;height: 105px; border-radius: 4px;" hide-on-click-modal :src="item.goodsPicture"  :zoom-rate="1.2" :preview-src-list="[item.goodsPicture]" :initial-index="0"  fit="cover"/> 
                    </el-col>
                    <el-col :span="16" style="text-align: left;">
                      <div class="item-desc">
                        <div class="desc-title">名称：{{ item.goodsName }}</div>
                        <div class="desc-info">编码：{{ item.goodsNo }}</div>
                        <div class="desc-info" v-if="item.goodsModel">型号：{{ item.goodsModel }}</div>
                        <div class="desc-info" v-if="item.goodsModel">库位：{{ item.deftStockBinCell||item.deftStockBin }}</div>
                        <div class="desc-info">数量：
                          <el-input-number style="width: 28%;" v-model="item.quantity" placeholder="领用数量" :min="1" :max="10000" />
                          <span style="margin-left: 6%;">{{ item.unitName }}</span></div>
                      </div>
                    </el-col> 
                    <el-col :span="2"> 
                      <div v-if="!submitDone" class="item-btn-remove" @click="onRemove(item)"><div>移除</div></div>
                      <div v-else class="item-btn-done" ><img src="/public/icon-img/xuanze_2.png"></div>
                    </el-col>
                 </el-row>
                </div>
              </el-col>
              <el-col :span="16">
                <div class="body-form">
                  <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left"> 
                    <el-row>
                      <el-col :span="11">
                        <el-form-item label="领用类型" prop="orderType">
                          <el-select v-model="ruleForm.orderType" :disabled="submitDone" class="m-2"  style="width:100%" placeholder="请选择领用类型">
                                    <el-option v-for="item in reqTypeData" :key="item.key" :label="item.value" :value="item.key">
                                      <span style="float: left">{{ item.value }}</span>
                                      <span style=" float: right; color: var(--el-text-color-secondary);  font-size: 13px;">{{ item.key }}</span>
                                  </el-option>
                            </el-select>
                          </el-form-item>  
                      </el-col> 
                    </el-row>
                    <br>
                    <el-row>
                      <el-col :span="11">
                        <el-form-item label="适用产线" prop="line">
                          <el-select v-model="ruleForm.line" :disabled="submitDone" class="m-2"  style="width:100%" placeholder="请选择产线">
                                    <el-option v-for="item in lineData" :key="item.key" :label="item.value" :value="item.key">
                                  </el-option>
                            </el-select>
                          </el-form-item>  
                      </el-col> 
                    </el-row>
                    <br>
                    <el-row> 
                      <el-col :span="11" >
                      <el-form-item label="领用目的" prop="purpose">
                        <el-input v-model="ruleForm.purpose" :disabled="submitDone" type="textarea" show-word-limit :autosize="{ minRows: 2, maxRows: 4 }" placeholder="用途、目的、备注"/>
                      </el-form-item>
                      </el-col>
                    </el-row> 
                    <br>
                    <el-row>
                      <el-col :span="11" >
                      <el-form-item label="刷卡认证" prop="createUserInfo">
                        <el-popover  :visible="authTipsVisible"  placement="right"  title="提示" :width="200"  content="确保客户端已安装读卡器，进行刷卡认证后领用单将自动提交">
                          <template #reference>
                            <el-input v-model="ruleForm.createUserInfo" :disabled="submitDone" ref="authInput" @click="onShowSubmitTips"  @keyup.enter="dataSubmit" placeholder="点击这里光标闪烁时开始刷卡认证"/>
                          </template>
                        </el-popover> 
                      </el-form-item> 
                      </el-col>
                    </el-row>
                    <br>
                    <el-row> 
                      <el-col :span="11" >
                      <el-form-item label="AGV送货" style="text-align: left;">
                        <el-checkbox  label="是" v-model="ruleForm.useAgv"/>     
                      </el-form-item>
                      </el-col> 
                    </el-row> 
                  </el-form>  
                  <div class="body-tips">
                    <el-icon class="tips-icon"><InfoFilled /></el-icon>
                    <span>提示：先选择领用类型、适用产线、填写领用目的（可选填），然后点击刷卡认证文本框，使其获取焦点后使用员工卡进行刷卡认证，认证通过后领料单将自动提交</span>
                  </div>
                </div> 
              </el-col>
            </el-row> 
          </div> 
       </div>
     </el-drawer>
     <AutoOperaionLayer :layer="autoOperationLayer" v-if="autoOperationLayer.show"/> 
    </div>
</template>

<script lang="ts" setup>
import {ref ,defineEmits,defineProps,onMounted,onBeforeMount,onBeforeUnmount} from 'vue';  
import {  getLines} from "@/api/common"; 
import { getRequisitionTypes,addReqisitionOrder,swipingCardAuth } from "@/api/inv/requisition";
import { ElForm } from 'element-plus'; 
import { ElLoading } from 'element-plus';
import { InfoFilled } from '@element-plus/icons-vue'; 
import AutoOperaionLayer from "@/components/auto-operation/autoOperaion.vue";
import { isAllowAutoTransport } from "@/api/auto-operation/auto-transport";
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
const autoOperationLayer:any = ref({
      show: false,
      title: "",
      showButton: true,
      btnLoading:false, 
      width:"78%",
      data:null,  
      otherButton:{}
}); 
const orderDrawer=ref();
var loadingOption={
 target:'', 
 text:"The data submission is being initiated to the warehouse.Waiting...", 
 background: 'rgba(6, 255, 255, 0.178)' 
}
const drawerHeight=ref(400);
const emit = defineEmits(['submitRemove','submited']);
const ruleForm=ref({
        goodsClassifyGroup:props.options.type,
        purpose:'',
        line:'', 
        orderType:'',  
        createUserCard:'',
        createUserInfo:'',
        useAgv:true, 
        details:props.options.data
});
const rules = { 
  purpose: [{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
  orderType:   [{ required: true, message: '请选择领用类型', trigger: 'blur' }],
  line:   [{ required: true, message: '请选择适用产线', trigger: 'change' }],
  createUserInfo:   [{ required: true, message: '请进行刷卡认证', trigger: 'blur' }]
}
const lineData=ref(new Array<any>());
const reqTypeData=ref(new Array<any>());
const formRef= ref(ElForm||null);
const authInput=ref<null | HTMLElement>(null);
const authTipsVisible=ref(false);
const submitDone=ref(false);
const pendingStyle=ref({
  border:'1px solid #e6a23c',
  borderLeft:'5px solid #e6a23c'
})
const processedStyle=ref({
  border:'1px solid #337d80',
  borderLeft:'5px solid #337d80'
})

onMounted(()=>{  
  console.log("ruleForm",ruleForm.value)
  setTimeout(() => { 
    onShowSubmitTips();
  }, 500);
  getLines().then(res=>{
    lineData.value=res.data;
  }) 
  getRequisitionTypes(ruleForm.value.goodsClassifyGroup).then(res=>{
    reqTypeData.value=res.data;
  })
});

const onRemove=(item:any)=>{
  ruleForm.value.details.splice(ruleForm.value.details.indexOf(item),1);
  emit('submitRemove',ruleForm.value.details);
}

const onShowSubmitTips=()=>{
  authTipsVisible.value=true;
    setTimeout(() => {
      authTipsVisible.value=false;
    }, 4000);
}

const dataSubmit=()=>{
  formRef.value.validate((valid:any)=>{ 
    if(valid){ 
      swipingCardAuth(ruleForm.value.createUserInfo).then(res=>{
        if(res.data){ 
          ruleForm.value.createUserCard=ruleForm.value.createUserInfo;
          ruleForm.value.createUserInfo=`${res.data.userName}/${res.data.cardId}`;
          loadingOption.target=orderDrawer.value.$refs.drawerRef;
          let loadingInstance= ElLoading.service(loadingOption);  
          setTimeout(() => {  
            addReqisitionOrder(ruleForm.value)
            .then((addRes:any)=>{
              //启用AGV送货
              if(ruleForm.value.goodsClassifyGroup=="SparePart"){
                if(ruleForm.value.useAgv){
                    autoOperationLayer.value.data={
                        title:'领用出库',
                        orderNo:addRes.data,
                        goodsClassifyGroup:ruleForm.value.goodsClassifyGroup,
                        operatorId:res.data.userId,
                        operator:res.data.userName,
                        orderType:ruleForm.value.orderType,
                        line:ruleForm.value.line,
                        remark:ruleForm.value.purpose,
                        details:ruleForm.value.details
                      };
                    autoOperationLayer.value.show=true; 
                  }
              } 
              submitDone.value=true;
              emit('submited'); 
            })
            .catch(err=>{
              submitDone.value=false;
              ruleForm.value.createUserCard="";
              authInput.value?.focus(); 
            })
            .finally(()=>loadingInstance.close())
          }, 1000);
        }
      }) 
    }
  })   
}
  
const onCloseDrawer=()=>{
  orderDrawer.value.handleClose();
}

const onClosed=()=>{ 
  authTipsVisible.value=false;
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
          // border:1px solid #e6a23c;
          // border-left: 5px solid #e6a23c; 
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
                top:35%;
                left: 15%;
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
        padding: 20px 10px 0 15%;
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