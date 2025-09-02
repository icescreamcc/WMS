<template>
    <el-form-item :label="fieldsInfo.fieldDesc" style="margin-left:20px;text-align:left" :prop="fieldsInfo.fielsRule">
      <div v-if="fieldsInfo.fieldType=='Text'">
          <el-input @input="modelChanged" v-model="modelValue" style="width:100%"/>
      </div>
        <div v-else-if="fieldsInfo.fieldType=='Number'">
          <el-input type="number" @input="modelChanged" v-model="modelValue" style="width:100%"></el-input>
      </div>
       <div v-else-if="fieldsInfo.fieldType=='Date'">
            <el-date-picker v-model="modelValue" style="width:100%" type="date" value-format="YYYY-MM-DD" @change="modelChanged" placeholder="请选择日期"> </el-date-picker>
      </div>
       <div v-else-if="fieldsInfo.fieldType=='SingleSelect'">
                <el-select v-model="modelValue" class="m-2" :placeholder="'请选择'+fieldsInfo.fieldDesc" @change="modelChanged" style="width:100%">
                  <el-option
                  v-for="item in optionData"
                  :key="item.optionId"
                  :label="item.optionName"
                  :value="item.optionName"
                >
                </el-option>
          </el-select>
      </div>
       <div v-else-if="fieldsInfo.fieldType=='Check'">
              <el-checkbox  label="是" v-model="modelValue" @change="modelChanged" />    
      </div>
  </el-form-item>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue' 
import{getDictionaryOption} from '@/api/common'    
export default defineComponent({ 
  props: {
    fieldsInfo: {
      type: Object,
      default: () => {
        return {
          fieldName: null,
          fieldDesc: null,
          fieldType: null ,
          fieldArgsKey:null,
          fieldLength:0
        }
      }
    },
    fieldsData:null,
    fielsRule:null
  },
  setup(props, ctx) {   

  let optionData=ref(new Array<any>()) 
  if(props.fieldsInfo.fieldArgsKey){
     getDictionaryOption(props.fieldsInfo.fieldArgsKey).then((res:any)=>{
      optionData.value=res.data.argsOptions;
    })
  } 

  let modelValue=ref(props.fieldsData) 
  if(props.fieldsInfo.fieldType=='Check'){
   modelValue.value= props.fieldsData=='true'?true:false
  }
  let modelChanged=(e:any)=>{ 
    ctx.emit("update:modelValue",e)
  } 
    return {  
      modelValue,
      optionData,
      modelChanged, 
    }
  }
})
</script>

<style lang="scss" scoped> 
</style>