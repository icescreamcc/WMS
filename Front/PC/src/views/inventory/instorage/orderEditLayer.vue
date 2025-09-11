<template>
  <Layer :layer="layer" @confirm="submit">
    <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left"
      style="padding:0 15px">
      <el-row>
        <el-col :span="11">
          <el-form-item v-if="false" label="入库单号" prop="orderNo">
            <el-input v-model="ruleForm.orderNo" disabled placeholder="系统生成 无需填写"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item v-if="false" label="入库类型" prop="inStorageType">
            <el-select v-model="ruleForm.inStorageType" class="m-2" :disabled="!props.layer.showButton"
              style="width:100%" placeholder="选择入库类型 *必填">
              <el-option v-for="item in inStorageTypeData" :key="item.key" :label="item.value" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item label="物品类型" prop="goodsClassify">
            <el-select v-model="ruleForm.goodsClassify" class="m-2" :disabled="!props.layer.showButton"
              style="width:100%" @change="onClassifyChanged" placeholder="请选择需要入库的物品大类 *必填">
              <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item :label="remarkLabel" prop="remark">
            <el-input v-model="ruleForm.remark" :disabled="!props.layer.showButton" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item v-if="false" label="入库仓库" prop="warehouseId">
            <el-select v-model="ruleForm.warehouseId" class="m-2" :disabled="!props.layer.showButton" style="width:100%"
              placeholder="选择入库的仓库">
              <el-option v-for="item in warehouseData" :key="item.warehouseId" :label="item.warehouseName"
                :value="item.warehouseId">
              </el-option>
            </el-select>
          </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item v-if="false" label="责任人" prop="responsible">
            <el-select style="width: 100%;" :disabled="!props.layer.showButton" v-model="ruleForm.responsible"
              filterable remote reserve-keyword placeholder="输入责任人关键字查询" :remote-method="getUserData"
              @change="userSelectChanged" :loading="userSearchLoading">
              <el-option v-for="item in userData" :key="item.userId" :label="item.userName + ' ' + item.authAccount"
                :value="item.userId" />
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>
      <el-scrollbar max-height="300px">
        <div class="option-content">
          <el-row class="head">
            <el-col :span="props.layer.showButton ? 21 : 24">
              <p class="title">入库单明细</p>
            </el-col>
            <el-col v-if="props.layer.showButton" :span="3" style="text-align:right">
              <el-button v-if="false" style="margin-bottom:5px" type="success"
                :disabled="!ruleForm.goodsClassify || !props.layer.showButton" @click="onShowGoodsDrawer">选择111{{
                  invTitle
                }}</el-button>
            </el-col>
          </el-row>
          <div>
            <el-table :data="detailsData" border>
              <el-table-column prop="goodsNo" label="入库物品" align="center" min-width="200" :show-overflow-tooltip="true">
                <template #default="detail">
                  <el-input v-model="detail.row.goodsFullName" readonly :title="detail.row.goodsFullName"
                    :disabled="!props.layer.showButton">
                    <template #prepend>
                      <div style="width: 40px;">{{ detail.row.goodsClassifyName }}</div>
                    </template>
                    <template #append>{{ detail.row.goodsNo }}</template>
                  </el-input>
                </template>
              </el-table-column>
              <el-table-column prop="quantity" label="入库数量" align="center" min-width="130" :show-overflow-tooltip="true">
                  <template #default="detail">
                    <el-popover placement="top-start" title="入库货位推荐" :width="680"  trigger="focus">
                     <template #reference>
                       <el-input v-model="detail.row.quantity" style="width:65%" placeholder="数量"  :disabled="!props.layer.showButton" type="number" @change="onInputTotalPrice(detail.row)"  @focus="getWorkbinRecommendData(detail.row)"/>
                     </template>
                     <el-table :data="workbinRecommendData" height="200" @row-click="onSelectRecommend">
                      <el-table-column  property="warehouseName" width="105" label="仓库" />
                      <el-table-column  property="shelfName" width="115" label="货架" />
                      <el-table-column  property="binName" width="105" label="货位" />
                      <el-table-column  property="cellNo"  width="115" label="料箱" />
                      <el-table-column  property="specName" label="规格"/>
                      <el-table-column  property="maxStock" label="最大堆放">
                        <template #default="scope">
                          <span v-if="scope.row.maxStock>scope.row.stock">{{scope.row.maxStock+scope.row.maxStockUnitName}}</span>
                          <span v-else class="text-danger">{{scope.row.maxStock+scope.row.maxStockUnitName}}</span>
                        </template>
                      </el-table-column> 
                      <el-table-column  property="stock" label="现存">
                        <template #default="scope">
                          <span>{{scope.row.stock+scope.row.unitName}}</span>
                        </template>
                      </el-table-column> 
                    </el-table>
                  </el-popover> 
                  <el-select v-model="detail.row.unitId" class="m-2"  :disabled="true" style="width:35%" placeholder="单位">
                      <el-option v-for="item in detail.row.goodsUnitList" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
                     </el-select>
                </template>
                </el-table-column> 
              <el-table-column v-if="false" prop="minimumContainer" label="入库库位" align="center" min-width="150"
                :show-overflow-tooltip="true">
                <template #default="detail">
                  <el-input v-model="detail.row.minimumContainer" placeholder="点击选择入库库位" readonly
                    :disabled="!props.layer.showButton"
                    :title="`入库${detail.row.type == 'WorkbinCell' ? '料箱' : '货位'}${detail.row.minimumContainer || ''}`"
                    @click="onShowBinDrawer(detail.row)">
                    <template #prepend>
                      <span v-if="detail.row.type == 'WorkbinCell'">料箱</span>
                      <span v-else>货位</span>
                    </template>
                  </el-input>
                </template>
              </el-table-column>
              <!-- <el-table-column prop="totalPrice" label="入库总价" align="center" min-width="130"
                :show-overflow-tooltip="true">
                <template #default="detail">
                  <el-input v-model="detail.row.totalPrice" placeholder="请输入入库总价" style="width:65%" type="number"
                    :disabled="!props.layer.showButton" title="入库总价" @change="onInputTotalPrice(detail.row)">
                  </el-input>
                  <el-select v-model="detail.row.priceUnit" class="m-2" :disabled="!props.layer.showButton"
                    style="width:35%" placeholder="单位">
                    <el-option v-for="item in detail.row.priceUnitList" :key="item.key" :label="item.value"
                      :value="item.value">
                    </el-option>
                  </el-select>
                </template>
              </el-table-column> -->
              <el-table-column label="删除" align="center" min-width="50" :show-overflow-tooltip="true">
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
      </el-scrollbar>
      <GoodsSelectDrawer :options="goodsDrawerOptions" v-if="goodsDrawerOptions.show" @selectItem="onSelectGoods" />
      <BinSelectDrawer :options="binDrawerOptions" v-if="binDrawerOptions.show" @selectItem="onSelectBin" />
    </el-form>
  </Layer>
</template>

<script lang="ts" setup>
import { ref, defineEmits, defineProps, onMounted, nextTick } from 'vue'
import Layer from '@/components/layer/index.vue'
import { ElForm } from 'element-plus'
import msg from '@/utils/system/message'
import { getOptions, getWorkbinRecommend, getBinRecommend } from '@/api/inv/instorage'
import permission from '@/utils/system/permission'
import GoodsSelectDrawer from '@/components/drawer/goodsSelector.vue'
import BinSelectDrawer from '@/components/drawer/binSelector.vue'
import { Delete } from '@element-plus/icons-vue';
import { getUserByKey } from '@/api/common';
import { deftClassifyGroup } from '@/config';
import { getGoodsByKeyAndClassify } from '@/api/common';

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
        data: null,
      }
    }
  }
})
const emit = defineEmits(['dataSubmit'])
const warehouseData = ref(new Array<any>());
var warehouseDataBuffer = new Array<any>();
var invbinDataBuffer = new Array<any>();

const unitData = ref(new Array<any>());
const inStorageTypeData = ref(new Array<any>());
const goodsClassifyData = ref(new Array<any>());
const curEditDetail = ref();
const formRef = ref(ElForm || null);
const workbinRecommendData = ref(new Array<any>());
const userData = ref(new Array<any>());
const userSearchLoading = ref(false);
const ruleForm = ref({
  orderNo: props.layer.data?.orderNo,
  sourceOrderNo: props.layer.data?.sourceOrderNo,
  inStorageType: props.layer.data?.inStorageType,
  goodsClassify: props.layer.data?.goodsClassify ? props.layer.data?.goodsClassify : deftClassifyGroup,
  warehouseId: props.layer.data?.warehouseId,
  remark: props.layer.data?.remark,
  responsible: props.layer.data?.responsible,
  responsibleId: props.layer.data?.responsibleId,
  details: props.layer.data?.details,
  createUserId: '',
  createUserName: ''
})
const rules = {
  inStorageType: [{ required: true, message: '请选择入库类型', trigger: 'change' }],
  goodsClassify: [{ required: true, message: '请选择需要入库物品大类', trigger: 'change' }],
  remark: [{ max: 50, message: '字符超出限制长度', trigger: 'blur' }],
  sourceOrderNo: [{ max: 20, message: '字符超出限制长度', trigger: 'blur' }]
}

const goodsDrawerOptions = ref({
  show: false,
  title: '',
  type: '',
  mode: 'repet',
  isMultiSelect: true,
  data: new Array<any>()
});

const binDrawerOptions = ref({
  show: false,
  title: '',
  type: '',
  isMultiSelect: false,
  warehouse: null,
  data: {},
  recommendData: {}
});

const detailsData = ref(new Array<any>())
const invTitle = ref('');
const remarkLabel = ref('备注');

onMounted(() => {
  getOptions().then((res: any) => {
    warehouseData.value = res.data.warehouseOptions.filter((f: any) => f.warehouseType == 'RawMaterial' || f.warehouseType == 'FinishedProduct');//入库仓库
    const cOption = warehouseData.value.find((f: any) => f.warehouseType === "FinishedProduct");
    if (cOption) {
      ruleForm.value.warehouseId = cOption.warehouseId;
    }

    warehouseDataBuffer = res.data.warehouseOptions;
    unitData.value = res.data.unitOptions;
    inStorageTypeData.value = res.data.inStorageTypeOptions;
    invbinDataBuffer = res.data.invBinData;//bin

    const defaultClassifyKey = "FinishedProduct";
    // 入库类型 默认选中“生产入库”
    const defaultOption = inStorageTypeData.value.find((f: any) => f.key === "ProductIn");
    if (defaultOption.value) {
      ruleForm.value.inStorageType = defaultOption.key;;
    }
    // 物品类型 默认选中“成品”
    if (deftClassifyGroup == 'SparePart') {
      goodsClassifyData.value = res.data.goodsClassifyOptions.filter((f: any) => f.key == 'SparePart' || f.key == 'Consumables');
    }
    else {
      goodsClassifyData.value = res.data.goodsClassifyOptions.filter((f: any) => f.key == 'FinishedProduct' || f.key == 'RawMaterial');
      const dOption = goodsClassifyData.value.find((f: any) => f.key == "FinishedProduct");// f.value === "成品"
      if (dOption.value) {
        ruleForm.value.goodsClassify = dOption.key;
      }
    }
    if (deftClassifyGroup) {
      invTitle.value = res.data.goodsClassifyOptions.find((f: any) => f.key == deftClassifyGroup).value;
    }
    // const initialClassify =
    //   props.layer.data?.goodsClassify || // 优先用已有数据
    //   deftClassifyGroup ||               // 其次用外部传的分组
    //   goodsClassifyData.value[0]?.key || // 再用下拉第一个
    //   defaultClassifyKey;                // 最后兜底 FinishedProduct

    // 给表单赋值
    // ruleForm.value.goodsClassify = initialClassify;

    // onClassifyChanged(initialClassify || deftClassifyGroup, false);
    if (props.layer.data) {

      // 编辑：加载已有数据，不创建新行
      const initialClassify =
        props.layer.data.goodsClassify ||
        deftClassifyGroup ||
        goodsClassifyData.value[0]?.key ||
        defaultClassifyKey;

      onClassifyChanged(initialClassify, false); //  不触发新增逻辑

      detailsData.value = props.layer.data.details;
      detailsData.value.forEach(detail => {
        detail.goodsUnitList = unitData.value.filter(u => u.value == detail.packageUnitName || u.value == detail.minPackageUnitName || u.value == detail.maxPackageUnitName);
        detail.priceUnitList = unitData.value.filter(f => f.type == 'Currency');
        if (!detail.priceUnit) {
          detail.priceUnit = '元'
        }
        detail.goodsFullName = detail.goodsModel ? detail.goodsName + ' ' + detail.goodsModel : detail.goodsName;
        if (detail.workbinCellNo) {
          detail.minimumContainer = detail.workbinCellNo;
          detail.type = "WorkbinCell";
        }
        else if (detail.binName) {
          detail.minimumContainer = detail.binName;
          detail.type = "Bin";
        }
      });
    } else {
      // 新增：默认 RawMaterial 并且自动创建一行
      const initialClassify =
        deftClassifyGroup ||
        goodsClassifyData.value[0]?.key ||
        defaultClassifyKey;

      onClassifyChanged(initialClassify, true); //  新增逻辑
    }
  })
})

const getUserData = (keyword: string) => {
  userSearchLoading.value = true;
  getUserByKey(keyword).then((res: any) => {
    userData.value = res.data;
  }).finally(() => userSearchLoading.value = false)
}

const userSelectChanged = (value: any) => {
  let user = userData.value.find(f => f.userId == value);
  if (user) {
    ruleForm.value.responsibleId = value;
    ruleForm.value.responsible = user.userName;
  }
}

const onShowGoodsDrawer = () => {
  goodsDrawerOptions.value.show = true;
  goodsDrawerOptions.value.type = ruleForm.value.goodsClassify;
  goodsDrawerOptions.value.title = invTitle.value + '选择';
  goodsDrawerOptions.value.data = detailsData.value;
}

const onShowBinDrawer = (detail: any) => {
  if (ruleForm.value.warehouseId) {
    getBinRecommend(ruleForm.value.warehouseId, detail.goodsSpecificationId).then(res => {
      binDrawerOptions.value.recommendData = res.data;
      binDrawerOptions.value.show = true;
      if (detail.workbinCellNo) {
        binDrawerOptions.value.title = "选择入库料箱"
        binDrawerOptions.value.type = "Workbin";
      }
      else if (detail.binNo) {
        binDrawerOptions.value.title = "选择入库货位"
        binDrawerOptions.value.type = "Bin";
      }
      else {
        binDrawerOptions.value.title = "选择入库货架"
        binDrawerOptions.value.type = "Shelf";
      }
      binDrawerOptions.value.warehouse = warehouseData.value.find(f => f.warehouseId == ruleForm.value.warehouseId);
      binDrawerOptions.value.data = detail;
      curEditDetail.value = detail;
    })
  }
  else {
    msg.warningAuto("请先选择出库仓库");
  }
}

const onInputTotalPrice = (detail: any) => {
  if (detail.totalPrice && detail.quantity && detail.totalPrice > 0 && detail.quantity > 0) {
    detail.unitPrice = (Number(detail.totalPrice) / Number(detail.quantity)).toFixed(2);
  }
}

const onSelectGoods = (selectedGoods: any) => {
  let objStr = JSON.stringify(selectedGoods);
  detailsData.value.push(JSON.parse(objStr));
  detailsData.value.forEach(f => {
    f.goodsFullName = f.goodsModel ? f.goodsName + ' ' + f.goodsModel : f.goodsName;
    f.goodsUnitList = unitData.value.filter(u => u.value == f.packageUnitName || u.value == f.minPackageUnitName || u.value == f.maxPackageUnitName);
    f.priceUnitList = unitData.value.filter(f => f.type == 'Currency');
    if (f.goodsId == selectedGoods.goodsId) {
      f.priceUnit = selectedGoods.priceUnitName || "元";
    }
    if (f.goodsUnitList.length >= 1 && (!f.unitId || Number(f.unitId) == 0)) {
      f.unitId = f.goodsUnitList[0].key;
    }
  });
}

const onRemoveDetail = (detail: any) => {
  detailsData.value.splice(detailsData.value.indexOf(detail), 1);
}

const onSelectBin = (selectedElement: any) => {
  curEditDetail.value.warehouseId = selectedElement.warehouseId;
  curEditDetail.value.shelfId = selectedElement.shelfId;
  curEditDetail.value.shelfNo = selectedElement.shelfNo;
  curEditDetail.value.shelfName = selectedElement.shelfName;
  curEditDetail.value.binId = selectedElement.binId;
  curEditDetail.value.binNo = selectedElement.binNo;
  curEditDetail.value.binName = selectedElement.binName;
  curEditDetail.value.workbinId = selectedElement.workbinId;
  curEditDetail.value.workbinNo = selectedElement.workbinNo;
  curEditDetail.value.workbinCellId = selectedElement.workbinCellId;
  curEditDetail.value.workbinCellNo = selectedElement.workbinCellNo;
  curEditDetail.value.type = selectedElement.type;
  if (selectedElement.workbinCellNo) {
    curEditDetail.value.minimumContainer = selectedElement.workbinCellNo
  }
  else if (selectedElement.binName) {
    curEditDetail.value.minimumContainer = selectedElement.binName
  }
  else {
    curEditDetail.value.minimumContainer = "";
  }
}

const getWorkbinRecommendData = (detail: any) => {
  getWorkbinRecommend(detail.goodsId).then(res => {
    workbinRecommendData.value = res.data
  })
}

const onSelectRecommend = (row: any, column: any, event: any) => {
  detailsData.value.forEach(d => {
    if (d.goodsId == row.goodsId) {
      d.warehouseId = row.warehouseId;
      d.shelfId = row.shelfId;
      d.shelfNo = row.shelfNo;
      d.shelfName = row.shelfName;
      d.binId = row.binId;
      d.binNo = row.binNo;
      d.binName = row.binName;
      d.workbinId = row.workbinId;
      d.workbinNo = row.workbinNo;
      d.workbinCellId = row.cellId;
      d.workbinCellNo = row.cellNo;
      // const unit = d.goodsUnitList.find((u: any) => u.key == row.unitId);
      // d.unitId = unit ? unit.key : d.goodsUnitList[0]?.key; // 默认取第一项
      d.unitId = Number(row.unitId);
      d.unitName = row.unitName;

      if (row.cellNo) {
        d.minimumContainer = row.cellNo;
        d.type = 'WorkbinCell';
      }
      else if (row.binName) {
        d.minimumContainer = row.binName;
        d.type = 'Bin';
      }
      else {
        d.minimumContainer = "";
        d.type = 'Shelf';
      }
    }
  })
}

const onClassifyChanged = (val: any, isNew: boolean = true) => {
  if (val) {
    remarkLabel.value = val == 'Separator' ? '备注' : '备注';
    invTitle.value = goodsClassifyData.value.find(f => f.key == val).value;
    let warehouseBygroup = warehouseDataBuffer.filter(f => f.warehouseType == ruleForm.value.goodsClassify);
    if (warehouseBygroup?.length > 0) {
      ruleForm.value.warehouseId = warehouseBygroup[0].warehouseId;
      warehouseData.value = warehouseBygroup;
    }
    else {
      warehouseData.value = warehouseDataBuffer.filter((f: any) => f.warehouseType == 'RawMaterial' || f.warehouseType == 'FinishedProduct');
    }
    let binData = invbinDataBuffer.filter(f => f.warehouseId == warehouseData.value[0].warehouseId);

    //add 只有新增时才执行下面这段
    ruleForm.value.goodsClassify = val;
    if (isNew) { //控制是否新增行，首次增加新增行，否则编辑不新增
      // 调接口拉取原材料数据
      getGoodsByKeyAndClassify(val, 0, "", "", 60).then((res: any) => {

        if (res.data?.length > 0) {
          // 自动选第一条
          const first = res.data[0];

          // 从全局 unitData 里匹配单位（用名字匹配）
          const goodsUnits = unitData.value.filter((u: any) =>
            u.value == first.packageUnitName ||
            u.value == first.minPackageUnitName ||
            u.value == first.maxPackageUnitName
          );
          const defaultUnit = goodsUnits[0] || { key: first.unitId, value: first.unitName };
          const newRow = {
            goodsId: first.goodsId,
            binId: first.binId,
            goodsFullName: first.goodsName + (first.goodsModel ? ' ' + first.goodsModel : ''),
            goodsClassifyName: first.goodsClassifyName,
            goodsNo: first.goodsNo,
            goodsName: first.goodsName,
            quantity: 1, // 默认数量
            unitId: defaultUnit.key,
            unitName: defaultUnit.value,
            goodsUnitList: goodsUnits,
            goodsSpecificationId: first.goodsSpecificationId,
            type: 'Bin',
            minimumContainer: '',
            warehouseId: '', // 新增属性
            warehouseName: '', // 可选：新增属性
            binNo: '', // 新增属性
            binName: '', // 可选：新增属性
          };


          // 自动获取货位推荐
          // 这里直接用默认仓库，获取推荐库位并自动带第一个  不再调 getWorkbinRecommend
          newRow.warehouseId = warehouseBygroup[0].warehouseId;
          if (binData?.length > 0) {
            newRow.binNo = binData[0]?.binNo;
            newRow.binId = binData[0]?.binId;
            newRow.warehouseName = binData[0]?.binName;
            newRow.minimumContainer = binData[0]?.binName;
          } else {
            msg.warningAuto("请新增库位")
            return
          }
          // 追加新行
          detailsData.value.push(newRow);

          // 强制刷新视图（保险做法）
          const idx = detailsData.value.indexOf(newRow);
          if (idx !== -1) {
            detailsData.value.splice(idx, 1, Object.assign({}, detailsData.value[idx]));
          }


        }
      });
    }

  }
}


const submit = () => {
  formRef.value.validate((valid: any) => {
    if (valid) {
      if (detailsData.value.length == 0) {
        msg.warningAuto("请添加入库单明细")
        return
      }
      else {
        for (let opt of detailsData.value) {
          if (!opt.goodsId) {
            msg.warningAuto("请选择入库物品")
            return
          }
          if (!opt.quantity || Number(opt.quantity) <= 0) {
            msg.warningAuto("请输入正确的入库数量")
            return
          }
          if (!opt.totalPrice || Number(opt.totalPrice) <= 0) {
            if (deftClassifyGroup == 'SparePart') {
              msg.warningAuto("请输入正确的入库总价")
              return
            }
            else {
              opt.totalPrice = 0;
            }
          }
          if (!opt.unitId || Number(opt.unitId) == 0) {
            msg.warningAuto("请选择入库单位")
            return
          }
          if (!opt.binId) {
            msg.warningAuto("请选择入库货位")
            return
          }
        }
      }
      ruleForm.value.details = detailsData.value;
      let editType = props.layer.data ? 'update' : 'add';
      ruleForm.value.createUserId = permission.getOperator().userId;
      ruleForm.value.createUserName = permission.getOperator().userName;
      emit('dataSubmit', ruleForm.value, editType)
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
    // border-bottom: 1px solid rgb(230, 230, 230);
    margin-bottom: 5px;

    .title {
      margin: 5px 0 0 0;
    }
  }

  .item {
    margin: 3px 0;
  }
}
</style>