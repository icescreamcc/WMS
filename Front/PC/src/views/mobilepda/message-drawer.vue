<template>
    <div>
        <el-drawer v-model="props.options.show" :direction="direction" :show-close="false" :size="props.options.type=='MultipleSelect'?'34%':'30%'" :lock-scroll="false" :close-on-click-modal="false"> 
                <template #default>
                <div class="drawer-content"> 
                    <h4>{{ props.options.title }}</h4>
                    <div v-if="props.options.type=='MultipleSelect'" style="text-align: left; padding:0 20px;margin-bottom: 20px;">
                        <el-checkbox-group v-model="submitData" >
                            <el-checkbox  style="margin-bottom: 5px;" v-for="item in props.options.data" :label="item.key" :checked="item.remark" v-model="item.key">{{ item.value }}</el-checkbox>
                        </el-checkbox-group>
                    </div>
                    <div v-else-if="props.options.type=='SingleSelect'" style="text-align: left; padding:0 20px;margin-bottom: 20px;">
                        <el-radio-group v-model="submitData">
                            <el-radio v-for="item in props.options.data" :label="item.key" >{{ item.value }}</el-radio> 
                        </el-radio-group> 
                    </div>
                    <div v-else style="font-size: 14px;margin-bottom: 25px;"> {{ props.options.message }}</div>
                    <el-row :class="props.options.type=='MultipleSelect'?'btn-pos-2':'btn-pos-1'" justify="center">
                        <el-col :span="11">
                            <el-button class="btn"  @click="onCancelClick">取消</el-button> 
                        </el-col>
                        <el-col :span="11" :offset="1"> 
                             <el-button class="btn" type="success" :loading="props.options.btnLoading"  @click="confirmClick">确认</el-button>
                        </el-col>
                    </el-row> 
                </div>
                </template> 
            </el-drawer>
    </div>
</template>
<script setup lang="ts">
import { ref ,defineProps,defineEmits,onMounted} from 'vue'

const direction = ref('btt')
const emit = defineEmits(['cancel','confirm']);
const props=defineProps({
    options:{
        type: Object,
        default:()=>{
            return{
                show: false,
                title: '',
                message:'',
                type:'',
                data:null,
                btnLoading:false 
            }
        }
    }
  });   
  const submitData=ref([]);
  
  onMounted(()=>{ 
    console.log("props",props.options.data);
    if(props.options.type=='SingleSelect'){
      let selectedOption =props.options.data.map((m:any)=>{
            if(m.remark){
                return m.key;
            }
        }); 
      if(selectedOption?.length>0)
         submitData.value=selectedOption[0];
    } 
  })

  const confirmClick=()=>{ 
     emit('confirm',submitData.value); 
  }

  const onCancelClick=()=>{
    emit('cancel');
 }
</script>

<style lang="scss" scoped>
@media screen and (max-width: 450px){
    :deep .el-drawer{
        border-radius: 15px 15px 0 0 !important;
 }  
 .drawer-content{
    padding:0 10px;
    h4{
        margin-top: -8%;
    }
    .btn-pos-1{ 
    padding-right:20px;
    position: fixed;bottom: 4%;
    width: 100%;
    .btn{
        width: 90%;
        height: 32px; 
    }
    }
    .btn-pos-2{
        padding-right:20px;
         position: fixed;bottom: 4%;
         width: 100%;
        .btn{
            width: 90%;
            height: 32px;
           
        }
    }
 }
}

@media screen and (min-width: 450px){
    :deep .el-drawer{
        border-radius: 15px 15px 0 0 !important;
        background-color: rgb(247, 252, 252)!important;
 } 

 .drawer-content{
    position: relative;
    padding:0 10px;  
    h4{
        margin-top: 0%;
    }
    .btn-pos-1{ 
    position: fixed;bottom: 4%;
    width: 100%;
    padding-right:20px;
        .btn{
            width: 90%;
            height: 35px; 
        }
    }
    .btn-pos-2{
         position: fixed;bottom: 4%;
         width: 100%;
         padding-right:20px;
        .btn{
            width: 90%;
            height: 35px;
           
        }
    }
 }
}

</style>