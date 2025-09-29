using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using Logic.LogicBase; 
using Models.Model;
using Models.Model.AutomationDevice;
using Models.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.AutomationDevice
{
    public class AGVSettingMgr : DbOperationHandler
    {
        private readonly IMapper _mapper;

        public AGVSettingMgr(Repository repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }

        public async Task<TableModel<AutoProdDeviceAGVDto>> GetAGVInfo(int pgSize, int pgIndex, string orderFiled, string orderType, string agvType, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "AGVType" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<AutoProdDeviceAGV>()
                  .WhereIF(!string.IsNullOrEmpty(agvType), d => d.AGVType == agvType)
                  .Where(d => d.AGVNo.Contains(searchKey) || d.TaskType.Contains(searchKey) || d.UpdateUserName.Contains(searchKey))
                  .Select<AutoProdDeviceAGVDto>()
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            data.ForEach(f =>
            {
                f.AGVTypeDesc = EnumHelper.GetDescFromEnumVal<AGVType>(f.AGVType);
                f.StatusDesc=EnumHelper.GetDescFromEnumVal<AGVStatus>(f.Status);
            });
            var res = new TableModel<AutoProdDeviceAGVDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task<AutoProdDeviceAGVDto> GetAGVInfoDetail(int agvId)
        {
            var data = await Repository.ClientDb.Queryable<AutoProdDeviceAGV>()
                .Where(d => d.AGVId == agvId)
                .Select<AutoProdDeviceAGVDto>().SingleAsync();
            data.AGVTypeDesc = EnumHelper.GetDescFromEnumVal<AGVType>(data.AGVType);
            data.StatusDesc= EnumHelper.GetDescFromEnumVal<AGVStatus>(data.Status);
            return data;
        }
          
        public async Task AddAGVInfo(AutoProdDeviceAGVDto data)
        {
            if (string.IsNullOrEmpty(data.AGVNo))
            {
                throw new BusinessException("保存失败,AGV编号不能为空");
            }
            var exist = await Repository.Exist<AutoProdDeviceAGV>(x => x.AGVNo == data.AGVNo);
            if (exist)
            {
                throw new BusinessException("保存失败,当前存在相同的AGV编号");
            }
            if (data.IsDeft)
            {
                var otherTypeAgv = await Repository.ClientDb.Queryable<AutoProdDeviceAGV>().Where(w => w.AGVType == data.AGVType).ToListAsync();
                if(otherTypeAgv.Count > 0)
                {
                    otherTypeAgv.ForEach(f => f.IsDeft = false);
                    Repository.ClientDb.Updateable(otherTypeAgv).AddQueue();
                } 
            }
            var model = _mapper.Map<AutoProdDeviceAGV>(data);
            model.UpdateDate = DateTime.Now;
            Repository.ClientDb.Insertable(model).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task UpdateAGVInfo(AutoProdDeviceAGVDto data)
        {
            if (string.IsNullOrEmpty(data.AGVNo))
            {
                throw new BusinessException("保存失败,AGV编号不能为空");
            }
            var exist = await Repository.Exist<AutoProdDeviceAGV>(x => x.AGVId != data.AGVId && x.AGVNo == data.AGVNo);
            if (exist)
            {
                throw new BusinessException("保存失败,当前存在相同的AGV编号");
            }
            if (data.IsDeft)
            {
                var otherTypeAgv = await Repository.ClientDb.Queryable<AutoProdDeviceAGV>().Where(w =>w.AGVId!=data.AGVId && w.AGVType == data.AGVType).ToListAsync();
                if(otherTypeAgv.Count > 0)
                {
                    otherTypeAgv.ForEach(f => f.IsDeft = false);
                    Repository.ClientDb.Updateable(otherTypeAgv).AddQueue();
                } 
            }
            var model = _mapper.Map<AutoProdDeviceAGV>(data);
            model.UpdateDate = DateTime.Now;
            Repository.ClientDb.Updateable(model).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task DelAGVInfo(int agvId)
        {
            var old = await Repository.ClientDb.Queryable<AutoProdDeviceAGV>().Where(w =>w.AGVId== agvId).SingleAsync();
            if (old==null)
            {
                throw new BusinessException("删除失败,当前AGV信息不存在或已被删除");
            }
            if(old.Status!=AGVStatus.Free.ToString())
            {
                throw new BusinessException("删除失败,当前AGV正在使用中，不允许删除");
            }
            await Repository.ClientDb.Deleteable<AutoProdDeviceAGV>(d => d.AGVId== agvId).ExecuteCommandAsync();
        }
    }
}
