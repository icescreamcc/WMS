<template>
    <div>
      <el-dialog 
        ref="dialog"
        :modal="true"
        v-model="props.layer.show" 
        :width="props.layer.width"
        :show-close="false"
        :fullscreen="isFullScreen"
        top="5vh"
        @close="onModalCloseEvent"
        :close-on-press-escape="false"
        :close-on-click-modal="false">  
        <div style="background-color: #fff;border: 1px solid #000;border-radius: 8px;">
        <div class="layer-content">
           <div class="content-order"> 
            <div class="order-title">{{ orderInfo.title }}</div> 
            <div class="order-body">
                <div class="order-item"  v-for="(task,index) in taskList" :class="task.isSelected?'order-item-select':''" @click="onSelectTask(task)" title="选择一个任务后点击启动AGV送货">
                    <div class="item-info">
                        <span>任务{{ index+1 }}</span> 
                    </div>
                    <div class="item-info">
                        <span>{{  task.goodsRemark }}</span> 
                    </div>  
                    <div class="item-info">
                        <span>状态：{{  task.statusDesc }}</span> 
                    </div>  
                </div> 
            </div>
           </div>
           <div class="content-operation">
            <div class="operation-log">
                <div v-for="log in logInfo" class="log-item">
                    <div class="item-title">
                      <el-icon v-if="log.Status=='Success'" class="text-success"><InfoFilled />
                      </el-icon><el-icon v-else class="text-danger"><WarningFilled /></el-icon>
                      <span>{{ commonHelper.formatToDateTime(log.Date) }}</span>
                      <span style="margin-left: 15px;">{{ log.Message }}</span>
                    </div>   
                </div> 
            </div>
            <div class="operation-btn"> 
                <div class="opera-icon">
                    <img v-if="deftTransportDevice?.isReady" src="/public/icon-img/agv_start.png" @click="onStartUpAgv" title="启动AGV送货">
                    <img v-else src="/public/icon-img/agv_start0.png" title="启动AGV送货">
                </div>  
                <div class="opera-icon">
                    <img v-if="deftTransportDevice?.isAllowRollOut" src="/public/icon-img/roll-out.png" @click="onRollOutside" title="传送带送出料箱">
                    <img v-else src="/public/icon-img/roll-out0.png"  title="传送带送出料箱">
                </div> 
                <div class="opera-icon">
                    <img v-if="deftTransportDevice?.isAllowRollIn" src="/public/icon-img/fuwei.png" @click="onRollInside" title="传送带送回料箱">
                    <img v-else src="/public/icon-img/fuwei0.png"  title="传送带送回料箱">
                </div>   
                <div class="opera-icon" style="position: absolute;right: 0%;" @click="onCloseModal"><img  src="/public/icon-img/guanbi_1.png" title="关闭窗口"></div> 
            </div>
           </div>
           <transition name="el-zoom-in-center"> 
           <div class="workbin-layout" v-if="workbinCells?.length>0">
            <el-row :gutter="20" style="width: 100%;height: inherit;border: 2px solid #1e3055;">
                <el-col v-if="workbinCells.length==1" v-for="(opt,index) in workbinCells" 
                    class="workbin-cell" :span="24"
                    :style="{height: '100px',lineHeight:'100px'}" :title="opt.cellNo">
                    <transition name="tips">
                        <div style="height: 97%;width: 97%;border:2px solid #e98f36; " v-show="opt.tipsShow" v-if="opt.isCurrent"></div> 
                    </transition> 
                    <span :class="opt.isCurrent?'text-warning':''" style="position: absolute;left: 50%;top: 50%;transform: translate(-50%,-50%);display: inline-block;width: 100%;">{{ opt.cellNo }}</span>
                    </el-col> 
                    <el-col v-else-if="workbinCells.length==2" v-for="(opt,index) in workbinCells" 
                    class="workbin-cell" :span="12"
                    :style="{height: '100px',lineHeight:'100px'}" :title="opt.cellNo">
                    <transition name="tips">
                        <div style="height: 97%;width: 97%;border:2px solid #e98f36; " v-show="opt.tipsShow" v-if="opt.isCurrent"></div> 
                    </transition> 
                    <span :class="opt.isCurrent?'text-warning':''" style="position: absolute;left: 50%;top: 50%;transform: translate(-50%,-50%);display: inline-block;width: 100%;">{{ opt.cellNo }}</span>
                    </el-col> 

                    <!-- <el-col v-else v-for="(opt,index) in workbinCells" 
                    class="workbin-cell" :span="24/(workbinCells.length/2)"
                    :style="{height: '50px',lineHeight:'50px'}" :title="opt.cellNo">
                    <transition name="tips">
                        <div style="height: 97%;width: 97%;border:2px solid #e98f36; " v-show="opt.tipsShow" v-if="opt.isCurrent"></div> 
                    </transition> 
                    <span :class="opt.isCurrent?'text-warning':''" style="position: absolute;left: 50%;top: 50%;transform: translate(-50%,-50%);display: inline-block;width: 100%;">{{ opt.cellNo }}</span>
                    </el-col>  -->
                    <el-col v-for="(opt, index) in workbinCells" 
            :key="index"
            :span="12" 
            class="workbin-cell"
            :style="{ height: '50px', lineHeight: '50px' }" :title="opt.cellNo">
      <transition name="tips">
        <div v-show="opt.tipsShow" v-if="opt.isCurrent" style="height: 97%; width: 97%; border: 2px solid #e98f36;"></div>
      </transition>
      <span :class="opt.isCurrent ? 'text-warning' : ''" style="position: absolute; left: 50%; top: 50%; transform: translate(-50%, -50%); display: inline-block; width: 100%;">
        {{ opt.cellNo }}
      </span>
    </el-col>

                </el-row> 
           </div> 
        </transition>
           <div class="content-warehouse" v-for="warehouse in warehouseInfo" :style="{height:`${(100/warehouseInfo.length)}%`}">
                <div class="content-conveyor" v-for="device in warehouse.transportDevice" :style="{top:`${(50/warehouse.transportDevice.length)}%`}">
                    <div class="conveyor-title" :title="`编号：${device.deviceNo}，PLC地址：${device.connectAddress}`">{{ device.deviceName }}</div>
                    <transition name="lamp">
                        <div class="conveyor-body" v-show="device.lampShow" :style="{background:device.isConnected?'#acf4b87d':'#d3d3d373'}"></div> 
                    </transition>  
                    <div class="agv-dock" v-if="device.isAgvDock">
                        <img src="/public/icon-img/agv-business6.png">
                    </div>
                </div> 
                <div class="content-pos" >
                    <img src="/public/icon-img/weizhi.png">
                    <div class="pos-title">取货点</div>
                </div>
                <div class="warehouse-title">{{ warehouse.warehouseName }}</div>
                <div class="warehouse-layout">
                    <div class="layout-area" v-for="(area,index) in warehouse.areas" :style="{height:`${(100/warehouse.areas.length)-.2}%`,borderBottom:(index==warehouse.areas.length-1)?'none':'1px dashed #337d80'}">
                        <div class="area-title">{{ area.areaNo }}区</div>
                        <div class="area-shelfs"> 
                            <div class="shelf-item" v-for="shelf in area.shelfs" :style="{width:`${(92/shelf.col)-1}%`}">
                                <span>{{ shelf.shelfNo }}</span>
                                <div class="agv-dock" v-if="shelf.isAgvDock">
                                    <img src="/public/icon-img/agv-business5.png">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
          </div> 
         </div> 
        </div>
      </el-dialog>  
    </div> 
    </template>
    
    <script lang="ts" setup>
    import {  ref,defineEmits,defineProps,onMounted,onBeforeUnmount } from 'vue';   
    import {InfoFilled,WarningFilled} from '@element-plus/icons-vue'; 
    import commonHelper from "@/utils/system/common-helper";
    import { connectAllPLC,closeAllPLC,sendDeviceRollOut,sendDeviceRollIn,getAutoTransportInfrastructureInfo,getPendingExecTransTask,executeTransTask,getDeviceState } from "@/api/auto-operation/auto-transport";
    import { getWorkbinCellsByCellNo} from "@/api/inv/workbin";
    import  socketHelper from "@/utils/system/socketClient"; 
    import msg from "@/utils/system/message";
    import {confirmOutStorage,updateActualQuantity} from "@/api/inv/outstorage";
    import {confirmInStorage} from "@/api/inv/instorage"; 
    import { ElMessage, ElMessageBox } from 'element-plus'

    const props=defineProps({
      layer: {
          type: Object,
          default: () => {
            return {
              show: false,
              title: '',
              showButton: true,
              btnLoading:false,
              type:'',
              options:null,
              data:null 
            }
          }
        }
    }); 
    const emit = defineEmits(['operationFinished']);   
    const agvInfo=ref(new Array<any>());
    const warehouseInfo=ref(new Array<any>()); 
    const orderInfo=ref();
    const taskList=ref(new Array<any>());
    const isFullScreen=ref(false);
    const logInfo=ref(new Array<any>());
    var deviceWatcherHandler=new Array<any>(); 
    const deftTransportDevice:any=ref();
    const goodsClassifyGroup=ref();
    var isConfirmLoading=false;
    const workbinCells=ref(new Array<any>());
    const plcMsgType={
        Log:'Log',
        Tick:'Tick',
        Connect:'Connect',
        Read:'Read',
        Write:'Write',
        Business:'Business',
        AGVCallback:'AGVCallback',
        Test:'Test'
    } 
    
    onBeforeUnmount(()=>{  
        socketHelper.socketApiClose(socketClosedHandler);
        deviceWatcherHandler.forEach(h=>{
            clearInterval(h);
        }); 
        logInfo.value.length=0;
    })

    onMounted(()=>{    
        orderInfo.value=props.layer.data; 
        goodsClassifyGroup.value=props.layer.data.goodsClassifyGroup;
        socketHelper.socketApiConnect(socketConnectSuccessHandler, socketConnectFailedHandler,socketDataReceiveHandler); 
        getAutoTransportInfrastructureInfo(goodsClassifyGroup.value).then(info=>{
            agvInfo.value=info.data.agvInfo;
            warehouseInfo.value=info.data.warehouseInfo;  
            warehouseInfo.value.forEach(w=>{
                w.transportDevice.forEach((dev:any) => {
                    dev.isReady=false; 
                    dev.isAllowRollIn=false;
                    dev.isAllowRollOut=false; 
                    dev.lampShow=true;
                    deftTransportDevice.value=dev; 
                }); 
            }); 
            setPendingExecTransTask();
            deciceConnectToAll();
            deviceConnectingWatcher(); 
        });   
    }); 

    const setPendingExecTransTask=()=>{
        getPendingExecTransTask(goodsClassifyGroup.value,props.layer.data.operatorId).then(res=>{
            taskList.value=res.data;   
            let isSetSelect=false;
            taskList.value.forEach(f=>{
                if(f.isExecuting){
                    f.isSelected=true;
                    isSetSelect=true; 
                    if(f.taskStatus=="End"&&f.returnTaskStatus=="Await"){
                        deftTransportDevice.value.isAllowRollOut=true; 
                    } 
                }
            })
            taskList.value.forEach(f=>{
                f.goodsDetail=JSON.parse(f.goodsInfo);
                f.goodsRemark=f.goodsDetail.map((m:any)=>{ 
                    return `领用物品：${m.GoodsName}，数量：${m.Quantity}，库位：${f.startingBinNo}`
                }).join("，");
                if(!isSetSelect){
                    f.isSelected=true;
                    isSetSelect=true; 
                    deftTransportDevice.value.isReady=true; 
                } 
            }) 
            //如果列表中只有一个任务，且没有执行的情况下直接开始执行
            if(taskList.value.length==1&&!taskList.value[0].isExecuting){
                deftTransportDevice.value.isReady=false; 
                logInfo.value.unshift({
                    Name:'AGV调度', 
                    Status:'Success',
                    Type:plcMsgType.Business,
                    Date:new Date(),
                    Message:'2秒后开始自动执行任务'
                });    
                setTimeout(() => {
                    onStartUpAgv();
                }, 2000);
            }
        }) 
    }

    const onSelectTask=(task:any)=>{
        let isAllowSelect=true;
        taskList.value.forEach(f=>{
            if(f.isExecuting){
                isAllowSelect=false;
            } 
        });
        if(isAllowSelect){
            taskList.value.forEach(f=>f.isSelected=false);
            task.isSelected=true;
            deftTransportDevice.value.isReady=true; 
        }
        else{
            msg.warningAuto("当前有运送任务正在执行中，无法选择和执行其他任务");
        }
    }

    //查询界面所展示的所有PLC设备
    const getAllPlcDeviceInfo=()=>{
        var deviceArr=new Array<any>();
        warehouseInfo.value.forEach(w=>{
            deviceArr=[...w.transportDevice]; 
        }) 
        return deviceArr;
    } 

    //连接PLC
    const deciceConnectToAll=()=>{ 
        var deviceArr=getAllPlcDeviceInfo();
        connectAllPLC(deviceArr);
    }
    
    //设备连接状态监测和动画显示
    const deviceConnectingWatcher=()=>{ 
        //socket心跳监测
        var serverToticks=setInterval(()=>{
            socketHelper.socketSendApiMsg({Type:'Tick',Status:'success',Message:'1'}); 
            if(!socketHelper.socketConnectState.isConnected){
                setDeviceConnectStatus("ALL",false); 
                if(!socketHelper.socketConnectState.connectExecute){
                    socketHelper.socketConnectState.connectExecute=true;
                    var msgFailed={
                        Name:'',
                        Address:socketHelper.socketConnectState.url,
                        Status:'Failed',
                        Type:"Socket-Obstruct",
                        Date:new Date(),
                        Message:socketHelper.socketConnectState.message+'3秒后开始重新连接...'
                    }
                    logHandle(msgFailed);   
                    setTimeout(() => {
                        socketHelper.socketApiConnect(socketConnectSuccessHandler, socketConnectFailedHandler,socketDataReceiveHandler);
                    }, 3000); 
                } 
            } 
        },2000);
        if(logInfo.value.length>=10){
            logInfo.value.pop();
        }  
        deviceWatcherHandler.push(serverToticks); 
        //传送带连接监测
        setTimeout(() => {
            var transporthandler=setInterval(()=>{
                warehouseInfo.value.forEach(w=>{
                    w.transportDevice.forEach((dev:any) => {
                        if(!dev.connecting){
                            dev.ticks=(dev.ticks||0)+1;
                        }
                        else{
                            dev.ticks=0;
                            dev.isConnected=true;
                            dev.lampShow=!dev.lampShow;
                        }
                        if(dev.ticks>=5&&!dev.connecting){
                            dev.isConnected=false;
                        } 
                    }); 
                }); 
            },1500);  
            deviceWatcherHandler.push(transporthandler);
        }, 200);
    }

    //与后端建立Socket连接成功事件处理
    const socketConnectSuccessHandler=(target:any)=>{ 
        var msg={
            Name:target.url, 
            Status:'Success',
            Type:plcMsgType.Connect,
            Date:new Date(),
            Message:`已与后端服务器建立Socket连接`
        }
        logHandle(msg);  
        socketHelper.socketSendApiMsg({Type:'ConnectQuery'});  
    }

    //与后端建立Socket连接失败事件处理
    const socketConnectFailedHandler=(target:any)=>{  
        logInfo.value.unshift({
            Name:target.url, 
            Status:'Failed',
            Type:plcMsgType.Connect,
            Date:new Date(),
            Message:'与后端服务器建立Socket连接失败'
        });  
    }

     //关闭与后端建立的Socket连接
    const socketClosedHandler=(target:any)=>{ 
    }

    //获取Socket消息
    const socketDataReceiveHandler=(data:any)=>{  
        if(data.Type==plcMsgType.Connect){
            logHandle(data); 
            if(data.Status=='Success'){  
                if(taskList.value.find(f=>f.isSelected)){
                    deftTransportDevice.value.isReady=true;
                }
            }
        }
        else if(data.Type==plcMsgType.Tick){
            if(data.Status=='Failed'){
                setDeviceConnectStatus(data.Name,false);
            }
            else{
                setDeviceConnectStatus(data.Name,true);  
            }
        }
        else if(data.Type==plcMsgType.Read){ 
            if(data.Status=='Failed'){
                logHandle(data); 
            }
        }
        else if(data.Type==plcMsgType.Write){ 
            if(data.Status=='Failed'){
                logHandle(data); 
            }
        }
        else if(data.Type==plcMsgType.Business){  
            if(data.Data){ 
                if(data.Data.BusinessType=='State'){  
                }
                else if(data.Data.BusinessType=='RollOutEnd'){     
                    //向外滚动到位信号  
                    deftTransportDevice.value.isAllowRollIn=data.Data.Value==1;    
                    if(deftTransportDevice.value.isAllowRollIn){
                        //openEdit();
                    }
                }   
            }
            else{
                logHandle(data);  
            }
           
        }
        else if(data.Type==plcMsgType.AGVCallback){
            logHandle(data);  
            if(data.Data.BusinessType=="Arrive"||data.Data.BusinessType=="WaitePickup"||data.Data.BusinessType=="WaitPutdown"){
                warehouseInfo.value.forEach(w=>{
                    w.areas.forEach((a:any) => {
                        a.shelfs.forEach((s:any) => {
                            s.isAgvDock=false;
                            if(s.shelfId==data.Data.AGVDockDeviceNo){
                                s.isAgvDock=true;
                            }
                        });
                    });
                    w.transportDevice.forEach((d:any) => {
                        d.isAgvDock=false;
                        if(d.deviceNo==data.Data.AGVDockDeviceNo){
                            d.isAgvDock=true;
                        }
                    });
                }); 
                setTimeout(() => {
                    warehouseInfo.value.forEach(w=>{
                        w.areas.forEach((a:any) => {
                            a.shelfs.forEach((s:any) => {
                                s.isAgvDock=false; 
                            });
                        });
                        w.transportDevice.forEach((d:any) => {
                            d.isAgvDock=false; 
                        });
                    });
                }, 12000);
            } 
            else if(data.Data.BusinessType=="End"||data.Data.BusinessType=="Cancel"){
                warehouseInfo.value.forEach(w=>{
                    w.areas.forEach((a:any) => {
                        a.shelfs.forEach((s:any) => {
                            s.isAgvDock=false; 
                        });
                    });
                    w.transportDevice.forEach((d:any) => {
                        d.isAgvDock=false; 
                    });
                }); 
                if(data.Data.BusinessType=="End"){  
                    if(data.Data.IsFinished){
                        // if(!isConfirmLoading){ 
                        //     isConfirmLoading=true;
                        //     if(data.Data.TaskType=="OutStorage"){ 
                        //         confirmOutStorage(data.Data.OrderNo).then(()=>{  
                        //             setTimeout(() => { 
                        //                 isConfirmLoading=false;
                        //             }, 5000);
                        //         });
                        //     }
                        //     else{
                        //         confirmInStorage(data.Data.OrderNo).then(()=>{  
                        //             setTimeout(() => { 
                        //                 isConfirmLoading=false;
                        //             }, 5000);
                        //         });
                        //     }
                        // } 
                        setTimeout(() => {
                            onCloseModal(); 
                        }, 1000);
                    }
                    else{
                        deftTransportDevice.value.isAllowRollOut= true;
                    }
                }
            }
            setPendingExecTransTask(); 
        } 
    }

    const onOutStorageQtyConfirm = (data:any) => {    
       return ElMessageBox.prompt('请确认领用数量', orderInfo.value.title, {
            showClose:false,
            showCancelButton:false,
            confirmButtonText: '确认', 
            inputPattern:/^[0-9]\d*$/,
            inputType:'number',
            inputValue:data.Quantity,
            inputErrorMessage: '无效的输入',
            closeOnPressEscape:false,
            closeOnClickModal:false
        }).then(({ value }) => { 
            data.ActualQuantity=value;
            return updateActualQuantity(data);
        }) 
    }

    const onInStorageQtyConfirm = (data:any) => {    
       return ElMessageBox.prompt('请确认入库数量', orderInfo.value.title, {
            showClose:false,
            showCancelButton:false,
            confirmButtonText: '确认', 
            inputPattern:/^[0-9]\d*$/,
            inputType:'number',
            inputValue:data.Quantity,
            inputErrorMessage: '无效的输入',
            closeOnPressEscape:false,
            closeOnClickModal:false
        }).then(({ value }) => { 
            data.ActualQuantity=value;
            return updateActualQuantity(data);
        }) 
    }

    //执行AGV送货任务
    const onStartUpAgv=()=>{
       var selectedTaskId= taskList.value.find(f=>f.isSelected).taskId;
        executeTransTask(selectedTaskId).then(()=>{
            deftTransportDevice.value.isReady=false; 
        });
    }

    //控制传送带向外滚动
    const onRollOutside=()=>{  
        sendDeviceRollOut(deftTransportDevice.value.deviceNo).then(()=>{
            deftTransportDevice.value.isAllowRollOut=false;
            setTimeout(() => {
                queryBinLayout();
            }, 3000);
        });
    }

    //控制传送带向内滚动
    const onRollInside=()=>{ 
        var curTask=taskList.value.find(f=>f.isExecuting); 
        if(curTask.taskType=='OutStorage'){
            onOutStorageQtyConfirm(curTask.goodsDetail[0]).then(()=>{
                sendDeviceRollIn(deftTransportDevice.value.deviceNo).then(()=>{
                    deftTransportDevice.value.isAllowRollIn=false; 
                    if(curTask.taskStatus=="End"){
                        confirmOutStorage(curTask.orderNo);          
                    }  
                });
            });
        }
        else{
            sendDeviceRollIn(deftTransportDevice.value.deviceNo).then(()=>{
                deftTransportDevice.value.isAllowRollIn=false; 
                if(curTask.taskStatus=="End"){
                    confirmInStorage(curTask.orderNo);        
                }  
            });
        }
        if(workbinCells.value?.length>0){
            setTimeout(() => {
                workbinCells.value.forEach(f=>{
                    if(f.tipsHandle){
                        clearInterval(f.tipsHandle);
                    }
                });
                workbinCells.value.length=0;
            }, 2000);
        }  
    }

    //查询料箱布局，并标识当前取货bin位
    const queryBinLayout=()=>{
        workbinCells.value.length=0;
        var curTask=taskList.value.find(f=>f.isExecuting);  
        var workBinCellNo=curTask.startingBinNo;
       getWorkbinCellsByCellNo(workBinCellNo).then(res=>{
        workbinCells.value=res.data;
        workbinCells.value.forEach(f=>{
            f.isCurrent=false;
            f.tipsShow=false;
            if(f.cellNo==workBinCellNo){
                f.isCurrent=true;
                f.tipsShow=true;
                f.tipsHandle=setInterval(() => {
                    f.tipsShow=!f.tipsShow;
                }, 1000);
            } 
        })
       })
    }
  
    const setDeviceConnectStatus=(deviceNo:string,isConnected:boolean)=>{
        if(deviceNo=="ALL"){
            warehouseInfo.value.forEach(w=>{
                w.transportDevice.forEach((d:any) => {
                    if(!isConnected){ 
                        d.connecting=false;
                        d.lampShow=true; 
                    }
                    else{
                        d.connecting=true; 
                    }
                    
                }); 
            });
        }
        else{
            warehouseInfo.value.forEach(w=>{
                var curTransport= w.transportDevice.find((d:any)=>d.deviceNo==deviceNo);
                if(curTransport){
                    if(!isConnected){ 
                        curTransport.connecting=false;
                        curTransport.lampShow=true;
                    }
                    else{
                        curTransport.connecting=true;
                    }
                } 
            });
        } 
    }
 
    const logHandle=(data:any)=>{
        if(logInfo.value.length>0){
        if(data.Message!=logInfo.value[0].Message||data.Name!=logInfo.value[0].Name){
            logInfo.value.unshift(data); 
        }
        }
        else{
            logInfo.value.unshift(data); 
        }
        if(logInfo.value.length>=13){
            logInfo.value.pop();
        }
    }
        
    const onModalCloseEvent=()=>{
     
    }

    const onCloseModal=()=>{
        props.layer.show=false; 
        emit('operationFinished'); 
    }
    </script>
    
    <style lang="scss" scoped> 
    .tips-enter-active, .tips-leave-active {
      transition: opacity 1s;
    }
    .tips-enter, .tips-leave-to {
      opacity: 0; 
    }
    .lamp-enter-active, .lamp-leave-active {
      transition: opacity 1.5s;
    }
    .lamp-enter, .lamp-leave-to {
      opacity: 0; 
    }
    ::v-deep .el-dialog__header{
      padding: 0;
    }
    ::v-deep .el-dialog__body{
      padding:3px; 
      background: #000; 
      padding: 6px;   
    }
    ::v-deep .el-dialog__footer{
      padding: 5px; 
    }
    ::v-deep .el-radio-button__inner{
      font-weight: 600;
      padding: 3px 20px;
    } 
    ::v-deep .el-radio-button__original-radio:checked+.el-radio-button__inner{
      background-color:#357d6c; 
      border-color:#fff;
      box-shadow:none;
    }
    .layer-content{  
        user-select: none;
         background-image: linear-gradient(to top,#01262b, #002c2ef6,#002c2eec,#002c2ef6, #01262b);  
        //background-image: radial-gradient(circle at center, #002c2eec, #00393cf6, #01262b); 
        padding: 5% 0 10% 0;
        border-radius: 3px;
        position: relative;
        height: 600px;
        .workbin-cell{
            background-color: #717c91;
            border-right: 1px solid #fff;
            border-bottom: 1px solid #fff;
            color: #fff; 
            font-size: xx-small;
            text-align: center;
            position: relative;
        }
        .workbin-cell-tip{
            position: absolute;
            left: 0%;
            top: 0%;
            height: 18px;
        }
        .content-order{
            position: absolute;
            top: .5%;
            left: .5%;
            width: 33%;
            min-height: 35%;
            max-height: 35%;
            border:1px solid #337d80;
            background: #037f850a;
            border-radius: 8px 20px 8px 20px;
            img{
                position: absolute;
                left: 1%;
                top: 0%;
                height: 25px;
                z-index: 999;
            }
            .order-title{
                position: relative;
                top: 2%;
                height: 10%;
                color: #fff; 
                padding-top: 5px;
            }
            .order-info{
                padding: 2% 2%;
                text-align: left;
                color: #fff;
                font-size: 1.2vh;
                border-bottom: 1px solid #337d80; 
                .info-item{
                    padding: .7% 1%;
                }
            }
            .order-body{
                position: relative; 
                top: 2%;
                height: 87%; 
                padding: 3% 2%;
                overflow: hidden scroll;
                .order-item{
                    color: #fff;
                    font-size: 1.2vh;
                    border: 1px solid #337d80;
                    border-radius: 3px;
                    border-left: 6px solid #337d80;
                    padding: 1% 0;
                    margin-bottom: 1%;
                    cursor: pointer;
                    .item-info{ 
                        overflow: hidden;
                        text-align: left;
                        span{ 
                            width: 99%;
                            display: inline-block;
                            padding: .7% 1%;
                        }
                    }
                }
                .order-item:hover{
                    border: 1px solid #c0c4bfa0;
                    border-left: 6px solid #c0c4bfa0;
                } 
                .order-item-select{
                    border: 1px solid #e8b201;
                    border-left: 6px solid #e8b201;
                } 
            }
        }
        .content-operation{
            position: absolute;
            bottom: .5%;
            left: .5%;
            width: 33%; 
            height: 35%;
            border: 1px solid #337d80;
            background-color: #d9d8d70a;
            border-radius: 3px 3px 0 0;
            .operation-log{
                position: absolute; 
                width: 96%;
                height: 70%; 
                padding: 10px 10px;
                font-size: 1.1vh;
                color: #edecec7d;
                overflow: hidden; 
                .log-item{
                    text-align: left;
                    margin-bottom: 5px;
                    .item-title{ 
                        i{ 
                            font-size: 1.15vh;
                            position: relative;
                            bottom: -1px; 
                            opacity: .7;
                        }
                        span{
                            font-size: 1.15vh;
                        }
                    }
                    .item-info{
                        font-size: 1.1vh;
                        margin-top: 3px;
                        padding-left: 8px;
                    }
                }
                }
            .operation-btn{
                position: absolute;
                height: 15%;
                width: 100%;
                border-top:1px solid #337d80;
                text-align: left;
                bottom: 0%;
                padding: 1% 0;
                .opera-icon{
                    cursor: pointer;
                    position: relative;    
                    display: inline-block;
                    margin-right: 5px;
                    img{
                        height: 33px;
                        padding: 5px; 
                    }
                }
                .opera-icon:hover{  
                    img{  
                    background: #ffffff47;
                    border-radius: 5px; 
                    }
                }
                .opera-icon-select{
                    background: #ffffff2e; 
                    border-radius: 5px
                }
            }
        }
        .workbin-layout{ 
            height: auto;
            width: 20%;
            position: absolute;
            top: 50%;
            right: 42%; 
            transform: translate(-50%,-50%);
            border: 1px solid #84d2d5; 
            border-radius: 3px;
            background-color: #1e3055;
        } 
        .content-warehouse{
            position: absolute;
            top: 0%;
            right: 0%;
            border-left: 5px solid #000; 
            border-radius: 10px 0 0 10px;
            height: 100%;
            width: 42%;
            cursor: pointer;
            .content-conveyor{ 
                height: 7%;
                width: 17%;
                border: 1px solid #048d9443; 
                border-left: 5px solid #048d9443;
                border-right: 5px solid #048d9443;
                border-radius: 3px ;
                position: absolute;
                top: 50%;
                left: -9%;
                transform: translateY(-50%);
                background-color: #02464a;
                z-index: 999;
                .conveyor-title{
                    width: 100%;
                    color: #acf4b87d;
                    font-size: 1.8vh;
                    font-weight: 600;
                    position: absolute;
                    left: 50%; 
                    top: 50%; 
                    transform: translate(-50%,-50%); 
                    z-index: 999;
                }
                .conveyor-body{
                    position: absolute;
                    left: 50%; 
                    top: 50%; 
                    transform: translate(-50%,-50%); 
                    height: 60%;
                    width: 100%;
                    background: #00e7df2f; 
                }
                .agv-dock{
                    position: absolute;
                    top: 50%;
                    left: 105%;
                    transform: translateY(-50%);
                    img{
                        height: 25px;
                    }
                }
            } 
            .content-pos{ 
                position: absolute;
                top: 50%;
                left: -18%;
                transform: translateY(-50%);
                img{
                    height: 25px;
                }
                .pos-title{
                    font-size: 12px;
                    color: #fff;
                }
            }
            .warehouse-title{
                position: absolute;
                left: 3%;
                top: 3%;
                color: #ffffff15;
                font-size: 3.3vh;
                font-weight: 600;
                writing-mode: vertical-rl;
            }
            .warehouse-layout{
                width: 80%;
                height: 100%;
                position: absolute;
                right: 0%;
                .layout-area{
                    position: relative;
                    border:1px dashed #337d80;
                    border-right: none;
                    border-top: none;
                    width: 100%;   
                    .area-title{
                        font-weight: 600;
                        position: absolute;
                        color: #6fff0077;
                        top: 1%;
                        left: 1%;
                    }
                    .area-shelfs{
                        position: absolute;
                        right: 0%;
                        bottom: 0%;
                        width: 92%;
                        height: 95%; 
                        text-align: left;
                        .shelf-item{ 
                            position: relative;
                            height: 22%;
                            background: #4747477e; 
                            display: inline-block;
                            margin: 4px 3px;
                            color: #fff;
                            font-size: 1.1vh;
                            font-weight: 600; 
                            border-radius: 2px; 
                            text-align: center; 
                            span{
                                position: absolute;
                                top: 50%;
                                left: 50%;
                                transform: translate(-50%,-50%);
                            }
                            .agv-dock{
                                position: absolute;
                                top: 60%;
                                left: 50%;
                                transform: translateX(-50%);
                                img{
                                    height: 25px;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    </style>