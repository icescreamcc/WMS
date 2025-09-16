<template>
  <el-upload :disabled="!uploadParams.isEdit" class="muilt-uploader" :action="uploadUrl" :headers="headersObj"
    list-type="picture-card" :file-list="imageUrlList" :on-preview="handlePictureCardPreview" :on-remove="handleRemove"
    :on-change="handleChange" :accept="'image/*'">
    <el-icon>
      <Plus />
    </el-icon>
  </el-upload>
  <el-dialog v-model="dialogVisible">
    <img w-full :src="dialogImageUrl" alt="Preview Image" />
  </el-dialog>
</template>
<script lang="ts">
import { defineComponent, ref } from "vue";
import { UploadFile } from 'element-plus/lib/el-upload/src/upload.type'
import store from '@/store'
import msg from '@/utils/system/message'
import { Plus } from '@element-plus/icons-vue'
export default defineComponent({
  components: {
    Plus
  },
  props: {
    uploadParams: {
      type: Object,
      default: () => {
        return {
          uploadApi: '',
          limit: 3,
          imgUrlList: [],
          validFileType: '',
          validFileSize: 1024,
          isEdit: true,
          titile: '',
          width: '',
          height: ''
        }
      }
    }
  },
  setup(props, context) {
    const headersObj = { authorization: store.getters['user/token'] }
    const baseURL: any = import.meta.env.VITE_BASE_URL
    const uploadUrl = baseURL + props.uploadParams.uploadApi
    const imageUrlList = ref(props.uploadParams.imgUrlList || [])

    const dialogImageUrl = ref('')
    const dialogVisible = ref(false)

    //删除图片
    const handleRemove = (uploadFile: any, uploadFiles: any) => {
      imageUrlList.value = uploadFiles
      context.emit("handleImgChanged", imageUrlList.value)
    }

    //点击预览
    const handlePictureCardPreview = (uploadFile: any) => {
      dialogImageUrl.value = uploadFile.url!
      dialogVisible.value = true
    }

    //选择文件,上传成功时将后端url添加进文件列表
    const handleChange = (file: any, fileList: UploadFile[]) => {
      if (fileList.length > props.uploadParams.limit) {
        msg.warningAuto("已超出允许上传的数量")
        fileList.splice(fileList.length - 1, 1)
        return false
      }
      if (file.raw.size / 1024 > props.uploadParams.validFileSize) {
        msg.warningAuto(`上传文件大小不符合要求，请不要超过${props.uploadParams.validFileSize}kb`)
        fileList.splice(fileList.length - 1, 1)
        return false
      }
      if (file.raw.type.indexOf(props.uploadParams.validFileType) < 0) {
        msg.warningAuto(`上传文件大小不符合要求，请不要超过${props.uploadParams.validFileSize}kb`)
        fileList.splice(fileList.length - 1, 1)
        return false
      }
      //上传成功
      if(file?.response){
       if(file.response.status=="Success"){ 
         imageUrlList.value.push({
             name:file.name,
             url:file.response.data
         })  
        context.emit("handleImgChanged",imageUrlList.value)
      }
        else{
            msg.errorAuto(file.response.message||"上传失败，未知原因")
        } 
      } 
    }


    return {
      uploadUrl,
      headersObj,
      imageUrlList,
      dialogImageUrl,
      dialogVisible,
      handleRemove,
      handlePictureCardPreview,
      handleChange
    }
  }
})
</script>

<style lang="scss">
@media screen and (max-width: 450px) {
  .muilt-uploader {
    .el-upload-list--picture-card .el-upload-list__item {
      height: 70px !important;
      width: 70px !important;
      float: left !important;
    }

    .el-upload--picture-card {
      height: 70px !important;
      width: 70px !important;
      float: left !important;
      line-height: 70px !important;

      .el-icon {
        position: relative !important;
        top: 10px !important;
      }
    }
  }
}

@media screen and (min-width: 450px) {
  .muilt-uploader {
    .el-upload-list--picture-card .el-upload-list__item {
      height: 90px !important;
      width: 90px !important;
      float: left !important;
    }

    .el-upload--picture-card {
      height: 90px !important;
      width: 90px !important;
      float: left !important;
      line-height: 90px !important;

      .el-icon {
        position: relative !important;
        top: 10px !important;
      }
    }
  }
}
</style>