import request from '@/utils/system/request';

// 获取所有备件列表 
export function getGoodsList(pgSize:number,  pgIndex:number,  orderFiled:string,  orderType:string,goodsGroup:string,  goodsClassifyId:number,shelfId:string,binId:number, searchKey:string) {
return request.get(`/Goods/GetGoodsList?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&goodsGroup=${goodsGroup}&goodsClassifyId=${goodsClassifyId}&shelfId=${shelfId}&binId=${binId}&searchKey=${searchKey}`)
}

//获取备件详细信息
export function getGoodsDetail(goodsId:string) {
return request.get(`/Goods/GetGoodsDetail?goodsId=${goodsId}`);
}

//修改备件图片
export function updateGoodsPhoto(goodsId:string, data:any){
return request.put(`/Goods/UpdateGoodsPhoto?goodsId=${goodsId}`,data);
}

//获取货品分类关系图
export function getGoodsTreeMapData( classifyGroup:string) {
return request.get(`/Goods/GetGoodsTreeMapData?classifyGroup=${classifyGroup}`);
}

//删除附件
export function delFiles(url:string, data:Array<number>){
return request.post(url,data);
}

