<template>
    <Layer :layer="layer" @confirm="submit" >
       <el-form :model="ruleForm" :rules="formRules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
        <el-row>
          <el-col :span="11">
               <el-form-item label="收货单号" prop="orderNo">
                <el-input v-model="ruleForm.orderNo" disabled placeholder="系统生成 无需填写" ></el-input>
           </el-form-item>
          </el-col>
          <el-col :span="11" :offset="2">
            <el-form-item label="物料分类" prop="goodsClassify">
                <el-select v-model="ruleForm.goodsClassify" class="m-2" :disabled="true"  style="width:100%" @change="onClassifyChanged" placeholder="请选择收货物料大类 *必填">
                              <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                            </el-option>
                      </el-select>
              </el-form-item>  
          </el-col>
        </el-row> 
        <el-row>
            <el-col :span="11">
            <el-form-item label="订单号" prop="externalOrderNo">
              <el-input v-model="ruleForm.externalOrderNo" :disabled="!props.layer.showButton" placeholder="SAP订单号" />
              </el-form-item>
          </el-col>
          <el-col :span="11" :offset="2"> 
            <el-form-item label="发票号" prop="invoiceNumber">
              <el-input v-model="ruleForm.invoiceNumber" :disabled="!props.layer.showButton" placeholder="发票号" />
              </el-form-item> 
          </el-col>  
        </el-row>
        <el-row>
            <el-col :span="11">
            <el-form-item label="物料编号" prop="goodsNo">
              <el-input v-model="ruleForm.goodsNo" :disabled="!props.layer.showButton" readonly :title="ruleForm.goodsNo"  @click="onShowGoodsDrawer" placeholder="物料号"></el-input> 
              </el-form-item>
          </el-col>
          <el-col :span="11" :offset="2"> 
            <el-form-item label="物料类型" prop="goodsClassifyName">
              <el-input v-model="ruleForm.goodsClassifyName" :disabled="!props.layer.showButton"  :title="ruleForm.goodsClassifyName" placeholder="物料类型"></el-input> 
              </el-form-item> 
          </el-col>  
        </el-row>  
        <el-row>
            <el-col :span="11">
            <el-form-item label="优先级" prop="receivingLevel">
              <el-select v-model="ruleForm.receivingLevel" class="m-2" :disabled="!props.layer.showButton" style="width:100%" placeholder="选择优先级" >
                        <el-option v-for="item in receivingLevelData" :key="item.key" :label="item.value" :value="item.key">
                      </el-option>
                      </el-select>
              </el-form-item>
          </el-col>
          <el-col :span="11" :offset="2"> 
            <el-form-item label="收货数量" prop="quantity">
              <el-input-number v-model="ruleForm.quantity" @input="onInputQty(curSelectedGoods)" :disabled="!props.layer.showButton" style="width:100%" :min="0" controls-position="right" placeholder="收货数量" type="number" /> 
              </el-form-item> 
          </el-col>  
        </el-row>  
        <el-row>
            <el-col :span="11">
            <el-form-item label="紧急数量" prop="quantityUrgency">
              <el-input-number v-model="ruleForm.quantityUrgency" :disabled="!props.layer.showButton||ruleForm.receivingLevel!='Urgent_Especial'" style="width:100%" :min="0" controls-position="right" placeholder="紧急数量" type="number" /> 
              </el-form-item>
          </el-col>
          <el-col :span="11" :offset="2"> 
            <el-form-item label="数量单位" prop="quantityUnitId">
              <el-select v-model="ruleForm.quantityUnitId" :disabled="!props.layer.showButton" class="m-2" style="width:100%;margin-left:5px" placeholder="数量单位" @change="onSelectUnit(ruleForm)">
                      <el-option v-for="item in unitData.filter(f=>f.type=='Pack')" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
                    </el-select>
              </el-form-item> 
          </el-col>  
        </el-row>
        <el-row>
            <el-col :span="11">
            <el-form-item label="料盘数" prop="workpieceTray">
              <el-input-number v-model="ruleForm.workpieceTray" :disabled="!props.layer.showButton" :min="0" style="width: 100%" controls-position="right"  placeholder="料盘数" type="number" />
              </el-form-item>
          </el-col>
          <el-col :span="11" :offset="2"> 
            <el-form-item label="托数/桶数" prop="pallet">
              <el-input-number v-model="ruleForm.pallet" :disabled="!props.layer.showButton" :min="0" style="width: 100%" controls-position="right"  placeholder="托数/桶数" type="number"/>
              </el-form-item> 
          </el-col>  
        </el-row>
        <el-row>
            <el-col :span="11">
            <el-form-item label="供应商" prop="supplierName">
              <el-select style="width: 100%;"
                      :disabled="!props.layer.showButton"
                      v-model="ruleForm.supplierName" 
                      filterable
                      remote
                      reserve-keyword
                      placeholder="输入供应商关键字查询"
                      :remote-method="getSupplierData"
                      @change="supplierSelectChanged(ruleForm)"
                      :loading="supplierSearchLoading">
                      <el-option v-for="item in supplierData" :value="item.supplierName">
                        <span style="float: left">{{ item.supplierName }}</span>
                        <span style="float: right; color: #8492a6; font-size: 13px">{{ item.supplierNo }}</span>
                      </el-option> 
                    </el-select>
              </el-form-item>
          </el-col>
          <el-col :span="11" :offset="2"> 
            <el-form-item label="运单号" prop="waybillNo">
              <el-input v-model="ruleForm.waybillNo" :disabled="!props.layer.showButton" placeholder="物流运单号" />
              </el-form-item> 
          </el-col>  
        </el-row> 
         <el-row>
          <el-col :span="11">
              <el-form-item label="计价单位" prop="priceUnitId">
                      <el-select v-model="ruleForm.priceUnitId" :disabled="!props.layer.showButton" class="m-2" style="width:100%" placeholder="选择计价单位 *必填" @change="priceUnitChanged">
                        <el-option v-for="item in unitData.filter(x=>x.type=='Currency')" :key="item.key" :label="item.value" :value="item.key">
                      </el-option>
                </el-select>
            </el-form-item> 
          </el-col>
          <el-col :span="11" :offset="2">
          <el-form-item label="收货总价" prop="totalPrice">
           <el-input v-model="ruleForm.totalPrice" :disabled="!props.layer.showButton"  controls-position="right"  style="width:100%" @blur="onInputTotalPrice" type="number" placeholder="收货总价" />
         </el-form-item>
          </el-col>
        </el-row>  
           <el-row>
            <el-col :span="11">
              <el-form-item label="收货责任人" prop="receivingResponsableUserFullName"> 
              <el-select style="width: 100%;"
                  v-model="ruleForm.receivingResponsableUserFullName" 
                  :disabled="!props.layer.showButton"
                  filterable
                  remote
                  reserve-keyword
                  placeholder="输入收货人关键字查询"
                  :remote-method="getUserData" 
                  @change="userSelectChanged"
                  :loading="userSearchLoading">
                  <el-option v-for="item in userData" :key="item.userId"  :label="item.userName+' '+item.email"  :value="item.authAccount" /> 
                </el-select>
            </el-form-item> 
            </el-col>
          <el-col :span="11" :offset="2">
            <el-form-item label="收货地址" prop="receivingAddress">
           <el-input v-model="ruleForm.receivingAddress" :disabled="!props.layer.showButton" placeholder="收货地址" />
         </el-form-item> 
          </el-col>
        </el-row>  
           <el-row>
            <el-col :span="11">
            <el-form-item label="预计到货日期" prop="expectDate">
                <el-date-picker v-model="ruleForm.expectDate" :disabled="!props.layer.showButton" style="width:100%" type="date" value-format="YYYY-MM-DD"  placeholder="请选择日期"> </el-date-picker>
              </el-form-item>
          </el-col>
          <el-col :span="11" :offset="2"> 
            <el-form-item label="备注" prop="remark">
                <el-input v-model="ruleForm.remark" :disabled="!props.layer.showButton"/>
              </el-form-item> 
          </el-col>  
        </el-row>   
        <el-row>
          <el-col :span="11">
            <el-form-item label="预计停线时间" prop="downTime">
                <el-date-picker v-model="ruleForm.downTime" :disabled="!props.layer.showButton" style="width:100%" type="datetime"   placeholder="请选择预计停线时间"> </el-date-picker>
              </el-form-item>
          </el-col>
            <el-col :span="11" :offset="2">
            <el-form-item label="ASN收货" prop="isASN">
              <el-checkbox v-model="ruleForm.isASN" :disabled="!props.layer.showButton" label="是"/> 
              </el-form-item>
          </el-col>  
        </el-row> 
             <GoodsSelectDrawer :options="goodsDrawerOptions" v-if="goodsDrawerOptions.show" @selectItem="onSelectGoods"/>
       </el-form> 
    </Layer> 
  </template>
  
  <script lang="ts" setup>
  import {  ref,defineEmits,defineProps,onMounted } from 'vue'; 
  import Layer from '@/components/layer/index.vue'; 
  import { ElForm } from 'element-plus'; 
  import msg from '@/utils/system/message';
  import{getOptions} from'@/api/purchase/receivingorder';
  import{getUserByKey,getSupplierByKey,GetGoodsById}from '@/api/common';
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
  const userData=ref(new Array<any>()); 
  const userSearchLoading=ref(false);
  const receivingLevelData=ref(new Array<any>());  
  const unitData=ref(new Array<any>());   
  const supplierData=ref(new Array<any>()); 
  const invTitle=ref(''); 
  const goodsClassifyData=ref(new Array<any>());  
  const formRef= ref(ElForm||null);   
  const curSelectedGoods:any=ref(); 
  const ruleForm = ref({
      orderNo:props.layer.data?.orderNo,
      externalOrderNo:props.layer.data?.externalOrderNo,   
      totalPrice:props.layer.data?.totalPrice?Number(props.layer.data?.totalPrice).toFixed(2):0,
      priceUnitId:props.layer.data?.priceUnitId,
      priceUnitName:props.layer.data?.priceUnitName,
      expectDate:props.layer.data?.expectDate,  
      remark:props.layer.data?.remark,  
      downTime:commonHelper.formatToDateTime(props.layer.data?.downTime),
      goodsClassify:props.layer.data?.goodsClassify||props.layer.options?.goodsGroup,  
      receivingResponsableUserId:props.layer.data?.receivingResponsableUserId,
      receivingResponsableUserName:props.layer.data?.receivingResponsableUserName,
      receivingResponsableUserEmail:props.layer.data?.receivingResponsableUserEmail,
      receivingResponsableUserFullName:props.layer.data?.receivingResponsableUserName?props.layer.data?.receivingResponsableUserName+' '+props.layer.data?.receivingResponsableUserEmail:'',
      receivingAddress:props.layer.data?.receivingAddress, 
      createUserId:props.layer.data?.createUserId,
      createUserName:props.layer.data?.createUserName, 
      createDate:props.layer.data?.createDate,
      goodsId:props.layer.data?.goodsId,
      goodsNo:props.layer.data?.goodsNo,
      goodsClassifyName:props.layer.data?.goodsClassifyName,
      receivingLevel:props.layer.data?.receivingLevel||"Common",
      quantity:props.layer.data?.quantity,
      quantityUrgency:props.layer.data?.quantityUrgency,
      quantityUnitId:props.layer.data?.quantityUnitId,
      quantityUnitName:props.layer.data?.quantityUnitName,
      quantityUnitList:props.layer.data?.quantityUnitList,
      workpieceTray:props.layer.data?.workpieceTray,
      pallet:props.layer.data?.pallet,
      supplierId:props.layer.data?.supplierId,
      supplierName:props.layer.data?.supplierName,
      waybillNo:props.layer.data?.waybillNo, 
      invoiceNumber:props.layer.data?.invoiceNumber, 
      isASN:props.layer.data?.isASN,
      details:new Array<any>() 
  });  
   
  const checkExpectDate=(rule: any, value: any, callback: any)=>{
    if(value){  
      var res=commonHelper.isDateBeforeToday(value);  
       if(res){
         return callback(new Error('预计到货日期不能小于当前日期'));
       }
    }
    callback();
  }

  const validateQuantity = (rule: any, value: any, callback: any) => {
    if (!value) {
      callback(new Error('请输入收货数量'));
    } else {
      if (Number(value)<=0) { 
        callback(new Error('收货数量必须大于0'))
      }
      callback()
    }
  }
  const formRules = {  
      expectDate:[{ required: true, message: '请选择预计到货日期', trigger: 'change' },{ validator: checkExpectDate, trigger: 'change' }],
      goodsClassify:[{ required: true, message: '请选择物料类型', trigger: 'change' }], 
      externalOrderNo:[{ required: true, message: '请填写SAP订单号', trigger: 'blur' },{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
      invoiceNumber:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],    
      receivingAddress:[{ required: true, message: '请填写收货地址', trigger: 'blur' },{ max: 50, message: '字符超出限制长度', trigger: 'blur'}], 
      remark:[{ max: 100, message: '字符超出限制长度', trigger: 'blur'}] ,
      goodsNo:[{ required: true, message: '请输入物料编码', trigger: 'change' }],
      receivingLevel:[{ required: true, message: '请选择优先级', trigger: 'change' }],
      quantity:[{ required: true, message: '请输入收货数量', trigger: 'change' },{ validator: validateQuantity, trigger: 'blur' }],
      quantityUnitId:[{ required: true, message: '请选择数量单位', trigger: 'change' }],
      supplierName:[{ required: true, message: '请选择供应商', trigger: 'change' }]
  }  
  const goodsDrawerOptions= ref({
    show: false,
    title: '', 
    type:'',
    mode:'no-repet',
    isMultiSelect:false,
    data:new Array<any>() 
  });
 
  onMounted(()=>{ 
    console.log("ruleForm.value.goodsNo",ruleForm.value.goodsNo) 
    getOptions().then((res:any)=>{ 
        receivingLevelData.value=res.data.receivingLevelOptions; 
        unitData.value=res.data.unitOptions;
        ruleForm.value.priceUnitName='元';
        ruleForm.value.priceUnitId=unitData.value.find(f=>f.value==ruleForm.value.priceUnitName).key;
        if(!ruleForm.value.receivingAddress){
          ruleForm.value.receivingAddress=res.data.receivingAddress
        } 
        goodsClassifyData.value=res.data.goodsClassifyOptions;
        onClassifyChanged(props.layer.data?.goodsClassify)
        if(props.layer.data?.details.length>0){ 
          var goodsDetail=  props.layer.data.details[0];
          ruleForm.value.externalOrderNo=goodsDetail.externalOrderNo;
          ruleForm.value.goodsId=goodsDetail.goodsId;
          ruleForm.value.goodsNo=goodsDetail.goodsNo;
          ruleForm.value.goodsClassifyName=goodsDetail.goodsClassifyName;
          ruleForm.value.receivingLevel=goodsDetail.receivingLevel;
          ruleForm.value.quantity=goodsDetail.quantity;
          ruleForm.value.quantityUnitId=goodsDetail.quantityUnitId;
          ruleForm.value.quantityUnitName=goodsDetail.quantityUnitName;
          ruleForm.value.quantityUrgency=goodsDetail.quantityUrgency; 
          ruleForm.value.workpieceTray=goodsDetail.workpieceTray;
          ruleForm.value.pallet=goodsDetail.pallet;
          ruleForm.value.supplierId=goodsDetail.supplierId;
          ruleForm.value.supplierName=goodsDetail.supplierName;
          ruleForm.value.waybillNo=goodsDetail.waybillNo;
          ruleForm.value.invoiceNumber=goodsDetail.invoiceNumber;
          ruleForm.value.isASN=goodsDetail.isASN ; 
          ruleForm.value.downTime=commonHelper.formatToDateTime(goodsDetail.downTime);
          GetGoodsById(ruleForm.value.goodsId,ruleForm.value.goodsClassify).then(res=>{
            curSelectedGoods.value=res.data;
          })
        } 
      })
  }); 
  
  const onClassifyChanged=(val:any)=>{  
    if(val)
    invTitle.value=goodsClassifyData.value.find(f=>f.key==val).value; 
  }
     
  const getUserData=(keyword:string)=>{
    userSearchLoading.value=true;
    getUserByKey(keyword).then((res:any)=>{
      userData.value=res.data;
    }).finally(()=>userSearchLoading.value=false)
  }

  const userSelectChanged=(value:any)=>{  
    let user=userData.value.find(f=>f.authAccount==value); 
    if(user){
      ruleForm.value.receivingResponsableUserId=user.userId;
      ruleForm.value.receivingResponsableUserName=user.userName;
      ruleForm.value.receivingResponsableUserEmail=user.email;
      ruleForm.value.receivingResponsableUserFullName=user.userName+' '+user.email;
    }
  }
   

  const priceUnitChanged=()=>{
    ruleForm.value.priceUnitName=unitData.value.find(x=>x.key==ruleForm.value.priceUnitId).value
  } 

  const onInputTotalPrice=()=>{
    ruleForm.value.totalPrice=Number(ruleForm.value.totalPrice).toFixed(2);
  }

  const getSupplierData=(keyword:string)=>{
    supplierSearchLoading.value=true
    getSupplierByKey(keyword).then((res:any)=>{
      supplierData.value=res.data 
    }).finally(()=>  supplierSearchLoading.value=false)
  } 

  const supplierSelectChanged=(row:any)=>{  
    let supplierName=row.supplierName;
    let obj:any=supplierData.value.find((x:any)=>x.supplierName==supplierName)
    if(obj){ 
        row.supplierId=obj.supplierId;
    }   
  }

  const onSelectUnit=(detail:any)=>{
      let unitObj:any=unitData.value.find(f=>f.key==detail.quantityUnitId);
      if(unitObj){
        detail.quantityUnitName=unitObj.value;
      }
  }
 
     
  const onShowGoodsDrawer=()=>{ 
    goodsDrawerOptions.value.show=true;
    goodsDrawerOptions.value.type=ruleForm.value.goodsClassify;
    goodsDrawerOptions.value.title=invTitle.value+'选择';
    goodsDrawerOptions.value.data=[
    {
      goodsId:ruleForm.value.goodsId
    }
    ]
  }
   
  const onSelectGoods=(goodsArr:Array<any>)=>{     
    curSelectedGoods.value=goodsArr[0];
    ruleForm.value.externalOrderNo=goodsArr[0].goodsField1;
    ruleForm.value.goodsId=goodsArr[0].goodsId;
    ruleForm.value.goodsNo=goodsArr[0].goodsNo;
    ruleForm.value.goodsClassifyName=goodsArr[0].goodsClassifyName; 
    ruleForm.value.supplierName=goodsArr[0].supplier; 
    ruleForm.value.quantityUnitId=unitData.value.find(f=>f.value==goodsArr[0].minPackageUnitName).key;
    ruleForm.value.quantityUnitName=goodsArr[0].minPackageUnitName; 
    ruleForm.value.workpieceTray=0;
    ruleForm.value.pallet=0;
    onInputQty(goodsArr[0]); 
}
     
 //当收货单位是个时（收货单位=物料最新包装单位）：
//如果最大包装单位是托，则需要计算托盘数=(收货数量/标准包装数)/最大包装数
//如果最大包装单位是盘，则需要计算料盘数=(收货数量/标准包装数)
const onInputQty=(selectedGoods:any)=>{
  if(ruleForm.value.quantity>0){
      if(true){ //ruleForm.value.quantityUnitName=='个'
      if(selectedGoods.maxPackageUnitName=='盘'){
        if(selectedGoods.packageCount>0){
            ruleForm.value.workpieceTray= Math.ceil(Number(Number(ruleForm.value.quantity)/selectedGoods.packageCount)); 
          } 
        }
        else{
          if(selectedGoods.packageCount>0&&selectedGoods.maxPackageCount>0){
            ruleForm.value.pallet= Math.ceil(Number((Number(ruleForm.value.quantity)/selectedGoods.packageCount)/selectedGoods.maxPackageCount)); 
          } 
        }
      } 
    }   
}
  const submit=()=> {      
      formRef.value.validate((valid:any)=>{  
          if(valid){  
            ruleForm.value.details=[{
              goodsId: ruleForm.value.goodsId,
              goodsNo: ruleForm.value.goodsNo,
              goodsClassifyName:ruleForm.value.goodsClassifyName,
              receivingLevel:ruleForm.value.receivingLevel,
              quantity:ruleForm.value.quantity,
              quantityUnitId:ruleForm.value.quantityUnitId,
              quantityUnitName:ruleForm.value.quantityUnitName,
              quantityUrgency:ruleForm.value.quantityUrgency,
              workpieceTray:ruleForm.value.workpieceTray, 
              pallet:ruleForm.value.pallet,
              supplierId:ruleForm.value.supplierId,
              supplierName:ruleForm.value.supplierName,
              waybillNo:ruleForm.value.waybillNo,
              invoiceNumber:ruleForm.value.invoiceNumber,
              isASN:ruleForm.value.isASN,
              externalOrderNo:ruleForm.value.externalOrderNo,
              downTime:ruleForm.value.downTime?commonHelper.formatToDateTime(ruleForm.value.downTime):null,
            }]
            emit('dataSubmit', ruleForm.value,props.layer.data?'update':'add'); 
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
    .popper-cls{
      color: #888;
      font-size: small;
    }
  </style>