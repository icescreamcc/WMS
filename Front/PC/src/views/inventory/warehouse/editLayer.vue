<template>
  <Layer :layer="layer" @confirm="submit" >
     <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px;position:relative;left:2%">
       <el-row>
        <el-col :span="11">
             <el-form-item label="仓库编码" prop="warehouseNo" style="margin-left:20px">
              <el-input v-model="ruleForm.warehouseNo" ></el-input>
         </el-form-item>
        </el-col>
        <el-col :span="11" >
               <el-form-item label="仓库名称" prop="warehouseName" style="margin-left:20px">
         <el-input v-model="ruleForm.warehouseName" />
       </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11" > 
          <el-form-item label="仓库类型" prop="warehouseType" style="margin-left:20px">
            <el-select v-model="ruleForm.warehouseType" style="width: 100%;" placeholder="请选择仓库类型 *必填">
                      <el-option
                      v-for="item in warehouseTypeData"
                      :key="item.optionKey"
                      :label="item.optionName"
                      :value="item.optionKey">
                      </el-option>
                  </el-select>
          </el-form-item> 
        </el-col>
        <el-col :span="11">
              <el-form-item label="备注" prop="remark" style="margin-left:20px">
              <el-input v-model="ruleForm.remark"  ></el-input>
         </el-form-item>
        </el-col>
      </el-row>
   
      <!-- <el-row> 
        <el-col :span="11" > 
           <el-row> 
             <el-col :span="14">
              <el-form-item label="所在省市" prop="province" style="margin-left:20px">
                  <el-select v-model="ruleForm.province" class="m-2" placeholder="选择省份" @change="provinceSelChanged">
                      <el-option
                      v-for="item in provinceData"
                      :key="item.value"
                      :label="item.value"
                      :value="item.key">
                      </el-option>
                  </el-select>
                </el-form-item>
           </el-col> 
           <el-col :span="10">
            <el-form-item  prop="city" label-width="10px" >
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
          <el-col :span="11" >
              <el-form-item label="仓库地址" prop="address" style="margin-left:20px">
         <el-input v-model="ruleForm.address"  />
       </el-form-item>
          </el-col>
      </el-row> --> 
      <el-row> 
        <el-col :span="24" >
          <el-form-item label="负责人邮箱" prop="chargePersonPhone" style="margin-left:20px"> 
          <el-select style="width: 90%;"
                      v-model="ruleForm.chargePersonPhone" 
                      filterable
                      remote
                      multiple
                      collapse-tags
                      :max-collapse-tags="2"
                      reserve-keyword
                      placeholder="输入负责人关键字查询"
                      :remote-method="getUserData"  
                      :loading="userSearchLoading">
                      <el-option v-for="item in userData" :key="item.userId"  :label="item.userName+' '+item.email"  :value="item.email" /> 
                  </el-select>
       </el-form-item>
        </el-col>
      </el-row>
       <el-row> 
        <el-col :span="11" >
       <el-form-item label="是否弃用" prop="isAbandon" style="text-align:left;margin-left:20px">
        <el-checkbox  label="是" v-model="ruleForm.isAbandon" ></el-checkbox>  
      </el-form-item> 
        </el-col>
      </el-row> 
         <el-row >
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
     </el-form> 
  </Layer> 
</template>

<script lang="ts" setup>
import { ref,onMounted } from 'vue' 
import Layer from '@/components/layer/index.vue' 
import { ElForm } from 'element-plus' 
import msg from '@/utils/system/message'
import {getProvinces, getCitysByProvince,getUserByKey} from "@/api/common";  
import {getOptions} from '@/api/inv/warehouse'
import DynamicFields from '@/components/form/dynamicFields.vue'

const props=defineProps({
    layer:{
        type: Object,
        default:()=>{
            return{
              show: false,
              title: '',
              showButton: true,
              btnLoading:false,
              type:'',
              data:null, 
              binArgs:false
            }
        }
    }
  });  
  const formRef= ref(ElForm||null);   
    const isDisabled=ref(false); 
    const warehouseTypeData=ref(new Array<any>());
    const provinceData=ref(new Array<any>());
    const cityData=ref(new Array<any>());
    const userSearchLoading=ref(false);
    const userData=ref(new Array<any>()); 
      //表单  
    const rules = {
        warehouseNo: [{ required: true, message: '请输入字仓库编码', trigger: 'blur' },{ max:15, message: '字符超出限制长度', trigger: 'blur'}],
        warehouseName: [{ required: true, message: '请输入仓库名称', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}], 
        warehouseType:[{ required: true, message: '请选择仓库类型', trigger: 'blur' }], 
        remark:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        chargePerson:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}], 
        address:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}]
    }   
    const ruleForm = ref({
        warehouseId:props.layer.row?.warehouseId,
        warehouseNo:props.layer.row?.warehouseNo,
        warehouseName:props.layer.row?.warehouseName, 
        warehouseType:props.layer.row?.warehouseType,
        province:props.layer.row?.province,
        city:props.layer.row?.city,
        address:props.layer.row?.address, 
        chargePerson:props.layer.row?.chargePerson,  
        chargePersonPhone:props.layer.row?.chargePersonPhone?props.layer.row?.chargePersonPhone.split(','):[],
        isAbandon:props.layer.row?.isAbandon,
        remark:props.layer.row?.address,
        warehouseBin:new Array<any>(),
        spareField1 : props.layer.row?.spareField1,
        spareField2 : props.layer.row?.spareField2,
        spareField3 : props.layer.row?.spareField3,
        spareField4 : props.layer.row?.spareField4,
        spareField5 : props.layer.row?.spareField5
    })

    onMounted(()=>{

    })

    const getUserData=(keyword:string)=>{
        userSearchLoading.value=true;
        getUserByKey(keyword).then((res:any)=>{
        userData.value=res.data;
        }).finally(()=>userSearchLoading.value=false)
    } 

    //获取仓库类型
    getOptions().then((res:any)=>{ 
      warehouseTypeData.value=res.data.argsOptions; 
    })

    //获取省份数据
    getProvinces().then((res:any)=>{ 
      provinceData.value=res.data
      if(ruleForm.value.province){
      var curProvince:any= provinceData.value.filter((x:any)=>x.value==ruleForm.value.province)[0]
      if(curProvince){
          getCitysByProvince(curProvince.key).then((citys:any)=>{
            cityData.value=citys.data;
          });
      }
      }
    })

   //选择省份
   const provinceSelChanged=(val:any)=>{ 
      ruleForm.value.city=null;
      let obj:any= provinceData.value.filter((x:any)=>x.key==val)
      if(obj.length>0) {
        ruleForm.value.province=obj[0].value; 
      };
      getCitysByProvince(val).then(res=>{
          cityData.value=res.data;
        });
   } 
   
   //选择城市
   const citySelChanged=(val:any)=>{
      let obj:any= cityData.value.filter((x:any)=>x.key==val)
      if(obj.length>0)
         ruleForm.value.city=obj[0].value; 
   }
 
   const getSpareFields=(fieldsName:string)=>{ 
   let obj= props.layer.data?.filter((x:any)=>x.fieldName==fieldsName)  
    return obj[0]
  }


  //库位信息
     const binData=ref([
      { 
        binNo:'',
        binName:'',
        remark:''
    }
    ]) 
    if(props.layer.row){
      binData.value=props.layer.row.warehouseBin
      isDisabled.value=true
    }
    else{
      isDisabled.value=false
    }  
     

      //点击确认提交
    const emit = defineEmits(['dataSubmit'])
    const  submit=()=> {    
        formRef.value.validate((valid:any)=>{ 
            if(valid){  
               ruleForm.value.warehouseBin=new Array<any>()
               let tempArr=new Array<any>();
              for(let b of binData.value){
                if(b.binNo&&b.binName){
                  if(tempArr.indexOf(b.binNo)>-1){
                      msg.warningAuto("存在重复的库位编码")
                      return
                  }
                  else{
                     ruleForm.value.warehouseBin.push(b)
                     tempArr.push(b.binNo)
                  }
                }
              }
              ruleForm.value.warehouseBin=binData.value.filter((b:any)=>b.binNo&&b.binName)
              for(let val of ruleForm.value.warehouseBin){
                if(val.binNo.length>20){
                  msg.warningAuto("库位编码超出限制长度")
                  return
                }
                else if(val.binName.length>20){
                  msg.warningAuto("库位名称超出限制长度")
                  return
                }
              }
              if(ruleForm.value.chargePersonPhone&&ruleForm.value.chargePersonPhone.length>0){
                 ruleForm.value.chargePersonPhone=ruleForm.value.chargePersonPhone.join(',');
              }
              else{
                ruleForm.value.chargePersonPhone='';
              }
               emit('dataSubmit', ruleForm.value,props.layer.row?'update':'add') 
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