<template>
    <Layer :layer="layer"> 
      <el-table class="system-table" ref="reftable"  border  height="250"  :data="layer.data">
        <el-table-column prop="sender" label="发送人" align="center"  min-width="100" :show-overflow-tooltip="true"/> 
        <el-table-column prop="createDate" label="发送时间" align="center"  min-width="100" :show-overflow-tooltip="true">
          <template #default="scope">   
                 <span>{{ commonHelper.formatToDateTime(scope.row.createDate) }}</span>
              </template>
          </el-table-column>  
        <el-table-column prop="toReceiver" label="收件人" align="center"  min-width="100" :show-overflow-tooltip="true">
          <template #default="scope">   
                 <span>{{ scope.row.toReceiver.join(',') }}</span>
              </template>
          </el-table-column>  
        <el-table-column prop="toCC" label="抄送人" align="center"  min-width="100" :show-overflow-tooltip="true"> 
          <template #default="scope">   
                 <span>{{ scope.row.toCC?.join(',') }}</span>
              </template>
          </el-table-column>  
        <el-table-column prop="subject" label="主题" align="center"  min-width="100" :show-overflow-tooltip="true"/> 
        <el-table-column prop="body" label="内容" align="center"  min-width="100" :show-overflow-tooltip="true"/> 
        <el-table-column prop="attachments" label="附件" align="center"  min-width="100" :show-overflow-tooltip="true">
          <template #default="scope">   
                <div v-if="scope.row.attachments" v-for="att in scope.row.attachments"> 
                  <el-button  style="height: 30px;" :title="att.fileName" circle>
                    <a :href="att.url"><img src="../../../../public/icon-img/fujian.png" height="16" :title="att.fileName"></a>
                </el-button>  
                </div>
              </template>
          </el-table-column> 
      </el-table>
    </Layer>
  </template>
  
  <script lang="ts" setup>
  import { onMounted,defineProps,ref,} from 'vue';
  import { ElForm } from 'element-plus'; 
  import Layer from '@/components/layer/index.vue' 
  import commonHelper from "@/utils/system/common-helper";

  const props=defineProps({
    layer: {
        type: Object,
        default: () => {
          return {
            show: false,
            title:'',
            width:"40%",
            showButton: true,
            btnLoading:false, 
            data:null,
            selected:null
          }
        }
      }
  });      
  </script>
  
  <style lang="scss" scoped>  
  .text-ellipsis {
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
    max-width: 120px;
  }
  </style>