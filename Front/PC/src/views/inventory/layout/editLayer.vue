 <template>
    <Layer :layer="layer" @otherEvent="onSubmit" ref="layerDom">   
        <el-descriptions  :title="`库位：${dataForm.binNo}`"  direction="vertical" :column="2" border>
          <el-descriptions-item label="发货型号" :span="2">{{ dataForm.consignNum }}</el-descriptions-item>
            <el-descriptions-item label="生产订单号">{{ dataForm.deliverNo }}</el-descriptions-item>
            <el-descriptions-item label="小车唯一码">{{ dataForm.carSoleCode }}</el-descriptions-item>
            <el-descriptions-item label="产品型号">{{ dataForm.prodctionTypeNo }}</el-descriptions-item>
            <el-descriptions-item label="生产数量" >{{ dataForm.total?(dataForm.total+' '+dataForm.unitName):'' }}</el-descriptions-item>
            <el-descriptions-item label="计划装车数量" >{{ dataForm.planTotalByCar?(dataForm.planTotalByCar+' '+dataForm.unitName):'' }}</el-descriptions-item>
            <el-descriptions-item label="实际装车数量" >{{ dataForm.actualTotal?(dataForm.actualTotal+' '+dataForm.unitName):'' }}</el-descriptions-item>
            <el-descriptions-item label="小车配对数" >{{dataForm.matchingCount }}</el-descriptions-item>
            <el-descriptions-item label="需求车次" >{{ dataForm.countByCar }}</el-descriptions-item>
            <el-descriptions-item label="入库日期" >{{commonHelper.formatToDateTime(dataForm.createDate ) }}</el-descriptions-item>  
            <el-descriptions-item label="库位状态" >
              <span v-if="dataForm.status=='Lock'" class="bin-status bg-danger">{{dataForm.status}}</span>
              <span v-else-if="dataForm.status=='Full'" class="bin-status bg-warning">{{dataForm.status}}</span>
              <span v-else class="bin-status bg-success">{{dataForm.status}}</span>
            </el-descriptions-item>  
        </el-descriptions>
    </Layer>
  </template>
  
  <script lang="ts" setup> 
  import { ref,defineEmits,defineProps } from 'vue'  
  import Layer from '@/components/layer/index.vue'
  import commonHelper from '@/utils/system/common-helper';
 
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
  
  const dataForm = ref({
    consignNum:props.layer.data?.consignNum,
    detailNo:props.layer.data?.detailNo,
    carSoleCode:props.layer.data?.carSoleCode,
    deliverNo:props.layer.data?.deliverNo,
    prodctionTypeNo:props.layer.data?.prodctionTypeNo,
    total:props.layer.data?.total,
    matchingCount:props.layer.data?.matchingCount,
    planTotalByCar:props.layer.data?.planTotalByCar,
    actualTotal:props.layer.data?.actualTotal,
    countByCar:props.layer.data?.countByCar,
    unitName:props.layer.data?.unitName,
    binId:props.layer.data?.binId,
    binNo:props.layer.data?.binNo,
    createDate:props.layer.data?.createDate,
    createUser:props.layer.data?.createUser,
    status:props.layer.data?.status
  });  
    
  const emit = defineEmits(['onUnlock'])
  const onSubmit=()=>{
    emit('onUnlock', dataForm.value.binId,dataForm.value.binNo); 
  }
  </script>
  
  <style lang="scss" scoped>
    .bin-status{
      padding: 3px 5px;
    }
  </style>