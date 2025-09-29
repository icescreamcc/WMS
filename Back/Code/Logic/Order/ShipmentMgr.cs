using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using External.Common.Extension;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio.DataModel;
using Models.Model;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Order;
using Models.Model.Purchase;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using StackExchange.Redis;
using System.Data;
using System.Globalization;
using System.Text;

namespace Logic.Order
{
    public class ShipmentMgr : ApprovalHandler
    {
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public ShipmentMgr(Repository repository, IMapper mapper, IFileStorage fileStorage, IConfiguration configuration, EmailService emailService) : base(repository)
        {
            _mapper = mapper;
            _fileStorage = fileStorage;
            _configuration = configuration;
            _emailService = emailService;
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
                   .LeftJoin<OrderPlan>((p, b,o) => p.CustomerOrderNo == o.OrderNo)
                   .LeftJoin<BaseSuppliers>((p, b, o,e) => p.ReceivingResponsableUserInfo == e.SupplierId)
                  //.WhereIF(!string.IsNullOrEmpty(searchKey), (p, b) => p.OrderNo.Contains(searchKey) || p.Remark.Contains(searchKey) || p.SpecialRequest.Contains(searchKey) || p.CreateUserName.Contains(searchKey) || p.ReceivingResponsableUserInfo.Contains(searchKey))
                  .WhereIF(!string.IsNullOrEmpty(isUrgentShipment), (p, b) => p.IsUrgentShipment.Contains(isUrgentShipment))
                  //.Where((p, b) => p.CreateUserId.Contains(createUserName))
                  //.Where((p, b) => p.SendingAddress.Contains(sendingAddress))
                  .WhereIF(!string.IsNullOrEmpty(detailStatus), (p, b) => p.Status.Equals(detailStatus))
                  .Where((p, b, o) => p.GoodsClassify == goodsGroup && p.SendingDate.Date >= GetDateStart(dateStart).Date && p.SendingDate.Date <= GetDateEnd(dateEnd).Date)
                  .WhereIF(!string.IsNullOrEmpty(searchKey), (p, b) => SqlFunc.Subqueryable<SendingOrderDetail>().Where(s => s.OrderNo==p.OrderNo 
                  &&(s.GoodsNo.Contains(searchKey) || s.CustomerGoodsNo.Contains(searchKey) || s.CustomerIdentificationCode.Contains(searchKey))).Any())
                  .Select((p, b,o,e) => new SendingOrderExpandDto
                  {
                      OrderNo = p.OrderNo,
                      CustomerOrderNo=p.CustomerOrderNo,
                      ContractNo=o.ContractNo,
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
                      ReceivingResponsableUserInfo = e.SupplierName,
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

        public async Task<List<OrderPrintDto>> GetOrderPrint(string orderNo, string customerOrderNo)
        {
            var printDetail = await Repository.ClientDb.Queryable<SendingOrderDetail>()
                .LeftJoin<SendingOrder>((i, s) => i.OrderNo == s.OrderNo)
                .LeftJoin<BaseGoods>((i, s, g) => i.GoodsId == g.GoodsId)
                .LeftJoin<BaseSuppliers>((i, s, g, bs) => bs.SupplierId == s.SupplierId)//运输
                .LeftJoin<BaseSuppliers>((i, s, g, bs,kh) => s.ReceivingResponsableUserInfo == kh.SupplierId)//客户
                .LeftJoin<OrderPlan>((i, s, g, bs, kh, or) =>s.CustomerOrderNo==or.OrderNo)
                .LeftJoin<BaseUnits>((i, s, g, bs, kh, or, u) => or.Unit == u.UnitId.ToString())
                .Where((i, s, g, bs, kh, or, u) => i.OrderNo == orderNo)
                .Select((i, s, g, bs, kh, or, u) => new OrderPrintDto
                {
                    OrderNo = i.OrderNo,
                    CustomerOrderNo=s.CustomerOrderNo,
                    SendingDate=s.SendingDate.ToString(), /// 发货日期
                    SendingAddress = s.SendingAddress,/// 目的地
                    GoodsName = g.GoodsName,/// 物品名称
                    Quantity = i.Quantity.ToString(),/// 计划发货数量
                    Unit = u.UnitName,/// 单位
                    Remarks = s.Remark,/// 备注
                    ContractNo = or.ContractNo, /// 合同号
                    Status = s.Status,/// 状态
                    Customer = kh.SupplierName,/// 客户信息
                    CustomerName = kh.Consignee,/// 客户联系人
                    CustomerTelephone = kh.ConsigneeTel,/// 客户联系电话
                    //OurCompany = s.SendingAddress, /// 我方公司信息
                    //OurCompanyName = s.SendingAddress,/// 我方联系人信息
                    //OurCompanyTelephone = s.SendingAddress,/// 我方联系电话
                    Supplier = bs.SupplierName,/// 运输公司
                    SupplierNo = bs.ConsigneeTel,/// 货船编号
                    SupplierName = bs.Consignee,/// 船长姓名
                })
                .ToListAsync();

            var dataCompany =  Repository.ClientDb.Queryable<SysCompany>().ToList();
            for (int i = 0; i < printDetail.Count; i++)
            {
                if (printDetail[i].Status == "WaitingShipment")
                {
                    printDetail[i].Status = "待发货";
                }
                else if (printDetail[i].Status == "Shipment")
                {
                    printDetail[i].Status = "已发货";
                }
                else if (printDetail[i].Status == "CancelShipment")
                {
                    printDetail[i].Status = "取消发货";
                }
                else if (printDetail[i].Status == "WaitingNotification")
                {
                    printDetail[i].Status = "等通知发货";
                }
                printDetail[i].OurCompany = dataCompany[0].CompanyName;
                printDetail[i].OurCompanyName = dataCompany[0].User;
                printDetail[i].OurCompanyTelephone = dataCompany[0].Phone;

            }

            return printDetail;
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
            //if (data.Details.Count == 0)
            //{
            //    throw new BusinessException("保存失败,请添加发货计划明细");
            //}
            //var sameGoods = data.Details.GroupBy(d => new { d.GoodsNo, d.QuantityUnitName }).Count();
            //if (sameGoods != data.Details.Count)
            //{
            //    throw new BusinessException("保存失败,同一天发货日期中若存在相同物料编号，建议将其汇总后再添加到系统");
            //}
            var curDate = DateTime.Now;
            var lastData = await Repository.ClientDb.Queryable<SendingOrder>().MaxAsync(x => x.OrderNo);

            //根据订单号查询订单信息
            string orderNo = data.CustomerOrderNo;
            var orderPlanData = Repository.ClientDb.Queryable<OrderPlan>().Where(m => m.OrderNo== orderNo).ToList();
            if (orderPlanData.Count>0)
            {
                DateTime deliveryDate = DateTime.ParseExact(orderPlanData[0].DeliveryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var supplierData = Repository.ClientDb.Queryable<BaseSuppliers>().Where(m => m.SupplierId == orderPlanData[0].CustomerName).ToList();
                var goodsData = Repository.ClientDb.Queryable<BaseGoods>().Where(m => m.GoodsId == orderPlanData[0].GoodsName).ToList();
                if (supplierData.Count>0&&goodsData.Count>0)
                {
                    
                    float quantity = (float)Convert.ToSingle(data.PlanQuantity.ToString());
                    var SendingOrderModel = _mapper.Map<SendingOrder>(data);
                    SendingOrderModel.OrderNo = GetPrimaryId("S", lastData);
                    SendingOrderModel.YearAndMonth = curDate.ToStringYYMMExtension();
                    SendingOrderModel.CreateDate = curDate;
                    SendingOrderModel.RequestDate = deliveryDate;//要求到货日期
                    SendingOrderModel.SendingAddress = supplierData[0].Address;//到货地址
                    SendingOrderModel.ReceivingResponsableUserInfo = orderPlanData[0].CustomerName;
                    Repository.ClientDb.Insertable(SendingOrderModel).AddQueue();
                    
                    var SendingOrderDetailList = new List<SendingOrderDetail>();
                    var model = new SendingOrderDetail();
                    model.OrderNo= SendingOrderModel.OrderNo;
                    model.DetailStatus= SendingOrderModel.Status;
                    model.GoodsId = orderPlanData[0].GoodsName;
                    model.GoodsNo = goodsData[0].GoodsNo;
                    model.Quantity = quantity;
                    model.QuantityUnitId = Int32.Parse(orderPlanData[0].Unit);
                    //var SendingOrderDetailList = _mapper.Map<List<SendingOrderDetail>>(data.Details);

                    //SendingOrderDetailList.ForEach(f =>
                    //{
                    //    f.OrderNo = SendingOrderModel.OrderNo;
                    //    f.DetailStatus = SendingOrderModel.Status;
                    //});

                    //await Repository.AddAsync(model);

                    SendingOrderDetailList.Add(model);
                    Repository.ClientDb.Insertable(SendingOrderDetailList).AddQueue();
                    await Repository.ClientDb.SaveQueuesAsync();

                }
            }
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

            float quantity = (float)Convert.ToSingle(data.PlanQuantity.ToString());
            var SendingOrderDetailList = _mapper.Map<List<SendingOrderDetail>>(data.Details);
            SendingOrderDetailList.ForEach(f =>
            {
                f.OrderNo = SendingOrderModel.OrderNo;
                f.DetailStatus = SendingOrderModel.Status;
                f.Quantity = quantity;
            });
            Repository.ClientDb.Deleteable<SendingOrderDetail>(d => d.OrderNo == data.OrderNo).AddQueue();
            Repository.ClientDb.Insertable(SendingOrderDetailList).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task DelSending(string[] orderNos)
        {
            var details = await Repository.ClientDb.Queryable<SendingOrderDetail>().Where(w => orderNos.Contains(w.OrderNo)).ToListAsync();
            Repository.ClientDb.Deleteable(details).AddQueue();
            Repository.ClientDb.Deleteable<SendingOrder>(b => orderNos.Contains(b.OrderNo)).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task<List<KeyValueModel>> GetOrderPlan()
        {
            var res = await Repository.ClientDb.Queryable<OrderPlan>()
                //.Where(c => c.IsValid)
                .Select(c => new KeyValueModel { Key = c.OrderNo, Value = c.OrderNo, Remark = c.ContractNo })
                .ToListAsync();
            return res.OrderBy(x => x.Remark).ToList();
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
