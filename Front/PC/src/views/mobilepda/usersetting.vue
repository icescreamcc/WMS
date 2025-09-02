<template>
    <div class="app-content">
        <div class="content-header">
           <img src="../../assets/images/avatar.jpeg" height="70">
           <p>{{ user.userName }}.PS</p> 
        </div> 
        <div class="content-body">
            <el-row class="body-item" @click="onShowMessageSubscribe" v-if="permission.isPermisstion('MESSAGESUBSCRIBE')">
                <el-col class="item-text" :span="21" :offset="1">消息订阅</el-col>
                <el-col class="item-icon" :span="2"><el-icon><Message /></el-icon></el-col>
            </el-row>
            <el-row class="body-item" @click="onLoginOut">
                <el-col class="item-text" :span="21" :offset="1">退出登录</el-col>
                <el-col class="item-icon" :span="2"><el-icon><SwitchButton /></el-icon></el-col>
            </el-row>
        </div>
        <MessageDrawer :options="messageTypes" @cancel="onSettingCancel" @confirm="onSetingConfirm"/>
    </div> 
</template>

<script lang="ts" setup>
import {ref ,nextTick,onMounted} from "vue";
import { useI18n } from 'vue-i18n'    
import commonHelper from '@/utils/system/common-helper';
import {getSubscribeMessageType,submitSubscribeMessageType} from '@/api/mobile-pda/message'; 
import{loginOut} from '@/api/system/user';
import permission from '@/utils/system/permission';
import {ArrowRightBold,Message,SwitchButton} from '@element-plus/icons-vue';  
import { useRouter } from 'vue-router';
import MessageDrawer from './message-drawer.vue'

const { t } = useI18n(); 
const user=permission.getOperator(); 
const router = useRouter(); 
const messageTypes=ref({
    show:false,
    title:'',
    message:'',
    type:'',
    data:null
});

onMounted(()=>{ 
   
})

const onLoginOut=()=>{
    loginOut(user.userId).then(()=>router.push('/login')); 
}

const onShowMessageSubscribe=()=>{
    getSubscribeMessageType(user.userId).then(res=>{ 
        messageTypes.value.data=res.data;
        messageTypes.value.title="选择需要订阅的消息类型";
        messageTypes.value.type="MultipleSelect";
        messageTypes.value.show=true;
    })
}

const onSettingCancel=()=>{
    messageTypes.value.show=false;
}

const onSetingConfirm=(data:Array<string>)=>{
    console.log(data)
    submitSubscribeMessageType(data,user.userId).then(()=>messageTypes.value.show=false); 
}
    
</script>

<style lang="scss" scoped>
@media screen and (max-width: 450px){
    .app-content{
        height: 87%;
        background-color: #fff; 
        .content-header{
            height: 30%;
            background-image: url('../../assets/images/setting-header3.jpg');
            background-size: cover;
            background-position: center;
            background-repeat: no-repeat;
            opacity: calc(.85);  
            box-shadow: 0 0 70px 30px rgb(249, 249, 249) inset;
            img{
                position: relative;
                top: 30px;
            }
            p{
                position: relative;
                top: 17px; 
                text-align: center;  
                color: #fff; 
            }
        }
        .content-body{
            padding: 10px 20px 10px 10px; 
            margin-top: 4%;
            .body-item{  
                padding: 12px 10px;
                background-color: rgb(248, 248, 248);
                border-radius: 5px;
                margin-bottom: 15px;
                border-left: 2px solid #f7a500;
                color: #6e6e6e;
                .item-text{
                    font-size: 14px; 
                    text-align: left; 
                }
                .item-icon{
                    text-align: right;
                    .el-icon{
                        font-size: 16px;
                    }
                }
            }
        }
    }
}
@media screen and (min-width: 450px){
    .app-content{
        height: 87%;
        background-color: #fff; 
        .content-header{
            height: 30%;
            background-image: url('../../assets/images/setting-header3.jpg');
            background-size:cover;
            background-position: center;
            background-repeat: no-repeat;
            opacity: calc(.85);  
            box-shadow: 0 0 70px 30px rgb(249, 249, 249) inset;
            img{
                position: relative;
                top: 30px;
            }
            p{
                position: relative;
                top: 17px; 
                text-align: center;  
                color: #fff; 
            }
        }
        .content-body{
            padding: 10px 20px 10px 10px; 
            margin-top: 4%;
            .body-item{  
                padding: 12px 10px;
                background-color: rgb(248, 248, 248);
                border-radius: 5px;
                margin-bottom: 15px;
                border-left: 2px solid #f7a500;
                color: #6e6e6e;
                .item-text{
                    font-size: 16px; 
                    text-align: left; 
                }
                .item-icon{
                    text-align: right;
                    .el-icon{
                        font-size: 20px;
                    }
                }
            }
        }
    }
}

</style>