<template>
  <div class="page">
    <van-tabs v-model:active="active" swipeable sticky>
      <van-tab title="订单计划">
        <orderplan v-if="active === 0" />
      </van-tab>
      <van-tab title="发货计划">
        <shipplan v-if="active === 1" />
      </van-tab>

    </van-tabs>
  </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import orderplan from './orderplan.vue'
import shipplan from './shipplan.vue'
const active = ref(0) // 当前 tab 索引
</script>

<style scoped>
.page {
  background: #f5f5f5;
  min-height: 100vh;
}
</style>
