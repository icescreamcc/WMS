import message from "./message";

const apiHostConfig: any = {
    address:import.meta.env.VITE_BASE_URL?.toString().replace('https://','').replace('http://','').replace('/api',''),
    ssl:import.meta.env.VITE_BASE_URL?.toString().includes('https')
 };

 var socketClient:any=null;
 const socketConnectState:any={
    isConnected:false,
    message:'',
    state:0,
    url:"",
    connectExecute:false
 };

 export interface SocketMessageModel{
    Id?:string,
    Type?:string,
    Name?:string,
    Status?:string,
    Message?:string,
    Date?:string,
    Data?:any,
    BusinessType?:string
 } 

 const socketApiConnect=(successHandler:Function,failedHandler:Function,dataReceiveHandler:Function)=>{
    socketConnectState.connectExecute=true;
    let connectString=apiHostConfig.ssl?`wss://${apiHostConfig.address}`:`ws://${apiHostConfig.address}`; 
    socketClient=new WebSocket(connectString);
    socketClient.addEventListener('open',(event:any)=>{ 
        if(event.isTrusted&&event.type=='open'){   
            socketConnectState.isConnected=true;
            socketConnectState.connectExecute=false;
            socketConnectState.state=socketClient.readyState;
            socketConnectState.url=socketClient.url; 
            if(successHandler){
                successHandler(event.currentTarget); 
            }
            socketApiReceive(dataReceiveHandler); 
        } 
        else{
            socketConnectState.isConnected=false;
            socketConnectState.connectExecute=false;
            socketConnectState.state=socketClient.readyState;
            socketConnectState.message="socket连接失败!";
            socketConnectState.url=socketClient.url; 
            if(failedHandler){  
                failedHandler(event.currentTarget); 
            } 
        }
    });
}

const socketApiClose=(closeHandler:Function)=>{ 
    if(socketClient&&socketClient.readyState==WebSocket.OPEN){ 
        socketClient.addEventListener('close',(event:any)=>{ 
            socketConnectState.isConnected=false;
            socketConnectState.connectExecute=false;
            socketConnectState.state=socketClient.readyState;
            socketConnectState.message="socket连接已关闭!";
            socketConnectState.url=socketClient.url;
            if(closeHandler){ 
                closeHandler(event.currentTarget);
            } 
        }); 
        socketClient.close();
    } 
}

const socketApiReceive=(dataReceiveHandler:Function)=>{
    if(socketClient&&socketClient.readyState==WebSocket.OPEN){
        socketClient.addEventListener('message',(event:any)=>{ 
            if(event.data){  
                let data=JSON.parse(event.data);  
                if(dataReceiveHandler) {
                    dataReceiveHandler(data);
                }
            } 
        });
    }
}

const socketSendApiMsg=(msgData:SocketMessageModel)=>{  
    if(socketClient&&socketClient.readyState==WebSocket.OPEN){
         try{
            socketClient.send(JSON.stringify(msgData));
            socketConnectState.isConnected=true;
            socketConnectState.state=socketClient.readyState;
            socketConnectState.message="Ticks...";
            socketConnectState.url=socketClient.url; 
         }
         catch(ex:any){
            socketConnectState.isConnected=false;
            socketConnectState.connectExecute=false;
            socketConnectState.state=socketClient.readyState;
            socketConnectState.message="socket通讯异常，"+ex; 
            socketConnectState.url=socketClient.url;
         }
    }
    else{
        socketConnectState.isConnected=false;
        socketConnectState.connectExecute=false;
        socketConnectState.state=socketClient.readyState;
        socketConnectState.message="socket连接中断!"; 
        socketConnectState.url=socketClient.url;
    }
}

export default {
    socketConnectState,
    socketApiConnect,
    socketApiClose,
    socketSendApiMsg 
}