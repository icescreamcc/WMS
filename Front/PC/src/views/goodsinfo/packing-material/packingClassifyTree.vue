<template>
  <div class="category">
    <div class="header-box">
     <el-row style="width:100%">
       <el-col :span="12" style="text-align:left">
          <h2>包材分类</h2>
       </el-col>
        <el-col :span="12"  style="text-align:right"> 
           <el-tooltip v-if="permission.isPermisstion('PACKINGMATERIALTYPEADD')"  content="新增" placement="bottom" effect="light">
               <el-button type="primary" icon="el-icon-circle-plus-outline"  @click="handleAdd" ></el-button>
          </el-tooltip>
             <el-tooltip v-if="permission.isPermisstion('PACKINGMATERIALTYPEUPDATE')" content="编辑" placement="left" effect="light">
               <el-button type="warning" icon="el-icon-edit" :disabled="editDisabled" @click="handleUpdate" ></el-button>
          </el-tooltip>
         <el-popconfirm v-if="permission.isPermisstion('PACKINGMATERIALTYPEDEL')" :title="delTitie"  @confirm="handleDel()">
          <template #reference>
             <el-button type="danger" title="删除" icon="el-icon-delete" ></el-button>
          </template>
        </el-popconfirm> 
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
        icon="el-icon-edit"
        highlight-current
        default-expand-all
        @node-click="handleNodeClick">  
        </el-tree>
    </div>
  </div>
   <PackingLayer :layer="layer" v-if="layer.show" @submit="dataSave" />
</template>

<script lang="ts" setup> 
import { LayerInterface } from "@/components/layer/index.vue"; 
import {  ref, reactive,inject, nextTick, Ref,onMounted } from "vue"; 
import {getPackingMaterialClassifyData,addPackingMaterialClassify,updatePackingMaterialClassify,delPackingMaterialClassify} from '@/api/baseinfo/packingMaterial'  
import permission from '@/utils/system/permission'
import PackingLayer from './packingClassifyEditLayer.vue'  

const editDisabled=ref(true);   
const treeData = ref(new Array<any>()); 
  const delLoading=ref(false);
const delTitie=ref('');
const layer: LayerInterface = reactive({
      show: false,
      title: '新增',
      showButton: true,
      btnLoading:false,
      width:"30%",
      data:null,
      otherButton:{ }
    })

onMounted(() => {
  getTreeData();
}) 
    
const tree: Ref<any|null> = ref(null)
const defaultProps = {
  children: "children",
  label: "label",
};
const active: any = inject("active");
const getTreeData = (selectId:any=null) => {  
  getPackingMaterialClassifyData().then((res:any)=>{  
      treeData.value =res.data;  
      if(treeData.value.length>0){
        if(selectId){ 
        active.value= findNode(selectId, res.data) 
        }
        else{
            active.value = treeData.value[0]; 
        } 
        nextTick(() => {
          tree.value && tree.value.setCurrentKey(selectId?selectId:active.value.id) 
        }) 
      } 
  }) 
}; 

const findNode=(nodeId:string,data:any)=>{
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

// 新增弹窗功能
const handleAdd = () => {
    layer.title = '新增类型'
    layer.row=active.value; 
    layer.show = true
    layer.data=treeData
    layer.type='add'
  }

  //编辑弹窗功能
const handleUpdate = () => {
  layer.title = '编辑类型'  
  layer.row=active.value; 
  layer.data=treeData
  layer.show = true  
  layer.type='update'
}

//新增或编辑数据提交
const dataSave=(data:any,actionType:string)=>{ 
  layer.btnLoading=true;
    if(actionType=='add'){
      addPackingMaterialClassify(data).then(res=>{
        layer.show = false;
        getTreeData();
      }).finally(()=> layer.btnLoading=false);
    }
    else{
        updatePackingMaterialClassify(data).then(res=>{
        layer.show = false;
        getTreeData(data.typeId);
      }).finally(()=> layer.btnLoading=false);
    }
}



  //删除数据
  const handleDel=()=>{ 
    delLoading.value=true; 
    delPackingMaterialClassify(active.value.id)
      .then(()=>{ getTreeData(); })
      .finally(()=>{delLoading.value=false})
  }

const handleNodeClick = (row: any,nodePorps:any,e:any) => {
    active.value = row;
    if(row.id!='ROOT'){
      editDisabled.value=false  
    }
    else{
        editDisabled.value=true 
    } 
};
 
  
</script>

<style lang="scss" scoped>
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
</style>