<template>
  <Layer :layer="layer"> 
      <div class="upload-content">
        <el-upload class="upload-demo" drag ref="uploadImport" :action="uploadUrl" :headers="headersObj" list-type="text"
         :on-success="importSuccessHandle" :on-error="importErrorHandle" :before-upload="onBeforeUpload"  accept=".xlsx, .xls" limit=1>
        <el-icon class="el-icon--upload"><upload-filled /></el-icon>
        <div class="el-upload__text"  style="width: 100%;">
         拖拽文件到这里或<em>点击上传</em>
        </div>
        <template #tip>
          <div class="el-upload__tip" style="width: 100%;">
            仅限.xlsx/.xls文件，大小不超过20MB
          </div>
        </template>
      </el-upload>
    </div> 
  </Layer>
</template>

<script lang="ts" setup>
import { onMounted, defineEmits,defineProps, ref } from 'vue'
import Layer from '@/components/layer/index.vue'    
import msg from '@/utils/system/message'  
import { UploadFilled } from '@element-plus/icons-vue'
import store from '@/store'; 
import permission from '@/utils/system/permission';

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
            data:null
          }
        }
      }
  });
  const headersObj = { authorization: store.getters['user/token'],userId:permission.getOperator().userId };
  const baseURL: any = import.meta.env.VITE_BASE_URL;
  const uploadImport:any = ref()
  const uploadUrl = baseURL+props.layer.apiurl;
  const emit = defineEmits(['dataSubmit']);
 
  const onBeforeUpload=(rawFile:any)=>{ 
    if((rawFile.size/1024/1024)>20){
      msg.errorAuto("上传的文件大小超出了系统允许的范围");
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
        uploadImport.value!.clearFiles()
        msg.successAuto("导入成功"); 
        emit('dataSubmit') 
      }  
  } 
</script>

<style lang="scss" scoped>
   .field-chk{
     width: 100px;
     height: 35px;
   
   }
    ::v-deep .el-upload{ 
       width: 80%;
       .el-upload-dragger{
        width: 100%; 
       }
    }
</style>