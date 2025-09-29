<template>
  <div class="system-table-box">
    <el-table
      v-bind="$attrs"
      class="system-table system-scrollbar"
      border
      :height="csmHeight"
      :data="data"
      :header-cell-style="{'text-align':'center'}"
      :show-summary="isShowSum" 
      @selection-change="handleSelectionChange"  
      highlight-current-row
      :row-style="{height:'30px'}"
      :cell-style="{padding:'3px'}"
    >
      <el-table-column type="selection" align="center" width="50" v-if="showSelection" /> 
      <slot></slot>
    </el-table> 
  </div>
</template>

<script lang="ts">
import { defineComponent, reactive } from 'vue' 
export default defineComponent({
  props: {
    data: { type: Array, default: () => [] }, // 数据源
    select: { type: Array, default: () => [] }, // 已选择的数据，与selection结合使用 
    showSelection: { type: Boolean, default: false }, // 是否展示选择框，默认否 
    isShowSum:{ type: Boolean, default: false }, //是否行尾合计 
    csmHeight:{type:String,default:'100%'}
  },
  setup(props, context) {  
    // 选择监听器
    const handleSelectionChange = (val: []) =>{
      context.emit("selection-change", val) 
    } 
    return { 
      handleSelectionChange
    }
  }
})
</script>

<style lang="scss" scoped>
  .system-table-box {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: flex-start;
    height: 100%;
    .system-table {
      flex: 1;
      height: 100%;
    }
    
    .system-page {
      margin-top: 20px;
    }
  }
  
</style>