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
            <el-date-picker v-model="dateRange" type="daterange" range-separator="-" start-placeholder="最早日期"
                end-placeholder="最晚日期" size="small" value-format="YYYY-MM-DD" style="margin-right:10px;width:60%">
            </el-date-picker>
            <el-input v-model="query.input" placeholder="请输入关键词进行检索" size="small" style="width:100px" />
            <el-button type="primary" icon="el-icon-search" size="small" style="margin-left:10px"
                @click="getTableData(true)">
                搜索
            </el-button>
        </div>

        <!-- 表格 -->
        <el-table v-loading="loading" :data="tableData" border style="width: 100%; margin-top: 10px;">
            <el-table-column prop="orderNo" label="入库单号" align="center" :show-overflow-tooltip="true" />
            <el-table-column prop="goodsName" label="名称" align="center" show-overflow-tooltip />

            <el-table-column prop="actualQuantity" label="出库数量" align="center" :show-overflow-tooltip="true" />

            <el-table-column prop="createUserName" label="创建人" align="center" :show-overflow-tooltip="true" />
            <el-table-column prop="createDate" label="创建时间" align="center" :show-overflow-tooltip="true">
                <template #default="props">
                    <span>{{ commonHelper.formatToDateTime(props.row.createDate) }}</span>
                </template>
            </el-table-column>

        </el-table>

        <!-- 分页 -->
        <div class="pagination">
            <el-pagination v-model:current-page="page.index" v-model:page-size="page.size" :total="page.total"
                background layout="total, sizes, prev, pager, next, jumper" @current-change="getTableData"
                @size-change="getTableData" />
        </div>
    </div>
</template>

<script lang="ts" setup>
import { ref, reactive, onMounted } from "vue"

import { getOrders } from "@/api/inv/outstorage"
import { getGoodsGroup } from '@/api/common';
import permission from "@/utils/system/permission";
import commonHelper from "@/utils/system/common-helper";
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
    size: 10,
    total: 0,
    orderField: "",
    orderType: "",
})

// 查询条件
const query = reactive({
    input: "",
})
let dateRange = ref()
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
        getTableData(true)
    }
}
const getGoodsGroupData = () => {
    getGoodsGroup().then(res => {
        if (res.data) {
            goodsGroupData.value = res.data.filter((f: any) => f.key == 'FinishedProduct' || f.key == 'RawMaterial');
            selectedGoodsGroup.value = 'FinishedProduct';
            getTableData(true);
        }
    })
}
// 获取表格数据
const getTableData = (init?: boolean) => {
    loading.value = true
    if (init) {
        page.index = 1
    }
    let dateStart = '';
    let dateEnd = '';

    if (dateRange.value?.length > 0) {
        dateStart = dateRange.value[0]
    }
    if (dateRange.value?.length > 1) {
        dateEnd = dateRange.value[1]
    }
    getOrders(permission.getOperator().userId, page.size, page.index, page.orderField, page.orderType, query.input,
        dateStart, dateEnd, selectedGoodsGroup.value)
        .then((res) => {
            let data = res.data.rows
            data.forEach((d: any) => {
                d.loading = false
            })
            tableData.value = data
            page.total = Number(res.data.total);
        })
        .catch((error) => {
            tableData.value = [];
            page.index = 1;
            page.total = 0;
        })
        .finally(() => {
            loading.value = false;
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
