<template>
    <Layer :layer="layer" @confirm="onSubmit" ref="layerDom" >   
  
       <el-form :model="dataForm" :rules="dataRules" ref="dataFormRef"  label-width="auto" label-position="left" style="padding:0 15px">
        <el-form-item label="仓库" >
            <el-input v-model="dataForm.warehouseName" disabled/>
        </el-form-item>
        <el-form-item label="货架">
            <el-input v-model="dataForm.shelfNo" disabled/>
        </el-form-item>
        <el-form-item label="货位">
            <el-input v-model="dataForm.binNo" disabled/>
        </el-form-item>
        <el-form-item label="料箱编号" prop="workbinNo">
            <el-input v-model="dataForm.workbinNo" :readonly="isSameBinNo"/>
        </el-form-item>
        <el-form-item label="料箱规格" prop="specName">
            <el-select v-model="dataForm.specName" style="width: 100%;" placeholder="请选择料箱规格 *必填">
                      <el-option
                      v-for="item in specOption"
                      :key="item.specId"
                      :label="item.specName"
                      :value="item.specName">
                      </el-option>
                  </el-select>
        </el-form-item> 
        <el-form-item label="备注" prop="remark">
                <el-input v-model="dataForm.remark" placeholder="备注"  />
            </el-form-item>
       </el-form> 
    </Layer>
  </template>
  
  <script lang="ts" setup> 
  import { ref,defineEmits,defineProps,onMounted } from 'vue'  
  import Layer from '@/components/layer/index.vue'
  import { ElForm } from 'element-plus'   
  import {getWorkbinSpec,getArgs} from "@/api/inv/workbin";

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
    warehouseId:props.layer.data?.warehouseId,
    warehouseNo:props.layer.data?.warehouseNo,
    warehouseName:props.layer.data?.warehouseName,
    shelfId:props.layer.data?.shelfId,
    shelfNo:props.layer.data?.shelfNo, 
    shelfName:props.layer.data?.shelfName,
    binId:props.layer.data?.binId,
    binNo:props.layer.data?.binNo,
    binName:props.layer.data?.binName,
    workbinId:props.layer.data?.workbinId,
    workbinNo:props.layer.data?.workbinNo, 
    specId:props.layer.data?.specId,  
    specName:props.layer.data?.specName,
    status:props.layer.data?.status,
    remark:props.layer.data?.remark
  }); 
  
  const dataRules:any ={
    specName: [{ required: true, message: '请选择料箱规格', trigger: 'blur' }],
    remark: [{ max:50, message: '字符超出限制长度', trigger: 'blur'}],
    workbinNo: [{ required: true, message: '请指定料箱编号', trigger: 'blur' },{ max: 30, message: '字符超出限制长度', trigger: 'blur'}],  
  };
   
  
  const dataFormRef = ref(ElForm||null); 
  
  const specOption=ref();
  
  const isSameBinNo=ref(true);
  
  onMounted(()=>{
    getWorkbinSpec().then(res=>{
        specOption.value=res.data; 
    });
    getArgs().then(res=>{
        isSameBinNo.value=res.data.isSameBinNo; 
    });
  })
   
  
  const onSubmit=()=>{  
      dataFormRef.value.validate((valid:any)=>{
          if(valid){  
            dataForm.value.specId=specOption.value.find((item:any)=>item.specName==dataForm.value.specName).specId; 
              props.layer.btnLoading=true;
              emit('dataSubmit',dataForm.value,props.layer.type)
          }
      }) 
  }
  </script>
  
  <style lang="scss" scoped>
    
  </style>