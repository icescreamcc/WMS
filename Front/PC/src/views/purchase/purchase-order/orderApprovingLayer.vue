<template>
    <Layer :layer="layer" @confirm="submit" @otherEvent="save">
       <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
        <el-tabs v-model="tabSelected" style="box-shadow:none;-webkit-box-shadow:none;min-height:380px">
          <el-tab-pane label="审批" name="approval">
            <el-row>
             <el-col>
                  <el-form-item label="审批意见" prop="opinion">
                  <el-input v-model="ruleForm.approvalOpinion" type="textarea" rows="4"/>
                </el-form-item>
             </el-col>
           </el-row>
             <el-row>
              <el-col :span="11">
                  <el-form-item label="审批结果" prop="approvalResult" style="text-align:left">
                       <el-radio-group v-model="ruleForm.approvalResult" @change="onSelectApprovalResult">
                         <el-radio-button :key="'Approve'" :label="'Approve'">审批通过</el-radio-button> 
                         <el-radio-button :key="'Reject'" :label="'Reject'">审批拒绝</el-radio-button>  
                      </el-radio-group> 
              </el-form-item>
              </el-col> 
            </el-row>   
        </el-tab-pane> 
        <el-tab-pane label="PO单通用信息" name="po">
            <el-row>
              <el-col :span="11">
                <el-form-item label="单价" prop="price">
                  <el-input-number v-model="ruleForm.price" @input="onInputTotalPrice" :disabled="!ruleForm.isLastApproval" style="width:100%" :min="0" controls-position="right" placeholder="采购单价" type="number" /> 
                  </el-form-item>  
              </el-col>
              <el-col :span="11" :offset="2">
                <el-form-item label="总价" prop="detailTotalPrice">
                  <el-input-number v-model="ruleForm.detailTotalPrice" :disabled="!ruleForm.isLastApproval" style="width:100%" :min="0" controls-position="right" placeholder="采购总价" type="number" /> 
                </el-form-item> 
              </el-col>
            </el-row> 
            <el-row>
              <el-col :span="11">
                <el-form-item label="计价单位" prop="priceUnitName">
                          <el-select v-model="ruleForm.priceUnitName" :disabled="!ruleForm.isLastApproval" class="m-2" style="width:100%" placeholder="选择计价单位 *必填">
                            <el-option v-for="item in unitData.filter(x=>x.type=='Currency')" :key="item.key" :label="item.value" :value="item.value">
                          </el-option>
                    </el-select>
                </el-form-item> 
              </el-col>
              <el-col :span="11" :offset="2">  
                <el-form-item label="下单实际数量" prop="quantityActual">
                  <el-input-number v-model="ruleForm.quantityActual" :disabled="!ruleForm.isLastApproval" style="width:100%" :min="0" controls-position="right" placeholder="下单实际数量" type="number" />  
                  </el-form-item> 
              </el-col>  
            </el-row> 
            <el-row>
                <el-col :span="11">
                <el-form-item label="PO日期" prop="poDate">
                  <el-date-picker v-model="ruleForm.poDate" :disabled="!ruleForm.isLastApproval" style="width:100%" type="date" value-format="YYYY-MM-DD"  placeholder="选择PO日期"> </el-date-picker> 
                  </el-form-item>
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="PO号" prop="poNumber">
                  <el-input v-model="ruleForm.poNumber" :disabled="!ruleForm.isLastApproval" style="width:100%" @blur="onInputNumber" placeholder="PO号"/>  
                  </el-form-item> 
              </el-col>  
            </el-row> 
            <el-row>
                <el-col :span="11">
                <el-form-item label="PR日期" prop="prDate">
                  <el-date-picker v-model="ruleForm.prDate" :disabled="!ruleForm.isLastApproval" style="width:100%" type="date" value-format="YYYY-MM-DD"  placeholder="选择PR日期"> </el-date-picker> 
                  </el-form-item>
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="PR号" prop="prNumber">
                  <el-input v-model="ruleForm.prNumber" :disabled="!ruleForm.isLastApproval" style="width:100%" @blur="onInputNumber"  placeholder="PR号"/>  
                  </el-form-item> 
              </el-col>  
            </el-row> 
            <el-row>  
              <el-col :span="11">
                <el-form-item label="PO描述" prop="description">
                  <el-input v-model="ruleForm.description" :disabled="!ruleForm.isLastApproval" placeholder="PO描述"/>
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
                    <el-select v-model="ruleForm.goodsClassifyGroup" class="m-2" :disabled="!ruleForm.isLastApproval"  style="width:100%"  placeholder="请选择收货物料大类 *必填">
                                  <el-option v-for="item in goodsGroupData" :key="item.key" :label="item.value" :value="item.key">
                                </el-option>
                          </el-select>
                  </el-form-item>  
              </el-col>
            </el-row> 
            <el-row>
                <el-col :span="11">
                  <el-form-item label="采购方式" prop="purchaseTypeId">
                    <el-select v-model="ruleForm.purchaseTypeId" class="m-2" :disabled="!ruleForm.isLastApproval" @change="onSelectPurchaseType" style="width:100%" placeholder="请选择采购方式 *必填">
                                  <el-option v-for="item in purchaseTypeData" :key="item.optionId" :label="item.optionName" :value="item.optionId">
                                </el-option>
                          </el-select>
                  </el-form-item>   
              </el-col>
              <el-col :span="11" :offset="2"> 
                  <el-form-item label="物料型号" prop="goodsModel">
                  <el-input v-model="ruleForm.goodsModel" :disabled="!ruleForm.isLastApproval"  :title="ruleForm.goodsModel" placeholder="物料型号"></el-input> 
                  </el-form-item> 
              </el-col>
            </el-row>
            <el-row>
                <el-col :span="11">
                  <el-form-item label="是否申请料号" prop="isApplyMaterialNumber">
                  <el-checkbox v-model="ruleForm.isApplyMaterialNumber" :disabled="!ruleForm.isLastApproval" label="是"/> 
                  </el-form-item> 
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="是否采买" prop="isPurchaseBuy">
                  <el-checkbox v-model="ruleForm.isPurchaseBuy" :disabled="!ruleForm.isLastApproval" label="是"/> 
                  </el-form-item> 
              </el-col>  
            </el-row>
            <el-row>
                <el-col :span="11"> 
                <el-form-item label="物料名称(中文)" prop="goodsNameZH">
                  <el-input v-model="ruleForm.goodsNameZH" :disabled="!ruleForm.isLastApproval"  :title="ruleForm.goodsNameZH" placeholder="物料中文描述">  
                  </el-input> 
                  </el-form-item>
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="物料名称(英文)" prop="goodsNameEN">
                  <el-input v-model="ruleForm.goodsNameEN" :disabled="!ruleForm.isLastApproval"  :title="ruleForm.goodsNameEN" placeholder="物料英文描述"></el-input> 
                  </el-form-item> 
              </el-col>  
            </el-row>   
            <el-row>
                <el-col :span="11"> 
                  <el-form-item label="NPM采购" prop="npmBuyer"> 
                    <el-select style="width: 100%;" v-model="ruleForm.npmBuyer"  allow-create :disabled="!ruleForm.isLastApproval" filterable @change="onSelectNPMBuyer"  placeholder="检索或添加NPM采购" >
                        <el-option v-for="item in npmBuyerData" :key="item.key"  :label="item.key+' '+item.value"  :value="item.key"/> 
                      </el-select>
                  </el-form-item>
              </el-col>  
              <el-col :span="11" :offset="2">
                <el-form-item label="NPM采购邮箱" prop="npmuyerEMail">
                  <el-input v-model="ruleForm.npmBuyerEMail" :disabled="!ruleForm.isLastApproval" placeholder="NPM采购邮箱"/>
                  </el-form-item>
              </el-col> 
            </el-row>
            <el-row>
              <el-col :span="11">
                <el-form-item label="最小订货量" prop="minLotSize">
                  <el-input-number v-model="ruleForm.minLotSize"  :disabled="!ruleForm.isLastApproval" style="width:100%" :min="0" controls-position="right" placeholder="最小订货量" type="number" /> 
                  </el-form-item>  
              </el-col>
              <el-col :span="11" :offset="2">
                <el-form-item label="回货周期(周)" prop="returnCycle">
                  <el-input-number v-model="ruleForm.returnCycle" :disabled="!ruleForm.isLastApproval" style="width:100%" :min="0" controls-position="right" placeholder="回货周期" type="number" /> 
                </el-form-item> 
              </el-col>
            </el-row> 
            <el-row>
              <el-col :span="11">
                <el-form-item label="安全库存" prop="safetyInventory">
                  <el-input-number v-model="ruleForm.safetyInventory" :disabled="!ruleForm.isLastApproval" style="width:100%" :min="0" controls-position="right" placeholder="安全库存" type="number" /> 
                  </el-form-item> 
              </el-col>
              <el-col :span="11" :offset="2">
                <el-form-item label="是否一次性购买" prop="isOnceBuy">
                  <el-checkbox v-model="ruleForm.isOnceBuy" :disabled="!ruleForm.isLastApproval" label="是"/>  
                  </el-form-item> 
              </el-col>
            </el-row> 
          </el-tab-pane>
          <el-tab-pane label="下单通用信息" name="booking">
            <el-row>
                <el-col :span="11">
                <el-form-item label="供应商" prop="supplierName">
                  <el-select style="width: 100%;"
                          :disabled="!ruleForm.isLastApproval"
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
                  <el-input v-model="ruleForm.supplierNo" :disabled="!ruleForm.isLastApproval" placeholder="供应商代码"/>
                  </el-form-item>
              </el-col> 
            </el-row>  
            <el-row>
              <el-col :span="11">
                <el-form-item label="本次采买数量" prop="quantity">
                  <el-input-number v-model="ruleForm.quantity" :disabled="!ruleForm.isLastApproval||!ruleForm.isPurchaseBuy" style="width:100%" :min="0" controls-position="right" placeholder="本次采买数量" type="number" />  
                  </el-form-item>  
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="数量单位" prop="quantityUnitId">
                  <el-select v-model="ruleForm.quantityUnitId" :disabled="!ruleForm.isLastApproval" class="m-2" style="width:100%;margin-left:5px" placeholder="数量单位" @change="onSelectUnit(ruleForm)">
                          <el-option v-for="item in unitData.filter(f=>f.type=='Pack')" :key="item.key" :label="item.value" :value="item.key">
                        </el-option>
                        </el-select>
                  </el-form-item> 
              </el-col> 
            </el-row>
            <el-row>
              <el-col :span="11"> 
                <el-form-item label="生产厂家" prop="manufactor">
                  <el-input v-model="ruleForm.manufactor" :disabled="!ruleForm.isLastApproval" placeholder="生产厂家"/>
                  </el-form-item>
              </el-col> 
            </el-row>
          </el-tab-pane>
          <el-tab-pane label="下单CEOS信息" name="ceos" v-if="ruleForm.isPurchaseBuy&&ruleForm.purchaseTypeDesc?.includes('CEOS')"> 
              <el-row>
                  <el-col :span="11">
                    <el-form-item label="采购科目" prop="accountNumber"> 
                    <el-select style="width: 100%;" v-model="ruleForm.accountNumber" filterable  :disabled="!ruleForm.isLastApproval" placeholder="选择采购科目" >
                        <el-option v-for="item in purchaseAccountData" :key="item.optionKey"  :value="item.optionKey">
                          <span style="float: left">{{ item.optionKey }}</span>
                          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.optionName }}</span>
                        </el-option> 
                      </el-select>
                    </el-form-item> 
                </el-col>
                <el-col :span="11" :offset="2"> 
                  <el-form-item label="PM类型" prop="pmType"> 
                    <el-select style="width: 100%;" v-model="ruleForm.pmType" filterable  :disabled="!ruleForm.isLastApproval" placeholder="选择PM类型" >
                        <el-option v-for="item in pmTypeData" :key="item.optionKey"  :value="item.optionKey">
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
                    <el-select style="width: 100%;" v-model="ruleForm.cpmg" filterable  :disabled="!ruleForm.isLastApproval" placeholder="选择CPMG" >
                        <el-option v-for="item in purchaseCPMGData" :key="item.optionKey" :value="item.optionKey">
                          <span style="float: left">{{ item.optionKey }}</span>
                          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.optionName }}</span>
                          </el-option> 
                      </el-select>
                    </el-form-item> 
                </el-col>
                <el-col :span="11" :offset="2"> 
                  <el-form-item label="CEOS申请原因" prop="applyReason">
                    <el-input v-model="ruleForm.applyReason" :disabled="!ruleForm.isLastApproval" placeholder="CEOS申请原因"/>
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
                              :disabled="!ruleForm.isLastApproval"
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
                  <el-date-picker v-model="ruleForm.createDate" :disabled="!ruleForm.isLastApproval" style="width:100%" type="date" value-format="YYYY-MM-DD"  placeholder="申请日期"> </el-date-picker>   
                  </el-form-item>
              </el-col> 
            </el-row>
            <el-row> 
              <el-col :span="11"> 
                <el-form-item label="产线名称" prop="line">
                  <el-select v-model="ruleForm.line" class="m-2" filterable  :disabled="!ruleForm.isLastApproval" @change="onSelectLine" style="width:100%" placeholder="请选择产线名称 *必填">
                        <el-option v-for="item in lineData" :key="item.optionKey" :label="item.optionKey" :value="item.optionKey">
                          <span style="float: left">{{ item.optionKey }}</span>
                          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.optionName }}</span>
                      </el-option>
                </el-select>
                </el-form-item> 
              </el-col>  
              <el-col :span="11" :offset="2"> 
                <el-form-item label="产线编码" prop="lineNo">
                  <el-select v-model="ruleForm.lineNo" class="m-2" filterable  :disabled="!ruleForm.isLastApproval" @change="onSelectLineNo" style="width:100%" placeholder="请选择产线编码 *必填">
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
                    <el-select style="width: 100%;" v-model="ruleForm.classesNumber" filterable  :disabled="!ruleForm.isLastApproval" placeholder="选择类别" @change="onSelectClassesNumber">
                        <el-option v-for="item in purchaseClassesData" :key="item.optionKey"  :label="item.optionKey"  :value="item.optionKey">
                          <span style="float: left">{{ item.optionKey }}</span>
                          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.optionName }}</span>
                        </el-option> 
                      </el-select> 
                    </el-form-item> 
                </el-col>
                <el-col :span="11" :offset="2"> 
                  <el-form-item label="类别描述" prop="classesNumberDesc">
                    <el-input v-model="ruleForm.classesNumberDesc" :disabled="!ruleForm.isLastApproval" readonly placeholder="类别描述"/>
                    </el-form-item>
                </el-col>  
              </el-row> 
            <el-row>
                <el-col :span="11"> 
                  <el-form-item label="物料分类" prop="goodsClassifyId">
                    <el-select v-model="ruleForm.goodsClassifyId" class="m-2" filterable  :disabled="!ruleForm.isLastApproval"  style="width:100%" placeholder="请选择物料类型 *必填" @change="onSelectGoodsClassifyName">
                          <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                        </el-option>
                  </el-select>
                  </el-form-item> 
              </el-col>
              <el-col :span="11" :offset="2"> 
                <el-form-item label="MNA编码" prop="goodsNo">
                 <el-input v-model="ruleForm.goodsNo" :disabled="!ruleForm.isLastApproval" placeholder="MNA编码"/>
                 </el-form-item>
              </el-col>  
            </el-row>  
            <el-row>
              <el-col :span="11">
                <el-form-item label="备注" prop="remark">
                  <el-input v-model="ruleForm.remark"  :disabled="!ruleForm.isLastApproval" style="width:100%" :min="0"  placeholder="备注" /> 
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
            data:null,
            otherButton:null 
          }
        }
      }
  });
  const emit = defineEmits(['dataSubmit','dataSave']); 
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
  const purchaseTypeData=ref(new Array<any>()); 
  const purchaseCPMGData=ref(new Array<any>()); 
  const purchaseAccountData=ref(new Array<any>()); 
  const purchaseClassesData=ref(new Array<any>()); 
  const npmBuyerData=ref(new Array<any>()); 
  const pmTypeData=ref(new Array<any>()); 
  const formRef= ref(ElForm||null);   
  const curSelectedGoods:any=ref(); 
  const tabSelected=ref('approval');
  const photoLimit=ref(); 
  const eventType=ref('submit')
  const ruleForm = ref({
      detialId:props.layer.data?.detialId, 
      orderNo:props.layer.data?.orderNo,
      externalOrderNo:props.layer.data?.externalOrderNo,  
      purchaseTypeId:props.layer.data?.purchaseTypeId, 
      isApplyMaterialNumber:props.layer.data?.isApplyMaterialNumber, 
      isPurchaseBuy:props.layer.data?.isPurchaseBuy, 
      goodsNameZH:props.layer.data?.goodsNameZH, 
      goodsNameEN:props.layer.data?.goodsNameEN, 
      goodsModel:props.layer.data?.goodsModel, 
      goodsId:props.layer.data?.goodsId, 
      goodsNo:props.layer.data?.goodsNo, 
      goodsClassifyGroup:props.layer.data?.goodsClassifyGroup||props.layer.options.goodsGroup, 
      goodsClassifyId:props.layer.data?.goodsClassifyId, 
      goodsClassifyName:props.layer.data?.goodsClassifyName, 
      purchaseTypeDesc:props.layer.data?.purchaseTypeDesc,
      
      npmBuyer:props.layer.data?.npmBuyer,
      npmBuyerEMail:props.layer.data?.npmBuyerEMail,
      mnaNo:props.layer.data?.mnaNo,
      poNumber:props.layer.data?.poNumber,
      poDate:commonHelper.formatToDate(props.layer.data?.poDate),
      prNumber:props.layer.data?.prNumber,
      prDate:commonHelper.formatToDate(props.layer.data?.prDate),
      arrivalStatus:props.layer.data?.arrivalStatus,
      arrivalDate:props.layer.data?.arrivalDate,
      receivingStatus:props.layer.data?.receivingStatus,
      receivingDate:props.layer.data?.receivingDate,
      pmType:props.layer.data?.pmType,
      cpmg:props.layer.data?.cpmg,
      accountNumber:props.layer.data?.accountNumber,
      classesNumberDesc:props.layer.data?.classesNumberDesc,
      classesNumber:props.layer.data?.classesNumber,
      classesKey:props.layer.data?.classesKey,
      minLotSize:props.layer.data?.minLotSize,
      returnCycle:props.layer.data?.returnCycle||3,
      arrivalAbnormalReason:props.layer.data?.arrivalAbnormalReason,
      receivingAbnormalReason:props.layer.data?.receivingAbnormalReason,
      manufactor:props.layer.data?.manufactor,
      goodsSpecificationId:props.layer.data?.goodsSpecificationId,
      maxStock:props.layer.data?.maxStock,
      isOnceBuy:props.layer.data?.isOnceBuy,

      quantity:props.layer.data?.quantity,
      quantityUnitId:props.layer.data?.quantityUnitId,
      quantityUnitName:props.layer.data?.quantityUnitName, 
      quantityActual:props.layer.data?.quantityActual||props.layer.data?.quantity, 
      price:props.layer.data?.price, 
      detailTotalPrice:props.layer.data?.detailTotalPrice||0, 
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
      updateUserId:props.layer.data?.updateUserId,
      updateUserName:props.layer.data?.updateUserName,
      createDate:props.layer.data?.createDate,
      approverId:permission.getOperator().userId,
      approverName:permission.getOperator().userName, 
      approvalDate:props.layer.data?.approvalDate,
      approverRole:props.layer.data?.approverRole,
      approvalStatus:props.layer.data?.approvalStatus,
      approvalResult:'',
      approvalOpinion:'',
      isLastApproval:props.layer.data?.isLastApproval
  });  
    
  const validateQuantity = (rule: any, value: any, callback: any) => {
    if(ruleForm.value.isPurchaseBuy&&ruleForm.value.isLastApproval&&ruleForm.value.approvalResult=='Approve'){
      if (Number(value)<=0) { 
        callback(new Error('采购数量必须大于0'))
      }
    } 
      callback()
  }

  const validatePrice = (rule: any, value: any, callback: any) => {
    if(ruleForm.value.isPurchaseBuy&&ruleForm.value.isLastApproval&&ruleForm.value.approvalResult=='Approve'){
      if(!value){
        callback(new Error('如果勾选了采买，请填写PO单相关信息'));
        tabSelected.value='po';
      }
      else{
        if (Number(value)<=0) { 
          callback(new Error('单价必须大于0'));
          tabSelected.value='po';
        }
      } 
    } 
      callback()
  }

  const validateTotalPrice = (rule: any, value: any, callback: any) => {
    if(ruleForm.value.isPurchaseBuy&&ruleForm.value.isLastApproval&&ruleForm.value.approvalResult=='Approve'){
      if(!value){
        callback(new Error('如果勾选了采买，请填写PO单相关信息'));
        tabSelected.value='po';
      }
      else{
        if (Number(value)<=0) { 
          callback(new Error('总价必须大于0'));
          tabSelected.value='po';
        }
      } 
    } 
      callback()
  }

  const validatePriceUnit = (rule: any, value: any, callback: any) => {
    if(ruleForm.value.isPurchaseBuy&&ruleForm.value.isLastApproval&&ruleForm.value.approvalResult=='Approve'){
      if(!value){
        callback(new Error('如果勾选了采买，请填写PO单相关信息'));
        tabSelected.value='po';
      }
      else{
        if (Number(value)<=0) { 
          callback(new Error('请选择计价单位'));
          tabSelected.value='po';
        }
      } 
    } 
      callback()
  }

  const validateQuantityActual = (rule: any, value: any, callback: any) => {
    if(ruleForm.value.isPurchaseBuy&&ruleForm.value.isLastApproval&&ruleForm.value.approvalResult=='Approve'){
      if(!value){
        callback(new Error('如果勾选了采买，则请填写PO单相关信息'));
        tabSelected.value='po';
      }
      else{
       if (Number(value)!=Number(ruleForm.value.quantity)) {
          callback(new Error('下单实际数量与本次采买数量不一致'));
          tabSelected.value='po';
        }
      } 
    } 
      callback()
  }

  const validateDescription = (rule: any, value: any, callback: any) => {
    if(ruleForm.value.isPurchaseBuy&&ruleForm.value.isLastApproval&&ruleForm.value.approvalResult=='Approve'){
      if(!value){
        callback(new Error('如果勾选了采买，则填写PO单相关信息'));
        tabSelected.value='po';
      }  
    } 
      callback()
  }

  const validatePO = (rule: any, value: any, callback: any) => {
    if(ruleForm.value.isPurchaseBuy&&ruleForm.value.isLastApproval&&ruleForm.value.approvalResult=='Approve'){ 
      if(!value){
        callback(new Error('如果勾选了采买，则请填写PO单相关信息'));
        tabSelected.value='po';
      } 
    }  
      callback()
  }

  const validateMNA=(rule: any, value: any, callback: any)=>{
      if(ruleForm.value.isApplyMaterialNumber&&ruleForm.value.isLastApproval&&ruleForm.value.approvalResult=='Approve'){ 
          if(!value){
            callback(new Error('如果勾选了申请料号，则请填写MNA编号'));
            tabSelected.value='apply';
          }
      }
      callback()
  }

  const validateApprovalResult=(rule: any, value: any, callback: any)=>{
    if(eventType.value=='submit'){
      if(!value){
        callback(new Error('请选择审批结果'));
        tabSelected.value='approval';
     }
    }
      callback()
  }
  
  const rules = {  
      approvalResult:[{ validator: validateApprovalResult, trigger: 'blur' }], 
      goodsClassifyGroup:[{ required: true, message: '请选择物料分类', trigger: 'change' }],
      purchaseTypeId:[{ required: true, message: '请选择采购方式', trigger: 'change' }],    
      externalOrderNo:[{ required: true, message: '请填写SAP订单号', trigger: 'blur' },{ max: 50, message: '输入的字符数不能超过50个', trigger: 'blur'}],
      goodsNameZH:[{ required: true, message: '请填写物料中文名称', trigger: 'blur' },{ max: 100, message: '输入的字符数不能超过100个', trigger: 'blur'}], 
      goodsNameEN:[{ max: 100, message: '输入的字符数不能超过100个', trigger: 'blur'}], 
      flowStatus:[{ required: true, message: '请选择流程状态', trigger: 'change' }],  
      goodsModel:[{ required: true, message: '请物料型号', trigger: 'blur' },{ max: 50, message: '字符超出限制长度', trigger: 'blur'}], 
      
      quantity:[{ required: true, message: '请输入采购数量', trigger: 'blur' },{ validator: validateQuantity, trigger: 'blur' }],
      quantityUnitId:[{ required: true, message: '请选择数量单位', trigger: 'change' }],
      manufactor:[{ max: 100, message: '输入的字符数不能超过100个', trigger: 'blur'}],

      accountNumber:[{ required: true, message: '请选采购科目', trigger: 'change' }],
      pmType:[{ required: true, message: '请选采PM类型', trigger: 'change' }],
      cpmg:[{ required: true, message: '请选采CPMG', trigger: 'change' }],
      applyReason:[{ max: 200, message: '输入的字符数不能超过200个', trigger: 'blur'}],

      consigneeFullName:[{ required: true, message: '请选择收货人', trigger: 'change' }],
      line:[{ required: true, message: '请选择适用产线', trigger: 'change' }], 
      lineNo:[{ required: true, message: '请选择适用产线', trigger: 'change' }],
 
      price:[{ validator: validatePrice, trigger: 'blur' }],
      detailTotalPrice:[{ validator: validateTotalPrice, trigger: 'blur' }],
      priceUnitName:[{ validator: validatePriceUnit, trigger: 'blur' }],
      quantityActual:[{ validator: validateQuantityActual, trigger: 'blur' }], 
      description:[{ max: 100, message: '输入的字符数不能超过100个', trigger: 'blur'},{ validator: validateDescription, trigger: 'blur' }],  
      poNumber:[{  validator: validatePO, trigger: 'blur'}], 
      prNumber:[{  validator: validatePO, trigger: 'blur' }], 
      goodsNo:[{ validator: validateMNA, trigger: 'blur' },{ max: 50, message: '输入的字符数不能超过50个', trigger: 'blur'}],
  }  
    
  onMounted(()=>{ 
    getOptions().then((res:any)=>{ 
        goodsGroupData.value=res.data.goodsClassifyGroupOptions;
        unitData.value=res.data.unitOptions; 
        orderStatusData.value=res.data.orderStatusOptions;
        orderFlowStatusData.value=res.data.orderFlowStatusOptions
        purchaseTypeData.value=res.data.purchaseTypeOptions;  
        purchaseCPMGData.value=res.data.purchaseCPMGOptions;  
        purchaseAccountData.value=res.data.purchaseAccountOptions;  
        purchaseClassesData.value=res.data.purchaseClassesOptions;  
        pmTypeData.value=res.data.pmTypeOptions;
        lineData.value=res.data.lineOptions;
        lineData.value.unshift({optionKey:'All',optionName:'All'});
        npmBuyerData.value=res.data.npmBuyerOptions;
        invTitle.value=goodsGroupData.value.find(f=>f.key==ruleForm.value.goodsClassifyGroup).value;
        photoLimit.value=Number.parseInt(res.data.photoLimit);
        ruleForm.value.purchaseTypeDesc=purchaseTypeData.value.find(f=>f.optionId==ruleForm.value.purchaseTypeId).optionName; 
      })
      getGoodsClassify(ruleForm.value.goodsClassifyGroup).then(res=>{
        goodsClassifyData.value=res.data;
      })
  }); 

  const onInputNumber=()=>{
    if(ruleForm.value.poNumber){
      ruleForm.value.poDate=commonHelper.formatToDate(new Date());
    }
    else{
      ruleForm.value.poDate='';
    }
    if(ruleForm.value.prNumber){
      ruleForm.value.prDate=commonHelper.formatToDate(new Date());
    }
    else{
      ruleForm.value.prNumber='';
    }
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
      ruleForm.value.consigneeId=user.userId;
      ruleForm.value.consignee=user.userName;
      ruleForm.value.consigneeEmail=user.email;
      ruleForm.value.consigneeFullName=user.userName+' '+user.email;
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

  const onSelectPurchaseType=()=>{
    ruleForm.value.purchaseTypeDesc=purchaseTypeData.value.find(f=>f.optionId==ruleForm.value.purchaseTypeId).optionName; 
    if(ruleForm.value.purchaseTypeDesc.indexOf('CEOS')>-1){
      ruleForm.value.isPurchaseBuy=true;
      ruleForm.value.isApplyMaterialNumber=false;
    }
    else{
      ruleForm.value.isPurchaseBuy=false;
      ruleForm.value.isApplyMaterialNumber=true;
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

  const onSelectApprovalResult=()=>{
    if(ruleForm.value.approvalResult){
      if(ruleForm.value.approvalResult=='Approve'){
        ruleForm.value.approvalOpinion="同意";
      }
      else{
        ruleForm.value.approvalOpinion="不同意";
      }
    }
  }
     
  const submit=()=> {      
    eventType.value='submit';
      formRef.value.validate((valid:any)=>{ 
          if(valid){   
            var operationType=props.layer.data?'update':'add';
            if(operationType=='update'){
              ruleForm.value.updateUserId=permission.getOperator().userId;
              ruleForm.value.updateUserName=permission.getOperator().userName;
            }
            if(!ruleForm.value.poDate){
              ruleForm.value.poDate='1900-01-01';
            }
            if(!ruleForm.value.prDate){
              ruleForm.value.prDate='1900-01-01';
            }
            emit('dataSubmit', ruleForm.value); 
          }
      }) 
  }

  const save=(type:string)=>{
    eventType.value='save'; 
    formRef.value.validate((valid:any)=>{ 
          if(valid){   
            var operationType=props.layer.data?'update':'add';
            if(operationType=='update'){
              ruleForm.value.updateUserId=permission.getOperator().userId;
              ruleForm.value.updateUserName=permission.getOperator().userName;
            }
            if(!ruleForm.value.poDate){
              ruleForm.value.poDate='1900-01-01';
            }
            if(!ruleForm.value.prDate){
              ruleForm.value.prDate='1900-01-01';
            }
            emit('dataSave', ruleForm.value); 
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