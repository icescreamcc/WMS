<template>
    <div class="app-content">
        <div class="content-main">
            <div class="content-form">
            <el-form  :model="dataForm"  ref="dataFormRef"  label-width="auto" label-position="left" style="padding:0 15px">
              <div> 
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">{{ invTitle }}名称</div>
                    </el-col>
                    <el-col :span="14">
                        <input type="text" v-model="dataForm.goodsName" readonly class="input-cls" @click="onShowGoodsList" placeholder="点击查询并选择入库物品"> 
                    </el-col>
                    <el-col :span="2"> 
                       <div class="input-icon">
                        <img src="../../../../public/icon-img/dianji.png" alt="" height="20"> 
                       </div>
                    </el-col>
                </el-row> 
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">入库单位</div>
                    </el-col>
                    <el-col :span="14"> 
                        <input type="text" v-model="dataForm.unitName" readonly class="input-cls" @click="onShowtUnits" placeholder="选择入库单位">  
                    </el-col>
                    <el-col :span="2"> 
                       <div class="input-icon">
                        <img src="../../../../public/icon-img/dianji.png" alt="" height="20"> 
                       </div>
                    </el-col>
                </el-row>
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">{{ invTitle }}型号</div>
                    </el-col>
                    <el-col :span="16">
                        <input readonly v-model="dataForm.goodsModel"   class="input-cls"> 
                    </el-col> 
                </el-row> 
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">SAP编码</div>
                    </el-col>
                    <el-col :span="16">
                        <input type="text" v-model="dataForm.goodsNo" readonly class="input-cls"> 
                    </el-col>
                </el-row> 
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">入库数量</div>
                    </el-col>
                    <el-col :span="16">
                        <input type="text" v-model="dataForm.quantity" readonly class="input-cls"> 
                    </el-col>
                </el-row>
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">推荐货位</div>
                    </el-col>
                    <el-col :span="16">
                        <input type="text" :value="recommendBin" readonly class="input-cls"> 
                    </el-col>
                </el-row>
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">标签码</div> 
                    </el-col>
                    <el-col :span="14"> 
                        <input type="text" v-model="dataForm.codeString"  @focus="commonHelper.stopKeyborad($event)" id="input-code" placeholder="扫描标签码" autofocus class="input-cls">  
                    </el-col>
                    <el-col :span="2"> 
                       <div class="input-icon">
                        <img :src="scanImg" alt="" height="20"> 
                       </div>
                    </el-col>
                </el-row>
                <el-row class="input-item">
                    <el-col :span="8">
                        <div class="input-title">库位码</div> 
                    </el-col>
                    <el-col :span="14"> 
                        <input type="text" v-model="dataForm.scanNo" :disabled="!isScanNo" @keyup.enter="onScanBinCode($event)" @focus="commonHelper.stopKeyborad($event)" id="input-binNo" placeholder="扫描库位码（可跳过）" class="input-cls">  
                    </el-col>
                    <el-col :span="2"> 
                       <div class="input-icon">
                        <img :src="scanImg" alt="" height="20"> 
                       </div>
                    </el-col>
                </el-row> 
              </div> 
            </el-form>
            </div>
            <div class="content-desc">
               <div style="padding: 10;"> 
                <el-row gutter="15"> 
                   <el-col :xs="12" :sm="6" :md="6" :lg="6" :xl="6" v-for="(item,index) in unSubmitCodeData">
                    <div class="desc-item">
                        <span class="iconfont icon-qrcode_fill item-icon"></span><span class="item-info">{{ item }}</span>
                    </div> 
                   </el-col>   
                </el-row>
               </div>
            </div> 
        </div> 
        <div class="content-btn">
            <div v-if="permission.isPermisstion('SCANINSTORAGESUBMITSCAN')">
                <el-button  type="success" round  @click="onScanCode" style="width:82%;" :loading="submitLoading">扫码确认</el-button> 
            </div>
            <div v-if="permission.isPermisstion('SCANINSTORAGESUBMITCODE')" style="margin-top: 6px;">
                <el-button  round type="primary" @click="onSubmit" style="width:82%;" :loading="submitLoading">提交入库</el-button>
            </div>
            <div style="margin-top: 6px;">
                <el-button  round  @click="onClearForm" style="width:82%;" >重置信息</el-button>
            </div>
        </div> 
        <MessageDrawer :options="messageTypes" v-if="messageTypes.show" @cancel="onSelectUnitCancel" @confirm="onSelectUnitConfirm"/>
        </div> 
</template>

<script lang="ts" setup>
import {  ref ,nextTick,onMounted} from "vue";
import { useI18n } from 'vue-i18n'   
import { ElForm } from 'element-plus'   
import {InfoFilled} from '@element-plus/icons-vue';   
import commonHelper from '@/utils/system/common-helper';
import msg from "@/utils/system/message" 
import permission from '@/utils/system/permission'; 
import { deftClassifyGroup } from '@/config'
import { useRouter, useRoute } from 'vue-router';  
import {getUnSubmitCodes,getWorkbinRecommend,scanBinCheck,submitScan,submitCode} from "@/api/inv/scan-instorage";
import MessageDrawer from '../message-drawer.vue'

const router = useRouter(); 
const route=useRoute();
const { t } = useI18n()
const invTitle=ref('物品');
const messageTypes=ref({
    show:false,
    title:'',
    message:'',
    type:'',
    data:Array<any>()
});
const dataForm = ref({
    codeString:'',
    scanNo:'',
    goodsId:route.params?.goodsId,
    goodsNo:route.params?.goodsNo,
    goodsName:route.params?.goodsName,
    goodsModel:route.params?.goodsModel,
    goodsClassifyName:route.params?.goodsClassifyName,
    goodsClassifyGroup:deftClassifyGroup||route.params?.goodsClassifyGroup,
    goodsPicture:route.params?.goodsPicture, 
    quantity:0, 
    unitId:'',
    unitName:'',
    createUserId:'',
    createUserName:''
  });  
const dataFormRef = ref(ElForm||null); 
const scanImg='/public/icon-img/saoma2.png';
const submitLoading=ref(false); 
const unSubmitCodeData=ref(new Array<any>());
const recommendBin=ref('');
const unitData=ref(new Array<any>()); 
const isScanNo=ref(true);

onMounted(()=>{
    if(dataForm.value.goodsClassifyGroup){
        if(dataForm.value.goodsClassifyGroup=="SparePart"){ 
        invTitle.value='备件';
        } 
        else if(dataForm.value.goodsClassifyGroup=="SamplePiece"){ 
        invTitle.value='样件';
        } 
        else if(dataForm.value.goodsClassifyGroup=="PackingMaterial"){ 
        invTitle.value='包材';
        } 
        else if(dataForm.value.goodsClassifyGroup=="Separator"){ 
        invTitle.value='辅材';
        } 
    }
    document.getElementById('input-code')?.focus();  
    unitData.value.length=0;
    if(route.params?.goodsId){ 
        if(!unitData.value.find(f=>f.key==route.params.packageUnitId)){
            unitData.value.push({key:route.params.packageUnitId,value:route.params.packageUnitName,remark:true});
            dataForm.value.unitId=route.params.packageUnitId.toString();
            dataForm.value.unitName=route.params.packageUnitName.toString();
        }
        if(!unitData.value.find(f=>f.key==route.params.minPackageUnitId)){
            unitData.value.push({key:route.params.minPackageUnitId,value:route.params.minPackageUnitName});
        }
        if(!unitData.value.find(f=>f.key==route.params.maxPackageUnitId)){
            unitData.value.push({key:route.params.maxPackageUnitId,value:route.params.maxPackageUnitName});
        }  
        isScanNo.value=true;
        getUnSubmitCodeData(route.params.goodsId);
        getWorkbinRecommendData(route.params.goodsId);
    }
})

const getUnSubmitCodeData=(goodId:any)=>{
    dataForm.value.quantity=0;
    getUnSubmitCodes(goodId).then(res=>{
        if(res.data?.length>0){ 
            isScanNo.value=false;
            dataForm.value.quantity=res.data.length;
            unSubmitCodeData.value=res.data.map((m:any)=>m.codeString); 
            let units=  res.data.map((m:any)=>{
                if(m.unitId)
                return {
                    unitId:m.unitId,
                    unitName:m.unitName
                }
            });
            console.log('units',units)
            if(units?.length>0){
                let deftUnit=units[0];
                dataForm.value.unitId=deftUnit.unitId;
                dataForm.value.unitName=deftUnit.unitName;
                unitData.value.forEach(f=>{
                    if(f.key==deftUnit.unitId){
                        f.remark=true;
                    }
                })
            }
            console.log('unitData.value',unitData.value)
        } 
    })
}

const getWorkbinRecommendData=(goodId:any)=>{
    getWorkbinRecommend(goodId).then(res=>{
        if(res.data?.length>0){
            recommendBin.value= res.data.map((obj:any)=>{
                if(obj.cellNo){
                    return obj.cellNo;
                }
                else if(obj.binNo){
                    return obj.binNo;
                }
            }).join(',');
        }
    });
}

const onShowGoodsList=()=>{
    let routeName='select-instorage-goods';
    router.push({name:routeName,params:{goodsId:dataForm.value.goodsId,goodsClassifyGroup:dataForm.value.goodsClassifyGroup}}).then(()=>{
        permission.addRouteHis(routeName); 
    });
}

const onShowtUnits=()=>{
    if(route.params?.goodsId){ 
        messageTypes.value.data=unitData.value;
        messageTypes.value.title="选择入库单位";
        messageTypes.value.type="SingleSelect";
        messageTypes.value.show=true;
    } 
}

const onSelectUnitCancel=()=>{
    messageTypes.value.show=false;
}

const onSelectUnitConfirm=(selectedUnitId:any)=>{
    dataForm.value.unitId=selectedUnitId;
    dataForm.value.unitName=unitData.value.find(f=>f.key==selectedUnitId).value;
    messageTypes.value.show=false;
}

const onScanCode=()=>{  
 if(dataForm.value.codeString){  
    if(!dataForm.value.goodsId) {
        msg.deftAuto(`请选择入库${invTitle.value}`);
        return;
    }
    if(!dataForm.value.unitId){
        msg.deftAuto(`请选择入库单位`);
        return;
    }
    dataForm.value.createUserId=permission.getOperator().userId;
    dataForm.value.createUserName=permission.getOperator().userName;
    dataForm.value.unitId=dataForm.value.unitId?dataForm.value.unitId:'0';
    submitLoading.value=true;
    submitScan(dataForm.value)
    .then(()=>{
        unSubmitCodeData.value.unshift(dataForm.value.codeString);
        dataForm.value.quantity=unSubmitCodeData.value.length; 
    })
    .finally(()=>{
        dataForm.value.codeString='';
        submitLoading.value=false;
        document.getElementById('input-code')?.focus();
    }) 
 }
}

const onScanBinCode=(e:any)=>{ 
 e.preventDefault(); 
 if(e.keyCode==13&&dataForm.value.codeString&&dataForm.value.scanNo){ 
    scanBinCheck(dataForm.value.scanNo,dataForm.value.goodsId.toString()).then((res=>{
        msg.deftAuto('货位验证成功');
    })).catch((err)=>dataForm.value.scanNo=''); 
 }
}

const onSubmit=()=>{ 
    if(!dataForm.value.goodsId){
        msg.deftAuto(`请选择入库的${invTitle.value}`);
        return;
    }
    if(!dataForm.value.unitId){
        msg.deftAuto(`请选择入库单位`);
        return;
    }
    dataForm.value.createUserId=permission.getOperator().userId;
    dataForm.value.createUserName=permission.getOperator().userName;
    dataForm.value.unitId=dataForm.value.unitId?dataForm.value.unitId:'0';
    submitLoading.value=true;
    submitCode(dataForm.value)
    .then(()=>{

    })
    .finally(()=>{
        submitLoading.value=false;
        document.getElementById('input-binNo')?.focus();  
    })
    // if(!dataForm.value.carSoleCode){
    //     msg.deftAuto('请扫描或输入小车唯一码');
    //     document.getElementById('input-code')?.focus();  
    //     return;
    // }
    // if(!dataForm.value.binNo){
    //     msg.deftAuto('请扫描或输入库位码');
    //     document.getElementById('input-binNo')?.focus();  
    //     return;
    // }  
    // submitLoading.value=true;
    // dataForm.value.createUser=permission.getOperator().userName;
    // submitPutaway(dataForm.value).then(res=>{ 
    //     onClearForm();
    // }).catch(()=>dataForm.value.scanBinNo='').finally(()=>{ submitLoading.value=false;}) 
}
  
const onClearForm=()=>{
    dataForm.value.codeString='';
    dataForm.value.scanNo='';
    dataForm.value.goodsId='';
    dataForm.value.goodsNo='';
    dataForm.value.goodsName='';
    dataForm.value.goodsModel='';
    dataForm.value.goodsClassifyName=''; 
    dataForm.value.goodsPicture='';
    dataForm.value.quantity=0; 
    dataForm.value.unitId='';
    dataForm.value.unitName='';
    document.getElementById('input-code')?.focus();  
}
</script>

<style lang="scss" scoped>
@media screen and (max-width: 450px){
    .app-content{
        height: 87%;
        background-color:rgb(247, 252, 252);
        position: relative;
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
                overflow: hidden;  
                white-space: nowrap;  
                text-overflow: ellipsis; 
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
                height: 22%;
                text-align: left;
                padding: 0 10px; 
                overflow-y: scroll; 
                position: relative;
                top: 20px;
                .desc-item{
                    background:#f3f3f361;
                    padding: 1px 3px; 
                    border-radius: 2px;
                    margin-bottom: 3px; 
                    overflow: hidden;  
                    white-space: nowrap;  
                    text-overflow: ellipsis; 
                    .item-icon{
                        background:#fff;
                        color:#a6a5a5;
                        font-size: 14px; 
                    }
                    .item-info{ 
                        color:#a6a5a5;
                        font-size: 12px; 
                        margin-left: 3px;
                    } 
                } 
            } 
       }
       .content-btn{
            height: 18%; 
            position: fixed;
            width: 100%;
            bottom: 5%;
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
                overflow: hidden;  
                white-space: nowrap;  
                text-overflow: ellipsis;
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
                width: 110%;
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
                height: 50%;
                text-align: left;
                padding: 0 10px;
                overflow-y: scroll;
                position: relative;
                top: 20px;
                .desc-item{
                    background:#f3f3f361;
                    padding: 1px 3px; 
                    border-radius: 2px;
                    margin-bottom: 3px; 
                    overflow: hidden;  
                    white-space: nowrap;  
                    text-overflow: ellipsis; 
                    .item-icon{
                        background:#fff;
                        color:#a6a5a5;
                        font-size: 14px; 
                    }
                    .item-info{ 
                        color:#a6a5a5;
                        font-size: 12px; 
                        margin-left: 3px;
                    } 
                } 
            } 
       }
       .content-btn{
            height: 10%; 
            position: fixed;
            width: 100%;
            bottom: 6%;
        }
    }
} 
</style>