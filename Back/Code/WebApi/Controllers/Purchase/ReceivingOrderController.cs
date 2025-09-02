using DbRepository.Repository.DbModels;
using External.Common;
using Logic.BaseInfo;
using Logic.Inventory;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.Purchase;
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
using WebApi.Response;

namespace WebApi.Controllers.Purchase
{
    public class ReceivingOrderController : AuthTokenController
    {
        private readonly ReceivingOrderMgr _receivingOrderMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private readonly SysArgsService _sysArgsHelper;
        private readonly GoodsMgr _goodsMgr;

        private const string _moduleName = "收货计划";

        public ReceivingOrderController(ReceivingOrderMgr receivingOrderMgr, GoodsMgr goodsMgr, SelectOptionsService selectOptionsServer, SysArgsService sysArgsHelper)
        {
            _receivingOrderMgr = receivingOrderMgr;
            _selectOptionsServer = selectOptionsServer;
            _sysArgsHelper = sysArgsHelper;
            _goodsMgr = goodsMgr;
        }

        /// <summary>
        /// 收货单分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看收货单", LogType.Read, _moduleName)]
        public async Task<TableModel<ReceivingOrderExpandDto>> GetOrders(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, string goodsGroup,string receivingLevel, string createUserName, string detailStatus)
        {
            return await _receivingOrderMgr.GetOrders(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType, false), searchKey, dateStart, dateEnd, goodsGroup, receivingLevel, createUserName, detailStatus);
        }

        [HttpGet]
        [Skip]
        public async Task<ReceivingOrderDto> GetOrderDetail(string orderNo, string userId)
        {
            return await _receivingOrderMgr.GetOrderDetail(orderNo, userId);
        }
        /// <summary>
        /// 优先级
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public List<KeyValueModel> GetReceivingLevelGroup()
        {
            return EnumHelper.GetEnumValNames<ReceivingLevel>();
        }
        /// <summary>
        /// 计划员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetCreateUserNameGroup()
        {
            var data =await _receivingOrderMgr.GetCreateUserNameGroup();
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
            return EnumHelper.GetEnumValNames<ReceivingOrderDetailStatus>();
        }

        /// <summary>
        /// 获取收货单相关参数数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetArgs()
        {
            var hasApproval = bool.Parse((await _sysArgsHelper.GetValueByKey(BusinessConst.IsReceivingApproval)).Value.ToString()); 
            var receivingAbnormalTypeData = await _selectOptionsServer.GetDictionaryOption(BusinessConst.ReceivingAbnormalType);
            return new { HasApproval = hasApproval, ReceivingAbnormalTypeOptions = receivingAbnormalTypeData, };
        }
         

        /// <summary>
        /// 获取收货单相关选项及参数数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var unitData = await _selectOptionsServer.GetUnits();
            var receivingLevelData = EnumHelper.GetEnumValNames<ReceivingLevel>();
            var receivingAddress = (await _sysArgsHelper.GetValueByKey(BusinessConst.ReceivingAddress)).Value.ToString();
            var goodsClassifyData = EnumHelper.GetEnumValNames<BaseTypeGroup>();
            return new
            {
                UnitOptions = unitData,
                ReceivingLevelOptions = receivingLevelData, 
                ReceivingAddress = receivingAddress, 
                GoodsClassifyOptions= goodsClassifyData
            };
        }

        /// <summary>
        /// 查询审批明细
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<ApprovalHisModel>> GetApprovalHis(string orderNo)
        {
            return await _receivingOrderMgr.GetApprovalHis(orderNo);
        }

        ///// <summary>
        ///// 修改异常到货
        ///// </summary>
        ///// <param name="detailId"></param>
        ///// <param name="receivingAbnormalType"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task UpdateReceivingAbnormal(int detailId, string receivingAbnormalType, string receivingAbnormalDesc)
        //{
        //    await _receivingOrderMgr.UpdateReceivingAbnormal(detailId, receivingAbnormalType, receivingAbnormalDesc);
        //}

        /// <summary>
        /// 修改紧急数量
        /// </summary>
        /// <param name="detailId"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task UpdateReceivingUrgency(int detailId, float qty)
        {
            await _receivingOrderMgr.UpdateReceivingUrgency(detailId, qty);
        }

        /// <summary>
        /// 添加收货单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加收货单", LogType.Add, _moduleName)]
        public async Task AddReceivingOrder(ReceivingOrderDto data)
        {
            await _receivingOrderMgr.AddReceivingOrder(data);
        }

        /// <summary>
        /// 修改仓库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改收货单", LogType.Update, _moduleName)]
        public async Task UpdateReceivingOrder(ReceivingOrderDto data)
        {
            await _receivingOrderMgr.UpdateReceivingOrder(data);
        }

        /// <summary>
        /// 删除收货单
        /// </summary>
        /// <param name="detailsId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除收货单", LogType.Del, _moduleName)]
        public async Task DelReceivingOrder(int[] detailsId)
        {
            await _receivingOrderMgr.DelReceivingOrder(detailsId);
        }

        /// <summary>
        /// 发送邮件通知收货
        /// </summary>
        /// <param name="data"></param>
        /// <param name="orderNo"></param>
        [HttpPut] 
        public void AdviceReceiving( string orderNo, MailModel data)
        {
            _receivingOrderMgr.AdviceReceiving(data, orderNo);
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="detailsId"></param>        
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("确认收货", LogType.Other, _moduleName)]
        public async Task SubmitReceived(List<ReceivingActualDto> data)
        {
          await _receivingOrderMgr.SubmitReceived(data);
        }

        /// <summary>
        /// 审批入库单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="isApprove"></param>
        /// <param name="opinion"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("审批收货计划", LogType.Update, _moduleName)]
        public async Task ApprovalReceivingOrder(string[] orderNo, bool isApprove, string opinion, string userId, string userName)
        {
            await _receivingOrderMgr.ApprovalReceivingOrder(orderNo, isApprove, opinion, userId, userName);
        }

        [HttpGet, Skip]
        public List<FieldModel> GetExportFields()
        {
            return _receivingOrderMgr.GetExportFields();
        }

        [HttpGet,Skip]
        public async Task<string> CreateImportTemplate()
        {
            return await _receivingOrderMgr.CreateImportTemplate();
        }

        [HttpPost, BusinessLog("导入收货计划", LogType.Import, _moduleName)] 
        public async Task ImportReceivingData()
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
                            await _receivingOrderMgr.ImportReceivingData(uid, fileExtension, stream);
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

        [HttpPut, BusinessLog("导出收货计划", LogType.Export, _moduleName)]
        public async Task<string> ExportReceivingData(string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, string goodsGroup, List<KeyValueModel> fields)
        {
            return await _receivingOrderMgr.ExportReceivingData(orderFiled,  ConvertOrderType(orderType,false),  searchKey,  dateStart,  dateEnd,  goodsGroup,fields);
        }

        [HttpPost, BusinessLog("上传收货计划异常附件", LogType.Import, _moduleName)]
        public async Task UploadReceivingDocument(string orderNo)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.Length / 1024 < 1024 * 1024)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uid = HttpContext.Request.Headers.SingleOrDefault(w => w.Key == "userId").Value;
                        await _receivingOrderMgr.UploadReceivingDocument(uid, orderNo, file.FileName, stream);
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
        public async Task<TableModel<BaseFilesDto>> GetBaseFiles(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string orderNo, string fileInfoType)
        {
            return await _goodsMgr.GetBaseFiles(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType, false), searchKey, orderNo, fileInfoType);
        }
        /// <summary>
        /// 删除收货计划图片
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        [HttpPost, BusinessLog("删除收货计划图片", LogType.Del, _moduleName)]
        public async Task DelSendingFiles(int[] fileIds)
        {
            await _goodsMgr.DelBaseFiles(fileIds);
        }


    }
}
