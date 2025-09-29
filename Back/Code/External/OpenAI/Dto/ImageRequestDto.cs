using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class ImageRequestDto
    {
        /// <summary>
        /// 图片描述
        /// </summary>
        public string prompt { get; set; }

        /// <summary>
        /// 要生成的图像数量。必须在1到10之间
        /// 默认:1
        /// </summary>
        public int n { get; set; }

        /// <summary>
        /// 生成图像的大小。必须是256x256、512x512或1024x1024中的一个
        /// 默认:1024x1024
        /// </summary>
        public string size { get; set; }

        /// <summary>
        /// 返回生成的图像的格式。必须是url或b64_json中的一个。
        /// 默认:url
        /// </summary>
        public string response_format { get; set; }

        public string user { get; set; }
    }
}
