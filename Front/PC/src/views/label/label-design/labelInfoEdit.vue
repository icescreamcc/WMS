<template>
    <Layer :layer="layer" @confirm="submit" >
       <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
         <el-row>
          <el-col :span="11">
               <el-form-item label="模板ID">
                <el-input v-model="ruleForm.labelId" disabled placeholder="系统生成 无需填写" ></el-input>
           </el-form-item>
          </el-col>
          <el-col :span="11" :offset="2">
                 <el-form-item label="模板名称" prop="labelName">
                    <el-input v-model="ruleForm.labelName"  placeholder="标签模板名称 *必填"></el-input> 
            </el-form-item>
          </el-col>
        </el-row>
         <el-row>
          <el-col :span="11">
             <el-form-item label="物料类型" prop="goodsClassifyGroup">
              <el-select v-model="ruleForm.goodsClassifyGroup" class="m-2" :disabled="disableClassifyGroupSelect" style="width:100%"  placeholder="请选择对应的物料大类 *必填">
                        <el-option v-for="item in goodsClassifyGroupData" :key="item.key" :label="item.value" :value="item.key">
                      </el-option>
                </el-select>
         </el-form-item>   
          </el-col> 
          <el-col :span="11" :offset="2">
          <el-form-item label="备注" prop="remark">
           <el-input v-model="ruleForm.remark"/>
         </el-form-item>
          </el-col>
        </el-row> 
        <el-row>
          <el-col :span="11"> 
            <el-form-item label="标签宽度(mm)" prop="width">
                    <el-input-number v-model="ruleForm.width" controls-position="right" :min="30" :max="140" :precision="2" style="width: 100%;" placeholder="标签宽度 *必填"/>
            </el-form-item>
          </el-col> 
          <el-col :span="11" :offset="2">
            <el-form-item label="标签高度(mm)" prop="height">
                    <el-input-number v-model="ruleForm.height" controls-position="right" :min="20" :max="70" :precision="2" style="width: 100%;" placeholder="标签高度 *必填"/>
            </el-form-item>
          </el-col>
        </el-row> 
        <el-row>
          <el-col :span="11"> 
            <el-form-item label="标签背景色" prop="backgroundColor" style="text-align:left;">
                <el-color-picker v-model="ruleForm.backgroundColor" show-alpha />
                <span style="position: relative;bottom: 10px;left: 10px;">{{ ruleForm.backgroundColor }}</span>
            </el-form-item>
          </el-col> 
          <el-col :span="11" :offset="2">
            <el-form-item label="是否启用" style="text-align:left;">
                <el-checkbox  label="是" v-model="ruleForm.isValid" style="text-align: left;"/>    
            </el-form-item>
          </el-col>
        </el-row> 
            <el-scrollbar max-height="350px">
              <div class="option-content">
              <el-row class="head"> 
                 <el-col :span="21" >
                   <p class="title">模板明细</p>
                </el-col>
                
                <el-col :span="3" style="text-align:right"> 
                  <img src="../../../../public/icon-img/sheji8.png" class="head-btn head-btn-design" @click="onShowDesign" title="排版设计">
                   <img src="../../../../public/icon-img/tianjiaxiangmu.png" class="head-btn head-btn-add" title="新增项" @click="onAddItem">
                </el-col>
               </el-row>  
               <div>
                <el-table class="system-table"  border  height="300"  :data="ruleForm?.details" :row-class-name="tableRowClassName">
                  <el-table-column prop="itemType" label="分类" align="center" width="110" :show-overflow-tooltip="true">
                    <template #default="scope"> 
                      <el-select v-model="scope.row.itemType" class="m-2"  style="width:100%" @change="onSelectItemOption(scope.row)"  placeholder="请选择明细项类型 *必填">
                        <el-option v-for="item in itemTypeData" :key="item.key" :label="item.value" :value="item.key">
                        </el-option>
                      </el-select>
                    </template>
                  </el-table-column> 
                  <el-table-column prop="itemName" label="名称" align="center" width="160" :show-overflow-tooltip="true">
                    <template #default="scope"> 
                      <el-input v-model="scope.row.itemName" @input="onInputItem(scope.row)" :title="scope.row.itemName" placeholder="显示名称 *必填"></el-input> 
                    </template>
                  </el-table-column>    
                  <el-table-column prop="itemValueType" label="值类型" align="center"  width="125" :show-overflow-tooltip="true">
                    <template #default="scope"> 
                      <el-select v-model="scope.row.itemValueType" class="m-2"  style="width:100%" @change="onSelectItemOption(scope.row)" placeholder="请选择值类型 *必填">
                        <el-option v-for="item in itemValueTypeData" :key="item.key" :label="item.value" :value="item.key">
                        </el-option>
                      </el-select>
                    </template>
                  </el-table-column> 
                  <el-table-column prop="itemValueField" label="值" align="center"  width="200" :show-overflow-tooltip="true">
                    <template #default="scope"> 
                      <el-input v-if="scope.row.itemValueType=='Fixed'" v-model="scope.row.itemValueField" @input="onInputItem(scope.row)" :title="scope.row.itemValueField" placeholder="固定文本值"></el-input> 
                      <el-select v-else-if="scope.row.itemValueType=='BindingField'" v-model="scope.row.itemValueField" @change="onSelectItemOption(scope.row)" :title="scope.row.itemValueField" placeholder="绑定物料字段值" multiple clearable collapse-tags collapse-tags-tooltip :max-collapse-tags="3" style="width: 100%;">
                        <el-option v-for="field in itemValueFieldData" :key="field.key" :label="field.value" :value="field.value"/>
                      </el-select>
                    </template>
                  </el-table-column> 
                  <el-table-column prop="itemValueFormat" label="格式化值" align="center"  :show-overflow-tooltip="true">
                    <template #default="scope"> 
                      <el-input :disabled="!scope.row.itemType||!scope.row.itemValueType" v-model="scope.row.itemValueFormat" readonly :title="scope.row.itemValueFormat">
                        <template #append>
                          <el-button type="primary" @click="onShowFormatEdit(scope.row)"><el-icon style="position: relative;top:4px;font-size: 15px;" title="点击编辑格式化明细" :class="scope.row.itemValueFormat?'text-success':''"><Edit /></el-icon></el-button>
                        </template>
                      </el-input> 
                    </template>
                  </el-table-column>
                  <el-table-column label="样式" align="center" width="50" :show-overflow-tooltip="true">
                    <template #default="scope"> 
                      <el-icon class="text-success" v-if="scope.row.itemType" @click="onShowArgsEdit(scope.row)" style="margin-top: 8px;cursor: pointer;" title="点击编辑样式"><MoreFilled /></el-icon> 
                    </template>
                  </el-table-column>
                  <el-table-column align="center" width="50" :show-overflow-tooltip="true">
                    <template #default="scope"> 
                      <el-icon style="margin-top: 10px;font-size: 16px; text-align: center;cursor: pointer;" class="text-danger" @click="onRemoveDetail(scope.row)" title="删除项"><Delete /></el-icon> 
                    </template>
                  </el-table-column>
                </el-table>
               </div>
              </div>
            </el-scrollbar> 
       </el-form>  
       <ValueFormatEdit  :options="valueFormatEditDrawerOption" v-if="valueFormatEditDrawerOption.show" @dataSubmit="onSubmitValueFormat"/>
       <ArgsFormatEdit  :options="argsEditDrawerOption" v-if="argsEditDrawerOption.show" @dataSubmit="onSubmitArgs"/>
       <DesignEdit :options="designEditDrawerOption" v-if="designEditDrawerOption.show" @dataSubmit="onSubmitDesign"/>
    </Layer> 
  </template>
  
  <script lang="ts" setup>
  import {ref,reactive ,defineEmits,defineProps,onMounted} from 'vue' 
  import Layer from '@/components/layer/index.vue' 
  import { ElForm } from 'element-plus' 
  import msg from '@/utils/system/message'
  import{getGoodsGroup} from'@/api/common' 
  import { getOptions } from "@/api/baseinfo/labelDesign";
  import permission from '@/utils/system/permission'  
  import {Delete,Edit,InfoFilled,MoreFilled} from '@element-plus/icons-vue'; 
  import { deftClassifyGroup ,disableClassifyGroupSelect} from '@/config'; 
  import ValueFormatEdit from "./valueFormatEdit.vue";
  import ArgsFormatEdit from "./argsFormatEdit.vue"; 
  import DesignEdit from "./designEdit.vue";
import { stringify } from 'querystring'
  
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
            data:null, 
          }
        }
      }
  }) 
  const emit = defineEmits(['dataSubmit'])    
  const formRef= ref(ElForm||null);  
  const goodsClassifyGroupData=ref(new Array<any>()); 
  const itemTypeData=ref(new Array<any>());
  const itemValueTypeData=ref(new Array<any>());
  const itemValueFieldData=ref(new Array<any>()); 
  const curEditItem=ref(); 
  const barCodeFormats=ref(['CODE128','CODE39']);
  const qrcodeTypes=ref(['DataMatrix','QRCode']);
  const fontFamilys=ref(['Arial','Microsoft YaHei','fantasy','Cambria','Georgia','Impact']);
  const ruleForm = ref({
      labelId:props.layer.data?.labelId,
      labelName:props.layer.data?.labelName, 
      goodsClassifyGroup:props.layer.data?.goodsClassifyGroup||deftClassifyGroup,
      width:props.layer.data?.width, 
      height:props.layer.data?.height, 
      backgroundColor:props.layer.data?.backgroundColor||'#fff', 
      isDeft:props.layer.data?.isDeft, 
      remark:props.layer.data?.remark, 
      details:props.layer.data?.details||[],
      isValid:props.layer.data?props.layer.data.isValid:true,
      createUserId:'',
      createUserName:'',
      updateUserId:'',
      updateUserName:''
  })  
  const rules = {
      labelName: [{ required: true, message: '请输入模板名称', trigger: 'change' },{ max: 50, message: '字符超出限制长度', trigger: 'blur'}], 
      goodsClassifyGroup: [{ required: true, message: '请选择物料类型', trigger: 'change' }],  
      remark:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
      width:[{ required: true, message: '请输入标签宽度', trigger: 'change' }],
      height:[{ required: true, message: '请输入标签高度', trigger: 'change' }]
  } 
  const valueFormatEditDrawerOption=reactive({
      show: false,
      title: '', 
      type:'', 
      data:{}
    });
  const argsEditDrawerOption=reactive({
      show: false,
      title: '', 
      type:'', 
      data:{},
      dataOptions:{
        barCodeFormats:barCodeFormats.value,
        qrcodeTypes:qrcodeTypes.value,
        fontFamilys:fontFamilys.value
      }
    });
  const designEditDrawerOption=reactive({
      show: false,
      title: '标签设计排版', 
      type:'', 
      data:{},
      dataOptions:{ 
        fontFamilys:fontFamilys.value
      }
    });
  
  onMounted(()=>{   
    getGoodsGroup().then(res=>{
      goodsClassifyGroupData.value=res.data;
    });
    getOptions().then(res=>{
      itemTypeData.value=res.data.itemTypeOption;
      itemValueTypeData.value=res.data.itemValueTypeOption;
      itemValueFieldData.value=res.data.itemValueFieldOption; 
    })
  })

  const tableRowClassName=({row,index}:any)=>{
    if(row.isUsed){
      return 'success-row'
    } 
  }
  
  const onAddItem=()=>{
    ruleForm.value.details.push({
      labelId:ruleForm.value.labelId,
      itemId:0,
      itemType:'',
      itemName:'',
      itemValueType:'',
      itemValueField:'', 
      itemValueFormat:'',
      showName:true,
      isUsed:false,
      deftValue:'',
      style:{},
      args:null,
      remark:'',
      valuePerfix:'',
      valueSuffix:'',
      fieldSeparator:'',
      itemValueFieldInfo:[],
    });
  }
  
  const onRemoveDetail=(row:any)=>{
    ruleForm.value.details.splice( ruleForm.value.details.indexOf(row),1);
  }

  const onSelectItemOption=(row:any)=>{
    setDeftArgs(row);
    if(row.itemValueType=='BindingField'&&row.itemValueField){
      row.itemValueFieldInfo=row.itemValueField.map((m:string)=>{
          return{
              key:itemValueFieldData.value.find((f:any)=>f.value==m).key,
              value:m
          }
      })
    }
    else{
      row.itemValueFieldInfo.length=0;
    }
    if(row.itemType&&row.itemValueType){ 
      if(row.itemType=='Text'){
        if(row.itemName){
          if(row.itemValueType=='Fixed'){
            if(row.showName){
              row.itemValueFormat=`${row.itemName}：${row.valuePerfix}${row.itemValueField}${row.valueSuffix}`;
            }
            else{
              row.itemValueFormat=`${row.valuePerfix}${row.itemValueField}${row.valueSuffix}`;
            } 
          }
          else if(row.itemValueType=='BindingField'){
            if(row.itemValueField){ 
              let formatArr=row.itemValueFieldInfo.map((m:any)=>`${m.fieldPerfix?m.fieldPerfix:''}{${m.value}}${m.fieldSuffix?m.fieldSuffix:''}`);
              let formatStr=formatArr.join(row.fieldSeparator);
              if(row.showName){
                row.itemValueFormat=`${row.itemName}：${row.valuePerfix}${formatStr}${row.valueSuffix}`;
              }
              else{
                row.itemValueFormat=`${row.valuePerfix}${formatStr}${row.valueSuffix}`;
              } 
            } 
          } 
        }
        else if(row.itemValueField){
          if(row.itemValueType=='Fixed'){
              row.itemValueFormat=`${row.valuePerfix}${row.itemValueField}${row.valueSuffix}`;
          }
          else if(row.itemValueType=='BindingField'){
              let formatArr=row.itemValueFieldInfo.map((m:any)=>`${m.fieldPerfix?m.fieldPerfix:''}{${m.value}}${m.fieldSuffix?m.fieldSuffix:''}`);
              let formatStr=formatArr.join(row.fieldSeparator);
              row.itemValueFormat=`${row.valuePerfix}${formatStr}${row.valueSuffix}`;
          } 
        }
        else{
          row.itemValueFormat=`{value}`;
        } 
      } 
      else if(row.itemType=='QRCode'){
        if(row.itemValueField){
          if(row.itemValueType=='Fixed'){
            row.itemValueFormat=`${row.valuePerfix}${row.itemValueField}${row.valueSuffix}`;
          }
          else if(row.itemValueType=='BindingField'){
            let formatArr=row.itemValueFieldInfo.map((m:any)=>`${m.fieldPerfix?m.fieldPerfix:''}{${m.value}}${m.fieldSuffix?m.fieldSuffix:''}`);
            let formatStr=formatArr.join(row.fieldSeparator);
            row.itemValueFormat=`${row.valuePerfix}${formatStr}${row.valueSuffix}`;
          } 
        }
        else{
          row.itemValueFormat=`{value}`;
        }   
      }
      else if(row.itemType=='BarCode'){
        if(row.itemValueField){
          if(row.itemValueType=='Fixed'){
            row.itemValueFormat=`${row.valuePerfix}${row.itemValueField}${row.valueSuffix}`;
          }
          else if(row.itemValueType=='BindingField'){
            let formatArr=row.itemValueFieldInfo.map((m:any)=>`${m.fieldPerfix?m.fieldPerfix:''}{${m.value}}${m.fieldSuffix?m.fieldSuffix:''}`);
            let formatStr=formatArr.join(row.fieldSeparator);
            row.itemValueFormat=`${row.valuePerfix}${formatStr}${row.valueSuffix}`;
          }
        }
        else{
          row.itemValueFormat=`{value}`;
        } 
      } 
    } 
  }

  const setDeftArgs=(row:any)=>{
    if(row.itemType=='Text'){
      if(!row.args){
          row.args= {
            font:fontFamilys.value[0],
            fontSize:12,
            color:'#666',
            weight:false
          }
        } 
    }
    else if(row.itemType=='QRCode'){
      if(!row.args){
          row.args={  
              format:qrcodeTypes.value[0],
              width:22,
              height:22 
          }
        }
    }
    else if(row.itemType=='BarCode'){
      if(!row.args){
          row.args={  
            format:barCodeFormats.value[0],
            width:1,
            height:15,
            lineColor:'#666',
            displayValue:true,
            font:fontFamilys.value[0],
            fontSize:10,
            textMargin:0  
          }
        }
    }
  }

  const onInputItem=(row:any)=>{
    onSelectItemOption(row);
    let itemNameValid=ruleForm.value.details.filter((f:any)=>f.itemName); 
    if(itemNameValid.length>0){
      let itemNameDistinct=[...new Set(itemNameValid.map((m:any)=>m.itemName))];   
      if(itemNameDistinct.length!=itemNameValid.length){
        msg.warningAuto("名称不允许有重复");
        row.itemName='';
      }
    } 
  }

  const onShowFormatEdit=(row:any)=>{
    if(row.itemValueFormat){   
      curEditItem.value=row;
      valueFormatEditDrawerOption.data=row; 
      valueFormatEditDrawerOption.show=true; 
    } 
  }

  const onShowArgsEdit=(row:any)=>{ 
    curEditItem.value=row;
    argsEditDrawerOption.show=true;
    argsEditDrawerOption.data=row;
  }

  const onShowDesign=()=>{
    if(!ruleForm.value.height||Number(ruleForm.value.height)<=0||!ruleForm.value.width||Number(ruleForm.value.height)<=0){
      msg.deftAuto("排版设计之前，请先设置正确的标签高度和宽度");
      return;
    }
    if(ruleForm.value.details.length>0){
      for(let i=0;i<ruleForm.value.details.length;i++){
        let curDetail=ruleForm.value.details[i];
        if(!curDetail.itemType||!curDetail.itemName){
          msg.deftAuto("排版设计之前，请先为每个明细项指定一个分类和名称");
          return;
        } 
      }
      designEditDrawerOption.data=ruleForm.value;
      designEditDrawerOption.show=true;
    } 
    else{
      msg.deftAuto("先添加明细项");
    }
  }

  const onSubmitValueFormat=(data:any)=>{
    curEditItem.value.itemValueFieldInfo=data.itemValueFieldInfo; 
    curEditItem.value.valuePerfix=data.valuePerfix;
    curEditItem.value.valueSuffix=data.valueSuffix;
    curEditItem.value.fieldSeparator=data.fieldSeparator;
    curEditItem.value.itemValueFormat=data.itemValueFormat; 
    curEditItem.value.showName=data.showName;
    valueFormatEditDrawerOption.show=false;   
  }

  const onSubmitArgs=(data:any)=>{
    curEditItem.value.args=data;
    argsEditDrawerOption.show=false; 
  }
 
  const onSubmitDesign=(data:any)=>{
    ruleForm.value=data;
    designEditDrawerOption.show=false;
  }
 
  const submit=()=> {    
      formRef.value.validate((valid:any)=>{ 
          if(valid){ 
            if(ruleForm.value.details.length==0){
              msg.deftAuto("请添加模板明细项");
              return;
            }
            let submitData=JSON.parse(JSON.stringify(ruleForm.value));
            for(let i=0;i<submitData.details.length;i++){
              let detail=submitData.details[i];
              if(!detail.itemName){
                msg.deftAuto("请给明细项指定一个名称");
                return;
              }
              if(!detail.itemType){
                msg.deftAuto(`请给定明细项${detail.itemName}指定一个类型`);
                return;
              } 
              if(!detail.itemValueType){
                msg.deftAuto(`请给明细项${detail.itemName}指定一个值类型`);
                return;
              }
              if(detail.itemValueType&&!detail.itemValueField){
                msg.deftAuto(`请给明细项${detail.itemName}指定一个固定值或绑定字段`);
                return;
              } 
              detail.args=JSON.stringify(detail.args);
              if(detail.itemValueType=='BindingField'){
                detail.itemValueField=JSON.stringify(detail.itemValueField); 
              } 
              detail.itemValueFieldInfo=JSON.stringify(detail.itemValueFieldInfo);
              detail.style=JSON.stringify(detail.style); 
            }
            let editType=props.layer.data?'update':'add';
            if(editType=='add'){
              submitData.createUserId=permission.getOperator().userId;
              submitData.createUserName=permission.getOperator().userName; 
            }
            else{
              submitData.updateUserId=permission.getOperator().userId;
              submitData.updateUserName=permission.getOperator().userName; 
            }  
            emit('dataSubmit', submitData,editType);  
          }
      }) 
  } 
  </script>
  
  <style lang="scss" scoped>  
 :deep .el-table .warning-row {
   background-color: #e6a23c;
  }
  :deep .el-table .success-row {
    background-color: #89e25c30;
  }
    .option-content{
      border:1px solid rgb(230, 230, 230);
      border-radius: 3px;
      padding: 5px;
      .head{ 
        margin-bottom: 5px;
        .title{
          margin: 5px 0 0 0;
        }
        .head-btn-add{
          height: 28px;
          cursor: pointer; 
        }
        .head-btn-design{
          height: 28px;
        
          cursor: pointer; 
          margin-right: 15px;
          position: relative;
          top: -2px;
        } 
        .head-btn:hover{ 
          box-shadow: 2px 2px 3px #888;
        }
      } 
    }
  </style>