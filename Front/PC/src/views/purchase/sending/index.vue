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

        <el-button icon="el-icon-download" style="margin-left:10px" @click="getImportTemplate">模板下载</el-button>
        <el-button icon="el-icon-upload" style="margin-left:10px" @click="showImportModal">导入计划</el-button>
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

        <el-select v-model="query.isUrgentShipment" ref="refSelectClassifyGroup" size="small"
          :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;"
          @change="getTableData(true)" filterable clearable placeholder="选择是否紧急发货">
          <template #prefix>
            <div
              style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
              是否紧急发货</div>
          </template>
          <el-option v-for="item in urgentShipmentData" :key="item.key" :label="item.value"
            :value="item.key"></el-option>
        </el-select>

          <el-select v-model="query.sendingAddress" ref="refSelectClassifyGroup" size="small"
            :disabled="disableClassifyGroupSelect" class="m-2" style="width:90%;margin-left: 20px;"
            @change="getTableData(true)" filterable clearable placeholder="选择到货地址">
            <template #prefix>
              <div
                style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">
                到货地址</div>
            </template>
            <el-option v-for="item in sendingAddressData" :key="item.key" :label="item.value"
              :value="item.key"></el-option>
          </el-select>

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
      <Table ref="table" v-model:page="page" v-loading="loading" :showSelection="true" :data="tableData"
        @getTableData="getTableData" @selection-change="handleSelectionChange" @orderChanged="handleSortChange"
        @expandChange="handleExpandChange">
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
        <el-table-column prop="urgentShipmentDesc" label="是否紧急发货" align="center" sortable="custom" min-width="120"
          :show-overflow-tooltip="true" />
        <el-table-column prop="sufficientStockDesc" label="是否有足够库存" align="center" sortable="custom" min-width="130"
          :show-overflow-tooltip="true" />
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
                <el-button v-if="props.row.status=='WaitingShipment'" circle type="success" style="height: 30px;"  @click="handleConfirmShipment(props.row)" title="点击确认发货">
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

        <el-table-column label="客户标签" align="left" min-width="124"
          v-if="permission.isPermisstion('SENDINGUPLOAD', 'SENDINGNOTIFICATION')">
          <template #default="scope">
         <el-button 
          v-if="permission.isPermisstion('SENDINGUPLOAD')"  
          @click="handleUpload(scope.row)" :type="scope.row.isInBaseFiles ? 'success' : ''" :style="scope.row.isInBaseFiles ? { backgroundColor: '#67C23A', color: '#fff', borderColor: '#67C23A' } : {}">上传
        </el-button>
            <el-button v-if="permission.isPermisstion('SENDINGNOTIFICATION') && scope.row.isEmailNotification"
              style="height: 30px;" title="已发送邮件通知" @click="onSendingNotification(scope.row)" circle>
              <img src="../../../../public/icon-img/yidu1.png" height="14">
            </el-button>
            <el-button v-if="permission.isPermisstion('SENDINGNOTIFICATION') && !scope.row.isEmailNotification"
              style="height: 30px;" title="未发送邮件通知" @click="onSendingNotification(scope.row)" circle>
              <img src="../../../../public/icon-img/xiaoxi4.png" height="14">
            </el-button>

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
  name: "sending"
})
import { onMounted, ref, reactive } from "vue";
import { Page } from "@/components/table/type";
import {
  getSending, getOrderDetail, getYesOrNoGroup, getDetailStatusGroup, getSendingAddressGroup,addSending,updateSending, delSending
  , createImportTemplate, exportSendingData, getExportFields, getBaseFiles, adviceSending
} from "@/api/purchase/sendingorder";
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
  console.log(" ### [getTableData]  ermission.getOperator().userId = ",permission.getOperator().userId)
  console.log(" ### [getTableData]  page.index = ",page.index)
  console.log(" ### [getTableData]  page.orderField= ",page.orderField)
  console.log(" ### [getTableData]  page.orderType= ",page.orderType)
  console.log(" ### [getTableData]  query.input= ",query.input)
  console.log(" ### [getTableData] dateStart = ",dateStart)
  console.log(" ### [getTableData] dateEnd = ",dateEnd)
  console.log(" ### [getTableData] selectedGoodsGroup.value = ",selectedGoodsGroup.value)
  console.log(" ### [getTableData] query.isUrgentShipment = ",query.isUrgentShipment)
  console.log(" ### [getTableData] query.sendingAddress = ",query.sendingAddress)
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

const onSendingNotification = (row: any) => {
  getOrderDetail(permission.getOperator().userId, row.orderNo).then((res: any) => {
    let t = goodsGroupData.value.find(f => f.key == selectedGoodsGroup.value).value;
    let bodyTitle = `<div style="text-align: left;"> 
                          <div style="font-weight: 600;font-size:15px;">${res.data.length>0?res.data[0].createUserName:""}创建了一份${t}发货计划，计划发货日期为${commonHelper.formatToDate(res.data.length>0?res.data[0].sendingDate:"")}</div>
                          </div>`;
    let bodyContent = '<div style="padding:3px 10px;border: 1px solid #ededed;">';
    res.data.forEach((d: any) => {
      let info = `<div style="padding:5px;">
                          <div style="width:100%;padding-top:3px;">物料：${d.goodsNo}</span>
                          <div style="width:100%;padding-top:3px;">客户料号:${d.customerGoodsNo}</span>
                          <div style="width:100%;padding-top:3px;">客户配送中心:${d.customerIdentificationCode}</span>
                          <div style="width:100%;padding-top:3px;">发货数量:${d.quantity}</span>
                          <div style="width:100%;padding-top:3px;">发货托数:${d.palletsQuantity}</span>
                          <div style="width:100%;padding-top:3px;">运输供应商:${d.supplierName}</span>
                          <div style="width:100%;padding-top:3px;">收件人：${d.receivingResponsableUserInfo || ''}</div>
                          <div style="width:100%;padding-top:3px;">特殊要求：${d.specialRequest || ''}</div>
                          <div style="width:100%;padding-top:3px;">到货地址：${d.sendingAddress || ''}</div>
                          <div style="width:100%;padding-top:3px;">备注：${d.remark || ''}</div>
                        </div>`;
      bodyContent += info;
    });
    bodyContent += `</div>`;
    let content = `<div>${bodyTitle}${bodyContent}</div>`;

    getBaseFiles(permission.getOperator().userId, page.size, page.index, page.orderField, page.orderType, query.input, row.orderNo, "SendingOrderAttachment")
      .then((res) => {
        let data = res.data.rows;
        mailEditLayer.data = {
          subject: t + '发货通知',
          body: content,
          bodyType: 'html',
          toReciver: row.sendingSupplierUserEmail.split(","),
          toCC: [],
          orderNo: row.orderNo,
          attachments: data
        }
        mailEditLayer.show = true;
      })
      .catch((error) => {
      })
      .finally(() => {
      })
  });
}

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
    orderLayer.title = "确认发货";
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

const handleUpload = (row: any) => {
  getOrderDetail(permission.getOperator().userId, row.orderNo).then((res: any) => {
    uploadDocumentLayer.show = true;
    uploadDocumentLayer.data = res.data[0];
  })
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

const getImportTemplate = () => {
  createImportTemplate().then(res => {
    let link = document.createElement('a')
    link.style.display = 'none'
    link.href = res.data
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
  })
}

const uploadSuccess = () => {
  importLayer.show = false;
  getTableData(true);
}

const showImportModal = () => {
  importLayer.show = true;
}

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
.csm-link {
  cursor: pointer;
  text-decoration: underline;
}
</style>