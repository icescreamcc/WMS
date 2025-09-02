/*
* 使用说明：用户只需传入options即可，options请参照官网示例中的options
* 本组件采用整包引入echarts的方法，用于适配所有的echarts控件
* 如需按需加载引入echarts，可参照写法：echarts官网/在打包环境中使用ECharts
*/
<template>
  <div>
    <div ref="chart" id="chartDiv" class="chart" ></div>
  </div>
</template>

<script lang="ts" setup >
import { defineComponent, ref ,defineEmits,defineProps,onMounted} from 'vue'
import { useEventListener } from '@vueuse/core' // 引入监听函数，监听在vue实例中可自动销毁，无须手动销毁
import * as echarts from 'echarts'

const props=defineProps({
  option:{
        type: Object,
        default:()=>{
            return{ }
        }
    }
  });   

  onMounted(()=>{ 
  let chartDiv=document.getElementById('chartDiv');  
  let myChart = echarts.init(chartDiv as HTMLElement);
  myChart.setOption(props.option);
  useEventListener("resize", () => myChart.resize());
  })

 
</script>

<style lang="scss" scoped>
  .chart {
    width: 100%;
    height: 100%;
    display: flex;
    justify-content: center;
  }
</style>