<template>
    <div class="tab-content">
        <!-- 搜索条件 -->
        <div class="search-bar">
            <el-select v-model="selectedGoodsGroup" size="small" @change="onGroupSelected">
                <template #prefix>
                    <div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; 
              background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
                        分类
                    </div>
                </template>
                <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key" />
            </el-select>

            <el-input v-model="query.input" placeholder="请输入关键词进行检索" size="small"
                style="margin-left:10px;width:200px" />
            <el-button type="primary" icon="el-icon-search" size="small" style="margin-left:10px"
                @click="getDetailTableData(true)">
                搜索
            </el-button>
        </div>

        <!-- 表格 -->
        <el-table v-loading="loading" :data="tableData" border style="width: 100%; margin-top: 10px;">
            <el-table-column prop="goodsName" label="名称" align="center"  show-overflow-tooltip />
            <el-table-column prop="goodsModel" label="型号" align="center" show-overflow-tooltip />
            <el-table-column prop="supplierName" label="供应商" align="center" show-overflow-tooltip />
            
            <el-table-column prop="stock" label="库存量" align="center"  show-overflow-tooltip>
                <template #default="scope">
                    <span>{{ scope.row.stock + ' ' + scope.row.unitName }}</span>
                </template>
            </el-table-column>
        </el-table>

        <!-- 分页 -->
        <div class="pagination">
            <el-pagination v-model:current-page="page.index" v-model:page-size="page.size" :total="page.total"
                background layout="total, sizes, prev, pager, next, jumper" @current-change="getDetailTableData"
                @size-change="getDetailTableData" />
        </div>
    </div>
</template>

<script lang="ts" setup>
import { ref, reactive, onMounted } from "vue"

import { getStorageDetailList } from "@/api/inv/storage"
import {getGoodsGroup} from '@/api/common';  

// 商品分类
const goodsGroupData = ref<any[]>([]);
const selectedGoodsGroup = ref("");

// 分页
interface Page {
    index: number
    size: number
    total: number
    orderField: string
    orderType: string
}
const page = reactive<Page>({
    index: 1,
    size: 20,
    total: 0,
    orderField: "",
    orderType: "",
})

// 查询条件
const query = reactive({
    input: "",
})

// 表格数据
const tableData = ref<any[]>([])
const loading = ref(false)

// 仓库数据
const warehouseData = ref<any[]>([])
const selectedWarehouseId = ref("")
let warehouseDataBuffer: any[] = []

// 选择分类
const onGroupSelected = () => {
    warehouseData.value.length = 0
    selectedWarehouseId.value = ""
    const warehouseBygroup = warehouseDataBuffer.filter(
        (f) => f.warehouseType == selectedGoodsGroup.value
    )
    if (warehouseBygroup?.length > 0) {
        warehouseData.value = [{ warehouseId: "", warehouseName: "All" }, ...warehouseBygroup]
        getDetailTableData(true)
    }
}
const getGoodsGroupData = () => {
  getGoodsGroup().then(res => {
    if (res.data) {
      goodsGroupData.value = res.data.filter((f:any)=>f.key == 'FinishedProduct'||f.key == 'RawMaterial');
      selectedGoodsGroup.value='FinishedProduct';
      getDetailTableData(true);
    }
  })
}
// 获取表格数据
const getDetailTableData = (init?: boolean) => {
    loading.value = true
    if (init) {
        page.index = 1
    }
    getStorageDetailList(
        page.size,
        page.index,
        page.orderField,
        page.orderType,
        query.input,
        selectedWarehouseId.value,
        selectedGoodsGroup.value
    )
        .then((res: any) => {
            const data = res.data.rows || []
            data.forEach((d: any) => {
                d.loading = false
            })
            tableData.value = data
            page.total = Number(res.data.total || 0)
        })
        .catch(() => {
            tableData.value = []
            page.index = 1
            page.total = 0
        })
        .finally(() => {
            loading.value = false
        })
}

onMounted(() => {
    getGoodsGroupData();
    
})
</script>

<style scoped>
.tab-content {
    padding: 16px;
    background: #fff;
}

.search-bar {
    display: flex;
    align-items: center;
}

.pagination {
    margin-top: 15px;
    text-align: right;
}
</style>
