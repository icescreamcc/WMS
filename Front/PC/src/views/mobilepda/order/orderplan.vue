<template>
    <div class="tab-content">
        <!-- 搜索条件 -->
        <div class="search-bar">

            <el-select v-model="query.customerName" ref="refSelectClassifyGroup" size="small" class="m-2"
                style="width:90%;margin-left: 20px;" @change="getTableData(true)" placeholder="选择客户">
                <template #prefix>
                    <div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; 
                background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
                        客户</div>
                </template>
                <el-option v-for="item in supplierUserData" :key="item.key" :label="item.value"
                    :value="item.key"></el-option>
            </el-select>

            <el-input v-model="query.input" placeholder="请输入关键词进行检索" size="small"
                style="margin-left:10px;width:400px" />
            <el-button type="primary" icon="el-icon-search" size="small" style="margin-left:10px"
                @click="getTableData(true)">
                搜索
            </el-button>
        </div>

        <!-- 表格 -->
        <el-table v-loading="loading" :data="tableData" border style="width: 100%; margin-top: 10px;"
            @row-click="handleRead">
            <el-table-column prop="goodsName" label="名称" align="center" show-overflow-tooltip />
            <el-table-column prop="customerNames" label="客户名称" align="center" :show-overflow-tooltip="true" />
            <el-table-column prop="goodsNames" label="物品名称" align="center" :show-overflow-tooltip="true" />
            <el-table-column prop="orderNum" label="订单量" align="center" :show-overflow-tooltip="true" />
            <el-table-column prop="orderAmount" label="订单金额" align="center" show-overflow-tooltip />
        </el-table>

        <!-- 分页 -->
        <div class="pagination">
            <el-pagination v-model:current-page="page.index" :page-size="7" :total="page.total" background
                layout="total, prev, pager, next, jumper" @current-change="getTableData(false)" />
        </div>
    </div>
    <OrderDetail v-if="orderLayer.row" :row="orderLayer.row" v-model:show="orderLayer.show" :title="orderLayer.title" />
</template>

<script lang="ts" setup>
import { ref, reactive, onMounted } from "vue"
import { getOrders } from "@/api/order/orderplan";
import permission from "@/utils/system/permission";
import { getOptions } from '@/api/order/orderplan';
import OrderDetail from "./planditails.vue";
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
    customerName: "",
})
const supplierUserData = ref(new Array<any>());
// 表格数据
const tableData = ref<any[]>([])
const loading = ref(false)

const getDetailStatusGroupData = () => {
    getOptions().then((res: any) => {
        supplierUserData.value = res.data.userOptions;
        //goodsData.value=res.data.goodsNameOptions; 
    })
}
// 获取表格数据
const getTableData = (init: Boolean) => {
    loading.value = true
    if (init) {
        page.index = 1
    }

    loading.value = false
    getOrders(permission.getOperator().userId, page.size, page.index, page.orderField, page.orderType, query.input)
        .then((res) => {
            let data = res.data.rows
            data.forEach((d: any) => {
                d.loading = false
            })
            tableData.value = data
            page.total = Number(res.data.total);
            // query.sumWorkpieceTray = res.data.sum;
            // query.sumPallet = res.data.count;
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
const orderLayer = ref({
  show: false,
  title: "",
  row: null
});
const handleRead = (row: any) => {
  orderLayer.value = {
    show: true,
    title: "查看订单计划",
    row
  };
};
onMounted(() => {
    getDetailStatusGroupData();
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
