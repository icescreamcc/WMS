 
<template>
  <el-upload 
           ref="uploadDom"
           :disabled="!uploadParams.isEdit"
           class="upload-demo"  
           drag
            :action="uploadUrl"
            :headers="headersObj" 
            list-type="text"
            :file-list="fileList" 
            :on-remove="handleRemove"  
            :before-upload="onBeforeUpload"
            :on-success="uploadSuccessHandle"
            :on-error="uploadErrorHandle"
            :accept="'.'+props.uploadParams.validFileType"
            :style="{width:`${props.uploadParams.width} !important`,height:`${props.uploadParams.height} !important`}"
          >
          <el-icon class="el-icon--upload" style="font-size: 20px;"><upload-filled /></el-icon>
        <div class="el-upload__text"  style="width: 100%;font-size: 12px;height: 21px;">
         拖拽文件到这里或<em>点击上传</em>  
        </div>
        <div style="font-size: 12px; color: #888;">仅限{{props.uploadParams.validFileType}}文件，大小不超过{{props.uploadParams.validFileSize}}MB</div>
    </el-upload> 
</template>
<script lang="ts" setup>
import {   ref,defineEmits,defineProps,onMounted } from "vue";
import { UploadFile } from 'element-plus/lib/el-upload/src/upload.type' 
import store from '@/store'
import msg from '@/utils/system/message'
import { UploadFilled, Plus} from '@element-plus/icons-vue'  

const props=defineProps({
  uploadParams: {
        type: Object,
        default: () => {
          return {
            uploadApi:'', 
            limit:3,
            fileUrlList:[],
            validFileType:'',
            validFileSize:5,
            isEdit:true,
            titile:'',
            width:'',
            height:'' 
          }
        }
      }
  });
const headersObj={authorization:store.getters['user/token']}
const baseURL: any = import.meta.env.VITE_BASE_URL 
const uploadUrl=baseURL+props.uploadParams.uploadApi
const fileList = ref(props.uploadParams.fileUrlList||[])   
const uploadDom:any = ref()
const emit = defineEmits(['uploadSuccess','fileRemove']);   

 const handleRemove = (uploadFile:any, uploadFiles:any) => {  
    fileList.value=uploadFiles 
    emit("fileRemove",fileList.value)
}
 
 
  const onBeforeUpload=(rawFile:any)=>{ 
    if(fileList.value.find((f:any)=>f.name==rawFile.name)){
      msg.deftAuto("文件重复上传");
      return false;
    }
    console.log("onBeforeUpload",rawFile)
    if(rawFile.type!=`application/${props.uploadParams.validFileType.toLowerCase()}`){
      msg.warningAuto(`请上传${props.uploadParams.validFileType}类型的文件`);
      return false;
    }
    if((rawFile.size/1024/1024)>props.uploadParams.validFileSize){
      msg.warningAuto("上传的文件大小超出了系统允许的范围");
      return false;
    }
  }

  const uploadErrorHandle=(res:any)=>{   
    if(res.message){
      msg.errorAuto(JSON.parse(res.message).message,5000)
    }
    else{
      msg.errorAuto("上传失败，未知原因")
    }
  }

  const uploadSuccessHandle=(res: any)=>{   
      if(res.status=="Success"){
        uploadDom.value!.clearFiles();
        fileList.value.push({
          name:res.data.key,
          url:res.data.value
        })
        msg.successAuto("上传成功");  
        emit('uploadSuccess',fileList.value) 
      }  
  }
 
</script>

<style lang="scss" > 
.upload-demo{
    height: inherit !important;
        width: inherit !important; 
    .el-upload {
      height: inherit !important;
        width: inherit !important; 
        .el-upload-dragger{
          height: inherit !important;
          width: inherit !important; 
        }
    }
    .el-upload-list{  
      position: absolute !important;
      width: inherit !important; 
      top: 85% !important;
      width: 73% !important;
    } 
  }
  
</style>