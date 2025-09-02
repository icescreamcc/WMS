using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class AudioRequestDto
    {
        /// <summary>
        /// 必需，音频文件对象（而不是文件名），支持的格式有：mp3、mp4、mpeg、mpga、m4a、wav、webm
        /// </summary>
        public byte[] file { get; set; }

        /// <summary>
        /// 必需，要使用的模型的ID，目前只有whisper-1可用。
        /// </summary>
        public string model { get; set; }

        /// <summary>
        /// 可选，用于指导模型风格或继续之前音频片段的文本。提示文本应与音频语言相匹配
        /// </summary>
        public string prompt { get; set; }

        /// <summary>
        /// 可选，默认为json，指定转录输出的格式，可选项有：json、text、srt、verbose_json、vtt
        /// </summary>
        public string response_format { get; set; }

        /// <summary>
        /// 可选，默认为0，采样温度，介于0和1之间。较高的值（如0.8）会使输出更随机，而较低的值（如0.2）会使输出更加聚焦和确定性。
        /// 如果设置为0，模型将使用对数概率自动增加温度，直到达到一定的阈值
        /// </summary>
        public double temperature { get; set; }

        /// <summary>
        /// 可选，输入音频的语言。以ISO-639-1格式提供输入语言将提高准确性和延迟(en、zh...)
        /// </summary>
        public string language { get; set; }
    }
}
