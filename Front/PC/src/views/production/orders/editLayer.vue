<template>
  <Layer :layer="layer" @confirm="onSubmit" ref="layerDom" >   

     <el-form :model="orderFrom" :rules="orderRules" ref="orderFromRef"  label-width="auto" label-position="left" style="padding:0 15px">
      <el-form-item label="生产工单号" prop="deliverNo">
          <el-input v-model="orderFrom.deliverNo"  placeholder="请输入生产工单号 *必填"/>
      </el-form-item>
      <el-form-item label="发货型号" prop="deliverNo">
          <el-input v-model="orderFrom.consignNum"  placeholder="请输入发货型号 *必填"/>
      </el-form-item>
      <el-form-item label="产品型号" prop="prodctionTypeNo">
          <el-input v-model="orderFrom.prodctionTypeNo"  placeholder="请输入产品型号 *必填"/>
      </el-form-item>
      <el-form-item label="计划数量" prop="total">
          <el-input v-model="orderFrom.total" type="number" @input="calcCountByCar" placeholder="请指定计划生产数量 *必填"/>
      </el-form-item>
      <el-form-item label="装车数量" prop="totalByCar">
          <el-input v-model="orderFrom.totalByCar" type="number"  @input="calcCountByCar" placeholder="请指定每小车装车数量 *必填" />
      </el-form-item>
      <el-form-item label="数量单位" prop="unitName"> 
          <el-select v-model="orderFrom.unitName" style="width: 100%;" placeholder="选择数量单位">
                <el-option v-for="item in unitOption" :key="item.value" :label="item.value" :value="item.value">
                </el-option>
          </el-select>
      </el-form-item>
      <el-form-item label="小车配对数" prop="matchingCount">
              <el-input v-model="orderFrom.matchingCount" type="number" placeholder="请指定小车配对数 *必填"/>
          </el-form-item> 
      <el-form-item label="需求车次" prop="countByCar">
          <el-input v-model="orderFrom.countByCar" type="number"  placeholder="请指定该订单的需求车次 *必填"/>
      </el-form-item> 
      <el-form-item label="生产产线" prop="line"> 
          <el-select v-model="orderFrom.line" style="width: 100%;" placeholder="选择生产的产线">
                <el-option v-for="item in lineOption" :key="item.key" :label="item.value" :value="item.key">
                </el-option>
          </el-select>
      </el-form-item> 
      <el-form-item label="备注" prop="remark">
              <el-input v-model="orderFrom.remark" placeholder="备注"  />
          </el-form-item>
     </el-form>
    
  
  </Layer>
</template>

<script lang="ts" setup> 
import { ref,defineEmits,defineProps } from 'vue'  
import Layer from '@/components/layer/index.vue'
import { ElForm } from 'element-plus'  
import {getOptions} from "@/api/production/productionOrder";

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

const orderFrom = ref({
    orderId:props.layer.data?.orderId,
    deliverNo:props.layer.data?.deliverNo,
    consignNum:props.layer.data?.consignNum,
    prodctionTypeNo:props.layer.data?.prodctionTypeNo,
    productName:props.layer.data?.productName, 
    matchingCount:props.layer.data?.matchingCount,
    total:props.layer.data?.total,
    totalByCar:props.layer.data?.totalByCar,
    countByCar:props.layer.data?.countByCar,
    unitName:props.layer.data?.unitName,
    line:props.layer.data?.line, 
    createDate:props.layer.data?.createDate,  
    modifyDate:props.layer.data?.modifyDate,
    createUser:props.layer.data?.createUser,
    modifyUser:props.layer.data?.modifyUser,
    printDate:props.layer.data?.printDate, 
    remark:props.layer.data?.remark,
    status:props.layer.data?.status
}); 

const orderRules:any ={
    deliverNo: [{ required: true, message: '请输入生产订单号', trigger: 'blur' },{ max:30, message: '字符超出限制长度', trigger: 'blur'}],
    consignNum: [{ required: true, message: '请输入发货型号', trigger: 'blur' },{ max:30, message: '字符超出限制长度', trigger: 'blur'}],
    prodctionTypeNo: [{ required: true, message: '请输入产品型号', trigger: 'blur' },{ max: 30, message: '字符超出限制长度', trigger: 'blur'}], 
    matchingCount: [{ required: true, message: '请输入小车配对数', trigger: 'blur' },{validator:(rule:any, value:any, callback:any)=>{
        if(value<=0){
            callback(new Error('小车配对数必须大于0'));
        }else{
            callback();
        }
    }, trigger: 'blur'}], 
    total: [{ required: true, message: '请输入生产数量', trigger: 'blur' },{validator:(rule:any, value:any, callback:any)=>{
        if(value<=0){
            callback(new Error('生产数量必须大于0'));
        }else{
            callback();
        }
    }, trigger: 'blur' }], 
    totalByCar: [{ required: true, message: '请输入装车数量', trigger: 'blur' },{validator:(rule:any, value:any, callback:any)=>{
        if(value<=0){
            callback(new Error('装车数量必须大于0'));
        }else{
            callback();
        }
    }, trigger: 'blur' }], 
    countByCar: [{ required: true, message: '请输入需求车次', trigger: 'blur' },{validator:(rule:any, value:any, callback:any)=>{
        if(value<=0){
            callback(new Error('需求车次必须大于0'));
        }else{
            callback();
        }
    }, trigger: 'blur' }], 
    remark:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
     
};

const chkTotal=(rule:any, value:any, callback:any)=>{
    if(value<=0){
        callback(new Error('生产数量必须大于0'));
    }else{
        callback();
    }
}

const orderFromRef = ref(ElForm||null); 

const unitOption=ref();

const lineOption=ref();

getOptions().then(res=>{
    unitOption.value=res.data.unitOptions;
    lineOption.value=res.data.lineOptions.argsOptions;
});

const calcCountByCar=()=>{
    if(orderFrom.value.total>0&&orderFrom.value.totalByCar>0)
       orderFrom.value.countByCar=Math.ceil(orderFrom.value.total/orderFrom.value.totalByCar);
    else
       orderFrom.value.countByCar=0;
}

const onSubmit=()=>{  
    orderFromRef.value.validate((valid:any)=>{
        if(valid){  
            props.layer.btnLoading=true;
            emit('dataSubmit',orderFrom.value,props.layer.type)
        }
    }) 
}
</script>

<style lang="scss" scoped>
  
</style>