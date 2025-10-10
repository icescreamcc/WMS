<template>
  <el-dialog v-model="innerShow" :title="title" width="90%" append-to-body class="mobile-dialog" @close="handleClose">
    <el-card shadow="never" class="detail-card">
      <div class="detail-item"><span class="label">订单号：</span><span class="value">{{ row.orderNo }}</span></div>
      <div class="detail-item"><span class="label">合同号：</span><span class="value">{{ row.contractNo }}</span></div>
      <div class="detail-item"><span class="label">签约日期：</span><span class="value">{{ row.signingDate }}</span></div>
      <div class="detail-item"><span class="label">客户名称：</span><span class="value">{{ row.customerName }}</span></div>
      <div class="detail-item"><span class="label">物品名称：</span><span class="value">{{ row.goodsName }}</span></div>
      <div class="detail-item"><span class="label">订单量：</span><span class="value">{{ row.orderNum }}</span></div>
      <div class="detail-item"><span class="label">订单金额：</span><span class="value">{{ row.orderAmount }}</span></div>
      <div class="detail-item"><span class="label">单位：</span><span class="value">{{ row.unit }}</span></div>
      <div class="detail-item"><span class="label">交货截止日期：</span><span class="value">{{ row.deliveryDate }}</span></div>
      <div class="detail-item"><span class="label">备注：</span><span class="value">{{ row.remarks }}</span></div>
    </el-card>

    <template #footer>
      <el-button @click="handleClose">取消</el-button>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup>
import { defineProps, defineEmits, ref, watch } from "vue";

const props = defineProps({
  row: { type: Object as () => any, required: true },
  show: { type: Boolean, default: false },
  title: { type: String, default: "查看订单详情" }
});
const emit = defineEmits(["update:show"]);

const innerShow = ref(props.show);

// 父 -> 子同步 show
watch(() => props.show, (val) => {
  innerShow.value = val;
});

// 子 -> 父同步 show
const handleClose = () => {
  innerShow.value = false;
  emit("update:show", false);
};
</script>

<style scoped>
.detail-card {
  font-size: 14px;
}

.detail-item {
  display: flex;
  justify-content: space-between;
  padding: 6px 0;
  border-bottom: 1px solid #eee;
}

.detail-item:last-child {
  border-bottom: none;
}

.label {
  color: #666;
  font-weight: 500;
}

.value {
  color: #333;
}

.mobile-dialog>>>.el-dialog__body {
  padding: 10px 20px;
}
</style>
