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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.BaseInfo
{
   public class ClientMgr: DataPermissionHandler
    { 
        private readonly IFileStorage _fileStorage;

        public ClientMgr(Repository repository, IFileStorage fileStorage) : base(repository)
        { 
            _fileStorage = fileStorage;
        }

        /// <summary>
        /// 分页查询客户列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task<TableModel<ClientDetail>> GetClients(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "ClientName" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            var data = Repository.ClientDb.Queryable<BaseClients>()
                  .LeftJoin<SysArgsOptions>((c, a) => c.ClientTypeId == a.OptionId)
                  .Where((c, a) =>c.ClientId.Contains(searchKey)|| c.ClientNo.Contains(searchKey) || c.ClientName.Contains(searchKey) || c.ClientLevel.Contains(searchKey) || c.ClientTypeName.Contains(searchKey)
                  || c.Address.Contains(searchKey) || c.Province.Contains(searchKey) || c.City.Contains(searchKey) || c.Email.Contains(searchKey) || c.Wechat.Contains(searchKey))
                  .Where(c=>c.IsValid)
                  .Select((c, a) => new ClientDetail
                  {
                      ClientId = c.ClientId,
                      ClientNo = c.ClientNo,
                      ClientName = c.ClientName,
                      ClientTypeId = c.ClientTypeId,
                      ClientTypeName = a.OptionName,
                      ClientLevel = c.ClientLevel,
                      ClientProperty = c.ClientProperty,
                      Address = c.Address,
                      Province = c.Province,
                      City = c.City,
                      Email = c.Email,
                      Wangwang = c.Wangwang,
                      Wechat = c.Wechat,
                      Alipay = c.Alipay,
                      CommonContact = c.CommonContact,
                      IsImportant = c.IsImportant,
                      Telephone = c.Telephone,
                      Mobilephone = c.Mobilephone,
                      Remark = c.Remark,
                      Consignee=c.Consignee,
                      ConsigneeTel=c.ConsigneeTel,
                      ConsigneeAddress=c.ConsigneeAddress,
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
            await SetDeniedFieldValue<BaseClients, ClientDetail>(userId, data);
            var res = new TableModel<ClientDetail>() { Total = total, Rows = data};
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 获取客户明细
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<ClientDetail> GetClientDetail(string clientId)
        {
            return await Repository.ClientDb.Queryable<BaseClients>()
                .LeftJoin<SysArgsOptions>((c, a) => c.ClientTypeId == a.OptionId)
                .Where((c, a) => c.ClientId == clientId)
                .Select((c, a) => new ClientDetail
                {
                    ClientId = c.ClientId,
                    ClientNo = c.ClientNo,
                    ClientName = c.ClientName,
                    ClientTypeId = c.ClientTypeId,
                    ClientTypeName = a.OptionName,
                    ClientLevel = c.ClientLevel,
                    ClientProperty = c.ClientProperty,
                    Address = c.Address,
                    Province = c.Province,
                    City = c.City,
                    Email = c.Email,
                    Wangwang = c.Wangwang,
                    Wechat = c.Wechat,
                    Alipay = c.Alipay,
                    CommonContact = c.CommonContact,
                    IsImportant = c.IsImportant,
                    Telephone = c.Telephone,
                    Mobilephone = c.Mobilephone,
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
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddClient(ClientDetail data)
        {
            var lastData = await Repository.ClientDb.Queryable<BaseClients>().MaxAsync(x => x.ClientId);
            var model = new BaseClients
            {
                ClientId = GetPrimaryId("C", lastData),
                ClientNo = data.ClientNo,
                ClientName = data.ClientName,
                ClientTypeId = data.ClientTypeId,
                ClientTypeName = data.ClientTypeName,
                ClientLevel = data.ClientLevel,
                ClientProperty = data.ClientProperty,
                Address = data.Address,
                Province = data.Province,
                City = data.City,
                Email = data.Email,
                Wangwang = data.Wangwang,
                Wechat = data.Wechat,
                Alipay = data.Alipay,
                CommonContact = data.CommonContact,
                IsImportant = data.IsImportant,
                Telephone = data.Telephone,
                Mobilephone = data.Mobilephone,
                Remark = data.Remark,
                Consignee = data.Consignee,
                ConsigneeTel = data.ConsigneeTel,
                ConsigneeAddress = data.ConsigneeAddress,
                CreateDate = DateTime.Now.ToStringExtension(),
                CreateUser = data.CreateUser,
                IsValid = true,
                SpareField1 = data.SpareField1,
                SpareField2 = data.SpareField2,
                SpareField3 = data.SpareField3,
                SpareField4 = data.SpareField4,
                SpareField5 = data.SpareField5
            };
            await Repository.AddAsync(model); 
        }

        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateClient(ClientDetail data)
        {
            var exist = await Repository.Exist<BaseClients>(u => u.ClientId == data.ClientId);
            if (!exist)
            {
                throw new BusinessException("保存失败,当前客户ID不存在或已删除"); 
            }
            var model = new BaseClients
            {
                ClientId = data.ClientId,
                ClientNo = data.ClientNo,
                ClientName = data.ClientName,
                ClientTypeId = data.ClientTypeId,
                ClientTypeName = data.ClientTypeName,
                ClientLevel = data.ClientLevel,
                ClientProperty = data.ClientProperty,
                Address = data.Address,
                Province = data.Province,
                City = data.City,
                Email = data.Email,
                Wangwang = data.Wangwang,
                Wechat = data.Wechat,
                Alipay = data.Alipay,
                CommonContact = data.CommonContact,
                IsImportant = data.IsImportant,
                Telephone = data.Telephone,
                Mobilephone = data.Mobilephone,
                Remark = data.Remark,
                Consignee = data.Consignee,
                ConsigneeTel = data.ConsigneeTel,
                ConsigneeAddress = data.ConsigneeAddress,
                CreateDate = data.CreateDate,
                CreateUser = data.CreateUser,
                IsValid = true,
                SpareField1 = data.SpareField1,
                SpareField2 = data.SpareField2,
                SpareField3 = data.SpareField3,
                SpareField4 = data.SpareField4,
                SpareField5 = data.SpareField5
            };
            await Repository.UpdateAsync(model); 
        }

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="clientsId"></param>
        /// <returns></returns>
        public async Task DelClient(string [] clientsId)
        {
            var models = await Repository.ClientDb.Queryable<BaseClients>().Where(x => clientsId.Contains(x.ClientId)).ToListAsync();
            models.ForEach(x => x.IsValid = false);
            await Repository.UpdateAsync(models); 
        }

        /// <summary>
        /// 导出客户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public async Task<string> ExportClients( string searchKey, string orderField, string orderType, List<KeyValueModel> fields)
        {
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey; 
            var typeList = new List<Type> { typeof(BaseClients) };
            StringBuilder sb = new StringBuilder();
            fields.ForEach(f =>
            {
                string fieldStr = SqlJoint.GetSelectFieldFormat(f, typeList);
                sb.Append(fieldStr);
            });
            string selField = sb.ToString().TrimEnd(',');
            typeList.ToList().Reverse();
            string orderbyStr = SqlJoint.GetOrderFieldFormat(orderField, orderType, typeList);
            var param = new Dictionary<string, object>();
            param.Add("ClientId", searchKey);
            param.Add("ClientNo", searchKey);
            param.Add("ClientName", searchKey);
            param.Add("ClientTypeName", searchKey);
            param.Add("ClientLevel", searchKey);
            param.Add("Province", searchKey);
            param.Add("City", searchKey);
            param.Add("Email", searchKey);
            param.Add("Wechat", searchKey);
            param.Add("Address", searchKey); 
            string sql = $@"select {selField} from BaseClients    
                            where ([ClientId] like '%'+@ClientId+'%' or ClientNo like '%'+@ClientNo+'%' 
                            or ClientName like '%'+@ClientName+'%' or ClientTypeName like '%'+@ClientTypeName+'%' or [Address] like '%'+@Address+'%'
                            or ClientLevel like '%'+@ClientLevel+'%' or Province like '%'+@Province+'%' or City like '%'+@City+'%'
                            or Email like '%'+@Email+'%' or Wechat like '%'+@Wechat+'%')
                            and IsValid=1
                            {orderbyStr}";
            var queryData = await Repository.QueryBySqlAsync(sql, param);
            var stream = ExcelHelper.ConvertDataTableToStream(queryData);
            var fileName = $"客户信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
            var savePath = @$"{Directory.GetCurrentDirectory()}\Files\Export\{fileName}";
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel); 
            return fileUrl;
        }
    }
}
