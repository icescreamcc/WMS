<template>
  <Layer :layer="layer" @confirm="submit">
    <el-form  label-width="auto" label-position="left" :model="ruleForm" :rules="rules" ref="formRef" style="padding: 7px 15px">
      <el-form-item label="单位类型" prop="unitType">
           <el-select v-model="ruleForm.unitType"  class="m-2" style="width:100%" placeholder="请选择单位类型 *必填" >
              <el-option v-for="item in unitTypeData" :key="item.key" :label="item.value" :value="item.key">
              </el-option>
            </el-select>
      </el-form-item>
    <el-form-item label="单位编号" prop="unitNo" >
          <el-input v-model="ruleForm.unitNo"   placeholder="请输入单位编号 *必填"></el-input>
          </el-form-item> 
                <el-form-item label="单位名称" prop="unitName">
        <el-input v-model="ruleForm.unitName" placeholder="请输入单位名称  *必填"></el-input>
      </el-form-item>
            <el-form-item label="备注" prop="remark">
        <el-input v-model="ruleForm.remark" placeholder="备注"></el-input>
      </el-form-item> 
    </el-form>
  </Layer>
</template>

<script lang="ts">
import { defineComponent,  ref } from 'vue'
import Layer from '@/components/layer/index.vue'  
import { ElForm } from 'element-plus';
import {getAboutUnitOptions} from '@/api/baseinfo/unit'
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
          type:'',
          data:null
        }
      }
    }
  }, 
  setup(props, ctx) {  
    let formRef= ref(ElForm||null)  
    let unitTypeData=ref(new Array<any>())
    let ruleForm = ref({
      unitType:props.layer.row?.unitType,
      unitId: props.layer.row?.unitId,
      unitNo:props.layer.row?.unitNo,
      unitName:props.layer.row?.unitName, 
      remark:props.layer.row?.remark
    }) 
    let rules = {
      unitType: [{ required: true, message: '请选择单位类型', trigger: 'blur' }],
      unitNo: [{ required: true, message: '请输入单位编号', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      unitName: [{ required: true, message: '请输入单位名称', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
      remark:[{ max: 50, message: '字符超出限制长度', trigger: 'blur'}] 
    }
  
  getAboutUnitOptions().then(res=>{
    unitTypeData.value=res.data.unitTypeOptions
  })

  //点击确认提交
  let submit=()=> {  
      formRef.value.validate((valid:any) => { 
        if (valid) {   
           ctx.emit('dataSubmit', ruleForm.value,props.layer.type)
        } else {
          return false;
        }
      }); 
    }

    return {
      ruleForm,
      unitTypeData,
      rules,
      formRef,
      submit 
    }
  } 
})
</script>

<style lang="scss" scoped>
  
</style>