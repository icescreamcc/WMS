using External.Common;
using Logic.Label;
using Logic.LogicCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Label;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.Label
{
    public class MaterialLabelDesignController : AuthTokenController
    {
        private readonly MaterialLabelDesignMgr _materialLabelDesignMgr;

        private readonly SysArgsService _sysArgsService;

        private const string _moduleName = "标签设计";

        public MaterialLabelDesignController(MaterialLabelDesignMgr materialLabelDesignMgr, SysArgsService sysArgsService)
        {
            _materialLabelDesignMgr = materialLabelDesignMgr;
            _sysArgsService = sysArgsService;
        }

        [HttpGet]
        [BusinessLog("查看标签模板", LogType.Read, _moduleName)]
        public async Task<List<LabelDesignDto>> GetLabelDesign(string orderFiled, string orderType, string goodsClassifyGroup, string searchKey)
        {
            return await _materialLabelDesignMgr.GetLabelDesign(orderFiled, ConvertOrderType(orderType), goodsClassifyGroup, searchKey);
        }

        [HttpGet]
        [Skip]
        public async Task<List<LabelDesignDetailsDto>> GetLableDesignDetails(string labelId)
        {
            return await _materialLabelDesignMgr.GetLableDesignDetails(labelId);
        }

        [HttpGet]
        [Skip]
        public object GetOptions()
        {
            var itemTypes = EnumHelper.GetEnumValNames<LabelItemType>();
            var itemValueTypes = EnumHelper.GetEnumValNames<LabelItemValueType>();
            var itemValueFields = _materialLabelDesignMgr.GetItemValieFields();
            return new
            {
                ItemTypeOption = itemTypes,
                ItemValueTypeOption = itemValueTypes,
                ItemValueFieldOption = itemValueFields
            };
        }

        [HttpPost]
        [BusinessLog("添加标签模板", LogType.Add, _moduleName)]
        public async Task AddLabelDesign(LabelDesignDto data)
        {
            await _materialLabelDesignMgr.AddLabelDesign(data);
        }

        [HttpPost]
        [BusinessLog("修改标签模板", LogType.Update, _moduleName)]
        public async Task UpdateLabelDesign(LabelDesignDto data)
        {
            await _materialLabelDesignMgr.UpdateLabelDesign(data);
        }

        [HttpGet]
        [Skip]
        [BusinessLog("修改默认标签模板", LogType.Update, _moduleName)]
        public async Task UpdateLabelDeft(string labelId, string goodsClassifyGroup)
        {
            await _materialLabelDesignMgr.UpdateLabelDeft(labelId, goodsClassifyGroup);
        }

        [HttpGet]
        [BusinessLog("删除标签模板", LogType.Add, _moduleName)]
        public async Task DelLabelDesign(string labelId)
        {
            await _materialLabelDesignMgr.DelLabelDesign(labelId);
        }
    }
}
