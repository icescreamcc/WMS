<template>
  <div class="layout-container">
      <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
           <h2>菜单明细</h2>
      </div> 
    </div>
      <el-row>
        <el-col :span="12" :offset="6">
      <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:15px"> 
      <el-form-item label="菜单编码" prop="menuId">
        <el-input v-model="ruleForm.menuId" disabled ></el-input>
      </el-form-item>
      <el-form-item label="菜单名称" prop="menuName">
        <el-input v-model="ruleForm.menuName" :disabled="!isEdit" placeholder="请输入菜单名称 *必填"></el-input>
      </el-form-item>
      <el-form-item label="菜单类型" prop="menuType"> 
         <el-select v-model="ruleForm.menuType" :disabled="!isEdit" class="m-2" placeholder="请选择菜单类型  *必填" style="width:100%" >
            <el-option
              v-for="item in menuTypes"
              :key="item.key"
              :label="item.value"
              :value="item.key"
            >
            </el-option>
          </el-select>
      </el-form-item>
      <el-form-item label="父级菜单" prop="parentId">
        <el-input v-model="ruleForm.parentName" disabled placeholder="请输入企业名称 *必填"></el-input>
      </el-form-item> 
        <el-form-item label="菜单排序" prop="rank">
        <el-input v-model="ruleForm.rank" type="number"
         @keypress="(event)=>{event.target.value=event.target.value.replace(/[^\d]/g,'')}" 
         :disabled="!isEdit" placeholder="请输入菜单排序"></el-input>
      </el-form-item> 
       <el-form-item label="菜单图标" prop="icon">
        <el-input v-model="ruleForm.icon" :disabled="!isEdit" ></el-input>
      </el-form-item>  
       <el-form-item label="前端路由URL" prop="routePath">
        <el-input v-model="ruleForm.routePath" :disabled="!isEdit" ></el-input>
      </el-form-item> 
      <el-form-item label="前端组件URL" prop="componentPath">
        <el-input v-model="ruleForm.componentPath" :disabled="!isEdit" ></el-input>
      </el-form-item> 
     <el-form-item label="后端Controller" prop="ctrlName"> 
           <el-input v-model="ruleForm.ctrlName" :disabled="!isEdit" ></el-input>
      </el-form-item> 
         <el-form-item label="后端Action" prop="actionName"> 
           <el-input v-model="ruleForm.actionName" :disabled="!isEdit"></el-input>
      </el-form-item> 
        <el-form-item label="备注" prop="remark">
        <el-input v-model="ruleForm.remark" type="textarea" :disabled="!isEdit" ></el-input>
      </el-form-item> 
          <el-form-item label="是否生效" prop="isValid" style="text-align:left"> 
        <el-checkbox v-model="ruleForm.isValid" :disabled="!isEdit" label="是"/>
      </el-form-item> 
       <el-form-item label="是否可见" prop="isVisible" style="text-align:left"> 
        <el-checkbox v-model="ruleForm.isVisible" :disabled="!isEdit" label="是"/>
      </el-form-item> 
      <br>
      <el-form-item>
         <el-button type="primary" style="width:20%"  icon="el-icon-edit" size="small" @click="editStart" v-if="!isEdit">编辑</el-button>
            <el-button type="primary" style="width:20%" icon="el-icon-check" size="small" @click="editSubmit" v-if="isEdit" :loading="submitLoading">保存</el-button>
             <el-popconfirm :title="delTitie"  @confirm="handleDel()">
              <template #reference>
                <el-button type="danger" style="width:20%"  size="small" title="删除菜单" icon="el-icon-delete" >删除</el-button>
              </template>
            </el-popconfirm> 
      </el-form-item>
        </el-form>
        </el-col>
      </el-row> 
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, reactive, inject, watch, Ref } from 'vue' 
import{getMenuInfo,addMenu,updateMenu,delMenu} from '@/api/system/menus' 
import emitter from '@/utils/system/eventBus'
import { ElForm } from 'element-plus'
import { Model } from 'echarts'
export default defineComponent({   
  setup(props,ctx) {   
    const isEdit=ref(false)
    const activeCategory: any = inject('active')
    const btnLoading=ref(false); 
    type FormInstance = InstanceType<typeof ElForm> 
    const formRef: Ref<FormInstance|null> = ref(null) 
    const delTitie=ref('')
    const menuTypes=reactive([{key:1,value:'Menu'},{key:2,value:'Action'}])
    const rules = {
      menuId: [{ required: true, message: '请输入菜单编码', trigger: 'blur' }],
      menuName: [{ required: true, message: '请输入菜单名称名称', trigger: 'blur' }], 
    }
    const ruleForm = ref({
      menuId: '',
      menuName:'',
      parentId:'',
      parentName:'',
      parentType:'',
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
    // 获取菜单详细 
    const getMenuDetail = () => {  
      if(activeCategory.value){
        getMenuInfo(activeCategory.value.id).then((res:any)=>{
        ruleForm.value=res.data 
      }) 
      }
    }
 
    watch(activeCategory, (newVal) => {  
      getMenuDetail()
     if(activeCategory.value.children?.length>0){
        delTitie.value="当前菜单存在下级菜单或方法,是否确定删除?"
      }
      else{
        delTitie.value='是否确定删除?'
      }
    }) 

    //开始编辑
    const editStart=()=>{
      isEdit.value=true
    }

    //编辑提交
    const editSubmit=()=>{
      btnLoading.value=true  
      ruleForm.value.rank=ruleForm.value.rank?ruleForm.value.rank:1
      updateMenu(ruleForm.value).then(res=>{
         emitter.emit("reloadTree",ruleForm.value.menuId)
         isEdit.value=false
      }).finally(()=>btnLoading.value=false)
    }

    //删除数据
    const handleDel=()=>{   
      if(activeCategory.value.id){ 
        btnLoading.value=true;
         delMenu(activeCategory.value.id,activeCategory.value.parentId,activeCategory.value.type)
        .then(()=>{ emitter.emit("reloadTree") })
        .finally(()=>{btnLoading.value=false})
      } 
    }
    return {   
      formRef,
      isEdit,
      menuTypes,
      rules,
      delTitie,
      ruleForm,
      btnLoading,    
      getMenuDetail,
      editStart,
      editSubmit,
      handleDel
    }
  }
})
</script>

<style lang="scss" scoped>
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
</style>