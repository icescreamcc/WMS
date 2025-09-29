using External.Common;
using Logic.BaseInfo;
using Logic.Inventory;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Logic.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.AutomationDevice;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Purchase;
using Models.Model.Sys;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filter;
using WebApi.Response;
using static NPOI.HSSF.Util.HSSFColor;

namespace WebApi.Controllers
{
    public class CommonController : AuthTokenController
    {
        private readonly SysArgsService _sysArgsHelper;

        private readonly SelectOptionsService _selectOptionsServer;

        private readonly IFileStorage _fileStorage;

        public CommonController(SysArgsService sysArgsHelper, SelectOptionsService selectOptionsServer, IFileStorage fileStorage)
        {
            _sysArgsHelper = sysArgsHelper;
            _selectOptionsServer = selectOptionsServer; 
            _fileStorage = fileStorage;
        }

        /// <summary>
        /// 获取系统版本号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<KeyValueModel> GetSysVersion()
        {
            return await _sysArgsHelper.GetValueByKey(BusinessConst.SysVersion);
        }

        /// <summary>
        /// 获取所有字典列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<Args>>  GetDictionarys()
        {
            return await _sysArgsHelper.GetDictionarys(); 
        }

        /// <summary>
        /// 根据参数Key获取字典选项
        /// </summary>
        /// <param name="argskey"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<Args> GetDictionaryOption(string argskey)
        {
            return await _sysArgsHelper.GetDictionaryOption(argskey); 
        }

        /// <summary>
        /// 获取字段类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetFieldTypes()
        {
            return await _selectOptionsServer.GetFieldTypes(); 
        }

         
        /// <summary>
        /// 获取所有省份
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetProvinces()
        {
            return await _selectOptionsServer.GetProvinces();  
        }

        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetCitysByProvince(int provinceId)
        {
            return await _selectOptionsServer.GetCitysByProvince(provinceId); 
        }

        /// <summary>
        /// 获取所有仓库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<WarehouseSimple>> GetWarehouses()
        {
            return await _selectOptionsServer.GetWarehouses(); 
        }

        /// <summary>
        /// 查询仓库中所有货架、货位
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<WarehouseElement>> GetElementByWarehouse(string warehouseId)
        {
            return await _selectOptionsServer.GetElementByWarehouse(warehouseId);
        }

        /// <summary>
        /// 根据仓库获取货架
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<WarehouseElement>> GetShelfByWarehouse(string warehouseId)
        {
            return await _selectOptionsServer.GetShelfByWarehouse(warehouseId);
        }

        /// <summary>
        /// 根据仓库获取库位
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<WarehouseElement>> GetBinByWarehouse(string warehouseId)
        {
            return await _selectOptionsServer.GetBinByWarehouse(warehouseId); 
        }

        /// <summary>
        /// 根据货架获取库位
        /// </summary>
        /// <param name="shelfId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<WarehouseElement>> GetBinByShelf(string shelfId)
        {
            return await _selectOptionsServer.GetBinByShelf(shelfId);
        }

        /// <summary>
        /// 根据货位获取料箱
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<WarehouseElement>> GetWorkbinCellsByBin(int binId)
        {
            return await _selectOptionsServer.GetWorkbinCellsByBin(binId);
        }

        /// <summary>
        /// 根据库位查询库存信息
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<StorageWarehouseDetail>> GetStorageDetailsByBin(int binId)
        {
            return await _selectOptionsServer.GetStorageDetailsByBin(binId);
        }

        [HttpGet]
        [Skip]
        public async Task<List<StorageWarehouseDetail>> GetStorageDetailsByShelf(string shelfId)
        {
            return await _selectOptionsServer.GetStorageDetailsByShelf(shelfId);
        }
        /// <summary>
        /// 获取料箱规格
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetWorkbinSpec()
        {
            return await _selectOptionsServer.GetWorkbinSpec();
        }

        /// <summary>
        /// 获取物品大类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public List<KeyValueModel> GetGoodsGroup()
        {
            return EnumHelper.GetEnumValNames<BaseTypeGroup>();
        }

        /// <summary>
        /// 获取物品小类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetGoodsClassify(string goodsGroup)
        {
            return await _selectOptionsServer.GetGoodsClassify(goodsGroup);
        }

        /// <summary>
        /// 根据关键字查询用户
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<User>> GetUserByKey(string keyword, int limit)
        {
            return await _selectOptionsServer.GetUserByKey(keyword, limit);
        }

        /// <summary>
        /// 根据关键字查询商品
        /// </summary>
        /// <param name="goodsClassify"></param>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<GoodsSimple>> GetGoodsByKey(string goodsClassify, string keyword, int limit)
        {
            return await _selectOptionsServer.GetGoodsByKey(goodsClassify,keyword, limit);
        }

        /// <summary>
        /// 根据关键字查询商品(模糊查询、大类、小类、是否在SAP)
        /// </summary> 
        /// <param name="goodsClassify"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="isSAP"></param>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<GoodsSimple>> GetGoodsByKeyAndClassify(string goodsClassify, int goodsClassifyId, string isSAP, string keyword, int limit = 40)
        {
            return await _selectOptionsServer.GetGoodsByKey(goodsClassify, goodsClassifyId,isSAP, keyword, limit);
        }

        /// <summary>
        /// 根据物料编码查询物料信息
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="goodsClassify"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<GoodsSimple> GetGoodsById(string goodsId, string goodsClassify)
        {
            return await _selectOptionsServer.GetGoodsById(goodsId, goodsClassify);
        }

        /// <summary>
        /// 根据关键字查询商品(模糊查询、大类、小类、区域)
        /// 同时进行智能排序：按top领用、最近领用记录排序
        /// </summary> 
        /// <param name="goodsClassify"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="area"></param>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<GoodsSimple>> GetGoodsAutoSort(string goodsClassify, int goodsClassifyId, string area, string keyword, int limit = 40)
        {
            return await _selectOptionsServer.GetGoodsAutoSort(goodsClassify, goodsClassifyId, area, keyword, limit);
        }

        [HttpGet]
        [Skip]
        public async Task<TableModel<GoodsSimple>> GetGoodsByPage(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsClassify, int goodsClassifyId, string area, string keyword)
        {
            return await _selectOptionsServer.GetGoodsByPage(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), goodsClassify, goodsClassifyId, area, keyword);
        }

        /// <summary>
        /// 根据关键字分页查询商品(模糊查询、大类、小类、是否在SAP)
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsClassify"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="isSAP"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<TableModel<GoodsSimple>> GetGoodsPageByKeyAndClassify(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsClassify, int goodsClassifyId, string isSAP, string keyword)
        {
          return await _selectOptionsServer.GetGoodsByKey(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), goodsClassify, goodsClassifyId, isSAP, keyword);
        }

      

        /// <summary>
        /// 根据关键字查询供应商
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<SupplierSimple>> GetSupplierByKey(string keyword, int limit)
        {
            return await _selectOptionsServer.GetSupplierByKey(keyword, limit); 
        }

        /// <summary>
        /// 根据单位类型获取单位
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetUnits(string unitType)
        {
            return await _selectOptionsServer.GetUnits(unitType);
        }

        /// <summary>
        /// 获取所有工厂区域
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetAreas()
        {
            return await _selectOptionsServer.GetAreas();
        }

        /// <summary>
        /// 获取所有产线
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetLines()
        {
            return await _selectOptionsServer.GetLines(BusinessConst.PlantNo);
        }

        /// <summary>
        /// 获取允许上传货品照片的上限数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetPhotoLimit()
        {
            var photoLimit = await _sysArgsHelper.GetValueByKey(BusinessConst.GoodsPhotoLimit);
            return photoLimit.Value;
        }

        /// <summary>
        /// 上传货品图片
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost]
        [Skip]
        public async Task<string> UploadGoodsPhoto()
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.Length / 1024 < 1024 * 10)
                {
                    if (file.ContentType.Contains("image"))
                    {
                        var fileName = $"SparePartPhoto{DateTime.Now.Ticks}.{file.FileName.Split('.')[1]}";
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

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost]
        [Skip]
        public async Task<KeyValueModel> UploadFile()
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                var fileName = file.FileName;
                var fileType = file.ContentType.Split('/')[1].ToUpper();
                var fileTypeEnums = Enum.GetNames(typeof(FileType));
                var fileEnumStr = fileTypeEnums.FirstOrDefault(w => w.ToUpper() == fileType.ToUpper());
                if(!string.IsNullOrEmpty(fileEnumStr))
                {
                    var fileTypeEnum = Enum.Parse<FileType>(fileEnumStr);
                    using (var stream = file.OpenReadStream())
                    {
                        string fileUrl = await _fileStorage.SaveFile(fileName, stream, fileTypeEnum);
                        return new KeyValueModel
                        {
                            Key = fileName,
                            Value = fileUrl
                        };
                    }
                }
                throw new BusinessException("上传的文件不在系统支持的范围");
            }
            throw new BusinessException("未获取到文件信息");
        }
    }
}
