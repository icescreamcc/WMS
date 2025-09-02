<template>
  <div class="box">
    <!-- <basic-template />  -->
    <div :style="{height:chartDivHeight+'px'}" class="content">
      <div id="mainChart" :style="{height:chartDivHeight+'px'}" style="background-color: #fff;border-radius: 6px;margin-top: 3px;padding: 10px 0;"></div>
    </div> 
  </div>
</template>

<script lang="ts" setup>
import { ref,onMounted,onBeforeMount } from 'vue' 
import { useEventListener } from '@vueuse/core' 
import * as echarts from 'echarts/core';
import {TitleComponent,TitleComponentOption, TooltipComponent, TooltipComponentOption} from 'echarts/components';
import { TreemapChart, TreemapSeriesOption } from 'echarts/charts';
import { CanvasRenderer } from 'echarts/renderers';
import getOption from './components/charts-option/treemap';
import { getGoodsTreeMapData } from "@/api/baseinfo/goods"; 
import { deftClassifyGroup } from '@/config';

echarts.use([TitleComponent, TooltipComponent, TreemapChart, CanvasRenderer]); 
type EChartsOption = echarts.ComposeOption<TitleComponentOption | TooltipComponentOption | TreemapSeriesOption>;
const chartDivHeight=ref(700);

onBeforeMount(()=>{
  chartDivHeight.value=window.innerHeight-160;
})

onMounted(()=>{  
  let chartDiv=document.getElementById('mainChart');  
  let myChart = echarts.init(chartDiv as HTMLElement);
  myChart.showLoading();
  getGoodsTreeMapData(deftClassifyGroup).then(res=>{
    if(res.data?.length>0){
      res.data.forEach((item:any) => {
        item.color=['#73c0de','#5470c6','#016a2f','#91cc75','#fac858','#673108','#20041e','#3ba272','#fc8452','#9a60b4','#ea7ccc'];
        item.label={show:true} 
      }); 
    } 
    let option:EChartsOption=getOption(echarts,res.data);
    myChart.setOption(option);
    myChart.hideLoading();
    useEventListener("resize", () => myChart.resize()); 
  }) 
})
</script>

<style lang="scss" scoped>
  .box {
    padding: 15px;   
  }
</style>