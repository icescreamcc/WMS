using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using External.Common.Extension;
using External.Log;
using Logic.LogicBase;
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
using System.Data;
using System.Globalization;
using System.Text;
namespace Logic.Purchase
{
    public class ReceivingOrderMgr : ApprovalHandler
    { 
        private readonly EmailService _emailService;

        private readonly MessageService _messageService;

        private readonly SysArgsService _sysArgsHelper;

        private readonly IMapper _mapper;

        private readonly IFileStorage _fileStorage;

        private readonly IConfiguration _configuration;

        private readonly HttpHelperAsync _httpHelper;

        private readonly LogHelper _logHelper;

        public ReceivingOrderMgr(Repository repository, LogHelper logHelper, EmailService emailService, MessageService messageService, SysArgsService sysArgsService, IMapper mapper, IFileStorage fileStorage, IConfiguration configuration, HttpHelperAsync httpHelper) : base(repository)
        {
            _logHelper = logHelper;
            _emailService = emailService;
            _messageService = messageService;
            _sysArgsHelper = sysArgsService;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _configuration = configuration;
            _httpHelper = httpHelper;
        }
        /// <summary>
        /// 定期将第二天ASN未创建的清单发给他们，他们可以针对这个清单马上创建ASN信息，保证现场收货时都会有ASN

        /// </summary>
        public void CreateASNWarningRecord()
        {
            //定义ASN检查且检查结果不是Y的物料
           var data = Repository.ClientDb.Queryable<ReceivingOrder>()
                .InnerJoin<ReceivingOrderDetail>((p, d) => p.OrderNo == d.OrderNo)
                .Where((p, d) => d.IsASN == true  &&  d.ASNCheckStatus != "Y" && p.Status != "ReceivedAll" &&  p.ExpectDate == DateTime.Now.AddDays(1).Date)
                .Select((p, d) => new ReceivingOrderExpandDto
                {
                    OrderNo = p.OrderNo,
                    ExternalOrderNo = d.ExternalOrderNo,
                    SupplierId = d.SupplierId,
                    SupplierName = d.SupplierName,
                    ReceivingLevel = d.ReceivingLevel,
                    TotalPrice = p.TotalPrice,
                    PriceUnitId = p.PriceUnitId,
                    PriceUnitName = p.PriceUnitName,
                    ExpectDate = p.ExpectDate,
                    IsMakeInvoice = d.IsMakeInvoice,
                    InvoiceNumber = d.InvoiceNumber,
                    IsAccountPaid = p.IsAccountPaid,
                    PaymentAmount = p.PaymentAmount,
                    DownTime = d.DownTime,
                    QuantityActual = d.QuantityActual,
                    Status = p.Status,
                    CreateDate = p.CreateDate,
                    CreateUserId = p.CreateUserId,
                    CreateUserName = p.CreateUserName,
                    Remark = p.Remark,
                    ReceivingResponsableUserId = p.ReceivingResponsableUserId,
                    ReceivingResponsableUserName = p.ReceivingResponsableUserName,
                    ReceivingResponsableUserEmail = p.ReceivingResponsableUserEmail,
                    ReceivingAddress = p.ReceivingAddress,
                    GoodsClassify = p.GoodsClassify,
                    IsEmailNotification = p.IsEmailNotification,
                    IsASN = d.IsASN,
                    ASNCheckStatus = d.ASNCheckStatus,
                    WaybillNo = d.WaybillNo,
                    DetialId = d.DetialId,
                    GoodsId = d.GoodsId,
                    GoodsNo = d.GoodsNo,
                    GoodsName = d.GoodsName,
                    GoodsClassifyName = d.GoodsClassifyName,
                    GoodsModel = d.GoodsModel,
                    Quantity = d.Quantity,
                    QuantityUrgency = d.QuantityUrgency,
                    QuantityUnitId = d.QuantityUnitId,
                    QuantityUnitName = d.QuantityUnitName,
                    Price = d.Price,
                    WorkpieceTray = d.WorkpieceTray,
                    Pallet = d.Pallet,
                    AbnormalDeliveryPallet = d.AbnormalDeliveryPallet,
                    DetailTotalPrice = d.DetailTotalPrice,
                    DetailStatus = d.DetailStatus,
                    ReceivingAbnormalType = d.ReceivingAbnormalType,
                    ReceivingAbnormalDesc = d.ReceivingAbnormalDesc,
                    ReceivingDate = d.ReceivingDate,
                    ReceivingOperatorId = d.ReceivingOperatorId,
                    ReceivingOperatorName = d.ReceivingOperatorName
                }).MergeTable().ToList();
            _logHelper.LogInfo("CreateASNWarningRecord", $"监测到ASN未创建的物料数：{data.Count}", "");
           // 有的话，发邮件通知
            if (data.Count > 0)
            {
                try
                {
                    SendMailToWarning_ASN(data).Wait();
            
                       

                   
    
                }
                catch (Exception ex)
                {
                    _logHelper.LogError("CreateASNWarningRecord", ex.Message, "", ex, ex.StackTrace);
                }
            }
        }
        public async Task SendMailToWarning_ASN(List<ReceivingOrderExpandDto> data)
        {

            var attachment = await CreateAttachment_ASN(data);
            // 1. 获取所有CreateUserId
            var CreateUserId = data.Select(x => x.CreateUserId).Distinct().ToList();
            // With this corrected line:

            // 2. 批量查出邮箱
            var userEmailDict = Repository.ClientDb.Queryable<SysUser>()
                .Where(u => CreateUserId.Contains(u.UserId))
                .ToDictionary(u => u.UserId, u => u.Email);


            // 3. 按用户名取邮箱
            if (userEmailDict.Count>0)
            {
                // email is successfully retrieved and cast to string
              
            }
            else
            {
               // email = string.Empty; // 如果没有找到邮箱，可以设置为默认值或空字符串
                _logHelper.LogInfo("SendMailToASNWarning", $"发送邮件失败，收件人信息为空", "");
            }
            //发送邮件
            var mail = new SysMail
            {
                ToReceiver = string.Join(',', userEmailDict.Values),
                //ToReceiver = email,
                Subject = "ASN未创建提醒",
                Body = "该邮件由系统自动发送，详细请查看附件",
                Sender = $"{_configuration.GetSection("MailConfig:Sender").Value}",
                AttachmentId = Guid.NewGuid().ToString("N"),
                BusinessType = BusinessType.SafetyInvenstoryWarning.ToString(),
                CreateDate = DateTime.Now,
            };
            _emailService.SendEmail(mail.Subject, mail.Body, mail.ToReceiver.Split(','), null, attachment == null ? "" : attachment.Path).Wait();
        }
        public List<KeyValueModel> GetExportFields_ASN()
        {
            return new List<KeyValueModel>
            {
                new KeyValueModel {Key="CreateUserName",Value="计划员",Remark=4},
                new KeyValueModel {Key="GoodsNo",Value="物料编号",Remark=4},
                new KeyValueModel {Key="ExternalOrderNo",Value="SAP订单号",Remark=5},
                new KeyValueModel {Key="OrderNo",Value="订单号", Remark = 7},
                new KeyValueModel {Key="ExpectDate",Value="预计到货日期",Remark=8},
                new KeyValueModel {Key="SupplierName",Value="供应商名称", Remark = 9},
                new KeyValueModel {Key="GoodsClassifyName",Value="物料分类",Remark=10},
                new KeyValueModel {Key="Quantity",Value="数量", Remark = 11},
                new KeyValueModel {Key="QuantityUnitName",Value="物料单位", Remark = 12},
            };
        }
        public async Task<FileInfoDto> CreateAttachment_ASN(List<ReceivingOrderExpandDto> data)
        {
            var fields = GetExportFields_ASN();
            var tb = new DataTable();
            var type = typeof(ReceivingOrderExpandDto);
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
                        var propertyInfo = record.GetType().GetProperty(field.Key?.ToString() ?? string.Empty);
                        if (propertyInfo != null)
                        {
                            var v = propertyInfo.GetValue(record);
                            if (field.Value != null)
                            {
                                row[field.Value.ToString()] = v?.ToString() ?? string.Empty;
                            }
                        }
                    }
                }
                tb.Rows.Add(row);
            }
            var stream = ExcelHelper.ConvertDataTableToStream(tb);
            //var goodsGroupDesc = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(data.First().GoodsClassifyGroup);
            var fileName = $"ASN未创建预警信息{DateTime.Now.ToString("yyyy-MM-dd")}.xlsx";
            return await _fileStorage.GetFileAndSave(fileName, stream, FileType.Excel);
        }
        //public async Task<TableModel<ReceivingOrderExpandDto>> GetOrders(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, string goodsGroup, string receivingLevel, string createUserName, string detailStatus)
        //{
        //    int total = 0;
        //    orderFiled = string.IsNullOrEmpty(orderFiled) ? "CreateDate" : orderFiled;
        //    searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
        //    receivingLevel = string.IsNullOrEmpty(receivingLevel) ? "" : receivingLevel.Trim();
        //    createUserName = string.IsNullOrEmpty(createUserName) ? "" : createUserName.Trim();
        //    detailStatus = string.IsNullOrEmpty(detailStatus) ? "" : detailStatus.Trim();
        //    var curApprover = await GetApprover(ApprovalDataType.ReceivingPlan.ToString(), userId);
        //    var approvalModel = curApprover.FirstOrDefault()?.ApprovalModel;
        //    var curApproverRank = curApprover.Select(x => x.Rank).Distinct().ToList();
        //    var isAnyApproval = approvalModel == ApprovalModel.Any.ToString() && curApprover?.Count > 0;
        //    string receivingPlan = ApprovalDataType.ReceivingPlan.ToString();
        //    var data = Repository.ClientDb.Queryable<ReceivingOrder>()
        //          .InnerJoin<ReceivingOrderDetail>((p,d)=>p.OrderNo==d.OrderNo)
        //          .LeftJoin<SysArgsOptions>((p, d,s) => s.ArgsKey== BusinessConst.AbnormalReceiptClassification && d.ReceivingAbnormalType == s.OptionKey)
        //          .Where((p,d) => p.OrderNo.Contains(searchKey) || p.Remark.Contains(searchKey)|| d.ExternalOrderNo.Contains(searchKey) || p.CreateUserName.Contains(searchKey) || d.SupplierName.Contains(searchKey)
        //           || d.GoodsNo.Contains(searchKey) || d.GoodsName.Contains(searchKey)||d.GoodsClassifyName.Contains(searchKey)||d.WaybillNo.Contains(searchKey)
        //           || d.ReceivingAbnormalType.Contains(searchKey))
        //          .Where((p, d) => d.ReceivingLevel.Contains(receivingLevel) )
        //          .Where((p, d) => p.CreateUserId.Contains(createUserName))
        //          .Where((p, d) => d.DetailStatus.Contains(detailStatus))
        //          .Where((p, d) => p.GoodsClassify == goodsGroup && p.ExpectDate.Date >= GetDateStart(dateStart).Date && p.ExpectDate.Date <= GetDateEnd(dateEnd).Date) 
        //          .Select((p,d,s) => new ReceivingOrderExpandDto
        //          {
        //              OrderNo = p.OrderNo,
        //              ExternalOrderNo = d.ExternalOrderNo, 
        //              SupplierId = d.SupplierId,
        //              SupplierName = d.SupplierName,
        //              ReceivingLevel = d.ReceivingLevel,
        //              TotalPrice = p.TotalPrice,
        //              PriceUnitId = p.PriceUnitId,
        //              PriceUnitName = p.PriceUnitName,
        //              ExpectDate = p.ExpectDate,
        //              IsMakeInvoice = d.IsMakeInvoice,
        //              InvoiceNumber = d.InvoiceNumber,
        //              IsAccountPaid = p.IsAccountPaid,
        //              PaymentAmount = p.PaymentAmount,
        //              DownTime=d.DownTime,
        //              QuantityActual=d.QuantityActual,
        //              Status = p.Status,
        //              CreateDate = p.CreateDate,
        //              CreateUserId = p.CreateUserId,
        //              CreateUserName = p.CreateUserName,
        //              Remark = p.Remark,
        //              ReceivingResponsableUserId = p.ReceivingResponsableUserId,
        //              ReceivingResponsableUserName = p.ReceivingResponsableUserName,
        //              ReceivingResponsableUserEmail = p.ReceivingResponsableUserEmail,
        //              ReceivingAddress = p.ReceivingAddress, 
        //              GoodsClassify=p.GoodsClassify,
        //              IsEmailNotification=p.IsEmailNotification,
        //              IsASN=d.IsASN,
        //              ASNCheckStatus=d.ASNCheckStatus,
        //              WaybillNo=d.WaybillNo,
        //              DetialId =d.DetialId,
        //              GoodsId=d.GoodsId,
        //              GoodsNo=d.GoodsNo,
        //              GoodsName = d.GoodsName,
        //              GoodsClassifyName = d.GoodsClassifyName,
        //              GoodsModel=d.GoodsModel,
        //              Quantity=d.Quantity,
        //              QuantityUrgency=d.QuantityUrgency,
        //              QuantityUnitId = d.QuantityUnitId,
        //              QuantityUnitName= d.QuantityUnitName,
        //              Price= d.Price,
        //              WorkpieceTray = d.WorkpieceTray,
        //              Pallet = d.Pallet,
        //              AbnormalDeliveryPallet=d.AbnormalDeliveryPallet,
        //              DetailTotalPrice = d.DetailTotalPrice,
        //              DetailStatus = d.DetailStatus,
        //              ReceivingAbnormalType=d.ReceivingAbnormalType,
        //              ReceivingAbnormalTypeName=s.OptionName,
        //              ReceivingAbnormalDesc = d.ReceivingAbnormalDesc,
        //              ReceivingDate = d.ReceivingDate,
        //              ReceivingOperatorId=d.ReceivingOperatorId,
        //              ReceivingOperatorName=d.ReceivingOperatorName,
        //              ApprovalLastRank = SqlFunc.Subqueryable<ApprovalHis>().Where(h => h.PrimaryId == p.OrderNo && h.DataType == receivingPlan).Max(h => h.ApprovalRank),
        //              //增加判断是有保存照片，如果有，照片按钮enable
        //              IsInBaseFiles = SqlFunc.Subqueryable<BaseFiles>().Where(bf => bf.PrimaryId == p.OrderNo && bf.FileInfoType == "ReceivingOrderAttachment").Any()
        //          })
        //          .OrderBy($"{orderFiled} {orderType}")
        //          .ToPageList(pgIndex, pgSize, ref total);

        //    string pending = ReceivingOrderStatus.Pending.ToString();
        //    string approvaling = ReceivingOrderStatus.Approvaling.ToString();
        //    data.ForEach(row =>
        //    {
        //        row.StatusDesc= EnumHelper.GetDescFromEnumVal<ReceivingOrderStatus>(row.Status);
        //        row.DetailStatusDesc = EnumHelper.GetDescFromEnumVal<ReceivingOrderDetailStatus>(row.DetailStatus);
        //        row.ReceivingLevelDesc = EnumHelper.GetDescFromEnumVal<ReceivingLevel>(row.ReceivingLevel);
        //        row.ApprovalStatusDesc = EnumHelper.GetDescFromEnumVal<ApprovalStatus>(row.ApprovalStatus);
        //        // row.IsApproval = (row.CreateDate.Date==row.ExpectDate.Date && isAnyApproval && row.Status == pending) || (curApprover?.Count > 0 && curApproverRank.Contains(row.ApprovalLastRank + 1) && (row.Status == pending || row.Status == approvaling ));
        //        row.IsApproval = (isAnyApproval && row.Status == pending) || (curApprover?.Count > 0 && curApproverRank.Contains(row.ApprovalLastRank + 1) && (row.Status == pending || row.Status == approvaling));
        //    });

        //    var dataSum = Repository.ClientDb.Queryable<ReceivingOrder>()
        //          .InnerJoin<ReceivingOrderDetail>((p, d) => p.OrderNo == d.OrderNo)
        //          .LeftJoin<SysArgsOptions>((p, d, s) => s.ArgsKey == BusinessConst.AbnormalReceiptClassification && d.ReceivingAbnormalType == s.OptionKey)
        //          .Where((p, d) => p.OrderNo.Contains(searchKey) || p.Remark.Contains(searchKey) || d.ExternalOrderNo.Contains(searchKey) || p.CreateUserName.Contains(searchKey) || d.SupplierName.Contains(searchKey)
        //           || d.GoodsNo.Contains(searchKey) || d.GoodsName.Contains(searchKey) || d.GoodsClassifyName.Contains(searchKey) || d.WaybillNo.Contains(searchKey)
        //           || d.ReceivingAbnormalType.Contains(searchKey))
        //          .Where((p, d) => d.ReceivingLevel.Contains(receivingLevel))
        //          .Where((p, d) => p.CreateUserId.Contains(createUserName))
        //          .Where((p, d) => d.DetailStatus.Contains(detailStatus))
        //          .Where((p, d) => p.GoodsClassify == goodsGroup && p.ExpectDate.Date >= GetDateStart(dateStart).Date && p.ExpectDate.Date <= GetDateEnd(dateEnd).Date)
        //          .Select((p, d, s) => new ReceivingOrderExpandDto
        //          {
        //              OrderNo = p.OrderNo,
        //              ExternalOrderNo = d.ExternalOrderNo,
        //              SupplierId = d.SupplierId,
        //              SupplierName = d.SupplierName,
        //              ReceivingLevel = d.ReceivingLevel,
        //              TotalPrice = p.TotalPrice,
        //              PriceUnitId = p.PriceUnitId,
        //              PriceUnitName = p.PriceUnitName,
        //              ExpectDate = p.ExpectDate,
        //              IsMakeInvoice = d.IsMakeInvoice,
        //              InvoiceNumber = d.InvoiceNumber,
        //              IsAccountPaid = p.IsAccountPaid,
        //              PaymentAmount = p.PaymentAmount,
        //              DownTime = d.DownTime,
        //              QuantityActual = d.QuantityActual,
        //              Status = p.Status,
        //              CreateDate = p.CreateDate,
        //              CreateUserId = p.CreateUserId,
        //              CreateUserName = p.CreateUserName,
        //              Remark = p.Remark,
        //              ReceivingResponsableUserId = p.ReceivingResponsableUserId,
        //              ReceivingResponsableUserName = p.ReceivingResponsableUserName,
        //              ReceivingResponsableUserEmail = p.ReceivingResponsableUserEmail,
        //              ReceivingAddress = p.ReceivingAddress,
        //              GoodsClassify = p.GoodsClassify,
        //              IsEmailNotification = p.IsEmailNotification,
        //              IsASN = d.IsASN,
        //              ASNCheckStatus = d.ASNCheckStatus,
        //              WaybillNo = d.WaybillNo,
        //              DetialId = d.DetialId,
        //              GoodsId = d.GoodsId,
        //              GoodsNo = d.GoodsNo,
        //              GoodsName = d.GoodsName,
        //              GoodsClassifyName = d.GoodsClassifyName,
        //              GoodsModel = d.GoodsModel,
        //              Quantity = d.Quantity,
        //              QuantityUrgency = d.QuantityUrgency,
        //              QuantityUnitId = d.QuantityUnitId,
        //              QuantityUnitName = d.QuantityUnitName,
        //              Price = d.Price,
        //              WorkpieceTray = d.WorkpieceTray,
        //              Pallet = d.Pallet,
        //              AbnormalDeliveryPallet = d.AbnormalDeliveryPallet,
        //              DetailTotalPrice = d.DetailTotalPrice,
        //              DetailStatus = d.DetailStatus,
        //              ReceivingAbnormalType = d.ReceivingAbnormalType,
        //              ReceivingAbnormalTypeName = s.OptionName,
        //              ReceivingAbnormalDesc = d.ReceivingAbnormalDesc,
        //              ReceivingDate = d.ReceivingDate,
        //              ReceivingOperatorId = d.ReceivingOperatorId,
        //              ReceivingOperatorName = d.ReceivingOperatorName,
        //              ApprovalLastRank = SqlFunc.Subqueryable<ApprovalHis>().Where(h => h.PrimaryId == p.OrderNo && h.DataType == receivingPlan).Max(h => h.ApprovalRank)
        //          })
        //          .ToList();

        //    int sumWorkpieceTray = 0;
        //    int sumPallet = 0;
        //    for (int i = 0; i < dataSum.Count; i++)
        //    {
        //        int WorkpieceTray = dataSum[i].WorkpieceTray;
        //        sumWorkpieceTray+= WorkpieceTray;
        //        int Pallet = dataSum[i].Pallet;
        //        sumPallet+= Pallet;
        //    }

        //    var res = new TableModel<ReceivingOrderExpandDto>() { Total = total, Rows = data,Sum= sumWorkpieceTray, Count= sumPallet };
        //    return await Task.FromResult(res);
        //}
        public async Task<TableModel<ReceivingOrderExpandDto>> GetOrders(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, string goodsGroup, string receivingLevel, string createUserName, string detailStatus)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "CreateDate" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            receivingLevel = string.IsNullOrEmpty(receivingLevel) ? "" : receivingLevel.Trim();
            createUserName = string.IsNullOrEmpty(createUserName) ? "" : createUserName.Trim();
            detailStatus = string.IsNullOrEmpty(detailStatus) ? "" : detailStatus.Trim();

            var curApprover = await GetApprover(ApprovalDataType.ReceivingPlan.ToString(), userId);
            var approvalModel = curApprover.FirstOrDefault()?.ApprovalModel;
            var curApproverRank = curApprover.Select(x => x.Rank).Distinct().ToList();
            var isAnyApproval = approvalModel == ApprovalModel.Any.ToString() && curApprover?.Count > 0;
            string receivingPlan = ApprovalDataType.ReceivingPlan.ToString();

            // 构建查询条件
            var query = Repository.ClientDb.Queryable<ReceivingOrder>()
                .InnerJoin<ReceivingOrderDetail>((p, d) => p.OrderNo == d.OrderNo)
                .LeftJoin<SysArgsOptions>((p, d, s) => s.ArgsKey == BusinessConst.AbnormalReceiptClassification && d.ReceivingAbnormalType == s.OptionKey)
                .Where((p, d) => p.OrderNo.Contains(searchKey) || p.Remark.Contains(searchKey) || d.ExternalOrderNo.Contains(searchKey) ||
                                 p.CreateUserName.Contains(searchKey) || d.SupplierName.Contains(searchKey) || d.GoodsNo.Contains(searchKey) ||
                                 d.GoodsName.Contains(searchKey) || d.GoodsClassifyName.Contains(searchKey) || d.WaybillNo.Contains(searchKey) ||
                                 d.ReceivingAbnormalType.Contains(searchKey))
                .Where((p, d) => d.ReceivingLevel.Contains(receivingLevel))
                .Where((p, d) => p.CreateUserId.Contains(createUserName))
                .Where((p, d) => d.DetailStatus.Contains(detailStatus))
                .Where((p, d) => p.GoodsClassify == goodsGroup &&
                                 p.ExpectDate.Date >= GetDateStart(dateStart).Date &&
                                 p.ExpectDate.Date <= GetDateEnd(dateEnd).Date);

            // 分页查询数据
            var data = query
                .Select((p, d, s) => new ReceivingOrderExpandDto
                {
                    OrderNo = p.OrderNo,
                    ExternalOrderNo = d.ExternalOrderNo,
                    SupplierId = d.SupplierId,
                    SupplierName = d.SupplierName,
                    ReceivingLevel = d.ReceivingLevel,
                    TotalPrice = p.TotalPrice,
                    PriceUnitId = p.PriceUnitId,
                    PriceUnitName = p.PriceUnitName,
                    ExpectDate = p.ExpectDate,
                    IsMakeInvoice = d.IsMakeInvoice,
                    InvoiceNumber = d.InvoiceNumber,
                    IsAccountPaid = p.IsAccountPaid,
                    PaymentAmount = p.PaymentAmount,
                    DownTime = d.DownTime,
                    QuantityActual = d.QuantityActual,
                    Status = p.Status,
                    CreateDate = p.CreateDate,
                    CreateUserId = p.CreateUserId,
                    CreateUserName = p.CreateUserName,
                    Remark = p.Remark,
                    ReceivingResponsableUserId = p.ReceivingResponsableUserId,
                    ReceivingResponsableUserName = p.ReceivingResponsableUserName,
                    ReceivingResponsableUserEmail = p.ReceivingResponsableUserEmail,
                    ReceivingAddress = p.ReceivingAddress,
                    GoodsClassify = p.GoodsClassify,
                    IsEmailNotification = p.IsEmailNotification,
                    IsASN = d.IsASN,
                    ASNCheckStatus = d.ASNCheckStatus,
                    WaybillNo = d.WaybillNo,
                    DetialId = d.DetialId,
                    GoodsId = d.GoodsId,
                    GoodsNo = d.GoodsNo,
                    GoodsName = d.GoodsName,
                    GoodsClassifyName = d.GoodsClassifyName,
                    GoodsModel = d.GoodsModel,
                    Quantity = d.Quantity,
                    QuantityUrgency = d.QuantityUrgency,
                    QuantityUnitId = d.QuantityUnitId,
                    QuantityUnitName = d.QuantityUnitName,
                    Price = d.Price,
                    WorkpieceTray = d.WorkpieceTray,
                    Pallet = d.Pallet,
                    AbnormalDeliveryPallet = d.AbnormalDeliveryPallet,
                    DetailTotalPrice = d.DetailTotalPrice,
                    DetailStatus = d.DetailStatus,
                    ReceivingAbnormalType = d.ReceivingAbnormalType,
                    ReceivingAbnormalTypeName = s.OptionName,
                    ReceivingAbnormalDesc = d.ReceivingAbnormalDesc,
                    ReceivingDate = d.ReceivingDate,
                    ReceivingOperatorId = d.ReceivingOperatorId,
                    ReceivingOperatorName = d.ReceivingOperatorName,
                    ApprovalLastRank = SqlFunc.Subqueryable<ApprovalHis>().Where(h => h.PrimaryId == p.OrderNo && h.DataType == receivingPlan).Max(h => h.ApprovalRank),
                    IsInBaseFiles = SqlFunc.Subqueryable<BaseFiles>().Where(bf => bf.PrimaryId == p.OrderNo && bf.FileInfoType == "ReceivingOrderAttachment").Any()
                })
                .OrderBy($"{orderFiled} {orderType}")
                .ToPageList(pgIndex, pgSize, ref total);

            // 处理数据
            string pending = ReceivingOrderStatus.Pending.ToString();
            string approvaling = ReceivingOrderStatus.Approvaling.ToString();
            data.ForEach(row =>
            {
                row.StatusDesc = EnumHelper.GetDescFromEnumVal<ReceivingOrderStatus>(row.Status);
                row.DetailStatusDesc = EnumHelper.GetDescFromEnumVal<ReceivingOrderDetailStatus>(row.DetailStatus);
                row.ReceivingLevelDesc = EnumHelper.GetDescFromEnumVal<ReceivingLevel>(row.ReceivingLevel);
                row.ApprovalStatusDesc = EnumHelper.GetDescFromEnumVal<ApprovalStatus>(row.ApprovalStatus);
                row.IsApproval = (isAnyApproval && row.Status == pending) ||
                                 (curApprover?.Count > 0 && curApproverRank.Contains(row.ApprovalLastRank + 1) &&
                                  (row.Status == pending || row.Status == approvaling));
            });

            // 使用同一查询结果计算汇总，避免重复查询数据库
            var allData = query
                .Select((p, d, s) => new ReceivingOrderExpandDto
                {
                    WorkpieceTray = d.WorkpieceTray,
                    Pallet = d.Pallet
                })
                .ToList();

            int sumWorkpieceTray = allData.Sum(x => x.WorkpieceTray);
            int sumPallet = allData.Sum(x => x.Pallet);

            var res = new TableModel<ReceivingOrderExpandDto>() { Total = total, Rows = data, Sum = sumWorkpieceTray, Count = sumPallet };
            return await Task.FromResult(res);
        }
        public async Task<ReceivingOrderDto> GetOrderDetail(string orderNo,string userId)
        {
            var order = await Repository.ClientDb.Queryable<ReceivingOrder>() 
                 .LeftJoin<BaseUnits>((p,  u) => u.UnitId == p.PriceUnitId) 
                 .Where((p,u) => p.OrderNo==orderNo)
                 .Select<ReceivingOrderDto>().SingleAsync();
            order.Details = await Repository.ClientDb.Queryable<ReceivingOrderDetail>() 
                 .Where(d => d.OrderNo == orderNo)
                 .Select<ReceivingOrderDetailDto>()
                 .ToListAsync();
            order.Details.ForEach(f=>f.ReceivingLevelDesc = EnumHelper.GetDescFromEnumVal<ReceivingLevel>(f.ReceivingLevel));
            return order;
        }

        public async Task<List<KeyValueModel>> GetCreateUserNameGroup()
        {
            var data = Repository.ClientDb.Queryable<ReceivingOrder>()
                .LeftJoin<SysUser>((m, d) => m.CreateUserId == d.UserId)
                .Where((m, d) => d.IsVaild == true)
                .GroupBy((m, d) => m.CreateUserId)
                .Select((m, d) => new KeyValueModel { Key = d.UserId, Value = d.UserName, Remark = d.UserCode })
                .ToList();

            //var data=  Repository.ClientDb.Queryable<SysUser>()
            //    .LeftJoin<SysUserRoles>((m, d) => m.UserId == d.UserId)
            //    .LeftJoin<SysRoles>((m, d, s) => d.RoleId == s.RoleId)
            //     .Where((m, d, s) => m.IsVaild == true)
            //    .Select((m, d, s) => new KeyValueModel { Key = m.UserId, Value = m.UserName, Remark = m.UserCode })
            //    .ToList();
            return data;
        }

        public async Task AddReceivingOrder(ReceivingOrderDto data)
        {
            if (data.Details.Count == 0)
            {
                throw new BusinessException("保存失败,请添加收货计划明细");
            }
            var sameGoods = data.Details.GroupBy(d => new { d.GoodsNo,d.QuantityUnitName }).Count();
            if (sameGoods != data.Details.Count)
            {
                throw new BusinessException("保存失败,同一天到货日期中若存在相同物料编号，建议将其汇总后再添加到系统");
            } 
            var curDate = DateTime.Now; 
            var lastData = await Repository.ClientDb.Queryable<ReceivingOrder>().MaxAsync(x => x.OrderNo);
            var receivingOrderModel = _mapper.Map<ReceivingOrder>(data);
            receivingOrderModel.OrderNo = GetPrimaryId("R", lastData);
            receivingOrderModel.YeareAndMonth = curDate.ToStringYYMMExtension();
            receivingOrderModel.CreateDate = curDate; 
            var isReceivingApproval = false;
            var receivingApprovalArgs = await _sysArgsHelper.GetValueByKey(BusinessConst.IsReceivingApproval);
            if (receivingApprovalArgs != null)
            {
                bool.TryParse(receivingApprovalArgs.Value.ToString(), out isReceivingApproval);
            }
            if (isReceivingApproval && data.ExpectDate.Date == curDate.Date)
            {
                //审批信息(如果当前创建人也是审批人,则需要修改和添加相应的审批信息) 
                var process = await GetApprovalProcess(ApprovalDataType.ReceivingPlan.ToString());
                var curApprover = process.Where(a => a.ApproverId == data.CreateUserId).ToList();
                if (curApprover?.Count > 0)
                {
                    var hightApprover = curApprover.OrderByDescending(x => x.Rank).First();
                    receivingOrderModel.ApprovalDate = curDate;
                    receivingOrderModel.ApproverId = data.CreateUserId;
                    receivingOrderModel.ApproverName = data.CreateUserName;
                    receivingOrderModel.ApproverRole = hightApprover.ApproverRole;
                    if (hightApprover.IsLastApproval)
                    {
                        receivingOrderModel.ApprovalStatus = ApprovalStatus.Approve.ToString();
                        receivingOrderModel.Status = ReceivingOrderStatus.WaitReceiving.ToString();
                    }
                    else
                    {
                        receivingOrderModel.Status = ReceivingOrderStatus.Approvaling.ToString();
                        receivingOrderModel.ApprovalStatus = ApprovalStatus.Approvaling.ToString();
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
                                ApproverId = data.CreateUserId,
                                ApproverName = data.CreateUserName,
                                ApproverRoleId = item.ApproverRole,
                                ApproverRoleName = item.ApproverRoleName,
                                DataType = ApprovalDataType.ReceivingPlan.ToString(),
                                Opinion = "自动处理",
                                ApprovalRank = item.Rank,
                                PrimaryId = receivingOrderModel.OrderNo
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
                            ApproverId = data.CreateUserId,
                            ApproverName = data.CreateUserName,
                            ApproverRoleId = hightApprover.ApproverRole,
                            ApproverRoleName = hightApprover.ApproverRoleName,
                            DataType = ApprovalDataType.ReceivingPlan.ToString(),
                            Opinion = "自动处理",
                            ApprovalRank = hightApprover.Rank,
                            PrimaryId = receivingOrderModel.OrderNo
                        };
                        Repository.ClientDb.Insertable(approvalHis).AddQueue();
                    } 
                }
                else
                {
                    receivingOrderModel.Status = ReceivingOrderStatus.Pending.ToString();
                    receivingOrderModel.ApprovalStatus = ApprovalStatus.Pending.ToString();
                }
            }
            else
            {
                receivingOrderModel.Status = ReceivingOrderStatus.WaitReceiving.ToString();
                receivingOrderModel.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
            }
            Repository.ClientDb.Insertable(receivingOrderModel).AddQueue();
            var receivingOrderDetailList= _mapper.Map<List<ReceivingOrderDetail>>(data.Details);
            receivingOrderDetailList.ForEach(f =>
            {
                f.OrderNo = receivingOrderModel.OrderNo;
                f.DetailStatus = receivingOrderModel.Status;
                if (f.ExternalOrderNo.First().ToString() == "5")
                {
                    f.IsASN = true;
                }
                else
                {
                    f.IsASN = false;
                }
            });  
            if (data.Details[0].QuantityUrgency >0&& data.Details[0].ReceivingLevel== ReceivingLevel.Urgent_Especial.ToString())
            {
                await UpdateReceivingUrgency(receivingOrderModel.OrderNo, data.Details[0].GoodsNo, data.Details[0].QuantityUrgency);
            }
            else
            {
                receivingOrderDetailList[0].QuantityUrgency = 0;
            }
            Repository.ClientDb.Insertable(receivingOrderDetailList).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }
         
        public async Task UpdateReceivingOrder(ReceivingOrderDto data)
        {
            if (data.Details.Count == 0)
            {
                throw new BusinessException("保存失败,请添加收货计划明细");
            } 
            var oldReceiving = await Repository.ClientDb.Queryable<ReceivingOrder>().SingleAsync(u => u.OrderNo == data.OrderNo);
            if (oldReceiving==null)
            {
                throw new BusinessException("保存失败,当前收货计划不存在或已删除");
            }
            if (oldReceiving.Status == ReceivingOrderStatus.ReceivedAll.ToString()|| oldReceiving.Status == ReceivingOrderStatus.ReceivedPart.ToString())
            {
                throw new BusinessException("保存失败,该收货计划单已确认收货，不允许再次修改");
            } 
            var curDate = DateTime.Now;
            var receivingOrderModel = _mapper.Map<ReceivingOrder>(data); 
            receivingOrderModel.YeareAndMonth = curDate.ToStringYYMMExtension();
            receivingOrderModel.UpdateDate = curDate;
            var receivingOrderDetailList = _mapper.Map<List<ReceivingOrderDetail>>(data.Details);
            var isReceivingApproval = false;
            var receivingApprovalArgs = await _sysArgsHelper.GetValueByKey(BusinessConst.IsReceivingApproval);
            var oldUrgencyQty = await Repository.ClientDb.Queryable<ReceivingOrderDetail>().Where(w => w.OrderNo==data.OrderNo).Select(w => w.QuantityUrgency).SingleAsync();
            if (receivingApprovalArgs != null)
            {
                bool.TryParse(receivingApprovalArgs.Value.ToString(), out isReceivingApproval);
            }
            if (isReceivingApproval && data.ExpectDate.Date == curDate.Date)
            {
                //审批信息(如果当前创建人也是审批人,则需要修改和添加相应的审批信息) 
                var process = await GetApprovalProcess(ApprovalDataType.ReceivingPlan.ToString());
                var curApprover = process.Where(a => a.ApproverId == data.CreateUserId).ToList();
                if (curApprover?.Count > 0)
                {
                    var hightApprover = curApprover.OrderByDescending(x => x.Rank).First();
                    receivingOrderModel.ApprovalDate = curDate;
                    receivingOrderModel.ApproverId = data.CreateUserId;
                    receivingOrderModel.ApproverName = data.CreateUserName;
                    receivingOrderModel.ApproverRole = hightApprover.ApproverRole;
                    if (hightApprover.IsLastApproval)
                    {
                        receivingOrderModel.ApprovalStatus = ApprovalStatus.Approve.ToString();
                        receivingOrderModel.Status = ReceivingOrderStatus.WaitReceiving.ToString();
                    }
                    else
                    {
                        receivingOrderModel.Status = ReceivingOrderStatus.Approvaling.ToString();
                        receivingOrderModel.ApprovalStatus = ApprovalStatus.Approvaling.ToString();
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
                                ApproverId = data.CreateUserId,
                                ApproverName = data.CreateUserName,
                                ApproverRoleId = item.ApproverRole,
                                ApproverRoleName = item.ApproverRoleName,
                                DataType = ApprovalDataType.ReceivingPlan.ToString(),
                                Opinion = "自动处理",
                                ApprovalRank = item.Rank,
                                PrimaryId = receivingOrderModel.OrderNo
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
                            ApproverId = data.CreateUserId,
                            ApproverName = data.CreateUserName,
                            ApproverRoleId = hightApprover.ApproverRole,
                            ApproverRoleName = hightApprover.ApproverRoleName,
                            DataType = ApprovalDataType.ReceivingPlan.ToString(),
                            Opinion = "自动处理",
                            ApprovalRank = hightApprover.Rank,
                            PrimaryId = receivingOrderModel.OrderNo
                        };
                        Repository.ClientDb.Insertable(approvalHis).AddQueue();
                    } 
                }
                else
                {
                    receivingOrderModel.Status = ReceivingOrderStatus.Pending.ToString();
                    receivingOrderModel.ApprovalStatus = ApprovalStatus.Pending.ToString();
                }
            }
            else
            {
                receivingOrderModel.Status = ReceivingOrderStatus.WaitReceiving.ToString();
                receivingOrderModel.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
            }
            receivingOrderModel.UpdateDate = DateTime.Now;
            Repository.ClientDb.Updateable(receivingOrderModel).AddQueue();
           
            receivingOrderDetailList.ForEach(f =>
            {
                f.OrderNo = receivingOrderModel.OrderNo;
                f.DetailStatus = receivingOrderModel.Status;
                if (f.ExternalOrderNo.First().ToString() == "5")
                {
                    f.IsASN = true;
                }
                else
                {
                    f.IsASN = false;
                }    
            });  
            if (data.Details[0].QuantityUrgency>0&& data.Details[0].QuantityUrgency != oldUrgencyQty && data.Details[0].ReceivingLevel == ReceivingLevel.Urgent_Especial.ToString())
            {
                await UpdateReceivingUrgency(receivingOrderModel.OrderNo, data.Details[0].GoodsNo, data.Details[0].QuantityUrgency);
            }
            else
            {
                receivingOrderDetailList[0].QuantityUrgency = 0;
            }
            Repository.ClientDb.Deleteable<ReceivingOrderDetail>(d => d.OrderNo == data.OrderNo).AddQueue();
            Repository.ClientDb.Insertable(receivingOrderDetailList).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }
          
        public async Task DelReceivingOrder(int[] detailsId)
        {
            var details = await Repository.ClientDb.Queryable<ReceivingOrderDetail>().Where(w => detailsId.Contains(w.DetialId)).ToListAsync();
            var isReceived = details.Exists(e => e.DetailStatus == ReceivingOrderDetailStatus.Received.ToString()|| e.DetailStatus == ReceivingOrderDetailStatus.InStorage.ToString());
            if (isReceived)
            {
                throw new BusinessException("删除失败,选项中存在已收货的计划");
            }
            Repository.ClientDb.Deleteable(details).AddQueue();
            var ordersNo = details.Select(d => d.OrderNo).Distinct().ToList(); 
            var dataCount = await Repository.ClientDb.Queryable<ReceivingOrderDetail>().Where(w => ordersNo.Contains(w.OrderNo))
             .Select(s => new
             {
                 Count = SqlFunc.AggregateCount(s.DetialId),
                 s.OrderNo
             }).GroupBy(g=>g.OrderNo).ToListAsync();
            var delOrderNo = new List<string>();
            foreach(var dc in dataCount)
            {
                var delCount = details.Count(c => c.OrderNo == dc.OrderNo);
                if (delCount == dc.Count)
                {
                    delOrderNo.Add(dc.OrderNo);
                }
            }
            if(delOrderNo.Count > 0)
            {
                Repository.ClientDb.Deleteable<ReceivingOrder>(b => delOrderNo.Contains(b.OrderNo)).AddQueue();
            } 
            await Repository.ClientDb.SaveQueuesAsync();
        } 
           
        public async void AdviceReceiving(MailModel data,string orderNo)
        {
            if (data.ToReceiver.Length == 0)
            {
                throw new BusinessException("请选择邮件收件人");
            }
            if (string.IsNullOrWhiteSpace(data.Body))
            {
                throw new BusinessException("请输入邮件内容");
            }
            await Repository.ClientDb.Updateable<ReceivingOrder>().SetColumns(s => s.IsEmailNotification == true).Where(w => w.OrderNo == orderNo).ExecuteCommandAsync();
            _= _emailService.SendEmail(data.Subject, data.Body, data.ToReceiver, data.ToCC); 
        }
         
        public async Task SubmitReceived(List<ReceivingActualDto> data)
        {
            var detailsId = data.Select(s => s.DetialId).ToList();
            var details = await Repository.ClientDb.Queryable<ReceivingOrderDetail>().Where(w => detailsId.Contains(w.DetialId)).ToListAsync();
            var isReceived = details.Exists(e => e.DetailStatus != ReceivingOrderDetailStatus.WaitReceiving.ToString());
            if (isReceived)
            {
                throw new BusinessException("提交失败,当前选择的明细中存在不在待收货状态的计划");
            }
            var ordersNo = details.Select(d => d.OrderNo).Distinct().ToList();
            var orders = await Repository.ClientDb.Queryable<ReceivingOrder>().Where(w => ordersNo.Contains(w.OrderNo)).ToListAsync();
            var goodsClassifyGroup = orders.Select(s => s.GoodsClassify).Distinct().ToList();
            if (goodsClassifyGroup.Count > 1)
            {
                throw new BusinessException("提交失败,所选收货计划明细不能包含多种物料分类");
            }
            foreach(var item in details)
            {
                var curData = data.Single(s => s.DetialId == item.DetialId);
                item.QuantityActual = curData.QuantityActual;
                item.DetailStatus= ReceivingOrderDetailStatus.Received.ToString();
                item.ReceivingDate = DateTime.Now;
                item.ReceivingOperatorId = curData.ReceivingOperatorId;
                item.ReceivingOperatorName = curData.ReceivingOperatorName;

                //实际收货数量不等于计划收货数量,计算异常到货托数
                if (item.Quantity!= curData.QuantityActual)
                {
                    item.ReceivingAbnormalType = curData.ReceivingAbnormalType;
                    item.ReceivingAbnormalDesc = curData.ReceivingAbnormalDesc;
                    //查询商品基本信息
                    var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => item.GoodsId.Contains(w.GoodsId)).SingleAsync();
                    if (goodsInfo!=null)
                    {
                        if (goodsInfo.MaxPackageCount > 0 && goodsInfo.PackageCount > 0)
                        {
                            item.AbnormalDeliveryPallet = (int)Math.Ceiling(((item.Quantity - curData.QuantityActual) / goodsInfo.PackageCount) / goodsInfo.MaxPackageCount);
                        }
                    }
                    item.DetailStatus = ReceivingOrderDetailStatus.ReceivingAbnormal.ToString();
                }
            } 
            Repository.ClientDb.Updateable(details).AddQueue();

            var orderDetailStatus = await Repository.ClientDb.Queryable<ReceivingOrderDetail>().Where(w => ordersNo.Contains(w.OrderNo)) 
             .Select(s => new
             {
                s.DetailStatus,
                s.DetialId,
                s.OrderNo
             }).ToListAsync(); 
            foreach (var orderNo in ordersNo)
            {
                var curOrder = orders.Single(o => o.OrderNo == orderNo);
                var curOrderDetailStatus= orderDetailStatus.Where(w=>w.OrderNo == orderNo && !detailsId.Contains(w.DetialId)).ToList();
                if(curOrderDetailStatus.Count > 0)
                {
                    //如果该计划订单中除了当前编辑的明细以外还存在待收货且不存在收货异常和已入库状态的明细，则该计划订单的状态标识为部分已收货
                    if (curOrderDetailStatus.Exists(e => e.DetailStatus == ReceivingOrderDetailStatus.WaitReceiving.ToString()
                    && e.DetailStatus != ReceivingOrderDetailStatus.ReceivingAbnormal.ToString()
                    && e.DetailStatus != ReceivingOrderDetailStatus.InStorage.ToString()))
                    {
                        curOrder.Status = ReceivingOrderStatus.ReceivedPart.ToString();
                    }
                    else if (curOrderDetailStatus.Exists(e => e.DetailStatus != ReceivingOrderDetailStatus.ReceivingAbnormal.ToString()
                    && e.DetailStatus != ReceivingOrderDetailStatus.InStorage.ToString()))
                    {
                        curOrder.Status = ReceivingOrderStatus.ReceivedAll.ToString();
                    }
                }
                else
                {
                    //如果等于0，则表示当前所选的明细包含了该收货计划单的所有明细
                    curOrder.Status = ReceivingOrderStatus.ReceivedAll.ToString();
                }
            }
             Repository.ClientDb.Updateable(orders).AddQueue();
            //如果是备件、样件、辅材、包材则添加对应的入库单
            var goodsClassifyData = EnumHelper.GetEnumValNames<BaseTypeGroup>();
            if (goodsClassifyGroup[0]== BaseTypeGroup.SparePart.ToString()|| goodsClassifyGroup[0] == BaseTypeGroup.SamplePiece.ToString()
                || goodsClassifyGroup[0] == BaseTypeGroup.PackingMaterial.ToString()|| goodsClassifyGroup[0] == BaseTypeGroup.Separator.ToString())
            {
               await _setInStorage(orders,details);
            }
            await Repository.ClientDb.SaveQueuesAsync();
        }

        private async Task _setInStorage(List<ReceivingOrder> orders, List<ReceivingOrderDetail> details)
        {
            var curDate = DateTime.Now;
            var lastData = await Repository.ClientDb.Queryable<InvInStorage>().MaxAsync(x => x.OrderNo); 
            var inStorageOrderlList = new List<InvInStorage>();
            var inStorageOrderlDetailList = new List<InvInStorageDetail>();
            foreach (var data in orders)
            {
                var inStorageModel = new InvInStorage
                {
                    OrderNo = GetPrimaryId("I", lastData),
                    SourceOrderNo = data.OrderNo,
                    InStorageType = InStorageType.PurchaseIn.ToString(),
                    GoodsClassify = data.GoodsClassify,
                    CreateDate = curDate,
                    CreateUserId = data.CreateUserId,
                    CreateUserName = data.CreateUserName,
                    Remark = "来自收货计划的入库单",
                    ApprovalStatus = ApprovalStatus.NoApproval.ToString(),
                    Status = InStorageStatus.WaitInStorage.ToString()
                };
                inStorageOrderlList.Add(inStorageModel);
                lastData = inStorageModel.OrderNo;
                var inStorageDetail = details.Where(w=>w.OrderNo== data.OrderNo).Select(b => new InvInStorageDetail
                {
                    OrderNo = inStorageModel.OrderNo,
                    GoodsId = b.GoodsId,
                    GoodsName = b.GoodsName,
                    Quantity = b.Quantity,
                    UnitId = b.QuantityUnitId
                }).ToList();
                inStorageOrderlDetailList.AddRange(inStorageDetail);
            } 
            Repository.ClientDb.Insertable(inStorageOrderlList).AddQueue(); 
            Repository.ClientDb.Insertable(inStorageOrderlDetailList).AddQueue();
        }

        public async Task<List<ApprovalHisModel>> GetApprovalHis(string orderNo)
        {
            var order = await Repository.GetSingeAsync<ReceivingOrder>(orderNo);
            var createUserRole = await Repository.ClientDb.Queryable<SysUserRoles>()
                .InnerJoin<SysRoles>((ur, r) => ur.RoleId == r.RoleId)
                .Where((ur, r) => ur.UserId == order.CreateUserId)
                .Select((ur, r)=>r.RoleName)
                .FirstAsync();
            var his = new List<ApprovalHisModel>
            {
                new ApprovalHisModel
                {
                    PrimaryId=orderNo,
                    ApprovalDate=order.CreateDate.ToStringExtension(),
                    ApproverName=order.CreateUserName,
                    ApproverRoleName=createUserRole,
                    ApprovalStatusDesc="提交计划",
                    Opinion=order.Remark
                }
            };
            var hisQuery= await Repository.ClientDb.Queryable<ApprovalHis>().Where(w => w.DataType == ApprovalDataType.ReceivingPlan.ToString() && w.PrimaryId == orderNo)
                .Select<ApprovalHisModel>().ToListAsync();
            hisQuery.ForEach(w =>
            { 
                w.ApprovalStatusDesc= EnumHelper.GetDescFromEnumVal<ApprovalStatus>(w.ApprovalStatus);
            });
            his.AddRange(hisQuery);
            return his;
        }

        public async Task UpdateReceivingAbnormal(int detailId,string receivingAbnormalType,string receivingAbnormalDesc)
        {
            var detail = await Repository.GetSingeAsync<ReceivingOrderDetail>(detailId);
            if (detail != null)
            {
                detail.ReceivingAbnormalDesc= receivingAbnormalDesc;
                detail.ReceivingAbnormalType = receivingAbnormalType;
                detail.DetailStatus = ReceivingOrderDetailStatus.ReceivingAbnormal.ToString();
                await Repository.ClientDb.Updateable(detail).ExecuteCommandAsync();
            }
            else
            {
                throw new BusinessException("修改失败,当前计划明细不存在或已被删除");
            } 
        }

        public async Task UpdateReceivingUrgency(string orderNo, string goodsNo, float qty)
        {   
            if(qty > 0)
            {
                var requstInpput = new WMSApiRequest
                {
                    customNo = orderNo,
                    itemCode = goodsNo,
                    qty = (int)qty,
                    operate = "U",
                    itemAreaType = "YCL"
                };
                await _callApiWMS(requstInpput);
            } 
        }

        public async Task UpdateReceivingUrgency(int detailId,float qty)
        {
            if (qty == 0)
            {
                throw new BusinessException("修改失败,如果需要设置紧急需求数量，则必须大于0");
            }
            var detail = await Repository.GetSingeAsync<ReceivingOrderDetail>(detailId);
            if (detail != null)
            {
                if (detail.QuantityUrgency == qty)
                {
                    throw new BusinessException("当前修改的紧急数量未发生变化");
                }
                detail.QuantityUrgency = qty; 
                var requstInpput = new WMSApiRequest
                {
                    customNo=detail.OrderNo,
                    itemCode=detail.GoodsNo,
                    qty=(int)qty,
                    operate="U",
                    itemAreaType="YCL"
                };
               await _callApiWMS(requstInpput);
               await Repository.ClientDb.Updateable(detail).ExecuteCommandAsync();
            }
            else
            {
                throw new BusinessException("修改失败,当前计划明细不存在或已被删除");
            }
        }

        private async Task _callApiWMS(WMSApiRequest input)
        {
            var host = _configuration.GetSection("WMS:Url").Value;
            var url = $"{host}/openApi/other/emergentInvOrder2Wms";
            try
            {
                var res = await _httpHelper.RequestPostAsync<WMSApiRequest, WMSApiResponse>(input, url);
                if (res == null)
                {
                    throw new BusinessException("紧急数量写入WMS失败，WMS接口请求异常，详细请查看日志");
                }
                else if (res.code != 0)
                {
                    throw new BusinessException("紧急数量写入WMS失败，WMS接口请求异常，" + res.msg);
                }
            }
            catch(Exception ex)
            {
                throw new BusinessException($"紧急数量写入WMS失败，WMS接口请求异常，{ex.Message},{ex.InnerException?.Message}");
            }
           
        }

        public async Task ApprovalReceivingOrder(string[] orderNo, bool isApprove, string opinion, string userId, string userName)
        {
            var approvalStatus = isApprove ? ApprovalStatus.Approve.ToString() : ApprovalStatus.Reject.ToString();
            var orders = await Repository.ClientDb.Queryable<ReceivingOrder>().Where(x => orderNo.Contains(x.OrderNo)).ToListAsync();
            foreach (var order in orders)
            {
                if (order.Status != ReceivingOrderStatus.Pending.ToString() && order.Status != ReceivingOrderStatus.Approvaling.ToString())
                {
                    throw new BusinessException("审批失败,所选计划中存在不在审批流程中的数据");
                }
            }
            //查询当前审批流程信息
            var process = await GetApprover(ApprovalDataType.ReceivingPlan.ToString(), userId);
            if (process.Count == 0)
            {
                throw new BusinessException("审批失败,当前用户没有审批权限");
            }
            var apprivalModel = process.First().ApprovalModel;
            //流程审批模式：如果当前审批用户拥有多个审批角色，将以最高审批节点角色来审批，同时还需要将该用户的其他角色以自动审批模式记录下来
            if (apprivalModel == ApprovalModel.Process.ToString())
            {
                //判断上一级是否已审批
                var approvalHis = await Repository.ClientDb.Queryable<ApprovalHis>().Where(a => a.DataType == ApprovalDataType.ReceivingPlan.ToString() && orderNo.Contains(a.PrimaryId)).ToListAsync();
                foreach (var curOrder in orderNo)
                {
                    var lastRank = approvalHis.Count == 0 ? 0 : approvalHis.Where(h => h.PrimaryId == curOrder).Max(h => h.ApprovalRank);
                    var approverRank = process.Select(x => x.Rank).Distinct().ToList();
                    if (!approverRank.Contains(lastRank + 1))
                    {
                        throw new BusinessException($"审批失败,收货计划编号{curOrder}需要等待下级审批");
                    }
                }
                if (!process.Exists(x => x.IsLastApproval) && isApprove)
                {
                    approvalStatus = ApprovalStatus.Approvaling.ToString();
                } 
            }
            //更新收货计划主表审批状态 
            var hightApprover = process.OrderByDescending(x => x.Rank).First();
            orders.ForEach(x =>
            {
                x.ApprovalDate = DateTime.Now;
                x.ApproverId = userId;
                x.ApproverName = userName;
                x.ApproverRole = hightApprover.ApproverRole;
                x.ApprovalStatus = approvalStatus;
                x.Status = approvalStatus == ApprovalStatus.Approve.ToString() ? ReceivingOrderStatus.WaitReceiving.ToString() : approvalStatus;
            });
            Repository.ClientDb.Updateable(orders).AddQueue();
            //更新收货计划子表状态
            var details = await Repository.ClientDb.Queryable<ReceivingOrderDetail>().Where(x => orderNo.Contains(x.OrderNo)).ToListAsync();
            foreach (var detail in details)
            {
                var curOrder= orders.Single(s=>s.OrderNo==detail.OrderNo);
                detail.DetailStatus = curOrder.Status;
            }
            Repository.ClientDb.Updateable(details).AddQueue();
            //添加审批历史记录
            var approvalHisList = new List<ApprovalHis>();
            foreach (var id in orderNo)
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
                                DataType = ApprovalDataType.ReceivingPlan.ToString(),
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
                        DataType = ApprovalDataType.ReceivingPlan.ToString(),
                        Opinion = opinion,
                        ApprovalRank = hightApprover.Rank,
                        PrimaryId = id
                    };
                    approvalHisList.Add(newApprovalHis); 
                }   
            }
            Repository.ClientDb.Insertable(approvalHisList).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }


        public List<FieldModel> GetExportFields()
        {
            return new List<FieldModel>
            {
                new FieldModel {Key="OrderNo",Value="订单号",Remark=3,Type=typeof(string),IsIgnoreImport=true},
                new FieldModel {Key="GoodsNo",Value="物料号",Remark=4,Type=typeof(string),IsRequired=true,DeftVal="Material-Num-Example"},
                new FieldModel {Key="ReceivingLevel",Value="优先级",Remark=5,Type=typeof(string),IsRequired=true,DeftVal="正常/一般紧急/特别紧急"},
                new FieldModel {Key="CreateUserName",Value="计划员", Remark = 6, Type = typeof(string),IsRequired=true,DeftVal="曹孟德"},
                //new FieldModel {Key="IsASN",Value="ASN收货", Remark = 7, Type = typeof(bool),IsRequired=true,DeftVal="Y/N"},
                new FieldModel {Key="ASNCheckStatus",Value="ASNCheck",Remark=8,Type=typeof(string)},
                new FieldModel {Key="ExternalOrderNo",Value="SAP订单号",Remark=10, Type = typeof(string), IsRequired = false ,DeftVal="ASP00001"},
                new FieldModel {Key="InvoiceNumber",Value="发票号", Remark = 9,Type=typeof(string),DeftVal="Invoice-Num"},
                new FieldModel {Key="WaybillNo",Value="运单号",Remark=10, Type = typeof(string),DeftVal="SF000001"},
                new FieldModel {Key="GoodsClassify",Value="物料大类",Remark=11, Type = typeof(string), IsRequired = true,DeftVal="原材料"},
                new FieldModel {Key="GoodsClassifyName",Value="物料类型", Remark = 12, Type = typeof(string), IsRequired = true,DeftVal="电子类"},
                new FieldModel {Key="SupplierName",Value="供应商", Remark = 13, Type = typeof(string), IsRequired = true,DeftVal="请填写供应商编码，例如：700017033"},
                new FieldModel {Key="Quantity",Value="数量", Remark = 14, Type = typeof(float), IsRequired = true,DeftVal=1},
                new FieldModel {Key="QuantityUrgency",Value="紧急数量", Remark = 15, Type = typeof(float),DeftVal=1}, 
                new FieldModel {Key="QuantityUnitName",Value="数量单位", Remark = 16, Type = typeof(string),DeftVal="个"},
                new FieldModel {Key="WorkpieceTray",Value="料盘数", Remark = 17, Type =typeof(int),DeftVal=0},
                new FieldModel {Key="Pallet",Value="托盘数", Remark = 18, Type =typeof(int),DeftVal=0},
                new FieldModel {Key="ExpectDate",Value="预计到货日期", Remark = 19, Type = typeof(DateTime),IsRequired=true,DeftVal=DateTime.Now.ToShortDateString()},
                new FieldModel {Key="DetailStatus",Value="收货状态", Remark = 20, Type = typeof(bool),IsIgnoreImport=true},
                new FieldModel {Key="ReceivingDate",Value="收货日期", Remark = 21, Type = typeof(DateTime),IsIgnoreImport=true},
                new FieldModel {Key="ReceivingOperatorName",Value="收货人", Remark = 22, Type = typeof(string),IsIgnoreImport=true},
                //new FieldModel {Key="ReceivingAbnormalType",Value="异常到货类别", Remark = 23, Type = typeof(string),IsIgnoreImport=true},
                 new FieldModel {Key="OptionName",Value="异常到货类别", Remark = 23, Type = typeof(string),IsIgnoreImport=true},
                new FieldModel {Key="AbnormalDeliveryPallet",Value="异常到货托数", Remark = 24, Type =typeof(int),DeftVal=0},
                new FieldModel {Key="ReceivingAbnormalDesc",Value="异常详细描述", Remark = 25, Type = typeof(string),IsIgnoreImport=true}
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
            string fileName = "收货计划数据导入模板.xlsx";
            return await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
        }

        public async Task ImportReceivingData(string userId, string fileType, Stream stream)
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
            var details = new List<ReceivingOrderExpandDto>();
            var entityType = typeof(ReceivingOrderExpandDto);
            foreach (DataRow row in dt.Rows)
            {
                var model = new ReceivingOrderExpandDto();
                foreach (var field in fields)
                {
                    var cellValue = row[field.Value];
                    object val ;
                    if (field.IsRequired&& cellValue == null)
                    {
                        throw new BusinessException($"{field.Value}不能有空值");
                    }
                    if (field.Type == typeof(bool))
                    {
                         val = false;
                        if (cellValue != null)
                        {
                            if(cellValue.ToString()?.Trim() == "是"|| cellValue.ToString()?.Trim().ToUpper() == "Y")
                            {
                                val = true;
                            }
                        }
                    }
                    else if (field.Type == typeof(int))
                    {
                        int temp=0;
                        int.TryParse(cellValue.ToString()?.Trim(), out temp);
                        val = temp;
                    }
                    else if(field.Type == typeof(float))
                    {
                        float temp = 0;
                        float.TryParse(cellValue.ToString()?.Trim(), out temp);
                        val = temp;
                    }
                    else if (field.Type == typeof(DateTime))
                    {
                        DateTime date;
                        if(!DateTime.TryParse(cellValue.ToString(), out date))
                        {
                            throw new BusinessException($"{field.Value}解析出错，不是正确的日期格式");
                        } 
                        val= date;
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
                    throw new BusinessException($"物料大类解析出错，{g}不是系统内置枚举，请使用：{str}");
                }
            }
            //物料编号验证 (原材料编号回根据供应商的不同存在多个相同的编号）
            var goodsNoArr = details.Select(s => s.GoodsNo).Distinct().ToList();
            var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsNoArr.Contains(w.GoodsNo)).ToListAsync();

            //供应商信息
            var suppliersInfo = await Repository.ClientDb.Queryable<BaseSuppliers>().ToListAsync();

            foreach (var d in details)
            {
                var sameGoods = goodsInfo.Where(s => s.GoodsNo.Equals(d.GoodsNo)).ToList();
                if(sameGoods.Count > 0)
                {
                    if (sameGoods.Count == 1)
                    {
                        var goods = sameGoods.Where(s => s.GoodsNo.Equals(d.GoodsNo)).Single();
                        d.GoodsId = goods.GoodsId;
                        d.GoodsName = goods.GoodsName;
                        d.ExternalOrderNo = d.ExternalOrderNo ?? goods.GoodsField1;
                    }
                    else
                    {
                        var supplierNameCurrent= suppliersInfo.Where(p => p.SupplierNo == d.SupplierName).FirstOrDefault();
                        var goods = sameGoods.Where(s => s.GoodsNo.Equals(d.GoodsNo)&&s.Supplier== supplierNameCurrent?.SupplierName).FirstOrDefault();
                        if(goods != null)
                        {
                            d.GoodsId = goods.GoodsId;
                            d.GoodsName = goods.GoodsName;
                            d.ExternalOrderNo = d.ExternalOrderNo==""?goods.GoodsField1: d.ExternalOrderNo;
                        }
                        else
                        {
                            d.GoodsId = sameGoods[0].GoodsId;
                            d.GoodsName = sameGoods[0].GoodsName;
                            d.ExternalOrderNo = d.ExternalOrderNo ?? sameGoods[0].GoodsField1;
                        }
                    } 
                }
                else
                {
                    throw new BusinessException($"物料编号：{d.GoodsNo}未在系统中建立基础信息维护");
                }
                if (string.IsNullOrEmpty(d.ExternalOrderNo))
                {
                    throw new BusinessException($"物料编号：{d.GoodsNo}未在匹配到对应的SAP订单号，请在导入文件中手动指定");
                }
            }
            //优先级验证
            var levelEnums = EnumHelper.GetEnumValNames<ReceivingLevel>();
            var levels = details.Select(s => s.ReceivingLevel).Distinct().ToList();
            foreach (var le in levels)
            {
                if (!levelEnums.Exists(e => e.Value.ToString() == le))
                {
                    var str = string.Join(", ", levelEnums.Select(s => s.Value.ToString()).ToList());
                    throw new BusinessException($"优先级解析出错，{le}不是系统内置枚举，请使用：{str}标识");
                }
            }
             
            //查询供应商ID
            var suppliersName = details.Select(s => s.SupplierName).Distinct().ToList();
            var suppliersInfoCurrent = suppliersInfo.Where(w => suppliersName.Contains(w.SupplierNo)).ToList();

            //查询单位ID
            var unitsName = details.Select(s => s.QuantityUnitName).Distinct().ToList();
            var unitsInfo = await Repository.ClientDb.Queryable<BaseUnits>().Where(w => unitsName.Contains(w.UnitName)&&w.IsValid).ToListAsync();
            var deftPriceUnit = await Repository.ClientDb.Queryable<BaseUnits>().SingleAsync(s => s.UnitName == "元");
            var user = await Repository.GetSingeAsync<SysUser>(userId);
            var orderList = new List<ReceivingOrderDto>(); 
            var lastData = await Repository.ClientDb.Queryable<ReceivingOrder>().MaxAsync(x => x.OrderNo);
            foreach (var grp in details)
            {
                if (grp.ExpectDate.Date < DateTime.Now.Date)
                {
                    throw new BusinessException($"预计到货日期不能小于当前日期");
                } 
                var order = new ReceivingOrderDto
                {
                    OrderNo = GetPrimaryId("R", lastData),
                    PriceUnitId = deftPriceUnit == null ? 0 : deftPriceUnit.UnitId,
                    PriceUnitName= deftPriceUnit == null ? "元" : deftPriceUnit.UnitName, 
                    ExpectDate= grp.ExpectDate, 
                    YeareAndMonth= int.Parse(grp.ExpectDate.ToString("yyyyMM")),
                    ReceivingAddress = (await _sysArgsHelper.GetValueByKey(BusinessConst.ReceivingAddress)).Value.ToString(),
                    GoodsClassify= classifyGroupEnums.Single(s=>s.Value.ToString()== grp.GoodsClassify).Key.ToString(),
                    CreateUserId=userId,
                    CreateUserName= user.UserName,
                    CreateDate=DateTime.Now
                };
                lastData = order.OrderNo; 
                var orderDetail = _mapper.Map<ReceivingOrderDetailDto>(grp);
                orderDetail.OrderNo=order.OrderNo;
                order.Details = new List<ReceivingOrderDetailDto> { orderDetail };
                orderList.Add(order);
            }
            foreach(var order in orderList)
            {   
                foreach (var d in order.Details)
                {
                    if (d.Quantity <= 0)
                    {
                        throw new BusinessException($"收货数量必须大于0");
                    } 
                     var curSupplier = suppliersInfoCurrent.Where(e => e.SupplierNo == d.SupplierName).ToList();
                    if (curSupplier == null|| curSupplier.Count==0)
                    {
                         throw new BusinessException($"供应商编码{d.SupplierName}未在系统中维护，请在供应商管理中添加该信息");
                    } 
                    else
                    {
                        d.SupplierName= curSupplier[0].SupplierName;
                        d.SupplierId = curSupplier[0].SupplierId;
                    } 
                    var curUnit = unitsInfo.SingleOrDefault(e => e.UnitName == d.QuantityUnitName);
                    if (curUnit == null)
                    {
                        throw new BusinessException($"数量单位{d.QuantityUnitName}未在系统中维护，请在单位管理中添加该信息");
                    }
                    //重新计算料盘数、托盘数
                    if (d.WorkpieceTray == 0 && d.Pallet == 0)
                    {
                        var curGoodsInfo = goodsInfo.Where(w => w.GoodsId == d.GoodsId && w.Supplier== d.SupplierName).Single();
                        //if (curGoodsInfo.MaxPackageUnitName == "托")
                        //{
                        //    if (curGoodsInfo.MaxPackageCount > 0 && curGoodsInfo.PackageCount > 0)
                        //    {
                        //        d.Pallet = (int)Math.Ceiling((d.Quantity / curGoodsInfo.PackageCount) / curGoodsInfo.MaxPackageCount);
                        //    }
                        //}
                        if (curGoodsInfo.MaxPackageUnitName == "盘")
                        {
                            if (curGoodsInfo.PackageCount > 0)
                            {
                                d.WorkpieceTray = (int)Math.Ceiling(d.Quantity / curGoodsInfo.PackageCount);
                            }
                        }
                        else 
                        {
                            if (curGoodsInfo.MaxPackageCount > 0 && curGoodsInfo.PackageCount > 0)
                            {
                                d.Pallet = (int)Math.Ceiling((d.Quantity / curGoodsInfo.PackageCount) / curGoodsInfo.MaxPackageCount);
                            }
                        }
                    }
                    d.QuantityUnitId = curUnit.UnitId; 
                    d.ReceivingLevel = levelEnums.Single(s => s.Value.ToString() == d.ReceivingLevel).Key.ToString();
                    d.OrderNo = order.OrderNo; 
                }  
            }
            foreach (var order in orderList)
            {
                await AddReceivingOrder(order);
            }
        }

        public async Task<string> ExportReceivingData(string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, string goodsGroup, List<KeyValueModel> fields)
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
                        { "@ExternalOrderNo", "%"+searchKey+"%" },
                        { "@SupplierName", "%"+searchKey+"%" },
                        { "@CreateUserName", "%"+searchKey+"%" },
                        { "@GoodsNo", "%"+searchKey+"%" },
                        { "@GoodsName", "%"+searchKey+"%" },
                        { "@GoodsClassifyName", "%"+searchKey+"%" },
                        { "@WaybillNo", "%"+searchKey+"%" },
                        { "@ReceivingAbnormalType", "%"+searchKey+"%" },
                        { "@GoodsClassify", goodsGroup },
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
                string sign = BusinessConst.AbnormalReceiptClassification;
                var fieldStr = sb.ToString().TrimEnd(',');
                //string sql = $@" select {fieldStr} from ReceivingOrder o
                //                join ReceivingOrderDetail d on o.OrderNo=d.OrderNo
                //                Left Join SysArgsOptions  s on d.ReceivingAbnormalType = s.OptionKey   
                //                where (o.OrderNo like @OrderNo or o.Remark like @Remark or ExternalOrderNo like @ExternalOrderNo or SupplierName like @SupplierName or CreateUserName like @CreateUserName or GoodsNo like @GoodsNo 
                //                or GoodsName like @GoodsName or GoodsClassifyName like @GoodsClassifyName or WaybillNo like @WaybillNo or ReceivingAbnormalType like @ReceivingAbnormalType)
                //                and s.ArgsKey='{sign}'
                //                and GoodsClassify=@GoodsClassify
                //                and DATE(ExpectDate)>=@ds and DATE(ExpectDate)<=@dn
                //                order by {orderFiled} {orderType}";

                string sql = $@" select {fieldStr} from ReceivingOrder o
                                join ReceivingOrderDetail d on o.OrderNo=d.OrderNo
                                Left Join SysArgsOptions  s on d.ReceivingAbnormalType = s.OptionKey   
                                where (o.OrderNo like @OrderNo or o.Remark like @Remark or ExternalOrderNo like @ExternalOrderNo or SupplierName like @SupplierName or CreateUserName like @CreateUserName or GoodsNo like @GoodsNo 
                                or GoodsName like @GoodsName or GoodsClassifyName like @GoodsClassifyName or WaybillNo like @WaybillNo or ReceivingAbnormalType like @ReceivingAbnormalType)
                               
                                and GoodsClassify=@GoodsClassify
                                and DATE(ExpectDate)>=@ds and DATE(ExpectDate)<=@dn
                                order by {orderFiled} {orderType}";

                var queryData = await Repository.QueryBySqlAsync(sql, param);
                var stream = ExcelHelper.ConvertDataTableToStream(queryData);
                var fileName = $"收货计划信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
                var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
                return fileUrl;
            }
            return await Task.FromResult("");
        }


        public async Task UploadReceivingDocument(string userId, string orderNo, string fileName, Stream stream)
        {
            ////上传到MIO
            //MinIO文件路径
            var MIOPath = await _fileStorage.SaveFile(fileName, stream, FileType.Image);
            if (!string.IsNullOrWhiteSpace(MIOPath))
            {
                var attachmentEntity = new BaseFiles
                {
                    PrimaryId = orderNo,
                    FileName = fileName[..fileName.LastIndexOf(".")],
                    FileInfoType = FileInfoType.ReceivingOrderAttachment.ToString(),
                    Path = MIOPath,
                    Url = MIOPath
                };
                Repository.ClientDb.Insertable(attachmentEntity).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task AddDeliveryOrder()
        {

        }

        public async Task PrintMatLabel()
        {

        }
    }
}
