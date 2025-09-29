using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using External.Common.Extension; 
using External.Log;
using Logic.LogicBase;
using Logic.LogicBase.CacheService;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Microsoft.Extensions.Configuration;
using Models.Model;
using Models.Model.Baseinfo; 
using Models.Model.Enum; 
using Models.Model.Inv;
using Models.Model.Purchase;
using Models.Model.Sys;
using SqlSugar;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Inventory
{
    public class SafetyWarningRecordMgr : ExternalApiHandler
    {
        private readonly IConfiguration _configuration;

        private readonly LogHelper _logHelper;

        private readonly IMapper _mapper;

        private readonly IFileStorage _fileStorage;

        private readonly EmailService _emailService;

        private readonly SysArgsService _sysArgsService;

        public SafetyWarningRecordMgr(Repository repository,
            BusinessCacheService businessCacheService,
            BusinessCacheItem businessCacheItem,
            HttpHelperAsync httpHelper,
            IConfiguration configuration,
            LogHelper logHelper,
            IMapper mapper,
            IFileStorage fileStorage,
            EmailService emailService,
            SysArgsService sysArgsService) : base(repository, businessCacheService, businessCacheItem, httpHelper)
        {
            _configuration = configuration;
            _logHelper = logHelper;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _emailService = emailService;
            _sysArgsService = sysArgsService;
        }

        public async Task<TableModel<InvSafetyWarningRecordDto>> GetWarningInfo(string userId, int pgSize, int pgIndex, string orderField, string orderType, string searchKey, string goodsGroup, int goodsClassifyType, string status)
        {
            int total = 0;
            if (string.IsNullOrEmpty(orderField))
            {
                orderField = "r.CreateDate";
            }
            else if (orderField.ToLower() == "createdate")
            {
                orderField = "r.CreateDate";
            }
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var curApprover = await GetApprover(ApprovalDataType.SafetyInventory.ToString(), userId);
            var approvalModel = curApprover.FirstOrDefault()?.ApprovalModel;
            var curApproverRank = curApprover.Select(x => x.Rank).Distinct().ToList();
            var isAnyApproval = approvalModel == ApprovalModel.Any.ToString() && curApprover?.Count > 0;
            var data = Repository.ClientDb.Queryable<InvSafetyWarningRecord>()
                .InnerJoin<BaseGoods>((r, g) => r.GoodsId == g.GoodsId)
                .InnerJoin<BaseType>((r, g, t) => t.TypeId == g.GoodsClassifyId)
                .Where((r, g, t) => r.GoodsId.Contains(searchKey) || g.GoodsNo.Contains(searchKey) || g.GoodsName.Contains(searchKey) || g.GoodsModel.Contains(searchKey) || g.Supplier.Contains(searchKey))
                .Where((r, g, t) => t.Group == goodsGroup)
                .WhereIF(goodsClassifyType > 0, (r, g, t) => t.TypeId == goodsClassifyType)
                .WhereIF(!string.IsNullOrEmpty(status), (r, g, t) => r.Status == status)
                .Select((r, g, t) => new InvSafetyWarningRecordDto
                {
                    DetailId = r.DetailId,
                    FlowId = r.FlowId,
                    Year = r.Year, Month = r.Month, Week = r.Week,
                    Supplier = g.Supplier,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsModel = g.GoodsModel,
                    GoodsClassifyGroup = t.Group,
                    GoodsClassifyTypeName = t.TypeName,
                    AreaName = g.ForArea,
                    CurInvenstory = SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(w => w.GoodsId == g.GoodsId && w.UnitId == g.SafetyInventoryUnitId).Sum(s => s.Stock),
                    SafetyInventory = g.SafetyInventory,
                    SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                    PurchaseMinimum = g.PurchaseMinimum,
                    PurchaseMinimumUnitName = g.PurchaseMinimumUnitName,
                    CostPrice = g.CostPrice,
                    CostPriceUnitName = g.CostPriceUnitName,
                    PriceUnitName = g.PriceUnitName,
                    Status = r.Status,
                    PurchaseOrderNo = r.PurchaseOrderNo,
                    RequirementQuantity = r.RequirementQuantity,
                    IsNeedPurchase = r.IsNeedPurchase,
                    NeedPurchase = r.NeedPurchase,
                    IsSendMail = r.IsSendMail,
                    Remark = r.Remark,
                    CreateDate = r.CreateDate,
                    ApprovalDate = r.ApprovalDate,
                    ApproverId = r.ApproverId,
                    ApproverName = r.ApproverName,
                    ApproverRole = r.ApproverRole,
                    ApprovalStatus = r.ApprovalStatus,
                    ApprovalLastRank = SqlFunc.Subqueryable<ApprovalHis>().Where(h => h.PrimaryId == r.FlowId && h.DataType == ApprovalDataType.SafetyInventory.ToString()).Max(h => h.ApprovalRank)
                })
                .OrderBy($"{orderField} {orderType}")
                .ToPageList(pgIndex, pgSize, ref total);
            data.ForEach(f =>
            {
                f.StatusDesc = EnumHelper.GetDescFromEnumVal<SafetyWarningRecordStatus>(f.Status);
                f.ApprovalStatusDesc = EnumHelper.GetDescFromEnumVal<ApprovalStatus>(f.ApprovalStatus);
                f.IsApproval = (isAnyApproval && f.ApprovalStatus == ApprovalStatus.Pending.ToString()) || (curApprover?.Count > 0 && curApproverRank.Contains(f.ApprovalLastRank + 1) && (f.ApprovalStatus == ApprovalStatus.Pending.ToString() || f.ApprovalStatus == ApprovalStatus.Approvaling.ToString()));
            });
            var res = new TableModel<InvSafetyWarningRecordDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task<InvSafetyWarningRecordDto> GetSafetyInfoDetail(int detailId)
        {
            var data = await Repository.ClientDb.Queryable<InvSafetyWarningRecord>()
            .InnerJoin<BaseGoods>((r, g) => r.GoodsId == g.GoodsId)
            .InnerJoin<BaseType>((r, g, t) => t.TypeId == g.GoodsClassifyId)
            .Where((r, g, t) => r.DetailId == detailId)
            .Select((r, g, t) => new InvSafetyWarningRecordDto
            {
                DetailId = r.DetailId,
                FlowId = r.FlowId,
                Year = r.Year,
                Month = r.Month,
                Week = r.Week,
                Supplier = g.Supplier,
                GoodsId = g.GoodsId,
                GoodsNo = g.GoodsNo,
                GoodsName = g.GoodsName,
                GoodsModel = g.GoodsModel,
                GoodsClassifyGroup = t.Group,
                GoodsClassifyTypeName = t.TypeName,
                AreaName = g.ForArea,
                CurInvenstory = SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(w => w.GoodsId == g.GoodsId && w.UnitId == g.SafetyInventoryUnitId).Sum(s => s.Stock),
                SafetyInventory = g.SafetyInventory,
                SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                PurchaseMinimum = g.PurchaseMinimum,
                PurchaseMinimumUnitName = g.PurchaseMinimumUnitName,
                CostPrice = g.CostPrice,
                CostPriceUnitName = g.CostPriceUnitName,
                PriceUnitName = g.PriceUnitName,
                Status = r.Status,
                PurchaseOrderNo = r.PurchaseOrderNo,
                RequirementQuantity = r.RequirementQuantity,
                IsNeedPurchase = r.IsNeedPurchase,
                NeedPurchase = r.NeedPurchase,
                IsSendMail = r.IsSendMail,
                Remark = r.Remark
            }).SingleAsync();
            data.StatusDesc = EnumHelper.GetDescFromEnumVal<SafetyWarningRecordStatus>(data.Status);
            data.ApprovalHis = await GetWarningApprovalHis(data.DetailId);
            return data;
        }

        public async Task<List<ApprovalHisModel>> GetWarningApprovalHis(int detailId)
        {
            var warningInfo = await Repository.ClientDb.Queryable<InvSafetyWarningRecord>().Where(w => w.DetailId == detailId).SingleAsync();
            return await GetApprovalHis(warningInfo.FlowId, warningInfo.UpdateUserId, warningInfo.UpdateUserName, warningInfo.CreateDate, warningInfo.ApprovalStatus, warningInfo.Remark, ApprovalDataType.SafetyInventory);
        }

        private async Task<bool> _isSafetyInvenstoryApproval()
        {
            var isSafetyInventoryApproval = false;
            var safetyInventoryApprovalArgs = await _sysArgsService.GetValueByKey(BusinessConst.IsSafetyInventoryApproval);
            if (safetyInventoryApprovalArgs != null)
            {
                bool.TryParse(safetyInventoryApprovalArgs.Value.ToString(), out isSafetyInventoryApproval);
            }
            return isSafetyInventoryApproval;
        }

        private async Task<bool> _isPurchaseOrderApproval()
        {
            var isSafetyInventoryApproval = false;
            var safetyInventoryApprovalArgs = await _sysArgsService.GetValueByKey(BusinessConst.IsPurchaseOrderApproval);
            if (safetyInventoryApprovalArgs != null)
            {
                bool.TryParse(safetyInventoryApprovalArgs.Value.ToString(), out isSafetyInventoryApproval);
            }
            return isSafetyInventoryApproval;
        }

        /// <summary>
        /// 安全库存检查 
        /// 同一个物料只有当Status=Received或NoPurchase时，才会再次新增一条记录
        /// </summary>
        public void CreateSafetyWarningRecord()
        {
            //查询已经低于安全库存的物料
            var warningGoods = Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, t) => g.GoodsClassifyId == t.TypeId)
                .Where((g, t) => g.SafetyInventory > 0 && g.IsValid && !g.IsDeleted)
                .Select((g, t) => new InvSafetyWarningRecordDto
                {
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsModel = g.GoodsModel,
                    AreaName = g.ForArea,
                    Supplier = g.Supplier,
                    GoodsClassifyTypeName = t.TypeName,
                    GoodsClassifyGroup = t.Group,
                    SafetyInventory = g.SafetyInventory,
                    SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                    PurchaseMinimum = g.PurchaseMinimum,
                    PurchaseMinimumUnitName = g.PurchaseMinimumUnitName,
                    CostPrice = g.CostPrice,
                    PriceUnitName = g.PriceUnitName,
                    CostPriceUnitName = g.CostPriceUnitName,
                    IsValid = g.IsValid,
                    IsNeedPurchase = g.IsNotPurchase == false,
                    CurInvenstory = SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(w => w.GoodsId == g.GoodsId && w.UnitId == g.SafetyInventoryUnitId).Sum(s => s.Stock)
                }).MergeTable().Where(w => w.CurInvenstory < w.SafetyInventory && w.IsNeedPurchase).ToList();
            _logHelper.LogInfo("CreateSafetyWarningRecord", $"监测到低于安全库存的物料数：{warningGoods.Count}", "");
            //检查预警信息表中当前物料是否存在状态=已预警的信息，如果不存在则写入预警信息表
            if (warningGoods.Count > 0)
            {
                try
                {
                    var warningGoodsId = warningGoods.Select(s => s.GoodsId).ToList();
                    var existsData = Repository.ClientDb.Queryable<InvSafetyWarningRecord>().Where(w => warningGoodsId.Contains(w.GoodsId)).ToList();
                    var curYear = DateTime.Now.Year;
                    var curMonth = int.Parse(DateTime.Now.ToString("yyyyMM"));
                    var curWeek = new GregorianCalendar().GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                    var addWarningList = new List<InvSafetyWarningRecord>();
                    foreach (var goods in warningGoods)
                    {
                        goods.Year = curYear;
                        goods.Month = curMonth;
                        goods.Week = curWeek;
                        goods.Status = SafetyWarningRecordStatus.Warning.ToString();
                        //var existsEntity = existsData.SingleOrDefault(s => s.GoodsId == goods.GoodsId);
                        var existsEntity = existsData.Where(s => s.GoodsId == goods.GoodsId).ToList();
                        if (existsEntity == null || existsEntity.Count == 0)
                        {
                            var newWarningEntity = new InvSafetyWarningRecord
                            {
                                Year = curYear,
                                Month = curMonth,
                                Week = curWeek,
                                GoodsId = goods.GoodsId,
                                IsNeedPurchase = true,
                                GoodsClassifyGroup = goods.GoodsClassifyGroup,
                                Status = SafetyWarningRecordStatus.Warning.ToString(),
                                CreateDate = DateTime.Now,
                                CreateUserName = "Sys",
                                FlowId = Guid.NewGuid().ToString("N").ToUpper(),
                                ApprovalStatus = ApprovalStatus.NoApproval.ToString(),
                            };
                            addWarningList.Add(newWarningEntity);
                        }
                        else
                        {
                            string bs = "0";
                            for (var i = 0; i < existsEntity.Count; i++)
                            {
                                //循环判断是否都为状态为已到货 或不采购 如果都为已到货或不采访，那么新增一条数据
                                if (existsEntity[i].Status != SafetyWarningRecordStatus.Received.ToString() && existsEntity[i].Status != SafetyWarningRecordStatus.NoPurchase.ToString())
                                {
                                    bs = "1";
                                }

                            }
                            if (bs == "0")
                            {
                                var newWarningEntity = new InvSafetyWarningRecord
                                {
                                    Year = curYear,
                                    Month = curMonth,
                                    Week = curWeek,
                                    GoodsId = goods.GoodsId,
                                    IsNeedPurchase = true,
                                    GoodsClassifyGroup = goods.GoodsClassifyGroup,
                                    Status = SafetyWarningRecordStatus.Warning.ToString(),
                                    CreateDate = DateTime.Now,
                                    CreateUserName = "Sys",
                                    FlowId = Guid.NewGuid().ToString("N").ToUpper(),
                                    ApprovalStatus = ApprovalStatus.NoApproval.ToString(),
                                };
                                addWarningList.Add(newWarningEntity);
                            }
                        }
                    }
                    if (addWarningList.Count > 0)
                    {
                        Repository.ClientDb.Insertable(addWarningList).ExecuteCommand();
                    }
                    bool isAutoSendMail = false;
                    var args = _sysArgsService.GetValueByKey(BusinessConst.IsAutoSendMailBySavetyInvenstory).Result;
                    bool.TryParse(args.Value.ToString(), out isAutoSendMail);
                    if (isAutoSendMail)
                    {
                        var groups = warningGoods.Select(s => s.GoodsClassifyGroup).Distinct().ToList();
                        //邮件中需要过滤掉设置为不需要采购的和采购中的数据
                        var filterGoodsId = existsData.Where(w => w.Status == SafetyWarningRecordStatus.Purchasing.ToString() || !w.IsNeedPurchase).Select(w => w.GoodsId).ToList();
                        foreach (var group in groups)
                        {
                            var groupData = warningGoods.Where(w => w.GoodsClassifyGroup == group && !filterGoodsId.Contains(w.GoodsId)).ToList();
                            SendMailToWarning(groupData).Wait();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logHelper.LogError("CreateSafetyWarningRecord", ex.Message, "", ex, ex.StackTrace);
                }
            }
        }

        public async Task UpdateReceived(int[] detailId, string userId, string userName)
        {
            var warningInfo = await Repository.ClientDb.Queryable<InvSafetyWarningRecord>().Where(w => detailId.Contains(w.DetailId)).ToListAsync();
            foreach (var info in warningInfo)
            {
                if (info.Status != SafetyWarningRecordStatus.Purchasing.ToString())
                {
                    throw new BusinessException("当前库存报警信息不在采购中");
                }
                info.Status = SafetyWarningRecordStatus.Received.ToString();
                info.UpdateDate = DateTime.Now;
                info.UpdateUserId = userId;
                info.UpdateUserName = userName;
            }
            await Repository.ClientDb.Updateable(warningInfo).ExecuteCommandAsync();
        }

        public async Task UpdateWarningInfo(InvSafetyWarningRecordDto data)
        {
            if (data.Status != SafetyWarningRecordStatus.Warning.ToString())
            {
                throw new BusinessException("当前安全库存预计信息已编辑处理，无法再次编辑");
            }
            var goods = await Repository.GetSingeAsync<BaseGoods>(data.GoodsId);
            //如果修改了物料的安全库存，且修改值大于当前库存，则删除当前预警信息，同时更新物料安全库存信息
            if (goods.SafetyInventory != data.SafetyInventory)
            {
                var stock = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().Where(w => w.GoodsId == goods.GoodsId && w.UnitId == goods.SafetyInventoryUnitId).SumAsync(s => s.Stock);
                goods.SafetyInventory = data.SafetyInventory;
                if (stock >= data.SafetyInventory || data.SafetyInventory == 0)
                {
                    Repository.ClientDb.Updateable(goods).AddQueue();
                    Repository.ClientDb.Deleteable<InvSafetyWarningRecord>(d => d.DetailId == data.DetailId).AddQueue();
                    await Repository.ClientDb.SaveQueuesAsync();
                    return;
                }
            }
            if (!data.IsNeedPurchase)
            {
                if (string.IsNullOrEmpty(data.Remark))
                {
                    throw new BusinessException("如果该物料无需采购，请填写备注说明原因");
                }
            }
            else
            {
                if (data.RequirementQuantity <= 0)
                {
                    throw new BusinessException("如果该物料需要采购，请填写采购数量");
                }
            }

            var isSafetyInventoryApproval = await _isSafetyInvenstoryApproval();
            var entity = await Repository.GetSingeAsync<InvSafetyWarningRecord>(data.DetailId);
            entity.IsNeedPurchase = data.IsNeedPurchase;
            entity.PurchaseOrderNo = data.PurchaseOrderNo;
            entity.RequirementQuantity = data.RequirementQuantity;
            entity.UpdateUserId = data.UpdateUserId;
            entity.UpdateUserName = data.UpdateUserName;
            entity.Remark = data.Remark;
            entity.UpdateDate = DateTime.Now;
            if (data.IsNeedPurchase)
            {
                entity.Status = SafetyWarningRecordStatus.Purchasing.ToString();
                entity.NeedPurchase = "需要";
            }
            else
            {
                entity.Status = SafetyWarningRecordStatus.NoPurchase.ToString();
                entity.NeedPurchase = "不需要";
            }
            //检查审批
            if (isSafetyInventoryApproval)
            {
                //审批信息(如果当前创建人也是审批人,则需要修改和添加相应的审批信息) 
                var process = await GetApprovalProcess(ApprovalDataType.SafetyInventory.ToString());
                var curApprover = process.Where(a => a.ApproverId == data.UpdateUserId).ToList();
                if (curApprover?.Count > 0)
                {
                    var hightApprover = curApprover.OrderByDescending(x => x.Rank).First();
                    entity.ApprovalDate = DateTime.Now;
                    entity.ApproverId = data.UpdateUserId;
                    entity.ApproverName = data.UpdateUserName;
                    entity.ApproverRole = hightApprover.ApproverRole;
                    if (hightApprover.IsLastApproval)
                    {
                        entity.ApprovalStatus = ApprovalStatus.Approve.ToString();
                    }
                    else
                    {
                        entity.ApprovalStatus = ApprovalStatus.Approvaling.ToString();
                    }
                    if (hightApprover.ApprovalModel == ApprovalModel.Process.ToString())
                    {
                        var newApprovalHisArr = new List<ApprovalHis>();
                        foreach (var item in curApprover.OrderBy(x => x.Rank))
                        {
                            var newApprovalHis = new ApprovalHis
                            {
                                ApprovalDate = DateTime.Now,
                                ApprovalModel = hightApprover.ApprovalModel,
                                ApprovalStatus = ApprovalStatus.Approve.ToString(),
                                ApproverId = data.UpdateUserId,
                                ApproverName = data.UpdateUserName,
                                ApproverRoleId = item.ApproverRole,
                                ApproverRoleName = item.ApproverRoleName,
                                DataType = ApprovalDataType.SafetyInventory.ToString(),
                                Opinion = "自动处理",
                                ApprovalRank = item.Rank,
                                PrimaryId = entity.FlowId
                            };
                            newApprovalHisArr.Add(newApprovalHis);
                        }
                        Repository.ClientDb.Insertable(newApprovalHisArr).AddQueue();
                    }
                    else
                    {
                        var approvalHis = new ApprovalHis
                        {
                            ApprovalDate = DateTime.Now,
                            ApprovalModel = hightApprover.ApprovalModel,
                            ApprovalStatus = ApprovalStatus.Approve.ToString(),
                            ApproverId = data.UpdateUserId,
                            ApproverName = data.UpdateUserName,
                            ApproverRoleId = hightApprover.ApproverRole,
                            ApproverRoleName = hightApprover.ApproverRoleName,
                            DataType = ApprovalDataType.SafetyInventory.ToString(),
                            Opinion = "自动处理",
                            ApprovalRank = hightApprover.Rank,
                            PrimaryId = entity.FlowId
                        };
                        Repository.ClientDb.Insertable(approvalHis).AddQueue();
                    }
                    if (entity.ApprovalStatus == ApprovalStatus.Approve.ToString())
                    {
                        if (data.IsNeedPurchase)
                        {
                            //选择需要采购时，创建采购订单
                            var supplier = await Repository.ClientDb.Queryable<BaseSuppliers>().Where(w => w.SupplierName == goods.Supplier).FirstAsync();
                            var lastData = await Repository.ClientDb.Queryable<PurchaseOrder>().MaxAsync(x => x.OrderNo);
                            var goodsClassifyGroup = await Repository.ClientDb.Queryable<BaseType>().Where(w => w.TypeId == goods.GoodsClassifyId).Select(w => w.Group).SingleAsync();
                            var purchaseOrder = new PurchaseOrder
                            {
                                OrderNo = GetPrimaryId("P", lastData),
                                GoodsId = data.GoodsId,
                                GoodsNameZH = data.GoodsName,
                                GoodsModel = data.GoodsModel,
                                GoodsClassifyGroup = goodsClassifyGroup,
                                IsPurchaseBuy = true,
                                GoodsClassifyId = goods.GoodsClassifyId,
                                SafetyInventory = goods.SafetyInventory,
                                MNANo = goods.GoodsNo,
                                GoodsNo = goods.GoodsNo,
                                Quantity = data.RequirementQuantity,
                                QuantityUnitId = goods.SafetyInventoryUnitId,
                                QuantityUnitName = goods.SafetyInventoryUnitName,
                                MinLotSize = goods.PurchaseMinimum,
                                SupplierName = goods.Supplier,
                                SupplierNo = supplier?.SupplierNo,
                                GoodsSpecificationId = goods.GoodsSpecificationId,
                                MaxStock = goods.MaxStock,
                                Remark = "由安全库存报警自动触发的采购订单",
                                CreateUserName = data.UpdateUserName,
                                CreateUserId = data.UpdateUserId,
                                CreateDate = DateTime.Now
                            };
                            lastData = purchaseOrder.OrderNo;
                            var isPurchaseApproval = await _isPurchaseOrderApproval();
                            if (isPurchaseApproval)
                            {
                                purchaseOrder.Status = PurchaseOrderStatus.Pending.ToString();
                                purchaseOrder.FlowStatus = "";
                                purchaseOrder.ApprovalStatus = ApprovalStatus.Pending.ToString();
                            }
                            else
                            {
                                purchaseOrder.Status = PurchaseOrderStatus.WaitReceiving.ToString();
                                purchaseOrder.FlowStatus = PurchaseOrderFlowStatus.WaitingReceiving.ToString();
                                purchaseOrder.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
                            }
                            Repository.ClientDb.Insertable(purchaseOrder).AddQueue();
                        }
                        else
                        {
                            goods.IsNotPurchase = true;
                            goods.Remark = data.Remark;
                            goods.ModifyDate = DateTime.Now;
                            goods.ModifyUser = data.CreateUserName;
                            Repository.ClientDb.Updateable(goods).AddQueue();
                        }
                    }
                }
                else
                {
                    entity.ApprovalStatus = ApprovalStatus.Pending.ToString();
                }
            }
            else
            {
                entity.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
                if (data.IsNeedPurchase)
                {
                    //选择需要采购时，创建采购订单
                    var supplier = await Repository.ClientDb.Queryable<BaseSuppliers>().Where(w => w.SupplierName == goods.Supplier).FirstAsync();
                    var lastData = await Repository.ClientDb.Queryable<PurchaseOrder>().MaxAsync(x => x.OrderNo);
                    var goodsClassifyGroup = await Repository.ClientDb.Queryable<BaseType>().Where(w => w.TypeId == goods.GoodsClassifyId).Select(w => w.Group).SingleAsync();
                    var purchaseOrder = new PurchaseOrder
                    {
                        OrderNo = GetPrimaryId("P", lastData),
                        GoodsId = data.GoodsId,
                        GoodsNameZH = data.GoodsName,
                        GoodsModel = data.GoodsModel,
                        GoodsClassifyGroup = goodsClassifyGroup,
                        GoodsClassifyId = goods.GoodsClassifyId,
                        SafetyInventory = goods.SafetyInventory,
                        IsPurchaseBuy = true,
                        MNANo = goods.GoodsNo,
                        GoodsNo = goods.GoodsNo,
                        Quantity = data.RequirementQuantity,
                        QuantityUnitId = goods.SafetyInventoryUnitId,
                        QuantityUnitName = goods.SafetyInventoryUnitName,
                        MinLotSize = goods.PurchaseMinimum,
                        SupplierName = goods.Supplier,
                        SupplierNo = supplier?.SupplierNo,
                        GoodsSpecificationId = goods.GoodsSpecificationId,
                        MaxStock = goods.MaxStock,
                        Remark = "由安全库存报警自动触发的采购订单",
                        CreateUserName = data.UpdateUserName,
                        CreateUserId = data.UpdateUserId,
                        CreateDate = DateTime.Now
                    };
                    lastData = purchaseOrder.OrderNo;
                    var isPurchaseApproval = await _isPurchaseOrderApproval();
                    if (isPurchaseApproval)
                    {
                        purchaseOrder.Status = PurchaseOrderStatus.Pending.ToString();
                        purchaseOrder.FlowStatus = "";
                        purchaseOrder.ApprovalStatus = ApprovalStatus.Pending.ToString();
                    }
                    else
                    {
                        purchaseOrder.Status = PurchaseOrderStatus.WaitReceiving.ToString();
                        purchaseOrder.FlowStatus = PurchaseOrderFlowStatus.WaitingReceiving.ToString();
                        purchaseOrder.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
                    }
                    Repository.ClientDb.Insertable(purchaseOrder).AddQueue();
                }
                else
                {
                    goods.IsNotPurchase = true;
                    goods.Remark = data.Remark;
                    goods.ModifyDate = DateTime.Now;
                    goods.ModifyUser = data.CreateUserName;
                    Repository.ClientDb.Updateable(goods).AddQueue();
                }
            }
            Repository.ClientDb.Updateable(entity).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task ApprovalSafetyInventory(string[] flowId, bool isApprove, string opinion, string goodsClassifyGroup, string userId, string userName)
        {
            var approvalStatus = isApprove ? ApprovalStatus.Approve.ToString() : ApprovalStatus.Reject.ToString();
            var orders = await Repository.ClientDb.Queryable<InvSafetyWarningRecord>().Where(x => flowId.Contains(x.FlowId)).ToListAsync();
            foreach (var order in orders)
            {
                if (order.ApprovalStatus != ApprovalStatus.Pending.ToString() && order.Status != ApprovalStatus.Approvaling.ToString())
                {
                    throw new BusinessException("审批失败,所选数据中存在不在审批流程中的数据");
                }
            }
            //查询当前审批流程信息
            var process = await GetApprover(ApprovalDataType.SafetyInventory.ToString(), userId);
            if (process.Count == 0)
            {
                throw new BusinessException("审批失败,当前用户没有审批权限");
            }
            var apprivalModel = process.First().ApprovalModel;
            //流程审批模式：如果当前审批用户拥有多个审批角色，将以最高审批节点角色来审批，同时还需要将该用户的其他角色以自动审批模式记录下来
            if (apprivalModel == ApprovalModel.Process.ToString())
            {
                //判断上一级是否已审批
                var approvalHis = await Repository.ClientDb.Queryable<ApprovalHis>().Where(a => a.DataType == ApprovalDataType.SafetyInventory.ToString() && flowId.Contains(a.PrimaryId)).ToListAsync();
                foreach (var curItem in orders)
                {
                    var lastRank = approvalHis.Count == 0 ? 0 : approvalHis.Where(h => h.PrimaryId == curItem.FlowId).Max(h => h.ApprovalRank);
                    var approverRank = process.Select(x => x.Rank).Distinct().ToList();
                    if (!approverRank.Contains(lastRank + 1))
                    {
                        throw new BusinessException($"审批失败,流程：{curItem.FlowId}需要等待下级审批");
                    }
                }
                if (!process.Exists(x => x.IsLastApproval) && isApprove)
                {
                    approvalStatus = ApprovalStatus.Approvaling.ToString();
                }
            }
            //更新安全库存审批状态 ,当审批通过时根据是否需要采购做对应的处理
            var hightApprover = process.OrderByDescending(x => x.Rank).First();
            var goodsIdArr = orders.Select(s => s.GoodsId).Distinct().ToArray();
            var goodsInfoArr = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsIdArr.Contains(w.GoodsId)).ToListAsync();
            var supplierNameArr = goodsInfoArr.Select(s => s.Supplier).ToList();
            var supplierInfoArr = await Repository.ClientDb.Queryable<BaseSuppliers>().Where(w => supplierNameArr.Contains(w.SupplierName)).Select(w => new { w.SupplierNo, w.SupplierName }).ToListAsync();
            var updateGoods = new List<BaseGoods>();
            var insertPurchase = new List<PurchaseOrder>();
            var lastData = await Repository.ClientDb.Queryable<PurchaseOrder>().MaxAsync(x => x.OrderNo);
            foreach (var x in orders)
            {
                x.ApprovalDate = DateTime.Now;
                x.ApproverId = userId;
                x.ApproverName = userName;
                x.ApproverRole = hightApprover.ApproverRole;
                x.ApprovalStatus = approvalStatus;
                if (approvalStatus == ApprovalStatus.Approve.ToString())
                {
                    var curGoods = goodsInfoArr.Single(s => s.GoodsId == x.GoodsId);
                    if (x.IsNeedPurchase)
                    {
                        x.Status = SafetyWarningRecordStatus.Purchasing.ToString();
                        //选择需要采购时，创建采购订单
                        var purchaseOrder = new PurchaseOrder
                        {
                            OrderNo = GetPrimaryId("P", lastData),
                            GoodsId = x.GoodsId,
                            GoodsNameZH = curGoods.GoodsName,
                            GoodsModel = curGoods.GoodsModel,
                            GoodsClassifyGroup = goodsClassifyGroup,
                            GoodsClassifyId = curGoods.GoodsClassifyId,
                            SafetyInventory = curGoods.SafetyInventory,
                            IsPurchaseBuy = true,
                            MNANo = curGoods.GoodsNo,
                            GoodsNo = curGoods.GoodsNo,
                            Quantity = x.RequirementQuantity,
                            QuantityUnitId = curGoods.SafetyInventoryUnitId,
                            QuantityUnitName = curGoods.SafetyInventoryUnitName,
                            MinLotSize = curGoods.PurchaseMinimum,
                            SupplierName = curGoods.Supplier,
                            SupplierNo = supplierInfoArr.SingleOrDefault(s => s.SupplierName == curGoods.Supplier)?.SupplierNo,
                            GoodsSpecificationId = curGoods.GoodsSpecificationId,
                            MaxStock = curGoods.MaxStock,
                            Remark = "由安全库存报警自动触发的采购订单",
                            CreateUserName = x.UpdateUserName,
                            CreateUserId = x.UpdateUserId,
                            CreateDate = DateTime.Now
                        };
                        lastData = purchaseOrder.OrderNo;
                        var isPurchaseApproval = await _isPurchaseOrderApproval();
                        if (isPurchaseApproval)
                        {
                            purchaseOrder.Status = PurchaseOrderStatus.Pending.ToString();
                            purchaseOrder.FlowStatus = "";
                            purchaseOrder.ApprovalStatus = ApprovalStatus.Pending.ToString();
                        }
                        else
                        {
                            purchaseOrder.Status = PurchaseOrderStatus.WaitReceiving.ToString();
                            purchaseOrder.FlowStatus = PurchaseOrderFlowStatus.WaitingReceiving.ToString();
                            purchaseOrder.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
                        }
                        insertPurchase.Add(purchaseOrder);
                    }
                    else
                    {
                        //选择不需要采购时，修改物料信息IsNotPurchase 
                        x.Status = SafetyWarningRecordStatus.NoPurchase.ToString();
                        if (curGoods != null)
                        {
                            curGoods.IsNotPurchase = true;
                            curGoods.Remark = x.Remark;
                            curGoods.ModifyDate = DateTime.Now;
                            curGoods.ModifyUser = userName;
                            updateGoods.Add(curGoods);
                        }
                    }
                }
                else if (approvalStatus == ApprovalStatus.Reject.ToString())
                {
                    x.Status = SafetyWarningRecordStatus.Warning.ToString();
                }
            }
            Repository.ClientDb.Updateable(orders).AddQueue();
            if (updateGoods.Count > 0)
            {
                Repository.ClientDb.Updateable(updateGoods).AddQueue();
            }
            if (insertPurchase.Count > 0)
            {
                Repository.ClientDb.Insertable(insertPurchase).AddQueue();
            }
            //添加审批历史记录
            var approvalHisList = new List<ApprovalHis>();
            foreach (var id in flowId)
            {
                if (apprivalModel == ApprovalModel.Process.ToString())
                {
                    if (process.Count > 0)
                    {
                        foreach (var item in process.OrderBy(x => x.Rank))
                        {
                            var newApprovalHis = new ApprovalHis
                            {
                                ApprovalDate = DateTime.Now,
                                ApprovalModel = apprivalModel,
                                ApprovalStatus = isApprove ? ApprovalStatus.Approve.ToString() : ApprovalStatus.Reject.ToString(),
                                ApproverId = userId,
                                ApproverName = userName,
                                ApproverRoleId = item.ApproverRole,
                                ApproverRoleName = item.ApproverRoleName,
                                DataType = ApprovalDataType.SafetyInventory.ToString(),
                                Opinion = item.Rank == hightApprover.Rank ? opinion : "自动处理",
                                ApprovalRank = item.Rank,
                                PrimaryId = id
                            };
                            approvalHisList.Add(newApprovalHis);
                        }
                    }
                }
                else
                {
                    var newApprovalHis = new ApprovalHis
                    {
                        ApprovalDate = DateTime.Now,
                        ApprovalModel = apprivalModel,
                        ApprovalStatus = isApprove ? ApprovalStatus.Approve.ToString() : ApprovalStatus.Reject.ToString(),
                        ApproverId = userId,
                        ApproverName = userName,
                        ApproverRoleId = hightApprover.ApproverRole,
                        ApproverRoleName = hightApprover.ApproverRoleName,
                        DataType = ApprovalDataType.SafetyInventory.ToString(),
                        Opinion = opinion,
                        ApprovalRank = hightApprover.Rank,
                        PrimaryId = id
                    };
                    approvalHisList.Add(newApprovalHis);
                }
            }
            Repository.ClientDb.Insertable(approvalHisList).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
            if (insertPurchase.Count > 0)
            {
                _ = Task.Run(() =>
                {
                    _ = _sendMailToPurchase(insertPurchase);
                });
            }
        }

        private async Task _sendMailToPurchase(List<PurchaseOrder> orders)
        {
            var creators = orders.Select(s => s.CreateUserId).ToList();
            var usersEmail = await Repository.ClientDb.Queryable<SysUser>().Where(w => creators.Contains(w.UserId) && !SqlFunc.IsNullOrEmpty(w.Email)).Select(w => w.Email).ToArrayAsync();
            var goodsNames = orders.Select(s => s.GoodsNameZH).ToList();
            if (usersEmail?.Length > 0)
            {
                var groupDesc = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(orders[0].GoodsClassifyGroup);
                var subject = $"{groupDesc}采购订单信息完善";
                var body = $"由安全库存预警触发了采购订单，请完善采购订单明细，物料如下：{string.Join(',', goodsNames)}";
                await _emailService.SendEmail(subject, body, usersEmail, null, "");
            }
        }

        public List<KeyValueModel> GetExportFields()
        {
            return new List<KeyValueModel>
            {
                new KeyValueModel {Key="Month",Value="年月",Remark=4},
                new KeyValueModel {Key="Supplier",Value="供应商",Remark=4},
                new KeyValueModel {Key="GoodsNo",Value="物料编码",Remark=5},
                new KeyValueModel {Key="GoodsName",Value="物料名称", Remark = 6},
                new KeyValueModel {Key="GoodsModel",Value="物料型号", Remark = 7},
                new KeyValueModel {Key="Group",Value="物料分类",Remark=8},
                new KeyValueModel {Key="TypeName",Value="物料类型", Remark = 9},
                new KeyValueModel {Key="ForArea",Value="所属区域",Remark=10},
                new KeyValueModel {Key="CurInvenstory",Value="现存量", Remark = 11},
                new KeyValueModel {Key="SafetyInventory",Value="安全库存", Remark = 12},
                new KeyValueModel {Key="SafetyInventoryUnitName",Value="库存单位", Remark = 12},
                new KeyValueModel {Key="PurchaseMinimum",Value="最小采购量", Remark = 13},
                new KeyValueModel {Key="PurchaseMinimumUnitName",Value="采购单位", Remark = 14},
                new KeyValueModel {Key="CostPrice",Value="单价", Remark = 15},
                new KeyValueModel {Key="PriceUnitName",Value="价格单位", Remark = 16},
                new KeyValueModel {Key="Status",Value="状态", Remark = 17},
                new KeyValueModel {Key="NeedPurchase",Value="是否需要采购", Remark = 18},
                new KeyValueModel {Key="PurchaseOrderNo",Value="采购单号", Remark = 19},
                new KeyValueModel {Key="RequirementQuantity",Value="采购数量", Remark = 20},
                new KeyValueModel {Key="Remark",Value="备注", Remark = 21},
                new KeyValueModel {Key="CreateDate",Value="创建时间", Remark = 21}
            };
        }
  

        public async Task<string> ExportWarningInfo(string orderField, string orderType, string searchKey, string goodsGroup, int goodsClassifyType, string status, List<KeyValueModel> fields)
        {
            if (fields.Count > 0)
            {
                var dbType = _configuration.GetSection("Sqlsugar:DbType").Value;
                if (string.IsNullOrEmpty(orderField))
                {
                    orderField = "A.CreateDate";
                }
                else if (orderField.ToLower() == "createdate")
                {
                    orderField = "A.CreateDate";
                }
                searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
                var param = new Dictionary<string, object>
                    {
                        { "@GoodsName", "%"+searchKey+"%" },
                        { "@GoodsId", "%"+searchKey+"%" },
                        { "@GoodsNo", "%"+searchKey+"%" },
                        { "@Supplier", "%"+searchKey+"%" },
                        { "@GoodsModel","%"+searchKey+"%"},
                        { "@goodsGroup", goodsGroup }
                    };
                StringBuilder sb = new StringBuilder();
                foreach (var field in fields)
                {
                    if (field.Key.ToString() == "CurInvenstory")
                    {
                        sb.Append($"(SELECT SUM(Stock) FROM InvStorageWarehouseDetail WHERE GoodsId=A.GoodsId AND UnitId=B.SafetyInventoryUnitId) AS {field.Value.ToString()},");
                    }
                    else if (field.Key.ToString() == "Group" || field.Key.ToString() == "TypeName")
                    {
                        sb.Append($"C.{field.Key} AS {field.Value},");
                    }
                    else if (field.Key.ToString() == "Remark" || field.Key.ToString() == "CreateDate")
                    {
                        sb.Append($"A.{field.Key} AS {field.Value},");
                    }
                    else
                    {
                        sb.Append($"{field.Key} AS {field.Value},");
                    }
                }
                var fieldStr = sb.ToString().TrimEnd(',');
                string sql = $@" SELECT {fieldStr} FROM InvSafetyWarningRecord A 
                                INNER JOIN BaseGoods B ON A.GoodsId=B.GoodsId
                                INNER JOIN BaseType C ON C.TypeId=B.GoodsClassifyId
                                WHERE (B.GoodsId LIKE @GoodsId OR B.GoodsNo LIKE @GoodsNo OR B.GoodsName LIKE @GoodsName OR B.GoodsModel LIKE @GoodsModel OR B.Supplier LIKE @Supplier)
                                AND C.Group=@goodsGroup 
                                AND (case when {goodsClassifyType}=0 then 1=1 else C.TypeId={goodsClassifyType} end)
                                AND (case when '{status}'='' then 1=1 else A.Status='{status}' end)
                                ORDER BY {orderField} {orderType}";
                var queryData = await Repository.QueryBySqlAsync(sql, param);
                var stream = ExcelHelper.ConvertDataTableToStream(queryData);
                var fileName = $"{EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(goodsGroup)}安全库存预警信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
                var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
                return fileUrl;
            }
            return await Task.FromResult("");
        }

        public async Task SendMailToWarning(List<InvSafetyWarningRecordDto> data)
        {
            var first = data.First();
            var goodsGroupDesc = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(first.GoodsClassifyGroup);
            var rep = Repository.ClientDb.Queryable<InvWarehouse>().Where(w => w.WarehouseType == first.GoodsClassifyGroup && !SqlFunc.IsNullOrEmpty(w.ChargePersonPhone)).Select(s => s.ChargePersonPhone).ToList();
            if (rep.Count > 0)
            {
                var attachment = await CreateAttachment(data);
                var mail = new SysMail
                {
                    ToReceiver = string.Join(',', rep),
                    Subject = $"{goodsGroupDesc}安全库存预警",
                    Body = "该邮件由系统自动发送，详细请查看附件",
                    Sender = $"{_configuration.GetSection("MailConfig:Sender").Value}",
                    AttachmentId = Guid.NewGuid().ToString("N"),
                    BusinessType = BusinessType.SafetyInvenstoryWarning.ToString(),
                    CreateDate = DateTime.Now,
                };
                await _emailService.SendEmail(mail.Subject, mail.Body, mail.ToReceiver.Split(','), null, attachment == null ? "" : attachment.Path);
                Repository.ClientDb.Insertable(mail).AddQueue();
                if (attachment != null)
                {
                    var attachmentEntity = new BaseFiles
                    {
                        PrimaryId = mail.AttachmentId,
                        FileName = attachment.FileName,
                        FileInfoType = FileInfoType.MaterialRequirementPlanAttachment.ToString(),
                        Path = attachment.Path,
                        Url = attachment.Url
                    };
                    Repository.ClientDb.Insertable(attachmentEntity).AddQueue();
                }
                await Repository.ClientDb.SaveQueuesAsync();
            }
            else
            {
                _logHelper.LogInfo("SendMailToWarning", $"发送邮件失败，收件人信息为空，请添加{goodsGroupDesc}仓库对应的负责人", "");
            }
        }

        public async Task<FileInfoDto> CreateAttachment(List<InvSafetyWarningRecordDto> data)
        {
            var fields = GetExportFields();
            var tb = new DataTable();
            var type = typeof(InvSafetyWarningRecordDto);
            fields.ForEach(f => tb.Columns.Add(f.Value.ToString(), typeof(string)));
            foreach (var record in data)
            {
                var row = tb.NewRow();
                var props = type.GetProperties();
                foreach (var prop in props)
                {
                    var field = fields.SingleOrDefault(s => s.Key.ToString() == prop.Name);
                    if (field != null)
                    {
                        var v = record.GetType().GetProperty(field.Key.ToString()).GetValue(record);
                        row[field.Value.ToString()] = v != null ? v.ToString() : "";
                    }
                }
                tb.Rows.Add(row);
            }
            var stream = ExcelHelper.ConvertDataTableToStream(tb);
            var goodsGroupDesc = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(data.First().GoodsClassifyGroup);
            var fileName = $"{goodsGroupDesc}安全库存预警信息{DateTime.Now.ToString("yyyy-MM-dd")}).xlsx";
            return await _fileStorage.GetFileAndSave(fileName, stream, FileType.Excel);
        }
    

   
}
    
}
