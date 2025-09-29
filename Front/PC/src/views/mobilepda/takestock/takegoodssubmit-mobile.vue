<template>
        <div class="app-content">
            <div class="content-header">
                <el-carousel class="header-carousel"  v-if="goodsPhotos.length>0">
                    <el-carousel-item class="header-carousel-item" v-for="item in goodsPhotos" :key="item.fileId">   
                        <img :src="item.url" alt=""/>
                    </el-carousel-item>
                </el-carousel>   
            </div>
            <div class="content-body">  
                <div class="body-header">
                    <el-row>
                        <el-col >
                            <div class="body-header-desc">
                                <span class="body-header-title">{{ goodsData?.goodsName }}</span> 
                                <span class="body-header-info"> {{ goodsData?.goodsModel }}</span>
                            </div>
                        </el-col>
                    </el-row>
                </div> 
                <div class="body-row">
                    <el-row v-if="goodsData.inventoryDetails">
                        <el-col :xs="24" :sm="12" :md="12" :lg="6" :xl="6" v-for="item in goodsData.inventoryDetails" class="body-item">
                            <el-card class="item-card" shadow="never">   
                                <div class="item-desc">  
                                    <div class="desc-info">
                                        <div><img src="/public/icon-img/cangku5.png" height="18" style="margin-bottom: -3px;">
                                        {{ item.warehouseName }}</div>
                                    </div>
                                    <div class="desc-info">
                                        <img src="/public/icon-img/huojia.png" height="18" style="margin-bottom: -3px;">
                                        {{ item.shelfName }}
                                    </div>
                                    <div class="desc-info">
                                        <img src="/public/icon-img/xuanzekuwei.png" height="18" style="margin-bottom: -3px;">
                                        {{ item.binName }}
                                    </div>
                                    <div class="desc-info">
                                        <img src="/public/icon-img/icon_chakuc.png" height="18" style="margin-bottom: -3px;">
                                        {{ item.cellNo?item.cellNo:'None' }}
                                    </div>
                                    <div class="desc-info">
                                        <img src="/public/icon-img/huowu_3.png" height="18" style="margin-bottom: -3px;">
                                        <el-input class="info-input" v-model="item.stockActual" type="number"/>
                                        <span class="info-input-desc">{{ item.unitName }} </span> 
                                    </div> 
                                    <div class="desc-info">
                                        <img src="/public/icon-img/xinzengshengchanjihua.png" height="18" style="margin-bottom: -3px;">
                                        <span @click="onEditReason(item)"  style="max-width: 80%;overflow: hidden;display: inline-block;white-space: nowrap;text-overflow: ellipsis;margin-bottom: -6px; ">{{ item.remark}}</span>
                                        <el-icon @click="onEditReason(item)" title="点击编辑原因" style="cursor: pointer;margin-left: 5px;color: #67c23a;position: relative;bottom: -3px;margin-left: 15px;"><EditPen /></el-icon>
                                    </div>
                                </div> 
                            </el-card>
                        </el-col>
                    </el-row>
                    <el-row v-else>
                        <el-col><el-empty description="没有任何数据" /></el-col>
                    </el-row>
                </div> 
                <div class="body-foot">
                    <el-button class="foot-btn" v-if="showSubmitBtn" round type="success" @click="onAffirm">盘点确认</el-button>
                </div>
                <MessageDrawer :options="msgDrawerOptions" @cancel="onCancel" @confirm="onSubmit"/>
                <ReasonEditModal :layer="reasonEditOption"  @dataSubmit="submitReason" v-if="reasonEditOption.show"/>
            </div>
        </div>
</template>
<script lang="ts" setup>
import {ref,reactive,defineEmits,defineProps,onMounted,onBeforeMount } from 'vue'  
import { useRouter, useRoute } from 'vue-router'  
import {getGoodsInventoryDetail,addTaskStockOrderByGoods,getOptions} from "@/api/inv/takestock";
import MessageDrawer from '../message-drawer.vue';
import permission from '@/utils/system/permission';  
import msg from "@/utils/system/message";
import {EditPen}  from '@element-plus/icons-vue';
import ReasonEditModal from './reasonLayer.vue';
import { LayerInterface } from "@/components/layer/index.vue";

const route=useRoute();
const goodsId=ref(route.query.goodsId);
const goodsData:any=ref({});
const goodsPhotos=ref(new Array<any>()); 
const showSubmitBtn=ref(true);
const reasonTypeOptionData=ref(new Array<any>());
const curEditDetil=ref();
const submitData=ref({
    goodsId:goodsData.value?.goodsId,
    goodsName:goodsData.value?.goodsName,
    goodsClassifyGroup:goodsData.value?.goodsClassifyGroup,
    goodsModel:goodsData.value?.goodsModel,
    createUserId:'',
    createUserName:'', 
    unitPrice:0,
    totalPrice:0,
    priceUnit:'',
    inventoryDetails:goodsData.value?.inventoryDetails
});
const msgDrawerOptions=ref({
    show:false,
    title:'',
    message:'',
    type:'',
    data:null,
    btnLoading:false
});

const reasonEditOption:LayerInterface = reactive({
        show: false,
        title: "原因分析",
        showButton: true,
        btnLoading:false,
        width:"95%",
        data:null , 
        options:{},
        otherButton:{}
      });

onMounted(()=>{  
    getGoodsDetailData();
    getOptions().then(res=>{
        reasonTypeOptionData.value=res.data.reasonTypeOptions;
        reasonTypeOptionData.value.unshift({optionKey:'',optionName:'无'});
      })
})
 
const getGoodsDetailData=()=>{
    if(goodsId.value){
        getGoodsInventoryDetail(goodsId.value.toString()).then((res)=>{  
            if(res.data.inventoryDetails?.length>0){
                res.data.inventoryDetails.forEach((item:any) => { 
                    item.stockActual=item.stock;
                });
                goodsData.value=res.data;
                goodsPhotos.value=res.data.photos; 
                submitData.value.goodsId=goodsData.value?.goodsId;
                submitData.value.goodsName=goodsData.value?.goodsName;
                submitData.value.goodsClassifyGroup=goodsData.value?.goodsClassifyGroup;
                submitData.value.goodsModel=goodsData.value?.goodsModel;
                submitData.value.unitPrice=goodsData.value?.costPrice;
                submitData.value.priceUnit=goodsData.value?.priceUnitName;
                submitData.value.inventoryDetails=goodsData.value?.inventoryDetails; 
            } 
         })
    } 
}

const onEditReason=(item:any)=>{  
    curEditDetil.value=item;
    reasonEditOption.data=item;
    reasonEditOption.show=true;
}

const submitReason=(data:any)=>{
    curEditDetil.value.remark=data.remark;
    curEditDetil.value.remarkType=data.remarkType;
    curEditDetil.value.remarkTypeDesc=data.remarkTypeDesc;
    reasonEditOption.show=false;
}

const onAffirm=()=>{ 
    msgDrawerOptions.value.type='Message'
    msgDrawerOptions.value.title='盘点确认提示';
    msgDrawerOptions.value.message=`是否确认对${goodsData.value.goodsName},型号${goodsData.value.goodsModel}的库存修改？`; 
    msgDrawerOptions.value.show=true;
}

const onCancel=()=>{
    msgDrawerOptions.value.show=false;
}

const onSubmit=()=>{
    for(let index in submitData.value.inventoryDetails){ 
        if(submitData.value.inventoryDetails[index].stockActual<0){
            msg.errorAuto('实际库存数量不能小于0');
            return;
        }
    } 
    submitData.value.createUserId=permission.getOperator().userId;
    submitData.value.createUserName=permission.getOperator().userName; 
    msgDrawerOptions.value.btnLoading=true;
    addTaskStockOrderByGoods(submitData.value).then(res=>{
        msgDrawerOptions.value.show=false;
        showSubmitBtn.value=false;
    }).finally(()=>{
        msgDrawerOptions.value.btnLoading=false;
    })
}
</script>
<style lang="scss" scoped>
@media screen and (max-width: 450px){
    :deep .el-input__inner{
       border-radius: 0; border-left: none;border-top: none;border-right: none !important;
    } 
    .app-content{
        height: 95%;
        background-color:#fff;
        padding:0 15px;
        .content-header{  
            padding:10px 0;
            height: 20%; 
            background-color:rgb(247, 252, 252); 
            .header-carousel{ 
                height: 100%;
                img{
                   width: 100%; height:100%;object-fit:cover; 
                } 
            }
           
        }
        .content-body{ 
            position: relative;
            padding: 10px 5px 5px 5px; 
            height: 67%;
            margin-top: 5px; 
            background-color:rgb(247, 252, 252); 
            .body-header{
                height: 7%;
                text-align: left;
                position: relative; 
                font-size: 14px; 
                padding-top: 6px;  
                .body-header-desc{ 
                    border-left: 3px solid #f7a500; 
                    background-color: #fff;
                    padding: 3px 0 3px 3px;
                    border-radius: 3px;
                    .body-header-title{ 
                    font-weight: 600;
                    }
                    .body-header-info{
                        font-weight: 600;
                        color: #888;
                    }
                }  
            }
            .body-row{
                height: 84%;
                overflow-y: scroll;
                text-align: left;
                position: relative; 
                .body-item{ 
                    padding:2px 0; 
                    .item-card{     
                        .item-desc{   
                            position: relative;
                            bottom: 8px;
                            left: 5px;
                            font-size: 12px;
                            float: left; 
                            width: 100%;
                            .desc-info{  
                                // background-color: #f7f7f7;
                                // margin-bottom: 1px; 
                                width: 100%;
                                color:#888;
                                padding: 3px 0; 
                                img{
                                    position: relative;
                                    bottom: -4px;
                                    padding: 3px;
                                } 
                                .info-input{
                                    width: 35%; 
                                    position: relative;
                                    left: 5px;
                                    bottom: 3px; 
                                    
                                }
                                .info-input-desc{
                                    margin-left: 20px;
                                }
                            }
                        }
                    }
            }
            } 
            .body-foot{ 
                position: absolute;
                bottom: 0px;
                width: 100%;
                .foot-btn{
                    width: 96%;
                }
            }
        }  
    } 
}

@media screen and (min-width: 450px){
    :deep .el-input__inner{
       border-radius: 0; border-left: none;border-top: none;border-right: none !important;
    }
    .app-content{
        height: 95%;
        background-color:#fff;
        padding:0 15px;
        .content-header{  
            padding:10px 0;
            height: 32%; 
            background-color:rgb(247, 252, 252); 
            .header-carousel{
                height: 100%;
                img{
                   width: 100%; height:100%;object-fit:cover; 
                } 
            }
        }
        .content-body{ 
            position: relative;
            padding: 10px 5px 5px 5px; 
            height: 62%;
            margin-top: 5px; 
            background-color:rgb(247, 252, 252); 
            .body-header{
                height: 7%; 
                text-align: left;
                position: relative; 
                font-size: 14px; 
                padding-top: 6px; 
                .body-header-desc{
                    padding: 3px 0 3px 3px;
                    border-left: 3px solid #f7a500;
                    background-color: #fff;
                    border-radius: 3px;
                    .body-header-title{ 
                    font-weight: 600;
                    }
                    .body-header-info{
                        font-weight: 600;
                        color: #888;
                    }
                }  
            }
            .body-row{
                height: 92%;
                overflow-y: scroll;
                text-align: left;
                position: relative;
                left: 11px; 
                .body-item{ 
                    padding:3px 5px 5px 5px;  
                    .item-card{    
                        .item-desc{  
                            position: relative;
                            bottom: 8px;
                            left: 5px;
                            font-size: 14px;
                            float: left; 
                            .desc-info{  
                                color:#888;
                                padding: 5px 0;
                                img{
                                    position: relative;
                                    bottom: -4px;
                                } 
                                .info-input{
                                    width: 35%; 
                                    position: relative;
                                    left: 5px;
                                    bottom: 3px; 
                                    font-size: 14px;
                                }
                                .info-input-desc{
                                    margin-left: 20px;
                                }
                            }
                        }
                    }
            }
            } 
            .body-foot{ 
                position: absolute;
                bottom: 3%;
                width: 100%;
                .foot-btn{
                    width: 96%;
                }
            }
        }  
    } 
}
   
</style>