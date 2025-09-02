import request from '@/utils/system/request'
 
export function getLabels(goodsClassifyGroup:string) {
return request.get(`/LabelPrint/GetLabels?goodsClassifyGroup=${goodsClassifyGroup}`)
}

export function getLableDesignDetails(labelId:string) {
return request.get(`/LabelPrint/GetLableDesignDetails?labelId=${labelId}`)
}
   
export function addPrintRecord(data:any){
return request.post(`/LabelPrint/AddPrintRecord`,data);
}
  
export function getLabelRecord( pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string, goodsClassifyGroup:string, searchKey:string,dateStart:string,dateEnd:string) {
return request.get(`/LabelPrint/GetLabelRecord?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&goodsClassifyGroup=${goodsClassifyGroup}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}`)
}
 
