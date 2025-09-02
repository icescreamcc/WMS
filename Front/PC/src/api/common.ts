import request from '@/utils/system/request'

// 获取系统版本号
export function getSysVersion(argskey:string) {
return request.get(`/Common/GetSysVersion`)
}
    
// 根据参数Key获取字典选项
export function getDictionaryOption(argskey:string) {
return request.get(`/Common/GetDictionaryOption?argskey=${argskey}`)
}

//获取所有字典列表
export function getDictionarys() {
return request.get(`/Common/GetDictionarys`)
}

//获取字段类型列表
export function getFieldTypes() {
return request.get(`/Common/GetFieldTypes`)
}

//获取所有省份列表
export function getProvinces() {
return request.get(`/Common/GetProvinces`)
}

//根据省份获取城市
export function getCitysByProvince(provinceId:string) {
return request.get(`/Common/GetCitysByProvince?provinceId=${provinceId}`)
}

//获取所有仓库
export function getWarehouses() {
return request.get(`/Common/GetWarehouses`)
}

//查询仓库中所有货架、货位
export function getElementByWarehouse(warehouseId:string) {
return request.get(`/Common/GetElementByWarehouse?warehouseId=${warehouseId}`)
}

//根据仓库获取库位
export function getShelfByWarehouse(warehouseId:string) {
return request.get(`/Common/GetShelfByWarehouse?warehouseId=${warehouseId}`)
}

//根据货架获取库位
export function getBinByShelf(shelfId:string) {
return request.get(`/Common/GetBinByShelf?shelfId=${shelfId}`)
}

//根据货位获取料箱
export function getWorkbinCellsByBin(binId:number) {
return request.get(`/Common/GetWorkbinCellsByBin?binId=${binId}`)
}

//根据库位查询库存信息
export function getStorageDetailsByBin(binId:number) {
return request.get(`/Common/GetStorageDetailsByBin?binId=${binId}`)
}

//根据货架查询库存信息
export function getStorageDetailsByShelf(shelfId:string) {
return request.get(`/Common/GetStorageDetailsByShelf?shelfId=${shelfId}`)
}
 
//根据仓库获取库位v
export function getBinByWarehouse(warehouseId:string) {
return request.get(`/Common/GetBinByWarehouse?warehouseId=${warehouseId}`)
}

//获取料箱规格
export function getWorkbinSpec(){
    return request.get(`/Common/GetWorkbinSpec`)
}
 
//获取物品大类
export function getGoodsGroup() {
return request.get(`/Common/GetGoodsGroup`)
}

//获取物品小类
export function getGoodsClassify(goodsGroup:string) {
return request.get(`/Common/GetGoodsClassify?goodsGroup=${goodsGroup}`)
}

//根据单位类型获取单位
export function getUnits(unitType:string) {
return request.get(`/Common/GetUnits?unitType=${unitType}`)
}

//获取所有产线
export function getAreas() {
return request.get(`/Common/GetAreas`)
}

//获取所有产线
export function getLines() {
return request.get(`/Common/GetLines`)
}

//根据关键字查询用户
export function getUserByKey( keyword:string,limit=20) {
return request.get(`/Common/GetUserByKey?keyword=${keyword}&limit=${limit}`)
}

//根据关键字查询物品
export function getGoodsByKey(goodsClassify:string, keyword:string,limit=20) {
return request.get(`/Common/GetGoodsByKey?goodsClassify=${goodsClassify}&keyword=${keyword}&limit=${limit}`)
}

//根据关键字查询商品(模糊查询、大类、小类、是否在SAP)
export function getGoodsByKeyAndClassify(goodsClassify:string,goodsClassifyId:number, isSAP:string, keyword:string,limit:number) {
return request.get(`/Common/GetGoodsByKeyAndClassify?goodsClassify=${goodsClassify}&goodsClassifyId=${goodsClassifyId}&isSAP=${isSAP}&keyword=${keyword}&limit=${limit}`)
}

//根据关键字查询商品(模糊查询、大类、小类、是否在SAP)
export function getGoodsAutoSort(goodsClassify:string,goodsClassifyId:number, area:string, keyword:string,limit:number) {
return request.get(`/Common/GetGoodsAutoSort?goodsClassify=${goodsClassify}&goodsClassifyId=${goodsClassifyId}&area=${area}&keyword=${keyword}&limit=${limit}`)
}

//商品分页查询
export function getGoodsByPage(pgSize:number, pgIndex:number,  orderFiled:string,  orderType:string,goodsClassify:string,goodsClassifyId:number, area:string, keyword:string) {
return request.get(`/Common/GetGoodsByPage?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType${orderType}&goodsClassify=${goodsClassify}&goodsClassifyId=${goodsClassifyId}&area=${area}&keyword=${keyword}`)
}

//根据关键字分页查询商品(模糊查询、大类、小类、是否在SAP)
export function getGoodsPageByKeyAndClassify( pgSize:Number,  pgIndex:Number,  orderFiled:string,  orderType:string, goodsClassify:string,goodsClassifyId:number, isSAP:string, keyword:string,limit=40) {
return request.get(`/Common/GetGoodsPageByKeyAndClassify?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&goodsClassify=${goodsClassify}&goodsClassifyId=${goodsClassifyId}&isSAP=${isSAP}&keyword=${keyword}&limit=${limit}`)
}

//根据物料编码查询物料信息
export function GetGoodsById(goodsId:string,goodsClassify:string) {
return request.get(`/Common/GetGoodsById?goodsId=${goodsId}&goodsClassify=${goodsClassify}`)
}

//根据关键字查询供应商
export function getSupplierByKey(keyword:string,limit=20) {
return request.get(`/Common/GetSupplierByKey?keyword=${keyword}&limit=${limit}`)
}
 
//获取允许上传照片的上限数
export function getPhotoLimit() {
return request.get(`/Common/GetPhotoLimit`);
}
 