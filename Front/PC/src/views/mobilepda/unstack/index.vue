<template>
    <div class="app-content">
        <div class="content-main">
            <div class="content-form">
            <el-form  :model="dataForm"  ref="dataFormRef"  label-width="auto" label-position="left" style="padding:0 15px">
              <div>
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">小车唯一码</div> 
                    </el-col>
                    <el-col :span="14"> 
                        <input type="text" v-model="dataForm.carSoleCode" @keyup.enter="onScanCarCode($event)" @focus="commonHelper.stopKeyborad($event)" id="input-carSoleCode" placeholder="请扫描小车唯一码" autofocus class="input-cls">  
                    </el-col>
                    <el-col :span="2"> 
                       <div class="input-icon">
                        <img :src="scanImg" alt="" height="20"> 
                       </div>
                    </el-col>
                </el-row>
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">拆垛库位码</div> 
                    </el-col>
                    <el-col :span="14"> 
                        <input type="text" v-model="dataForm.scanBinNo" @keyup.enter="onScanBinCode($event)" @focus="commonHelper.stopKeyborad($event)" id="input-binNo" placeholder="请扫描拆垛机库位码" class="input-cls">  
                    </el-col>
                    <el-col :span="2"> 
                       <div class="input-icon">
                        <img :src="scanImg" alt="" height="20"> 
                       </div>
                    </el-col>
                </el-row> 
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">发货型号</div>
                    </el-col>
                    <el-col :span="16">
                        <input type="text" v-model="dataForm.consignNum" readonly class="input-cls"> 
                    </el-col>
                </el-row>
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">推荐拆垛库位</div>
                    </el-col>
                    <el-col :span="16">
                        <input readonly v-model="dataForm.unstackBinNo"   class="input-cls"> 
                    </el-col> 
                </el-row> 
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">产品型号</div>
                    </el-col>
                    <el-col :span="16">
                        <input type="text" v-model="dataForm.prodctionTypeNo" readonly class="input-cls"> 
                    </el-col>
                </el-row>
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">生产数量</div>
                    </el-col>
                    <el-col :span="16">
                        <input type="text" :value="dataForm.total+dataForm.unitName" readonly class="input-cls"> 
                    </el-col>
                </el-row>
                <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">需求车次</div>
                        </el-col>
                        <el-col :span="16">
                            <input type="text" v-model="dataForm.countByCar" readonly class="input-cls"> 
                        </el-col>
                </el-row> 
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">小车配对数</div>
                    </el-col>
                    <el-col :span="16">
                        <input type="text" v-model="dataForm.matchingCount" readonly class="input-cls"> 
                    </el-col>
                </el-row> 
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">实际装车数量</div>
                    </el-col>
                    <el-col :span="16">
                        <input type="text" :value="dataForm.actualTotal+dataForm.unitName" readonly class="input-cls">
                    </el-col> 
                </el-row>  
              </div> 
            </el-form>
            </div>
            <div class="content-desc">
               <div style="padding-top: 4%;">
                <div style="margin-bottom: 10px;">
                    <el-icon><InfoFilled /></el-icon>
                    <span> 操作流程：</span>
                </div> 
                <div>
                    <p style="margin-top: -5px;">扫描小车唯一码→查询装车信息→扫描推荐的拆垛机库位码→确认上架</p>
                    <p style="margin-top: -5px;">当扫描库位码与推荐的库位不一致时不允许上架</p> 
                </div>
               </div>
            </div> 
        </div> 
        <div class="content-btn">
            <div v-if="permission.isPermisstion('PUTAWAYSUBMIT')">
                <el-button  type="success" round  @click="onSubmit" style="width:82%;" :loading="submitLoading">确认上架</el-button> 
            </div>
            <div style="margin-top: 4px;">
                <el-button  round  @click="onClearForm" style="width:82%;" :loading="submitLoading">重置信息</el-button>
            </div>
        </div> 
        </div> 
</template>

<script lang="ts" setup>
import {  ref ,nextTick,onMounted} from "vue";
import { useI18n } from 'vue-i18n'   
import { ElForm } from 'element-plus'   
import {InfoFilled} from '@element-plus/icons-vue';   
import commonHelper from '@/utils/system/common-helper';
import msg from "@/utils/system/message"
import {getProductionOrderInfo,checkOnUnstack,submitOnUnStack} from '@/api/mobile-pda/unstack'
import permission from '@/utils/system/permission'; 

const { t } = useI18n()
const dataForm = ref({
    detailNo:'',
    consignNum:'',
    carSoleCode:'',
    deliverNo:'',
    prodctionTypeNo:'',
    total:'',
    matchingCount:'',
    planTotalByCar:'',
    actualTotal:'',
    countByCar:'',
    unitName:'',
    binId:'',
    binNo:'',
    unstackBinId:'',
    unstackBinNo:'',
    scanBinNo:'',
    createUser:''
  });  
const dataFormRef = ref(ElForm||null); 
const scanImg='/public/icon-img/saoma2.png';
const submitLoading=ref(false); 

onMounted(()=>{
    document.getElementById('input-carSoleCode')?.focus();  
})
 
const onScanCarCode=(e:any)=>{ 
 e.preventDefault(); 
 if(e.keyCode==13&&dataForm.value.carSoleCode){   
    getProductionOrderInfo(dataForm.value.carSoleCode).then(res=>{
        dataForm.value=res.data; 
        msg.deftAuto('请扫描库位码进行上架校验');
        document.getElementById('input-binNo')?.focus();  
    }).catch(()=>dataForm.value.carSoleCode='');
 }
}

const onScanBinCode=(e:any)=>{ 
 e.preventDefault(); 
 if(e.keyCode==13&&dataForm.value.carSoleCode&&dataForm.value.scanBinNo){ 
    checkOnUnstack(dataForm.value.carSoleCode,dataForm.value.scanBinNo).then(res=>{
        if(res.data){
            msg.deftAuto('校验成功');
        }
    }).catch(()=>dataForm.value.scanBinNo='');
 }
}

const onSubmit=()=>{ 
    if(!dataForm.value.carSoleCode){
        msg.deftAuto('请扫描或输入小车唯一码');
        document.getElementById('input-carSoleCode')?.focus();  
        return;
    }
    if(!dataForm.value.binNo){
        msg.deftAuto('请扫描或输入库位码');
        document.getElementById('input-binNo')?.focus();  
        return;
    }  
    submitLoading.value=true;
    dataForm.value.createUser=permission.getOperator().userName;
    submitOnUnStack(dataForm.value).then(res=>{ 
        onClearForm();
    }).catch(()=>dataForm.value.scanBinNo='').finally(()=>{ submitLoading.value=false;}) 
}
  
const onClearForm=()=>{
    dataForm.value.detailNo='';
    dataForm.value.consignNum='';
    dataForm.value.carSoleCode='';
    dataForm.value.deliverNo='';
    dataForm.value.prodctionTypeNo='';
    dataForm.value.total='';
    dataForm.value.matchingCount='';
    dataForm.value.planTotalByCar='';
    dataForm.value.actualTotal='';
    dataForm.value.countByCar='';
    dataForm.value.unitName='';
    dataForm.value.binId='';
    dataForm.value.binNo='';
    dataForm.value.unstackBinId='';
    dataForm.value.unstackBinNo='';
    dataForm.value.scanBinNo='';
    document.getElementById('input-carSoleCode')?.focus();  
}
</script>

<style lang="scss" scoped>
@media screen and (max-width: 450px){
    .app-content{
        height: 87%;
        background-color: #fff;
       .content-main{
        padding :0px 30px 0 25px;
        height: 85%; 
        .content-form{ 
            .input-item{
            font-size: 14px;
            color: #535353;
            margin-bottom: 12px;
            .input-cls{  
                width: 100%; 
                height: 26px;
                line-height: 26px;
                border: none; 
                outline: none;
                background-color: transparent; 
                border-bottom: 1px solid #dfdfdf;
                text-align: center;
                position: relative;
                bottom: -17px;
                font-size: 14px;
            }
            .input-title{
                color: #8b8b8b;
                width: 100%;
                position: relative;
                text-align: left;
                bottom: -24px;
                padding-bottom: 5px;
                border-bottom: 1px solid #dfdfdf;
            }
            .input-icon{
                position: relative;
                bottom: -71%;
                left: 19%;
                text-align: right;
                border-bottom: 1px solid #dfdfdf;
                padding-bottom: 1px; 
            }
        } 
        }
      
        .content-desc{
                font-size: 11px;
                color: #a6a5a5; 
                height: 30%;
                text-align: left;
                padding: 0 10px;
            } 
       }
       .content-btn{
            height: 10%; 
        }
    }
}

@media screen and (min-width: 450px){
    .app-content{
        height: 87%;
        background-color: #fff;
       .content-main{
        padding :0px 30px 0 25px;
        height: 85%; 
        .content-form{ 
            .input-item{
            font-size: 14px;
            color: #535353;
            margin-bottom: 12px;
            .input-cls{  
                width: 100%; 
                height: 26px;
                line-height: 26px;
                border: none; 
                outline: none;
                background-color: transparent; 
                border-bottom: 1px solid #dfdfdf;
                text-align: center;
                position: relative;
                bottom: -17px;
                font-size: 14px;
            }
            .input-title{
                color: #8b8b8b;
                width: 100%;
                position: relative;
                text-align: left;
                bottom: -24px;
                padding-bottom: 5px;
                border-bottom: 1px solid #dfdfdf;
            }
            .input-icon{
                width: 110%;
                position: relative;
                bottom: -71%;
                left: 1%;
                text-align: right;
                border-bottom: 1px solid #dfdfdf;
                padding-bottom: 1px; 
            }
        } 
        }
      
        .content-desc{
                font-size: 11px;
                color: #a6a5a5; 
                height: 30%;
                text-align: left;
                padding: 0 10px;
            } 
       }
       .content-btn{
            height: 10%; 
            button{
                    height: 35px; 
                }
        }
    }
} 
</style>