<template>
   <div class="app-content">
            <div class="content-header">
             <el-row :gutter="20" >
                <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6" style="text-align: left;">
                    <el-select v-model="selectedGoodsGroup" class="m-2" style="width:100%" @change="onSelectGroup">
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
                    <el-select v-model="selectedWarehouseId"  class="m-2" style="width:100%" @change="getGoodsDatya">
                        <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">仓库</div></template>
                        <el-option v-for="item in warehouseData" :key="item.warehouseId" :label="item.warehouseName" :value="item.warehouseId">
                    </el-option>
                  </el-select>
                </el-col>
                <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                    <el-select v-model="goodsSort" class="m-2" style="width:100%" @change="getGoodsDatya">
                        <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">排序</div></template>
                        <el-option value="goodsName-asc" label="按名称升序"/>
                        <el-option value="goodsName-desc" label="按名称降序"/>
                        <el-option value="supplier-asc" label="按供应商升序"/>
                        <el-option value="supplier-desc" label="按供应商降序"/>
                  </el-select>
                </el-col>
             </el-row>
             <el-row class="header-row" :gutter="20">
                <el-col :xs="12" :sm="12" :md="12" :lg="6" :xl="6">
                    <el-select v-model="isTakeStockCurDate" class="m-2" style="width:100%" @change="getGoodsDatya">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">状态</div></template>
                         <el-option :value=true label="当月已盘点"/>
                         <el-option :value=false label="当月未盘点"/>  
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
                    <el-card class="item-card" shadow="never">  
                        <div class="item-img">
                            <el-image  hide-on-click-modal class="img-goods" :src="item.goodsPicture"  :zoom-rate="1.2" :preview-src-list="[item.goodsPicture]" :initial-index="0"  fit="cover" />
                            <img v-if="item.isTakeStockLock" class="img-lock" src="/public/icon-img/suo.png" >
                        </div>
                        <div class="item-desc" @click="onTakeStock(item)"> 
                            <div class="desc-title">{{ item.goodsName }} <span class="desc-info" v-if="item.goodsModel">{{ item.goodsModel }}</span></div> 
                            <div class="desc-info">分&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;类：{{item.goodsClassifyName}}</div>
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
              <div v-if="totalPage>1">
                <img src="/public/icon-img/left-circle-fill.png" alt="上一页" v-if="pgIndex>1"  class="btn-page-pre" @click="onPrePage">
                 <img src="/public/icon-img/left-circle-fill-dis.png" alt="上一页" v-else class="btn-page-pre">
                 <img src="/public/icon-img/right-circle-fill.png" alt="下一页" v-if="pgIndex<totalPage" class="btn-page-next" @click="onNextPage">
                 <img src="/public/icon-img/right-circle-fill-dis.png" alt="下一页" v-else class="btn-page-next">
              </div>
            </div>
            <MessageDrawer :options="msgDrawerOptions" @cancel="onCancel" @confirm="onSubmit"/>
        </div> 
</template>
<script lang="ts" setup>
import {ref,defineEmits,defineProps,onMounted } from 'vue'  
import { useRouter, useRoute } from 'vue-router'  
import {ArrowLeft,Search} from '@element-plus/icons-vue';    
import {getGoodsByKey,getGoodsTakeStockStatus,setTakeStockLockByGoods} from "@/api/inv/takestock";
import {getGoodsGroup,getGoodsClassify,getWarehouses} from '@/api/common'; 
import MessageDrawer from '../message-drawer.vue';
import permission from '@/utils/system/permission'; 
import commonHelper from "@/utils/system/common-helper";
import { deftClassifyGroup} from '@/config';
 
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
const selectedWarehouseId=ref('');
const searchKey=ref('');
const isTakeStockCurDate=ref(false);
const goodsSort=ref('goodsName-asc')
const curSelectGoods=ref();
const warehouseData=ref(new Array<any>());
var warehouseDataBuffer=new Array<any>();
const msgDrawerOptions=ref({
    show:false,
    title:'',
    message:'',
    type:'',
    data:null
});
 
 onMounted(()=>{  
    getGoodsGroupData().then(()=>{ 
        getWarehousesData().then(()=>{
            onSelectGroup();
    }); 
    }) 
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
    goodsClassifyData.value.length=0;
    selectedGoodsClassifyId.value=0;
    warehouseData.value.length=0;
    selectedWarehouseId.value='';
    let warehouseBygroup=warehouseDataBuffer.filter(f=>f.warehouseType==selectedGoodsGroup.value); 
    if(warehouseBygroup?.length>0){ 
        warehouseData.value=[{warehouseId:'',warehouseName:'All'},...warehouseBygroup] 
    }   
    getGoodsClassifyData();
    getGoodsDatya();
}
  
const getGoodsClassifyData=()=>{
    return getGoodsClassify(selectedGoodsGroup.value).then(res=>{
        goodsClassifyData.value=[{key:0,value:'All'},...res.data];
    })
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

 const getGoodsDatya=()=>{ 
    let orderField=goodsSort.value.split('-')[0];
    let orderType=goodsSort.value.split('-')[1];
    getGoodsByKey(pgSize.value,pgIndex.value,orderField,orderType,selectedGoodsGroup.value,selectedGoodsClassifyId.value,selectedWarehouseId.value, searchKey.value,isTakeStockCurDate.value).then(res=>{
        goodsData.value=res.data.rows; 
        totalDate.value=res.data.total;
        totalPage.value=Math.ceil(totalDate.value/pgSize.value);
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

const onTakeStock=(item:any)=>{
    getGoodsTakeStockStatus(item.goodsId).then(res=>{
        curSelectGoods.value=item;
        msgDrawerOptions.value.type='Message'
        msgDrawerOptions.value.title='盘点锁定提示';
        let msgIsModel=item.goodsModel?`,型号${item.goodsModel}`:''
        if(!res.data){   
            msgDrawerOptions.value.message=`您将开始对${item.goodsName}${msgIsModel}进行盘点，期间将对其锁定且无法进行出入库操作，是否确认继续？`; 
        } 
        else{
            msgDrawerOptions.value.message=`${item.goodsName}${msgIsModel}正在盘点中，是否确认进入继续盘点？`; 
        }
        msgDrawerOptions.value.show=true;
    }) 
}

const onCancel=()=>{
    msgDrawerOptions.value.show=false;
}
  
const onSubmit=()=>{
    setTakeStockLockByGoods([curSelectGoods.value.goodsId]).then(res=>{
        router.push({name:'takegoodssubmit',query:{goodsId:curSelectGoods.value.goodsId,warehouseId:selectedWarehouseId.value}}).then(()=>{
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
                            float: left; 
                            width: 120px;
                            .img-goods{
                                width: 113px;
                                height: 113px; 
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
                            width: 55%;
                            .desc-title{ 
                            font-weight: 600;
                            white-space: nowrap;
                            overflow: hidden;  
                            text-overflow: ellipsis; 
                            }
                            .desc-info{ 
                                color:#888;
                                padding-top: 3px;
                                white-space: nowrap;
                                overflow: hidden;  
                                text-overflow: ellipsis; 
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
                    .img-goods{
                        width: 100%;height: 210px; display: block;border:1px solid #e7e7e7;border-radius: 5px;
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
                        white-space: nowrap;
                        overflow: hidden;  
                        text-overflow: ellipsis; 
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