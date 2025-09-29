using DbRepository.Repository;
using DbRepository.Repository.DbModels; 
using External.Common;
using Logic.LogicBase;
using Models.Model;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using System; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.BaseInfo
{
    /// <summary>
    /// 单位管理
    /// </summary>
   public class UnitMgr: DbOperationHandler
    {
        public UnitMgr(Repository repository) : base(repository) { }

        /// <summary>
        /// 单位分页查询
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task<TableModel<UnitDetail>> GetUnits(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "UnitType" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<BaseUnits>()
                  .Where(m=>m.IsValid)
                  .Where(m => m.UnitNo.Contains(searchKey) || m.UnitName.Contains(searchKey) || m.Remark.Contains(searchKey)) 
                  .Select(m => new UnitDetail { UnitId = m.UnitId, UnitNo = m.UnitNo,UnitName=m.UnitName, UnitType=m.UnitType, Remark = m.Remark })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            data.ForEach(x => x.UnitTypeDesc = EnumHelper.GetDescFromEnumVal<UnitType>(x.UnitType));
            var res = new TableModel<UnitDetail>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 添加单位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddUnit(UnitDetail data)
        {
            var exist = await Repository.Exist<BaseUnits>(u => u.UnitNo == data.UnitNo || u.UnitName == data.UnitName);
            if (exist)
            {
                throw new BusinessException("保存失败,当前单位编码或名称已存在");
            }
            var model = new BaseUnits
            { 
                UnitNo=data.UnitNo,
                UnitName=data.UnitName,
                UnitType=data.UnitType,
                Remark=data.Remark,
                IsValid=true
            };
            await Repository.AddAsync(model); 
        }

        /// <summary>
        /// 修改单位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateUnit(UnitDetail data)
        {
            var exist = await Repository.Exist<BaseUnits>(u => u.UnitId == data.UnitId);
            if (!exist)
            {
                throw new BusinessException("保存失败,当前单位ID不存在或已删除"); 
            }
            var model = new BaseUnits
            {
                UnitId = data.UnitId,
                UnitNo = data.UnitNo,
                UnitName = data.UnitName,
                UnitType = data.UnitType,
                Remark = data.Remark,
                IsValid = true
            };
            await Repository.UpdateAsync(model);
        }

        /// <summary>
        /// 删除单位
        /// </summary>
        /// <param name="unitsId"></param>
        /// <returns></returns>
        public async Task DelUnit(int [] unitsId)
        {
            var models = await Repository.ClientDb.Queryable<BaseUnits>().Where(x => unitsId.Contains(x.UnitId)).ToListAsync();
            models.ForEach(x => x.IsValid = false);
            await Repository.UpdateAsync(models);
        }
    }
}
