<template>
    <div class="app-content">
             <div class="content-header">
              <el-row :gutter="20" >
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6" style="text-align: left;">
                     <el-select v-model="selectedGoodsGroup" :disabled="disableClassifyGroupSelect" class="m-2" style="width:100%" @change="onSelectGroup">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">大类</div></template>
                         <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key">
                     </el-option>
                   </el-select> 
                 </el-col>
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                     <el-select v-model="selectedGoodsClassifyId"  class="m-2" style="width:100%" @change="getGoodsDatya">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">小类</div></template>
                         <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                     </el-option>
                   </el-select>
                 </el-col>
              </el-row>
              <el-row class="header-row" :gutter="20">
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                    <el-select v-model="goodsSort" class="m-2" style="width:100%" @change="getGoodsDatya">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">排序</div></template>
                         <el-option value="goodsName-asc" label="按名称升序"/>
                         <el-option value="goodsName-desc" label="按名称降序"/>
                         <el-option value="supplier-asc" label="按供应商升序"/>
                         <el-option value="supplier-desc" label="按供应商降序"/>
                   </el-select>
                 </el-col>
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6"> 
                   <el-input v-model="searchKey" placeholder="可按关键字检索">
                     <template #append>
                         <el-button type="primary" @click="getGoodsDatya"><el-icon style="position: relative;top:4px;font-size: 15px;"><Search /></el-icon></el-button>
                     </template>
                     </el-input>
                 </el-col>
              </el-row> 
             </div>
             <div class="content-body">  
                <el-row v-if="goodsData.length>0" class="body-row">
                 <el-col :xs="24" :sm="12" :md="12" :lg="6" :xl="6" v-for="item in goodsData" class="body-item">
                     <el-card class="item-card" :shadow="item.isSelected?'always':'never'" :style="{border:item.isSelected?'1px solid #f7a500':''}">  
                         <div class="item-img">
                             <el-image  hide-on-click-modal class="img-goods" :src="item.goodsPicture"  :zoom-rate="1.2" :preview-src-list="[item.goodsPicture]" :initial-index="0"  fit="cover" />
                             <img v-if="item.isTakeStockLock" class="img-lock" src="/public/icon-img/suo.png" >
                         </div>
                         <div class="item-desc" @click="onSelectGoods(item)"> 
                             <div class="desc-title">{{ item.goodsName }} <span class="desc-info" v-if="item.goodsModel">{{ item.goodsModel }}</span></div> 
                             <div class="desc-info">分&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;类：{{item.goodsClassifyName}}</div>
                             <div class="desc-info">SAP编码：{{ item.goodsNo }}</div>
                             <div class="desc-info">供&nbsp;&nbsp;应&nbsp;&nbsp;商：{{item.supplier}}</div>
                             <div class="desc-info">库&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;存：
                                 <span v-if="item.minPackageUnitName==item.packageUnitName&&item.packageUnitName==item.maxPackageUnitName">
                                     <span>{{item.standardPackageStock+item.packageUnitName}}</span>  
                                 </span>
                                 <span v-else-if="item.minPackageUnitName!=item.packageUnitName&&item.packageUnitName==item.maxPackageUnitName">
                                 <span v-if="item.standardPackageStock!=0">{{item.standardPackageStock+item.packageUnitName}}</span> 
                                     <span v-if="item.minPackageStock!=0">{{item.minPackageStock+item.minPackageUnitName}}</span>
                                     <span v-if="item.standardPackageStock==0&&item.minPackageStock==0&&item.maxPackageStock==0">0</span>   
                                 </span>
                                 <span v-else-if="item.minPackageUnitName==item.packageUnitName&&item.packageUnitName!=item.maxPackageUnitName">
                                     <span v-if="item.maxPackageStock!=0">{{item.maxPackageStock+item.maxPackageUnitName}}</span>  
                                     <span v-if="item.standardPackageStock!=0">{{item.standardPackageStock+item.packageUnitName}}</span> 
                                     <span v-if="item.standardPackageStock==0&&item.minPackageStock==0&&item.maxPackageStock==0">0</span>  
                                 </span>
                                 <span v-else>
                                     <span v-if="item.maxPackageStock!=0">{{item.maxPackageStock+item.maxPackageUnitName}}</span>  
                                     <span v-if="item.standardPackageStock!=0">{{item.standardPackageStock+item.packageUnitName}}</span>   
                                     <span v-if="item.minPackageStock!=0">{{item.minPackageStock+item.minPackageUnitName}}</span> 
                                     <span v-if="item.standardPackageStock==0&&item.minPackageStock==0&&item.maxPackageStock==0">0</span>  
                                 </span>   
                             </div> 
                             <div class="desc-info">状&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;态：
                                <span v-if="item.isTakeStockLock" class="text-warning">盘点中</span>
                                <span v-else>
                                    <span v-if="item.isUnSubmitLabels" class="text-warning">存在未提交的标签码</span>
                                    <span v-else>正常</span>
                                </span>
                            </div> 
                         </div> 
                     </el-card>
                 </el-col>
                </el-row>
                <el-row v-else>
                 <el-col><el-empty description="没有任何数据" /></el-col>
               </el-row>
               <div v-if="totalPage>1">
                 <img src="/public/icon-img/left-circle-fill.png" alt="上一页" v-if="pgIndex>1"  class="btn-page-pre" @click="onPrePage">
                  <img src="/public/icon-img/left-circle-fill-dis.png" alt="上一页" v-else class="btn-page-pre">
                  <img src="/public/icon-img/right-circle-fill.png" alt="下一页" v-if="pgIndex<totalPage" class="btn-page-next" @click="onNextPage">
                  <img src="/public/icon-img/right-circle-fill-dis.png" alt="下一页" v-else class="btn-page-next">
               </div>
             </div> 
         </div> 
 </template>
 <script lang="ts" setup>
 import {ref,defineEmits,defineProps,onMounted } from 'vue'  
 import { useRouter, useRoute } from 'vue-router'  
 import {ArrowLeft,Search} from '@element-plus/icons-vue';    
 import {getGoodsByKey} from "@/api/inv/scan-instorage";
 import {getGoodsGroup,getGoodsClassify} from '@/api/common';  
 import permission from '@/utils/system/permission'; 
 import commonHelper from "@/utils/system/common-helper";
 import { deftClassifyGroup ,disableClassifyGroupSelect} from '@/config';
 import msg from "@/utils/system/message";
  
 const router = useRouter();
 const route=useRoute();
 const goodsData=ref(new Array<any>());
 const pgSize=ref(10);
 const pgIndex=ref(1); 
 const totalDate=ref(0);
 const totalPage=ref(0);
 const goodsGroupData=ref(new Array<any>()); 
 const selectedGoodsGroup:any=ref(deftClassifyGroup); 
 const goodsClassifyData=ref(new Array<any>()); 
 const selectedGoodsClassifyId=ref(0); 
 const searchKey=ref('');
 const goodsSort=ref('goodsName-asc') 
  
  onMounted(()=>{  
    if(route.params?.goodsClassifyGroup){
        selectedGoodsGroup.value=route.params?.goodsClassifyGroup;
    }
     getGoodsGroupData().then(()=>{ 
        onSelectGroup(); 
     }) 
  });
 
  const getGoodsGroupData=()=>{
     return getGoodsGroup().then(res=>{
         goodsGroupData.value=res.data;
     })
 } 
 
 const onSelectGroup=()=>{  
     getGoodsClassifyData();
     getGoodsDatya();
 }
   
 const getGoodsClassifyData=()=>{
     return getGoodsClassify(selectedGoodsGroup.value).then(res=>{
         goodsClassifyData.value=[{key:0,value:'All'},...res.data];
     })
 }
  
  const getGoodsDatya=()=>{ 
     let orderField=goodsSort.value.split('-')[0];
     let orderType=goodsSort.value.split('-')[1];
     getGoodsByKey(pgSize.value,pgIndex.value,orderField,orderType,selectedGoodsGroup.value,selectedGoodsClassifyId.value, searchKey.value).then(res=>{
         goodsData.value=res.data.rows; 
         totalDate.value=res.data.total;
         totalPage.value=Math.ceil(totalDate.value/pgSize.value); 
         if(route.params?.goodsId){
           let selectedGoods= goodsData.value.find(f=>f.goodsId==route.params?.goodsId);
           if(selectedGoods){
            selectedGoods.isSelected=true;
           }
         }
     })
  }
 
  const onPrePage=()=>{
     if(pgIndex.value>1){
         pgIndex.value--; 
         getGoodsDatya();
     }
 }
 
 const onNextPage=()=>{
     if(pgIndex.value<totalPage.value){
         pgIndex.value++; 
         getGoodsDatya();
     }
 } 
 
 const onSelectGoods=(item:any)=>{  
    goodsData.value.forEach(obj => {
        obj.isSelected=false;
        if(item.goodsId==obj.goodsId){
            obj.isSelected=true;
        }
    });
    if(item.isTakeStockLock){
        msg.deftAuto(`${item.goodsName}正在盘点中，暂停入库操作`);
        return;
    }
    item.goodsClassifyGroup=selectedGoodsGroup.value;
    let routeName='scan-instorage';
    router.push({name:routeName,params:item}).then(()=>{
        permission.addRouteHis(routeName); 
    });
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
             height: 80px; 
             background-color:rgb(247, 252, 252);
             .header-row{
                 margin-top: 10px; 
             }
         }
         .content-body{ 
             padding: 10px 15px 5px 0px; 
             height: 75%;
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
                             float: left; 
                             width: 40%;
                             .img-goods{
                                 width: 90%;
                                 height: 100px; 
                                 display: block;
                                 border:1px solid #e7e7e7;
                                 border-radius: 5px;
                             }
                             .img-lock{
                                 height: 30px;position: absolute;top:0
                             } 
                         }
                         .item-desc{  
                             position: relative;
                             bottom: 8px;
                             left: 5px;
                             font-size: 12px;
                             float: left;
                             width: 60%;
                             .desc-title{ 
                             font-weight: 600;
                             }
                             .desc-info{ 
                                 color:#888;
                                 padding-top: 3px;
                             }
                         }
                     }
             }
             } 
             .btn-page-pre{
                 height: 50px;position: fixed;bottom: 40%;left: 0;opacity:.2
             }
             .btn-page-next{
                 height: 50px;position: fixed;bottom: 40%;right: 0;opacity:.2
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
             height: 90px; 
             background-color:rgb(247, 252, 252);
             .header-row{
                 margin-top: 10px; 
             }
         }
         .content-body{ 
             padding: 10px 15px 5px 0px; 
             height: 82%;
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
                    float: left; 
                    width: 40%;
                    bottom: 8px;
                     .img-goods{
                         width: 90%;
                         height: 110px; 
                         display: block;
                         border:1px solid #e7e7e7;
                         border-radius: 5px;
                     }
                     .img-lock{
                         height: 60px;position: absolute;top:0
                     }
                 }
                 .item-desc{ 
                     font-size: 12px;
                     float: left; 
                     .desc-title{ 
                        font-weight: 600;
                        position: relative;
                        bottom: 4px;
                        }
                     .desc-info{ 
                         color:#888;
                         padding-bottom: 4px; 
                     }
                 }
                }
             }
             } 
             .btn-page-pre{
                 height: 80px;position: fixed;bottom: 40%;left: 0;opacity:.2
             }
             .btn-page-next{
                 height: 80px;position: fixed;bottom: 40%;right: 0;opacity:.2
             }
         }  
     } 
 } 
 </style>