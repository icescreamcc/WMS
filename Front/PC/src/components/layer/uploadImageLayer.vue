<template>
  <!-- <Layer :layer="layer">  -->
    <div class="upload-content" v-show="props.layer.showButton">
        <el-upload class="upload-demo" drag ref="uploadImport" :action="uploadUrl" :headers="headersObj" list-type="text"
         :on-success="importSuccessHandle" :on-error="importErrorHandle" :before-upload="onBeforeUpload"  accept=".jpg,.jpeg,.png" limit=1>
        <el-icon class="el-icon--upload"><upload-filled /></el-icon>
        <div class="el-upload__text"  style="width: 100%;">
         拖拽文件到这里或<em>点击上传</em>
        </div>
        <template #tip>
          <div class="el-upload__tip" style="width: 100%;">
            仅限.jpg,.jpeg,.png图片，大小不超过20MB
          </div>
        </template>
      </el-upload>
    </div> 

    <div class="layout-container">
      <div class="layout-container-form flex space-between">
        <div class="layout-container-form-handle">
          <el-popconfirm title='确定删除选中的数据吗？' v-if="permission.isPermisstion(props.layer.deletepermission)"
          @confirm="handleDel(chooseData)">
          <template #reference>
            <el-button type="danger" icon="el-icon-delete" :disabled="chooseData.length === 0">批量删除</el-button>
          </template>
        </el-popconfirm>
        </div>
        <div class="layout-container-form-search">
          <el-input v-model="query.input" placeholder="请输入关键词进行检索" size="small" style="width: 80%;margin-left: 10px;"
            clearable @clear="getTableData(true)"></el-input>
          <el-button type="primary" icon="el-icon-search" class="search-btn" @click="getTableData(true)">搜索</el-button>
        </div>
      </div>
      <div class="layout-container-table">
 <Table ref="table" v-model:page="page" v-loading="loading" :showSelection="true" :data="tableData"
          @getTableData="getTableData" @orderChanged="handleSortChange" @selection-change="handleSelectionChange">
          <el-table-column prop="fileName" label="图片名称" align="center" sortable="custom" min-width="160"
            :show-overflow-tooltip="true" />
          <el-table-column prop="url" label="图片预览" align="center" min-width="120"
            :show-overflow-tooltip="true" >
            <template #default="props">
              <el-image
                style="width: 100px; height: 100px"
                :src="props.row.url"
                :zoom-rate="1.2"
                :max-scale="7"
                :min-scale="0.2"
                :preview-src-list="srcList"
                show-progress
                fit="cover"
              />
            </template>
          </el-table-column>
          <el-table-column prop="url" label="图片地址" align="center" sortable="custom" min-width="300"
            :show-overflow-tooltip="true" >
            <template #default="props">
              <el-link type="primary" :href="props.row.url" style="color:#409eff;" show-overflow-tooltip>{{props.row.url}}</el-link>
            </template>
          </el-table-column>
          <el-table-column :label="$t('message.common.handle')" align="center" min-width="60"
         v-if="permission.isPermisstion(props.layer.deletepermission)" >
          <template #default="scope">
            <el-popconfirm  v-if="permission.isPermisstion(props.layer.deletepermission)"
              :title="$t('message.common.delTip')" @confirm="handleDel([scope.row])">
              <template #reference>
                <el-button type="danger">{{ $t("message.common.del") }}</el-button>
              </template>
            </el-popconfirm>
          </template>
        </el-table-column>
        </Table>
      </div>
    </div>

  <!-- </Layer> -->
</template>

<script lang="ts" setup>
import { onMounted, defineEmits,defineProps, ref,reactive } from 'vue'
// import Layer from '@/components/layer/index.vue'    
import msg from '@/utils/system/message'  
import { UploadFilled } from '@element-plus/icons-vue'
import store from '@/store'; 
import permission from '@/utils/system/permission';
import { Page } from "@/components/table/type";
import Table from "@/components/table/tableServer.vue";
import { getBaseFiles } from '@/api/purchase/receivingorder'
import { delFiles } from '@/api/baseinfo/goods'; 

const props=defineProps({
    layer: {
        type: Object,
        default: () => {
          return {
            show: false,
            title:'',
            width:"40%",
            showButton: false , 
            options:null,
            apiurl:null,
            deleteapiurl:null,
            deletepermission:null,
            data:null
          }
        }
      }
  });
  const chooseData = ref([]);
  const headersObj = { authorization: store.getters['user/token'],userId:permission.getOperator().userId };
  const baseURL: any = import.meta.env.VITE_BASE_URL;
  const uploadImport:any = ref()
  const uploadUrl = baseURL+props.layer.apiurl+`?orderNo=${props.layer.data?.orderNo}`;
  const deleteUrl = baseURL+props.layer.deleteapiurl;
  const emit = defineEmits(['dataSubmit']);
  const query = reactive({
    input: "",
    fileInfoType:"ReceivingOrderAttachment",
  });
  const page: Page = reactive({
  index: 1,
  size: 20,
  total: 0,
  orderField: '',
  orderType: ''
});
const loading = ref(false);
const tableData = ref([]);
const srcList=ref([]);

onMounted(() => {
  getTableData(true);
})

  const getTableData = (init: Boolean) => {
  loading.value = true
  if (init) {
    page.index = 1
  }
  loading.value = false
  getBaseFiles(permission.getOperator().userId, page.size, page.index, page.orderField, page.orderType, query.input, props.layer.data?.orderNo, query.fileInfoType)
    .then((res) => {
      let data = res.data.rows
      data.forEach((d: any) => {
        d.loading = false
        srcList.value.push(d.url);
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
 const handleSelectionChange = (val: []) => {
  chooseData.value = val;
};
const handleDel = (data: any[]) => {
  let fileIds = Array<any>();
  data.forEach(d => {
    fileIds.push(d.fileId);
  })
  delFiles(deleteUrl,fileIds).then((res) => {
    getTableData(tableData.value.length === 1 ? true : false);
  });
}
const handleSortChange = (orderRow: any) => {
  getTableData(true);
}

  const onBeforeUpload=(rawFile:any)=>{ 
    if((rawFile.size/1024/1024)>20){
      msg.errorAuto("上传的图片大小超出了系统允许的范围");
      return false;
    }
  }

  const importErrorHandle=(res:any)=>{  
    if(res.message){
      msg.errorAuto(JSON.parse(res.message).message,5000)
    }
    else{
      msg.errorAuto("上传失败，未知原因")
    }
  }

  const importSuccessHandle=(res: any)=>{   
      if(res.status=="Success"){
        getTableData(true);
        uploadImport.value!.clearFiles()
        msg.successAuto("上传图片成功"); 
        emit('dataSubmit') 
      }  
  } 
</script>

<style lang="scss" scoped>
   .field-chk{
     width: 100px;
     height: 35px;
   
   }

  
</style>