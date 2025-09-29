<template>
        <div class="app-content"> 
            <div class="content-body">  
                <div class="body-header">
                    <el-row>
                        <el-col >
                            <div class="body-header-desc">
                                <span class="body-header-title">{{ binData?.warehouseName }}</span>  
                                <el-icon class="header-desc-icon"><CaretRight /></el-icon>
                                <span class="body-header-title">{{ binData?.shelfName }}</span>  
                                <el-icon class="header-desc-icon"><CaretRight /></el-icon>
                                <span class="body-header-title">{{ binData?.binName }}</span>  
                            </div>
                        </el-col>
                    </el-row>
                </div> 
                <div class="body-row">
                    <el-row v-if="binData?.inventoryDetails">
                        <el-col :xs="24" :sm="12" :md="12" :lg="6" :xl="6" v-for="item in binData.inventoryDetails" class="body-item">
                            <el-card class="item-card" shadow="never">   
                                <div class="item-img">
                                    <el-image  hide-on-click-modal class="img-goods" :src="item.goodsPicture"  :zoom-rate="1.2" :preview-src-list="[item.goodsPicture]" :initial-index="0"  fit="cover" />
                                </div>
                                <div class="item-desc">  
                                    <div class="desc-info">名称：<span class="info-title">{{ item.goodsName }}</span></div>
                                    <div class="desc-info">分类：{{item.goodsClassifyName}}</div> 
                                    <div class="desc-info">型号：{{ item.goodsModel }}</div> 
                                    <div class="desc-info">料箱：{{ item.cellNo?item.cellNo:'None' }} </div>
                                    <div class="desc-info" style="margin-top: -5px;">库存：<el-input class="info-input" v-model="item.stockActual" type="number"/><span class="info-input-desc">{{ item.unitName }}</span></div> 
                                    <div class="desc-info" style="margin-top: -5px;">原因：
                                        <span @click="onEditReason(item)" style="max-width: 30%;overflow: hidden;display: inline-block;white-space: nowrap;text-overflow: ellipsis;margin-bottom: -3px; ">{{ item.remark}}</span>
                                        <el-icon @click="onEditReason(item)" title="点击编辑原因" style="cursor: pointer;color: #67c23a;position: relative;bottom: -3px;margin-left: 15px;"><EditPen /></el-icon>
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
import {getBinInventoryDetail,addTaskStockOrderByBin} from "@/api/inv/takestock";
import MessageDrawer from '../message-drawer.vue';
import permission from '@/utils/system/permission';  
import {CaretRight} from '@element-plus/icons-vue'; 
import msg from "@/utils/system/message";
import { LayerInterface } from "@/components/layer/index.vue";
import ReasonEditModal from './reasonLayer.vue';
import {EditPen}  from '@element-plus/icons-vue';

const route=useRoute();
const binId:any=ref(route.query.binId);
const binData:any=ref();
const goodsPhotos=ref(new Array<any>()); 
const showSubmitBtn=ref(true);  
const curEditDetil=ref();
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
    getBinDetailData();
})
 
const getBinDetailData=()=>{
    if(binId.value){
        getBinInventoryDetail(binId.value).then((res)=>{  
            if(res.data.inventoryDetails?.length>0){
                res.data.inventoryDetails.forEach((item:any) => { 
                    item.stockActual=item.stock;
                });
                binData.value=res.data;
                goodsPhotos.value=res.data.photos;  
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
    msgDrawerOptions.value.message=`是否确认对库位${binData.value.binName}的库存修改？`; 
    msgDrawerOptions.value.show=true;
}

const onCancel=()=>{
    msgDrawerOptions.value.show=false;
}

const onSubmit=()=>{
    for(let index in binData.value.inventoryDetails){ 
        if(binData.value.inventoryDetails[index].stockActual<0){
            msg.errorAuto('实际库存数量不能小于0');
            return;
        }
    } 
    binData.value.createUserId=permission.getOperator().userId;
    binData.value.createUserName=permission.getOperator().userName;  
    msgDrawerOptions.value.btnLoading=true;
    addTaskStockOrderByBin(binData.value).then(res=>{
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
        .content-body{ 
            position: relative;
            padding: 10px 5px 5px 5px; 
            height: 95%;
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
                    border-radius: 3px;
                    border-left: 3px solid #f7a500;
                    background-color: #fff;
                    .body-header-title{ 
                    font-weight: 600;
                    } 
                    .header-desc-icon{
                        color: #f7a500;
                        vertical-align: middle;
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
                        padding-top: 5px;
                        .item-img{
                            position: relative; 
                            bottom: 10px;
                            float: left; 
                            .img-goods{
                                width: 150px;
                                height: 150px; 
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
                            .desc-info{  
                                color:#888;
                                padding: 5px 0;  
                                .info-input{
                                    display :inline-block;
                                    width: 25%;  
                                    bottom: 3px; 
                                }
                                .info-input-desc{
                                    margin-left: 20px;
                                }
                                .info-title{
                                    font-weight: 600;
                                    color: #393939;
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

@media screen and (min-width: 450px){
    :deep .el-input__inner{
       border-radius: 0; border-left: none;border-top: none;border-right: none !important;
    }
    .app-content{
        height: 95%;
        background-color:#fff;
        padding:0 15px; 
        .content-body{ 
            position: relative;
            padding: 10px 5px 5px 5px; 
            height: 95%;
            margin-top: 5px; 
            background-color:rgb(247, 252, 252); 
            .body-header{
                height: 7%; 
                text-align: left;
                position: relative; 
                font-size: 14px; 
                padding-top: 6px; 
                .body-header-desc{
                    border-radius: 3px;
                    padding: 5px 0 5px 3px;
                    border-left: 3px solid #f7a500;
                    background-color: #fff;
                    .body-header-title{ 
                    font-weight: 600;
                    }
                    .header-desc-icon{
                        color: #f7a500;
                        vertical-align: middle;
                    }
                }  
            }
            .body-row{
                height: 86%;
                overflow-y: scroll;
                text-align: left;
                position: relative;
                left: 11px; 
                .body-item{ 
                    padding:4px;  
                    .item-card{    
                        .item-img{
                            position: relative; 
                            bottom: 10px;
                            width: 100%;
                            .img-goods{
                                width: 100%;height: 210px; display: block;border:1px dashed #d2d2d2;border-radius: 5px;
                            }  
                        }
                        .item-desc{  
                            padding: 10px;
                            font-size: 14px; 
                            .desc-info{  
                                color:#888;
                                padding: 5px 0; 
                                .info-input{
                                    width: 50%; 
                                    position: relative; 
                                    bottom: 3px; 
                                    font-size: 14px; 
                                }
                                .info-input-desc{
                                    margin-left: 20px;
                                }
                                .info-title{
                                    font-weight: 600;
                                    color: #393939;
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