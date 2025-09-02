<template>
    <div>
       <el-drawer v-model="props.options.show" :with-header="false" :show-close="true" size="33%">
       <div class="content">
         <div class="content-header text-success">
           <h4><el-icon style="position: relative; top: 2px;font-size: medium;"><InfoFilled /></el-icon>&nbsp;{{ title }}</h4>
         </div>
          <div class="content-body" :style="{height:drawerHeight+'px'}">
            <div class="body-title"> 
              <el-steps :active="4" finish-status="success"  simple style="padding: 3px 3px;">
              <el-step v-if="curSelectElement.warehouseName">
                <template #title>
                  <span style="font-size: xx-small;">{{ curSelectElement.warehouseName }}</span> 
                </template> 
              </el-step>
              <el-step v-if="curSelectElement.shelfName" :title="curSelectElement.shelfName">
                <template #title>
                  <span style="font-size: xx-small;">{{ curSelectElement.shelfName }}</span>
                </template>
              </el-step>
              <el-step v-if="curSelectElement.binName" :title="curSelectElement.binName">
                <template #title>
                  <span style="font-size: xx-small;">{{ curSelectElement.binName }}</span>
                </template>
              </el-step>
              <el-step v-if="curSelectElement.workbinCellNo" :title="curSelectElement.workbinCellNo">
                <template #title>
                  <span style="font-size: xx-small;">{{ curSelectElement.workbinCellNo }}</span>
                </template>
              </el-step>
            </el-steps>
            </div> 
            <div v-if="((curShowType=='Shelf'||curShowType=='Bin') && warehouseElementData.length>0) ||warehouseElementWorkbinCell.length==0"> 
              <div class="layout-bin layout-item-shelf" :class="setElementClass(item)" :style="{width:warehouseShelfElementWidth+'%'}" v-for="item in warehouseElementData.filter((item:any)=>item.type=='Shelf')" @click="onSelectElement(item)">
                    <div class="el-icon-place" style="position: absolute;left: 2%; top: 6%;color: #fff;" v-if="item.selected"></div>
                    <img src="/public/icon-img/tuijian_1.png" style="height:20px;position: absolute;right: 0; top: 0;" v-if="item.isRecommend">
                    <P class="bin-title" style="font-size: small;">{{ item.name }}</P>  
                    <div class="div-shelf">
                    <div class="shelf-item">
                        <div class="shelf-item-sub"></div> 
                    </div>
                    <div class="shelf-item">
                        <div class="shelf-item-sub"></div> 
                    </div>
                    <div class="shelf-item">
                        <div class="shelf-item-sub"></div> 
                    </div>
                    <div class="shelf-item-last">
                        <div class="shelf-item-sub"></div> 
                    </div>
                    </div>  
                </div>
                <div> 
                    <div class="layout-bin bin-cls" :class="setElementClass(item)" :style="{width:warehouseBinElementWidth+'%'}" v-for="item in warehouseElementData.filter((item:any)=>item.type=='Bin')" @click="onSelectElement(item)">
                        <div class="el-icon-place" style="position: absolute;left: 2%; top: 6%;color: #fff;" v-if="item.selected"></div>
                        <img src="/public/icon-img/tuijian_1.png" style="height:30px;position: absolute;right: 0; top: 0;" v-if="item.isRecommend">
                        <el-popover placement="left-start" title="存货" :width="430" trigger="hover">
                          <template #reference> 
                            <P class="bin-title">{{ item.name }} <span style="font-size: 12px;" v-if="item.specificationName">({{ item.specificationName }})</span></P>  
                          </template>
                          <el-table :data="item.stockInfo" max-height="250"  v-if="item.stockInfo?.length>0" border size="small" stripe>
                            <el-table-column  prop="goodsName" width="105" label="品名" :show-overflow-tooltip="true"/>
                            <el-table-column  prop="goodsModel" width="105" label="型号" :show-overflow-tooltip="true"/>
                            <el-table-column  prop="goodsNo" width="105" label="编码" :show-overflow-tooltip="true"/> 
                            <el-table-column  label="现存"  width="105" :show-overflow-tooltip="true">
                              <template #default="scope">
                                <span  v-for="st in scope.row.stockArr">{{st.stock+st.unitName}}</span>
                              </template>
                            </el-table-column> 
                          </el-table> 
                          <div v-else style="text-align: center;">
                            <img src="/public/icon-img/sorry.png" style="height:30px;">
                            <p style="font-size: 12px;color: #888;">此处没有查到任何库存</p>
                          </div>
                        </el-popover>  
                        <P class="bin-status" v-if="item.isTakeStockLock"><span class="text-warning">盘点中</span></P>
                    </div>
                </div>  
            </div>
            <div v-else-if="curShowType=='Workbin'&& warehouseElementWorkbinCell.length>0"> 
                <el-row>
                  <el-col :span="20" :offset="2"> 
                      <div style="width: 444px;height:284px ;margin-top: 20px; border-radius: 4px;background-color: #1e3055;position: relative;left: 50%;transform: translateX(-50%);">
                      <el-row style="width: 440px;position: relative;top: 2px;left: 2px;">
                        <el-col v-if="warehouseElementWorkbinCell.length==1" v-for="opt in warehouseElementWorkbinCell" 
                          class="workbin-cell" :span="24" @click="onSelectElement(opt)"
                          :style="{height: '280px',lineHeight:'280px'}"
                          :class="opt.selected?'workbin-cell-selected':''" :title="opt.no"> 
                          <el-popover placement="left-start" title="存货" :width="430" trigger="hover">
                          <template #reference> 
                            <div class="cell-title">{{ opt.no }}</div>  
                          </template>
                          <el-table :data="opt.stockInfo" max-height="250" v-if="opt.stockInfo?.length>0"  border size="small" stripe>
                            <el-table-column  prop="goodsName" width="105" label="品名" :show-overflow-tooltip="true"/>
                            <el-table-column  prop="goodsModel" width="105" label="型号" :show-overflow-tooltip="true"/>
                            <el-table-column  prop="goodsNo" width="105" label="编码" :show-overflow-tooltip="true"/> 
                            <el-table-column  label="现存"  width="105" :show-overflow-tooltip="true">
                              <template #default="scope">
                                <span  v-for="st in scope.row.stockArr">{{st.stock+st.unitName}}</span>
                              </template>
                            </el-table-column> 
                          </el-table> 
                          <div v-else style="text-align: center;">
                            <img src="/public/icon-img/sorry.png" style="height:30px;">
                            <p style="font-size: 12px;color: #888;">此处没有查到任何库存</p>
                          </div>
                        </el-popover> 
                        </el-col> 
                        <el-col v-else-if="warehouseElementWorkbinCell.length==2" v-for="opt in warehouseElementWorkbinCell" 
                          class="workbin-cell" :span="12" @click="onSelectElement(opt)"
                          :style="{height: '280px',lineHeight:'280px'}"
                          :class="opt.selected?'workbin-cell-selected':''" :title="opt.no">
                          <el-popover placement="left-start" title="存货" :width="430" trigger="hover">
                            <template #reference> 
                              <div class="cell-title">{{ opt.no }}</div>  
                            </template>
                            <el-table :data="opt.stockInfo" max-height="250" v-if="opt.stockInfo?.length>0"  border size="small" stripe>
                              <el-table-column  prop="goodsName" width="105" label="品名" :show-overflow-tooltip="true"/>
                              <el-table-column  prop="goodsModel" width="105" label="型号" :show-overflow-tooltip="true"/>
                              <el-table-column  prop="goodsNo" width="105" label="编码" :show-overflow-tooltip="true"/> 
                              <el-table-column  label="现存"  width="105" :show-overflow-tooltip="true">
                                <template #default="scope">
                                  <span  v-for="st in scope.row.stockArr">{{st.stock+st.unitName}}</span>
                                </template>
                              </el-table-column> 
                            </el-table> 
                            <div v-else style="text-align: center;">
                              <img src="/public/icon-img/sorry.png" style="height:30px;">
                              <p style="font-size: 12px;color: #888;">此处没有查到任何库存</p>
                            </div>
                          </el-popover> 
                        </el-col> 
                        <el-col v-else v-for="opt in warehouseElementWorkbinCell" 
                          class="workbin-cell" :span="24/(warehouseElementWorkbinCell.length/2)" @click="onSelectElement(opt)"
                          :style="{height: '140px',lineHeight:'140px'}"
                          :class="opt.selected?'workbin-cell-selected':''" :title="opt.no">
                          <el-popover placement="left-start" title="存货" :width="430" trigger="hover">
                              <template #reference> 
                                <div class="cell-title">{{ opt.no }}</div>  
                              </template>
                              <el-table :data="opt.stockInfo" max-height="250" v-if="opt.stockInfo?.length>0"  border size="small" stripe>
                                <el-table-column  prop="goodsName" width="105" label="品名" :show-overflow-tooltip="true"/>
                                <el-table-column  prop="goodsModel" width="105" label="型号" :show-overflow-tooltip="true"/>
                                <el-table-column  prop="goodsNo" width="105" label="编码" :show-overflow-tooltip="true"/> 
                                <el-table-column  label="现存"  width="105" :show-overflow-tooltip="true">
                                  <template #default="scope">
                                    <span  v-for="st in scope.row.stockArr">{{st.stock+st.unitName}}</span>
                                  </template>
                                </el-table-column> 
                              </el-table> 
                              <div v-else style="text-align: center;">
                                <img src="/public/icon-img/sorry.png" style="height:30px;">
                                <p style="font-size: 12px;color: #888;">此处没有查到任何库存</p>
                              </div>
                            </el-popover> 
                        </el-col> 
                      </el-row>  
                    </div> 
                  </el-col>
                </el-row>
             </div>
             <el-row v-else>
               <el-col><el-empty description="没有任何数据" /></el-col> 
             </el-row>
          </div>
          <div class="content-footer"> 
            <el-button  style="width: 90%;" @click="refresh"><el-icon style="position: relative;top:2px"><Refresh /></el-icon>刷新</el-button>
          </div>
       </div>
     </el-drawer>
    </div>
</template>

<script lang="ts" setup>
import {ref ,defineEmits,defineProps,onMounted,nextTick,onBeforeMount} from 'vue'; 
import{getElementByWarehouse,getShelfByWarehouse,getBinByShelf,getWorkbinCellsByBin,getStorageDetailsByBin,getStorageDetailsByShelf}from '@/api/common';  
import msg from '@/utils/system/message'
import { Refresh ,InfoFilled} from '@element-plus/icons-vue'; 

const props=defineProps({
  options: {
      type: Object,
      default: () => {
        return {
          show: false,
          title: '', 
          type:'',
          isMultiSelect:false,
          warehouse:null,  
          data:null,
          recommendData:null 
        }
      }
    }
});
const emit = defineEmits(['selectItem']);
const selectedElementName=ref(props.options.warehouse.warehouseName);
const title=ref(props.options.title);
const curShowType=ref(props.options.type);
const warehouseElementData=ref(new Array<any>());   
const warehouseElementWorkbinCell=ref(new Array<any>());
const warehouseShelfElementWidth=ref(10);
const warehouseBinElementWidth=ref(10);
const drawerHeight=ref(400);
const curSelectElement=ref({
  warehouseId:props.options.warehouse.warehouseId,
  warehouseNo:props.options.warehouse.warehouseNo,
  warehouseName:props.options.warehouse.warehouseName,
  shelfId:props.options.data?.shelfId,
  shelfNo:props.options.data?.shelfNo,
  shelfName:props.options.data?.shelfName,
  binId:props.options.data?.binId,
  binNo:props.options.data?.binNo,
  binName:props.options.data?.binName,
  workbinId:props.options.data?.workbinId,
  workbinNo:props.options.data?.workbinNo,
  workbinCellId:props.options.data?.workbinCellId,
  workbinCellNo:props.options.data?.workbinCellNo,
  type:props.options.data?.type
});

onBeforeMount(()=>{
  drawerHeight.value=window.innerHeight-150;
})

onMounted(()=>{  
    if(props.options.data.shelfNo||props.options.data.binNo||props.options.data.workbinCellNo){
       if(props.options.type=='Bin'){  
            getBinData(props.options.data.shelfId)
        }
        else if(props.options.type=='Workbin'){
          getWorkbinData(props.options.data.binId)
        } 
        else{
          getWarehouseElementData(props.options.warehouse.warehouseId);
        }
    }
   else{ 
    getWarehouseElementData(props.options.warehouse.warehouseId);
   }
    setSelectedElementNav();
})
 
const getWarehouseElementData=(warehouseId:string)=>{  
    getElementByWarehouse(warehouseId).then((res:any)=>{
    warehouseElementData.value=res.data;  
    let shelfData=res.data.filter((item:any)=>item.type=="Shelf"&&item.props);
    let binData=res.data.filter((item:any)=>item.type=="Bin"&&item.props); 
    if(shelfData&&shelfData.length>0){
        warehouseShelfElementWidth.value=getLayoutElementWidth(shelfData[0]?.props);
    }
    if(binData&&binData.length>0){
        warehouseBinElementWidth.value=getLayoutElementWidth(binData[0]?.props); 
    }  
    if(props.options.data.shelfId){
       let curShelf= warehouseElementData.value.find(f=>f.id==props.options.data.shelfId&&f.type=='Shelf');
       if(curShelf){
        curShelf.selected=true;
       }
      }
    if(props.options.recommendData){
      warehouseElementData.value.forEach(w=>{
        props.options.recommendData.forEach((r:any)=>{
          if(w.id==r.shelfId){
            w.isRecommend=true;
          }
        })
      })
    } 
  }); 
}

const refresh=()=>{
  curShowType.value='Shelf';
  title.value="选择入库货架";
  warehouseElementWorkbinCell.value.length=0;
  initCurSelectElement();
  setSelectedElementNav();
  getWarehouseElementData(props.options.warehouse.warehouseId); 
}

const setElementClass=(item:any)=>{ 
    let className="";
    if(item.isAbandon){
      className= "bg-deft"
    }
    else{
      if(item.type=='Shelf'){
        className= "bg-primary-dark"
      } 
      else{
        if(item.status=="Free"){
          className= "bg-success-dark"
        }
        else if(item.status=="Full"){
          className= "bg-warning"
        }
        else{
          className= "bg-danger"
        }
      } 
    }
    if(item.selected){
      className= className+" layout-item-select";
    } 
    return className;
  }

var getLayoutElementWidth=(props:string)=>{
    if(props&&props.indexOf('*')>-1){
      let col= Number(props.split('*')[0]);
      return  100/col-3.2;
    }
    return 100/6-3.2;
   }

var initCurSelectElement=()=>{
  curSelectElement.value.warehouseId=props.options.warehouse.warehouseId;
  curSelectElement.value.warehouseNo=props.options.warehouse.warehouseNo;
  curSelectElement.value.warehouseName=props.options.warehouse.warehouseName;
  curSelectElement.value.shelfId='';
  curSelectElement.value.shelfNo='';
  curSelectElement.value.shelfName='';
  curSelectElement.value.binId=0;
  curSelectElement.value.binNo='';
  curSelectElement.value.binName='';
  curSelectElement.value.workbinId=0;
  curSelectElement.value.workbinNo='';
  curSelectElement.value.workbinCellId=0;
  curSelectElement.value.workbinCellNo='';
  curSelectElement.value.type='Shelf'; 
}

  //点击元素
const onSelectElement=(item:any)=>{     
  curSelectElement.value.type=item.type; 
  if(item.type=="Shelf"){ 
    if(item.isAbandon){
        msg.warningAuto('该货架已被弃用，请选择其他库位')
        return;
    }
    title.value="选择入库货位";
    curSelectElement.value.shelfId=item.id;
    curSelectElement.value.shelfNo=item.no;
    curSelectElement.value.shelfName=item.name;  
    getBinData(item.id); 
  }
  else if(item.type=="Bin"){ 
    if(item.isAbandon){
        msg.warningAuto('该库位已被弃用，请选择其他库位')
        return;
    }  
    if(item.isTakeStockLock){
      msg.warningAuto('该库位正在盘点中，请选择其他库位')
        return;
    }
    setSelectElement(warehouseElementData.value,item);
    curSelectElement.value.binId=item.id;
    curSelectElement.value.binNo=item.no;
    curSelectElement.value.binName=item.name; 
    if(item.hasWorkbin){
      curShowType.value='Workbin';
      title.value="选择入库料箱";
      getWorkbinData(item.id);
    }
    else{
      emit('selectItem',curSelectElement.value);
    } 
  }
  else if(item.type=="WorkbinCell"){
    setSelectElement(warehouseElementWorkbinCell.value,item); 
    curSelectElement.value.workbinId=item.parentId;
    curSelectElement.value.workbinNo=item.parentNo;
    curSelectElement.value.workbinCellId=item.id;
    curSelectElement.value.workbinCellNo=item.no;
    emit('selectItem',curSelectElement.value);
  }
  setSelectedElementNav();
} 

var getBinData=(shelfId:any)=>{
    getBinByShelf(shelfId).then((res:any)=>{  
      warehouseElementData.value=res.data;
      if(warehouseElementData.value?.length>0){
        getStorageDetailsByShelf(shelfId).then(storage=>{
          if(storage.data?.length>0){
            warehouseElementData.value.forEach(bin=>{
              let stockInfo=storage.data.filter((f:any)=>f.binId==bin.id); 
              if(stockInfo?.length>0){
                let goodsArr=new Array<any>();
                stockInfo.forEach((stock:any) => {
                  let exists=goodsArr.filter(f=>f.goodsId==stock.goodsId);
                  if(exists?.length==0){
                    goodsArr.push({
                      goodsId:stock.goodsId,
                      goodsName:stock.goodsName,
                      goodsNo:stock.goodsNo,
                      goodsModel:stock.goodsModel
                    })
                  }
                });
                goodsArr.forEach(g=>{
                  g.stockArr=stockInfo.filter((f:any)=>f.goodsId==g.goodsId);
                });
                bin.stockInfo=goodsArr;
              }
            }) 
          }
        })
        warehouseBinElementWidth.value=getLayoutElementWidth(res.data[0].props); 
        if(props.options.data.binId&&props.options.data.binName){ 
        let curBin= warehouseElementData.value.find(f=>f.id==props.options.data.binId&&f.type=='Bin');
          if(curBin){
            curBin.selected=true;
          } 
        }
        if(props.options.recommendData){
          warehouseElementData.value.forEach(w=>{
            props.options.recommendData.forEach((r:any)=>{
              if(w.id==r.binId){
                w.isRecommend=true;
              }
            })
          })
        } 
      } 
      console.log('warehouseElementData.value',warehouseElementData.value)
    });
}

var getWorkbinData=(binId:number)=>{
  getWorkbinCellsByBin(binId).then(res=>{ 
      warehouseElementWorkbinCell.value=res.data;
      if(res.data?.length>0){
        getStorageDetailsByBin(binId).then(storage=>{
          if(storage.data?.length>0){
            warehouseElementWorkbinCell.value.forEach(cell=>{
              let stockInfo=storage.data.filter((f:any)=>f.cellId==cell.id); 
              if(stockInfo?.length>0){
                let goodsArr=new Array<any>();
                stockInfo.forEach((stock:any) => {
                  let exists=goodsArr.filter(f=>f.goodsId==stock.goodsId);
                  if(exists?.length==0){
                    goodsArr.push({
                      goodsId:stock.goodsId,
                      goodsName:stock.goodsName,
                      goodsModel:stock.goodsModel
                    })
                  }
                });
                goodsArr.forEach(g=>{
                  g.stockArr=stockInfo.filter((f:any)=>f.goodsId==g.goodsId);
                });
                cell.stockInfo=goodsArr;
              }
            }) 
          }
        })
        if(props.options.data.workbinCellId&&props.options.data.workbinCellNo){ 
         let curWorkbin= warehouseElementWorkbinCell.value.find(f=>f.id==props.options.data.workbinCellId);
          if(curWorkbin){
            curWorkbin.selected=true;
          } 
        }
      }
    })  
}

var setSelectElement=(elementArr:Array<any>,selectItem:any)=>{
  elementArr.forEach((element:any) => {
    element.selected=false;
    }); 
    selectItem.selected=true;
} 

var setSelectedElementNav=()=>{ 
   if(curSelectElement.value.warehouseName){
    selectedElementName.value=curSelectElement.value.warehouseName;
   }
   if(curSelectElement.value.shelfName){
    selectedElementName.value+=" / "+curSelectElement.value.shelfName;
   }
   if(curSelectElement.value.binName){
    selectedElementName.value+=" / "+curSelectElement.value.binName;
   }
   if(curSelectElement.value.workbinCellNo){
    selectedElementName.value+=" / "+curSelectElement.value.workbinCellNo;
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
    .content-body{ 
      height: 49%;
      padding: 10px;
      overflow-y: scroll;
      text-align: left;
      .body-title{   
        margin-top: -10px;
        margin-bottom: 10px;
      }
      .layout-bin{
            height: 85px;
            width: 14.45%;
            margin-top: 1%;
            margin-left: 1%;
            margin-right: 1%;  
            display: inline-block; 
            vertical-align: middle; 
            text-align: center; 
            position: relative; 
            cursor: pointer;
            .bin-title{
              margin-top: 20%;
              font-weight: 600; 
            }
            .bin-status{  
              font-size: 11px;
              font-weight:600;
            } 
         }
        .bin-cls{
          border-radius: 3px; 
          box-shadow:1px 1px 2px #012a10;
        }
        .layout-item-shelf{
        box-shadow: 1.5px 1.5px #818080;
        }
        .div-shelf{
        position: absolute;bottom: 0;height: 30px;width: 100%;border-top:1px dashed #fff;text-align: left;
        .shelf-item{
            border-right:1px dashed #fff;height: 30px;width: 24.3%;display: inline-block; 
        }
        .shelf-item-last{
            height: 20px;width: 23.5%;display: inline-block;
        }
        .shelf-item-sub{
            border-bottom:1px dashed #fff;height: 15px;width: 100%;display: inline-block;
            }
        }
        .stock-info-group{
          margin-top: -10px;
          font-size: 12px;
          transform: scale(.9);
          height: 70%;
          overflow: hidden;
          .stock-info{  
            width: 100%;
            height: 18px;    
            overflow: hidden;
            white-space: nowrap;
          }
        } 
        .icon-select{
            position: absolute;
            left: 2%;
            top: 6%;
        }
        .workbin-cell{
              background-color: #717c91;
              border-right: 1px solid #fff;
              border-bottom: 1px solid #fff;
              color: #fff; 
              font-size: xx-small;
              text-align: center;
              cursor: pointer; 
              .workbin-cell-desc{   
                position: relative;
                .cell-title{
                  position: absolute;
                  left:50%;
                  top: -20%;
                  transform: translateX(-50%);
                  height: 18px;   
                  font-weight: 600;
                  font-size: 14px;
                }
                .cell-info-group{ 
                  position: relative; 
                    left: 50%;
                    transform: translateX(-50%);
                  .cell-info{    
                    position: relative; 
                    top: 50%;
                    height: 28px; 
                    width: 100%;   
                  }
                } 
              } 
            }
        .workbin-cell-selected{
          background-color: #ddddde;
          color: #022e5b;
        }
    }
    .content-footer{ 
       width: 100%;
       position: absolute;
       bottom: 3%;
    }
   
}
</style>