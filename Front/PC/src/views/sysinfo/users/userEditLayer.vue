<template>
  <Layer :layer="layer" @confirm="submit" @otherEvent="initPassword" >
    <el-form  label-width="auto" label-position="left" :model="ruleForm" :rules="rules" ref="formRef" style="padding: 7px 15px">
      <el-row>
        <el-col :span="11">
          <el-form-item label="工号" prop="UserCode" >
          <el-input v-model="ruleForm.UserCode"   placeholder="请输入工号  *必填"></el-input>
          </el-form-item> 
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item label="UI编号"  prop="AuthAccount">
          <el-input v-model="ruleForm.AuthAccount"  placeholder="请输入uif编号 *必填"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item label="中文名" prop="UserName">
        <el-input v-model="ruleForm.UserName" placeholder="请输入用户姓名  *必填"></el-input>
      </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
             <el-form-item label="英文名" prop="NickName">
        <el-input v-model="ruleForm.NickName" placeholder="请输入用户英文名"></el-input>
      </el-form-item>
        </el-col>
      </el-row>
       <el-row>
        <el-col :span="11">
          <el-form-item label="邮箱" prop="Email">
        <el-input v-model="ruleForm.Email" placeholder="请输入用户邮箱"></el-input>
      </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item label="域用户名" prop="DomainName">
        <el-input v-model="ruleForm.DomainName" placeholder="请输入域用户名 *必填"></el-input>
      </el-form-item> 
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
           <!-- <el-form-item label="手机" prop="MobilePhone">
        <el-input v-model="ruleForm.MobilePhone" type="number" placeholder="请输入用户手机号"></el-input>
      </el-form-item> -->
      <el-form-item label="卡号" prop="CardId">
        <el-input v-model="ruleForm.CardId"  placeholder="请输入员工卡号" ></el-input>
      </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
          <el-form-item label="电话" prop="Phone">
        <el-input v-model="ruleForm.Phone" type="number" placeholder="请输入用户电话" ></el-input>
      </el-form-item>
        </el-col>
      </el-row>
       <el-row>
        <el-col :span="11">
          <el-form-item label="所属部门" prop="DeptId">
          <el-select v-model="ruleForm.DeptId" class="m-2" placeholder="请选择部门  *必填" style="width:100%" @change="deptSelChanged">
            <el-option
              v-for="item in deptOptions"
              :key="item.key"
              :label="item.value"
              :value="item.key"
            >
            </el-option>
          </el-select>
      </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2"> 
           <el-row> 
             <el-col :span="14">
              <el-form-item label="来自省市" prop="ProvinceId">
                  <el-select v-model="ruleForm.ProvinceName" class="m-2" placeholder="选择省份" @change="provinceSelChanged">
                      <el-option
                      v-for="item in provinceOptions"
                      :key="item.value"
                      :label="item.value"
                      :value="item.key"
                      >
                      </el-option>
                  </el-select>
                </el-form-item>
           </el-col> 
           <el-col :span="10">
            <el-form-item  prop="CityId" label-width="10px">
              <el-select v-model="ruleForm.CityName" class="m-2" placeholder="选择城市" @change="citySelChanged">
                  <el-option
                  v-for="item in cityOptions"
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

      </el-row>
        <el-row>
          <el-col :span="11" >
          <el-form-item label="通讯地址" prop="Address">
        <el-input v-model="ruleForm.Address" placeholder="请输入通讯地址"></el-input>
      </el-form-item> 
        </el-col>
        <el-col :span="11" :offset="2">
              <el-form-item label="备注" prop="Remark">
        <el-input v-model="ruleForm.Remark" placeholder="备注"></el-input>
      </el-form-item>
        </el-col>
      </el-row> 
    </el-form>
  </Layer>
</template>

<script lang="ts">
import { defineComponent, ref, reactive } from 'vue'
import Layer from '@/components/layer/index.vue' 
import {getAboutUserOptions,getCitysByProvince,initUserPassword} from "@/api/system/user";
import { ElForm } from 'element-plus';
export default defineComponent({
  components: {
    Layer
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
          data:null,
          otherButton:{
                show:false,
                otherBtnLoading:false,
                text:"重置密码",
                type:"warning"
              }
        }
      }
    }
  }, 
  setup(props, ctx) {
    let isEdit=props.layer.row; 
    let formRef =ref(ElForm||null)
    let ruleForm = reactive({
      UserId: '',
      AuthAccount:'',
      UserCode:'',
      DomainName:'',
      UserName:'',
      NickName:'',
      Email:'', 
      Phone:'',
      MobilePhone:'',
      Address:'',
      ProvinceId:0,
      ProvinceName:'',
      CityId:0,
      CityName:'',
      DeptId:'',
      DeptName:'',
      Remark:'',
      DepartureDate:'',
      EditDate:'',
      CardId:''
    })
    let vaildateMobile=(rule: any, value: any, callback: any)=>{ 
       if (value == '') {
          callback(new Error('请输入用户手机号'))
        } else if (value.length!=11 || value[0]!='1') {
          callback(new Error("请输入正确的手机号码"))
        } else {
          callback()
        }
   }
    let rules = {
      UserCode: [{ required: true, message: '请输入员工编号', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      AuthAccount: [{ required: true, message: '请输入uif编号', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      UserName: [{ required: true, message: '请输入用户姓名', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      DomainName: [{ required: true, message: '请输入域用户名', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      // MobilePhone: [{ validator: vaildateMobile, trigger: 'blur'}],
      DeptId: [{ required: true, message: '请给用户指定一个部门', trigger: 'change' }], 
      Email:[{type: 'email',message: '请输入正确的邮箱地址',trigger: ['blur', 'change']},{ max: 100, message: '字符超出限制长度', trigger: 'blur'}],
      Address:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
      Remark:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}]
    }
   let deptOptions = ref()
   let provinceOptions=ref();
   let cityOptions= ref();  

   //组件加载时获取部门、省份选项数据
    getAboutUserOptions().then(res=>{  
      deptOptions.value=res.data.deptOptions;
      provinceOptions.value=res.data.provinceOptions;  
    }); 

  //选择部门
  let deptSelChanged=(val:any)=>{
   let obj:any= deptOptions.value.filter((x:any)=>x.key==val)
   if(obj.length>0)
    ruleForm.DeptName=obj[0].value; 
  }

   //选择省份
   let provinceSelChanged=(val:any)=>{ 
      ruleForm.CityId=0;
      let obj:any= provinceOptions.value.filter((x:any)=>x.key==val)
      if(obj.length>0) {
        ruleForm.ProvinceName=obj[0].value; 
      };
      getCitysByProvince(val).then(res=>{
          cityOptions.value=res.data;
        });
   } 
   
   //选择城市
   let citySelChanged=(val:any)=>{
      let obj:any= cityOptions.value.filter((x:any)=>x.key==val)
      if(obj.length>0)
         ruleForm.CityName=obj[0].value; 
   }
  
    //编辑模式下表单赋值
    if(props.layer.row&&props.layer.data){
      ruleForm.UserId=props.layer.data.userId;
      ruleForm.AuthAccount=props.layer.data.authAccount;
      ruleForm.DomainName=props.layer.data.domainName;
      ruleForm.UserCode=props.layer.data.userCode;
      ruleForm.UserName=props.layer.data.userName;
      ruleForm.NickName=props.layer.data.nickName;
      ruleForm.Email=props.layer.data.email; 
      ruleForm.Phone=props.layer.data.phone;
      ruleForm.MobilePhone=props.layer.data.mobilePhone;
      ruleForm.Address=props.layer.data.address;
      ruleForm.ProvinceId=props.layer.data.provinceId;
      ruleForm.ProvinceName=props.layer.data.provinceName;
      if(props.layer.data.provinceId&&Number(props.layer.data.provinceId)>0)
      {
        getCitysByProvince(props.layer.data.provinceId).then(res=>{
             cityOptions.value=res.data;
        });
      }
      ruleForm.CityId=props.layer.data.cityId;
      ruleForm.CityName=props.layer.data.cityName;
      ruleForm.DeptId=props.layer.data.deptId==0?null:props.layer.data.deptId;
      ruleForm.DeptName=props.layer.data.deptName;
      ruleForm.Remark=props.layer.data.remark;
      ruleForm.DepartureDate=props.layer.data.departureDate;
      ruleForm.EditDate=props.layer.data.editDate;
    }
    else{
      props.layer.data=null; 
    }

   //编辑提交
    const submit=()=> {  
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

     //恢复初始密码
   const initPassword=()=>{
       if (props.layer.row){ 
         props.layer.otherButton.otherBtnLoading=true;
         initUserPassword(ruleForm.UserId).then(res=>{}).finally(()=>{
            props.layer.otherButton.otherBtnLoading=false; 
         })
       }
    } 

    return {
      ruleForm,
      rules,
      deptOptions,
      provinceOptions,
      cityOptions,
      formRef, 
      deptSelChanged,
      provinceSelChanged,
      citySelChanged,
      isEdit,
      submit,
      initPassword
    }
  } 
})
</script>

<style lang="scss" scoped>
  
</style>