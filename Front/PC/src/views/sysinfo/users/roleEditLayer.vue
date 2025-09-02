<template>
  <Layer :layer="layer" @confirm="submit" > 
     <el-checkbox-group v-model="roleSelected" style="text-align: left;">
        <el-checkbox  v-for="role in roleDatas" :key="role.roleId" :label="role.roleId" :checked="role.isSelected" :disabled="!layer.showButton" class="role-chk">{{
            role.roleName
          }}</el-checkbox>
      </el-checkbox-group>
  </Layer>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import Layer from '@/components/layer/index.vue'    
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
          showButton: true ,
          data:[] 
        }
      }
    }
  },
  setup(props, ctx) { 
   let roleDatas = ref(props.layer.data) 
   let roleSelected=ref([])  
    //点击确认提交
   let submit=()=> {   
         ctx.emit('dataSubmit', roleSelected.value,roleDatas.value[0].userId) 
    }
    return { 
      roleDatas,
      roleSelected,
      submit
    }
  }
})
</script>

<style lang="scss" scoped>
   .role-chk{
     width: 100px;
     height: 35px;
   
   }
</style>