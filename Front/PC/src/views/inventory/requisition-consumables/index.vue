<template>
    <div class="content"> 
       <div class="content-header">
           <el-row >
               <el-col :span="8" class="header-logo"><img src="@/assets/header-logo-b.png" alt=""></el-col>
               <el-col :span="8"  class="header-title"><h3>{{ selectedGoodsGroupDesc }}领用</h3></el-col>
               <el-col :span="8" style="text-align: right;line-height: 100%;">
                <h5 style="position: relative;right: 5%; display: inline-block;" title="Go Home">
                   <span style="cursor: pointer;"  @click="toHome">
                        <el-icon style="font-size: 19px;"><HomeFilled /></el-icon>
                        <span style="position: relative;top:-3px;">主菜单</span>
                   </span>
                </h5>
            </el-col>
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
                <el-select v-model="selectedGoodsClassifyId"  class="m-2" style="width:100%" @change="getGoodsData">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">小类</div></template>
                         <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                        </el-option>
                </el-select>
              </el-col> 
              <el-col :span="6">
                <el-select v-model="selectedArea"  class="m-2" style="width:100%" @change="getGoodsData">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">适用产线</div></template>
                         <el-option value="" label="All"/> 
                         <el-option :label="item.key" :value="item.key" v-for="item in areaData"/>  
                   </el-select>
              </el-col>
              <el-col :span="6">
                  <el-input v-model="goodsSearchKey" placeholder="可按关键字检索"  clearable @clear="getGoodsData">
                  <template #append>
                    <el-button type="primary" @click="getGoodsData"><el-icon style="position: relative;top:4px;font-size: 15px;"><Search /></el-icon></el-button>
                  </template>
                </el-input>
              </el-col>
            </el-row>  
           </div>
           <div class="body-content" >   
               <el-row v-if="goodsData?.length>0" :gutter="20">
                <el-col :span="6" v-for="item in goodsData" :key="item.goodsId">
                   <div class="content-item" >
                    <div class="item-img"> 
                        <el-image  :id="`img_${item.goodsId}`" :style="{border:item.goodsPicture?'none':'1px dashed #cecdcd'}" style="width: 100%;height: 190px; border-radius: 4px;" hide-on-click-modal :src="item.goodsPicture"  :zoom-rate="1.2" :preview-src-list="[item.goodsPicture]" :initial-index="0"  fit="cover">
                            <template #error>
                            <div class="image-slot">
                                <div>No Picture</div>
                                <div><el-icon><PictureFilled /></el-icon></div>
                            </div>
                            </template> 
                        </el-image>    
                    </div>
                    <div class="item-desc">
                            <div class="desc-title" :title="item.goodsName+' '+item.goodsModel">{{ item.goodsName }}<span  v-if="item.goodsModel">{{ ' '+item.goodsModel }}</span><span style="position: absolute;right: 5%;">{{item.goodsClassifyName}}</span></div>
                            <div class="desc-info">SAP&nbsp;&nbsp;编&nbsp;&nbsp;码：{{item.goodsNo}}</div>
                            <div class="desc-info">供&nbsp;&nbsp;&nbsp;应&nbsp;&nbsp;&nbsp;&nbsp;商：{{ item.supplier }}</div> 
                            <div class="desc-info">安&nbsp;全&nbsp;库&nbsp;存：<span>{{ item.safetyInventory+item.safetyInventoryUnitName }}</span></div>
                            <div class="desc-info">是否在盘点：<span v-if="item.isTakeStockLock" style="color: #ff6600">是</span><span v-else>否</span></div> 
                            <div class="desc-info">库&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;存：
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
                            <div class="desc-info">存&nbsp;放&nbsp;库&nbsp;位：{{item.deftStockBin}}</div>
                            <div class="desc-info">
                                <el-input-number  v-model="item.quantity" placeholder="领用数量" :min="1" :max="10000" />
                                <el-select  v-model="item.unitId" class="m-2" style="width:20%" placeholder="单位">
                                <el-option v-for="unit in item.goodsUnitList" :key="unit.key" :label="unit.value" :value="unit.key">
                                </el-option>
                                </el-select>
                            </div> 
                    </div> 
                    <div v-if="!item.isNoStock&&!item.isTakeStockLock" class="item-chk" @click="selectGoods(item)">  
                        <img v-if="item.selected" src="/public/icon-img/jiarugouwuche2.png">
                        <img v-else src="/public/icon-img/jiarugouwuche1.png">    
                    </div>
                    <div class="item-tag" v-if="item.isNoStock||item.isTakeStockLock">
                        <img class="tag-stock" v-if="item.isNoStock" src="/public/icon-img/quehuo-biaoqian.png">   
                        <img class="tag-lock" v-else-if="item.isTakeStockLock" src="/public/icon-img/suo.png">    
                    </div>
                   </div>
                </el-col> 
               </el-row>
               <el-row v-else>
                <el-col><el-empty description="没有任何数据" /></el-col>
              </el-row>   
           </div> 
           <div class="content-order" id="orderImgBtn"> 
            <div class="order-car">
                <img v-if="selectedGoods?.length>0" src="/public/icon-img/gouwucheman1.png" title="点击查看本次领用清单" @click="onShowCurOrdersDrawer">
                <img v-else src="/public/icon-img/gouwuche.png"> 
            </div>
            <img class="order-list" src="/public/icon-img/dingdan_2.png" title="点击查看历史领用记录" @click="onShowHisOrderDrawer"> 
        </div> 
        <div v-if="totalPage>1">
            <img src="../../../../public/icon-img/left-circle-fill.png" alt="上一页" v-if="pgIndex>1"  class="btn-page-pre" @click="onPrePage">
            <img src="../../../../public/icon-img/left-circle-fill-dis.png" alt="上一页" v-else class="btn-page-pre">
            <img src="../../../../public/icon-img/right-circle-fill.png" alt="下一页" v-if="pgIndex<totalPage" class="btn-page-next" @click="onNextPage">
            <img src="../../../../public/icon-img/right-circle-fill-dis.png" alt="下一页" v-else class="btn-page-next">
        </div>
       </div>   
       <CurOrderDrawer :options="curOrderDrawerOptions" v-if="curOrderDrawerOptions.show" @submitRemove="onSubmitRemove" @submited="onSubmited"/> 
       <HisOrderDrawer :options="hisOrderDrawerOptions" v-if="hisOrderDrawerOptions.show" @submited=""/> 
   </div>
</template>

<script setup lang="ts"> 
defineOptions({
    name: "requisition-consumables"
  })
import { reactive, ref,onMounted,onBeforeUnmount } from "vue";
import { useI18n } from 'vue-i18n';    
import msg from "@/utils/system/message";  
import { ElLoading } from 'element-plus';
import CurOrderDrawer from "./curOrderDrawer.vue";
import HisOrderDrawer from "./hisOrderDrawer.vue";
import { Search,PictureFilled,HomeFilled } from '@element-plus/icons-vue';
import {getGoodsGroup,getGoodsClassify,getGoodsByPage,getUnits,getAreas} from '@/api/common';   
import { deftClassifyGroup ,disableClassifyGroupSelect} from '@/config'; 
 
const goodsGroupData=ref(new Array<any>()); 
const selectedGoodsGroup=ref('Consumables'); 
const selectedGoodsGroupDesc=ref(''); 
const goodsClassifyData=ref(new Array<any>()); 
const selectedGoodsClassifyId=ref(0);
const selectedArea=ref("");
const goodsSearchKey=ref('');
const goodsData=ref(new Array<any>());
const selectedGoods=ref(new Array<any>());  
const unitData=ref(new Array<any>());   
const areaData=ref(new Array<any>());
const pgSize=ref(8);
const pgIndex=ref(1); 
const totalDate=ref(0);
const totalPage=ref(0);
const curOrderDrawerOptions= ref({
  show: false,
  title: '', 
  type:'', 
  data:{}
});
const hisOrderDrawerOptions= ref({
  show: false,
  title: '', 
  type:'', 
  data:{}
});
var loadingOption={
 target:'', 
 text:"页面加载中...", 
 background: 'rgba(6, 255, 255, 0.118)' 
}
 
onMounted(()=>{  
    loadingOption.target='.content';
    let loadingInstance= ElLoading.service(loadingOption);  
    getGoodsGroupData();
    getUnitData().then(()=>{
        onSelectGroup()?.finally(()=>{
            loadingInstance.close()
        });
    }) 
    getAreas().then(res=>areaData.value=res.data);
})
 
const getGoodsGroupData=()=>{
   return getGoodsGroup().then(res=>{
        goodsGroupData.value=res.data;
        selectedGoodsGroupDesc.value=goodsGroupData.value.find(f=>f.key==selectedGoodsGroup.value).value;
    })
}

const onSelectGroup=()=>{ 
    selectedGoods.value.length=0;
    getGoodsClassifyData();
   return getGoodsData();
}
  
const getGoodsClassifyData=()=>{
    if(selectedGoodsGroup.value){
         getGoodsClassify(selectedGoodsGroup.value).then(res=>{
            goodsClassifyData.value=[{key:0,value:'All'},...res.data];
        })
    } 
}
 
const getGoodsData=(init:boolean=true)=>{ 
    if (init) {
        pgIndex.value = 1
      }  
     let orderField='';
     let orderType=''; 
     if(selectedGoodsGroup.value){ 
        return getGoodsByPage(pgSize.value,pgIndex.value,orderField,orderType,selectedGoodsGroup.value,selectedGoodsClassifyId.value,selectedArea.value,goodsSearchKey.value).then(res=>{
         goodsData.value=res.data.rows; 
         totalDate.value=res.data.total;
         totalPage.value=Math.ceil(totalDate.value/pgSize.value);  
         goodsData.value.forEach(o => {
                setDeftStokAndUnit(o);   
                if(o.stockInfo?.length==1){
                    o.unitId=o.stockInfo[0].unitId;
                    o.goodsUnitList=[{key:o.stockInfo[0].unitId,value:o.stockInfo[0].unitName}]
                }
                else{
                    let hasStock=o.stockInfo.filter((f:any)=>f.stock>0);
                    if(hasStock.length>0){
                        o.goodsUnitList= hasStock.map((m:any)=>{
                            return {
                                key:m.unitId,
                                value:m.unitName
                            }
                        })
                    }
                    else{
                        o.goodsUnitList= o.stockInfo.map((m:any)=>{
                            return {
                                key:m.unitId,
                                value:m.unitName
                            }
                        })
                    } 
                }
                if(o.stockInfo.filter((f:any)=>f.stock>0).length==0){
                    o.isNoStock=true;
                }
                if(selectedGoods.value.length>0){
                    o.selected=false;
                        selectedGoods.value.forEach(n=>{
                            if(o.goodsId==n.goodsId){
                                o.selected=true;
                            }
                        })
                    }
                }); 
     })
     } 
  }
 
  const onPrePage=()=>{
     if(pgIndex.value>1){
         pgIndex.value--; 
         getGoodsData(false);
     }
 }
 
 const onNextPage=()=>{
     if(pgIndex.value<totalPage.value){
         pgIndex.value++; 
         getGoodsData(false);
     }
 }
 
 const setDeftStokAndUnit=(item:any)=>{
    item.stockInfo=[]
    if(item.minPackageUnitName==item.packageUnitName&&item.packageUnitName==item.maxPackageUnitName){
        item.stockInfo.push({
            stock:item.standardPackageStock,
            unitId:unitData.value.find(f=>f.value==item.packageUnitName).key,
            unitName:item.packageUnitName
        })
    }
    else if(item.minPackageUnitName!=item.packageUnitName&&item.packageUnitName==item.maxPackageUnitName){ 
        item.stockInfo.push({
                stock:item.standardPackageStock,
                unitId:unitData.value.find(f=>f.value==item.packageUnitName).key,
                unitName:item.packageUnitName
            })
        item.stockInfo.push({
                stock:item.minPackageStock,
                unitId:unitData.value.find(f=>f.value==item.minPackageUnitName).key,
                unitName:item.minPackageUnitName
            })
    }
    else if(item.minPackageUnitName==item.packageUnitName&&item.packageUnitName!=item.maxPackageUnitName){
        item.stockInfo.push({
                stock:item.maxPackageStock,
                unitId:unitData.value.find(f=>f.value==item.maxPackageUnitName).key,
                unitName:item.maxPackageUnitName
            })
        item.stockInfo.push({
            stock:item.standardPackageStock,
            unitId:unitData.value.find(f=>f.value==item.packageUnitName).key,
            unitName:item.packageUnitName
        })
    }
    else{
        item.stockInfo.push({
            stock:item.maxPackageStock,
            unitId:unitData.value.find(f=>f.value==item.maxPackageUnitName).key,
            unitName:item.maxPackageUnitName
        })
        item.stockInfo.push({
            stock:item.standardPackageStock,
            unitId:unitData.value.find(f=>f.value==item.packageUnitName).key,
            unitName:item.packageUnitName
        })
        item.stockInfo.push({
            stock:item.minPackageStock,
            unitId:unitData.value.find(f=>f.value==item.minPackageUnitName).key,
            unitName:item.minPackageUnitName
        })
    }
 }

 const getUnitData=()=>{
   return getUnits('Pack').then(res=>{
        unitData.value=res.data;
    })
 }

const selectGoods=(item:any)=>{    
    if(!item.unitId || Number(item.unitId)==0){
        msg.deftAuto("请选择单位");
    }
    else if(!item.quantity || Number(item.quantity)<=0){
        msg.deftAuto("请输入领用数量");
    }
    else{
        item.selected=!item.selected;  
        if(item.selected){
            item.unitName=unitData.value.find(f=>f.key==item.unitId).value;
            selectedGoods.value.push(item);
        }
        if(item.selected && item.goodsPicture){ 
            let img:any=document.getElementById(`img_${item.goodsId}`); 
            let old_position = img.getBoundingClientRect();    
            let old_x = old_position.left;  
            let old_y = old_position.top;    
            let newImg:any = document.createElement("img");  
            newImg.src=item.goodsPicture;
            newImg.height=img.height*.8;
            newImg.width=img.width*.6; 
            newImg.style.position = "absolute"; 
            newImg.style.left=old_x+'px';
            newImg.style.top=old_y+'px';
            document.body.appendChild(newImg);  
            let orderBtnDiv:any= document.getElementById('orderImgBtn');
            let new_Position = orderBtnDiv.getBoundingClientRect();
            let new_x = new_Position.left;  
            let new_y = new_Position.top;   
            let domArgs={
                x:Math.ceil(new_x),
                y:Math.ceil(new_y),
                height:newImg.height,
                width:newImg.width
            }
            animate(newImg,domArgs,()=>{ document.body.removeChild(newImg)}) 
        } 
    } 
}

const animate=(obj:any, domArgs:any, callback:Function) =>{   
    clearInterval(obj.timer);  
    obj.timer = setInterval(function() { 
        // 缓动动画原理（目标值-现在的长度）/10 做为每次移动的距离 步长
        var stepX = (domArgs.x - obj.offsetLeft) / 10;
        var stepY= (domArgs.y - obj.offsetTop) / 10;
        var stepHeight=domArgs.height/40;
        var stepWidth=domArgs.width/40; 
        stepX = stepX > 0 ? Math.ceil(stepX) : Math.floor(stepX);
        stepY = stepY > 0 ? Math.ceil(stepY) : Math.floor(stepY);  
        if (obj.offsetLeft == domArgs.x || obj.offsetTop==domArgs.y) { 
            clearInterval(obj.timer); 
            if(callback)
                callback();
        }
        obj.style.left = obj.offsetLeft + stepX + 'px';
        obj.style.top = obj.offsetTop + stepY + 'px';
        obj.height=obj.height-stepHeight;
        obj.width=obj.width-stepWidth;
    }, 20);
} 
 
const onShowCurOrdersDrawer=()=>{ 
    curOrderDrawerOptions.value.type=selectedGoodsGroup.value;
    curOrderDrawerOptions.value.data=selectedGoods.value.concat();
    curOrderDrawerOptions.value.show=true;   
}

const onSubmitRemove=(details:any)=>{
    selectedGoods.value=details;
    goodsData.value.forEach(o => {  
    if(selectedGoods.value.length>0){
        o.selected=false;
            selectedGoods.value.forEach(n=>{
                if(o.goodsId==n.goodsId){
                    o.selected=true;
                }
            })
        }
    }); 
}

const onSubmited=()=>{
    selectedGoods.value.length=0;
    goodsData.value.forEach(f=>f.selected=false);
}

const onShowHisOrderDrawer=()=>{ 
    hisOrderDrawerOptions.value.show=true;
    hisOrderDrawerOptions.value.type=selectedGoodsGroup.value;
}

const toHome=()=>{
    window.history.back();
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
               .content-item{    
                margin-top:10px;
                padding: 15px;
                background:rgba(255, 255, 255, 0.7);   
                position: relative; 
                border-radius: 8px;
                    .item-img{  
                        width: 100%;
                        height: 190px;
                        text-align: left; 
                        border-radius: 4px;
                        background-color: #d1cfcf29; 
                        .image-slot{
                            display: flex;
                            justify-content: center;
                            align-items: center;
                            width: 100%;
                            height: 100%; 
                            color: var(--el-text-color-secondary);
                            font-size: 30px;
                            .el-icon {
                             font-size: 30px;
                            }
                        }
                    }
                    .item-desc{   
                        text-align: left; 
                        margin-top: 5px;
                        .desc-title{ 
                        font-size:15px; 
                        font-weight: 600; 
                        width: 100%;
                        overflow: hidden;
                        white-space: nowrap;
                        text-overflow: ellipsis;
                        -o-text-overflow: ellipsis; 
                        }
                        .desc-info{
                        font-size:13px; 
                        color:#464646;
                        padding-top: 4px; 
                        }
                    }
                    .item-chk{
                        position: absolute;
                        bottom: 0;  
                        right: 3%;
                        padding: 10px;
                        cursor: pointer;
                        img{
                            height: 30px;
                            opacity: .6;
                        }
                    }
                    .item-tag{ 
                        .tag-stock{
                            position: absolute;
                            bottom:0;
                            right: 0;
                            height: 25px;
                            opacity: .8;
                        }
                        .tag-lock{
                            position: absolute;
                            top:0;
                            left: 0;
                            height: 100px;
                            opacity: .85;
                        }
                    }
                } 
           }
           .content-order{
                    position: absolute;
                    right: 2%;
                    bottom: 28%;
                    height: 60px;
                    width: 60px; 
                    padding: 10px; 
                    border-radius: 50%;  
                    background:rgba(255, 255, 255, 1); 
                    cursor: pointer; 
                    .order-car{ 
                        img{
                            height: 40px; 
                            position: relative; 
                            text-align: center;
                            top: 12px; 
                        }
                    }
                    .order-list{
                        height: 82px; 
                        position: relative; 
                        text-align: center;
                        top: 66px; 
                        right: 10px;
                    } 
                }
                .btn-page-pre{
                 height: 60px;position: fixed;bottom: 40%;left: 1%;opacity:.6;z-index: 999;
             }
             .btn-page-next{
                 height: 60px;position: fixed;bottom: 40%;right: 1%;opacity:.6;z-index: 999;
             }
       } 
   }
</style>