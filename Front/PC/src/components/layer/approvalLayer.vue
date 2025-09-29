<template>
  <Layer :layer="layer" @confirm="submit" > 
         <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
           <el-row>
             <el-col>
                  <el-form-item label="审批意见" prop="opinion">
                  <el-input v-model="ruleForm.opinion" type="textarea" rows="4"/>
                </el-form-item>
             </el-col>
           </el-row>
             <el-row>
              <el-col :span="11">
                  <el-form-item label="审批结果" prop="isApprove" style="text-align:left">
                       <el-radio-group v-model="ruleForm.isApprove" @change="onSelectApprovalResult">
                         <el-radio-button :key="true" :label="true">审批通过</el-radio-button> 
                         <el-radio-button :key="false" :label="false">审批拒绝</el-radio-button>  
                      </el-radio-group> 
              </el-form-item>
              </el-col> 
            </el-row>    
         </el-form> 
  </Layer>
</template>

<script lang="ts">
import { defineComponent, onMounted,ref } from 'vue'
import Layer from '@/components/layer/index.vue'     
export default defineComponent({
  components: {
    Layer
  },
  props: {
    layer: {
      type: Object,
      default: () => {
        return {
          show: false,
          title: '',
          showButton: true ,
          data:null ,
          type:''
        }
      }
    }
  },
  setup(props, ctx) {   
      const ruleForm = ref({
        opinion:'同意',
        isApprove:true, 
    }) 
     const rules = {
       opinion: [{ max: 100, message: '字符超出限制长度', trigger: 'blur'}]
    } 
    let onSelectApprovalResult=()=>{
      if(ruleForm.value.isApprove){
        ruleForm.value.opinion="同意";
      }
      else{
        ruleForm.value.opinion="不同意";
      }
  }
    //点击确认提交
   let submit=()=> {    
       ctx.emit('dataSubmit', props.layer.data,ruleForm.value) 
    }
    return {  
      ruleForm,
      rules,
      submit,
      onSelectApprovalResult
    }
  }
})
</script>

<style lang="scss" scoped>
   .field-chk{
     width: 100px;
     height: 35px;
   
   }
     .list{
      // width: 100%;
      padding: 0 0 0 10px;
      overflow-y: auto;
      flex: 1;
      height: auto;
      width: calc(100% - 10px);
    }
</style>