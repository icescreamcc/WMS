<template>
  <Layer :layer="layer" @confirm="submit">
    <div v-if="props.layer.data?.fromConfirmShipment" style="background-color: #f0f9eb; color: #67c23a; padding: 10px; border-radius: 4px; margin-bottom: 15px;">
      <el-icon style="margin-right: 5px;"><InfoFilled /></el-icon>
      系统已自动将单据状态设置为"已发货"
    </div>    
    <el-form :model="ruleForm" :rules="formRules" ref="formRef" label-width="auto" label-position="left"
      style="padding:0 15px">
      <el-row>
        <el-col :span="11">
          <el-form-item label="发货单号*" prop="orderNo">
            <el-input v-model="ruleForm.orderNo" disabled placeholder="系统生成 无需填写"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <!-- <el-form-item label="物料分类*" prop="goodsClassify">
            <el-select v-model="ruleForm.goodsClassify" class="m-2" :disabled="props.layer.title!='新增发货计划'"
              style="width:100%" @change="onClassifyChanged" placeholder="请选择发货物料大类 *必填">
              <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item> -->
           <el-form-item label="订单编号*" prop="customerOrderNo">
            <!-- <el-input v-model="ruleForm.customerOrderNo" :disabled="!props.layer.showButton" placeholder="请输入订单编号" /> -->
              <el-select v-model="ruleForm.customerOrderNo" class="m-2" clearable :disabled="!props.layer.showButton"
              style="width:100%" placeholder="请选择订单">
              <el-option v-for="item in orderPlanData" :key="item.key" :label="item.value" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>

      <el-row>
        <el-col :span="11">
          <el-form-item label="计划发货数量*" prop="planQuantity">
            <el-input v-model="ruleForm.planQuantity" :disabled="!props.layer.showButton" placeholder="请输入计划发货数量" />
          </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item label="计划发货日期" prop="sendingDate">
            <el-date-picker v-model="ruleForm.sendingDate" :disabled="!props.layer.showButton" style="width:100%"
              type="date" value-format="YYYY-MM-DD" placeholder="请选择计划发货日期*"> </el-date-picker>
          </el-form-item>
            <!-- <el-form-item label="实际发货数量" prop="ActualQuantity">
            <el-input v-model="ruleForm.ActualQuantity" :disabled="!props.layer.showButton" placeholder="请输入订单编号" />
          </el-form-item>-->
        </el-col> 
      </el-row>
      <el-row>
        <el-col :span="11">
           <el-form-item label="要求到货日期" prop="requestDate">
            <el-date-picker v-model="ruleForm.requestDate" :disabled="!props.layer.showButton" style="width:100%"
              type="date" value-format="YYYY-MM-DD" placeholder="请选择要求到货日期"> </el-date-picker>
          </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item label="运输供应商" prop="supplierId">
            <el-select v-model="ruleForm.supplierId" class="m-2" clearable :disabled="!props.layer.showButton"
              style="width:100%" placeholder="请选择运输供应商">
              <el-option v-for="item in supplierData" :key="item.key" :label="item.value" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item> 
        </el-col>
      </el-row>

      <el-row>
         <el-col :span="11" >
          <el-form-item label="特殊要求" prop="specialRequest">
            <el-input v-model="ruleForm.specialRequest" :disabled="!props.layer.showButton" placeholder="请输入特殊要求" />
          </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item label="单据状态" prop="status">
            <el-select v-model="ruleForm.status" class="m-2" :disabled="!props.layer.showButton" style="width:100%"
              placeholder="请选择单据状态">
              <el-option v-for="item in statusData" :key="item.key" :label="item.value" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>
      
      <!-- <el-row>
        <el-col :span="11">
          <el-form-item label="收件人信息" prop="receivingResponsableUserInfo">
            <el-input v-model="ruleForm.receivingResponsableUserInfo" :disabled="!props.layer.showButton"
              placeholder="请输入收件人信息" />
          </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
         
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item label="到货地址" prop="sendingAddress">
            <el-input v-model="ruleForm.sendingAddress" :disabled="!props.layer.showButton" placeholder="请输入到货地址" />
          </el-form-item>
        </el-col>
      </el-row> -->
      <el-row>
        <el-col :span="24">
          <el-form-item label="备注" prop="remark">
            <el-input v-model="ruleForm.remark" :disabled="!props.layer.showButton" placeholder="请输入备注" />
          </el-form-item>
        </el-col>
      </el-row>

      <!-- <el-scrollbar max-height="300px">
        <div class="option-content">
          <el-row class="head">
            <el-col :span="props.layer.showButton ? 21 : 24">
              <p class="title">发货计划明细</p>
            </el-col>
            <el-col v-if="props.layer.showButton" :span="3" style="text-align:right">
              <el-button style="margin-bottom:5px" type="success"
              :disabled="!ruleForm.goodsClassify || !props.layer.showButton || props.layer.data?.fromConfirmShipment" 
              @click="onShowGoodsDrawer">选择{{ invTitle }}</el-button>
            </el-col>
          </el-row>
          <div>
            <el-table :data="detailsData" border>
              <el-table-column prop="goodsNo" label="物料编号" align="center" min-width="140" :show-overflow-tooltip="true">
                <template #default="detail">
                  <el-input v-model="detail.row.goodsFullName" readonly :title="detail.row.goodsFullName"
                    :disabled="!props.layer.showButton">
                    <template #append>{{ detail.row.goodsNo }}</template>
                  </el-input>
                </template>
              </el-table-column>
              <el-table-column prop="customerGoodsNo" label="客户料号" align="center" min-width="70"
                :show-overflow-tooltip="true">
                <template #default="detail">
                  <el-input v-model="detail.row.customerGoodsNo" disabled>
                    {{ detail.row.customerGoodsNo }}
                  </el-input>
                </template>
              </el-table-column>
              <el-table-column prop="customerIdentificationCode" label="客户配送中心" align="center" min-width="70"
                :show-overflow-tooltip="true">
                <template #default="detail">
                  <el-input v-model="detail.row.customerIdentificationCode" disabled>
                    {{ detail.row.customerIdentificationCode }}
                  </el-input>
                </template>
              </el-table-column>
              <el-table-column prop="quantity" label="计划发货数量" align="center" min-width="60"
                :show-overflow-tooltip="true">
                <template #default="detail">
                  <el-input v-model="detail.row.quantity" placeholder="请输入计划发货数量" type="number"
                    :disabled="!props.layer.showButton" title="计划发货数量" @change="onInputQuantity(detail.row)">
                  </el-input>
                </template>
              </el-table-column>
              <el-table-column prop="palletsQuantity" label="发货托数" align="center" min-width="50"
                :show-overflow-tooltip="true">
                <template #default="detail">
                  <el-input v-model="detail.row.palletsQuantity" disabled> {{ detail.row.palletsQuantity }}
                  </el-input>
                </template>
              </el-table-column>

              <el-table-column label="删除" align="center" min-width="40" :show-overflow-tooltip="true">
                <template #default="detail">
                  <el-button class="text-danger" :disabled="!props.layer.showButton"
                    style="height: 22px;line-height: 15px;" @click="onRemoveDetail(detail.row)"><el-icon>
                      <Delete />
                    </el-icon></el-button>
                </template>
              </el-table-column>
            </el-table>
          </div>
        </div>
      </el-scrollbar> -->

      <!-- <GoodsSelectDrawer :options="goodsDrawerOptions" v-if="goodsDrawerOptions.show" @selectItem="onSelectGoods" /> -->
    </el-form>
  </Layer>
</template>

<script lang="ts" setup>
import { ref, defineEmits, defineProps, onMounted, initCustomFormatter } from 'vue';
import Layer from '@/components/layer/index.vue';
import { ElForm } from 'element-plus';
import msg from '@/utils/system/message';
// import { getOptions } from '@/api/purchase/sendingorder';
import { getOptions } from '@/api/order/shipment';
import { getUserByKey, GetGoodsById } from '@/api/common';
import commonHelper from "@/utils/system/common-helper";
import { Delete } from '@element-plus/icons-vue';
import GoodsSelectDrawer from '@/components/drawer/goodsSelector.vue';

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
const userSearchLoading = ref(false);
const isUrgentShipmentData = ref(new Array<any>());
const isSufficientStockData = ref(new Array<any>());
const statusData = ref(new Array<any>());
const invTitle = ref('');
const goodsClassifyData = ref(new Array<any>());
const supplierData = ref(new Array<any>());
const orderPlanData = ref(new Array<any>());

const formRef = ref(ElForm || null);
const ruleForm = ref({
  customerOrderNo: props.layer.data?.customerOrderNo,  // 客户订单号
  planQuantity: props.layer.data?.details[0].quantity,  // 计划发货数量
  ActualQuantity: props.layer.data?.ActualQuantity,  // 实际发货数量
  orderNo: props.layer.data?.orderNo,
  sendingDate: props.layer.data?.sendingDate == "1900-01-01T00:00:00" ? "" : props.layer.data?.sendingDate,
  requestDate: props.layer.data?.requestDate == "1900-01-01T00:00:00" ? "" : props.layer.data?.requestDate,
  remark: props.layer.data?.remark,
  specialRequest: props.layer.data?.specialRequest,
  goodsClassify: props.layer.data?.goodsClassify || props.layer.options?.goodsGroup,
  sendingResponsableUserId: props.layer.data?.sendingResponsableUserId,
  sendingResponsableUserName: props.layer.data?.sendingResponsableUserName,
  sendingResponsableUserEmail: props.layer.data?.sendingResponsableUserEmail,
  sendingResponsableUserFullName: props.layer.data?.sendingResponsableUserName ? props.layer.data?.sendingResponsableUserName + ' ' + props.layer.data?.sendingResponsableUserEmail : '',
  receivingResponsableUserInfo: props.layer.data?.receivingResponsableUserInfo,
  sendingAddress: props.layer.data?.sendingAddress,
  supplierId: props.layer.data?.supplierId,
  createUserId: props.layer.data?.createUserId,
  createUserName: props.layer.data?.createUserName,
  createDate: props.layer.data?.createDate,
  isUrgentShipment: props.layer.data?.isUrgentShipment || "N",
  isSufficientStock: props.layer.data?.isSufficientStock || "N",
  status: props.layer.data?.status || "WaitingNotification",

  details: props.layer.data?.details,  // 保存物料详细信息

  // goodsId: props.layer.data?.goodsId,
  // goodsNo: props.layer.data?.goodsNo,
  // customerGoodsNo: props.layer.data?.customerGoodsNo,
  // customerIdentificationCode: props.layer.data?.customerIdentificationCode,
  // quantity: props.layer.data?.quantity,
  // palletsQuantity: props.layer.data?.palletsQuantity,
});

const goodsClassifyDefault = ref(["SamplePiece", "FinishedProduct", "RawMaterial"]);
// const detailsData = ref(new Array<any>())

const goodsDrawerOptions = ref({
  show: false,
  title: '',
  type: '',
  mode: 'repet',
  isMultiSelect: true,
  data: new Array<any>()
});

const curSelectedGoods: any = ref();

const checkSendingDate = (rule: any, value: any, callback: any) => {
  if (value) {
    var res = commonHelper.isDateBeforeToday(value);
    if (res) {
      return callback(new Error('计划发货日期不能小于当前日期'));
    }
  }
  callback();
}

const checkRequestDate = (rule: any, value: any, callback: any) => {
  if (value) {
    var res = commonHelper.isDateBeforeToday(value);
    if (res) {
      return callback(new Error('要求到货日期不能小于当前日期'));
    }
  }
  callback();
}


const validateQuantity = (rule: any, value: any, callback: any) => {
  if (Number(value) <= 0) {
    callback(new Error('计划发货数量必须大于0'))
  }
  callback()
}

const validatePalletsQuantity = (rule: any, value: any, callback: any) => {
  if (Number(value) < 0) {
    callback(new Error('发货托数必须大于等于0'))
  }
  callback()
}

const formRules = {
  sendingDate: [{ required: false, message: '请选择计划发货日期', trigger: 'change' }, { validator: checkSendingDate, trigger: 'change' }],
  requestDate: [{ required: false, message: '请选择要求到货日期', trigger: 'change' }, { validator: checkRequestDate, trigger: 'change' }],
  goodsClassify: [{ required: false, message: '请选择物料分类', trigger: 'change' }],
  supplierId: [{ required: false, message: '请选择运输供应商', trigger: 'change' }],
  receivingResponsableUserInfo: [{ required: false, message: '请输入收件人信息', trigger: 'blur' }, { max: 200, message: '字符超出限制长度', trigger: 'blur' }],
  sendingAddress: [{ required: false, message: '请填写到货地址', trigger: 'blur' }, { max: 100, message: '字符超出限制长度', trigger: 'blur' }],
  remark: [{ max: 100, message: '字符超出限制长度', trigger: 'blur' }],
  specialRequest: [{ max: 100, message: '字符超出限制长度', trigger: 'blur' }],  // 订单号
  customerOrderNo: [{ required: true,max: 100, message: '必填项', trigger: 'blur' }],
  planQuantity: [{ required: true, message: '必填项', trigger: 'blur' }, { validator: validateQuantity, trigger: 'blur' }],
  // ActualQuantity: [{ required: false,max: 100, message: '必填项', trigger: 'blur' }],
  initCustomFormatter: [{ max: 100, message: '字符超出限制长度', trigger: 'blur' }],
  isUrgentShipment: [{ required: false, message: '请选择是否紧急发货', trigger: 'change' }],
  isSufficientStock: [{ required: false, message: '请选择是否有足够库存', trigger: 'change' }],

}

onMounted(() => {
  statusData.value = props.layer.options?.statusOptions;
  getOptions().then((res: any) => {
    isUrgentShipmentData.value = res.data.yesOrNoDataOptions;
    isSufficientStockData.value = res.data.yesOrNoDataOptions;
    if (!ruleForm.value.sendingAddress) {
      ruleForm.value.sendingAddress = res.data.sendingAddress
    }
    goodsClassifyData.value = props.layer.options?.goodsGroupData;
    if (props.layer.options?.goodsGroup) {
      invTitle.value = res.data.goodsClassifyOptions.find((f: any) => f.key == props.layer.options?.goodsGroup).value;
    }
    orderPlanData.value=res.data.orderPlanOptions;
    supplierData.value = res.data.supplierOptions;
    onClassifyChanged('FinishedProduct')  //默认选择成品
    // // 默认赋值为成品，注意成品不能修改。这里编码写死了
    // detailsData.value = [{"goodsId":"S10000005","goodsNo":"SYK200M","goodsName":"石英矿成品1","goodsClassifyGroup":"FinishedProduct","quantity":5555}]
    // if (props.layer.data) {
    //   detailsData.value = props.layer.data.details;
      

    //   detailsData.value.forEach(detail => {
    //     detail.goodsFullName = detail.goodsModel ? detail.goodsName + ' ' + detail.goodsModel : detail.goodsName;
       
    //   });
    //    if (props.layer.data.fromConfirmShipment) {
    //     ruleForm.value.status = "Shipment"; // 设置为已发货状态
    //   }
    // }
  })
});

const onClassifyChanged = (val: any) => {
  // 强制使用成品
  val = 'FinishedProduct'  
  if (val) {
    invTitle.value = goodsClassifyData.value.find(f => f.key == val).value;
    // console.log(" ###  onClassifyChanged invTitle   =   ",invTitle)
    // detailsData.value=[]; 
  }
}

// 这里是确认提交
const submit = () => {
  formRef.value.validate((valid: any) => {
    console.log("#### ruleForm.value 1  =",ruleForm.value)
    if (valid) {

      if (ruleForm.value.requestDate == "" || ruleForm.value.requestDate == null) {
        // ruleForm.value.requestDate = "1900-01-01T00:00:00";
      }
      // detailsData.value = [{"goodsId":"S10000005","goodsNo":"SYK200M","goodsName":"石英矿成品1","goodsClassifyGroup":"FinishedProduct","quantity":ruleForm.value.planQuantity  }]
      // ruleForm.value.details = detailsData.value;
      emit('dataSubmit', ruleForm.value, props.layer.data ? 'update' : 'add');  // 这是触发了提交
    }
  })
}
</script>

<style lang="scss" scoped>
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