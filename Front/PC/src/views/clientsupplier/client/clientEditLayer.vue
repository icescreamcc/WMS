<template>
  <Layer :layer="layer" @confirm="submit">
   <el-form  label-width="auto" label-position="left" :model="ruleForm" :rules="rules" ref="formRef" style="padding: 7px 15px;position:relative;left:2%">
     <el-tabs type="border-card" style="box-shadow:none;-webkit-box-shadow:none;min-height:320px">
          <el-tab-pane label="基本信息">
            <el-row>
        <el-col :span="11">
        <el-form-item label="客户ID" prop="clientId" style="margin-left:20px">
          <el-input v-model="ruleForm.clientId" readonly placeholder="系统生成 无需填写"  ></el-input>
          </el-form-item> 
        </el-col>
        <el-col :span="11"  >
          <el-form-item label="客户名称" prop="clientName" style="margin-left:20px">
        <el-input v-model="ruleForm.clientName" placeholder="请输入客户名称  *必填"></el-input>
      </el-form-item>
        </el-col>
            </el-row>
              <el-row>
              <el-col :span="11">
            <el-form-item label="客户编码" prop="clientType" style="margin-left:20px">
              <el-input v-model="ruleForm.clientNo" ></el-input>
            </el-form-item>
              </el-col>
              <el-col :span="11"  >
                  <el-form-item label="客户类型" prop="clientTypeId" style="margin-left:20px">
                        <el-select v-model="ruleForm.clientTypeId" class="m-2" style="width:100%" placeholder="选择客户类型 *必填" @change="clientTypeChanged">
                        <el-option
                        v-for="item in clientTypeData"
                        :key="item.optionId"
                        :label="item.optionName"
                        :value="item.optionId"  >
                      </el-option>
                </el-select>
                  </el-form-item>
              </el-col>
            </el-row>
               <el-row>
        <el-col :span="11">
           <el-form-item label="客户等级" prop="clientLevel" style="margin-left:20px">
               <el-select v-model="ruleForm.clientLevel" class="m-2" style="width:100%" placeholder="选择客户等级">
                  <el-option
                  v-for="item in clientLevelData"
                  :key="item.optionId"
                  :label="item.optionName"
                  :value="item.optionName"  >
                </el-option>
                 </el-select>
            </el-form-item>
        </el-col>
        <el-col :span="11"  >
                <el-form-item label="是否重要客户" prop="isImportant" style="text-align:left;margin-left:20px">
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
      </el-row>  
          </el-tab-pane>
           <el-tab-pane label="联系方式">
               <el-row>
                <el-col :span="11">
                        <el-form-item label="手机号" prop="mobilephone" style="margin-left:20px">
                <el-input v-model="ruleForm.mobilephone" type="number"></el-input>
              </el-form-item>
                </el-col>
                <el-col :span="11"  >
                    <el-form-item label="联系电话" prop="telephone" style="margin-left:20px">
                <el-input v-model="ruleForm.telephone"  placeholder=""></el-input>
              </el-form-item>
                </el-col>
              </el-row>
                       <el-row>
        <el-col :span="11">
         <el-form-item label="邮箱" prop="email" style="margin-left:20px">
        <el-input v-model="ruleForm.email" ></el-input>
      </el-form-item>
        </el-col>
        <el-col :span="11"  >
    <el-form-item label="旺旺" prop="wangwang" style="margin-left:20px">
        <el-input v-model="ruleForm.wangwang" ></el-input>
      </el-form-item>
        </el-col>
      </el-row> 
                  <el-row>
        <el-col :span="11"> 
              <el-form-item label="支付宝" prop="alipay" style="margin-left:20px">
        <el-input v-model="ruleForm.alipay" ></el-input>
      </el-form-item>
        </el-col>
        <el-col :span="11"  >
             <el-form-item label="微信号" prop="wechat" style="margin-left:20px">
        <el-input v-model="ruleForm.wechat" placeholder=""></el-input>
      </el-form-item>
        </el-col>
      </el-row> 
            <el-row>
        <el-col :span="11">
           <el-form-item label="常用联系方式" prop="commonContact" style="margin-left:20px">
        <el-input v-model="ruleForm.commonContact" ></el-input>
      </el-form-item>  
        </el-col>
        <el-col :span="11"  >
        <el-row> 
             <el-col :span="16">
              <el-form-item label="来自省市" prop="province" style="margin-left:20px">
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
                  <el-option
                  v-for="item in cityData"
                  :key="item.value"
                  :label="item.value"
                  :value="item.key"
                >
                </el-option>
          </el-select>
            </el-form-item> 
             </el-col> 
           </el-row> 
        </el-col>
      </el-row>
        <el-row>
        <el-col :span="11">
             <el-form-item label="详细地址" prop="address" style="margin-left:20px">
        <el-input v-model="ruleForm.address" ></el-input>
      </el-form-item>
        </el-col>
        <el-col  :span="11"  >
        <el-form-item label="收货人" prop="consignee" style="margin-left:20px">
        <el-input v-model="ruleForm.consignee" ></el-input>
      </el-form-item>
        </el-col>
      </el-row> 
             <el-row>
        <el-col :span="11">
             <el-form-item label="收货人电话" prop="consigneeTel" style="margin-left:20px">
        <el-input v-model="ruleForm.consigneeTel" ></el-input>
      </el-form-item>
        </el-col>
        <el-col  :span="11"  >
        <el-form-item label="收货地址" prop="consigneeAddress" style="margin-left:20px">
        <el-input v-model="ruleForm.consigneeAddress" ></el-input>
      </el-form-item>
        </el-col>
      </el-row> 
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

<script lang="ts">
import { defineComponent, ref, reactive } from 'vue'
import Layer from '@/components/layer/index.vue' ; 
import { ElForm } from 'element-plus';
import { getOptions} from "@/api/baseinfo/client";
import {getCitysByProvince} from "@/api/common";
import DynamicFields from '@/components/form/dynamicFields.vue'
export default defineComponent({
  components: {
    Layer,
    DynamicFields
  },
  props: {
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
  }, 
  setup(props, ctx) { 
    const formRef =ref(ElForm||null)  
    let ruleForm = reactive({
       clientId:props.layer.row?.clientId,
       clientNo:props.layer.row?.clientNo,
       clientName:props.layer.row?.clientName,
       clientTypeId:props.layer.row?.clientTypeId,
       clientTypeName:props.layer.row?.clientTypeName,
       clientLevel:props.layer.row?.clientLevel,
       telephone:props.layer.row?.telephone,
       mobilephone:props.layer.row?.mobilephone,
       email:props.layer.row?.email,
       wechat:props.layer.row?.wechat,
       wangwang:props.layer.row?.wangwang,
       alipay:props.layer.row?.alipay,
       commonContact:props.layer.row?.commonContact,  
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
      spareField5 : props.layer.row?.spareField5
       }) 

    let vaildateMobile=(rule: any, value: any, callback: any)=>{ 
       if (value) { 
          if (value.length!=11 || value[0]!='1') {
          callback(new Error("请输入正确的手机号码"))
        } else {
          callback()
        }
        } 
   }
    let rules = {
      clientNo:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      clientName: [{ required: true, message: '请输入客户名称', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      clientTypeId: [{ required: true, message: '请选择客户类型', trigger: 'blur' }],
     // mobilephone: [{ required: true, message: '请输入客户手机号', trigger: 'blur' },{ validator: vaildateMobile, trigger: 'blur'}],
      email:[ {  type: 'email',  message: '请输入正确的邮箱地址',  trigger: ['blur', 'change']  }, { max: 20, message: '字符超出限制长度', trigger: 'blur'}] ,
      wechat:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      wangwang:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      alipay:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      commonContact:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      address:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
      remark:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
      consignee:[{ max: 10, message: '字符超出限制长度', trigger: 'blur'}],
      consigneeTel:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      consigneeAddress:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}]
    }
    

  let getSpareFields=(fieldsName:string)=>{ 
   let obj= props.layer.data?.filter((x:any)=>x.fieldName==fieldsName) 
    return obj[0]
  }

  let clientLevelData=ref()
  let clientTypeData=ref()
  let provinceData=ref()
  let cityData=ref()

  getOptions().then((res:any)=>{
    clientLevelData.value=res.data.clientLevelOptions?.argsOptions
    clientTypeData.value=res.data.clientTypeOptions?.argsOptions
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

  //选择客户类型
  let clientTypeChanged=(val:any)=>{
    clientTypeData.value.forEach((x:any)=>{
      if(x.optionId==val){
        ruleForm.clientTypeName=x.optionName
      }
    })
  }
 
   //选择省份
   let provinceSelChanged=(val:any)=>{ 
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
   let citySelChanged=(val:any)=>{
      let obj:any= cityData.value.filter((x:any)=>x.key==val)
      if(obj.length>0)
         ruleForm.city=obj[0].value; 
   }

  //提交数据
  let submit=()=> {   
      formRef.value.validate((valid:any) => {  
        if (valid) {    
          if (props.layer.row) {   
              ctx.emit('dataSubmit', ruleForm,'update')
           } else { 
             ctx.emit('dataSubmit', ruleForm,'add') 
          }
        } else {
          return false;
        }
      }); 
    } 
    return {
      formRef,
      ruleForm,
      rules ,
      clientLevelData,
      clientTypeData,
      provinceData, 
      cityData,
      provinceSelChanged,
      citySelChanged,
      clientTypeChanged,
      getSpareFields ,
      submit
    }
  } 
})
</script>

<style lang="scss" scoped>
  
</style>