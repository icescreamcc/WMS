using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Models.Model.Prod;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Model.Enum;
using Logic.LogicCommon;
using System.Reflection;

namespace Logic.ProductOffLine
{
    public class ProductPackageMgr : DbOperationHandler
    {
        private readonly MessageService _messageService;

        public ProductPackageMgr(Repository repository, MessageService messageService) : base(repository)
        {
            _messageService = messageService;
        }

        public async Task<TableModel<MatchingCarDto>> GetMatchingInfo(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey,bool isPackage)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "CarSoleCode" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            var data = Repository.ClientDb.Queryable<ProdMatchingCar>()
                 .InnerJoin<ProdOrders>((m, o) => m.DeliverNo == o.DeliverNo)
                 .LeftJoin<ProdOrderDetails>((m, o, d) => d.CarSoleCode == m.CarSoleCode)
                 .LeftJoin<ProdUnstackBinQueue>((m, o, d, q) => q.CarSoleCode == d.CarSoleCode)
                 .OrderBy((m, o, d, q) => new { m.ConsignNum, m.CarSoleCode }).OrderByDescending(m => m.MatchingCode)
                 .Where((m, o, d, q)=>m.IsPackage== isPackage&&m.IsMatch)
                 .WhereIF(!string.IsNullOrEmpty(searchKey), (m, o, d, q)=>m.DeliverNo.Contains(searchKey)||m.ConsignNum.Contains(searchKey)||m.CarSoleCode.Contains(searchKey)||m.BinNo.Contains(searchKey))
                 .Select((m, o, d, q) => new MatchingCarDto
                 {
                     MatchingId = m.MatchingId,
                     DeliverNo = m.DeliverNo,
                     ConsignNum = m.ConsignNum,
                     Prodct = m.Prodct,
                     CountByCar = m.CountByCar,
                     MatchingCount = m.MatchingCount,
                     Total = m.Total,
                     TotalPutout = o.TotalPutout,
                     UnitName = o.UnitName,
                     CarSoleCode = m.CarSoleCode,
                     MatchingNum = m.MatchingNum,
                     MatchingCode = m.MatchingCode,
                     PlanTotalByCar = m.PlanTotalByCar,
                     ActualTotalByCar = m.ActualTotalByCar,
                     BinId = m.BinId,
                     BinNo = m.BinNo,
                     UnstackBinId = q.UnstackBinId,
                     UnstackBinNo = q.UnstackBinNo,
                     IsPackage = m.IsPackage,
                     IsMatch=m.IsMatch,
                     MatchDate = m.MatchDate,
                     PackageDate = m.PackageDate,
                     Status = d.Status
                 })
                 .OrderBy($"{orderFiled} {orderType}")
                 .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<MatchingCarDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 包装完成提交
        /// 1.修改配对信息状态:IsPackage=true,添加包装完成时间
        /// 2.修改订单状态：Packed
        /// 3.修改订单明细状态：Packed
        /// 4.修改队列中对应小车信息状态：PutOut
        /// </summary>
        /// <param name="matchingCode"></param>
        /// <returns></returns>
        public async Task SubmitMatchPackage(string userName,string matchingCode)
        {
            var matchData=await Repository.ClientDb.Queryable<ProdMatchingCar>().Where(w=>w.MatchingCode== matchingCode&&!w.IsPackage).ToListAsync();
            if(matchData.Count==0)
            {
                throw new BusinessException($"未查询到该配对信息，或已完成包装");
            } 
            var matchFirst = matchData[0];
            var order = await Repository.ClientDb.Queryable<ProdOrders>().SingleAsync(s => s.DeliverNo == matchFirst.DeliverNo);
            var orderDetails = await Repository.ClientDb.Queryable<ProdOrderDetails>().Where(w => w.DeliverNo == matchFirst.DeliverNo).ToListAsync();
            var carCodeArr = matchData.Select(s => s.CarSoleCode).ToArray();
            var details = orderDetails.Where(w => carCodeArr.Contains(w.CarSoleCode)).ToList();
            if (!details.All(e => e.Status == ProdOrderDetailStatus.PutOut.ToString()))
            {
                throw new BusinessException($"当前配对小车存在未下架的车次");
            }
            if (matchData.Count != matchFirst.MatchingCount)
            {
                throw new BusinessException($"当前配对车次未满足订单要求的小车配对数，小车配对要求{matchFirst.MatchingCount}车");
            }
            matchData.ForEach(p =>
            {
                p.IsPackage = true;
                p.PackageDate = DateTime.Now;
            });
            Repository.ClientDb.Updateable(matchData).AddQueue();
             
           var packagedCount=orderDetails.Count(w=>w.Status==ProdOrderDetailStatus.Packed.ToString());
            if (packagedCount+ matchData.Count== orderDetails.Count)
            {
                order.Status = ProdOrderStatus.PackageFinished.ToString();
            }
            else
            {
                order.Status = ProdOrderStatus.Packed.ToString();
            } 
            order.ModifyDate = DateTime.Now;
            order.ModifyUser = userName;
            Repository.ClientDb.Updateable(order).AddQueue();
             
            details.ForEach(f=>f.Status=ProdOrderDetailStatus.Packed.ToString());
            Repository.ClientDb.Updateable(details).AddQueue();

            //var binIdArr=matchData.Select(p=>p.BinId).ToArray();
            //var bins=await Repository.ClientDb.Queryable<InvBin>().Where(w=> binIdArr.Contains(w.BinId)).ToListAsync();
            //bins.ForEach(f=>f.Status=BinStatus.Free.ToString());
            //Repository.ClientDb.Updateable(bins).AddQueue();
            Repository.ClientDb.Updateable<ProdUnstackBinQueue>().SetColumns(s => s.Status == ProdUnstackBinQueueStatus.Putout.ToString()).Where(s => s.MatchingCode== matchFirst.MatchingCode).AddQueue(); 
            await Repository.ClientDb.SaveQueuesAsync();
            await _messageService.CreateMessage(userName, $"发货单{matchFirst.ConsignNum}完成了一次配对包装", "", MessageType.Packed);
        }
    }
}
