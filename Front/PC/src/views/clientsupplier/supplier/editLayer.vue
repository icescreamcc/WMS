<template>
  <Layer :layer="layer" @confirm="submit">
   <el-form  label-width="auto" label-position="left" :model="ruleForm" :rules="rules" ref="formRef" style="margin-top: -10px;">
      <el-tabs style="box-shadow:none;-webkit-box-shadow:none;min-height:360px">
          <el-tab-pane label="基本信息">
              <el-row>
                <el-col :span="11">
                <el-form-item label="供应商ID" prop="supplierId" style="margin-left:20px">
                  <el-input v-model="ruleForm.supplierId" readonly placeholder="系统生成 无需填写"  ></el-input>
                  </el-form-item> 
                </el-col>
                <el-col :span="11"  >
                  <el-form-item label="供应商名称" prop="supplierName" style="margin-left:20px">
                <el-input v-model="ruleForm.supplierName" placeholder="请输入供应商名称 *必填"></el-input>
              </el-form-item>
                </el-col>
              </el-row>
                <el-row>
                <el-col :span="11">
              <el-form-item label="供应商编码" prop="supplierNo" style="margin-left:20px">
                <el-input v-model="ruleForm.supplierNo" ></el-input>
              </el-form-item>
                </el-col>
                <el-col :span="11"  >
                    <el-form-item label="供应商类型" prop="supplierTypeId" style="margin-left:20px">
                          <el-select v-model="ruleForm.supplierTypeId" class="m-2" style="width:100%" placeholder="选择供应商类型 *必填" @change="supplierTypeChanged">
                          <el-option
                          v-for="item in supplierTypeData"
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
                    <el-row> 
                        <el-col :span="16">
                          <el-form-item label="所在省市" prop="province" style="margin-left:20px">
                              <el-select v-model="ruleForm.province" class="m-2" placeholder="选择省份" @change="provinceSelChanged">
                                  <el-option
                                  v-for="item in provinceData"
                                  :key="item.value"
                                  :label="item.value"
                                  :value="item.key"
                                  >
                                  </el-option>
                              </el-select>
                            </el-form-item>
                      </el-col> 
                      <el-col :span="8">
                        <el-form-item  prop="city" label-width="10px">
                          <el-select v-model="ruleForm.city" class="m-2" placeholder="选择城市" @change="citySelChanged">
                              <el-option v-for="item in cityData" :key="item.value" :label="item.value" :value="item.key">
                            </el-option>
                      </el-select>
                        </el-form-item> 
                        </el-col> 
                      </el-row> 
                  </el-col>
                  <el-col :span="11">
                    <el-form-item label="详细地址" prop="address" style="margin-left:20px">
                    <el-input v-model="ruleForm.address"></el-input>
                  </el-form-item> 
                  </el-col>
                </el-row> 
                  <el-row>
                <el-col :span="11">
                  <el-form-item label="供应商性质" prop="supplierPropertyId" style="margin-left:20px">
                      <el-select v-model="ruleForm.supplierPropertyId" class="m-2" style="width:100%" placeholder="选择供应商性质" @change="supplierPropChanged">
                          <el-option
                          v-for="item in supplierPropertyData"
                          :key="item.optionId"
                          :label="item.optionName"
                          :value="item.optionId"  >
                        </el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
                <el-col :span="11"  >
                  <el-form-item label="是否重要供应商" prop="isImportant" style="text-align:left;margin-left:20px">
                  <el-checkbox  label="是" v-model="ruleForm.isImportant" ></el-checkbox>  
                </el-form-item> 
                </el-col>
              </el-row>
                    <el-row>
                  <el-col :span="11">
                    <el-form-item label="备注" prop="remark" style="margin-left:20px">
                  <el-input v-model="ruleForm.remark" ></el-input>
                </el-form-item>
                  </el-col>
                  <el-col :span="11" >
               
                  </el-col>
                </el-row> 
          </el-tab-pane>
          <el-tab-pane label="联系方式">   
               <el-row>
                <el-col>
                  <el-row class="head">  
                    <el-col :span="4" :offset="20" style="text-align:right">
                      <el-button style="margin-bottom:5px;margin-right:23px" type="success" @click="addContact">新增联系方式</el-button> 
                    </el-col>
                  </el-row> 
                  <el-table  class="system-table"  border  height="250" :header-cell-style="{'text-align':'center'}" :data="ruleForm.contactDetails">
                    <el-table-column  label="联系人" align="center" min-width="90" >
                        <template #default="detail">
                          <el-input v-model="detail.row.contactPerson"  placeholder="输入联系人姓名" />
                      </template> 
                    </el-table-column>  
                    <el-table-column  label="联系电话" align="center" min-width="100" >
                        <template #default="detail">
                          <el-input v-model="detail.row.telephone"  placeholder="输入联系人电话" />
                      </template> 
                    </el-table-column> 
                    <el-table-column  label="邮箱" align="center" min-width="120" >
                        <template #default="detail">
                          <el-input v-model="detail.row.email"  placeholder="输入联系人邮箱" />
                      </template> 
                    </el-table-column>
                    <el-table-column  label="常用联系方式" align="center" min-width="120" >
                        <template #default="detail">
                          <el-checkbox  label="是" v-model="detail.row.isDeft"/>   
                      </template> 
                    </el-table-column>
                    <el-table-column  label="删除" align="center">
                      <template #default="detail"> 
                        <el-button @click="delContact(detail.row)" type="danger">删除</el-button> 
                      </template>
                  </el-table-column>
                  </el-table>
                </el-col>
               </el-row>
          </el-tab-pane>
          <el-tab-pane label="收款信息">
             <div class="option-content">
             <el-row class="head"> 
               <el-col :span="20" style="text-align:left" >
             
              </el-col>
              <el-col :span="4" style="text-align:right">
                 <el-button style="margin-bottom:5px;margin-right:23px" type="success" @click="addAccount">新增收款信息</el-button> 
              </el-col>
             </el-row> 
              <el-table  class="system-table"  border  height="250" :header-cell-style="{'text-align':'center'}" :data="ruleForm.accountDetails"> 
                <el-table-column  label="收款方式" align="center"  min-width="100" >
                   <template #default="detail">
                       <el-select  v-model="detail.row.accountTypeId"  placeholder="选收款方式" @change="creditTypeChanged(detail.row)">
                    <el-option v-for="item in creditTypeData" :key="item.optionId"  :label="item.optionName" :value="item.optionId"/>
                  </el-select>
                </template>
                </el-table-column>  
                <el-table-column  label="账户名" align="center" min-width="120" >
                    <template #default="detail">
                       <el-input v-model="detail.row.accountName"  placeholder="输入账户名" />
                   </template> 
                </el-table-column>  
                   <el-table-column  label="账号" align="center"  min-width="150" >
                   <template #default="detail">
                          <el-input v-model="detail.row.accountNumber"  placeholder="输入账户号" /> 
                </template>
                </el-table-column>  
                   <el-table-column  label="开户信息" align="center"  min-width="150" >
                   <template #default="detail">
                          <el-input v-model="detail.row.openingBank"  placeholder="输入开户行信息" />
                </template>
                </el-table-column>
                       <el-table-column label="是否常用" align="center" >
                   <template #default="detail">
                        <el-checkbox  label="是" v-model="detail.row.isCommonAccount" @change="isCommonSelected(detail.row)" />
                </template>
                </el-table-column>
                <el-table-column  label="删除" align="center">
                  <template #default="scope"> 
                    <el-button @click="delAccount(scope.row)" type="danger">删除</el-button> 
                  </template>
                </el-table-column>
             </el-table>  
             </div>
          </el-tab-pane>
          <el-tab-pane label="自定义"> 
             <el-row>
              <el-col :span="11" v-if="getSpareFields('SpareField1')">
                <DynamicFields :fieldsInfo="getSpareFields('SpareField1')" :fieldsData="ruleForm.spareField1" v-model="ruleForm.spareField1"/>
              </el-col>
              <el-col :span="11"   v-if="getSpareFields('SpareField2')">
                <DynamicFields :fieldsInfo="getSpareFields('SpareField2')" :fieldsData="ruleForm.spareField2"  v-model="ruleForm.spareField2"/>
              </el-col>
                <el-col :span="11" v-if="getSpareFields('SpareField3')">
                <DynamicFields :fieldsInfo="getSpareFields('SpareField3')" :fieldsData="ruleForm.spareField3" v-model="ruleForm.spareField3"/>
              </el-col>
              <el-col :span="11"   v-if="getSpareFields('SpareField4')">
                <DynamicFields :fieldsInfo="getSpareFields('SpareField4')" :fieldsData="ruleForm.spareField4" v-model="ruleForm.spareField4"/>
              </el-col>
                <el-col :span="11" v-if="getSpareFields('SpareField5')">
                <DynamicFields :fieldsInfo="getSpareFields('SpareField5')" :fieldsData="ruleForm.spareField5" v-model="ruleForm.spareField5"/>
              </el-col> 
            </el-row> 
          </el-tab-pane>
      </el-tabs> 
    </el-form> 
  </Layer>
</template>

<script lang="ts" setup>
import { ref, reactive ,defineEmits,defineProps,onMounted} from 'vue'
import Layer from '@/components/layer/index.vue' ; 
import { ElForm } from 'element-plus';
import { getOptions} from "@/api/baseinfo/supplier";
import {getCitysByProvince} from "@/api/common";
import DynamicFields from '@/components/form/dynamicFields.vue';
import msg from "@/utils/system/message";

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
    }
});

const formRef =ref(ElForm||null)  
const ruleForm = reactive({
       supplierId:props.layer.row?.supplierId,
       supplierNo:props.layer.row?.supplierNo,
       supplierName:props.layer.row?.supplierName,
       supplierTypeId:props.layer.row?.supplierTypeId?props.layer.row?.supplierTypeId:'',
       supplierTypeName:props.layer.row?.supplierTypeName,
       supplierPropertyId:props.layer.row?.supplierPropertyId?props.layer.row?.supplierPropertyId:'',
       supplierPropertyName:props.layer.row?.supplierPropertyName, 
       address:props.layer.row?.address,
       province:props.layer.row?.province,
       city:props.layer.row?.city,
       isImportant:props.layer.row?.isImportant,
       createUser:props.layer.row?.createUser,
       createDate:props.layer.row?.createDate,
       remark:props.layer.row?.remark,
        consignee:props.layer.row?.consignee,
       consigneeTel:props.layer.row?.consigneeTel,
       consigneeAddress:props.layer.row?.consigneeAddress,
        spareField1 : props.layer.row?.spareField1,
        spareField2 : props.layer.row?.spareField2,
        spareField3 : props.layer.row?.spareField3,
        spareField4 : props.layer.row?.spareField4,
        spareField5 : props.layer.row?.spareField5,
        accountDetails:props.layer.row?.accountDetails?props.layer.row?.accountDetails:[{
          accountId:1,
          supplierId:props.layer.row?.supplierId,
          accountTypeId:'',
          accountTypeName:'',
          accountName:'',
          accountNumber:'',
          isCommonAccount:false,
          openingBank:'',
          remark:''
         }],
         contactDetails:props.layer.row?.contactDetails||[]
     }); 

  const supplierPropertyData=ref();

  const supplierTypeData=ref();

  const creditTypeData=ref();

  const provinceData=ref();

  const cityData=ref();

  const emit = defineEmits(['dataSubmit']);

  onMounted(()=>{
    getOptions().then((res:any)=>{
      supplierPropertyData.value=res.data.supplierPropertyOptions?.argsOptions
      supplierTypeData.value=res.data.supplierTypeOptions?.argsOptions
      creditTypeData.value=res.data.supplierCreditTypeOptions?.argsOptions
      provinceData.value=res.data.provinceOptions 
      if(ruleForm.province){
      var curProvince:any= provinceData.value.filter((x:any)=>x.value==ruleForm.province)[0]
      if(curProvince){
          getCitysByProvince(curProvince.key).then(res=>{
            cityData.value=res.data;
          });
      }
      }
    })
  });

  const vaildateMobile=(rule: any, value: any, callback: any)=>{ 
       if (value) {  
          if (value.length!=11 || value[0]!='1') {
          callback(new Error("请输入正确的手机号码"))
        } else {
         callback()
        }
        } 
   }

  const rules:any = {
      supplierNo:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      supplierName: [{ required: true, message: '请输入供应商名称', trigger: 'blur' },{ max: 100, message: '字符超出限制长度', trigger: 'blur'}],
       supplierTypeId: [{ required: true, message: '请选择供应商类型', trigger: 'blur' }],
      //mobilephone: [{ validator: vaildateMobile, trigger: 'blur'}],
      // email:[ {  type: 'email',  message: '请输入正确的邮箱地址',  trigger: ['blur', 'change']  }, { max: 20, message: '字符超出限制长度', trigger: 'blur'}] ,
      // wechat:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      // wangwang:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      // alipay:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      // commonContact:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      address:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
      remark:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
       consignee:[{ max: 10, message: '字符超出限制长度', trigger: 'blur'}],
      consigneeTel:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      consigneeAddress:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}]
    }
    

  const getSpareFields=(fieldsName:string)=>{ 
   let obj= props.layer.data?.filter((x:any)=>x.fieldName==fieldsName) 
    return obj[0]
  }
  
  //选择供应商类型
  const supplierTypeChanged=(val:any)=>{
    supplierTypeData.value.forEach((x:any)=>{
      if(x.optionId==val){
        ruleForm.supplierTypeName=x.optionName
      }
    })
  }
 
 //选择供应商性质
 const supplierPropChanged=(val:any)=>{
   let obj:any= supplierPropertyData.value.filter((x:any)=>x.optionId==val)[0]
  ruleForm.supplierPropertyName=obj.optionName
 }

   //选择省份
 const provinceSelChanged=(val:any)=>{ 
      ruleForm.city=null;
      let obj:any= provinceData.value.filter((x:any)=>x.key==val)
      if(obj.length>0) {
        ruleForm.province=obj[0].value; 
      };
      getCitysByProvince(val).then(res=>{
          cityData.value=res.data;
        });
   } 
   
   //选择城市
  const citySelChanged=(val:any)=>{
      let obj:any= cityData.value.filter((x:any)=>x.key==val)
      if(obj.length>0)
         ruleForm.city=obj[0].value; 
   }

  //选择收款方式
  const creditTypeChanged=(accountDetail:any)=>{ 
    let obj:any=creditTypeData.value.filter((x:any)=>x.optionId==accountDetail.accountTypeId)[0]
     accountDetail.accountTypeName= obj.optionName
   }

  //选择常用备注
  const isCommonSelected=(accountDetail:any)=>{
    for(let item of ruleForm.accountDetails){
      if(item.accountId!=accountDetail.accountId){
        item.isCommonAccount=false
      }
    } 
   }
   const addContact=()=>{
      let id=ruleForm.contactDetails.length+1
      ruleForm.contactDetails.push({
          contacttId:id,
          supplierId:'',
          contactPerson:'',
          telephone:'',
          mobilephone:'',
          email:'',
          wechat:'',
          wangwang:'',
          alipay:'',
          isDeft:false,  
      }) 
    }
 
   const delContact=(item:any)=>{  
      ruleForm.contactDetails.splice(ruleForm.contactDetails.indexOf(item),1)
    }

   const addAccount=()=>{
      let id=ruleForm.accountDetails.length+1
      ruleForm.accountDetails.push({
          accountId:id,
          supplierId:props.layer.row?.supplierId,
          accountTypeId:'',
          accountTypeName:'',
          accountName:'',
          accountNumber:'',
          isCommonAccount:false,
          openingBank:'',
          remark:''
      }) 
    }
 
   const delAccount=(item:any)=>{  
      ruleForm.accountDetails.splice(ruleForm.accountDetails.indexOf(item),1)
    }
 
  const submit=()=> {   
      formRef.value.validate((valid:any) => {   
        if (valid) {    
          if(!ruleForm.supplierTypeId){
            ruleForm.supplierTypeId=0;
          }
          if(!ruleForm.supplierPropertyId){
            ruleForm.supplierPropertyId=0;
          }
          ruleForm.accountDetails=ruleForm.accountDetails.filter((x:any)=>x.accountTypeId);
          for(var i=0;i<ruleForm.contactDetails.length;i++){
            if(!ruleForm.contactDetails[i].contactPerson){
              msg.warningAuto("联系人不能为空");
              return;
            }
            if(ruleForm.contactDetails[i].email){
              console.log(ruleForm.contactDetails[i].email)
              var emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/; 
              if(!emailRegex.test(ruleForm.contactDetails[i].email)){
                msg.warningAuto("请输入正确的邮箱格式");
                return;
              } 
            }
          }
          console.log("contactDetails",ruleForm.contactDetails)
          if (props.layer.row) {   
              emit('dataSubmit', ruleForm,'update')
           } else { 
             emit('dataSubmit', ruleForm,'add') 
          }
        } else {
          return false;
        }
      }); 
    } 
</script>

<style lang="scss" scoped>
    .option-content{
    border:1px solid rgb(230, 230, 230);
    border-radius: 3px;
    padding: 5px;
    .head{
      margin-bottom: 2px;
      .title{
        margin: 5px 0 0 0;
      }
    }
    .item{
      margin:3px 0;
    }
  }
</style>