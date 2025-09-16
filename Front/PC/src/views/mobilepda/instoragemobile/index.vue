<template>
    <div class="app-content">
        <div class="content-main">
            <div class="content-form">
                <el-form label-width="auto" label-position="left" style="padding:0 15px">

                    <!-- 大类 -->
                    <el-row v-if="false" class="input-item">
                        <el-col :span="8">
                            <div class="input-title">大类</div>
                        </el-col>
                        <el-col :span="16">
                            <el-select v-model="selectedGoodsGroup" class="input-cls" @change="onSelectGoodsGroup">
                                <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value"
                                    :value="item.key" />
                            </el-select>
                        </el-col>
                    </el-row>


                    <!-- 小类 -->
                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">小类*</div>
                        </el-col>
                        <el-col :span="16">
                            <el-select v-model="selectedGoodsClassifyId" class="input-cls"
                                @change="getGoodsDatya(true)">
                                <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value"
                                    :value="item.key" />
                            </el-select>
                        </el-col>
                    </el-row>

                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">物品名称*</div>
                        </el-col>
                        <el-col :span="16">
                            <input id="input-goodsname" type="text" class="input-cls" :value="selectedGoods?.goodsName"
                                disabled>
                            <!-- <span class="body-item-info">{{ selectedGoods?.goodsName }}</span>   -->
                        </el-col>
                    </el-row>

                    <!-- 仓库 -->
                    <el-row v-if="false" class="input-item">
                        <el-col :span="8">
                            <div class="input-title">仓库</div>
                        </el-col>
                        <el-col :span="16">
                            <el-select v-model="selectedWarehouseId" class="input-cls" @change="onWarehouseSelected">
                                <el-option v-for="item in warehouseData" :key="item.warehouseId"
                                    :label="item.warehouseName" :value="item.warehouseId" />
                            </el-select>
                        </el-col>
                    </el-row>

                    <!-- 货架 -->
                    <el-row v-if="false" class="input-item">
                        <el-col :span="8">
                            <div class="input-title">货架</div>
                        </el-col>
                        <el-col :span="16">
                            <el-select v-model="selectedShelfId" class="input-cls" @change="getBinDataByShelf">
                                <el-option v-for="item in shelfData" :key="item.id" :label="item.name"
                                    :value="item.id" />
                            </el-select>
                        </el-col>
                    </el-row>

                    <!-- 货位 -->
                    <el-row v-if="false" class="input-item">
                        <el-col :span="8">
                            <div class="input-title">货位</div>
                        </el-col>
                        <el-col :span="16">
                            <el-select v-model="selectedBinId" class="input-cls" @change="getGoodsDatya(true)">
                                <el-option v-for="item in binData" :key="item.id" :label="item.name" :value="item.id" />
                            </el-select>
                        </el-col>
                    </el-row>
                    <!-- 入库时间 -->
                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">入库时间*</div>
                        </el-col>
                        <el-col :span="16">
                            <input id="input-storageDate" type="date" class="input-cls" placeholder="请选择入库时间"
                                v-model="storageDate">
                            <!-- <el-select  class="input-cls">
                                
                            </el-select> -->
                        </el-col>
                    </el-row>
                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">运输单号</div>
                        </el-col>
                        <el-col :span="16">
                            <input id="input-transportOrderNo" type="text" class="input-cls" placeholder="请输入运输单号">
                        </el-col>

                    </el-row>

                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">车牌号码</div>
                        </el-col>
                        <el-col :span="16">
                            <input id="input-licensePlateNo" type="text" class="input-cls" placeholder="请输入车牌号码">
                            <!-- <el-select  class="input-cls">
                                
                            </el-select> -->
                        </el-col>
                    </el-row>
                    <!-- 入库数量 -->
                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">入库数量*</div>
                        </el-col>
                        <el-col :span="14">
                            <input type="number" v-model="quantity" class="input-cls" placeholder="请输入入库数量">
                        </el-col>
                        <el-col :span="2">
                            <div class="input-icon">
                                <img :src="editImg" alt="" height="20">
                            </div>
                        </el-col>
                    </el-row>

                    <!-- 备注 -->
                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">备注</div>
                        </el-col>
                        <el-col :span="14">
                            <input type="text" v-model="remark" class="input-cls" placeholder="请输入备注">
                        </el-col>
                        <el-col :span="2">
                            <div class="input-icon">
                                <img :src="editImg" alt="" height="20">
                            </div>
                        </el-col>
                    </el-row>
                    <el-row class="input-item"> </el-row>
                    <el-row class="input-item"> </el-row>
                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-titlenone">运输单签字附件</div>
                        </el-col>
                        <el-col :span="16" class="upload-col">
                            <Upload :uploadParams="uploadParams" v-if="showUpload" @handleImgChanged="imgChanged" />
                        </el-col>
                    </el-row>
                </el-form>
            </div>


            <!-- <div class="content-body">
      <el-row v-if="goodsData.length > 0" class="body-row">
        <el-col
          :xs="24"
          :sm="12"
          :md="12"
          :lg="6"
          :xl="6"
          v-for="item in goodsData"
          class="body-item"
        >
          <el-card class="item-card" shadow="never">
            <div class="item-img">
              <el-image
                hide-on-click-modal
                class="img-goods"
                :src="item.goodsPicture"
                :zoom-rate="1.2"
                :preview-src-list="[item.goodsPicture]"
                :initial-index="0"
                fit="cover"
              />
            </div>
            <div class="item-desc" @click="onShowDetail(item)">
              <div class="desc-title">
                {{ item.goodsName }}
                <span class="desc-info" v-if="item.goodsModel">{{ item.goodsModel }}</span>
              </div>
              <div class="desc-info">分类：{{ item.goodsClassifyName }}</div>
              <div class="desc-info">储存规格：{{ item.goodsSpecificationName }}</div>
              <div class="desc-info">供应商：{{ item.supplier }}</div>
              <div class="desc-info">
                安全库存：
                <span v-if="item.safetyInventory > 0">
                  {{ item.safetyInventory + item.safetyInventoryUnitName }}
                </span>
              </div>
              <div class="desc-info">
                最小采购：
                <span v-if="item.purchaseMinimum > 0">
                  {{ item.purchaseMinimum + item.purchaseMinimumUnitName }}
                </span>
              </div>
            </div>
          </el-card>
        </el-col>
      </el-row>
      <el-row v-else>
        <el-col><el-empty description="没有任何数据" /></el-col>
      </el-row>
      <div v-if="totalPage > 1">
        <img src="/public/icon-img/left-circle-fill.png" alt="上一页" v-if="pgIndex > 1" class="btn-page-pre" @click="onPrePage">
        <img src="/public/icon-img/left-circle-fill-dis.png" alt="上一页" v-else class="btn-page-pre">
        <img src="/public/icon-img/right-circle-fill.png" alt="下一页" v-if="pgIndex < totalPage" class="btn-page-next" @click="onNextPage">
        <img src="/public/icon-img/right-circle-fill-dis.png" alt="下一页" v-else class="btn-page-next">
      </div>
    </div> -->
            <div class="content-desc">
                <div style="padding-top: 4%;">
                    <div style="margin-bottom: 10px;">
                        <el-icon>
                            <InfoFilled />
                        </el-icon>
                        <span> 操作流程：</span>
                    </div>
                    <div>
                        <p style="margin-top: -5px;">输入入库数量→上传运输单签字附件→点击确认→入库成功</p>
                    </div>
                </div>
            </div>
        </div>
        <div class="content-btn">
            <div v-if="permission.isPermisstion('TRUCKSUBMIT')">
                <el-button type="success" round @click="onmessageinfo" style="width:82%;"
                    :loading="submitLoading">确认</el-button>
            </div>
            <div style="margin-top: 4px;">
                <el-button round @click="onClearForm" style="width:82%;" :loading="submitLoading">重置信息</el-button>
            </div>
        </div>
        <MessageDrawer :options="msgDrawerOptions" @cancel="onLockBinCancel" @confirm="onSubmit" />

    </div>

</template>
<script lang="ts" setup>
import { ref, defineEmits, defineProps, onMounted, onBeforeUnmount } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { ArrowLeft, Search } from '@element-plus/icons-vue';
import { getGoodsList } from "@/api/baseinfo/goods";
import { getGoodsGroup, getGoodsClassify, getWarehouses, getShelfByWarehouse, getBinByShelf } from '@/api/common';
import { getOrders, getArgs, getOrderDetail, addInStorage, updateInStorage, delInStorage, exportInStorage, getAllowField, approvalInStorage, confirmInStorage, agvScheduling } from "@/api/inv/instorage";
import permission from '@/utils/system/permission';
import commonHelper from "@/utils/system/common-helper";
import { deftClassifyGroup } from '@/config';
import msg from "@/utils/system/message";
import MessageDrawer from '../message-drawer.vue'
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
                data: null,
            }
        }
    }
})
//提示框
const msgDrawerOptions = ref({
    show: false,
    title: '',
    message: '',
    type: '',
    data: null
});

const onClearForm = () => {
    quantity.value = 1;
    document.getElementById('input-carSoleCode')?.focus();
}
const editImg = '/public/icon-img/bianji5.png';
const router = useRouter();
const route = useRoute();
const goodsData = ref(new Array<any>());
const pgSize = ref(10);
const pgIndex = ref(1);
const totalDate = ref(0);
const totalPage = ref(0);
const goodsGroupData = ref(new Array<any>());
const selectedGoodsGroup = ref(deftClassifyGroup);
const goodsClassifyData = ref(new Array<any>());
const selectedGoodsClassifyId = ref(0);
const searchKey = ref('');
const quantity = ref(0);
const remark = ref('');
const goodsSort = ref('goodsName-asc');
const warehouseData = ref(new Array<any>());
const shelfData = ref(new Array<any>());
const binData = ref(new Array<any>());
const selectedWarehouseId = ref('');
const selectedShelfId = ref('');
const selectedBinId = ref('');
const warehouseDataBuffer = ref(new Array<any>());
const storageKey = "instoragemobile";
const submitLoading = ref(false);
// 定义响应式日期变量
const storageDate = ref('');
const showUpload = ref(true);


interface DetailItem {
    goodsId: string
    goodsNo: string
    goodsName: string
    goodsFullName: string
    goodsClassifyId: number
    goodsClassifyName: string
    // goodsClassify: string
    goodsUnitList: any[]
    unitId: number
    unitName: string
    warehouseId: string
    // warehouseName: string
    shelfId: string
    binId: number
    binNo: string
    binName: string
    minimumContainer: string
    quantity: number
    totalPrice: number
    type: string
    createUserId: string
    createUserName: string
}

const dataForm = ref({
    orderNo: '',
    sourceOrderNo: '',
    inStorageType: 'ProductIn',
    goodsClassify: 'RawMaterial',
    warehouseId: '',
    warehouseName: '',
    responsible: '',
    responsibleId: '',
    details: [] as DetailItem[],
    createUserId: '',
    createUserName: '',
    goodsPicture: [],  // 三方签字图片
    remark: '',
    transportOrderNo:'',//运输单号
    licensePlateNo:'',//车牌单号
    createDate: '',//入库时间
})
const uploadParams = ref({
    uploadApi: '/InStorage/UploadInstorgePic',
    limit: 3,
    imgUrlList: [],
    validFileType: 'image',
    validFileSize: 10240,
    isEdit: true,
    titile: '点击上传图片',
    width: '80px',
    height: '80px',
});
const imgChanged = (imgList: Array<any>) => {
    selectedGoods.value.goodsPicture = imgList.map(x => {
        return {
            FileName: x.name,  // 原文件名
            Url: x.url,        // 图片 URL
        }
    })
    // selectedGoods.value.goodsPicture= imgList.map(x => {
    //     return x.response ?? { FileName: x.name, Url: x.url }
    // })
}


onMounted(() => {

    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    storageDate.value = `${year}-${month}-${day}`;

    getWarehousesData().then(() => {
        if (warehouseDataBuffer.value.length > 0) {
            // 默认选择第一个仓库
            selectedWarehouseId.value = warehouseDataBuffer.value[0].warehouseId
            dataForm.value.warehouseId = warehouseDataBuffer.value[0].warehouseId
            // 同时加载货架和货位
            getShelfDataByWarehouse()
        }
        getGoodsGroupData().then(() => {
            //取出来，再赋值给 selectedGoodsGroup，实现“记住上次选择”的效果。
            let queryParamsHis = commonHelper.getObjLocalStorage(storageKey);
            if (queryParamsHis) {
                pgSize.value = queryParamsHis.pgSize;
                pgIndex.value = queryParamsHis.pgIndex;
                goodsSort.value = queryParamsHis.orderField + '-' + queryParamsHis.orderType;
                selectedGoodsGroup.value = queryParamsHis.selectedGoodsGroup;
                selectedGoodsClassifyId.value = queryParamsHis.selectedGoodsClassifyId;
                selectedWarehouseId.value = queryParamsHis.selectedWarehouseId;
                selectedShelfId.value = queryParamsHis.selectedShelfId;
                selectedBinId.value = queryParamsHis.binId;
                searchKey.value = queryParamsHis.searchKey;
                let warehouseBygroup = warehouseDataBuffer.value.filter(f => f.warehouseType == selectedGoodsGroup.value);
                if (warehouseBygroup?.length > 0) {
                    warehouseData.value = warehouseBygroup;
                }
                if (selectedWarehouseId.value) {
                    getShelfDataByWarehouse();
                }
                if (selectedShelfId.value) {
                    getBinDataByShelf()
                }
                getGoodsClassifyData();
                getGoodsDatya(true);
            }
            else {
                onSelectGoodsGroup();
            }
        })
    })
});

onBeforeUnmount(() => {
    commonHelper.setObjLocalStorage(storageKey, null);
})

const defaultClassifyKey = "RawMaterial";
const getGoodsGroupData = () => {
    return getGoodsGroup().then(res => {
        if (deftClassifyGroup == 'SparePart') {
            goodsGroupData.value = res.data.filter((f: any) => f.key == 'SparePart' || f.key == 'Consumables');
        }
        else {
            goodsGroupData.value = res.data;
            // 优先选中
            let defaultItem = goodsGroupData.value.find((f: any) => f.key == defaultClassifyKey);
            selectedGoodsGroup.value = defaultItem ? defaultItem.key : goodsGroupData.value[0].key;
        }
    })
}

const onSelectGoodsGroup = () => {
    warehouseData.value.length = 0;
    shelfData.value.length = 0;
    binData.value.length = 0;
    selectedGoodsClassifyId.value = 0;
    selectedWarehouseId.value = '';
    selectedShelfId.value = '';
    selectedBinId.value = '';
    let warehouseBygroup = warehouseDataBuffer.value.filter(f => f.warehouseType == selectedGoodsGroup.value);
    if (warehouseBygroup?.length > 0) {
        warehouseData.value = warehouseBygroup;
        if (warehouseData.value.length > 0) {
            selectedWarehouseId.value = warehouseData.value[0].warehouseId;
            onWarehouseSelected(); // 选完仓库后，自动加载货架
        }
    }
    getGoodsClassifyData();
    getGoodsDatya(true);
}

const getGoodsClassifyData = () => {
    return getGoodsClassify(selectedGoodsGroup.value).then(res => {
        goodsClassifyData.value = res.data;
        if (goodsClassifyData.value.length > 0) {
            selectedGoodsClassifyId.value = goodsClassifyData.value[0].key; // 默认第一个
        }
    })
}

const getWarehousesData = () => {
    return getWarehouses().then(res => {
        if (res.data) {
            warehouseDataBuffer.value = res.data.map((m: any) => {
                return {
                    warehouseId: m.warehouseId,
                    warehouseName: m.warehouseName,
                    warehouseType: m.warehouseType
                }
            });
        }
    })
}
const selectedWarehouse = ref<any>(null)
const onWarehouseSelected = () => {
    getShelfDataByWarehouse();
}

const getShelfDataByWarehouse = () => {
    return getShelfByWarehouse(selectedWarehouseId.value).then(res => {
        shelfData.value = res.data;
        if (shelfData.value.length > 0) {
            selectedShelfId.value = shelfData.value[0].id; // 绑定第一个货架的ID
            // 选完货架后，自动加载货位
            getBinDataByShelf();
        }
    })
    // return getShelfByWarehouse(selectedWarehouseId.value).then(res => {
    //     // shelfData.value = [{ id: '', name: 'All' }, ...res.data];
    //     shelfData.value = res.data
    // })
}

const getBinDataByShelf = () => {
    binData.value.length = 0;
    selectedBinId.value = '';
    return getBinByShelf(selectedShelfId.value).then(res => {
        binData.value = res.data;// [{ id: '', name: 'All' }, ...res.data];
        selectedBinId.value = binData.value.length > 0 ? binData.value[0].id : '';
        getGoodsDatya(true);
    })
}
const selectedGoods = ref<any>(null);
const getGoodsDatya = (init: boolean) => {
    if (init) {
        pgIndex.value = 1
    }
    let orderField = goodsSort.value.split('-')[0];
    let orderType = goodsSort.value.split('-')[1];
    let binId: number = selectedBinId.value ? Number(selectedBinId.value) : 0;
    getGoodsList(pgSize.value, pgIndex.value, orderField, orderType, selectedGoodsGroup.value, selectedGoodsClassifyId.value, selectedShelfId.value, binId, searchKey.value).then(res => {
        goodsData.value = res.data.rows;
        totalDate.value = res.data.total;
        totalPage.value = Math.ceil(totalDate.value / pgSize.value);
    })
    if (goodsData.value.length > 0) {
        selectedGoods.value = goodsData.value[0];
    }
}

const onPrePage = () => {
    if (pgIndex.value > 1) {
        pgIndex.value--;
        getGoodsDatya(false);
    }
}

const onNextPage = () => {
    if (pgIndex.value < totalPage.value) {
        pgIndex.value++;
        getGoodsDatya(false);
    }
}

const onShowDetail = (item: any) => {
    let orderField = goodsSort.value.split('-')[0];
    let orderType = goodsSort.value.split('-')[1];
    let binId: number = selectedBinId.value ? Number(selectedBinId.value) : 0;
    let args = {
        goodsId: item.goodsId,
        pgSize: pgSize.value,
        pgIndex: pgIndex.value,
        orderField: orderField,
        orderType: orderType,
        selectedGoodsGroup: selectedGoodsGroup.value,
        selectedGoodsClassifyId: selectedGoodsClassifyId.value,
        selectedWarehouseId: selectedWarehouseId.value,
        selectedShelfId: selectedShelfId.value,
        binId: binId,
        searchKey: searchKey.value
    }
    router.push({ name: 'goodsdetail-mobile', query: { goodsId: item.goodsId } }).then(() => {
        permission.addRouteHis(route.name as string);
        commonHelper.setObjLocalStorage(storageKey, args);
    });
}

const onmessageinfo = () => {
    msgDrawerOptions.value.show = true;
    msgDrawerOptions.value.type = 'Message'
    msgDrawerOptions.value.title = '原材料入库提示';
    msgDrawerOptions.value.message = `是否确认入库？`;
}

const onLockBinCancel = () => {
    msgDrawerOptions.value.show = false;
}

const onSubmit = async () => {
    // 基本校验
    if (!selectedGoodsGroup.value) {
        msg.deftAuto('请选择大类')
        return
    }
    if (!selectedGoodsClassifyId.value) {
        msg.deftAuto('小类不能为空')
        return
    }
      if (!selectedGoods.value) {
        msg.deftAuto('物品名称不能为空')
        return
    }
      if (!storageDate.value) {
        msg.deftAuto('请选择入库时间')
        return
    }

    if (!selectedWarehouseId.value) {
        msg.deftAuto('仓库不能为空')
        return
    }
    if (!selectedShelfId.value) {
        msg.deftAuto('货架不能为空')
        return
    }
    if (!selectedBinId.value) {
        msg.deftAuto('货位不能为空')
        return
    }
    if (!quantity.value || quantity.value <= 0) {
        msg.deftAuto('请输入正确的入库数量')
        return
    }

    const warehouse = warehouseData.value.find(w => w.warehouseId === selectedWarehouseId.value);
    const warehouseName = warehouse ? warehouse.warehouseName : '';
    const binid = binData.value.find(b => b.id === selectedBinId.value);
    const binName = binid ? binid.name : '';
    const binNo = binid ? binid.no : '';

    const createUserName = permission.getOperator().userName;
    const userId = permission.getOperator().userId;
    const detail: DetailItem = {
        goodsId: selectedGoods.value.goodsId,
        goodsNo: selectedGoods.value.goodsNo,
        goodsName: selectedGoods.value.goodsName,
        goodsFullName: selectedGoods.value.goodsName,
        goodsClassifyId: selectedGoodsClassifyId.value!,
        goodsClassifyName: selectedGoods.value.goodsClassifyName,
        // goodsClassify: selectedGoodsGroup.value,
        goodsUnitList: selectedGoods.value.goodsUnitList || [],
        unitId: selectedGoods.value.packageUnitId,
        unitName: selectedGoods.value.packageUnitName,
        warehouseId: selectedWarehouseId.value,
        // warehouseName: binName,
        shelfId: selectedShelfId.value,
        binId: Number(selectedBinId.value),
        binNo: binNo,
        binName: '',
        minimumContainer: binName,//???
        quantity: quantity.value,
        totalPrice: 0,
        type: 'Bin',
        createUserId: userId,
        createUserName: createUserName,
    }

    dataForm.value.details = [detail]
    dataForm.value.createUserId = userId;
    dataForm.value.createUserName = createUserName;
    dataForm.value.warehouseName = binName;
    dataForm.value.goodsClassify = selectedGoodsGroup.value,
    dataForm.value.goodsPicture = selectedGoods.value.goodsPicture || [];
    dataForm.value.remark = remark.value;
    dataForm.value.createDate = (document.getElementById('input-storageDate') as HTMLInputElement).value.trim();
    dataForm.value.transportOrderNo = (document.getElementById('input-transportOrderNo') as HTMLInputElement).value.trim();
    dataForm.value.licensePlateNo = (document.getElementById('input-licensePlateNo') as HTMLInputElement).value.trim();
    submitLoading.value = true
    try {
        // const payload = JSON.parse(JSON.stringify(dataForm.value))
        const response = await addInStorage(dataForm.value)
        const orderNo = response.data as string;
        await confirmInStorage(orderNo);
        // msg.successAuto('入库已确认');
        msg.successAuto('入库成功')
        msgDrawerOptions.value.show = false
    } catch (err) {
        msg.errorAuto('入库失败，请重试')
    } finally {
        submitLoading.value = false
    }
}


</script>

<style lang="scss" scoped>
@media screen and (max-width: 450px) {
    .app-content {
        height: 88%;
        background-color: #fff;

        .content-main {
            padding: 0px 30px 0 25px;
            height: 85%;
            overflow-y: hidden;

            .content-form {
                .input-item {
                    font-size: 14px;
                    color: #535353;
                    margin-bottom: 12px;

                    .input-cls {
                        width: 100%;
                        height: 26px;
                        line-height: 26px;
                        border: none;
                        outline: none;
                        background-color: transparent;
                        border-bottom: 1px solid #dfdfdf;
                        text-align: center;
                        position: relative;
                        bottom: -17px;
                        font-size: 14px;
                    }

                    .input-title {
                        color: #8b8b8b;
                        width: 100%;
                        position: relative;
                        text-align: left;
                        bottom: -24px;
                        padding-bottom: 5px;
                        border-bottom: 1px solid #dfdfdf;
                    }

                    .input-icon {
                        position: relative;
                        bottom: -71%;
                        left: 19%;
                        text-align: right;
                        border-bottom: 1px solid #dfdfdf;
                        padding-bottom: 1px;
                    }
                }
            }

            .content-desc {
                font-size: 11px;
                color: #a6a5a5;
                height: 30%;
                text-align: left;
                padding: 0 10px;
            }
        }

        .content-btn {
            height: 10%;

        }

        .el-button--success {
            margin-top: 50px;
        }
    }
}

@media screen and (min-width: 450px) {
    .app-content {
        height: 88%;
        background-color: #fff;

        .content-main {
            padding: 0px 30px 0 25px;
            height: 85%;
            overflow-y: hidden;

            .content-form {
                .input-item {
                    font-size: 14px;
                    color: #535353;
                    margin-bottom: 12px;

                    .input-cls {
                        width: 100%;
                        height: 26px;
                        line-height: 26px;
                        border: none;
                        outline: none;
                        background-color: transparent;
                        border-bottom: 1px solid #dfdfdf;
                        text-align: center;
                        position: relative;
                        bottom: -17px;
                        font-size: 14px;

                    }

                    .input-title {
                        color: #8b8b8b;
                        width: 100%;
                        position: relative;
                        text-align: left;
                        bottom: -24px;
                        padding-bottom: 5px;
                        border-bottom: 1px solid #dfdfdf;
                    }

                    .input-titlenone {
                        color: #8b8b8b;
                        width: 100%;
                        position: relative;
                        text-align: left;
                        bottom: -24px;
                        padding-bottom: 5px;

                    }

                    .input-icon {
                        width: 110%;
                        position: relative;
                        bottom: -71%;
                        left: 1%;
                        text-align: right;
                        border-bottom: 1px solid #dfdfdf;
                        padding-bottom: 1px;
                    }
                }
            }

            .content-desc {
                font-size: 11px;
                color: #a6a5a5;
                height: 30%;
                text-align: left;
                padding: 0 10px;
            }
        }

        .content-btn {
            height: 10%;

            button {
                height: 35px;
            }
        }
    }
}

.input-cls {
    width: 100%;
    height: 26px;
    line-height: 26px;
    border: none;
    outline: none;
    background-color: transparent;
    border-bottom: 1px solid #dfdfdf;
    text-align: center;
    font-size: 14px;

    // 让 el-select、el-input 看起来也像 input
    :deep(.el-input__wrapper) {
        box-shadow: none !important;
        border-radius: 0 !important;
        padding: 0 !important;
        background: transparent;
        border-bottom: 1px solid #dfdfdf;
    }

    :deep(.el-input__inner) {
        text-align: center;
        height: 26px;
        line-height: 26px;
        font-size: 14px;
    }
}
</style>