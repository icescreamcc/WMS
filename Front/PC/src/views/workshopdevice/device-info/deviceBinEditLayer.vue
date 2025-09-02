<template>
    <Layer :layer="layer" @confirm="onSubmit" ref="layerDom" >    
       <el-form :model="dataForm" :rules="dataRules" ref="dataFormRef"  label-width="auto" label-position="left" style="padding:0 15px">
        <el-form-item label="设备类型" prop="deviceType">
            <el-select v-model="dataForm.deviceType" style="width: 100%;" :disabled="isReadOnly" placeholder="请选择设备类型 *必填" @change="onSelectDeviceType">
                      <el-option
                      v-for="item in deviceTypeOptions" 
                      :label="item.value"
                      :value="item.key">
                      </el-option>
                  </el-select>
        </el-form-item>  
        <el-form-item label="设备编码" prop="deviceId">
            <el-select v-model="dataForm.deviceId" style="width: 100%;" :disabled="isReadOnly" placeholder="请选择设备 *必填" @change="onSelectDeviceNo">
                      <el-option
                      v-for="item in deviceInfoOptions" 
                      :label="item.value"
                      :value="item.key">
                      </el-option>
                  </el-select>
        </el-form-item>  
        <el-form-item label="系统仓位编码" prop="binNo">
            <el-input v-model="dataForm.binNo" placeholder="请填写系统仓位编码 *必填"/>
        </el-form-item> 
        <el-form-item label="AGV送货仓位码" prop="agvBinCode_Delivery">
            <el-input v-model="dataForm.agvBinCode_Delivery" placeholder="请填写AGV送货仓位码 *必填"/>
        </el-form-item>  
        <el-form-item label="AGV取货仓位码" prop="agvBinCode_Receive">
            <el-input v-model="dataForm.agvBinCode_Receive" placeholder="请填写AGV取货仓位码 *必填"/>
        </el-form-item> 
        <el-form-item label="仓位排序" prop="sort">
          <el-input v-model="dataForm.sort" type="number" placeholder="仓位在设备上的顺序" />
      </el-form-item>
      <el-form-item label="列" prop="column">
          <el-input v-model="dataForm.column" type="number" placeholder="仓位布局-所在列数" />
      </el-form-item>
      <el-form-item label="行" prop="row">
          <el-input v-model="dataForm.row" type="number" placeholder="仓位布局-所在行数" />
      </el-form-item>
      <el-form-item label="层" prop="tier">
          <el-input v-model="dataForm.tier" type="number" placeholder="仓位布局-所在层数" />
      </el-form-item>
       </el-form>
    </Layer>
  </template>
  
  <script lang="ts" setup> 
  import { ref,defineEmits,defineProps,onMounted } from 'vue'  
  import Layer from '@/components/layer/index.vue'
  import { ElForm } from 'element-plus'   
  import {getArgs,getDeviceInfoOptions} from "@/api/workshop-device/device-info"; 
  import{getUserByKey}from '@/api/common';

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
    binId:props.layer.data?.binId,
    binNo:props.layer.data?.binNo,
    agvBinCode_Delivery:props.layer.data?.agvBinCode_Delivery,
    agvBinCode_Receive:props.layer.data?.agvBinCode_Receive,
    sort:props.layer.data?.sort||0,
    column:props.layer.data?.column||0,
    row:props.layer.data?.row||0,
    tier:props.layer.data?.tier||0,
  }); 
 
  const dataRules:any ={
    deviceType: [{ required: true, message: '请选择设备类型', trigger: 'change' }],
    deviceId: [{ required: true, message: '请选择设备', trigger: 'change' }], 
    binNo:[{ required: true, message: '请填写系统仓位编码', trigger: 'blur' },{ max:20, message: '字符超出限制长度', trigger: 'blur'}], 
    agvBinCode_Delivery:[{ max:20, message: '字符超出限制长度', trigger: 'blur'}],
    agvBinCode_Receive:[{ max:20, message: '字符超出限制长度', trigger: 'blur'}],
    sort:[{ required: true, message: '排序不能为空', trigger: 'blur' }],
    column:[{ required: true, message: '所处列数不能为空', trigger: 'blur' }],
    row:[{ required: true, message: '所处行数不能为空', trigger: 'blur' }],
    tier:[{ required: true, message: '所处层数不能为空', trigger: 'blur' }]
  };
  
  const isReadOnly=ref(false);

  const deviceTypeOptions=ref(new Array<any>()); 

  const deviceInfoOptions=ref(new Array<any>()); 

  const dataFormRef = ref(ElForm||null);  
   
  onMounted(()=>{ 
    getArgs().then(res=>{
        deviceTypeOptions.value=res.data.deviceTypeOptions; 
    });
    onSelectDeviceType(dataForm.value.deviceType);
    isReadOnly.value=props.layer.data?true:false;
  })
    
  const onSelectDeviceType=(deviceType:string)=>{ 
    if(deviceType){
        deviceInfoOptions.value.length=0;
        getDeviceInfoOptions(deviceType).then(res=>{
            deviceInfoOptions.value=res.data; 
            if(res.data?.length>0){
                if(!res.data.find((f:any)=>f.key==dataForm.value.deviceId)){
                    dataForm.value.deviceId=deviceInfoOptions.value[0].key;
                }
            }
            else{
                dataForm.value.deviceId='';
            } 
        })
    } 
  }
  
  const onSelectDeviceNo=(val:string)=>{

  }

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