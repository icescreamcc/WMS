<template>
  <Layer :layer="layer" @confirm="submit">
    <el-form :model="ruleForm" :rules="formRules" ref="formRef" label-width="auto" label-position="left"
      style="padding:0 15px">
      <el-row>
        <el-col :span="11">
          <el-form-item prop="orderNo">
            <template #label>
              <span>订单号 <span style="color: red;">*</span></span>
            </template>
            <el-input v-model="ruleForm.orderNo" disabled placeholder="系统生成 无需填写"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item prop="contractNo">
            <template #label>
              <span>合同号 <span style="color: red;">*</span></span>
            </template>
            <el-input v-model="ruleForm.contractNo" :disabled="!props.layer.showButton" placeholder="合同号" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item prop="signingDate">
            <template #label>
              <span>签约日期 <span style="color: red;"></span></span>
            </template>
            <el-date-picker v-model="ruleForm.signingDate" :disabled="!props.layer.showButton" style="width:100%"
              type="date" value-format="YYYY-MM-DD" placeholder="请选择日期"> </el-date-picker>
          </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item prop="customerName">
            <template #label>
              <span>客户名称 <span style="color: red;">*</span></span>
            </template>
            <el-select v-model="ruleForm.customerName" :disabled="!props.layer.showButton" class="m-2"
              style="width:100%;margin-left:5px" placeholder="客户名称">
              <el-option v-for="item in supplierUserData" :key="item.key" :label="item.value" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">

          <el-form-item prop="unit">
            <template #label>
              <span>单位 <span style="color: red;">*</span></span>
            </template>
            <el-select v-model="ruleForm.unit" :disabled="!props.layer.showButton" class="m-2"
              style="width:100%;margin-left:5px" placeholder="单位">
              <el-option v-for="item in unitsData" :key="item.key" :label="item.value" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>


        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item prop="orderNum">
            <template #label>
              <span>订单量 <span style="color: red;">*</span></span>
            </template>
            <el-input v-model="ruleForm.orderNum" :disabled="!props.layer.showButton" placeholder="订单量"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">

          <el-form-item prop="goodsName">
            <template #label>
              <span>物品名称 <span style="color: red;">*</span></span>
            </template>
            <el-select v-model="ruleForm.goodsName" :disabled="!props.layer.showButton" class="m-2"
              style="width:100%;margin-left:5px" placeholder="物品名称">
              <el-option v-for="item in goodsNameData" :key="item.key" :label="item.value" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>

        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item prop="orderAmount">
            <template #label>
              <span>订单金额 <span style="color: red;">*</span></span>
            </template>
            <el-input v-model="ruleForm.orderAmount" :disabled="!props.layer.showButton" placeholder="订单金额"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item prop="deliveryDate">
            <template #label>
              <span>交货截止日期 <span style="color: red;">*</span></span>
            </template>
            <el-date-picker v-model="ruleForm.deliveryDate" :disabled="!props.layer.showButton" style="width:100%"
              type="date" value-format="YYYY-MM-DD" placeholder="请选择日期"> </el-date-picker>
          </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item prop="remarks">
            <template #label>
              <span>备注 <span style="color: red;"></span></span>
            </template>
            <el-input v-model="ruleForm.remarks" :disabled="!props.layer.showButton" placeholder="备注"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>

        <el-col :span="11">
          <el-form-item prop="paymentState">
            <template #label>
              <span>回款状态 <span style="color: red;">*</span></span>
            </template>
            <el-select v-model="ruleForm.paymentState" :disabled="!props.layer.showButton" class="m-2"
              style="width:100%;margin-left:5px" placeholder="回款状态">
              <el-option v-for="item in paymentStatusOptions" :key="item.key" :label="item.value" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>

        </el-col>

        <el-col :span="11" :offset="2">
          <el-form-item prop="orderUrl">
            <template #label>
              <span>上传合同附件<span style="color: red;">*</span></span>
            </template>
            <Upload :uploadParams="uploadParams" @handleImgChanged="imgChanged" />
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
  </Layer>
</template>

<script lang="ts" setup>
import { ref, defineEmits, defineProps, onMounted, watch } from 'vue';
import Layer from '@/components/layer/index.vue';
import { ElForm } from 'element-plus';
import msg from '@/utils/system/message';
import store from '@/store'
import { getOptions } from '@/api/order/orderplan';
import commonHelper from "@/utils/system/common-helper";
import Upload from '@/components/imgUpload/muiltUpload.vue'

const props = defineProps({
  layer: {
    type: Object,
    default: () => {
      return {
        show: false,
        title: '',
        showButton: true,
        btnLoading: false,
        type: '',
        options: null,
        data: null
      }
    }
  }
});
const emit = defineEmits(['dataSubmit']);

const ruleForm = ref({
  orderNo: props.layer.row?.orderNo || '',
  contractNo: props.layer.row?.contractNo || '',
  signingDate: props.layer.row?.signingDate || '',
  customerName: props.layer.row?.customerName || '',
  goodsName: props.layer.row?.goodsName || 'S10000005',
  orderNum: props.layer.row?.orderNum || '',
  orderAmount: props.layer.row?.orderAmount || '',
  // 修改这里：默认值为8
  //unit:props.layer.row?.unit || 8,
  unit: Number(props.layer.row?.unit) || 8,
  deliveryDate: props.layer.row?.deliveryDate || '',
  remarks: props.layer.row?.remarks || '',
  orderUrl: props.layer.row?.orderUrl || '',
  paymentState: props.layer.row?.paymentState || '未回款',
});

const paymentStatusOptions = [
  { value: '已回款', key: '已回款' },
  { value: '未回款', key: '未回款' }
];

const unitsData = ref(new Array<any>());
const supplierUserData = ref(new Array<any>());
const goodsNameData = ref(new Array<any>());
const formRef = ref(ElForm || null);


// 文件类型判断函数
const getFileType = (url: string) => {
  if (!url) return 'unknown';
  const extension = url.split('.').pop()?.toLowerCase();
  if (['jpg', 'jpeg', 'png', 'gif', 'bmp'].includes(extension || '')) {
    return 'image';
  } else if (extension === 'pdf') {
    return 'pdf';
  }
  return 'unknown';
}

// 监听layer.data的变化，当编辑不同行时更新数据
watch(() => props.layer.row, (newVal) => {
  if (newVal) {
    ruleForm.value = {
      orderNo: newVal.orderNo || '',
      contractNo: newVal.contractNo || '',
      signingDate: newVal.signingDate || '',
      customerName: newVal.customerName || '',
      goodsName: newVal.goodsName || 'S10000005',
      orderNum: newVal.orderNum || '',
      orderAmount: newVal.orderAmount || '',

      //unit: newVal.unit || 8,
      unit: Number(newVal.unit) || 8,
      deliveryDate: newVal.deliveryDate || '',
      remarks: newVal.remarks || '',
      orderUrl: newVal.orderUrl || '',
      paymentState: newVal.paymentState || '未回款',
    };

    // 更新上传组件显示的文件
    uploadParams.value.imgUrlList = ruleForm.value.orderUrl ? [{
      name: '已上传文件',
      url: ruleForm.value.orderUrl
    }] : [];
  }
}, { deep: true });

let uploadParams = ref({
  uploadApi: '/OrderPlan/UploadSparePartPhoto',
  limit: 1,
  imgUrlList: ruleForm.value.orderUrl ? [{
    name: '已上传文件',
    url: ruleForm.value.orderUrl,
  }] : [],
  validFileType: ['image', 'pdf'],
  validFileSize: 1024,
  isEdit: true,
  titile: '点击上传签字图片',
  width: '80px',
  height: '80px',
  accept: '.jpg,.jpeg,.png,.gif,.bmp,.pdf',
})

// 上传图片回调
const imgChanged = (imgList: Array<any>) => {
  ruleForm.value.orderUrl = imgList.length > 0 ? imgList[0].url : '';
}



const checkExpectDate = (rule: any, value: any, callback: any) => {
  if (value) {
    var res = commonHelper.isDateBeforeToday(value);
    if (res) {
      return callback(new Error('交货截止日期不能小于当前日期'));
    }
  }
  callback();
}

const formRules = {
  signingDate: [{ required: true, message: '请选择签约日期', trigger: 'change' }],
  deliveryDate: [{ required: true, message: '请选择交货截止日期', trigger: 'change' }, { validator: checkExpectDate, trigger: 'change' }],
  customerName: [{ required: true, message: '请选择客户名称', trigger: 'change' }],
  goodsName: [{ required: true, message: '请选择物品名称', trigger: 'change' }],
  unit: [{ required: true, message: '请选择单位', trigger: 'change' }],
  remarks: [{ max: 200, message: '备注字符超出限制长度', trigger: 'blur' }],
  contractNo: [{ required: true, message: '请输入合同号', trigger: 'blur' }, { max: 100, message: '字符超出限制长度', trigger: 'blur' }],
  orderNum: [{ required: true, message: '请输入订单量', trigger: 'blur' }, { max: 100, message: '字符超出限制长度', trigger: 'blur' }],
  orderAmount: [{ required: true, message: '请输入订单金额', trigger: 'blur' }, { max: 100, message: '字符超出限制长度', trigger: 'blur' }],
  orderUrl: [{
    required: true,
    message: '请上传签字附件（支持图片和PDF）',
    trigger: 'change'
  }],
}

onMounted(() => {
  getOptions().then((res: any) => {

    supplierUserData.value = res.data.userOptions;
    goodsNameData.value = res.data.goodsNameOptions;
    unitsData.value = res.data.unitOptions;
  })
});

const submit = () => {
  formRef.value.validate((valid: any) => {
    if (valid) {
      // 准备提交数据，确保数据类型正确
      const submitData = {
        ...ruleForm.value,
        // 确保数值字段是数字类型
        orderNum: ruleForm.value.orderNum ? Number(ruleForm.value.orderNum) : 0,
        orderAmount: ruleForm.value.orderAmount ? Number(ruleForm.value.orderAmount) : 0,
      };

      // 如果是新增操作，移除orderNo字段（让后端生成）
      if (props.layer.type === 'add') {
        delete submitData.orderNo;
      }
      emit('dataSubmit', submitData, props.layer.type);
    }
  })
}
</script>

<style lang="scss" scoped>
* {
  text-align: left;
}

.box-card {
  margin-top: 10px;
}

.option-content {
  border: 1px solid rgb(230, 230, 230);
  border-radius: 3px;
  padding: 5px;

  .head {
    border-bottom: 1px solid rgb(230, 230, 230);
    margin-bottom: 5px;

    .title {
      margin: 5px 0 0 0;
    }
  }

  .item {
    margin: 3px 0;
  }
}

.popper-cls {
  color: #888;
  font-size: small;
}
</style>