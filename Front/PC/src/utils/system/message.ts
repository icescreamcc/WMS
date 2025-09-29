import { ElMessage } from 'element-plus'

const successAuto=  (msg:string,time:number=3000)=>{
    ElMessage.success({
        message: msg,
        type: 'success',
        showClose: false,
        center: true,
        duration: time
      })
}

const errorAuto= (msg:string,time:number=3000)=>{
    ElMessage.error({
        message: msg,
        type: 'error',
        showClose: false,
        center: true,
        duration: time
      })
}

const warningAuto= (msg:string,time:number=3000)=>{
    ElMessage.warning({
        message: msg,
        type: 'warning',
        showClose: false,
        center: true,
        duration: time
      })
}

const deftAuto= (msg:string)=>{
    ElMessage({ 
        message: msg,
        center: true
      })
}

export default {
    successAuto,
    errorAuto,
    warningAuto,
    deftAuto
}