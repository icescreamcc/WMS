<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
           <h2>权限列表</h2>
      </div> 
            <div class="layout-container-form-search"> 
          <el-button  icon="el-icon-check" v-if="!chkDisabled&&permission.isPermisstion('PERMISSIONUPDATE')"  style="margin-right:10px" type="primary" :loading="btnLoading" @click="submit">保存</el-button>
         </div>
    </div>
    <div class="layout-container-table" style="margin-top:-10px">
        <el-scrollbar height="750px"> 
               <el-descriptions  title="" direction="vertical" :column="3" size="small" border>  
                    <div v-for="item in menuData"  :key="item.menuId">   
                      <el-descriptions-item >{{item.menuName}}</el-descriptions-item>
                      <el-descriptions-item >
                        <el-row>
                        <el-col :span="4"  v-for="childItem in item.menuChildren" :key="childItem.menuId">
                           <el-checkbox @change="singleCheck(item,$event)" :disabled="chkDisabled" v-model="childItem.isAuth" :checked="childItem.isAuth||item.isAuth"> {{childItem.menuName}}</el-checkbox> 
                          </el-col>  
                        </el-row>
                        </el-descriptions-item> 
                          <el-descriptions-item >
                            <el-checkbox @change="allCheck(item,$event)" :disabled="chkDisabled" v-model="item.isAuth" :checked="item.isAuth">全选</el-checkbox> 
                          </el-descriptions-item> 
                    </div>  
                </el-descriptions>

       </el-scrollbar>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, reactive, inject, watch } from 'vue' 
import{getPermissionMenus,updatePermissions} from '@/api/system/userPermission'
import permission from '@/utils/system/permission'
export default defineComponent({ 
  setup() {  
    const activeCategory: any = inject('active')
    const btnLoading = ref(false)    
    const menuData=ref() 
    const chkDisabled=ref(true)

    const getMenuData=(roleId:any=null)=>{ 
    getPermissionMenus(roleId).then((res:any)=>{  
      let menuList:Array<any>=[]
      res.data.forEach((m:any) => {
        if(m.menuChildren?.length>0){
          let curChild=m.menuChildren[0];
          if(curChild.menuType=='Menu'){
            m.menuChildren.forEach((c:any) => {
            c.menuName=`【${m.menuName}】${c.menuName}`
             menuList.push(c) 
          });
          }
         else if(curChild.menuType=='Action'){
          menuList.push(m) 
         }
        } 
      });  
      menuData.value=menuList  
    })
  }
    
    watch(activeCategory, (newVal) => { 
      if(activeCategory.value.type=="Role"){
        chkDisabled.value=false
      }
      else{
        chkDisabled.value=true;
      }
      getMenuData(activeCategory.value.id)
    }) 

const singleCheck=(row:any,chk:boolean)=>{ 
    let chkAll=true
    let noRead=true
      row.menuChildren.forEach((c:any)=>{
        if(c.menuId.indexOf('READ')>=0&&!c.isAuth){
           chkAll=false
           noRead=false
        }
        if(!c.isAuth){
          chkAll=false
        } 
        c.isAuth=noRead?c.isAuth:false
        })
        row.isAuth=chkAll
}

 const allCheck=(row:any,chk:boolean)=>{ 
      if(row.menuChildren?.length>0){
        row.menuChildren.forEach((c:any)=>{
          c.isAuth=chk
        })
      }
   }

const submit=()=>{
  btnLoading.value=true 
  updatePermissions(activeCategory.value.id,menuData.value).then().finally(()=>btnLoading.value=false)
}

    return {    
      permission,
      menuData,
      chkDisabled,
      btnLoading,
      singleCheck,
      allCheck,
      submit
    }
  }
})
</script>

<style lang="scss" scoped>
  .layout-container {
    height: 100%;
    margin: 0 0 0 10px;
    width: calc(100% - 10px);
    h2 {
      padding: 0;
      margin: 0;
      margin-right: 20px;
      font-size: 14px;
      display: -webkit-box;
      -webkit-line-clamp: 1;
      -webkit-box-orient: vertical;
      overflow: hidden;
      height: 30px;
      line-height: 30px;
    }
  }
</style>