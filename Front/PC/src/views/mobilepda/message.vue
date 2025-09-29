<template>
    <div class="app-content">
        <div  class="content-list">
            <div v-if="dataList?.length>0" v-for="item in dataList"  class="list-item" @click="onSetRead($event,item)">
                <div class="item-title">  
                    <img v-if="item.isRead" :src="isReadImg" alt="" height="15"> 
                    <img v-else :src="msgImg" alt="" height="15"> 
                    {{ item.messageType }}
                </div>
                    <div class="item-content">{{ item.content }}</div> 
                    <div class="item-content text-deft">{{ item.remark }}</div>
                    <div class="item-content text-deft">{{commonHelper.formatToDateTime(item.createDate)  }}</div>
            </div>
            <div v-else>
                <el-empty  description="没有任何数据" /> 
            </div>
        </div> 
        <div class="content-footer">
           <el-row :gutter="2" >
            <el-col :span="12" ><div @click="onPrePage" class="footer-btn" :class="pgIndex==1?'disabled-btn':''">上一页</div></el-col>
            <el-col :span="12" ><div @click="onNextPage" class="footer-btn" :class="pgIndex==totalPage?'disabled-btn':''">下一页</div></el-col>
           </el-row>
        </div>
    </div> 
</template>

<script lang="ts" setup>
import {  ref ,nextTick,onMounted,onBeforeUnmount} from "vue";
import { useI18n } from 'vue-i18n'    
import commonHelper from '@/utils/system/common-helper';
import {getMessages,setRead} from '@/api/mobile-pda/message'; 
import permission from '@/utils/system/permission';
 
const { t } = useI18n();
const msgImg='/public/icon-img/xiaoxi4.png';
const isReadImg='/public/icon-img/yidu1.png';
const dataList=ref();
const pgSize=ref(4);
const pgIndex=ref(1);
const totalDate=ref(0);
const totalPage=ref(0);
const isReadMsgIdArr=ref(new Array<any>());
const user=permission.getOperator();    

getMessages(pgSize.value,pgIndex.value,user.userId).then(res=>{
    dataList.value=res.data.rows;
    totalDate.value=res.data.total;
    totalPage.value=Math.ceil(totalDate.value/pgSize.value);
    let idArr=dataList.value.filter((f:any)=>!f.isRead).map((item:any)=>item.messageId);
    isReadMsgIdArr.value=idArr; 
})

const onPrePage=()=>{
    if(pgIndex.value>1){
        pgIndex.value--; 
        getMessages(pgSize.value,pgIndex.value,user.userId).then(res=>{ 
            dataList.value=res.data.rows;
            totalDate.value=res.data.total;
            totalPage.value=Math.ceil(totalDate.value/pgSize.value); 
        })
    }
}

const onNextPage=()=>{
    if(pgIndex.value<totalPage.value){
        pgIndex.value++; 
        getMessages(pgSize.value,pgIndex.value,user.userId).then(res=>{ 
            dataList.value=res.data.rows;
            totalDate.value=res.data.total;
            totalPage.value=Math.ceil(totalDate.value/pgSize.value);
            let idArr=dataList.value.filter((f:any)=>!f.isRead).map((item:any)=>item.messageId);
            isReadMsgIdArr.value.push(...idArr); 
        })
    }
}

const onSetRead=(e:any,item:any)=>{ 
    setRead([item.messageId],user.userId).then(()=>{
        getMessages(pgSize.value,pgIndex.value,user.userId).then(res=>{ 
            dataList.value=res.data.rows;
            totalDate.value=res.data.total;
            totalPage.value=Math.ceil(totalDate.value/pgSize.value); 
        }) 
    })
}

onBeforeUnmount(()=>{ 
    isReadMsgIdArr.value=[...new Set(isReadMsgIdArr.value)]; 
    setRead(isReadMsgIdArr.value,user.userId).then(()=>{ 
    })
})
    
</script>

<style lang="scss" scoped>
 .app-content{
        height: 87%;
        background-color: #fff; 
        .content-list {
            height: 83%;  
            list-style: none;
            overflow-y: scroll;
            overflow-x: hidden;
            padding: 4% 4% 4% 6%;
       
            .list-item{
                margin-bottom: 7px;
                background-color: rgb(242, 242, 242);
                border-radius: 5px; 
                .item-title{
                    text-align: left;
                    padding:8px 10px;
                    font-size: 14px;
                    font-weight: 600;
                }
                .item-content{
                    padding: 8px;
                    text-align: left;
                    margin-left: 20px;
                    margin-top: -10px;
                    font-size: 12px;
                }
            }
        } 
        .content-footer{ 
            font-size: 13px;
            padding: 4% 6% ;
            color: #888; 
            .footer-btn{
                background-color: rgb(242, 242, 242);
                padding: 4px 0;
                cursor: pointer;  
            }
            .disabled-btn{
                color: #d6d6d6
            }
        }
    }
</style>