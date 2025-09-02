import request from '@/utils/system/request'

//查询所有物品列表
export function getGoodsByKey( pgSize:Number,  pgIndex:Number, orderFiled:string,  orderType:string, goodsGroup:string,  goodsClassifyId:number,  keyword:string) {
return request.get(`/InStorageLabels/GetGoodsByKey?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&goodsGroup=${goodsGroup}&goodsClassifyId=${goodsClassifyId}&keyword=${keyword}`)
}

export function getWorkbinRecommend(goodsId:string) {
return request.get(`/InStorageLabels/GetWorkbinRecommend?goodsId=${goodsId}`)
}

export function getUnSubmitCodes(goodsId:string) {
return request.get(`/InStorageLabels/GetUnSubmitCodes?goodsId=${goodsId}`)
}

export function scanBinCheck(scanNo:string, goodsId:string) {
return request.get(`/InStorageLabels/ScanBinCheck?scanNo=${scanNo}&goodsId=${goodsId}`)
}

export function submitScan(data:any) {
return request.post(`/InStorageLabels/SubmitScan`,data)
}

export function submitCode(data:any) {
return request.post(`/InStorageLabels/SubmitCode`,data)
}