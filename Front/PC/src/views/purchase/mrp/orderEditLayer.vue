<template>
    <Layer :layer="layer" @confirm="submit" >
       <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
         <el-row>
          <el-col :span="11">
               <el-form-item label="计划单号" prop="orderNo">
                <el-input v-model="ruleForm.orderNo" disabled placeholder="系统生成 无需填写" ></el-input>
           </el-form-item>
          </el-col>
          <el-col :span="11" :offset="2">
            <el-form-item label="物料分类" prop="goodsClassify">
                <el-select v-model="ruleForm.goodsClassifyGroup" class="m-2" :disabled="true"  style="width:100%"  placeholder="请选择物料分类 *必填">
                              <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                            </el-option>
                      </el-select>
              </el-form-item>  
          </el-col>
        </el-row>
          <el-row>
          <el-col :span="11">
            <el-form-item label="供应商" prop="supplierId">
                <el-select style="width: 100%;" 
                  v-model="ruleForm.supplierName" 
                  :disabled="!props.layer.showButton"
                  filterable
                  remote
                  reserve-keyword
                  placeholder="输入供应商关键字查询"
                  :remote-method="getSupplierData"
                  @change="supplierSelectChanged"
                  :loading="supplierSearchLoading">
                  <el-option v-for="item in supplierData" :label="item.supplierName"  :value="item.supplierId" /> 
                </el-select>
            </el-form-item>  
          </el-col>
          <el-col :span="11" :offset="2">
            <el-form-item label="计划版本" prop="version">
              <el-input v-model="ruleForm.version" :disabled="true"  controls-position="right"  style="width:100%"  type="number" placeholder="计划版本" />
         </el-form-item>
          </el-col>
        </el-row> 
         <el-row> 
          <el-col :span="11"> 
            <el-form-item label="备注" prop="remark">
                <el-input v-model="ruleForm.remark" :disabled="!props.layer.showButton"/>
              </el-form-item> 
          </el-col>  
          <el-col :span="11" :offset="2">
            <el-form-item label="计划周期" prop="week">
              <el-input v-model="ruleForm.yearWeek" :disabled="true"/>
         </el-form-item> 
          </el-col>
        </el-row>     
          <div class="option-content">
              <el-row class="head"> 
                      <el-col :span="20" >
                        <p class="title">物料需求明细</p>
                      </el-col> 
                    </el-row>   
                  <el-row gutter="10">
                    <el-col :span="5" style="text-align: center;color: #888;">物料名称</el-col>
                    <el-col :span="3" style="text-align: center;color: #888;">物料编号</el-col>
                    <el-col :span="3" style="text-align: center;color: #888;">物料类型</el-col>
                    <el-col :span="3" style="text-align: center;color: #888;">需求数量</el-col>
                    <el-col :span="3" style="text-align: center;color: #888;">单位</el-col> 
                    <!-- <el-col :span="3" style="text-align: center;color: #888;">需求等级</el-col> -->
                    <el-col :span="4" style="text-align: center;color: #888;">需求日期</el-col>
                    <el-col :span="3" style="text-align: center;color: #888;">需求班次</el-col> 
                  </el-row>
                  <div style="max-height: 400px;overflow-y: scroll">
                    <el-row v-for="detail in ruleForm.details" :key="detail.detailId" class="item" gutter="10"> 
                      <el-col :span="5" style="text-align: center;">
                        <el-input v-model="detail.goodsName" :disabled="!props.layer.showButton"  :title="detail.goodsName" placeholder="物料号名称"></el-input> 
                      </el-col>
                      <el-col :span="3" style="text-align: center;">
                        <el-input v-model="detail.goodsNo" :disabled="!props.layer.showButton"  :title="detail.goodsNo" placeholder="物料号"></el-input> 
                      </el-col>
                      <el-col :span="3" style="text-align: center;">
                        <el-input v-model="detail.goodsType" :disabled="!props.layer.showButton"  :title="detail.goodsType" placeholder="物料类型"></el-input> 
                      </el-col>
                      <el-col :span="3" style="text-align:center">
                        <el-input-number v-model="detail.quantity" :disabled="!props.layer.showButton" style="width:95%" :min="1" controls-position="right" placeholder="需求数量" type="number" />
                      </el-col>
                        <el-col :span="3" style="text-align:center">
                          <el-select v-model="detail.unitName" :disabled="!props.layer.showButton" class="m-2" style="width:90%;margin-left:5px" placeholder="单位">
                            <el-option v-for="item in detail.quantityUnitList" :key="item.key" :label="item.value" :value="item.value">
                          </el-option>
                          </el-select>
                      </el-col> 
                      <!-- <el-col :span="3" style="text-align:center"> 
                        <el-select v-model="detail.requirementLevel"  class="m-2" style="width:90%;margin-left:5px" placeholder="需求等级">
                            <el-option v-for="item in requirementLevelData" :key="item.key" :label="item.value" :value="item.key">
                          </el-option>
                          </el-select>
                      </el-col>  -->
                      <el-col :span="4" style="text-align:center"> 
                        <el-date-picker v-model="detail.reqDate" :disabled="!props.layer.showButton" style="width:100%" type="date"   value-format="YYYY-MM-DD"  placeholder="需求日期"> </el-date-picker> 
                      </el-col> 
                      <el-col :span="3" style="text-align:center"> 
                        <el-select v-model="detail.shift" :disabled="!props.layer.showButton" class="m-2" style="width:90%;margin-left:5px" placeholder="需求班次">
                            <el-option v-for="item in shiftData"  :label="item.value" :value="item.key">
                          </el-option>
                          </el-select> 
                      </el-col>  
                  </el-row> 
                  </div>
             </div>  
             <GoodsSelectDrawer :options="goodsDrawerOptions" v-if="goodsDrawerOptions.show" @selectItem="onSelectGoods"/>
       </el-form> 
    </Layer> 
  </template>
  
  <script lang="ts" setup>
  import {  ref,defineEmits,defineProps,onMounted } from 'vue'; 
  import Layer from '@/components/layer/index.vue'; 
  import { ElForm } from 'element-plus'; 
  import msg from '@/utils/system/message';
  import{getOptions} from'@/api/purchase/materialRequirementPlan';
  import{getUserByKey,getSupplierByKey}from '@/api/common';
  import permission from '@/utils/system/permission';
  import GoodsSelectDrawer from '@/components/drawer/goodsSelector.vue'; 
  import {Delete} from '@element-plus/icons-vue'; 
  import commonHelper from "@/utils/system/common-helper";

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
            options:null,
            data:null 
          }
        }
      }
  });
  const emit = defineEmits(['dataSubmit']); 
  const supplierSearchLoading=ref(false); 
  const requirementLevelData=ref(new Array<any>());  
  const unitData=ref(new Array<any>());   
  const supplierData=ref(new Array<any>());  
  const goodsClassifyData=ref(new Array<any>());  
  const shiftData=ref(new Array<any>());  
  const formRef= ref(ElForm||null);    
  const ruleForm = ref({
      orderNo:props.layer.data?.orderNo, 
      supplierId:props.layer.data?.supplierId,
      supplierName:props.layer.data?.supplierName, 
      version:props.layer.data?.version||'',
      year:props.layer.data?.year,
      week:props.layer.data?.week, 
      yearWeek:props.layer.data?.year+'CW'+props.layer.data?.week,
      remark:props.layer.data?.remark,  
      goodsClassifyGroup:props.layer.data?.goodsClassifyGroup||props.layer.options?.goodsClassifyGroup,  
       
      createUserId:permission.getOperator().userId,
      createUserName:permission.getOperator().userName, 
      details:props.layer.data?.details||[]
  });   
 
  const rules = {
     
  }  
  const goodsDrawerOptions= ref({
    show: false,
    title: '', 
    type:'',
    mode:'no-repet',
    isMultiSelect:true,
    data:new Array<any>() 
  });
 
  onMounted(()=>{ 
    getOptions().then((res:any)=>{ 
        requirementLevelData.value=res.data.requirementLevelData; 
        unitData.value=res.data.unitData; 
        goodsClassifyData.value=res.data.goodsClassifyData; 
        shiftData.value=res.data.shiftData;  
        if(ruleForm.value.details.length>0){    
          ruleForm.value.details.forEach((detail:any)=>{ 
            detail.quantityUnitList=unitData.value.filter(f=>f.type=='Pack')
          })
        } 
      })
  }); 
  
       
  const getSupplierData=(keyword:string)=>{
    supplierSearchLoading.value=true
    getSupplierByKey(keyword).then((res:any)=>{
      supplierData.value=res.data 
    }).finally(()=>  supplierSearchLoading.value=false)
  } 

  const supplierSelectChanged=(supplierId:string)=>{ 
    let obj:any=supplierData.value.find((x:any)=>x.supplierId==supplierId)
    if(obj){
        ruleForm.value.supplierId=obj.supplierId;
        ruleForm.value.supplierName=obj.supplierName;
    }  
  }
  
  const onAddDetail=()=>{
    ruleForm.value.details.push({
      goodsId:'',
      goodsNo:'',
      goodsName:'',
      goodsClassifyName:'',
      quantity:'',
      quantityUnitId:'',
      quantityUnitList:unitData.value.filter(f=>f.type=='Pack'),
      workpieceTray:0,
      pallet:0
    });
  }
     
  const onShowGoodsDrawer=()=>{ 
    goodsDrawerOptions.value.show=true;
    goodsDrawerOptions.value.type=ruleForm.value.goodsClassifyGroup; 
    goodsDrawerOptions.value.data=ruleForm.value.details;
  }
   
  const onSelectGoods=(goodsArr:[])=>{   
    goodsArr.forEach((newItem:any)=>{
        let isExist=false;
        ruleForm.value.details.forEach((oldItem:any)=>{
          if(newItem.goodsId==oldItem.goodsId){
            isExist=true;
          } 
        });
        if(!isExist){
          let unitList=unitData.value.filter(u=>u.value==newItem.packageUnitName||u.value==newItem.minPackageUnitName||u.value==newItem.maxPackageUnitName)
          ruleForm.value.details.push({
            goodsId:newItem.goodsId,
            goodsNo:newItem.goodsNo,
            goodsName:newItem.goodsName,
            goodsClassifyName:newItem.goodsClassifyName,
            quantity:1,
            quantityUnitId:unitList?.length>0?unitList[0].key:'',
            quantityUnitName:unitList?.length>0?unitList[0].value:'',
            quantityUnitList:unitList,
            workpieceTray:0,
            pallet:0
          });
        }
      });  
}
   
const onRemoveDetail=(detail:any)=>{ 
    ruleForm.value.details.splice(ruleForm.value.details.indexOf(detail),1) 
  }
   
  
  const submit=()=> {      
      formRef.value.validate((valid:any)=>{ 
          if(valid){ 
            if(ruleForm.value.details.length==0){
              msg.warningAuto("请添加收货单明细")
              return
            }
            else{  
                for(let opt of ruleForm.value.details){  
                  if(!opt.goodsNo){
                    msg.warningAuto("物料编号不能为空")
                    return
                  }
                  if(!opt.goodsClassifyName){
                    msg.warningAuto("物料类型不能为空")
                    return
                  }
                  if(!opt.quantityUnitId){
                    msg.warningAuto("请选择收货计数单位")
                    return
                  } 
                } 
            }  
            emit('dataSubmit', ruleForm.value,props.layer.data?'update':'add') 
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
          margin: 5px 0 5px 0;
        }
      }
      .item{
        margin:3px 0;
      }
    }
    .popper-cls{
      color: #888;
      font-size: small;
    }
  </style>