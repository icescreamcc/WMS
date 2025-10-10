<template>
  <el-dialog v-model="innerShow" :title="title" width="90%" append-to-body class="mobile-dialog" @close="handleClose">
    <el-card shadow="never" class="detail-card">
      <div class="detail-item"><span class="label">发货单号:</span><span class="value">{{ row.orderNo }}</span></div>
      <div class="detail-item"><span class="label">订单编号：</span><span class="value">{{ row.customerOrderNo }}</span></div>
      <div class="detail-item"><span class="label">计划发货数量：</span><span class="value">{{ row.planQuantity }}</span></div>
      <div class="detail-item"><span class="label">计划发货日期：</span><span class="value">{{ row.sendingDate }}</span></div>
      <div class="detail-item"><span class="label">要求到货日期：</span><span class="value">{{ row.requestDate }}</span></div>
      <div class="detail-item"><span class="label">运输供应商：</span><span class="value">{{ row.supplierId }}</span></div>
      <div class="detail-item"><span class="label">特殊要求：</span><span class="value">{{ row.specialRequest }}</span></div>
      <div class="detail-item"><span class="label">单据状态：</span><span class="value">{{ row.status }}</span></div>

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
