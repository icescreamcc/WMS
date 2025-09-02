<template>
  <div class="category">
    <div class="header-box">
     <el-row style="width:100%">
       <el-col :span="12" style="text-align:left">
          <h2>表字段列表</h2>
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
        @node-click="handleNodeClick"
      > 
      </el-tree>
    </div>
  </div> 
</template>

<script lang="ts">
import { LayerInterface } from "@/components/layer/index.vue"; 
import { defineComponent, ref, reactive,inject, nextTick, Ref, watch } from "vue"; 
import{getTableFieldList} from '@/api/system/fieldsManage'
//import Layer from './layer.vue'  
import emitter from '@/utils/system/eventBus'
export default defineComponent({
  components:{
     // Layer,
  },
  setup(props,ctx) {   
    let treeData = ref([]);
      // 弹窗控制器
    const layer: LayerInterface = reactive({
      show: false,
      title: '新增',
      showButton: true,
      btnLoading:false,
      width:"40%",
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
      getTableFieldList().then((res:any)=>{ 
        treeData.value  =res.data?.map((d:any)=>{
          return {
            id:d.id,
            label:d.label+'【'+d.id+'】' ,
            type:'table',
            children:d.children.map((c:any)=>{
              return{
                id:c.id,
                label:c.type=='SpareField'? c.label+'【'+c.remark+'】●' :c.label+'【'+c.remark+'】',
                parentId:d.id,
                type:'field',
                remark:c.type
              }
            })
          }
        });  
        if(selectId){  
         active.value= findNode(selectId, res.data)  
        }
        else{
            active.value = treeData.value[0]; 
        } 
        nextTick(() => {
          tree.value && tree.value.setCurrentKey(selectId?selectId:active.value.id) 
        }) 
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
 
    // 新增弹窗功能
    const handleAdd = () => {
      layer.title = '新增表字段管理'
      layer.showButton=true;
      layer.show = true 
      layer.row=active.value;
      layer.data=treeData.value  
 
    } 

    //点击节点
    const handleNodeClick = (row: any,nodePorps:any,e:any) => {
       active.value = row; 
    };
    getTreeData();
    emitter.on("sparefield_reloadTree",(nodeId)=>{  
      getTreeData(nodeId)
    })

    //添加编辑提交数据
    const handleSubmit=(data:any)=>{
      // layer.btnLoading=true;
      // data.rank=data.rank?data.rank:1
      // addMenu(data).then(res=>{
      //   getTreeData(data.menuId);
      //   layer.show = false;
      // }).finally(()=>layer.btnLoading=false)
    }
 
    return {
      treeData,
      tree, 
      defaultProps,
      handleNodeClick,  
      handleSubmit
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