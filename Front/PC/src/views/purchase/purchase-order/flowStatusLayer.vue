<template>
    <Layer :layer="layer" @confirm="submit" > 
           <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
            <el-form-item label="内部订单号">
              <el-input v-model="ruleForm.orderNo" :disabled="true"/>
              </el-form-item>
              <el-form-item label="流程状态" prop="flowStatus">
                    <el-select v-model="ruleForm.flowStatus" class="m-2" :disabled="!props.layer.showButton"  style="width:100%" placeholder="请选择流程状态 *必填">
                                  <el-option v-for="item in orderFlowStatusData" :key="item.key" :label="item.value" :value="item.key">
                                </el-option>
                          </el-select>
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
    detialId:props.layer.data.detialId,
    orderNo:props.layer.data.orderNo,
    flowStatus:props.layer.data.flowStatus, 
  }); 
  const orderFlowStatusData=ref(new Array<any>());
  const rules = {   
     
  }    
    onMounted(()=>{ 
        orderFlowStatusData.value=props.layer.options;
    })
    const submit=()=> {      
        formRef.value.validate((valid:any)=>{ 
            if(valid){    
              if(ruleForm.value.flowStatus==props.layer.data.flowStatus){
                msg.deftAuto("当前流程状态未发生变化");
                return;
              }
             emit('dataSubmit', ruleForm.value); 
            }
        }) 
  }
  </script>
  
  <style lang="scss" scoped>  
  </style>