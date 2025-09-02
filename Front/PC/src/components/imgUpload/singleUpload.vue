 
<template>
     <el-upload
           :disabled="!uploadParams.isEdit"
            class="avatar-uploader"  
            :action="uploadUrl"
            :headers="headersObj"
            :show-file-list="false"
            :on-success="handleAvatarSuccess"
            :before-upload="beforeAvatarUpload"
            :on-progress="handleProgress"
            :on-change="handleChange"
          >
         <el-row>
            <el-col :span="24"  v-if="imageUrl||(imageUrl&&uploadParams.isEdit)">
                <img  :src="imageUrl" class="avatar"  :style="{width:uploadParams.width,height:uploadParams.height}"/>    
            </el-col>
            <el-col :span="24"  v-if="!imageUrl&&!uploadParams.isEdit">  
                <div  class="image-empty el-icon-document-delete" :style="{width:uploadParams.width,height:uploadParams.height}"></div>  
            </el-col>
            <el-col :span="24"  v-if="!imageUrl&&uploadParams.isEdit">
                   <div  class="upload-icon" :style="{width:uploadParams.width,height:uploadParams.height}">
                    <el-icon class="el-icon-upload" ><plus /></el-icon> 
                    <p>{{uploadParams.titile}}</p>
                  </div> 
            </el-col>
         </el-row> 
          </el-upload>
</template>
<script lang="ts">
import { computed, defineComponent, ref, watch } from "vue";
import { ElFile, ElUploadProgressEvent, UploadFile } from 'element-plus/lib/el-upload/src/upload.type' 
import store from '@/store'
import msg from '@/utils/system/message'
export default defineComponent({
props:{
    uploadParams:{
        type:Object,
        default:()=>{
            return{
                isMuilt:false,
                uploadApi:'', 
                imgUrl:'',
                validFileType:'',
                validFileSize:1024,
                isEdit:false,
                titile:'',
                width:'',
                height:''
            }
        }
    } 
},
setup(props, context){

const headersObj={authorization:store.getters['user/token']}
const baseURL: any = import.meta.env.VITE_BASE_URL 
const uploadUrl=baseURL+props.uploadParams.uploadApi
const imageUrl = ref(props.uploadParams.imgUrl)  

watch(()=>props.uploadParams.imgUrl,(newVal,oldVal)=>{ 
  imageUrl.value=newVal 
})
 
//选择文件
const handleChange=(file: any, fileList: UploadFile[])=>{
  if(!props.uploadParams.isMuilt&&fileList.length>1){
    fileList.splice(0,1)
  }
  imageUrl.value= URL.createObjectURL(file.raw)  
  if(file?.response?.status!="Success"){
     imageUrl.value=null
  } 
}

//开始上传
const beforeAvatarUpload=(res: any)=>{ 
  let isJPG = res.type.indexOf(props.uploadParams.validFileType)>=0
  let isLt2M = (res.size / 1024 ) < props.uploadParams.validFileSize

  if (!isJPG) { 
      msg.warningAuto("上传文件格式不符合要求")
      return false
  }
  if (!isLt2M) { 
      msg.warningAuto(`上传文件大小不符合要求，请不要超过${props.uploadParams.validFileSize}kb`)
      return false
  } 
  return true
}

//上传中
const handleProgress=(evt:ElUploadProgressEvent, file: UploadFile, fileList: UploadFile[])=>{

}

//上传成功
const handleAvatarSuccess=(res: any)=>{  
    if(res.status=="Success"){
      context.emit("handleSuccess",res.data)
    }
    else{
        msg.errorAuto(res.message||"上传失败，未知原因")
    }

}
return{
  uploadUrl,
  headersObj,
  imageUrl, 
  beforeAvatarUpload,
  handleAvatarSuccess,
  handleChange,
  handleProgress
}
}
})
</script>

<style lang="scss" scoped>
.image-empty {
  display: flex;
  justify-content: center;
  align-items: center; 
  font-size: 30px;
    padding: 5px 5px 2px 5px;
    color: rgb(185, 184, 184);
   border: 1px dotted rgb(185, 184, 184);
   border-radius: 2px;
}
.avatar { 
  display: block;
   padding: 5px 5px 2px 5px;
     border: 1px dotted rgb(185, 184, 184);
     border-radius: 4px;
}
.upload-icon{ 
  padding: 5px 5px 2px 5px;
  border: 1px dotted rgb(185, 184, 184);
   border-radius: 2px;
  margin-left: 5px;
  i{
    font-size: 35px;
    color: rgb(185, 184, 184);
    position: relative;
    top:35px
  }
  p{
    font-size: 12px;
    color: rgb(185, 184, 184);
     position: relative;
    top:20px
  }
}
</style>