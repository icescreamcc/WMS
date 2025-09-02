<template>
  <Layer :layer="layer" @confirm="handleSubmit" ref="layerDom">  
     <el-form-item label="" prop="radio" >
        <el-radio-group v-model="selectedType" @change="handleTypeChanged" >
          <el-radio-button  v-for="item in radioData" :key="item.value" :label="item.value" :disabled="radioDisabled&&selectedType!=item.value">{{ item.label }}</el-radio-button>
        </el-radio-group>
      </el-form-item>

    <el-form :model="orgaForm" :rules="orgaRules" ref="orgaRuleForm"  label-width="auto" label-position="left" style="padding:0 15px">
        <el-form-item label="直属上级" prop="ParentName">
            <el-input v-model="orgaForm.ParentName" disabled></el-input>
      </el-form-item>
        <el-form-item :label="orgaForm.NoLabel" prop="No">
          <el-input v-model="orgaForm.No" placeholder="请输入编号 *必填" :disabled="inputDisabled"></el-input>
        </el-form-item>
        <el-form-item :label="orgaForm.NameLabel" prop="Name">
          <el-input v-model="orgaForm.Name" placeholder="请输入名称 *必填" :disabled="inputDisabled"></el-input>
        </el-form-item> 
        <el-form-item label="排序" prop="Rank">
          <el-input v-model="orgaForm.Rank" type="number" :disabled="inputDisabled"
           @keypress="(event)=>{event.target.value=event.target.value.replace(/[^\d]/g,'')}" 
           placeholder="请输入序号 *必填"></el-input>
        </el-form-item>  
     </el-form>
  
  </Layer>
</template>

<script lang="ts"> 
import { defineComponent, ref } from 'vue'  
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
          type:'',
          data:null 
         }
      }
    }
  },
  setup(props, ctx) { 
    const layerDom = ref(ElForm||null) 
    let rootInfo:any={}

      //表单
    const inputDisabled=ref(false); 
    const orgaForm = ref({ 
      Id: '',
      No:'',
      Name:'',
      DeptId:'',
      ParentId:'',
      ParentName:'', 
      ParentType:'',
      NoLabel:'',
      NameLabel:'',
      Rank:1
    })
    const orgaRules = {
      No: [{ required: true, message: '请输入编号', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      Name: [{ required: true, message: '请输入名称', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      ParentName: [{ required: true, message: '未获取到直属上级', trigger: 'blur' }]
    }
    const orgaRuleForm = ref(ElForm||null) 

    //组织架构类型选择
    const radioData = [ { value: 'Dept', label: '部门' }, { value: 'Role', label: '角色' }  ]
    const selectedType=ref('Dept');
    const radioDisabled=ref(false); 
    const handleTypeChanged=(sel:any)=>{
       infoSwitch();
    } 

    let infoSwitch=()=>{ 
       if(selectedType.value=="Company"){
         orgaForm.value.ParentId=rootInfo.id;
         orgaForm.value.ParentName=rootInfo.label; 
         orgaForm.value.ParentType=rootInfo.type;
         orgaForm.value.NoLabel="编号";
         orgaForm.value.NameLabel="名称";
          if(props.layer.type=="Update"){
            orgaForm.value.Id=props.layer.row.id;
            orgaForm.value.No=props.layer.row.remark;
            orgaForm.value.Name=props.layer.row.label;
            orgaForm.value.Rank=  props.layer.row.rank;
            radioDisabled.value=true;
            inputDisabled.value=true;
          }
       }
      else if(selectedType.value=="Dept"){ 
        //如果是新增或编辑部门,则以公司作为直属上级
         orgaForm.value.ParentId=rootInfo.id;
         orgaForm.value.ParentName=rootInfo.label; 
         orgaForm.value.ParentType=rootInfo.type;
         orgaForm.value.NoLabel="部门编号";
         orgaForm.value.NameLabel="部门名称";
          if(props.layer.type=="Update"){
            orgaForm.value.Id=props.layer.row.id;
            orgaForm.value.No=props.layer.row.remark;
            orgaForm.value.Name=props.layer.row.label;
            orgaForm.value.Rank=  props.layer.row.rank;
            radioDisabled.value=true;
          }
      }
      else{ 
        //如果是新增或编辑角色,则以当前选择的节点作为直属上级
         orgaForm.value.ParentId=props.layer.row.id;
         orgaForm.value.ParentName=props.layer.row.label; 
         orgaForm.value.ParentType=props.layer.row.type;
         orgaForm.value.NoLabel="角色编号";
         orgaForm.value.NameLabel="角色名称";
         //如果是编辑当前选择的角色,则需要另外找出他的直属上级
         if(props.layer.type=="Update"){ 
           let parent=findParent(rootInfo,props.layer.row.id); 
           if(parent){
               orgaForm.value.ParentId=parent.id;
               orgaForm.value.ParentName=parent.label; 
               orgaForm.value.ParentType=parent.type;
           }
           orgaForm.value.Id=props.layer.row.id;
           orgaForm.value.No=props.layer.row.remark;
           orgaForm.value.Name=props.layer.row.label;
           orgaForm.value.Rank=  props.layer.row.rank;
           radioDisabled.value=true;
        }   
      }
    }

    let findParent=(parent:any,searchId:any)=>{
      let find:any; 
      if(parent.children&&parent.children.length>0){
        for(let child of parent.children){
          if(child.id==searchId){
            find= parent;  
          }
          if(!find){
            find= findParent(child,searchId);
          }
          else{
            break;
          }
        } 
      }
      return find; 
    }
    
    //用于判断新增还是编辑,部门或者角色
    function init() {  
      rootInfo=props.layer.data[0]; 
      selectedType.value=props.layer.row.type; 
      infoSwitch();  
    }
    init()
    
  //提交
  const handleSubmit=()=>{
   if(selectedType.value=="Company"){
     msg.warningAuto("请选择组织架构类型")
     return
   }
    orgaRuleForm.value.validate((valid:any)=>{
       if(valid){
        ctx.emit("submit",selectedType.value,props.layer.type,rootInfo.id,orgaForm.value) 
       }
    })
  }

  return {
    layerDom, 
    orgaForm,
    inputDisabled,
    orgaRules,
    orgaRuleForm,
    radioData,
    radioDisabled, 
    selectedType,
    handleTypeChanged,
    handleSubmit
  }
  }
})
</script>

<style lang="scss" scoped>
  
</style>