<template>
    <Layer :layer="layer" @confirm="onSubmit" ref="layerDom" >   
  
       <el-form :model="dataFrom" :rules="orderRules" ref="fromRef"  label-width="auto" label-position="left" style="padding:0 15px"> 
        <el-form-item label="规格名称" prop="specName">
            <el-input v-model="dataFrom.specName"  placeholder="请输入规格名称 *必填"/>
        </el-form-item>
        <el-form-item label="分隔数" prop="cellCount">
            <el-input v-model="dataFrom.cellCount" type="number"  placeholder="料箱分隔单元格的数量 *必填" />
        </el-form-item> 
        <el-form-item label="尺寸(cm)" prop="size">
            <el-input v-model="dataFrom.size"  placeholder="长*宽*高"/>
        </el-form-item>
        <el-form-item label="承重(kg)" prop="loadWeight">
            <el-input v-model="dataFrom.loadWeight" type="number" placeholder="最大可承载重量"/>
        </el-form-item> 
        <el-form-item label="排序" prop="rank">
                <el-input v-model="dataFrom.rank" type="number" placeholder="排序"/>
        </el-form-item>  
       </el-form> 
    </Layer>
  </template>
  
  <script lang="ts" setup> 
  import { ref,defineEmits,defineProps } from 'vue'  
  import Layer from '@/components/layer/index.vue'
  import { ElForm } from 'element-plus'   
  
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
  
  const dataFrom = ref({
    specId:props.layer.data?.specId,
    specName:props.layer.data?.specName,
    size:props.layer.data?.size,
    loadWeight:props.layer.data?.loadWeight,
    cellCount:props.layer.data?.cellCount,
    rank:props.layer.data?.rank 
  }); 
  
  const orderRules:any ={
    specName: [{ required: true, message: '请输入规格名称', trigger: 'blur' },{ max:30, message: '字符超出限制长度', trigger: 'blur'}],
    size: [{ max:30, message: '字符超出限制长度', trigger: 'blur'}], 
    cellCount: [{ required: true, message: '请输入料箱分隔数', trigger: 'blur' },{validator:(rule:any, value:any, callback:any)=>{
          if(value<=0){
              callback(new Error('料箱分隔数必须大于0'));
          }else{
              callback();
          }
      }, trigger: 'blur'}], 
      rank: [{ required: true, message: '请输入排序编号', trigger: 'blur' }] 
  };
  
  
  const fromRef = ref(ElForm||null); 
  
   
  const onSubmit=()=>{  
    fromRef.value.validate((valid:any)=>{
          if(valid){  
              props.layer.btnLoading=true;
              emit('dataSubmit',dataFrom.value,props.layer.type)
          }
      }) 
  }
  </script>
  
  <style lang="scss" scoped>
    
  </style>