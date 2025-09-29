<template>
  <Layer :layer="layer" @confirm="submit" >
     <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
       <el-row>
        <el-col :span="11">
          <el-form-item label="出库仓库" prop="outWarehouseId">
               <el-select v-model="ruleForm.outWarehouseId" class="m-2" style="width:100%" placeholder="请选择出库仓库 *必填">
                      <el-option v-for="item in warehouseData" :key="item.warehouseId" :label="item.warehouseName" :value="item.warehouseId"/> 
              </el-select>
         </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item label="入库仓库" prop="inWarehouseId">
               <el-select v-model="ruleForm.inWarehouseId" class="m-2" style="width:100%" placeholder="请选择入库仓库 *必填">
                      <el-option v-for="item in warehouseData" :key="item.warehouseId" :label="item.warehouseName" :value="item.warehouseId"/>
              </el-select>
         </el-form-item> 
        </el-col>
      </el-row>    
       <el-row>
        <el-col :span="11">
           <el-form-item label="物品类型" prop="goodsClassify">
            <el-select v-model="ruleForm.goodsClassify"  class="m-2" style="width:100%" @change="onClassifyChanged" placeholder="请选择需要出库的物品大类 *必填">
                      <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
              </el-select>
       </el-form-item>  
        </el-col>
        <el-col :span="11" :offset="2">
             <el-form-item label="备注" prop="remark"> 
              <el-input v-model="ruleForm.remark"/>
         </el-form-item>
        </el-col> 
      </el-row>  
       <div class="option-content">
        <el-scrollbar max-height="300px">
          <div class="option-content">
          <el-row class="head"> 
              <el-col :span="21" style="text-align: left;">
                <p class="title">调拨明细</p>
            </el-col>
            <el-col :span="3" style="text-align:right">
                <el-button style="margin-bottom:5px" type="success" :disabled="!ruleForm.goodsClassify" @click="onShowGoodsDrawer">选择{{ invTitle }}</el-button> 
            </el-col>
            </el-row> 
            <el-row v-for="detail in ruleForm.details" :key="detail.goodsId" class="item"> 
            <el-col :span="8">
              <el-input v-model="detail.goodsFullName"  readonly>
                <template #prepend><div style="width: 40px;">{{ detail.goodsClassifyName }}</div></template> 
                <template #append>{{ detail.goodsNo }}</template>
              </el-input> 
            </el-col>
            <el-col :span="3" style="text-align:right">
              <el-popover placement="top-start" title="库存提示" :width="500"  trigger="focus">
                    <template #reference>
                  <el-input v-model="detail.quantity"  placeholder="数量" type="number"  @focus="getStorageData(detail)"/>
                    </template>
                    <el-table :data="storageData" height="200" @row-click="onSelectInvInfo">
                    <el-table-column  property="warehouseName" label="仓库" />
                    <el-table-column  property="shelfName" label="货架" />
                    <el-table-column  property="binName" label="货位" />
                    <el-table-column  property="cellNo" label="料箱" />
                    <el-table-column  property="stock" label="库存量" >
                      <template #default="scope">
                        <span>{{scope.row.stock+scope.row.unitName}}</span>
                      </template>
                    </el-table-column>
                  </el-table>
                </el-popover> 
            </el-col>
              <el-col :span="2" style="text-align:center">
                  <el-select v-model="detail.unitId" class="m-2" style="width:90%" placeholder="单位">
                    <el-option v-for="item in detail.goodsUnitList" :key="item.key" :label="item.value" :value="item.key">
                  </el-option>
                    </el-select>
            </el-col> 
            <el-col :span="5" style="text-align:center"> 
              <el-input  v-model="detail.outMinimumContainer" placeholder="点击选择出库货位" readonly :title="`出库${detail.type=='WorkbinCell'?'料箱':'货位'}${detail.outMinimumContainer}`"  @click="onShowBinDrawer(detail,'Out')">
                    <template #prepend>
                      <span v-if="detail.type=='WorkbinCell'">出库料箱</span>
                      <span v-else>出库货位</span>
                    </template>
                  </el-input> 
              </el-col>
              <el-col :span="5" style="text-align:center"> 
                <el-input  v-model="detail.inMinimumContainer" placeholder="点击选择入库货位" readonly :title="`入库${detail.type=='WorkbinCell'?'料箱':'货位'}${detail.inMinimumContainer}`"  @click="onShowBinDrawer(detail,'In')">
                    <template #prepend>
                      <span v-if="detail.type=='WorkbinCell'">入库料箱</span>
                      <span v-else>入库货位</span>
                    </template>
                  </el-input> 
              </el-col>
              <el-col :span="1"> 
                <el-button class="text-danger" style="height: 22px;line-height: 15px;" @click="onRemoveDetail(detail)"><el-icon ><Delete /></el-icon></el-button>
              </el-col>
          </el-row>  
          </div>
          </el-scrollbar>   
          </div>
          <GoodsSelectDrawer :options="goodsDrawerOptions" v-if="goodsDrawerOptions.show" @selectItem="onSelectGoods"/>
          <BinSelectDrawer :options="binDrawerOptions" v-if="binDrawerOptions.show" @selectItem="onSelectBin"/>
     </el-form> 
  </Layer> 
</template>

<script lang="ts" setup>
import {ref,defineEmits,defineProps,onMounted } from 'vue' 
import Layer from '@/components/layer/index.vue' 
import { ElForm } from 'element-plus' 
import msg from '@/utils/system/message'
import{getOptions} from'@/api/inv/storage' 
import permission from '@/utils/system/permission'  
import {getStorageDetails} from "@/api/inv/storage";
import GoodsSelectDrawer from '@/components/drawer/goodsSelector.vue';
import BinSelectDrawer from '@/components/drawer/binSelector.vue';
import{getBinRecommend} from'@/api/inv/instorage'; 
import {Delete} from '@element-plus/icons-vue';
import { deftClassifyGroup} from '@/config';

const emit = defineEmits(['allocationDataSubmit'])
const props=defineProps({
  layer: {
      type: Object,
      default: () => {
        return {
          show: false,
          title: '',
          showButton: true,
          btnLoading:false,
          type:'',
          data:null, 
        }
      }
    }
  });
  const formRef= ref(ElForm||null)     
  const ruleForm = ref({
      inWarehouseId:'', 
      outWarehouseId:'',  
      createUserId:permission.getOperator().userId,
      createUserName:permission.getOperator().userName, 
      remark:'',  
      goodsClassify:deftClassifyGroup, 
      details:new Array<any>()
  })    
  const rules = {
      inWarehouseId: [{ required: true, message: '请选择入库仓库', trigger: 'change' }], 
      inMinimumContainer: [{ required: true, message: '请选择入库库位', trigger: 'change' }], 
      outWarehouseId: [{ required: true, message: '请选择出库仓库', trigger: 'change' }], 
      outMinimumContainer: [{ required: true, message: '请选择出库库位', trigger: 'change' }], 
      remark:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}]
  }  
  const goodsDrawerOptions= ref({
    show: false,
    title: '', 
    type:'',
    mode:'repet',
    isMultiSelect:true,
    data:new Array<any>() 
  });
      
  const binDrawerOptions= ref({
    show: false,
    title: '', 
    type:'', 
    isMultiSelect:false,
    warehouse:null,
    data:{},
    recommendData:[] 
  });
  var curFlowType='';
  const invTitle=ref(''); 
  const warehouseData=ref(new Array<any>());  
  var warehouseDataBuffer=new Array<any>();
  const unitData=ref(new Array<any>());  
  const goodsClassifyData=ref(new Array<any>());
  const curEditDetail:any=ref();

  onMounted(()=>{ 
    getOptions().then((res:any)=>{  
      warehouseData.value=res.data.warehouseOptions; 
      warehouseDataBuffer=res.data.warehouseOptions;
      unitData.value=res.data.unitOptions;
      if(deftClassifyGroup=='SparePart'){
        goodsClassifyData.value=res.data.goodsClassifyOptions.filter((f:any)=>f.key=='SparePart'||f.key=='Consumables');
      }
      else{
        goodsClassifyData.value=res.data.goodsClassifyOptions;
      } 
      if(deftClassifyGroup){
        invTitle.value=res.data.goodsClassifyOptions.find((f:any)=>f.key==deftClassifyGroup).value;
      }  
      onClassifyChanged(ruleForm.value.goodsClassify);
    })
  }) 

  const onClassifyChanged=(val:any)=>{  
    if(val){
      invTitle.value=goodsClassifyData.value.find(f=>f.key==val).value;
      let warehouseBygroup=warehouseDataBuffer.filter(f=>f.warehouseType==ruleForm.value.goodsClassify); 
      if(warehouseBygroup?.length>0){
          ruleForm.value.inWarehouseId=warehouseBygroup[0].warehouseId;
          ruleForm.value.outWarehouseId=warehouseBygroup[0].warehouseId;
          warehouseData.value =warehouseBygroup; 
        } 
        else{
          warehouseData.value=warehouseDataBuffer;
        }  
      }
  }

  const onSelectInvInfo=(row:any, column:any, event:any)=>{ 
    ruleForm.value.details.forEach(d=>{
      if(d.goodsId==row.goodsId){ 
        d.outWarehouseId=row.warehouseId;
        d.outShelfId=row.shelfId;
        d.outShelfNo=row.shelfNo;
        d.outShelfName=row.shelfName;
        d.outBinId=row.binId;
        d.outBinNo=row.binNo;
        d.outBinName=row.binName;
        d.outWorkbinId=row.workbinId;
        d.outWorkbinNo=row.workbinNo;
        d.outWorkbinCellId=row.cellId;
        d.outWorkbinCellNo=row.cellNo; 
        d.unitId=Number(row.unitId);
        if(row.cellNo){
          d.outMinimumContainer=row.cellNo;
          d.outType='WorkbinCell';
        }
        else if(row.binName){
          d.outMinimumContainer=row.binName;
          d.outType='Bin';
        }
        else{
          d.outMinimumContainer="";
          d.outType='Shelf';
        } 
      }
    }) 
  }
  
  const onShowGoodsDrawer=()=>{
    goodsDrawerOptions.value.show=true;
    goodsDrawerOptions.value.type=ruleForm.value.goodsClassify;
    goodsDrawerOptions.value.title=invTitle.value+'选择'; 
    goodsDrawerOptions.value.data=ruleForm.value.details;
  }

const onShowBinDrawer=(detail:any,flowType:string)=>{  
  if(ruleForm.value.inWarehouseId||ruleForm.value.outWarehouseId){ 
      curFlowType=flowType; 
      binDrawerOptions.value.data=connvertDetaleObj(detail,flowType);
      curEditDetail.value=detail;
      if(flowType=='In'){ 
        getBinRecommend(ruleForm.value.inWarehouseId,detail.goodsSpecificationId).then(res=>{
          binDrawerOptions.value.recommendData=res.data;
          if(detail.inWorkbinCellNo){
            binDrawerOptions.value.title="选择出库料箱"
            binDrawerOptions.value.type="Workbin";
          }
          else if(detail.inBinNo){
            binDrawerOptions.value.title="选择出库货位"
            binDrawerOptions.value.type="Bin";
          }
          else{
            binDrawerOptions.value.title="选择出库货架"
            binDrawerOptions.value.type="Shelf";
          } 
          binDrawerOptions.value.warehouse=warehouseData.value.find(f=>f.warehouseId==ruleForm.value.inWarehouseId);    
          binDrawerOptions.value.show=true;   
        }) 
      }
      else{
        if(binDrawerOptions.value.recommendData){
          binDrawerOptions.value.recommendData.length=0;
        } 
        if(detail.outWorkbinCellNo){
            binDrawerOptions.value.title="选择出库料箱"
            binDrawerOptions.value.type="Workbin";
          }
          else if(detail.outBinNo){
            binDrawerOptions.value.title="选择出库货位"
            binDrawerOptions.value.type="Bin";
          }
          else{
            binDrawerOptions.value.title="选择出库货架"
            binDrawerOptions.value.type="Shelf";
          } 
          binDrawerOptions.value.warehouse=warehouseData.value.find(f=>f.warehouseId==ruleForm.value.outWarehouseId);  
          binDrawerOptions.value.show=true;  
      }  
  } 
  else{
    if(!ruleForm.value.inWarehouseId&&flowType=='In'){
      msg.warningAuto("请先选择调拨入库仓库")
    }
    if(!ruleForm.value.outWarehouseId&&flowType=='Out'){
      msg.warningAuto("请先选择调拨出库仓库")
    }
  }
}

const connvertDetaleObj=(detail:any,flowType:string)=>{
  if(flowType=='In'){
    return{
      goodsId:detail.goodsId, 
      shelfId:detail.inShelfId,
      shelfNo:detail.inShelfNo,
      shelfName:detail.inShelfName,
      binId:detail.inBinId,
      binNo:detail.inBinNo,
      binName:detail.inBinName,
      workbinId:detail.inWorkbinId,
      workbinNo:detail.inWorkbinNo,
      workbinCellId:detail.inWorkbinCellId,
      workbinCellNo:detail.inWorkbinCellNo,
      type:detail.inType
    }
  }
  else{
    return{
      goodsId:detail.goodsId, 
      shelfId:detail.outShelfId,
      shelfNo:detail.outShelfNo,
      shelfName:detail.outShelfName,
      binId:detail.outBinId,
      binNo:detail.outBinNo,
      binName:detail.outBinName,
      workbinId:detail.outWorkbinId,
      workbinNo:detail.outWorkbinNo,
      workbinCellId:detail.outWorkbinCellId,
      workbinCellNo:detail.outWorkbinCellNo,
      type:detail.outType
    }
  }
}

const onSelectGoods=(selectedGoods:any)=>{  
  // goodsArr.forEach((newItem:any)=>{
  //     let isExist=false;
  //     ruleForm.value.details.forEach(oldItem=>{
  //       if(newItem.goodsId==oldItem.goodsId){
  //         isExist=true;
  //       } 
  //     });
  //     if(!isExist){
  //       ruleForm.value.details.push(newItem);
  //     }
  //   }); 
  let objStr=JSON.stringify(selectedGoods);
  ruleForm.value.details.push(JSON.parse(objStr));
  ruleForm.value.details.forEach(f=>{
    f.goodsFullName=f.goodsModel?f.goodsName+' '+f.goodsModel:f.goodsName;
    f.goodsUnitList=unitData.value.filter(u=>u.value==f.packageUnitName||u.value==f.minPackageUnitName||u.value==f.maxPackageUnitName);  
    if(f.goodsUnitList.length==1&&(!f.unitId||Number(f.unitId)==0)){
      f.unitId=f.goodsUnitList[0].key;
    }
  })
}

const onRemoveDetail=(detail:any)=>{ 
  ruleForm.value.details.splice(ruleForm.value.details.indexOf(detail),1);
}

const onSelectBin=(selectedElement:any)=>{   
  if(curFlowType=='In'){ 
    curEditDetail.value.inWarehouseId=selectedElement.warehouseId;
    curEditDetail.value.inShelfId=selectedElement.shelfId;
    curEditDetail.value.inShelfNo=selectedElement.shelfNo;
    curEditDetail.value.inShelfName=selectedElement.shelfName;
    curEditDetail.value.inBinId=selectedElement.binId;
    curEditDetail.value.inBinNo=selectedElement.binNo;
    curEditDetail.value.inBinName=selectedElement.binName;
    curEditDetail.value.inWorkbinId=selectedElement.workbinId;
    curEditDetail.value.inWorkbinNo=selectedElement.workbinNo;
    curEditDetail.value.inWorkbinCellId=selectedElement.workbinCellId;
    curEditDetail.value.inWorkbinCellNo=selectedElement.workbinCellNo;
    curEditDetail.value.inType=selectedElement.type;  
    if(selectedElement.workbinCellNo){
      curEditDetail.value.inMinimumContainer=selectedElement.workbinCellNo
    }
    else if(selectedElement.binName){
      curEditDetail.value.inMinimumContainer=selectedElement.binName
    }
    else{
      curEditDetail.value.inMinimumContainer="";
    } 
  }
  else{
    curEditDetail.value.outWarehouseId=selectedElement.warehouseId;
    curEditDetail.value.outShelfId=selectedElement.shelfId;
    curEditDetail.value.outShelfNo=selectedElement.shelfNo;
    curEditDetail.value.outShelfName=selectedElement.shelfName;
    curEditDetail.value.outBinId=selectedElement.binId;
    curEditDetail.value.outBinNo=selectedElement.binNo;
    curEditDetail.value.outBinName=selectedElement.binName;
    curEditDetail.value.outWorkbinId=selectedElement.workbinId;
    curEditDetail.value.outWorkbinNo=selectedElement.workbinNo;
    curEditDetail.value.outWorkbinCellId=selectedElement.workbinCellId;
    curEditDetail.value.outWorkbinCellNo=selectedElement.workbinCellNo;
    curEditDetail.value.outType=selectedElement.type;  
    if(selectedElement.workbinCellNo){
      curEditDetail.value.outMinimumContainer=selectedElement.workbinCellNo
    }
    else if(selectedElement.binName){
      curEditDetail.value.outMinimumContainer=selectedElement.binName
    }
    else{
      curEditDetail.value.outMinimumContainer="";
    } 
  } 
} 
 
//查询库存 
const storageData=ref()
  const getStorageData=(detail:any)=>{  
  if(detail.goodsId){
    getStorageDetails(detail.goodsId).then(res=>{
      storageData.value=res.data  
    })
  } 
}
  
//点击确认提交
const  submit=()=> {    
  formRef.value.validate((valid:any)=>{  
      if(valid){   
        if(ruleForm.value.details.length==0){
          msg.warningAuto("请添加调拨物品明细")
          return
        }  
        for(let d of ruleForm.value.details) {
          if(!d.goodsId){
            msg.warningAuto("请选择调拨的物品")
            return
          } 
          if(d.quantity<=0){
            msg.warningAuto("请填写正确的调拨数量")
            return
          } 
          if(!d.unitId){
            msg.warningAuto("请选择物品单位")
            return
          } 
          if(d.inWorkbinCellId==d.outWorkbinCellId&&d.inBinId==d.outBinId){
            msg.warningAuto("出库仓库库位和入库仓库库位不能相同")
            return
          }
        };    
        emit('allocationDataSubmit', ruleForm.value); 
      }
    }) 
  }
</script>

<style lang="scss" scoped>
  * {
    text-align: left;
  }
  .box-card{
    margin-top: 10px;
  }
  .option-content{
    border:1px solid rgb(230, 230, 230);
    border-radius: 3px;
    padding: 5px;
    .head{
      border-bottom: 1px solid rgb(230, 230, 230);
      margin-bottom: 5px;
      .title{
        margin: 5px 0 0 0;
      }
    }
    .item{
      margin:3px 0;
    }
  }
</style>