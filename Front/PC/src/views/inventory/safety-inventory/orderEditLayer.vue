<template>
    <Layer :layer="layer" @confirm="submit" >
       <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
        <el-tabs v-model="tabSelected" style="box-shadow:none;-webkit-box-shadow:none;min-height: 280px;">
          <el-tab-pane label="审批" name="approval"  v-if="false">
            <el-row>
             <el-col>
                  <el-form-item label="审批意见" prop="opinion">
                  <el-input v-model="ruleForm.approvalOpinion" type="textarea" rows="4"/>
                </el-form-item>
             </el-col>
           </el-row>
             <el-row>
              <el-col :span="11">
                  <el-form-item label="审批结果" prop="approvalResult" style="text-align:left">
                       <el-radio-group v-model="ruleForm.approvalResult" @change="onSelectApprovalResult">
                         <el-radio-button :key="'Approve'" :label="'Approve'">审批通过</el-radio-button> 
                         <el-radio-button :key="'Reject'" :label="'Reject'">审批拒绝</el-radio-button>  
                      </el-radio-group> 
              </el-form-item>
              </el-col> 
            </el-row>  
          </el-tab-pane>
          <el-tab-pane label="预警信息" name="info">
            <el-descriptions class="margin-top"  :column="3" size="small" border> 
            <el-descriptions-item>
              <template #label>
                <div class="cell-item"> <el-icon class="item-icon"><Goods /></el-icon> 名称</div>
              </template>
              <span class="item-title">{{ ruleForm.goodsName }}</span>
            </el-descriptions-item>
            <el-descriptions-item>
              <template #label>
                <div class="cell-item"><el-icon class="item-icon"><Postcard /></el-icon> 编码</div>
              </template>
              <span class="item-title">{{ ruleForm.goodsNo }}</span>
            </el-descriptions-item>
            <el-descriptions-item>
              <template #label>
                <div class="cell-item"><el-icon class="item-icon"><PriceTag /></el-icon> 型号</div>
              </template>
              <span class="item-title">{{ ruleForm.goodsModel }}</span> 
            </el-descriptions-item>
            <el-descriptions-item>
              <template #label>
                <div class="cell-item">
                  <el-icon class="item-icon"> <Coin /></el-icon> 现存量
                </div>
              </template>
              <el-tag size="small" :type="ruleForm.curInvenstory==0?'danger':'warning'">  {{ ruleForm.curInvenstory }}{{ ruleForm.safetyInventoryUnitName }}</el-tag>
            </el-descriptions-item>
            <el-descriptions-item>
              <template #label>
                <div class="cell-item"><el-icon class="item-icon"><Box /></el-icon> 安全库存量</div>
              </template>
              <el-tag size="small" v-if="!isEditSafetyInv">  {{ ruleForm.safetyInventory }}{{ ruleForm.safetyInventoryUnitName }}</el-tag> 
              <span  v-if="!isEditSafetyInv&&props.layer.showButton" @click="()=>isEditSafetyInv=!isEditSafetyInv" style="display: inline-block; color: #888;margin-left: 10px;cursor: pointer;position: relative;bottom: -3px;" title="修改安全库存"><el-icon><EditPen /></el-icon></span>
              <el-input-number v-if="isEditSafetyInv" v-model="ruleForm.safetyInventory"  :min="0" controls-position="right" style=" width:35%" /> 
            </el-descriptions-item>
            <el-descriptions-item>
              <template #label>
                <div class="cell-item">
                  <el-icon class="item-icon">
                    <Star />
                  </el-icon>
                  成本价
                </div>
              </template>
              {{ ruleForm.costPrice }}{{ ruleForm.priceUnitName }}/{{ ruleForm.costPriceUnitName }}
            </el-descriptions-item>
            <el-descriptions-item>
              <template #label>
                <div class="cell-item">
                  <el-icon class="item-icon">
                    <ShoppingCartFull />
                  </el-icon>
                  最小采购量
                </div>
              </template>
              {{ruleForm.purchaseMinimum  }}{{ruleForm.purchaseMinimumUnitName  }}
            </el-descriptions-item>
            <el-descriptions-item>
              <template #label>
                <div class="cell-item">
                  <el-icon class="item-icon">
                    <CircleCheck/>
                  </el-icon>
                  是否需要采购
                </div>
              </template>
              <el-radio-group v-model="ruleForm.isNeedPurchase" v-if="props.layer.showButton">
                <el-radio :label="true" :key="1">是</el-radio>
                <el-radio :label="false" :key="0">否</el-radio> 
              </el-radio-group> 
              <span v-else>{{ ruleForm.needPurchase }}</span>
            </el-descriptions-item>
            <el-descriptions-item>
              <template #label>
                <div class="cell-item">
                  <el-icon class="item-icon">
                    <tickets />
                  </el-icon>
                  采购单号
                </div>
              </template>
              <el-input v-if="props.layer.showButton" v-model="ruleForm.purchaseOrderNo" :disabled="!ruleForm.isNeedPurchase" placeholder="请输入采购单号"></el-input> 
              <span v-else>{{ ruleForm.purchaseOrderNo }}</span>
            </el-descriptions-item>
            <el-descriptions-item>
              <template #label>
                <div class="cell-item">
                  <el-icon class="item-icon">
                    <ShoppingCart />
                  </el-icon>
                  采购数量
                </div>
              </template>
              <el-input-number v-if="props.layer.showButton" v-model="ruleForm.requirementQuantity" :disabled="!ruleForm.isNeedPurchase"  :min="0" controls-position="right" /> 
              <span v-else>{{ ruleForm.requirementQuantity }}</span>
            </el-descriptions-item>
            <el-descriptions-item>
              <template #label>
                <div class="cell-item">
                  <el-icon class="item-icon">
                    <Bell />
                  </el-icon>
                  状态
                </div>
              </template>
              <el-tag size="small" v-if="ruleForm.status=='Warning'" :type="'danger'"> {{ ruleForm.statusDesc }}</el-tag>
              <el-tag size="small" v-else-if="ruleForm.status=='NoPurchase'" :type="'info'"> {{ ruleForm.statusDesc }}</el-tag>
              <el-tag size="small" v-else-if="ruleForm.status=='Purchasing'" :type="'warning'"> {{ ruleForm.statusDesc }}</el-tag>
              <el-tag size="small" v-else-if="ruleForm.status=='Received'" :type="'success'"> {{ ruleForm.statusDesc }}</el-tag> 
            </el-descriptions-item>
            <el-descriptions-item>
              <template #label>
                <div class="cell-item">
                  <el-icon class="item-icon">
                    <CollectionTag />
                  </el-icon>
                  备注
                </div>
              </template>
              <el-input v-model="ruleForm.remark" v-if="props.layer.showButton" placeholder="备注"></el-input> 
              <span v-else>{{ ruleForm.remark }}</span>
            </el-descriptions-item>
          </el-descriptions>
          </el-tab-pane>
          <el-tab-pane label="审批记录" name="approvalHis" v-if="!props.layer.showButton">
            <div style="max-height: 225px; overflow-y: scroll;">
                <el-timeline style="max-width: 600px">
                    <el-timeline-item v-for="(his,index) in ruleForm.approvalHis" center size="normal" :placement="'bottom'" :timestamp="commonHelper.formatToDateTime(his.approvalDate)" :icon="his.icon" :color="his.color">
                        <el-card style="text-align: left;height: 120px;margin-top: 5px;" shadow="hover">
                            <el-row>
                                <el-col :span="2"><img src="../../../../public/icon-img/renyuan.png" height="18"></el-col>
                                <el-col :span="22">
                                    <div style="padding: 3px 0;font-size: 13px;color: #888;"> <span>{{ his.approverName }}</span> <span>（{{ his.approverRoleName }}）</span></div> 
                                </el-col>
                            </el-row>
                            <el-row style="margin-top: 10px;">
                                <el-col :span="2"><img src="../../../../public/icon-img/jieguobijiao.png" height="18"></el-col>
                                <el-col :span="22">
                                    <div :style="{color:his.color}" style="padding: 3px 0;font-size: 13px;">{{ his.approvalStatusDesc }}</div>
                                </el-col>
                            </el-row>
                            <el-row style="margin-top: 10px;margin-bottom: 15px;">
                                <el-col :span="2"><img src="../../../../public/icon-img/tiaochajieguo.png" height="18"></el-col>
                                <el-col :span="22">
                                    <div style="border-top: 1px solid #ececec;padding: 5px 0;font-size: 12px;color: #888;">{{ his.opinion }}</div>
                                </el-col>
                            </el-row>   
                        </el-card>
                    </el-timeline-item> 
                </el-timeline>
              </div>
          </el-tab-pane>
        </el-tabs> 
       </el-form> 
    </Layer> 
  </template>
  
  <script lang="ts" setup>
  import {  ref,defineEmits,defineProps,onMounted } from 'vue'; 
  import Layer from '@/components/layer/index.vue'; 
  import { ElForm } from 'element-plus'; 
  import msg from '@/utils/system/message';  
  import permission from '@/utils/system/permission';  
  import commonHelper from "@/utils/system/common-helper";
  import {Tickets,Postcard,EditPen, PriceTag,Goods,Bell,CircleCheck,Box,Suitcase,ShoppingCart,Coin,CollectionTag,Star,ShoppingCartFull} from '@element-plus/icons-vue'
  import {getOptions} from "@/api/inv/safety-warning-inventory";

  const props=defineProps({
    layer: {
        type: Object,
        default: () => {
          return {
            show: false,
            title: '',
            showButton: true,
            btnLoading:false,
            type:'',
            options:null,
            data:null 
          }
        }
      }
  });
  const emit = defineEmits(['dataSubmit']);     
  const formRef= ref(ElForm||null);    
  const isEditSafetyInv=ref(false);
  const tabSelected=ref('info');
  const ruleForm = ref({
    detailId:props.layer.data?.detailId,   
    goodsId:props.layer.data?.goodsId,
    goodsNo:props.layer.data?.goodsNo,
    goodsName:props.layer.data?.goodsName,     
    goodsModel:props.layer.data?.goodsModel,     
    curInvenstory:props.layer.data?.curInvenstory,   
    safetyInventory:props.layer.data?.safetyInventory, 
    safetyInventoryUnitName:props.layer.data?.safetyInventoryUnitName, 
    isNeedPurchase:props.layer.data?.isNeedPurchase, 
    needPurchase:props.layer.data?.needPurchase, 
    purchaseMinimum:props.layer.data?.purchaseMinimum,
    purchaseMinimumUnitName:props.layer.data?.purchaseMinimumUnitName,
    purchaseOrderNo:props.layer.data?.purchaseOrderNo,
    requirementQuantity:props.layer.data?.requirementQuantity,
    costPrice:props.layer.data?.costPrice,
    priceUnitName:props.layer.data?.priceUnitName,
    costPriceUnitName:props.layer.data?.costPriceUnitName,
    status:props.layer.data?.status, 
    statusDesc:props.layer.data?.statusDesc, 
    remark:props.layer.data?.remark,  
    updateUserId:permission.getOperator().userId,
    updateUserName:permission.getOperator().userName,
    approvalOpinion:props.layer.data?.approvalOpinion, 
    approvalResult:props.layer.data?.approvalResult, 
    approvalHis:props.layer.data?.approvalHis, 
  });   
  const statusData=ref(new Array<any>());
  const rules = {
     
  }   
  onMounted(()=>{  
    getOptions().then(res=>{
        statusData.value=res.data.statusData; 
      })
  }); 

  const onSelectApprovalResult=()=>{
    if(ruleForm.value.approvalResult){
      if(ruleForm.value.approvalResult=='Approve'){
        ruleForm.value.approvalOpinion="同意";
      }
      else{
        ruleForm.value.approvalOpinion="不同意";
      }
    }
  }
     
  const submit=()=> {    
    if(!ruleForm.value.isNeedPurchase){
      if(!ruleForm.value.remark){
        msg.warningAuto("如果不需要采购，则请备注说明");
        return;
      } 
    }
    if(ruleForm.value.status!="Warning"){
      if(!ruleForm.value.requirementQuantity||ruleForm.value.requirementQuantity==0||!ruleForm.value.purchaseOrderNo){
        msg.warningAuto("请填写采购单号和采购数量");
        return;
      }
    }  
    emit('dataSubmit', ruleForm.value,'update') 
  }
  </script>
  
  <style lang="scss" scoped>
    .item-title{
      font-weight: 600;
    }
    .item-icon{
      position: relative;
      bottom: -3px;
    }
  </style>