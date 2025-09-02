<template>
  <div class="layout-container">
      <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
           <h2>字段明细</h2>
      </div> 
    </div>
      <el-row>
        <el-col :span="12" :offset="6">
      <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:15px"> 
       <el-form-item label="表编码" prop="tableName">
        <el-input v-model="ruleForm.tableName" disabled></el-input>
      </el-form-item>
      <el-form-item label="表名称" prop="tableDesc">
        <el-input v-model="ruleForm.tableDesc" disabled ></el-input>
      </el-form-item>
        <el-form-item label="字段编码" prop="fieldName">
        <el-input v-model="ruleForm.fieldName" disabled placeholder="请输入菜单名称 *必填"></el-input>
      </el-form-item>
         <el-form-item label="字段名称" prop="fieldDesc">
        <el-input v-model="ruleForm.fieldDesc" :disabled="!isEdit" placeholder="请输入字段名称 *必填"></el-input>
      </el-form-item> 
       <el-form-item label="字段类型" prop="fieldType"> 
          <el-select v-model="ruleForm.fieldType" :disabled="!isEdit" class="m-2" placeholder="请选择字段类型  *必填" style="width:100%" >
            <el-option
              v-for="item in fieldTypesList"
              :key="item.key"
              :label="item.value"
              :value="item.key" >
            </el-option>
          </el-select>
      </el-form-item>
         <el-form-item label="字段长度" prop="fieldLength">
        <el-input v-model="ruleForm.fieldLength" type="number" :disabled="!isEdit" placeholder="请输入字段长度 *必填"></el-input>
      </el-form-item> 
      <el-form-item label="数据字典" prop="argsKey"> 
         <el-select v-model="ruleForm.fieldArgsKey" :disabled="!isEdit" class="m-2" placeholder="请选择数据字典" style="width:100%" >
            <el-option
              v-for="item in fieldArgsList"
              :key="item.argsKey"
              :label="item.argsKeyName"
              :value="item.argsKey"
            >
            </el-option>
          </el-select>
      </el-form-item> 
       <el-form-item label="是否启用" prop="isEnable" style="text-align:left"> 
        <el-checkbox v-model="ruleForm.isEnable" :disabled="!isEdit" label="是"/>
      </el-form-item> 
      <br>
      <el-form-item  v-if="permission.isPermisstion('FIELDSUPDATE')">
         <el-button type="primary" style="width:20%"  icon="el-icon-edit" size="small" @click="editStart" v-if="!isEdit&&showEdit">编辑</el-button>
         <el-button type="primary" style="width:20%" icon="el-icon-check" size="small" @click="editSubmit" v-if="isEdit&&showEdit" >保存</el-button> 
      </el-form-item>
        </el-form>
        </el-col>
      </el-row> 
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, reactive, inject, watch, Ref } from 'vue' 
import{getSpareField,setSpareField} from '@/api/system/fieldsManage' 
import{getDictionarys,getFieldTypes} from '@/api/common' 
import emitter from '@/utils/system/eventBus'
import { ElForm } from 'element-plus' 
import permission from '@/utils/system/permission'
export default defineComponent({   
  setup(props,ctx) {   
    const isEdit=ref(false)
    const activeCategory: any = inject('active')
    const btnLoading=ref(false); 
    type FormInstance = InstanceType<typeof ElForm> 
    const formRef: Ref<FormInstance|null> = ref(null)  
    const fieldArgsList=ref()
    const fieldTypesList=ref()
    const showEdit=ref(true); 
    const rules = {
      fieldDesc: [{ required: true, message: '请输入字段名称', trigger: 'blur' },{ max: 7, message: '字符超出限制长度', trigger: 'blur'}],
      fieldType: [{ required: true, message: '请选择字段类型', trigger: 'blur' }], 
    }
    const ruleForm = ref({
      fieldsManageId:'',
      tableName: '',
      tableDesc:'',
      fieldName:'',
      fieldDesc:'',
      fieldType:'',
      fieldLength:50,
      isEnable:false,
      fieldArgsKey:'',
      remark:'',
      rank:''
    })

    // 获取菜单详细 
    const getDetail = () => {  
      showEdit.value=false
      if(activeCategory.value){
        if(activeCategory.value.type=="field"){
             getSpareField(activeCategory.value.id).then((res:any)=>{
               if(activeCategory.value.remark=="SpareField"){
                   showEdit.value=true;
               } 
              ruleForm.value=res.data 
            }) 
        }
        else{
          ruleForm.value.tableName=activeCategory.value.id
          ruleForm.value.tableDesc=activeCategory.value.label
        } 
      }
    }

  const getOptions=()=>{
     getDictionarys().then((res:any)=>{
        fieldArgsList.value=res.data;
      })
      getFieldTypes().then((res:any)=>{
        fieldTypesList.value=res.data;
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
          setSpareField(ruleForm.value).then(res=>{
            emitter.emit("sparefield_reloadTree",ruleForm.value.fieldsManageId)
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
      fieldArgsList,
      fieldTypesList,
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