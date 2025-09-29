<template>
    <el-drawer v-model="props.options.show" :with-header="false" :show-close="true"  size="37%"> 
       <div class="drawer-content">
           <div class="content-header">
               <div>{{ props.options.title }}</div>
           </div>
           <div class="content-body"> 
             <div class="body-item" >
                <el-popover placement="bottom" v-for="(item,index) in unUsedItem"  :width="200"  trigger="hover">
                    <template #reference>
                        <div class="item-content" :style="{border:item.isSelected?'1px solid rgb(255, 119, 0)':''}" :draggable="true" @dragstart="dragTagStart($event,item)" @mousedown="onItemMousedown($event,item)" @mouseup="onItemMouseup($event)" @mouseover="onItemMouseover($event)" @mouseout="onItemMouseout($event)">
                            <div class="item-icon">
                                <img v-if="item.itemType=='Text'" src="../../../../public/icon-img/wenben.png">
                                <img v-else-if="item.itemType=='QRCode'" src="../../../../public/icon-img/erweima.png">
                                <img v-else-if="item.itemType=='BarCode'" src="../../../../public/icon-img/tiaoxingma.png">
                            </div>
                            <div class="item-title">{{ item.itemName }}</div>
                        </div>
                    </template>
                    <div>
                        <div>名称：{{ item.itemName }}</div>
                        <div v-if="item.itemType=='Text'">
                        <div>字体样式：{{ item.args.font }}</div>
                        <div>字体大小：{{ item.args.fontSize }}</div>
                        <div>字体颜色：{{ item.args.color }}</div>
                        <div>字体加粗：{{ item.args.weight }}</div>
                        </div>
                        <div v-else-if="item.itemType=='QRCode'">
                        <div>二维码类型：{{ item.args.format }}</div>
                        <div>宽度：{{ item.args.width }}</div>
                        <div>高度：{{ item.args.height }}</div>
                        </div>
                        <div v-else-if="item.itemType=='BarCode'">
                        <div>一维码格式：{{ item.args.format }}</div>
                        <div>线宽：{{ item.args.width }}</div>
                        <div>高度：{{ item.args.height }}</div>
                        <div>线条颜色：{{ item.args.lineColor }}</div>
                        <div>是否显示文本：{{ item.args.displayValue }}</div>
                        <div>字体样式：{{ item.args.font }}</div>
                        <div>字体大小：{{ item.args.fontSize }}</div>
                        <div>文本与条码距离：{{ item.args.textMargin }}</div>
                        </div>
                    </div>
                </el-popover>  
             </div>
             <div class="body-form">
                <el-form  label-width="auto" label-position="left" style="padding:15px">
                   <el-row >
                     <el-col :span="12">
                        <el-form-item label="左偏移(mm)" style="text-align: left;">
                            <el-input-number v-model="desginStyle.offsetX" controls-position="right" :precision="2" :min="0"  @change="onInputOffsetMM" style=";width: 80%;" placeholder="距离左侧距离" />
                        </el-form-item> 
                     </el-col>
                     <el-col :span="12">
                        <el-form-item label="顶偏移(mm)"  style="text-align: left;">
                            <el-input-number v-model="desginStyle.offsetY" controls-position="right" :precision="2" :min="0"  @change="onInputOffsetMM" style=";width: 80%;"  placeholder="距离顶部距离" />
                        </el-form-item> 
                     </el-col>
                   </el-row>
                   <el-row >
                     <el-col :span="12">
                        <el-form-item label="左偏移(%)" style="text-align: left;">
                            <el-input-number v-model="desginStyle.offsetPercentX" controls-position="right" :precision="2" :min="0" @change="onInputOffsetPercent" style=";width: 80%;" placeholder="距离左侧距离" />
                        </el-form-item> 
                     </el-col>
                     <el-col :span="12">
                        <el-form-item label="顶偏移(%)"  style="text-align: left;">
                            <el-input-number v-model="desginStyle.offsetPercentY" controls-position="right" :precision="2" :min="0" @change="onInputOffsetPercent"  style=";width: 80%;"  placeholder="距离顶部距离" />
                        </el-form-item> 
                     </el-col>
                   </el-row> 
                   <el-row >
                     <el-col :span="12">
                        <el-form-item label="示例值"  style="text-align: left;">
                            <el-input v-model="desginStyle.deftValue" style=";width: 80%;" @blur="onChangeDeftValue" placeholder="用于模板项设计展示的值" />
                        </el-form-item> 
                     </el-col> 
                   </el-row> 
                </el-form>
             </div>
             <div class="body-template" style="">
                <div class="template-title">
                    <div style="width: 10mm;height: 10mm;background-color: rgb(255, 119, 0);float: left; font-size: 10px;color: #fff;line-height: 10mm;cursor: pointer;" @dblclick="onHelper" title="拖动过程中如果出现元素消失的BUG，可以尝试点击我">10mm</div>
                    <div style="float: left;margin-left: 5mm;margin-top: 1mm;">
                        <div style="font-size: 12px;">{{ `画布尺寸：${standardLayoutWidth}mm * ${standardLayoutHeight}mm` }}</div> 
                        <div style="font-size: 12px;float: left;margin-top: 1mm;">{{ `标签尺寸：${labelData.width}mm * ${labelData.height}mm` }}</div> 
                    </div>
                </div>  
                <div class="template-content" id="labelDesignContent" :style="{height:standardLayoutHeight+'mm',width:standardLayoutWidth+'mm'}" @dragover="allowDrop($event)" @drop="dragTagEnd($event)"> 
                    <div class="template-actual" id="labelDesignActual" :style="{height:labelData.height+'mm',width:labelData.width+'mm',backgroundColor:labelData.backgroundColor||'#fff'}">
                        <div v-for="item in usedItem"  :draggable="true" @dragstart="dragTagStart($event,item)" @mousedown="onItemMousedown($event,item)" @mouseup="onItemMouseup($event)" @mouseover="onItemMouseover($event)" @mouseout="onItemMouseout($event)">
                            <div v-if="item.itemType=='Text'" :title="item.itemName" :id="item.itemName" :style="{color:item.args.color,fontFamily:item.args.font,fontSize:item.args.fontSize+'px',fontWeight:item.args.weight?'bold':'normal',position:'absolute',left:(item.style.leftPercent||0)+'%',top:(item.style.topPercent||0)+'%',border:item.isSelected?'1px dashed rgb(255, 119, 0)':'', whiteSpace:'nowrap',overflow:'hidden'}">{{item.showName? `${item.itemName}：`:'' }}{{ item.deftValue||'xxxxxx' }}</div>
                            <div v-else-if="item.itemType=='QRCode'"  :title="item.itemName" :id="item.itemName" :style="{height:item.args.height+'mm',width:item.args.width+'mm',position:'absolute',left:(item.style.leftPercent||0)+'%',top:(item.style.topPercent||0)+'%',border:item.isSelected?'1px dashed rgb(255, 119, 0)':''}"> 
                                <canvas v-if="item.args.format=='DataMatrix'" :id="'qrCode'+item.itemName" style="width: 100%;height: 100%;"></canvas>
                                <VueQr v-else-if="item.args.format=='QRCode'" :id="'qrCode'+item.itemName" :text="item.deftValue||'qrCode'+item.itemName.toUpperCase()" :size="item.args.width" :dotScale="1" :margin="0"></VueQr>
                            </div>
                            <div v-else-if="item.itemType=='BarCode'"  :title="item.itemName" :id="item.itemName" :style="{height:item.args.height+'mm',position:'absolute',left:(item.style.leftPercent||0)+'%',top:(item.style.topPercent||0)+'%'}">
                                <img :id="'barCode'+item.itemName" :style="{border:item.isSelected?'1px dashed rgb(255, 119, 0)':''}">
                            </div>
                        </div>
                    </div>
                </div>
             </div>
           </div> 
           <!-- <div class="content-footer">
                <img src="../../../../public/icon-img/tijiaochenggong.png" title="编辑保存" @click="submit"> 
            </div> -->
       </div>
    </el-drawer> 
 </template>
 
 <script lang="ts" setup>
 import {ref ,defineEmits,defineProps,onMounted,nextTick} from 'vue';
 import Draggable from 'vuedraggable';
 import JsBarcode from 'jsbarcode';     
 import * as bwipjs from 'bwip-js';
 import VueQr from 'vue-qr/src/packages/vue-qr.vue';

 const props=defineProps({
   options: {
       type: Object,
       default: () => {
           return {
           show: false,
           title: '', 
           type:'',  
           data:{},
           dataOptions:{}
           }
       }
       }
   }); 
 const emit = defineEmits(['dataSubmit']);
 const fontFamilys=ref(props.options.dataOptions.fontFamilys);
 const labelData=ref(props.options.data);       
 const unUsedItem=ref();
 const usedItem=ref(); 
 const standardLayoutWidth=ref(140);
 const standardLayoutHeight=ref(Number((standardLayoutWidth.value*(labelData.value.height/labelData.value.width)).toFixed(2)));
 const deftDPI=ref(96); 
 const curDraggedItem=ref();
 const desginStyle=ref({
    offsetX:0,
    offsetY:0,
    offsetPercentX:0,
    offsetPercentY:0,
    deftValue:''
 }); 

 onMounted(()=>{   
    labelData.value.details.forEach((f:any) => f.isSelected=false);  
    usedItem.value=labelData.value.details.filter((f:any)=>f.isUsed);
    unUsedItem.value=labelData.value.details.filter((f:any)=>!f.isUsed); 
    if(usedItem.value?.length>0){
       nextTick(()=>{
        usedItem.value.forEach((item:any) => {
            setBarcode(item);
        });
       })
    } 
    getWindowDeftDPI(); 
 })

 const getWindowDeftDPI=()=>{
    nextTick(()=>{
        let containerDom:any=document.getElementById('labelDesignContent'); 
        deftDPI.value= (containerDom.offsetWidth)/( standardLayoutWidth.value/25.4);
    })
 }

 const getScaleplateData=(size:number)=>{
   let span= 10; 
   let carValue=0;
   let arr=[];
   while(carValue<=size){
    arr.push(carValue);
    carValue+=span;
   }
   return arr;
 }
  
 const convertToMM=(pixel:number)=>{ 
    return Number(((pixel/deftDPI.value)*25.4).toFixed(2));
 }

 const onItemMousedown=(ev:any,item:any)=>{ 
    ev.target.style.cursor='grabbing';  
    labelData.value.details.forEach((detail:any) => {
        detail.isSelected=false;
    });
    item.isSelected=true;  
    curDraggedItem.value=item; 
    setStyleInputValue(item.style.left,item.style.top);
 }

 const onItemMouseup=(ev:any)=>{
    ev.target.style.cursor='grab';
 }
 
 const onItemMouseover=(ev:any)=>{
    ev.target.style.cursor='grab';
 }

 const onItemMouseout=(ev:any)=>{
    ev.target.style.cursor='default';
 }

 const onInputOffsetMM=()=>{
    if( curDraggedItem.value){ 
        desginStyle.value.offsetPercentX=Number(((desginStyle.value.offsetX/standardLayoutWidth.value)*100).toFixed(2));
        desginStyle.value.offsetPercentY=Number(((desginStyle.value.offsetY/standardLayoutHeight.value)*100).toFixed(2));
        curDraggedItem.value.style.left=desginStyle.value.offsetX;
        curDraggedItem.value.style.top=desginStyle.value.offsetY; 
        curDraggedItem.value.style.leftPercent=desginStyle.value.offsetPercentX;
        curDraggedItem.value.style.topPercent=desginStyle.value.offsetPercentY;
    } 
 }

 const onInputOffsetPercent=()=>{
    if( curDraggedItem.value){
        curDraggedItem.value.style.left= (desginStyle.value.offsetPercentX/100)*standardLayoutWidth.value;
        curDraggedItem.value.style.top= (desginStyle.value.offsetPercentY/100)*standardLayoutHeight.value;
        desginStyle.value.offsetX=curDraggedItem.value.style.left;
        desginStyle.value.offsetY=curDraggedItem.value.style.top;
        curDraggedItem.value.style.leftPercent=desginStyle.value.offsetPercentX;
        curDraggedItem.value.style.topPercent=desginStyle.value.offsetPercentY;
    } 
 }

 const onChangeDeftValue=()=>{
    if(curDraggedItem.value){
        curDraggedItem.value.deftValue=desginStyle.value.deftValue;
        setBarcode(curDraggedItem.value);
    } 
 }

 const onHelper=()=>{
    if(usedItem.value?.length>0){
        usedItem.value.forEach((item:any) => {
            setBarcode(item);
        });
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

const allowDrop=(ev:any)=>{
    ev.preventDefault();
 }

const dragTagStart=(ev:any,item:any)=> {      
   
}  

const  dragTagEnd=(ev:any)=> {   
    ev.preventDefault();   
    curDraggedItem.value.isUsed=true; 
    usedItem.value=labelData.value.details.filter((f:any)=>f.isUsed);
    unUsedItem.value=labelData.value.details.filter((f:any)=>!f.isUsed); 
    nextTick(()=>{
        setBarcode(curDraggedItem.value);
        let curDom:any=document.getElementById(curDraggedItem.value.itemName); 
        let mouseX = ev.clientX;
        let mouseY = ev.clientY; 
        let containerDom:any=document.getElementById('labelDesignActual');
        let containerRect = containerDom.getBoundingClientRect();
        let containerX = containerRect.left;
        let containerY = containerRect.top; 
        let relativePixelX = mouseX - containerX;
        let relativePixelY = mouseY - containerY; 
        let relativeMMX=convertToMM(relativePixelX);
        let relativeMMY=convertToMM(relativePixelY);
        curDom.style.position="absolute";
        curDom.style.left=relativeMMX+'mm';
        curDom.style.top=relativeMMY+'mm';
        setStyleInputValue(relativeMMX,relativeMMY);
        curDraggedItem.value.style.left=relativeMMX;
        curDraggedItem.value.style.top=relativeMMY;
        curDraggedItem.value.style.leftPercent=desginStyle.value.offsetPercentX;
        curDraggedItem.value.style.topPercent=desginStyle.value.offsetPercentY;
    })   
}

const setStyleInputValue=(relativeMMX:number,relativeMMY:number)=>{
    desginStyle.value.offsetX=relativeMMX;
    desginStyle.value.offsetY=relativeMMY;
    desginStyle.value.offsetPercentX=Number(((relativeMMX/standardLayoutWidth.value)*100).toFixed(2));
    desginStyle.value.offsetPercentY=Number(((relativeMMY/standardLayoutHeight.value)*100).toFixed(2));
}
 
const setBarcode=(item:any)=>{
    if(item.itemType=='BarCode'){
        let barcodeArgs=item.args;
        let barcodeDomId=`barCode${item.itemName}`;
        let deftText=item.deftValue||barcodeDomId.toLocaleUpperCase(); 
        item.deftValue=deftText;
        createBarcode(barcodeDomId,deftText,barcodeArgs.format,barcodeArgs.width,barcodeArgs.height,barcodeArgs.lineColor,barcodeArgs.displayValue,barcodeArgs.font,barcodeArgs.fontSize,barcodeArgs.textMargin);
    }
    else if(item.itemType=='QRCode'){
        let qrcodeArgs=item.args;
        if(qrcodeArgs.format=='DataMatrix'){
            let qrcodeId=`qrCode${item.itemName}`;
            let deftText=item.deftValue||qrcodeId.toLocaleUpperCase();
            item.deftValue=deftText;
            createDataMatrix(qrcodeId,deftText);
        }
        else{ 
        }
    }
 }

 const submit=()=> {    
     emit('dataSubmit', labelData.value);  
 } 
 </script>
 
 <style lang="scss" scoped> 
   .drawer-content{
       padding: 30px 15px;
       position: relative;
       height: 99%; 
       .content-header{
           padding:0 20px 0 15px;
           margin-bottom: 10px;
           background-color: rgb(242, 248, 248);
           height: 5%; 
           div{
               padding: 10px 0;
               font-weight: 600; 
               border-radius: 2px;  
           }
       }
       .content-body{
               background-color: rgb(242, 248, 248);
               padding: 0 10px 1px 10px;
               height: 97%; 
               .body-item{
                text-align: left;
                padding:20px 10px;
                border-bottom: 2px solid #fff;
                height: 26%;
                overflow-x: hidden;
                overflow-y: scroll;
                .item-content{ 
                    background-color: #b2b0b0;
                    border: 1px solid #b2b0b0;
                    border-left: none;
                    height: 30px;
                    color: #fff;  
                    width: 20%;
                    border-radius: 5px;
                    display: inline-block;
                    margin-right: 10px;
                    cursor: grab; 
                    position: relative; 
                    margin-bottom: 10px;
                    .item-selected{
                        border:1px solid rgb(2, 54, 2)
                    }
                    .item-icon{
                        float: left;
                        background-color: #fff; 
                        width: 30px;
                        height: 100%;  
                        border-left: 1px solid #b2b0b0;
                        border-radius: 5px 0 0 5px;
                        position: relative; 
                        img{
                            position: absolute;
                            left: 10%;
                            top: 10%; 
                            height: 23px; 
                        }
                    }
                    .item-title{ 
                        text-align: center;
                        position: relative; 
                        top:20%;
                        white-space: nowrap;  
                        overflow: hidden;  
                        text-overflow: ellipsis;   
                    } 
                } 
               }
               .body-form{
                height: 21%;
                border-bottom: 2px solid #fff;
               }
               .body-template{  
                    position: relative;
                    text-align: center;
                    height: 57%; 
                    overflow: scroll; 
                    .template-title{
                        position: relative;
                        padding: 5px 15px 0 15px;
                    }
                    .template-scaleplate-x{
                        position: relative;
                        left: 52%;
                        top: 11%;
                        transform: translate(-50% ); 
                        border-bottom: 1px dashed #8d2626;
                        text-align: left; 
                        .x-item{
                            position: relative; 
                            font-size: 10px;
                            margin-right: 10mm;  
                        }
                    }
                    .template-content{ 
                        position: relative;
                        left: 52%;
                        top: 13%;
                        transform: translate(-50% ); 
                        background-color: #fff; 
                        .template-actual{
                            overflow: hidden;
                            position: absolute;
                            left: 50%; 
                            top:50%;
                            transform: translate(-50%,-50%);
                            border:1px dashed #888; 
                        }
                    }
                }
       } 
       .content-footer{
               text-align: right;
               background-color: rgb(242, 248, 248);
               padding: 0 10px 1px 10px;
               margin-top: 10px;
               height: 8%; 
               position: relative;
               img{
                   height: 45px;
                   cursor: pointer;  
                   position: absolute;
                   right: 2%;
                   top: 50%;
                   transform: translate(0%,-50%);
               }
           }
   }
 </style>