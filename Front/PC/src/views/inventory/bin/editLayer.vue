<template>
    <Layer :layer="layer" @confirm="onSubmit" ref="layerDom">   
      <el-form  v-if="layer.type=='Shelf'" :model="shelfForm" :rules="shelfRules" ref="shelfFormRef"  label-width="auto" label-position="left" style="padding:0 15px">
        <el-form-item label="所属仓库" prop="warehouseName"  style="margin-left:20px">
            <el-input v-model="shelfForm.warehouseName" readonly placeholder="所属仓库 不可编辑"/>
        </el-form-item>
        <el-form-item label="货架编码" prop="shelfNo" style="margin-left:20px">
                <el-input v-model="shelfForm.shelfNo" placeholder="货架编码 *必填"/> 
            </el-form-item>
            <el-form-item label="货架名称" prop="shelfName" style="margin-left:20px">
            <el-input v-model="shelfForm.shelfName" placeholder="货架名称 *必填"/>
        </el-form-item>
        <el-form-item label="货架布局" prop="property" style="margin-left:20px">
            <el-input v-model="shelfForm.property"  placeholder="货架在仓库中的布局（列*行） 例：5*2"/>
        </el-form-item>  
        <el-form-item label="排序" prop="rank" style="margin-left:20px">
            <el-input v-model="shelfForm.rank" type="number"  placeholder="所在场景的排序" />
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
    </el-form>
       <el-form v-else :model="binForm" :rules="binRules" ref="binFormRef"  label-width="auto" label-position="left" style="padding:0 15px">
       <el-row>
            <el-col :span="11">
                <el-form-item label="所属仓库" prop="warehouseName" style="margin-left:20px">
                    <el-input v-model="binForm.warehouseName" readonly placeholder="所属仓库 不可编辑"/>
                </el-form-item>
            </el-col>
            <el-col :span="11">
                <el-form-item label="所属货架" prop="shelfName" style="margin-left:20px">
                    <el-input v-model="binForm.shelfName" readonly placeholder="所属货架 不可编辑"/>
                </el-form-item>
             </el-col>
       </el-row>
       <el-row>
        <el-col :span="11">
            <el-form-item label="货位编码" prop="binNo" style="margin-left:20px">
                <el-input v-model="binForm.binNo" placeholder="货位编码 *必填"/>
            </el-form-item>
        </el-col>
        <el-col :span="11">
            <el-form-item label="货位名称" prop="binName" style="margin-left:20px">
            <el-input v-model="binForm.binName" placeholder="货位名称 *必填"/>
        </el-form-item>
        </el-col>
       </el-row>  
       <el-row>
        <el-col :span="11">
            <el-form-item label="货位布局" prop="property" style="margin-left:20px">
            <el-input v-model="binForm.property" placeholder="货位在货架或仓库中的布局（列*行*排）例：4*5*2" />
        </el-form-item>
        </el-col>
        <el-col :span="11"> 
            <el-form-item label="AGV寻址编码" prop="aGVNo" style="margin-left:20px">
            <el-input v-model="binForm.aGVNo"  placeholder="AGV寻址编码"/>
        </el-form-item>
        </el-col>
       </el-row> 
       <el-row>
        <el-col :span="11">
            <el-form-item label="货位长度" prop="long" style="margin-left:20px">
            <el-input v-model="binForm.long"  placeholder="货位长度（单位：cm）"/>
        </el-form-item>
        </el-col>
        <el-col :span="11">
            <el-form-item label="货位宽度" prop="width" style="margin-left:20px">
            <el-input v-model="binForm.width"  placeholder="货位宽度（单位：cm）"/>
        </el-form-item>
        </el-col>
       </el-row> 
       <el-row>
        <el-col :span="11">
            <el-form-item label="备注" prop="remark" style="margin-left:20px">
                <el-input v-model="binForm.remark" placeholder="备注"  />
            </el-form-item>
        </el-col>
        <el-col :span="11">

            <el-form-item label="排序" prop="rank" style="margin-left:20px">
            <el-input v-model="binForm.rank" type="number"  placeholder="所在场景的排序"/>
        </el-form-item>
        </el-col>
       </el-row> 
       <el-row>
        <el-col :span="11">
            <el-form-item label="允许多样存放" prop="isVarietyStock" style="text-align:left;margin-left:20px">
            <el-checkbox  label="是" v-model="binForm.isVarietyStock" ></el-checkbox>  
            </el-form-item> 
        </el-col>
        <el-col :span="11">
            <el-form-item label="是否弃用" prop="isAbandon" style="text-align:left;margin-left:20px">
            <el-checkbox  label="是" v-model="binForm.isAbandon" ></el-checkbox>  
            </el-form-item>
        </el-col> 
       </el-row>  
       </el-form> 
    </Layer>
  </template>
  
  <script lang="ts" setup> 
  import { ref,defineEmits,defineProps,onMounted } from 'vue'  
  import Layer from '@/components/layer/index.vue'
  import { ElForm } from 'element-plus'  
  import msg from '@/utils/system/message'
 
  const props=defineProps({
    layer:{
        type: Object,
        default:()=>{
            return{
                show: false,
                title: '',
                showButton: true,
                btnLoading:false,
                width:"30%",
                type:'',
                data:null,
                options:null 
            }
        }
    }
  });  

  onMounted(()=>{ 
  })

  const emit = defineEmits(['invSubmit'])
  const shelfForm = ref({
        shelfId:'',
        warehouseId:props.layer.data?.warehouseId,
        warehouseName:props.layer.data?.warehouseName,
        shelfNo:'', 
        shelfName:'',
        size:'',
        property:'',  
        isAbandon:false,
        hasWorkbin:false,
        rank:0,
        remark:'',
  }); 
  const shelfRules={
        shelfNo: [{ required: true, message: '请输入货架编号', trigger: 'blur' },{ max:15, message: '字符超出限制长度', trigger: 'blur'}],
        shelfName: [{ required: true, message: '请输入货架名称', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}], 
        remark:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        size:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        property:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}] 
  };
  const shelfFormRef = ref(ElForm||null); 
  const binForm = ref({
        binId:0,
        warehouseId:props.layer.data?.warehouseId,
        warehouseName:props.layer.data?.warehouseName,
        shelfId:props.layer.data?.shelfId, 
        shelfName:props.layer.data?.shelfName,
        binNo:'',
        binName:'',
        aGVNo:'',
        long:0,
        width:0, 
        property:'',  
        specification:'',
        isVarietyStock:false,
        isAbandon:false,
        rank:0,
        remark:'',
        status:''
  }); 
  const binRules={
        binNo: [{ required: true, message: '请输入货位编号', trigger: 'blur' },{ max:15, message: '字符超出限制长度', trigger: 'blur'}],
        binName: [{ required: true, message: '请输入货位名称', trigger: 'blur' },{ max: 20, message: '字符超出限制长度', trigger: 'blur'}], 
        remark:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        aGVNo:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}],
        property:[{ max: 20, message: '字符超出限制长度', trigger: 'blur'}] 
  };
  const binFormRef = ref(ElForm||null); 

  const onSubmit=()=>{  
    if(props.layer.type=='Shelf'){
        shelfFormRef.value.validate((valid:any)=>{
            if(valid){ 
                props.layer.btnLoading=true;
                emit('invSubmit',props.layer.type,shelfForm.value)
            }
        }) 
    }
    else{
        binFormRef.value.validate((valid:any)=>{
            if(valid){ 
                props.layer.btnLoading=true;
                emit('invSubmit',props.layer.type,binForm.value)
            }
        }) 
    } 
  }
  </script>
  
  <style lang="scss" scoped>
    
  </style>