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
using Models.Model.Order;
using Models.Model.Sys;
using SqlSugar;
using System.Data;
using System.Globalization;
using System.Text;

//namespace Logic.Purchase
namespace Logic.Order
{
    public class OrderPlanMgr : ApprovalHandler
    { 
  
        private readonly IMapper _mapper;

        private readonly IFileStorage _fileStorage;

        private readonly IConfiguration _configuration;

        private readonly HttpHelperAsync _httpHelper;

        private readonly LogHelper _logHelper;

        public OrderPlanMgr(Repository repository, LogHelper logHelper, IMapper mapper, IFileStorage fileStorage, IConfiguration configuration, HttpHelperAsync httpHelper) : base(repository)
        {
            _logHelper = logHelper;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _configuration = configuration;
            _httpHelper = httpHelper;

        }
        /// <summary>
        /// 
        /// </summary>

        public async Task<TableModel<OrderPlanDto>> GetOrder(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "CreateDate" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();


            // 构建查询条件
            var data = Repository.ClientDb.Queryable<OrderPlan>()
                      .LeftJoin<BaseUnits>((m,b) => m.Unit == b.UnitId.ToString())
                      .LeftJoin<BaseGoods>((m, b,g) => m.GoodsName == g.GoodsId)
                      .LeftJoin<BaseSuppliers>((m, b, g,u) => m.CustomerName == u.SupplierId.ToString())
                      .Where((m, b, g,u) => m.Unit.Contains(searchKey) || m.OrderAmount.Contains(searchKey))
                      .Select((m, b, g,u) => new OrderPlanDto
                      {
                          OrderNo = m.OrderNo,
                          ContractNo = m.ContractNo,
                          SigningDate = m.SigningDate,
                          CustomerName = m.CustomerName,
                          CustomerNames = u.SupplierName,
                          GoodsName = m.GoodsName,
                          GoodsNames = g.GoodsName,
                          OrderNum = m.OrderNum,
                          Unit = m.Unit,
                          UnitName=b.UnitName,
                          OrderAmount = m.OrderAmount,
                          DeliveryDate = m.DeliveryDate,
                          Remarks = m.Remarks,
                          ShippedNum = m.ShippedNum,
                          Belial = m.Belial,
                          Invoice = m.Invoice,
                          PaymentState = m.PaymentState,
                          //OrderState ="新建",
                          OrderState = m.OrderState,
                          OrderUrl = m.OrderUrl,
                          CreateDate = m.CreateDate,
                          ModifyDate = m.ModifyDate,
                      })
                       .OrderBy($"{orderFiled} {orderType}")
                      .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<OrderPlanDto>() { Total = total, Rows = data};
            return await Task.FromResult(res);
        }

        public async Task<List<KeyValueModel>> GetUnits()
        {
            var res = await Repository.ClientDb.Queryable<BaseUnits>()
                .Where(c => c.IsValid && c.UnitType == "Pack")
                .Select(c => new KeyValueModel { Key = c.UnitId, Value = c.UnitName, Remark = c.UnitNo, Type = c.UnitType })
                .ToListAsync();
            return res.OrderBy(x => x.Remark).ToList();
        }

        public async Task<List<KeyValueModel>> GetSupplier()
        {
            var res = await Repository.ClientDb.Queryable<BaseSuppliers>()
                .Where(c => c.IsValid)
                .Select(c => new KeyValueModel { Key = c.SupplierId, Value = c.SupplierTypeName, Remark = c.SupplierPropertyId, Type = c.Address })
                .ToListAsync();
            return res.OrderBy(x => x.Remark).ToList();
        }
        public async Task<List<KeyValueModel>> GetBaseGoods()
        {
            var res = await Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, a2) => a2.TypeId == g.GoodsClassifyId )
                .Where((g, a2) =>  g.IsDeleted==false && a2.Group == BaseTypeGroup.FinishedProduct.ToString())
                .Select((g, a2) => new KeyValueModel { Key = g.GoodsId, Value = g.GoodsName, Remark = g.Remark, Type = a2.Group })
                .ToListAsync();
            return res.OrderBy(x => x.Remark).ToList();
        }


        /// <summary>
        /// 添加订单计划
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task AddOrderPlan(OrderPlanDto data)
        {
            //var exist = await Repository.Exist<OrderPlanDto>(u => u.OrderNo == data.OrderNo);
            //if (exist)
            //{
            //    throw new BusinessException("保存失败,当前任务单已存在!");
            //}

            //string sId = GetIds();//时间生产ID

            var lastData = await Repository.ClientDb.Queryable<OrderPlan>().MaxAsync(x => x.OrderNo);
            string sId= GetPrimaryId("F", lastData);
            var model = new OrderPlan
            {
                OrderNo = sId,
                ContractNo = data.ContractNo,
                SigningDate = data.SigningDate,
                CustomerName = data.CustomerName,
                GoodsName = data.GoodsName,
                OrderNum = data.OrderNum,
                Unit = data.Unit,
                OrderAmount = data.OrderAmount,
                DeliveryDate = data.DeliveryDate,
                Remarks = data.Remarks,
                ShippedNum = data.ShippedNum,
                Belial = data.Belial,
                Invoice = data.Invoice,
                //PaymentState = data.PaymentState,
                //OrderState = data.OrderState,
                PaymentState = "未回款",
                OrderState = "进行中",
                OrderUrl = data.OrderUrl,
                CreateDate = DateTime.Now.ToStringExtension()
            };
            await Repository.AddAsync(model);

        }

        /// <summary>
        /// 修改订单计划
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateOrderPlan(OrderPlanDto data)
        {
            var exist = await Repository.Exist<OrderPlan>(u => u.OrderNo == data.OrderNo);
            if (!exist)
            {
                throw new BusinessException("保存失败,当前任务单不存在或已删除!");
            }
            string orderState = "已关闭";
            if (data.PaymentState== "未回款")
            {
                orderState = "进行中";
            }
            var model = new OrderPlan
            {
                OrderNo = data.OrderNo,
                ContractNo = data.ContractNo,
                SigningDate = data.SigningDate,
                CustomerName = data.CustomerName,
                GoodsName = data.GoodsName,
                OrderNum = data.OrderNum,
                Unit=data.Unit,
                OrderAmount = data.OrderAmount,
                DeliveryDate = data.DeliveryDate,
                Remarks = data.Remarks,
                ShippedNum = data.ShippedNum,
                Belial=data.Belial,
                Invoice=data.Invoice,
                PaymentState = data.PaymentState,
                OrderState = orderState,
                OrderUrl = data.OrderUrl,
                CreateDate=data.CreateDate,
                ModifyDate = DateTime.Now.ToStringExtension(),
                //IsValid = true,
            };
            await Repository.UpdateAsync(model);
        }
        /// <summary>
        /// 删除订单计划
        /// </summary>
        /// <param name="unitsId"></param>
        /// <returns></returns>
        public async Task DelOrderPlan(string[] Ids)
        {
            Repository.ClientDb.Deleteable<OrderPlan>(b => Ids.Contains(b.OrderNo)).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
            
        }
        public async Task<string> UploadSparePartPhoto(string fileName, Stream stream)
        {
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Image);
            return fileUrl;
        }

        public async Task<string> UploadSparePartPdf(string fileName, Stream stream)
        {
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.PDF);
            return fileUrl;
        }

        public async Task<string> UploadFiles(string fileName, string fileType, Stream stream)
        {
            DataTable dt = new DataTable();
            var fileUrl = string.Empty;
            MemoryStream fs = new MemoryStream();
            stream.CopyTo(fs);
            stream.Position = 0;
            fs.Seek(0, SeekOrigin.Begin);

            if (fileType == ".pdf")
            {
                return await _fileStorage.SaveFile(fileName, fs, FileType.PDF);
            }
            else if (fileType == ".xlsx" || fileType == ".xls")
            {
                return await _fileStorage.SaveFile(fileName, fs, FileType.Excel);
            }
            else if (fileType == ".doc" || fileType == ".docx")
            {
                return await _fileStorage.SaveFile(fileName, fs, FileType.WORD);
            }
            else if (fileType == ".mp4" || fileType == ".MP4")
            {
                return await _fileStorage.SaveFile(fileName, fs, FileType.MP4);
            }

            return null;
        }


        public string GetIds()
        {
            Guid id = Guid.NewGuid();
            return id.ToString();
        }
    }
}
