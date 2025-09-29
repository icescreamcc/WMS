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
using Models.Model.Plan;
using Models.Model.PlanMaterial; 
using Models.Model.Sys;
using Newtonsoft.Json; 
using SqlSugar;  
using System.Data;
using System.Globalization; 
using System.Text; 

namespace Logic.PlanMaterial
{
    public class MaterialRequirementPlanMgr : ExternalApiHandler
    {
        private readonly IConfiguration _configuration;

        private readonly LogHelper _logHelper;

        private readonly IMapper _mapper;

        private readonly IFileStorage _fileStorage;

        private readonly EmailService _emailService;

        private readonly SysArgsService _sysArgsService;

        public MaterialRequirementPlanMgr(Repository repository,
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

        public async Task<TableModel<PlanMaterialRequirementOrderDto>> GetPlanOrders(int pgSize, int pgIndex, string orderField, string orderType, string searchKey, int year, int week, string goodsGroup,string status)
        {
            int total = 0;
            orderField = string.IsNullOrEmpty(orderField) ? "CreateDate" : orderField;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<PlanMaterialRequirementOrder>()
                .LeftJoin<BaseSuppliers>((o,s)=>o.SupplierId==s.SupplierId) 
                .Where((o,s)=>s.SupplierName.Contains(searchKey)
                || SqlFunc.Subqueryable<PlanMaterialRequirementDetail>().Where(d=>d.GoodsId.Contains(searchKey)||d.GoodsNo.Contains(searchKey)||d.GoodsName.Contains(searchKey)).Any())
                .Where((o,s)=>o.GoodsClassifyGroup==goodsGroup && o.Year == year && o.Week == week) 
                .WhereIF(!string.IsNullOrEmpty(status),(o,s)=>o.Status == status)
                .Select<PlanMaterialRequirementOrderDto>()
                .OrderBy($"{orderField} {orderType}")
                .ToPageList(pgIndex, pgSize, ref total);
            data.ForEach(f => f.StatusDesc = EnumHelper.GetDescFromEnumVal<PlanMaterialRequirementStatus>(f.Status));
            var res = new TableModel<PlanMaterialRequirementOrderDto>() { Total = total, Rows = data };
            return await Task.FromResult(res); 
        }
         
        public async Task<PlanMaterialRequirementOrderDto> GetPlanOrderDetails(string orderNo)
        {
            var order = await Repository.ClientDb.Queryable<PlanMaterialRequirementOrder>()
                .LeftJoin<BaseSuppliers>((o,s)=>o.SupplierId==s.SupplierId)
                .Where((o, s) => o.OrderNo==orderNo).Select<PlanMaterialRequirementOrderDto>().SingleAsync();
            order.StatusDesc = EnumHelper.GetDescFromEnumVal<PlanMaterialRequirementStatus>(order.Status);
            order.Details = await Repository.ClientDb.Queryable<PlanMaterialRequirementDetail>()
                .LeftJoin<BaseGoods>((d, g) => d.GoodsId == g.GoodsId)
                .LeftJoin<BaseType>((d, g, t) => t.TypeId == g.GoodsClassifyId)
                .Where((d, g, t) => d.OrderNo == orderNo)
                .OrderBy((d,g,t)=>new { d.ReqDate ,d.Shift})
                .Select((d, g, t) => new PlanMaterialRequirementDetailDto
                {
                    DetailId = d.DetailId,
                    OrderNo = d.OrderNo,
                    GoodsId = d.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsClassifyGroup = d.GoodsClassifyGroup,
                    GoodsType = t.TypeName,
                    RequirementLevel = d.RequirementLevel,
                    Shift = d.Shift,
                    ReqDate = d.ReqDate,
                    Quantity = d.Quantity,
                    UnitName = d.UnitName

                }).ToListAsync();
            order.Details.ForEach(f =>
            {
                f.RequirementLevelDesc = EnumHelper.GetDescFromEnumVal<MaterialRequirementLevel>(f.RequirementLevel);
            }); 
            if (!string.IsNullOrEmpty(order.SupplierId))
            {
                order.SupplierContactList = await Repository.ClientDb.Queryable<BaseSupplierContact>().Where(s => s.SupplierId == order.SupplierId).Select<SupplierContactDto>().ToListAsync();
            } 
            return order;
        }

        public async Task<List<MailDto>> GetOrderMailInfo(string orderNo)
        {
            var mailData = await Repository.ClientDb.Queryable<SysMail>().Where(w => w.PrimaryId == orderNo).OrderByDescending(o=>o.CreateDate).Select<SysMail>().ToListAsync();
            var mailInfo = new List<MailDto>();
            foreach(var data in mailData)
            {
                var info = new MailDto
                {
                    Subject = data.Subject,
                    Body = data.Body,
                    CreateDate = data.CreateDate,
                    Sender=data.Sender,
                    AttachmentId = data.AttachmentId,
                    PrimaryId= data.PrimaryId,
                };
                info.ToReceiver = data.ToReceiver.Split(',');
                if (data.ToCC != null && data.ToCC.Length > 0)
                {
                    info.ToCC = data.ToCC.Split(",");
                }
                mailInfo.Add(info);
            }
            if (mailInfo.Count > 0)
            {
                var attachmentsId = mailInfo.Select(s => s.AttachmentId).ToList();
                if(attachmentsId.Count > 0)
                {
                    var attachments = await Repository.ClientDb.Queryable<BaseFiles>().Where(w => attachmentsId.Contains(w.PrimaryId)).Select<FileInfoDto>().ToListAsync();
                    foreach(var mail in mailInfo)
                    {
                        mail.Attachments = attachments.Where(w => w.PrimaryId == mail.AttachmentId).ToList();
                    }
                }
            } 
            return mailInfo;
        }

        public async Task<List<PlanFinishedProductOrderDto>> GetProductionInfoByReqPlanOrder(int year,int week,float version)
        {
            var verRound = Math.Round(version, 2);
            return await Repository.ClientDb.Queryable<PlanFinishedProductOrder>()
                .Where(w => w.Year == year && w.Week == week && SqlFunc.Round(w.Version,2) == verRound)
                .Select<PlanFinishedProductOrderDto>()
                .ToListAsync();

        }

        public async Task AddPlanOrder(PlanMaterialRequirementOrderDto data)
        {
            if(data.Details==null|| data.Details.Count == 0)
            {
                throw new BusinessException("保存失败，请添加计划物料需求明细");
            } 
            var curYear = DateTime.Now.Year;
            var curWeek = new GregorianCalendar().GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            if (data.Year < curYear)
            {
                throw new BusinessException("保存失败，只能创建下周之后的物料需求计划");
            }
            else if (data.Year == curYear && data.Week <= curWeek)
            {
                throw new BusinessException("保存失败，只能创建下周之后的物料需求计划");
            }
            var exist = await Repository.ClientDb.Queryable<PlanMaterialRequirementOrder>().Where(w => w.Year == data.Year && w.Week == data.Week).AnyAsync();
            if (exist)
            {
                throw new BusinessException($"保存失败，{data.Year}CW{data.Week}已存在一份需求计划");
            }
            var orderEntity = _mapper.Map<PlanMaterialRequirementOrder>(data);
            orderEntity.CreateDate = DateTime.Now;
            orderEntity.OrderNo = Guid.NewGuid().ToString("N").ToUpper();
            orderEntity.Status = PlanMaterialRequirementStatus.Created.ToString();
            Repository.ClientDb.Insertable(orderEntity).AddQueue();
            var detailEntity = _mapper.Map<List<PlanMaterialRequirementDetail>>(data.Details);
            detailEntity.ForEach(f => f.OrderNo = orderEntity.OrderNo);
            Repository.ClientDb.Insertable(detailEntity).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task UpdatePlanOrder(PlanMaterialRequirementOrderDto data)
        {
            if (data.Details == null || data.Details.Count == 0)
            {
                throw new BusinessException("保存失败，请添加计划明细");
            }
            var curYear = DateTime.Now.Year;
            var curWeek = new GregorianCalendar().GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            if (data.Year < curYear)
            {
                throw new BusinessException("保存失败，只能创建下周之后的物料需求计划");
            }
            else if (data.Year == curYear && data.Week <= curWeek)
            {
                throw new BusinessException("保存失败，只能创建下周之后的物料需求计划");
            }
            var isExceed = await Repository.ClientDb.Queryable<PlanMaterialRequirementOrder>().AnyAsync(w => w.OrderNo == data.OrderNo && w.Status == PlanMaterialRequirementStatus.Exceed.ToString());
            if(isExceed)
            {
                throw new BusinessException("保存失败，不能对已过期的物料需求计划进行调整");
            }
            var exist = await Repository.ClientDb.Queryable<PlanMaterialRequirementOrder>().Where(w =>w.OrderNo!=data.OrderNo && w.Year == data.Year && w.Week == data.Week).AnyAsync();
            if (exist)
            {
                throw new BusinessException($"保存失败，{data.Year}CW{data.Week}已存在一份需求计划");
            }
            var orderEntity = _mapper.Map<PlanMaterialRequirementOrder>(data);
            orderEntity.UpdateDate = DateTime.Now; 
            Repository.ClientDb.Insertable(orderEntity).AddQueue();
            var detailEntity = _mapper.Map<List<PlanMaterialRequirementDetail>>(data.Details);
            detailEntity.ForEach(f => f.OrderNo = orderEntity.OrderNo);
            Repository.ClientDb.Insertable(detailEntity).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task DelPlanOrder(string orderNo)
        {
            var order = await Repository.GetSingeAsync<PlanMaterialRequirementOrder>(orderNo);

            var isExceed = await Repository.ClientDb.Queryable<PlanMaterialRequirementOrder>().AnyAsync(w => w.OrderNo == orderNo && w.Status == PlanMaterialRequirementStatus.Exceed.ToString());
            if (isExceed)
            {
                throw new BusinessException("删除失败，不能删除已过期的物料需求计划");
            }
            Repository.ClientDb.Deleteable<PlanMaterialRequirementOrder>(w => w.OrderNo == orderNo).AddQueue();
            Repository.ClientDb.Deleteable<PlanMaterialRequirementDetail>(w => w.OrderNo == orderNo).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public List<FieldModel> GetExportFields()
        {
            return new List<FieldModel>
            {
                new FieldModel {Key="Year",Value="计划年份", Remark = 1},
                new FieldModel {Key="Week",Value="计划周", Remark = 2},
                new FieldModel {Key="Version",Value="计划版本", Remark = 3},
                new FieldModel {Key="SupplierName",Value="供应商", Remark = 4},
                new FieldModel {Key="GoodsNo",Value="物料编码",Remark=5},
                new FieldModel {Key="GoodsName",Value="物料名称",Remark=6},
                new FieldModel {Key="GoodsClassifyGroup",Value="物料分类",Remark=7},
                new FieldModel {Key="TypeName",Value="物料类型", Remark = 8}, 
                new FieldModel {Key="Quantity",Value="需求数量",Remark=9},
                new FieldModel {Key="UnitName",Value="数量单位", Remark = 10},
                new FieldModel {Key="RequirementLevel",Value="需求等级", Remark = 11},
                new FieldModel {Key="Shift",Value="需求班次",Remark=12},
                new FieldModel {Key="ReqDate",Value="需求日期", Remark = 13},
                new FieldModel {Key="Remark",Value="备注", Remark = 13}

            };
        }

        public async Task<string> ExportPlanOrder(string orderField, string orderType, string searchKey, int year, int week, string goodsGroup, string status, List<KeyValueModel> fields)
        {
            if (fields.Count > 0)
            {
                var dbType = _configuration.GetSection("Sqlsugar:DbType").Value;
                orderField = "d.ReqDate";
                searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
                var param = new Dictionary<string, object>
                    {
                        { "@GoodsName", "%"+searchKey+"%" },
                        { "@GoodsId", "%"+searchKey+"%" },
                        { "@GoodsNo", "%"+searchKey+"%" },
                        { "@SupplierName", "%"+searchKey+"%" },
                        { "@GoodsClassifyGroup", goodsGroup },
                        { "@Year", year },
                        { "@Week", week }
                    };
                StringBuilder sb = new StringBuilder();
                foreach (var field in fields)
                {
                     if (field.Key.ToString() == "GoodsNo"|| field.Key.ToString() == "GoodsName")
                    {
                        if (dbType == "MySql")
                        {
                            sb.Append($"g.`{field.Key}` as {field.Value},");
                        }
                        else
                        {
                            sb.Append($"g.[{field.Key}] as {field.Value},");
                        }
                    }
                   else if (field.Key.ToString() == "GoodsClassifyGroup" || field.Key.ToString() == "Remark")
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
                string sql = $@" select {fieldStr} from planmaterialrequirementorder o
                                 join planmaterialrequirementdetail d on o.OrderNo=d.OrderNo
                                 left join basesuppliers s on s.SupplierId=o.SupplierId
                                 left join basegoods g on g.GoodsId=d.goodsid
                                 left join basetype t on t.TypeId=g.GoodsClassifyId
                                 where (d.GoodsNo like @GoodsNo or d.GoodsId like @GoodsId or d.GoodsName like @GoodsName or s.SupplierName like @SupplierName)
                                 and Year=@Year and Week=@Week and o.GoodsClassifyGroup=@GoodsClassifyGroup 
                                 and (case when '{status}'='' then 1=1 else o.Status='{status}' end)
                                 order by {orderField} {orderType}";
                var queryData = await Repository.QueryBySqlAsync(sql, param);
                var stream = ExcelHelper.ConvertDataTableToStream(queryData);
                var fileName = $"物料需求计划信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
                var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
                return fileUrl;
            }
            return await Task.FromResult("");
        }

        public async Task CreateMailToSend(string orderNo,string supplierId)
        { 
            var supplierContact = await Repository.ClientDb.Queryable<BaseSupplierContact>().Where(w => w.SupplierId == supplierId).ToListAsync();
            if (supplierContact == null || supplierContact.Count == 0)
            {
                throw new BusinessException("发送邮件失败，未查询到收件人相关信息");
            }
            var attachment = await CreateAttachment(orderNo);
            var mail= new MailDto
            {
                PrimaryId=orderNo,
                ToReceiver = supplierContact.Select(s => s.Email).Distinct().ToArray(),
                Subject = "大陆（长沙）工厂物料需求计划",
                Body = "JIT生产需求",
                Attachments=new List<FileInfoDto> { attachment }
            };
            await SendMailToSupplier(mail);
        }

        public async Task SendMailToSupplier(MailDto data)
        {
            FileInfoDto? attachment=null;
            if (data.Attachments != null && data.Attachments.Count > 0)
            {
                attachment= data.Attachments.First();
            }  
            if (data.ToReceiver==null || data.ToReceiver.Length==0)
            {
                throw new BusinessException("发送邮件失败，收件人信息不能为空");
            }
            try
            {
                await _emailService.SendEmail(data.Subject, data.Body, data.ToReceiver, data.ToCC, attachment == null ? "" : attachment.Path);
            } 
            catch (Exception ex)
            {
                _logHelper.LogError("SendMailToSupplier", ex.Message, JsonConvert.SerializeObject(data), ex, ex.StackTrace);
            }
            var mailEntiy = new SysMail();
            mailEntiy.Subject = data.Subject;
            mailEntiy.Body = data.Body;
            mailEntiy.PrimaryId = data.PrimaryId;
            mailEntiy.Sender = $"{_configuration.GetSection("MailConfig:Sender").Value}";
            mailEntiy.AttachmentId = Guid.NewGuid().ToString("N");
            mailEntiy.BusinessType = BusinessType.MaterialRequirementPlan.ToString();
            mailEntiy.CreateDate= DateTime.Now;
            mailEntiy.ToReceiver= string.Join(",", data.ToReceiver);
            if(data.ToCC!=null&& data.ToCC.Length > 0)
            {
                mailEntiy.ToCC=string.Join(",", data.ToCC);
            } 
            Repository.ClientDb.Insertable(mailEntiy).AddQueue();
            if (attachment!= null){
                var attachmentEntity = new BaseFiles
                {
                    PrimaryId = mailEntiy.AttachmentId,
                    FileName = attachment.FileName,
                    FileInfoType = FileInfoType.MaterialRequirementPlanAttachment.ToString(),
                    Path = attachment.Path,
                    Url = attachment.Url
                };
                Repository.ClientDb.Insertable(attachmentEntity).AddQueue();
            } 
            Repository.ClientDb.Updateable<PlanMaterialRequirementOrder>().SetColumns(s => s.IsSendMail == true).Where(w => w.OrderNo == data.PrimaryId).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task<FileInfoDto> CreateAttachment(string orderNo)
        {
            var details = await Repository.ClientDb.Queryable<PlanMaterialRequirementOrder>()
              .InnerJoin<PlanMaterialRequirementDetail>((o, d) => o.OrderNo == d.OrderNo)
              .LeftJoin<BaseGoods>((o, d, g) => g.GoodsId == d.GoodsId)
              .LeftJoin<BaseType>((o, d, g, t) => t.TypeId == g.GoodsClassifyId)
              .LeftJoin<BaseSuppliers>((o, d, g, t,s)=>s.SupplierId==o.SupplierId)
              .Where((o, d, g, t,s) => o.OrderNo == orderNo)
              .OrderBy((o, d, g, t,s) => new { d.ReqDate,d.Shift})
              .Select((o, d, g, t, s) => new
              {
                  o.Year,
                  o.Week,
                  o.Version,
                  o.SupplierId,
                  s.SupplierName,
                  o.GoodsClassifyGroup,
                  g.GoodsNo,
                  g.GoodsName,
                  t.TypeName,
                  d.Quantity,
                  d.UnitName,
                  d.RequirementLevel,
                  d.Shift,
                  d.ReqDate,
                  o.Remark
              }).ToListAsync();
            if (details == null || details.Count == 0)
            {
                throw new BusinessException("生成附件失败，未查询到相关计划明细或已被删除");
            }
            var fields = GetExportFields().Where(w => !w.IsIgnoreImport).ToList();
            var tb = new DataTable();
            fields.ForEach(f => tb.Columns.Add(f.Value, typeof(string)));
            foreach (var item in details)
            {
                var row = tb.NewRow();
                foreach (var field in fields)
                { 
                    if (field.Key == "GoodsClassifyGroup")
                    {
                        row[field.Value] = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(item.GoodsClassifyGroup);
                    }
                    else if (field.Key == "RequirementLevel")
                    {
                        row[field.Value] = EnumHelper.GetDescFromEnumVal<MaterialRequirementLevel>(item.RequirementLevel);
                    }
                    else
                    {
                        row[field.Value] = item.GetType().GetProperty(field.Key).GetValue(item);
                    }
                }
                tb.Rows.Add(row);
            } 
            var stream = ExcelHelper.ConvertDataTableToStream(tb);
            var order = details.First();
            var goodsGroupDesc = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(order.GoodsClassifyGroup);
            var fileName = $"大陆长沙工厂{goodsGroupDesc}需求计划{order.Year}-CW{order.Week}-V{order.Version}({order.SupplierName}).xlsx";
            return await _fileStorage.GetFileAndSave(fileName, stream, FileType.Excel); 
        } 

        public void SetMaterialRequirementExceed()
        {
            var curYear = DateTime.Now.Year;
            var curWeek = new GregorianCalendar().GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            Repository.ClientDb.Updateable<PlanMaterialRequirementOrder>()
                .SetColumns(s => s.Status == PlanMaterialRequirementStatus.Exceed.ToString())
                .Where(w => w.Year == curYear && w.Week <= curWeek && w.Status == PlanMaterialRequirementStatus.Created.ToString())
               .ExecuteCommand(); 
        }

        private static bool _isNextWeek = true;

        public void GetFinishedProductOrdersFormPMS()
        {
            var porvider = "";
            var secret = "";
            var host = "";
            var sign = _configuration.GetSection("Factory:sign").Value;
            if (sign=="dl") 
            {
                 porvider = _configuration.GetSection("PMS:Porvider").Value;
                 secret = _configuration.GetSection("PMS:Secret").Value;
                 host = _configuration.GetSection("PMS:Host").Value;
            }
            else
            if (sign == "w3")
            {
                porvider = _configuration.GetSection("UserPlatformW3:Porvider").Value;
                secret = _configuration.GetSection("UserPlatformW3:Secret").Value;
                host = _configuration.GetSection("UserPlatformW3:Host").Value;
            }
            
            var token = GetAccessToken(host, porvider, secret);
            if (string.IsNullOrEmpty(token))
            {
                _logHelper.LogInfo("GetFinishedProductOrdersFormPMS", "没有获取到Token", "");
                return;
            }
            var curDate=DateTime.Now;
            var curYear = curDate.Year; 
            var week = new GregorianCalendar().GetWeekOfYear(curDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int totalWeeksInYear = _getTotalWeeksInYear(curDate.Year);
            if (_isNextWeek)
            {
                week= new GregorianCalendar().GetWeekOfYear(curDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday) + 1;
                if (week > totalWeeksInYear)
                {
                    week = 1;
                    curYear += 1;
                } 
            }
            _isNextWeek = !_isNextWeek;
            var url =  $"{host}/api/ProductionOrderApi/getProductionPlanByYearAndWeek?year={curYear}&weekly=CW{week}";
            var header = new Dictionary<string, string>
            {
                { "authorization", token }
            };
            try
            {
                var res = HttpHelper.RequestGet<ExternalResponseDto>(url, header);
                if (res != null && res.data != null)
                {
                    _logHelper.LogInfo("GetFinishedProductOrdersFormPMS", $"获取到PMS成品生产计划", $"{curYear}-{week}");
                    var dataJson = JsonConvert.SerializeObject(res.data);
                    var dataList = JsonConvert.DeserializeObject<List<PMSProductOrderResponseDto>>(dataJson);
                    if (dataList != null && dataList.Count > 0)
                    {
                        var curVersion = dataList.Select(s => s.version).First();
                        var verRound=Math.Round(curVersion,2);
                        if (dataList != null && dataList.Count > 0)
                        {
                            var isExistsCurWeekData =  Repository.ClientDb.Queryable<PlanFinishedProductOrder>().Where(w => w.Year == curYear && w.Week == week && SqlFunc.Round(w.Version,2) == verRound).Any(); 
                            if (!isExistsCurWeekData)
                            {
                                var planOrderEnity = _planOrderMapping(dataList, curYear, week);
                                var materialReq = _getMaterialsReqByProduct(planOrderEnity); 
                                Repository.ClientDb.Insertable(planOrderEnity).AddQueue();
                                var reqOrderList = new List<PlanMaterialRequirementOrder>();
                                if (materialReq.Count > 0)
                                {
                                    //物料的供应商取自成品的供应商，同时需要根据供应商分组
                                    var productsId = planOrderEnity.Select(s => s.ConsignNum).Distinct().ToList();
                                    var suppliers =  Repository.ClientDb.Queryable<BaseGoods>()
                                        .InnerJoin<BaseSuppliers>((g, s) => g.Supplier == s.SupplierName)
                                        .Where((g, s) => productsId.Contains(g.GoodsNo))
                                        .Select((g, s) => new {g.GoodsNo, s.SupplierId }).ToList();
                                    if (suppliers.Count == 0)
                                    {
                                        _logHelper.LogInfo("GetFinishedProductOrdersFormPMS", "获取物料需求计划失败，系统未查询到成品供应商",string.Join(',', productsId));
                                        return; 
                                    }
                                    var planOrder = planOrderEnity.First();
                                    var supplierGroup = suppliers.Select(s => s.SupplierId).Distinct().ToList(); 
                                    var reqDetailList=new List<PlanMaterialRequirementDetail>();
                                    foreach(var suplierId in supplierGroup)
                                    {
                                        var reqOrder = new PlanMaterialRequirementOrder
                                        {
                                            OrderNo = Guid.NewGuid().ToString("N").ToUpper(),
                                            Status = PlanMaterialRequirementStatus.Created.ToString(),
                                            PlanId = planOrder.PlanId,
                                            SupplierId= suplierId,
                                            Year = curYear,
                                            Week = week,
                                            Version = curVersion,
                                            GoodsClassifyGroup = BaseTypeGroup.PackingMaterial.ToString(),
                                            CreateDate = curDate,
                                            CreateUserName = "Sys",
                                            Remark = $"该需求计划来自{curYear}-CW{week}-V{planOrder.Version}版本成品生产计划清单"
                                        };
                                        var productBySupplier = suppliers.Where(w => w.SupplierId == reqOrder.SupplierId).Select(s => s.GoodsNo).Distinct().ToList();
                                        var reqDetail = materialReq.Where(w => productBySupplier.Contains(w.ProductNo)).ToList();
                                        var reqDetailGroup = reqDetail.GroupBy(f => new
                                        { 
                                            f.GoodsClassifyGroup,
                                            f.GoodsId,
                                            f.GoodsNo,
                                            f.GoodsName,
                                            f.UnitName,
                                            f.RequirementLevel,
                                            f.Shift,
                                            f.ReqDate,
                                            f.Status
                                        }).Select(f => new PlanMaterialRequirementDetail
                                        { 
                                            GoodsClassifyGroup = f.Key.GoodsClassifyGroup,
                                            GoodsId = f.Key.GoodsId,
                                            GoodsNo = f.Key.GoodsNo,
                                            GoodsName = f.Key.GoodsName,
                                            Quantity = f.Sum(sum => sum.Quantity),
                                            UnitName = f.Key.UnitName,
                                            RequirementLevel = f.Key.RequirementLevel,
                                            Shift = f.Key.Shift,
                                            ReqDate = f.Key.ReqDate,
                                            Status = f.Key.Status
                                        }).ToList();
                                        reqDetailGroup.ForEach(f => f.OrderNo = reqOrder.OrderNo);
                                        reqDetailList.AddRange(reqDetailGroup);
                                        reqOrderList.Add(reqOrder);
                                    } 
                                    Repository.ClientDb.Insertable(reqDetailList).AddQueue();
                                    Repository.ClientDb.Insertable(reqOrderList).AddQueue();
                                }
                                Repository.ClientDb.SaveQueues();
                                //自动发送邮件到供应商
                                bool isAutoSendMail = false;
                                var args =  _sysArgsService.GetValueByKey(BusinessConst.IsAutoSendMailByMaterialRequirement).Result;
                                bool.TryParse(args.Value.ToString(), out isAutoSendMail);
                                if (isAutoSendMail)
                                {
                                    foreach (var order in reqOrderList)
                                    {
                                         CreateMailToSend(order.OrderNo, order.SupplierId).Wait();
                                    }
                                }
                                //如果当周有其他版本计划，则设置为废弃
                                Repository.ClientDb.Updateable<PlanMaterialRequirementOrder>().SetColumns(s => s.Status == PlanMaterialRequirementStatus.Obsolete.ToString())
                                    .Where(w => w.Year == curYear && w.Week == week && SqlFunc.Round(w.Version, 2) < verRound).ExecuteCommand();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _logHelper.LogError("GetFinishedProductOrdersFormPMS", ex.Message, $"{curYear}-{week}", ex, ex.StackTrace);
            }
        }

        private  int _getTotalWeeksInYear(int year)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            DateTime endDate = new DateTime(year, 12, 31);
            Calendar cal = dfi.Calendar;
            return cal.GetWeekOfYear(endDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        private List<PlanFinishedProductOrder> _planOrderMapping(List<PMSProductOrderResponseDto> res,int year,int week)
        {
            var data = new List<PlanFinishedProductOrder>();
            res.ForEach(f =>
            {
                var order = new PlanFinishedProductOrder()
                {
                    PlanId = f.id,
                    PlanName = f.planName,
                    Version = f.version,
                    Year = year,
                    Week = week,
                    ConsignNum = f.salesPn,
                    ProdctionTypeNo = f.pn,
                    QuantityTotal = string.IsNullOrEmpty(f.qty) ? 0 : float.Parse(f.qty),
                    UnitName = "pcs",
                    DateOfMon = res.Max(m => m.monDate),
                    QuantityDayOfMon = string.IsNullOrEmpty(f.monDay) ? 0 : float.Parse(f.monDay),
                    QuantityNightOfMon = string.IsNullOrEmpty(f.monNight) ? 0 : int.Parse(f.monNight),
                    DateOfTues = res.Max(m => m.tuesDate),
                    QuantityDayOfTues = string.IsNullOrEmpty(f.tuesDay) ? 0 : int.Parse(f.tuesDay),
                    QuantityNightOfTues = string.IsNullOrEmpty(f.tuesNight) ? 0 : int.Parse(f.tuesNight),
                    DateOfWed = res.Max(m => m.wedsDate),
                    QuantityDayOfWed = string.IsNullOrEmpty(f.wedsDay) ? 0 : int.Parse(f.wedsDay),
                    QuantityNightOfWed = string.IsNullOrEmpty(f.wedsNight) ? 0 : int.Parse(f.wedsNight),
                    DateOfThur = res.Max(m => m.thursDate),
                    QuantityDayOfThur = string.IsNullOrEmpty(f.thursDay) ? 0 : int.Parse(f.thursDay),
                    QuantityNightOfThur = string.IsNullOrEmpty(f.thursNight) ? 0 : int.Parse(f.thursNight),
                    DateOfFri = res.Max(m => f.friDate),
                    QuantityDayOfFri = string.IsNullOrEmpty(f.friDay) ? 0 : int.Parse(f.friDay),
                    QuantityNightOfFri = string.IsNullOrEmpty(f.friNight) ? 0 : int.Parse(f.friNight),
                    DateOfSat = res.Max(m => m.satDate),
                    QuantityDayOfSat = string.IsNullOrEmpty(f.satDay) ? 0 : int.Parse(f.satDay),
                    QuantityNightOfSat = string.IsNullOrEmpty(f.satNight) ? 0 : int.Parse(f.satNight),
                    DateOfSun = res.Max(m => m.sunDate),
                    QuantityDayOfSun = string.IsNullOrEmpty(f.sunDay) ? 0 : int.Parse(f.sunDay),
                    QuantityNightOfSun = string.IsNullOrEmpty(f.sunNight) ? 0 : int.Parse(f.sunNight),
                    CreateDate = DateTime.Now
                };
                order.QuantityTotal = order.QuantityDayOfMon + order.QuantityNightOfMon + order.QuantityDayOfTues + order.QuantityNightOfTues 
                + order.QuantityDayOfWed + order.QuantityNightOfWed + order.QuantityDayOfThur + order.QuantityNightOfThur 
                + order.QuantityDayOfFri + order.QuantityNightOfFri + order.QuantityDayOfSat + order.QuantityNightOfSat 
                + order.QuantityDayOfSun + order.QuantityNightOfSun;
                data.Add(order);
            });
            return data;
        }

        private List<PlanMaterialRequirementDetail> _getMaterialsReqByProduct(List<PlanFinishedProductOrder> orderList)
        {
            var productNoArr = orderList.Select(s => s.ConsignNum).Distinct().ToList();
            var materialInfo = Repository.ClientDb.Queryable<BaseGoods>()
                .InnerJoin<BaseType>((g, t) => g.GoodsClassifyId == t.TypeId)
                .InnerJoin<BaseBOM>((g,t,b)=>b.ParentId==g.GoodsId) 
                .Where((g, t,b) => t.Group == BaseTypeGroup.FinishedProduct.ToString() && productNoArr.Contains(g.GoodsNo)  && b.MaterialClassifyGroup== BaseTypeGroup.PackingMaterial.ToString())
                .Select((g, t, b) =>new
                { 
                    g.GoodsId,
                    g.GoodsNo,
                    g.PackageCount,
                    b.MaterialId,
                    b.MaterialName,
                    b.Quantity,
                    b.Unit,
                    b.MaterialClassifyGroup
                }).ToList();
            //(成品计划生产数量数量/成品每包装数量)*对应物料需求数量 
            var reqList = new List<PlanMaterialRequirementDetail>();
            foreach (var order in orderList)
            {
                var curBomList = materialInfo.Where(x => x.GoodsNo == order.ConsignNum).ToList();
                if (curBomList.Count>0)
                { 
                    if (order.QuantityDayOfMon>0)
                    { 
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {  
                                GoodsClassifyGroup= material.MaterialClassifyGroup,
                                GoodsId=material.MaterialId,
                                GoodsName=material.MaterialName,
                                ProductNo=order.ConsignNum,
                                Quantity= (float)Math.Ceiling((order.QuantityDayOfMon / material.PackageCount) * material.Quantity),
                                ReqDate = DateTime.Parse(order.DateOfMon),
                                UnitName =material.Unit, 
                                RequirementLevel= MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Day.ToString(), 
                                Status= PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        } 
                    }
                    if (order.QuantityNightOfMon > 0)
                    {   
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityNightOfMon / material.PackageCount) * material.Quantity),
                                ReqDate = DateTime.Parse(order.DateOfMon),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Night.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                    if (order.QuantityDayOfTues > 0)
                    {   
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {  
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityDayOfTues / material.PackageCount) * material.Quantity) ,
                                ReqDate = DateTime.Parse(order.DateOfTues),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Day.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                    if ( order.QuantityNightOfTues > 0)
                    {   
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityNightOfTues / material.PackageCount) * material.Quantity) ,
                                ReqDate = DateTime.Parse(order.DateOfTues),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Night.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                    if (order.QuantityDayOfWed > 0)
                    {   
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {  
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityDayOfWed / material.PackageCount) * material.Quantity) ,
                                ReqDate = DateTime.Parse(order.DateOfWed),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Day.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                    if (order.QuantityNightOfWed > 0)
                    {   
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityNightOfWed / material.PackageCount) * material.Quantity) ,
                                ReqDate = DateTime.Parse(order.DateOfWed),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Night.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                    if (order.QuantityDayOfThur > 0)
                    {   
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {  
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityDayOfThur / material.PackageCount) * material.Quantity) ,
                                ReqDate = DateTime.Parse(order.DateOfThur),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Day.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                    if (order.QuantityNightOfThur > 0)
                    {   
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityNightOfThur / material.PackageCount) * material.Quantity) ,
                                ReqDate = DateTime.Parse(order.DateOfThur),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Night.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                    if (order.QuantityDayOfFri > 0)
                    {   
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {  
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityDayOfFri / material.PackageCount) * material.Quantity) ,
                                ReqDate = DateTime.Parse(order.DateOfFri),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Day.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                    if (order.QuantityNightOfFri > 0)
                    {   
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityNightOfFri / material.PackageCount) * material.Quantity) ,
                                ReqDate = DateTime.Parse(order.DateOfFri),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Night.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                    if (order.QuantityDayOfSat > 0)
                    {   
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {  
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityDayOfSat / material.PackageCount) * material.Quantity) ,
                                ReqDate = DateTime.Parse(order.DateOfSat),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Day.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                    if (order.QuantityNightOfSat > 0)
                    {   
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityNightOfSat / material.PackageCount) * material.Quantity) ,
                                ReqDate = DateTime.Parse(order.DateOfSat),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Night.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                    if (order.QuantityDayOfSun > 0 )
                    {   
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {  
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityDayOfSun / material.PackageCount) * material.Quantity) ,
                                ReqDate = DateTime.Parse(order.DateOfSun),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Day.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                    if ( order.QuantityNightOfSun > 0)
                    {  
                        foreach (var material in curBomList)
                        {
                            var req = new PlanMaterialRequirementDetail
                            {
                                GoodsClassifyGroup = material.MaterialClassifyGroup,
                                GoodsId = material.MaterialId,
                                GoodsName = material.MaterialName,
                                ProductNo = order.ConsignNum,
                                Quantity = (float)Math.Ceiling((order.QuantityNightOfSun / material.PackageCount) * material.Quantity),
                                ReqDate = DateTime.Parse(order.DateOfSun),
                                UnitName = material.Unit,
                                RequirementLevel = MaterialRequirementLevel.Common.ToString(),
                                Shift = ProdShift.Night.ToString(),
                                Status = PlanMaterialRequirementStatus.Created.ToString()
                            };
                            reqList.Add(req);
                        }
                    }
                }
            }
            return reqList;
        } 
    }
}
