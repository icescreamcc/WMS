<template>
    <div class="app-content">
             <div class="content-header">
              <el-row :gutter="20" >
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6" style="text-align: left;">
                     <el-select v-model="selectedGoodsGroup"  class="m-2" style="width:100%" @change="onSelectGoodsGroup">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">大类</div></template>
                         <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key">
                     </el-option>
                   </el-select> 
                 </el-col>
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                     <el-select v-model="selectedGoodsClassifyId"  class="m-2" style="width:100%" @change="getGoodsDatya(true)">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">小类</div></template>
                         <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                     </el-option>
                   </el-select>
                 </el-col>
              </el-row>
              <el-row class="header-row" :gutter="20">
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                    <el-select v-model="selectedWarehouseId"  class="m-2" style="width:100%" @change="onWarehouseSelected">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">仓库</div></template>
                         <el-option v-for="item in warehouseData" :key="item.warehouseId" :label="item.warehouseName" :value="item.warehouseId">
                     </el-option>
                   </el-select> 
                 </el-col>
                 <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                    <el-select v-model="selectedShelfId"  class="m-2" style="width:100%" @change="getBinDataByShelf">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">货架</div></template>
                         <el-option v-for="item in shelfData" :key="item.id" :label="item.name" :value="item.id">
                     </el-option>
                   </el-select> 
                 </el-col>
              </el-row>
              <el-row class="header-row" :gutter="20">
                 <el-col  :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                    <el-select v-model="selectedBinId"  class="m-2" style="width:100%" @change="getGoodsDatya(true)">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">货位</div></template>
                         <el-option v-for="item in binData" :key="item.id" :label="item.name" :value="item.id">
                     </el-option>
                   </el-select>
                 </el-col>
                 <el-col  :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                    <el-input v-model="searchKey" placeholder="可按关键字检索">
                     <template #append>
                         <el-button type="primary" @click="getGoodsDatya(true)"><el-icon style="position: relative;top:4px;font-size: 15px;"><Search /></el-icon></el-button>
                     </template>
                     </el-input>
                    <!-- <el-select v-model="goodsSort" class="m-2" style="width:100%" @change="getGoodsDatya(true)">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">排序</div></template>
                         <el-option value="goodsName-asc" label="按名称升序"/>
                         <el-option value="goodsName-desc" label="按名称降序"/>
                         <el-option value="supplier-asc" label="按供应商升序"/>
                         <el-option value="supplier-desc" label="按供应商降序"/>
                   </el-select> -->
                 </el-col>
              </el-row>
             </div>
             <div class="content-body">  
                <el-row v-if="goodsData.length>0" class="body-row">
                 <el-col :xs="24" :sm="12" :md="12" :lg="6" :xl="6" v-for="item in goodsData" class="body-item">
                     <el-card class="item-card" shadow="never">  
                         <div class="item-img">
                             <el-image  hide-on-click-modal class="img-goods" :src="item.goodsPicture"  :zoom-rate="1.2" :preview-src-list="[item.goodsPicture]" :initial-index="0"  fit="cover" />
                         </div>
                         <div class="item-desc" @click="onShowDetail(item)"> 
                             <div class="desc-title">{{ item.goodsName }} <span class="desc-info" v-if="item.goodsModel">{{ item.goodsModel }}</span></div> 
                             <div class="desc-info">分&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;类：{{item.goodsClassifyName}}</div>
                             <div class="desc-info">储存规格：{{item.goodsSpecificationName}}</div>
                             <div class="desc-info">供&nbsp;&nbsp;应&nbsp;&nbsp;商：{{item.supplier}}</div> 
                             <div class="desc-info">安全库存：<span v-if="item.safetyInventory>0">{{item.safetyInventory+item.safetyInventoryUnitName}}</span></div>
                             <div class="desc-info">最小采购：<span v-if="item.purchaseMinimum>0">{{item.purchaseMinimum+item.purchaseMinimumUnitName}}</span></div>
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
 import {ref,defineEmits,defineProps,onMounted,onBeforeUnmount } from 'vue'  
 import { useRouter, useRoute } from 'vue-router'  
 import {ArrowLeft,Search} from '@element-plus/icons-vue';    
 import {getGoodsList} from "@/api/baseinfo/goods";
 import {getGoodsGroup,getGoodsClassify,getWarehouses,getShelfByWarehouse,getBinByShelf} from '@/api/common';  
 import permission from '@/utils/system/permission'; 
 import commonHelper from "@/utils/system/common-helper";
 import { deftClassifyGroup } from '@/config';
  
 const router = useRouter();
 const route=useRoute();  
 const goodsData=ref(new Array<any>());
 const pgSize=ref(10);
 const pgIndex=ref(1); 
 const totalDate=ref(0);
 const totalPage=ref(0);
 const goodsGroupData=ref(new Array<any>()); 
 const selectedGoodsGroup=ref(deftClassifyGroup); 
 const goodsClassifyData=ref(new Array<any>()); 
 const selectedGoodsClassifyId=ref(0); 
 const searchKey=ref('');
 const goodsSort=ref('goodsName-asc');
 const warehouseData=ref(new Array<any>());
 const shelfData=ref(new Array<any>());
 const binData=ref(new Array<any>()); 
 const selectedWarehouseId=ref('');
 const selectedShelfId=ref(''); 
 const selectedBinId=ref('');
 const warehouseDataBuffer=ref(new Array<any>());
 const storageKey="goodslist-mobile";
  
  onMounted(()=>{   
    getWarehousesData().then(()=>{
        getGoodsGroupData().then(()=>{ 
            let queryParamsHis=commonHelper.getObjLocalStorage(storageKey);  
            if(queryParamsHis){
                pgSize.value=queryParamsHis.pgSize;
                pgIndex.value=queryParamsHis.pgIndex; 
                goodsSort.value=queryParamsHis.orderField+'-'+queryParamsHis.orderType; 
                selectedGoodsGroup.value=queryParamsHis.selectedGoodsGroup;
                selectedGoodsClassifyId.value=queryParamsHis.selectedGoodsClassifyId;
                selectedWarehouseId.value=queryParamsHis.selectedWarehouseId;
                selectedShelfId.value=queryParamsHis.selectedShelfId;
                selectedBinId.value=queryParamsHis.binId;
                searchKey.value=queryParamsHis.searchKey;  
                let warehouseBygroup=warehouseDataBuffer.value.filter(f=>f.warehouseType==selectedGoodsGroup.value); 
                if(warehouseBygroup?.length>0){ 
                    warehouseData.value=[{warehouseId:'',warehouseName:'All'},...warehouseBygroup] 
                }   
                if(selectedWarehouseId.value){
                    getShelfDataByWarehouse();
                }
                if(selectedShelfId.value){
                    getBinDataByShelf()
                }
                getGoodsClassifyData();
                getGoodsDatya(true);
            }
            else{
                onSelectGoodsGroup();
            }   
        }) 
    }) 
  });

  onBeforeUnmount(()=>{
    commonHelper.setObjLocalStorage(storageKey,null);
  })
 
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

 const onSelectGoodsGroup=()=>{
    warehouseData.value.length=0;
    shelfData.value.length=0;
    binData.value.length=0;
    selectedGoodsClassifyId.value=0;
    selectedWarehouseId.value='';
    selectedShelfId.value='';
    selectedBinId.value='';
    let warehouseBygroup=warehouseDataBuffer.value.filter(f=>f.warehouseType==selectedGoodsGroup.value); 
    if(warehouseBygroup?.length>0){ 
        warehouseData.value=[{warehouseId:'',warehouseName:'All'},...warehouseBygroup] 
    }   
    getGoodsClassifyData();
    getGoodsDatya(true);
 }
 
 const getGoodsClassifyData=()=>{
     return getGoodsClassify(selectedGoodsGroup.value).then(res=>{
         goodsClassifyData.value=[{key:0,value:'All'},...res.data];
     })
 }

 const getWarehousesData=()=>{
     return getWarehouses().then(res=>{
         if(res.data){ 
             warehouseDataBuffer.value=res.data.map((m:any)=>{
                return{
                  warehouseId:m.warehouseId,
                  warehouseName:m.warehouseName,
                  warehouseType:m.warehouseType
                }
              });
         } 
     })
 }

 const onWarehouseSelected=()=>{
    getShelfDataByWarehouse(); 
}

 const getShelfDataByWarehouse=()=>{
     return getShelfByWarehouse(selectedWarehouseId.value).then(res=>{
        shelfData.value=[{id:'',name:'All'},...res.data];
     })
 }
 
 const getBinDataByShelf=()=>{
    binData.value.length=0;
    selectedBinId.value='';
    return getBinByShelf(selectedShelfId.value).then(res=>{
        binData.value=[{id:'',name:'All'},...res.data];
        getGoodsDatya(true);
    })
 }
   
  const getGoodsDatya=(init:boolean)=>{ 
    if (init) {
        pgIndex.value = 1
      }  
     let orderField=goodsSort.value.split('-')[0];
     let orderType=goodsSort.value.split('-')[1];
     let binId:number=selectedBinId.value?Number(selectedBinId.value):0;
     getGoodsList(pgSize.value,pgIndex.value,orderField,orderType,selectedGoodsGroup.value,selectedGoodsClassifyId.value,selectedShelfId.value,binId, searchKey.value).then(res=>{
         goodsData.value=res.data.rows; 
         totalDate.value=res.data.total;
         totalPage.value=Math.ceil(totalDate.value/pgSize.value);  
     })
  }
 
  const onPrePage=()=>{
     if(pgIndex.value>1){
         pgIndex.value--; 
         getGoodsDatya(false);
     }
 }
 
 const onNextPage=()=>{
     if(pgIndex.value<totalPage.value){
         pgIndex.value++; 
         getGoodsDatya(false);
     }
 } 
 
 const onShowDetail=(item:any)=>{
    let orderField=goodsSort.value.split('-')[0];
    let orderType=goodsSort.value.split('-')[1];
    let binId:number=selectedBinId.value?Number(selectedBinId.value):0;
    let args={
        goodsId:item.goodsId,
        pgSize:pgSize.value,
        pgIndex:pgIndex.value,
        orderField:orderField,
        orderType:orderType,
        selectedGoodsGroup:selectedGoodsGroup.value,
        selectedGoodsClassifyId:selectedGoodsClassifyId.value,
        selectedWarehouseId:selectedWarehouseId.value,
        selectedShelfId:selectedShelfId.value,
        binId:binId,
        searchKey:searchKey.value
    }    
    router.push({name:'goodsdetail-mobile',query:{goodsId:item.goodsId}}).then(()=>{
             permission.addRouteHis(route.name as string);  
             commonHelper.setObjLocalStorage(storageKey,args);
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
             height: 19%; 
             background-color:rgb(247, 252, 252);
             .header-row{
                 margin-top: 10px; 
             }
         }
         .content-body{ 
             padding: 10px 15px 5px 0px; 
             height: 73%;
             margin-top: 5px; 
             background-color:rgb(247, 252, 252);
             overflow-y: scroll;
             .body-row{
                 text-align: left;
                 position: relative;
                 left: 8px; 
                 .body-item{
                     padding:2px; 
                     .item-card{  
                         .item-img{
                             position: relative; 
                             bottom: 10px;
                             float: left; 
                             width: 110px;
                             .img-goods{
                                 width: 100px;
                                 height: 100px; 
                                 display: block;
                                 border:1px dashed #d2d2d2;
                                 border-radius: 5px;
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
             .btn-page-pre{
                 height: 50px;position: fixed;bottom: 40%;left: 1%;opacity:.2
             }
             .btn-page-next{
                 height: 50px;position: fixed;bottom: 40%;right: 1%;opacity:.2
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
             height: 12%; 
             background-color:rgb(247, 252, 252);
             .header-row{
                 margin-top: 10px; 
             }
         }
         .content-body{ 
             padding: 10px 15px 5px 0px; 
             height: 78%;
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
                     .img-goods{
                         width: 100%;height: 210px; display: block;border:1px dashed #d2d2d2;border-radius: 5px;
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
             .btn-page-pre{
                 height: 80px;position: fixed;bottom: 40%;left: 2%;opacity:.2
             }
             .btn-page-next{
                 height: 80px;position: fixed;bottom: 40%;right: 2%;opacity:.2
             }
         }  
     } 
 } 
 </style>