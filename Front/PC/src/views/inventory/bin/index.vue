<template>
    <div class="full">
      <div class="left">
        <div class="category">
            <div class="header-box">
                <el-row style="width:100%">
                <el-col :span="12" style="text-align:left">
                    <h2>货架&货位</h2>
                </el-col>
                    <el-col :span="12"  style="text-align:right"> 
                    <el-popconfirm v-if="(nodeType=='Warehouse'&&permission.isPermisstion('WAREHOUSEDEL','WAREHOUSEDEL'))||(nodeType=='Shelf'&&permission.isPermisstion('SHELFDEL','SHELFDEL'))||(nodeType=='Bin'&&permission.isPermisstion('BINDEL','BINDEL'))" :title="delTitie"  @confirm="onDelSubmit()">
                      <template #reference>
                          <el-button type="danger" title="删除货架或货位" icon="el-icon-delete" :disabled="btnDisabled" :loading="submitLoading"></el-button>
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
      </div>
      <div class="right">
        <div class="layout-container">
          <div class="layout-container-form flex space-between">
            <div class="layout-container-form-handle">
                <h2>{{ rightTitle }}信息编辑</h2>
            </div> 
          </div>
          <div class="layout-container-table" style="margin-top:-10px">
           <br>
            <el-form v-if="nodeType=='Warehouse'" :model="warehouseForm" :rules="warehouseRules" ref="warehouseFormRef" label-width="auto" label-position="left" style="padding:0 15px;position:relative;left:2%">
                <el-row>
                  <el-col :span="12" :offset="5">
                      <el-form-item label="仓库编码" prop="warehouseNo" style="margin-left:20px">
                        <el-input v-model="warehouseForm.warehouseNo" readonly  placeholder="仓库编码 *必填"></el-input>
                        </el-form-item>
                        <el-form-item label="仓库名称" prop="warehouseName" style="margin-left:20px">
                        <el-input v-model="warehouseForm.warehouseName" placeholder="仓库名称 *必填"></el-input>
                      </el-form-item>
                      <el-form-item label="仓库类型" prop="warehouseType" style="margin-left:20px">
                        <el-select v-model="warehouseForm.warehouseType" style="width: 100%;" placeholder="请选择仓库类型 *必填">
                                  <el-option
                                  v-for="item in warehouseTypeData"
                                  :key="item.optionKey"
                                  :label="item.optionName"
                                  :value="item.optionKey">
                                  </el-option>
                              </el-select>
                      </el-form-item> 
                      <el-form-item label="负责人" prop="chargePerson" style="margin-left:20px">
                              <el-input v-model="warehouseForm.chargePerson" placeholder="仓库负责人"></el-input>
                        </el-form-item>
                        <el-form-item label="负责人电话" prop="chargePersonPhone" style="margin-left:20px">
                        <el-input v-model="warehouseForm.chargePersonPhone" placeholder="负责人电话"/>
                      </el-form-item> 
                        <el-form-item label="仓库地址" prop="address" style="margin-left:20px">
                        <el-input v-model="warehouseForm.address"  placeholder="仓库地址"></el-input>
                      </el-form-item>
                      <el-form-item label="是否弃用" prop="isAbandon" style="text-align:left;margin-left:20px">
                        <el-checkbox  label="是" v-model="warehouseForm.isAbandon" ></el-checkbox>  
                      </el-form-item> 
                      <el-form-item label="备注" prop="remark" style="margin-left:20px">
                              <el-input v-model="warehouseForm.remark" placeholder="备注" ></el-input>
                        </el-form-item>
                  </el-col> 
                </el-row>     
              </el-form> 
              <el-form v-if="nodeType=='Shelf'" :model="shelfForm" :rules="shelfRules" ref="shelfFormRef" label-width="auto" label-position="left" style="padding:0 15px;position:relative;left:2%">
                <el-row> 
                  <el-col :span="12" :offset="5">
                        <el-form-item label="所属仓库" prop="warehouseName" style="margin-left:20px">
                          <el-input v-model="shelfForm.warehouseName" readonly placeholder="所属仓库 *必填"></el-input>
                        </el-form-item>
                        <el-form-item label="货架编码" prop="shelfNo" style="margin-left:20px">
                                <el-input v-model="shelfForm.shelfNo" placeholder="货架编码 *必填"></el-input>
                          </el-form-item>
                          <el-form-item label="货架名称" prop="shelfName" style="margin-left:20px">
                          <el-input v-model="shelfForm.shelfName" placeholder="货架名称 *必填"></el-input>
                        </el-form-item>
                        <el-form-item label="货架布局" prop="property" style="margin-left:20px">
                          <el-input v-model="shelfForm.property"  placeholder="货架在仓库中的布局（列*行） 例：5*2"></el-input>
                        </el-form-item>  
                        <el-form-item label="排序" prop="rank" style="margin-left:20px">
                          <el-input v-model="shelfForm.rank" type="number" placeholder="所在场景的排序"></el-input>
                        </el-form-item>
                        <el-form-item label="是否启用料箱" prop="hasWorkbin" style="text-align:left;margin-left:20px">
                        <el-checkbox  label="是" v-model="shelfForm.hasWorkbin" ></el-checkbox>  
                      </el-form-item>
                        <el-form-item label="是否弃用" prop="isAbandon" style="text-align:left;margin-left:20px">
                        <el-checkbox  label="是" v-model="shelfForm.isAbandon" ></el-checkbox>  
                      </el-form-item> 
                        <el-form-item label="备注" prop="remark" style="margin-left:20px">
                                <el-input v-model="shelfForm.remark" placeholder="备注" ></el-input>
                          </el-form-item>
                  </el-col> 
                </el-row>      
              </el-form> 
              <el-form v-if="nodeType=='Bin'" :model="binForm" :rules="binRules" ref="binFormRef" label-width="auto" label-position="left" style="padding:0 15px;position:relative;left:2%">
                <el-row> 
                  <el-col :span="12" :offset="5">
                        <el-form-item label="所属仓库" prop="warehouseName" style="margin-left:20px">
                          <el-input v-model="binForm.warehouseName" readonly placeholder="所属仓库"></el-input>
                        </el-form-item>
                        <el-form-item label="所属货架" prop="shelfName" style="margin-left:20px">
                          <el-input v-model="binForm.shelfName" readonly placeholder="所属货架"></el-input>
                        </el-form-item>
                        <el-form-item label="货位编码" prop="binNo" style="margin-left:20px">
                                <el-input v-model="binForm.binNo" :readonly="isEdit" placeholder="货位编码 *必填"></el-input>
                          </el-form-item>
                          <el-form-item label="货位名称" prop="binName" style="margin-left:20px">
                          <el-input v-model="binForm.binName" placeholder="货位名称 *必填"></el-input>
                        </el-form-item>
                        <el-form-item label="货位布局" prop="property" style="margin-left:20px">
                          <el-input v-model="binForm.property"  placeholder="货位在货架或仓库中的布局（列*行*排）例：4*5*2"></el-input>
                        </el-form-item> 
                        <el-form-item label="货位长度" prop="long" style="margin-left:20px">
                          <el-input v-model="binForm.long"  placeholder="货位长度（单位：cm）"></el-input>
                        </el-form-item>
                        <el-form-item label="货位宽度" prop="width" style="margin-left:20px">
                          <el-input v-model="binForm.width" placeholder="货位宽度（单位：cm）" ></el-input>
                        </el-form-item>
                        <el-form-item label="AGV寻址编码" prop="aGVNo" style="margin-left:20px">
                          <el-input v-model="binForm.aGVNo" placeholder="AGV寻址编码" ></el-input>
                        </el-form-item>
                        <el-form-item label="排序" prop="rank" style="margin-left:20px">
                          <el-input v-model="binForm.rank"  placeholder="所在场景的排序"></el-input>
                        </el-form-item>
                        <el-form-item label="允许多样存放" prop="isVarietyStock" style="text-align:left;margin-left:20px">
                        <el-checkbox  label="是" v-model="binForm.isVarietyStock" ></el-checkbox>  
                      </el-form-item> 
                        <el-form-item label="是否弃用" prop="isAbandon" style="text-align:left;margin-left:20px">
                        <el-checkbox  label="是" v-model="binForm.isAbandon"></el-checkbox>  
                      </el-form-item> 
                        <el-form-item label="备注" prop="remark" style="margin-left:20px">
                                <el-input v-model="binForm.remark" placeholder="备注"></el-input>
                          </el-form-item>
                  </el-col> 
                </el-row>      
              </el-form>
             
             <div v-if="(nodeType=='Warehouse'&&permission.isPermisstion('WAREHOUSEUPDATE'))||(nodeType=='Shelf'&&permission.isPermisstion('SHELFUPDATE'))||(nodeType=='Bin'&&permission.isPermisstion('BINUPDATE'))">
              <el-button   icon="iconfont icon-baocun-xianxing" type="primary" @click="onEditSubmit" style="width:45%" :loading="submitLoading">编辑保存</el-button>
             </div>   
             <div style="margin-top: 10px;" v-if="nodeType=='Warehouse'&&permission.isPermisstion('SHELFADD')">
              <el-button   type="warning" icon="iconfont icon-gonggeshitu" style="width:45%"  @click="onAdd('Shelf')" :loading="submitLoading" block>添加货架</el-button>
             </div>  
             <div style="margin-top: 10px;" v-if="(nodeType=='Warehouse'||nodeType=='Shelf')&&permission.isPermisstion('BINADD')">
              <el-button   type="warning" icon="el-icon-box" style="width:45%"  @click="onAdd('Bin')" :loading="submitLoading" block>添加货位</el-button>
             </div>
             
          </div>
        </div>
      </div>
      <EditLayer :layer="layer" v-if="layer.show" @invSubmit="onAddSubmit"/>
    </div> 
  </template>
  
  <script lang="ts" setup>
  defineOptions({
    name: "bin"
  })
  import { ref, nextTick,reactive ,onMounted} from 'vue' 
  import { LayerInterface } from "@/components/layer/index.vue"; 
  import { ElForm } from 'element-plus' 
  import{getWarehouseTree,getWarehouseDetail,getShelfDetail,getBinDetail,addShelf,updateShelf,delShelf,addBin,updateBin,delBin} from '@/api/inv/shelfBin' 
  import{getOptions,delWarehouseBin, updateWarehouseBin} from '@/api/inv/warehouse'
  import permission from '@/utils/system/permission';
  import EditLayer from './editLayer.vue'  

  const treeData = ref(); 
  const layer: LayerInterface = reactive({
      show: false,
      title: '新增',
      showButton: true,
      btnLoading:false,
      width:"30%",
      data:{},
      type:'',
      options:{},
      otherButton:{ }
    })
  const selectNode=ref();
  const nodeType=ref();
  const rightTitle=ref('');
  const delTitie=ref('');
  const warehouseTypeData=ref(new Array<any>());
  const isEdit=ref(false);
  const warehouseForm = ref({
        warehouseId:'',
        warehouseNo:'',
        warehouseName:'', 
        warehouseType:'',
        province:'',
        city:'',
        address:'', 
        chargePerson:'',
        chargePersonPhone:'',
        isAbandon:'',
        remark:'',
  }); 
  const warehouseFormRef = ref(ElForm||null);
  const warehouseRules={ 
        warehouseName: [{ required: true, message: '请输入仓库名称', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}], 
        remark:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        chargePerson:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        chargePersonPhone:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        address:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}]
  };
  const shelfForm = ref({
        shelfId:'',
        warehouseId:'',
        warehouseName:'',
        shelfNo:'', 
        shelfName:'',
        size:'',
        property:'',  
        isAbandon:false,
        hasWorkbin:false,
        rank:0,
        remark:'',
  }); 
  const shelfFormRef = ref(ElForm||null);
  const shelfRules={
        shelfNo: [{ required: true, message: '请输入货架编号', trigger: 'blur' },{ max:15, message: '字符超出限制长度', trigger: 'blur'}],
        shelfName: [{ required: true, message: '请输入货架名称', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}], 
        remark:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        size:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        property:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}] 
  }; 
  const binForm = ref({
        binId:0,
        warehouseId:'',
        warehouseName:'',
        shelfId:'', 
        shelfName:'',
        binNo:'',
        binName:'',
        aGVNo:'',
        long:0,
        width:0, 
        property:'',  
        specification:'',
        isAbandon:false,
        isVarietyStock:false,
        rank:0,
        remark:'',
        status:''
  }); 
  const binFormRef = ref(ElForm||null);
  const binRules={
        binNo: [{ required: true, message: '请输入货位编号', trigger: 'blur' },{ max:15, message: '字符超出限制长度', trigger: 'blur'}],
        binName: [{ required: true, message: '请输入货位名称', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}], 
        remark:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        aGVNo:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        property:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}] 
  };
  const submitLoading=ref(false);
  const btnDisabled=ref(true);   
  const tree= ref()
  const defaultProps = {
   children: "children",
   label: "label"
  };  

  onMounted(()=>{
      getTreeData(); 
      getWarehouseOptionsData(); 
    })

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

 const getWarehouseOptionsData=()=>{
  getOptions().then((res:any)=>{ 
      warehouseTypeData.value=res.data.argsOptions; 
    })
 }
  
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
  
 //点击节点
 const handleNodeClick = (row: any,nodePorps:any,e:any) => {  
  showSelectNode(row);
 }; 

 let showSelectNode=(row: any)=>{
  nodeType.value=row.type;
    selectNode.value=row;
    if(row.type=="Warehouse"){
      rightTitle.value="仓库"; 
      delTitie.value="删除该仓库将同时删除该仓库下的所有货架或货位";
      getWarehouseDetail(row.id).then(res=>{
        warehouseForm.value=res.data;
      })
    }
    else if(row.type=="Shelf"){
      rightTitle.value="货架";
      delTitie.value="删除该货架将同时删除该仓库下的所有货位";
      getShelfDetail(row.id).then(res=>{
        shelfForm.value=res.data;
      })
    }
    else{
      rightTitle.value="货位";
      delTitie.value="确定删除该货位吗？";
      getBinDetail(row.id).then(res=>{
        binForm.value=res.data;
      })
    }
    btnDisabled.value=false;
    isEdit.value=true;
 }

 //点击添加
 const onAdd=(type:string)=>{ 
  layer.show=true;
  layer.type=type;
  if(type=='Shelf'){
    layer.title='添加货架';
    layer.width='30%';
    layer.data.warehouseId=selectNode.value.id;
    layer.data.warehouseName=selectNode.value.label;
  }
  else{ 
    layer.title='添加货位'; 
    layer.width='45%'; 
    if(selectNode.value.type=='Warehouse'){
      layer.data.warehouseId=selectNode.value.id;
      layer.data.warehouseName=selectNode.value.label;
      layer.data.shelfId='';
      layer.data.shelfName=''; 
    }
    else if(selectNode.value.type=='Shelf'){ 
      layer.data.warehouseId=shelfForm.value.warehouseId;
      layer.data.warehouseName=shelfForm.value.warehouseName;
      layer.data.shelfId=selectNode.value.id;
      layer.data.shelfName=selectNode.value.label; 
    }
  }  
 }
  
 //删除提交
 const onDelSubmit=()=>{
  if(selectNode.value.type=='Warehouse'){
    delWarehouseBin([selectNode.value.id]).then(res=>{
      getTreeData();
    })
  }
  else if(selectNode.value.type=='Shelf'){
    delShelf(selectNode.value.id).then(res=>{
      getTreeData();
    })
  }
  else{
    delBin(selectNode.value.id).then(res=>{
      getTreeData();
    })
  }
 }

 //添加提交
 const onAddSubmit=(type:string,data:any)=>{
  if(type=='Shelf'){
    addShelf(data).then(res=>{
       layer.show=false;
       getTreeData(res.data);
    })
    .finally(()=>{
      layer.btnLoading=false;
    })
  }
  else{
    addBin(data).then(res=>{
      layer.show=false;
      getTreeData(res.data);
    })
    .finally(()=>{
      layer.btnLoading=false;
    })
  } 
 }

//编辑提交 
const onEditSubmit=()=>{
  if(nodeType.value=='Warehouse'){ 
      warehouseFormRef.value.validate((valid:any)=>{  
      if(valid){ 
          submitLoading.value=true
          updateWarehouseBin(warehouseForm.value)
          .then((res:any)=>{ 
            getTreeData(warehouseForm.value.warehouseId); 
          })
          .finally(()=>{
            submitLoading.value=false
          }) 
      }
    })
  }
  else if(nodeType.value=='Shelf'){
    shelfFormRef.value.validate((valid:any)=>{  
      if(valid){ 
          submitLoading.value=true
          updateShelf(shelfForm.value)
          .then((res:any)=>{ 
            getTreeData(shelfForm.value.shelfId); 
          })
          .finally(()=>{
            submitLoading.value=false
          }) 
      }
    })
  }
  else if(nodeType.value=='Bin'){
    binFormRef.value.validate((valid:any)=>{  
      if(valid){ 
          submitLoading.value=true
          updateBin(binForm.value)
          .then((res:any)=>{ 
            getTreeData(binForm.value.binId); 
          })
          .finally(()=>{
            submitLoading.value=false
          }) 
      }
    })
  } 
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
        }
      }
    }
  </style>