<template>
    <Layer :layer="layer" @confirm="submit" > 
      <div style="text-align: left;"> 
              <el-table border :data="props.layer.data" class="system-table system-scrollbar" :header-cell-style="{'text-align':'center'}" highlight-current-row :row-style="{height:'30px'}" :cell-style="{padding:'3px'}">
                <el-table-column prop="goodsNo" label="物料号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
                <el-table-column prop="quantity" label="计划收货数量" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
                  <template #default="props">
                      <span>{{props.row.quantity+props.row.quantityUnitName}}</span>
                    </template>
                </el-table-column> 
                <el-table-column prop="quantityActual" label="实际收货数量" align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"> 
                    <template #default="scope">
                      <el-input-number v-model="scope.row.quantityActual" :disabled="!props.layer.showButton" style="width:100%" :min="0" controls-position="right" placeholder="请输入实际收货数量" type="number" /> 
                    </template>
                </el-table-column>  
              </el-table> 
          </div>    

          <el-form  label-width="auto" label-position="left" :model="ruleForm" :rules="rules" ref="formRef" style="min-height:6rem;margin-top:1rem;">
            <el-row>
              <el-col :span="11"> 
                <el-form-item label="异常类别" style="margin-left:20px"> 
                  <el-select v-model="ruleForm.receivingAbnormalType" :disabled="!props.layer.showButton" class="m-2" placeholder="选择异常类别" style="width:100%" clearable>
                    <el-option
                      v-for="item in abnormalReceiptClassificationData"
                      :key="item.optionKey"
                      :label="item.optionName"
                      :value="item.optionKey">
                    </el-option>
                  </el-select> 
                </el-form-item> 
              </el-col>
              <el-col :span="11" >
                <el-form-item label="异常到货托数" prop="abnormalDeliveryPallet" style="margin-left:20px" >
                  <el-input-number v-model="ruleForm.abnormalDeliveryPallet" :min="0" disabled style="width: 100%" controls-position="right" placeholder="异常到货托数" 
                  type="number"/>
                </el-form-item> 
              </el-col>
            </el-row>  
            <el-row>
              <el-col :span="22" >
                <el-form-item label="异常详细描述" prop="receivingAbnormalDesc" style="margin-left:20px" >
                  <el-input type="textarea" :autosize="{ minRows: 2, maxRows: 4}" maxlength="200" :disabled="!props.layer.showButton" show-word-limit  placeholder="其他异常描述" 
                  v-model="ruleForm.receivingAbnormalDesc"></el-input>
                </el-form-item> 
              </el-col>
            </el-row>
          </el-form>

          <UploadImageModal :layer="uploadImageLayer" v-if="uploadImageLayer.show"  />
    </Layer>
  </template>
  
  <script lang="ts" setup>
  import { onMounted,defineEmits,defineProps,ref, reactive} from 'vue';
  import { ElForm } from 'element-plus'; 
  import Layer from '@/components/layer/index.vue' 
  import permission from '@/utils/system/permission';
  import msg from "@/utils/system/message";
  import {getOptions} from '@/api/baseinfo/rawMaterial'
  import { LayerInterface } from "@/components/layer/index.vue";
  import UploadImageModal from "@/components/layer/uploadImageLayer.vue";
import { TRUE } from 'sass';

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
  const ruleForm = ref( {
    receivingAbnormalType:props.layer.data[0]?.receivingAbnormalType,
    detailId :props.layer.data[0].detialId||'',
    receivingAbnormalDesc:props.layer.data[0].receivingAbnormalDesc,
    abnormalDeliveryPallet:props.layer.data[0].abnormalDeliveryPallet,
  });

  const rules :any= {
    // receivingAbnormalType: [{ required: true, message: '请选择一个异常类型', trigger: 'blur' }],
  };

  const uploadImageLayer: LayerInterface = reactive({
    show: true,
    title: "上传收货异常图片",
    showButton: true,
    btnLoading: false,
    width: "55%",
    apiurl:"/ReceivingOrder/UploadReceivingDocument",
    //deleteapiurl: "/ReceivingOrder/DelSendingFiles",
   // deletepermission: "RECEIVINGDELSENDINGFILES",
    deleteapiurl: "/SendingOrder/DelSendingFiles",
    deletepermission: "DELSENDINGFILES",
    data: props.layer.data[0],
    otherButton: {}
  });

  const abnormalReceiptClassificationData=ref(new Array<any>());

  const emit = defineEmits(['dataSubmit']);   
  onMounted(()=>{ 
    getOptionData();
    props.layer.data.forEach((f:any) => {
      f.quantityActual=props.layer.data[0].quantityActual==0?f.quantity:props.layer.data[0].quantityActual;
    });
    
  })

const getOptionData=()=>{
  getOptions().then(res=>{
    abnormalReceiptClassificationData.value=res.data.abnormalReceiptClassificationOptions?.argsOptions;
  })
} 

  const submit=()=> {     
    for(var i=0;i<props.layer.data.length;i++){
      var curItem=props.layer.data[i];
      if(!curItem.quantityActual){
         msg.warningAuto("请输入实际收货数量");
         return;
      }
      else if(Number(curItem.quantityActual)<=0){
        msg.warningAuto("实际收货数量必须大于0");
        return;
      }
      if(Number(curItem.quantityActual)!=Number(curItem.quantity) && (ruleForm.value.receivingAbnormalType=="" || ruleForm.value.receivingAbnormalType==null)){
        msg.warningAuto("实际收货数量不等于计划收货数量,请选择异常类别!");
        return;
      }
      curItem.receivingOperatorId=permission.getOperator().userId;
      curItem.receivingOperatorName=permission.getOperator().userName;

      curItem.receivingAbnormalType=ruleForm.value.receivingAbnormalType;
      curItem.receivingAbnormalDesc=ruleForm.value.receivingAbnormalDesc;
    } 
    emit('dataSubmit', props.layer.data); 
  }
  </script>
  
  <style lang="scss" scoped>  
    .system-table-box {
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: flex-start;
      height: 100%;
      .system-table {
        flex: 1;
        height: 100%;
      }
      
      .system-page {
        margin-top: 20px;
      }
    }


    ::v-deep {
      .el-upload{ 
       width: 80%;
       .el-upload-dragger{
        width: 100%; 
       }
      }

  .el-dialog {
    height: 85%;
  }

  .el-dialog__body {
    height: 85%;
    overflow: auto;
    padding-bottom: inherit;
  }

  .layout-container {
    height: 60%;
  }

  .el-dialog__footer {
    margin-top: -2rem
  }
}
  </style>