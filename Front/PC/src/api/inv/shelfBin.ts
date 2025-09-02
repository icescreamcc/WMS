import request from '@/utils/system/request'

export function getWarehouseTree() {
return request.get(`/ShelfBin/GetWarehouseTree`);
}

export function getWarehouseDetail(warehouseId:string) {
return request.get(`/ShelfBin/GetWarehouseDetail?warehouseId=${warehouseId}`);
}

export function getShelfDetail(shelfId:string) {
return request.get(`/ShelfBin/GetShelfDetail?shelfId=${shelfId}`);
}
 
export function getBinDetail(binId:number) {
return request.get(`/ShelfBin/GetBinDetail?binId=${binId}`);
}
  
//添加货架
export function addShelf(data:any){
return request.post(`/ShelfBin/AddShelf`,data);
}

//修改货架
export function updateShelf(data:any){
return request.post(`/ShelfBin/UpdateShelf`,data);
}

//删除货架
export function delShelf(shelfId:string){
return request.get(`/ShelfBin/DelShelf?shelfId=${shelfId}`);
}

//添加货位
export function addBin(data:any){
return request.post(`/ShelfBin/AddBin`,data);
}

//修改货位
export function updateBin(data:any){
return request.post(`/ShelfBin/UpdateBin`,data);
}

//删除货位
export function delBin(binId:number){
return request.get(`/ShelfBin/DelBin?binId=${binId}`);
}