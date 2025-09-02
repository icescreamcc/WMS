using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Aliyun.OSS
{
    /// <summary>
    /// 图片格式
    /// </summary>
    public enum ImageFormat
    {
        /// <summary>
        /// 默认格式
        /// </summary>
        DEFAULT,

        /// <summary>
        /// 将原图保存成 jpg 格式，如果原图是 png、webp、bmp 存在透明通道，默认会把透明填充成白色。
        /// </summary>
        JPG,

        /// <summary>
        /// 将原图保存成 png 格式。
        /// </summary>
        PNG,

        /// <summary>
        /// 将原图保存成 webp 格式。
        /// </summary>
        WEBP,

        /// <summary>
        /// 将原图保存成 bmp 格式
        /// </summary>
        BMP,

        /// <summary>
        /// 将 gif 格式保存成 gif 格式，非 gif 格式是按原图格式保存
        /// </summary>
        GIF,

        /// <summary>
        /// 将原图保存成 tiff 格式
        /// </summary>
        TIFF
    }
}
