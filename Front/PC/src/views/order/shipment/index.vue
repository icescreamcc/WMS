<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
        <el-button v-if="permission.isPermisstion('SENDINGADD')" type="primary" icon="el-icon-circle-plus-outline"
          @click="handleAdd">新增</el-button>
        <el-popconfirm title='确定删除选中的数据吗？' v-if="permission.isPermisstion('SENDINGDEL')"
          @confirm="handleDel(chooseData)">
          <template #reference>
            <el-button type="danger" icon="el-icon-delete" :disabled="chooseData.length === 0">批量删除</el-button>
          </template>
        </el-popconfirm>

        <!-- <el-button icon="el-icon-download" style="margin-left:10px" @click="getImportTemplate">模板下载</el-button>
        <el-button icon="el-icon-upload" style="margin-left:10px" @click="showImportModal">导入计划</el-button> -->
      </div>
      <div class="layout-container-form-search">
        <el-select v-model="selectedGoodsGroup" ref="refSelectClassifyGroup" size="small"
          :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;"
          @change="getTableData(true)" placeholder="选择物品大类">
          <template #prefix>
            <div
              style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              分类</div>
          </template>
          <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key"></el-option>
        </el-select>

        <!-- <el-select v-model="query.isUrgentShipment" ref="refSelectClassifyGroup" size="small"
          :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;"
          @change="getTableData(true)" filterable clearable placeholder="选择是否紧急发货">
          <template #prefix>
            <div
              style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              是否紧急发货</div>
          </template>
          <el-option v-for="item in urgentShipmentData" :key="item.key" :label="item.value"
            :value="item.key"></el-option>
        </el-select> -->

          <!-- <el-select v-model="query.sendingAddress" ref="refSelectClassifyGroup" size="small"
            :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;"
            @change="getTableData(true)" filterable clearable placeholder="选择到货地址">
            <template #prefix>
              <div
                style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
                到货地址</div>
            </template>
            <el-option v-for="item in sendingAddressData" :key="item.key" :label="item.value"
              :value="item.key"></el-option>
          </el-select> -->

        <el-select v-model="query.detailStatus" ref="refSelectClassifyGroup" size="small"
          :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;"
          @change="getTableData(true)" filterable clearable placeholder="选择单据状态">
          <template #prefix>
            <div
              style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              单据状态</div>
          </template>
          <el-option v-for="item in detailStatusData" :key="item.key" :label="item.value" :value="item.key"></el-option>
        </el-select>

        <el-date-picker v-model="dateRange" type="daterange" range-separator="-" start-placeholder="最早计划发货日期"
          end-placeholder="最晚计划发货日期" size="small" value-format="YYYY-MM-DD" style="margin-left: 10px;;width:100%"
          @change="getTableData(true)"></el-date-picker>
        <el-input v-model="query.input" placeholder="请输入关键词进行检索" size="small" style="width: 80%;margin-left: 10px;"
          clearable @clear="getTableData(true)"></el-input>
        <el-button type="primary" icon="el-icon-search" class="search-btn" @click="getTableData(true)">搜索</el-button>
        <!-- <el-button icon="el-icon-download" type="info" v-if="permission.isPermisstion('SENDINGSAPEXPORT')"
          style="margin-left:10px" @click="getExportAllowField">导出计划</el-button> -->
      </div>
    </div>
    <div class="layout-container-table">
       <!-- <Table ref="table" v-model:page="page" v-loading="loading" :showSelection="true" :data="tableData"
        @getTableData="getTableData" @selection-change="handleSelectionChange" @orderChanged="handleSortChange"> -->
      <Table ref="table" v-model:page="page" v-loading="loading" :showSelection="true" :data="tableData"
        @getTableData="getTableData" @selection-change="handleSelectionChange" @orderChanged="handleSortChange"
        @expandChange="handleExpandChange"
        >
        <el-table-column prop="orderNo" label="明细" fixed type="expand" align="center" sortable :show-overflow-tooltip="true">
          <template #default="props">
            <div style="margin-bottom:10px">
              <span style="font-weight:600;">发货计划明细</span>
            </div>
            <div>
              <el-table :data="detailList.filter(x => x.orderNo == props.row.orderNo)" style="width: 100%">
                <el-table-column prop="goodsNo" label="物料编号" width="200" />
                <el-table-column prop="goodsName" label="物料名称" width="240" />
                <el-table-column prop="customerGoodsNo" label="客户料号" width="200" />
                <el-table-column prop="customerIdentificationCode" label="客户配送中心" width="200" />
                <el-table-column prop="quantity" label="计划发货数量" width="180" />
                <el-table-column prop="palletsQuantity" label="发货托数" width="160" />
              </el-table>
            </div>
          </template>
        </el-table-column>

        <el-table-column prop="orderNo" label="发货单号" fixed align="center" sortable="custom" min-width="110"
          :show-overflow-tooltip="true" />
        <el-table-column prop="customerOrderNo" label="订单号" fixed align="center" sortable="custom" min-width="110"
          :show-overflow-tooltip="true" />
        <el-table-column prop="contractNo" label="合同号" fixed align="center" sortable="custom" min-width="110"
          :show-overflow-tooltip="true" />
        <el-table-column prop="goodsClassifyName" label="物料分类" fixed align="center" sortable="custom" min-width="100"
          :show-overflow-tooltip="true" />
        <el-table-column prop="supplierName" label="运输供应商" fixed align="center" sortable="custom" min-width="180"
          :show-overflow-tooltip="true" />
        <el-table-column prop="sendingDate" label="计划发货日期" fixed align="center" sortable="custom" min-width="120"
          :show-overflow-tooltip="true">
          <template #default="props">
            <span>{{ commonHelper.formatToDate(props.row.sendingDate) }}</span>
          </template>
        </el-table-column>

        <el-table-column prop="requestDate" label="要求到货日期" align="center" sortable="custom" min-width="120"
          :show-overflow-tooltip="true">
          <template #default="props">
            <span>{{ commonHelper.formatToDate(props.row.requestDate) }}</span>
          </template>
        </el-table-column>
        <!-- <el-table-column prop="urgentShipmentDesc" label="是否紧急发货" align="center" sortable="custom" min-width="120"
          :show-overflow-tooltip="true" />
        <el-table-column prop="sufficientStockDesc" label="是否有足够库存" align="center" sortable="custom" min-width="130"
          :show-overflow-tooltip="true" /> -->
        <el-table-column prop="receivingResponsableUserInfo" label="收件人信息" align="center" sortable="custom"
          min-width="120" :show-overflow-tooltip="true" />
        <el-table-column prop="specialRequest" label="特殊要求" align="center" sortable="custom" min-width="100"
          :show-overflow-tooltip="true" />
        <el-table-column prop="status" label="状态" align="center" sortable="custom" min-width="110"
          :show-overflow-tooltip="true">
          <template #default="props">
            <span v-if="props.row.status == 'Shipment'" class="text-primary">{{ props.row.statusDesc }}</span>
            <span v-else-if="props.row.status == 'CancelShipment'" class="text-danger">{{ props.row.statusDesc
            }}</span>
            <span v-else-if="props.row.status == 'WaitingNotification'" class="text-warning">{{
              props.row.statusDesc }}</span>
            <span v-else class="text-success">{{ props.row.statusDesc }}</span>
          </template>
        </el-table-column>

        <el-table-column prop="sendingAddress" label="到货地址" align="center" sortable="custom" min-width="150"
          :show-overflow-tooltip="true" />
  
            <el-table-column  label="确认发货" v-if="permission.isPermisstion('SENDINGUPDATE')" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true">
               <template #default="props">
                <el-button v-if="props.row.status=='WaitingNotification'" circle type="success" style="height: 30px;"  @click="handleConfirmShipment(props.row)" title="点击确认发货">
                  <el-icon><ShoppingCartFull /></el-icon>
                </el-button> 
                <span v-else>{{ props.row.statusDesc}}</span>
              </template>
          </el-table-column> 
          

        <el-table-column prop="remark" label="备注" align="center" sortable="custom" min-width="120"
          :show-overflow-tooltip="true" />
        <el-table-column prop="createUserName" label="发起人" align="center" sortable="custom" min-width="120"
          :show-overflow-tooltip="true" />
        <el-table-column prop="createDate" label="发起时间" align="center" sortable="custom" min-width="120"
          :show-overflow-tooltip="true">
          <template #default="props">
            <span>{{ commonHelper.formatToDateTime(props.row.createDate) }}</span>
          </template>
        </el-table-column>

        <el-table-column :label="$t('message.common.handle')" align="left" min-width="220"
          v-if="permission.isPermisstion('SENDINGUPDATE', 'SENDINGDEL')">
          <template #default="scope">
            <el-button @click="handleRead(scope.row)">{{ $t("message.common.read") }}</el-button>
            <el-button @click="handleEdit(scope.row)"
              v-if="permission.isPermisstion('SENDINGUPDATE') && (scope.row.detailStatus != 'Shipment')">{{
                $t("message.common.update") }}</el-button>
            <el-popconfirm v-if="permission.isPermisstion('SENDINGDEL') && (scope.row.detailStatus != 'Shipment')"
              :title="$t('message.common.delTip')" @confirm="handleDel([scope.row])">
              <template #reference>
                <el-button type="danger">{{ $t("message.common.del") }}</el-button>
              </template>
            </el-popconfirm>
          </template>
        </el-table-column>

        <el-table-column label="客户标签" align="left" min-width="90"
          v-if="permission.isPermisstion('SENDINGUPLOAD', 'SENDINGNOTIFICATION')">
          <template #default="scope">
         <el-button 
          v-if="permission.isPermisstion('SENDINGUPLOAD')"  
          @click="handleUpload(scope.row)" :type="scope.row.isInBaseFiles ? 'success' : ''" :style="scope.row.isInBaseFiles ? { backgroundColor: '#67C23A', color: '#fff', borderColor: '#67C23A' } : {}">打印
        </el-button>
            <!-- <el-button v-if="permission.isPermisstion('SENDINGNOTIFICATION') && scope.row.isEmailNotification"
              style="height: 30px;" title="已发送邮件通知" @click="onSendingNotification(scope.row)" circle>
              <img src="../../../../public/icon-img/yidu1.png" height="14">
            </el-button>
            <el-button v-if="permission.isPermisstion('SENDINGNOTIFICATION') && !scope.row.isEmailNotification"
              style="height: 30px;" title="未发送邮件通知" @click="onSendingNotification(scope.row)" circle>
              <img src="../../../../public/icon-img/xiaoxi4.png" height="14">
            </el-button> -->

          </template>
        </el-table-column>
      </Table>
      <!--  提交会触发 dataSave函数 ，参数来自 orderEditlayer.submit-->
      <OrderEditModal :layer="orderLayer" @dataSubmit="dataSave" v-if="orderLayer.show" />   
      <ExportModal :layer="exportLayer" @dataSubmit="exportData" v-if="exportLayer.show" />
      <ImportModal :layer="importLayer" @dataSubmit="uploadSuccess" v-if="importLayer.show" />
      <UploadDocumentModal :layer="uploadDocumentLayer" v-if="uploadDocumentLayer.show" />
      <MailEditLayer :layer="mailEditLayer" @dataSubmit="onMailSubmit" v-if="mailEditLayer.show" />
    </div>
  </div>
</template>

<script lang="ts" setup>
defineOptions({
  name: "shipment"
})
import { onMounted, ref, reactive } from "vue";
import { Page } from "@/components/table/type";
import {
  getSending, getOrderDetail, getYesOrNoGroup, getDetailStatusGroup, getSendingAddressGroup,getOrderPrint,addSending,updateSending, delSending
  , createImportTemplate, exportSendingData, getExportFields, getBaseFiles, adviceSending
} from "@/api/order/shipment";
import { LayerInterface } from "@/components/layer/index.vue";
import Table from "@/components/table/tableServer.vue";
import OrderEditModal from "./orderEditLayer.vue";
import permission from '@/utils/system/permission';
import { getGoodsGroup } from '@/api/common';
import msg from "@/utils/system/message";
import { deftClassifyGroup, disableClassifyGroupSelect } from '@/config';
import commonHelper from "@/utils/system/common-helper";
import ExportModal from "@/components/layer/exportLayer.vue";
import ImportModal from "@/components/layer/importLayer.vue";
import UploadDocumentModal from "@/components/layer/uploadDocumentLayer.vue";
import MailEditLayer from "@/components/layer/mailLayer.vue";
import {Message,ShoppingCartFull,Checked,Promotion,View,EditPen ,Upload}  from '@element-plus/icons-vue';
import printJS from 'print-js';
import QRCode from 'qrcode';

const query = reactive({
  input: "",
  isUrgentShipment: "",
  createUserNames: "",
  sendingAddress: "",
  detailStatus: "",
});
const dateRange = ref()
const page: Page = reactive({
  index: 1,
  size: 20,
  total: 0,
  orderField: '',
  orderType: ''
});
const orderLayer: LayerInterface = reactive({
  show: false,
  title: "",
  showButton: true,
  btnLoading: false,
  width: "70%",
  data: null,
  options: null,
  otherButton: {}
});
const exportLayer: LayerInterface = reactive({
  show: false,
  title: "选择导出字段",
  showButton: true,
  btnLoading: false,
  width: "45%",
  data: null,
  otherButton: {}
});
const importLayer: LayerInterface = reactive({
  show: false,
  title: "导入文件",
  showButton: false,
  btnLoading: false,
  width: "45%",
  apiurl: "/SendingOrder/ImportSendingData",
  data: null,
  otherButton: {}
});

const uploadDocumentLayer: LayerInterface = reactive({
  show: false,
  title: "上传文件",
  showButton: false,
  btnLoading: false,
  width: "60%",
  apiurl: "/SendingOrder/UploadSendingDocument",
  deleteapiurl: "/SendingOrder/DelSendingFiles",
  deletepermission: "DELSENDINGFILES",
  data: null,
  otherButton: {}
});
const mailEditLayer: LayerInterface = reactive({
  show: false,
  title: "发货邮件通知",
  showButton: true,
  btnLoading: false,
  width: "65%",
  data: null,
  otherButton: {}
})

const loading = ref(false);
const tableData = ref([]);
const chooseData = ref([]);
const goodsGroupData = ref(new Array<any>());
const urgentShipmentData = ref(new Array<any>());
const sendingAddressData = ref(new Array<any>());
const detailStatusData = ref(new Array<any>());
const selectedGoodsGroup = ref(deftClassifyGroup || "FinishedProduct");
const refSelectClassifyGroup = ref<null | HTMLElement>(null);
const goodsClassifyDefault = ref(["SamplePiece", "FinishedProduct", "RawMaterial"]);

onMounted(() => {
//  getCreateUserNameGroupData();
  getGoodsGroupData();
  getYesOrNoGroupData();
  getDetailStatusGroupData();
  getSendingAddressData(); // 需要实现获取到货地址数据的方法
  getTableData(true);
})

//加载下拉框
const getGoodsGroupData = () => {
  getGoodsGroup().then(res => {
    
    goodsGroupData.value = res.data.filter((f: any) => goodsClassifyDefault.value.includes(f.key));
    console.log('#### goodsGroupData.value = ',goodsGroupData.value)
  })
}

const getYesOrNoGroupData = () => {
  getYesOrNoGroup().then(res => {
    urgentShipmentData.value = res.data;
  })
}


const getSendingAddressData  = () => {
    getSendingAddressGroup().then(res => {
    sendingAddressData.value = res.data;
    console.log("### sendingAddressData.value = ",sendingAddressData.value)
  })
}

const getDetailStatusGroupData = () => {
  getDetailStatusGroup().then(res => {
    detailStatusData.value = res.data;
  })
}

const getTableData = (init: Boolean) => {
  loading.value = true
  if (init) {
    page.index = 1
  }
  let dateStart = '';
  let dateEnd = '';
  if (dateRange.value?.length > 0) {
    dateStart = dateRange.value[0]
  }
  if (dateRange.value?.length > 1) {
    dateEnd = dateRange.value[1]
  }
  loading.value = false
  // console.log(" ### [getTableData]  ermission.getOperator().userId = ",permission.getOperator().userId)
  // console.log(" ### [getTableData]  page.index = ",page.index)
  // console.log(" ### [getTableData]  page.orderField= ",page.orderField)
  // console.log(" ### [getTableData]  page.orderType= ",page.orderType)
  // console.log(" ### [getTableData]  query.input= ",query.input)
  // console.log(" ### [getTableData] dateStart = ",dateStart)
  // console.log(" ### [getTableData] dateEnd = ",dateEnd)
  // console.log(" ### [getTableData] selectedGoodsGroup.value = ",selectedGoodsGroup.value)
  // console.log(" ### [getTableData] query.isUrgentShipment = ",query.isUrgentShipment)
  // console.log(" ### [getTableData] query.sendingAddress = ",query.sendingAddress)
 getSending(permission.getOperator().userId, page.size, page.index, page.orderField, page.orderType, query.input, dateStart, dateEnd, selectedGoodsGroup.value, query.isUrgentShipment,query.sendingAddress, query.detailStatus)
    .then((res) => {
      let data = res.data.rows
      console.log('###########  getTableData   res.data.rows ',res.data.rows)
      // data  = sendingAddressData.value
      data.forEach((d: any) => {
        d.loading = false
      })
      tableData.value = data
      page.total = Number(res.data.total);
    })
    .catch((error) => {
      tableData.value = [];
      page.index = 1;
      page.total = 0;
    })
    .finally(() => {
      loading.value = false;
    })
}

 const detailList = ref(new Array<any>())
//展开与收缩
const handleExpandChange = (row: any) => {
  getOrderDetail(permission.getOperator().userId, row.orderNo).then(res => {
    detailList.value = detailList.value.filter((b: any) => b.orderNo != row.orderNo)
    res.data?.forEach((b: any) => {
      detailList.value.push(b)
    });
  })
}

const handleSortChange = (orderRow: any) => {
  getTableData(true);
}

const handleSelectionChange = (val: []) => {
  chooseData.value = val;
};

const handleDel = (data: any[]) => {
  let orderNos = Array<any>();
  data.forEach(d => {
    orderNos.push(d.orderNo);
  })
  delSending(orderNos).then((res) => {
    getTableData(tableData.value.length === 1 ? true : false);
  });
}

// const onSendingNotification = (row: any) => {
//   getOrderDetail(permission.getOperator().userId, row.orderNo).then((res: any) => {
//     let t = goodsGroupData.value.find(f => f.key == selectedGoodsGroup.value).value;
//     let bodyTitle = `<div style="text-align: left;"> 
//                           <div style="font-weight: 600;font-size:15px;">${res.data.length>0?res.data[0].createUserName:""}创建了一份${t}发货计划，计划发货日期为${commonHelper.formatToDate(res.data.length>0?res.data[0].sendingDate:"")}</div>
//                           </div>`;
//     let bodyContent = '<div style="padding:3px 10px;border: 1px solid #ededed;">';
//     res.data.forEach((d: any) => {
//       let info = `<div style="padding:5px;">
//                           <div style="width:100%;padding-top:3px;">物料：${d.goodsNo}</span>
//                           <div style="width:100%;padding-top:3px;">客户料号:${d.customerGoodsNo}</span>
//                           <div style="width:100%;padding-top:3px;">客户配送中心:${d.customerIdentificationCode}</span>
//                           <div style="width:100%;padding-top:3px;">发货数量:${d.quantity}</span>
//                           <div style="width:100%;padding-top:3px;">发货托数:${d.palletsQuantity}</span>
//                           <div style="width:100%;padding-top:3px;">运输供应商:${d.supplierName}</span>
//                           <div style="width:100%;padding-top:3px;">收件人：${d.receivingResponsableUserInfo || ''}</div>
//                           <div style="width:100%;padding-top:3px;">特殊要求：${d.specialRequest || ''}</div>
//                           <div style="width:100%;padding-top:3px;">到货地址：${d.sendingAddress || ''}</div>
//                           <div style="width:100%;padding-top:3px;">备注：${d.remark || ''}</div>
//                         </div>`;
//       bodyContent += info;
//     });
//     bodyContent += `</div>`;
//     let content = `<div>${bodyTitle}${bodyContent}</div>`;

//     getBaseFiles(permission.getOperator().userId, page.size, page.index, page.orderField, page.orderType, query.input, row.orderNo, "SendingOrderAttachment")
//       .then((res) => {
//         let data = res.data.rows;
//         mailEditLayer.data = {
//           subject: t + '发货通知',
//           body: content,
//           bodyType: 'html',
//           toReciver: row.sendingSupplierUserEmail.split(","),
//           toCC: [],
//           orderNo: row.orderNo,
//           attachments: data
//         }
//         mailEditLayer.show = true;
//       })
//       .catch((error) => {
//       })
//       .finally(() => {
//       })
//   });
// }

const onMailSubmit = (data: any) => {
  mailEditLayer.btnLoading = true;
  adviceSending(data.orderNo, data)
    .then(() => {
      msg.successAuto("已发送邮件通知到运输供应商联系人")
      mailEditLayer.show = false;
      getTableData(false);
    })
    .finally(() => mailEditLayer.btnLoading = false);
}

const handleAdd = () => {
  if (!selectedGoodsGroup.value) {
    console.log(" ##### selectedGoodsGroup  =  ",selectedGoodsGroup.value)
    msg.deftAuto("请先选择物料分类");
    refSelectClassifyGroup.value?.focus();
    return;
  }
  orderLayer.title = "新增发货计划";
  orderLayer.show = true;
  orderLayer.showButton = true;
  orderLayer.options = {
    goodsGroup: selectedGoodsGroup.value,
    isDisabledGroup: disableClassifyGroupSelect,
    statusOptions: detailStatusData.value,
    goodsGroupData: goodsGroupData.value,
  }
  delete orderLayer.data;
}
const handleConfirmShipment = (row: any) => {
  getOrderDetail(permission.getOperator().userId, row.orderNo).then((res: any) => {
    orderLayer.title = "发货";
    orderLayer.show = true;
    orderLayer.showButton = true;
    orderLayer.options = {
      goodsGroup: selectedGoodsGroup.value,
      isDisabledGroup: true,
      statusOptions: detailStatusData.value,
      goodsGroupData: goodsGroupData.value,
    }
    
    // 设置行数据和详情
    row.details = res.data;
    row.status='WaitingShipment';
    orderLayer.data = row;
    
    // 添加标记，表示这是从确认发货按钮进入的
    if (orderLayer.data) {
      orderLayer.data.fromConfirmShipment = true;
    }
  });
}
const handleEdit = (row: any) => {
  getOrderDetail(permission.getOperator().userId, row.orderNo).then((res: any) => {
    orderLayer.title = "编辑发货计划";
    orderLayer.show = true;
    orderLayer.showButton = true;
    orderLayer.options = {
      goodsGroup: selectedGoodsGroup.value,
      isDisabledGroup: true,
      statusOptions: detailStatusData.value,
      goodsGroupData: goodsGroupData.value,
    }
    row.details = res.data
    orderLayer.data = row
     // 确保不设置 fromConfirmShipment 标记
    if (orderLayer.data) {
      delete orderLayer.data.fromConfirmShipment;
    }
  })
}

const handleRead = (row: any) => {
  getOrderDetail(permission.getOperator().userId, row.orderNo).then((res: any) => {
    orderLayer.title = "查看发货计划";
    orderLayer.show = true;
    orderLayer.showButton = false;
    orderLayer.options = {
      goodsGroup: selectedGoodsGroup.value,
      isDisabledGroup: true,
      statusOptions: detailStatusData.value,
      goodsGroupData: goodsGroupData.value,
    }
    row.details = res.data
    orderLayer.data = row
  })
}

const handleUpload = async (row: any) => {
  // 获取当前行的明细数据
  getOrderDetail(permission.getOperator().userId, row.orderNo).then((res: any) => {
    const details = res.data || [];
     getOrderPrint(row.orderNo, row.customerOrderNo)
     .then(async(res: any) => {
     let data = res.data[0];

     // 生成二维码
      let qrCodeDataURL = '';
      try {
        // 二维码内容可以是发货单号，也可以包含更多信息
        const qrContent = `${row.orderNo}`;
        qrCodeDataURL = await QRCode.toDataURL(qrContent, {
          width: 80,
          height: 80,
          margin: 1
        });
      } catch (error) {
        console.error('生成二维码失败:', error);
      }

      //创建打印容器并插入到页面
      const printContainer = document.createElement('div');
      printContainer.id = 'print-content-' + Date.now(); // 唯一ID
      printContainer.innerHTML = generatePrintContent(data, details,qrCodeDataURL, row.orderNo);
      //隐藏容器并添加到页面
      printContainer.style.position = 'fixed';
      printContainer.style.left = '-9999px';
      printContainer.style.top = '-9999px';
      document.body.appendChild(printContainer);
      //打印该容器
     printJS({
      printable: printContainer.id,
      type: 'html',
      style: `
        body { 
          font-family: "Microsoft YaHei", sans-serif; 
          margin: 0; 
          padding: 15px;
          font-size: 12px; 
          -webkit-print-color-adjust: exact;
          position: relative;
        }
        .print-container {
          width: 100%;
          position: relative;
        }
        .print-section-title { 
          font-weight: bold; 
          margin-bottom: 15px; 
          border-bottom: 2px solid #000; 
          padding-bottom: 8px;
          text-align: center;
          font-size: 18px;
        }
        .qr-label {
          width: 150px !important;
          font-weight: bold;
          background-color: #f5f5f5;
        }
        .qr-image {
          width: 150px !important;
        }
        .qr-code-img {
          display: inline-block;
          text-align: center;
        }
        .qr-code-img img {
          max-width: 100%;
          height: auto;
        }
        /* 固定宽度表格样式 */
        .print-table.fixed-width {
          width: 100%;
          table-layout: fixed;
          border-collapse: collapse;
          margin: 8px 0;
          font-size: 11px;
        }
        .print-table.fixed-width td {
          border: 1px solid #ddd;
          padding: 6px;
          vertical-align: top;
          word-wrap: break-word;
          overflow-wrap: break-word;
        }
        .fixed-label {
          width: 150px !important;
          font-weight: bold;
          background-color: #f5f5f5;
          text-align: right;
        }
        .fixed-value {
          width: 150px !important;
          text-align: left;
        }
        /* 签收信息表格样式 */
        .print-table:not(.fixed-width):not(.qr-table) {
          width: 100%;
          table-layout: fixed;
          border-collapse: collapse;
          margin: 8px 0;
          font-size: 11px;
        }
        .print-table:not(.fixed-width):not(.qr-table) td {
          border: 1px solid #ddd;
          padding: 6px;
          vertical-align: top;
          word-wrap: break-word;
          overflow-wrap: break-word;
        }
        .info-label {
          width: 150px !important;
          font-weight: bold;
          background-color: #f5f5f5;
          vertical-align: top;
        }
        .info-content {
          line-height: 1.6;
          vertical-align: top;
        }
        @page {
          size: A4;
          margin: 0.5cm;
        }
        @media print {
          body { 
            margin: 0; 
            padding: 10px;
            font-size: 11px;
          }
          .print-container {
            width: 100%;
            height: 100%;
          }
          .fixed-label {
            width: 140px !important;
          }
          .fixed-value {
            width: 140px !important;
          }
          .info-label {
            width: 140px !important;
          }
          .qr-label {
            width: 140px !important;
          }
          .qr-image {
            width: 140px !important;
          }
        }
      `,
        onPrintDialogClose: () => {
          if (document.body.contains(printContainer)) {
            document.body.removeChild(printContainer);
          }
          row.printLoading = false;
        },
        onError: (error) => {
          if (document.body.contains(printContainer)) {
            document.body.removeChild(printContainer);
          }
          row.printLoading = false;
          console.error('打印错误:', error);
          msg.errorAuto('打印失败');
        }
      });
    })
    .catch(error => {
      row.printLoading = false;
      msg.errorAuto('获取打印数据失败');
      console.error('获取数据错误:', error);
    });
    })          
}

// 生成打印内容的函数 - 修改后的版本
const generatePrintContent = (data: any, details: any[], qrCodeDataURL: string, orderNo: string) => {
  const qrCodeHTML = qrCodeDataURL 
    ? `<div class="qr-code-img"><img src="${qrCodeDataURL}" alt="二维码" style="width:80px;height:80px;" /></div>`
    : '<div class="qr-code-img" style="width:80px;height:80px;border:1px solid #ccc;text-align:center;line-height:80px;font-size:10px;color:#999;">二维码生成失败</div>';
  
  return `
    <div class="print-container">
      <div class="print-section">
        <div class="print-section-title">发货计划单</div>
        <style>
        .print-table{
          width:700px;
          border-collapse:collapse;
          margin-bottom:20px;
          table-layout:fixed;
        }
          .print-table td,.print-table th{
          border:1px solid #000;
          padding:8px;
          text-align:center;
          word-break:break-word;
          }
        </style>
        <!-- 二维码表格 -->
        <table class="print-table">
          <tbody>
            <tr>
              <td class="qr-label" style="width: 300px; height: 100px; text-align: center; vertical-align: middle;">
                二维码：
              </td>
              <td class="qr-image" style="width: 300px; height: 100px; text-align: center; vertical-align: middle;">
                ${qrCodeHTML}
              </td>
            </tr>
          </tbody>
        </table>

        <!-- 基本信息表格 -->
        <table class="print-table">
          <tbody>
            <tr>
              <td class="fixed-label">订单号：</td>
              <td class="fixed-value">${data.customerOrderNo || ''}</td>
              <td class="fixed-label">发货单号：</td>
              <td class="fixed-value">${data.orderNo || ''}</td>
            </tr>
            <tr>
              <td class="fixed-label">计划发货日期：</td>
              <td class="fixed-value">${commonHelper.formatToDate(data.sendingDate) || ''}</td>
              <td class="fixed-label">目的地：</td>
              <td class="fixed-value">${data.sendingAddress || ''}</td>
            </tr>
            <tr>
              <td class="fixed-label">物品名称：</td>
              <td class="fixed-value">${data.goodsName || ''}</td>
              <td class="fixed-label">计划发货数量：</td>
              <td class="fixed-value">${data.quantity || ''}</td>
            </tr>
            <tr>
              <td class="fixed-label">单位：</td>
              <td class="fixed-value">${data.unit || ''}</td>
              <td class="fixed-label">备注：</td>
              <td class="fixed-value">${data.remarks || ''}</td>
            </tr>
            <tr>
              <td class="fixed-label">合同号：</td>
              <td class="fixed-value">${data.contractNo || ''}</td>
              <td class="fixed-label">状态：</td>
              <td class="fixed-value">${data.status || ''}</td>
            </tr>
            <tr>
              <td class="fixed-label">客户信息：</td>
              <td class="fixed-value">${data.customer || ''}</td>
              <td class="fixed-label">客户联系人：</td>
              <td class="fixed-value">${data.customerName || ''}</td>
            </tr>
            <tr>
              <td class="fixed-label">客户联系电话：</td>
              <td class="fixed-value">${data.customerTelephone || ''}</td>
              <td class="fixed-label">我方公司信息：</td>
              <td class="fixed-value">${data.ourCompany || ''}</td>
            </tr>
            <tr>
              <td class="fixed-label">我方联系人信息：</td>
              <td class="fixed-value">${data.ourCompanyName || ''}</td>
              <td class="fixed-label">我方联系电话：</td>
              <td class="fixed-value">${data.ourCompanyTelephone || ''}</td>
            </tr>
            <tr>
              <td class="fixed-label">运输公司：</td>
              <td class="fixed-value">${data.supplier || ''}</td>
              <td class="fixed-label">货船编号：</td>
              <td class="fixed-value">${data.supplierNo || ''}</td>
            </tr>
            <tr>
              <td class="fixed-label">船长姓名：</td>
              <td class="fixed-value">${data.supplierName || ''}</td>
              <td class="fixed-label"></td>
              <td class="fixed-value"></td>
            </tr>
          </tbody>
        </table>

        <!-- 签收信息表格 -->
        <table class="print-table">
          <tbody>
            <tr>
              <td class="info-label">公司出货信息</td>
              <td class="info-content">
                出库验证：<br/>
                加工方签字：____________________________________<br/>
                运输方签字：____________________________________<br/>
                公司方签字：____________________________________<br/>
                验证日期：______年______月______日<br/>
                (货物完好无损□/异常情况备注：________________)<br/>
              </td>
            </tr>
            <tr>
              <td class="info-label">附说明信息</td>
              <td class="info-content">
                1.随车附本批货物质检报告一份。<br/>
                2.请您在签收前仔细核对产品数量、规格及包装是否完好。<br/>
                3.如有任何问题，请及时与我司联系人沟通。<br/>
                4.此单据为重要结算凭证，请妥善保管。
              </td>
            </tr>
            <tr>
              <td class="info-label">客户签收信息</td>
              <td class="info-content">
                签收栏：<br/>
                收货单位(章)：_________________________________<br/>
                签收人：_______________________________________<br/>
                签收日期：______年______月______日<br/>
                (货物完好无损□/异常情况备注：________________)<br/>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  `;
}

//新增或编辑数据提交 点击确认后触发
const dataSave = (data: any, actionType: string) => {
  orderLayer.btnLoading = true;
  if (actionType == 'add') {
      data.createUserId = permission.getOperator().userId,
      data.createUserName = permission.getOperator().userName,
      addSending(data).then(res => {
        orderLayer.show = false;
        getTableData(true);
      }).finally(() => orderLayer.btnLoading = false);
  }
  else {
    data.updateUserName = permission.getOperator().userName;
    data.updateUserId = permission.getOperator().userId;
    updateSending(data).then(res => {
      orderLayer.show = false;
      getTableData(false);
    }).finally(() => orderLayer.btnLoading = false);
  }
}

// const getImportTemplate = () => {
//   createImportTemplate().then(res => {
//     let link = document.createElement('a')
//     link.style.display = 'none'
//     link.href = res.data
//     document.body.appendChild(link)
//     link.click()
//     document.body.removeChild(link)
//   })
//

const uploadSuccess = () => {
  importLayer.show = false;
  getTableData(true);
}

// const showImportModal = () => {
//   importLayer.show = true;
// }

const getExportAllowField = () => {
  getExportFields().then(res => {
    exportLayer.row = permission.getOperator().userId;
    exportLayer.show = true;
    exportLayer.data = res.data;
  })
}

const exportData = (selField: Array<any>) => {
  exportLayer.btnLoading = true;
  let dateStart = '';
  let dateEnd = '';
  if (dateRange.value?.length > 0) {
    dateStart = dateRange.value[0]
  }
  if (dateRange.value?.length > 1) {
    dateEnd = dateRange.value[1]
  }

  exportSendingData(page.orderField, page.orderType, query.input, dateStart, dateEnd, selectedGoodsGroup.value, selField).then(res => {
    let link = document.createElement('a')
    link.style.display = 'none'
    link.href = res.data
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
  }).finally(() => exportLayer.btnLoading = false)
}
</script>

<style lang="scss" scoped>
/* 打印样式优化 */
@media print {
  .layout-container,
  .layout-container-form,
  .layout-container-table {
    visibility: hidden;
  }
  
  .print-area {
    visibility: visible;
    position: absolute;
    left: 0;
    top: 0;
    width: 100%;
  }
  
  // /* 确保二维码在打印时位置正确 - 固定在右上角 */
  // .qr-code {
  //   position: absolute !important;
  //   top: 0px !important;
  //   right: 0px !important;
  //   width: 80px !important;
  //   height: 80px !important;
  //   z-index: 1000 !important;
  // }
}

/* 确保表格内容不会溢出 */
.print-table {
  table-layout: fixed;
  word-wrap: break-word;
}
.print-info-item {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  flex: 1;
  min-width: 0; /* 防止flex项目溢出 */
  padding: 2px 5px;
  font-size: 11px;
}
/* 添加新的样式确保二维码位置正确 */
.print-container {
  position: relative;
  font-family: Arial,sans-serif; 
  width: 100%;
  padding-top: 10px; /* 为二维码留出空间 */
}
/* 调整打印头部，避免与二维码重叠 */
.print-header {
  text-align: center;
  margin-bottom: 15px;
  border-bottom: 2px solid #000;
  padding-bottom: 8px;
  margin-right: 90px; /* 为二维码留出足够空间 */
}
/* 新增样式：信息行布局 */
.print-info-row {
  display: flex;
  flex-wrap: nowrap;
  justify-content: space-between;
  margin-bottom: 8px;
  width: 100%;
}
.print-label {
  font-weight: bold;
  display: inline-block;
  min-width: 80px; /* 调整标签宽度 */
}

/* 固定标签列宽 */
.fixed-label {
  width: 150px !important; /* 固定标签宽度 */
  font-weight: bold;
  background-color: #f5f5f5;
  text-align: right;
}
.fixed-value {
  width: 150px !important; /* 固定值宽度 */
  text-align: left;
}
/* 签收信息表格样式 */
.info-label {
  width: 150px !important;
  font-weight: bold;
  background-color: #f5f5f5;
}
.info-content {
  line-height: 1.6;
}
</style>