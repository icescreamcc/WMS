<template> 
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
      
      </div>
      <div class="layout-container-form-search"> 
        <el-date-picker
          v-model="selectedMonth"
          type="month"
          size="small"
          placeholder="请选择月份" 
          style="margin-right:10px;width:90%"
          @change="getTakeStockHisTableData(true)"
        />
        <el-select v-model="selectedGoodsGroup"  size="small" class="m-2" style="width:100%;margin-right:10px;" @change="getGoodsClassifyData" placeholder="请选择物品大类 *必填">
          <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">大类</div></template>        
          <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
          </el-select>
        <el-select v-model="selectedGoodsClassifyId"  size="small" class="m-2" style="width:100%;margin-right:10px;" @change="getTakeStockHisTableData(true)"  placeholder="请选择物品小类">
          <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 30px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">小类</div></template>    
                      <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
              </el-select>
        <el-input v-model="queryTakeStockByGoods.input" placeholder="请输入关键词进行检索" size="small"></el-input>
        <el-button type="primary" icon="el-icon-search" class="search-btn" @click="getTakeStockHisTableData(true)">搜索</el-button>  
        <el-button v-if="permission.isPermisstion('TAKESTOCKEXPORT')" icon="el-icon-download" :loading="exportBtnLoading" style="margin-left:20px"  type="info" @click="exportData">导出</el-button>
      </div>
    </div>
    <div class="layout-container-table">
      <TableServer   v-model:page="pageTakeStockByGoods" v-loading="takeStockByGoodsTableLoading"  :data="takeStockByGoodsTableData" 
          @getTableData="getTakeStockHisTableData"  
          @orderChanged="handleTakeStockByGoodsSortChange"> 
          <el-table-column prop="goodsName" label="名称"   align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
          <el-table-column prop="goodsModel" label="型号"   align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
          <el-table-column prop="supplier" label="供应商"   align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
          <el-table-column prop="typeName" label="分类"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/>
          <el-table-column prop="warehouseName" label="仓库"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/> 
          <el-table-column prop="shelfName" label="货架"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/> 
          <el-table-column prop="binName" label="货位"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/> 
          <el-table-column prop="cellNo" label="料箱"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/> 
          <el-table-column prop="quantity" label="数量"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="scope">
              <span>{{scope.row.quantity+scope.row.unitName}}</span>
            </template>
          </el-table-column>
          <el-table-column prop="flowType" label="盈亏"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="scope">
              <span v-if="scope.row.quantity==0">盘平</span>
              <span v-else-if="scope.row.flowType=='In'">盘盈</span>
              <span v-else>盘亏</span>
            </template>
          </el-table-column> 
          <el-table-column prop="unitPrice" label="单价"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="scope">
              <span>{{scope.row.unitPrice}}{{ scope.row.priceUnit }}</span>
            </template>
          </el-table-column>  
          <el-table-column prop="totalPrice" label="总价"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="scope">
              <span>{{scope.row.totalPrice}}{{ scope.row.priceUnit }}</span>
            </template>
          </el-table-column>
          <el-table-column prop="operateDate" label="盘点时间"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="scope">
              <span>{{commonHelper.formatToDateTime(scope.row.operateDate)}}</span>
            </template>
          </el-table-column>  
          <el-table-column prop="operatorName" label="盘点人员"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true"/> 
          <el-table-column prop="remark" label="原因分析"  align="center" sortable="custom" min-width="120" :show-overflow-tooltip="true">
            <template #default="scope">
              <span v-if="permission.isPermisstion('TAKESTOCKUPDATEREASON')" >
                <span v-if="scope.row.remark" style="cursor: pointer;" @click="onEditReason(scope.row)">{{scope.row.remark}}</span>
                <el-icon v-else @click="onEditReason(scope.row)" title="点击编辑原因" style="cursor: pointer;margin-left: 5px;color: #1dcc09;position: relative;bottom: -2px"><EditPen /></el-icon>
              </span>
              <span v-else>{{scope.row.remark}}</span> 
            </template>
          </el-table-column>
        </TableServer> 
    </div> 
    <ReasonEditModal :layer="reasonEditOption"  @dataSubmit="submitReason" v-if="reasonEditOption.show"/>
  </div>
</template>

<script lang="ts" setup>
  defineOptions({
    name: "takestock"
  })
import { ref, reactive,onMounted,onBeforeMount,h, shallowRef} from "vue";  
import {getTakeStockHis,exportTakeStockHis,getOptions,updateReason,getTaskStockDetil} from "@/api/inv/takestock";
import {getGoodsGroup,getGoodsClassify} from '@/api/common'; 
import TableServer from "@/components/table/tableServer.vue"; 
import { Page } from "@/components/table/type"; 
import commonHelper from "@/utils/system/common-helper"; 
import {EditPen}  from '@element-plus/icons-vue';
import { useRouter, useRoute } from 'vue-router'; 
import { deftClassifyGroup } from '@/config';
import permission from '@/utils/system/permission';
import { LayerInterface } from "@/components/layer/index.vue";
import ReasonEditModal from './reasonLayer.vue'
import option from "@/views/home/components/charts-option/bar_3d";

const router = useRouter();

const goodsGroupData=ref(new Array<any>());

const selectedGoodsGroup=ref(deftClassifyGroup);

const goodsClassifyData=ref(new Array<any>());

const selectedGoodsClassifyId=ref(0);

const selectedMonth=ref(new Date());

const queryTakeStockByGoods = reactive({
      input: "",
    }); 

const exportBtnLoading=ref(false);

const reasonTypeOptionData=ref(new Array<any>());

const reasonEditOption: LayerInterface = reactive({
        show: false,
        title: "原因分析",
        showButton: true,
        btnLoading:false,
        width:"40%",
        data:null , 
        options:null,
        otherButton:{}
      });
       
onMounted(()=>{
  getGoodsGroupData();
  getGoodsClassifyData();
  getTakeStockHisTableData(true); 
  getOptions().then(res=>{
        reasonTypeOptionData.value=res.data.reasonTypeOptions; 
      })
})
  
const getGoodsGroupData=()=>{
  getGoodsGroup().then(res=>{
    if(deftClassifyGroup=='SparePart'){
        goodsGroupData.value=res.data.filter((f:any)=>f.key=='SparePart'||f.key=='Consumables');
      }
      else{
        goodsGroupData.value=res.data;
      }  
  })
} 

const getGoodsClassifyData=()=>{
  getGoodsClassify(selectedGoodsGroup.value).then(res=>{
    goodsClassifyData.value=[{key:0,value:'All'},...res.data];
    getTakeStockHisTableData(true);
  })
}

//物品盘点
const pageTakeStockByGoods: Page = reactive({
      index: 1,
      size: 20,
      total: 0,
      orderField:'',
      orderType:''
    }); 
 const takeStockByGoodsTableLoading = ref(false);
 const takeStockByGoodsTableData = ref([]);    
     
  const handleTakeStockByGoodsSortChange=(orderRow:any)=>{   
    getTakeStockHisTableData(true);
  }
 
  const onEditReason=(row:any)=>{ 
    getTaskStockDetil(row.flowId).then(res=>{
      reasonEditOption.data=res.data;
      reasonEditOption.options=reasonTypeOptionData.value;
      reasonEditOption.show=true;
    }) 
  }

  const submitReason=(data:any)=>{
    reasonEditOption.btnLoading=true;
    updateReason(data).then(()=>{
      reasonEditOption.show=false;
      getTakeStockHisTableData(false);
    }).finally(()=>reasonEditOption.btnLoading=false)
  }

  const getTakeStockHisTableData = (init: Boolean) => { 
    if (init) {
      pageTakeStockByGoods.index = 1
    }   
    takeStockByGoodsTableLoading.value = true; 
    let curMonth=commonHelper.formatToCustomDate(selectedMonth.value,'yyyyMM');
    getTakeStockHis( pageTakeStockByGoods.size,pageTakeStockByGoods.index,pageTakeStockByGoods.orderField,pageTakeStockByGoods.orderType,selectedGoodsGroup.value,selectedGoodsClassifyId.value, Number(curMonth),queryTakeStockByGoods.input)
      .then((res) => { 
        let data = res.data.rows
        data.forEach((d: any) => {
          d.workbinTableLoading = false
        })
        takeStockByGoodsTableData.value = data
        pageTakeStockByGoods.total = Number(res.data.total);
      })
      .catch((error) => {
        takeStockByGoodsTableData.value = [];
        pageTakeStockByGoods.index = 1;
        pageTakeStockByGoods.total = 0;
      })
      .finally(() => {
        takeStockByGoodsTableLoading.value = false;
      });
  }
  
  const exportData=()=>{  
      exportBtnLoading.value=true;
      let curMonth=commonHelper.formatToCustomDate(selectedMonth.value,'yyyyMM');
      exportTakeStockHis(pageTakeStockByGoods.orderField,pageTakeStockByGoods.orderType,selectedGoodsGroup.value,selectedGoodsClassifyId.value,Number(curMonth),queryTakeStockByGoods.input).then(res=>{ 
            let link = document.createElement('a') 
            link.style.display = 'none'
            link.href =res.data
            document.body.appendChild(link)
            link.click() 
            document.body.removeChild(link)
        }).finally(()=>exportBtnLoading.value=false);
    }
  
</script>

<style lang="scss" scoped> 
.workbin-cell{
  background-color: #409eff;
  border-right: 1px solid #fff;
  border-bottom: 1px solid #fff;
  color: #fff; 
  font-size: xx-small;
  text-align: center;
}
</style>
