<template> 
    <div class="app-content">
        <div class="content-header">
            <div id="takestockChartDiv" style="height: 100%;"></div> 
        </div>
        <div class="content-body">  
             <el-row  class="body-item" style="border-left: none;margin-top: -10px;" :gutter="10">
                <el-col :span="6" v-if="deftClassifyGroup=='SparePart'">
                    <img src="/public/icon-img/beijian.png" height="40" @click="onSelectGroup('SparePart')">
                </el-col>
                <el-col :span="6" v-if="deftClassifyGroup=='SparePart'">
                    <img src="/public/icon-img/baocai.png" height="40" @click="onSelectGroup('Consumables')">
                </el-col>
                <el-col :span="6" v-if="!deftClassifyGroup">
                    <img src="/public/icon-img/yangjian.png" height="40" @click="onSelectGroup('SamplePiece')">
                </el-col>
                <el-col :span="6" v-if="!deftClassifyGroup">
                    <img src="/public/icon-img/gelijian.png" height="40" @click="onSelectGroup('Separator')">
                </el-col>
                <el-col :span="6" v-if="!deftClassifyGroup">
                    <img src="/public/icon-img/baocai.png" height="40" @click="onSelectGroup('PackingMaterial')">
                </el-col>
             </el-row>
            <el-row class="body-item" @click="onRoutePage('takestockbygoods')">
                <el-col class="item-text" :span="19" :offset="1">按物品盘点</el-col>
                <el-col class="item-icon" :span="2"><img src="/public/icon-img/huowu_3.png" height="20"></el-col>
                <el-col :span="1" class="text-deft"><el-icon><ArrowRightBold /></el-icon></el-col>
            </el-row>
            <el-row class="body-item" @click="onRoutePage('takestockbybin')">
                <el-col class="item-text" :span="19" :offset="1">按货位盘点</el-col>
                <el-col class="item-icon" :span="2"><img src="/public/icon-img/xuanzekuwei.png" height="18"></el-col>
                <el-col :span="1" class="text-deft"><el-icon><ArrowRightBold /></el-icon></el-col>
            </el-row> 
        </div>
    </div>  
</template>
<script lang="ts" setup>
import {ref,defineEmits,defineProps,onMounted,onBeforeMount,onBeforeUnmount } from 'vue'; 
import { useEventListener } from '@vueuse/core';  
import { useRouter, useRoute } from 'vue-router';
import * as echarts from 'echarts/core';
import {TitleComponent,TooltipComponent,VisualMapComponent,LegendComponent} from 'echarts/components';
import { RadarChart } from 'echarts/charts';
import { CanvasRenderer } from 'echarts/renderers';
import pieOption from './pie-option'; 
import {ArrowRightBold} from '@element-plus/icons-vue';  
import permission from '@/utils/system/permission';
import {getExpendTrend} from "@/api/inv/takestock";
import { deftClassifyGroup} from '@/config';
echarts.use([
  TitleComponent,
  TooltipComponent,
  VisualMapComponent,
  LegendComponent,
  RadarChart,
  CanvasRenderer
]);
const chartDivHeight=ref(700);
const router = useRouter(); 
const goodsGroup=ref(deftClassifyGroup)
var myChart:any;
onBeforeMount(()=>{
  chartDivHeight.value=window.innerHeight-160;
})

 onMounted(()=>{
    let chartDiv=document.getElementById('takestockChartDiv');  
     myChart = echarts.init(chartDiv as HTMLElement);
     setChartOption();
 }) 

 onBeforeUnmount(()=>{
    if(myChart)
     myChart.dispose();
 })

 const setChartOption=()=>{
    let group=goodsGroup.value||'SamplePiece'; 
    let title='';
    if(group=='SparePart'){
        title='备件'
    }
    else if(group=='Consumables'){
        title='耗材'
    }
    else if(group=='SamplePiece'){
        title='样件'
    }
    else if(group=='Separator'){
        title='辅材'
    }
    else if(group=='PackingMaterial'){
        title='包材'
    }
    getExpendTrend(group).then((res:any)=>{
        if(res.data.dataArray?.length>0){
            let curYear=new Date().getFullYear()+"年";
            let option=pieOption(title,curYear,res.data.maxValue, res.data.yearMonthArray,res.data.indicatorArray,res.data.dataArray);
            myChart.setOption(option);
            useEventListener("resize", () => myChart.resize()); 
        } 
    }) 
 }

 const onSelectGroup=(group:string)=>{
    goodsGroup.value=group;
    setChartOption();
 }

 const onRoutePage=(routeName:string)=>{
    router.push({name:routeName}).then(()=>{
        permission.addRouteHis(routeName); 
    });
 }
</script>

<style lang="scss" scoped> 
@media screen and (max-width: 450px){
    .app-content{
        height: 95%;
        background-color: #fff; 
        padding:0 15px;
        .content-header{  
            padding:20px 0px 20px 0px;
            height: 35%; 
            background-color:rgb(244, 253, 253);
        }
        .content-body{ 
            margin-top: 5%; 
            .body-item{   
                background-color: rgb(247, 252, 252);
                border-radius: 5px;
                margin-bottom: 15px;
                border-left: 2px solid #f7a500;
                color: #6e6e6e;
                height: 41px;
                line-height: 41px;
                .item-text{
                    font-size: 14px; 
                    text-align: left;  
                }
                .item-icon{
                    text-align: right;
                    position: relative;
                    bottom: -2px;
                    img{
                            opacity: .9; 
                        }
                }
            }
        } 
    } 
}

@media screen and (min-width: 450px){
    .app-content{
        height: 95%;
        background-color: #fff; 
        padding:0 15px;
        .content-header{  
            padding:20px 0px 20px 0px;
            height: 35%; 
            background-color:rgb(244, 253, 253);
        }
        .content-body{ 
            padding: 10px 20px 10px 10px; 
            margin-top: 4%;
            .body-item{   
                background-color: rgb(247, 252, 252);
                border-radius: 5px;
                margin-bottom: 15px;
                border-left: 2px solid #f7a500;
                color: #6e6e6e;
                height: 55px;
                line-height: 55px;
                .item-text{
                    font-size: 14px; 
                    text-align: left;  
                }
                .item-icon{
                    text-align: right;
                    position: relative;
                    bottom: -2px;
                    img{
                            opacity: .9; 
                        }
                }
            }
        } 
    } 
}
 
</style>