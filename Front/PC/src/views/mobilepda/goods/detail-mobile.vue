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
                <div class="body-desc">
                    <el-row class="body-desc-scroll">
                        <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
                            <div class="body-item">
                                <span class="body-item-title">备件名称：</span>
                                <span class="body-item-info">{{ goodsData?.goodsName }}</span>  
                            </div>
                        </el-col>
                        <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
                            <div class="body-item"> 
                                <span class="body-item-title">备件型号：</span>
                                <span class="body-item-info"> {{ goodsData?.goodsModel }}</span>
                            </div>
                        </el-col>
                         <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
                            <div class="body-item">
                                <span class="body-item-title">备件分类：</span>
                                <span class="body-item-info">{{ goodsData?.goodsClassifyName }}</span>  
                            </div>
                        </el-col>
                         <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
                            <div class="body-item">
                                <span class="body-item-title">是否在SAP：</span>
                                <span class="body-item-info">{{ goodsData?.isInSAP }}</span>  
                            </div>
                        </el-col>
                         <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
                            <div class="body-item">
                                <span class="body-item-title">供应商：</span>
                                <span class="body-item-info">{{ goodsData?.supplier }}</span>  
                            </div>
                        </el-col> 
                         <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
                            <div class="body-item">
                                <span class="body-item-title">安全库存：</span>
                                <span class="body-item-info">{{ goodsData?.safetyInventory+' '+goodsData?.safetyInventoryUnitName }}</span>  
                            </div>
                        </el-col>
                         <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
                            <div class="body-item">
                                <span class="body-item-title">所属区域：</span>
                                <span class="body-item-info">{{ goodsData?.forArea }}</span>  
                            </div>
                        </el-col>
                         <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
                            <div class="body-item">
                                <span class="body-item-title">最小采购量：</span>
                                <span class="body-item-info">{{ goodsData?.purchaseMinimum+goodsData?.purchaseMinimumUnitName }}</span>  
                            </div>
                        </el-col>
                         <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
                            <div class="body-item">
                                <span class="body-item-title">采购周期：</span>
                                <span class="body-item-info">{{ goodsData?.purchaseCycle+' 天'}}</span>  
                            </div> 
                        </el-col>
                         <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
                            <div class="body-item">
                                <span class="body-item-title">储存规格：</span>
                                <span class="body-item-info">{{ goodsData?.goodsSpecificationName}}</span>  
                            </div>
                        </el-col>
                    </el-row> 
                    <div class="body-upload-item">
                        <span class="body-upload-title">上传图片</span>&nbsp;<span class="body-upload-info">(支持{{ photoLimit}}张)</span> 
                    </div>
                    <div class="body-upload"> 
                        <Upload :uploadParams="uploadParams" v-if="showUpload" @handleImgChanged="imgChanged" /> 
                    </div>
                </div>  
                <div class="body-foot">
                    <el-button class="foot-btn" v-if="permission.isPermisstion('GOODSMOBILEDETAILUPDATE')" :loading="submitBtnLoading" round type="success" @click="onSubmit">保存</el-button>
                </div> 
            </div>
        </div>
</template>
<script lang="ts" setup>
import {ref,defineEmits,defineProps,onMounted,onBeforeMount } from 'vue'  
import { useRouter, useRoute } from 'vue-router'  
import {getGoodsDetail,updateGoodsPhoto} from "@/api/baseinfo/goods"; 
import permission from '@/utils/system/permission';  
import Upload from '@/components/imgUpload/muiltUpload.vue';
import commonHelper from '@/utils/system/common-helper';
import { getPhotoLimit } from "@/api/common";
 
const route=useRoute();
const goodsId=ref(route.query.goodsId);
const goodsData:any=ref({});
const goodsPhotos=ref(new Array<any>()); 
const submitBtnLoading=ref(false); 
const photoLimit=ref(); 
const showUpload=ref(false);
const uploadParams=ref({
    uploadApi:'/Common/UploadGoodsPhoto', 
    limit:3,
    imgUrlList: [],
    validFileType:'image',
    validFileSize:10240,
    isEdit:true,
    titile:'点击上传图片',
    width:'80px',
    height:'80px', 
});

onMounted(()=>{  
    getPhotoLimitData();
    getGoodsDetailData(); 
})
 
const getGoodsDetailData=()=>{
    if(goodsId.value){
        getGoodsDetail(goodsId.value.toString()).then((res)=>{  
            goodsData.value=res.data;
            goodsPhotos.value=res.data.photos;  
            showUpload.value=true;
            uploadParams.value.imgUrlList=goodsData.value.photos.map((x:any)=>{
            return{
                name:x.fileName,
                url:x.url
            }
            }); 
         })
    } 
}

const getPhotoLimitData=()=>{
    getPhotoLimit().then(res=>{
        photoLimit.value=res.data;
        uploadParams.value.limit=photoLimit.value;
    })
}
 
const imgChanged=(imgList:Array<any>)=>{  
    goodsData.value.photos=imgList.map(x=>{
    return{
        fileName:x.name,
        url:x.url
    }
    })
} 

const onSubmit=()=>{
    goodsData.value.createUserId=permission.getOperator().userId;
    goodsData.value.createUserName=permission.getOperator().userName; 
    submitBtnLoading.value=true;
    updateGoodsPhoto(goodsData.value.goodsId,goodsData.value.photos).then(res=>{  
    }).finally(()=>{
        submitBtnLoading.value=false;
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
            height: 68%;
            margin-top: 5px; 
            background-color:rgb(247, 252, 252); 
            .body-desc{
                height: 60%;
                text-align: left;
                position: relative; 
                font-size: 12px; 
                padding-top: 6px;  
                .body-desc-scroll{
                    overflow-y: scroll;
                    height: 100%;
                    .body-item{
                        padding: 4px 0 0 5px;
                        margin-bottom: 10px;
                        background-color: #fff;
                        border-left: 3px solid #f7a500;
                        .body-item-title{ 
                            font-weight: 600;
                        } 
                        .body-item-info{ 
                            color: #888;
                        } 
                    }
                }
                .body-upload-item{
                    padding: 4px 0 0 5px;
                    margin-bottom: 10px;
                    background-color: #fff;
                    border-left: 3px solid #f7a500;
                    .body-upload-title{ 
                        font-weight: 600;
                    } 
                    .body-upload-info{ 
                        color: #888;
                    }  
                }
                .body-upload{
                    background-color: #fff;
                    border:1px solid #dbdbdb;
                    border-radius: 4px;
                    padding: 10px;
                    height: 70px;
                    line-height: 100px;
                    vertical-align: middle;
                }   
            }  
            .body-foot{
                height: 7%;
                position: absolute;
                bottom: 5px;
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
            height: 60%;
            margin-top: 5px; 
            background-color:rgb(247, 252, 252); 
            .body-desc{
                height: 60%; 
                text-align: left;
                position: relative; 
                font-size: 14px; 
                padding-top: 6px; 
                .body-desc-scroll{
                    overflow-y: scroll;
                    height: 100%;
                    .body-item{
                    padding: 10px 0 10px 5px;
                    margin-bottom: 15px;
                    background-color: #fff;
                    border-left: 3px solid #f7a500;
                    .body-item-title{ 
                         font-weight: 600;
                    } 
                    .body-item-info{ 
                        color: #888;
                    } 
                }
                } 
                .body-upload-item{
                    padding: 10px 0 10px 5px;
                    margin-bottom: 15px;
                    background-color: #fff;
                    border-left: 3px solid #f7a500;
                    .body-upload-title{ 
                        font-weight: 600;
                    } 
                    .body-upload-info{ 
                        color: #888;
                    }  
                }
                .body-upload{
                    background-color: #fff;
                    border:1px solid #dbdbdb;
                    border-radius: 4px;
                    padding: 10px;
                    height: 90px;
                    line-height: 100px;
                    vertical-align: middle;
                }   
            } 
            .body-foot{
                height: 7%;
                position: absolute;
                bottom: 5px;
                width: 100%;
                .foot-btn{
                    width: 96%;
                }
            }
        }  
    } 
}
   
</style>