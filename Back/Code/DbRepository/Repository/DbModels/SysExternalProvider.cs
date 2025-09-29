using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysExternalProvider", "第三方身份信息")]
    public class SysExternalProvider
    {
        [SugarColumn(IsPrimaryKey = true, Length = 100)]
        public string ProviderName { get; set; }

        [SugarColumn(Length = 100)]
        public string ProviderSecretKey { get; set; }

        [SugarColumn(Length = 100)]
        public string ProviderSecret { get; set; }

        [SugarColumn(Length = 100)]
        public string ProviderHost { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string Remark { get; set; }

        [SugarColumn()]
        public bool IsValid { get; set; }
    }
}
