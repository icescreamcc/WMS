<template>
     <el-drawer v-model="props.options.show" :with-header="false" :show-close="true"  size="33%"> 
        <div class="drawer-content">
            <div class="content-header">
                <div>格式化内容编辑</div>
            </div>
            <div class="content-body">
                <el-form :model="ruleForm"  ref="formatformRef" label-width="auto" label-position="top" style="padding:0 15px;">
                    <div :style="{maxHeight:scrollHeight+'px',overflowX:'hidden',overflowY:'scroll',padding:'10px'}">
                      <el-form-item v-if="ruleForm.itemType=='Text'" label="是否显示名称" prop="showName" style="text-align: left;">
                        <el-checkbox  label="是" v-model="ruleForm.showName" @change="onFormatInput" style="text-align: left;"/>   
                    </el-form-item>   
                      <el-form-item label="值前缀">
                            <el-input v-model="ruleForm.valuePerfix"  maxlength="10" @input="onFormatInput" placeholder="值的前缀文本" ></el-input>
                        </el-form-item>
                        <el-form-item label="值后缀">
                            <el-input v-model="ruleForm.valueSuffix" maxlength="10" @input="onFormatInput" placeholder="值的后缀文本" ></el-input>
                        </el-form-item>
                        <el-form-item label="字段分隔符">
                            <el-input v-model="ruleForm.fieldSeparator" maxlength="1" @input="onFormatInput" placeholder="字段分隔符" ></el-input>
                        </el-form-item>   
                        <div v-for="(field,index) in ruleForm.itemValueFieldInfo" :style="{paddingBottom:'10px', borderTop: '2px solid #fff',borderBottom:(index==ruleForm.itemValueFieldInfo.length-1)?'2px solid #fff':'none'}">
                            <div style="text-align: left;margin-bottom: 10px;margin-top: 10px;">
                                <div style="border-radius: 20px;height: 20px;width: 20px;display: inline-block;background-color: #fff;text-align: center;color: rgb(12, 159, 44);vertical-align: middle;">{{ index+1 }}</div>
                                {{ field.value }}
                            </div>
                            <el-row gutter="15" justify="start">
                                <el-col :span="12">
                                  <el-input v-model="field.fieldPerfix" maxlength="10" @input="onFormatInput" :title="field.value+'的前缀'" :placeholder="field.value+'的前缀'" >
                                        <template #prepend>
                                            <span>前缀</span>
                                        </template>
                                    </el-input>
                                </el-col>
                                <el-col :span="12"> 
                                    <el-input v-model="field.fieldSuffix" maxlength="10" @input="onFormatInput" :title="field.value+'的后缀'" :placeholder="field.value+'的后缀'">
                                        <template #append>
                                            <span>后缀</span>
                                        </template>
                                    </el-input> 
                                </el-col> 
                            </el-row> 
                            <el-row style="margin-top: 5px;">
                                <el-col :span="12" style="text-align: left;"> 
                                    <el-select  v-model="field.dateFormat" @change="onFormatInput" title="针对日期字段的格式化" placeholder="针对日期字段的格式化" style="width: 97.5%;">
                                        <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;">日期格式</div></template>
                                        <el-option-group v-for="group in dateFormatOptions" :key="group.type" :label="group.type">
                                        <el-option v-for="item in group.options" :key="item" :label="item" :value="item"></el-option>
                                        </el-option-group>
                                    </el-select> 
                                </el-col>
                            </el-row>
                        </div> 
                    </div>
                    <div class="body-content">
                        <el-form-item label="格式化内容" prop="labelName"> 
                            <div class="text-area">{{ ruleForm.itemValueFormat }}</div>
                        </el-form-item>
                    </div> 
                </el-form> 
            </div>  
            <div class="content-footer">
                <img src="../../../../public/icon-img/tijiaochenggong.png" title="编辑保存" @click="submit"> 
            </div>
        </div>
     </el-drawer> 
  </template>
  
  <script lang="ts" setup>
  import {ref ,defineEmits,defineProps,onMounted} from 'vue'  
  import { ElForm } from 'element-plus'  
import { Console } from 'console';
  const props=defineProps({
    options: {
        type: Object,
        default: () => {
            return {
            show: false,
            title: '', 
            type:'', 
            isMultiSelect:true,
            data:null 
            }
        }
        }
    }); 
  const emit = defineEmits(['dataSubmit'])    
  const formatformRef= ref(ElForm||null);  
  const scrollHeight=ref(window.innerHeight*.68)
  const dateFormatOptions=ref([{type:'Year',options:['yyyy','yyyy年']},
  {type:'Month',options:['yyyy-MM','yyyy/MM','yyyyMM','yyyy年MM月']},
  {type:'Date',options:['yyyy-MM-dd','yyyy/MM/dd','yyyyMMdd','yyyy年MM月dd日']},
  {type:'DateTime',options:[ 'yyyy-MM-dd HH:mm','yyyy/MM/dd HH:mm','yyyyMMddHHmm','yyyy年MM月dd日 HH时mm分','yyyy-MM-dd HH:mm:ss','yyyy/MM/dd HH:mm:ss','yyyyMMddHHmmss','yyyy年MM月dd日 HH时mm分ss秒']}]) 
  const ruleForm = ref({  
      labelId:props.options.data.labelId,
      itemId:props.options.data.itemId,
      itemType:props.options.data.itemType,
      itemName:props.options.data.itemName,
      itemValueType:props.options.data.itemValueType, 
      itemValueField:props.options.data.itemValueField,
      itemValueFieldInfo:props.options.data.itemValueFieldInfo,
      itemValueFormat:props.options.data.itemValueFormat,
      showName:props.options.data.showName,
      style:props.options.data.style,
      args:props.options.data.args,
      remark:props.options.data.remark,
      valuePerfix:props.options.data.valuePerfix,
      valueSuffix:props.options.data.valueSuffix,
      fieldSeparator:props.options.data.fieldSeparator
  })  
  
  onMounted(()=>{     
  })
  
  const onFormatInput=()=>{  
    if(ruleForm.value.itemType&&ruleForm.value.itemValueType){ 
      if(ruleForm.value.itemType=='Text'){
        if(ruleForm.value.itemName){
          if(ruleForm.value.itemValueType=='Fixed'){
            if(ruleForm.value.showName){
              ruleForm.value.itemValueFormat=`${ruleForm.value.itemName}：${ruleForm.value.valuePerfix}${ruleForm.value.itemValueField}${ruleForm.value.valueSuffix}`;
            }
            else{
              ruleForm.value.itemValueFormat=`${ruleForm.value.valuePerfix}${ruleForm.value.itemValueField}${ruleForm.value.valueSuffix}`;
            } 
          }
          else if(ruleForm.value.itemValueType=='BindingField'){
            if(ruleForm.value.itemValueField){ 
              let formatArr=ruleForm.value.itemValueFieldInfo.map((m:any)=>`${m.fieldPerfix?m.fieldPerfix:''}{${m.value}}${m.fieldSuffix?m.fieldSuffix:''}`);
              let formatStr=formatArr.join(ruleForm.value.fieldSeparator);
              if(ruleForm.value.showName){
                ruleForm.value.itemValueFormat=`${ruleForm.value.itemName}：${ruleForm.value.valuePerfix}${formatStr}${ruleForm.value.valueSuffix}`;
              }
              else{
                ruleForm.value.itemValueFormat=`${ruleForm.value.valuePerfix}${formatStr}${ruleForm.value.valueSuffix}`;
              } 
            } 
          } 
        }
        else if(ruleForm.value.itemValueField){
          if(ruleForm.value.itemValueType=='Fixed'){
              ruleForm.value.itemValueFormat=`${ruleForm.value.valuePerfix}${ruleForm.value.itemValueField}${ruleForm.value.valueSuffix}`;
          }
          else if(ruleForm.value.itemValueType=='BindingField'){
              let formatArr=ruleForm.value.itemValueFieldInfo.map((m:any)=>`${m.fieldPerfix?m.fieldPerfix:''}{${m.value}}${m.fieldSuffix?m.fieldSuffix:''}`);
              let formatStr=formatArr.join(ruleForm.value.fieldSeparator);
              ruleForm.value.itemValueFormat=`${ruleForm.value.valuePerfix}${formatStr}${ruleForm.value.valueSuffix}`;
          } 
        }
        else{
          ruleForm.value.itemValueFormat=`{value}`;
        } 
      } 
      else if(ruleForm.value.itemType=='QRCode'){
        if(ruleForm.value.itemValueField){
          if(ruleForm.value.itemValueType=='Fixed'){
            ruleForm.value.itemValueFormat=`${ruleForm.value.valuePerfix}${ruleForm.value.itemValueField}${ruleForm.value.valueSuffix}`;
          }
          else if(ruleForm.value.itemValueType=='BindingField'){
            let formatArr=ruleForm.value.itemValueFieldInfo.map((m:any)=>`${m.fieldPerfix?m.fieldPerfix:''}{${m.value}}${m.fieldSuffix?m.fieldSuffix:''}`);
            let formatStr=formatArr.join(ruleForm.value.fieldSeparator);
            ruleForm.value.itemValueFormat=`${ruleForm.value.valuePerfix}${formatStr}${ruleForm.value.valueSuffix}`;
          } 
        }
        else{
          ruleForm.value.itemValueFormat=`{value}`;
        } 
      }
      else if(ruleForm.value.itemType=='BarCode'){
        if(ruleForm.value.itemValueField){
          if(ruleForm.value.itemValueType=='Fixed'){
            ruleForm.value.itemValueFormat=`${ruleForm.value.valuePerfix}${ruleForm.value.itemValueField}${ruleForm.value.valueSuffix}`;
          }
          else if(ruleForm.value.itemValueType=='BindingField'){
            let formatArr=ruleForm.value.itemValueFieldInfo.map((m:any)=>`${m.fieldPerfix?m.fieldPerfix:''}{${m.value}}${m.fieldSuffix?m.fieldSuffix:''}`);
            let formatStr=formatArr.join(ruleForm.value.fieldSeparator);
            ruleForm.value.itemValueFormat=`${ruleForm.value.valuePerfix}${formatStr}${ruleForm.value.valueSuffix}`;
          }
        }
        else{
          ruleForm.value.itemValueFormat=`{value}`;
        }
      } 
    } 
  }

  const submit=()=> {    
      emit('dataSubmit', ruleForm.value) 
  } 
  </script>
  
  <style lang="scss" scoped> 
    .drawer-content{
        padding: 30px 15px;
        position: relative;
        height: 93%; 
        .content-header{ 
            padding:0 20px 0 15px;
            margin-bottom: 10px;
            background-color: rgb(242, 248, 248);
            height: 5%; 
            div{
                padding: 10px 0;
                font-weight: 600; 
                border-radius: 2px; 
            }
        }
        .content-body{
          background-color: rgb(242, 248, 248);
          height: 95%; 
            .body-content{ 
                padding: 0 10px 1px 10px;
                .text-area{
                    min-height: 50px;
                    max-height: 80px; 
                    background-color: #fff;
                    border-radius: 5px;
                    text-align: left;
                    padding: 5px 10px;
                }
            }
            .body-footer{
                text-align: right;
                background-color: rgb(242, 248, 248);
                padding: 0 10px 1px 10px;
                img{
                    height: 45px;
                    cursor: pointer;
                }
            }
        }
        .content-footer{
               text-align: right;
               background-color: rgb(242, 248, 248);
               padding: 0 10px 1px 10px;
               margin-top: 10px;
               height: 8%; 
               position: relative;
               img{
                   height: 45px;
                   cursor: pointer;  
                   position: absolute;
                   right: 2%;
                   top: 50%;
                   transform: translate(0%,-50%);
               }
           } 
    }
  </style>