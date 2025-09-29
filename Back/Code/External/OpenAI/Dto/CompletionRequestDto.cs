using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class CompletionRequestDto: DialogRequestParamsDto
    {
        /// <summary>
        /// 语言模型ID
        /// </summary>
        public string model { get; set; }

        /// <summary>
        /// 你的提问
        /// </summary>
        public string prompt { get; set; }

        /// <summary>
        /// 在服务器端生成多个补全，并返回“最佳”的补全
        /// 默认:1
        /// </summary>
        public int best_of { get; set; } = 1;

        /// <summary>
        /// （对数概率）：包括对数概率最高的标记的对数概率，以及选择的标记。例如，如果 logprobs 为 5，则 API 将返回最有可能的 5 个标记的列表
        /// 默认:null
        /// </summary>
        public int? logprobs { get; set; }


    }
}
