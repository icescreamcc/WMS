<template>
  <div class="page">
    <van-tabs v-model:active="active" swipeable sticky>
      <van-tab title="库存信息">
        <stockinfo v-if="active === 0" />
      </van-tab>
      <van-tab title="入库记录">
        <inrecords v-if="active === 1" />
      </van-tab>
      <van-tab title="出库记录">
        <outrecords v-if="active === 2" />
      </van-tab>
    </van-tabs>
  </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import stockinfo from './stockinfo.vue'
import inrecords from './inrecords.vue'
import outrecords from './outrecords.vue'

const active = ref(0) // 当前 tab 索引
</script>

<style scoped>
.page {
  background: #f5f5f5;
  min-height: 100vh;
}
</style>
