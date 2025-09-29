<template>
  <div v-drag>
    <el-dialog
      ref="dialog"
      v-model="layer.show"
      :title="layer.title"
      :width="layer.width"
      :close-on-click-modal="false">
    <hr v-if="layer.title" class="dialog-decollator-top"/>
      <slot></slot>
     <!-- <hr class="dialog-decollator-bottom"/> -->
      <template #footer  >
        <div style="text-align:right;margin-top:-10px" >
            <el-button auto-insert-space @click="close" size="small">取消</el-button>
           <el-button auto-insert-space v-if="layer.otherButton.show"  :type="layer.otherButton.type" @click="otherEvent(layer.otherButton.text)" :loading="getOtherBtnStatus()" size="small">{{layer.otherButton.text}}</el-button>
          <el-button auto-insert-space v-if="layer.showButton" type="primary" @click="confirm" size="small" :loading="getSubmitBtnStatus()">确认</el-button>
        </div>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts">
import type { Ref } from 'vue'
import { defineComponent, ref, watch } from 'vue'
import drag from '@/directive/drag/index'
import { UseDialogProps } from 'element-plus/lib/el-dialog/src/dialog'
export interface LayerInterface {
  show: boolean;
  title: string;
  showButton?: boolean;
  btnLoading:boolean;
  width?: string;
  [propName: string]: any; 
}
interface SystemDialogProps extends UseDialogProps {
  handleClose: Function
}
export interface LayerType {
  close: Function
}
export default defineComponent({
  props: {
    layer: {
      type: Object,
      default: () => {
        return {
          show: false,
          title: '',
          showButton: false,
          btnLoading:false,
          type:'',
          otherButton:{
                show:false,
                otherBtnLoading:false,
                text:"",
                type:""
              }
        }
      },
      required: true
    }
  }, 
  directives: {
    drag
  },
  setup(props, ctx) { 
    const dialog: Ref<SystemDialogProps> = ref(null) as any  
    const getSubmitBtnStatus =()=>{
      return props.layer.btnLoading
    };
    const getOtherBtnStatus=()=>{
      return props.layer.otherButton.otherBtnLoading;
    }
    function confirm() {
        ctx.emit('confirm')  
    }
    function close() {
      dialog.value.handleClose()
    }
    function otherEvent(type:any){
      ctx.emit('otherEvent',type)
    }
    return {
      dialog,
      confirm,
      close,
      otherEvent,
      getSubmitBtnStatus,
      getOtherBtnStatus
    }
  } 
})
</script>

<style lang="scss" scoped> 
   .dialog-decollator-top{
     border: 1px dashed rgb(243, 239, 239);
     margin-top: -25px;
     margin-bottom: 10px;
   }
    .dialog-decollator-bottom{
     border: 1px solid rgb(243, 239, 239);
     margin-bottom: -5px;
   }
</style>