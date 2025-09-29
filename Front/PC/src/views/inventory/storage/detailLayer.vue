<template>
  <Layer :layer="layer" > 
      <el-table class="system-table"  border  height="250"  :data="layer?.data">
        <el-table-column prop="goodsName" label="物品名称" align="center" sortable min-width="180" :show-overflow-tooltip="true">
          <template #default="scope"> 
            <span>{{scope.row.goodsName}}</span>&nbsp;
            <span>{{scope.row.goodsModel}}</span>
           </template>
        </el-table-column>  
        <el-table-column prop="warehouseName" label="仓库" align="center" sortable min-width="100" :show-overflow-tooltip="true"/> 
        <el-table-column prop="shelfName" label="货架" align="center" sortable min-width="100" :show-overflow-tooltip="true"/>
        <el-table-column prop="binName" label="货位" align="center" sortable min-width="100" :show-overflow-tooltip="true"/>
        <el-table-column prop="cellNo" label="料箱" align="center" sortable min-width="100" :show-overflow-tooltip="true"/>
        <el-table-column prop="stock" label="库存量" align="center" sortable min-width="100" :show-overflow-tooltip="true">
           <template #default="scope">
            <span>{{scope.row.stock+scope.row.unitName}}</span>   
          </template>
        </el-table-column>
      </el-table>
  </Layer> 
</template>

<script lang="ts" setup>
import { ref,defineEmits,defineProps,onMounted } from 'vue' 
import Layer from '@/components/layer/index.vue'   

const props=defineProps({
  layer: {
      type: Object,
      default: () => {
        return {
          show: false,
          title: '',
          showButton: true,
          btnLoading:false,
          type:'',
          data:null, 
        }
      }
    }
}) 
</script>

<style lang="scss" scoped>
  * {
    text-align: left;
  }
  .box-card{
    margin-top: 10px;
  }
  .option-content{
    border:1px solid rgb(230, 230, 230);
    border-radius: 3px;
    padding: 5px;
    .head{
      border-bottom: 1px solid rgb(230, 230, 230);
      margin-bottom: 5px;
      .title{
        margin: 5px 0 0 0;
      }
    }
    .item{
      margin:3px 0;
    }
  }
</style>