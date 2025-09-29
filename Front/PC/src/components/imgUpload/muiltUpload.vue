<template>
  <div class="muilt-uploader">
    <el-upload 
      :disabled="!uploadParams.isEdit" 
      :action="uploadUrl" 
      :headers="headersObj"
      list-type="picture-card" 
      :file-list="imageUrlList" 
      :on-preview="handlePictureCardPreview" 
      :on-remove="handleRemove"
      :on-change="handleChange" 
      :before-upload="beforeUpload"
      :accept="uploadParams.accept || '.jpg,.jpeg,.png,.gif,.bmp,.pdf'"
      :limit="uploadParams.limit"
    >
      <el-icon>
        <Plus />
      </el-icon>
      
      <template #file="{ file }">
        <div class="custom-file-item">
          <img v-if="isImageFile(file)" class="el-upload-list__item-thumbnail" :src="file.url" alt="" />
          <div v-else class="file-pdf">
            <el-icon style="font-size: 40px; color: #f56c6c;"><Document /></el-icon>
            <div class="file-name">{{ getFileName(file.name) }}</div>
          </div>
          <span class="el-upload-list__item-actions">
            <span class="el-upload-list__item-preview" @click="handlePictureCardPreview(file)">
              <el-icon><ZoomIn /></el-icon>
            </span>
            <span v-if="uploadParams.isEdit" class="el-upload-list__item-delete" @click="handleRemoveFile(file)">
              <el-icon><Delete /></el-icon>
            </span>
          </span>
        </div>
      </template>
    </el-upload>
    
    <el-dialog v-model="dialogVisible" title="文件预览" width="80%">
      <img v-if="previewType === 'image'" w-full :src="dialogImageUrl" alt="Preview Image" style="max-width: 100%;" />
      <embed v-else-if="previewType === 'pdf'" :src="dialogImageUrl" type="application/pdf" width="100%" height="600px" />
      <div v-else class="unsupported-file">
        <el-icon style="font-size: 48px; color: #909399;"><Document /></el-icon>
        <p>不支持在线预览此文件类型</p>
        <el-button type="primary" @click="downloadFile(dialogImageUrl)">下载文件</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, watch } from "vue";
import { UploadFile } from 'element-plus/lib/el-upload/src/upload.type'
import store from '@/store'
import msg from '@/utils/system/message'
import { Plus, Document, ZoomIn, Delete } from '@element-plus/icons-vue'

export default defineComponent({
  name: 'MuiltUpload',
  components: {
    Plus,
    Document,
    ZoomIn,
    Delete
  },
  props: {
    uploadParams: {
      type: Object,
      default: () => {
        return {
          uploadApi: '',
          limit: 3,
          imgUrlList: [],
          validFileType: ['image/jpeg', 'image/png', 'image/gif', 'image/bmp', 'application/pdf'],
          validFileSize: 1024,
          isEdit: true,
          titile: '',
          width: '',
          height: '',
          accept: '.jpg,.jpeg,.png,.gif,.bmp,.pdf'
        }
      }
    }
  },
  setup(props, context) {
    const headersObj = { authorization: store.getters['user/token'] }
    const baseURL: any = import.meta.env.VITE_BASE_URL
    const uploadUrl = baseURL + props.uploadParams.uploadApi
    const imageUrlList = ref<any[]>(props.uploadParams.imgUrlList || [])

    const dialogImageUrl = ref('')
    const dialogVisible = ref(false)
    const previewType = ref('') // 'image', 'pdf', 'other'

    // 监听props变化，更新文件列表
    watch(() => props.uploadParams.imgUrlList, (newVal) => {
      imageUrlList.value = newVal || [];
    }, { deep: true });

    // 判断是否为图片文件
    const isImageFile = (file: any) => {
      const url = file.url || '';
      const name = file.name || '';
      const type = file.type || '';
      
      if (type.includes('image')) return true;
      
      const imageExtensions = ['.jpg', '.jpeg', '.png', '.gif', '.bmp', '.webp'];
      return imageExtensions.some(ext => 
        url.toLowerCase().includes(ext) || name.toLowerCase().endsWith(ext)
      );
    }

    // 获取文件显示名称
    const getFileName = (fileName: string) => {
      if (!fileName) return '文件';
      if (fileName.length > 10) {
        return fileName.substring(0, 8) + '...' + fileName.split('.').pop();
      }
      return fileName;
    }

    // 文件上传前的验证
    const beforeUpload = (file: File) => {
      // // 文件大小验证
      // const isLtSize = file.size / 1024 <= props.uploadParams.validFileSize;
      // if (!isLtSize) {
      //   msg.warningAuto(`文件大小不能超过 ${props.uploadParams.validFileSize}KB`);
      //   return false;
      // }

      // 文件类型验证
      //const isValidType = props.uploadParams.validFileType.includes(file.type);
      // if (!isValidType) {
      //   const allowedTypes = props.uploadParams.validFileType.join(', ');
      //   msg.warningAuto(`只支持 ${allowedTypes} 格式的文件`);
      //   return false;
      // }

      return true;
    }

    // 删除文件 - 修复版本
    const handleRemove = (uploadFile: any, uploadFiles: any) => {
      imageUrlList.value = uploadFiles;
      context.emit("handleImgChanged", imageUrlList.value);
    }

    // 手动删除文件
    const handleRemoveFile = (file: any) => {
      const index = imageUrlList.value.findIndex(item => item.uid === file.uid || item.url === file.url);
      if (index > -1) {
        imageUrlList.value.splice(index, 1);
        context.emit("handleImgChanged", imageUrlList.value);
      }
    }

    // 点击预览 - 修复版本
    const handlePictureCardPreview = (uploadFile: any) => {
      dialogImageUrl.value = uploadFile.url || uploadFile.response?.data || '';
      
      // 判断文件类型
      const url = dialogImageUrl.value.toLowerCase();
      if (url.includes('.pdf') || (uploadFile.raw && uploadFile.raw.type === 'application/pdf')) {
        previewType.value = 'pdf';
      } else if (url.match(/\.(jpg|jpeg|png|gif|bmp|webp)$/) || 
                (uploadFile.raw && uploadFile.raw.type.includes('image'))) {
        previewType.value = 'image';
      } else {
        previewType.value = 'other';
      }
      
      dialogVisible.value = true;
    }

    // 下载文件
    const downloadFile = (fileUrl: string) => {
      const link = document.createElement('a');
      link.href = fileUrl;
      link.target = '_blank';
      link.download = fileUrl.split('/').pop() || 'download';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }

    // 选择文件，上传成功时将后端url添加进文件列表 - 修复版本
    const handleChange = (file: any, fileList: UploadFile[]) => {
      // 数量限制验证
      if (fileList.length > props.uploadParams.limit) {
        msg.warningAuto(`已超出允许上传的数量，最多 ${props.uploadParams.limit} 个文件`);
        // 这里让el-upload自己处理数量限制，我们只做提示
        return false;
      }

      // 上传成功处理
      if (file.status === 'success' && file.response) {
        if (file.response.status === "Success") {
          // 检查是否已存在相同文件
          const exists = imageUrlList.value.some(item => 
            item.url === file.response.data || item.uid === file.uid
          );
          if (!exists) {
            // 确保文件对象有必要的属性
            const newFile = {
              name: file.name,
              url: file.response.data,
              uid: file.uid,
              status: 'success'
            };
            
            // 更新文件列表
            imageUrlList.value = fileList.map(f => ({
              name: f.name,
              url: f.response?.data || f.url,
              uid: f.uid,
              status: f.status
            }));
            
            context.emit("handleImgChanged", imageUrlList.value);
          }
        } else {
          msg.errorAuto(file.response.message || "上传失败，未知原因");
        }
      }
      
      // 如果是上传失败，移除失败的文件
      if (file.status === 'fail') {
        const index = fileList.findIndex(f => f.uid === file.uid);
        if (index > -1) {
          fileList.splice(index, 1);
        }
      }
    }

    return {
      uploadUrl,
      headersObj,
      imageUrlList,
      dialogImageUrl,
      dialogVisible,
      previewType,
      handleRemove,
      handleRemoveFile,
      handlePictureCardPreview,
      handleChange,
      beforeUpload,
      isImageFile,
      getFileName,
      downloadFile
    }
  }
})
</script>

<style lang="scss" scoped>
.muilt-uploader {
  .custom-file-item {
    width: 100%;
    height: 100%;
    position: relative;
    
    .el-upload-list__item-thumbnail {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }
    
    .file-pdf {
      width: 100%;
      height: 100%;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      background: #f5f7fa;
      
      .file-name {
        margin-top: 5px;
        font-size: 12px;
        color: #606266;
        text-align: center;
        padding: 0 5px;
        word-break: break-all;
      }
    }
    
    .el-upload-list__item-actions {
      position: absolute;
      width: 100%;
      height: 100%;
      left: 0;
      top: 0;
      cursor: default;
      text-align: center;
      color: #fff;
      opacity: 0;
      font-size: 20px;
      background-color: rgba(0, 0, 0, 0.5);
      transition: opacity 0.3s;
      display: flex;
      align-items: center;
      justify-content: center;
      
      &:hover {
        opacity: 1;
      }
      
      .el-upload-list__item-preview,
      .el-upload-list__item-delete {
        cursor: pointer;
        margin: 0 8px;
        
        &:hover {
          color: #409eff;
        }
      }
    }
  }
  
  .unsupported-file {
    text-align: center;
    padding: 40px 0;
    
    p {
      margin: 16px 0;
      color: #606266;
    }
  }
}

// 响应式样式
@media screen and (max-width: 450px) {
  .muilt-uploader {
    :deep(.el-upload-list--picture-card .el-upload-list__item) {
      height: 70px !important;
      width: 70px !important;
      float: left !important;
    }

    :deep(.el-upload--picture-card) {
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
    :deep(.el-upload-list--picture-card .el-upload-list__item) {
      height: 90px !important;
      width: 90px !important;
      float: left !important;
    }

    :deep(.el-upload--picture-card) {
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