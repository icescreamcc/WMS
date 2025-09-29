using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Cache;
using Logic.LogicBase;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Sys
{
    /// <summary>
    /// 备用字段管理
    /// </summary>
   public class FieldManageMgr: DbOperationHandler
    {
        public FieldManageMgr(Repository repository) : base(repository)
        { 
        }

        /// <summary>
        /// 获取所有表及备用字段信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeModel>> GetTableFieldList()
        {
            var data = await Repository.ClientDb.Queryable<SysFieldsManage>().Where(x=>x.Remark== FielsManageRemark.SpareField.ToString()||x.Remark== FielsManageRemark.All.ToString()).OrderBy(x=>x.Rank).ToListAsync();
            var treeRoot = data.GroupBy(d=>new { d.TableName ,d.TableDesc,d.Remark}).Select(m => new TreeModel { Id = m.Key.TableName, Label = m.Key.TableDesc ,Remark=m.Key.Remark}).ToList();
            treeRoot.ForEach(r =>
            {
                r.Children = data.Where(m => m.TableName == r.Id.ToString()).Select(m => new TreeModel { Id =m.FieldsManageId,  Label = m.FieldDesc, Remark=m.FieldName, Type= m.FieldName.Contains("Field")? "SpareField":"" }).ToList();
            });
            return treeRoot;
        }

        /// <summary>
        /// 获取指定表字段详细信息
        /// </summary>
        /// <param name="fieldsManageId"></param> 
        /// <returns></returns>
        public async Task<FieldsManage> GetSpareField(string fieldsManageId)
        {
            var data = await Repository.ClientDb.Queryable<SysFieldsManage>().SingleAsync(x => x.FieldsManageId == fieldsManageId);
            var model = new FieldsManage
            {
                FieldsManageId = fieldsManageId,
                TableName = data.TableName,
                TableDesc = data.TableDesc,
                FieldName = data.FieldName,
                FieldDesc = data.FieldDesc,
                FieldLength = data.FieldLength,
                FieldType = data.FieldType,
                IsEnable = data.IsEnable,
                FieldArgsKey = data.FieldArgsKey,
                Remark=data.Remark,
                Rank=data.Rank
            }; 
            return model;
        }


        /// <summary>
        /// 修改表字段信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SetSpareField(FieldsManage data)
        {
            var model = new SysFieldsManage
            {
                FieldsManageId = data.FieldsManageId,
                TableName = data.TableName,
                TableDesc = data.TableDesc,
                FieldName = data.FieldName,
                FieldDesc = data.FieldDesc,
                FieldType=data.FieldType,
                FieldLength=data.FieldLength,
                IsEnable = data.IsEnable,
                FieldArgsKey=data.FieldArgsKey,
                Remark=data.Remark,
                Rank=data.Rank
            };
            await Repository.UpdateAsync(model); 
        }
    }
}
