<template>
    <Layer :layer="layer" @confirm="onSubmit" ref="layerDom" >    
       <el-form :model="dataForm" :rules="dataRules" ref="dataFormRef"  label-width="auto" label-position="left" style="padding:0 15px">
        <el-form-item label="AGV类型" prop="agvType">
            <el-select v-model="dataForm.agvType" style="width: 100%;" placeholder="请选择AGV类型 *必填">
                      <el-option
                      v-for="item in agvTypeOptions" 
                      :label="item.value"
                      :value="item.key">
                      </el-option>
                  </el-select>
        </el-form-item>  
        <el-form-item label="AGV编码" prop="agvNo">
            <el-input v-model="dataForm.agvNo" placeholder="请填写AGV编码 *必填"/>
        </el-form-item> 
        <el-form-item label="执行任务模板" prop="taskType">
            <el-input v-model="dataForm.taskType" placeholder="请填写执行任务模板 *必填"/>
        </el-form-item> 
        <el-form-item label="搬运容器类型" prop="ctnrType">
            <el-input v-model="dataForm.ctnrType" placeholder="请填写搬运容器类型 *必填"/>
        </el-form-item>  
        <el-form-item label="路径位置类型" prop="positionCodeType">
            <el-input v-model="dataForm.positionCodeType" placeholder="请填写路径位置类型 *必填"/>
        </el-form-item>  
        <el-form-item label="AGV状态" prop="status">
            <el-select v-model="dataForm.status" style="width: 100%;" placeholder="请选择AGV当前所处状态 *必填">
                <el-option
                v-for="item in agvStatusOptions" 
                :label="item.value"
                :value="item.key">
                </el-option>
            </el-select>
        </el-form-item> 
        <el-form-item label="默认配置" style="text-align: left;">
                <el-checkbox  label="是"  v-model="dataForm.isDeft"/>
                <span class="text-success" style="font-size: 11px;margin-left: 10px">同一AGV类型中只会启用一个默认配置</span>
            </el-form-item>
       </el-form> 
    </Layer>
  </template>
  
  <script lang="ts" setup> 
  import { ref,defineEmits,defineProps,onMounted } from 'vue'  
  import Layer from '@/components/layer/index.vue'
  import { ElForm } from 'element-plus'   
  import {getArgs} from "@/api/workshop-device/agv-setting"; 
  import permission from '@/utils/system/permission' 

  const props=defineProps({
    layer:{
        type: Object,
        default:()=>{
            return{
                show: false,
                title: '',
                showButton: true,
                btnLoading:false,
                width:"30%",
                type:'', 
                data:null 
            }
        }
    }
  });  
  const emit = defineEmits(['dataSubmit'])
  
  const dataForm = ref({
    agvId:props.layer.data?.agvId,
    agvNo:props.layer.data?.agvNo, 
    agvType:props.layer.data?.agvType,
    taskType:props.layer.data?.taskType,
    ctnrType:props.layer.data?.ctnrType,
    positionCodeType:props.layer.data?.positionCodeType,
    isDeft:props.layer.data?.isDeft,
    status:props.layer.data?.status,
    updateUserId:'',
    updateUserName:''
  }); 
    
  const dataRules:any ={
    agvNo: [{ required: true, message: 'AGV编码不能为空', trigger: 'blur' },{ max:20, message: '字符超出限制长度', trigger: 'blur'}],
    agvType: [{ required: true, message: '请选择AGV类型', trigger: 'change' }],
    taskType: [{ required: true, message: '执行任务模板不能为空', trigger: 'blur' },{ max:20, message: '字符超出限制长度', trigger: 'blur'}],
    ctnrType: [{ max:20, message: '字符超出限制长度', trigger: 'blur'}],  
    positionCodeType: [{ required: true, message: '路径位置类型不能为空', trigger: 'blur' },{ max:20, message: '字符超出限制长度', trigger: 'blur'}] ,
    status:[{ required: true, message: '请选择AGV当前状态', trigger: 'blur' }],
  };
    
  const dataFormRef = ref(ElForm||null); 
   
  const agvTypeOptions=ref(new Array<any>()); 

  const agvStatusOptions=ref(new Array<any>()); 
   
  onMounted(()=>{ 
    getArgs().then(res=>{ 
        agvTypeOptions.value=res.data.agvTypeOptions; 
        agvStatusOptions.value=res.data.agvStatusOptions;
    });
  })
      
  const onSubmit=()=>{  
      dataFormRef.value.validate((valid:any)=>{
          if(valid){     
            let operator=permission.getOperator();
            dataForm.value.updateUserId=operator.userId;
            dataForm.value.updateUserName=operator.userName;
              emit('dataSubmit',dataForm.value,props.layer.type)
          }
      }) 
  }
  </script>
  
  <style lang="scss" scoped>
    
  </style>