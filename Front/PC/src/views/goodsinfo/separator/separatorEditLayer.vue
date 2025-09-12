<template>
  <Layer :layer="layer" @confirm="submit" > 
   <el-form  label-width="auto" label-position="left" :model="ruleForm" :rules="rules" ref="formRef" style="min-height:400px;margin-top: -10px;">
      <el-tabs v-model="tabSelected" style="box-shadow:none;-webkit-box-shadow:none;min-height:380px">
        <el-tab-pane label="基本信息" name="base">
        <el-row> 
        <el-col :span="11">
        <el-form-item label="辅材ID" style="margin-left:20px">
          <el-input v-model="ruleForm.goodsId" readonly placeholder="系统生成 无需填写"></el-input>
          </el-form-item> 
        </el-col>
        <el-col :span="11"  >
       <el-form-item label="辅材分类"  style="margin-left:20px">
             <el-input v-model="ruleForm.goodsClassifyName" readonly  ></el-input>
      </el-form-item> 
        </el-col>
      </el-row> 
      <el-row> 
        <el-col :span="11">
        <el-form-item label="SAP编码" prop="goodsNo" style="margin-left:20px">
          <el-input v-model="ruleForm.goodsNo"  placeholder="请输入辅材编码" autofocus></el-input>
          </el-form-item> 
        </el-col>
        <el-col :span="11" >
         <el-form-item label="辅材名称" prop="goodsName" style="margin-left:20px" >
        <el-input v-model="ruleForm.goodsName"  placeholder="请输入辅材名称  *必填"></el-input>
      </el-form-item> 
        </el-col>
      </el-row> 
        <el-row>
        <el-col :span="11"> 
          <el-form-item label="辅材属性" prop="goodsProperty" style="margin-left:20px"> 
          <el-input v-model="ruleForm.goodsProperty" placeholder="输入辅材属性、参数等信息"></el-input>
          </el-form-item> 
        </el-col>
        <el-col :span="11" >
           <el-form-item label="辅材类型" style="margin-left:20px"> 
             <el-select v-model="ruleForm.goodsTypeId"  class="m-2" placeholder="选择辅材类型" style="width:100%" >
                  <el-option
                    v-for="item in separatorTypeData"
                    :key="item.optionId"
                    :label="item.optionName"
                    :value="item.optionId">
                  </el-option>
                </el-select> 
            </el-form-item> 
        </el-col>
      </el-row>  
       <el-row> 
        <el-col :span="11">
          <el-form-item label="供应商" prop="supplier" style="margin-left:20px">  
            <el-select style="width: 100%;"
              v-model="ruleForm.supplier" 
              filterable
              remote
              reserve-keyword
              placeholder="输入供应商关键字查询"
              :remote-method="getSupplierData"
              @change="onSelectSupplier"
              :loading="querySupplierLoading" >
              <el-option v-for="item in supplierData" :key="item.supplierId"  :label="item.supplierName"  :value="item.supplierName" />
            </el-select>
      </el-form-item>
        </el-col>
        <el-col :span="11" >
            <el-form-item label="标准包装单位" prop="packageUnitName" style="margin-left:20px">
               <el-select v-model="ruleForm.packageUnitName"  class="m-2" placeholder="请选择标准包装单位 *必填" style="width:100%" @change="onSelectPackageUnit">
            <el-option
              v-for="item in unitsData"
              :key="item.key"
              :label="item.value"
              :value="item.value" >
            </el-option>
          </el-select> 
      </el-form-item> 
        </el-col>
      </el-row> 
           <el-row> 
        <el-col :span="11" >
           <el-form-item label="最小包装单位" prop="minPackageUnitName" style="margin-left:20px">
               <el-select v-model="ruleForm.minPackageUnitName"  class="m-2" placeholder="请选择最小包装单位 *必填" style="width:100%" >
            <el-option
              v-for="item in unitsData"
              :key="item.key"
              :label="item.value"
              :value="item.value">
            </el-option>
          </el-select> 
          </el-form-item> 
        </el-col>
          <el-col :span="11">  
                <el-form-item label="最大包装单位" prop="maxPackageUnitName" style="margin-left:20px">
               <el-select v-model="ruleForm.maxPackageUnitName"  class="m-2" placeholder="请选择最大包装单位 *必填" style="width:100%" >
            <el-option
              v-for="item in unitsData"
              :key="item.key"
              :label="item.value"
              :value="item.value" >
            </el-option>
          </el-select> 
      </el-form-item>
        </el-col>
      </el-row> 
           <el-row>
               <el-col :span="9" >
               <el-form-item label="每标准包装数量" prop="packageCount" style="margin-left:20px">
             <el-input v-model="ruleForm.packageCount" type="number"  placeholder="请填写最小包装的数量 *必填" ></el-input> 
          </el-form-item>
        </el-col>
        <el-col :span="2">
              <p style="margin-top:6px;color:#888">{{ruleForm.minPackageUnitName?ruleForm.minPackageUnitName:'--'}}</p>
        </el-col>
        <el-col :span="9"> 
           <el-form-item label="每最大包装数量" prop="maxPackageCount" style="margin-left:20px">
             <el-input v-model="ruleForm.maxPackageCount" type="number"  placeholder="请填写最大包装的数量 *必填" ></el-input>
          </el-form-item>  
        </el-col> 
         <el-col :span="2">
              <p style="margin-top:6px;color:#888">{{ruleForm.packageUnitName?ruleForm.packageUnitName:'--'}}</p>
        </el-col>
      </el-row>
             <el-row> 
           <el-col :span="11" >
        <el-form-item label="库存单位" prop="safetyInventoryUnitName" style="margin-left:20px">
           <el-select v-model="ruleForm.safetyInventoryUnitName"  class="m-2" placeholder="请选择库存单位 *必填" style="width:100%" >
            <el-option
              v-for="item in unitsData.filter(x=>x.value==ruleForm.packageUnitName||x.value==ruleForm.minPackageUnitName||x.value==ruleForm.maxPackageUnitName)"
              :key="item.key"
              :label="item.value"
              :value="item.value" >
            </el-option>
          </el-select> 
      </el-form-item>
        </el-col>
            <el-col :span="9">
          <el-form-item label="安全库存"  prop="safetyInventory"  style="margin-left:20px">
        <el-input v-model="ruleForm.safetyInventory" type="number"></el-input>
      </el-form-item>
        </el-col> 
         <el-col :span="2">
              <p style="margin-top:6px;color:#888">{{ruleForm.safetyInventoryUnitName?ruleForm.safetyInventoryUnitName:'--'}}</p>
        </el-col>
      </el-row> 
      <el-row>
              <el-col :span="11">
                <el-form-item label="支持以旧换新"  prop="isOldForNew"  style="margin-left:20px;text-align: left;">
                  <el-checkbox  label="是" v-model="ruleForm.isOldForNew"/>  
            </el-form-item>
              </el-col> 
                <el-col :span="11" >
                  <el-form-item label="是否在SAP" style="margin-left:20px;text-align: left;" prop="isInSAP">
                    <el-checkbox  label="是" v-model="ruleForm.isInSAP"/>    
               </el-form-item>
              </el-col>
            </el-row> 
        </el-tab-pane>
        <el-tab-pane label="外观尺寸" name="facade">
           <el-row>
        <el-col :span="11"> 
           <el-form-item label="长度" style="margin-left:20px">
             <el-input v-model="ruleForm.long" type="number"  ></el-input>
          </el-form-item>  
        </el-col>
        <el-col :span="11" >
           <el-form-item label="宽度" style="margin-left:20px">
        <el-input v-model="ruleForm.wide"  type="number" ></el-input>
      </el-form-item> 
        </el-col>
      </el-row> 
            <el-row>
        <el-col :span="11"> 
           <el-form-item label="高度" style="margin-left:20px">
             <el-input v-model="ruleForm.height" type="number"   ></el-input>
          </el-form-item>  
        </el-col>
        <el-col :span="11" >
           <el-form-item label="尺寸单位" style="margin-left:20px">
           <el-select v-model="ruleForm.sizeUnitName"  class="m-2" placeholder="选择尺寸单位" style="width:100%" >
            <el-option
              v-for="item in unitsData.filter(x=>x.type=='Size')"
              :key="item.key"
              :label="item.value"
              :value="item.value" >
            </el-option>
          </el-select> 
      </el-form-item> 
        </el-col>
      </el-row> 
        <el-row>
        <el-col :span="11">
                <el-form-item label="重量" style="margin-left:20px">
           <el-input v-model="ruleForm.weight" type="number" ></el-input>
      </el-form-item>
        </el-col>
        <el-col :span="11"  > 
             <el-form-item label="重量单位" prop="color" style="margin-left:20px"> 
          <el-select v-model="ruleForm.weightUnitName"  class="m-2" placeholder="选择重量单位" style="width:100%" >
            <el-option
              v-for="item in unitsData.filter(x=>x.type=='Weight')"
              :key="item.key"
              :label="item.value"
              :value="item.value" >
            </el-option>
          </el-select> 
      </el-form-item>
        </el-col>
      </el-row>
      <el-row> 
        <el-col :span="11" >
           <el-form-item label="储存规格" style="margin-left:20px"> 
             <el-select v-model="ruleForm.goodsSpecificationId"  class="m-2" placeholder="选择辅材存放料箱的规格" style="width:100%" >
                  <el-option
                    v-for="item in workbinSpecData"
                    :key="item.key"
                    :label="item.value"
                    :value="item.key">
                  </el-option>
                </el-select> 
            </el-form-item> 
        </el-col>
        <el-col :span="9">
                <el-form-item label="最大堆放量" prop="color" style="margin-left:20px">
                <el-input v-model="ruleForm.maxStock" type="number" title="相对该规格料箱的最大堆放数量" placeholder="相对该规格料箱的最大堆放数量"></el-input>
              </el-form-item>
        </el-col>
        <el-col :span="2">
              <p style="margin-top:6px;color:#888">{{ruleForm.safetyInventoryUnitName?ruleForm.safetyInventoryUnitName:'--'}}</p>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11" >
                  <el-form-item label="按此规格入库" style="margin-left:20px;text-align: left;" prop="isConstraintSpec">
                    <el-checkbox  label="是" v-model="ruleForm.isConstraintSpec"/>    
               </el-form-item>
              </el-col>
        <el-col :span="11">
                <el-form-item label="颜色" prop="color" style="margin-left:20px">
                <el-input v-model="ruleForm.color" ></el-input>
              </el-form-item>
        </el-col>  
      </el-row>
       <el-row>
        <el-col :span="24" style="margin-bottom:40px">
           <el-form-item label="辅材图片"  style="height:50px;margin-left:20px">
               <Upload :uploadParams="uploadParams"  @handleImgChanged="imgChanged" /> 
          </el-form-item>
        </el-col>  
      </el-row> 
        </el-tab-pane>
        <el-tab-pane label="相关价格">
             <el-row> 
            <el-col :span="11" > 
            <el-row>
                  <el-col :span="16">
                      <el-form-item label="成本价" style="margin-left:20px">
                        <el-input v-model="ruleForm.costPrice" type="number" ></el-input>
                    </el-form-item>
                  </el-col>
                    <el-col :span="1"><p class="unit-tag">/</p></el-col>
                  <el-col :span="7">
                        <el-form-item  label-width="10px">
                          <el-select v-model="ruleForm.costPriceUnitName"  class="m-2" placeholder="选择单位" style="width:100%" >
                            <el-option
                              v-for="item in unitsData.filter(x=>x.value==ruleForm.packageUnitName||x.value==ruleForm.minPackageUnitName||x.value==ruleForm.maxPackageUnitName)"
                              :key="item.key"
                              :label="item.value"
                              :value="item.value">
                            </el-option>
                          </el-select> 
                      </el-form-item>
                  </el-col>
                </el-row> 
            </el-col>
            <el-col :span="11"> 
              <el-row>
            <el-col :span="16">
                   <el-form-item label="采购参考价" style="margin-left:20px">
                  <el-input v-model="ruleForm.refPurchPrice" type="number"></el-input>
                </el-form-item>
            </el-col>
              <el-col :span="1"><p class="unit-tag">/</p></el-col>
            <el-col :span="7">
                   <el-form-item  label-width="10px">
                    <el-select v-model="ruleForm.refPurchPriceUnitName"  class="m-2" placeholder="选择单位" style="width:100%" >
                      <el-option
                        v-for="item in unitsData.filter(x=>x.value==ruleForm.packageUnitName||x.value==ruleForm.minPackageUnitName||x.value==ruleForm.maxPackageUnitName)"
                        :key="item.key"
                        :label="item.value"
                        :value="item.value">
                      </el-option>
                    </el-select> 
                </el-form-item>
            </el-col>
          </el-row> 
            </el-col>
          </el-row>  
         <el-row> 
        <el-col :span="11" >
          <el-form-item label="价格单位" style="margin-left:20px">
          <el-select v-model="ruleForm.priceUnitName"  class="m-2" placeholder="选择价格单位" style="width:100%" >
            <el-option
              v-for="item in unitsData.filter(x=>x.type=='Currency')"
              :key="item.key"
              :label="item.value"
              :value="item.value">
            </el-option>
          </el-select> 
       </el-form-item>
        </el-col>
      </el-row>     
         
        </el-tab-pane>
        <el-tab-pane label="其他信息">   
              <el-row>
                <el-col :span="11">
              <el-form-item label="用途" style="margin-left:20px" prop="direction"> 
                <el-input v-model="ruleForm.direction" ></el-input>
            </el-form-item>
              </el-col>
              <el-col :span="11" >
              <el-form-item label="保质期(月)" style="margin-left:20px" prop="expirationDate">
                <el-input v-model="ruleForm.expirationDate" type="number"></el-input>
            </el-form-item>
              </el-col>
            </el-row>
               <el-row>
                <el-col :span="11">
                  <el-row>
                  <el-col :span="16">
                      <el-form-item label="最小采购量" style="margin-left:20px">
                        <el-input v-model="ruleForm.purchaseMinimum" type="number" ></el-input>
                    </el-form-item>
                  </el-col>
                    <el-col :span="1"><p class="unit-tag">/</p></el-col>
                  <el-col :span="7">
                        <el-form-item  label-width="10px">
                          <el-select v-model="ruleForm.purchaseMinimumUnitName"  class="m-2" placeholder="选择单位" style="width:100%" >
                            <el-option
                              v-for="item in unitsData.filter(x=>x.value==ruleForm.packageUnitName||x.value==ruleForm.minPackageUnitName||x.value==ruleForm.maxPackageUnitName)"
                              :key="item.key"
                              :label="item.value"
                              :value="item.value">
                            </el-option>
                          </el-select> 
                      </el-form-item>
                  </el-col>
                </el-row>  
              </el-col> 
                <el-col :span="11"  > 
                  <el-form-item label="采购周期(天)" prop="barcode" style="margin-left:20px">
                    <el-input v-model="ruleForm.purchaseCycle" type="number"></el-input>
                 </el-form-item>
                </el-col>
            </el-row>
           <el-row> 
            <el-col :span="11" >
                  <el-form-item label="是否有效" style="margin-left:20px;text-align: left;" prop="isInSAP">
                    <el-checkbox  label="是" v-model="ruleForm.isValid"/>    
               </el-form-item>
              </el-col>
              <el-col :span="11" >
                <el-form-item label="备注" prop="remark" style="margin-left:20px">
              <el-input v-model="ruleForm.remark" ></el-input>
            </el-form-item> 
              </el-col>
            </el-row>   
        </el-tab-pane> 
        <el-tab-pane label="自定义">
         <el-row>
        <el-col :span="11" v-if="getSpareFields('GoodsField1')">
          <DynamicFields :fieldsInfo="getSpareFields('GoodsField1')" :fieldsData="ruleForm.goodsField1" v-model="ruleForm.goodsField1" />
        </el-col>
        <el-col :span="11"  v-if="getSpareFields('GoodsField2')">
          <DynamicFields :fieldsInfo="getSpareFields('GoodsField2')" :fieldsData="ruleForm.goodsField2"  v-model="ruleForm.goodsField2"/>
        </el-col>
            <el-col :span="11" v-if="getSpareFields('GoodsField3')">
          <DynamicFields :fieldsInfo="getSpareFields('GoodsField3')" :fieldsData="ruleForm.goodsField3" v-model="ruleForm.goodsField3"/>
        </el-col>
        <el-col :span="11"  v-if="getSpareFields('GoodsField4')">
          <DynamicFields :fieldsInfo="getSpareFields('GoodsField4')" :fieldsData="ruleForm.goodsField4" v-model="ruleForm.goodsField4"/>
        </el-col>
          <el-col :span="11" v-if="getSpareFields('GoodsField5')">
          <DynamicFields :fieldsInfo="getSpareFields('GoodsField5')" :fieldsData="ruleForm.goodsField5" v-model="ruleForm.goodsField5"/>
        </el-col>  
      </el-row>
        </el-tab-pane>
    </el-tabs> 
    </el-form> 
  </Layer>
</template>

<script lang="ts" setup>
import { ref, reactive ,defineEmits,defineProps,onMounted} from 'vue'
import Layer from '@/components/layer/index.vue'  
import { ElForm } from 'element-plus'
import {getOptions} from '@/api/baseinfo/separator'
import Upload from '@/components/imgUpload/muiltUpload.vue'
import DynamicFields from '@/components/form/dynamicFields.vue' 
import msg from '@/utils/system/message'
import {getSupplierByKey} from '@/api/common'
import permission from '@/utils/system/permission'

const props=defineProps({
  layer: {
      type: Object,
      default: () => {
        return {
          show: false,
          title: '',
          showButton: true,
          btnLoading:false,
          data:null
        }
      }
    },
    spareClassify: {
      type: Object,
      default: () => {
        return {
          typeId: 0,
          typeName: null
        }
      }
    }
  }) 
  const emit = defineEmits(['dataSubmit']);
  const tabSelected=ref('base')
  const formRef =ref(ElForm||null)  
  const ruleForm = ref( {
        goodsId:props.layer.row?.goodsId,
        goodsNo:props.layer.row?.goodsNo, 
        goodsName:props.layer.row?.goodsName,
        goodsClassifyId:props.layer.row?.goodsClassifyId,
        goodsClassifyName:props.layer.row?.goodsClassifyName,
        goodsTypeId:props.layer.row?.goodsTypeId,
        goodsTypeName:props.layer.row?.goodsTypeName,
        goodsModel:props.layer.row?.goodsModel,
        goodsProperty:props.layer.row?.goodsProperty,
        goodsSpecificationId:props.layer.row?.goodsSpecificationId?props.layer.row?.goodsSpecificationId:'',
        maxStock:props.layer.row?.maxStock?props.layer.row?.maxStock:0,
        isConstraintSpec:props.layer.row?.isConstraintSpec,
        forArea:props.layer.row?.forArea,
        supplier:props.layer.row?.supplier,
        packageCount:props.layer.row?.packageCount>0?props.layer.row?.packageCount:1,
        packageUnitId:props.layer.row?.packageUnitId,
        packageUnitName:props.layer.row?.packageUnitName, 
        minPackageUnitId:props.layer.row?.minPackageUnitId,
        minPackageUnitName:props.layer.row?.minPackageUnitName,
        maxPackageUnitId :props.layer.row?.maxPackageUnitId,
        maxPackageUnitName :props.layer.row?.maxPackageUnitName,
        maxPackageCount :props.layer.row?.maxPackageCount>0?props.layer.row?.maxPackageCount:1,
        safetyInventory:props.layer.row?.safetyInventory>0?props.layer.row?.safetyInventory:0,
        safetyInventoryUnitId:props.layer.row?.safetyInventoryUnitId,
        safetyInventoryUnitName:props.layer.row?.safetyInventoryUnitName,
        long:props.layer.row?.long>0?props.layer.row?.long:0,
        wide:props.layer.row?.wide>0?props.layer.row?.wide:0,
        height: props.layer.row?.height>0?props.layer.row?.height:0,
        sizeUnitName :props.layer.row?.sizeUnitName,
        weight :props.layer.row?.weight>0?props.layer.row?.weight:0,
        weightUnitName :props.layer.row?.weightUnitName,
        color :props.layer.row?.color,
        
        source :props.layer.row?.source,
        costPrice :props.layer.row?.costPrice>0?props.layer.row?.costPrice:0, 
        costPriceUnitName:props.layer.row?.costPriceUnitName,
        refPurchPrice :props.layer.row?.refPurchPrice>0?props.layer.row?.refPurchPrice:0, 
        refPurchPriceUnitName:props.layer.row?.refPurchPriceUnitName,
        priceUnitName :props.layer.row?.priceUnitName?props.layer.row?.priceUnitName:'元',
        remark :props.layer.row?.remark,
        
        direction:props.layer.row?.direction,
        productDate:props.layer.row?.productDate,
        expirationDate:props.layer.row?.expirationDate,  
        createUser :props.layer.row?.createUser,
        modifyUser:props.layer.row?.modifyUser,
        isInSAP :props.layer.row?.isInSAP,
        purchaseCycle :props.layer.row?.purchaseCycle,
        purchaseMinimum :props.layer.row?.purchaseMinimum,
        purchaseMinimumUnitName:props.layer.row?.purchaseMinimumUnitName,
        isOldForNew:props.layer.row?.isOldForNew,
        isValid:props.layer.row?.isValid,
        createDate:props.layer.row?.createDate,
        goodsField1 :props.layer.row?.goodsField1,
        goodsField2 :props.layer.row?.goodsField2,
        goodsField3 :props.layer.row?.goodsField3,
        goodsField4 :props.layer.row?.goodsField4,
        goodsField5 :props.layer.row?.goodsField5, 
        photos:props.layer.row?.photos
  })  
  const supplierData=ref(new Array<any>());
  const querySupplierLoading=ref(false);
  const separatorTypeData=ref(new Array<any>());
  const areasData=ref(new Array<any>());
  const unitsData=ref(new Array<any>());
  const photoLimit=ref();
  const workbinSpecData=ref(new Array<any>());

  onMounted(()=>{
    getOptionData();
    if(props.layer.type=='add'){
      ruleForm.value.goodsClassifyId=props.spareClassify.typeId;
      ruleForm.value.goodsClassifyName=props.spareClassify.typeName;
      ruleForm.value.isValid=true;
    }
    else{
        ruleForm.value.goodsClassifyId=props.layer.row.goodsClassifyId;
        ruleForm.value.goodsClassifyName=props.layer.row.goodsClassifyName;
    } 
  })

  const getSpareFields=(fieldsName:string)=>{ 
   let obj= props.layer.data?.filter((x:any)=>x.fieldName==fieldsName) 
    return obj[0]
  }

  const packageCountValid=(rule: any, value: any, callback: any)=>{
     let reg=new RegExp(/^(\+)?\d+(\.\d+)?$/)
     if(!reg.test(value)){
         callback(new Error("包装数量只能为正数"))
     }
     else{
        callback()
     } 
  }

  const sapNoValid=(rule:any,value:any,callback:any)=>{
     if(value&&!ruleForm.value.goodsNo?.trim()){
      callback(new Error("请输入辅材在SAP中的编码"))
     }
     else{
        callback()
     } 
  }

  const specificationValid=(rule:any,value:any,callback:any)=>{
     if(value&&(!ruleForm.value.goodsSpecificationId||Number(ruleForm.value.goodsSpecificationId)==0)){
      tabSelected.value='facade';
      callback(new Error("请选择储存规格"));
     }
     else{
        callback()
     } 
  }

  const rules :any= {
      goodsNo: [{ max: 30, message: '字符超出限制长度', trigger: 'blur'}], 
      goodsName: [{ required: true, message: '请输入辅材名称', trigger: 'blur' },{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
      goodsModel:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],  
      goodsProperty:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],   
      isInSAP:[{validator:sapNoValid,trigger: 'change'}],
      packageCount:[{ required: true, message: '请输入每标准包装的数量', trigger: 'blur' },{validator:packageCountValid,trigger: 'blur'}],  
      maxPackageCount:[{ required: true, message: '请输入每最大包装的数量', trigger: 'blur' },{validator:packageCountValid,trigger: 'blur'}],  
      packageUnitName:[{ required: true, message: '请选择标准包装单位', trigger: 'change' }],
      minPackageUnitName:[{ required: true, message: '请选择最小包装单位', trigger: 'change' }],
      maxPackageUnitName:[{ required: true, message: '请选择最大包装单位', trigger: 'change' }],
      safetyInventoryUnitName:[{ required: true, message: '请选择库存单位', trigger: 'change' }],
      color :[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],  
      source :[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],  
      remark :[{ max:50, message: '字符超出限制长度', trigger: 'blur'}],  
      direction:[{ max:50, message: '字符超出限制长度', trigger: 'blur'}],  
      isConstraintSpec:[{validator:specificationValid,trigger: 'change'}] 
    } 
  

const getOptionData=()=>{
  getOptions().then(res=>{
    separatorTypeData.value=res.data.separatorTypeOptions?.argsOptions;
    areasData.value=res.data.areasOptions;
    unitsData.value=res.data.unitsOptions.filter((res:any)=>res.type=="Pack");
    workbinSpecData.value=res.data.workbinSpecOptions;
    photoLimit.value=Number.parseInt(res.data.photoLimit)
  })
} 

const onSelectPackageUnit=(val:any)=>{ 
  ruleForm.value.maxPackageUnitId=ruleForm.value.packageUnitId;
  ruleForm.value.maxPackageUnitName=ruleForm.value.packageUnitName;
  ruleForm.value.minPackageUnitId=ruleForm.value.packageUnitId;
  ruleForm.value.minPackageUnitName=ruleForm.value.packageUnitName;
  ruleForm.value.safetyInventoryUnitId=ruleForm.value.packageUnitId;
  ruleForm.value.safetyInventoryUnitName=ruleForm.value.packageUnitName;
  ruleForm.value.costPriceUnitName=ruleForm.value.packageUnitName;
  ruleForm.value.refPurchPriceUnitName=ruleForm.value.packageUnitName; 
}

const getSupplierData=(val:string)=>{ 
  getSupplierByKey(val.trim()).then(res=>{
    supplierData.value=res.data;
  })
}

const onSelectSupplier=(val:any)=>{ 
  ruleForm.value.supplier=val;
}
 
const uploadParams=ref({
      uploadApi:'/Common/UploadGoodsPhoto', 
      limit:photoLimit,
      imgUrlList: ruleForm.value.photos?.map((x:any)=>{
        return{
          name:x.fileName,
          url:x.url
        }
      }),  
      validFileType:'image',
      validFileSize:10240,
      isEdit:true,
      titile:'点击上传辅材图片',
      width:'80px',
      height:'80px', 
  }) 
 
const imgChanged=(imgList:Array<any>)=>{  
 ruleForm.value.photos=imgList.map(x=>{
   return{
     fileName:x.name,
     url:x.url
   }
 })
}
 
const submit=()=> {  
      formRef.value.validate((valid:any) => { 
        if (valid) {    
          debugger
            if((ruleForm.value.long||ruleForm.value.wide||ruleForm.value.height) &&!ruleForm.value.sizeUnitName  ) {
               msg.warningAuto("若填写了尺寸相关属性则请选择对应尺寸单位")
               return
            } 
              if(ruleForm.value.weight &&!ruleForm.value.weightUnitName  ) {
               msg.warningAuto("若填写了重量属性则请选择对应重量单位")
               return
            } 
            if(ruleForm.value.maxStock&&Number(ruleForm.value.maxStock)>0 &&(!ruleForm.value.goodsSpecificationId||Number(ruleForm.value.goodsSpecificationId)==0)) {
               msg.warningAuto("若填写了最大堆放量则请选择储存规格")
               return
            } 
             if(ruleForm.value.costPrice &&(!ruleForm.value.priceUnitName||!ruleForm.value.costPriceUnitName)  ) {
               msg.warningAuto("若填写了成本价格则请选择对应价格单位和计数单位")
               return
            }
            if(ruleForm.value.refPurchPrice &&(!ruleForm.value.priceUnitName||!ruleForm.value.refPurchPriceUnitName)) {
               msg.warningAuto("若填写了采购参考价则请选择对应价格单位和计数单位")
               return
            } 
          ruleForm.value.long=ruleForm.value.long?ruleForm.value.long:0;
          ruleForm.value.wide=ruleForm.value.wide?ruleForm.value.wide:0;
          ruleForm.value.height=ruleForm.value.height?ruleForm.value.height:0;
          ruleForm.value.weight=ruleForm.value.weight?ruleForm.value.weight:0;
          ruleForm.value.maxStock=ruleForm.value.maxStock?ruleForm.value.maxStock:0;
          ruleForm.value.costPrice=ruleForm.value.costPrice?ruleForm.value.costPrice:0;
          ruleForm.value.refPurchPrice=ruleForm.value.refPurchPrice?ruleForm.value.refPurchPrice:0; 
          ruleForm.value.goodsSpecificationId=ruleForm.value.goodsSpecificationId?ruleForm.value.goodsSpecificationId:0;
          unitsData.value.forEach((x:any)=>{
            if(x.value==ruleForm.value.packageUnitName){
              ruleForm.value.packageUnitId=x.key 
            }
            if(x.value==ruleForm.value.minPackageUnitName){
              ruleForm.value.minPackageUnitId=x.key
            }
            if(x.value==ruleForm.value.maxPackageUnitName){
              ruleForm.value.maxPackageUnitId=x.key
            }
            if(x.value==ruleForm.value.safetyInventoryUnitName){
              ruleForm.value.safetyInventoryUnitId=x.key
            }
          }); 
          if (props.layer.row) {   
            ruleForm.value.modifyUser=permission.getOperator().userName;
            emit('dataSubmit', ruleForm.value,'update')
           } else {
            ruleForm.value.createUser=permission.getOperator().userName;
            emit('dataSubmit', ruleForm.value,'add') 
          }
        } else {
          return false;
        }
      }); 
    }   
</script>

<style lang="scss" scoped>
  .avatar {
  width: 50px;
  height: 50px;
  display: block;
  border:1px solid rgb(230, 230, 230);
  .el-form-item-mg{
      margin-left: 20px;
    } 
}
.unit-tag{
      margin-top:5px;
      margin-right:-5px;
      color:rgb(192, 190, 190);
      font-size:large
    }
</style>