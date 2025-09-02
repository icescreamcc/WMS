using DbRepository.Repository.DbModels;
using Models.Model.Sys;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class SysArgsOptionsInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var sysArgsOptions = new List<SysArgsOptions>
            {
                new SysArgsOptions { ArgsKey="ClientType", OptionKey="Line", OptionName="线上客户", Rank=1},
                new SysArgsOptions { ArgsKey="ClientType", OptionKey="Offline", OptionName="线下客户", Rank=2},
                new SysArgsOptions { ArgsKey="ClientLevel", OptionKey="A", OptionName="A", Rank=1},
                new SysArgsOptions { ArgsKey="ClientLevel", OptionKey="B", OptionName="B", Rank=2},
                new SysArgsOptions { ArgsKey="ClientLevel", OptionKey="C", OptionName="C", Rank=3},
                new SysArgsOptions { ArgsKey="ClientLevel", OptionKey="D", OptionName="D", Rank=4},
                new SysArgsOptions { ArgsKey="SupplierType", OptionKey="SparePart", OptionName="备件供应商", Rank=1},
                new SysArgsOptions { ArgsKey="SupplierType", OptionKey="SamplePiece", OptionName="样件供应商", Rank=2},
                new SysArgsOptions { ArgsKey="SupplierType", OptionKey="Separator", OptionName="辅材供应商", Rank=3},
                new SysArgsOptions { ArgsKey="SupplierType", OptionKey="PackingMaterial", OptionName="包材供应商", Rank=4},
                new SysArgsOptions { ArgsKey="SupplierProperty", OptionKey="Internal", OptionName="国内供应商", Rank=1},
                new SysArgsOptions { ArgsKey="SupplierProperty", OptionKey="Foreign", OptionName="国外供应商", Rank=2},
                new SysArgsOptions { ArgsKey="CreditType", OptionKey="Alipay", OptionName="支付宝", Rank=1},
                new SysArgsOptions { ArgsKey="CreditType", OptionKey="BankAccount", OptionName="银行账户", Rank=2},
                new SysArgsOptions { ArgsKey="CreditType", OptionKey="CashCheque", OptionName="现金支票", Rank=3},
                new SysArgsOptions { ArgsKey="ProductLevel", OptionKey="A", OptionName="A级", Rank=1},
                new SysArgsOptions { ArgsKey="ProductLevel", OptionKey="B", OptionName="B级", Rank=2},
                new SysArgsOptions { ArgsKey="ProductLevel", OptionKey="C", OptionName="C级", Rank=3},
                new SysArgsOptions { ArgsKey="ProductProperty", OptionKey="Standard", OptionName="标准", Rank=1},
                new SysArgsOptions { ArgsKey="ProductProperty", OptionKey="Nonstandard", OptionName="非标", Rank=2}, 
                new SysArgsOptions { ArgsKey="WarehouseType", OptionKey="ProductBuffer", OptionName="成品缓存仓", Rank=11},
                new SysArgsOptions { ArgsKey="WarehouseType", OptionKey="SparePart", OptionName="备件仓", Rank=12},
                new SysArgsOptions { ArgsKey="WarehouseType", OptionKey="Consumables", OptionName="耗材仓", Rank=12},
                new SysArgsOptions { ArgsKey="WarehouseType", OptionKey="SamplePiece", OptionName="样件仓", Rank=13},
                new SysArgsOptions { ArgsKey="WarehouseType", OptionKey="PackingMaterial", OptionName="包材仓", Rank=14},
                new SysArgsOptions { ArgsKey="WarehouseType", OptionKey="RawMaterial", OptionName="原材料仓", Rank=14},
                new SysArgsOptions { ArgsKey="WarehouseType", OptionKey="Separator", OptionName="辅材仓", Rank=15},
                new SysArgsOptions { ArgsKey="WarehouseType", OptionKey="Unstack", OptionName="拆垛仓", Rank=14}, 
                new SysArgsOptions { ArgsKey="SparePartType", OptionKey="General", OptionName="一般备件", Rank=13},
                new SysArgsOptions { ArgsKey="SparePartType", OptionKey="Specific", OptionName="特殊备件", Rank=14},
                new SysArgsOptions { ArgsKey="ConsumablesType", OptionKey="General", OptionName="一般耗材", Rank=13},
                new SysArgsOptions { ArgsKey="ConsumablesType", OptionKey="Specific", OptionName="特殊耗材", Rank=14},
                new SysArgsOptions { ArgsKey="SamplePieceType", OptionKey="General", OptionName="一般样件", Rank=13},
                new SysArgsOptions { ArgsKey="SamplePieceType", OptionKey="Specific", OptionName="特殊样件", Rank=14},
                new SysArgsOptions { ArgsKey="SeparatorType", OptionKey="General", OptionName="一般辅材", Rank=13},
                new SysArgsOptions { ArgsKey="SeparatorType", OptionKey="Specific", OptionName="特殊辅材", Rank=14},
                new SysArgsOptions { ArgsKey="PackingMaterialType", OptionKey="General", OptionName="一般包材", Rank=13},
                new SysArgsOptions { ArgsKey="PackingMaterialType", OptionKey="Specific", OptionName="特殊包材", Rank=14},
                new SysArgsOptions { ArgsKey="RawMaterialType", OptionKey="General", OptionName="一般原材料", Rank=13},
                new SysArgsOptions { ArgsKey="RawMaterialType", OptionKey="Specific", OptionName="特殊原材料", Rank=14},
                new SysArgsOptions { ArgsKey="PurchaseChanne", OptionKey="Taobao", OptionName="淘宝", Rank=1},
                new SysArgsOptions { ArgsKey="PurchaseChanne", OptionKey="Jindong", OptionName="京东", Rank=2},
                new SysArgsOptions { ArgsKey="PurchaseChanne", OptionKey="Dunhuang", OptionName="亚马逊", Rank=3}, 
                new SysArgsOptions { ArgsKey="PurchaseChanne", OptionKey="Exhibition", OptionName="供应商", Rank=4},
                new SysArgsOptions { ArgsKey="PurchaseChanne", OptionKey="Exhibition", OptionName="其他", Rank=5},
                new SysArgsOptions { ArgsKey="PurchaseType", OptionKey="Platform", OptionName="SAP采买", Rank=1},
                new SysArgsOptions { ArgsKey="PurchaseType", OptionKey="Offline", OptionName="CEOS-报价单", Rank=2},

                new SysArgsOptions { ArgsKey="Purchase-CPMG", OptionKey="AS0305", OptionName="手持工具",Remark="螺丝拧紧系统，锤子，切割机，扳钳，镊子，衬套，动力工具等", Rank=1},
                new SysArgsOptions { ArgsKey="Purchase-CPMG", OptionKey="AS0306", OptionName="机械工具",Remark="生产设备用工具，如：刀具，磨具，锯片，夹钳", Rank=2},

                new SysArgsOptions { ArgsKey="Purchase-Account", OptionKey="K007", OptionName="化学品类(不指定前后道）", Rank=1},
                new SysArgsOptions { ArgsKey="Purchase-Account", OptionKey="K05111", OptionName="低值易耗品(<2000CNY)（不指定前后道）", Rank=2},

                new SysArgsOptions { ArgsKey="Purchase-Classes", OptionKey="800000", OptionName="Betriebsmittel, Fertigungshilfsmittel", Rank=1},
                new SysArgsOptions { ArgsKey="Purchase-Classes", OptionKey="800009", OptionName="Betriebsmittel, Sammelkl.", Rank=2},

                new SysArgsOptions { ArgsKey="SparePartRequisitionType", OptionKey="PM01", OptionName="紧急维修", Rank=1},
                new SysArgsOptions { ArgsKey="SparePartRequisitionType", OptionKey="PM02", OptionName="维修保养", Rank=2},
                new SysArgsOptions { ArgsKey="SparePartRequisitionType", OptionKey="PM03", OptionName="计划维修", Rank=3},
                new SysArgsOptions { ArgsKey="SparePartRequisitionType", OptionKey="PM04", OptionName="改造", Rank=4}, 

                new SysArgsOptions { ArgsKey="GeneralRequisitionType", OptionKey="ProductionRequisition", OptionName="生产领用", Rank=1},
                new SysArgsOptions { ArgsKey="GeneralRequisitionType", OptionKey="BorrowRequisition", OptionName="借用出库", Rank=2},

                new SysArgsOptions { ArgsKey="ReceivingAbnormalType", OptionKey="LabelAbnormal", OptionName="标签异常", Rank=1},
                new SysArgsOptions { ArgsKey="ReceivingAbnormalType", OptionKey="MaterialAbnormal", OptionName="物料异常", Rank=2},
                new SysArgsOptions { ArgsKey="ReceivingAbnormalType", OptionKey="PackageAbnormal", OptionName="包装异常", Rank=3},
                new SysArgsOptions { ArgsKey="ReceivingAbnormalType", OptionKey="DeliveryAbnormal", OptionName="送货单异常", Rank=4},
                new SysArgsOptions { ArgsKey="ReceivingAbnormalType", OptionKey="FreeMaterial", OptionName="免费样件", Rank=5},
                new SysArgsOptions { ArgsKey="ReceivingAbnormalType", OptionKey="DataAbnormal", OptionName="主数据异常", Rank=6},

                new SysArgsOptions { ArgsKey="TakeStockReasonType", OptionKey="NaturalLosses", OptionName="自然损失", Rank=1},
                new SysArgsOptions { ArgsKey="TakeStockReasonType", OptionKey="AccountingError", OptionName="核算错误", Rank=2},
                new SysArgsOptions { ArgsKey="TakeStockReasonType", OptionKey="PoorStorage", OptionName="保管不善", Rank=3},
                new SysArgsOptions { ArgsKey="TakeStockReasonType", OptionKey="Other", OptionName="其他原因", Rank=4},

                new SysArgsOptions { ArgsKey="Line", OptionKey="DPA Backlight Line 1", OptionName="DJD-123", Rank=1},
                new SysArgsOptions { ArgsKey="Line", OptionKey="DPA Backlight Line 2", OptionName="DD013", Rank=2},
                new SysArgsOptions { ArgsKey="Line", OptionKey="DPA Bonding Line 1", OptionName="DD2933", Rank=3},
                new SysArgsOptions { ArgsKey="Line", OptionKey="DPA Gluing Line 1", OptionName="23WD3", Rank=4},
            };
            db.Insertable(sysArgsOptions).AddQueue();
        }
    }
}
