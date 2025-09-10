<template>
     <div>
        <el-drawer v-model="props.options.show" :with-header="false" :show-close="true"  size="33%">
        <div class="content">
          <div class="content-header text-success">
            <h4><el-icon style="position: relative; top: 2px;font-size: medium;"><InfoFilled /></el-icon>&nbsp;{{ props.options.title }}</h4> 
          </div>
          <div class="content-form">
            <el-row style="margin-bottom: 5px;" :gutter="20">
              <el-col :span="12">
                <el-select v-model="isSAP"  class="m-2" style="width:100%" @change="getGoodsData">
                         <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">SAP</div></template>
                         <el-option value="" label="All"/> 
                         <el-option value="1" label="是"/> 
                         <el-option vaue="0" label="否"/> 
                   </el-select>
              </el-col>
              <el-col :span="12">
                <el-select v-model="selectedGoodsClassifyId"  class="m-2" style="width:100%" @change="getGoodsData">
                        <template #prefix><div style="color:#909399; border: 1px solid #dcdfe6;border-left: none;height: 26px;background-color: #f5f7fa;padding: 0 7px;position: relative;left: -3px;margin-right:10px ;">分类</div></template>
                        <el-option v-for="item in goodsClassifyData" :key="item.key" :label="item.value" :value="item.key">
                    </el-option>
                  </el-select>
              </el-col>
            </el-row> 
            <el-row>
              <el-col>
                  <el-input v-model="goodsSearchKey" clearable @clear="getGoodsData" placeholder="可按关键字检索" >
                  <template #append>
                    <el-button type="primary" @click="getGoodsData"><el-icon style="position: relative;top:4px;font-size: 15px;"><Search /></el-icon></el-button>
                  </template>
                </el-input>
              </el-col>
            </el-row>
           </div>
           <div class="content-body" :style="{height:drawerHeight+'px'}">
            <el-row class="content-item" v-if="goodsData.length>0" v-for="item in goodsData" :key="item.goodsId" @click="selectGoods(item)">
                <el-col :span="22">
                  <div class="item-img">
                        <el-image style="width: 128px; height: 128px;border-radius: 4px;" hide-on-click-modal :src="item.goodsPicture"  :zoom-rate="1.2" :preview-src-list="[item.goodsPicture]" :initial-index="0"  fit="cover" />
                      </div>
                  <div class="item-desc">
                        <div class="desc-title" :title="item.goodsName+' '+item.goodsModel+' '+item.goodsId">{{ item.goodsName }}<span class="desc-info" v-if="item.goodsModel">{{ ' '+item.goodsModel }}</span></div>
                        <div class="desc-info">分&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;类：{{item.goodsClassifyName}}</div>
                        <div class="desc-info" :title="item.supplier">供&nbsp;&nbsp;&nbsp;应&nbsp;&nbsp;&nbsp;&nbsp;商：{{ item.supplier }}</div>
                        <div class="desc-info">储&nbsp;存&nbsp;规&nbsp;&nbsp;格：{{ item.goodsSpecificationName }}</div>
                        <!-- <div class="desc-info">是否在SAP：<span v-if="item.isInSAP">是</span><span v-else>否</span></div> -->
                        <div class="desc-info">SAP编码：{{ item.goodsNo }}</div>
                        <div class="desc-info">是否在盘点：<span v-if="item.isTakeStockLock" style="color: #e6a23c">是</span><span v-else>否</span></div>
                        <div class="desc-info">
                          <span>包装：</span>
                          <span v-if="item.minPackageUnitName==item.packageUnitName&&item.packageUnitName==item.maxPackageUnitName">{{item.packageCount+item.minPackageUnitName}}</span>
                          <span v-else-if="item.minPackageUnitName!=item.packageUnitName&&item.packageUnitName==item.maxPackageUnitName">{{item.packageCount+item.minPackageUnitName+'/'+item.packageUnitName}}</span>
                          <span v-else-if="item.minPackageUnitName==item.packageUnitName&&item.packageUnitName!=item.maxPackageUnitName">{{item.maxPackageCount+item.packageUnitName+'/'+item.maxPackageUnitName}}</span>
                          <span v-else>{{item.packageCount+item.minPackageUnitName+'/'+item.packageUnitName+','+item.maxPackageCount+item.packageUnitName+'/'+item.maxPackageUnitName}}</span>
                        </div>
                        <div class="desc-info">库&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;存：
                          <span v-if="item.minPackageUnitName==item.packageUnitName&&item.packageUnitName==item.maxPackageUnitName">
                              <span>{{item.standardPackageStock+item.packageUnitName}}</span>  
                          </span>
                            <span v-else-if="item.minPackageUnitName!=item.packageUnitName&&item.packageUnitName==item.maxPackageUnitName">
                                <span v-if="item.standardPackageStock!=0">{{item.standardPackageStock+item.packageUnitName}}</span> 
                                <span v-if="item.minPackageStock!=0">{{item.minPackageStock+item.minPackageUnitName}}</span> 
                                <span v-if="item.standardPackageStock==0&&item.minPackageStock==0&&item.maxPackageStock==0">0</span>  
                            </span>
                            <span v-else-if="item.minPackageUnitName==item.packageUnitName&&item.packageUnitName!=item.maxPackageUnitName">
                                <span v-if="item.maxPackageStock!=0">{{item.maxPackageStock+item.maxPackageUnitName}}</span>  
                                <span v-if="item.standardPackageStock!=0">{{item.standardPackageStock+item.packageUnitName}}</span>
                                <span v-if="item.standardPackageStock==0&&item.minPackageStock==0&&item.maxPackageStock==0">0</span>   
                            </span>
                            <span v-else>
                                <span v-if="item.maxPackageStock!=0">{{item.maxPackageStock+item.maxPackageUnitName}}</span>  
                                <span v-if="item.standardPackageStock!=0">{{item.standardPackageStock+item.packageUnitName}}</span>   
                                <span v-if="item.minPackageStock!=0">{{item.minPackageStock+item.minPackageUnitName}}</span>
                                <span v-if="item.standardPackageStock==0&&item.minPackageStock==0&&item.maxPackageStock==0">0</span>   
                            </span>   
                        </div>
                      </div> 
                </el-col>
                <el-col :span="2">
                 <div class="item-chk"> 
                  <img v-if="item.selected" src="/public/icon-img/xuanze_chk2.png" height="20">
                  <img v-else src="/public/icon-img/xuanze.png" height="20">
                 </div>
                </el-col>
              </el-row>
              <el-row v-else>
                <el-col><el-empty description="没有任何数据" /></el-col>
              </el-row>
           </div> 
        </div>
      </el-drawer>
     </div>
</template>

<script lang="ts" setup>
import {ref ,defineEmits,defineProps,onMounted,nextTick,onBeforeMount,onBeforeUnmount} from 'vue'; 
import{getGoodsByKeyAndClassify}from '@/api/common';
import { Search,InfoFilled } from '@element-plus/icons-vue';
import {getGoodsGroup,getGoodsClassify} from '@/api/common'; 

const props=defineProps({
  options: {
      type: Object,
      default: () => {
        return {
          show: false,
          title: '', 
          type:'', 
          mode:'no-repet',
          isMultiSelect:true,
          data:null 
        }
      }
    }
});

const drawerHeight=ref(400);
const emit = defineEmits(['selectItem'])
const goodsData=ref(new Array<any>()); 
const goodsSearchKey=ref(''); 
const goodsClassifyData=ref(new Array<any>()); 
const selectedGoodsClassifyId=ref(0);
const isSAP=ref("");
 
onBeforeMount(()=>{
  drawerHeight.value=window.innerHeight-150;
})

onMounted(()=>{  
  document.addEventListener('keydown', handleKeyDown); 
  getGoodsClassifyData();
  getGoodsData();
})
 
onBeforeUnmount(()=>{ 
  document.removeEventListener('keydown', handleKeyDown); 
}) 

const getGoodsClassifyData=()=>{
    return getGoodsClassify(props.options.type).then(res=>{
        goodsClassifyData.value=[{key:0,value:'All'},...res.data];
    })
}

const getGoodsData=()=>{  
  getGoodsByKeyAndClassify(props.options.type,selectedGoodsClassifyId.value,isSAP.value, goodsSearchKey.value,60).then((res:any)=>{
     goodsData.value = res.data;

    if(props.options.mode=='no-repet'){
      if(props.options.data?.length>0){
        goodsData.value.forEach(f=>{
          props.options.data.forEach((f2:any)=>{
            if(f.goodsId==f2.goodsId){
              f.selected=true;
            }
          })
        })
      } 
    } 

  });
}

const handleKeyDown=(event:any)=> {
      if (event.keyCode === 13) {
        event.preventDefault(); 
        getGoodsData(); 
      }
    }

const selectGoods=(item:any)=>{ 
  if(props.options.isMultiSelect){
    item.selected=!item.selected;
  }
  else{
    goodsData.value.forEach(f=>f.selected=false);
    item.selected=true;
  } 
  if(props.options.mode=='no-repet'){
    emit('selectItem',goodsData.value.filter(f=>f.selected));
  }
  else{
    if(item.selected){
      emit('selectItem',item); 
    } 
  } 
}
</script>
<style lang="scss" scoped>
.content{  
    height: 100%;
    position: relative;
    .content-header{
      margin-left: 10px; 
      text-align: left; 
      h4{
        background-color: #f5f7fa; 
        padding: 5px 3px;
      }
    }
    .content-form{
      padding:5px 10px;
    }
    .content-body{ 
      height: 70%;
      padding: 10px;
      overflow-y: scroll;
      .content-item{
          padding: 10px;
          margin-bottom: 3px;
          background-color: #eff0f0;
          border-radius: 4px;
          cursor: pointer;
          .item-img{ 
            float: left;
            width: 135px;
            text-align: left;
          }
          .item-desc{  
            float: left;
            text-align: left;
            width: 60%; 
            .desc-title{ 
              font-size:12px; 
              font-weight: 600; 
              width: 100%;
              overflow: hidden;
              white-space: nowrap;
              text-overflow: ellipsis;
              -o-text-overflow: ellipsis; 
            }
            .desc-info{
              font-size:12px; 
              color:#888;
              padding-top: 3px;
            }
          }
          .item-chk{
            padding:25px 0;
            text-align: right; 
          }
        }
    }
    .content-footer{
       padding: 10px; 
       width: 28.5%;
       position: fixed;
       bottom: 0;
    } 
}
</style>