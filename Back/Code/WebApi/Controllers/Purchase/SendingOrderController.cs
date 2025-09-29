using External.Common;
using Logic.BaseInfo;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Logic.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model.Purchase;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.Purchase
{
    public class SendingOrderController : AuthTokenController
    {
        private readonly SendingOrderMgr _sendingOrderMgr;
        private readonly GoodsMgr _goodsMgr;
        private readonly SupplierMgr _supplierMgr;
        

        private readonly SelectOptionsService _selectOptionsServer;

        private const string _moduleName = "发货计划";
        private readonly IFileStorage _fileStorage;

        public SendingOrderController(SendingOrderMgr sendingOrderMgr, GoodsMgr goodsMgr, SelectOptionsService selectOptionsServer, SupplierMgr supplierMgr, IFileStorage fileStorage)
        {
            _sendingOrderMgr = sendingOrderMgr; 
            _goodsMgr = goodsMgr;
            _selectOptionsServer = selectOptionsServer;
            _supplierMgr = supplierMgr;
            _fileStorage = fileStorage;
        }

        /// <summary>
        /// 发货单分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看发货单", LogType.Read, _moduleName)]
        public async Task<TableModel<SendingOrderExpandDto>> GetSending(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, string goodsGroup,string isUrgentShipment, string sendingAddress, string detailStatus)
        {
            return await _sendingOrderMgr.GetSending(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType, false), searchKey, dateStart, dateEnd, goodsGroup, isUrgentShipment, sendingAddress, detailStatus);
        }

        [HttpGet]
        [Skip]
        public async Task<List<SendingOrderExpandDto>> GetOrderDetail(string userId,string orderNo)
        {
            return await _sendingOrderMgr.GetOrderDetail(userId,orderNo);
        }
        /// <summary>
        /// 是否紧急发货/是否有足够库存
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public List<KeyValueModel> GetYesOrNoGroup()
        {
            return EnumHelper.GetEnumValNames<YesOrNo>();
        }
        /// <summary>
        /// 计划员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetCreateUserNameGroup()
        {
            var data =await _sendingOrderMgr.GetCreateUserNameGroup();
            return data;
        }
        // <summary>
        /// 到货地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetSendingAddressGroup()
        {
            var data = await _sendingOrderMgr.GetSendingAddressGroup();
            return data;
        }
        /// <summary>
        /// 单据状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public List<KeyValueModel> GetDetailStatusGroup()
        {
            return EnumHelper.GetEnumValNames<SendingOrderStatus>();
        }

        /// <summary>
        /// 获取发货单相关选项及参数数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var unitData = await _selectOptionsServer.GetUnits();
            var yesOrNoData = EnumHelper.GetEnumValNames<YesOrNo>();
            var goodsClassifyData = EnumHelper.GetEnumValNames<BaseTypeGroup>();
            var supplierData = _supplierMgr.GetSupplierBySupplierTypeId(121).Result;
            return new
            {
                UnitOptions = unitData,
                YesOrNoDataOptions = yesOrNoData, 
                GoodsClassifyOptions= goodsClassifyData,
                SupplierOptions = supplierData
            };
        }

        /// <summary>
        /// 添加发货单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加发货单", LogType.Add, _moduleName)]
        public async Task AddSending(SendingOrderDto data)
        {
            await _sendingOrderMgr.AddSending(data);
        }

        /// <summary>
        /// 修改发货单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改发货单", LogType.Update, _moduleName)]
        public async Task UpdateSending(SendingOrderDto data)
        {
            await _sendingOrderMgr.UpdateSending(data);
        }

        /// <summary>
        /// 删除发货单
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除发货单", LogType.Del, _moduleName)]
        public async Task DelSending(string[] orderNos)
        {
            await _sendingOrderMgr.DelSending(orderNos);
        }

        [HttpGet, Skip]
        public List<FieldModel> GetExportFields()
        {
            return _sendingOrderMgr.GetExportFields();
        }

        [HttpGet,Skip]
        public async Task<string> CreateImportTemplate()
        {
            return await _sendingOrderMgr.CreateImportTemplate();
        }

        [HttpPost, BusinessLog("导入发货计划", LogType.Import, _moduleName)] 
        public async Task ImportSendingData()
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.Length / 1024 < 1024 * 1024)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    { 
                        using (var stream = file.OpenReadStream())
                        {
                            var uid = HttpContext.Request.Headers.SingleOrDefault(w => w.Key == "userId").Value; 
                            await _sendingOrderMgr.ImportSendingData(uid, fileExtension, stream);
                        }
                    }
                    else
                    {
                        throw new BusinessException("导入文件必须是Excel格式");
                    }

                }
                else
                {
                    throw new BusinessException("文件不能大于1G");
                }

            }
            else
            {
                throw new BusinessException("未获取到文件信息");
            }

        }

        [HttpPut, BusinessLog("导出发货计划", LogType.Export, _moduleName)]
        public async Task<string> ExportSendingData(string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, string goodsGroup, List<KeyValueModel> fields)
        {
            return await _sendingOrderMgr.ExportSendingData(orderFiled,  ConvertOrderType(orderType,false),  searchKey,  dateStart,  dateEnd,  goodsGroup,fields);
        }

        [HttpPost, BusinessLog("上传发货计划附件", LogType.Import, _moduleName)]
        public async Task UploadSendingDocument(string orderNo)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.Length / 1024 < 1024 * 1024)
                {
                    //var fileExtension = Path.GetExtension(file.FileName);
                    //if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    //{
                        using (var stream = file.OpenReadStream())
                        {
                            var uid = HttpContext.Request.Headers.SingleOrDefault(w => w.Key == "userId").Value;
                            await _sendingOrderMgr.UploadSendingDocument(uid, orderNo, file.FileName, stream);
                        }
                    //}
                    //else
                    //{
                    //    throw new BusinessException("导入文件必须是Excel格式");
                    //}
                }
                else
                {
                    throw new BusinessException("文件不能大于1G");
                }

            }
            else
            {
                throw new BusinessException("未获取到文件信息");
            }

        }


        /// <summary>
        /// 获取文件存储信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="orderNo"></param>
        /// <param name="fileInfoType"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<TableModel<BaseFilesDto>> GetBaseFiles(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey,string orderNo, string fileInfoType)
        {
            return await _goodsMgr.GetBaseFiles(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType, false), searchKey, orderNo, fileInfoType);
        }

        /// <summary>
        /// 删除发货计划附件
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        [HttpPost, BusinessLog("删除发货计划附件", LogType.Del, _moduleName)]
        public async Task DelSendingFiles(int[] fileIds)
        {
            await _goodsMgr.DelBaseFiles(fileIds);
        }

        /// <summary>
        /// 发送邮件通知
        /// </summary>
        /// <param name="data"></param>
        /// <param name="orderNo"></param>
        [HttpPut]
        [BusinessLog("发送邮件通知", LogType.Update, _moduleName)]
        public void AdviceSending(string orderNo, MailModel data)
        {
            _sendingOrderMgr.AdviceSending(data, orderNo);
        }

        [HttpPost]
        [Skip]
        public async Task<string> UploadInstorgePic()
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.Length / 1024 < 1024 * 10)
                {
                    if (file.ContentType.Contains("image"))
                    {
                        var fileName = $"SendingOrder{DateTime.Now.Ticks}.{file.FileName.Split('.')[1]}";
                        using (var stream = file.OpenReadStream())
                        {
                            string fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Image);
                            return fileUrl;
                        }
                    }
                    throw new BusinessException("上传文件不属于图片类型");
                }
                throw new BusinessException("图片不能大于5M");
            }
            throw new BusinessException("未获取到文件信息");
        }

        [HttpPost]
        [BusinessLog("扫描二维码,发货单", LogType.Update, _moduleName)]
        public async void ConfirmSendingAndOutStorage(SendingOrderDto data)
        {
            await _sendingOrderMgr.ConfirmSendingAndOutStorage(data);
        }

    }
}
