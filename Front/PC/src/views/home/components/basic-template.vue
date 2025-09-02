<template>
  <el-card class="box-card">
    <template #header>
      <div class="card-header">
        <el-row>
          <el-col :span="12">
              <span>企业公告</span>
          </el-col>
            <el-col :span="12" style="text-align: right">
                <span type="primary" style="margin-left: 20px;font-size:small">{{msgDetail?.dateTime}}</span>
          </el-col>
        </el-row> 
      </div>
    </template>
      <div  style="padding:0 20px;font-size:small;text-align: left">
           <p>{{msgDetail?.content?msgDetail?.content:'暂无公告'}}</p> 
        </div> 
  </el-card>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import {getNewMessageDetail} from '@/api/system/messageCompany'
export default defineComponent({
  setup() {
    const msgDetail=ref()
    getNewMessageDetail().then(res=>{
      msgDetail.value=res.data
    })
    return{
      msgDetail
    }
  }
})
</script>

<style lang="scss" scoped>
  .box-card {
    .card-header{
      text-align: left;
    }
    .box {
      .item {
        display: flex;
        align-items: center;
        * {
          margin-right: 20px;
        }
      }
    }
  }
</style>