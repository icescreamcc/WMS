using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
    /// <summary>
    /// 'system' 角色：系统角色用于在对话中传递系统级别的指令或提示。通常在对话开始时使用系统角色来设置场景、定义规则或提供初始信息。
    /// 系统角色的消息对模型的生成结果具有重要影响
    /// 'assistant' 角色：助手角色代表了模型自身，它会根据对话的上下文和前面的消息生成回复。助手角色会受到系统角色和用户角色的消息影响，并据此产生响应。
    /// 您可以将助手角色视为对话模型的一部分，负责生成回复
    /// 'user' 角色：用户角色代表了对话中的用户或使用者。用户角色通常是由实际用户或应用程序发送的消息。
    /// 模型将根据用户角色的消息理解用户的输入，并生成适当的回复
    /// 这些角色的区别在于它们的作用和影响范围。系统角色用于设定对话的环境和规则，助手角色负责生成回复，用户角色代表实际用户或应用程序的输入。
    /// 在构建对话时，您可以使用这些不同的角色来指导和控制对话的进行
    /// </summary>
    public enum ChatRole
    {
        system, assistant, user
    }
}
