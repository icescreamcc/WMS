<template>
  <Layer :layer="layer" @confirm="submit" >
     <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
       <el-row>
        <el-col :span="11">
             <el-form-item label="出库单号" prop="orderNo">
              <el-input v-model="ruleForm.orderNo" disabled placeholder="系统生成 无需填写" ></el-input>
         </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
               <el-form-item label="出库类型" prop="outStorageType">
                    <el-select v-model="ruleForm.outStorageType" :disabled="!props.layer.showButton" class="m-2" style="width:100%" placeholder="选择出库类型 *必填" >
                      <el-option v-for="item in outStorageTypeData" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
              </el-select>
          </el-form-item>
        </el-col>
      </el-row>
       <el-row>
        <el-col :span="11">
           <el-form-item label="物品类型" prop="goodsClassify">
            <el-select v-model="ruleForm.goodsClassify" class="m-2" :disabled="!props.layer.showButton"  style="width:100%" @change="onClassifyChanged" placeholder="请选择需要出库的物品大类 *必填">
                      <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
              </el-select>
       </el-form-item>  
        </el-col>
        <el-col :span="11" :offset="2">
        <el-form-item label="备注" prop="remark">
         <el-input v-model="ruleForm.remark"  :disabled="!props.layer.showButton"/>
       </el-form-item>
        </el-col>
      </el-row> 
         <el-row>
        <el-col :span="11"> 
             <el-form-item label="出库仓库" prop="warehouseId">
               <el-select v-model="ruleForm.warehouseId" class="m-2"  :disabled="!props.layer.showButton" style="width:100%" placeholder="选择出库的仓库">
                      <el-option v-for="item in warehouseData" :key="item.warehouseId" :label="item.warehouseName" :value="item.warehouseId">
                    </el-option>
              </el-select>
         </el-form-item>
        </el-col> 
        <el-col :span="11" :offset="2">
          <el-form-item label="产线" prop="line"> 
          <el-select v-model="ruleForm.lineNo" style="width: 100%;"  :disabled="!props.layer.showButton" placeholder="选择生产的产线">
                <el-option v-for="item in lineData" :key="item.key" :label="item.value" :value="item.key">
                </el-option>
          </el-select>
          </el-form-item>
        </el-col>
      </el-row> 
          <el-scrollbar max-height="300px">
            <div class="option-content">
            <el-row class="head"> 
               <el-col :span="props.layer.showButton?21:24" style="text-align: left;">
                 <p class="title">出库单明细</p>
              </el-col>
              <el-col v-if="props.layer.showButton" :span="3" style="text-align:right">
                 <el-button style="margin-bottom:5px" type="success" :disabled="!ruleForm.goodsClassify||!props.layer.showButton" @click="onShowGoodsDrawer">选择{{ invTitle }}</el-button> 
              </el-col>
             </el-row> 
             <el-table  :data="detailsData" border>
              <el-table-column prop="goodsNo" label="出库物品" align="center" min-width="200" :show-overflow-tooltip="true">
                  <template #default="detail">
                    <el-input v-model="detail.row.goodsFullName"   readonly :title="detail.row.goodsFullName"  :disabled="!props.layer.showButton">
                    <template #prepend><div style="width: 40px;">{{ detail.row.goodsClassifyName }}</div></template> 
                    <template #append>{{ detail.row.goodsNo }}</template>
                  </el-input> 
                </template>
                </el-table-column>
                <el-table-column prop="quantity" label="出库数量" align="center" min-width="130" :show-overflow-tooltip="true">
                  <template #default="detail">
                    <el-popover placement="top-start" title="库存提示" :width="550"  trigger="focus">
                     <template #reference>
                   <el-input v-model="detail.row.quantity"  placeholder="数量" style="width:65%" :disabled="!props.layer.showButton" type="number" @change="onInputTotalPrice(detail.row)" @focus="getStorageData(detail.row)"/>
                     </template>
                     <el-table :data="storageData" height="200" @row-click="onSelectInvInfo">
                      <el-table-column  property="warehouseName" width="115" label="仓库" />
                      <el-table-column  property="shelfName" width="115" label="货架" />
                      <el-table-column  property="binName" width="115" label="货位" />
                      <el-table-column  property="cellNo" width="115" label="料箱" />
                      <el-table-column  property="stock" label="库存量" >
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
                <el-table-column prop="minimumContainer" label="出库库位" align="center" min-width="150" :show-overflow-tooltip="true">
                  <template #default="detail">
                    <el-input  v-model="detail.row.minimumContainer" :disabled="!props.layer.showButton" placeholder="点击选择出库货位" readonly :title="`出库${detail.row.type=='WorkbinCell'?'料箱':'货位'}${detail.row.minimumContainer||''}`">
                    <template #prepend>
                      <span v-if="detail.row.type=='WorkbinCell'">料箱</span>
                      <span v-else>货位</span>
                    </template>
                  </el-input>
                </template>
                </el-table-column>
                <el-table-column prop="totalPrice" label="出库总价" align="center" min-width="90" :show-overflow-tooltip="true">
                  <template #default="detail">
                    <span>{{ detail.row.totalPrice +detail.row.priceUnit}}</span> 
                </template>
                </el-table-column>
                <el-table-column  label="删除" align="center" min-width="50" :show-overflow-tooltip="true">
                  <template #default="detail">
                    <el-button class="text-danger"  :disabled="!props.layer.showButton" style="height: 22px;line-height: 15px;" @click="onRemoveDetail(detail.row)"><el-icon ><Delete /></el-icon></el-button> 
                </template>
                </el-table-column>
             </el-table>

             <!-- <el-row v-for="detail in detailsData" :key="detail.goodsId" class="item">
              <el-col :span="props.layer.showButton?10:12">
                <el-input v-model="detail.goodsFullName" :title="detail.goodsNo" readonly :disabled="!props.layer.showButton">
                  <template #prepend><div style="width: 40px;">{{ detail.goodsClassifyName }}</div></template> 
                  <template #append>{{ detail.goodsNo }}</template>
                </el-input> 
              </el-col>
              <el-col :span="3" style="text-align:right">
                <el-popover placement="top-start" title="库存提示" :width="550"  trigger="focus">
                     <template #reference>
                   <el-input v-model="detail.quantity"  placeholder="数量" :disabled="!props.layer.showButton" type="number"  @focus="getStorageData(detail)"/>
                     </template>
                     <el-table :data="storageData" height="200" @row-click="onSelectInvInfo">
                      <el-table-column  property="warehouseName" width="115" label="仓库" />
                      <el-table-column  property="shelfName" width="115" label="货架" />
                      <el-table-column  property="binName" width="115" label="货位" />
                      <el-table-column  property="cellNo" width="115" label="料箱" />
                      <el-table-column  property="stock" label="库存量" >
                        <template #default="scope">
                          <span>{{scope.row.stock+scope.row.unitName}}</span>
                        </template>
                      </el-table-column>
                    </el-table>
                  </el-popover> 
              </el-col>
                <el-col :span="3" style="text-align:center">
                   <el-select v-model="detail.unitId" class="m-2" style="width:90%" :disabled="!props.layer.showButton" placeholder="单位">
                      <el-option v-for="item in detail.goodsUnitList" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
                     </el-select>
              </el-col>
              <el-col :span="6" style="text-align:center"> 
                <el-input  v-model="detail.minimumContainer" :disabled="!props.layer.showButton" placeholder="点击选择出库货位" readonly :title="`出库${detail.type=='WorkbinCell'?'料箱':'货位'}${detail.minimumContainer||''}`">
                    <template #prepend>
                      <span v-if="detail.type=='WorkbinCell'">料箱</span>
                      <span v-else>货位</span>
                    </template>
                  </el-input> 
              </el-col> 
              <el-col v-if="props.layer.showButton" :span="2"> 
                <el-button class="text-danger" style="height: 22px;line-height: 15px;" :disabled="!props.layer.showButton" @click="onRemoveDetail(detail)"><el-icon ><Delete /></el-icon></el-button>
              </el-col>
           </el-row>   -->
            </div>
          </el-scrollbar> 
          <GoodsSelectDrawer :options="goodsDrawerOptions" v-if="goodsDrawerOptions.show" @selectItem="onSelectGoods"/>
          <BinSelectDrawer :options="binDrawerOptions" v-if="binDrawerOptions.show" @selectItem="onSelectBin"/>
     </el-form>  
  </Layer> 
</template>

<script lang="ts" setup>
import {ref ,defineEmits,defineProps,onMounted} from 'vue' 
import Layer from '@/components/layer/index.vue' 
import { ElForm } from 'element-plus' 
import msg from '@/utils/system/message'
import{getOptions} from'@/api/inv/outstorage' 
import permission from '@/utils/system/permission' 
import GoodsSelectDrawer from '@/components/drawer/goodsSelector.vue'
import BinSelectDrawer from '@/components/drawer/binSelector.vue'
import {getStorageDetails} from "@/api/inv/storage";
import {Delete} from '@element-plus/icons-vue';
import { deftClassifyGroup} from '@/config';

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
}) 
const emit = defineEmits(['dataSubmit'])
//选项数据
const warehouseData=ref(new Array<any>());  
var warehouseDataBuffer=new Array<any>();
const unitData=ref(new Array<any>());   
const outStorageTypeData=ref(new Array<any>());
const goodsClassifyData=ref(new Array<any>());
const lineData=ref(new Array<any>());
const curEditDetail:any=ref();
const storageData=ref();
//表单
const formRef= ref(ElForm||null)     
const ruleForm = ref({
    orderNo:props.layer.data?.orderNo,
    sourceOrderNo:props.layer.data?.sourceOrderNo,
    outStorageType:props.layer.data?.outStorageType,
    lineNo:props.layer.data?.lineNo,
    goodsClassify:props.layer.data?.goodsClassify?props.layer.data?.goodsClassify:deftClassifyGroup,
    warehouseId:props.layer.data?.warehouseId, 
    remark:props.layer.data?.remark, 
    details:props.layer.data?.details,
    createUserId:'',
    createUserName:''
}) 
const rules = {
    outStorageType: [{ required: true, message: '请选择出库类型', trigger: 'blur' }], 
    goodsClassify: [{ required: true, message: '请选择需要出库物品大类', trigger: 'blur' }], 
    remark:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
    sourceOrderNo:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}]
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
  data:{}  
});
  
const detailsData=ref(new Array<any>())  
const invTitle=ref(''); 

//获取相关选项及参数数据
onMounted(()=>{    
  getOptions().then((res:any)=>{  
    warehouseData.value=res.data.warehouseOptions;
    warehouseDataBuffer=res.data.warehouseOptions;
    unitData.value=res.data.unitOptions;
    outStorageTypeData.value=res.data.outStorageTypeOptions;
    if(deftClassifyGroup=='SparePart'){
        goodsClassifyData.value=res.data.goodsClassifyOptions.filter((f:any)=>f.key=='SparePart'||f.key=='Consumables');
      }
      else{
        goodsClassifyData.value=res.data.goodsClassifyOptions;
      } 
      if(deftClassifyGroup){
        invTitle.value=res.data.goodsClassifyOptions.find((f:any)=>f.key==deftClassifyGroup).value;
      } 
    lineData.value=res.data.lineOptions;
    onClassifyChanged(props.layer.data?.goodsClassify||deftClassifyGroup);
    if(props.layer.data){  
      detailsData.value=props.layer.data.details;    
      detailsData.value.forEach(detail=>{ 
        detail.goodsUnitList=unitData.value.filter(u=>u.value==detail.packageUnitName||u.value==detail.minPackageUnitName||u.value==detail.maxPackageUnitName);    
        detail.goodsFullName=detail.goodsModel?detail.goodsName+' '+detail.goodsModel:detail.goodsName;
        if(detail.workbinCellNo){
          detail.minimumContainer=detail.workbinCellNo;
          detail.type="WorkbinCell";
        }
        else if(detail.binName){
          detail.minimumContainer=detail.binName;
          detail.type="Bin";
        }  
      });
    } 
  })
});


const getStorageData=(detail:any)=>{  
  getStorageDetails(detail.goodsId).then(res=>{
      storageData.value=res.data  
  })
}

const onSelectInvInfo=(row:any, column:any, event:any)=>{ 
  detailsData.value.forEach(d=>{
    if(d.goodsId==row.goodsId){ 
      d.warehouseId=row.warehouseId;
      d.shelfId=row.shelfId;
      d.shelfNo=row.shelfNo;
      d.shelfName=row.shelfName;
      d.binId=row.binId;
      d.binNo=row.binNo;
      d.binName=row.binName;
      d.workbinId=row.workbinId;
      d.workbinNo=row.workbinNo;
      d.workbinCellId=row.cellId;
      d.workbinCellNo=row.cellNo; 
      d.unitId=Number(row.unitId);
      if(row.cellNo){
        d.minimumContainer=row.cellNo;
        d.type='WorkbinCell';
      }
      else if(row.binName){
        d.minimumContainer=row.binName;
        d.type='Bin';
      }
      else{
        d.minimumContainer="";
        d.type='Shelf';
      } 
    }
  }) 
}

const onShowGoodsDrawer=()=>{
  goodsDrawerOptions.value.show=true;
  goodsDrawerOptions.value.type=ruleForm.value.goodsClassify;
  goodsDrawerOptions.value.title=invTitle.value+'选择';
  goodsDrawerOptions.value.data=detailsData.value;
}

const onShowBinDrawer=(detail:any)=>{ 
  if(ruleForm.value.warehouseId){
    binDrawerOptions.value.show=true;   
    if(detail.workbinCellNo){
      binDrawerOptions.value.title="选择出库料箱"
      binDrawerOptions.value.type="Workbin";
    }
    else if(detail.binNo){
      binDrawerOptions.value.title="选择出库货位"
      binDrawerOptions.value.type="Bin";
    }
    else{
      binDrawerOptions.value.title="选择出库货架"
      binDrawerOptions.value.type="Shelf";
    } 
    binDrawerOptions.value.warehouse=warehouseData.value.find(f=>f.warehouseId==ruleForm.value.warehouseId);
    binDrawerOptions.value.data=detail;
    curEditDetail.value=detail;
  } 
  else{
    msg.warningAuto("请先选择出库仓库")
  }
}

const onInputTotalPrice=(detail:any)=>{
  if(detail.unitPrice&&detail.quantity&&detail.unitPrice>0&&detail.quantity>0){
    detail.totalPrice=(Number(detail.unitPrice)*Number(detail.quantity)).toFixed(2);
  }
}

const onSelectGoods=(selectedGoods:any)=>{  
  // goodsArr.forEach((newItem:any)=>{
  //     let isExist=false;
  //     detailsData.value.forEach(oldItem=>{
  //       if(newItem.goodsId==oldItem.goodsId){
  //         isExist=true;
  //       } 
  //     });
  //     if(!isExist){
  //       detailsData.value.push(newItem);
  //     }
  //   }); 
  let objStr=JSON.stringify(selectedGoods);
  detailsData.value.push(JSON.parse(objStr)); 
  detailsData.value.forEach(f=>{
    f.goodsFullName=f.goodsModel?f.goodsName+' '+f.goodsModel:f.goodsName;
    f.goodsUnitList=unitData.value.filter(u=>u.value==f.packageUnitName||u.value==f.minPackageUnitName||u.value==f.maxPackageUnitName);   
    if(f.goodsId==selectedGoods.goodsId){
       f.unitPrice=selectedGoods.costPrice;
       f.totalPrice=0;
       f.priceUnit=selectedGoods.priceUnitName||"元";
    } 
    if(f.goodsUnitList.length==1&&(!f.unitId||Number(f.unitId)==0)){
      f.unitId=f.goodsUnitList[0].key;
    }
  })
}

const onRemoveDetail=(detail:any)=>{ 
  detailsData.value.splice(detailsData.value.indexOf(detail),1);
}

const onSelectBin=(selectedElement:any)=>{  
  curEditDetail.value.warehouseId=selectedElement.warehouseId;
  curEditDetail.value.shelfId=selectedElement.shelfId;
  curEditDetail.value.shelfNo=selectedElement.shelfNo;
  curEditDetail.value.shelfName=selectedElement.shelfName;
  curEditDetail.value.binId=selectedElement.binId;
  curEditDetail.value.binNo=selectedElement.binNo;
  curEditDetail.value.binName=selectedElement.binName;
  curEditDetail.value.workbinId=selectedElement.workbinId;
  curEditDetail.value.workbinNo=selectedElement.workbinNo;
  curEditDetail.value.workbinCellId=selectedElement.workbinCellId;
  curEditDetail.value.workbinCellNo=selectedElement.workbinCellNo;
  curEditDetail.value.type=selectedElement.type;
  if(selectedElement.workbinCellNo){
    curEditDetail.value.minimumContainer=selectedElement.workbinCellNo
  }
  else if(selectedElement.binName){
    curEditDetail.value.minimumContainer=selectedElement.binName
  }
  else{
    curEditDetail.value.minimumContainer="";
  } 
}
 
const onClassifyChanged=(val:any)=>{  
  if(val)
  invTitle.value=goodsClassifyData.value.find(f=>f.key==val).value;
  let warehouseBygroup=warehouseDataBuffer.filter(f=>f.warehouseType==ruleForm.value.goodsClassify); 
  if(warehouseBygroup?.length>0){
      ruleForm.value.warehouseId=warehouseBygroup[0].warehouseId;
      warehouseData.value =warehouseBygroup; 
    } 
    else{
      warehouseData.value=warehouseDataBuffer;
    }  
}
  
//点击确认提交
const submit=()=> {    
    formRef.value.validate((valid:any)=>{ 
        if(valid){
          if(detailsData.value.length==0){
            msg.warningAuto("请添加出库单明细")
            return
          }
          else{  
              for(let opt of detailsData.value){
                if(!opt.goodsId){
                  msg.warningAuto("请选择出库物品")
                  return
                }
                if(!opt.quantity||Number(opt.quantity)<=0){
                  msg.warningAuto("请输入正确的出库数量")
                  return
                }
                if(!opt.unitId||Number(opt.unitId)==0){
                  msg.warningAuto("请选择出库单位")
                  return
                } 
                if(!opt.binId){
                  msg.warningAuto("请选择出库货位")
                  return
                }  
              } 
          }
            ruleForm.value.details=detailsData.value; 
            let editType=props.layer.data?'update':'add';
            ruleForm.value.createUserId=permission.getOperator().userId;
            ruleForm.value.createUserName=permission.getOperator().userName;
            emit('dataSubmit', ruleForm.value,editType) 
        }
    }) 
} 
</script>

<style lang="scss" scoped> 
  .box-card{
    margin-top: 10px;
  } 
  .option-content{
    border:1px solid rgb(230, 230, 230);
    border-radius: 3px;
    padding: 5px;
    .head{
     // border-bottom: 1px solid rgb(230, 230, 230);
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