using AutoMapper;
using Logic.LogicBase;
using Logic.LogicCommon.FileStorage;
using Logic.LogicCommon; 
using System.Text; 
using DbRepository.Repository;
using External.Common; 
using DbRepository.Repository.DbModels;
using Models.Model.Enum;
using Models.Model.Purchase;
using Models.Model;
using SqlSugar; 
using Microsoft.Extensions.Configuration;
using External.Common.Extension;
using Models.Model.Sys;
using System.Data;
using NPOI.SS.Formula.Functions;

namespace Logic.Purchase
{
    public class PurchaseOrderMgr : ApprovalHandler
    {
        private readonly EmailService _emailService;

        private readonly SysArgsService _sysArgsHelper;

        private readonly IMapper _mapper;

        private readonly IFileStorage _fileStorage;

        private readonly IConfiguration _configuration;

        private readonly HttpHelperAsync _httpHelper;

        public PurchaseOrderMgr(Repository repository, EmailService emailService, SysArgsService sysArgsService, IMapper mapper, IFileStorage fileStorage, IConfiguration configuration, HttpHelperAsync httpHelper) : base(repository)
        {
            _emailService = emailService;
            _sysArgsHelper = sysArgsService;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _configuration = configuration;
            _httpHelper = httpHelper;
        }

        public async Task<TableModel<PurchaseOrderDto>> GetOrders(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, int purchaseTypeId, string flowStatus, string goodsClassifyGroup, int goodsClassifyId)
        { 
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "CreateDate" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var curApprover = await GetApprover(ApprovalDataType.Purchase.ToString(), userId);
            var approvalModel = curApprover.FirstOrDefault()?.ApprovalModel;
            var curApproverRank = curApprover.Select(x => x.Rank).Distinct().ToList();
            var isAnyApproval = approvalModel == ApprovalModel.Any.ToString() && curApprover?.Count > 0;
            var data = Repository.ClientDb.Queryable<PurchaseOrder>()
                  .LeftJoin<SysArgsOptions>((p, a) => p.PurchaseTypeId == a.OptionId)
                  .Where((p, a) => p.OrderNo.Contains(searchKey) || p.Description.Contains(searchKey) || p.ExternalOrderNo.Contains(searchKey) || p.CreateUserName.Contains(searchKey) || p.SupplierName.Contains(searchKey)||p.SupplierNo.Contains(searchKey)
                   || p.GoodsNo.Contains(searchKey) || p.GoodsNameZH.Contains(searchKey) || p.GoodsNameEN.Contains(searchKey)  || p.Consignee.Contains(searchKey) || p.GoodsModel.Contains(searchKey)||p.PONumber.Contains(searchKey)||p.PRNumber.Contains(searchKey))
                  .Where((p, a) => p.GoodsClassifyGroup == goodsClassifyGroup && p.CreateDate.Date >= GetDateStart(dateStart).Date && p.CreateDate.Date <= GetDateEnd(dateEnd).Date) 
                  .WhereIF(purchaseTypeId > 0, (p, a) => p.PurchaseTypeId == purchaseTypeId)
                  .WhereIF(goodsClassifyId>0, (p, a) => p.GoodsClassifyId == goodsClassifyId)
                  .WhereIF(!string.IsNullOrEmpty(flowStatus), (p, a) => p.FlowStatus == flowStatus)
                  .Select((p, a) => new PurchaseOrderDto
                  {
                      DetialId = p.DetialId,
                      OrderNo = p.OrderNo,
                      PurchaseTypeId = p.PurchaseTypeId,
                      PurchaseTypeDesc=a.OptionName,
                      IsApplyMaterialNumber = p.IsApplyMaterialNumber,
                      IsPurchaseBuy = p.IsPurchaseBuy,
                      IsOnceBuy= p.IsOnceBuy,
                      GoodsNameZH = p.GoodsNameZH,
                      GoodsNameEN = p.GoodsNameEN,
                      GoodsModel = p.GoodsModel,
                      GoodsId = p.GoodsId,
                      GoodsNo = p.GoodsNo,
                      ExternalOrderNo = p.ExternalOrderNo,
                      GoodsClassifyGroup = p.GoodsClassifyGroup,
                      GoodsClassifyName = p.GoodsClassifyName,
                      Quantity = p.Quantity,
                      QuantityActual = p.QuantityActual,
                      QuantityUnitId = p.QuantityUnitId,
                      QuantityUnitName = p.QuantityUnitName,
                      Price = p.Price,
                      DetailTotalPrice = p.DetailTotalPrice,
                      PriceUnitName = p.PriceUnitName,
                      Status = p.Status,
                      FlowStatus = p.FlowStatus,
                      Description = p.Description,
                      ReceivingAbnormalReason= p.ReceivingAbnormalReason,
                      ArrivalAbnormalReason=p.ArrivalAbnormalReason,
                      Line = p.Line, 
                      ApplyReason = p.ApplyReason,
                      SupplierNo = p.SupplierNo,
                      SupplierName = p.SupplierName,
                      QuoteLink = p.QuoteLink,
                      Consignee = p.Consignee,
                      SafetyInventory = p.SafetyInventory,
                      ConsigneeEmail = p.ConsigneeEmail, 
                      IsEmailNotification = p.IsEmailNotification,
                      CreateDate = p.CreateDate,
                      CreateUserId = p.CreateUserId,
                      CreateUserName = p.CreateUserName,
                      UpdateDate = p.UpdateDate,
                      UpdateUserName = p.UpdateUserName,
                      ArrivalStatus = p.ArrivalStatus,
                      ArrivalDate= p.ArrivalDate,
                      ReceivingStatus= p.ReceivingStatus,
                      ReceivingDate=p.ReceivingDate,
                      ApprovalDate = p.ApprovalDate,
                      ApproverId = p.ApproverId,
                      ApproverName = p.ApproverName,
                      ApproverRole = p.ApproverRole,
                      ApprovalStatus = p.ApprovalStatus,
                      ApprovalLastRank = SqlFunc.Subqueryable<ApprovalHis>().Where(h => h.PrimaryId == p.OrderNo && h.DataType == ApprovalDataType.Purchase.ToString()).Max(h => h.ApprovalRank)
                  })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            data.ForEach(row =>
            {
                row.StatusDesc = EnumHelper.GetDescFromEnumVal<PurchaseOrderStatus>(row.Status);
                if (!string.IsNullOrEmpty(row.FlowStatus))
                {
                    row.FlowStatusDesc = EnumHelper.GetDescFromEnumVal<PurchaseOrderFlowStatus>(row.FlowStatus);
                } 
                row.ApprovalStatusDesc = EnumHelper.GetDescFromEnumVal<ApprovalStatus>(row.ApprovalStatus);
                row.ArrivalStatusDesc = EnumHelper.GetDescFromEnumVal<PurchaseReceiveStatus>(row.ArrivalStatus);
                row.ReceivingStatusDesc = EnumHelper.GetDescFromEnumVal<PurchaseReceiveStatus>(row.ReceivingStatus);
                row.IsApproval = (isAnyApproval && row.Status == PurchaseOrderStatus.Pending.ToString()) || (curApprover?.Count > 0 && curApproverRank.Contains(row.ApprovalLastRank + 1) && (row.Status == PurchaseOrderStatus.Pending.ToString() || row.Status == PurchaseOrderStatus.Approvaling.ToString()));
            });
            var res = new TableModel<PurchaseOrderDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task<PurchaseOrderDto> GetOrderDetail(int detailId,string userId)
        { 
            var data= await Repository.ClientDb.Queryable<PurchaseOrder>() 
                  .Where(p=> p.DetialId == detailId)
                  .Select<PurchaseOrderDto>()
                  .SingleAsync();
            data.QuoteLinkFileList = await Repository.ClientDb.Queryable<BaseFiles>()
           .Where(p => data.OrderNo == p.PrimaryId && p.FileInfoType == FileInfoType.PurchaseQuoteLinkFile.ToString())
           .Select(p => new Models.Model.Baseinfo.FileInfoDto { FileId = p.FileId, FileName = p.FileName, Url = p.Url, FileInfoType = p.FileInfoType, Path = p.Path, PrimaryId = p.PrimaryId, Remark = p.Remark })
           .ToListAsync();
            data.GoodsPictureList = await Repository.ClientDb.Queryable<BaseFiles>()
            .Where(p => data.OrderNo == p.PrimaryId && p.FileInfoType == FileInfoType.PurchaseGoodsPhoto.ToString())
            .Select(p => new Models.Model.Baseinfo.FileInfoDto { FileId = p.FileId, FileName = p.FileName, Url = p.Url, FileInfoType = p.FileInfoType, Path = p.Path, PrimaryId = p.PrimaryId, Remark = p.Remark })
            .ToListAsync();
            data.PurchaseTypeDesc = await Repository.ClientDb.Queryable<SysArgsOptions>().Where(s => s.OptionId == data.PurchaseTypeId).Select(s => s.OptionName).SingleAsync();
            data.ApprovalHis = await GetApprovalHis(data.OrderNo);
            data.IsLastApproval = await IsLastApproval(userId);
            return data;
        }
  
        public async Task<bool> IsLastApproval(string userId)
        {
            var curApprover = await GetApprover(ApprovalDataType.Purchase.ToString(), userId);
            if (curApprover?.Count > 0)
            {
                return curApprover.Any(w => w.IsLastApproval);
            }
            return false;
        }

        public async Task<List<KeyValueModel>> GetNPMBuyer()
        {
            return await Repository.ClientDb.Queryable<PurchaseOrder>().Where(s=>!SqlFunc.IsNullOrEmpty(s.NPMBuyer)).Select(s => new KeyValueModel
            {
                Key = s.NPMBuyer,
                Value = s.NPMBuyerEMail
            }).Distinct().ToListAsync();
        }

        public async Task AddPurchaseOrder(PurchaseOrderDto data)
        {
            if (!string.IsNullOrEmpty(data.PONumber))
            {
                var exist = await Repository.ClientDb.Queryable<PurchaseOrder>().AnyAsync(w => w.PONumber == data.PONumber && w.GoodsModel == data.GoodsModel);
                if (exist)
                {
                    throw new BusinessException($"已存在相同的PO号{data.PONumber}和物料型号{data.GoodsModel}");
                }
            }
            if (data.GoodsClassifyId==0)
            {
                data.GoodsClassifyId = 1;
                data.GoodsClassifyName = "其他类";
            }
            var model= _mapper.Map<PurchaseOrder>(data);
            var curDate= DateTime.Now;
            var lastData = await Repository.ClientDb.Queryable<PurchaseOrder>().MaxAsync(x => x.OrderNo);
            model.OrderNo = GetPrimaryId("P", lastData);
            model.CreateDate = curDate; 
            model.ArrivalStatus= PurchaseReceiveStatus.NoArrived.ToString();
            model.ReceivingStatus = PurchaseReceiveStatus.NoArrived.ToString();
            var isApproval = false;
            var approvalArgs = await _sysArgsHelper.GetValueByKey(BusinessConst.IsPurchaseOrderApproval);
            if (approvalArgs != null)
            {
                bool.TryParse(approvalArgs.Value.ToString(), out isApproval);
            }
            if (isApproval)
            {
                ////审批信息(如果当前创建人也是审批人,则需要修改和添加相应的审批信息) 
                //var process = await GetApprovalProcess(ApprovalDataType.Purchase.ToString());
                ////var curApprover = process.Where(a => a.ApproverId == model.CreateUserId).ToList();
                //var curApprover = process.Where(a => a.ApproverId == "999999999").ToList();
                //if (curApprover?.Count > 0)
                //{
                //    var hightApprover = curApprover.OrderByDescending(x => x.Rank).First();
                //    model.ApprovalDate = curDate;
                //    model.ApproverId = data.CreateUserId;
                //    model.ApproverName = data.CreateUserName;
                //    model.ApproverRole = hightApprover.ApproverRole;
                //    if (hightApprover.IsLastApproval)
                //    {
                //        model.ApprovalStatus = ApprovalStatus.Approve.ToString();
                //        model.Status = PurchaseOrderStatus.WaitReceiving.ToString();
                //        model.FlowStatus = PurchaseOrderFlowStatus.WaitingReceiving.ToString();
                //    }
                //    else
                //    {
                //        model.Status = PurchaseOrderStatus.Approvaling.ToString();
                //        model.ApprovalStatus = ApprovalStatus.Approvaling.ToString();
                //    }
                //    if (hightApprover.ApprovalModel == ApprovalModel.Process.ToString())
                //    {
                //        var newApprovalHisArr = new List<ApprovalHis>();
                //        foreach (var item in curApprover.OrderBy(x => x.Rank))
                //        {
                //            var newApprovalHis = new ApprovalHis
                //            {
                //                ApprovalDate = DateTime.Now,
                //                ApprovalModel = hightApprover.ApprovalModel,
                //                ApprovalStatus = ApprovalStatus.Approve.ToString(),
                //                ApproverId = model.ApproverId,
                //                ApproverName = model.ApproverName,
                //                ApproverRoleId = item.ApproverRole,
                //                ApproverRoleName = item.ApproverRoleName,
                //                DataType = ApprovalDataType.Purchase.ToString(),
                //                Opinion = "自动处理",
                //                ApprovalRank = item.Rank,
                //                PrimaryId = model.OrderNo
                //            };
                //            newApprovalHisArr.Add(newApprovalHis);
                //        }
                //        Repository.ClientDb.Insertable(newApprovalHisArr).AddQueue();
                //    }
                //    else
                //    {
                //        var approvalHis = new ApprovalHis
                //        {
                //            ApprovalDate = curDate,
                //            ApprovalModel = hightApprover.ApprovalModel,
                //            ApprovalStatus = ApprovalStatus.Approve.ToString(),
                //            ApproverId = data.CreateUserId,
                //            ApproverName = data.CreateUserName,
                //            ApproverRoleId = hightApprover.ApproverRole,
                //            ApproverRoleName = hightApprover.ApproverRoleName,
                //            DataType = ApprovalDataType.Purchase.ToString(),
                //            Opinion = "自动处理",
                //            ApprovalRank = hightApprover.Rank,
                //            PrimaryId = model.OrderNo
                //        };
                //        Repository.ClientDb.Insertable(approvalHis).AddQueue();
                //    } 
                //}
                //else
                //{
                    model.Status = PurchaseOrderStatus.Pending.ToString();
                    model.ApprovalStatus = ApprovalStatus.Pending.ToString();
                //}
            }
            else
            {
                model.Status = PurchaseOrderStatus.WaitReceiving.ToString();
                model.FlowStatus= PurchaseOrderFlowStatus.WaitingReceiving.ToString();
                model.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
            }
            if (model.Status == PurchaseOrderStatus.WaitReceiving.ToString())
            {
                //审批通过后，将不在系统中的物料创建到系统中
                await _addGoodsInfo(new List<PurchaseOrder> { model });
            }
            Repository.ClientDb.Insertable(model).AddQueue();
            if (data.QuoteLinkFileList?.Count > 0)
            {
                var photos = new List<BaseFiles>();
                var isSetDeft = false;
                data.QuoteLinkFileList.ForEach(p =>
                {
                    photos.Add(new BaseFiles { PrimaryId = model.OrderNo, FileName = p.FileName, FileInfoType = FileInfoType.PurchaseQuoteLinkFile.ToString(), Url = p.Url, IsDeft = !isSetDeft ? true : false });
                    isSetDeft = true;
                });
                Repository.ClientDb.Insertable(photos).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync();
            //给下一个审批节点发送邮件
            _ = Task.Run(async () =>
            {
                var nextApprover = await GetNextApprover(model.OrderNo, ApprovalDataType.Purchase);
                if (nextApprover?.Count > 0)
                {
                    var recever = nextApprover.Where(w => !string.IsNullOrEmpty(w.ApproverEmail)).Select(w => w.ApproverEmail).ToList();
                    if (recever?.Count > 0)
                        _ = _approvalSendMail(recever.ToArray(), model);
                }
            });
        }

        public async Task UpdatePurchaseOrder(PurchaseOrderDto data)
        {
            if (!string.IsNullOrEmpty(data.PONumber))
            {
                var exist = await Repository.ClientDb.Queryable<PurchaseOrder>().AnyAsync(w => w.PONumber == data.PONumber && w.GoodsModel == data.GoodsModel);
                if (exist)
                {
                    throw new BusinessException($"已存在相同的PO号{data.PONumber}和物料型号{data.GoodsModel}");
                }
            }
            var oldOrderStatus = await Repository.ClientDb.Queryable<PurchaseOrder>().Where(u => u.OrderNo == data.OrderNo).Select(u => u.Status).SingleAsync();
            if (string.IsNullOrEmpty(oldOrderStatus))
            {
                throw new BusinessException("保存失败,当前采购订单不存在或已删除");
            }
            if (oldOrderStatus == PurchaseOrderStatus.Approvaling.ToString())
            {
                throw new BusinessException("保存失败,当前采购订单正在审批中，不允许再次修改");
            }
            if (oldOrderStatus == PurchaseOrderStatus.Received.ToString())
            {
                throw new BusinessException("保存失败,当前采购订单已确认收货，不允许再次修改");
            }
            var curDate = DateTime.Now;
            var model = _mapper.Map<PurchaseOrder>(data);  
            model.UpdateDate = curDate;
            model.ArrivalStatus = PurchaseReceiveStatus.NoArrived.ToString();
            model.ReceivingStatus = PurchaseReceiveStatus.NoArrived.ToString();
            if (oldOrderStatus == PurchaseOrderStatus.Reject.ToString())
            {
                model.FlowStatus = "";
            }

            var isApproval = false;
            var approvalArgs = await _sysArgsHelper.GetValueByKey(BusinessConst.IsPurchaseOrderApproval); 
            if (approvalArgs != null)
            {
                bool.TryParse(approvalArgs.Value.ToString(), out isApproval);
            }
            if (isApproval)
            {
                //审批信息(如果当前创建人也是审批人,则需要修改和添加相应的审批信息) 
                Repository.ClientDb.Deleteable<ApprovalHis>(d => d.PrimaryId == model.OrderNo && d.DataType == ApprovalDataType.Purchase.ToString()).AddQueue();
                var process = await GetApprovalProcess(ApprovalDataType.Purchase.ToString());
                var curApprover = process.Where(a => a.ApproverId == data.UpdateUserId).ToList();
                if (curApprover?.Count > 0)
                {
                    var hightApprover = curApprover.OrderByDescending(x => x.Rank).First();
                    model.ApprovalDate = curDate;
                    model.ApproverId = data.UpdateUserId;
                    model.ApproverName = data.UpdateUserName;
                    model.ApproverRole = hightApprover.ApproverRole;
                    if (hightApprover.IsLastApproval)
                    {
                        model.ApprovalStatus = ApprovalStatus.Approve.ToString();
                        model.Status = PurchaseOrderStatus.WaitReceiving.ToString();
                        model.FlowStatus = PurchaseOrderFlowStatus.WaitingReceiving.ToString();
                    }
                    else
                    {
                        model.Status = PurchaseOrderStatus.Approvaling.ToString();
                        model.ApprovalStatus = ApprovalStatus.Approvaling.ToString();
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
                                ApproverId = model.ApproverId,
                                ApproverName = model.ApproverName,
                                ApproverRoleId = item.ApproverRole,
                                ApproverRoleName = item.ApproverRoleName,
                                DataType = ApprovalDataType.Purchase.ToString(),
                                Opinion = "自动处理",
                                ApprovalRank = item.Rank,
                                PrimaryId = model.OrderNo
                            };
                            newApprovalHisArr.Add(newApprovalHis);
                        }
                        Repository.ClientDb.Insertable(newApprovalHisArr).AddQueue();
                    }
                    else
                    {
                        var approvalHis = new ApprovalHis
                        {
                            ApprovalDate = curDate,
                            ApprovalModel = hightApprover.ApprovalModel,
                            ApprovalStatus = ApprovalStatus.Approve.ToString(),
                            ApproverId = data.UpdateUserId,
                            ApproverName = data.UpdateUserName,
                            ApproverRoleId = hightApprover.ApproverRole,
                            ApproverRoleName = hightApprover.ApproverRoleName,
                            DataType = ApprovalDataType.Purchase.ToString(),
                            Opinion = "自动处理",
                            ApprovalRank = hightApprover.Rank,
                            PrimaryId = model.OrderNo
                        };
                        Repository.ClientDb.Insertable(approvalHis).AddQueue();
                    }
                  
                }
                else
                {
                    model.Status = PurchaseOrderStatus.Pending.ToString();
                    model.ApprovalStatus = ApprovalStatus.Pending.ToString();
                }
            }
            else
            {
                model.Status = PurchaseOrderStatus.WaitReceiving.ToString();
                model.FlowStatus = PurchaseOrderFlowStatus.WaitingReceiving.ToString();
                model.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
            }
            if (model.Status == PurchaseOrderStatus.WaitReceiving.ToString())
            {
                //审批通过后，将不在系统中的物料创建到系统中
                await _addGoodsInfo(new List<PurchaseOrder> { model });
            }
            Repository.ClientDb.Updateable(model).AddQueue();
            if (data.QuoteLinkFileList?.Count > 0)
            {
                var photos = new List<BaseFiles>();
                var isSetDeft = false;
                data.QuoteLinkFileList.ForEach(p =>
                {
                    photos.Add(new BaseFiles { PrimaryId = model.OrderNo, FileName = p.FileName, FileInfoType = FileInfoType.PurchaseQuoteLinkFile.ToString(), Url = p.Url, IsDeft = !isSetDeft ? true : false });
                    isSetDeft = true;
                });
                Repository.ClientDb.Deleteable<BaseFiles>(x => x.PrimaryId == data.OrderNo && x.FileInfoType == FileInfoType.PurchaseQuoteLinkFile.ToString()).AddQueue();
                Repository.ClientDb.Insertable(photos).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync();
            //给下一个审批节点发送邮件
            _ = Task.Run(async () =>
            {
                var nextApprover = await GetNextApprover(model.OrderNo, ApprovalDataType.Purchase);
                if (nextApprover?.Count > 0)
                {
                    var recever = nextApprover.Where(w => !string.IsNullOrEmpty(w.ApproverEmail)).Select(w => w.ApproverEmail).ToList();
                    if (recever?.Count > 0)
                        _ = _approvalSendMail(recever.ToArray(), model);
                }
            });
        }

        public async Task UpdateApprovalPurchaseOrder(PurchaseOrderDto data)
        {
            if (!string.IsNullOrEmpty(data.PONumber))
            {
                var exist = await Repository.ClientDb.Queryable<PurchaseOrder>().AnyAsync(w => w.PONumber == data.PONumber && w.GoodsModel == data.GoodsModel);
                if (exist)
                {
                    throw new BusinessException($"已存在相同的PO号{data.PONumber}和物料型号{data.GoodsModel}");
                }
            }
            var oldOrderStatus = await Repository.ClientDb.Queryable<PurchaseOrder>().Where(u => u.OrderNo == data.OrderNo).Select(u => u.Status).SingleAsync();
            if (string.IsNullOrEmpty(oldOrderStatus))
            {
                throw new BusinessException("保存失败,当前采购订单不存在或已删除");
            } 
            if (oldOrderStatus == PurchaseOrderStatus.Received.ToString())
            {
                throw new BusinessException("保存失败,当前采购订单已确认收货，不允许再次修改");
            }
            var curDate = DateTime.Now;
            var model = _mapper.Map<PurchaseOrder>(data);
            model.UpdateDate = curDate;
            model.ArrivalStatus = PurchaseReceiveStatus.NoArrived.ToString();
            model.ReceivingStatus = PurchaseReceiveStatus.NoArrived.ToString(); 
            Repository.ClientDb.Updateable(model).AddQueue();
            if (data.QuoteLinkFileList?.Count > 0)
            {
                var photos = new List<BaseFiles>();
                var isSetDeft = false;
                data.QuoteLinkFileList.ForEach(p =>
                {
                    photos.Add(new BaseFiles { PrimaryId = model.OrderNo, FileName = p.FileName, FileInfoType = FileInfoType.PurchaseQuoteLinkFile.ToString(), Url = p.Url, IsDeft = !isSetDeft ? true : false });
                    isSetDeft = true;
                });
                Repository.ClientDb.Deleteable<BaseFiles>(x => x.PrimaryId == data.OrderNo && x.FileInfoType == FileInfoType.PurchaseQuoteLinkFile.ToString()).AddQueue();
                Repository.ClientDb.Insertable(photos).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync(); 
        }

        public async Task DelPurchaseOrder(int[] id)
        {
            var orders = await Repository.ClientDb.Queryable<PurchaseOrder>().Where(w => id.Contains(w.DetialId)).ToListAsync();
            foreach (var order in orders)
            {
                if (order.Status == PurchaseOrderStatus.Approvaling.ToString())
                {
                    throw new BusinessException("删除失败,当前采购订单正在审批中，不允许删除");
                }
                if (order.Status == PurchaseOrderStatus.Received.ToString())
                {
                    throw new BusinessException("删除失败,当前采购订单已确认收货，不允许删除");
                }
            }
             Repository.ClientDb.Deleteable(orders).AddQueue();
            var orderNoArr = orders.Select(s => s.OrderNo).ToList();
            Repository.ClientDb.Deleteable<BaseFiles>(d => orderNoArr.Contains(d.PrimaryId) && (d.FileInfoType == FileInfoType.PurchaseQuoteLinkFile.ToString() || d.FileInfoType == FileInfoType.PurchaseGoodsPhoto.ToString())).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task ApprovalPurchaseOrder( PurchaseOrderDto data)
        {
            var order = _mapper.Map<PurchaseOrder>(data);
            //查询当前审批流程信息
            var process = await GetApprover(ApprovalDataType.Purchase.ToString(), data.ApproverId);
            if (process.Count == 0)
            {
                throw new BusinessException("审批失败,当前用户没有审批权限");
            }
            var curOrderStatus = await Repository.ClientDb.Queryable<PurchaseOrder>().Where(w => w.OrderNo == data.OrderNo).Select(w => w.Status).SingleAsync();
            if (curOrderStatus != PurchaseOrderStatus.Pending.ToString() && curOrderStatus != PurchaseOrderStatus.Approvaling.ToString())
            {
                throw new BusinessException("审批失败,当前订单不在审批流程中");
            }
            if (string.IsNullOrEmpty(data.ApprovalResult))
            {
                order.UpdateDate = DateTime.Now;
                Repository.ClientDb.Updateable(order).AddQueue();
            }
            else
            { 
                var apprivalModel = process.First().ApprovalModel; 
                var approvalStatus = data.ApprovalResult;
                //流程审批模式：如果当前审批用户拥有多个审批角色，将以最高审批节点角色来审批，同时还需要将该用户的其他角色以自动审批模式记录下来
                var hightApprover = process.OrderByDescending(x => x.Rank).First();
                if (apprivalModel == ApprovalModel.Process.ToString())
                {
                    //判断上一级是否已审批
                    var approvalHis = await Repository.ClientDb.Queryable<ApprovalHis>().Where(a => a.DataType == ApprovalDataType.Purchase.ToString() && a.PrimaryId == order.OrderNo).ToListAsync();
                    var lastRank = approvalHis.Count == 0 ? 0 : approvalHis.Where(h => h.PrimaryId == order.OrderNo).Max(h => h.ApprovalRank);
                    var approverRank = process.Select(x => x.Rank).Distinct().ToList();
                    if (!approverRank.Contains(lastRank + 1))
                    {
                        throw new BusinessException($"审批失败,当前采购订单：{order.OrderNo}需要等待下级审批");
                    }
                    if (!process.Exists(x => x.IsLastApproval) && data.ApprovalResult == ApprovalStatus.Approve.ToString())
                    {
                        approvalStatus = ApprovalStatus.Approvaling.ToString();
                    }
                    if (process.Count > 0)
                    {
                        var newApprovalHisArr = new List<ApprovalHis>();
                        foreach (var item in process.OrderBy(x => x.Rank))
                        {
                            var newApprovalHis = new ApprovalHis
                            {
                                ApprovalDate = DateTime.Now,
                                ApprovalModel = apprivalModel,
                                ApprovalStatus = data.ApprovalResult,
                                ApproverId = data.ApproverId,
                                ApproverName = data.ApproverName,
                                ApproverRoleId = item.ApproverRole,
                                ApproverRoleName = item.ApproverRoleName,
                                DataType = ApprovalDataType.Purchase.ToString(),
                                Opinion =item.Rank== hightApprover.Rank? data.ApprovalOpinion:"自动处理",
                                ApprovalRank = item.Rank,
                                PrimaryId = data.OrderNo
                            };
                            newApprovalHisArr.Add(newApprovalHis);
                        }
                        Repository.ClientDb.Insertable(newApprovalHisArr).AddQueue();
                    }
                   
                }
                //任意审批模式：将当前用户最高审批角色当作审批节点记录
                else
                {
                    //添加审批历史记录
                    var newApprovalHis = new ApprovalHis
                    {
                        ApprovalDate = DateTime.Now,
                        ApprovalModel = apprivalModel,
                        ApprovalStatus = data.ApprovalResult,
                        ApproverId = data.ApproverId,
                        ApproverName = data.ApproverName,
                        ApproverRoleId = hightApprover.ApproverRole,
                        ApproverRoleName = hightApprover.ApproverRoleName,
                        DataType = ApprovalDataType.Purchase.ToString(),
                        Opinion = data.ApprovalOpinion,
                        ApprovalRank = hightApprover.Rank,
                        PrimaryId = data.OrderNo
                    };
                    Repository.ClientDb.Insertable(newApprovalHis).AddQueue();
                }
                //更新采购订单审批状态  
                order.ApprovalDate = DateTime.Now;
                order.ApproverRole = hightApprover.ApproverRole;
                order.ApprovalStatus = approvalStatus;
                order.UpdateDate= DateTime.Now;
                order.Status = approvalStatus; 
                if (order.ApprovalStatus == ApprovalStatus.Approve.ToString())
                {
                    order.Status = PurchaseOrderStatus.WaitReceiving.ToString();
                    order.FlowStatus = PurchaseOrderFlowStatus.WaitingReceiving.ToString();
                }
                Repository.ClientDb.Updateable(order).AddQueue();
               
                //审批通过后，将不在系统中的物料创建到系统中
                if(order.ApprovalStatus==ApprovalStatus.Approve.ToString())
                {
                    await _addGoodsInfo(new List<PurchaseOrder> { order });
                } 
            }
            await Repository.ClientDb.SaveQueuesAsync();

            //给下一个审批节点发送邮件
            _ = Task.Run(async () =>
            {
                var nextApprover = await GetNextApprover(order.OrderNo, ApprovalDataType.Purchase);
                if (nextApprover?.Count > 0)
                {
                    var recever = nextApprover.Where(w => !string.IsNullOrEmpty(w.ApproverEmail)).Select(w => w.ApproverEmail).ToList();
                    if (recever?.Count > 0)
                        _ = _approvalSendMail(recever.ToArray(), order);
                }
            });
        }

        private async Task _addGoodsInfo(List<PurchaseOrder> orders)
        {
            var insertGoodsArr = new List<BaseGoods>();
            var lastData = await Repository.ClientDb.Queryable<BaseGoods>().MaxAsync(x => x.GoodsId);
            foreach (var order in orders)
            {
                var exist = await Repository.ClientDb.Queryable<BaseGoods>().AnyAsync(w => w.GoodsName == order.GoodsNameZH && w.GoodsModel == order.GoodsModel);
                if (string.IsNullOrEmpty(order.GoodsId)&&!exist)
                {
                    var newGoods = new BaseGoods
                    {
                        GoodsId = GetPrimaryId("S", lastData),
                        GoodsNo = order.GoodsNo,
                        GoodsName = order.GoodsNameZH,
                        GoodsModel = order.GoodsModel,
                        GoodsClassifyId = order.GoodsClassifyId,
                        Supplier = order.SupplierName,
                        MinPackageUnitId = order.QuantityUnitId,
                        MinPackageUnitName = order.QuantityUnitName,
                        PackageUnitId = order.QuantityUnitId,
                        PackageUnitName = order.QuantityUnitName,
                        MaxPackageUnitId = order.QuantityUnitId,
                        MaxPackageUnitName = order.QuantityUnitName,
                        PackageCount = 1,
                        MaxPackageCount = 1,
                        SafetyInventoryUnitId = order.QuantityUnitId,
                        SafetyInventoryUnitName = order.QuantityUnitName,
                        SafetyInventory = order.SafetyInventory,
                        CostPrice = order.Price,
                        PriceUnitName = order.PriceUnitName,
                        CostPriceUnitName = order.QuantityUnitName,
                        IsInSAP = !string.IsNullOrEmpty(order.GoodsNo),
                        GoodsSpecificationId = order.GoodsSpecificationId,
                        MaxStock= order.MaxStock,
                        IsConstraintSpec= order.GoodsSpecificationId>0?true:false,
                        CreateDate = DateTime.Now,
                        CreateUser = order.CreateUserName,
                        IsValid = true
                    };
                    insertGoodsArr.Add(newGoods);
                    lastData = newGoods.GoodsId;
                }
            }
            if (insertGoodsArr.Count > 0)
            {
                Repository.ClientDb.Insertable(insertGoodsArr).AddQueue();
                _=Task.Run(async () =>
                {
                    await _addNewGoodsSendMail(insertGoodsArr, orders[0].GoodsClassifyGroup);
                });
            }
        }

        private async Task _approvalSendMail(string [] receiver,PurchaseOrder order)
        { 
            if (receiver?.Length > 0)
            {
                var sysHost = _configuration.GetSection("FrontConfig:Host").Value;
                var groupDesc = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(order.GoodsClassifyGroup);
                var bodyTitle = $@"<div style=""text-align: left;""> 
                          <div style=""font-weight: 600;font-size:15px;"">来自{groupDesc}采购申请需要您的审批</div>
                          </div>";
                var bodyContent = "<div style=\"padding:3px 10px;border: 1px solid #ededed;\">";
                bodyContent += $@"<div style=""padding:5px;"">
                              <div style=""width:100%;padding-top:3px;"">物料名称：{order.GoodsNameZH}</span>
                              <div style=""width:100%;padding-top:3px;"">物料型号：{order.GoodsModel}</span>
                              <div style=""width:100%;padding-top:3px;"">采购数量：{order.Quantity}{order.QuantityUnitName}</span>
                              <div style=""width:100%;padding-top:3px;"">供 应 商：{order.SupplierName}</span> 
                            </div>";
                bodyContent += "</ div >";
                var bodyFooter = $"<div><a href='{sysHost}/#/purchase/purchase-order' target='_bank'>点击进入系统</a></div>";
                var content = $"<div>{bodyTitle}{bodyContent}{bodyFooter}</div>";
                await _emailService.SendEmail($"{groupDesc}采购申请", content, receiver, null);
            }
        }

        private async Task _addNewGoodsSendMail(List<BaseGoods> data,string goodsGroup)
        {
            var receiver = Repository.ClientDb.Queryable<InvWarehouse>().Where(w => w.WarehouseType == goodsGroup && !SqlFunc.IsNullOrEmpty(w.ChargePersonPhone)).Select(s => s.ChargePersonPhone).ToList();
            if(receiver?.Count > 0)
            {
                var groupDesc = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(goodsGroup);
                var bodyTitle = $@"<div style=""text-align: left;""> 
                          <div style=""font-weight: 600;font-size:15px;"">来自采购申请的新增{groupDesc}需要补充必须信息，如：每标准包装数量、存储规格、保修期、单价等</div>
                          </div>";
                var bodyContent = "<div style=\"padding:3px 10px;border: 1px solid #ededed;\">";
                foreach (var item in data)
                {
                    bodyContent += $@"<div style=""padding:5px;"">
                              <div style=""width:100%;padding-top:3px;"">物料名称：{item.GoodsName}</span>
                              <div style=""width:100%;padding-top:3px;"">物料型号：{item.GoodsModel}</span>
                              <div style=""width:100%;padding-top:3px;"">内部编码：{item.GoodsId}</span> 
                            </div>";
                }
                bodyContent += "</ div >";
                var content = $"<div>{bodyTitle}{bodyContent}</div>";
                await _emailService.SendEmail($"新增{groupDesc}信息提醒", content, receiver.ToArray(), null);
            } 
        }

        public async Task UpdateFlowStatus(int detailsId,string status)
        {
            await Repository.ClientDb.Updateable<PurchaseOrder>().SetColumns(s => s.FlowStatus == status).Where(s=>s.DetialId== detailsId).ExecuteCommandAsync();
        }

        public async Task SubmitReceivedWH(List<PurchaseReceiveDto> data)
        {
            var detailsId = data.Select(s => s.DetialId).ToList();
            var orders = await Repository.ClientDb.Queryable<PurchaseOrder>().Where(w => detailsId.Contains(w.DetialId)).ToListAsync();
            foreach(var order in orders)
            {
                if (string.IsNullOrEmpty(order.FlowStatus))
                {
                    throw new BusinessException($"提交失败,订单号{order.OrderNo}状态未知，不能提交实物收货操作");
                }
                if(order.Status!= PurchaseOrderStatus.WaitReceiving.ToString())
                {
                    var stsDesc = EnumHelper.GetDescFromEnumVal<PurchaseOrderStatus>(order.Status);
                    throw new BusinessException($"提交失败,订单号{order.OrderNo}处于{stsDesc}状态，不能提交实物收货操作");
                }
                if (order.FlowStatus != PurchaseOrderFlowStatus.WaitingReceiving.ToString())
                {
                    var flowStsDesc = EnumHelper.GetDescFromEnumVal<PurchaseOrderFlowStatus>(order.FlowStatus);
                    throw new BusinessException($"提交失败,订单号{order.OrderNo}处于{flowStsDesc}状态，不能提交实物收货操作");
                }
            } 
            var updateOrders = new List<PurchaseOrder>();
            var receivedOrders= new List<PurchaseOrder>();
            foreach (var curData in data)
            {
                var curOrder = orders.SingleOrDefault(s => s.DetialId == curData.DetialId);
                if(curOrder != null)
                {
                    
                    curOrder.ArrivalStatus=curData.ArrivalStatus;
                    curOrder.ArrivalDate=curData.ArrivalDate;
                    curOrder.QuantityArrival=curData.QuantityArrival;
                    curOrder.UpdateDate=DateTime.Now;
                    curOrder.UpdateUserId=curData.UpdateUserId;
                    curOrder.UpdateUserName=curData.UpdateUserName;
                    curOrder.GoodsSpecificationId = curData.GoodsSpecificationId;
                    curOrder.MaxStock=curData.MaxStock;
                    if(curOrder.ArrivalStatus == PurchaseReceiveStatus.Arrived.ToString())
                    {
                        curOrder.Status = PurchaseOrderStatus.Received.ToString();
                        curOrder.FlowStatus = PurchaseOrderFlowStatus.InStoraged.ToString();
                        receivedOrders.Add(curOrder);
                    }
                    else if (curOrder.ArrivalStatus == PurchaseReceiveStatus.Abnormal.ToString())
                    {
                        curOrder.ArrivalAbnormalReason=curData.ArrivalAbnormalReason;
                    }
                    else
                    {
                        curOrder.ArrivalAbnormalReason = "";
                    }
                    updateOrders.Add(curOrder); 
                    if (curData.GoodsPictureList?.Count > 0)
                    {
                        //更新物料照片
                        var curGoods = await Repository.ClientDb.Queryable<BaseGoods>().SingleAsync(s => s.GoodsName == curOrder.GoodsNameZH && s.GoodsModel == curOrder.GoodsModel);
                        if(curGoods != null )
                        {
                            var goodsPhotos = new List<BaseFiles>();
                            var isSetDeft = false;
                            curData.GoodsPictureList.ForEach(p =>
                            {
                                goodsPhotos.Add(new BaseFiles { PrimaryId = curGoods.GoodsId, FileName = p.FileName, FileInfoType = FileInfoType.GoodsPhoto.ToString(), Url = p.Url, IsDeft = !isSetDeft ? true : false });
                                isSetDeft = true;
                            });
                            Repository.ClientDb.Deleteable<BaseFiles>(x => x.PrimaryId == curGoods.GoodsId && x.FileInfoType == FileInfoType.GoodsPhoto.ToString()).AddQueue();
                            Repository.ClientDb.Insertable(goodsPhotos).AddQueue();
                            if (curOrder.GoodsSpecificationId > 0)
                            {
                                curGoods.GoodsSpecificationId = curOrder.GoodsSpecificationId;
                                curGoods.MaxStock=curOrder.MaxStock;
                                curGoods.IsConstraintSpec = true;
                                Repository.ClientDb.Updateable(curGoods).AddQueue();
                            }

                        }
                        //更新采购订单物品照片
                        var purchasePhotos= new List<BaseFiles>();
                        var isDeft = false;
                        curData.GoodsPictureList.ForEach(p =>
                        {
                            purchasePhotos.Add(new BaseFiles { PrimaryId = curOrder.OrderNo, FileName = p.FileName, FileInfoType = FileInfoType.PurchaseGoodsPhoto.ToString(), Url = p.Url, IsDeft = !isDeft ? true : false });
                            isDeft = true;
                        });
                        Repository.ClientDb.Deleteable<BaseFiles>(x => x.PrimaryId == curOrder.OrderNo && x.FileInfoType == FileInfoType.PurchaseGoodsPhoto.ToString()).AddQueue();
                        Repository.ClientDb.Insertable(purchasePhotos).AddQueue();
                    }
                } 
            } 
            //添加对应的入库单 
            await _setInStorage(receivedOrders);
            Repository.ClientDb.Updateable(updateOrders).AddQueue();
            //查找是否由安全库存预警触发的采购订单，并修改预警信息的状态为已收货
            var goodsIdArr = orders.Where(w => !string.IsNullOrEmpty(w.GoodsId)).Select(w => w.GoodsId).ToList();
            var safetyWarningInfo = await Repository.ClientDb.Queryable<InvSafetyWarningRecord>().Where(w => goodsIdArr.Contains(w.GoodsId) && w.Status == SafetyWarningRecordStatus.Purchasing.ToString()).ToListAsync();
            if (safetyWarningInfo?.Count > 0)
            {
                foreach (var item in safetyWarningInfo)
                {
                    var curPO = orders.SingleOrDefault(f => f.GoodsId == item.GoodsId);
                    item.Status = SafetyWarningRecordStatus.Received.ToString();
                    item.UpdateDate = DateTime.Now;
                    if (curPO != null)
                    {
                        item.PurchaseOrderNo = curPO.PONumber;
                        item.UpdateUserId = curPO.UpdateUserId;
                        item.UpdateUserName = curPO.UpdateUserName;
                    } 
                }
                Repository.ClientDb.Updateable(safetyWarningInfo).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task SubmitReceivedWK(List<PurchaseReceiveDto> data)
        {
            var detailsId = data.Select(s => s.DetialId).ToList();
            var orders = await Repository.ClientDb.Queryable<PurchaseOrder>().Where(w => detailsId.Contains(w.DetialId)).ToListAsync();
            foreach (var order in orders)
            { 
                if (order.ArrivalStatus != PurchaseReceiveStatus.Arrived.ToString())
                {
                    var arrivedStsDesc = EnumHelper.GetDescFromEnumVal<PurchaseReceiveStatus>(order.ArrivalStatus);
                    throw new BusinessException($"提交失败,订单号{order.OrderNo}实物收货处于{arrivedStsDesc}状态，暂不能进行WK收货操作");
                }
            } 
            var updateOrders = new List<PurchaseOrder>();
            foreach (var curData in data)
            {
                var curOrder = orders.SingleOrDefault(s => s.DetialId == curData.DetialId);
                if(curOrder != null)
                {
                    curOrder.ReceivingStatus = curData.ReceivingStatus;
                    curOrder.ReceivingDate = curData.ReceivingDate;
                    curOrder.UpdateDate = DateTime.Now;
                    curOrder.UpdateUserId = curData.UpdateUserId;
                    curOrder.UpdateUserName = curData.UpdateUserName;
                    curOrder.GRNo = curData.GRNo;
                    if (curOrder.ReceivingStatus == PurchaseReceiveStatus.Abnormal.ToString())
                    {
                        curOrder.ReceivingAbnormalReason = curData.ReceivingAbnormalReason;
                    }
                    else
                    {
                        curOrder.ReceivingAbnormalReason = "";
                    }
                    updateOrders.Add(curOrder);
                } 
            }
            await Repository.ClientDb.Updateable(updateOrders).ExecuteCommandAsync(); 
        }

        private async Task _setInStorage(List<PurchaseOrder> orders)
        {
            var curDate = DateTime.Now;
            var lastData = await Repository.ClientDb.Queryable<InvInStorage>().MaxAsync(x => x.OrderNo);
            var inStorageOrderlList = new List<InvInStorage>();
            var inStorageOrderlDetailList = new List<InvInStorageDetail>();
            foreach (var data in orders)
            {
                var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => w.GoodsName == data.GoodsNameZH && w.GoodsModel == data.GoodsModel).SingleAsync();
                if (goodsInfo != null)
                {
                    var inStorageModel = new InvInStorage
                    {
                        OrderNo = GetPrimaryId("I", lastData),
                        SourceOrderNo = data.OrderNo,
                        InStorageType = InStorageType.PurchaseIn.ToString(),
                        GoodsClassify = data.GoodsClassifyGroup,
                        CreateDate = curDate,
                        CreateUserId = data.CreateUserId,
                        CreateUserName = data.CreateUserName,
                        Remark = "来自采购订单",
                        ApprovalStatus = ApprovalStatus.NoApproval.ToString(),
                        Status = InStorageStatus.WaitInStorage.ToString()
                    };
                    inStorageOrderlList.Add(inStorageModel);
                    lastData = inStorageModel.OrderNo;
                    var inStorageDetail = new InvInStorageDetail
                    {
                        OrderNo = inStorageModel.OrderNo,
                        GoodsId = goodsInfo.GoodsId,
                        GoodsName = data.GoodsNameZH,
                        Quantity = data.QuantityActual,
                        UnitId = data.QuantityUnitId,
                        PriceUnit=data.PriceUnitName,
                        UnitPrice=data.Price,
                        TotalPrice=data.DetailTotalPrice
                    };
                    inStorageOrderlDetailList.Add(inStorageDetail);
                } 
            }
            Repository.ClientDb.Insertable(inStorageOrderlList).AddQueue();
            Repository.ClientDb.Insertable(inStorageOrderlDetailList).AddQueue();
        }

        public async Task<List<ApprovalHisModel>> GetApprovalHis(string orderNo)
        {  
            var order = await Repository.ClientDb.Queryable<PurchaseOrder>().Where(s => s.OrderNo == orderNo).Select(s => new { s.CreateDate, s.CreateUserId, s.CreateUserName, s.Remark,s.ApprovalStatus }).SingleAsync();
            var createUserRole = await Repository.ClientDb.Queryable<SysUserRoles>()
                .InnerJoin<SysRoles>((ur, r) => ur.RoleId == r.RoleId)
                .Where((ur, r) => ur.UserId == order.CreateUserId)
                .Select((ur, r) => r.RoleName)
                .FirstAsync();
            var curApprovalStatus = EnumHelper.GetDescFromEnumVal<ApprovalStatus>(order.ApprovalStatus);
            var approvalList = new List<ApprovalHisModel>
            {
                new ApprovalHisModel
                {
                    PrimaryId=orderNo,
                    ApprovalDate=order.CreateDate.ToStringExtension(),
                    ApproverName=order.CreateUserName,
                    ApproverRoleName=createUserRole,
                    ApprovalStatusDesc="提交订单",
                    Opinion=$"当前状态：{curApprovalStatus}"
                }
            }; 
            if(order.ApprovalStatus!= ApprovalStatus.NoApproval.ToString())
            {
                var process = await Repository.ClientDb.Queryable<ApprovalProcess>()
              .LeftJoin<SysRoles>((p, r) => p.ApproverRole == r.RoleId)
              .LeftJoin<SysUserRoles>((p, r, ur) => p.ApproverRole == ur.RoleId)
              .LeftJoin<SysUser>((p, r, ur, u) => u.UserId == ur.UserId && u.IsVaild)
              .Where((p, r, ur, u) => p.DataType == ApprovalDataType.Purchase.ToString())
              .OrderBy((p, r, ur, u) => p.Rank)
              .Select((p, r, ur, u) => new
              {
                  p.ApproverRole,
                  r.RoleName,
                  u.UserId,
                  u.UserName,
                  p.Rank,
                  p.IsLastApproval,
                  p.ApprovalModel
              }).ToListAsync();
                var hisQuery = await Repository.ClientDb.Queryable<ApprovalHis>().Where(w => w.DataType == ApprovalDataType.Purchase.ToString() && w.PrimaryId == orderNo).OrderBy(w => w.ApprovalRank).Select<ApprovalHisModel>().ToListAsync();
                var rankList = process.Select(s => s.Rank).Distinct().ToList();
                foreach (var rank in rankList)
                {
                    var curHis = hisQuery.Where(w => w.ApprovalRank == rank).ToList();
                    if (curHis?.Count > 0)
                    {
                        curHis.ForEach(f =>
                        {
                            approvalList.Add(new ApprovalHisModel
                            {
                                PrimaryId = orderNo,
                                ApprovalDate = f.ApprovalDate,
                                ApproverName = f.ApproverName,
                                ApproverRoleName = f.ApproverRoleName,
                                ApprovalStatus = f.ApprovalStatus,
                                ApprovalStatusDesc = EnumHelper.GetDescFromEnumVal<ApprovalStatus>(f.ApprovalStatus),
                                Opinion = f.Opinion
                            });
                        });
                    }
                    else
                    {
                        var curPro = process.Where(w => w.Rank == rank).ToList();
                        approvalList.Add(new ApprovalHisModel
                        {
                            PrimaryId = orderNo,
                            ApprovalDate = "",
                            ApproverName = string.Join(',', curPro.Select(s => s.UserName).Distinct()),
                            ApproverRoleName = string.Join(',', curPro.Select(s => s.RoleName).Distinct()),
                            ApprovalStatus = ApprovalStatus.Pending.ToString(),
                            ApprovalStatusDesc = EnumHelper.GetDescFromEnum(ApprovalStatus.Pending),
                            Opinion = ""
                        });
                    }
                }
            } 
            return approvalList;
        }

        public async void AdviceReceiving(MailModel data, string orderNo)
        {
            if (data.ToReceiver.Length == 0)
            {
                throw new BusinessException("请选择邮件收件人");
            }
            if (string.IsNullOrWhiteSpace(data.Body))
            {
                throw new BusinessException("请输入邮件内容");
            }
            await _emailService.SendEmail(data.Subject, data.Body, data.ToReceiver, data.ToCC);
            await Repository.ClientDb.Updateable<PurchaseOrder>().SetColumns(s => s.IsEmailNotification == true).Where(w => w.OrderNo == orderNo).ExecuteCommandAsync();
        }

        public List<FieldModel> GetExportFields()
        {
            return new List<FieldModel>
            {
                new FieldModel {Key="OrderNo",Value="订单号",Remark=11,Type=typeof(string)},
                new FieldModel {Key="OptionName",Value="采购方式",Remark=22,Type=typeof(string)},
                new FieldModel {Key="IsApplyMaterialNumber",Value="是否申请料号",Remark=33,Type=typeof(bool)},
                new FieldModel {Key="IsPurchaseBuy",Value="是否采买", Remark = 44, Type = typeof(bool)},
                new FieldModel {Key="NPMBuyer",Value="NPM采购", Remark = 55, Type = typeof(string)},
                new FieldModel {Key="NPMBuyerEMail",Value="NPM采购邮箱", Remark = 66, Type = typeof(string)},
                new FieldModel {Key="GoodsNameZH",Value="物料中文名称", Remark = 77, Type = typeof(string)},
                new FieldModel {Key="GoodsNameEN",Value="物料英文名称",Remark=88,Type=typeof(string)},
                new FieldModel {Key="GoodsModel",Value="物料型号",Remark=99, Type = typeof(string), IsRequired = true},
                new FieldModel {Key="SupplierName",Value="供应商", Remark = 111, Type = typeof(string)},
                new FieldModel {Key="SupplierNo",Value="供应商代码", Remark = 222, Type = typeof(string)},
                new FieldModel {Key="Manufactor",Value="生产厂家", Remark = 223, Type = typeof(string)},
                new FieldModel {Key="Quantity",Value="本次采买数量", Remark = 333, Type = typeof(float)},
                new FieldModel {Key="QuantityUnitName",Value="数量单位", Remark = 444, Type = typeof(float)},
                new FieldModel {Key="QuoteLink",Value="报价单链接", Remark = 555, Type = typeof(string)},
                new FieldModel {Key="PMType",Value="PM类型", Remark = 666, Type = typeof(string)},
                new FieldModel {Key="ApplyReason",Value="CEOS申请原因", Remark = 777, Type = typeof(string)},
                new FieldModel {Key="CPMG",Value="CPMG", Remark = 888, Type = typeof(string)},
                new FieldModel {Key="AccountNumber",Value="科目号", Remark = 999, Type = typeof(string)},
                new FieldModel {Key="Consignee",Value="需求者", Remark = 1111, Type = typeof(string)},
                new FieldModel {Key="ConsigneeEmail",Value="需求者邮箱", Remark = 2222, Type = typeof(string)},
                new FieldModel {Key="CreateDate",Value="申请日期", Remark = 3333, Type = typeof(DateTime)},
                new FieldModel {Key="Line",Value="产线名称", Remark = 4444, Type = typeof(string)},
                new FieldModel {Key="LineNo",Value="产线编码", Remark = 4445, Type = typeof(string)},
                new FieldModel {Key="ClassesNumber",Value="类别编号", Remark = 5555, Type = typeof(string)},
                new FieldModel {Key="ClassesNumberDesc",Value="类别描述", Remark = 6666, Type = typeof(string)},
                new FieldModel {Key="GoodsClassifyName",Value="物料分类", Remark = 7777, Type = typeof(string)},
                new FieldModel {Key="SafetyInventory",Value="安全库存", Remark = 8888, Type = typeof(float)},
                new FieldModel {Key="MinLotSize",Value="最小订货量", Remark = 9999, Type = typeof(float)},
                new FieldModel {Key="ReturnCycle",Value="回货周期", Remark = 11111, Type = typeof(float)},
                new FieldModel {Key="Remark",Value="备注", Remark = 22222, Type = typeof(string)},
                new FieldModel {Key="FlowStatus",Value="状态", Remark = 33333,Type=typeof(string)},
                new FieldModel {Key="GoodsNo",Value="MNA编码", Remark = 44444, Type = typeof(string)},

                new FieldModel {Key="PODate",Value="PO日期",Remark=55555, Type = typeof(DateTime)},
                new FieldModel {Key="PONumber",Value="PO号", Remark = 66666, Type = typeof(string)},
                new FieldModel {Key="PRDate",Value="PR日期", Remark = 77777, Type = typeof(DateTime)},
                new FieldModel {Key="PRNumber",Value="PR号", Remark = 88888, Type = typeof(string)},

                new FieldModel {Key="QuantityActual",Value="下单实际数量", Remark = 99999, Type = typeof(float)},
                new FieldModel {Key="Description",Value="PO描述", Remark = 111111, Type = typeof(string)},
                new FieldModel {Key="Price",Value="单价", Remark = 222222, Type = typeof(float)},
                new FieldModel {Key="DetailTotalPrice",Value="总价", Remark = 333333, Type = typeof(float)},
                new FieldModel {Key="PriceUnitName",Value="计价单位", Remark = 444444, Type = typeof(string)},

                new FieldModel {Key="ArrivalStatus",Value="实物到货状态", Remark = 555555, Type =typeof(string)},
                new FieldModel {Key="ArrivalDate",Value="实物到货时间", Remark = 666666, Type =typeof(DateTime)},
                new FieldModel {Key="ArrivalAbnormalReason",Value="实物到货异常", Remark = 777777, Type = typeof(string)},
                new FieldModel {Key="ReceivingStatus",Value="WK收货状态", Remark = 888888, Type = typeof(string)},
                new FieldModel {Key="ReceivingDate",Value="WK收货时间", Remark = 999999, Type = typeof(DateTime)},
                new FieldModel {Key="ReceivingAbnormalReason",Value="WK收货异常", Remark = 1111111, Type = typeof(string)},
                new FieldModel {Key="GRNo",Value="GR号", Remark = 1111112, Type = typeof(string)}
            };
        }

        public async Task<string> ExportPurchaseData(string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, int purchaseTypeId, string flowStatus, string goodsClassifyGroup, int goodsClassifyId, List<KeyValueModel> fields)
        {
            if (fields.Count > 0)
            {
                var dbType = _configuration.GetSection("Sqlsugar:DbType").Value;
                orderFiled = string.IsNullOrEmpty(orderFiled) ? "CreateDate" : orderFiled;
                searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
                flowStatus = string.IsNullOrEmpty(flowStatus) ? "" : flowStatus; 
                var ds = GetDateStart(dateStart).Date;
                var dn = GetDateEnd(dateEnd).Date;
                var param = new Dictionary<string, object>
                    {
                        { "@OrderNo", "%"+searchKey+"%" },
                        { "@Description", "%"+searchKey+"%" },
                        { "@ExternalOrderNo", "%"+searchKey+"%" },
                        { "@SupplierNo", "%"+searchKey+"%" },
                        { "@SupplierName", "%"+searchKey+"%" },
                        { "@CreateUserName", "%"+searchKey+"%" },
                        { "@GoodsNo", "%"+searchKey+"%" },
                        { "@GoodsNameZH", "%"+searchKey+"%" },
                        { "@GoodsNameEN", "%"+searchKey+"%" },
                        { "@GoodsModel", "%"+searchKey+"%" },  
                        { "@Consignee", "%"+searchKey+"%" },
                        { "@PONumber", "%"+searchKey+"%" },
                        { "@PRNumber", "%"+searchKey+"%" },
                        { "@GoodsClassifyGroup", goodsClassifyGroup },
                        { "@ds", ds },
                        { "@dn", dn },
                        { "@FlowStatus", flowStatus }
                    };
                StringBuilder sb = new StringBuilder();
                foreach (var field in fields)
                {
                    if (dbType == "MySql")
                    {
                        if (field.Key.ToString() == "Remark")
                            sb.Append($"p.`{field.Key}` as {field.Value},");
                        else
                            sb.Append($"`{field.Key}` as {field.Value},"); 
                    }
                    else
                    {
                        if (field.Key.ToString() == "Remark")
                            sb.Append($"p.[{field.Key}] as {field.Value},");
                        else
                            sb.Append($"[{field.Key}] as {field.Value},");
                    } 
                }
                var fieldStr = sb.ToString().TrimEnd(','); 
                string sql = @$"select {fieldStr} from PurchaseOrder p 
                        left join SysArgsOptions a on p.PurchaseTypeId=a.OptionId
                        where (p.OrderNo like @OrderNo or p.Description like @Description or p.ExternalOrderNo like @ExternalOrderNo or p.CreateUserName like @CreateUserName
                        or p.SupplierName like @SupplierName  or p.SupplierNo like @SupplierNo or p.GoodsNo like @GoodsNo or p.GoodsNameZH like @GoodsNameZH or p.GoodsNameEN like @GoodsNameEN or p.GoodsModel like @GoodsModel or p.Consignee like @Consignee or p.PONumber like @PONumber or p.PRNumber like @PRNumber)
                        and p.GoodsClassifyGroup=@GoodsClassifyGroup 
                        and DATE(CreateDate)>=@ds and DATE(CreateDate)<=@dn
                        and (case when {purchaseTypeId}>0 then p.PurchaseTypeId={purchaseTypeId} else 1=1 end)
                        and (case when {goodsClassifyId}>0 then p.GoodsClassifyId={goodsClassifyId} else 1=1 end)
                        and (case when @FlowStatus!='' then p.FlowStatus=@FlowStatus else 1=1 end)
                        order by {orderFiled} {orderType}";
                var queryData = await Repository.QueryBySqlAsync(sql, param);
                if (queryData != null&& queryData.Rows.Count>0)
                {
                   
                    foreach (DataRow row in queryData.Rows)
                    {
                        if (queryData.Columns.Contains("状态"))
                        {
                            if (row["状态"] != null && !string.IsNullOrEmpty(row["状态"].ToString()))
                            {
                                row["状态"] = EnumHelper.GetDescFromEnumVal<PurchaseOrderFlowStatus>(row["状态"].ToString());
                            } 
                        }
                        if (queryData.Columns.Contains("实物到货状态"))
                        {
                            if (row["实物到货状态"] != null && !string.IsNullOrEmpty(row["实物到货状态"].ToString()))
                            {
                                row["实物到货状态"] = EnumHelper.GetDescFromEnumVal<PurchaseReceiveStatus>(row["实物到货状态"].ToString());
                            }
                        }
                        if (queryData.Columns.Contains("WK收货状态"))
                        {
                            if (row["WK收货状态"] != null && !string.IsNullOrEmpty(row["WK收货状态"].ToString()))
                            {
                                row["WK收货状态"] = EnumHelper.GetDescFromEnumVal<PurchaseReceiveStatus>(row["WK收货状态"].ToString());
                            }
                        }
                    }   
                }
                var stream = ExcelHelper.ConvertDataTableToStream(queryData);
                var fileName = $"采购订单导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
                var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
                return fileUrl;
            }
            return await Task.FromResult("");
        }
    }
}
