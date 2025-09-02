<template>
  <Layer :layer="layer" @confirm="handleSubmit" ref="layerDom">   
    <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
       <el-row>
    <el-col :span="11">
            <el-form-item label="菜单编码" prop="menuId">
        <el-input v-model="ruleForm.menuId"  ></el-input>
      </el-form-item>
    </el-col>
    <el-col :span="11" :offset="2">
            <el-form-item label="菜单名称" prop="menuName">
        <el-input v-model="ruleForm.menuName"  placeholder="请输入菜单名称 *必填"></el-input>
      </el-form-item>
    </el-col>
  </el-row>
         <el-row>
    <el-col :span="11">
         <el-form-item label="菜单类型" prop="menuType"> 
         <el-select v-model="ruleForm.menuType"  class="m-2" placeholder="请选择菜单类型  *必填" style="width:100%" >
            <el-option
              v-for="item in menuTypes"
              :key="item.key"
              :label="item.value"
              :value="item.value"
            >
            </el-option>
          </el-select>
      </el-form-item>
    </el-col>
    <el-col :span="11" :offset="2">
       <el-form-item label="父级菜单" prop="parentId">
          <el-select v-model="ruleForm.parentId"  class="m-2" placeholder="请选择父级菜单  *必填" style="width:100%" >
            <el-option
              v-for="item in parentMenus"
              :key="item.id"
              :label="item.label"
              :value="item.id" >
            </el-option>
          </el-select>
        <!-- <el-input v-model="ruleForm.parentName" disabled placeholder="请输入企业名称 *必填"></el-input> -->
      </el-form-item> 
    </el-col>
  </el-row>
         <el-row>
    <el-col :span="11">
        <el-form-item label="菜单排序" prop="rank">
        <el-input v-model="ruleForm.rank" type="number"
         @keypress="(event)=>{event.target.value=event.target.value.replace(/[^\d]/g,'')}" 
          placeholder="请输入菜单排序"></el-input>
      </el-form-item> 
    </el-col>
    <el-col :span="11" :offset="2">
         <el-form-item label="菜单图标" prop="icon">
        <el-input v-model="ruleForm.icon"  ></el-input>
      </el-form-item>  
    </el-col>
  </el-row>
         <el-row>
    <el-col :span="11">
        <el-form-item label="前端路由URL" prop="routePath">
        <el-input v-model="ruleForm.routePath"  ></el-input>
      </el-form-item> 
    </el-col>
    <el-col :span="11" :offset="2">
       <el-form-item label="前端组件URL" prop="componentPath">
        <el-input v-model="ruleForm.componentPath"  ></el-input>
      </el-form-item> 
    </el-col>
  </el-row>
         <el-row>
    <el-col :span="11">
       <el-form-item label="后端Controller" prop="ctrlName"> 
           <el-input v-model="ruleForm.ctrlName"  ></el-input>
      </el-form-item> 
    </el-col>
    <el-col :span="11" :offset="2">
       <el-form-item label="后端Action" prop="actionName"> 
           <el-input v-model="ruleForm.actionName" ></el-input>
      </el-form-item> 
    </el-col>
  </el-row>
           <el-row>
    <el-col :span="11">
          <el-form-item label="是否生效" prop="isValid" style="text-align:left"> 
        <el-checkbox v-model="ruleForm.isValid" label="是"/>
      </el-form-item> 
    </el-col>
    <el-col :span="11" :offset="2">
           <el-form-item label="是否可见" prop="isVisible" style="text-align:left"> 
        <el-checkbox v-model="ruleForm.isVisible" label="是"/>
      </el-form-item> 
    </el-col>
  </el-row>
             <el-row>
    <el-col :span="11">
              <el-form-item label="备注" prop="remark">
        <el-input v-model="ruleForm.remark"   ></el-input>
      </el-form-item> 
    </el-col>
    <el-col :span="11" :offset="2">
    </el-col>
  </el-row> 
     </el-form>
  
  </Layer>
</template>

<script lang="ts"> 
import { defineComponent, ref ,reactive,Ref} from 'vue'  
import Layer from '@/components/layer/index.vue'
import { ElForm } from 'element-plus' 
import msg from '@/utils/system/message'
export default defineComponent({
  components: {
    Layer
  },
  props: {
    layer: {
      type: Object,
      default: () => {
        return {
          show: false,
          title: '',
          showButton: true,
          btnLoading:false,
          width:"30%", 
          data:null 
         }
      }
    }
  },
  setup(props, ctx) { 
    const formRef = ref(ElForm||null)  
      //表单 
    const ruleForm = ref({
      menuId: '',
      menuName:'',
      parentId:'ROOT',
      parentName:'ROOT',
      parentType:'ROOT',
      menuType:'',
      icon:'',
      rank:1,
      routePath:'',
      componentPath:'',
      ctrlName:'',
      actionName:'',
      isValid:true,
      isVisible:true,
      remark:''
    }) 
     const rules = {
      menuId: [{ required: true, message: '请输入菜单编码', trigger: 'blur' }],
      menuName: [{ required: true, message: '请输入菜单名称', trigger: 'blur' }], 
      menuType:[{ required: true, message: '请选择菜单类型名称', trigger: 'blur' }]
    }  
    const menuTypes=reactive([{key:1,value:'Menu'},{key:2,value:'Action'}])
    const parentMenus=ref(new Array<any>())
    let findParent=(node:any)=>{ 
       let menus=findMenus(props.layer.data);  
         for(let m of menus){
           if(m.id==node.parentId){
              ruleForm.value.parentId=m.id
              ruleForm.value.parentName=m.label
              ruleForm.value.parentType=m.type
              return ;
           }
         } 
    } 
   let findMenus=(treeData:any)=>{
      let menus=[{id:'ROOT',label:'ROOT',type:'ROOT'}];
      treeData.forEach((element:any) => {
        menus.push({id:element.id,label:element.label,type:element.type});
        if(element.children?.length>0&&element.type=="Menu"){
          element.children.forEach((child:any) => {
              menus.push({id:child.id,label:child.label,type:child.type})
          }); 
        }
      });
      parentMenus.value=menus 
     return menus;
    }   

  findParent(props.layer.row) 
 

  //提交
  const handleSubmit=()=>{ 
    if(ruleForm.value.parentId=="ROOT"&&ruleForm.value.menuType=="ACTION"){
      msg.warningAuto("父级为ROOT根目录时只允许创建Menu,不允许创建Action")
      return
    } 
    formRef.value.validate((valid:any)=>{
       if(valid){ 
        ctx.emit("submit",ruleForm.value) 
       }
    })
  }

  return { 
    formRef,
    ruleForm, 
    rules,  
    menuTypes,
    parentMenus, 
    handleSubmit
  }
  }
})
</script>

<style lang="scss" scoped>
  
</style>