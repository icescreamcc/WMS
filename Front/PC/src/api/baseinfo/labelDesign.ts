import request from '@/utils/system/request'
 
export function getLabelDesign(orderFiled:string,  orderType:string,goodsClassifyGroup:string, searchKey:string) {
return request.get(`/MaterialLabelDesign/GetLabelDesign?orderFiled=${orderFiled}&orderType=${orderType}&goodsClassifyGroup=${goodsClassifyGroup}&searchKey=${searchKey}`)
}
    
export function getLableDesignDetails(labelId:string){
return request.get(`/MaterialLabelDesign/GetLableDesignDetails?labelId=${labelId}`);
}

export function getOptions(){
return request.get(`/MaterialLabelDesign/GetOptions`);
}
 
export function addLabelDesign(data:any){
return request.post(`/MaterialLabelDesign/AddLabelDesign`,data);
}
 
export function updateLabelDesign(data:any){
return request.post(`/MaterialLabelDesign/UpdateLabelDesign`,data);
}

export function updateLabelDeft(labelId:string,goodsClassifyGroup:string){
return request.get(`/MaterialLabelDesign/UpdateLabelDeft?labelId=${labelId}&goodsClassifyGroup=${goodsClassifyGroup}`);
}
 
export function delLabelDesign(labelId:string){
return request.get(`/MaterialLabelDesign/DelLabelDesign?labelId=${labelId}`);
}
 
