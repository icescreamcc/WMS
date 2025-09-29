<template>
    <div class="layout-container">
      <el-tabs v-model="tabsName" @tab-click="onTabsChanged">
        <el-tab-pane label="标签打印" name="print" :style="{height:tbHeight+'px'}">
            <template #label>
                <span class="custom-tabs-label">
                 <img src="../../../../public/icon-img/dayinji.png">
                 <span>标签打印</span>
                </span>
            </template> 
            <el-row class="tab-print" :gutter="20">
                    <el-col :span="12">
                        <el-row :gutter="10" style="height: 35px;">
                            <el-col :span="12">
                                <el-select v-model="selectedGoodsGroup" :disabled="disableClassifyGroupSelect" size="small"  class="m-2" style="width:100%;margin-right:10px;" @change="onClassifyChanged" >
                                <template #prefix>
                                    <div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">标签分类</div>
                                </template>        
                                <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key">
                                    </el-option>
                                </el-select>  
                            </el-col>
                            <el-col :span="12">
                                <el-select v-model="selectedLabelId"  size="small"  class="m-2" style="width:100%;margin-right:10px;" @change="onLabelChanged" >
                                <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">标签名称</div></template>        
                                <el-option v-for="item in labelListData" :key="item.labelId" :label="item.labelName" :value="item.labelId">
                                    </el-option>
                                </el-select>  
                            </el-col> 
                        </el-row>
                        <div>
                            <el-card shadow="always"> 
                                <div v-if="selectedLabelInfo" class="label-content" :style="{width:standardLayoutWidth+'mm',height:standardLayoutHeight+'mm'}">
                                    <div class="label-background" :style="{width:selectedLabelInfo.widthScale+'mm',height:selectedLabelInfo.heightScale+'mm',backgroundColor:selectedLabelInfo.backgroundColor}">
                                        <div class="label-item" v-for="item in selectedLabelInfo.details">
                                        <div v-if="item.isUsed&&item.itemType=='Text'" :style="{color:item.args.color,fontFamily:item.args.font,fontSize:item.args.fontSize+'px',fontWeight:item.args.weight?'bold':'normal',position:'absolute',left:item.style.leftPercent+'%',top:item.style.topPercent+'%', whiteSpace:'nowrap',overflow:'hidden'}">{{item.showName? `${item.itemName}：`:'' }}{{item.deftValue||'xxxxxx'}}</div>
                                        <div v-else-if="item.isUsed&&item.itemType=='QRCode'" :style="{height:item.args.height+'mm',width:item.args.width+'mm',position:'absolute',left:item.style.leftPercent+'%',top:item.style.topPercent+'%'}"> 
                                            <canvas v-if="item.args.format=='DataMatrix'" :id="selectedLabelInfo.labelId+'_'+item.itemName" style="width: 100%;height: 100%;"></canvas>
                                            <VueQr v-else-if="item.args.format=='QRCode'" :id="selectedLabelInfo.labelId+'_'+item.itemName" :text="(selectedLabelInfo.labelId+'_'+item.itemName).toUpperCase()" :size="item.args.width" :dotScale="1" :margin="0"></VueQr>
                                        </div>
                                        <div v-else-if="item.isUsed&&item.itemType=='BarCode'" :style="{height:item.args.height+'mm',position:'absolute',left:item.style.leftPercent+'%',top:item.style.topPercent+'%'}">
                                            <img :id="selectedLabelInfo.labelId+'_'+item.itemName">
                                        </div>
                                        </div>
                                    </div> 
                                </div>
                                <el-row v-else>
                                    <el-col><el-empty description="请选择一个标签模板" /></el-col>
                                </el-row>
                            </el-card>
                        </div>
                    </el-col>
                    <el-col :span="12">
                        <div style="margin-top: 35px;">
                            <el-card shadow="always">
                                <el-descriptions  :column="2" border> 
                                    <el-descriptions-item label="物料名称" :span="2"></el-descriptions-item>
                                    <el-descriptions-item label="物料型号"></el-descriptions-item>
                                    <el-descriptions-item label="SAP编码"></el-descriptions-item>
                                    <el-descriptions-item label="标准包装数量"></el-descriptions-item> 
                                    <el-descriptions-item label="打印数量" >
                                        <el-input-number  :min="1" />
                                    </el-descriptions-item> 
                                    <el-descriptions-item label="请选择对应的物料">
                                        <img src="../../../../public/icon-img/qingxuanze.png" @click="onShowGoodsDrawer" title="点击打开物料列表" class="btn-select">
                                    </el-descriptions-item> 
                                </el-descriptions>
                            </el-card> 
                        </div>
                    </el-col>
                </el-row>  
        </el-tab-pane>
        <el-tab-pane label="打印记录" name="record" :style="{height:tbHeight+'px'}">
            <template #label>
                <span class="custom-tabs-label">
                 <img src="../../../../public/icon-img/xiaoxijilu.png">
                 <span>打印记录</span>
                </span>
            </template>
         <div class="layout-container-form-search">
            <el-select v-model="selectedGoodsGroup" :disabled="disableClassifyGroupSelect" size="small"  class="m-2" style="width:100%;margin-right:10px;" @change="onClassifyChanged" placeholder="选择物品大类">
                <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px; background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">分类</div></template>        
                <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key">
                        </el-option>
                </el-select>
            <el-date-picker
                v-model="dateRange"
                type="daterange"
                range-separator="-"
                start-placeholder="最早日期"
                end-placeholder="最晚日期"
                size="small"
                value-format="YYYY-MM-DD"
                style="margin-right:10px;width:100%"
            >
            </el-date-picker>
                <el-input 
                v-model="query.input"
                placeholder="请输入关键词进行检索"
                size="small"
                ></el-input>
                <el-button
                type="primary"
                icon="el-icon-search"
                class="search-btn"
                @click="getTableData(true)"
                >搜索</el-button>   
            </div>
            <Table ref="table"  v-model:page="page" v-loading="loading"  :data="tableData"  @getTableData="getTableData"   @orderChanged="getTableData">  
              <el-table-column prop="packageId" label="包装ID" align="center" sortable="custom" :show-overflow-tooltip="true"/> 
              <el-table-column prop="goodsClassifyGroup" label="分类" align="center" sortable="custom" :show-overflow-tooltip="true"/>   
              <el-table-column prop="goodsId" label="物料ID" align="center" sortable="custom" :show-overflow-tooltip="true"/>   
              <el-table-column prop="goodsName" label="物料名称" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>  
              <el-table-column prop="goodsModel" label="物料型号" align="center" sortable="custom" :show-overflow-tooltip="true"/>
              <el-table-column prop="goodsNo" label="SAP编码" align="center" sortable="custom" :show-overflow-tooltip="true"/>  
              <el-table-column prop="goodsNo" label="标准包装数量" align="center" sortable="custom" :show-overflow-tooltip="true">
                <template #default="scope">
                    <span>{{scope.row.packageCount+scope.row.packageUnitName}}</span> 
                </template>
              </el-table-column>  
              <el-table-column  :label="$t('message.common.handle')" align="center"  min-width="200">
                <template #default="scope"> 
                  <!-- <el-button @click="showStorageDetails(scope.row)" type="plain">仓储明细</el-button>  -->
                </template>
              </el-table-column>
          </Table>
        </el-tab-pane>
      </el-tabs> 
      <GoodsSelectDrawer :options="goodsDrawerOptions" v-if="goodsDrawerOptions.show" @selectItem="onSelectGoods"/>
    </div>
</template>
<script lang="ts" setup>
  defineOptions({
    name: "label-print"
  })
import { ref, reactive,onMounted,onBeforeMount,nextTick } from "vue";
import { Page } from "@/components/table/type";
import permission from '@/utils/system/permission';
import msg from "@/utils/system/message";
import {getGoodsGroup,getGoodsByKeyAndClassify} from '@/api/common';  
import {getLabels,addPrintRecord,getLabelRecord,getLableDesignDetails  } from "@/api/baseinfo/labelPrint";
import { deftClassifyGroup ,disableClassifyGroupSelect} from '@/config';
import Table from "@/components/table/tableServer.vue";
import {Printer} from '@element-plus/icons-vue';
import GoodsSelectDrawer from '@/components/drawer/goodsSelector.vue';
import JsBarcode from 'jsbarcode';    
import VueQr from 'vue-qr/src/packages/vue-qr.vue';
import * as bwipjs from 'bwip-js';

const tabsName=ref('print');
const tbHeight=ref(0);
const query = reactive({
      input: "",
 }); 
const dateRange=ref()  
const goodsGroupData=ref(new Array<any>()); 
const selectedGoodsGroup=ref(deftClassifyGroup); 
const selectedPrintGoods=ref();
const page: Page = reactive({
    index: 1,
    size: 20,
    total: 0,
    orderField:'',
    orderType:''
});
const goodsDrawerOptions= ref({
  show: false,
  title: '', 
  type:'',
  mode:'repet',
  isMultiSelect:false,
  data:new Array<any>() 
});
const loading = ref(true);
const tableData = ref([]); 
const labelListData=ref(new Array<any>());
const selectedLabelId=ref('');
const selectedLabelInfo=ref(); 
const standardLayoutWidth=ref(100);
const standardLayoutHeight=ref(50);

onBeforeMount(()=>{
    tbHeight.value=window.innerHeight-260;
});

onMounted(()=>{
    getGoodsGroupData();
    getLabelData();
})

const getGoodsGroupData=()=>{
    getGoodsGroup().then(res=>{
        goodsGroupData.value=res.data; 
    })
}

const onShowGoodsDrawer=()=>{ 
  if(!selectedGoodsGroup.value){
    msg.deftAuto('请先选择标签分类');
    return;
  }
  goodsDrawerOptions.value.show=true;
  goodsDrawerOptions.value.type=selectedGoodsGroup.value;
  let groupDesc= goodsGroupData.value.find(f=>f.key==selectedGoodsGroup.value).value;
  goodsDrawerOptions.value.title=groupDesc+'选择';
  goodsDrawerOptions.value.data=selectedPrintGoods.value;
}

const onSelectGoods=(selectedGoods:any)=>{      
    selectedPrintGoods.value=selectedGoods;
}

const onTabsChanged=()=>{ 
}

const onClassifyChanged=()=>{
    getLabelData();
}

const getLabelData=()=>{
    if(selectedGoodsGroup.value){
        getLabels(selectedGoodsGroup.value).then(res=>{
            labelListData.value=res.data;
        })
    } 
}

const onLabelChanged=()=>{
    selectedLabelInfo.value=labelListData.value.find(f=>f.labelId==selectedLabelId.value); 
    layoutContentEqualScaling(selectedLabelInfo.value); 
    getLableDesignDetails(selectedLabelInfo.value.labelId).then(res=>{
        selectedLabelInfo.value.details=res.data;
        parseDetailArgs(selectedLabelInfo.value.details);
        itemContentEqualScaling(selectedLabelInfo.value.widthScalingRatio,selectedLabelInfo.value.heightScalingRatio,selectedLabelInfo.value.details);
        nextTick(()=>{
            selectedLabelInfo.value.details.forEach((item:any) => {
                setBarcode( selectedLabelInfo.value.labelId,item);
            });
        }) 
    })
}
 
const layoutContentEqualScaling=(label:any)=>{
    label.widthScale=label.width;
    label.heightScale=label.height;
    let whRatio=label.width/label.height;
    if(label.width>standardLayoutWidth.value){
      label.widthScale=standardLayoutWidth.value;
      label.heightScale=label.widthScale/whRatio; 
    }
    else if(label.height>standardLayoutHeight.value){
      label.heightScale=standardLayoutHeight.value;
      label.widthScale= label.heightScale*whRatio;
    } 
    label.widthScalingRatio=label.widthScale/label.width;
    label.heightScalingRatio=label.heightScale/label.height;
  }

  const parseDetailArgs=(details:Array<any>)=>{
    details.forEach((detail:any)=>{
      detail.args=JSON.parse(detail.args);
      if(detail.itemValueType=='BindingField'){
        detail.itemValueField=JSON.parse(detail.itemValueField); 
      } 
      detail.itemValueFieldInfo=JSON.parse(detail.itemValueFieldInfo);
      detail.style=JSON.parse(detail.style); 
    });
  }

  const itemContentEqualScaling=(widthScalingRatio:number,heightScalingRatio:number,details:Array<any>)=>{
    details.forEach((item:any)=>{ 
      if(item.itemType=='BarCode'){
     
      }
      else if(item.itemType=='QRCode'){
        item.args.widthScale=item.args.width*widthScalingRatio;
        //item.args.heightScale=item.args.height*heightScalingRatio;
        item.args.heightScale=item.args.widthScale;
      }
      item.style.leftPercentScale=item.style.leftPercent*widthScalingRatio;
      item.style.topPercentScale=item.style.topPercent*heightScalingRatio;
    }); 
  }

  const setBarcode=(labelId:string,item:any)=>{
    if(item.itemType=='BarCode'){
        let barcodeArgs=item.args;
        let barcodeDomId=`${labelId}_${item.itemName}`;
        let deftText=item.deftValue||barcodeDomId.toLocaleUpperCase(); 
        createBarcode(barcodeDomId,deftText,barcodeArgs.format,barcodeArgs.width,barcodeArgs.height,barcodeArgs.lineColor,barcodeArgs.displayValue,barcodeArgs.font,barcodeArgs.fontSize,barcodeArgs.textMargin);
    }
    else if(item.itemType=='QRCode'){
        let qrcodeArgs=item.args;
        if(qrcodeArgs.format=='DataMatrix'){
            let qrcodeId=`${labelId}_${item.itemName}`;
            let deftText=item.deftValue||qrcodeId.toLocaleUpperCase();
            createDataMatrix(qrcodeId,deftText);
        }
        else{ 
        }
    }
 }

  const createBarcode=(imgId:string, value:string,format:string,width:number,height:number,lineColor:string,displayValue:boolean,font:string,fontSize:number,textMargin:number)=>{ 
    JsBarcode("#"+imgId,value,{
        format:format,
        width:width,
        height:height,
        lineColor:lineColor, 
        displayValue:displayValue, 
        margin:0, 
        font:font,
        fontSize:fontSize,
        textMargin:textMargin
    })
 }

const createDataMatrix=(canvasId:string,value:string)=>{
    let opt={
        bcid: 'datamatrix',   
        text: value,
        scale: 3,
        includetext: true,
    };
    bwipjs.toCanvas(canvasId,opt); 
}

const getTableData = (init: Boolean=true) => {
    loading.value = true
    if (init) {
      page.index = 1
    }    
    let dateStart='';
      let dateEnd='';
      if(dateRange.value?.length>0){
        dateStart=dateRange.value[0]
      }
       if(dateRange.value?.length>1){
        dateEnd=dateRange.value[1]
      }
      loading.value = false;
      getLabelRecord(page.size,page.index,page.orderField,page.orderType,selectedGoodsGroup.value,query.input,dateStart,dateEnd)
      .then((res) => {
        let data = res.data.rows
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
</script>
<style lang="scss" scoped>
.tab-print{
    cursor: default;
}
.label-content{
    border: 1px solid #888;
    background-color: rgb(238, 238, 238);
    position: relative; 
    border-radius: 3px; 
    cursor: pointer;
    .label-background{ 
    overflow: hidden;
    position: absolute;
    left: 50%; 
    top:50%;
    transform: translate(-50%,-50%);
    }
}
.btn-select{
    cursor:pointer;
    height: 25px;
}
.custom-tabs-label{
    img{
        height: 20px;
        margin-bottom: -5px;
        margin-right: 3px;
    } 
}
</style>