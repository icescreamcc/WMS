<template>
    <Layer :layer="layer"> 
          <div style="max-height: 500px; overflow-y: scroll;">
            <el-timeline style="max-width: 600px">
                <el-timeline-item v-for="(his,index) in approvalHis" center size="large" :timestamp="commonHelper.formatToDateTime(his.approvalDate)" :icon="his.icon" :color="his.color" placement="center">
                    <el-card style="text-align: left;">
                        <el-row>
                            <el-col :span="2"><img src="../../../public/icon-img/renyuan.png" height="18"></el-col>
                            <el-col :span="22">
                                <div style="padding: 3px 0;font-size: 13px;"> <span>{{ his.approverName }}</span> <span>（{{ his.approverRoleName }}）</span></div> 
                            </el-col>
                        </el-row>
                        <el-row style="margin-top: 10px;">
                            <el-col :span="2"><img src="../../../public/icon-img/jieguobijiao.png" height="18"></el-col>
                            <el-col :span="22">
                                <div :style="{color:his.color}" style="padding: 3px 0;font-size: 13px;">{{ his.approvalStatusDesc }}</div>
                            </el-col>
                        </el-row>
                        <el-row style="margin-top: 10px;margin-bottom: 15px;">
                            <el-col :span="2"><img src="../../../public/icon-img/tiaochajieguo.png" height="18"></el-col>
                            <el-col :span="22">
                                <div style="border-top: 1px solid #ececec;padding: 5px 0;font-size: 13px;color: #888;">{{ his.opinion }}</div>
                            </el-col>
                        </el-row>   
                    </el-card>
                </el-timeline-item> 
            </el-timeline>
          </div>
    </Layer>
  </template>
  
  <script lang="ts">
  import { defineComponent, onMounted,ref } from 'vue'
  import Layer from '@/components/layer/index.vue'     
  import commonHelper from "@/utils/system/common-helper";
  import { MoreFilled } from '@element-plus/icons-vue'
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
            showButton: false ,
            data:null ,
            type:''
          }
        }
      }
    },
    setup(props, ctx) {   
    const approvalHis=props.layer.data||[]; 
    onMounted(()=>{
        for(let i=0;i<approvalHis.length;i++){
            if(i==0){
                approvalHis[i].color="#67c23a";
                approvalHis[i].icon="MoreFilled"
            }
            else{
                if(approvalHis[i].approvalStatus=='Reject'){
                    approvalHis[i].color="#f56c6c";
                }
                else{
                    approvalHis[i].color="#409eff";
                }
            }
        }
    })
      return {  
        approvalHis,
        commonHelper 
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