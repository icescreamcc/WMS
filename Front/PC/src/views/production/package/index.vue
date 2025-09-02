<template>
    <div class="content"> 
       <div class="content-header">
           <el-row >
               <el-col :span="8" class="header-logo"><img src="@/assets/header-logo-b.png" alt=""></el-col>
               <el-col :span="8"  class="header-title"><h3>成品下线包装操作</h3></el-col>
           </el-row> 
       </div>
       <div class="content-body" > 
        <div class="body-query"> 
            <el-row justify="end">
                <el-col :span="13" style="text-align: left;">
                    <div style="padding: 3px 0 0 10px;float: left;">
                        <el-button class="search-btn" :disabled="pgIndex==1" @click="onPrePage"><el-icon style="position: relative;top: 2.5px;"><CaretLeft /></el-icon><span>上一页</span></el-button>  
                        <span style="display: inline-block;padding: 0 10px;margin: 0 5px;color: #fff;">{{ pgIndex }}</span>
                        <el-button class="search-btn" :disabled="pgIndex==totalPage" @click="onNextPage"><span>下一页</span><el-icon style="position: relative;top: 2.5px;"><CaretRight /></el-icon></el-button>  
                        <el-button class="search-btn"  @click="onRefresh"><span></span><el-icon style="position: relative;top: 2.5px;"><Refresh /></el-icon></el-button>  
                        <!-- <span style="display: inline-block;padding: 0 15px;margin: 0 5px;color: #fff;">共{{ totalPage }}页</span> -->
                    </div>
                </el-col>
                <el-col :span="2" style="text-align: right;vertical-align: middle;margin-top: 10px;">
                    <el-checkbox v-model="isNotPackage" label="未包装" name="type"  size="large" @change="onQuery" /> 
                </el-col>
                <el-col :span="1"></el-col>
                <el-col :span="8"> 
                    <div class="layout-container-form-search" style="padding: 3px 10px 0 0;"> 
                        <el-input v-model="searchKey" placeholder="请输入关键词进行检索(生产订单号/发货型号/小车码/库位号)" size="small"></el-input>
                        <el-button type="primary"  icon="el-icon-search" class="search-btn" @click="onQuery">搜索</el-button>  
                    </div>
                </el-col>
            </el-row> 
        </div>
           <div class="body-order">
               <el-table class="order-table" :data="matchData" stripe height=750>
                   <el-table-column prop="consignNum" label="发货型号" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column prop="deliverNo" label="生产订单号" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column prop="carSoleCode" label="小车唯一码" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column prop="prodct" label="产品型号" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column prop="total" label="生产数量" width="80" align="center" :show-overflow-tooltip="true">
                       <template #default="scope"> 
                       <span>{{ scope.row.total+ scope.row.unitName}}</span> 
                   </template>
                   </el-table-column>  
                   <el-table-column prop="totalPutout" label="剩余下架数量" width="100" align="center" :show-overflow-tooltip="true">
                       <template #default="scope"> 
                       <span>{{ (scope.row.total-scope.row.totalPutout)+ scope.row.unitName}}</span> 
                   </template>
                   </el-table-column>  
                   <el-table-column prop="countByCar" label="需求车次" width="80" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column prop="planTotalByCar" label="剩余车次" width="100" align="center" :show-overflow-tooltip="true">
                        <template #default="scope"> 
                        <span>{{ Math.ceil((scope.row.total-scope.row.totalPutout)/scope.row.planTotalByCar)}}</span> 
                    </template>
                    </el-table-column>
                   <el-table-column prop="planTotalByCar" label="计划装车数量" width="100" align="center" :show-overflow-tooltip="true">
                       <template #default="scope"> 
                       <span>{{ scope.row.planTotalByCar+ scope.row.unitName}}</span> 
                   </template>
                   </el-table-column> 
                   <el-table-column prop="actualTotalByCar" label="实际装车数量" width="100" align="center" :show-overflow-tooltip="true">
                       <template #default="scope"> 
                       <span>{{ scope.row.actualTotalByCar+ scope.row.unitName}}</span> 
                   </template>
                   </el-table-column> 
                   <el-table-column prop="matchingCount" label="小车配对数" width="80" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column prop="matchingNum" label="配对车次" width="80" align="center" :show-overflow-tooltip="true"/>
                   <el-table-column prop="matchingCode" label="配对库位"  align="center" :show-overflow-tooltip="true">
                       <template #default="scope"> 
                       <el-row v-if="isNotPackage">
                          <el-col :span="12" style="text-align: right;">
                           <span  style="color: #fff; padding:3px 5px;border-radius: 4px;position: relative;" :style="{backgroundColor:scope.row.matchColor}">{{ scope.row.binNo }}</span> 
                          </el-col>
                          <el-col :span="12" style="text-align: left;">
                           <span v-if="scope.row.status=='PutOut'" style="color:#fff;background-color: #909399;padding:3px 5px;border-radius: 4px;margin-left: 5px;">已下架</span>
                          </el-col>
                       </el-row>
                       <span v-else>{{ scope.row.binNo}}</span> 
                   </template>                                                                                                                                                                                                                               
                   </el-table-column>  
                   <el-table-column v-if="permission.isPermisstion('PRODPACKAGECLEARMATCH')" :label="$t('message.common.handle')" align="center" width="200">
                    <template #default="scope"> 
                        <el-popconfirm v-if="permission.isPermisstion('PRODPACKAGECLEARMATCH')&&!scope.row.isPackage&&scope.row.isMatch&&scope.row.status=='PutOut'"  title="是否确认已完成该配对包装？"  @confirm="onSubmit(scope.row)" >
                        <template #reference>
                            <el-button :type="scope.row.status=='PutOut'?'success':''" :disabled="scope.row.status!='PutOut'">包装完成</el-button>
                        </template>
                        </el-popconfirm>
                    </template>
                    </el-table-column>
               </el-table> 
           </div> 
       </div>   
   </div>
</template>

<script setup lang="ts"> 
import { reactive, ref,onMounted } from "vue";
import { useI18n } from 'vue-i18n';  
import commonHelper from '@/utils/system/common-helper';
import {getMatchingInfo,submitMatchPackage } from "@/api/production/package";  
import permission from '@/utils/system/permission'
import {CaretLeft,CaretRight,Refresh} from '@element-plus/icons-vue';  
import { useRouter, useRoute } from 'vue-router';
 
const matchData=ref();
const orderFiled=ref();
const orderType=ref();
const searchKey=ref(''); 
const pgIndex=ref(1);
const pgSize=ref(20);
const totalDate=ref(0);
const totalPage=ref(0);
const isNotPackage=ref(true);
const router=useRouter();

onMounted(()=>{
   getMatchingData(); 
})

const onPrePage=()=>{
    if(pgIndex.value>1){
        pgIndex.value--; 
        getMatchingData();
    }
}

const onNextPage=()=>{
    if(pgIndex.value<totalPage.value){
        pgIndex.value++; 
        getMatchingData();
    }
}

const onRefresh=()=>{ 
    pgIndex.value=1;
    getMatchingData();
}

const onQuery=()=>{
    pgIndex.value=1;
    getMatchingData();
}

const getMatchingData=()=>{  
   getMatchingInfo(pgSize.value,pgIndex.value,orderFiled.value,orderType.value,searchKey.value,!isNotPackage.value).then(res=>{ 
   totalDate.value=res.data.total;
   totalPage.value=Math.ceil(totalDate.value/pgSize.value);  
   let curcode=''
   if(res.data.rows){  
       res.data.rows.forEach((item:any) => { 
        if(item.matchingCode!=curcode){
            let matchingArr=res.data.rows.filter((f:any)=>f.matchingCode==item.matchingCode);
           if(matchingArr.length==item.matchingCount){
               let color=getColor(); 
               matchingArr.forEach((f1:any) => {  
                   f1.matchColor=color;
               }); 
           }
           else{
                item.matchColor='#9e9e9e'; 
            }
            curcode=item.matchingCode;
        } 
           let orderArr=res.data.rows.filter((f2:any)=>f2.consignNum==item.consignNum&&f2.carSoleCode!=item.carSoleCode);
           orderArr.forEach((f3:any) => {
               f3.consignNum='';
           });
       }); 
       matchData.value=res.data.rows;  
   }
});
} 
 

//定义一个颜色数组，包含20种颜色，每次取一个颜色，如果取完了，就从头开始取
const colorArray=['#004af7','#9c00f7','#00bdf7','#f56c6c','#f700e3','#c01509','#673108','#20041e','#016a2f','#3f2d77','#0dff00'];
let colorIndex=0;
const getColor=()=>{
   let color=colorArray[colorIndex];
   colorIndex++;
   if(colorIndex==colorArray.length){
       colorIndex=0;
   }
   return color;
}    
 
const onSubmit=(row:any)=>{
    submitMatchPackage(permission.getOperator().userName,row.matchingCode).then(res=>{
        pgIndex.value=1;
        getMatchingData();
    })
}
  
</script>
<style lang="scss" scoped>
.fade-enter-active, .fade-leave-active {
       transition: opacity 1s;
   }
   .fade-enter, .fade-leave-to  {
       opacity: 0;
   } 
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
               }
           }
       .content-body{  
           background-image: url('../../../assets/images/backimg.png');
           background-position: 100%;
           background-size: cover;
           background-repeat: no-repeat;
           height:94.3%;
           background-color: #efefef; 
           overflow: auto; 
           padding:0 .5%;   
           .body-query{
            padding: 10px 0;
            background:rgba(255, 255, 255, 0.404);
           }
           .body-order{
               padding: 10px;
               background:rgba(255, 255, 255, 0.304);
               .order-table{
                   background:rgba(255, 255, 255, 0.36);
                  :deep .el-table__row{
                       background:rgba(255, 255, 255, 0.36);
                   }
                   // :deep colgroup{
                   //     background:rgba(255, 255, 255, 0.36);
                   // }
               }
           } 
       } 
   }
</style>