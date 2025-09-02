<template>
  <div class="category">
    <div class="header-box">
     <el-row style="width:100%">
       <el-col :span="12" style="text-align:left">
          <h2>组织架构</h2>
       </el-col>
        <el-col :span="12"  style="text-align:right">
          <el-button    v-if="permission.isPermisstion('ROLEADD','DEPTADD')" type="primary" title="新增部门或角色" icon="el-icon-circle-plus-outline"  @click="handleAdd" ></el-button>
          <el-button    v-if="permission.isPermisstion('ROLEUPDATE','DEPTUPDATE')" type="warning" title="编辑部门或角色" icon="el-icon-edit"  @click="handleUpdate" ></el-button>
          <el-popconfirm v-if="permission.isPermisstion('ROLEDEL','DEPTDEL')" :title="delTitie"  @confirm="handleDel()">
          <template #reference>
             <el-button type="danger" title="删除部门或角色" icon="el-icon-delete" :disabled="delDisabled" ></el-button>
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
        highlight-current
        default-expand-all
        @node-click="handleNodeClick"
      > 
      </el-tree>
    </div>
  </div>
   <Layer :layer="layer" v-if="layer.show" @submit="handleSubmit"/>
</template>

<script lang="ts">
import { LayerInterface } from "@/components/layer/index.vue"; 
import { defineComponent, ref, reactive,inject, nextTick, Ref, watch } from "vue"; 
import{getOrganizationData,addDept,updateDept,delDept,addRole,updateRole,delRole} from '@/api/system/organization'
import Layer from './layer.vue'  
import permission from '@/utils/system/permission'
export default defineComponent({
  components:{
      Layer,
  },
  setup(props,ctx) {  
    const delDisabled=ref(false);
    const delLoading=ref(false);
    const delTitie=ref('');
    let treeData = ref(new Array<any>());
      // 弹窗控制器
    const layer: LayerInterface = reactive({
      show: false,
      title: '新增',
      showButton: true,
      btnLoading:false,
      width:"30%",
      data:null,
      otherButton:{ }
    })

   //树形结构数据加载
    const tree: Ref<any|null> = ref(null)
    const defaultProps = {
      children: "children",
      label: "label"
    };
    const active: any = inject("active");
    const getTreeData = (selectId:any=null) => {  
      getOrganizationData().then((res:any)=>{ 
        let treeModel  = new Array<any>();
        treeModel.push(res.data)
         treeData.value =treeModel;  
       if(selectId){ 
         active.value= findNode(selectId, treeModel) 
        }
        else{
            active.value = treeData.value[0]; 
        } 
        nextTick(() => {
          tree.value && tree.value.setCurrentKey(selectId?selectId:active.value.id)
          delDisabled.value=true; 
        })
        selectedNode(active.value.id,active.value.type)
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

  //选择节点获取节点参数,organizationType:Company,Dept,Role
  let selectedNode=(searchId:string,organizationType:string)=>{
     ctx.emit("selectedNode",searchId,organizationType)
  }

    // 新增弹窗功能
    const handleAdd = () => {
      layer.title = '新增数据'
      layer.showButton=true;
      layer.show = true
      layer.type="Add"
      layer.row=active.value;
      layer.data=treeData.value  
    }

    //编辑弹窗功能
      const handleUpdate = () => {
      layer.title = '编辑数据'
      layer.showButton=true;
      layer.type="Update"
      layer.row=active.value;
      layer.data=treeData.value
      layer.show = true 
      if(layer.row.type=='Company'){
        layer.showButton=false;
      }
    }

    //点击节点
    const handleNodeClick = (row: any,nodePorps:any,e:any) => {
       active.value = row;
       delDisabled.value=false; 
       if(row.type=="Company"){
       delDisabled.value=true; 
       } 
       else if(row.type=="Dept"){
         if(row.children.length>0){
           delTitie.value="当前部门存在下级角色，是否确认一并删除";
         }
         else{
           delTitie.value="是否确认删除当前部门？"
         }
       }
       else{
         if(row.children.length>0){
           delTitie.value="当前角色存在下级角色，是否确认一并删除";
         }
         else{
           delTitie.value="是否确认删除当前角色？"
         }
       }
       selectedNode(row.id,row.type)
    };
    getTreeData();
    
    //添加编辑提交数据
    const handleSubmit=(organizationType:string,submitType:string,rootId:string,data:any)=>{
      layer.btnLoading=true;
      if(organizationType=="Dept"){
        let deptModel={
          DeptId:data.Id,
          DeptNo:data.No,
          DeptName:data.Name,
          Rank:data.Rank,
          CompanyId:rootId
          };
          if(submitType=="Add"){
            addDept(deptModel)
            .then((res:any)=>{
              getTreeData(res.data);
              layer.show=false;
              })
            .finally(()=>layer.btnLoading=false);
          }
          else{
            updateDept(deptModel)
             .then(()=>{
              getTreeData(deptModel.DeptId); 
              })
            .finally(()=>layer.btnLoading=false);
          } 
         }
         else{
           let roleModel={
             RoleId:data.Id,
             RoleNo:data.No,
             RoleName:data.Name,
             Rank:data.Rank,
             ParentId:data.ParentId,
             ParentType:data.ParentType,
             DeptId:data.ParentType=="Dept"?data.ParentId:null
             };
            if(submitType=="Add"){
              addRole(roleModel)
              .then((res:any)=>{
              getTreeData(res.data);
              layer.show=false;
              })
              .finally(()=>layer.btnLoading=false);
            }
            else{
              updateRole(roleModel)
              .then(()=>{
              getTreeData(roleModel.RoleId); 
              })
              .finally(()=>layer.btnLoading=false);
            } 
         }
        
    }

    //删除数据
    const handleDel=()=>{ 
      delLoading.value=true;
      if(active.value.type=="Dept"){
        delDept(active.value.id)
        .then(()=>{ getTreeData(); })
        .finally(()=>{delLoading.value=false})
      }
      else if(active.value.type=="Role"){
        delRole(active.value.id)
        .then(()=>{ getTreeData(); })
        .finally(()=>{delLoading.value=false})
      }
    }

    return {
      permission,
      treeData,
      tree,
      layer,
      defaultProps,
      handleNodeClick,
      delDisabled,
      delTitie,
      delLoading,
      handleAdd,
      handleUpdate,
      handleSubmit,
      handleDel
    };
  },
});
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