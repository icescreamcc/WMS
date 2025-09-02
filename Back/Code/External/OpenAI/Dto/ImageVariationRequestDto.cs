using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class ImageVariationRequestDto: ImageRequestDto
    {
        /// <summary>
        /// 要编辑的图像。必须是有效的 PNG 文件base64字符串，大小不超过 4MB，且为正方形。如果未提供 mask 参数，则图像必须具有透明度，透明部分将用作蒙版
        /// </summary>
        public string image { get; set; }
    }
}
