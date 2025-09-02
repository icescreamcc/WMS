<template>
    <Layer :layer="layer" @confirm="onSubmit" ref="layerDom" >    
       <el-form :model="dataForm" :rules="dataRules" ref="dataFormRef"  label-width="auto" label-position="left" style="padding:0 15px">
        <el-form-item label="设备类型" prop="deviceType">
            <el-select v-model="dataForm.deviceType" style="width: 100%;" placeholder="请选择设备类型 *必填">
                      <el-option
                      v-for="item in deviceTypeOptions" 
                      :label="item.value"
                      :value="item.key">
                      </el-option>
                  </el-select>
        </el-form-item> 
        <el-form-item label="所属产线" prop="warehouseId">
            <el-select v-model="dataForm.warehouseId" style="width: 100%;" placeholder="请选择所属产线">
                      <el-option
                      v-for="item in warehouseOptions" 
                      :label="item.warehouseName"
                      :value="item.warehouseId">
                      </el-option>
                  </el-select>
        </el-form-item> 
        <el-form-item label="设备编码" prop="deviceNo">
            <el-input v-model="dataForm.deviceNo" placeholder="请填写设备编码 *必填"/>
        </el-form-item> 
        <el-form-item label="设备名称" prop="deviceName">
            <el-input v-model="dataForm.deviceName" placeholder="请填写设备名称 *必填"/>
        </el-form-item> 
        <el-form-item label="连接地址" prop="connectAddress">
            <el-input v-model="dataForm.connectAddress" placeholder="格式：IP:Port"/>
        </el-form-item>  
        <el-form-item label="是否启用" style="text-align: left;">
                <el-checkbox  label="是"  v-model="dataForm.isActive"/>
            </el-form-item>
       </el-form> 
    </Layer>
  </template>
  
  <script lang="ts" setup> 
  import { ref,defineEmits,defineProps,onMounted } from 'vue'  
  import Layer from '@/components/layer/index.vue'
  import { ElForm } from 'element-plus'   
  import {getArgs} from "@/api/workshop-device/device-info"; 
  import { deftClassifyGroup ,disableClassifyGroupSelect} from '@/config'; 

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
    deviceId:props.layer.data?.deviceId,
    deviceNo:props.layer.data?.deviceNo,
    deviceName:props.layer.data?.deviceName,
    deviceType:props.layer.data?.deviceType,
    connectAddress:props.layer.data?.connectAddress,
    warehouseId:props.layer.data?.warehouseId,
    isActive:props.layer.data?.deviceNo?props.layer.data?.isActive:true 
  }); 
  
  const checkIPAddress=(rule: any, value: any, callback: any)=>{ 
    if(value){ 
        const ipPortregex = /^(\d{1,3}\.){3}\d{1,3}:([1-9]\d{1,4}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$/;
        if(!ipPortregex.test(value)){
            return callback(new Error('请按正确的格式填写连接地址：ip:port')); 
        } 
    }  
    callback();
  }

  const dataRules:any ={
    deviceNo: [{ required: true, message: '设备编码不能为空', trigger: 'blur' },{ max:20, message: '字符超出限制长度', trigger: 'blur'}],
    deviceName: [{ required: true, message: '设备名称不能为空', trigger: 'blur' },{ max:20, message: '字符超出限制长度', trigger: 'blur'}],
    deviceType: [{ required: true, message: '请选择设备类型', trigger: 'change' }],
    warehouseId: [{ required: true, message: '请选择设备所属仓库', trigger: 'change' }],
    connectAddress: [{ max:20, message: '字符超出限制长度', trigger: 'blur'},{ validator: checkIPAddress, trigger: 'blur' }]  
  };
    
  const dataFormRef = ref(ElForm||null); 
   
  const warehouseOptions=ref(new Array<any>());

  const deviceTypeOptions=ref(new Array<any>()); 
   
  onMounted(()=>{ 
    getArgs().then(res=>{
      if(deftClassifyGroup){
        warehouseOptions.value=res.data.warehouseOptions.filter((f:any)=>f.warehouseType==deftClassifyGroup);  
      }
       else{
        warehouseOptions.value=res.data.warehouseOptions; 
       } 
        deviceTypeOptions.value=res.data.deviceTypeOptions; 
    });
  })
      
  const onSubmit=()=>{  
      dataFormRef.value.validate((valid:any)=>{
          if(valid){     
              emit('dataSubmit',dataForm.value,props.layer.type)
          }
      }) 
  }
  </script>
  
  <style lang="scss" scoped>
    
  </style>