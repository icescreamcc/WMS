<template>
    <Layer :layer="layer"> 
      <el-table class="system-table" ref="reftable"  border  height="500"  :data="layer?.data">
          <el-table-column prop="consignNum" label="发货型号" align="center" sortable min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="prodctionTypeNo" label="产品型号" align="center" sortable min-width="100" :show-overflow-tooltip="true"/> 
          <el-table-column prop="quantityTotal" label="生产总量" align="center" sortable min-width="100" :show-overflow-tooltip="true">
            <template #default="scope">  
                <span>{{scope.row.quantityTotal}}{{scope.row.unitName}}</span>
              </template>
          </el-table-column>
          <el-table-column  align="center">
            <template #header="th">
              <span>周一 {{ monTitle }}</span>
            </template>
            <el-table-column prop="quantityDayOfMon" label="白班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/> 
            <el-table-column prop="quantityNightOfMon" label="晚班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/> 
          </el-table-column>
          <el-table-column align="center">
            <template #header="th">
              <span>周二 {{ tusTitle }}</span>
            </template>
            <el-table-column prop="quantityDayOfTues" label="白班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/> 
            <el-table-column prop="quantityNightOfTues" label="晚班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/>
          </el-table-column>
          <el-table-column align="center">
            <template #header="th">
              <span>周三 {{ wedTitle }}</span>
            </template>
            <el-table-column prop="quantityDayOfWed" label="白班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/>
            <el-table-column prop="quantityNightOfWed" label="晚班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/> 
          </el-table-column>
          <el-table-column align="center">
            <template #header="th">
              <span>周四 {{ thurTitle }}</span>
            </template>
            <el-table-column prop="quantityDayOfThur" label="白班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/> 
            <el-table-column prop="quantityNightOfThur" label="晚班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/>
          </el-table-column>
          <el-table-column align="center">
            <template #header="th">
              <span>周五 {{ friTitle }}</span>
            </template>
            <el-table-column prop="quantityDayOfFri" label="白班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/>
            <el-table-column prop="quantityNightOfFri" label="晚班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/>
          </el-table-column>
          <el-table-column align="center">
            <template #header="th">
              <span>周六 {{ satTitle }}</span>
            </template>
            <el-table-column prop="quantityDayOfSat" label="白班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/>
            <el-table-column prop="quantityNightOfSat" label="晚班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/> 
          </el-table-column>
          <el-table-column align="center">
            <template #header="th">
              <span>周日 {{ sunTitle }}</span>
            </template>
            <el-table-column prop="quantityDayOfSun" label="白班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/> 
            <el-table-column prop="quantityNightOfSun" label="晚班" align="center" sortable min-width="80" :show-overflow-tooltip="true"/>
          </el-table-column> 
        </el-table>
    </Layer>
  </template>
  
  <script lang="ts" setup>
  import { onMounted,defineProps,ref,} from 'vue';
  import { ElForm } from 'element-plus'; 
  import Layer from '@/components/layer/index.vue' 
  import commonHelper from "@/utils/system/common-helper";

  const props=defineProps({
    layer: {
        type: Object,
        default: () => {
          return {
            show: false,
            title:'',
            width:"40%",
            showButton: true,
            btnLoading:false, 
            data:null,
            selected:null
          }
        }
      }
  });  
  const monTitle= ref(); 
  const tusTitle= ref(); 
  const wedTitle= ref(); 
  const thurTitle=ref(); 
  const friTitle= ref(); 
  const satTitle= ref(); 
  const sunTitle= ref();     
  onMounted(()=>{  
    for(let i=0;i<props.layer.data.length;i++){
      let row=props.layer.data[i];
      if(row.dateOfMon){
        monTitle.value=row.dateOfMon; 
      }
      if(row.dateOfTues){
        tusTitle.value=row.dateOfTues; 
      }
      if(row.dateOfWed){
        wedTitle.value=row.dateOfWed; 
      }
      if(row.dateOfThur){
        thurTitle.value=row.dateOfThur; 
      }
      if(row.dateOfFri){
        friTitle.value=row.dateOfFri; 
      }
      if(row.dateOfSat){
        satTitle.value=row.dateOfSat; 
      }
      if(row.dateOfSun){
        sunTitle.value=row.dateOfSun;  
      }
    }
  }); 
  </script>
  
  <style lang="scss" scoped>  
  </style>