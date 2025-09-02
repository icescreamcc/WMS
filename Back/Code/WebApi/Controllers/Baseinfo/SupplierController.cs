using DbRepository.Repository.DbModels;
using Logic.BaseInfo;
using Logic.LogicBase;
using Logic.LogicCommon; 
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Baseinfo;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filter; 

namespace WebApi.Controllers.Baseinfo
{ 
    public class SupplierController : AuthTokenController
    {

        private readonly SupplierMgr _supplierMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private const string _moduleName = "供应商信息管理";

        public SupplierController(SupplierMgr supplierMgr, SelectOptionsService selectOptionsServer)
        {
            _supplierMgr = supplierMgr;
            _selectOptionsServer = selectOptionsServer; 
        }

        /// <summary>
        /// 获取所有供应商列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看供应商信息", Models.Model.Enum.LogType.Read, _moduleName)]
        public async Task<TableModel<SupplierDto>> GetSuppliers(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            return await _supplierMgr.GetSuppliers(userId, pgSize,  pgIndex,  orderFiled,  ConvertOrderType(orderType),  searchKey); 
        }

        /// <summary>
        /// 获取供应商表启用的闲置字段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldsManage>> GetEnableSpareFields()
        {
            return await _supplierMgr.GetEnableSpareFields(typeof(BaseSuppliers).Name);  
        }

        /// <summary>
        /// 获取供应商明细
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<SupplierDto> GetSupplierDetail(string supplierId)
        {
            return await _supplierMgr.GetSupplierDetail(supplierId); 
        }

        /// <summary>
        /// 获取供应商相关选项的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var supplierType = await _selectOptionsServer.GetDictionaryOption(BusinessConst.SupplierType);
            var supplierProperty = await _selectOptionsServer.GetDictionaryOption(BusinessConst.SupplierProperty); 
            var creditType = await _selectOptionsServer.GetDictionaryOption(BusinessConst.CreditType);
            var provinceOptions = await _selectOptionsServer.GetProvinces();
            return new { SupplierTypeOptions = supplierType, SupplierCreditTypeOptions = creditType, SupplierPropertyOptions= supplierProperty,  ProvinceOptions = provinceOptions }; 
        }
         

        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加供应商", Models.Model.Enum.LogType.Add, _moduleName)]
        public async Task AddSupplier(SupplierDto data)
        {
            await _supplierMgr.AddSupplier(data); 
        }

        /// <summary>
        /// 修改供应商
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改供应商", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateSupplier(SupplierDto data)
        {
             await _supplierMgr.UpdateSupplier(data); 
        }

        /// <summary>
        /// 删除供应商
        /// </summary>
        /// <param name="suppliersId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除供应商", Models.Model.Enum.LogType.Del, _moduleName)]
        public async Task DelSupplier(string[] suppliersId)
        {
            await _supplierMgr.DelSupplier(suppliersId); 
        }

        /// <summary>
        /// 导出供应商
        /// </summary> 
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("导出供应商", Models.Model.Enum.LogType.Export, _moduleName)]
        public async Task<string> ExportSuppliers( string searchKey, string orderField, string orderType, List<KeyValueModel> fields)
        {
            var fileUrl = await _supplierMgr.ExportSuppliers(searchKey, orderField, ConvertOrderType(orderType), fields); 
            return fileUrl;
        }

        /// <summary>
        /// 获取当前用户导出供应商列表被允许的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldPermission>> GetAllowField(string userId)
        {
            return await _supplierMgr.GetAllowField<BaseSuppliers>(userId); 
        }
    }
}
