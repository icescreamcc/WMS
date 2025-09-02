using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Models.Model.Enum;
using Models.Model.Inv;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Inventory
{
    public class WarehousePickDashboardMgr : DbOperationHandler
    {
        public WarehousePickDashboardMgr(Repository repository) : base(repository)
        {
        }

        public async Task<List<GoodsPickData>> GetData(string goodsClassifyGroup,int goodsClassifyId,string searchKey)
        {
            return await Repository.ClientDb.Queryable<InvRequisitionOrder>()
               .InnerJoin<InvRequisitionOrderDetail>((o, d) => o.OrderNo == d.OrderNo)
               .InnerJoin<BaseGoods>((o, d, g) => g.GoodsId == d.GoodsId)
               .InnerJoin<BaseType>((o,d,g,t)=>t.TypeId==g.GoodsClassifyId)
               .Where((o, d, g, t) => o.Status == RequisitionStatus.Receiving.ToString() && t.Group==goodsClassifyGroup)
               .WhereIF(!string.IsNullOrEmpty(searchKey),(o, d, g, t)=>g.GoodsNo.Contains(searchKey)||g.GoodsName.Contains(searchKey)||g.GoodsModel.Contains(searchKey))
               .WhereIF(goodsClassifyId>0,(o, d, g, t)=>g.GoodsClassifyId==goodsClassifyId)
               .Select<GoodsPickData>()
               .ToListAsync();
        }
    }
}
