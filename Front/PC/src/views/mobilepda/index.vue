<template>
    <div class="app-container">
        <div class="app-header">
            <el-row style="color: #fff;">
                <el-col :span="6">
                   <div class="app-return" v-if="!showMain" @click="onGoBack">
                    <el-icon class="app-return-icon"><ArrowLeft /></el-icon>
                    <span >返回</span>
                   </div>
                </el-col>
                <el-col :span="12">
                    <div class="app-name">
                        {{ route.meta.title }} 
                    </div>
                </el-col>
                <el-col :span="6">
                 
                </el-col>
            </el-row> 
    </div>
    <div class="app-content" v-if="showMain">
        <div class="content-header">
            <p class="header-title">Gemba.Digital.Efficient</p>
        </div>
        <div class="content-body">
            <el-row :gutter="2">
                <el-col :span="5" :offset="1" class="body-item" v-for="item in menus" @click="onRoute(item)">  
                    <img :src="item.icon" alt="">
                    <div>{{ item.menuName }}</div> 
                </el-col>  
                <el-col :span="5" :offset="1" class="body-item" @click="toggle">
                    <img src="/public/icon-img/quanping1.png" alt="">
                    <div>全屏切换</div> 
                </el-col>
            </el-row>
        </div> 
    </div> 
    <router-view></router-view> 
    <div  class="app-footer">
        <el-row>
            <el-col :span="8"  class="footer-item">
               <span @click="onGoHome($event)"> <el-icon class="footer-item-icon" ><Menu /></el-icon></span>
            </el-col>
            <el-col :span="8"  class="footer-item">
                <el-badge  class="item-badge" type="danger" :value="msgNotReadCount" v-if="permission.isPermisstion('MESSAGE')">
                    <span @click="onGoMessage($event)"><el-icon class="footer-item-icon"><Comment /></el-icon></span>
                </el-badge> 
            </el-col>
            <el-col :span="8"  class="footer-item" v-if="permission.isPermisstion('MYSETTING')"> 
                <span @click="onGoUserSetting($event)"><el-icon class="footer-item-icon"><UserFilled /></el-icon></span>
            </el-col>
        </el-row>
    </div> 
</div>
</template>

<script lang="ts" setup>
import {  ref,onMounted,onBeforeUnmount } from "vue";
import { useI18n } from 'vue-i18n';  
import { useRouter, useRoute } from 'vue-router';
import {ArrowLeft,Menu,Comment,UserFilled} from '@element-plus/icons-vue';  
import commonHelper from '@/utils/system/common-helper';
import { useStore } from 'vuex';
import permission from '@/utils/system/permission';
import{getUserMenus} from '@/api/mobile-pda/mobilePermission';
import {getNotReadMessageCount} from '@/api/mobile-pda/message' 
import { useFullscreen } from '@vueuse/core'  
import Vconsole from 'vconsole'

const router = useRouter();
const route = useRoute();
const { t } = useI18n(); 
const user=permission.getOperator();    
const menus=ref(new Array<any>()); 
const showMain=ref(true); 
const home='mobilepda';
const msg='pdamsg';
const setting='pdausersetting'
const msgNotReadCount=ref(0);
const preNotReadMsgCount=ref(0);
const { isFullscreen, toggle,enter } = useFullscreen();  
var msgIntervalHandler:any=0;
//const vconsole=new Vconsole();

window.addEventListener("popstate", function (e) {
    if(route.name==home){
        showMain.value=true;
    }
    else{
        showMain.value=false;
    }
}, false);

onBeforeUnmount(()=>{
    clearInterval(msgIntervalHandler); 
});

onMounted(()=>{  
    permission.clearRouteHis();
    permission.addRouteHis(route.name as string);   
    getMeuns();
    getMsgCount();
    msgIntervalHandler=setInterval(() => getMsgCount(), 1000*6); 
});
   
const getMeuns=()=>{
    getUserMenus(user.userId).then((res: any) => {  
        if(res.data.length>0){
            res.data.forEach((item:any) => {
                if(item.routePath!=route.name&&item.routePath!=msg&&item.routePath!=setting&&item.menuLayout=="Primary"){ 
                // item.icon=commonHelper.getImgUrl(`/public/icon-img/${item.icon}`); 
                    item.icon=`/public/icon-img/${item.icon}`;
                    menus.value.push(item);
                }
            });  
        }  
    });
}

const getMsgCount=()=>{ 
    getNotReadMessageCount(user.userId).then(res=>{
        msgNotReadCount.value=res.data;
        if(Number(res.data)>preNotReadMsgCount.value){
            preNotReadMsgCount.value=res.data;
            setTimeout(() => {
                speakText()
            }, 2000);
        } 
    })
}
 
const onRoute=(item:any)=>{
    router.push({name:item.routePath}).then(()=>{
        showMain.value=false;
        permission.addRouteHis(route.name as string);  
    });
}

const onGoBack=()=>{
    let routeHis=permission.getRouteHis(); 
    let preRoute= routeHis[routeHis.length-2]; 
        if(preRoute){
            permission.removeRouteHis(preRoute);
            router.push({name:preRoute}).then(()=>{  
                if(route.name==home){
                    showMain.value=true;
                } 
                else{
                    showMain.value=false;
                } 
            });
        } 
}

const onGoHome=(e:any)=>{
    router.push({name:home}); 
    showMain.value=true;
    permission.clearRouteHis();
    permission.addRouteHis(home);   
    e.target.style.color='#409EFF';
    setTimeout(() => {
        e.target.style.color='#fff';
    }, 150);  
} 

const onGoMessage=(e:any)=>{
     router.push({name:msg}).then(()=>{
        showMain.value=false;
        permission.addRouteHis(route.name as string);    
     });  
    e.target.style.color='#409EFF';
    setTimeout(() => {
        e.target.style.color='#fff';
    }, 150);  
}

const onGoUserSetting=(e:any)=>{
    router.push({name:setting}).then(()=>{
        showMain.value=false;
        permission.addRouteHis(route.name as string); 
     });  
    e.target.style.color='#409EFF';
    setTimeout(() => {
        e.target.style.color='#fff';
    }, 150);
}
  
const speakText=()=> {  
        let text="您有新的消息，请注意查看哦"; 
        const utterance = new SpeechSynthesisUtterance(text);
        utterance.lang = 'zh-CN';//'en-US';
        let voices = speechSynthesis.getVoices(); 
        console.log("speakText_voices",voices);
        //let selectedVoice = voices.find(voice => voice.name == "Google US English"||voice.name == "Microsoft Zira - English (United States)");
        let selectedVoice = voices.find(voice => voice.name == "Microsoft Xiaoxiao Online (Natural) - Chinese (Mainland)");
        if (selectedVoice) { 
            utterance.voice = selectedVoice; 
            speechSynthesis.speak(utterance);
        } 
    }
 
</script>

<style lang="scss" scoped>
@media screen and (max-width: 450px){ /* 手机样式 */
    .app-container{
        background-color: rgb(248, 248, 248);
        height: 100%; 
        .app-header {  
            height: 43px;
            line-height: 43px; 
            background-color: var(--system-header-background);  
        .app-name{
                color: #fff;
                text-align: center; 
                }
        .app-return{
            color: #fff; 
            font-size: 14px;
            .app-return-icon{
                position: relative;
                bottom: -2px;
            }
        }
        }
        .app-content{
            height: 90%;
            background-color: #fff;
            .content-header{
                height: 30%;
                background-image: url('../../assets/images/mobile-header2.jpeg');
                background-size: cover;
                background-position: center;
                background-repeat: no-repeat;
                opacity: calc(.85);
                .header-title{
                    float: left;
                    font-size: 20px;
                    font-weight: 600;
                    color: #fff;
                    text-align: center;
                    position: relative;
                    top: 10%;
                    left: 35%;
                    transform: translateY(-50%);
                }
            }
            .content-body{
                padding: 10px 20px 10px 10px; 
                margin-top: 4%;
                .body-item{  
                    padding:10px;
                    background-color: rgb(248, 248, 248);
                    border-radius: 5px;
                    margin-bottom: 10px;
                    font-size: x-small;
                    color: #888;
                    img{
                        height: 45px;
                    } 
                }
            } 
        }
        .app-footer{
                height: 47px; 
                background-color: var(--system-header-background);   
                position: absolute;
                bottom: 0px;
                width: 100%; 
                .footer-item{  
                    z-index: 0; 
                    .footer-item-icon{
                        position: relative;
                        top: 4px;
                        color: #fff;
                        font-size: 30px;
                    } 
                    .item-badge{
                        position: relative;
                        top: -1px;
                    }
                }
        }
    }
}


@media screen and (min-width: 450px){/* PC样式 */
    .app-container{
        background-color: rgb(248, 248, 248);
        height: 100%; 
        .app-header {  
            height: 47px;
            line-height: 47px;
            background-color: var(--system-header-background);  
        .app-name{
                color: #fff;
                text-align: center; 
                font-size: 18px;
                }
        .app-return{
            color: #fff; 
            font-size: 18px;
        }
        }
        .app-content{
            height: 93%;
            background-color: #fff;
            .content-header{
                height: 30%;
                background-image: url('../../assets/images/mobile-header2.jpeg');
                background-size: cover;
                background-position: center;
                background-repeat: no-repeat;
                opacity: calc(.85);
                .header-title{
                    float: left;
                    font-size: 30px;
                    font-weight: 600;
                    color: #fff;
                    text-align: center;
                    position: relative;
                    top: 5%;
                    left: 35%;
                    transform: translateY(-50%);
                }
            }
            .content-body{
                padding: 10px 20px 10px 10px; 
                margin-top: 4%;
                .body-item{  
                    padding:25px 0px;
                    background-color: rgb(248, 248, 248);
                    border-radius: 5px;
                    margin-bottom: 10px;
                    font-size: 14px;
                    color: #888;
                    img{
                        height: 70px;
                        margin-bottom: 3px;
                    }
                }
            } 
        }
        .app-footer{
                height: 47px; 
                background-color: var(--system-header-background);   
                position: absolute;
                bottom: 0px;
                width: 100%; 
                .footer-item{  
                    z-index: 0; 
                    .footer-item-icon{
                        position: relative;
                        top: 4px;
                        color: #fff;
                        font-size: 30px;
                    } 
                    .item-badge{
                        position: relative;
                        top: -1px;
                    }
                }
        }
    }
}
 
// /* 设置滚动条的宽度、高度、背景色和边框样式 */
// ::-webkit-scrollbar { 
// width: 20px;
// height: 10px;  
// border-radius: 5px;
// }

// /* 设置滚动条轨道的背景色和圆角 */
// ::-webkit-scrollbar-track { 
//   background-color: transparent; 
// }
  
// /* 设置滚动条滑块的背景色和圆角 */
// ::-webkit-scrollbar-thumb { 
//   background-color: #f1f1f1c0;
//   border-radius: 5px;
// }

// /* 设置滚动条滑块在悬停状态下的背景色和圆角 */
// ::-webkit-scrollbar-thumb:hover { 
// background-color: #d3d3d3c2;  
// border-radius: 5px;
// }



// /* 隐藏滚动条 IE Edge */
// :-ms-scrollbar {
//   width: 10px;
//   height: 10px;  
//   border-radius: 5px;
// }

// :-ms-scrollbar-track {
//   background-color: transparent;
// }
  
// :-ms-thumb {
//   background-color: #f1f1f1c0;
//   border-radius: 5px;
// }

// :-ms-thumb:hover {
//   background-color: #d3d3d3c2;  
// border-radius: 5px
// }

// /* 隐藏滚动条 Firefox */
//  .hide-scrollbar {
//   scrollbar-width: none; 
//   scrollbar-color: transparent transparent; 
// }
 
</style>