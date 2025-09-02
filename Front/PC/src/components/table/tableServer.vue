<template>
  <div class="system-table-box">
    <el-table
      v-bind="$attrs"
      class="system-table"
      border
      :height="csmHeight"
      :data="data"
      :header-cell-style="{'text-align':'center'}"
      @selection-change="handleSelectionChange"
      @sort-change="handleSortChange"
      @expand-change="handleRowExpandChange"
      size="small"
      highlight-current-row
      :row-style="{height:'30px'}"
      :cell-style="{padding:'3px'}"
    >
      <el-table-column type="selection" align="center" width="50" v-if="showSelection" />
      <el-table-column label="序号" width="60" align="center" v-if="showIndex">
        <template #default="scope">
          {{ (page.index - 1) * page.size + scope.$index + 1 }}
        </template>
      </el-table-column>
      <slot></slot>
    </el-table>
    <el-pagination
      v-if="showPage"
      v-model:current-page="page.index"
      class="system-page"
      background
      :layout="pageLayout"
      :total="page.total"
      :page-size="page.size"
      :page-sizes="pageSizes"
      @current-change="handleCurrentChange"
      @size-change="handleSizeChange"
    >
    </el-pagination>
  </div>
</template>

<script lang="ts">
import { defineComponent, reactive } from 'vue' 
export default defineComponent({
  props: {
    csmHeight:{type:String,default:'100%'},
    data: { type: Array, default: () => [] }, // 数据源
    select: { type: Array, default: () => [] }, // 已选择的数据，与selection结合使用
    showIndex: { type: Boolean, default: false }, // 是否展示index选择，默认否
    showSelection: { type: Boolean, default: false }, // 是否展示选择框，默认否
    showPage: { type: Boolean, default: true }, // 是否展示页级组件，默认是
    page: { // 分页参数
      type: Object,
      default: () => {
        return { index: 1, size: 20, total: 0 , orderField:'', orderType:''}
      }
    },
    pageLayout: { type: String, default: "total, sizes, prev, pager, next, jumper" }, // 分页需要显示的东西，默认全部
    pageSizes: { type: Array, default: [10, 20, 50, 100] }
  },
  setup(props, context) { 
    let timer: any = null
    // 分页相关：监听页码切换事件
    const handleCurrentChange = (val: Number) => { 
      if (timer) { 
        props.page.index = 1
      } else {
        props.page.index = val
        context.emit("getTableData")
      }
    }
    // 分页相关：监听单页显示数量切换事件
    const handleSizeChange = (val: Number) => { 
      timer = 'work'
      setTimeout(() => {
        timer = null
      }, 100)
      props.page.size = val
      context.emit("getTableData", true)
    }
    // 选择监听器
    const handleSelectionChange = (val: []) =>{
      context.emit("selection-change", val) 
    }
    //监听排序
    const handleSortChange=(orderRow:any)=>{ 
      props.page.orderField=orderRow.prop==null?"":orderRow.prop;
      props.page.orderType=orderRow.order==null?"":orderRow.order;  
      context.emit("orderChanged", orderRow) 
    }
    //监听行展开和收缩
    const handleRowExpandChange=(row:any)=>{ 
         context.emit("expandChange",row) 
    }
    return {
      handleCurrentChange,
      handleSizeChange,
      handleSelectionChange,
      handleSortChange,
      handleRowExpandChange
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