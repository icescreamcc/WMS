<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
        <el-button type="primary"  v-if="permission.isPermisstion('USERADD')" icon="el-icon-circle-plus-outline" @click="handleAdd">新增</el-button>
        <el-popconfirm  v-if="permission.isPermisstion('USERDEL')" title='确定删除选中的数据吗'  @confirm="handleDel(chooseData)">
          <template #reference>
            <el-button type="danger"  icon="el-icon-delete" :disabled="chooseData.length === 0">批量删除</el-button>
          </template>
        </el-popconfirm> 
      </div>
      <div class="layout-container-form-search">
        <el-input 
          v-model="query.input"
          placeholder="请输入关键词进行检索"
          size="small"
        ></el-input>
        <el-button
          type="primary"
          icon="el-icon-search"
          class="search-btn"
          @click="getTableData(true)"
          >搜索</el-button> 
          <el-button icon="el-icon-download" v-if="permission.isPermisstion('USEREXPORT')" style="margin-left:20px"  type="info" @click="getExportAllowField">导出</el-button>
      </div>
    </div>
    <div class="layout-container-table">
      <Table
        ref="table"
        v-model:page="page"
        v-loading="loading"
        :showSelection="true"
        :data="tableData"  
        @getTableData="getTableData"
        @selection-change="handleSelectionChange"
        @orderChanged="handleSortChange"
      >
      <el-table-column prop="userCode" label="工号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>
        <el-table-column prop="authAccount" label="UI编号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
        <el-table-column prop="domainName" label="域账户" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
        <el-table-column prop="cardId" label="卡号" align="center" sortable="custom" :show-overflow-tooltip="true"/> 
        <el-table-column prop="userName" label="中文名" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>
        <el-table-column prop="nickName" label="英文名" align="center" sortable="custom"  min-width="100" :show-overflow-tooltip="true"/> 
        <el-table-column prop="email" label="邮箱" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
        <!-- <el-table-column prop="phone" label="电话" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> -->
        <!-- <el-table-column prop="mobilePhone" label="手机号" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> -->
        <!-- <el-table-column prop="address" label="通讯地址" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>
        <el-table-column prop="provinceName" label="省份" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/>
        <el-table-column prop="cityName" label="城市" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> -->
        <el-table-column prop="createDate" label="创建时间" align="center" sortable="custom" min-width="100" :show-overflow-tooltip="true"/> 
        <el-table-column prop="deptName" label="部门" align="center" sortable="custom" min-width="80" :show-overflow-tooltip="true"/> 
        <el-table-column  label="角色" align="center" min-width="100">
         <template #default="scope"> 
            <el-button  @click="handleRoleEdit(scope.row)" type="primary" icon="el-icon-user" circle></el-button>
          </template>
        </el-table-column> 
        <el-table-column prop="isVaild" label="状态" align="center" sortable="custom" min-width="120">
          <template #default="scope">
            <span class="statusName">{{ scope.row.isVaild ==true ? "激活" : "冻结" }}</span>
            <el-switch
            v-if="permission.isPermisstion('USERUPDATESTATUS')"
              v-model="scope.row.isVaild"
              active-color="#13ce66"
              inactive-color="#ff4949"
              :active-value="true"
              :inactive-value="false"
              :loading="scope.row.loading"
              :disabled="true"
              @change="handleUpdateStatus(scope.row)"
            ></el-switch>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('message.common.handle')"
          align="center" 
          width="200"
           v-if="permission.isPermisstion('USERUPDATE','USERDEL')"
        >
          <template #default="scope">
            <el-button  v-if="permission.isPermisstion('USERUPDATE')" @click="handleEdit(scope.row)">{{
              $t("message.common.update")
            }}</el-button>
            <el-popconfirm
               v-if="permission.isPermisstion('USERDEL')"
              :title="$t('message.common.delTip')"
              @confirm="handleDel([scope.row])"
            >
              <template #reference>
                <el-button type="danger">{{ $t("message.common.del") }}</el-button>
              </template>
            </el-popconfirm>
          </template>
        </el-table-column>
      </Table>
      <UserEditModal :layer="ueLayer"  @dataSubmit="userDataSave" v-if="ueLayer.show" />
      <RoleEditModal :layer="reLayer"  @dataSubmit="roleDataSave" v-if="reLayer.show" />
      <ExportModal :layer="exLayer"  @dataSubmit="exportUser" v-if="exLayer.show" />
    </div>
  </div>
</template>

<script lang="ts"> 
import { defineComponent, ref, reactive } from "vue";
import { Page } from "@/components/table/type";
import { getUsers,getUserRoles, addUser,updateUserRole, updateUser,updateUserStatus,delUser,getAllowField,exportUsers,getUserDetail } from "@/api/system/user";
import { LayerInterface } from "@/components/layer/index.vue"; 
import Table from "@/components/table/tableServer.vue";
import UserEditModal from "./userEditLayer.vue";
import RoleEditModal from "./roleEditLayer.vue";
import ExportModal from "@/components/layer/exportLayer.vue"
import permission from '@/utils/system/permission'
export default defineComponent({
  name: "users",
  components: {
    Table,
    UserEditModal,
    RoleEditModal,
    ExportModal
  },
  setup() { 
    // 存储搜索用的数据
    let query = reactive({
      input: "",
    });

    //用户编辑弹窗控制器
    let ueLayer: LayerInterface = reactive({
      show: false,
      title: "",
      showButton: true,
      btnLoading:false,
      width:"45%",
      data:null,
      otherButton:{
                show:false, 
                text:"重置密码",
                type:"warning"
              }
    });

    //角色编辑弹窗控制器
    let reLayer: LayerInterface = reactive({
      show: false,
      title: "选择角色",
      showButton: true,
      btnLoading:false,
      width:"30%",
      data:null,
      otherButton:{
              show:false,
              text:"", 
              type:""
            }
    });

    //数据导出弹窗控制器
    let exLayer: LayerInterface = reactive({
      show: false,
      title: "选择导出字段",
      showButton: true,
      btnLoading:false,
      width:"30%",
      data:null,
      otherButton:{
              show:false,
              text:"", 
              type:""
            }
    });

    // 分页参数, 供table使用
    const page: Page = reactive({
      index: 1,
      size: 20,
      total: 0,
      orderField:'',
      orderType:''
    });
    let loading = ref(true);
    let tableData = ref([]);
    let chooseData = ref([]);
    let handleSelectionChange = (val: []) => {
      chooseData.value = val;
    };

    //排序事件
    let handleSortChange=(orderRow:any)=>{ 
      getTableData(true);
    }

    // 获取表格数据
    // params <init> Boolean ，默认为false，用于判断是否需要初始化分页
    let getTableData = (init: Boolean) => {
      loading.value = true
      if (init) {
        page.index = 1
      }  
      getUsers(page.size,page.index,page.orderField,page.orderType,query.input,permission.getOperator().userId)
        .then((res) => {
          let data = res.data.rows
          data.forEach((d: any) => {
            d.loading = false
          })
          tableData.value = data
          page.total = Number(res.data.total);
        })
        .catch((error) => {
          tableData.value = [];
          page.index = 1;
          page.total = 0;
        })
        .finally(() => {
          loading.value = false;
        });
    }

     // 删除功能
    let handleDel = (data: any[]) => {  
      let ids=Array<any>();
      data.forEach(d=>{
        ids.push(d.userId);
      }) 
      delUser(ids).then((res) => { 
        getTableData(tableData.value.length === 1 ? true : false);
      });
    }

    // 新增弹窗功能
    let handleAdd = () => {
      ueLayer.title = "新增用户";
      ueLayer.show = true;
      ueLayer.otherButton.show=false;
      delete ueLayer.row;
    }

    // 编辑弹窗功能
    let handleEdit = (row: any) => { 
      getUserDetail(row.userId).then(res=>{ 
         ueLayer.title = "编辑用户";
         ueLayer.row = row;
         ueLayer.otherButton.show=permission.isPermisstion('USERINITPASSWORD');
         ueLayer.show = true;
         ueLayer.data=res.data;
      }); 
    }

    //角色编辑
    let handleRoleEdit=(row:any)=>{
        getUserRoles(row.userId).then(res=>{ 
          reLayer.row = row; 
          reLayer.show = true;
          reLayer.showButton=permission.isPermisstion('USERUPDATEROLE')
          reLayer.data=res.data;
        })
    }
  
    //角色编辑提交
    let roleDataSave=(data:any,userId:string)=>{ 
        reLayer.btnLoading=true;
        updateUserRole(userId,data).then(res=>{
            reLayer.show = false;
        }).finally(()=>{ reLayer.btnLoading=false;});
    }

    // 状态编辑功能
    let handleUpdateStatus = (row: any) => { 
      row.loading = true 
      updateUserStatus(row.userId,row.isVaild)
      .then(res=>{
       
      })
      .catch(err=>{ 
          row.isVaild=!row.isVaild
      })
      .finally(() => { 
        row.loading = false
      })
    }

    //新增或编辑数据提交
    let userDataSave=(data:any,actionType:string)=>{ 
      ueLayer.btnLoading=true;
        if(actionType=='add'){
          addUser(data).then(res=>{
            ueLayer.show = false;
            getTableData(true);
          }).finally(()=> ueLayer.btnLoading=false);
        }
        else{
           updateUser(data).then(res=>{
            ueLayer.show = false;
            getTableData(false);
          }).finally(()=> ueLayer.btnLoading=false);
        }
    }
    
    //获取被允许导出的字段列表
    let getExportAllowField=()=>{
      getAllowField(permission.getOperator().userId).then(res=>{
        exLayer.row = permission.getOperator().userId; 
        exLayer.show = true;
        exLayer.data=res.data; 
      }) 
    }

   //导出用户
    let exportUser=(selField:Array<any>)=>{ 
        exportUsers(query.input,page.orderField,page.orderType,selField).then(res=>{ 
          let link = document.createElement('a') 
            link.style.display = 'none'
            link.href =res.data
            document.body.appendChild(link)
            link.click() 
            document.body.removeChild(link)
        })
    }
    getTableData(true)
    return {
      permission,
      query, 
      tableData,
      chooseData,
      loading,
      page, 
      ueLayer,
      reLayer,
      exLayer,
      handleSelectionChange,
      handleSortChange,
      getTableData,
      handleDel,
      handleAdd,
      handleEdit,
      handleRoleEdit,
      handleUpdateStatus,
      userDataSave,
      roleDataSave,
      getExportAllowField,
      exportUser
    };
  }
});
</script>

<style lang="scss" scoped>
.statusName {
  margin-right: 10px;
}
</style>
