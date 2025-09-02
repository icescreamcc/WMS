using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Label
{
    public class LabelDesignDto
    {
        public string LabelId { get; set; }

        public string LabelName { get; set; }

        public string GoodsClassifyGroup { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public string BackgroundColor { get; set; }

        public bool IsDeft { get; set; }

        public string Remark { get; set; }

        public bool IsValid { get; set; }

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime CreateDate { get; set; }

        public string UpdateUserId { get; set; }

        public string UpdateUserName { get; set; }

        public DateTime UpdateDate { get; set; }

        public List<LabelDesignDetailsDto> Details { get; set; }
    }
}
