<template>
    <el-dialog
      ref="dialog"
      :show-close="false"
      v-model="props.layer.show"
      :title="props.layer.title"
      :width="props.layer.width"
      :close-on-click-modal="false"> 
      <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
        <el-form-item label="原因类型" prop="remarkType">
              <el-select v-model="ruleForm.remarkType" class="m-2"   style="width:100%" placeholder="请选择原因类型 *必填" @change="onSelectType">
                            <el-option v-for="item in reasonTypeOptionData" :key="item.optionKey" :label="item.optionName" :value="item.optionKey">
                          </el-option>
                    </el-select>
            </el-form-item> 
        <el-form-item label="原因描述" prop="remark">
        <el-input v-model="ruleForm.remark" :disabled="ruleForm.remarkType==''" :autosize="{ minRows: 2, maxRows: 4}" placeholder="请输入原因描述" type="textarea"/>
        </el-form-item>
      </el-form> 
      <template #footer>
        <el-row>
          <el-col :span="12" style="text-align: center;"><el-button auto-insert-space @click="onClose" size="small">取消</el-button> </el-col>
          <el-col :span="12" style="text-align: center;">
            <el-button auto-insert-space v-if="props.layer.showButton" type="primary" @click="submit" size="small" :loading="props.layer.btnLoading">确认</el-button>
          </el-col>
        </el-row>
      </template>
    </el-dialog>
</template>

<script lang="ts" setup>
import { onMounted,defineEmits,defineProps,ref,Ref} from 'vue';
import { ElForm } from 'element-plus'; 
import Layer from '@/components/layer/index.vue';
import msg from "@/utils/system/message"; 
import {getOptions} from "@/api/inv/takestock";
import { UseDialogProps } from 'element-plus/lib/el-dialog/src/dialog'
import { spawn } from 'child_process';

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
interface SystemDialogProps extends UseDialogProps {
  handleClose: Function
}
const emit = defineEmits(['dataSubmit']); 
const formRef= ref(ElForm||null);  
const dialog: Ref<SystemDialogProps> = ref(null) as any 
const ruleForm = ref({ 
  remarkType:props.layer.data.remarkType,
  remarkTypeDesc:props.layer.data.remarkTypeDesc,
  remark:props.layer.data.remark, 
});  
const reasonTypeOptionData=ref(new Array<any>());
const rules = {   
  remarkType:[{ required: true, message: '请选择原因类型', trigger: 'change' }],
  remark:[{ max: 200, message: '字符超出限制长度', trigger: 'blur'}]
}    
  onMounted(()=>{ 
    getOptions().then(res=>{
        reasonTypeOptionData.value=res.data.reasonTypeOptions; 
      })
  })

  const onSelectType=()=>{
    if(ruleForm.value.remarkType){
      ruleForm.value.remarkTypeDesc=reasonTypeOptionData.value.find(f=>f.optionKey==ruleForm.value.remarkType).optionName;
    }
  }

  const onClose=()=> {
      dialog.value.handleClose()
    }

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