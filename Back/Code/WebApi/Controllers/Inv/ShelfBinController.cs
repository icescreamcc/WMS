using DbRepository.Repository.DbModels;
using DbRepository.Repository;
using Logic.Inventory;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Inv;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter;
using External.Common;
using Models.Model.Enum;
using Logic.LogicCommon;
using Logic.LogicBase;

namespace WebApi.Controllers.Inv
{
    public class ShelfBinController : AuthTokenController
    {
        private readonly ShelfBinMgr  _shelfBinMgr;

        private readonly SysArgsService _sysArgsService;

        private const string _moduleName = "仓库货位管理";

        public ShelfBinController(ShelfBinMgr  shelfBinMgr, SysArgsService  sysArgsService)
        {
            _shelfBinMgr = shelfBinMgr;
            _sysArgsService = sysArgsService;
        }

        /// <summary>
        /// 仓库、货架、库位树形结构数据查看
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看仓库货位信息", LogType.Read, _moduleName)]
        public async Task<List<TreeModel>> GetWarehouseTree()
        {
            return await _shelfBinMgr.GetWarehouseTree();
        }

        /// <summary>
        /// 仓库明细查询
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<WarehouseDetail> GetWarehouseDetail(string warehouseId)
        {
           return await _shelfBinMgr.GetWarehouseDetail(warehouseId);
        }

        /// <summary>
        /// 货架明细查询
        /// </summary>
        /// <param name="shelfId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<Shelf> GetShelfDetail(string shelfId)
        {
            return await _shelfBinMgr.GetShelfDetail(shelfId);
        }

        /// <summary>
        /// 货位明细查询
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<Bin> GetBinDetail(int binId)
        {
            return await _shelfBinMgr.GetBinDetail(binId);
        } 

        /// <summary>
        /// 添加货架
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加货架", LogType.Add, _moduleName)]
        public async Task<string> AddShelf(Shelf data)
        {
           return await _shelfBinMgr.AddShelf(data);
        }

        /// <summary>
        /// 修改货架
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改货架", LogType.Update, _moduleName)]
        public async Task UpdateShelf(Shelf data)
        {
            await _shelfBinMgr.UpdateShelf(data);
        }

        /// <summary>
        /// 删除货架
        /// </summary>
        /// <param name="shelfId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除货架", LogType.Del, _moduleName)]
        public async Task DelShelf(string shelfId)
        {
            await _shelfBinMgr.DelShelf(shelfId);
        }

        /// <summary>
        /// 添加货位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加货位", LogType.Add, _moduleName)]
        public async Task<int> AddBin(Bin data)
        {
           return await _shelfBinMgr.AddBin(data);
        }

        /// <summary>
        /// 修改货位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改货位", LogType.Update, _moduleName)]
        public async Task UpdateBin(Bin data)
        {
            await _shelfBinMgr.UpdateBin(data);
        }

        /// <summary>
        /// 删除货位
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除货位", LogType.Del, _moduleName)]
        public async Task DelBin(int binId)
        {
            await _shelfBinMgr.DelBin(binId);
        }
    }
}
