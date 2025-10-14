using Models.Model.Baseinfo;
using Models.Model.Inv;
using System;
using System.Collections.Generic;

namespace Models.Model.Purchase
{
    /// <summary>
    /// 没用
    /// </summary>
    public class OutStorageDtoForFront
    {
        public string OrderNo { get; set; }
        public String OutStorageType { get; set; }
        public string GoodsClassify { get; set; }
        public string CreateUserName { get; set; }

        public List<OutStorageDetail> Details { get; set; }
    }
}
