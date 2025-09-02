using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Models.Model.Label;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Label
{
    public class LabelPrintMgr : DbOperationHandler
    {
        private readonly IMapper _mapper;

        public LabelPrintMgr(Repository repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }

        public async Task<List<LabelDesignDto>> GetLabels(string goodsClassifyGroup)
        {
            return await Repository.ClientDb.Queryable<LabelDesign>()
                .Where(w => w.GoodsClassifyGroup == goodsClassifyGroup)
                .Select<LabelDesignDto>()
                .ToListAsync();
        }

        public async Task<List<LabelDesignDetailsDto>> GetLableDesignDetails(string labelId)
        {
            return await Repository.ClientDb.Queryable<LabelDesignDetails>()
                .Where(w => w.LabelId == labelId)
                .Select<LabelDesignDetailsDto>()
                .ToListAsync();
        }

        public async Task AddPrintRecord(LabelPrintRecordDto data)
        {
            data.PackageId = Guid.NewGuid().ToString();
            var entitys=_mapper.Map<LabelPrintRecord>(data);
            await Repository.AddAsync(entitys);
        }

        public List<LabelPrintRecordDto> GetLabelRecord(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsClassifyGroup, string searchKey, string dateStart, string dateEnd)
        {
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "CreateDate" : orderFiled; 
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            int total=0;
            return Repository.ClientDb.Queryable<LabelPrintRecord>()
                .Where(w => w.GoodsClassifyGroup == goodsClassifyGroup)
                .Where(w => w.GoodsId.Contains(searchKey) || w.PackageId.Contains(searchKey) || w.LabelId.Contains(searchKey) || w.GoodsNo.Contains(searchKey) || w.GoodsName.Contains(searchKey) || w.GoodsModel.Contains(searchKey))
                .Where(w => SqlFunc.ToDateShort(w.CreateDate) >= GetDateStart(dateStart) && SqlFunc.ToDateShort(w.CreateDate) <= GetDateEnd(dateEnd))
                .Select<LabelPrintRecordDto>()
                .OrderBy($"{orderFiled} {orderType}")
                .ToPageList(pgIndex, pgSize, ref total);
        }
    } 
}
