<template>
    <div class="content"> 
       <div class="content-header">
           <el-row >
               <el-col :span="8" class="header-logo"><img src="@/assets/header-logo-b.png" alt=""></el-col>
               <el-col :span="8"  class="header-title"><h3>仓库拣货看板 {{ invTitle }}</h3></el-col>
           </el-row> 
       </div>
       <div class="content-body" > 
           <div class="body-form"> 
            <el-row  :gutter="20">
                <el-col :span="6">
                <el-select v-model="selectedGoodsGroup" :disabled="disableClassifyGroupSelect" class="m-2" style="width:100%" @change="onSelectGroup">
                        <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">大类</div></template>
                        <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
                  </el-select>
              </el-col>
              <el-col :span="6">
                <el-select v-model="selectedGoodsClassifyId"  class="m-2" style="width:100%" @change="getPickData">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">小类</div></template>
                         <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                        </el-option>
                </el-select>
              </el-col>  
              <el-col :span="6">
                  <el-input v-model="goodsSearchKey" placeholder="可按关键字检索" >
                  <template #append>
                    <el-button type="primary" @click="getPickData"><el-icon style="position: relative;top:4px;font-size: 15px;"><Search /></el-icon></el-button>
                  </template>
                </el-input>
              </el-col>
              <el-col :span="6" style="text-align: center;vertical-align: middle;margin-top: 5px;">
                 <el-checkbox v-model="isAuto" label="自动查询" name="type"  size="large" @change="modelChanged" /> 
              </el-col>
            </el-row>  
           </div>
           <div class="body-content" >   
            <el-table class="order-table" :data="goodsData" stripe height=750>
                   <el-table-column prop="typeName" label="类型" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column prop="goodsName" label="名称" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column prop="goodsNo" label="SAP编码" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column prop="goodsModel" label="型号" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column prop="quantity" label="拣货数量" align="center" :show-overflow-tooltip="true">
                    <template #default="scope"> 
                       <span>{{ scope.row.quantity+ scope.row.unitName}}</span> 
                   </template>
                   </el-table-column> 
                   <el-table-column prop="line" label="需求产线" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column prop="createDate" label="创建时间" align="center" :show-overflow-tooltip="true">
                       <template #default="scope"> 
                       <span>{{ commonHelper.formatToDateTime( scope.row.createDate)}}</span> 
                   </template>
                   </el-table-column>   
                   <el-table-column prop="createUserName" label="创建人" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column align="center">
                    <template #default="scope"> 
                        <el-button type="success" @click="showStorageDetail(scope.row)">查看库存</el-button>
                    </template>
                    </el-table-column>
               </el-table> 
           </div>  
       </div>    
       <StorageDetailLayer :layer="detailLayer"  v-if="detailLayer.show" />  
   </div>
</template>

<script setup lang="ts"> 
import { reactive, ref,onMounted,onBeforeUnmount } from "vue";
import { useI18n } from 'vue-i18n';    
import commonHelper from '@/utils/system/common-helper';
import { Search,PictureFilled } from '@element-plus/icons-vue';
import {getRequisitionData} from '@/api/inv/warehouse-pick-dashboard';  
import {getGoodsGroup,getGoodsClassify} from '@/api/common';   
import { deftClassifyGroup ,disableClassifyGroupSelect} from '@/config'; 
import {getStorageDetails} from "@/api/inv/storage";
import StorageDetailLayer from './storageDetail.vue';
import { ElStep } from "element-plus";

const goodsGroupData=ref(new Array<any>()); 
const selectedGoodsGroup=ref(deftClassifyGroup); 
const goodsClassifyData=ref([{key:0,value:'All'}]); 
const selectedGoodsClassifyId=ref(0); 
const goodsSearchKey=ref('');
const goodsData=ref(new Array<any>()); 
const invTitle=ref('');
const detailLayer = reactive({
    show: false,
    title: "",
    showButton: false,
    btnLoading:false,
    width:"60%",
    data:null ,
    otherButton:{ }
});  
const voices=ref(new Array<any>());
const isAuto=ref(true);
var autoSearchHandle:any=0;

onMounted(()=>{ 
    voices.value = speechSynthesis.getVoices();   
    getGoodsGroupData(); 
    getGoodsClassifyData();
    autoSearch();
});

const modelChanged=(val:any)=>{ 
    if(!isAuto.value){
        clearInterval(autoSearchHandle);
    }
    else{
        autoSearch();
    }
}

const autoSearch=()=>{
    autoSearchHandle=setInterval(()=>{
        getPickData();
    },8000)
}
 
const getGoodsGroupData=()=>{
   return getGoodsGroup().then(res=>{
        goodsGroupData.value=res.data;
        if(selectedGoodsGroup.value){
            invTitle.value=goodsGroupData.value.find(f=>f.key==selectedGoodsGroup.value).value;
        } 
    })
}

const onSelectGroup=()=>{  
    invTitle.value=goodsGroupData.value.find(f=>f.key==selectedGoodsGroup.value).value;
    getGoodsClassifyData();
    getPickData();
}
  
const getGoodsClassifyData=()=>{
    if(selectedGoodsGroup.value){
         getGoodsClassify(selectedGoodsGroup.value).then(res=>{
            goodsClassifyData.value=[...res.data];
        })
    } 
}
 
const getPickData=()=>{  
    if(selectedGoodsGroup.value){
        getRequisitionData(selectedGoodsGroup.value,selectedGoodsClassifyId.value,goodsSearchKey.value).then(res=>{ 
            if(res.data.length>0){ 
                if(res.data.length!= goodsData.value.length){
                    speakText();
                }
                else{
                    for(let i=0;i<res.data.length;i++){
                        if(!goodsData.value.find((f:any)=>f.orderNo==res.data[i].orderNo)){
                            speakText();
                            break;
                        }
                    }
                } 
            }
            goodsData.value=res.data;   
        })
    } 
 } 

const showStorageDetail=(row:any)=>{
    getStorageDetails(row.goodsId).then(res=>{
        detailLayer.data=res.data;
        detailLayer.show=true;
    })
}

const speakText=()=>{  
        let text="有新的领料订单需要拣货"; 
        const utterance = new SpeechSynthesisUtterance(text);
        if(voices.value.length==0){
            voices.value = speechSynthesis.getVoices();  
        } 
        utterance.lang = 'zh-CN'; 
        let  voicesCN= voices.value.filter(f=>f.lang=='zh-CN');  
        let selectedVoice = voicesCN.find(voice => voice.name == "Microsoft Xiaoyi Online (Natural) - Chinese (Mainland)"); 
        if (selectedVoice) {
            utterance.voice = selectedVoice; 
            speechSynthesis.speak(utterance);
            let index=1;
            var handle=setInterval(()=>{
                if(index>0){
                    speechSynthesis.speak(utterance);
                }
                if(index==0){
                    clearInterval(handle);
                }
                index--;
            },5000)
        } 
    }
 
</script>
<style lang="scss" scoped>   
.content{
       background-color: #fff; 
       height: 100%; 
      
       .content-header{ 
               text-align: center;
               height:4%;
               background-color:#f7a500;
               color: #fff;
               padding: 1px 0 15px 0; 
               .header-logo{ 
                   text-align: left;
                   padding: 8px 10px;
                   img{
                       height: 40%;
                   }
               }
               .header-title{
                   text-align: center; 
                   line-height: 100%;
               }
           }
       .content-body{  
           background-image: url('../../../assets/images/lingliao_bg4.jpg');
           background-position: 100%;
           background-size: cover;
           background-repeat: no-repeat;
           height:94%;
           background-color: #efefef; 
           overflow: auto; 
           padding:0 .5%;   
           .body-form{ 
               padding: 10px;
               background:rgba(255, 255, 255, 0.304); 
           }
           .body-content{ 
               margin-top: 10px;
               padding: 10px 10px;
               background:rgba(255, 255, 255, 0.404);  
               height: 90%;
               overflow-y:scroll;
               position: relative;
               .order-table{
                   background:rgba(255, 255, 255, 0.36);
                  :deep .el-table__row{
                       background:rgba(255, 255, 255, 0.36);
                   }
                }
           }  
       } 
   }
</style>