<template>
    <div class="content">
        <div class="content-body"  v-for="(item,index) in ordersFrom">
            <el-descriptions  :title="`成品交接单（${item.detailNo}）第${item.carRank}车`"  direction="vertical" :column="2" border>
                <el-descriptions-item label="发货型号" :span="2">
                <img :id="`consignNum${item.detailId}`"> 
                <span v-if="!isConsignNum" class="text-warning">{{ barcodeMsg }}</span>
            </el-descriptions-item>
            <el-descriptions-item label="生产订单号" :span="2">
                <img  :id="`deliverNo${item.detailId}`">
                <span v-if="!isDeliverNoCode" class="text-warning">{{ barcodeMsg }}</span>
            </el-descriptions-item>
            <el-descriptions-item label="小车唯一码" :span="2">
                <img  :id="`carSoleCode${item.detailId}`"> 
                <span v-if="!isCarSoleCode" class="text-warning">{{ barcodeMsg }}</span>
            </el-descriptions-item>
            <el-descriptions-item label="产品型号">{{ item.prodctionTypeNo }}</el-descriptions-item>
            <el-descriptions-item label="生产数量" >{{ item.total }}</el-descriptions-item>
            <el-descriptions-item label="装车数量" >{{ item.totalByCar }}</el-descriptions-item>
            <el-descriptions-item label="小车配对数" >{{item.matchingCount }}</el-descriptions-item>
            <el-descriptions-item label="需求车次" >{{ item.countByCar }}</el-descriptions-item>
            <el-descriptions-item label="生产产线" >{{ item.line }}</el-descriptions-item> 
            <el-descriptions-item label="备注" :span="2">
            <el-tag size="small">i</el-tag>
            {{ item.remark }}
            </el-descriptions-item> 
            </el-descriptions> 
        </div>  
        <div class="content-footer" v-if="!isprint">
            <el-button class="btn" @click="onReturn">返回</el-button>
            <el-button class="btn" type="primary" @click="onPrint($event)">打印选项</el-button>
        </div>
    </div>
</template>
<script lang="ts" setup>
import { ref} from "vue"; 
import { useRoute, useRouter} from "vue-router";
import { getOrderDetail,updatePrintDate} from "@/api/production/productionOrder";
import msg from '@/utils/system/message'
import JsBarcode from 'jsbarcode' 

const route = useRoute();
const ordersFrom = ref(new Array<any>()); 
const isprint=ref(false);
const isDeliverNoCode=ref(false);
const isCarSoleCode=ref(false);
const isConsignNum=ref(false);
const barcodeMsg=ref('正在绘制条形码...');
const createBarcode=(img:string, value:string)=>{ 
    JsBarcode("#"+img,value,{
            format:"CODE128",//条形码的格式
            width:1,//线宽
            height:40,//条码高度
            lineColor:"#000",//线条颜色
            displayValue:true,//是否显示文字
            margin:5,//设置条形码周围的空白区域
        })
}
 

getOrderDetail(route.query.deliverNo as string).then((res:any)=>{ 
    res.data.details.forEach((item:any) => { 
        ordersFrom.value.push({
            orderId:res.data.orderId,
            deliverNo:res.data.deliverNo,
            consignNum:res.data.consignNum,
            prodctionTypeNo:res.data.prodctionTypeNo,
            productName:res.data.productName, 
            matchingCount:res.data.matchingCount+'车',
            total:res.data.total+res.data.unitName,
            totalByCar:item.planTotalByCar+res.data.unitName,
            countByCar:res.data.countByCar+'车',
            unitName:res.data.unitName,
            line:res.data.line,  
            createUser:res.data.createUser, 
            remark:res.data.remark,  
            detailId:item.detailId,
            detailNo:item.detailNo,
            carSoleCode:item.carSoleCode,
            carRank:item.carRank 
        } ); 
    });
});

setTimeout(() => { 
    ordersFrom.value.forEach((item:any) => {
        isConsignNum.value=true;
        isDeliverNoCode.value=true; 
        isCarSoleCode.value=true;
        createBarcode('consignNum'+item.detailId,item.consignNum); 
        createBarcode('deliverNo'+item.detailId,item.deliverNo); 
        createBarcode('carSoleCode'+item.detailId,item.carSoleCode); 
    }); 
}, 1000);

const onPrint=(e:any)=>{
    isprint.value=true;
    e.target.style.display="none";
    setTimeout(() => {
        window.print();
        updatePrintDate(ordersFrom.value[0].orderId).then((res:any)=>{
           
        })
    }, 500);
}

const onReturn=()=>{
    history.go(-1)
}

</script>
<style scoped lang="scss">
@page {
size: auto;
margin: 0mm;
}
.content{
    padding:2% 3% 0 3%; 
    position: relative;
    .content-body{ 
        height: calc(100vh);
        background-color: #fff; 
        overflow: auto; 
    }
    .content-footer{  
        position: fixed;
        bottom: 2%; 
        left: 50%;   
    }
}
</style>