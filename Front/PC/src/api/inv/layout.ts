import request from '@/utils/system/request'

//仓库、货架、库位树形结构数据查看
export function getWarehouseTree() {
return request.get(`/WarehouseLayout/GetWarehouseTree`);
}

//获取仓库下的所有货架或货位
export function getElementByWarehouse(warehouseId:string) {
return request.get(`/WarehouseLayout/GetElementByWarehouse?warehouseId=${warehouseId}`);
}

//获取货架下所有货位
export function getElementByShelf(shelfId:string) {
return request.get(`/WarehouseLayout/GetElementByShelf?shelfId=${shelfId}`);
}

//获取货位对应的料箱单元格
export function getWorkbinCells(binId:number) {
return request.get(`/WarehouseLayout/GetWorkbinCells?binId=${binId}`);
}

//根据库位查询存放信息
export function getStockInfoByBin(binId:number) {
return request.get(`/WarehouseLayout/GetStockInfoByBin?binId=${binId}`);
}

//解锁库位
export function unlockBin(binId:number,userName:string) {
return request.get(`/WarehouseLayout/UnlockBin?binId=${binId}&userName=${userName}`);
}