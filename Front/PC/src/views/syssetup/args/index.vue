<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
           <h2>参数列表</h2>
      </div> 
           <div class="layout-container-form-search"   v-if="permission.isPermisstion('SYSARGSUPDATE')"> 
          <el-button type="primary" style="margin-right:10px" icon="el-icon-edit" @click="editStart" v-if="!isEdit">编辑</el-button>
          <el-button  icon="el-icon-check" style="margin-right:10px" type="primary" v-if="isEdit" :loading="btnLoading" @click="dataSave">保存</el-button>
         </div>
    </div>
    <div class="layout-container-table" style="margin-top:-10px">
        <el-scrollbar height="750px"> 
               <el-descriptions  title="" direction="vertical" :column="2" size="small" border>  
                    <div v-for="item in argsData"  :key="item.argsKey" >   
                      <el-descriptions-item >
                         <el-row>
                            <el-col :span="8" >
                             <span style="font-weight: 600;position: relative;top: 7px;margin-left:30%">{{item.argsKeyName}}</span>
                            </el-col>
                            <el-col :span="16">
                                <div v-if="item.argsType=='Number'" >
                                  <el-input type="number" v-model="item.argsValue" :disabled="!isEdit"/>
                                </div>
                                <div v-if="item.argsType=='Check'" >
                                  <el-checkbox  label="是" v-model="item.argsValue" :disabled="!isEdit"></el-checkbox>  
                                </div>
                                <div v-else-if="item.argsType=='Range'">
                                    <el-row>
                                      <el-col :span="11">
                                        <el-input type="number" v-model="item.argsValue" :disabled="!isEdit"/>
                                      </el-col>
                                      <el-col :span="2">
                                        <hr style="margin:13px 10px -13px 10px"/>
                                      </el-col>
                                      <el-col :span="11">
                                        <el-input type="number" v-model="item.remark" :disabled="!isEdit"/>
                                      </el-col>
                                   </el-row>
                                </div>
                                <div v-else-if="item.argsType=='Date'">
                                   <el-date-picker v-model="item.argsValue" :disabled="!isEdit" style="width:100%" type="date" value-format="YYYY-MM-DD"  placeholder="请选择日期"> </el-date-picker>
                                </div> 
                                <div v-else-if="item.argsType=='Text'">
                                    <el-input v-model="item.argsValue" :disabled="!isEdit" />
                                </div>
                                <div v-else-if="item.argsType=='SingleSelect'">
                                    <el-select v-model="item.argsValue" :disabled="!isEdit" :placeholder="'请选择'+item.argsKeyName" style="width:100%">
                                      <el-option
                                        v-for="opt in item.argsOptions"
                                        :key="opt.optionKey"
                                        :label="opt.optionName"
                                        :value="opt.optionKey">
                                      </el-option>
                                    </el-select>
                                </div>
                                <div v-else-if="item.argsType=='Upload'">
                                  <Upload v-model="item.argsValue" :uploadParams="uploadParams" :disabled="!isEdit" @handleSuccess="uploadSuccess" />
                                  <div class="el-upload__tip" style="font-weight: 600">
                                    建议上传图片尺寸不超过472*91像素，小于10M的jpg/png文件
                                  </div>
                                </div>
                                <div v-else-if="item.argsType=='ColorPicker'">
                                  <el-color-picker v-model="systemColor" show-alpha :disabled="!isEdit" :predefine="predefineColors" size="large" />
                                </div>
                              </el-col>
                            </el-row>
       
                      </el-descriptions-item> 
                    </div>  
                </el-descriptions>
       </el-scrollbar>
    </div>
  </div>
</template>

<script lang="ts">
import { useStore } from 'vuex';
import { defineComponent, ref, reactive } from "vue";  
import {getArgs,updateArgs} from '@/api/system/args' 
import permission from '@/utils/system/permission'
import Upload from '@/components/imgUpload/singleUpload.vue'

export default defineComponent({ 
  name: 'args',
  components:{
   Upload
 },
  setup() {   
    const store = useStore();
    let btnLoading = ref(false);
    let isEdit=ref(false)
    let argsData = ref(new Array<any>()); 
    let dataSource:Array<any>=[]
    let logoUrl=ref('');
    const systemColor = ref('')
    const predefineColors = ref([
      '#ff4500',
      '#ff8c00',
      '#ffd700',
      '#90ee90',
      '#00ced1',
      '#1e90ff',
      '#c71585',
      'rgba(255, 69, 0, 0.68)',
      'rgb(255, 120, 0)',
      '#f7a500',
      'hsva(120, 40, 94, 0.5)',
      'hsl(181, 100%, 37%)',
      'hsla(209, 100%, 56%, 0.73)',
      '#c7158577',
    ])

    const uploadParams=ref({
        uploadApi:'/SysArgs/UploadSystemHomePageLogo', 
        imgUrl:'', 
        validFileType:'image',
        validFileSize:10240,
        isEdit:false,
        titile:'点击上传系统主页LOGO',
         width:'472px',
         height:'91px', 
    }) 
    const uploadSuccess=(url:string)=>{
      logoUrl.value =url ;
    } 

    // 获取参数数据 
    let getArgsData = () => { 
     getArgs().then((res:any)=>{
       res.data.forEach((element:any) => {
        element.argsValue=valueConvert(element.argsValue)
       });
       dataSource=res.data.map((d:any)=>{
         return{
           argsId:d.argsId,
           argsKey:d.argsKey,
           argsValue:valueConvert(d.argsValue)
         }
       })
       argsData.value=res.data;
       uploadParams.value.imgUrl=res.data.find((item: any) => item.argsKey === "SystemHomePageLogo")?.argsValue;
       logoUrl.value=uploadParams.value.imgUrl;
       systemColor.value=res.data.find((item: any) => item.argsKey === "SystemColor")?.argsValue;
     }) 
    }
  
  let valueConvert=(val:any)=>{
    if(val=='true'){
      return true
    }
    else if(val=='false'){
      return false
    }
    else{
      return val
    }
  }

  let editStart=()=>{
    isEdit.value=true
  }

    //编辑数据提交
    let dataSave=()=>{  
      let submitData:Array<any>=[]

      argsData.value.forEach((model:any)=>{
          if(model.argsKey=="SystemHomePageLogo"){
            model.argsValue=logoUrl.value;
          }
          if(model.argsKey=="SystemColor"){
            model.argsValue=systemColor.value;
          }
          submitData.push(model)
        });

       btnLoading.value=true
      if(submitData.length>0){
        updateArgs(submitData).then(res=>isEdit.value=false).finally(()=>
        {
          btnLoading.value=false;

          try {
            const res = store.dispatch('user/loadSystemInfo');
          } catch (err) {
            console.error('加载系统参数信息失败', err);
          }
        })
      }
    }
      
    getArgsData()
    return {  
      permission,
      argsData, 
      isEdit,
      btnLoading,  
      editStart,
      getArgsData,  
      dataSave, 
      uploadParams,
      uploadSuccess,
      systemColor,
      predefineColors,
    };
  }
});
</script>

<style lang="scss" scoped>
  .layout-container {
    height: 100%;
    margin: 0 0 0 10px;
    width: calc(100% - 10px);
    h2 {
      padding: 0;
      margin: 0;
      margin-right: 20px;
      font-size: 14px;
      display: -webkit-box;
      -webkit-line-clamp: 1;
      -webkit-box-orient: vertical;
      overflow: hidden;
      height: 30px;
      line-height: 30px;
    }
  }

  :deep(.el-descriptions__content){
    width: 50%;
  }
</style>
