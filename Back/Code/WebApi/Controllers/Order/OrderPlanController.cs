using DbRepository.Repository.DbModels;
using External.Common;
using Logic.BaseInfo;
using Logic.Inventory;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Logic.Order;
using Logic.Purchase;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model.Order;
using Models.Model.Purchase;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filter;
using WebApi.Response;

namespace WebApi.Controllers.Order
{
    public class OrderPlanController : AuthTokenController
    {
        private readonly OrderPlanMgr _orderPlanMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private readonly SysArgsService _sysArgsHelper;
        //private readonly GoodsMgr _goodsMgr;

        private const string _moduleName = "收货计划";

        public OrderPlanController(OrderPlanMgr orderPlanMgr, SelectOptionsService selectOptionsServer, SysArgsService sysArgsHelper)
        {
            _orderPlanMgr = orderPlanMgr;
            _selectOptionsServer = selectOptionsServer;
            _sysArgsHelper = sysArgsHelper;
            //_goodsMgr = goodsMgr;
        }

        /// <summary>
        /// 收货单分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看收货单", LogType.Read, _moduleName)]
        public async Task<TableModel<OrderPlanDto>> GetOrders(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            return await _orderPlanMgr.GetOrder(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType, false), searchKey);
        }


        /// <summary>
        /// 获取di单相关选项及参数数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var unitData = await _orderPlanMgr.GetUnits();
            var userData = await _orderPlanMgr.GetSupplier();
            var goodsNameData = await _orderPlanMgr.GetBaseGoods();
           
            return new
            {
                UnitOptions = unitData,
                UserOptions = userData,
                GoodsNameOptions = goodsNameData,
            };
        }

        /// <summary>
        /// 添加订单计划
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加订单计划", LogType.Add, _moduleName)]
        public async Task AddOrderPlan(OrderPlanDto data)
        {
            await _orderPlanMgr.AddOrderPlan(data);
        }

        /// <summary>
        /// 修改订单计划
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改订单计划", LogType.Update, _moduleName)]
        public async Task UpdateOrderPlan(OrderPlanDto data)
        {
            await _orderPlanMgr.UpdateOrderPlan(data);
        }

        /// <summary>
        /// 删除订单计划
        /// </summary>
        /// <param name="detailsId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除订单计划", LogType.Del, _moduleName)]
        public async Task DelOrderPlan(string[] Ids)
        {
            await _orderPlanMgr.DelOrderPlan(Ids);
        }

        [HttpPost]
        [Skip]
        public async Task<string> UploadSparePartPhoto()
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.Length / 1024 < 1024 * 100)
                {
                    if (file.ContentType.Contains("image"))
                    {
                        var fileName = $"SparePartPhoto{DateTime.Now.Ticks}.{file.FileName.Split('.')[1]}";
                        using (var stream = file.OpenReadStream())
                        {
                            string fileUrl = await _orderPlanMgr.UploadSparePartPhoto(fileName, stream);
                            return fileUrl;
                        }
                    }
                    else
                    if (file.ContentType.Contains("pdf"))
                    {
                        var fileName = $"SparePartPhoto{DateTime.Now.Ticks}.{file.FileName.Split('.')[1]}";
                        using (var stream = file.OpenReadStream())
                        {
                            string fileUrl = await _orderPlanMgr.UploadSparePartPdf(fileName, stream);
                            return fileUrl;
                        }
                    }
                    else {
                        throw new BusinessException("上传文件不属于图片类型或PDF");
                    }
                    
                }
                throw new BusinessException("图片不能大于5M");
            }
            throw new BusinessException("未获取到文件信息");
        }

        ///// <summary>
        ///// 上传文件
        ///// </summary>
        ///// <returns></returns>
        ///// <exception cref="BusinessException"></exception>
        //[HttpPost]
        //[Skip]
        //public async Task<string> UploadFile(string ids, string user)
        //{
        //    var files = HttpContext.Request.Form.Files;
        //    if (files.Count > 0)
        //    {
        //        var file = files[0];
        //        if (file.Length / 1024 < 1024 * 1024 * 5)
        //        {
        //            var fileExtension = Path.GetExtension(file.FileName);
        //            if (fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension == ".doc" || fileExtension == ".docx" || fileExtension == ".pdf" || fileExtension == ".mp4" || fileExtension == ".MP4")
        //            {

        //                var fileName = file.FileName;


        //                using (var stream = file.OpenReadStream())
        //                {

        //                    //上传文件
        //                    string fileUrl = await _orderPlanMgr.UploadFiles(fileName, fileExtension, stream);
        //                    //上传对应PDF文件
        //                    return fileUrl;
        //                }
        //            }
        //            else
        //            {
        //                throw new BusinessException("上传文件必须是Excel,Word,PDF,MP4格式");
        //            }
        //        }
        //        else
        //        {
        //            throw new BusinessException("文件不能大于1G");
        //        }
        //    }
        //    else
        //    {
        //        throw new BusinessException("未获取到文件信息");
        //    }
        //}

    }
}
