<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
        <el-button v-if="permission.isPermisstion('OUTSTORAGEORDERADD')" type="primary"
          icon="el-icon-circle-plus-outline" @click="handleAdd">新增</el-button>
        <el-popconfirm title='确定删除选中的数据吗' v-if="permission.isPermisstion('OUTSTORAGEORDERDEL')"
          @confirm="handleDel(chooseData)">
          <template #reference>
            <el-button type="danger" icon="el-icon-delete" :disabled="chooseData.length === 0">批量删除</el-button>
          </template>
        </el-popconfirm>
        <el-popconfirm :title='apprvalMsg'
          v-if="permission.isPermisstion('OUTSTORAGEORDERAPPROVAL') && isOutStorageApproval"
          @confirm="handleApproval(chooseData)">
          <template #reference>
            <el-button type="warning" icon="el-icon-delete" :disabled="chooseData.length === 0">批量审批</el-button>
          </template>
        </el-popconfirm>
      </div>
      <div class="layout-container-form-search">
        <el-select v-model="selectedGoodsGroup" size="small" class="m-2" style="width:100%;margin-right:10px;"
          @change="getTableData(true)" placeholder="选择物品大类">
          <template #prefix>
            <div
              style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              分类</div>
          </template>
          <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key">
          </el-option>
        </el-select>
        <el-date-picker v-model="dateRange" type="daterange" range-separator="-" start-placeholder="最早日期"
          end-placeholder="最晚日期" size="small" value-format="YYYY-MM-DD" style="margin-right:10px;width:100%">
        </el-date-picker>
        <el-input v-model="query.input" placeholder="请输入关键词进行检索(单号/备注/创建人)" size="small"></el-input>
        <el-button type="primary" icon="el-icon-search" class="search-btn" @click="getTableData(true)">搜索</el-button>
        <el-button v-if="permission.isPermisstion('OUTSTORAGEORDEREXPORT')" icon="el-icon-download"
          style="margin-left:20px" type="info" @click="exportData">导出</el-button>
      </div>
    </div>
    <div class="layout-container-table">
      <Table ref="table" v-model:page="page" v-loading="loading" :showSelection="true" :data="tableData"
        @getTableData="getTableData" @selection-change="handleSelectionChange" @orderChanged="handleSortChange"
        @expandChange="handleExpandChange">
        <!-- <el-table-column prop="orderNo" label="明细" type="expand" align="center" sortable :show-overflow-tooltip="true">
          <template #default="props">
            <div style="margin-bottom:10px">
              <span style="font-weight:600;">出库明细</span>
            </div>
            <div>
              <el-table :data="detailList.filter(x => x.orderNo == props.row.orderNo)" style="width: 100%">
                <el-table-column prop="goodsName" label="名称" width="180" />
                <el-table-column prop="goodsModel" label="型号" width="150" />
                <el-table-column prop="goodsNo" label="SAP编码" width="150" />
                <el-table-column prop="goodsClassifyName" label="类型" width="180" />
                <el-table-column prop="quantity" label="计划出库">
                  <template #default="detail">
                    <span>{{ detail.row.quantity + detail.row.unitName }}</span>
                  </template>
                </el-table-column>
                <el-table-column prop="actualQuantity" label="实际出库">
                  <template #default="detail">
                    <span>{{ detail.row.actualQuantity + detail.row.unitName }}</span>
                  </template>
                </el-table-column>
                <el-table-column prop="warehouseName" label="出库仓库" />
                <el-table-column prop="binName" label="出库货位" />
                <el-table-column prop="workbinCellNo" label="出库料箱" />
              </el-table>
            </div>
          </template>
        </el-table-column> -->
        <el-table-column prop="orderNo" label="出库单号" align="center" sortable="custom" min-width="100"
          :show-overflow-tooltip="true" />
        <el-table-column prop="goodsName" label="名称" align="center" sortable="custom" min-width="100"
          :show-overflow-tooltip="true" />
        <!-- <el-table-column prop="quantity" label="计划入库数量" align="center" sortable="custom" min-width="100"
          :show-overflow-tooltip="true">  
                 <template #default="detail">
                  <span>{{ detail.row.quantity + detail.row.unitName }}</span>
                  </template>
        </el-table-column> -->
        <el-table-column prop="actualQuantity" label="实际入库数量" align="center" sortable="custom" min-width="100"
          :show-overflow-tooltip="true">
          <template #default="detail">
            <span>{{ detail.row.quantity + detail.row.unitName }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="warehouseName" label="出库仓库" align="center" sortable="custom" min-width="100"
          :show-overflow-tooltip="true" />
        <el-table-column prop="outStorageType" label="出库类型" align="center" sortable="custom" min-width="100"
          :show-overflow-tooltip="true">
          <template #default="props">
            <span>{{ props.row.outStorageTypeDesc }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="createUserName" label="创建人" align="center" sortable="custom"
          :show-overflow-tooltip="true" />
        <el-table-column prop="createDate" label="创建时间" align="center" sortable="custom" :show-overflow-tooltip="true">
          <template #default="props">
            <span>{{ commonHelper.formatToDateTime(props.row.createDate) }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="outStorageDate" label="出库时间" align="center" sortable="custom"
          :show-overflow-tooltip="true">
          <template #default="props">
            <span>{{ commonHelper.formatToDateTime(props.row.outStorageDate) }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="remark" label="备注" align="center" sortable="custom" :show-overflow-tooltip="true" />
        <el-table-column prop="status" label="状态" align="center" sortable="custom" :show-overflow-tooltip="true">
          <template #default="props">
            <div v-if="props.row.status == 'QualityFailed' || props.row.status == 'Reject'" class="text-danger">
              {{ props.row.statusDesc }}</div>
            <div v-else-if="props.row.status == 'WaitOutStorage'">
              <el-popconfirm title="是否确认出库？" v-if="permission.isPermisstion('OUTSTORAGEORDERCONFIRM')"
                @confirm="submitOutStorage(props.row)">
                <template #reference>
                  <el-button title="点击确认出库" type="warning" :loading="props.row.loading">{{ props.row.statusDesc
                    }}</el-button>
                </template>
              </el-popconfirm>
              <span v-else>{{ props.row.statusDesc }}</span>
            </div>
            <span v-else>{{ props.row.statusDesc }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="approvalDate" v-if="isOutStorageApproval" label="审批时间" align="center" min-width="130"
          sortable="custom" :show-overflow-tooltip="true" />
        <el-table-column :label="$t('message.common.handle')" align="left" min-width="120" fixed="right"
          v-if="permission.isPermisstion('OUTSTORAGEORDERUPDATE', 'OUTSTORAGEORDERDEL')">
          <template #default="scope">
            <div v-if="scope.row.status == 'WaitOutStorage'">
              <el-button @click="handleEdit(scope.row)" v-if="permission.isPermisstion('OUTSTORAGEORDERUPDATE')">{{
                $t("message.common.update") }}</el-button>
              <el-popconfirm v-if="permission.isPermisstion('OUTSTORAGEORDERDEL')" :title="$t('message.common.delTip')"
                @confirm="handleDel([scope.row])">
                <template #reference>
                  <el-button type="danger">{{ $t("message.common.del") }}</el-button>
                </template>
              </el-popconfirm>
            </div>
            <div v-else>
              <el-button @click="handleRead(scope.row)">{{ $t("message.common.read") }}</el-button>
            </div>
          </template>
        </el-table-column>
      </Table>
      <OrderEditModal :layer="orderLayer" @dataSubmit="dataSave" v-if="orderLayer.show" />
      <ExportModal :layer="exportLayer" @dataSubmit="exportData" v-if="exportLayer.show" />
      <ApprovalModal :layer="approvalLayer" @dataSubmit="approvalSubmit" v-if="approvalLayer.show" />
    </div>
  </div>
</template>

<script lang="ts" setup>
defineOptions({
  name: "outstorage"
})
import { ref, reactive, onMounted } from "vue";
import { Page } from "@/components/table/type";
import { getOrders, getArgs, getOrderDetail, addOutStorage, updateOutStorage, delOutStorage, exportOutStorage, getAllowField, approvalOutStorage, confirmOutStorage } from "@/api/inv/outstorage";
import { LayerInterface } from "@/components/layer/index.vue";
import Table from "@/components/table/tableServer.vue";
import OrderEditModal from "./orderEditLayer.vue";
import ApprovalModal from '@/components/layer/approvalLayer.vue';
import ExportModal from "@/components/layer/exportLayer.vue";
import permission from '@/utils/system/permission';
import commonHelper from "@/utils/system/common-helper";
import { getGoodsGroup } from '@/api/common';
import { deftClassifyGroup } from '@/config';

const query = reactive({
  input: "",
});
let dateRange = ref()
const page: Page = reactive({
  index: 1,
  size: 20,
  total: 0,
  orderField: '',
  orderType: ''
});
const loading = ref(true);
const tableData = ref([]);
const chooseData = ref([]);
const apprvalMsg = ref("")
const goodsGroupData = ref(new Array<any>());
const selectedGoodsGroup = ref(deftClassifyGroup);
const isOutStorageApproval = ref(false);
const orderLayer: LayerInterface = reactive({
  show: false,
  title: "",
  showButton: true,
  btnLoading: false,
  width: "75%",
  data: null,
  otherButton: {
    show: false,
    otherBtnLoading: false,
    text: "",
    type: ""
  }
});
const approvalLayer: LayerInterface = reactive({
  show: false,
  title: "出库审批",
  showButton: true,
  btnLoading: false,
  width: "40%",
  data: null,
  otherButton: {
    show: false,
    otherBtnLoading: false,
    text: "",
    type: ""
  }
})

onMounted(() => {
  getArgs().then(res => {
    isOutStorageApproval.value = res.data.isOutStorageApproval
  })
  getGoodsGroupData();
  getTableData(true);
})

const getGoodsGroupData = () => {
  getGoodsGroup().then(res => {
    if (deftClassifyGroup == 'SparePart') {
      goodsGroupData.value = res.data.filter((f: any) => f.key == 'SparePart' || f.key == 'Consumables');
    }
    else {
      goodsGroupData.value = res.data.filter((f: any) => f.key == 'FinishedProduct' || f.key == 'RawMaterial');;
    }
  })
}

const getTableData = (init: Boolean) => {
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
  loading.value = false
  getOrders(permission.getOperator().userId, page.size, page.index, page.orderField, page.orderType, query.input, dateStart, dateEnd, selectedGoodsGroup.value)
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

//展开与收缩
const detailList = ref(new Array<any>())
const handleExpandChange = (row: any) => {
  getOrderDetail(permission.getOperator().userId, row.orderNo).then(res => {
    detailList.value = detailList.value.filter((b: any) => b.orderNo != row.orderNo)
    res.data?.forEach((b: any) => {
      detailList.value.push(b)
    });
  })
}

//排序事件
const handleSortChange = (orderRow: any) => {
  getTableData(true);
}

//批量选择
const handleSelectionChange = (val: []) => {
  chooseData.value = val;
  let isApprovalOrderNo = val.filter((x: any) => x.isApproval)
  if (isApprovalOrderNo.length != val.length) {
    apprvalMsg.value = "当前选择存在不符合审批条件的数据"
  }
  else {
    apprvalMsg.value = "确定审批选中的数据吗"
  }
};

const handleDel = (data: any[]) => {
  let ids = Array<any>();
  data.forEach(d => {
    ids.push(d.orderNo);
  })
  delOutStorage(ids).then((res) => {
    getTableData(tableData.value.length === 1 ? true : false);
  });
}

//审批弹窗功能 
const handleApproval = (data: any[]) => {
  let isApprovalOrderNo = data.filter(x => x.isApproval).map(x => { return x.orderNo })
  if (isApprovalOrderNo.length == data.length) {
    approvalLayer.show = true;
    approvalLayer.data = data;
  }
}

//审批数据提交
const approvalSubmit = (orders: any[], data: any) => {
  let orderNo = orders.filter(x => x.isApproval).map(x => { return x.orderNo })
  approvalLayer.btnLoading = true
  approvalOutStorage(orderNo, data.isApprove, data.opinion, permission.getOperator().userId, permission.getOperator().userName)
    .then(res => {
      approvalLayer.show = false
      getTableData(false);
    })
    .finally(() => approvalLayer.btnLoading = false)
}

// 新增弹窗功能
const handleAdd = () => {
  orderLayer.title = "新增出库单";
  orderLayer.show = true;
  orderLayer.showButton = true;
  delete orderLayer.data;
}

// 编辑弹窗功能
const handleEdit = (row: any) => {
  getOrderDetail(permission.getOperator().userId, row.orderNo).then((res: any) => {
    orderLayer.title = "编辑出库单";
    orderLayer.show = true;
    orderLayer.showButton = true;
    row.details = res.data
    orderLayer.data = row;
  })
}

const handleRead = (row: any) => {
  getOrderDetail(permission.getOperator().userId, row.orderNo).then((res: any) => {
    orderLayer.title = "出库单";
    orderLayer.showButton = false;
    orderLayer.show = true;
    row.details = res.data;
    orderLayer.data = row;
  })
}

//新增或编辑数据提交
const dataSave = (data: any, actionType: string) => {
  orderLayer.btnLoading = true;
  if (actionType == 'add') {
    addOutStorage(data).then(res => {
      const orderNo = res.data;
      if (data.goodsClassify == 'RawMaterial') {//原材料直接入库
        if (orderNo.length > 0) {
          confirmOutStorage(orderNo)
        }
      }

      orderLayer.show = false;
      getTableData(true);
    }).finally(() => orderLayer.btnLoading = false);
  }
  else {
    updateOutStorage(data).then(res => {
      orderLayer.show = false;
      getTableData(false);
    }).finally(() => orderLayer.btnLoading = false);
  }
}

//确认出库
const submitOutStorage = (row: any) => {
  row.loading = true
  confirmOutStorage(row.orderNo).then(res => {
    getTableData(false);
  }).finally(() => row.loading = false)
}

//数据导出弹窗控制器
const exportLayer: LayerInterface = reactive({
  show: false,
  title: "选择导出字段",
  showButton: true,
  btnLoading: false,
  width: "40%",
  data: null,
  otherButton: {}
});

//导出出库单
const exportData = () => {
  let dateStart = '';
  let dateEnd = '';
  if (dateRange.value?.length > 0) {
    dateStart = dateRange.value[0]
  }
  if (dateRange.value?.length > 1) {
    dateEnd = dateRange.value[1]
  }
  exportLayer.btnLoading = true
  exportOutStorage(query.input, page.orderField, page.orderType, dateStart, dateEnd, selectedGoodsGroup.value).then(res => {
    let link = document.createElement('a')
    link.style.display = 'none'
    link.href = res.data
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
  }).finally(() => exportLayer.btnLoading = false)
}
</script>

<style lang="scss" scoped>
.statusName {
  margin-right: 10px;
}
</style>
