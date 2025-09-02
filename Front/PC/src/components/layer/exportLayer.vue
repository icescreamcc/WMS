<template>
  <Layer :layer="layer" @confirm="submit" > 
      <div class="list" ref="sortList">
          <el-checkbox-group v-model="fieldSelected" style="text-align: left;">
        <el-checkbox class="sort-chk field-chk"  v-for="field in fieldDatas" :key="field.key" :label="field.key" checked >{{field.value }}</el-checkbox>
      </el-checkbox-group>
    </div> 
  </Layer>
</template>

<script lang="ts">
import { defineComponent, onMounted, Ref, ref } from 'vue'
import Layer from '@/components/layer/index.vue'    
import msg from '@/utils/system/message'
//import Sortable, { CustomEvent } from 'sortablejs'
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
   let fieldDatas = ref(props.layer.data) 
   let fieldSelected=ref([]) 
   
   //拖拽
  //  let sortList: Ref<HTMLDivElement> = ref(null) as any
  //  let sortObj
  //  let sortOptions={
  //    draggable:'.sort-chk',
  //    group:'shared',
  //    handle:'.sort-chk',
  //    onEnd(){

  //    }
  //  }
  //  onMounted(() => {
  //    sortObj=Sortable.create(sortList.value,sortOptions)
  //     sortList.value = fieldDatas.value
  //     new Sortable(dom.value, {
  //       group: 'shared',
  //       animation: 150,
  //       ghostClass: 'blue-background-class',
  //       onEnd: function(evt: CustomEvent) {
  //         const pullMode = evt.pullMode
  //         const oldIndex = evt.oldIndex
  //         const newIndex = evt.newIndex
          
  //         let oldList = evt.target.list.children
  //         let newList = evt.to.list.children
  //         if (pullMode) { // 移动至toList并去除旧数据
  //           newList.splice(newIndex, 0, oldList[oldIndex])
  //           oldList.splice(oldIndex, 1)
  //         } else { // 同List位置修改
  //           const tem = oldList[oldIndex]
  //           oldList[oldIndex] = oldList[newIndex]
  //           oldList[newIndex] = tem
  //           console.log(oldList[0])
  //         }
  //       }
  //     })
  //   })
    //点击确认提交
   let submit=()=> {   
     if(fieldSelected.value.length>0){
       let submitData=Array<any>();
        fieldSelected.value .forEach(sel=>{
          submitData.push(fieldDatas.value.find((f:any)=>f.key==sel))
         }) 
       ctx.emit('dataSubmit', submitData) 
     }
     else{
       msg.warningAuto("请勾选要导出的字段");
     } 
    }
    return { 
      fieldDatas,
      fieldSelected,
      //sortList,
      submit
    }
  }
})
</script>

<style lang="scss" scoped>
   .field-chk{
     width: 100px;
     height: 35px;
   
   }
     .list{
      // width: 100%;
      padding: 0 0 0 10px;
      overflow-y: auto;
      flex: 1;
      height: auto;
      width: calc(100% - 10px);
    }
</style>