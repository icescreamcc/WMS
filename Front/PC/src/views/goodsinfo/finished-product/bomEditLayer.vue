<template>
    <Layer :layer="layer" @confirm="submit" >
       <el-form :model="ruleForm"  ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
         <el-row>
          <el-col :span="11">
            <el-form-item label="成品编号" prop="orderNo">
              <el-input v-model="ruleForm.goodsNo" readonly ></el-input>
            </el-form-item> 
          </el-col>
          <el-col :span="11" :offset="2"> 
            <el-form-item label="成品名称" prop="orderNo">
              <el-input v-model="ruleForm.goodsName" readonly ></el-input>
            </el-form-item> 
          </el-col>
        </el-row> 
        <el-row>
        <el-col :span="11">
           <el-form-item label="BOM类型" prop="goodsClassify">
            <el-select v-model="selectedGoodsClassifyGroup" class="m-2" disabled style="width:100%"  placeholder="请选择BOM类型 *必填">
                      <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
              </el-select>
       </el-form-item>   
        </el-col>  
      </el-row> 
            <el-scrollbar max-height="300px">
              <div class="option-content">
              <el-row class="head"> 
                 <el-col :span="21" style="text-align: left;">
                   <p class="title">BOM明细</p>
                </el-col>
                <el-col :span="3" style="text-align:right">
                   <el-button style="margin-bottom:5px" type="success" @click="onShowGoodsDrawer">选择{{ invTitle }}</el-button> 
                </el-col>
               </el-row> 
               <el-row v-for="detail in ruleForm.bomInfo" :key="detail.detailId" class="item">
                <el-col :span="12">
                  <el-input v-model="detail.materialFullName"  readonly :title="detail.materialFullName">
                    <template #prepend><div style="width: 40px;">{{ detail.materialClassifyName }}</div></template> 
                  </el-input> 
                </el-col> 
                <el-col :span="4" style="text-align:center">
                    <el-input v-model="detail.quantity" style="width:95%" placeholder="数量" type="number"/>
                </el-col>
                  <el-col :span="6" style="text-align:center">
                     <el-select v-model="detail.unit" class="m-2" style="width:90%" placeholder="单位">
                        <el-option v-for="item in detail.materialUnitList" :key="item.key" :label="item.value" :value="item.value">
                      </el-option>
                       </el-select>
                </el-col> 
                <el-col :span="2"> 
                  <el-button class="text-danger" style="height: 22px;line-height: 15px;" @click="onRemoveDetail(detail)"><el-icon ><Delete /></el-icon></el-button>
                </el-col>
             </el-row>  
              </div>
            </el-scrollbar> 
            <GoodsSelectDrawer :options="goodsDrawerOptions" v-if="goodsDrawerOptions.show" @selectItem="onSelectGoods"/> 
       </el-form>  
    </Layer> 
  </template>
  
  <script lang="ts" setup>
  import {ref ,defineEmits,defineProps,onMounted} from 'vue' 
  import Layer from '@/components/layer/index.vue' 
  import { ElForm } from 'element-plus' 
  import msg from '@/utils/system/message'  
  import GoodsSelectDrawer from '@/components/drawer/goodsSelector.vue' 
  import {Delete} from '@element-plus/icons-vue';
  import{getUnits}from '@/api/common'; 
  
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
  const goodsClassifyData=ref([{key:'PackingMaterial',value:'包材'}]);
  const selectedGoodsClassifyGroup=ref('PackingMaterial');
  const invTitle=ref('包材');  
  const emit = defineEmits(['dataSubmit'])  
  const unitData=ref(new Array<any>());     
  const formRef= ref(ElForm||null);     
  const ruleForm = ref({
      goodsId:props.layer.data?.goodsId,
      goodsNo:props.layer.data?.goodsNo,
      goodsName:props.layer.data?.goodsName,
      bomInfo:props.layer.data?.bomInfo
      
  })   
  const goodsDrawerOptions= ref({
    show: false,
    title: '', 
    mode:'no-repet',
    type:selectedGoodsClassifyGroup.value,
    isMultiSelect:true,
    data:new Array<any>() 
  }); 
  
  onMounted(()=>{     
    getUnits("Pack").then((res:any)=>{   
      unitData.value=res.data;  
      if(ruleForm.value.bomInfo?.length>0){   
        ruleForm.value.bomInfo.forEach((detail:any)=>{ 
          detail.materialUnitList=unitData.value.filter(u=>u.value==detail.packageUnitName||u.value==detail.minPackageUnitName||u.value==detail.maxPackageUnitName);    
          detail.materialFullName=detail.materialModel?detail.materialName+' '+detail.materialModel:detail.materialName; 
        }); 
      } 
    })
  })
    
  const onShowGoodsDrawer=()=>{ 
    goodsDrawerOptions.value.show=true; 
    goodsDrawerOptions.value.title=invTitle.value+'选择';
    goodsDrawerOptions.value.data=ruleForm.value.bomInfo?.map((m:any)=>{
        return {
            goodsId:m.materialId
        }
    });
  }
    
  const onSelectGoods=(goodsArr:[])=>{     
    goodsArr.forEach((newItem:any)=>{
        let isExist=false;
        ruleForm.value.bomInfo.forEach((oldItem:any)=>{
          if(newItem.goodsId==oldItem.materialId){
            isExist=true;
          } 
        });
        if(!isExist){
            ruleForm.value.bomInfo.push({
                materialId:newItem.goodsId,
                materialName:newItem.goodsName,
                materialClassifyGroup:selectedGoodsClassifyGroup.value,
                materialClassifyName:newItem.goodsClassifyName,
                minPackageUnitId:newItem.minPackageUnitId,
                minPackageUnitName:newItem.minPackageUnitName,
                packageUnitId:newItem.packageUnitId,
                packageUnitName:newItem.packageUnitName,
                maxPackageUnitId:newItem.maxPackageUnitId,
                maxPackageUnitName:newItem.maxPackageUnitName,
                group:selectedGoodsClassifyGroup.value,
                parentId:ruleForm.value.goodsId,
                quantity:0, 
                materialFullName:newItem.goodsModel?newItem.goodsName+' '+newItem.goodsModel:newItem.goodsName,
                materialUnitList:unitData.value.filter(u=>u.value==newItem.packageUnitName||u.value==newItem.minPackageUnitName||u.value==newItem.maxPackageUnitName), 
                unit:newItem.packageUnitName
            });
        }
      });   
  }
  
  const onRemoveDetail=(detail:any)=>{ 
    ruleForm.value.bomInfo.splice(ruleForm.value.bomInfo.indexOf(detail),1);
  }
     
  const submit=()=> {    
      formRef.value.validate((valid:any)=>{ 
          if(valid){ 
            if(ruleForm.value.bomInfo.length==0){
              msg.warningAuto("请添加BOM明细")
              return
            }
            else{  
                for(let opt of ruleForm.value.bomInfo){  
                  if(!opt.quantity||Number(opt.quantity)<=0){
                    msg.warningAuto("请输入正确的数量")
                    return
                  }
                  if(!opt.unit){
                    msg.warningAuto("请选择单位")
                    return
                  }  
                } 
            }  
              emit('dataSubmit', ruleForm.value.bomInfo) 
          }
      }) 
  } 
  </script>
  
  <style lang="scss" scoped> 
    .box-card{
      margin-top: 10px;
    }
    .option-content{
      border:1px solid rgb(230, 230, 230);
      border-radius: 3px;
      padding: 5px;
      .head{
       // border-bottom: 1px solid rgb(230, 230, 230);
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