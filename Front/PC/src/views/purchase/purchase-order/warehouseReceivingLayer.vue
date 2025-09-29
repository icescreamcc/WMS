<template>
    <Layer :layer="layer" @confirm="submit" >
       <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
        <el-tabs v-model="tabSelected" style="box-shadow:none;-webkit-box-shadow:none;min-height:380px">
          <el-tab-pane label="实物到货信息" name="arrived">
            <el-row>
                <el-col :span="11">
                <el-form-item label="到货状态" prop="arrivalStatus">
                  <el-select v-model="ruleForm.arrivalStatus" class="m-2" :disabled="!props.layer.showButton"  style="width:100%" placeholder="选择到货状态">     
                            <el-option v-for="item in orderReceiveStatusData" :value="item.key" :label="item.value"></el-option> 
                    </el-select> 
                  </el-form-item>
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="到货时间" prop="arrivalDate">
                  <el-date-picker v-model="ruleForm.arrivalDate" :disabled="!props.layer.showButton" style="width:100%" type="datetime"  placeholder="选择到货时间"> </el-date-picker>   
                  </el-form-item> 
              </el-col>  
            </el-row>
            <el-row>
                <el-col :span="11">
                <el-form-item label="存储规格" prop="goodsSpecificationId">
                  <el-select v-model="ruleForm.goodsSpecificationId" class="m-2" :disabled="!props.layer.showButton"  style="width:100%" placeholder="选择存储规格">     
                            <el-option v-for="item in workbinSpecData" :value="item.key" :label="item.value"></el-option> 
                    </el-select> 
                  </el-form-item>
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="最大堆放量" prop="maxStock">
                  <el-input-number v-model="ruleForm.maxStock" :disabled="!props.layer.showButton" style="width:100%" :min="0" controls-position="right" placeholder="每个库位最多存放数量" type="number" />   
                  </el-form-item> 
              </el-col>  
            </el-row>
            <el-row >
              <el-col :span="11">
                <el-form-item label="实际收货数量" prop="quantityArrival">
                  <el-input-number v-model="ruleForm.quantityArrival" :disabled="!props.layer.showButton" style="width:100%" :min="0" controls-position="right" placeholder="实际收货数量" type="number" /> 
                  </el-form-item>  
              </el-col>
                  <el-col :span="11" :offset="2" v-if="ruleForm.arrivalStatus=='Abnormal'">
                    <el-form-item label="收货异常原因" prop="arrivalAbnormalReason">
                    <el-input v-model="ruleForm.arrivalAbnormalReason"  :disabled="!props.layer.showButton" style="width:100%" :min="0"  placeholder="收货异常原因"/> 
                    </el-form-item> 
                </el-col> 
              </el-row> 
            <el-row>
                  <el-col :span="11">
                    <el-form-item label="实物图片">
                      <ImgUpload :uploadParams="pictureUploadParams"  @handleImgChanged="imgChanged"/>
                    </el-form-item> 
                </el-col> 
              </el-row> 
          </el-tab-pane> 
          <el-tab-pane label="物料基本信息" name="base">
            <el-row>
              <el-col :span="11">
                  <el-form-item label="内部订单号" prop="orderNo">
                    <el-input v-model="ruleForm.orderNo" disabled placeholder="系统生成 无需填写" ></el-input>
              </el-form-item>
              </el-col>
              <el-col :span="11" :offset="2">
                <el-form-item label="物料分类" prop="goodsClassifyGroup"> 
                    <el-select v-model="ruleForm.goodsClassifyGroup" class="m-2" :disabled="true"  style="width:100%"  placeholder="请选择收货物料大类 *必填">
                                  <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key">
                                </el-option>
                          </el-select>
                  </el-form-item>  
              </el-col>
            </el-row> 
            <el-row>
                <el-col :span="11">
                  <el-form-item label="采购方式" prop="purchaseTypeId">
                    <el-select v-model="ruleForm.purchaseTypeId" class="m-2" :disabled="true"  style="width:100%" placeholder="请选择采购方式 *必填">
                                  <el-option v-for="item in purchaseTypeData" :key="item.optionId" :label="item.optionName" :value="item.optionId">
                                </el-option>
                          </el-select>
                  </el-form-item>   
              </el-col>
              <el-col :span="11" :offset="2">   
                <el-form-item label="物料型号" prop="goodsModel">
                  <el-input v-model="ruleForm.goodsModel" :disabled="true"  :title="ruleForm.goodsModel" placeholder="物料型号"></el-input> 
                  </el-form-item> 
              </el-col>  
            </el-row>
            <el-row>
                <el-col :span="11">
                  <el-form-item label="是否申请料号" prop="isApplyMaterialNumber">
                  <el-checkbox v-model="ruleForm.isApplyMaterialNumber" :disabled="true" label="是"/> 
                  </el-form-item> 
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="是否采买" prop="isPurchaseBuy">
                  <el-checkbox v-model="ruleForm.isPurchaseBuy" :disabled="true" label="是"/> 
                  </el-form-item> 
              </el-col>  
            </el-row>
            <el-row>
                <el-col :span="11"> 
                <el-form-item label="物料名称(中文)" prop="goodsNameZH">
                  <el-input v-model="ruleForm.goodsNameZH" :disabled="true"  :title="ruleForm.goodsNameZH" placeholder="物料中文描述">  
                  </el-input> 
                  </el-form-item>
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="物料名称(英文)" prop="goodsNameEN">
                  <el-input v-model="ruleForm.goodsNameEN" :disabled="true"  :title="ruleForm.goodsNameEN" placeholder="物料英文描述"></el-input> 
                  </el-form-item> 
              </el-col>  
            </el-row>   
            <el-row>
                <el-col :span="11"> 
                  <el-form-item label="NPM采购" prop="npmBuyer"> 
                    <el-select style="width: 100%;" v-model="ruleForm.npmBuyer"  allow-create :disabled="true" filterable @change="onSelectNPMBuyer"  placeholder="检索或添加NPM采购" >
                        <el-option v-for="item in npmBuyerData" :key="item.key"  :label="item.key+' '+item.value"  :value="item.key"/> 
                      </el-select>
                  </el-form-item>
              </el-col>  
              <el-col :span="11" :offset="2">
                <el-form-item label="NPM采购邮箱" prop="npmBuyerEMail">
                  <el-input v-model="ruleForm.npmBuyerEMail" :disabled="true" placeholder="NPM采购邮箱"/>
                  </el-form-item>
              </el-col> 
            </el-row>
            <el-row>
              <el-col :span="11">
                <el-form-item label="最小订货量" prop="minLotSize">
                  <el-input-number v-model="ruleForm.minLotSize"  :disabled="true" style="width:100%" :min="0" controls-position="right" placeholder="最小订货量" type="number" /> 
                  </el-form-item>  
              </el-col>
              <el-col :span="11" :offset="2">
                <el-form-item label="回货周期(周)" prop="returnCycle">
                  <el-input-number v-model="ruleForm.returnCycle" :disabled="true" style="width:100%" :min="0" controls-position="right" placeholder="回货周期" type="number" /> 
                </el-form-item> 
              </el-col>
            </el-row> 
            <el-row>
              <el-col :span="11">
                <el-form-item label="安全库存" prop="safetyInventory">
                  <el-input-number v-model="ruleForm.safetyInventory" :disabled="true" style="width:100%" :min="0" controls-position="right" placeholder="安全库存" type="number" /> 
                  </el-form-item> 
              </el-col>
              <el-col :span="11" :offset="2">
                <el-form-item label="是否一次性购买" prop="isOnceBuy">
                  <el-checkbox v-model="ruleForm.isOnceBuy" :disabled="true" label="是"/>  
                  </el-form-item> 
              </el-col>
            </el-row> 
          </el-tab-pane>
          <el-tab-pane label="下单通用信息" name="booking">
            <el-row>
                <el-col :span="11">
                <el-form-item label="供应商" prop="supplierName">
                  <el-select style="width: 100%;"
                          :disabled="true"
                          v-model="ruleForm.supplierName" 
                          allow-create
                          filterable
                          remote
                          reserve-keyword
                          placeholder="输入供应商关键字查询"
                          :remote-method="getSupplierData"
                          @change="supplierSelectChanged"
                          :loading="supplierSearchLoading">
                          <el-option v-for="item in supplierData" :label="item.supplierName"  :value="item.supplierName" /> 
                        </el-select>
                  </el-form-item>
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="供应商代码" prop="supplierNo">
                  <el-input v-model="ruleForm.supplierNo" :disabled="true" placeholder="供应商代码"/>
                  </el-form-item>
              </el-col> 
            </el-row>  
            <el-row>
              <el-col :span="11">
                <el-form-item label="本次采买数量" prop="quantity">
                  <el-input-number v-model="ruleForm.quantity" :disabled="true" style="width:100%" :min="0" controls-position="right" placeholder="本次采买数量" type="number" />  
                  </el-form-item>  
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="数量单位" prop="quantityUnitId">
                  <el-select v-model="ruleForm.quantityUnitId" :disabled="true" class="m-2" style="width:100%;margin-left:5px" placeholder="数量单位" @change="onSelectUnit(ruleForm)">
                          <el-option v-for="item in unitData.filter(f=>f.type=='Pack')" :key="item.key" :label="item.value" :value="item.key">
                        </el-option>
                        </el-select>
                  </el-form-item> 
              </el-col> 
            </el-row>
            <el-row>
              <el-col :span="11"> 
                <el-form-item label="生产厂家" prop="manufactor">
                  <el-input v-model="ruleForm.manufactor" :disabled="true" placeholder="生产厂家"/>
                  </el-form-item>
              </el-col> 
            </el-row>
          </el-tab-pane>
          <el-tab-pane label="下单CEOS信息" name="ceos" v-if="ruleForm.isPurchaseBuy&&ruleForm.purchaseTypeDesc.includes('CEOS')"> 
              <el-row>
                  <el-col :span="11">
                    <el-form-item label="采购科目" prop="accountNumber"> 
                    <el-select style="width: 100%;" v-model="ruleForm.accountNumber" :disabled="true" placeholder="选择采购科目" >
                        <el-option v-for="item in purchaseAccountData" :key="item.optionKey"  :value="item.optionKey">
                          <span style="float: left">{{ item.optionKey }}</span>
                          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.optionName }}</span>
                        </el-option> 
                      </el-select>
                    </el-form-item> 
                </el-col>
                <el-col :span="11" :offset="2"> 
                  <el-form-item label="PM类型" prop="pmType"> 
                    <el-select style="width: 100%;" v-model="ruleForm.pmType" :disabled="true" placeholder="选择PM类型" >
                        <el-option v-for="item in pmTypeData" :key="item.optionKey" :value="item.optionKey">
                          <span style="float: left">{{ item.optionKey }}</span>
                          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.optionName }}</span>
                          </el-option> 
                      </el-select>
                    </el-form-item> 
                </el-col>  
              </el-row>  
              <el-row>
                  <el-col :span="11">
                    <el-form-item label="CPMG" prop="cpmg"> 
                    <el-select style="width: 100%;" v-model="ruleForm.cpmg" :disabled="true" placeholder="选择CPMG" >
                        <el-option v-for="item in purchaseCPMGData" :key="item.optionKey"   :value="item.optionKey">
                          <span style="float: left">{{ item.optionKey }}</span>
                          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.optionName }}</span>
                          </el-option> 
                      </el-select>
                    </el-form-item> 
                </el-col>
                <el-col :span="11" :offset="2"> 
                  <el-form-item label="CEOS申请原因" prop="applyReason">
                    <el-input v-model="ruleForm.applyReason" :disabled="true" placeholder="CEOS申请原因"/>
                    </el-form-item>
                </el-col>  
              </el-row>  
              <el-row style="height: 200px;">
                  <el-col :span="11">
                    <el-form-item label="报价单" prop="quoteLink"> 
                      <el-link style="display: block;" target="_blank" type="info" v-for="file in ruleForm.quoteLinkFileList" :href="file.url">{{ file.fileName }}<i class="el-icon-view el-icon--right"></i> </el-link> 
                    </el-form-item>  
                </el-col>  
              </el-row>  
          </el-tab-pane>
          <el-tab-pane label="可供筛选信息" name="fitler">
            <el-row>
              <el-col :span="11"> 
                        <el-form-item label="需求者" prop="consigneeFullName"> 
                          <el-select style="width: 100%;"
                              v-model="ruleForm.consigneeFullName" 
                              :disabled="true"
                              filterable
                              remote
                              reserve-keyword
                              placeholder="输入需求者关键字查询"
                              :remote-method="getUserData" 
                              @change="userSelectChanged"
                              :loading="userSearchLoading">
                              <el-option v-for="item in userData" :key="item.userId"  :label="item.userName+' '+item.email"  :value="item.authAccount" /> 
                            </el-select>
                        </el-form-item> 
                    </el-col>  
                    <el-col :span="11" :offset="2"> 
                <el-form-item label="申请日期" prop="createDate">
                  <el-date-picker v-model="ruleForm.createDate" :disabled="true" style="width:100%" type="date" value-format="YYYY-MM-DD"  placeholder="申请日期"> </el-date-picker>   
                  </el-form-item>
              </el-col> 
            </el-row>
            <el-row> 
              <el-col :span="11"> 
                <el-form-item label="产线名称" prop="line">
                  <el-select v-model="ruleForm.line" class="m-2" :disabled="true" @change="onSelectLine" style="width:100%" placeholder="请选择产线名称 *必填">
                        <el-option v-for="item in lineData" :key="item.optionKey" :label="item.optionKey" :value="item.optionKey">
                          <span style="float: left">{{ item.optionKey }}</span>
                          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.optionName }}</span>
                      </el-option>
                </el-select>
                </el-form-item> 
              </el-col>  
              <el-col :span="11" :offset="2"> 
                <el-form-item label="产线编码" prop="lineNo">
                  <el-select v-model="ruleForm.lineNo" class="m-2" :disabled="true" @change="onSelectLineNo" style="width:100%" placeholder="请选择产线编码 *必填">
                        <el-option v-for="item in lineData" :key="item.optionKey" :label="item.optionName" :value="item.optionName">
                          <span style="float: left">{{ item.optionName }}</span>
                          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.optionKey }}</span>
                      </el-option>
                </el-select>
                </el-form-item> 
              </el-col>
            </el-row>
          </el-tab-pane>
          <el-tab-pane label="申请SAP号信息" name="apply" v-if="ruleForm.isApplyMaterialNumber">
            <el-row>
                  <el-col :span="11">
                    <el-form-item label="类别编号" prop="classesNumber"> 
                    <el-select style="width: 100%;" v-model="ruleForm.classesNumber" :disabled="true" placeholder="选择类别" @change="onSelectClassesNumber">
                        <el-option v-for="item in purchaseClassesData" :key="item.optionKey"  :label="item.optionKey"  :value="item.optionKey">
                          <span style="float: left">{{ item.optionKey }}</span>
                          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.optionName }}</span>
                        </el-option> 
                      </el-select> 
                    </el-form-item> 
                </el-col>
                <el-col :span="11" :offset="2"> 
                  <el-form-item label="类别描述" prop="classesNumberDesc">
                    <el-input v-model="ruleForm.classesNumberDesc" :disabled="true" placeholder="类别描述"/>
                    </el-form-item>
                </el-col>  
              </el-row> 
            <el-row>
                <el-col :span="11"> 
                  <el-form-item label="物料分类" prop="goodsClassifyId">
                    <el-select v-model="ruleForm.goodsClassifyId" class="m-2" :disabled="true"  style="width:100%" placeholder="请选择物料类型 *必填" @change="onSelectGoodsClassifyName">
                          <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                        </el-option>
                  </el-select>
                  </el-form-item> 
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="MNA编码" prop="goodsNo">
                  <el-input v-model="ruleForm.goodsNo" :disabled="true" placeholder="MNA编码"/>
                  </el-form-item>
              </el-col>  
            </el-row> 
            <el-row>
                <el-col :span="11"> 
                  <el-form-item label="备注" prop="remark">
                  <el-input v-model="ruleForm.remark"  :disabled="true" style="width:100%" :min="0"  placeholder="备注" /> 
                  </el-form-item>
              </el-col>   
            </el-row>  
          </el-tab-pane>
          <el-tab-pane label="PO单通用信息" name="po">
            <el-row>
              <el-col :span="11">
                <el-form-item label="单价" prop="price">
                  <el-input-number v-model="ruleForm.price" @input="onInputTotalPrice" :disabled="true" style="width:100%" :min="0" controls-position="right" placeholder="采购单价" type="number" /> 
                  </el-form-item>  
              </el-col>
              <el-col :span="11" :offset="2">
                <el-form-item label="总价" prop="detailTotalPrice">
                  <el-input-number v-model="ruleForm.detailTotalPrice" :disabled="true" style="width:100%" :min="0" controls-position="right" placeholder="采购总价" type="number" /> 
                </el-form-item> 
              </el-col>
            </el-row> 
            <el-row>
              <el-col :span="11">
                <el-form-item label="计价单位" prop="priceUnitName">
                          <el-select v-model="ruleForm.priceUnitName" :disabled="true" class="m-2" style="width:100%" placeholder="选择计价单位 *必填">
                            <el-option v-for="item in unitData.filter(x=>x.type=='Currency')" :key="item.key" :label="item.value" :value="item.value">
                          </el-option>
                    </el-select>
                </el-form-item> 
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="下单实际数量" prop="quantityActual">
                  <el-input-number v-model="ruleForm.quantityActual" :disabled="true" style="width:100%" :min="0" controls-position="right" placeholder="下单实际数量" type="number" />  
                  </el-form-item> 
              </el-col>  
            </el-row> 
            <el-row>
                <el-col :span="11">
                <el-form-item label="PO日期" prop="poDate">
                  <el-date-picker v-model="ruleForm.poDate" :disabled="true" style="width:100%" type="date" value-format="YYYY-MM-DD"  placeholder="选择PO日期"> </el-date-picker> 
                  </el-form-item>
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="PO号" prop="poNumber">
                  <el-input v-model="ruleForm.poNumber" :disabled="true" style="width:100%"  placeholder="PO号"/>  
                  </el-form-item> 
              </el-col>  
            </el-row> 
            <el-row>
                <el-col :span="11">
                <el-form-item label="PR日期" prop="prDate">
                  <el-date-picker v-model="ruleForm.prDate" :disabled="true" style="width:100%" type="date" value-format="YYYY-MM-DD"  placeholder="选择PR日期"> </el-date-picker> 
                  </el-form-item>
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="PR号" prop="prNumber">
                  <el-input v-model="ruleForm.prNumber" :disabled="true" style="width:100%"  placeholder="PR号"/>  
                  </el-form-item> 
              </el-col>  
            </el-row>
           
            <el-row>  
              <el-col :span="11">
                <el-form-item label="PO描述" prop="description">
                  <el-input v-model="ruleForm.description" :disabled="true" placeholder="PO描述"/>
                </el-form-item> 
            </el-col>  
          </el-row> 
          </el-tab-pane> 
        </el-tabs>     
       </el-form> 
    </Layer> 
  </template>
  
  <script lang="ts" setup>
  import {  ref,defineEmits,defineProps,onMounted } from 'vue'; 
  import Layer from '@/components/layer/index.vue'; 
  import { ElForm } from 'element-plus'; 
  import msg from '@/utils/system/message';
  import {getGoodsGroup,getGoodsClassify} from '@/api/common'; 
  import{getOptions} from'@/api/purchase/purchaseOrder';
  import{getUserByKey,getSupplierByKey}from '@/api/common';
  import permission from '@/utils/system/permission'; 
  import {Delete} from '@element-plus/icons-vue'; 
  import commonHelper from "@/utils/system/common-helper";
  import ImgUpload from '@/components/imgUpload/muiltUpload.vue';
  import FileUpload from '@/components/imgUpload/fileUpload.vue';

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
  const unitData=ref(new Array<any>());   
  const supplierData=ref(new Array<any>()); 
  const invTitle=ref(''); 
  const goodsGroupData=ref(new Array<any>());
  const goodsClassifyData=ref(new Array<any>());  
  const lineData=ref(new Array<any>());   
  const orderStatusData=ref(new Array<any>());
  const orderFlowStatusData=ref(new Array<any>());
  const orderReceiveStatusData=ref(new Array<any>());
  const purchaseTypeData=ref(new Array<any>()); 
  const purchaseCPMGData=ref(new Array<any>()); 
  const purchaseAccountData=ref(new Array<any>()); 
  const purchaseClassesData=ref(new Array<any>()); 
  const workbinSpecData=ref(new Array<any>()); 
  const npmBuyerData=ref(new Array<any>()); 
  const pmTypeData=ref(new Array<any>()); 
  const formRef= ref(ElForm||null);   
  const curSelectedGoods:any=ref(); 
  const tabSelected=ref('arrived');
  const photoLimit=ref(); 
  const ruleForm = ref({
      detialId:props.layer.data?.detialId, 
      orderNo:props.layer.data?.orderNo,
      externalOrderNo:props.layer.data?.externalOrderNo,  
      purchaseTypeId:props.layer.data?.purchaseTypeId, 
      purchaseTypeDesc:props.layer.data?.purchaseTypeDesc,
      isApplyMaterialNumber:props.layer.data?.isApplyMaterialNumber, 
      isPurchaseBuy:props.layer.data?.isPurchaseBuy, 
      goodsNameZH:props.layer.data?.goodsNameZH, 
      goodsNameEN:props.layer.data?.goodsNameEN, 
      goodsModel:props.layer.data?.goodsModel, 
      goodsId:props.layer.data?.goodsId, 
      goodsNo:props.layer.data?.goodsNo, 
      goodsClassifyGroup:props.layer.data?.goodsClassifyGroup, 
      goodsClassifyId:props.layer.data?.goodsClassifyId, 
      goodsClassifyName:props.layer.data?.goodsClassifyName, 
      
      npmBuyer:props.layer.data?.npmBuyer,
      npmBuyerEMail:props.layer.data?.npmBuyerEMail,
      mnaNo:props.layer.data?.mnaNo,
      poNumber:props.layer.data?.poNumber,
      poDate:commonHelper.formatToDate(props.layer.data?.poDate),
      prNumber:props.layer.data?.prNumber,
      prDate:commonHelper.formatToDate(props.layer.data?.prDate),
      arrivalStatus:props.layer.data?.arrivalStatus,
      quantityArrival:props.layer.data?.quantityArrival,
      arrivalDate:commonHelper.formatToDateTime(props.layer.data?.arrivalDate)|| commonHelper.formatToDateTime(new Date()),
      receivingStatus:props.layer.data?.receivingStatus,
      receivingDate:props.layer.data?.receivingDate,
      pmType:props.layer.data?.pmType,
      cpmg:props.layer.data?.cpmg,
      accountNumber:props.layer.data?.accountNumber,
      classesNumberDesc:props.layer.data?.classesNumberDesc,
      classesNumber:props.layer.data?.classesNumber,
      classesKey:props.layer.data?.classesKey,
      minLotSize:props.layer.data?.minLotSize,
      returnCycle:props.layer.data?.returnCycle,
      arrivalAbnormalReason:props.layer.data?.arrivalAbnormalReason,
      receivingAbnormalReason:props.layer.data?.receivingAbnormalReason,
      manufactor:props.layer.data?.manufactor,
      goodsSpecificationId:(!props.layer.data?.goodsSpecificationId||props.layer.data?.goodsSpecificationId==0)?'':props.layer.data?.goodsSpecificationId,
      maxStock:props.layer.data?.maxStock||0,
      isOnceBuy:props.layer.data?.isOnceBuy,

      quantity:props.layer.data?.quantity,
      quantityUnitId:props.layer.data?.quantityUnitId,
      quantityUnitName:props.layer.data?.quantityUnitName, 
      quantityActual:props.layer.data?.quantityActual, 
      price:props.layer.data?.price, 
      detailTotalPrice:props.layer.data?.detailTotalPrice?Number(props.layer.data?.detailTotalPrice).toFixed(2):0, 
      priceUnitName:props.layer.data?.priceUnitName,
      status:props.layer.data?.status, 
      flowStatus:props.layer.data?.flowStatus, 
      description :props.layer.data?.description,   
      line:props.layer.data?.line,  
      lineNo: props.layer.data?.lineNo, 
      applyReason:props.layer.data?.applyReason,  
      supplierId:props.layer.data?.supplierId,
      supplierNo:props.layer.data?.supplierNo,
      supplierName:props.layer.data?.supplierName,
      quoteLinkFileList:props.layer.data?.quoteLinkFileList, 
      goodsPictureList:props.layer.data?.goodsPictureList,
      safetyInventory:props.layer.data?.safetyInventory, 
      consignee:props.layer.data?.consignee, 
      consigneeId:0,
      consigneeName:'',
      consigneeFullName:props.layer.data?.consignee?props.layer.data?.consignee+' '+props.layer.data?.consigneeEmail:'',
      consigneeEmail:props.layer.data?.consigneeEmail, 
      isAutoEmailToReceiving :props.layer.data?.isAutoEmailToReceiving, 
      remark:props.layer.data?.remark,  
      createUserId:props.layer.data?.createUserId,
      createUserName:props.layer.data?.createUserName, 
      updateUserId:permission.getOperator().userId,
      updateUserName:permission.getOperator().userName,
      createDate:commonHelper.formatToDate(props.layer.data?.createDate)
  });  
   
  const quoteLinkUploadParams=ref({
      uploadApi:'/Common/UploadFile', 
      limit:3,
      fileUrlList:ruleForm.value.quoteLinkFileList?.map((x:any)=>{
        return{
          name:x.fileName,
          url:x.url
        }
      }),  
      validFileType:'pdf',
      validFileSize:20,
      isEdit:true,
      titile:'点击上传报价单',
      width:'250px',
      height:'80px', 
  });
  
  const pictureUploadParams=ref({
    uploadApi:'/Common/UploadGoodsPhoto', 
      limit:photoLimit.value,
      imgUrlList: ruleForm.value.goodsPictureList?.map((x:any)=>{
        return{
          name:x.fileName,
          url:x.url
        }
      }),  
      validFileType:'image',
      validFileSize:1024*5,
      isEdit:true,
      titile:'点击上传物料图片',
      width:'80px',
      height:'80px', 
  });
 
  const validateQuantity = (rule: any, value: any, callback: any) => {
    if (Number(ruleForm.value.quantityArrival)<=0) { 
        callback(new Error('实际收货数量必须大于0'))
      }
      else if(Number(ruleForm.value.quantityArrival)!=ruleForm.value.quantityActual){
        if(ruleForm.value.arrivalStatus!='Abnormal'){
          callback(new Error('实际收货数量与下单实际数量不一致时，请选择异常到货'))
        } 
      }
      callback()
  }
 
  const validateStatus = (rule: any, value: any, callback: any) => {
    if(ruleForm.value.arrivalStatus=='NoArrived'&&Number(ruleForm.value.quantityArrival)>0){
      callback(new Error('如果填写了实际收货数量，则请调整收货状态'))
    } 
    if(Number(ruleForm.value.quantityArrival)>0&&Number(ruleForm.value.quantityArrival)!=ruleForm.value.quantityActual){
        if(ruleForm.value.arrivalStatus!='Abnormal'){
          callback(new Error('实际收货数量与下单实际数量不一致时，请选择异常到货'))
        } 
      }
      callback()
  }

  const validateReason = (rule: any, value: any, callback: any) => {
    if(ruleForm.value.arrivalStatus=='Abnormal'){
       if(!ruleForm.value.arrivalAbnormalReason){
        callback(new Error('收货异常时，请填写异常原因'));
       }
    }
    callback()
  }

  const validateMaxStock = (rule: any, value: any, callback: any) => {
    if(value){
      if(Number(value)<0){
        callback(new Error('最大堆放数量必须大于0'));
      } 
      }
    callback()
  }
 
  const rules = {  
      quantityArrival:[{ required: true, message: '请填写实际收货数量', trigger: 'blur' },{ validator: validateQuantity, trigger: 'blur' }],
      arrivalAbnormalReason:[{ max: 200, message: '输入的字符数不能超过200个', trigger: 'blur'},{ validator: validateReason, trigger: 'change' }],
      maxStock:[{ validator: validateMaxStock, trigger: 'change' }],
      arrivalStatus:[{ validator: validateStatus, trigger: 'change' }] 
  }  
    
  onMounted(()=>{ 
    getOptions().then((res:any)=>{ 
        goodsGroupData.value=res.data.goodsClassifyGroupOptions;
        unitData.value=res.data.unitOptions; 
        orderStatusData.value=res.data.orderStatusOptions;
        orderFlowStatusData.value=res.data.orderFlowStatusOptions
        orderReceiveStatusData.value=res.data.orderReceiveStatusOptions;
        purchaseTypeData.value=res.data.purchaseTypeOptions;  
        purchaseCPMGData.value=res.data.purchaseCPMGOptions;  
        purchaseAccountData.value=res.data.purchaseAccountOptions;  
        purchaseClassesData.value=res.data.purchaseClassesOptions;  
        workbinSpecData.value=res.data.workbinSpecOptions;  
        pmTypeData.value=res.data.pmTypeOptions;
        lineData.value=res.data.lineOptions;
        lineData.value.unshift({optionKey:'All',optionName:'All'});
        npmBuyerData.value=res.data.npmBuyerOptions;
        invTitle.value=goodsGroupData.value.find(f=>f.key==ruleForm.value.goodsClassifyGroup).value;
        photoLimit.value=Number.parseInt(res.data.photoLimit)
      })
      getGoodsClassify(ruleForm.value.goodsClassifyGroup).then(res=>{
        goodsClassifyData.value=res.data;
      })
  }); 
   
  const getUserData=(keyword:string)=>{
    userSearchLoading.value=true;
    getUserByKey(keyword).then((res:any)=>{
      userData.value=res.data;
    }).finally(()=>userSearchLoading.value=false)
  }

  const userSelectChanged=(value:any)=>{  
    let user=userData.value.find(f=>f.authAccount==value); 
    if(user){
      ruleForm.value.consigneeId=user.userId;
      ruleForm.value.consignee=user.userName;
      ruleForm.value.consigneeEmail=user.email;
      ruleForm.value.consigneeFullName=user.userName+' '+user.email;
    }
  }
    
  const onSelectGoodsClassifyName=()=>{
    ruleForm.value.goodsClassifyName=goodsClassifyData.value.find(f=>f.key==ruleForm.value.goodsClassifyId).value;
  } 

  const onInputTotalPrice=()=>{
    if(ruleForm.value.price&&ruleForm.value.quantity)
      ruleForm.value.detailTotalPrice=Number(ruleForm.value.price)*Number(ruleForm.value.quantity);
  }

  const onSelectNPMBuyer=()=>{
     var buyers=npmBuyerData.value.filter(f=>f.key==ruleForm.value.npmBuyer);
     if(buyers&&buyers.length>0){
      ruleForm.value.npmBuyerEMail=buyers[0].value;
     }
  }

  const onSelectLine=()=>{
    if(ruleForm.value.line){
      ruleForm.value.lineNo=lineData.value.find(f=>f.optionKey==ruleForm.value.line).optionName;
    } 
  }

  const onSelectLineNo=()=>{
    if(ruleForm.value.lineNo){
      ruleForm.value.line=lineData.value.find(f=>f.optionName==ruleForm.value.lineNo).optionKey;
    } 
  }
  
  const imgChanged=(imgList:Array<any>)=>{  
    ruleForm.value.goodsPictureList=imgList.map(x=>{
      return{
        fileName:x.name,
        url:x.url
      }
    })
  }
  const getSupplierData=(keyword:string)=>{
    supplierSearchLoading.value=true
    getSupplierByKey(keyword).then((res:any)=>{
      supplierData.value=res.data 
    }).finally(()=>  supplierSearchLoading.value=false)
  } 

  const supplierSelectChanged=()=>{   
    let obj:any=supplierData.value.find((x:any)=>x.supplierName==ruleForm.value.supplierName)
    if(obj){ 
      ruleForm.value.supplierId=obj.supplierId;
      ruleForm.value.supplierNo=obj.supplierNo;
    }   
  }

  const onSelectUnit=(detail:any)=>{
      let unitObj:any=unitData.value.find(f=>f.key==detail.quantityUnitId);
      if(unitObj){
        detail.quantityUnitName=unitObj.value;
      }
  }

  const onSelectClassesNumber=()=>{
    let obj =purchaseClassesData.value.find(f=>f.optionKey==ruleForm.value.classesNumber);
    if(obj){
      ruleForm.value.classesNumberDesc=obj.optionName;
    }
  } 
  
  const submit=()=> {      
      formRef.value.validate((valid:any)=>{  
          if(valid){    
            if(!ruleForm.value.goodsSpecificationId){
              ruleForm.value.goodsSpecificationId=0;
            }
            ruleForm.value.updateUserId=permission.getOperator().userId;
            ruleForm.value.updateUserName=permission.getOperator().userName;
            emit('dataSubmit', ruleForm.value); 
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
  </style>