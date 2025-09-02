import request from '@/utils/system/request'

// 分页查询操作日志
export function getLogs(pgSize:Number,pgIndex:Number,  orderFiled:string,  orderType:string,  searchKey:string, dateStart:string,  dateEnd:string) {
return request.get(`/LogInfo/GetLogs?pgSize=${pgSize}&pgIndex=${pgIndex}&orderFiled=${orderFiled}&orderType=${orderType}&searchKey=${searchKey}&dateStart=${dateStart}&dateEnd=${dateEnd}`)
}