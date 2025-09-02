using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using External.Log;
using External.Socket.Socket;
using Logic.AutomationDevice;
using Logic.LogicBase;
using Logic.LogicCommon;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Sys;
using Models.Model.AutomationDevice;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Cmp;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using External.Common.Extension;

namespace Logic.Inventory
{
    public class RequisitionMgr : DbOperationHandler
    {
        private readonly IMapper _mapper;

        private readonly MessageService _messageService;

        private readonly SysArgsService _sysArgsService;

        private readonly LogHelper _logHelper;

        private readonly AutoTransportHandler _autoTransportHandler;

        public RequisitionMgr(Repository repository, IMapper mapper, MessageService messageService, SysArgsService sysArgsService, LogHelper logHelper, AutoTransportHandler autoTransportHandler ) : base(repository)
        {
            _mapper = mapper;
            _messageService = messageService;
            _sysArgsService = sysArgsService;
            _logHelper= logHelper;
            _autoTransportHandler= autoTransportHandler;
        }

        public async Task<UserAuthorizationDto> SwipingCardAuth(string cardNo)
        {
            cardNo=cardNo.Trim();
            var user = await Repository.ClientDb.Queryable<SysUser>().Select<UserAuthorizationDto>().SingleAsync(s => s.CardId == cardNo);
            if (user == null)
            {
                throw new BusinessException("未查询到用户卡信息");
            }
            if (!user.IsVaild)
            {
                throw new BusinessException("用户已被冻结，无法使用系统，请联系系统管理员");
            }
            var isAuth = await Repository.ClientDb.Queryable<SysUserPermissions>()
                .InnerJoin<SysUserRoles>((up, ur) => ur.RoleId == up.RoleId)
                .InnerJoin<SysUser>((up, ur, u) => u.UserId == ur.UserId)
                .InnerJoin<SysMenus>((up, ur, u, m) => m.MenuId == up.MenuId)
                .Where((up, ur, u, m) => u.UserId == user.UserId && m.CtrlName == "Requisition" && m.ActionName == "AddReqisitionOrder")
                .AnyAsync();
            if (!isAuth)
            {
                throw new BusinessException("您没有使用系统的物料领用权限！");
            }
            return user;
        }

        public async Task<List<RequisitionGoodsDetail>> GetOrderList(string userId,string goodsClassifyGroup,DateTime date)
        {
            var data= await Repository.ClientDb.Queryable<InvRequisitionOrder>()
                .InnerJoin<InvRequisitionOrderDetail>((o, d) => d.OrderNo == o.OrderNo)
                .LeftJoin<BaseGoods>((o, d, g) => g.GoodsId == d.GoodsId)
                .LeftJoin<BaseType>((o, d, g, t) => t.TypeId == g.GoodsClassifyId)
                .LeftJoin<BaseFiles>((o, d, g, t,f)=>f.PrimaryId==g.GoodsId && f.IsDeft && f.FileInfoType == FileInfoType.GoodsPhoto.ToString())
                .Where((o, d, g, t, f) => o.GoodsClassifyGroup == goodsClassifyGroup && o.CreateDate.Date == date && o.CreateUserId == userId)
                .OrderByDescending((o, d, g, t, f) =>o.CreateDate)
                .Select<RequisitionGoodsDetail>()
                .ToListAsync();
            data.ForEach(f =>
            {
                f.StatusDesc = EnumHelper.GetDescFromEnumVal<RequisitionStatus>(f.Status);
            });
            return data;
        }


        public async Task<TableModel<AutoProdTaskTrackingDto>> GetAGVList(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "TaskCreateTime" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<AutoProdTaskTracking>()
                  .Select((p) => new AutoProdTaskTrackingDto
                  {
                      TaskId = p.TaskId,
                      TaskCode = p.TaskCode,
                      AGVReqCode = p.AGVReqCode,
                      LineNo = p.LineNo,
                      OrderNo = p.OrderNo,
                      GoodsClassifyGroup=p.GoodsClassifyGroup,

                      BusinessType = p.BusinessType,
                      GoodsInfo = p.GoodsInfo,
                      TaskType = p.TaskType,
                      ActionType = p.ActionType,
                      IsExecuting = p.IsExecuting,
                      TaskModel = p.TaskModel,
                      TaskStatus = p.TaskStatus,
                      IsReturn = p.IsReturn,
                      ReturnTaskStatus = p.ReturnTaskStatus,
                      StartingDeviceNo = p.StartingDeviceNo,
                      StartingDeviceType=p.StartingDeviceType,
                      StartingAGVPositionNo=p.StartingAGVPositionNo,
                      StartingBinNo = p.StartingBinNo,
                      StartingBinRank = p.StartingBinRank,
                      DestinationDeviceNo = p.DestinationDeviceNo,
                      DestinationDeviceType=p.DestinationDeviceType,
                      DestinationAGVPositionNo = p.DestinationAGVPositionNo,
                      DestinationBinNo = p.DestinationBinNo,
                      DestinationBinRank = p.DestinationBinRank,
                      AGVNo = p.AGVNo,
                      TaskCreateTime = p.TaskCreateTime,
                      TaskStartTime = p.TaskStartTime,
                      TaskEndTime = p.TaskEndTime,
                      OperatorId = p.OperatorId,
                      OperatorName = p.OperatorName,

                  })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);

            var res = new TableModel<AutoProdTaskTrackingDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }


        public async Task UpdateAGVisExecuting(AutoProdTaskTrackingDto data)
        {
            var isReceiving = await Repository.ClientDb.Queryable<AutoProdTaskTracking>().Where(w => w.TaskId == data.TaskId && w.TaskCode ==data.TaskCode).AnyAsync();
            if (!isReceiving)
            {
                throw new BusinessException("该AGV任务在数据表中未找到，已无法修改！");
            }
            var model = new AutoProdTaskTracking
            {
                TaskId = data.TaskId,
                TaskCode = data.TaskCode,
                AGVReqCode = data.AGVReqCode,
                LineNo = data.LineNo,
                OrderNo = data.OrderNo,
                GoodsClassifyGroup = data.GoodsClassifyGroup,
                BusinessType = data.BusinessType,
                GoodsInfo = data.GoodsInfo,
                TaskType = data.TaskType,
                ActionType = data.ActionType,
                IsExecuting = false,//状态修改为不执行
                TaskModel = data.TaskModel,
                TaskStatus = data.TaskStatus,
                IsReturn = data.IsReturn,
                ReturnTaskStatus = data.ReturnTaskStatus,
                StartingDeviceNo = data.StartingDeviceNo,
                StartingDeviceType = data.StartingDeviceType,
                StartingAGVPositionNo = data.StartingAGVPositionNo,
                StartingBinNo = data.StartingBinNo,
                StartingBinRank = data.StartingBinRank,
                DestinationDeviceNo = data.DestinationDeviceNo,
                DestinationDeviceType = data.DestinationDeviceType,
                DestinationAGVPositionNo = data.DestinationAGVPositionNo,
                DestinationBinNo = data.DestinationBinNo,
                DestinationBinRank = data.DestinationBinRank,
                AGVNo = data.AGVNo,
                TaskCreateTime = data.TaskCreateTime,
                TaskStartTime = data.TaskStartTime,
                TaskEndTime = data.TaskEndTime,
                OperatorId = data.OperatorId,
                OperatorName = data.OperatorName,
            };
            await Repository.UpdateAsync(model);

        }


        public async Task<string> AddReqisitionOrder(RequisitionOrderDto data)
        { 
            var curUser= await SwipingCardAuth(data.CreateUserCard);
            if (data.Details?.Count == 0)
            {
                throw new BusinessException("请添加领用明细");
            } 
            var lastData = await Repository.ClientDb.Queryable<InvRequisitionOrder>().MaxAsync(x => x.OrderNo);
            var order = _mapper.Map<InvRequisitionOrder>(data);
            order.OrderNo = GetPrimaryId("R", lastData);
            order.CreateDate = DateTime.Now;
            order.CreateUserId = curUser.UserId;
            order.CreateUserName= curUser.UserName; 
            order.Status= RequisitionStatus.Receiving.ToString();
            Repository.ClientDb.Insertable(order).AddQueue(); 
            var details = _mapper.Map<List<InvRequisitionOrderDetail>>(data.Details);
            details.ForEach(f => f.OrderNo = order.OrderNo);
            Repository.ClientDb.Insertable(details).AddQueue();
            await _setOutStorage(order, details);
            await Repository.ClientDb.SaveQueuesAsync();  
            //创建AGV运输任务
            if (data.UseAgv)
            {
                _=Task.Run(() =>
                {
                    var outInvOrderNo = Repository.ClientDb.Queryable<InvOutStorage>().Where(w => w.SourceOrderNo == order.OrderNo).Select(w => w.OrderNo).ToList();
                    outInvOrderNo.ForEach(f =>
                    {
                        _autoTransportHandler.CreateTransTask(new Models.Model.AutomationDevice.AutoTransTaskInput
                        {
                            OrderNo = f,
                            TaskType = AutoTransTaskType.OutStorage.ToString(),
                            BusinessType = OutStorageType.ReceiveOut.ToString(),
                            TaskModel = AGVTaskModel.TransportSingle.ToString(),
                            IsRequirementForRemaining = true,
                            GoodsClassifyGroup = data.GoodsClassifyGroup,
                            OperatorId = order.CreateUserId,
                            OperatorName = order.CreateUserName
                        });
                    });
                   
                });
            } 
            return order.OrderNo;
        }

        public async Task AddReqisitionOrderByExternal(ProductReqisitionInputExternalDto input)
        {
            _logHelper.LogInfo("AddReqisitionOrderByExternal", "收到IMS自动呼叫包接口信号，开始查询BOM信息", input.ProductNo);
            var mtList = await Repository.ClientDb.Queryable<BaseBOM>()
                .InnerJoin<BaseGoods>((b, g1) => g1.GoodsId == b.ParentId)
                .InnerJoin<BaseGoods>((b, g1, g2) => g2.GoodsId == b.MaterialId)
                .Where((b, g1, g2) => g1.GoodsNo == input.ProductNo && b.MaterialClassifyGroup==BaseTypeGroup.PackingMaterial.ToString())
                .Select((b, g1, g2) => new
                {
                    g2.GoodsId,
                    g2.GoodsName,
                    b.Quantity, 
                    b.Unit,
                    g2.PackageUnitId,
                    g2.PackageUnitName,
                    g2.MinPackageUnitId,
                    g2.MinPackageUnitName,
                    g2.MaxPackageUnitId,
                    g2.MaxPackageUnitName,
                    ParentPackageCount= g1.PackageCount,
                    ParentMaxPackageCount= g1.MaxPackageCount,
                })
                .ToListAsync();
            if(mtList?.Count == 0)
            {
                _logHelper.LogInfo("AddReqisitionOrderByExternal", "IMS自动呼叫包材失败,未查询到BOM信息", input.ProductNo);
                throw new BusinessException("未查询到产品BOM信息");
            }
            var lastData = await Repository.ClientDb.Queryable<InvRequisitionOrder>().MaxAsync(x => x.OrderNo);
            var reqOrder = new InvRequisitionOrder
            {
                OrderNo = GetPrimaryId("R", lastData),
                OrderType = "ProductionRequisition",
                GoodsClassifyGroup = BaseTypeGroup.PackingMaterial.ToString(),
                Purpose = "成品下线自动叫料",
                Status= RequisitionStatus.Receiving.ToString(),
                CreateUserId= "Sys",
                CreateUserName="Sys",
                CreateDate = DateTime.Now
            };
            Repository.ClientDb.Insertable(reqOrder).AddQueue();
            var reqOrderDetails = new List<InvRequisitionOrderDetail>();
            foreach (var item in mtList)
            {
                int finishedProdQuantity = (int)Math.Ceiling(input.Quantity / item.ParentPackageCount);
                int unitId = item.PackageUnitId;
                string unitName = item.PackageUnitName;
                if (item.Unit == item.MinPackageUnitName)
                {
                    unitId = item.MinPackageUnitId;
                    unitName = item.MinPackageUnitName;
                }
                else if (item.Unit == item.MaxPackageUnitName)
                {
                    unitId = item.MaxPackageUnitId;
                    unitName = item.MaxPackageUnitName;
                }
                var reqDetail = new InvRequisitionOrderDetail
                {
                    OrderNo=reqOrder.OrderNo,
                    GoodsId=item.GoodsId,
                    GoodsName=item.GoodsName,
                    Quantity = item.Quantity* finishedProdQuantity,
                    UnitId=unitId,
                    UnitName= unitName 
                };
                reqOrderDetails.Add(reqDetail);
            }
            Repository.ClientDb.Insertable(reqOrderDetails).AddQueue();
            await _setOutStorage2(reqOrder, reqOrderDetails);
            await Repository.ClientDb.SaveQueuesAsync();
            //创建AGV运输任务（暂不启用AGV）
            //_ = Task.Run(() =>
            //{
            //    var outInvOrderNo = Repository.ClientDb.Queryable<InvOutStorage>().Where(w => w.SourceOrderNo == reqOrder.OrderNo).Select(w => w.OrderNo).ToList();
            //    outInvOrderNo.ForEach(f =>
            //    {
            //        _autoTransportHandler.CreateTransTask(new Models.Model.AutomationDevice.AutoTransTaskInput
            //        {
            //            OrderNo = f,
            //            TaskType = AutoTransTaskType.OutStorage.ToString(),
            //            BusinessType = OutStorageType.ReceiveOut.ToString(),
            //            TaskModel = AGVTaskModel.TransportSingle.ToString(),
            //            IsRequirementForRemaining = true,
            //            GoodsClassifyGroup = reqOrder.GoodsClassifyGroup,
            //            OperatorId = reqOrder.CreateUserId,
            //            OperatorName = reqOrder.CreateUserName
            //        });
            //    }); 
            //});
            var classifyGroup = EnumHelper.GetDescFromEnum(BaseTypeGroup.PackingMaterial);
            var msgRemark = "";
            foreach (var d in reqOrderDetails)
            {
                msgRemark += $"{d.GoodsName}  数量:{d.Quantity}{d.UnitName}，";
            }
            msgRemark = msgRemark.TrimEnd('，');
            var msgContent = $"IMS提交了一份自动叫料{classifyGroup}领用单，需求：{msgRemark}"; 
            _logHelper.LogInfo("AddReqisitionOrderByExternal", msgContent, input.ProductNo);
        }

        public async Task UpdateReqisitionOrder(RequisitionOrderDto data)
        {
            var curUser = await SwipingCardAuth(data.CreateUserCard);
            var isReceiving = await Repository.ClientDb.Queryable<InvRequisitionOrder>().Where(w => w.OrderNo == data.OrderNo && w.Status == RequisitionStatus.Receiving.ToString()).AnyAsync();
            if (!isReceiving)
            {
                throw new BusinessException("该领用单不在待领用状态，已无法修改");
            }
            var order = _mapper.Map<InvRequisitionOrder>(data);
            order.UpdateDate = DateTime.Now;
            order.UpdateUserId = curUser.UserId;
            order.UpdateUserName = curUser.UserName;
            Repository.ClientDb.Updateable(order).AddQueue();
            var details = _mapper.Map<List<InvRequisitionOrderDetail>>(data.Details); 
            Repository.ClientDb.Updateable(details).AddQueue();
            if(data.Status == RequisitionStatus.Receiving.ToString())
            {
                await _setOutStorage(order, details);
            }
            await Repository.ClientDb.SaveQueuesAsync();
            //创建AGV运输任务
            _ = Task.Run(() =>
            {
                if (data.UseAgv)
                {
                    var outInvOrderNo = Repository.ClientDb.Queryable<InvOutStorage>().Where(w => w.SourceOrderNo == order.OrderNo).Select(w => w.OrderNo).ToList();
                    outInvOrderNo.ForEach(f =>
                    {
                        _autoTransportHandler.CreateTransTask(new Models.Model.AutomationDevice.AutoTransTaskInput
                        {
                            OrderNo = f,
                            TaskType = AutoTransTaskType.OutStorage.ToString(),
                            BusinessType = OutStorageType.ReceiveOut.ToString(),
                            TaskModel = AGVTaskModel.TransportSingle.ToString(),
                            IsRequirementForRemaining = true,
                            GoodsClassifyGroup = order.GoodsClassifyGroup,
                            OperatorId = order.CreateUserId,
                            OperatorName = order.CreateUserName
                        });
                    }); 
                }
            }); 
        }

        public async Task DelReqisitionOrder(string orderNo,int detailId, string userCardNo)
        {
            await SwipingCardAuth(userCardNo);
            var isReceiving = await Repository.ClientDb.Queryable<InvRequisitionOrder>().Where(w => w.OrderNo == orderNo && w.Status == RequisitionStatus.Receiving.ToString()).AnyAsync();
            if (!isReceiving)
            {
                throw new BusinessException("该领用单不在待领用状态，已无法删除");
            } 
            var details = await Repository.ClientDb.Queryable<InvRequisitionOrderDetail>().Where(w => w.OrderNo == orderNo).ToListAsync();
            var delDetail= details.Single(s=>s.DetailId== detailId);
            Repository.ClientDb.Deleteable(delDetail).AddQueue();
            if (details.Count==1)
            {
                Repository.ClientDb.Deleteable<InvRequisitionOrder>(d => d.OrderNo == orderNo).AddQueue();
            }
            //删除出库单  
            var outStorageList = await Repository.ClientDb.Queryable<InvOutStorage>().Where(x =>x.SourceOrderNo==orderNo&&x.OutStorageType== OutStorageType.ReceiveOut.ToString()).ToListAsync();
            string delOrderNo = "";
            string delBusinessType = "";
            string delGoodsClassifyGroup = "";
            string delOperatorId = "";
            string delOperatorName = "";
            if (outStorageList?.Count>0 )
            {
                var outStorageOrders = outStorageList.Select(s => s.OrderNo).ToList();
                var outStorageDetails = await Repository.ClientDb.Queryable<InvOutStorageDetail>().Where(w => outStorageOrders.Contains(w.OrderNo)).ToListAsync();
                var delOutStorageDetail = outStorageDetails.Where(s => s.GoodsId == delDetail.GoodsId).ToList(); 
                delOrderNo = delOutStorageDetail.Select(s => s.OrderNo).Single();
                var delOrder = outStorageList.Single(s => s.OrderNo == delOrderNo);
                Repository.ClientDb.Deleteable(delOutStorageDetail).AddQueue();
                if (outStorageDetails.Count(w=>w.OrderNo== delOrderNo)==1)
                {  
                    Repository.ClientDb.Deleteable<InvOutStorage>(d => d.OrderNo == delOrderNo).AddQueue();
                }
                delBusinessType = delOrder.OutStorageType;
                delGoodsClassifyGroup = delOrder.GoodsClassify;
                delOperatorId = delOrder.CreateUserId;
                delOperatorName = delOrder.CreateUserName;
            } 
            await Repository.ClientDb.SaveQueuesAsync();
            //更新AGV任务 
            if (!string.IsNullOrEmpty(delOrderNo))
            {
                _ = Task.Run(() =>
                {
                    _autoTransportHandler.CreateTransTask(new Models.Model.AutomationDevice.AutoTransTaskInput
                    {
                        OrderNo = delOrderNo,
                        TaskType = AutoTransTaskType.OutStorage.ToString(),
                        BusinessType = delBusinessType,
                        TaskModel = AGVTaskModel.TransportSingle.ToString(),
                        IsRequirementForRemaining = true,
                        GoodsClassifyGroup = delGoodsClassifyGroup,
                        OperatorId = delOperatorId,
                        OperatorName = delOperatorName
                    });
                });
            } 
        }

        public async Task ConfirmReceived(RequisitionOrderDto data)
        {
            var curUser = await SwipingCardAuth(data.CreateUserCard);
            var order = _mapper.Map<InvRequisitionOrder>(data); 
            order.Status = RequisitionStatus.Received.ToString(); 
            order.UpdateDate = DateTime.Now;
            order.UpdateUserId = curUser.UserId;
            order.UpdateUserName = curUser.UserName;
            await Repository.ClientDb.Updateable(order).ExecuteCommandAsync();
        }

        private async Task _setOutStorage(InvRequisitionOrder data, List<InvRequisitionOrderDetail> details)
        {
            var goodsIdArr = details.Select(s => s.GoodsId).Distinct().ToList();
            var goodsPriceInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsIdArr.Contains(w.GoodsId)).Select(w => new { w.GoodsId, w.CostPrice, w.PriceUnitName }).ToListAsync();
            var curDate = DateTime.Now;
            var lastData = await Repository.ClientDb.Queryable<InvOutStorage>().MaxAsync(x => x.OrderNo);
            var outStorageList=new List<InvOutStorage>();
            var outStorageDetailList=new List<InvOutStorageDetail>();
            foreach (var item in details)
            {
                var outStorageModel = new InvOutStorage
                {
                    OrderNo = GetPrimaryId("O", lastData),
                    SourceOrderNo = data.OrderNo,
                    OutStorageType = OutStorageType.ReceiveOut.ToString(),
                    LineNo = data.Line,
                    GoodsClassify = data.GoodsClassifyGroup,
                    CreateDate = curDate,
                    CreateUserId = data.CreateUserId,
                    CreateUserName = data.CreateUserName,
                    Remark = data.Purpose,
                    ApprovalStatus = ApprovalStatus.NoApproval.ToString(),
                    Status = OutStorageStatus.WaitOutStorage.ToString()
                };
                outStorageList.Add(outStorageModel);
                var curGoodsInfo = goodsPriceInfo.Single(s => s.GoodsId == item.GoodsId);
                var outStorageDetail = new InvOutStorageDetail
                {
                    OrderNo = outStorageModel.OrderNo,
                    GoodsId = item.GoodsId,
                    GoodsName = item.GoodsName,
                    Quantity = item.Quantity,
                    UnitId = item.UnitId,
                    UnitPrice = curGoodsInfo.CostPrice,
                    TotalPrice = Math.Round(curGoodsInfo.CostPrice * item.Quantity, 2),
                    PriceUnit = curGoodsInfo.PriceUnitName
                };
                outStorageDetailList.Add(outStorageDetail);
                lastData = outStorageModel.OrderNo;
            }
           
            ////先注释 20250312
            ////删除上一次该领用单产生的出库单
            //var oldInstorage = Repository.ClientDb.Queryable<InvOutStorage>().Where(x => x.OutStorageType == OutStorageType.ReceiveOut.ToString() && x.SourceOrderNo == data.OrderNo).Single();
            //if (oldInstorage != null && !string.IsNullOrEmpty(oldInstorage.OrderNo))
            //{
            //    Repository.ClientDb.Deleteable(oldInstorage).AddQueue();
            //    Repository.ClientDb.Deleteable<InvOutStorageDetail>(x => x.OrderNo == oldInstorage.OrderNo).AddQueue();
            //}
            Repository.ClientDb.Insertable(outStorageList).AddQueue(); 
            Repository.ClientDb.Insertable(outStorageDetailList).AddQueue();
        }

        private async Task _setOutStorage2(InvRequisitionOrder data, List<InvRequisitionOrderDetail> details)
        {
            var goodsIdArr = details.Select(s => s.GoodsId).Distinct().ToList();
            var goodsPriceInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsIdArr.Contains(w.GoodsId)).Select(w => new { w.GoodsId, w.CostPrice, w.PriceUnitName }).ToListAsync();
            var curDate = DateTime.Now;
            var lastData = await Repository.ClientDb.Queryable<InvOutStorage>().MaxAsync(x => x.OrderNo);
            var outStorageModel = new InvOutStorage
            {
                OrderNo = GetPrimaryId("O", lastData),
                SourceOrderNo = data.OrderNo,
                OutStorageType = OutStorageType.ReceiveOut.ToString(),
                LineNo=data.Line,
                GoodsClassify = data.GoodsClassifyGroup,
                CreateDate = curDate,
                CreateUserId = data.CreateUserId,
                CreateUserName = data.CreateUserName,
                Remark = data.Purpose,
                ApprovalStatus = ApprovalStatus.NoApproval.ToString(),
                Status=OutStorageStatus.WaitOutStorage.ToString()
            }; 
            //删除上一次该领用单产生的出库单
            var oldInstorage = Repository.ClientDb.Queryable<InvOutStorage>().Where(x => x.OutStorageType == OutStorageType.ReceiveOut.ToString() && x.SourceOrderNo == data.OrderNo).Single();
            if (oldInstorage != null && !string.IsNullOrEmpty(oldInstorage.OrderNo))
            {
                Repository.ClientDb.Deleteable(oldInstorage).AddQueue();
                Repository.ClientDb.Deleteable<InvOutStorageDetail>(x => x.OrderNo == oldInstorage.OrderNo).AddQueue();
            }
            Repository.ClientDb.Insertable(outStorageModel).AddQueue();
            //写入新的出库单明细 
            var outStorageDetail = details.Select(b => new InvOutStorageDetail
            {
                OrderNo = outStorageModel.OrderNo,
                GoodsId = b.GoodsId,
                GoodsName = b.GoodsName,
                Quantity = b.Quantity, 
                UnitId = b.UnitId,
                UnitPrice= goodsPriceInfo.Single(s=>s.GoodsId== b.GoodsId).CostPrice,
                TotalPrice=Math.Round(goodsPriceInfo.Single(s => s.GoodsId == b.GoodsId).CostPrice*b.Quantity,2),
                PriceUnit= goodsPriceInfo.Single(s => s.GoodsId == b.GoodsId).PriceUnitName
            }).ToList();
            Repository.ClientDb.Insertable(outStorageDetail).AddQueue(); 
        }
    }
}
