<template>
    <div class="full">
      <div class="left">
        <div class="category">
            <div class="header-box">
                <el-row style="width:100%">
                <el-col :span="12" style="text-align:left">
                    <h2>货架&货位</h2>
                </el-col> 
                </el-row>   
            
            </div>
            <div class="list system-scrollbar">
            <el-tree
                ref="tree"
                class="my-tree"
                :data="treeData"
                :props="defaultProps"
                :expand-on-click-node="false" 
                node-key="id"
                highlight-current
                default-expand-all
                @node-click="handleNodeClick"
            > 
            </el-tree>
            </div>
        </div> 
      </div>
      <div class="right">
        <div class="layout-container">
          <div class="layout-container-form flex space-between">
            <div class="layout-container-form-handle">
                <h2>{{ rightTitle }}布局图</h2>
            </div> 
          </div>
          <div class="layout-container-table" style="margin-top:-10px">
           <br>
          <div class="layout-content" v-if="nodeType=='Warehouse'">
            <div class="layout-title">{{ selectNode.label }}</div>
           <div>
            <div class="layout-bin layout-item-shelf" :class="setElementClass(item)" :style="{width:warehouseElementWidth_Shelf+'%'}" v-for="item in warehouseElementShelf" @click="onSelectElement(item)">
              <div class="el-icon-place" style="position: absolute;left: 2%; top: 6%;" v-if="item.isSelected"></div>
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
           </div> 
            <div>
              <div class="layout-bin" :class="setElementClass(item)" :style="{width:warehouseElementWidth_Bin+'%'}" v-for="item in warehouseElementBin" @click="onSelectElement(item)">
                <div class="el-icon-place" style="position: absolute;left: 2%; top: 6%;" v-if="item.isSelected"></div>
                <P class="bin-title">{{ item.name }}</P>  
                <P class="bin-info">{{ item.rank }}</P>
                <P class="bin-status">{{ item.status }}</P>
            </div>
            </div> 
          </div>
          <div class="layout-content" v-if="nodeType=='Shelf'">
            <div class="layout-title">{{ selectNode.label }}</div>
            <div class="layout-bin" :class="setElementClass(item)" :style="{width:warehouseElementWidth_Bin+'%'}" v-for="item in warehouseElementBin" @click="onSelectElement(item)">
                <div class="el-icon-place" style="position: absolute;left: 2%; top: 6%;" v-if="item.isSelected"></div>  
                <P class="bin-title">{{ item.name }}</P> 
                <P class="bin-info">{{ item.rank }}</P>
                <P class="bin-status">{{ item.status }}</P>
            </div> 
          </div>
          <div class="layout-content" v-if="nodeType=='Bin'&& warehouseElementWorkbinCell.length>0">
            <div class="layout-title">{{ selectNode.label }}</div>
            <el-row>
              <el-col :span="8" :offset="8" style="text-align: center;">
                <div style=" border:1px solid #d5d5d5;margin-top: 20%;">
                  <span style="font-size: xx-small;color: #888;" v-if="warehouseElementWorkbinCell[0].props">{{ warehouseElementWorkbinCell[0].props.split('*')[0] +'cm'}}</span>
                  <span style="float: right;position: relative;top:100px;font-size: xx-small;color: #888;" v-if="warehouseElementWorkbinCell[0].props">{{ warehouseElementWorkbinCell[0].props.split('*')[1] +'cm'}}</span>
                  <div style="width: 364px;height:184px ;margin-top: 20px; border-radius: 4px;background-color: #1e3055;">
                  <el-row style="width: 360px;position: relative;top: 2px;left: 2px;">
                    <el-col v-if="warehouseElementWorkbinCell.length==1" v-for="opt in warehouseElementWorkbinCell" 
                      class="workbin-cell" :span="24"
                      :style="{height: '180px',lineHeight:'180px'}" :title="opt.no">
                    <span>{{ opt.no }}</span>
                    </el-col> 
                    <el-col v-else-if="warehouseElementWorkbinCell.length==2" v-for="opt in warehouseElementWorkbinCell" 
                      class="workbin-cell" :span="12"
                      :style="{height: '180px',lineHeight:'180px'}" :title="opt.no">
                    <span>{{ opt.no }}</span>
                    </el-col> 
                    <el-col v-else v-for="opt in warehouseElementWorkbinCell" 
                      class="workbin-cell" :span="24/(warehouseElementWorkbinCell.length/2)"
                      :style="{height: '90px',lineHeight:'90px'}" :title="opt.no">
                      <span>{{ opt.no }}</span>
                    </el-col> 
                  </el-row>  
                </div>
                </div>
              </el-col>
            </el-row>
          </div>
          </div>
        </div>
      </div> 
      <EditLayer :layer="layer" v-if="layer.show" @onUnlock="onUnlockSubmit"/>
    </div> 
  </template>
  
  <script lang="ts" setup>
  defineOptions({
    name: "layout"
  })
  import { ref, nextTick,reactive ,onMounted} from 'vue'   
  import{ getWarehouseTree,getElementByWarehouse,getElementByShelf,getWorkbinCells,getStockInfoByBin,unlockBin} from '@/api/inv/layout'
  import { LayerInterface } from "@/components/layer/index.vue"; 
  import EditLayer from './editLayer.vue' ;
  import permission from '@/utils/system/permission'; 
  import msg from '@/utils/system/message'

  const layer: LayerInterface = reactive({
      show: false,
      title: '新增',
      showButton: false,
      btnLoading:false,
      width:"30%",
      data:{},
      type:'',
      otherButton:{ }
    })
  const treeData = ref();  
  const selectNode=ref();
  const nodeType=ref();
  const rightTitle=ref(''); 
  const warehouseElementShelf = ref(new Array<any>());  
  const warehouseElementBin = ref(new Array<any>());   
  const warehouseElementWorkbinCell=ref(new Array<any>());
  const warehouseElementWidth_Shelf=ref(10); 
  const warehouseElementWidth_Bin=ref(10); 
  const tree= ref()
  const defaultProps = {
   children: "children",
   label: "label"
  };  
 const getTreeData = (selectId:any=null) => {  
    getWarehouseTree().then((res:any)=>{  
      treeData.value =res.data;   
     if(selectId){
      nextTick(() => {
       tree.value && tree.value.setCurrentKey(selectId);
     });
     let curNode= findNode(selectId,res.data);
     showSelectNode(curNode);
     } 
   }) 
 }; 

 let findNode=(nodeId:string,data:any)=>{
      let node:any;
      for(let m of data){
        if(m.id==nodeId){
          node= m; 
        }
        if(!node){
           if(m.children?.length>0){
          node=  findNode(nodeId,m.children)
         } 
        }
       else{
         break
       }
      }
      return node
    }

    onMounted(()=>{
      getTreeData(); 
    })

   
 //点击节点
 const handleNodeClick = (row: any,nodePorps:any,e:any) => {  
  showSelectNode(row);
 }; 

 let showSelectNode=(row: any)=>{
    nodeType.value=row.type;
    selectNode.value=row;
    if(row.type=="Warehouse"){
      rightTitle.value="仓库-货架/货位";  
      getElementByWarehouse(row.id).then(res=>{
        warehouseElementShelf.value=res.data.filter((item:any)=>item.type=="Shelf");
        warehouseElementBin.value=res.data.filter((item:any)=>item.type=="Bin");
       if(warehouseElementShelf.value.length>0){
        let shelfProps=warehouseElementShelf.value.filter((item:any)=>item.props)[0];  
        warehouseElementWidth_Shelf.value=getLayoutElementWidth(shelfProps?.props); 
       }
       if(warehouseElementBin.value.length>0){
        let binProps=warehouseElementBin.value.filter((item:any)=>item.props)[0];   
        warehouseElementWidth_Bin.value=getLayoutElementWidth(binProps?.props); 
       } 
      })
    }
    else if(row.type=="Shelf"){
      rightTitle.value="货架-货位"; 
      getElementByShelf(row.id).then(res=>{
        warehouseElementBin.value=res.data; 
        if(res.data?.length>0){
          let propsItem=warehouseElementBin.value.filter((item:any)=>item.props)[0];  
          warehouseElementWidth_Bin.value=getLayoutElementWidth(propsItem?.props);  
        } 
      })
    }
    else if(row.type=="Bin"){
      rightTitle.value="货位-料箱"; 
      getWorkbinCells(row.id).then(res=>{
        if(res.data?.length>0){
          warehouseElementWorkbinCell.value=res.data;
        }
      })
    } 
 }

 let getLayoutElementWidth=(props:string)=>{ 
    if(props&&props.indexOf('*')>-1){
      let col= Number(props.split('*')[0]);
      return  100/col-2.2;
    }
    return 100/6-2.2;
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
    if(item.isSelected){
      className= className+" layout-item-select";
    } 
    return className;
  }

  //点击元素
const onSelectElement=(item:any)=>{ 
  warehouseElementShelf.value.forEach((element:any) => {
    element.isSelected=false;
  });
  warehouseElementBin.value.forEach((element:any) => {
    element.isSelected=false;
  });
  item.isSelected=true;
  console.log('onSelectElement',item)
  getTreeData(item.id); 
  if(item.type=="Shelf"){
   
  }
  else{ 
    if(item.status=="Full"||item.status=="Lock"){ 
      // getStockInfoByBin(item.id).then(res=>{
      //   layer.show=true;
      //   layer.title="库位存储信息"; 
      //   if(res.data){ 
      //     layer.data=res.data;
      //   }
      //   else{
      //     layer.data={
      //       binId:item.id,
      //       binNo:item.no,
      //       status:item.status,
      //     }
      //   }
      //   if(layer.data.status=='Lock'){
      //     layer.otherButton.show=true;
      //     layer.otherButton.type='primary';
      //     layer.otherButton.text='解除锁定';
      //   } 
      //   else{
      //     layer.otherButton.show=false;
      //   }
      // })
    }
  }  
}

const onUnlockSubmit=(binId:number,binNo:string)=>{
  layer.otherButton.otherBtnLoading=true;
  unlockBin(binId,permission.getOperator().userName).then(res=>{
    layer.show=false;
    msg.successAuto(`库位${binNo}已解除锁定`);
    getTreeData(binId);
  }).finally(()=>layer.otherButton.otherBtnLoading=false)
}

  </script>
  
  <style lang="scss" scoped>
    .full {
      width: 100%;
      height: 100%;
      box-sizing: border-box;
      padding: 15px;
      display: flex;
      .left {
        width: 350px;
        .category {
          background: #fff;
          width: 100%;
          height: 100%;
          display: flex;
          flex-direction: column;
          .header-box {
            padding: 10px;
            display: flex;
            align-items: center;
            border-bottom: 1px solid #eee;
            h2 {
              padding: 0;
              margin: 0;
              margin-right: 20px;
              font-size: 14px;
              display: -webkit-box;
              -webkit-line-clamp: 1;
              -webkit-box-orient: vertical;
              overflow: hidden;
              height: 30px;
              line-height: 30px;
            }
            .el-input {
              flex: 1;
            }
          }
          .list {
            flex: 1;
            overflow: auto;
          }
          .my-tree {
            :deep(.el-tree-node__content) {
              height: 36px;
            }
            :deep(.el-tree-node.is-current>.el-tree-node__content) {
              background-color: rgba(64, 158, 255, 0.4);
            }
            :deep(.el-tree-node>.el-tree-node__content) {
              transition: 0.2s;
            }
          }
        }
      }
    
      .right {
        flex: 1;
        width: calc(100% - 330px);
        height: 100%;
        .layout-container {
          height: 100%;
          margin: 0 0 0 10px;
          width: calc(100% - 10px);
          .layout-item-select{
            border: 1px solid rgb(0, 242, 255); 
          }
          .layout-item-shelf{
            box-shadow: 1.5px 1.5px #818080;
          }
          .div-shelf{
            position: absolute;bottom: 0;height: 30px;width: 100%;border-top:1px dashed #fff;text-align: left;
            .shelf-item{
              border-right:1px dashed #fff;height: 30px;width: 24.5%;display: inline-block; 
            }
            .shelf-item-last{
              height: 30px;width: 24%;display: inline-block;
            }
            .shelf-item-sub{
                border-bottom:1px dashed #fff;height: 10px;width: 100%;display: inline-block;
              }
          }
          h2 {
            padding: 0;
            margin: 0;
            margin-right: 20px;
            font-size: 14px;
            display: -webkit-box;
            -webkit-line-clamp: 1;
            -webkit-box-orient: vertical;
            overflow: hidden;
            height: 30px;
            line-height: 30px;
          } 
          .layout-content{
            height: 95%; 
            width: 100%;
            border:1px solid #c7c6c6;
            text-align: left;
            border-radius: 4px;
            position: relative;
            .layout-bin{
                height: 65px;
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
                    margin-top: 10px;
                    font-weight: 600;
                 }
                 .bin-status{
                    margin-top: -7px;font-size: x-small;
                 }
                 .bin-info{
                    margin-top: -15px;font-size: x-small; 
                 }
                 .bin-free{
                    margin-top: 7.5px;
                 }
            }
            .layout-title{
                font-weight: 600;
                color: #aeaeae;
                text-align: center; 
                margin-top: 10px;
            }
            .workbin-cell{
              background-color: #717c91;
              border-right: 1px solid #fff;
              border-bottom: 1px solid #fff;
              color: #fff; 
              font-size: xx-small;
              text-align: center;
            }
          } 
        }
      }
    }
  </style>