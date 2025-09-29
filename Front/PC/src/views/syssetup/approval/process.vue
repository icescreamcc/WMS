<template>
  <div class="layout-container">
    <div class="layout-container-form flex space-between">
      <div class="layout-container-form-handle">
           <h2>审批流程设置</h2>
      </div>  
            <div class="layout-container-form-search"> 
          <el-button  icon="el-icon-check" v-if="permission.isPermisstion('APPROVALUPDATE')"  style="margin-right:10px" type="primary" :loading="btnLoading" @click="submit">保存</el-button>
         </div>
    </div> 
    <div class="layout-container-table" style="margin-top:0px">
        <el-scrollbar height="750px"> 
              <el-card shadow="hover" class="content-approval-model">  
                  <el-row>
                    <el-col :span="4" class="text-left">
                        <el-badge :value="1" type="danger">
                           <h2 class="text-deft">选择审批模式</h2>
                        </el-badge> 
                    </el-col>
                    <el-col :span="6" class="text-left">
                         <el-radio-group v-model="curApprovalModel" @change="approvalModelChanged">
                        <el-radio-button v-for="item in approvalModelData" :key="item.key" :label="item.key"  >{{item.value}}</el-radio-button> 
                      </el-radio-group> 
                    </el-col>
                    <el-col :span="14" class="text-left">
                         <p class="text-deft approval-model-desc" v-if="curApprovalModel=='Any'">仅需要一步审批,在指定的任意角色人员审批后则审批流程结束</p>
                         <p class="text-deft approval-model-desc" v-else-if="curApprovalModel=='Process'">按设定的流程审批,每步流程可以指定多个角色任意审批</p>
                    </el-col>
                  </el-row>
               </el-card> 
                 <el-card shadow="hover" class="content-approval-step">  
                  <el-row>
                    <el-col :span="4" class="text-left">
                        <el-badge :value="2" type="danger">
                           <h2 class="text-deft">选择审批步骤</h2>
                        </el-badge> 
                    </el-col>
                    <el-col :span="20" class="text-left">
                         <div class="step-item" v-for="(item,index) in stepArray">
                           <el-button circle  @click="selectStep(item.value)"  :type="item.selected?'warning':'primary'"><span class="step-item-val">{{item.value}}</span></el-button>  
                           <div class="step-item-close" v-if="index>0" title="删除当前及后续审批流程" @click="removeStep(item.value)"><p>x</p></div>
                         </div>
                         <div class="step-item">
                           <el-button circle  @click="addStep()" title="添加审批流程" v-if="curApprovalModel=='Process'"><span class="step-item-val">+</span></el-button>   
                         </div>
                    </el-col>
                  </el-row>
               </el-card>
                <el-card shadow="hover" class="content-approval-role">
                  <el-row>
                    <el-col :span="4" class="text-left">
                       <el-badge :value="3" type="danger">
                              <h2 class="text-deft">选择审批角色</h2>
                        </el-badge>  
                    </el-col>
                    <el-col :span="20" class="text-left">
                        <el-row>
                     <el-col :span="3" v-for="item in roleData" :key="item.roleId" style="text-align:left;margin-bottom:10px">
                       <el-button :disabled="btnRolesDisabled"  style="width:95%;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;" round :type="item.selected?'primary':''" :title="item.roleName" @click="selectRoles(item)">{{item.roleName}}</el-button>
                     </el-col>
                  </el-row>
                    </el-col>
                  </el-row> 
                </el-card> 
               <el-card shadow="hover" class="content-approval-process">
                   <el-row>
                    <el-col :span="4" class="text-left">
                      <el-badge :value="4" type="danger">
                           <h2 class="text-deft">设置审批流程</h2>
                        </el-badge>   
                    </el-col>
                    <el-col :span="20" >
                      <div class="process-non" v-if="processShowScheme=='NoData'">
                          <span>未设置审批流程</span>
                      </div>
                      <div class="process-process" v-if="processShowScheme=='Process'">
                          <el-row>
                            <el-col :span="3" v-for="process in processData" :key="process.rank">
                             <div class="process-any-tag1" :class="process.selected?'approval-warning':'approval-primary'" @click="selectStep(process.rank)"><p>{{process?.rank}}</p></div>
                              <p class="process-any-title1" :class="process.selected?'text-warning':'text-primary'">{{approvalModelData.filter(x=>x.key==curApprovalModel)[0]?.value}}</p>
                              <p class="process-any-desc1"  :class="process.selected?'text-warning':'text-primary'"> 
                                 <span  v-for="(role,index) in process?.roles" :key="role?.approverRole">
                                   {{role?.approverRoleName}}
                                   <span v-if="index<process?.roles?.length-1">,</span>
                                  </span>
                              </p>
                              </el-col>
                          </el-row>  
                      </div>
                      <div class="process-any" v-if="processShowScheme=='Any'">
                        <el-row>
                           <el-col :span="3" v-for="role in processData[0]?.roles" :key="role">
                               <div class="process-any-tag approval-primary iconfont ionfont-md icon-zhengquewancheng" ></div>
                               <p class="process-any-title text-primary">{{approvalModelData.filter(x=>x.key==curApprovalModel)[0]?.value}}</p>
                               <p class="process-any-desc text-primary" >{{role?.approverRoleName}}</p>
                           </el-col>
                        </el-row>
                      </div>
                    </el-col>
                  </el-row> 
               </el-card> 
       </el-scrollbar>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, reactive, inject, watch } from 'vue' 
import{getRoles,getApprovalModels,getProcess, updateProcess} from '@/api/system/approval' 
import permission from '@/utils/system/permission'
import msg from'@/utils/system/message'
import { title } from 'process';
export default defineComponent({ 
  setup() {  
    let activeCategory: any = inject('active')
    let btnLoading = ref(false)      
    let roleData=ref(new Array<any>())
    let processData=ref(new Array<any>())
    let approvalModelData=ref(new Array<any>())
    let curApprovalModel=ref('') 
    let processShowScheme=ref('NoData') 
    let btnRolesDisabled=ref(true)
    let stepArray=ref([{value:1,selected:true}])
    let curStep=ref(1)
    let processModel={
        dataType:activeCategory.value.id, 
        approvalModel:curApprovalModel.value,
        rank:0,
        isLastApproval:true,
        selected:true,
        roles:[{
               approverRole:'',
               approverRoleName:'',
           }]
        }
//获取角色信息
  getRoles().then(res=>{
    roleData.value=res.data
    setRolesBtnStyle()
  })

  //设置角色选择属性
  let setRolesBtnStyle=()=>{
     if(processData.value.length>0){
       roleData.value.forEach(x=>{
            x.selected=false
            processData.value.forEach((p:any)=>{
              p.roles.forEach((r:any)=>{
                 if(x.roleId==r.approverRole){
                  x.selected=true
                }
              }) 
            })
          })
    }
    else{
      roleData.value.forEach(x=>x.selected=false)
    }
  }

  //获取审批模式
  getApprovalModels().then(res=>{
    approvalModelData.value=res.data
  })

//根据数据类型获取审批流程
  let getProcessData=(approvalDataType:string)=>{ 
      btnRolesDisabled.value=true
      processData.value=new Array<any>()
     getProcess(approvalDataType).then(res=>{ 
       if(res.data?.length>0){
         //从数据中获取审批模式
          curApprovalModel.value=res.data[0].approvalModel 
         //从数据中获取审批步骤
         let stepArrayTemp=new Array<number>()
          res.data.forEach((x:any)=>{ 
            if(stepArrayTemp.indexOf(x.rank)<0){
              stepArrayTemp.push(x.rank)
            }
          })
          //整理审批步骤业务模型
          stepArray.value=new Array<any>()
          stepArrayTemp.forEach(x=>{  stepArray.value.push({value:x,selected:false})})
          stepArray.value[stepArray.value.length-1].selected=true
          curStep.value=stepArray.value[stepArray.value.length-1].value
          //整理审批角色业务模型
          btnRolesDisabled.value=false
          roleData.value.forEach(x=>{
            x.selected=false
            res.data.forEach((y:any)=>{
              if(x.roleId==y.approverRole){
                 x.selected=true
              }
            })
          })  
          //整理审批流业务模型 
          stepArrayTemp.forEach(x=>{  
            let process={
              rank:x,
              selected:x==curStep.value?true:false,
              dataType:activeCategory.value.id,
              approvalModel:curApprovalModel.value,
              isLastApproval:false,
              roles:new Array<any>()
            } 
            res.data.forEach((y:any)=>{ 
              if(x==y.rank){
                process.isLastApproval=y.isLastApproval
                process.roles.push({
                    approverRole:y.approverRole,
                    approverRoleName:y.approverRoleName
                    })
                
              }
            }) 
            processData.value.push(process)
          }) 
       }
       else{ 
          stepArray.value=[{value:1,selected:false}]
          curStep.value=1
          roleData.value.forEach(x=>x.selected=false) 
       }    
      setApprovalScheme()
     })
  }

  //审批步骤显示方案
  let setApprovalScheme=()=>{
    if(processData.value.length==0){
      processShowScheme.value='NoData'
    }
    else {
      processShowScheme.value=curApprovalModel.value
    } 
  } 
 

  //审批模式选择
  let approvalModelChanged=(val:string)=>{
    btnRolesDisabled.value=false
    stepArray.value=[{value:1,selected:true}]
    processModel.dataType=activeCategory.value.id
    processModel.approvalModel=val
    processModel.isLastApproval=true 
    processModel.roles=new Array<any>()  
    processModel.rank=1
    curStep.value=1
    roleData.value.forEach(x => {x.selected=false});
    processData.value=[processModel] 
    setApprovalScheme() 
  }

  //添加审批步骤
  let addStep=()=>{
    stepArray.value.forEach(x=>x.selected=false)
    stepArray.value.push({value:stepArray.value.length+1,selected:true})
    curStep.value=stepArray.value.length
    processData.value.forEach(x=>{x.selected=false;x.isLastApproval=false})
    processData.value.push({
              rank:curStep.value,
              selected:true,
              dataType:activeCategory.value.id,
              approvalModel:curApprovalModel.value,
              isLastApproval:true,
              roles:new Array<any>()
            } ) 
  }

  //选择审批步骤
  let selectStep=(step:number)=>{
      stepArray.value.forEach(x=>{ 
        x.selected=false
        if(x.value==step){
          x.selected=true
        }
      })  
      processData.value.forEach(x=>{
        x.selected=false
        if(x.rank==step){
          x.selected=true
        }
      })
      curStep.value=step
  }

  //删除步骤
  let removeStep=(step:number)=>{
    stepArray.value= stepArray.value.filter(x=>x.value<step)
    stepArray.value.forEach(x=>{x.selected=false})
    stepArray.value[stepArray.value.length-1].selected=true
    processData.value=processData.value.filter(x=>x.rank<step)
    processData.value.forEach(x=>{x.selected=false;x.isLastApproval=false})
    processData.value[processData.value.length-1].selected=true
    processData.value[processData.value.length-1].isLastApproval=true
    curStep.value=stepArray.value[stepArray.value.length-1].value
    setRolesBtnStyle() 
  }

  //选择角色 
  let selectRoles=(role:any)=>{
    let isAdd=false
    roleData.value.forEach(r=>{
      if(r.roleId==role.roleId){
        r.selected=!r.selected
        if(r.selected){
          isAdd=true
        }
      }
    })
    if(isAdd){  
      processData.value.forEach(x=>{
        if(x.rank==curStep.value){
          x.roles.push({
                    approverRole:role.roleId,
                    approverRoleName:role.roleName
                    })
        }
     }) 
    }
    else{
      processData.value.forEach(x=>{
         x.roles=x.roles.filter((r:any)=>r.approverRole!=role.roleId)
     }) 
    } 
     setApprovalScheme()
  }
    
  //监测审批数据类型的选择事件
    watch(activeCategory, (newVal) => {  
      curApprovalModel.value=''
      getProcessData(activeCategory.value.id)
    })  
 

let submit=()=>{
  if(processData.value.length==0){
    msg.warningAuto("未设置任何审批流程")
    return
  }
  else{
    for(let process of processData.value){
      if(process.roles?.length==0){
        msg.warningAuto("请给每个审批流程指定审批角色")
        return
      }
    }
  }
  let submitData=new Array<any>() 
  processData.value.forEach(x=>{
    x.roles.forEach((r:any)=>{
      let submitModel={
        dataType:x.dataType, 
        approvalModel:x.approvalModel,
        rank:x.rank,
        isLastApproval:x.isLastApproval,
        approverRole:r.approverRole,
        approverRoleName:r.approverRoleName,
    }
    submitData.push(submitModel)
    })
  }) 
  btnLoading.value=true    
  updateProcess(submitData).then().finally(()=>btnLoading.value=false)
}

    return {    
      permission, 
      processShowScheme, 
      btnLoading,  
      btnRolesDisabled,
      stepArray,
      curStep,
      roleData,
      processData,
      approvalModelData,
      curApprovalModel,
      addStep,
      selectStep,
      removeStep,
      getProcessData,
      approvalModelChanged,
      selectRoles,
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
    .text-left{
      text-align:left;
    }
    .content-approval-model{ 
      margin-bottom: 10px;
      border-left: 5px solid rgb(78, 115, 223);
      .approval-model-desc{
        font-size: 13px;
      }
    }
    .content-approval-step{
       margin-bottom: 10px;
      border-left: 5px solid rgb(54, 185, 204);
      .step-item{
        float: left;
        margin-right: 25px;
        position: relative;
        .step-item-val{
          margin:0 3.1px
         }
        .step-item-close{
          cursor:pointer;
          color: #fff; 
          background-color: #f56c6c;
          height: 13px;
          width: 13px;
          border-radius: 10px;  
          position: relative;
          top:-43px;
          right: -18px;
          p{
            font-size: xx-small;
            position: relative;
            bottom:-3%;
            left: 30%;
          }
        }
      } 
    }
    .content-approval-role{
     margin-bottom: 10px;
     border-left: 5px solid rgb(28, 200, 138);
    }
    .content-approval-process{
      margin-bottom: 10px;
      border-left: 5px solid rgb(246, 194, 62);
      .process-non{
        span{
          color:#b3b3b3
        }
      }
      .approval-primary{
         color: #409eff;
         border: 2px solid #409eff;
      }
      .approval-warning{
         color: #e6a23c;
         border: 2px solid #e6a23c;
      }
      .process-process{ 
        .process-any-tag1{ 
          cursor: pointer;
          height: 23px;
          width: 23px;
          border-radius: 15px; 
          position: relative;
          left: 40%;
          p{
            position: relative;
            bottom:50%
          }
        }
        .process-any-title1{
          position: relative; 
          top:-10px;
           text-align: center;
        }
        .process-any-desc1{
           position: relative;
           top:-18px;
           text-align: center;
           font-size: 11px;
        }
      }
      .process-any{
        .process-any-tag{ 
          cursor: pointer;
          height: 23px;
          width: 23px;
          border-radius: 15px; 
          position: relative;
          left: 40%;
        }
        .process-any-title{
          position: relative; 
          top:-10px;
           text-align: center;
        }
        .process-any-desc{
           position: relative;
           top:-18px;
           text-align: center;
           font-size: 11px;
        }
      }
    }
  }
</style>