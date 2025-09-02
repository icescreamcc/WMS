using Logic.Sys; 
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filter; 

namespace WebApi.Controllers.Sys
{ 
    public class FieldsManageController : AuthTokenController
    {
        private readonly FieldManageMgr _spareFieldMgr; 

        public FieldsManageController(FieldManageMgr spareFieldMgr)
        {
            _spareFieldMgr = spareFieldMgr; 
        }

        /// <summary>
        /// 获取所有表及备用字段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<TreeModel>> GetTableFieldList()
        {
            return await _spareFieldMgr.GetTableFieldList(); 
        }

        /// <summary>
        /// 获取指定表字段详细信息
        /// </summary>
        /// <param name="fieldsManageId"></param> 
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<FieldsManage> GetSpareField(string fieldsManageId)
        {
            return await _spareFieldMgr.GetSpareField(fieldsManageId); 
        }
         
        /// <summary>
        /// 修改表字段信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改字段信息", Models.Model.Enum.LogType.Update)]
        public async Task SetSpareField(FieldsManage data)
        {
            await _spareFieldMgr.SetSpareField(data); 
        }
    }
}
