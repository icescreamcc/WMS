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
                            <input type="text" v-model="dataForm.carSoleCode" @keyup.enter="onScanCode($event)" id="input-carSoleCode" @focus="commonHelper.stopKeyborad($event)" placeholder="请扫描小车唯一码" autofocus class="input-cls">  
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
                            <div class="input-title">计划装车数量</div>
                        </el-col>
                        <el-col :span="16">
                            <input type="text" :value="dataForm.planTotalByCar+dataForm.unitName" readonly class="input-cls">
                        </el-col>
                    </el-row>  
                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">存放库位</div>
                        </el-col>
                        <el-col :span="14">
                            <input readonly v-model="dataForm.binNo" :class="isSearchBin?'':'text-warning'" placeholder="扫码自动查询推荐"  class="input-cls"> 
                        </el-col>
                        <el-col :span="2"> 
                        <div class="input-icon">
                            <img :src="binImg" alt="" height="20"> 
                        </div>
                        </el-col>
                    </el-row> 
                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">实际装车数量</div>
                        </el-col>
                        <el-col :span="14">
                            <input type="number" id="input-actualTotalByCar" v-model="dataForm.actualTotal" placeholder="请输入实际装车数量" @focus="commonHelper.stopKeyborad($event)" class="input-cls"> 
                        </el-col>
                        <el-col :span="2"> 
                        <div class="input-icon">
                            <img :src="editImg" alt="" height="20"> 
                        </div>
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
                    <p style="margin-top: -5px;">扫描小车唯一码→自动查询并推荐上架库位→点击确认装车→锁定库位</p> 
                </div>
               </div>
            </div> 
        </div> 
        <div class="content-btn">
                <div v-if="permission.isPermisstion('TRUCKSUBMIT')">
                    <el-button  type="success" round  @click="onShowLockBinInfo" style="width:82%;" :loading="submitLoading">确认装车</el-button> 
                </div>
                <div style="margin-top: 4px;">
                    <el-button  round  @click="onClearForm" style="width:82%;" :loading="submitLoading">重置信息</el-button>
                </div>
            </div> 
        <MessageDrawer :options="msgDrawerOptions" @cancel="onLockBinCancel" @confirm="onSubmit"/>
    </div> 
</template>

<script lang="ts" setup>
import {  ref ,nextTick,onMounted} from "vue";
import { useI18n } from 'vue-i18n'   
import { ElForm } from 'element-plus'    
import {InfoFilled} from '@element-plus/icons-vue';  
import commonHelper from '@/utils/system/common-helper';
import msg from "@/utils/system/message"
import MessageDrawer from '../message-drawer.vue'
import {getProductionOrderInfo,getFreeBin,lockBin,submitCarLoad} from '@/api/mobile-pda/carLoading'
import permission from '@/utils/system/permission';  
 
const { t } = useI18n();
const msgDrawerOptions=ref({
    show:false,
    title:'',
    message:'',
    type:'',
    data:null
});
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
    createUser:permission.getOperator().userName
  });  
const dataFormRef = ref(ElForm||null); 
const scanImg='/public/icon-img/saoma2.png';
const editImg='/public/icon-img/bianji5.png';
const binImg='/public/icon-img/kuwei2.png';
const submitLoading=ref(false);
const isSearchBin=ref(false); 

onMounted(()=>{
    setTimeout(() => {
        document.getElementById('input-carSoleCode')?.focus(); 
    }, 500); 
})
  
const onScanCode=(e:any)=>{  
 e.preventDefault(); 
 if(e.keyCode==13&&dataForm.value.carSoleCode){
    getProductionOrderInfo(dataForm.value.carSoleCode).then(res=>{
        dataForm.value=res.data;  
        dataForm.value.actualTotal=dataForm.value.planTotalByCar;
        onSearchBin();
    }).catch(()=>dataForm.value.carSoleCode='');  
 }
}

const onSearchBin=()=>{ 
    dataForm.value.binNo='正在查询空余库位...'; 
    getFreeBin(dataForm.value.carSoleCode).then(res=>{
        isSearchBin.value=true;
        dataForm.value.binId=res.data.binId;
        dataForm.value.binNo=res.data.binNo;  
        document.getElementById('input-actualTotalByCar')?.focus(); 
    }) 
}

const onLockBinCancel=()=>{
    msgDrawerOptions.value.show=false;
}

// const onLockBinConfirm=()=>{
//     msgDrawerOptions.value.show=false;
//     lockBin(dataForm.value).then(res=>{
//         msg.deftAuto(`${dataForm.value.binNo}已锁定，请确认装车后配送到指定库位`);
//         document.getElementById('input-actualTotalByCar')?.focus(); 
//     }) 
// }

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
    document.getElementById('input-carSoleCode')?.focus();  
}

const onShowLockBinInfo=()=>{
    msgDrawerOptions.value.show=true;
    msgDrawerOptions.value.type='Message'
    msgDrawerOptions.value.title='装车提示';
    msgDrawerOptions.value.message=`本次上架推荐库位：${dataForm.value.binNo}，是否确认装车并锁定该库位？`;
}

const onSubmit=()=>{ 
    if(!dataForm.value.carSoleCode){
        msg.deftAuto('请扫描小车唯一码');
        document.getElementById('input-carSoleCode')?.focus();  
        return;
    }
    if(!dataForm.value.actualTotal){
        msg.deftAuto('请输入装车数量');
        document.getElementById('input-actualTotalByCar')?.focus();  
        return;
    }
    if(Number(dataForm.value.actualTotal)<0){
        msg.deftAuto('装车数量不能小于0');
        document.getElementById('input-actualTotalByCar')?.focus();  
        return;
    }
    if(Number(dataForm.value.actualTotal)>Number(dataForm.value.total)){
        msg.deftAuto('装车数量不能大于生产数量');
        document.getElementById('input-actualTotalByCar')?.focus();  
        return;
    }
    if(!isSearchBin.value){
        msg.deftAuto('请先查询空余库位');
        return;
    } 
    
    submitLoading.value=true; 
    dataForm.value.createUser=permission.getOperator().userName;
    submitCarLoad(dataForm.value).then(res=>{
        msgDrawerOptions.value.show=false;
        onClearForm();
    }).finally(()=>{submitLoading.value=false;})
}
 

</script>

<style lang="scss" scoped>
@media screen and (max-width: 450px){
    .app-content{
        height: 88%;
        background-color: #fff;
       .content-main{
            padding :0px 30px 0 25px;
            height: 85%;
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
        height: 88%;
        background-color: #fff;
       .content-main{
            padding :0px 30px 0 25px;
            height: 85%;
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