using DbRepository.Repository;
using DbRepository.Repository.DbModels; 
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicBase
{
   public class FieldsManageHandler: DbOperationHandler
    {
        public FieldsManageHandler(Repository repository) : base(repository)
        {
        }

        public async Task<List<FieldsManage>> GetEnableSpareFields(string tableName)
        {
            return await Repository.ClientDb.Queryable<SysFieldsManage>()
                .Where(f =>f.TableName== tableName&& f.IsEnable && f.FieldName.Contains("Field"))
                .Select(f => new FieldsManage
                {
                     FieldName=f.FieldName,
                     FieldDesc=f.FieldDesc,
                     FieldLength=f.FieldLength,
                     FieldType=f.FieldType,
                     FieldArgsKey=f.FieldArgsKey
                }).ToListAsync();
        }
    }
}
