import request from '@/utils/system/request'

export function getRequisitionData(goodsClassifyGroup:string,  goodsClassifyId:number,  searchKey:string) {
return request.get(`/WarehousePickDashboard/GetData?goodsClassifyGroup=${goodsClassifyGroup}&goodsClassifyId=${goodsClassifyId}&searchKey=${searchKey}`)
}