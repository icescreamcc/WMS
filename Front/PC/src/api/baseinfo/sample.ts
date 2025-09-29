import request from '@/utils/system/request'

//获取商品类型所有数据
export function getSamplePieceClassifyData() {
return request.get(`/SamplePiece/GetSamplePieceClassifyData`);
}

//添加商品类型
export function addSamplePieceClassify(data:any){
return request.post(`/SamplePiece/AddSamplePieceClassify`,data);
}

//修改商品类型
export function updateSamplePieceClassify(data:any){
return request.post(`/SamplePiece/UpdateSamplePieceClassify`,data);
}

//删除商品类型
export function delSamplePieceClassify(typeId:string){
return request.get(`/SamplePiece/DelSamplePieceClassify?typeId=${typeId}`);
}
 
// 获取所有样件列表
export function getSamplePieceList(userId:string, pgSize:number,  pgIndex:number,  orderFiled:string,  orderType:string, typeId:number, searchKey:string) {
return request.get(`/SamplePiece/GetSamplePieceList?userId=${userId}&pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&typeId=${typeId}&searchKey=${searchKey}`)
}

//获取样件表启用的闲置字段信息
export function getEnableSpareFields() {
return request.get(`/SamplePiece/GetEnableSpareFields`);
}

//获取样件详细信息
export function getSamplePieceDetail(sampleId:string) {
return request.get(`/SamplePiece/GetSamplePieceDetail?sampleId=${sampleId}`);
}

//获取样件相关选项的数据
export function getOptions() {
return request.get(`/SamplePiece/GetOptions`);
}
   
//添加样件
export function addSamplePiece(data:any){
return request.post(`/SamplePiece/AddSamplePiece`,data);
}

//修改样件
export function updateSamplePiece(data:any){
return request.post(`/SamplePiece/UpdateSamplePiece`,data);
}

//删除样件
export function delSamplePiece(data:Array<any>){
return request.post(`/SamplePiece/DelSamplePiece`,data);
}

 
//导出样件
export function exportSamplePiece( searchKey:string, orderField:string,  orderType:string,goodsClassifyId:number, fields:Array<any>){
return request.put(`/SamplePiece/ExportSamplePiece?searchKey=${searchKey}&orderField=${orderField}&orderType=${orderType}&goodsClassifyId=${goodsClassifyId}`,fields);
}

//获取当前用户导出用户列表被允许的字段
export function getAllowField() {
return request.get(`/SamplePiece/GetAllowField`);
}
