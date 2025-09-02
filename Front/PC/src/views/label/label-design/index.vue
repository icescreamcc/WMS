<template>
    <div class="layout-container">
      <div class="layout-container-form flex space-between">
        <div class="layout-container-form-handle">
          <el-button type="primary" v-if="permission.isPermisstion('LABELDESIGNADD')" icon="el-icon-circle-plus-outline" @click="onAddLabelTemplet">新增</el-button>
        </div>
        <div class="layout-container-form-search">
          <el-select v-model="selectedGoodsGroup" :disabled="disableClassifyGroupSelect" size="small"  class="m-2" style="width:100%;margin-right:10px;" @change="getLabelDesignData" placeholder="选择物品大类">
          <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">标签分类</div></template>        
          <el-option label="All" value=""></el-option>
          <el-option v-for="item in goodsClassifyGroupData" :label="item.value" :value="item.key">
                    </el-option>
          </el-select>
          <el-input 
            v-model="searchkey"
            placeholder="请输入关键词进行检索"
            size="small"
          ></el-input>
          <el-button
            type="primary"
            icon="el-icon-search"
            class="search-btn"
            @click="getLabelDesignData"
            >搜索</el-button>  
        </div>
      </div>
      <div class="layout-container-table">
       <div gutter="20" class="container-bg">
          <div class="container-scroll">
            <div class="container-item" v-for="label in labelData">
              <div class="item-desc">
                <div class="label-content" :style="{width:standardLayoutWidth+'mm',height:standardLayoutHeight+'mm'}"  @click="onShowLabelInfo(label)">
                  <div class="label-background" :style="{width:label.widthScale+'mm',height:label.heightScale+'mm',backgroundColor:label.backgroundColor}">
                    <div class="label-item" v-for="item in label.details">
                      <div v-if="item.isUsed&&item.itemType=='Text'" :style="{color:item.args.color,fontFamily:item.args.font,fontSize:item.args.fontSize+'px',fontWeight:item.args.weight?'bold':'normal',position:'absolute',left:item.style.leftPercent+'%',top:item.style.topPercent+'%', whiteSpace:'nowrap',overflow:'hidden'}">{{item.showName? `${item.itemName}：`:'' }}{{item.deftValue||'xxxxxx'}}</div>
                      <div v-else-if="item.isUsed&&item.itemType=='QRCode'" :style="{height:item.args.height+'mm',width:item.args.width+'mm',position:'absolute',left:item.style.leftPercent+'%',top:item.style.topPercent+'%'}"> 
                          <canvas v-if="item.args.format=='DataMatrix'" :id="label.labelId+'_'+item.itemName" style="width: 100%;height: 100%;"></canvas>
                          <VueQr v-else-if="item.args.format=='QRCode'" :id="label.labelId+'_'+item.itemName" :text="(label.labelId+'_'+item.itemName).toUpperCase()" :size="item.args.width" :dotScale="1" :margin="0"></VueQr>
                      </div>
                      <div v-else-if="item.isUsed&&item.itemType=='BarCode'" :style="{height:item.args.height+'mm',position:'absolute',left:item.style.leftPercent+'%',top:item.style.topPercent+'%'}">
                          <img :id="label.labelId+'_'+item.itemName">
                      </div>
                    </div>
                  </div> 
                </div>
                <div class="label-info">
                  <span>{{`${label.labelTitle}-${label.labelName}(${label.width}*${label.height})` }}</span> 
                 <div class="info-del-btn">
                  <el-popconfirm v-if="permission.isPermisstion('LABELDESIGNDEL')" title="是否确认删除该标签模板？" @confirm="onDelete(label)">
                    <template #reference>
                      <img src="../../../../public/icon-img/shanchu8.png" title="删除当前标签模板">
                    </template>
                  </el-popconfirm>
                 </div> 
                 <div class="info-vaild-btn" title="默认标签模板" v-if="permission.isPermisstion('LABELDESIGNUPDATEDEFT')">
                  <img src="../../../../public/icon-img/qiyong22.png" style="cursor: default;" v-if="label.isDeft" >
                  <img src="../../../../public/icon-img/qiyong11.png" v-else @click="onUpdateDeft(label)"> 
                 </div>
                </div>
              </div>
            </div>  
          </div>  
       </div>
      </div>
      <LabelInfoEdit  :layer="labelInfoLayer" v-if="labelInfoLayer.show" @dataSubmit="onSubmitLabelInfo"/>
    </div>
  </template>
  
  <script lang="ts" setup>
    defineOptions({
    name: "label-design"
  })
  import {  ref, reactive,onMounted,nextTick } from "vue"; 
  import { getLabelDesign,getLableDesignDetails,addLabelDesign,updateLabelDesign,updateLabelDeft,delLabelDesign} from "@/api/baseinfo/labelDesign";
  import { LayerInterface } from "@/components/layer/index.vue";   
  import permission from '@/utils/system/permission'
  import JsBarcode from 'jsbarcode';    
  import VueQr from 'vue-qr/src/packages/vue-qr.vue';
  import LabelInfoEdit from "./labelInfoEdit.vue";
  import * as bwipjs from 'bwip-js';
  import { getGoodsGroup } from "@/api/common";
  import { deftClassifyGroup ,disableClassifyGroupSelect} from '@/config'

  const orderFiled=ref('GoodsClassifyGroup');
  const orderType=ref('desc');
  const searchkey=ref('');
  const labelData=ref(new Array<any>()); 
  const standardLayoutWidth=ref(100);
  const standardLayoutHeight=ref(50);
  const goodsClassifyGroupData=ref(new Array<any>()); 
  const selectedGoodsGroup=ref(deftClassifyGroup||''); 
  const labelInfoLayer: LayerInterface = reactive({
      show: false,
      title: "",
      showButton: true,
      btnLoading:false,
      width:"58%",
      data:null ,
      otherButton:{ }
    }); 

  onMounted(()=>{ 
    getGoodsGroupData().then(()=>{
      getLabelDesignData();  
    }) 
  })

  const getGoodsGroupData=()=>{
   return getGoodsGroup().then(res=>{
      goodsClassifyGroupData.value=res.data;
    }) 
  }

  const getLabelDesignData=()=>{
   return getLabelDesign(orderFiled.value,orderType.value,selectedGoodsGroup.value,searchkey.value).then(res=>{
    if(res.data?.length>0){
      res.data.forEach((label:any) => {
          setLableName(label);
          layoutContentEqualScaling(label);
          parseDetailArgs(label.details); 
          itemContentEqualScaling(label.widthScalingRatio,label.heightScalingRatio,label.details)
        });
      } 
      labelData.value=res.data;
      nextTick(()=>{
       labelData.value.forEach((label:any)=>{
        label.details.forEach((item:any) => {
          setBarcode(label.labelId,item);
        });
       })
      }) 
    })
  }

  const layoutContentEqualScaling=(label:any)=>{
    label.widthScale=label.width;
    label.heightScale=label.height;
    let whRatio=label.width/label.height;
    if(label.width>standardLayoutWidth.value){
      label.widthScale=standardLayoutWidth.value;
      label.heightScale=label.widthScale/whRatio; 
    }
    else if(label.height>standardLayoutHeight.value){
      label.heightScale=standardLayoutHeight.value;
      label.widthScale= label.heightScale*whRatio;
    } 
    label.widthScalingRatio=label.widthScale/label.width;
    label.heightScalingRatio=label.heightScale/label.height;
  }

  const itemContentEqualScaling=(widthScalingRatio:number,heightScalingRatio:number,details:Array<any>)=>{
    details.forEach((item:any)=>{ 
      if(item.itemType=='BarCode'){
     
      }
      else if(item.itemType=='QRCode'){
        item.args.widthScale=item.args.width*widthScalingRatio;
        //item.args.heightScale=item.args.height*heightScalingRatio;
        item.args.heightScale=item.args.widthScale;
      }
      item.style.leftPercentScale=item.style.leftPercent*widthScalingRatio;
      item.style.topPercentScale=item.style.topPercent*heightScalingRatio;
    }); 
  }

  const parseDetailArgs=(details:Array<any>)=>{
    details.forEach((detail:any)=>{
      detail.args=JSON.parse(detail.args);
      if(detail.itemValueType=='BindingField'){
        detail.itemValueField=JSON.parse(detail.itemValueField); 
      } 
      detail.itemValueFieldInfo=JSON.parse(detail.itemValueFieldInfo);
      detail.style=JSON.parse(detail.style); 
    });
  }

  const setLableName=(label:any)=>{ 
    label.labelTitle= goodsClassifyGroupData.value.find((f:any)=>f.key==label.goodsClassifyGroup).value;
  }

  const setBarcode=(labelId:string,item:any)=>{
    if(item.itemType=='BarCode'){
        let barcodeArgs=item.args;
        let barcodeDomId=`${labelId}_${item.itemName}`;
        let deftText=item.deftValue||barcodeDomId.toLocaleUpperCase(); 
        createBarcode(barcodeDomId,deftText,barcodeArgs.format,barcodeArgs.width,barcodeArgs.height,barcodeArgs.lineColor,barcodeArgs.displayValue,barcodeArgs.font,barcodeArgs.fontSize,barcodeArgs.textMargin);
    }
    else if(item.itemType=='QRCode'){
        let qrcodeArgs=item.args;
        if(qrcodeArgs.format=='DataMatrix'){
            let qrcodeId=`${labelId}_${item.itemName}`;
            let deftText=item.deftValue||qrcodeId.toLocaleUpperCase();
            createDataMatrix(qrcodeId,deftText);
        }
        else{ 
        }
    }
 }

  const createBarcode=(imgId:string, value:string,format:string,width:number,height:number,lineColor:string,displayValue:boolean,font:string,fontSize:number,textMargin:number)=>{ 
    JsBarcode("#"+imgId,value,{
        format:format,
        width:width,
        height:height,
        lineColor:lineColor, 
        displayValue:displayValue, 
        margin:0, 
        font:font,
        fontSize:fontSize,
        textMargin:textMargin
    })
 }

const createDataMatrix=(canvasId:string,value:string)=>{
    let opt={
        bcid: 'datamatrix',   
        text: value,
        scale: 3,
        includetext: true,
    };
    bwipjs.toCanvas(canvasId,opt); 
}

const onDelete=(label:any)=>{
  delLabelDesign(label.labelId);
}

const onUpdateDeft=(label:any)=>{ 
  updateLabelDeft(label.labelId,label.goodsClassifyGroup).then(()=>{
    getLabelDesignData();
  }) 
}

const onAddLabelTemplet=()=>{
  delete labelInfoLayer.data;
  labelInfoLayer.show=true;
}

const onShowLabelInfo=(label:any)=>{ 
  labelInfoLayer.data=label;
  labelInfoLayer.show=true;
}

const onSubmitLabelInfo=(data:any,type:string)=>{
  labelInfoLayer.btnLoading=true;
  if(type=='add'){
    addLabelDesign(data).then(()=>{
      labelInfoLayer.show=false;
      getLabelDesignData();
    }).finally(()=>labelInfoLayer.btnLoading=false);
  }
  else{
  updateLabelDesign(data).then(()=>{
    labelInfoLayer.show=false;
    getLabelDesignData();
  }).finally(()=>labelInfoLayer.btnLoading=false);
  }
}
  </script>
  
  <style lang="scss" scoped> 
 

  @media screen and (max-width: 1530px){
    .container-bg{
      background-color: rgb(231, 243, 243);
      height: 95%;
      padding: 10px 5px;
      .container-scroll{
        height: 96%;
        overflow-y: scroll;
        padding:10px 0;
        .container-item{ 
          float: left; 
          margin-left: 11%;
          margin-top: 1%; 
          .item-desc{ 
            border-radius: 6px;
            padding: 10px;
            background-color: rgb(0, 85, 85);
            .label-content{
              background-color: rgb(208, 242, 217);
              position: relative; 
              border-radius: 3px; 
              cursor: pointer;
              .label-background{ 
                overflow: hidden;
                position: absolute;
                left: 50%; 
                top:50%;
                transform: translate(-50%,-50%);
              }
            }
            .label-info{
              margin-top: 10px;
              font-size: 14px;
              font-weight: 600;
              color: #fff;
              position: relative; 
              .info-del-btn{
                position: absolute;right:10%;bottom: 0%;
                cursor: pointer;
                display: grid;
                place-items: center; 
                img{
                  height: 20px; 
                }
              }
              .info-vaild-btn{
                position: absolute;right:1%;bottom: 0%;
                cursor: pointer;
                display: grid;
                place-items: center; 
                img{
                  height: 20px; 
                }
              } 
            }
          }
        }
        .container-item:hover{
          box-shadow: 2px 2px 5px #888;
          border-radius:6px;
          }
      } 
    } 
  }

  @media screen and (min-width: 1530px){
    .container-bg{
      background-color: rgb(246, 248, 248);
      height: 95%;
      padding: 10px 5px; 
      .container-scroll{
        height: 96%;
        overflow-y: scroll;
        padding:10px 0; 
        .container-item{ 
          float: left; 
          margin-left: 5%;
          margin-top: 1%; 
          .item-desc{ 
            border-radius: 6px;
            padding: 10px;
            background-color: rgb(0, 85, 85);
            .label-content{
              background-color: rgb(208, 242, 217);  
              border-radius: 3px;
              position: relative;  
              cursor: pointer; 
              .label-background{ 
                overflow: hidden;
                position: absolute;
                left: 50%; 
                top:50%;
                transform: translate(-50%,-50%);
              }
            }
            .label-info{
              margin-top: 10px;
              font-size: 14px;
              font-weight: 600;
              color: #fff;
              position: relative; 
              .info-del-btn{
                position: absolute;right:10%;bottom: 0%;
                cursor: pointer;
                display: grid;
                place-items: center; 
                img{
                  height: 20px; 
                }
              }
              .info-vaild-btn{
                position: absolute;right:1%;bottom: 0%;
                cursor: pointer;
                display: grid;
                place-items: center; 
                img{
                  height: 20px; 
                }
              } 
            }
          }
        }
        .container-item:hover{
          box-shadow: 2px 2px 5px #888;
          border-radius:6px; 
          }
      } 
    } 
  }

  </style>
  