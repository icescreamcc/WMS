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
        public string CreateUserId { get; set; }
        public List<FileInfoDto> GoodsPicture { get; set; }//图片
        public List<OutStorageDetail> Details { get; set; }
        public OutStorage OutStorageDetails { get; set; }
    }
}
