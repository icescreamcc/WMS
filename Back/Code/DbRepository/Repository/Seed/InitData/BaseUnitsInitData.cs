using DbRepository.Repository.DbModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class BaseUnitsInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var unitList = new List<BaseUnits>
            {
                new BaseUnits { UnitNo="CNY",UnitName="元", UnitType="Currency", IsValid=true},
                new BaseUnits { UnitNo="HKD",UnitName="港元",  UnitType="Currency", IsValid=true},
                new BaseUnits { UnitNo="TWD",UnitName="台币",  UnitType="Currency", IsValid=true},
                new BaseUnits { UnitNo="USD",UnitName="美元",  UnitType="Currency", IsValid=true},
                new BaseUnits { UnitNo="EUR",UnitName="欧元",  UnitType="Currency", IsValid=true},
                new BaseUnits { UnitNo="GBP",UnitName="英镑",  UnitType="Currency", IsValid=true},
                new BaseUnits { UnitNo="RU",UnitName="卢布",  UnitType="Currency", IsValid=true},
                new BaseUnits { UnitNo="T",UnitName="吨",  UnitType="Weight", IsValid=true},
                new BaseUnits { UnitNo="KG",UnitName="千克",  UnitType="Weight", IsValid=true},
                new BaseUnits { UnitNo="G",UnitName="克",  UnitType="Weight", IsValid=true},
                new BaseUnits { UnitNo="CHKG",UnitName="公斤",  UnitType="Weight", IsValid=true},
                new BaseUnits { UnitNo="CHG",UnitName="斤",  UnitType="Weight", IsValid=true},
                new BaseUnits { UnitNo="CHL",UnitName="两",  UnitType="Weight", IsValid=true},
                new BaseUnits { UnitNo="M",UnitName="米",  UnitType="Size", IsValid=true},
                new BaseUnits { UnitNo="DM",UnitName="分米",  UnitType="Size", IsValid=true},
                new BaseUnits { UnitNo="CM",UnitName="厘米",  UnitType="Size", IsValid=true},
                new BaseUnits { UnitNo="MM",UnitName="毫米",  UnitType="Size", IsValid=true},
                new BaseUnits { UnitNo="CHZ",UnitName="丈",  UnitType="Size", IsValid=true},
                new BaseUnits { UnitNo="CHC",UnitName="尺",  UnitType="Size", IsValid=true},
                new BaseUnits { UnitNo="CHGF",UnitName="公分",  UnitType="Size", IsValid=true},
                new BaseUnits { UnitNo="M2",UnitName="平方米",  UnitType="Acreage", IsValid=true},
                new BaseUnits { UnitNo="FM2",UnitName="平方分米",  UnitType="Acreage", IsValid=true},
                new BaseUnits { UnitNo="CM2",UnitName="平方厘米",  UnitType="Acreage", IsValid=true},
                new BaseUnits { UnitNo="M3",UnitName="立方米",  UnitType="Volume", IsValid=true},
                new BaseUnits { UnitNo="FM3",UnitName="立方分米",  UnitType="Volume", IsValid=true},
                new BaseUnits { UnitNo="CM3",UnitName="立方厘米",  UnitType="Volume", IsValid=true},
                new BaseUnits { UnitNo="ST",UnitName="托",  UnitType="Pack", IsValid=true},
                new BaseUnits { UnitNo="CH",UnitName="箱",  UnitType="Pack", IsValid=true},
                new BaseUnits { UnitNo="BOX",UnitName="盒",  UnitType="Pack", IsValid=true},
                new BaseUnits { UnitNo="BCK",UnitName="桶",  UnitType="Pack", IsValid=true},
                new BaseUnits { UnitNo="BAG",UnitName="袋",  UnitType="Pack", IsValid=true},
                new BaseUnits { UnitNo="SING",UnitName="个",  UnitType="Pack", IsValid=true},
                new BaseUnits { UnitNo="PIE",UnitName="件",  UnitType="Pack", IsValid=true},
                new BaseUnits { UnitNo="BAG2",UnitName="包",  UnitType="Pack", IsValid=true},
                new BaseUnits { UnitNo="BOT",UnitName="瓶",  UnitType="Pack", IsValid=true},
                new BaseUnits { UnitNo="TI",UnitName="听",  UnitType="Pack", IsValid=true},
                new BaseUnits { UnitNo="PI",UnitName="片",  UnitType="Pack", IsValid=true},
                new BaseUnits { UnitNo="HU",UnitName="壶",  UnitType="Pack", IsValid=true},
                new BaseUnits { UnitNo="BEN",UnitName="本",  UnitType="Pack", IsValid=true},
            };
            db.Insertable(unitList).AddQueue();
        }
    }
}
