using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Plan
{
    public class PlanFinishedProductOrderDto
    {
        public int OrderId { get; set; }
         
        public int PlanId { get; set; }
         
        public string PlanName { get; set; }
         
        public int Year { get; set; }
         
        public int Week { get; set; }
         
        public float Version { get; set; }

        public string GoodsClassifyGroup { get; set; }

        public string ConsignNum { get; set; }
         
        public string ProdctionTypeNo { get; set; }
         
        public float QuantityTotal { get; set; }
         
        public string UnitName { get; set; }
         
        public string DateOfMon { get; set; }
         
        public float QuantityDayOfMon { get; set; }
         
        public float QuantityNightOfMon { get; set; }
         
        public string DateOfTues { get; set; }
         
        public float QuantityDayOfTues { get; set; }
         
        public float QuantityNightOfTues { get; set; }
         
        public string DateOfWed { get; set; }
         
        public float QuantityDayOfWed { get; set; }
         
        public float QuantityNightOfWed { get; set; }
         
        public string DateOfThur { get; set; }
         
        public float QuantityDayOfThur { get; set; }
         
        public float QuantityNightOfThur { get; set; }
         
        public string DateOfFri { get; set; }
         
        public float QuantityDayOfFri { get; set; }
         
        public float QuantityNightOfFri { get; set; }
         
        public string DateOfSat { get; set; }
         
        public float QuantityDayOfSat { get; set; }
         
        public float QuantityNightOfSat { get; set; }
         
        public string DateOfSun { get; set; }
         
        public float QuantityDayOfSun { get; set; }
         
        public float QuantityNightOfSun { get; set; }
         
        public string Remark { get; set; }
    }
}
