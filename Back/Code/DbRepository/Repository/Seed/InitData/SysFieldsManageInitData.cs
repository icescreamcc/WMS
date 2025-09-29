using DbRepository.Repository.DbModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class SysFieldsManageInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var fieldList = new List<SysFieldsManage>();
            var goodsFields = _getTableFields<BaseGoods>();
            var clientsFields = _getTableFields<BaseClients>();
            var userFields = _getTableFields<SysUser>("PermissionField");
            var warehouseFields = _getTableFields<InvWarehouse>();
            var supplierFields = _getTableFields<BaseSuppliers>();
            fieldList.AddRange(goodsFields);
            fieldList.AddRange(clientsFields);
            fieldList.AddRange(userFields);
            fieldList.AddRange(warehouseFields);
            fieldList.AddRange(supplierFields);
            db.Insertable(fieldList).AddQueue();
        }

        private static List<SysFieldsManage> _getTableFields<T>(string remark = "All")
        {
            var fieldsList = new List<SysFieldsManage>();
            var tableType = typeof(T);
            var tableProps = tableType.GetProperties();
            var tableDesc = tableType.CustomAttributes.FirstOrDefault()?.ConstructorArguments[1].Value.ToString();
            foreach (var p in tableProps)
            {
                var fieldDesc = p.CustomAttributes.FirstOrDefault()?.NamedArguments?.Where(arg => arg.MemberName == "ColumnDescription")?.SingleOrDefault().TypedValue.Value.ToString();
                var model = new SysFieldsManage { TableName = tableType.Name, TableDesc = tableDesc, FieldName = p.Name, FieldDesc = fieldDesc, IsEnable = p.Name.Contains("Field") ? false : true, FieldsManageId = tableType.Name + p.Name, Remark = remark };
                fieldsList.Add(model);
            } 
            return fieldsList;
        }
    }
}
