import moment from 'moment'

const isMobile = () :boolean=> {
    let flag = navigator.userAgent.match(/(phone|pad|pod|iPhone|iPod|ios|iPad|Android|Mobile|BlackBerry|IEMobile|MQQBrowser|JUC|Fennec|wOSBrowser|BrowserNG|WebOS|Symbian|Windows Phone|Surface)/i)
    if(flag){
      return true
    } 
    else if(screen.width<920){
      return true;
    }
    return false 
  }
 
  const setObjLocalStorage = (key: string, value: any) => {  
    localStorage.setItem(key, JSON.stringify(value));
  }
 
  const getObjLocalStorage = (key: string) => {
    let value = localStorage.getItem(key);
    if (value) {
      return JSON.parse(value);
    }
    return null;
  }

  const setLocalStorage = (key: string, value: string) => {  
    localStorage.setItem(key, value);
  }
 
  const getLocalStorage = (key: string) => {
    return localStorage.getItem(key); 
  }

  const getImgUrl=(path:string)=>{
    return new URL(path, import.meta.url).href
  }

  const formatToDate=(date:any)=>{
    if(date){
      if(date=='1900-01-01 00:00:00'||date=='1900-01-01T00:00:00'){
        return '';
      }
      return moment(date).format('YYYY-MM-DD');
    } 
  }

  const formatToDateTime=(date:any)=>{ 
    if(date){
      if(date=='1900-01-01 00:00:00'||date=='1900-01-01T00:00:00'){
        return '';
      }
      return moment(date).format('YYYY-MM-DD HH:mm');
    } 
  }

  const formatToCustomDate=(date:any,format:string)=>{
    if(date){
      if(date=='1900-01-01 00:00:00'||date=='1900-01-01T00:00:00'){
        return '';
      }
      return moment(date).format(format);
    } 
  }

  const isDateBeforeToday=(date:string)=>{
    let someMoment = moment(date);
    let today = moment().format('YYYY-MM-DD');
    if(date==today){
      return false;
    }
    return someMoment.isBefore(today);
  }

  const stopKeyborad=(e:any)=>{
    e.target.setAttribute('readonly','readonly');
    setTimeout(() => {
        e.target.removeAttribute('readonly');
    }, 200);
}
  
export default{
  isMobile,
  setObjLocalStorage,
  getObjLocalStorage,
  formatToCustomDate,
  isDateBeforeToday,
  setLocalStorage,
  getLocalStorage,
  getImgUrl,
  formatToDate,
  formatToDateTime,
  stopKeyborad
}