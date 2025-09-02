using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class DialogRequestParamsDto
    {
        /// <summary>
        /// 本次生成文本最大长度
        /// </summary>
        public int max_tokens { get; set; }

        /// <summary>
        /// 生成结果的多样性,一般范围值0~2,值越大多样性和创造性就越大,反之则更加保守
        /// 默认:1
        /// </summary>
        public double temperature { get; set; } = 1;

        /// <summary>
        /// 与temperature采样相反，使用顶部 p 概率质量的方式进行采样。例如，0.1 表示只考虑占前 10% 概率质量的标记
        /// 一般建议改变top_p或temperature，但不建议两者都改变
        /// 默认:1
        /// </summary>
        public double top_p { get; set; } = 1;

        /// <summary>
        /// (补全数）：对于每个提示生成的补全数
        /// 默认:1
        /// </summary>
        public int n { get; set; } = 1;

        /// <summary>
        /// 表示终端用户的唯一标识符，可帮助 OpenAI 监测和检测滥用
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// (存在惩罚）：介于 -2.0 和 2.0 之间的数字，正值根据文本中是否出现惩罚新标记，增加模型谈论新主题的可能性
        /// 默认:0
        /// </summary>
        public double presence_penalty { get; set; }

        /// <summary>
        /// 频率惩罚）：介于 -2.0 和 2.0 之间的数字，正值根据文本中的频率惩罚新标记，减少模型重复相同行的可能性
        /// 默认:0
        /// </summary>
        public double frequency_penalty { get; set; }
         
        /// <summary>
        /// （对数偏差）：修改指定标记在补全中出现的可能性
        /// 默认:null
        /// </summary>
       // public object logit_bias { get; set; }
    }
}
