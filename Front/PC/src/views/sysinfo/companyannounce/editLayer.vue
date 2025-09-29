<template>
  <Layer :layer="layer" @confirm="submit" >
     <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
        <el-form-item label="标题" prop="remark">
                   <el-input v-model="ruleForm.remark"  ></el-input>
              </el-form-item>
       <el-form-item label="发布内容" prop="content">
         <el-input v-model="ruleForm.content" type="textarea" rows="6"/>
       </el-form-item>
        <el-form-item label="发布日期" prop="menuId">
                   <el-input v-model="ruleForm.dateTime" disabled ></el-input>
              </el-form-item>
        <el-form-item label="立即发布" prop="isPublishCurrent">
                 <el-checkbox label="是" v-model="ruleForm.isPublishCurrent"/>
              </el-form-item> 
     </el-form>
    <!-- <div style="padding: 7px 15px">
       <tinymce v-model="contentStr" :height="260" :toolbar="false" :menubar="false"/> 
    </div> -->
  </Layer> 
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import tinymce from '@/components/tinymce/index.vue'
import Layer from '@/components/layer/index.vue' 
import { ElForm } from 'element-plus'

export default defineComponent({
  components: {
    Layer,
    tinymce
  },
  props: {
    layer: {
      type: Object,
      default: () => {
        return {
          show: false,
          title: '',
          showButton: true,
          btnLoading:false,
          data:null, 
        }
      }
    }
  }, 
  setup(props, ctx) {  
    const formRef = ref(ElForm||null)  
      //表单  
    let curDate=new Date();
    const ruleForm = ref({
        messageId:props.layer.data?.messageId,
        content:props.layer.data?.content,
        isPublishCurrent:props.layer.data?props.layer.data.isPublishCurrent:false,
        remark:props.layer.data?.remark,
        dateTime:props.layer.data?.dateTime?props.layer.data?.dateTime:curDate//`${curDate.getFullYear()}-${curDate.getMonth()}-${curDate.getDay()}`
    }) 
     const rules = {
       remark: [{ required: true, message: '请输入发布标题', trigger: 'blur' },{ max: 100, message: '字符超出限制长度', trigger: 'blur'}],
      content: [{ required: true, message: '请输入发布内容', trigger: 'blur' },{ max: 300, message: '字符超出限制长度', trigger: 'blur'}]
    }  
      //点击确认提交
    let  submit=()=> {    
        formRef.value.validate((valid:any)=>{ 
            if(valid){
                ctx.emit('dataSubmit', ruleForm.value,props.layer.data?'update':'add') 
            }
        }) 
    }
    return{
      formRef,
      ruleForm,
      rules, 
      submit
    }
  } 
})
</script>

<style lang="scss" scoped>
  * {
    text-align: left;
  }
  .box-card{
    margin-top: 10px;
  }
</style>