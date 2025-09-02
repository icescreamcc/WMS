<template>
    <div class="app-content">
             <div class="content-header">
              <el-row :gutter="20" >
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6" style="text-align: left;">
                    <el-select v-model="selectedGoodsGroup" class="m-2" style="width:100%" @change="onSelectGroup">
                        <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">分类</div></template>
                        <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
                  </el-select>  
                 </el-col>
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                    <el-select v-model="selectedWarehouseId"  class="m-2" style="width:100%" @change="onWarehouseSelected">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">仓库</div></template>
                         <el-option v-for="item in warehouseData" :key="item.warehouseId" :label="item.warehouseName" :value="item.warehouseId">
                     </el-option>
                   </el-select> 
                 </el-col>
              </el-row>
              <el-row class="header-row" :gutter="20">
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                    <el-select v-model="binSort" class="m-2" style="width:100%" @change="getBinsData">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">排序</div></template>
                         <el-option value="binNo-asc" label="按货位升序"/>
                         <el-option value="binNo-desc" label="按货位降序"/> 
                         <el-option value="lastInventoryDate-asc" label="按盘点日期升序"></el-option>
                         <el-option value="lastInventoryDate-desc" label="按盘点日期降序"></el-option>
                   </el-select>
                 </el-col>
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                    <el-select v-model="selectedShelfId"  class="m-2" style="width:100%" @change="getBinsData">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">货架</div></template>
                         <el-option v-for="item in shelfData" :key="item.id" :label="item.name" :value="item.id">
                     </el-option>
                   </el-select> 
                 </el-col>
              </el-row>
              <el-row class="header-row" :gutter="20"> 
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                    <el-select v-model="isTakeStockCurDate" class="m-2" style="width:100%" @change="getBinsData">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">状态</div></template>
                         <el-option :value=true label="当月已盘点"/>
                         <el-option :value=false label="当月未盘点"/>  
                   </el-select>
                </el-col>
                <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                     <el-input v-model="searchKey" placeholder="可按关键字检索">
                     <template #append>
                         <el-button type="primary" @click="getBinsData"><el-icon style="position: relative;top:4px;font-size: 15px;"><Search /></el-icon></el-button>
                     </template>
                     </el-input>
                 </el-col> 
              </el-row>
             </div>
             <div class="content-body">  
                <el-row v-if="binData.length>0" class="body-row">
                 <el-col :xs="24" :sm="12" :md="12" :lg="6" :xl="6" v-for="item in binData" class="body-item">
                     <el-card class="item-card" shadow="never" @click="onTakeStock(item)">  
                         <div class="item-img">
                             <el-image  hide-on-click-modal class="img-bin" src="/public/icon-img/xuanzekuwei9.png"   fit="fill" />
                             <img v-if="item.isTakeStockLock" class="img-lock" src="/public/icon-img/suo.png" >
                         </div>
                         <div class="item-desc"> 
                             <div class="desc-title">{{ item.binName }}</div> 
                             <div class="desc-info">仓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;库：{{item.warehouseName}}</div>
                             <div class="desc-info">货&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;架：{{item.shelfName}}</div>
                             <div class="desc-info">货位规格：{{item.specName}} </div> 
                             <div class="desc-info">状&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;态：<span v-if="item.isTakeStockLock" class="text-warning">盘点中</span><span v-else>正常</span></div>
                             <div class="desc-info">上次盘点：<span v-if="item.lastInventoryDate">{{commonHelper.formatToDateTime(item.lastInventoryDate)}}</span><span v-else>--</span>
                             </div>
                             <div class="desc-info">盘点人员：<span v-if="item.lastInventoryOperator">{{item.lastInventoryOperator }}</span><span v-else>--</span>
                             </div>
                         </div> 
                     </el-card>
                 </el-col>
                </el-row>
                <el-row v-else>
                 <el-col><el-empty description="没有任何数据" /></el-col>
               </el-row>
               <img class="btn-scan-qr" src="/public/icon-img/saoyisao.png" alt=" 扫一扫"  @click="showScanLayer"> 
               <div v-if="totalPage>1">
                <img src="/public/icon-img/left-circle-fill.png" alt="上一页" v-if="pgIndex>1"  class="btn-page-pre" @click="onPrePage">
                 <img src="/public/icon-img/left-circle-fill-dis.png" alt="上一页" v-else class="btn-page-pre">
                 <img src="/public/icon-img/right-circle-fill.png" alt="下一页" v-if="pgIndex<totalPage" class="btn-page-next" @click="onNextPage">
                 <img src="/public/icon-img/right-circle-fill-dis.png" alt="下一页" v-else class="btn-page-next">
               </div>
               <div class="page-scan" v-if="showScan">
                    <div class="scan-box">
                        <video ref="video" id="video" class="scan-video" autoplay></video>
                        <div class="qr-scanner">
                            <div class="box">
                                <div class="line"></div>
                                <div class="angle"></div>
                            </div>
                        </div>
                        <div class="scan-tip">{{ scanTextData.tipMsg }}</div>
                    </div>
                </div>
             </div>
             <MessageDrawer :options="msgDrawerOptions" @cancel="onCancel" @confirm="onSubmit"/> 
         </div> 
 </template>
 <script lang="ts" setup>
 import {ref,reactive,defineEmits,defineProps,onMounted } from 'vue'  
 import { useRouter, useRoute } from 'vue-router'  
 import {ArrowLeft,Search} from '@element-plus/icons-vue';    
 import {getBins,getBinTakeStockStatus,setTakeStockLockByBin} from "@/api/inv/takestock";
 import {getWarehouses,getShelfByWarehouse,getWorkbinSpec,getGoodsGroup} from '@/api/common'; 
 import MessageDrawer from '../message-drawer.vue';
 import permission from '@/utils/system/permission'; 
 import commonHelper from "@/utils/system/common-helper";
 import { deftClassifyGroup} from '@/config'; 
 import { BrowserMultiFormatReader } from "@zxing/library";

 import msg from "@/utils/system/message";
 import Vconsole from 'vconsole'

 const router = useRouter();
 const route=useRoute(); 
 const pgSize=ref(10);
 const pgIndex=ref(1); 
 const totalDate=ref(0);
 const totalPage=ref(0); 
 const searchKey=ref('');
 const binSort=ref('binNo-asc');
 const isTakeStockCurDate=ref(false);
 const curSelectBin=ref();
 const goodsGroupData=ref(new Array<any>()); 
 const selectedGoodsGroup=ref(deftClassifyGroup); 
 const warehouseData=ref(new Array<any>());
 var warehouseDataBuffer=new Array<any>();
 const shelfData=ref(new Array<any>());
 const binData=ref(new Array<any>());
 const workbinSpecData=ref(new Array<any>());
 const selectedWarehouseId=ref('');
 const selectedShelfId=ref(''); 
 const selectedWorkbinSpecId=ref(0);
 const msgDrawerOptions=ref({
     show:false,
     title:'',
     message:'',
     type:'',
     data:null
 }); 
const showScan=ref(false); 
const scanTextData:any= ref({
        codeReader: null,
        tipMsg: "识别二维码", 
        num: 5, 
        videoLength: ""
    });
//const vconsole=new Vconsole();
  
  onMounted(()=>{  
    getWarehousesData().then(()=>{
        onSelectGroup();
     });  
     getWorkbinSpecData();
     getGoodsGroupData();
  });

const getGoodsGroupData=()=>{
    return getGoodsGroup().then(res=>{
        if(deftClassifyGroup=='SparePart'){
          goodsGroupData.value=res.data.filter((f:any)=>f.key=='SparePart'||f.key=='Consumables');
        }
        else{
          goodsGroupData.value=res.data;
        }  
    })
} 

const onSelectGroup=()=>{
    warehouseData.value.length=0;
    shelfData.value.length=0;
    selectedShelfId.value='';
    selectedWarehouseId.value='';
    let warehouseBygroup=warehouseDataBuffer.filter(f=>f.warehouseType==selectedGoodsGroup.value); 
    if(warehouseBygroup?.length>0){ 
        warehouseData.value=[{warehouseId:'',warehouseName:'All'},...warehouseBygroup] 
    }  
    getShelfDataByWarehouse();
    getBinsData();
}

const onWarehouseSelected=()=>{
    getShelfDataByWarehouse();
    getBinsData();
}

const getWarehousesData=()=>{
     return getWarehouses().then(res=>{
         if(res.data){ 
             warehouseDataBuffer=res.data.map((m:any)=>{
                return{
                  warehouseId:m.warehouseId,
                  warehouseName:m.warehouseName,
                  warehouseType:m.warehouseType
                }
              });
         } 
     })
 }
 
  const getShelfDataByWarehouse=()=>{
     return getShelfByWarehouse(selectedWarehouseId.value).then(res=>{
        shelfData.value=[{id:'',name:'All'},...res.data];
     })
 } 

 const getWorkbinSpecData=()=>{
    return getWorkbinSpec().then(res=>{
        workbinSpecData.value=[{key:0,value:'All'},...res.data];
    })
 }
 
 const getBinsData=()=>{
    let orderField=binSort.value.split('-')[0];
    let orderType=binSort.value.split('-')[1];
     return getBins(pgSize.value,pgIndex.value,selectedWarehouseId.value,selectedShelfId.value,selectedWorkbinSpecId.value,selectedGoodsGroup.value, orderField,orderType, searchKey.value,isTakeStockCurDate.value).then(res=>{
        binData.value=res.data.rows; 
        totalDate.value=res.data.total;
        totalPage.value=Math.ceil(totalDate.value/pgSize.value);
     })
 }

 const showScanLayer=()=>{ 
    if(!showScan.value){ 
        scanTextData.value.codeReader = new BrowserMultiFormatReader();
        openScan();  
    } 
 }

 const openScan=()=>{
    scanTextData.value.codeReader.getVideoInputDevices().then((videoInputDevices:any) => {
            // 默认获取第一个摄像头设备id
            let firstDeviceId = videoInputDevices[0].deviceId;
            console.log( "手机摄像头的数量",   videoInputDevices  );
            // 获取第一个摄像头设备的名称
            const videoInputDeviceslablestr = JSON.stringify(
              videoInputDevices[0].label
            );
            console.log("摄像头信息",videoInputDeviceslablestr)
            if (videoInputDevices.length > 1) {
              // 华为手机有6个摄像头，前三个是前置，后三个是后置，第6个摄像头最清晰
              if (videoInputDevices.length > 5) {
                firstDeviceId = videoInputDevices[5].deviceId;
              } else {
                // 判断是否后置摄像头
                if (videoInputDeviceslablestr.indexOf("back") > -1) {
                  firstDeviceId = videoInputDevices[0].deviceId;
                } else {
                  firstDeviceId = videoInputDevices[1].deviceId;
                }
              }
            }
            if(firstDeviceId){
                showScan.value=true;
                decodeFromInputVideoFunc(firstDeviceId);
            }
            else{
                msg.deftAuto("未获取到摄像头设备ID")
            }
          })
          .catch((err:any) => {
            console.error("getVideoInputDevices:",err);
          });
  }

  const decodeFromInputVideoFunc=(firstDeviceId:any)=>{
    scanTextData.value.codeReader.reset();
    scanTextData.value.codeReader.decodeFromInputVideoDeviceContinuously(
          firstDeviceId,
          "video",
          (result:any, err:any) => {
            if (result && result.text) {
              let content = result.text; 
              console.log("扫出的数据",content)
              if (content) {
                searchKey.value=result.text;
                let uuidString = /^[a-zA-Z0-9]{32}$/
                if (content.indexOf('handover') > -1) {
                  //扫码签字 
                }else if (uuidString.test(content)) {
                  //设备详情 
                }else { 
                    msg.errorAuto("识别错误，请确认是否扫描的设备二维码~")
                }
              }
            }
            if (err && !err) {
                msg.errorAuto(err) 
            }
          }
        );
  }
   
 const onPrePage=()=>{
    if(pgIndex.value>1){
        pgIndex.value--; 
        getBinsData();
    }
}

const onNextPage=()=>{
    if(pgIndex.value<totalPage.value){
        pgIndex.value++; 
        getBinsData();
    }
}
   
 const onTakeStock=(item:any)=>{
    getBinTakeStockStatus(item.binId).then(res=>{
        curSelectBin.value=item;
         msgDrawerOptions.value.type='Message'
         msgDrawerOptions.value.title='盘点锁定提示';
         if(!res.data){  
         msgDrawerOptions.value.message=`您将开始对货位${item.binName}进行盘点，期间将对其锁定且无法进行出入库操作，是否确认继续？`; 
         } 
         else{
             msgDrawerOptions.value.message=`货位${item.binName}正在盘点中，是否确认进入继续盘点？`; 
         }
         msgDrawerOptions.value.show=true;
     }) 
 }
 
 const onCancel=()=>{
     msgDrawerOptions.value.show=false;
 }
   
 const onSubmit=()=>{
    setTakeStockLockByBin([curSelectBin.value.binId]).then(res=>{
         router.push({name:'takebinsubmit',query:{binId:curSelectBin.value.binId}}).then(()=>{
             permission.addRouteHis(route.name as string); 
         });
     })
     
 }
 </script>
 
 <style lang="scss" scoped> 
 @media screen and (max-width: 450px){ 
     .app-content{
         height: 95%;
         background-color:#fff;
         padding:0 10px;
         .content-header{   
             padding:15px 10px 0 10px;
             height: 115px; 
             background-color:rgb(247, 252, 252);
             .header-row{
                 margin-top: 10px; 
             }
         }
         .content-body{ 
             padding: 10px 15px 5px 0px; 
             height: 71%;
             margin-top: 5px; 
             background-color:rgb(247, 252, 252);
             overflow-y: scroll;
             .body-row{
                 text-align: left;
                 position: relative;
                 left: 11px; 
                 .body-item{
                     padding:2px; 
                     .item-card{  
                         .item-img{
                             position: relative; 
                             bottom: 10px;
                             .img-bin{
                                 width: 113px;
                                 height: 113px; 
                                 display: block;  
                                 opacity: .9;
                             }
                             .img-lock{
                                 height: 30px;position: absolute;top:0
                             }
                             float: left; 
                         }
                         .item-desc{  
                             position: relative;
                             bottom: 8px;
                             left: 5px;
                             font-size: 12px;
                             float: left;
                             .desc-title{ 
                                font-weight: 600;
                                white-space: nowrap;
                                overflow: hidden;  
                                text-overflow: ellipsis; 
                             }
                             .desc-info{ 
                                 color:#888;
                                 padding-top: 3px;
                             }
                         }
                     }
             }
             } 
             .btn-scan-qr{
                height: 50px;position: absolute;bottom: 50%;right: 0;opacity:.5;z-index: 9999;
             }
             .btn-page-pre{
                 height: 50px;position: fixed;bottom: 40%;left: 0;opacity:.2
             }
             .btn-page-next{
                 height: 50px;position: fixed;bottom: 40%;right: 0;opacity:.2
             }
             .qr-scanner .box {
                width: 113px;
                height: 113px;
                position: absolute;
                left: 50%;
                top: 50%;
                transform: translate(-50%, -50%);
                overflow: hidden;
                border: 0.1rem solid rgba(0, 255, 51, 0.2); 
            }
         }  
     } 
 }
 
 @media screen and (min-width: 450px){ 
     .app-content{
         height: 95%;
         background-color:#fff;
         padding:0 15px;
         .content-header{  
             padding:25px 15px 0 15px;
             height: 115px; 
             background-color:rgb(247, 252, 252);
             .header-row{
                 margin-top: 10px; 
             }
         }
         .content-body{ 
             padding: 10px 15px 5px 0px; 
             height: 80%;
             margin-top: 5px; 
             background-color:rgb(247, 252, 252);
             overflow-y: scroll;
             .body-row{
                 text-align: left;
                 position: relative;
                 left: 11px;
                 .body-item{
                 padding:4px;
                .item-card{
                 .item-img{
                     position: relative; 
                     .img-bin{
                         width: 100%;
                         height: 210px; 
                         display: block; 
                         opacity: .9;
                     }
                     .img-lock{
                         height: 60px;position: absolute;top:0
                     }
                 }
                 .item-desc{
                     padding: 10px;
                     font-size: 14px;
                     .desc-title{ 
                        font-weight: 600;
                        white-space: nowrap;
                        overflow: hidden;  
                        text-overflow: ellipsis;
                     }
                     .desc-info{ 
                         color:#888;
                         padding-top: 5px;
                     }
                 }
                }
             }
             } 
             .btn-scan-qr{
                height: 80px;position: absolute;bottom: 50%;right: 0;opacity:.5;z-index: 9999;
             }
             .btn-page-pre{
                 height: 80px;position: absolute;bottom: 40%;left: 0;opacity:.5
             }
             .btn-page-next{
                 height: 80px;position: absolute;bottom: 40%;right: 0;opacity:.5
             }
             .qr-scanner .box {
                width: 213px;
                height: 213px;
                position: absolute;
                left: 50%;
                top: 50%;
                transform: translate(-50%, -50%);
                overflow: hidden;
                border: 0.1rem solid rgba(0, 255, 51, 0.2); 
            }
         }  
     } 
 } 
 .scan-box {
        position: fixed;
        top: 40px;
        left: 0;
        height: 100%;
        width: 100vw;
        background-color: #4c4b4bb8;
        background-image: linear-gradient(
                0deg,
                transparent 24%,
                rgba(32, 255, 77, 0.1) 25%,
                rgba(32, 255, 77, 0.1) 26%,
                transparent 27%,
                transparent 74%,
                rgba(32, 255, 77, 0.1) 75%,
                rgba(32, 255, 77, 0.1) 76%,
                transparent 77%,
                transparent
        ),
        linear-gradient(
                90deg,
                transparent 24%,
                rgba(32, 255, 77, 0.1) 25%,
                rgba(32, 255, 77, 0.1) 26%,
                transparent 27%,
                transparent 74%,
                rgba(32, 255, 77, 0.1) 75%,
                rgba(32, 255, 77, 0.1) 76%,
                transparent 77%,
                transparent
        );
        background-size: 3rem 3rem;
        background-position: -1rem -1rem;
    }

    .scan-video {
        height: 95vh;
        width: 100vw;
        object-fit: cover;
    }

    // .qr-scanner .box {
    //     width: 213px;
    //     height: 213px;
    //     position: absolute;
    //     left: 50%;
    //     top: 50%;
    //     transform: translate(-50%, -50%);
    //     overflow: hidden;
    //     border: 0.1rem solid rgba(0, 255, 51, 0.2);
    //     /* background: url('http://resource.beige.world/imgs/gongconghao.png') no-repeat center center; */
    // }

    .qr-scanner .line {
        height: calc(100% - 2px);
        width: 100%;
        background: linear-gradient(180deg, rgba(0, 255, 51, 0) 43%, #00ff33 211%);
        border-bottom: 3px solid #00ff33;
        transform: translateY(-100%);
        animation: radar-beam 2s infinite alternate;
        animation-timing-function: cubic-bezier(0.53, 0, 0.43, 0.99);
        animation-delay: 1.4s;
    }

    .qr-scanner .box:after,
    .qr-scanner .box:before,
    .qr-scanner .angle:after,
    .qr-scanner .angle:before {
        content: "";
        display: block;
        position: absolute;
        width: 3vw;
        height: 3vw;
        border: 0.2rem solid transparent;
    }

    .qr-scanner .box:after,
    .qr-scanner .box:before {
        top: 0;
        border-top-color: #00ff33;
    }

    .qr-scanner .angle:after,
    .qr-scanner .angle:before {
        bottom: 0;
        border-bottom-color: #00ff33;
    }

    .qr-scanner .box:before,
    .qr-scanner .angle:before {
        left: 0;
        border-left-color: #00ff33;
    }

    .qr-scanner .box:after,
    .qr-scanner .angle:after {
        right: 0;
        border-right-color: #00ff33;
    }

    @keyframes radar-beam {
        0% {
            transform: translateY(-100%);
        }

        100% {
            transform: translateY(0);
        }
    }

    .scan-tip {
        width: 100vw;
        text-align: center;
        margin-bottom: 5vh; 
        font-size: 5vw;
        position: absolute;
        bottom: 10%;
        left: 0;
        color: #00ff33;
    }

    .page-scan {
        overflow-y: hidden;
        height: 80%;
    }
 </style>