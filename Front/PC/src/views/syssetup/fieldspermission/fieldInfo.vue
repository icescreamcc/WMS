<template>
  <div class="layout-container">
      <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
           <h2>权限编辑</h2>
      </div> 
    </div>
      <el-row>
        <el-col :span="12" :offset="6">
      <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:15px"> 
       <el-form-item label="表编码" prop="tableName">
        <el-input v-model="ruleForm.tableName" readonly></el-input>
      </el-form-item>
      <el-form-item label="表名称" prop="tableDesc">
        <el-input v-model="ruleForm.tableDesc" readonly ></el-input>
      </el-form-item>
        <el-form-item label="字段编码" prop="fieldName">
        <el-input v-model="ruleForm.fieldName" readonly ></el-input>
      </el-form-item>
         <el-form-item label="字段名称" prop="fieldDesc">
        <el-input v-model="ruleForm.fieldDesc" readonly ></el-input>
      </el-form-item>  
      <div v-if="showEdit">
       <el-form-item label="无权限角色" > 
           <el-row>
          <el-col :span="6" v-for="item in roleList" :key="item.roleId" style="text-align:left;margin-bottom:10px">
             <el-checkbox v-model="item.isSelected" :disabled="!isEdit" :label="item.roleName" :checked="item.isSelected"/>
          </el-col>
        </el-row> 
      </el-form-item> 
      </div>
      <br>
      <el-form-item v-if="permission.isPermisstion('FIELDSPERMISSIONUPDATE')">
         <el-button type="primary" style="width:20%"  icon="el-icon-edit" size="small" @click="editStart" v-if="!isEdit&&showEdit">编辑</el-button>
         <el-button type="primary" style="width:20%" icon="el-icon-check" size="small" @click="editSubmit" v-if="isEdit&&showEdit" :loading="btnLoading">保存</el-button> 
      </el-form-item>
        </el-form>
        </el-col>
      </el-row> 
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, reactive, inject, watch, Ref } from 'vue' 
import{getPermissioField,getRoles,setPermissioFieldRole} from '@/api/system/fieldsPermission'   
import { ElForm } from 'element-plus' 
import permission from '@/utils/system/permission'
export default defineComponent({   
  setup(props,ctx) {   
    const isEdit=ref(false)
    const activeCategory: any = inject('active')
    const btnLoading=ref(false); 
    type FormInstance = InstanceType<typeof ElForm> 
    const formRef: Ref<FormInstance|null> = ref(null)  
    const roleList=ref([]) 
    const showEdit=ref(true)
    const rules = { 
    }
    const ruleForm = ref({
      fieldsManageId:'',
      tableName: '',
      tableDesc:'',
      fieldName:'',
      fieldDesc:'',
      permissionsRoles:new Array<any>()
    })

    // 获取菜单详细 
    const getDetail = () => {  
      showEdit.value=false
      if(activeCategory.value){
        if(activeCategory.value.type=="field"){
             getPermissioField(activeCategory.value.id).then((res:any)=>{
               ruleForm.value=res.data 
               showEdit.value=true
                if(ruleForm.value.permissionsRoles?.length>0){
                roleList.value.forEach((d:any) => {
                  d.isSelected=false
                  ruleForm.value.permissionsRoles.forEach((r:any)=>{
                    if(r.deniedRoleId==d.roleId){
                      d.isSelected=true
                    }
                  })
                });
              }
            }) 
        }
        else{
          ruleForm.value.tableName=activeCategory.value.id
          ruleForm.value.tableDesc=activeCategory.value.label
        } 
      }
    }

  const getOptions=()=>{
     getRoles().then((res:any)=>{ 
         roleList.value=res.data; 
      }) 
  }
 
   getOptions()
    watch(activeCategory, (newVal) => {  
      isEdit.value=false
      getDetail() 
    }) 

    //开始编辑
    const editStart=()=>{
      isEdit.value=true
    }

    //编辑提交
    const editSubmit=()=>{ 
      formRef.value?.validate((valid)=>{
        if(valid){
          btnLoading.value=true   
          ruleForm.value.permissionsRoles=roleList.value.filter((r:any)=>r.isSelected).map((r:any)=>{
              return{
              deniedRoleId:r.roleId
            }
          }) 
          setPermissioFieldRole(ruleForm.value).then(res=>{
            isEdit.value=false
          }).finally(()=>btnLoading.value=false)
        }
      })
    }
 
    return {   
      permission,
      showEdit,
      formRef,
      isEdit,
      roleList, 
      rules, 
      ruleForm,
      btnLoading,    
      getDetail,
      editStart,
      editSubmit
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