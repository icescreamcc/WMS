<template>
  <div class="full "> 
    <div class="content">
        <div class="layout-container">
          <br>
       <el-row>
        <el-col :span="12" :offset="6">
      <el-form :model="ruleForm" :rules="rules" ref="companyFormRef" label-width="auto" label-position="left" style="padding:15px"> 
      <el-form-item label="企业ID" prop="CompanyId">
        <el-input v-model="ruleForm.CompanyId" disabled ></el-input>
      </el-form-item>
      <el-form-item label="企业编码" prop="CompanyNo">
        <el-input v-model="ruleForm.CompanyNo" :disabled="!uploadParams.isEdit" placeholder="请输入企业编码 *必填"></el-input>
      </el-form-item>
      <el-form-item label="企业名称" prop="CompanyName">
        <el-input v-model="ruleForm.CompanyName" :disabled="!uploadParams.isEdit" placeholder="请输入企业名称 *必填"></el-input>
      </el-form-item> 
       <el-form-item label="企业官网" prop="Url">
        <el-input v-model="ruleForm.Url" :disabled="!uploadParams.isEdit" placeholder="请输入企业官网"></el-input>
      </el-form-item> 
       <el-form-item label="企业邮箱" prop="Email">
        <el-input v-model="ruleForm.Email" :disabled="!uploadParams.isEdit" placeholder="请输入企业邮箱"></el-input>
      </el-form-item> 
      <el-form-item label="企业电话" prop="Phone">
        <el-input v-model="ruleForm.Phone" :disabled="!uploadParams.isEdit" placeholder="请输入企业电话"></el-input>
      </el-form-item> 
     <el-form-item label="企业Logo"> 
          <Upload :uploadParams="uploadParams"  @handleSuccess="uploadSuccess"/>
      </el-form-item> 
        <el-form-item label="企业简介" prop="Remark">
        <el-input v-model="ruleForm.Remark" type="textarea" :disabled="!uploadParams.isEdit" placeholder="请输入企业简介"></el-input>
      </el-form-item> 
      <br>
      <el-form-item v-if="permission.isPermisstion('COMPANYUPDATE')">
             <el-button type="primary" style="width:60%" icon="el-icon-edit" size="small" @click="editStart" v-if="!uploadParams.isEdit">编辑</el-button>
            <el-button type="primary" style="width:60%" icon="el-icon-check" size="small" @click="editSubmit" v-if="uploadParams.isEdit" :loading="submitLoading">保存</el-button>
      </el-form-item>
        </el-form>
        </el-col>
      </el-row> 
        </div>
    </div>
  </div>
</template>

<script lang="ts">
import { ElForm } from 'element-plus' 
import { defineComponent, ref,  reactive } from 'vue'  
import{getCompanyInfo,updateCompany} from '@/api/system/organization' 
import Upload from '@/components/imgUpload/singleUpload.vue'
import permission from '@/utils/system/permission'
export default defineComponent({
 components:{
   Upload
 },
  setup() {
    //上传文件
    const uploadImage=ref();
    const uploadParams=ref({
        uploadApi:'/Organization/UploadCompanyLogo', 
        imgUrl:'', 
        validFileType:'image',
        validFileSize:1024,
        isEdit:false,
        titile:'点击上传LOGO',
         width:'120px',
         height:'120px', 
    }) 
    const uploadSuccess=(url:string)=>{
      ruleForm.Logo=url 
    } 

    //表单
    const submitLoading=ref(false)
    const rules = {
      CompanyNo: [{ required: true, message: '请输入企业编码', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      CompanyName: [{ required: true, message: '请输入企业名称名称', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}], 
      Url:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      Email:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      Phone:[{ max: 10, message: '字符超出限制长度', trigger: 'blur'}],
      Remark:[{ max: 100, message: '字符超出限制长度', trigger: 'blur'}]

    }
   let ruleForm = reactive({
      CompanyId: '',
      CompanyNo:'',
      CompanyName:'',
      Email:'',
      Phone:'',
      Url:'',
      Logo:'',
      Remark:''
    }) 
    const companyFormRef = ref(ElForm||null) 
 
    const editStart=()=>{
      uploadParams.value.isEdit=true;
    }
    const editSubmit=()=>{
      companyFormRef.value.validate((valid:any)=>{ 
        if(valid){
            submitLoading.value=true
            updateCompany(ruleForm)
            .then((res:any)=>{
              if(res.status=="Success"){
                 uploadParams.value.isEdit=false;
              }
            })
            .finally(()=>{
              submitLoading.value=false
            }) 
        }
      }) 
    }

    const getInfo=()=>{
      getCompanyInfo().then((res:any)=>{ 
        ruleForm.CompanyId=res.data.companyId;
        ruleForm.CompanyNo=res.data.companyNo;
        ruleForm.CompanyName=res.data.companyName;
        ruleForm.Email=res.data.email;
        ruleForm.Phone=res.data.phone;
        ruleForm.Url=res.data.url;
        ruleForm.Logo=res.data.logo; 
        ruleForm.Remark=res.data.remark;
        uploadParams.value.imgUrl=res.data.logo; 
      })
  }
  getInfo();
 
    return{ 
      permission,
      companyFormRef,
      submitLoading,
      ruleForm,
      rules, 
      uploadParams,
      uploadImage,
      editStart,
      editSubmit,
      getInfo, 
      uploadSuccess
    }
  }
})
</script>

<style lang="scss" scoped>
  .full {
    width: 100%;
    height: 100%;
    box-sizing: border-box;
    padding: 15px;
    display: flex;
    .left {
      width: 350px;
    }
    .content {
      flex: 1;
      width: calc(100% - 250px);
      height: 100%;
    }
  .layout-container {
    height: 100%;
    margin: 0;
    width: calc(100% - 10px); 
  }
  }

  .avatar-uploader .el-upload {
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}
.avatar-uploader .el-upload:hover {
  border-color: #409eff;
}
.el-icon.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 178px;
  height: 178px;
  text-align: center;
}
.avatar {
  width: 100px;
  height: 100px;
  display: block;
}
.upload-icon{
  height: 100px;
  width: 100px;
  padding: 5px 5px 2px 5px;
  border: 1px dotted rgb(185, 184, 184);
  i{
    font-size: 35px;
    color: #888;
    position: relative;
    top:20px
  }
  p{
    font-size: 4px;
    color: #888;
     position: relative;
    top:10px
  }
}
</style>