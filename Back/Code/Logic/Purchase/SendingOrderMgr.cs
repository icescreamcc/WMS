using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using External.Common.Extension;
using Logic.Inventory;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Microsoft.Extensions.Configuration;
using Minio.DataModel;
using Models.Model;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Purchase;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using StackExchange.Redis;
using System.Data;
using System.Text;

namespace Logic.Purchase
{
    public class SendingOrderMgr : ApprovalHandler
    {
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly StorageMgr _storageMgr;
        private readonly SysArgsService _sysArgsHelper;

        public SendingOrderMgr(Repository repository, IMapper mapper, IFileStorage fileStorage, IConfiguration configuration, EmailService emailService, StorageMgr storageMgr, SysArgsService sysArgsHelper) : base(repository)
        {
            _mapper = mapper;
            _fileStorage = fileStorage;
            _configuration = configuration;
            _emailService = emailService;
            _fileStorage = fileStorage;
            _sysArgsHelper = sysArgsHelper;
            _storageMgr = storageMgr;
        }

        public async Task<TableModel<SendingOrderExpandDto>> GetSending(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, string goodsGroup, string isUrgentShipment, string sendingAddress, string detailStatus)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "OrderNo" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            isUrgentShipment = string.IsNullOrEmpty(isUrgentShipment) ? "" : isUrgentShipment.Trim();
           // createUserName = string.IsNullOrEmpty(createUserName) ? "" : createUserName.Trim();
            detailStatus = string.IsNullOrEmpty(detailStatus) ? "" : detailStatus.Trim();
            sendingAddress = string.IsNullOrEmpty(sendingAddress) ? "" : sendingAddress.Trim();
            var data = Repository.ClientDb.Queryable<SendingOrder>()
                  //.InnerJoin<SendingOrderDetail>((p, d) => p.OrderNo == d.OrderNo)
                  .LeftJoin<BaseSuppliers>((p, b) => p.SupplierId == b.SupplierId)
                  //.WhereIF(!string.IsNullOrEmpty(searchKey), (p, b) => p.OrderNo.Contains(searchKey) || p.Remark.Contains(searchKey) || p.SpecialRequest.Contains(searchKey) || p.CreateUserName.Contains(searchKey) || p.ReceivingResponsableUserInfo.Contains(searchKey))
                  .WhereIF(!string.IsNullOrEmpty(isUrgentShipment), (p, b) => p.IsUrgentShipment.Contains(isUrgentShipment))
                  //.Where((p, b) => p.CreateUserId.Contains(createUserName))
                  .Where((p, b) => p.SendingAddress.Contains(sendingAddress))
                  .WhereIF(!string.IsNullOrEmpty(detailStatus), (p, b) => p.Status.Equals(detailStatus))
                  .Where((p, b) => p.GoodsClassify == goodsGroup && p.SendingDate.Date >= GetDateStart(dateStart).Date && p.SendingDate.Date <= GetDateEnd(dateEnd).Date)
                  .WhereIF(!string.IsNullOrEmpty(searchKey), (p, b) => SqlFunc.Subqueryable<SendingOrderDetail>().Where(s => s.OrderNo==p.OrderNo 
                  &&(s.GoodsNo.Contains(searchKey) || s.CustomerGoodsNo.Contains(searchKey) || s.CustomerIdentificationCode.Contains(searchKey))).Any())
                  .Select((p, b) => new SendingOrderExpandDto
                  {
                      OrderNo = p.OrderNo,
                      SendingDate = p.SendingDate,
                      RequestDate = p.RequestDate,
                      Status = p.Status,
                      IsUrgentShipment = p.IsUrgentShipment,
                      IsSufficientStock = p.IsSufficientStock,
                      YearAndMonth = p.YearAndMonth,
                      SpecialRequest = p.SpecialRequest,
                      Remark = p.Remark,
                      SendingResponsableUserId = p.SendingResponsableUserId,
                      SendingResponsableUserName = p.SendingResponsableUserName,
                      SendingResponsableUserEmail = p.SendingResponsableUserEmail,
                      ReceivingResponsableUserInfo = p.ReceivingResponsableUserInfo,
                      SendingAddress = p.SendingAddress,
                      GoodsClassify = p.GoodsClassify,
                      CreateDate = p.CreateDate,
                      CreateUserId = p.CreateUserId,
                      CreateUserName = p.CreateUserName,
                      UpdateUserId = p.UpdateUserId,
                      UpdateUserName = p.UpdateUserName,
                      UpdateDate = p.UpdateDate,

                      SupplierId = p.SupplierId,
                      SupplierName = b.SupplierName,
                      IsEmailNotification=p.IsEmailNotification,
                      IsInBaseFiles = SqlFunc.Subqueryable<BaseFiles>().Where(bf => bf.PrimaryId == p.OrderNo && bf.FileInfoType == "SendingOrderAttachment").Any()
                  })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);

            data.ForEach(row =>
            {
                row.StatusDesc = EnumHelper.GetDescFromEnumVal<SendingOrderStatus>(row.Status);
                //row.DetailStatusDesc = EnumHelper.GetDescFromEnumVal<SendingOrderStatus>(row.DetailStatus);
                row.UrgentShipmentDesc = EnumHelper.GetDescFromEnumVal<YesOrNo>(row.IsUrgentShipment);
                row.SufficientStockDesc = EnumHelper.GetDescFromEnumVal<YesOrNo>(row.IsSufficientStock);
                row.GoodsClassifyName = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(row.GoodsClassify);
                row.SendingSupplierUserEmail = string.Join(",", Repository.ClientDb.Queryable<BaseSupplierContact>().Where(bs => bs.SupplierId == row.SupplierId).Select(bs => bs.Email).ToList());
            });
            var res = new TableModel<SendingOrderExpandDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task<List<SendingOrderExpandDto>> GetOrderDetail(string userId, string orderNo)
        {
            var orderDetail = await Repository.ClientDb.Queryable<SendingOrderDetail>()
                .LeftJoin<SendingOrder>((i, s) => i.OrderNo == s.OrderNo)
                .LeftJoin<BaseGoods>((i, s, g) => i.GoodsId == g.GoodsId)
                .LeftJoin<BaseSuppliers>((i, s, g ,bs) => bs.SupplierId == s.SupplierId)
                 .Where((i) => i.OrderNo == orderNo)
                 .Select<SendingOrderExpandDto>().ToListAsync();
            return orderDetail;
        }

        public async Task<List<KeyValueModel>> GetCreateUserNameGroup()
        {
            var data = await Repository.ClientDb.Queryable<SendingOrder>()
                .LeftJoin<SysUser>((m, d) => m.CreateUserId == d.UserId)
                .Where((m, d) => d != null && d.IsVaild == true)
                .GroupBy((m, d) => d.UserId)
                .Select((m, d) => new KeyValueModel { Key = d.UserId, Value = d.UserName, Remark = d.UserCode })
                .ToListAsync();
            return data;
        }

        public async Task<List<KeyValueModel>> GetSendingAddressGroup()
        {
            var data = await Repository.ClientDb.Queryable<SendingOrder>()
                //.Where(m => !string.IsNullOrEmpty(m.SendingAddress))
                //.GroupBy(m => m.SendingAddress)
                .Select(m => new KeyValueModel { Key = m.SendingAddress, Value = m.SendingAddress })
                .ToListAsync();

            return data;
        }
        public async Task AddSending(SendingOrderDto data)
        {
            if (data.Details.Count == 0)
            {
                throw new BusinessException("保存失败,请添加发货计划明细");
            }
            var sameGoods = data.Details.GroupBy(d => new { d.GoodsNo, d.QuantityUnitName }).Count();
            if (sameGoods != data.Details.Count)
            {
                throw new BusinessException("保存失败,同一天发货日期中若存在相同物料编号，建议将其汇总后再添加到系统");
            }
            var curDate = DateTime.Now;
            var lastData = await Repository.ClientDb.Queryable<SendingOrder>().MaxAsync(x => x.OrderNo);
            var SendingOrderModel = _mapper.Map<SendingOrder>(data);
            SendingOrderModel.OrderNo = GetPrimaryId("S", lastData);
            SendingOrderModel.YearAndMonth = curDate.ToStringYYMMExtension();
            SendingOrderModel.CreateDate = curDate;
            Repository.ClientDb.Insertable(SendingOrderModel).AddQueue();

            var SendingOrderDetailList = _mapper.Map<List<SendingOrderDetail>>(data.Details);
            
            SendingOrderDetailList.ForEach(f =>
            {
                f.OrderNo = SendingOrderModel.OrderNo;
                f.DetailStatus = SendingOrderModel.Status;
            });
            Repository.ClientDb.Insertable(SendingOrderDetailList).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task UpdateSending(SendingOrderDto data)
        {
            if (data.Details.Count == 0)
            {
                throw new BusinessException("保存失败,请添加发货计划明细");
            }
            var oldSending = await Repository.ClientDb.Queryable<SendingOrder>().SingleAsync(u => u.OrderNo == data.OrderNo);
            if (oldSending == null)
            {
                throw new BusinessException("保存失败,当前发货计划不存在或已删除");
            }
            var curDate = DateTime.Now;
            var SendingOrderModel = _mapper.Map<SendingOrder>(data);
            SendingOrderModel.YearAndMonth = curDate.ToStringYYMMExtension();
            SendingOrderModel.UpdateDate = curDate;
            Repository.ClientDb.Updateable(SendingOrderModel).AddQueue();

            var SendingOrderDetailList = _mapper.Map<List<SendingOrderDetail>>(data.Details);
            SendingOrderDetailList.ForEach(f =>
            {
                f.OrderNo = SendingOrderModel.OrderNo;
                f.DetailStatus = SendingOrderModel.Status;
            });
            Repository.ClientDb.Deleteable<SendingOrderDetail>(d => d.OrderNo == data.OrderNo).AddQueue();
            Repository.ClientDb.Insertable(SendingOrderDetailList).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }
        /// <summary>
        /// 检查库存
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        public async Task<bool> CheckStorage(List<InvOutStorageDetail> details)
        {
            var goodsId = details.Select(x => x.GoodsId).Distinct().ToList();
            var storageData = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().Where(s => goodsId.Contains(s.GoodsId)).ToListAsync();
            foreach (var detail in details)
            {
                if (!storageData.Exists(s => s.GoodsId == detail.GoodsId && detail.WarehouseId == s.WarehouseId && detail.Quantity <= s.Stock && detail.UnitId == s.UnitId && detail.BinId == s.BinId && detail.WorkbinCellId == s.WorkbinCellId))
                {
                    throw new BusinessException($"{detail.GoodsName}库存不够,请检查出库数量、单位及货位是否选择正确");
                }
            }
            return true;
        }
        /// <summary>
        /// 扫描二维码 如计划发货和实际不同 修改发货数量，增加出库单 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task ConfirmSendingAndOutStorage(SendingOrderDto data)
        {
            if (data == null || string.IsNullOrEmpty(data.OrderNo))
            {
                throw new BusinessException("保存失败,单号不能为空");
            }
            using (var uow = Repository.ClientDb.UseTran())
            {
                try
                {
                    if (data.Quantity <= data.ActualQuantity)
                    {
                        // 查找发货单
                        var oldSending = await Repository.ClientDb.Queryable<SendingOrder>()
                            .SingleAsync(u => u.OrderNo == data.OrderNo);

                        if (oldSending == null)
                        {
                            throw new BusinessException("保存失败,当前发货计划不存在或已删除");
                        }

                        var curDate = DateTime.Now;
                        oldSending.UpdateDate = curDate;
                        oldSending.UpdateUserId = data.UpdateUserId;
                        oldSending.UpdateUserName = data.UpdateUserName;
                        oldSending.Status = SendingOrderStatus.Shipment.ToString();//data.DetailStatus; 

                        Repository.ClientDb.Updateable(oldSending).AddQueue();

                        // 更新实际出库数量
                        if (data.Details != null && data.Details.Count > 0)
                        {
                            foreach (var item in data.Details)
                            {
                                await Repository.ClientDb.Updateable<SendingOrderDetail>()
                                    .SetColumns(s => s.Quantity == item.Quantity && s.ActualQuantity==item.ActualQuantity)
                                    .Where(s => s.OrderNo == data.OrderNo
                                             && s.GoodsId == item.GoodsId)
                                    .ExecuteCommandAsync();

                                //await Repository.ClientDb.Updateable<OrderPlan>()
                                //    .SetColumns(op => op.ShippedNum == op.ShippedNum + (item.ActualQuantity ?? 0))
                                //    .Where(op => op.OrderNo == data.OrderNo && op.GoodsId == item.GoodsId)
                                //    .ExecuteCommandAsync();
                            }
                        }

                        //保存图片
                        if (data.GoodsPicture != null && data.GoodsPicture.Count > 0)
                        {
                            var photos = new List<BaseFiles>();
                            var isSetDeft = false;
                            foreach (var p in data.GoodsPicture)
                            {
                                photos.Add(new BaseFiles
                                {
                                    PrimaryId = data.OrderNo,   // 绑定到发货单号
                                    FileName = p.FileName,
                                    FileInfoType = FileInfoType.SendingPhoto.ToString(),
                                    Url = p.Url,
                                    IsDeft = !isSetDeft
                                });
                                isSetDeft = true;
                            }

                            Repository.ClientDb.Insertable(photos).AddQueue();
                        }
                        // 2. 生成出库单
                        var details = await Repository.ClientDb.Queryable<SendingOrderDetail>()
                                       .Where(d => d.OrderNo == data.OrderNo)
                                       .ToListAsync();

                        if (details.Count == 0)
                            throw new BusinessException("发货计划明细不存在");

                        var goodsArr = details.Select(s => s.GoodsId).Distinct().ToArray();
                        var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>()
                            .Where(w => goodsArr.Contains(w.GoodsId)).ToListAsync();

                        await _storageMgr.StorageStatistics(data.CreateUserId, data.CreateUserName);


                        //计算并检查库存 
                        //出库单明细表
                        var ordersDetail = await Repository.ClientDb.Queryable<InvOutStorageDetail>().Where(u => data.OrderNo == u.OrderNo).ToListAsync();
                        var chkStorage = await CheckStorage(ordersDetail);

                        var isOutStorageApproval = bool.Parse((await _sysArgsHelper.GetValueByKey(BusinessConst.IsOutStorageApproval)).Value.ToString());
                        if (chkStorage)
                        {
                            var lastData = await Repository.ClientDb.Queryable<InvOutStorage>().MaxAsync(x => x.OrderNo);
                            var finishedWarehouse = await Repository.ClientDb.Queryable<InvWarehouse>()
                             .Where(w => w.WarehouseType == "FinishedProduct" && w.IsAbandon == false)
                             .OrderBy(w => w.WarehouseId)   //取第一个
                             .FirstAsync();
                            //出库单
                            var outStorageModel = new InvOutStorage
                            {
                                OrderNo = GetPrimaryId("O", lastData),
                                SourceOrderNo = oldSending.OrderNo,//发货单 
                                OutStorageType = "Normal",
                                GoodsClassify = oldSending.GoodsClassify,
                                CreateDate = DateTime.Now,
                                CreateUserId = oldSending.CreateUserId,
                                CreateUserName = oldSending.CreateUserName,
                                WarehouseId = finishedWarehouse.WarehouseId,
                                Remark = oldSending.Remark,
                            };

                            //审批
                            if (isOutStorageApproval)
                            {
                                //审批信息(如果当前修改人也是审批人,则需要修改和添加相应的审批信息)
                                Repository.ClientDb.Deleteable<ApprovalHis>(a => a.DataType == ApprovalDataType.OutStorage.ToString() && a.PrimaryId == data.OrderNo).AddQueue();
                                var process = await GetApprovalProcess(ApprovalDataType.OutStorage.ToString());
                                var curApprover = process.Where(a => a.ApproverId == data.CreateUserId).ToList();
                                if (curApprover?.Count > 0)
                                {
                                    var hightApprover = curApprover.OrderByDescending(x => x.Rank).First();
                                    outStorageModel.ApprovalDate = DateTime.Now;
                                    outStorageModel.ApproverId = data.CreateUserId;
                                    outStorageModel.ApproverName = data.CreateUserName;
                                    outStorageModel.ApproverRole = hightApprover.ApproverRole;
                                    if (hightApprover.IsLastApproval)
                                    {
                                        outStorageModel.ApprovalStatus = ApprovalStatus.Approve.ToString();
                                        outStorageModel.Status = OutStorageStatus.WaitOutStorage.ToString();
                                    }
                                    else
                                    {
                                        outStorageModel.Status = OutStorageStatus.Approvaling.ToString();
                                        outStorageModel.ApprovalStatus = ApprovalStatus.Approvaling.ToString();
                                    }
                                    var approvalHis = new ApprovalHis
                                    {
                                        ApprovalDate = DateTime.Now,
                                        ApprovalModel = hightApprover.ApprovalModel,
                                        ApprovalStatus = ApprovalStatus.Approve.ToString(),
                                        ApproverId = data.CreateUserId,
                                        ApproverName = data.CreateUserName,
                                        ApproverRoleId = hightApprover.ApproverRole,
                                        ApproverRoleName = hightApprover.ApproverRoleName,
                                        DataType = ApprovalDataType.OutStorage.ToString(),
                                        ApprovalRank = hightApprover.Rank,
                                        PrimaryId = outStorageModel.OrderNo
                                    };
                                    Repository.ClientDb.Insertable(approvalHis).AddQueue();
                                }
                                else
                                {
                                    outStorageModel.Status = OutStorageStatus.Pending.ToString();
                                    outStorageModel.ApprovalStatus = ApprovalStatus.Pending.ToString();
                                }
                            }
                            else
                            {
                                outStorageModel.Status = OutStorageStatus.WaitOutStorage.ToString();
                                outStorageModel.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
                            }
                            Repository.ClientDb.Insertable(outStorageModel).AddQueue();

                            //出库明细

                            // 查出所有需要的货物信息（BaseGoods）
                            var goodsIds = details.Select(d => d.GoodsId).Distinct().ToList();

                            // 查出成品仓下面的 Bin（货位）
                            var bins = await Repository.ClientDb.Queryable<InvBin>()
                                .Where(b => b.WarehouseId == finishedWarehouse.WarehouseId && b.IsAbandon == false)
                                .ToListAsync();



                            var outStorageDetail = details.Select(d =>
                            {
                                var g = goodsInfo.FirstOrDefault(x => x.GoodsId == d.GoodsId);
                                var bin = bins.FirstOrDefault(); // 这里简单取第一个，如果有多个可以加逻辑
                                return new InvOutStorageDetail
                                {
                                    OrderNo = outStorageModel.OrderNo,
                                    GoodsId = d.GoodsId,
                                    GoodsName = g?.GoodsName ?? "",      // 根据 BaseGoods 找名称
                                    WarehouseId = finishedWarehouse.WarehouseId,
                                    ShelfId = bin?.ShelfId ?? "",       //pc端没有要修改
                                    BinId = bin?.BinId ?? 0,
                                    WorkbinId = 0,
                                    WorkbinCellId = 0,
                                    Quantity = data.Quantity,
                                    ActualQuantity = data.ActualQuantity,
                                    UnitId = g.PackageUnitId,//标准单位
                                    Remark = oldSending.Remark,
                                    UnitPrice = goodsInfo.Single(s => s.GoodsId == d.GoodsId).CostPrice,
                                    TotalPrice = Math.Round(goodsInfo.Single(s => s.GoodsId == d.GoodsId).CostPrice * (d.ActualQuantity > 0 ? d.ActualQuantity : d.Quantity), 2),

                                    PriceUnit = goodsInfo.Single(s => s.GoodsId == d.GoodsId).PriceUnitName
                                };
                            }).ToList();
                            Repository.ClientDb.Insertable(outStorageDetail).AddQueue();

                            await Repository.ClientDb.SaveQueuesAsync();

                            uow.CommitTran();

                            //return outStorageModel.OrderNo;
                        }

                    }else
                    {
                        throw new BusinessException("实际发货数量不能超过计划发货数量");
                    }
                }
                catch (Exception ex)
                {
                    uow.RollbackTran();
                    throw new BusinessException($"保存失败：{ex.Message}");
                }
            }
        }



        public async Task DelSending(string[] orderNos)
        {
            var details = await Repository.ClientDb.Queryable<SendingOrderDetail>().Where(w => orderNos.Contains(w.OrderNo)).ToListAsync();
            Repository.ClientDb.Deleteable(details).AddQueue();
            Repository.ClientDb.Deleteable<SendingOrder>(b => orderNos.Contains(b.OrderNo)).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public List<FieldModel> GetExportFields()
        {
            return new List<FieldModel>
            {
                new FieldModel {Key="CreateUserName",Value="计划员", Remark = 1, Type = typeof(string),IsRequired=true,DeftVal="李四"},
                new FieldModel {Key="OrderNo",Value="订单号",Remark=4,Type=typeof(string),IsIgnoreImport=true},
                //new FieldModel {Key="CustomerGoodsNo",Value="客户料号",Remark=6,Type=typeof(string),DeftVal="0000001"},
                //new FieldModel {Key="CustomerIdentificationCode",Value="客户识别码",Remark=7,Type=typeof(string),DeftVal="1000001"},
                new FieldModel {Key="GoodsClassify",Value="物料分类",Remark=7, Type = typeof(string), IsRequired = true,DeftVal="成品"},
                new FieldModel {Key="SupplierId",Value="运输供应商",Remark=8, Type = typeof(string), IsRequired = true,DeftVal="100001"},
                new FieldModel {Key="SendingDate",Value="计划发货日期", Remark = 9, Type = typeof(DateTime),IsRequired=true,DeftVal=DateTime.Now.ToShortDateString()},
                new FieldModel {Key="RequestDate",Value="要求到货日期", Remark = 10, Type = typeof(DateTime)},
                //new FieldModel {Key="PalletsQuantity",Value="发货托数", Remark = 14, Type = typeof(float),DeftVal=0},
                new FieldModel {Key="IsUrgentShipment",Value="是否紧急发货",Remark=15,Type=typeof(string),IsRequired=false,DeftVal="否"},
                new FieldModel {Key="IsSufficientStock",Value="是否有足够库存",Remark=16,Type=typeof(string),IsRequired=false,DeftVal="否"},

                new FieldModel {Key="ReceivingResponsableUserInfo",Value="收件人信息", Remark = 17, Type = typeof(string),IsRequired=true,DeftVal="张三"},
                new FieldModel {Key="SpecialRequest",Value="特殊要求", Remark = 18, Type = typeof(string),DeftVal=""},
                
                //new FieldModel {Key="Status",Value="单据状态", Remark = 18, Type = typeof(string),IsRequired=true,DeftVal="待发货"},
                new FieldModel {Key="SendingAddress",Value="到货地址", Remark = 19, Type = typeof(string),IsRequired=true,DeftVal="长沙"},
                new FieldModel {Key="Remark",Value="备注", Remark = 20, Type = typeof(string),DeftVal=""},
                new FieldModel {Key="GoodsNo",Value="物料编号",Remark=21,Type=typeof(string),IsRequired=true,DeftVal="A000001"},
                new FieldModel {Key="Quantity",Value="计划发货数量", Remark = 22, Type = typeof(float), IsRequired = true,DeftVal=1},
            };
        }

        public async Task<string> CreateImportTemplate()
        {
            var fields = GetExportFields().Where(w => !w.IsIgnoreImport).ToList();
            var tb = new DataTable();
            var row = tb.NewRow();
            foreach (var f in fields)
            {
                var col = new DataColumn();
                col.ColumnName = f.Value;
                col.DataType = typeof(string);
                if (f.IsRequired)
                {
                    col.ColumnName += "*";
                }
                tb.Columns.Add(col);
                row[col.ColumnName] = f.DeftVal == null ? "" : f.DeftVal;
            }
            tb.Rows.Add(row);
            var stream = ExcelHelper.ConvertDataTableToStream(tb);
            string fileName = "发货计划数据导入模板.xlsx";
            return await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
        }

        public async Task ImportSendingData(string userId, string fileType, Stream stream)
        {
            DataTable dt = ExcelHelper.ConvertStreamToDataTable(stream, fileType, "");
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new BusinessException("导入文件行数为空");
            }
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName.Contains("*"))
                {
                    column.ColumnName = column.ColumnName.Replace("*", "");
                }
            }
            var fields = GetExportFields().Where(w => !w.IsIgnoreImport).ToList();
            foreach (var field in fields)
            {
                if (!dt.Columns.Contains(field.Value))
                {
                    throw new BusinessException($"表头必须包含{field.Value}");
                }
            }
            var details = new List<SendingOrderExpandDto>();
            var entityType = typeof(SendingOrderExpandDto);
            foreach (DataRow row in dt.Rows)
            {
                var model = new SendingOrderExpandDto();
                foreach (var field in fields)
                {
                    var cellValue = row[field.Value];
                    object val;
                    if (field.IsRequired && cellValue == null)
                    {
                        throw new BusinessException($"{field.Value}不能有空值");
                    }
                    if (field.Type == typeof(bool))
                    {
                        val = false;
                        if (cellValue != null)
                        {
                            if (cellValue.ToString()?.Trim() == "是" || cellValue.ToString()?.Trim().ToUpper() == "Y")
                            {
                                val = true;
                            }
                        }
                    }
                    else if (field.Type == typeof(int))
                    {
                        int temp = 0;
                        int.TryParse(cellValue.ToString()?.Trim(), out temp);
                        val = temp;
                    }
                    else if (field.Type == typeof(float))
                    {
                        float temp = 0;
                        float.TryParse(cellValue.ToString()?.Trim(), out temp);
                        val = temp;
                    }
                    else if (field.Type == typeof(DateTime))
                    {
                        DateTime date;
                        val = null;
                        if (!string.IsNullOrEmpty(cellValue.ToString()))
                        {
                            if (!DateTime.TryParse(cellValue.ToString(), out date))
                            {
                                throw new BusinessException($"{field.Value}解析出错，不是正确的日期格式");
                            }
                            val = date;
                        }
                    }
                    else
                    {
                        val = cellValue.ToString().Trim();
                    }
                    entityType.GetProperty(field.Key)?.SetValue(model, val);
                }
                details.Add(model);
            }

            //物料大类验证
            var classifyGroupEnums = EnumHelper.GetEnumValNames<BaseTypeGroup>();
            var groups = details.Select(s => s.GoodsClassify).Distinct().ToList();
            foreach (var g in groups)
            {
                if (!classifyGroupEnums.Exists(e => e.Value.ToString() == g))
                {
                    var str = string.Join(", ", classifyGroupEnums.Select(s => s.Value.ToString()).ToList());
                    throw new BusinessException($"物料分类解析出错，{g}不是系统内置枚举，请使用：{str}");
                }
            }

            //运输供应商信息
            var suppliersInfo = await Repository.ClientDb.Queryable<BaseSuppliers>().Where(p => p.SupplierTypeId == 121).ToListAsync();
            var allSupplierNoList = suppliersInfo.GroupBy(p => p.SupplierNo).Select(p => p.Key).ToList();
            var supplierNo = details.Select(s => s.SupplierId).Distinct().ToList();
            foreach (var g in supplierNo)
            {
                if (!allSupplierNoList.Exists(e => e == g))
                {
                    var str = string.Join(", ", allSupplierNoList.Select(s => s).ToList());
                    throw new BusinessException($"运输供应商解析出错，{g}不是系统内置枚举，请使用：{str}");
                }
            }

            //是否紧急发货验证
            var yesOrNoEnums = EnumHelper.GetEnumValNames<YesOrNo>();
            var urgentShipmentList = details.Select(s => s.IsUrgentShipment).Distinct().ToList();
            foreach (var urgentShipment in urgentShipmentList)
            {
                if (!yesOrNoEnums.Exists(e => e.Value.ToString() == urgentShipment))
                {
                    var str = string.Join(", ", yesOrNoEnums.Select(s => s.Value.ToString()).ToList());
                    throw new BusinessException($"是否紧急发货解析出错，{urgentShipment}不是系统内置枚举，请使用：{str}标识");
                }
            }

            //是否有足够库存验证
            var sufficientStockList = details.Select(s => s.IsSufficientStock).Distinct().ToList();
            foreach (var sufficientStock in sufficientStockList)
            {
                if (!yesOrNoEnums.Exists(e => e.Value.ToString() == sufficientStock))
                {
                    var str = string.Join(", ", yesOrNoEnums.Select(s => s.Value.ToString()).ToList());
                    throw new BusinessException($"是否有足够库存解析出错，{sufficientStock}不是系统内置枚举，请使用：{str}标识");
                }
            }

            ////单据状态验证
            //var orderStatusEnums = EnumHelper.GetEnumValNames<SendingOrderStatus>();
            //var statusList = details.Select(s => s.Status).Distinct().ToList();
            //foreach (var status in statusList)
            //{
            //    if (!orderStatusEnums.Exists(e => e.Value.ToString() == status))
            //    {
            //        var str = string.Join(", ", orderStatusEnums.Select(s => s.Value.ToString()).ToList());
            //        throw new BusinessException($"单据状态解析出错，{status}不是系统内置枚举，请使用：{str}标识");
            //    }
            //}

            var goodsNoArr = details.Select(s => s.GoodsNo).Distinct().ToList();
            var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsNoArr.Contains(w.GoodsNo)).ToListAsync();
            foreach (var grp in details)
            {
                if (grp.Quantity <= 0)
                {
                    throw new BusinessException($"计划发货数量必须大于0");
                }
                //if (d.PalletsQuantity < 0)
                //{
                //    throw new BusinessException($"发货托数必须大于等于0");
                //}
                if (grp.SendingDate.Date < DateTime.Now.Date)
                {
                    throw new BusinessException($"计划发货日期不能小于当前日期");
                }
                if (grp.RequestDate != DateTime.MinValue && grp.RequestDate.Date < DateTime.Now.Date)
                {
                    throw new BusinessException($"要求到货日期不能小于当前日期");
                }

                var sameGoods = goodsInfo.Where(s => s.GoodsNo.Equals(grp.GoodsNo)).ToList();
                if (sameGoods.Count == 0)
                {
                    throw new BusinessException($"物料编号：{grp.GoodsNo}未在系统中建立基础信息维护");
                }
            }

            var user = await Repository.GetSingeAsync<SysUser>(userId);
            var orderList = new List<SendingOrderDto>();
            var lastData = await Repository.ClientDb.Queryable<SendingOrder>().MaxAsync(x => x.OrderNo);
            var curDate = DateTime.Now;
            var orderStatusEnums = EnumHelper.GetEnumValNames<SendingOrderStatus>();

            //计划发货日期同一天、同一到货地址合并成一笔发货计划
            var sendingOrderALLList = details.GroupBy(p => new {p.SendingDate, p.SendingAddress }).Select(p => new SendingOrderExpandDto() { SendingDate = p.Key.SendingDate, SendingAddress = p.Key.SendingAddress }).ToList();
            List<SendingOrderExpandDto> sendingOrderList = new();
            foreach (var item in sendingOrderALLList)
            {
               var firstOrder=details.Where(p => p.SendingDate == item.SendingDate && p.SendingAddress == item.SendingAddress).FirstOrDefault();
                sendingOrderList.Add(firstOrder);
            }

            foreach (var grp in sendingOrderList)
            {
                grp.Status = "待发货";
                var order = new SendingOrderDto
                {
                    OrderNo = GetPrimaryId("S", lastData),
                    SendingDate = grp.SendingDate,
                    RequestDate = grp.RequestDate,
                    YearAndMonth = curDate.ToStringYYMMExtension(),
                    SendingAddress = grp.SendingAddress,
                    GoodsClassify = classifyGroupEnums.Single(s => s.Value.ToString() == grp.GoodsClassify).Key.ToString(),
                    CreateUserId = userId,
                    CreateUserName = user.UserName,
                    CreateDate = DateTime.Now,
                    Status = orderStatusEnums.Single(s => s.Value.ToString() == grp.Status).Key.ToString(),
                    IsUrgentShipment = yesOrNoEnums.Single(s => s.Value.ToString() == grp.IsUrgentShipment).Key.ToString(),
                    IsSufficientStock = yesOrNoEnums.Single(s => s.Value.ToString() == grp.IsSufficientStock).Key.ToString(),
                    ReceivingResponsableUserInfo = grp.ReceivingResponsableUserInfo,
                    SpecialRequest = grp.SpecialRequest,
                    Remark = grp.Remark,
                    SupplierId = suppliersInfo.Single(p => p.SupplierNo == grp.SupplierId).SupplierId,
                };
                lastData = order.OrderNo;

                var orderDetailList = details.Where(p => p.SendingDate == grp.SendingDate && p.SendingAddress == grp.SendingAddress).ToList();
                var orderDetail = _mapper.Map<List<SendingOrderDetailDto>>(orderDetailList);
                orderDetail.ForEach(f =>
                {
                    //计算发货托数=(发货数量/标准包装数)/最大包装数
                    if (f.PalletsQuantity == 0)
                    {
                        var curGoodsList = goodsInfo.Where(w => w.GoodsNo == f.GoodsNo).ToList();
                        var curGoodsInfo = curGoodsList.FirstOrDefault();
                        f.CustomerGoodsNo = curGoodsInfo?.CustomerGoodsNo;
                        f.CustomerIdentificationCode = curGoodsInfo?.CustomerIdentificationCode;
                        f.GoodsId = curGoodsInfo?.GoodsId;
                        if (curGoodsInfo?.MaxPackageCount > 0 && curGoodsInfo?.PackageCount > 0)
                        {
                            f.PalletsQuantity = (int)Math.Ceiling((f.Quantity / curGoodsInfo.PackageCount) / curGoodsInfo.MaxPackageCount);
                        }
                    }
                    f.OrderNo = order.OrderNo;
                    f.DetailStatus = order.Status;
                });
                order.Details = orderDetail;
                orderList.Add(order);
            }
            foreach (var order in orderList)
            {
                await AddSending(order);
            }
        }

        public async Task<string> ExportSendingData(string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, string goodsGroup, List<KeyValueModel> fields)
        {
            if (fields.Count > 0)
            {
                var dbType = _configuration.GetSection("Sqlsugar:DbType").Value;
                orderFiled = string.IsNullOrEmpty(orderFiled) ? "CreateDate" : orderFiled;
                searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
                var ds = GetDateStart(dateStart).Date;
                var dn = GetDateEnd(dateEnd).Date;
                var param = new Dictionary<string, object>
                    {
                        { "@OrderNo", "%"+searchKey+"%" },
                        { "@Remark", "%"+searchKey+"%" },
                        { "@SpecialRequest", "%"+searchKey+"%" },
                        { "@CreateUserName", "%"+searchKey+"%" },
                        { "@GoodsClassify", goodsGroup },
                        { "@GoodsNo", "%"+searchKey+"%" },
                        { "@ds", ds },
                        { "@dn", dn },
                    };
                StringBuilder sb = new StringBuilder();
                foreach (var field in fields)
                {
                    if (field.Key.ToString() == "OrderNo")
                    {
                        if (dbType == "MySql")
                        {
                            sb.Append($"o.`{field.Key}` as {field.Value},");
                        }
                        else
                        {
                            sb.Append($"o.[{field.Key}] as {field.Value},");
                        }
                    }
                    else
                    {
                        if (dbType == "MySql")
                        {
                            sb.Append($"`{field.Key}` as {field.Value},");
                        }
                        else
                        {
                            sb.Append($"[{field.Key}] as {field.Value},");
                        }
                    }
                }
                var fieldStr = sb.ToString().TrimEnd(',');
                string sql = $@" select {fieldStr} from SendingOrder o
                                join SendingOrderDetail d on o.OrderNo=d.OrderNo
                                where (o.OrderNo like @OrderNo or Remark like @Remark or SpecialRequest like @SpecialRequest or CreateUserName like @CreateUserName or GoodsNo like @GoodsNo)
                                and GoodsClassify=@GoodsClassify
                                and DATE(SendingDate)>=@ds and DATE(SendingDate)<=@dn
                                order by {orderFiled} {orderType}";
                var queryData = await Repository.QueryBySqlAsync(sql, param);
                var stream = ExcelHelper.ConvertDataTableToStream(queryData);
                var fileName = $"发货计划信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
                var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
                return fileUrl;
            }
            return await Task.FromResult("");
        }


        public async Task UploadSendingDocument(string userId, string orderNo, string fileName, Stream stream)
        {
            //保存本地磁盘,上传到MIO
            FileInfoDto fileInfoDto = new();
            var fileExtension = Path.GetExtension(fileName);
            if (fileExtension == ".xls" || fileExtension == ".xlsx")
            {
                fileInfoDto = await _fileStorage.GetFileAndSave(fileName, stream, FileType.Excel);
            }
            else if (fileExtension == ".img" || fileExtension == ".png" || fileExtension == ".gif")
            {
                fileInfoDto = await _fileStorage.GetFileAndSave(fileName, stream, FileType.Image);
            }
            else if (fileExtension == ".txt")
            {
                fileInfoDto = await _fileStorage.GetFileAndSave(fileName, stream, FileType.TXT);
            }
            else if (fileExtension == ".pdf" || fileExtension == ".PDF")
            {
                fileInfoDto = await _fileStorage.GetFileAndSave(fileName, stream, FileType.PDF);
            }
            else if (fileExtension == ".doc" || fileExtension == ".docx")
            {
                fileInfoDto = await _fileStorage.GetFileAndSave(fileName, stream, FileType.DOC);
            }

            if (!string.IsNullOrWhiteSpace(fileInfoDto.Url))
            {
                var attachmentEntity = new BaseFiles
                {
                    PrimaryId = orderNo,
                    FileName = fileName[..fileName.LastIndexOf(".")],
                    FileInfoType = FileInfoType.SendingOrderAttachment.ToString(),
                    Path = fileInfoDto.Path,
                    Url = fileInfoDto.Url,
                };
                Repository.ClientDb.Insertable(attachmentEntity).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async void AdviceSending(MailModel data, string orderNo)
        {
            if (data.ToReceiver.Length == 0)
            {
                throw new BusinessException("请选择邮件收件人");
            }
            if (string.IsNullOrWhiteSpace(data.Body))
            {
                throw new BusinessException("请输入邮件内容");
            }
            await Repository.ClientDb.Updateable<SendingOrder>().SetColumns(s => s.IsEmailNotification == true).Where(w => w.OrderNo == orderNo).ExecuteCommandAsync();
            _ = _emailService.SendEmail(data.Subject, data.Body, data.ToReceiver, data.ToCC, data.Attachments == null ? "" : string.Join(",", data.Attachments.Select(p => p.Path)));
        }

    }
}
