<template>
    <div class="tab-content">
        <!-- 搜索条件 -->
        <div class="search-bar">
            <el-select v-model="selectedGoodsGroup" ref="refSelectClassifyGroup" size="small" class="m-2"
                style="width:90%;margin-left: 20px;" @change="getTableData(true)" placeholder="选择物品大类">
                <template #prefix>
                    <div
                        style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
                        分类</div>
                </template>
                <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value"
                    :value="item.key"></el-option>
            </el-select>

            <el-input v-model="query.input" placeholder="请输入关键词进行检索" size="small"
                style="margin-left:10px;width:200px" />
            <el-button type="primary" icon="el-icon-search" size="small" style="margin-left:10px"
                @click="getTableData(true)">
                搜索
            </el-button>
        </div>

        <!-- 表格 -->
        <el-table v-loading="loading" :data="tableData" border style="width: 100%; margin-top: 10px;"
            @row-click="handleRead">
            <el-table-column prop="orderNo" label="发货单号" :show-overflow-tooltip="true" />
            <el-table-column prop="customerOrderNo" label="订单号" :show-overflow-tooltip="true" />
            <el-table-column prop="supplierName" label="供应商" align="center" show-overflow-tooltip />
            <el-table-column prop="quantity" label="发货数" align="center" :show-overflow-tooltip="true" />
            <el-table-column prop="status" label="状态" align="center" :show-overflow-tooltip="true">
                <template #default="props">
                    <span v-if="props.row.status == 'Shipment'" class="text-primary">{{ props.row.statusDesc }}</span>
                    <span v-else-if="props.row.status == 'CancelShipment'" class="text-danger">{{ props.row.statusDesc
                    }}</span>
                    <span v-else-if="props.row.status == 'WaitingNotification'" class="text-warning">{{
                        props.row.statusDesc }}</span>
                    <span v-else class="text-success">{{ props.row.statusDesc }}</span>
                </template>
            </el-table-column>
        </el-table>

        <!-- 分页 -->
        <div class="pagination">
            <el-pagination v-model:current-page="page.index" :page-size="7" :total="page.total" background
                layout="total, prev, pager, next, jumper" @current-change="getTableData(false)" />
        </div>
    </div>
    <ShipDetail v-if="shipLayer.row" :row="shipLayer.row" v-model:show="shipLayer.show" :title="shipLayer.title" />
</template>

<script lang="ts" setup>
import { ref, reactive, onMounted } from "vue"
import { getSending } from "@/api/order/shipment";
import { getGoodsGroup } from '@/api/common';
import permission from "@/utils/system/permission";
import { deftClassifyGroup } from '@/config';
import ShipDetail from "./shipditails.vue";
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
    size: 7,
    total: 0,
    orderField: "",
    orderType: "",
})

// 查询条件
const query = reactive({
    input: "",
})
// 商品分类
const goodsGroupData = ref<any[]>([]);
const goodsClassifyDefault = ref(["SamplePiece", "FinishedProduct", "RawMaterial"]);
const selectedGoodsGroup = ref(deftClassifyGroup || "FinishedProduct");
// 表格数据
const tableData = ref<any[]>([])
const loading = ref(false)

const getGoodsGroupData = () => {
    getGoodsGroup().then(res => {

        goodsGroupData.value = res.data.filter((f: any) => goodsClassifyDefault.value.includes(f.key));
        console.log('#### goodsGroupData.value = ', goodsGroupData.value)
    })
}
// 获取表格数据
const getTableData = (init: Boolean) => {
    loading.value = true
    if (init) {
        page.index = 1
    }
    console.log('####  page.index = ', page.index)
    loading.value = false

    getSending(permission.getOperator().userId, page.size, page.index, page.orderField, page.orderType,
        query.input, '', '', selectedGoodsGroup.value, '', '', '')
        .then((res) => {
            let data = res.data.rows
            console.log('###########  getTableData   res.data.rows ', res.data.rows)
            // data  = sendingAddressData.value
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
const shipLayer = ref({
    show: false,
    title: "",
    row: null
});
const handleRead = (row: any) => {
    shipLayer.value = {
        show: true,
        title: "发货单计划",
        row
    };
};
onMounted(() => {
    getGoodsGroupData();
    getTableData(true);
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
