<template>
    <div class="app-content">
        <div class="content-main">
            <!-- 扫码入口 -->
            <div class="scan-entry" @click="startScanner">
                <img src="/icon-img/saoma.png" class="scan-icon" />
            </div>

            <!-- 扫码摄像头 -->
            <!-- <div v-show="scanning" class="scan-box">
                <div id="qr-reader" class="qr-reader"></div>
                <el-button type="warning" @click="stopScanner" class="btn-cancel">取消</el-button>
                <div class="status">{{ status }}</div>
            </div> -->

            <!-- 扫码摄像头 -->
            <div v-show="scanning" class="scan-box">
                <div id="qr-reader" class="qr-reader"></div>

                <!-- 遮罩 + 中间矩形框 -->
                <div class="scan-overlay">
                    <div class="scan-frame"></div>
                </div>
            </div>
            <div v-if="deliveryItem" class="content-form">
                <el-form label-width="auto" label-position="left" style="padding:0 15px">
                    <el-row>
                        <el-col :span="24">
                            <div>&nbsp;</div>
                        </el-col>
                    </el-row>
                    <el-row>
                        <el-col :span="24">
                            <div>&nbsp;</div>
                        </el-col>
                    </el-row>
                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">发货单号</div>
                        </el-col>
                        <el-col :span="16"><input type="text" class="input-cls" :value="deliveryItem.orderNo"
                                disabled></el-col>
                    </el-row>

                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">物品名称</div>
                        </el-col>
                        <el-col :span="16"><input type="text" class="input-cls" :value="deliveryItem.goodsName"
                                disabled></el-col>
                    </el-row>

                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">计划出库数量</div>
                        </el-col>

                        <el-col :span="16">
                            <el-input v-model="deliveryItem.quantity" type="number" class="input-cls" readonly>
                                <template #append>{{ deliveryItem.packageUnitName }}</template>
                            </el-input>
                        </el-col>
                        <!-- <el-col :span="16">
                            <input type="number" class="input-cls" :value="deliveryItem.quantity"
                                disabled> <template #append>{{ deliveryItem.packageUnitName }}</template>
                        </el-col> -->

                    </el-row>

                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-title">实际出库数量</div>
                        </el-col>
                        <el-col :span="16">
                            <el-input v-model="deliveryItem.actualQuantity" type="number" class="input-cls">
                                <template #append>{{ deliveryItem.packageUnitName }}</template>
                            </el-input>
                        </el-col>
                    </el-row>
                    <el-row>
                        <el-col :span="24">
                            <div></div>
                        </el-col>
                    </el-row>
                    <el-row>
                        <el-col :span="24">
                            <div>&nbsp;</div>
                        </el-col>
                    </el-row>

                    <el-row class="input-item">
                        <el-col :span="8">
                            <div class="input-titlenone">签字附件</div>
                        </el-col>
                        <el-col :span="16" class="upload-col">
                            <Upload :uploadParams="uploadParams" @handleImgChanged="imgChanged" />
                        </el-col>
                    </el-row>

                </el-form>
            </div>

            <div class="content-desc">
                <div style="padding-top: 4%;">
                    <div style="margin-bottom: 1px;">
                        <el-icon>
                            <InfoFilled />
                        </el-icon>
                        <span> 操作流程：</span>
                    </div>
                    <div>
                        <p style="margin-top: 20px;">点右上角扫一扫→输入实际出库数量→上传签字附件→点击确认→出库成功</p>
                    </div>
                </div>
            </div>

            <div class="content-btn">
                <div v-if="permission.isPermisstion('FINISHEDSCAN')">
                    <el-button type="success" round @click="onmessageinfo" style="width:82%;"
                        :loading="submitting">确认</el-button>
                </div>
                <div style="margin-top: 4px;">
                    <el-button round @click="resetForm" style="width:82%;">重置信息</el-button>
                </div>
            </div>
        </div>
        <!-- <div class="content-btn">
            <div v-if="permission.isPermisstion('INSTORAGEORDMOBILERADD')">
                <el-button type="success" round @click="onmessageinfo" style="width:82%;"
                    :loading="submitting">确认</el-button>
            </div>
            <div style="margin-top: 4px;">
                <el-button round @click="resetForm" style="width:82%;">重置信息</el-button>
            </div>
        </div> -->
        <!-- <div class="status">{{ status }}</div> -->
        <!-- <MessageDrawer :options="msgDrawerOptions" @cancel="onLockBinCancel" @confirm="submitDelivery" /> -->
        <!-- <MessageDrawer v-show="msgDrawerOptions.show" :options="msgDrawerOptions" @cancel="onLockBinCancel" @confirm="submitDelivery" /> -->

        <MessageDrawer v-if="msgDrawerOptions.show" :options="msgDrawerOptions" @cancel="onLockBinCancel"
            @confirm="submitDelivery" />
    </div>

</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, nextTick } from 'vue'
import { Html5Qrcode } from 'html5-qrcode'
import { getOrderDetail, ConfirmSendingAndOutStorage } from "@/api/purchase/sendingorder";
import permission from '@/utils/system/permission';
import Upload from '@/components/imgUpload/muiltUpload.vue'
import { ElMessage } from 'element-plus'
import commonHelper from "@/utils/system/common-helper";
import msg from "@/utils/system/message";
import MessageDrawer from '../message-drawer.vue'
import { addOutStorage } from "@/api/inv/outstorage";

const storageKey = "outstoragemobile";
const scanning = ref(false)
const status = ref('未启动')
const html5QrCode = ref<Html5Qrcode | null>(null)
const readerId = 'qr-reader'

const deliveryItem = ref<any | null>(null)
const submitting = ref(false)

const uploadParams = ref({
    uploadApi: '/SendingOrder/UploadInstorgePic',
    limit: 3,
    imgUrlList: [],
    validFileType: 'image',
    validFileSize: 10240,
    isEdit: true,
    titile: '点击上传图片',
    width: '80px',
    height: '80px',
});

const msgDrawerOptions = ref({
    show: false,
    title: '',
    message: '',
    type: '',
    data: null
});


const imgChanged = (imgList: Array<any>) => {
    if (deliveryItem.value) {
        deliveryItem.value.goodsPicture = imgList.map(x => ({
            fileName: x.name,
            url: x.url
        }))
    }
}

const onmessageinfo = () => {
    msgDrawerOptions.value.show = true
    msgDrawerOptions.value.type = 'Message'
    msgDrawerOptions.value.title = '扫码出库提示'
    msgDrawerOptions.value.message = `是否确认出库 ${deliveryItem.value.actualQuantity} 件 ${deliveryItem.value.goodsName}?`
}

const onLockBinCancel = () => {
    msgDrawerOptions.value.show = false;
}

onMounted(() => {
    let queryParamsHis = commonHelper.getObjLocalStorage(storageKey);
    html5QrCode.value = new Html5Qrcode(readerId)

    // PC端调试用默认发货单号
    const decodedText = 'S10000021'
    getOrderDetail(permission.getOperator().userId, decodedText).then(res => {
        deliveryItem.value = res.data[0]
        debugger
        if (deliveryItem.value) deliveryItem.value.actualQuantity = deliveryItem.value.quantity
    })
})

onBeforeUnmount(() => {
    stopScanner()
    commonHelper.setObjLocalStorage(storageKey, null);
    try {
        html5QrCode.value?.clear()
    } catch (e) {
        console.warn("清理扫码组件出错", e)
    }
})

const startScanner = async () => {
    if (!html5QrCode.value) return
    try {
        scanning.value = true
        status.value = '启动摄像头…'
        await html5QrCode.value.start(
            { facingMode: 'environment' }, // 后置摄像头
            {
                fps: 10,
                qrbox: (viewWidth: number, viewHeight: number) => {
                    const isPortrait = viewHeight > viewWidth
                    // 根据屏幕方向动态计算扫码框大小
                    return {
                        width: isPortrait ? viewWidth * 0.9 : viewHeight * 0.9,
                        height: isPortrait ? viewHeight * 0.5 : viewHeight * 0.8,
                    }
                },
                aspectRatio: 1.0, // 保持取景器比例，避免过度拉伸
            },
            onScanSuccess,
            () => { }
        )

        status.value = '请对准二维码'
    } catch (e: any) {
        status.value = '无法启动摄像头：' + (e?.message || e)
        alert(status.value)
        scanning.value = false
    }
}



const stopScanner = async () => {
    if (!html5QrCode.value) return
    try { await html5QrCode.value.stop() } catch { }
    scanning.value = false
    status.value = '已停止'
}

const onScanSuccess = (decodedText: string) => {
    stopScanner()

    status.value = '扫码成功：' + decodedText
    alert('扫码成功：' + status.value);
    getOrderDetail(permission.getOperator().userId, decodedText).then(res => {
        deliveryItem.value = res.data[0] || null;
        if (deliveryItem.value == null) {
            alert('扫码成功!未找到二维码对应出库信息');
        }

        if (deliveryItem.value) deliveryItem.value.actualQuantity = deliveryItem.value.quantity
    })
}

const submitDelivery = async () => {
    // 检查 deliveryItem 是否存在
    if (!deliveryItem.value) {
        ElMessage.error('deliveryItem 未定义');
        return;
    }

    const item = deliveryItem.value;

    // 检查实际出库数量
    if (!item.actualQuantity || item.actualQuantity <= 0) {
        msg.deftAuto('请输入正确的出库数量');
        return;
    }

    // 检查实际出库数量是否超过计划数量
    if (item.actualQuantity > item.quantity) {
        msg.deftAuto('实际出库数量不能大于计划出库数量');
        return;
    }

    // 检查签字附件
    if (!item.goodsPicture || item.goodsPicture.length === 0) {
        msg.deftAuto('请上传签字附件');
        return;
    }


    submitting.value = true;

    try {
        // 构建 FormData 上传文件（如果需要）
        const formData = new FormData();
        formData.append('deliveryNo', item.orderNo);
        item.goodsPicture.forEach((file: any, idx: number) => {
            formData.append('file' + idx, file);
        });

        // 构建发送给后端的对象
        const sendingData = {
            OrderNo: item.orderNo || '',
            GoodsName: item.goodsName || '',
            DetailStatus: item.detailStatus || '',
            Quantity: item.quantity || 0,
            ActualQuantity: item.actualQuantity || 0,
            CreateUserId: permission.getOperator()?.userId || '',
            CreateUserName: permission.getOperator()?.userName || '',
            GoodsPicture: (item.goodsPicture || []).map((p: any) => ({
                FileName: p.fileName || p.name || '',
                Url: p.url || ''
            }))

        };

        // 调用后端接口

        const res = await ConfirmSendingAndOutStorage(sendingData);
        const outStorageData = res.data; // 这里才是后端返回的对象
debugger
        if (outStorageData && outStorageData.details && outStorageData.details.length > 0) {
            // 把 OrderNo 写入每个明细
            outStorageData.details.forEach((d: any) => {
                d.orderNo = outStorageData.orderNo;
            });

            const res = await addOutStorage(outStorageData);
            const orderNo = res.data;
            if (orderNo && orderNo.length > 0) {
                ElMessage.success('发货提交成功！ 单号: ' + orderNo);
                msgDrawerOptions.value.show = false;
                resetForm();
            }
            else {
                ElMessage.error('添加出库记录失败');
                resetForm();
            }
        } else {
            ElMessage.error('提交失败：没有生成出库明细');
        }




        // await ConfirmSendingAndOutStorage(sendingData);
        // const res = await addOutStorage(sendingData);
        // const orderNo = res?.data || '';

        // if (orderNo.length > 0) {
        //     ElMessage.success('发货提交成功！');
        //     msgDrawerOptions.value.show = false; // 隐藏提示框
        //     resetForm(); // 重置表单
        // }

    } catch (err: any) {
        console.error('提交出错：', err);
        ElMessage.error(`提交失败: ${err?.message || err}`);
    } finally {
        submitting.value = false;
    }
};

const showScanEntry = ref(true)
const resetForm = () => {
    deliveryItem.value = null
    scanning.value = false
    status.value = '未启动'
    uploadParams.value.imgUrlList = []
    showScanEntry.value = true   // 重新显示扫码入口
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

/* 扫码入口固定在页面右上角，带阴影和圆角 */
.scan-entry {
    position: fixed;
    top: 50px;
    right: 10px;
    z-index: 999;
    width: 50px;
    height: 50px;
    cursor: pointer;

    background-color: #fff;
    /* 白色背景 */
    border-radius: 50%;
    /* 圆形按钮 */
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.25);
    /* 轻微阴影 */
    display: flex;
    justify-content: center;
    align-items: center;

    .scan-icon {
        width: 70%;
        /* 图标略小于按钮 */
        height: 70%;
    }

    &:active {
        transform: scale(0.95);
        /* 按下缩小效果，增加触控反馈 */
        transition: transform 0.1s;
    }
}

/* 扫码摄像头弹窗 */
.scan-box {
    position: fixed;
    inset: 0;
    /* 全屏 */
    background: black;
    z-index: 2000;
}

.qr-reader {
    width: 100%;
    height: 100%;
    object-fit: cover;
}

/* 遮罩层：中间留出扫码框 */
.scan-overlay {
    position: absolute;
    inset: 0;
    display: flex;
    justify-content: center;
    align-items: center;
    pointer-events: none;
    /* 遮罩层不拦截操作 */
}

// .scan-frame {
//     width: 70vw;
//     height: 70vw;
//     border: 3px solid #00ff00;
//     /* 绿色边框 */
//     border-radius: 8px;
//     box-sizing: border-box;
// }

.status {
    position: absolute;
    bottom: 100px;
    width: 100%;
    text-align: center;
    color: #fff;
    font-size: 16px;
}


.result-card {
    width: 100%;
    max-width: 480px;
    margin-top: 80px;
    /* 原来12px → 改成80px，整体往下移 */
    background: #fff;
    padding: 12px;
    border-radius: 8px;
    box-shadow: 0 1px 4px rgba(0, 0, 0, 0.1);
}
</style>
