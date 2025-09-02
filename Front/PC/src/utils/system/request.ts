import axios , { AxiosError, AxiosRequestConfig, AxiosResponse, AxiosInstance } from 'axios'
import store from '@/store' 
import msg from "./message" 
import {Base64} from 'js-base64'
const baseURL: any = import.meta.env.VITE_BASE_URL

const service: AxiosInstance = axios.create({
  baseURL: baseURL,
 // timeout: 5000
})
 
 

// 请求前的统一处理
service.interceptors.request.use(
  (config: AxiosRequestConfig) => { 
    // JWT鉴权处理 
    config.headers['authorization'] = store.getters['user/token']||''; 
     config.headers['userId'] = store.getters['user/userId']||'';    
     // 对 GET 请求的参数进行 Base64 加密 
     if (config.method === 'get') {
      if(config.params){
        let encryptedParams :any= {};
        for (let key in config.params) {
          if (config.params.hasOwnProperty(key)) {
            let value = config.params[key];
            encryptedParams[key] = Base64.encode(value);
          }
        }
        config.params = encryptedParams;
      }
      else if(config.url&& config.url.includes('?')){
        let urlArray= config.url.split('?');
        let queryString = urlArray[1];
        config.url=urlArray[0]+"?"+Base64.encode(queryString);
      }  
    }
    return config
  },
  (error: AxiosError) => { 
    return Promise.reject(error)
  }
)

service.interceptors.response.use(
  (response: AxiosResponse) => {     
      var token= response.headers["authorization"] ;  
      store.commit("user/tokenChange",token) 
      if (response.status == 200) {   
          if(response.data.status=="Success") {  
              if(response.data.message){
                  msg.successAuto(response.data.message)  
              }
              return response.data
          }
          else{ 
            if(response.data.message){
              msg.errorAuto(response.data.message)  
            }  
            return Promise.reject(response.data)
          }
      } 
      else {  
        if(response.status == 401){ 
          store.dispatch('user/loginOut'); 
        }
        if(response.data.message){
          msg.errorAuto(response.data.message); 
        }
        else{
          msg.errorAuto(response.data); 
        } 
        return Promise.reject(response.data);
      }
  },
  (error: AxiosError)=> {     
    if(error.response&&error.response.data){ 
      if(error.response.data.message){
        msg.errorAuto(error.response.data.message);
        if(error.response.status==401){
          store.dispatch('user/loginOut'); 
        }
      } 
      else{
        if(error.response.data.errors[0])
        {
          msg.errorAuto(error.response.data.errors[0]);
        }
        else
        {
          msg.errorAuto(error.response.data.title);
        }
     } 
    } 
    else{
      msg.errorAuto(error.message || '服务异常')
    }  
    return Promise.reject(error)
  }
)
  
export default service