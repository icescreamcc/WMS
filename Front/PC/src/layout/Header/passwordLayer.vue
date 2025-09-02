<template>
  <Layer :layer="layer" @confirm="submit" ref="layerDom">
    <el-form :model="form" :rules="rules" ref="ruleForm" label-width="120px" style="margin-right:30px;">
      <el-form-item :label="$t('message.common.userName')+'：'" prop="name">
        <el-input v-model="form.name" readonly></el-input>
      </el-form-item>
      <el-form-item :label="$t('message.common.oldPassword')+'：'" prop="old">
        <el-input v-model="form.old" :placeholder="$t('message.common.inputOldPwd')" show-password></el-input>
      </el-form-item>
			<el-form-item :label="$t('message.common.newPassword')+'：'" prop="new">
			  <el-input v-model="form.new" :placeholder="$t('message.common.inputNewPwd')" show-password></el-input>
			</el-form-item>
    </el-form>
  </Layer>
</template>

<script lang="ts">
import type { LayerType } from '@/components/layer/index.vue'
import type { Ref } from 'vue' 
import { defineComponent, ref } from 'vue' 
import { useStore } from 'vuex'
import { updateUserPassword } from '@/api/system/user'
import Layer from '@/components/layer/index.vue'
import permission from '@/utils/system/permission'

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
          
        }
      }
    }
  },
  setup(props, ctx) { 
    const layerDom: Ref<LayerType|null> = ref(null)
    const store = useStore();
    const user=permission.getOperator()
    let form = ref({
      userId: user.userId,
      name: user.userName,
      old: '',
      new: ''
    })
    const rules = {
      old: [{ required: true, message: '请输入原密码', trigger: 'blur' }],
      new: [{ required: true, message: '请输入新密码', trigger: 'blur' }],
    }
    function submit() {
         let params = {
              userId: form.value.userId,
              oldPassword: form.value.old,
              newPassword: form.value.new
            }
            updateUserPassword(params)
            .then((res:any) => { 
              layerDom.value && layerDom.value.close()
              setTimeout(() => {
                store.dispatch('user/loginOut')
              }, 2000)
            })
    }
    return {
      form,
      rules,
      layerDom, 
      submit
    }
  }
})
</script>

<style lang="scss" scoped>
  
</style>