<template>
    <Layer :layer="layer" @confirm="submit" >
      <el-form  label-width="auto" label-position="left" :model="ruleForm" :rules="rules" ref="formRef" style="padding: 7px 15px">
        <el-row>
          <el-col :span="11">
            <el-form-item label="Provider" prop="providerName" >
            <el-input v-model="ruleForm.providerName" :disabled="!isEdit"  placeholder="请输入Provider  *必填"></el-input>
            </el-form-item> 
          </el-col>
          <el-col :span="11" :offset="2">
            <el-form-item label="Secret" prop="providerSecretKey" >
            <el-input v-model="ruleForm.providerSecretKey"  placeholder="请输入SecretKey *必填"></el-input>
            </el-form-item>
          </el-col>
        </el-row> 
        <el-row>
            <el-col :span="11" >
            <el-form-item label="Secret" prop="providerSecret">
          <el-input v-model="ruleForm.providerSecret" disabled></el-input>
        </el-form-item> 
          </el-col>
          <el-col :span="11" :offset="2">
            <el-form-item label="Host" prop="providerHost">
          <el-input v-model="ruleForm.providerHost" placeholder="请输入Host *必填"></el-input>
        </el-form-item>  
          </el-col>
        </el-row> 
          <el-row>
            <el-col :span="11" >
              <el-form-item label="Remark" prop="Remark">
          <el-input v-model="ruleForm.remark" placeholder="备注"></el-input>
        </el-form-item>
          </el-col>
          <el-col :span="11" :offset="2">
            <el-form-item label="是否有效" prop="isValid">
              <el-checkbox  label="是" v-model="ruleForm.isValid" ></el-checkbox> 
        </el-form-item> 
          </el-col> 
        </el-row>   
      </el-form>
    </Layer>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref, reactive } from 'vue'
  import Layer from '@/components/layer/index.vue'  
  import { ElForm } from 'element-plus';
  import msg from "@/utils/system/message" 
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
                  text:"",
                  type:""
                }
          }
        }
      }
    }, 
    setup(props, ctx) {
      const isEdit=ref(props.layer.row?false:true);  
      let formRef =ref(ElForm||null)
      let ruleForm = reactive({
        providerName: props.layer.row?.providerName||'', 
        providerSecretKey:props.layer.row?.providerSecretKey|| '',
        providerSecret:props.layer.row?.providerSecret||'', 
        providerHost: props.layer.row?.providerHost||'', 
        remark: props.layer.row?.remark||'', 
        isValid:props.layer.row?.isValid|| true
      }) 
      let rules = {
        providerName: [{ required: true, message: '请输入provider', trigger: 'blur' },{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
        providerSecretKey: [{ required: true, message: '请输入SecretKey', trigger: 'blur' },{ min: 8, message: 'key必须大于8位', trigger: 'blur'}],
        providerHost: [{ required: true, message: '请输入Host', trigger: 'blur' },{ max: 50, message: '字符超出限制长度', trigger: 'blur'}], 
        remark:[{ max: 150, message: '字符超出限制长度', trigger: 'blur'}]
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
   
  
      return {
        ruleForm,
        rules, 
        formRef,  
        isEdit,
        submit 
      }
    } 
  })
  </script>
  
  <style lang="scss" scoped>
    
  </style>