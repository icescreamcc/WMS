<template>
  <div class="full">
    <div class="left">
      <OrganizationTree @selectedNode="getNodeParams"/>
    </div>
    <div class="content">
      <UserTable :params="treeNodeParams"/>
    </div>
  </div>
</template>

<script lang="ts"> 
import { defineComponent, ref, provide, reactive } from 'vue'
import OrganizationTree from './organizationTree.vue'
import UserTable from './userTable.vue'
export default defineComponent({
  name: 'deptroles',
  components: {
    OrganizationTree,
    UserTable,
  },
  
  setup() {
    let active: any = ref({})
    provide('active', active)
    const treeNodeParams=reactive({
      id:'',
      type:''
    });
    let getNodeParams=(searchId:string,organizationType:string)=>{
      treeNodeParams.id=searchId;
      treeNodeParams.type=organizationType;
    }

    return{
      treeNodeParams,
      getNodeParams
     }
  }
})
</script>

<style lang="scss" scoped>
  .full {
    width: 100%;
    height: 100%;
    box-sizing: border-box;
    padding: 15px;
    display: flex;
    .left {
      width: 350px;
    }
    .content {
      flex: 1;
      width: calc(100% - 330px);
      height: 100%;
    }
  }
</style>