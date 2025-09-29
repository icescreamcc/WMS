<template>
    <Layer :layer="layer" @confirm="submit" > 
           <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
            <el-row>
               <el-col>
                    <el-form-item label="收件人" prop="toReciver">
                        <el-select style="width: 100%;"
                            v-if="props.layer.options?.reciverType!='manual'"
                            v-model="ruleForm.toReceiver"  filterable remote multiple reserve-keyword placeholder="输入关键字查询"
                            :remote-method="getUserData"  @change="reciverSelectChanged" :loading="userSearchLoading" >
                            <el-option v-for="item in userData" :key="item.userId"  :label="item.userName+' '+item.email"  :value="item.email" /> 
                        </el-select>
                        <el-select  style="width: 100%;" v-else
                          v-model="ruleForm.toReceiver" multiple  filterable allow-create default-first-option :reserve-keyword="false"
                          placeholder="输入关键字查询" >
                          <el-option  v-for="item in ruleForm.toReciverOptions"  :key="item.contacttId" :label="item.contactPerson+' '+item.email"  :value="item.email" />
                        </el-select>
                  </el-form-item>
               </el-col>
             </el-row>
             <el-row>
               <el-col>
                    <el-form-item label="抄送人" prop="toCC">
                        <el-select style="width: 100%;"
                            v-model="ruleForm.toCC" 
                            filterable
                            remote
                            multiple
                            reserve-keyword
                            placeholder="输入抄送人关键字查询"
                            :remote-method="getUserData" 
                            @change="ccSelectChanged"
                            :loading="userSearchLoading" >
                            <el-option v-for="item in userData" :key="item.userId"  :label="item.userName+' '+item.email"  :value="item.email" /> 
                        </el-select>
                  </el-form-item>
               </el-col>
             </el-row>
            <el-row>
               <el-col>
                    <el-form-item label="主题" prop="subject">
                    <el-input v-model="ruleForm.subject" />
                  </el-form-item>
               </el-col>
             </el-row>
             <el-row v-if="ruleForm.attachments">
               <el-col>
                    <el-form-item label="附件" style="text-align: left;"> 
                    <el-link type="success" v-for="att in ruleForm.attachments" :href="att.url">{{att.fileName }}</el-link> 
                  </el-form-item>
               </el-col>
             </el-row>
             <el-row>
               <el-col> 
                    <el-form-item label="内容" prop="body"> 
                    <Editer v-model="ruleForm.body" :height="200" />
                  </el-form-item>
               </el-col>
             </el-row>    
           </el-form> 
    </Layer>
  </template>
  
  <script lang="ts" setup>
  import {  onMounted,defineEmits,defineProps,ref,shallowRef } from 'vue';
  import { ElForm } from 'element-plus'; 
  import Layer from '@/components/layer/index.vue';     
  import{getUserByKey,getSupplierByKey}from '@/api/common';   
  import Editer from "@/components/tinymce/index.vue";
  import msg from "@/utils/system/message";

  const props=defineProps({
    layer: {
        type: Object,
        default: () => {
          return {
            show: false,
            title:'',
            width:"40%",
            showButton: true , 
            options:null,
            data:null
          }
        }
      }
  });
  const emit = defineEmits(['dataSubmit']); 
  const userSearchLoading=ref(false);
  const userData=ref(new Array<any>()); 
 
  const ruleForm = ref({
          subject:props.layer.data?.subject,
          body:props.layer.data?.body, 
          bodyType:props.layer.data?.bodyType, 
          toReceiver:props.layer.data?.toReciver,
          toReciverOptions:props.layer.data?.toReciverOptions,
          toCC:props.layer.data?.toCC,
          attachments:props.layer.data?.attachments,
          orderNo:props.layer.data?.orderNo,
          primaryId:props.layer.data?.orderNo
    }) 
  const rules = {
        toReciver:[{ required: true, message: '请选择收件人', trigger: 'change' }],
        subject:[{ required: true, message: '请输入主题', trigger: 'blur' },{ max: 100, message: '字符超出限制长度', trigger: 'blur'}],     
        body: [{ max: 5000, message: '字符超出限制长度', trigger: 'blur'}]
  } 
 
  const getUserData=(keyword:string)=>{
        userSearchLoading.value=true;
        getUserByKey(keyword).then((res:any)=>{
        userData.value=res.data;
        }).finally(()=>userSearchLoading.value=false)
  }

  const reciverSelectChanged=(value:any)=>{  
    let user=userData.value.find(f=>f.authAccount==value); 
    if(user){
        
    }
  }

  const ccSelectChanged=(value:any)=>{  
    let user=userData.value.find(f=>f.authAccount==value); 
    if(user){
        
    }
  }

  const submit=()=> {   
    if(ruleForm.value.toReceiver.length==0){
      msg.warningAuto("至少选择一个收件人");
      return;
    }  
    else{
      for(let i=0;i<ruleForm.value.toReceiver.length;i++){
        if(!ruleForm.value.toReceiver[i]){
          msg.warningAuto("收件人信息不正确");
          return;
        }
      }
    }
    emit('dataSubmit', ruleForm.value); 
   }
  </script>
  
  <style lang="scss" scoped>
     .field-chk{
       width: 100px;
       height: 35px; 
     }
       .list{
        // width: 100%;
        padding: 0 0 0 10px;
        overflow-y: auto;
        flex: 1;
        height: auto;
        width: calc(100% - 10px); 
      }
  </style>