<template>
    <Layer :layer="layer" @confirm="submit" > 
           <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
             <div style="text-align: left;"> 
                    <el-checkbox-group v-model="ruleForm.abnormalType">
                        <el-checkbox v-for="option in typeData" :label="option.optionName"  :value="option.optionName"  style="width: 100px;margin-left: 0;margin-top: 10px;" />
                    </el-checkbox-group>
                    <br>
                    <el-input type="textarea" :autosize="{ minRows: 2, maxRows: 4}" maxlength="200" show-word-limit  placeholder="其他异常描述" v-model="ruleForm.abnormalDesc"></el-input>
             </div>    
           </el-form> 
    </Layer>
  </template>
  
  <script lang="ts" setup>
  import { onMounted,defineEmits,defineProps,ref,} from 'vue';
  import { ElForm } from 'element-plus'; 
  import Layer from '@/components/layer/index.vue' 
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
            selected:null
          }
        }
      }
  });     
  const emit = defineEmits(['dataSubmit']); 
  const formRef= ref(ElForm||null); 
  const typeData=ref(props.layer.data||[])
  const ruleForm = ref({
        abnormalType:props.layer.selected.abnormalType?props.layer.selected.abnormalType.split(','):[],
        detailId :props.layer.selected.detailId||'',
        abnormalDesc:props.layer.selected.abnormalDesc
    }) 
    const rules = {
      abnormalType: [{ required: true, message: '请选择一个异常类型', trigger: 'blur' }],
    } 
    onMounted(()=>{
        console.log("props.layer.selected",props.layer.selected)
    })
    const submit=()=> {      
        formRef.value.validate((valid:any)=>{ 
            if(valid){  
              ruleForm.value.abnormalType=ruleForm.value.abnormalType.join(',');
             emit('dataSubmit', ruleForm.value); 
            }
        }) 
  }
  </script>
  
  <style lang="scss" scoped>  
  </style>