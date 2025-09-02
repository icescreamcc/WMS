<template>
    <Layer :layer="layer" @confirm="onSubmit" ref="layerDom">    
      <div style="height: 60%;">
        <div class="page-scan">
            <div class="scan-box">
                <video ref="video" id="video" class="scan-video" autoplay></video>
                <div class="qr-scanner">
                    <div class="box">
                        <div class="line"></div>
                        <div class="angle"></div>
                    </div>
                </div>
                <div class="scan-tip">{{ scanTextData.tipMsg }}</div>
            </div>
        </div>
      </div>
    </Layer>
  </template>
  
  <script lang="ts" setup> 
  import { ref,defineEmits,defineProps,onMounted } from 'vue'  
  import Layer from '@/components/layer/index.vue'
  import { ElForm } from 'element-plus'  
  import msg from '@/utils/system/message'  
  import { BrowserMultiFormatReader } from "@zxing/library";
  import { useRouter, useRoute } from 'vue-router'  
 
  const router = useRouter();
  const route=useRoute(); 
  const props=defineProps({
    layer:{
        type: Object,
        default:()=>{
            return{
                show: false,
                title: '',
                showButton: true,
                btnLoading:false,
                width:"30%",
                type:'',
                data:null,
                options:null 
            }
        }
    }
  });  

 const scanTextData:any= ref({
          codeReader: null,
          tipMsg: "识别二维码", 
          num: 5, 
          videoLength: ""
        })

  onMounted(()=>{ 
    scanTextData.value.codeReader = new BrowserMultiFormatReader();
    openScan(); // 打开摄像头
  })

  const openScan=()=>{
    scanTextData.value.codeReader.getVideoInputDevices().then((videoInputDevices:any) => {
            // 默认获取第一个摄像头设备id
            let firstDeviceId = videoInputDevices[0].deviceId;
            console.log(
              "手机摄像头的数量",
              videoInputDevices.length,
              videoInputDevices
            );
            // 获取第一个摄像头设备的名称
            const videoInputDeviceslablestr = JSON.stringify(
              videoInputDevices[0].label
            );
            if (videoInputDevices.length > 1) {
              // 华为手机有6个摄像头，前三个是前置，后三个是后置，第6个摄像头最清晰
              if (videoInputDevices.length > 5) {
                firstDeviceId = videoInputDevices[5].deviceId;
              } else {
                // 判断是否后置摄像头
                if (videoInputDeviceslablestr.indexOf("back") > -1) {
                  firstDeviceId = videoInputDevices[0].deviceId;
                } else {
                  firstDeviceId = videoInputDevices[1].deviceId;
                }
              }
            }
            decodeFromInputVideoFunc(firstDeviceId);
          })
          .catch((err:any) => {
            console.error(err);
          });
  }

  const decodeFromInputVideoFunc=(firstDeviceId:any)=>{
    scanTextData.value.codeReader.reset();
        scanTextData.value.codeReader.decodeFromInputVideoDeviceContinuously(
          firstDeviceId,
          "video",
          (result:any, err:any) => {
            if (result && result.text) {
              let content = result.text
              console.log("扫出的数据",content)
              if (content) {
                let uuidString = /^[a-zA-Z0-9]{32}$/
                if (content.indexOf('handover') > -1) {
                  //扫码签字
                  // router.push({
                  //   path: '/scanCodeAndSign',
                  //   query:{url:content}
                  // })
                }else if (uuidString.test(content)) {
                  //设备详情
                  // router.push({
                  //   path: "/equipmentAccountDetail",
                  //   query: { unitBasicDeviceId: content },
                  // })
                }else {
                 // this.$toast("识别错误，请确认是否扫描的设备二维码~")
                }
              }
            }
            if (err && !err) {
              console.log(err); 
            }
          }
        );
  }
 

  const emit = defineEmits(['dataSubmit'])
   

  const onSubmit=()=>{  
    //  emit('dataSubmit',props.layer.type,shelfForm.value)
  }
  </script>
  
  <style lang="scss" scoped>
    .scan-box {
        position: fixed;
        top: 40px;
        left: 0;
        height: 100%;
        width: 100vw;
        background-image: linear-gradient(
                0deg,
                transparent 24%,
                rgba(32, 255, 77, 0.1) 25%,
                rgba(32, 255, 77, 0.1) 26%,
                transparent 27%,
                transparent 74%,
                rgba(32, 255, 77, 0.1) 75%,
                rgba(32, 255, 77, 0.1) 76%,
                transparent 77%,
                transparent
        ),
        linear-gradient(
                90deg,
                transparent 24%,
                rgba(32, 255, 77, 0.1) 25%,
                rgba(32, 255, 77, 0.1) 26%,
                transparent 27%,
                transparent 74%,
                rgba(32, 255, 77, 0.1) 75%,
                rgba(32, 255, 77, 0.1) 76%,
                transparent 77%,
                transparent
        );
        background-size: 3rem 3rem;
        background-position: -1rem -1rem;
    }

    .scan-video {
        height: 95vh;
        width: 100vw;
        object-fit: cover;
    }

    .qr-scanner .box {
        width: 213px;
        height: 213px;
        position: absolute;
        left: 50%;
        top: 50%;
        transform: translate(-50%, -50%);
        overflow: hidden;
        border: 0.1rem solid rgba(0, 255, 51, 0.2);
        /* background: url('http://resource.beige.world/imgs/gongconghao.png') no-repeat center center; */
    }

    .qr-scanner .line {
        height: calc(100% - 2px);
        width: 100%;
        background: linear-gradient(180deg, rgba(0, 255, 51, 0) 43%, #00ff33 211%);
        border-bottom: 3px solid #00ff33;
        transform: translateY(-100%);
        animation: radar-beam 2s infinite alternate;
        animation-timing-function: cubic-bezier(0.53, 0, 0.43, 0.99);
        animation-delay: 1.4s;
    }

    .qr-scanner .box:after,
    .qr-scanner .box:before,
    .qr-scanner .angle:after,
    .qr-scanner .angle:before {
        content: "";
        display: block;
        position: absolute;
        width: 3vw;
        height: 3vw;
        border: 0.2rem solid transparent;
    }

    .qr-scanner .box:after,
    .qr-scanner .box:before {
        top: 0;
        border-top-color: #00ff33;
    }

    .qr-scanner .angle:after,
    .qr-scanner .angle:before {
        bottom: 0;
        border-bottom-color: #00ff33;
    }

    .qr-scanner .box:before,
    .qr-scanner .angle:before {
        left: 0;
        border-left-color: #00ff33;
    }

    .qr-scanner .box:after,
    .qr-scanner .angle:after {
        right: 0;
        border-right-color: #00ff33;
    }

    @keyframes radar-beam {
        0% {
            transform: translateY(-100%);
        }

        100% {
            transform: translateY(0);
        }
    }

    .scan-tip {
        width: 100vw;
        text-align: center;
        margin-bottom: 5vh;
        color: white;
        font-size: 5vw;
        position: absolute;
        bottom: 50px;
        left: 0;
        color: #fff;
    }

    .page-scan {
        overflow-y: hidden;
        height: 80%;
    }
  </style>