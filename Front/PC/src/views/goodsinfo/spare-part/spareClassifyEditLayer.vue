<template>
    <Layer :layer="layer" @confirm="handleSubmit" >   
      <el-form :model="typeForm" :rules="formRules" ref="formRef"  label-width="auto" label-position="left" style="padding:0 15px">
          <el-form-item label="父级类型" prop="parentId">
            <el-select v-model="typeForm.parentId"  class="m-2" placeholder="请选择父级菜单  *必填" style="width:100%" >
              <el-option
                v-for="item in parentTypes"
                :key="item.id"
                :label="item.label"
                :value="item.id" >
              </el-option>
            </el-select> 
        </el-form-item>
          <el-form-item label="类型编码" prop="typeNo">
            <el-input v-model="typeForm.typeNo" placeholder="请输入类型编码 *必填" ></el-input>
          </el-form-item>
          <el-form-item label="类型名称" prop="typeName">
            <el-input v-model="typeForm.typeName" placeholder="请输入类型名称 *必填" ></el-input>
          </el-form-item> 
             <el-form-item label="备注" prop="remark">
            <el-input v-model="typeForm.remark" ></el-input>
          </el-form-item> 
          <el-form-item label="排序" prop="rank">
            <el-input v-model="typeForm.rank" type="number"  
             @keypress="(event:any)=>{event.target.value=event.target.value.replace(/[^\d]/g,'')}"></el-input>
          </el-form-item>  
       </el-form>
    
    </Layer>
  </template>
  
  <script lang="ts" setup> 
  import { defineEmits,defineProps, ref } from 'vue'  
  import Layer from '@/components/layer/index.vue'
  import { ElForm } from 'element-plus'  

  const props=defineProps({
    layer: {
        type: Object,
        default: () => {
          return {
            show: false,
            title: '',
            showButton: true,
            btnLoading:false,
            width:"30%",
            row:null,
            type:'',
            data:null 
           }
        }
      }
  });
  
  const emit = defineEmits(['submit'])

  const typeForm = ref({ 
        typeId:0,
        typeNo:null,
        typeName:null,
        remark:null,
        parentId:'ROOT',
        parentName:'ROOT',
        rank:1
      })
      const formRules = {
        typeNo: [{ required: true, message: '请输入类型编码', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        typeName: [{ required: true, message: '请输入类型名称', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        parentId: [{ required: true, message: '未获取到父级类型', trigger: 'blur' }],
        remark :[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}],
      }
      const formRef = ref(ElForm||null)  
      const parentTypes=ref(new Array<any>())
  
    if(props.layer.type=='add'){ 
        typeForm.value.typeId=0;
        typeForm.value.typeNo=null;
        typeForm.value.typeName=null;
        typeForm.value.remark=null;
        typeForm.value.parentId=props.layer.row?.id?props.layer.row.id:'ROOT';
        typeForm.value.parentName=props.layer.row?.label?props.layer.row.label:'ROOT'
        typeForm.value.rank=1;
    }
    else{
        typeForm.value.typeId=props.layer.row.id;
        typeForm.value.typeNo=props.layer.row.type;
        typeForm.value.typeName=props.layer.row.label;
        typeForm.value.remark=props.layer.row.remark;
        typeForm.value.parentId=props.layer.row.parentId
        typeForm.value.parentName=props.layer.row.parentName 
        typeForm.value.rank=props.layer.row?.rank;
    }
  
      const findTypes=(treeData:any)=>{
        let types=new Array<any>();
        treeData.forEach((element:any) => {
          types.push({id:element.id,label:element.label,type:element.type});
          findChilren(element.children,types);
        }); 
       return types;
      }
      const findChilren=(children:Array<any>,types:Array<any>)=>{
        if(children?.length>0){
          children.forEach((child:any)=>{
           types.push({id:child.id,label:child.label,type:child.type})
           findChilren(child.children,types);
          })
        }
      }
      parentTypes.value=findTypes(props.layer.data)
  
    //提交
    const handleSubmit=()=>{ 
      formRef.value.validate((valid:any)=>{
         if(valid){
           typeForm.value.parentId=typeForm.value.parentId=='ROOT'?'0':typeForm.value.parentId
           typeForm.value.rank=typeForm.value.rank?typeForm.value.rank:0
          emit("submit",typeForm.value,typeForm.value.typeId?'update':'add') 
         }
      })
    } 
  </script>
  
  <style lang="scss" scoped>
    
  </style>