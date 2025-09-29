<template>
  <Layer :layer="layer" @confirm="submit" > 
         <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
   
            <el-form-item label="原因类型" prop="remarkType">
                  <el-select v-model="ruleForm.remarkType" class="m-2"   style="width:100%" placeholder="请选择原因类型 *必填">
                           <el-option v-for="item in reasonTypeOptionData" :key="item.optionKey" :label="item.optionName" :value="item.optionKey">
                              </el-option>
                        </el-select>
                </el-form-item> 
            <el-form-item label="原因描述" prop="remark">
            <el-input v-model="ruleForm.remark"  :autosize="{ minRows: 2, maxRows: 4}" placeholder="请输入原因描述" type="textarea"/>
            </el-form-item>
         </el-form> 
  </Layer>
</template>

<script lang="ts" setup>
import { onMounted,defineEmits,defineProps,ref,} from 'vue';
import { ElForm } from 'element-plus'; 
import Layer from '@/components/layer/index.vue';
import msg from "@/utils/system/message"; 

const props=defineProps({
  layer: {
      type: Object,
      default: () => {
        return {
          show: false,
          title:'',
          width:"40%",
          showButton: true,
          btnLoading:false, 
          data:null,
          selected:null,
          options:[]
        }
      }
    }
});     
const emit = defineEmits(['dataSubmit']); 
const formRef= ref(ElForm||null);  
const ruleForm = ref({
  flowId:props.layer.data.flowId,
  remarkType:props.layer.data.remarkType,
  remark:props.layer.data.remark, 
});  
const reasonTypeOptionData=ref(new Array<any>());
const rules = {   
  remarkType:[{ required: true, message: '请选择原因类型', trigger: 'change' }],
  remark:[{ max: 200, message: '字符超出限制长度', trigger: 'blur'}]
}    
  onMounted(()=>{ 
    reasonTypeOptionData.value=props.layer.options; 
  })
  const submit=()=> {      
      formRef.value.validate((valid:any)=>{ 
          if(valid){     
           emit('dataSubmit', ruleForm.value); 
          }
      }) 
}
</script>

<style lang="scss" scoped>  
</style>