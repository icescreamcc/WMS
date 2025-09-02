<template>
    <div class="app-content">
        <div class="content-main">
            <div class="content-form">
            <el-form  :model="dataForm"  ref="dataFormRef"  label-width="auto" label-position="left" style="padding:0 15px">
              <div>
                <el-row class="input-item">
                    <el-col :span="6">
                        <div class="input-title" > 扫码查询</div> 
                    </el-col>
                    <el-col :span="16"> 
                        <input type="text"  v-model="dataForm.carSoleCode" @keyup.enter="onScanCarCode($event)" @focus="commonHelper.stopKeyborad($event)" id="input-carSoleCode" placeholder="扫描小车码/交接单号/库位码" autofocus class="input-cls">  
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
                        <div class="input-title">交接单号</div>
                    </el-col>
                    <el-col :span="16">
                        <input type="text" v-model="dataForm.detailNo" readonly class="input-cls"> 
                    </el-col>
                </el-row>
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">库位</div>
                    </el-col>
                    <el-col :span="16">
                        <input readonly v-model="dataForm.binNo"   class="input-cls"> 
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
                        <div class="input-title">计划装车数量</div>
                    </el-col>
                    <el-col :span="16">
                        <input type="text" :value="dataForm.planTotalByCar+dataForm.unitName" readonly class="input-cls">
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
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">状态</div>
                    </el-col>
                    <el-col :span="16">
                        <input type="text" :value="dataForm.statusDisplay" readonly class="input-cls">
                    </el-col> 
                </el-row>  
              </div> 
            </el-form>
            </div> 
        </div> 
        <div class="content-btn"> 
            <div v-if="permission.isPermisstion('QUERYORDERUNLOCK')&&dataForm.status=='Lock'&&!dataForm.prodctionTypeNo&&!dataForm.consignNum">
                <el-button  type="success" round  @click="unlockBinSubmit" style="width:82%;" :loading="submitLoading">库位解锁</el-button> 
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
import {getProductionOrderByCarCode,unlockBin} from '@/api/mobile-pda/queryOrder' 
import permission from '@/utils/system/permission'; 

const { t } = useI18n()
const dataForm = ref({
    scanCode:'',
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
    scanBinNo:'',
    createUser:'',
    status:'',
    statusDisplay:'',
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
    getProductionOrderByCarCode(dataForm.value.carSoleCode).then(res=>{
        dataForm.value=res.data;  
    }).catch(()=>dataForm.value.carSoleCode='');
 }
} 

const unlockBinSubmit=()=>{
    submitLoading.value=true;
    unlockBin(dataForm.value.binNo,permission.getOperator().userName).then(res=>{
        msg.deftAuto(`库位${dataForm.value.binNo}已解除锁定`);
        onClearForm();
    }).finally(()=>submitLoading.value=false);
}

const onClearForm=()=>{
    dataForm.value.scanCode='';
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
    dataForm.value.scanBinNo='';
    dataForm.value.status='';
    dataForm.value.statusDisplay='';
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
        height: 90%;
        overflow-y: hidden;
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
        height: 90%;
        overflow-y: hidden;
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