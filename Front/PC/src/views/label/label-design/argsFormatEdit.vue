<template>
    <el-drawer v-model="props.options.show" :with-header="false" :show-close="true"  size="33%"> 
       <div class="drawer-content">
           <div class="content-header">
               <div>{{ title }}</div>
           </div>
           <div class="content-body">
                <el-form v-if="itemType=='Text'" :model="textRuleForm" :rules="textRules" ref="textFormRef" label-width="auto" label-position="top" style="padding:0 15px;">  
                <el-form-item label="字体样式"  prop="format" style="text-align: left;">
                    <el-select v-model="textRuleForm.font" class="m-2"  style="width:60%"  placeholder="请选择字体样式 *必填">
                            <el-option v-for="item in fontFamilys" :key="item" :label="item" :value="item">
                            </el-option>
                    </el-select>
                </el-form-item> 
                <el-form-item label="字体大小(px)" prop="height" style="text-align: left;">
                    <el-input-number v-model="textRuleForm.fontSize" controls-position="right" @change="onInputQRCodeSize"  style="width:60%" :min="5" :max="30" placeholder="二维码高度：高=宽" />
                </el-form-item> 
                <el-form-item label="字体颜色" prop="height" style="text-align: left;">
                    <el-color-picker v-model="textRuleForm.color" show-alpha /> 
                        <span style="position: relative;bottom: 10px;left: 10px;">{{ textRuleForm.color }}</span>
                </el-form-item> 
                <el-form-item label="加粗" prop="width" style="text-align: left;">
                    <el-checkbox  label="是" v-model="textRuleForm.weight" style="text-align: left;"/>   
                </el-form-item>   
              </el-form> 
               <el-form v-if="itemType=='QRCode'" :model="qrRuleForm" :rules="qrRules" ref="qrFormRef" label-width="auto" label-position="top" style="padding:0 15px;">  
                    <el-form-item label="类型"  prop="format" style="text-align: left;">
                        <el-select v-model="qrRuleForm.format" class="m-2"  style="width:60%;"  placeholder="请选二维码类型 *必填">
                                <el-option v-for="item in qrcodeTypes" :key="item" :label="item" :value="item">
                            </el-option>
                        </el-select>
                    </el-form-item> 
                    <el-form-item label="高度(mm)" prop="height" style="text-align: left;">
                        <el-input-number v-model="qrRuleForm.height" controls-position="right" @change="onInputQRCodeSize"  style="width:60%" :precision="2" :min="10" :max="200" placeholder="二维码高度：高=宽" />
                    </el-form-item> 
                    <el-form-item label="宽度(mm)" prop="width" style="text-align: left;">
                        <el-input-number v-model="qrRuleForm.height" controls-position="right" @change="onInputQRCodeSize" style="width:60%" :precision="2" :min="10" :max="200" placeholder="二维码宽度：高=宽" />  
                    </el-form-item>  
             
               </el-form> 
               <el-form  v-if="itemType=='BarCode'" :model="barRuleForm" :rules="barRules" ref="barFormRef" label-width="auto" label-position="top" style="padding:0 15px;">
                    <el-form-item label="格式"  prop="bar_format" style="text-align: left;">
                        <el-select v-model="barRuleForm.format" class="m-2"  style="width:60%"  placeholder="请选择条形码的格式 *必填">
                                <el-option v-for="item in barCodeFormats" :key="item" :label="item" :value="item">
                            </el-option>
                        </el-select>
                    </el-form-item> 
                    <el-form-item label="高度(mm)" prop="height" style="text-align: left;">
                        <el-input-number v-model="barRuleForm.height" controls-position="right" style="width:60%" :precision="2" :min="5" :max="50" placeholder="条码高度 *必填" />   
                    </el-form-item>
                    <el-form-item label="线宽(px)" prop="width" style="text-align: left;">
                        <el-input-number v-model="barRuleForm.width" controls-position="right" style="width:60%" :precision="2" :step="0.1" :min=".1" :max="1" placeholder="条码线宽 *必填" />  
                    </el-form-item> 
                    <el-form-item label="线条颜色" prop="lineColor" style="text-align:left;">
                        <el-color-picker v-model="barRuleForm.lineColor" show-alpha /> 
                        <span style="position: relative;bottom: 10px;left: 10px;">{{ barRuleForm.lineColor }}</span>
                    </el-form-item>
                    <el-form-item label="是否显示文本" style="text-align: left;">
                        <el-checkbox  label="是" v-model="barRuleForm.displayValue" style="text-align: left;"/>    
                    </el-form-item>
                    <el-form-item label="字体样式"  prop="font" style="text-align: left;">
                        <el-select v-model="barRuleForm.font" class="m-2"  style="width:60%"  placeholder="请选择字体样式 *必填">
                                <el-option v-for="item in fontFamilys" :key="item" :label="item" :value="item">
                            </el-option>
                        </el-select>
                    </el-form-item> 
                    <el-form-item label="字体大小(px)" prop="fontSize" style="text-align:left;">
                        <el-input-number v-model="barRuleForm.fontSize" style="width:60%" :precision="1" :min="5" :max="30" placeholder="文本字体大小" />    
                    </el-form-item> 
                    <el-form-item label="文本与条码距离(px)" prop="textMargin" style="text-align:left;">
                        <el-input-number v-model="barRuleForm.textMargin" style="width:60%" :min="0" :max="40" placeholder="文本与条码距离" />   
                    </el-form-item> 
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
 const props=defineProps({
   options: {
       type: Object,
       default: () => {
           return {
           show: false,
           title: '', 
           type:'',  
           data:{},
           dataOptions:{}
           }
       }
       }
   }); 
 const emit = defineEmits(['dataSubmit'])     
 const itemType=ref(props.options.data.itemType);
 const title=ref('');  
 const barCodeFormats=ref(props.options.dataOptions.barCodeFormats);
 const qrcodeTypes=ref(props.options.dataOptions.qrcodeTypes);
 const fontFamilys=ref(props.options.dataOptions.fontFamilys);
 const textFormRef=ref(ElForm||null);
 const qrFormRef= ref(ElForm||null); 
 const barFormRef= ref(ElForm||null); 
 const textRuleForm = ref({  
    font:props.options.data.args?.font,
    fontSize:props.options.data.args?.fontSize,
    color:props.options.data.args?.color,
    weight:props.options.data.args.weight 
 });
 const qrRuleForm = ref({  
    format:props.options.data.args?.format,
    width:props.options.data.args?.width,
    height:props.options.data.args.height
 });
 const barRuleForm = ref({  
    format:props.options.data.args?.format,
    width:props.options.data.args?.width,
    height:props.options.data.args.height,
    lineColor:props.options.data.args.lineColor,
    displayValue:props.options.data.args.displayValue,
    font:props.options.data.args.font,
    fontSize:props.options.data.args.fontSize,
    textMargin:props.options.data.args.textMargin  
 })  
 const textRules = {
    font: [{ required: true, message: '请选择字体样式', trigger: 'change' }], 
    fontSize:[{ required: true, message: '请指定字体大小', trigger: 'change' }] 
  } 
 const qrRules = {
    width: [{ required: true, message: '请输入二维码宽度', trigger: 'change' }], 
    height:[{ required: true, message: '请输入二维码高度', trigger: 'change' }], 
    format: [{ required: true, message: '请选则二维码类型', trigger: 'change' }] 
  }  
 const barRules = {  
    height:[{ required: true, message: '请输入一维码高度', trigger: 'change' }], 
    format: [{ required: true, message: '请选则一维码格式', trigger: 'change' }],
    font: [{ required: true, message: '请选择字体样式', trigger: 'change' }], 
    fontSize:[{ required: true, message: '请指定字体大小', trigger: 'change' }]  
  } 

 onMounted(()=>{   
    if(itemType.value=='QRCode'){
        title.value='二维码参数编辑'; 
    } 
    else if(itemType.value=='BarCode'){
        title.value='一维码参数编辑'; 
    }  
    else if(itemType.value=='Text'){
        title.value='文本参数编辑'; 
    } 
 })
   
 const onInputQRCodeSize=()=>{ 
    qrRuleForm.value.width= qrRuleForm.value.height;
 }

 const submit=()=> {    
    if(itemType.value=='QRCode'){
        qrFormRef.value.validate((valid:any)=>{ 
          if(valid){ 
            emit('dataSubmit', qrRuleForm.value); 
          }
      })  
    }
    else if(itemType.value=='BarCode'){
        barFormRef.value.validate((valid:any)=>{ 
          if(valid){ 
            emit('dataSubmit', barRuleForm.value); 
          }
      })  
    }  
    else if(itemType.value=='Text'){
        textFormRef.value.validate((valid:any)=>{ 
          if(valid){ 
            emit('dataSubmit', textRuleForm.value); 
          }
      })  
    }  
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
               padding: 0 10px 1px 10px;
               height: 95%; 
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