<template>
  <Layer :layer="layer" @confirm="submit" >
     <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="auto" label-position="left" style="padding:0 15px">
       <el-row>
        <el-col :span="11">
             <el-form-item label="编码" prop="argsKey">
              <el-input v-model="ruleForm.argsKey" :disabled="isDisabled" ></el-input>
         </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
               <el-form-item label="名称" prop="argsKeyName">
         <el-input v-model="ruleForm.argsKeyName" />
       </el-form-item>
        </el-col>
      </el-row>
       <el-row>
        <el-col :span="11">
             <el-form-item label="备注" prop="remark">
              <el-input v-model="ruleForm.remark"  ></el-input>
         </el-form-item>
        </el-col>
        <el-col :span="11" :offset="2">
        <el-form-item label="排序" prop="rank">
         <el-input v-model="ruleForm.rank" type="number" />
       </el-form-item>
        </el-col>
      </el-row> 
          <el-scrollbar max-height="300px">
            <div class="option-content">
            <el-row class="head"> 
               <el-col :span="22" >
                 <p class="title">字典选项</p>
              </el-col>
              <el-col :span="2" >
                 <el-button style="margin-bottom:5px" type="primary" @click="addOption">添加</el-button> 
              </el-col>
             </el-row> 
             <el-row v-for="item in optionData" :key="item.optionId" class="item">
              <el-col :span="6">
                  <el-input v-model="item.optionKey" placeholder="请输入选项编码" >
                    <template #prepend>编码</template>
                  </el-input>
              </el-col>
              <el-col :span="6" :offset="1" >
                   <el-input v-model="item.optionName"  placeholder="请输入选项名称">
                    <template #prepend>名称</template>
                  </el-input>
              </el-col>
              <el-col :span="7" :offset="1" >
                   <el-input v-model="item.remark"  placeholder="备注">
                    <template #prepend>备注</template>
                  </el-input>
              </el-col>
              <el-col :span="2" :offset="1">
                  <el-button type="danger" @click="delOption(item)">删除</el-button> 
              </el-col>
           </el-row>  
            </div>
          </el-scrollbar>
      
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
    Layer,  
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
          type:'',
          data:null, 
        }
      }
    }
  }, 
  setup(props, ctx) {  
    const formRef= ref(ElForm||null)   
    const isDisabled=ref(false)
      //表单  
    const rules = {
       argsKey: [{ required: true, message: '请输入字典编码', trigger: 'blur' },{ max:100, message: '输入的字符数不能超过100个', trigger: 'blur'}],
       argsKeyName: [{ required: true, message: '请输入字典名称', trigger: 'blur' },{ max: 100, message: '输入的字符数不能超过100个', trigger: 'blur'}], 
       remark:[{ max: 100, message: '输入的字符数不能超过100个', trigger: 'blur'}]
    }   
    const ruleForm = ref({
        argsId:props.layer.data?.argsId,
        argsKey:props.layer.data?.argsKey,
        argsKeyName:props.layer.data?.argsKeyName, 
        argsGroup:'Dic',
        argsType:'SingleSelect',
        rank:props.layer.data?props.layer.data.rank:1,
        remark:'',
        argsOptions:new Array<any>()
    }) 
   const optionData=ref([
      {
        optionId:1,
        optionKey:'',
        optionName:'',
        remark:'',
        rank:1
    }
    ]) 
    if(props.layer.data){
      optionData.value=props.layer.data.argsOptions
      isDisabled.value=true
    }
    else{
      isDisabled.value=false
    }  
    
    const addOption=()=>{
      optionData.value.push({
        optionId: optionData.value.length+1,
        optionKey:'',
        optionName:'',
        remark:'',
        rank:optionData.value.length+1
      })
    }

    const delOption=(item:any)=>{  
      optionData.value.splice(optionData.value.indexOf(item),1)
    }

      //点击确认提交
    let  submit=()=> {    
        formRef.value.validate((valid:any)=>{ 
            if(valid){
              if(optionData.value.length==0){
                msg.warningAuto("请添加字典选项")
                return
              }
              else{
                 for(let opt of optionData.value){
                   if(!opt.optionKey||!opt.optionName){
                      msg.warningAuto("字典选项编码或名称不能为空")
                     return
                   }
                 } 
              }
              ruleForm.value.argsOptions=optionData.value
              for(let val of ruleForm.value.argsOptions){
                if(val.optionKey.length>40){
                  msg.warningAuto("字典选项编码超出限制长度")
                  return
                }
                else if(val.optionName.length>20){
                  msg.warningAuto("字典选项名称超出限制长度")
                  return
                }
              }
               ctx.emit('dataSubmit', ruleForm.value,props.layer.data?'update':'add') 
            }
        }) 
    }
    return{
      optionData,
      formRef,
      ruleForm,
      rules, 
      isDisabled,
      addOption,
      delOption,
      submit
    }
  } 
})
</script>

<style lang="scss" scoped>
  * {
    text-align: left;
  }
  .box-card{
    margin-top: 10px;
  }
  .option-content{
    border:1px solid rgb(230, 230, 230);
    border-radius: 3px;
    padding: 5px;
    .head{
      border-bottom: 1px solid rgb(230, 230, 230);
      margin-bottom: 5px;
      .title{
        margin: 5px 0 0 0;
      }
    }
    .item{
      margin:3px 0;
    }
  }
</style>