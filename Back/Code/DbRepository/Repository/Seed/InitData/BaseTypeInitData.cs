using DbRepository.Repository.DbModels;
using Models.Model.Enum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class BaseTypeInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var goodsTypeList = new List<BaseType>
            {
                new BaseType { TypeNo="RM",TypeName="原材料", Rank=1, ParentId=0,Group=BaseTypeGroup.SamplePiece.ToString()},
                new BaseType { TypeNo="SMG",TypeName="半成品", Rank=2, ParentId=0,Group=BaseTypeGroup.SamplePiece.ToString()},
                new BaseType { TypeNo="MUA",TypeName="产成品", Rank=3, ParentId=0,Group=BaseTypeGroup.SamplePiece.ToString()},

                new BaseType { TypeNo="LD",TypeName="螺刀类", Rank=1, ParentId=0,Group=BaseTypeGroup.SparePart.ToString()},
                new BaseType { TypeNo="MF",TypeName="密封类", Rank=2, ParentId=0,Group=BaseTypeGroup.SparePart.ToString()},
                new BaseType { TypeNo="WJ",TypeName="微机类", Rank=3, ParentId=0,Group=BaseTypeGroup.SparePart.ToString()},
                new BaseType { TypeNo="DZ",TypeName="电子类", Rank=4, ParentId=0,Group=BaseTypeGroup.SparePart.ToString()},
                new BaseType { TypeNo="HJ",TypeName="焊接ESD类", Rank=5, ParentId=0,Group=BaseTypeGroup.SparePart.ToString()},
                new BaseType { TypeNo="JX",TypeName="机械类", Rank=6, ParentId=0,Group=BaseTypeGroup.SparePart.ToString()},
                new BaseType { TypeNo="QD",TypeName="气动类", Rank=7, ParentId=0,Group=BaseTypeGroup.SparePart.ToString()},
                new BaseType { TypeNo="SJ",TypeName="视觉类", Rank=8, ParentId=0,Group=BaseTypeGroup.SparePart.ToString()},
                new BaseType { TypeNo="TJ",TypeName="涂胶类", Rank=9, ParentId=0,Group=BaseTypeGroup.SparePart.ToString()},
                new BaseType { TypeNo="CR",TypeName="C&Robot", Rank=10, ParentId=0,Group=BaseTypeGroup.SparePart.ToString()},
                new BaseType { TypeNo="MO",TypeName="Montrac", Rank=11, ParentId=0,Group=BaseTypeGroup.SparePart.ToString()},
                new BaseType { TypeNo="PL",TypeName="Plasma", Rank=12, ParentId=0,Group=BaseTypeGroup.SparePart.ToString()}
            };
            db.Insertable(goodsTypeList).AddQueue();
        }
    }
}
