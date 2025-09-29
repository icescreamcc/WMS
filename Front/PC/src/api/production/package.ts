import request from '@/utils/system/request'

// 配对信息分页查询
export function getMatchingInfo( pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string,isPackage:boolean) {
return request.get(`/ProductPackage/GetMatchingInfo?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&isPackage=${isPackage}`)
} 

//包装完成提交
export function submitMatchPackage( userName:string,matchingCode:string) {
return request.get(`/ProductPackage/SubmitMatchPackage?userName=${userName}&matchingCode=${matchingCode}`)
} 