<template>
     <div class="content"> 
        <div class="content-header">
            <el-row >
                <el-col :span="8" class="header-logo"><img src="@/assets/header-logo-b.png" alt=""></el-col>
                <el-col :span="8"  class="header-title"><h3>成品缓存仓看板</h3></el-col>
            </el-row> 
        </div>
        <div class="content-body" > 
            <div class="body-order">
                <el-table class="order-table" :data="matchData" stripe height=500>
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
                        <el-row>
                           <el-col :span="12" style="text-align: right;">
                            <span v-if="scope.row.status=='PutOut'" style="color: #888; padding:3px 5px;border-radius: 4px;border:1px solid #888">{{ scope.row.binNo }}</span>
                            <span v-else style="color: #fff; padding:3px 5px;border-radius: 4px;position: relative;" :style="{backgroundColor:scope.row.matchColor}">{{ scope.row.binNo }}</span> 
                           </el-col>
                           <el-col :span="12" style="text-align: left;">
                            <span v-if="scope.row.status=='PutOut'" style="color:#fff;background-color: #909399;padding:3px 5px;border-radius: 4px;margin-left: 5px;">已下架</span>
                           </el-col>
                        </el-row>
                    </template>                                                                                                                                                                                                                               
                    </el-table-column>  
                    <el-table-column prop="unstackBinNo" label="拆垛机库位" width="80" align="center" :show-overflow-tooltip="true"/>
                </el-table> 
            </div>
            <div class="body-layout" > 
                <div class="layout-content" >   
                    <div class="layout-bin" :class="setElementClass(item)" :style="{width:warehouseElementWidth+'%'}"   v-for="item in binData" @click="onSelectElement(item)" >
                        <div class="el-icon-place" v-if="item.isSelected"></div>  
                        <div class="el-rank-place" :style="setMatchBinStyle(item)">{{ item.rank}}</div>
                        <transition name="fade">
                            <div class="el-rank-animation" v-if="animationShow"> 
                                <el-icon><VideoCameraFilled /></el-icon>
                            </div>
                        </transition>
                        <P class="bin-title">{{ item.name}}</P> 
                        <P class="bin-info">{{ `${item.code?item.code:'NULL'} 数量:${item.total?item.total:'0'}` }}</P>
                        <P class="bin-status">{{ item.status }}</P>
                    </div>  
                </div>
            </div>
        </div>  
        <DetailLayer :layer="layer" v-if="layer.show"/> 
    </div>
</template>

<script setup lang="ts"> 
import { reactive, ref,onMounted,onBeforeUnmount } from "vue";
import { useI18n } from 'vue-i18n';  
import commonHelper from '@/utils/system/common-helper';
import {getMatchingInfo,getBufferWarehouseElement ,getStockInfoByBin } from "@/api/production/bufferWarehouseDashboard"; 
import { LayerInterface } from "@/components/layer/index.vue"; 
import DetailLayer from './detailLayer.vue' 
import {VideoCameraFilled} from '@element-plus/icons-vue';  

const layer: LayerInterface = reactive({
      show: false,
      title: '',
      showButton: true,
      btnLoading:false,
      width:"30%",
      data:{},
      type:'',
      otherButton:{ }
    })
const matchData=ref();
const binData=ref();
const warehouseElementWidth=ref(10); 
const animationShow=ref(false);
const binMatching=ref();
const pgIndex=ref(1);
const pgSize=ref(13);
const totalDate=ref(0);
const totalPage=ref(0);
var dashboardIntervalHandler:any=0;

onMounted(()=>{
    getMatchingData();
    getBinData();
    dashboardIntervalHandler= setInterval(()=>{
        getMatchingData();
        getBinData();
    },15000);
})

onBeforeUnmount(()=>{
    clearInterval(dashboardIntervalHandler);
}) 

const getMatchingData=()=>{  
    getMatchingInfo(pgSize.value,pgIndex.value).then(res=>{ 
    totalDate.value=res.data.total;
    totalPage.value=Math.ceil(totalDate.value/pgSize.value); 
    if(pgIndex.value==totalPage.value){
        pgIndex.value=1;
    }
    else{
        pgIndex.value=pgIndex.value+1;
    }
    if(res.data.rows){  
        let curcode=''
        res.data.rows.forEach((item:any) => {  
            let matchingArr=res.data.rows.filter((f:any)=>f.matchingCode===item.matchingCode);
            if(item.matchingCode!=curcode){
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
        binMatching.value=new Array<any>();
        matchData.value.forEach((item:any) => {
            if(item.matchColor )
              binMatching.value.push({color:item.matchColor,binNo:item.binNo,carSoleCode:item.carSoleCode});
        }); 
    }
});
}

const getBinData=()=>{
    getBufferWarehouseElement().then(res=>{
    binData.value=res.data;
    let propsItem=binData.value.filter((item:any)=>item.props)[0];  
    warehouseElementWidth.value=getLayoutElementWidth(propsItem?.props); 
  
})
}

const setMatchBinStyle=(bin:any)=>{ 
    if(binMatching.value){
       let obj= binMatching.value.filter((f:any)=>f.binNo==bin.no);  
       if(obj?.length>0&&bin.code==obj[0].carSoleCode){
              return {backgroundColor:obj[0].color}
       }
    }
    return {backgroundColor:'#fff',color:'#888'};
}

//定义一个颜色数组，包含20种颜色，每次取一个颜色，如果取完了，就从头开始取
const colorArray=['#004af7','#00bdf7','#f56c6c','#f700e3','#9c00f7','#c01509','#673108','#20041e','#016a2f','#3f2d77','#0dff00'];
let colorIndex=0;
const getColor=()=>{
    let color=colorArray[colorIndex];
    colorIndex++;
    if(colorIndex==colorArray.length){
        colorIndex=0;
    }
    return color;
}    

let getLayoutElementWidth=(props:string)=>{
    if(props&&props.indexOf('*')>-1){
      let col= Number(props.split('*')[0]);
      return  100/col-2.2;
    }
    return 100/6-2.2;
   }

const setElementClass=(item:any)=>{ 
    let className="";
    if(item.isAbandon){
      className= "bg-deft"
    }
    else{
      if(item.type=='Shelf'){
        className= "bg-primary-dark"
      } 
      else{
        if(item.status=="Free"){
          className= "bg-success-dark"
        }
        else if(item.status=="Full"){
          className= "bg-warning"
        }
        else{
          className= "bg-danger"
        }
      } 
    }
    if(item.isSelected){
      className= className+" layout-item-select";
    } 
    return className;
  }
 
const onSelectElement=(item:any)=>{   
    binData.value.forEach((element:any) => {
    element.isSelected=false;
  });
  item.isSelected=true;
  if(item.status=="Full"||item.status=="Lock"){ 
      getStockInfoByBin(item.id).then(res=>{
        layer.show=true;
        layer.title="库位存储信息";
        layer.data=res.data;
      })
    }
}

</script>
<style lang="scss" scoped>
 .fade-enter-active, .fade-leave-active {
        transition: opacity 1s;
    }
    .fade-enter, .fade-leave-to  {
        opacity: 0;
    }
 
  
 
.matching{
    background-color: #004af7;
    background-color: #9c00f7;
    background-color: #00bdf7;
    background-color: #f56c6c; 
    background-color: #f700e3;
    background-color: #c01509;
    background-color: #673108;
    background-color: #20041e;  
    background-color: #016a2f;
    background-color: #3f2d77; 
    background-color: #0dff00; 
    background-color: #9e9e9e; 
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
            .body-layout{ 
                padding: 0 10px 10px 10px;
                background:rgba(255, 255, 255, 0.304);  
                .layout-content{   
                    background:rgba(255, 255, 255, 0.36); 
                    width: 100%;
                    border:1px solid #e3e3e3;
                    text-align: left;
                    border-radius: 4px; 
                    padding: 0 0 10px 0;
                    height: 100%;
                    .layout-bin{
                        height: 62px;
                        width: 14.45%;
                        margin-top: 15px;
                        margin-left: 1%;
                        margin-right: 1%;  
                        border-radius: 3px;
                        display: inline-block; 
                        vertical-align: middle;
                        text-align: center; 
                        position: relative; 
                        cursor: pointer;
                        .el-icon-place{
                            position: absolute;
                            left: 2%; 
                            top: 6%;
                        }
                        .el-rank-place{
                            position: absolute;
                            right: 1%; 
                            top: 6%;
                            font-size: small;
                            border-radius: 100%;
                            height: 15px;
                            width: 16px;
                            line-height: 16px;
                            text-align: center;
                            border:2px solid #fff;
                            background-color: #790808;
                            color: #fff;
                        }
                        .el-rank-animation{
                            position: absolute;
                            left: 8%; 
                            top: 6%;
                            font-size: small;
                        }
                        .bin-title{
                            margin-top: 10px;
                            font-weight: 600;
                        }
                        .bin-status{
                            margin-top: -7px;font-size: x-small;
                        }
                        .bin-info{
                            margin-top: -15px;font-size: x-small; 
                        }
                        .bin-free{
                            margin-top: 7.5px;
                        }
                    }
                    .layout-title{
                        font-weight: 600;
                        color: #aeaeae;
                        text-align: center; 
                        margin-top: 10px;
                    }
                 }
            }
           
        } 
    }
</style>