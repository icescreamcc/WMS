using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Models.Model;
using Models.Model.Label; 

namespace Logic.Label
{
    public class MaterialLabelDesignMgr : DbOperationHandler
    {
        private readonly IMapper _mapper;

        public MaterialLabelDesignMgr(Repository repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }

        public async Task<List<LabelDesignDto>?> GetLabelDesign(string orderFiled, string orderType,string goodsClassifyGroup, string searchKey)
        {
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "GoodsClassifyGroup" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var tempData= await Repository.ClientDb.Queryable<LabelDesign>()
                .Where(w => w.LabelName.Contains(searchKey) || w.Remark.Contains(searchKey) || w.CreateUserName.Contains(searchKey))
                .WhereIF(!string.IsNullOrEmpty(goodsClassifyGroup),w=>w.GoodsClassifyGroup== goodsClassifyGroup)
                .OrderBy($"{orderFiled} {orderType},CreateDate desc") 
                .Select<LabelDesignDto>()
                .ToListAsync();
            if(tempData?.Count > 0)
            {
                var labelIdArr = tempData.Select(s => s.LabelId).ToArray();
                var details = await Repository.ClientDb.Queryable<LabelDesignDetails>()
                    .Where(w => labelIdArr.Contains(w.LabelId))
                    .Select<LabelDesignDetailsDto>()
                    .ToListAsync();
                tempData.ForEach(s =>
                {
                    s.Details = details.Where(w => w.LabelId == s.LabelId).ToList();
                });
            }
            return tempData;
        }

        public async Task<List<LabelDesignDetailsDto>> GetLableDesignDetails(string labelId)
        {
            return await Repository.ClientDb.Queryable<LabelDesignDetails>()
                .Where(w => w.LabelId == labelId)
                .Select<LabelDesignDetailsDto>()
                .ToListAsync();
        }

        public List<KeyValueModel> GetItemValieFields()
        {
            return new List<KeyValueModel>
            {
                new KeyValueModel{Key="GoodsNo",Value="SAP编码"},
                new KeyValueModel{Key="GoodsName",Value="物料名称"},
                new KeyValueModel{Key="GoodsModel",Value="物料型号"},
                new KeyValueModel{Key="ProductionDate",Value="生产日期"},
                new KeyValueModel{Key="ExpiresDate",Value="过期日期"},
                new KeyValueModel{Key="Batch1",Value="批次号1"},
                new KeyValueModel{Key="Batch2",Value="批次号2"},
                new KeyValueModel{Key="Remark",Value="物料备注"},
                new KeyValueModel{Key="SupplierNo",Value="供应商编码"},
                new KeyValueModel{Key="SupplierName",Value="供应商名称"},
                new KeyValueModel{Key="CurDate",Value="当前日期"},
                new KeyValueModel{Key="PackageId",Value="PackageID"}
            };
        }

        public async Task AddLabelDesign(LabelDesignDto data)
        {
            if (data.Details?.Count == 0)
            {
                throw new BusinessException("保存失败,请添加标签内容");
            }
            var itemNameDitinctArr= data.Details.Select(s=>s.ItemName).Distinct().ToList();
            if (itemNameDitinctArr.Count != data.Details.Count)
            {
                throw new BusinessException("保存失败,模板明细项的名称不允许有重复");
            }
            var exist = await Repository.Exist<LabelDesign>(u => u.LabelName==data.LabelName);
            if (exist)
            {
                throw new BusinessException("保存失败,当前标签名称存在");
            }
           
            var designEntity=_mapper.Map<LabelDesign>(data);
            designEntity.CreateDate = DateTime.Now;
            var lastData = await Repository.ClientDb.Queryable<LabelDesign>().MaxAsync(x => x.LabelId);
            designEntity.LabelId = GetPrimaryId("D", lastData);
            Repository.ClientDb.Insertable(designEntity).AddQueue();
            var designDetailEntitys = _mapper.Map<List<LabelDesignDetails>>(data.Details);
            designDetailEntitys.ForEach(d => d.LabelId = designEntity.LabelId);
            Repository.ClientDb.Insertable(designDetailEntitys).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task UpdateLabelDesign(LabelDesignDto data)
        {
            if (data.Details?.Count == 0)
            {
                throw new BusinessException("保存失败,请添加标签内容");
            }
            var itemNameDitinctArr = data.Details.Select(s => s.ItemName).Distinct().ToList();
            if (itemNameDitinctArr.Count != data.Details.Count)
            {
                throw new BusinessException("保存失败,模板明细项的名称不允许有重复");
            }
            var exist = await Repository.Exist<LabelDesign>(u => u.LabelName == data.LabelName&& u.LabelId!=data.LabelId);
            if (exist)
            {
                throw new BusinessException("保存失败,当前标签名称存在");
            } 
            var designEntity = _mapper.Map<LabelDesign>(data);
            designEntity.UpdateDate = DateTime.Now; 
            Repository.ClientDb.Updateable(designEntity).AddQueue();
            var designDetailEntitys = _mapper.Map<List<LabelDesignDetails>>(data.Details);
            Repository.ClientDb.Deleteable<LabelDesignDetails>(d => d.LabelId == designEntity.LabelId).AddQueue(); 
            Repository.ClientDb.Insertable(designDetailEntitys).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task UpdateLabelDeft(string labelId,string goodsClassifyGroup)
        {
            var classifyArr=await Repository.ClientDb.Queryable<LabelDesign>().Where(w=>w.GoodsClassifyGroup==goodsClassifyGroup).ToListAsync();
            if(classifyArr?.Count>0)
            {
                classifyArr.ForEach(f =>
                {
                    f.IsDeft = false;
                    if(f.LabelId==labelId)
                    {
                        f.IsDeft = true;
                    }
                });
            }
            await Repository.ClientDb.Updateable(classifyArr).ExecuteCommandAsync();
        }

        public async Task DelLabelDesign(string labelId)
        {
            Repository.ClientDb.Deleteable<LabelDesign>(d=>d.LabelId == labelId).AddQueue();
            Repository.ClientDb.Deleteable<LabelDesignDetails>(d => d.LabelId == labelId).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }
    }
}
