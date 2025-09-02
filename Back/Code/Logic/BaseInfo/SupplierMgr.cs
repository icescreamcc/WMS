using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using External.Common.Extension;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Models.Model;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.BaseInfo
{
   public class SupplierMgr: DataPermissionHandler
    {
        private readonly IFileStorage _fileStorage;

        private IMapper _mapper;

        public SupplierMgr(Repository repository, IFileStorage fileStorage,IMapper mapper) : base(repository)
        {
            _fileStorage = fileStorage;
            _mapper = mapper;
        }

        /// <summary>
        /// 供应商分页查询
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task<TableModel<SupplierDto>> GetSuppliers(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "SupplierName" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<BaseSuppliers>()
                  .LeftJoin<SysArgsOptions>((c, a1) => c.SupplierTypeId == a1.OptionId)
                  .LeftJoin<SysArgsOptions>((c,a1, a2) => c.SupplierPropertyId == a2.OptionId)
                  .Where(c => c.IsDeleted==false)
                  .Where((c,a1,a2) => c.SupplierId.Contains(searchKey) || c.SupplierNo.Contains(searchKey) || c.SupplierName.Contains(searchKey)  || c.SupplierTypeName.Contains(searchKey)
                  || c.Address.Contains(searchKey) || c.Province.Contains(searchKey) || c.City.Contains(searchKey) || c.SupplierPropertyName.Contains(searchKey)
                  || SqlFunc.Subqueryable<BaseSupplierContact>().Where(sc=>sc.SupplierId==c.SupplierId&&(sc.Email.Contains(searchKey)||sc.Telephone.Contains(searchKey))).Any())
                  .Select((c, a1,a2) => new SupplierDto
                  {
                      SupplierId = c.SupplierId,
                      SupplierNo = c.SupplierNo,
                      SupplierName = c.SupplierName,
                      SupplierTypeId = c.SupplierTypeId,
                      SupplierTypeName = a1.OptionName, 
                      SupplierPropertyId = c.SupplierPropertyId,
                      SupplierPropertyName=a2.OptionName,
                      Address = c.Address,
                      Province = c.Province,
                      City = c.City, 
                      IsImportant = c.IsImportant, 
                      Remark = c.Remark,
                      Consignee = c.Consignee,
                      ConsigneeTel = c.ConsigneeTel,
                      ConsigneeAddress = c.ConsigneeAddress,
                      CreateDate = c.CreateDate,
                      CreateUser = c.CreateUser,
                      SpareField1 = c.SpareField1,
                      SpareField2 = c.SpareField2,
                      SpareField3 = c.SpareField3,
                      SpareField4 = c.SpareField4,
                      SpareField5 = c.SpareField5
                  })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            await SetDeniedFieldValue<BaseSuppliers, SupplierDto>(userId, data);
            var res = new TableModel<SupplierDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 获取供应商明细
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<SupplierDto> GetSupplierDetail(string supplierId)
        {
            var data= await Repository.ClientDb.Queryable<BaseSuppliers>()
                .LeftJoin<SysArgsOptions>((c, a1) => c.SupplierTypeId == a1.OptionId)
                .LeftJoin<SysArgsOptions>((c, a1, a2) => c.SupplierPropertyId == a2.OptionId)
                .Where((c, a1, a2) => c.SupplierId == supplierId)
                .Select((c, a1, a2) => new SupplierDto
                {
                    SupplierId = c.SupplierId,
                    SupplierNo = c.SupplierNo,
                    SupplierName = c.SupplierName,
                    SupplierTypeId = c.SupplierTypeId,
                    SupplierTypeName = a1.OptionName, 
                    SupplierPropertyId = c.SupplierPropertyId,
                    SupplierPropertyName=a2.OptionName,
                    Address = c.Address,
                    Province = c.Province,
                    City = c.City, 
                    IsImportant = c.IsImportant, 
                    Remark = c.Remark,
                    Consignee = c.Consignee,
                    ConsigneeTel = c.ConsigneeTel,
                    ConsigneeAddress = c.ConsigneeAddress,
                    CreateDate = c.CreateDate,
                    CreateUser = c.CreateUser,
                    SpareField1 = c.SpareField1,
                    SpareField2 = c.SpareField2,
                    SpareField3 = c.SpareField3,
                    SpareField4 = c.SpareField4,
                    SpareField5 = c.SpareField5
                }).SingleAsync();
            data.AccountDetails = await Repository.ClientDb.Queryable<BaseSupplierAccountCredited>()
                 .LeftJoin<SysArgsOptions>((s, a) => s.AccountTypeId == a.OptionId)
                .Where((s, a) => s.SupplierId == supplierId)
                .Select((s, a) => new SupplierAccountCreditedDto
                {
                    AccountId = s.AccountId,
                    AccountName = s.AccountName,
                    AccountNumber = s.AccountNumber,
                    AccountTypeId = s.AccountTypeId,
                    AccountTypeName = a.OptionName,
                    IsCommonAccount = s.IsCommonAccount,
                    OpeningBank = s.OpeningBank,
                    Remark = s.Remark
                }).ToListAsync();
            data.ContactDetails = await Repository.ClientDb.Queryable<BaseSupplierContact>().Where(w => w.SupplierId == supplierId).Select<SupplierContactDto>().ToListAsync(); 
            return data;
        }

        /// <summary>
        /// 根据供应商类型获取供应商
        /// </summary>
        /// <param name="supplierTypeId"></param>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetSupplierBySupplierTypeId(int supplierTypeId)
        {
            return await Repository.ClientDb.Queryable<BaseSuppliers>()
                .Where(w => w.SupplierTypeId ==supplierTypeId)
                .Select(s => new KeyValueModel { Key = s.SupplierId, Value = s.SupplierName }).ToListAsync();
        }

        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddSupplier(SupplierDto data)
        {
            var exist = await Repository.Exist<BaseSuppliers>(u => u.SupplierName == data.SupplierName && !u.IsDeleted);
            if (exist)
            {
                throw new BusinessException("保存失败,当前供应商名称已存在");
            }
            var model = _mapper.Map<BaseSuppliers>(data);
            var lastData = await Repository.ClientDb.Queryable<BaseSuppliers>().MaxAsync(x => x.SupplierId);
            model.SupplierId = GetPrimaryId("S", lastData);
            model.CreateDate = DateTime.Now.ToStringExtension();
            model.IsValid = true; 
            Repository.ClientDb.Insertable(model).AddQueue();
            if (data.AccountDetails.Count > 0)
            {
                var creditDetail = _mapper.Map<List<BaseSupplierAccountCredited>>(data.AccountDetails);
                creditDetail.ForEach(x => x.SupplierId = model.SupplierId);
                Repository.ClientDb.Insertable(creditDetail).AddQueue();
            }
            if(data.ContactDetails.Count > 0)
            {
                var contactDetail = _mapper.Map<List<BaseSupplierContact>>(data.ContactDetails);
                contactDetail.ForEach(x => x.SupplierId = model.SupplierId);
                Repository.ClientDb.Insertable(contactDetail).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync();
        }

        /// <summary>
        /// 修改供应商
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateSupplier(SupplierDto data)
        {
            var exist = await Repository.Exist<BaseSuppliers>(u => u.SupplierId == data.SupplierId);
            if (!exist)
            {
                throw new BusinessException("保存失败,当前供应商ID不存在或已删除"); 
            }
            exist = await Repository.Exist<BaseSuppliers>(u => u.SupplierId != data.SupplierId&& u.SupplierName == data.SupplierName && !u.IsDeleted);
            if (exist)
            {
                throw new BusinessException("保存失败,当前供应商名称已存在"); 
            }
            var model = _mapper.Map<BaseSuppliers>(data);
            Repository.ClientDb.Updateable(model).AddQueue();
            Repository.ClientDb.Deleteable<BaseSupplierAccountCredited>(x => x.SupplierId == model.SupplierId).AddQueue();
            model.IsValid= true;
            if (data.AccountDetails.Count > 0)
            { 
                var creditDetail = _mapper.Map<List<BaseSupplierAccountCredited>>(data.AccountDetails);
                creditDetail.ForEach(x => x.SupplierId = model.SupplierId);
                Repository.ClientDb.Insertable(creditDetail).AddQueue();
            }
            Repository.ClientDb.Deleteable<BaseSupplierContact>(x => x.SupplierId == model.SupplierId).AddQueue();
            if (data.ContactDetails.Count > 0)
            { 
                var contactDetail = _mapper.Map<List<BaseSupplierContact>>(data.ContactDetails);
                contactDetail.ForEach(x => x.SupplierId = model.SupplierId);
                Repository.ClientDb.Insertable(contactDetail).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync();
        }

        /// <summary>
        /// 删除供应商
        /// </summary>
        /// <param name="suppliersId"></param>
        /// <returns></returns>
        public async Task DelSupplier(string[] suppliersId)
        {
            var models = await Repository.ClientDb.Queryable<BaseSuppliers>().Where(x => suppliersId.Contains(x.SupplierId)).ToListAsync();
            models.ForEach(x => x.IsDeleted = true);
            var res = await Repository.UpdateAsync(models); 
        }

        /// <summary>
        /// 导出供应商
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public async Task<string> ExportSuppliers(string searchKey, string orderField, string orderType, List<KeyValueModel> fields)
        {
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            var typeList = new List<Type> { typeof(BaseSuppliers) };
            StringBuilder sb = new StringBuilder();
            fields.ForEach(f =>
            {
                string fieldStr = SqlJoint.GetSelectFieldFormat(f, typeList);
                sb.Append(fieldStr);
            });
            string selField = sb.ToString().TrimEnd(',');
            typeList.ToList().Reverse();
            string orderbyStr = SqlJoint.GetOrderFieldFormat(orderField, orderType, typeList);
            var param = new Dictionary<string, object>
            {
                { "SupplierId", searchKey },
                { "SupplierNo", searchKey },
                { "SupplierName", searchKey },
                { "SupplierTypeName", searchKey },
                { "SupplierLevel", searchKey },
                { "Province", searchKey },
                { "City", searchKey },
                { "Email", searchKey },
                { "Telephone", searchKey },
                { "Address", searchKey }
            };
            string sql = $@"select {selField} from BaseSuppliers    
                            where (SupplierId like @SupplierId or SupplierNo like @SupplierNo
                            or SupplierName like @SupplierName or SupplierTypeName like @SupplierTypeName or Address like @Address
                            or Province like @Province or City like @City
                            or SupplierId in (select SupplierId from BaseSupplierContact where Email like  @Email or Telephone like @Telephone) )
                            and IsValid=1 and IsDeleted=0 
                            {orderbyStr}";
            var queryData = await Repository.QueryBySqlAsync(sql, param);
            var stream = ExcelHelper.ConvertDataTableToStream(queryData);
            var fileName = $"供应商信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
            var savePath = @$"{Directory.GetCurrentDirectory()}\Files\Export\{fileName}";
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
            return fileUrl;
        }
    }
}
