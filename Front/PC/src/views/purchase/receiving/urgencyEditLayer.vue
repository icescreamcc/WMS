<template>
    <Layer :layer="layer" @confirm="submit" > 
           <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
            <el-form-item label="订单号" prop="externalOrderNo">
              <el-input v-model="ruleForm.externalOrderNo" :disabled="true"/>
              </el-form-item>
              <el-form-item label="物料编号" prop="goodsNo">
              <el-input v-model="ruleForm.goodsNo" :disabled="true"></el-input> 
              </el-form-item>
              <el-form-item label="收货数量" prop="quantity">
              <el-input v-model="ruleForm.quantity" :disabled="true"></el-input>  
              </el-form-item>
            <el-form-item label="紧急数量" prop="quantityUrgency">
              <el-input-number v-model="ruleForm.quantityUrgency" :disabled="!props.layer.showButton" style="width:100%" :min="0"  placeholder="请输入紧急数量" type="number" />  
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
            selected:null
          }
        }
      }
  });     
  const emit = defineEmits(['dataSubmit']); 
  const formRef= ref(ElForm||null);  
  const ruleForm = ref({
    goodsId:props.layer.data.goodsId,
    goodsNo:props.layer.data.goodsNo,
    externalOrderNo:props.layer.data.externalOrderNo,
    quantity:props.layer.data.quantity,
    quantityUrgency:props.layer.data.quantityUrgency||0, 
  });
  const validateQuantity = (rule: any, value: any, callback: any) => {
    if (!value) {
      callback(new Error('请输入紧急数量'));
    } else {
      if (Number(value)<=0) { 
        callback(new Error('紧急数量必须大于0'));
      }
      callback()
    }
  }
  const rules = {   
    quantityUrgency:[{ required: true, message: '请输入紧急数量', trigger: 'change' },{ validator: validateQuantity, trigger: 'blur' }]
  }    
    onMounted(()=>{
        console.log("props.layer.selected",props.layer.selected)
    })
    const submit=()=> {      
        formRef.value.validate((valid:any)=>{ 
            if(valid){   
              if(ruleForm.value.quantityUrgency==0){
                msg.warningAuto("紧急数量必须大于0");
                return;
              }
              if(ruleForm.value.quantityUrgency==props.layer.data.quantityUrgency){
                msg.warningAuto("当前修改的紧急数量未发生变化");
                return;
              }
             emit('dataSubmit', ruleForm.value); 
            }
        }) 
  }
  </script>
  
  <style lang="scss" scoped>  
  </style>